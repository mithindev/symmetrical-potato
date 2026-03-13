Imports System.Data.SqlClient
Imports System.IO

Public Class MemberMerge
    Inherits System.Web.UI.Page

    Private Shared ReadOnly ConnStr As String =
        "Server=localhost\SQLEXPRESS;Database=fiscusdb;Integrated Security=True;"

    Private Shared ReadOnly SqlServerName As String =
        "localhost\SQLEXPRESS"

    ' ----------------------------------------------------------
    ' TABLE LIST (UI)
    ' ----------------------------------------------------------
    Public Shared Function GetAllowedTables() As List(Of String)
        Return New List(Of String) From {"KYC", "Member", "roi"}
    End Function

    ' ----------------------------------------------------------
    ' EXPORT TABLE → BCP (.dat)
    ' ----------------------------------------------------------
    Public Shared Function ExportTableBcp(tableName As String) As String
        If Not GetAllowedTables().Contains(tableName) Then
            Throw New Exception("Invalid table selected for export.")
        End If

        Dim exportDir As String = Path.Combine(Path.GetTempPath(), "MemberMerge")
        Directory.CreateDirectory(exportDir)

        Dim dataFile As String = Path.Combine(
            exportDir,
            $"{tableName}_{DateTime.Now:yyyyMMddHHmmss}.dat"
        )

        ' -c  = character format (portable across instances/versions)
        ' -T  = trusted connection
        ' -S  = server
        ' Using -c instead of -n to avoid binary format mismatches (string truncation + date errors)
        RunBcp($"dbo.{tableName} out ""{dataFile}"" -c -T -S ""{SqlServerName}"" -d fiscusdb")

        Return dataFile
    End Function

    ' ----------------------------------------------------------
    ' IMPORT + MERGE TABLE FROM BCP FILE
    ' ----------------------------------------------------------
    Public Shared Function ImportAndMergeTable(tableName As String, datFilePath As String) As Integer
        If Not GetAllowedTables().Contains(tableName) Then
            Throw New Exception("Invalid table selected for import.")
        End If

        If Not File.Exists(datFilePath) Then
            Throw New Exception("Data file not found.")
        End If

        Dim stageTableName As String = $"{tableName}_STAGE"

        Using con As New SqlConnection(ConnStr)
            con.Open()

            ' 1️⃣ Create staging table — same schema as target, but with identity
            '    columns converted to plain INT via CONVERT().
            '
            '    WHY: SELECT TOP 0 * INTO copies the IDENTITY property too. BCP runs
            '    as a separate OS process with its own SQL connection, so session-level
            '    SET IDENTITY_INSERT cannot affect it. CONVERT(int, [Id]) strips the
            '    identity constraint from the staging column so BCP can insert freely.
            '
            Dim stageSelectParts As New List(Of String)()
            Using cmdSchema As New SqlCommand($"
                SELECT
                    COLUMN_NAME,
                    DATA_TYPE,
                    COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS IsIdentity
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = '{tableName}'
                ORDER BY ORDINAL_POSITION", con)
                Using r = cmdSchema.ExecuteReader()
                    While r.Read()
                        Dim colName As String = r.GetString(0)
                        Dim dataType As String = r.GetString(1).ToUpper()
                        Dim isIdentity As Boolean = (Not r.IsDBNull(2)) AndAlso (r.GetInt32(2) = 1)
                        If isIdentity Then
                            ' CONVERT strips the identity property in the cloned staging table
                            stageSelectParts.Add($"CONVERT({dataType}, [{colName}]) AS [{colName}]")
                        Else
                            stageSelectParts.Add($"[{colName}]")
                        End If
                    End While
                End Using
            End Using

            Using cmd As New SqlCommand($"
                IF OBJECT_ID('dbo.{stageTableName}', 'U') IS NOT NULL
                    DROP TABLE dbo.{stageTableName};
                SELECT TOP 0 {String.Join(", ", stageSelectParts)}
                INTO dbo.{stageTableName}
                FROM dbo.{tableName};
            ", con)
                cmd.ExecuteNonQuery()
            End Using

            ' 2️⃣ BCP load the uploaded file into staging.
            ' -c (character format) must match how it was exported.
            RunBcp($"dbo.{stageTableName} in ""{datFilePath}"" -c -T -S ""{SqlServerName}"" -d fiscusdb")

            ' 3️⃣ MERGE — strategy differs per table
            Dim mergeSql As String

            If tableName.Equals("roi", StringComparison.OrdinalIgnoreCase) Then
                ' --------------------------------------------------------
                ' ROI: WIPE AND REPLACE
                ' Delete everything, then insert the full imported dataset.
                ' IDENTITY_INSERT ON is required because roi.Id is an identity
                ' column and the exact Id values must be preserved from source.
                ' --------------------------------------------------------
                Dim colList As String = GetColumnList(con, "roi", includeIdentity:=True)
                mergeSql = $"
                    SET IDENTITY_INSERT dbo.roi ON;
                    DELETE FROM dbo.roi;
                    INSERT INTO dbo.roi ({colList})
                        SELECT {colList} FROM dbo.{stageTableName};
                    SET IDENTITY_INSERT dbo.roi OFF;
                    DROP TABLE dbo.{stageTableName};
                "

            ElseIf tableName.Equals("member", StringComparison.OrdinalIgnoreCase) Then
                ' --------------------------------------------------------
                ' MEMBER: INSERT ONLY NEW MEMBERS
                ' Skip any row whose MemberNo already exists in member.
                ' member.Id is an identity column — it is excluded from the
                ' column list so SQL Server auto-generates a new Id on insert.
                ' Deduplication key: MemberNo
                ' --------------------------------------------------------
                Dim colList As String = GetColumnList(con, "member", includeIdentity:=False)
                mergeSql = $"
                    INSERT INTO dbo.member ({colList})
                        SELECT {colList} FROM dbo.{stageTableName} s
                        WHERE NOT EXISTS (
                            SELECT 1 FROM dbo.member t WHERE t.MemberNo = s.MemberNo
                        );
                    DROP TABLE dbo.{stageTableName};
                "

            ElseIf tableName.Equals("KYC", StringComparison.OrdinalIgnoreCase) Then
                ' --------------------------------------------------------
                ' KYC: INSERT ONLY NEW RECORDS
                ' Skip any row whose MemberNo already exists in KYC.
                ' KYC has no identity column so all columns are included.
                ' Deduplication key: MemberNo
                ' --------------------------------------------------------
                Dim colList As String = GetColumnList(con, "KYC", includeIdentity:=True)
                mergeSql = $"
                    INSERT INTO dbo.KYC ({colList})
                        SELECT {colList} FROM dbo.{stageTableName} s
                        WHERE NOT EXISTS (
                            SELECT 1 FROM dbo.KYC t WHERE t.MemberNo = s.MemberNo
                        );
                    DROP TABLE dbo.{stageTableName};
                "

            Else
                Throw New Exception($"No merge strategy defined for table: {tableName}")
            End If

            Using cmd As New SqlCommand(mergeSql, con)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ' ----------------------------------------------------------
    ' HELPER: Build a comma-separated [ColumnName] list.
    ' includeIdentity = False  →  skips identity columns (e.g. member.Id)
    ' includeIdentity = True   →  includes all columns
    ' ----------------------------------------------------------
    Private Shared Function GetColumnList(con As SqlConnection, tableName As String, includeIdentity As Boolean) As String
        Dim identityFilter As String = If(
            includeIdentity,
            "",
            " AND COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 0"
        )
        Dim cols As New List(Of String)()
        Using cmd As New SqlCommand(
            $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS
              WHERE TABLE_NAME = '{tableName}'{identityFilter}
              ORDER BY ORDINAL_POSITION",
            con)
            Using r = cmd.ExecuteReader()
                While r.Read()
                    cols.Add($"[{r.GetString(0)}]")
                End While
            End Using
        End Using
        If cols.Count = 0 Then
            Throw New Exception($"No columns found for table: {tableName}")
        End If
        Return String.Join(", ", cols)
    End Function

    ' ----------------------------------------------------------
    ' RUN BCP SAFELY
    ' ----------------------------------------------------------
    Private Shared Sub RunBcp(args As String)
        Dim psi As New ProcessStartInfo With {
            .FileName = "bcp",
            .Arguments = args,
            .UseShellExecute = False,
            .RedirectStandardError = True,
            .RedirectStandardOutput = True,
            .CreateNoWindow = True
        }

        Using p As Process = Process.Start(psi)
            Dim stdout As String = p.StandardOutput.ReadToEnd()
            Dim stderr As String = p.StandardError.ReadToEnd()

            p.WaitForExit()

            If p.ExitCode <> 0 Then
                Throw New Exception("BCP failed:" & vbCrLf & stdout & vbCrLf & stderr)
            End If
        End Using
    End Sub

    ' ---------------------------------------------
    ' PAGE LOAD
    ' ---------------------------------------------
    Protected Sub Page_Load(sender As Object, e As EventArgs)
        If Not IsPostBack Then
            LoadTableList()
        End If
    End Sub

    ' ---------------------------------------------
    ' LOAD TABLE DROPDOWN
    ' ---------------------------------------------
    Private Sub LoadTableList()
        ddlTables.Items.Clear()
        ddlTables.Items.Add("-- Select --")

        Dim tables = GetAllowedTables()
        For Each t In tables
            ddlTables.Items.Add(t)
        Next
    End Sub

    ' ---------------------------------------------
    ' DOWNLOAD .DAT
    ' ---------------------------------------------
    Protected Sub btnDownload_Click(sender As Object, e As EventArgs)
        If ddlTables.SelectedIndex <= 0 Then
            lblStatus.Text = "Please select a table to export."
            lblStatus.ForeColor = System.Drawing.Color.Red
            Return
        End If

        Try
            Dim tableName As String = ddlTables.SelectedValue
            Dim datPath As String = ExportTableBcp(tableName)
            Dim fileName As String = Path.GetFileName(datPath)

            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
            Response.TransmitFile(datPath)
            Response.End()

        Catch ex As Exception
            lblStatus.Text = "Download failed: " & ex.Message
            lblStatus.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    ' ---------------------------------------------
    ' UPLOAD + MERGE
    ' ---------------------------------------------
    Protected Sub btnUpload_Click(sender As Object, e As EventArgs)
        If ddlTables.SelectedIndex <= 0 Then
            lblStatus.Text = "Please select which table you are importing into."
            lblStatus.ForeColor = System.Drawing.Color.Red
            Return
        End If

        If Not fuTableFile.HasFile Then
            lblStatus.Text = "Please select a file to upload."
            lblStatus.ForeColor = System.Drawing.Color.Red
            Return
        End If

        Try
            Dim tableName As String = ddlTables.SelectedValue
            Dim tempDir As String = Path.Combine(Path.GetTempPath(), "MemberMerge")
            Directory.CreateDirectory(tempDir)

            Dim filePath As String = Path.Combine(tempDir, Path.GetFileName(fuTableFile.FileName))
            fuTableFile.SaveAs(filePath)

            Dim rowsInserted As Integer = ImportAndMergeTable(tableName, filePath)

            lblStatus.Text = $"Merge completed successfully! {rowsInserted} new rows inserted into `{tableName}`."
            lblStatus.ForeColor = System.Drawing.Color.Green

        Catch ex As Exception
            lblStatus.Text = "Upload failed: " & ex.Message
            lblStatus.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

End Class
