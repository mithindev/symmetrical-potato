Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Diagnostics
Imports System.Text
Imports MimeKit

Partial Public Class tds
    Inherits System.Web.UI.Page

    Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Initial load if needed
        End If
    End Sub

    Protected Sub btnFetch_Click(sender As Object, e As EventArgs)
        BindTDSReport()
    End Sub

    Private Function GetLocalTDSQuery(Optional includeFY As String = "") As String
        Dim fyCol As String = If(String.IsNullOrEmpty(includeFY), "", "'" & includeFY & "' as FinancialYear, ")
        Return "SELECT m.MemberNo, " & fyCol & "m.FirstName, CAST(m.address AS NVARCHAR(4000)) as address, m.pincode, m.mobile, m.dob, k.PAN, " &
            "SUM(CASE WHEN MONTH(act.date) = 4 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Apr_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 4 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Apr_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 4 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Apr_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 4 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Apr_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 4 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Apr_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 4 THEN act.Crc ELSE 0 END) as Apr_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 5 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as May_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 5 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as May_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 5 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as May_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 5 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as May_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 5 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as May_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 5 THEN act.Crc ELSE 0 END) as May_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 6 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Jun_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 6 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Jun_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 6 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Jun_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 6 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Jun_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 6 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Jun_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 6 THEN act.Crc ELSE 0 END) as Jun_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 7 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Jul_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 7 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Jul_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 7 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Jul_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 7 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Jul_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 7 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Jul_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 7 THEN act.Crc ELSE 0 END) as Jul_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 8 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Aug_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 8 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Aug_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 8 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Aug_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 8 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Aug_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 8 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Aug_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 8 THEN act.Crc ELSE 0 END) as Aug_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 9 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Sep_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 9 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Sep_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 9 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Sep_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 9 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Sep_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 9 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Sep_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 9 THEN act.Crc ELSE 0 END) as Sep_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 10 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Oct_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 10 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Oct_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 10 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Oct_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 10 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Oct_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 10 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Oct_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 10 THEN act.Crc ELSE 0 END) as Oct_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 11 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Nov_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 11 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Nov_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 11 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Nov_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 11 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Nov_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 11 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Nov_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 11 THEN act.Crc ELSE 0 END) as Nov_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 12 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Dec_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 12 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Dec_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 12 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Dec_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 12 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Dec_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 12 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Dec_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 12 THEN act.Crc ELSE 0 END) as Dec_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 1 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Jan_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 1 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Jan_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 1 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Jan_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 1 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Jan_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 1 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Jan_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 1 THEN act.Crc ELSE 0 END) as Jan_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 2 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Feb_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 2 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Feb_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 2 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Feb_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 2 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Feb_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 2 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Feb_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 2 THEN act.Crc ELSE 0 END) as Feb_Total, " &
            "SUM(CASE WHEN MONTH(act.date) = 3 AND mast.product = 'RD' THEN act.Crc ELSE 0 END) as Mar_RD, " &
            "SUM(CASE WHEN MONTH(act.date) = 3 AND mast.product = 'FD' THEN act.Crc ELSE 0 END) as Mar_FD, " &
            "SUM(CASE WHEN MONTH(act.date) = 3 AND mast.product = 'RID' THEN act.Crc ELSE 0 END) as Mar_RID, " &
            "SUM(CASE WHEN MONTH(act.date) = 3 AND mast.product = 'SB' THEN act.Crc ELSE 0 END) as Mar_SB, " &
            "SUM(CASE WHEN MONTH(act.date) = 3 AND mast.product = 'KMK' THEN act.Crc ELSE 0 END) as Mar_KMK, " &
            "SUM(CASE WHEN MONTH(act.date) = 3 THEN act.Crc ELSE 0 END) as Mar_Total, " &
            "SUM(act.Crc) as TotalInterest " &
            "FROM dbo.member m " &
            "INNER JOIN dbo.master mast ON m.MemberNo = mast.cid " &
            "INNER JOIN dbo.actrans act ON mast.acno = act.acno " &
            "LEFT OUTER JOIN dbo.kyc k ON m.MemberNo = k.memberno " &
            "WHERE mast.product IN ('RD', 'FD', 'RID', 'SB', 'KMK') " &
            "AND act.Narration = 'By INTEREST' " &
            "AND CONVERT(VARCHAR(8), act.date, 112) BETWEEN @start AND @end " &
            "GROUP BY m.MemberNo, m.FirstName, CAST(m.address AS NVARCHAR(4000)), m.pincode, m.mobile, m.dob, k.PAN "
    End Function

    Private Sub BindTDSReport()
        Dim fyFull As String = ddlFY.SelectedValue ' e.g., "2025-2026"
        Dim fyStart As String = ""
        If fyFull.Contains("-") Then
            fyStart = fyFull.Split("-"c)(0)
        Else
            fyStart = fyFull ' Fallback just in case
        End If

        Dim startDate As String = fyStart & "0401"
        Dim endDate As String = (CInt(fyStart) + 1) & "0331"
        Dim query As String

        If ddlDataSource.SelectedValue = "Hub" Then
            query = "SELECT * FROM TDS_Consolidated_Data WHERE FinancialYear = @fy ORDER BY MemberNo"
        Else
            query = GetLocalTDSQuery() & " ORDER BY m.MemberNo"
        End If

        Using cmd As New SqlCommand(query, con)
            If ddlDataSource.SelectedValue = "Hub" Then
                cmd.Parameters.AddWithValue("@fy", fyFull)
            Else
                cmd.Parameters.AddWithValue("@start", startDate)
                cmd.Parameters.AddWithValue("@end", endDate)
            End If

            Dim dt As New DataTable()
            Dim da As New SqlDataAdapter(cmd)

            Try
                con.Open()
                da.Fill(dt)

                ' Always dynamically calculate the TDS for the view regardless of Local or Hub source
                AppendTDSToDataTable(dt)

                ' Cache and Bind
                ViewState("TDSReportData") = dt
                gvTDSReport.DataSource = dt
                gvTDSReport.DataBind()

                If dt.Rows.Count > 0 Then
                    gvTDSReport.FooterRow.Cells(6).Text = "Grand Total:"
                    For i As Integer = 7 To gvTDSReport.Columns.Count - 1
                        Dim boundField As BoundField = TryCast(gvTDSReport.Columns(i), BoundField)
                        If boundField IsNot Nothing AndAlso Not String.IsNullOrEmpty(boundField.DataField) Then
                            Dim colHeader As String = boundField.DataField
                            If dt.Columns.Contains(colHeader) Then
                                ' Safely sum the column by name, immune to index shifts
                                gvTDSReport.FooterRow.Cells(i).Text = Convert.ToDouble(dt.Compute("SUM(" & colHeader & ")", "")).ToString("N2")
                                gvTDSReport.FooterRow.Cells(i).HorizontalAlign = HorizontalAlign.Right
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ' Handle exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    ' TDS CALCULATION LOGIC
    ' Applied to the DataTable in-memory before binding or exporting.

    Private Sub AppendTDSToDataTable(ByVal dt As DataTable)
        ' 1. Add the 13 new columns to the DataTable
        Dim tdsColumns As String() = {"Apr_TDS", "May_TDS", "Jun_TDS", "Jul_TDS", "Aug_TDS", "Sep_TDS", "Oct_TDS", "Nov_TDS", "Dec_TDS", "Jan_TDS", "Feb_TDS", "Mar_TDS"}
        For Each col As String In tdsColumns
            If Not dt.Columns.Contains(col) Then
                dt.Columns.Add(col, GetType(Decimal)).DefaultValue = 0D
            End If
        Next
        If Not dt.Columns.Contains("TotalMemberTDS") Then
            dt.Columns.Add("TotalMemberTDS", GetType(Decimal)).DefaultValue = 0D
        End If

        ' 2. Months ordered chronologically starting from April
        Dim monthPrefixes As String() = {"Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar"}

        For Each row As DataRow In dt.Rows
            Dim accumulator As Decimal = 0D
            Dim totalMemberTDS As Decimal = 0D
            Dim thresholdCrossed As Boolean = False

            For Each prefix As String In monthPrefixes
                Dim totalCol As String = prefix & "_Total"
                Dim tdsCol As String = prefix & "_TDS"

                Dim monthInterest As Decimal = 0D
                If Not IsDBNull(row(totalCol)) Then
                    monthInterest = Convert.ToDecimal(row(totalCol))
                End If

                If Not thresholdCrossed Then
                    accumulator += monthInterest

                    ' Rule: If accumulated amount > 5000, calculate 10% TDS on the accumulated amount.
                    If accumulator > 5000D Then
                        thresholdCrossed = True
                        Dim calculatedTDS As Decimal = Math.Round(accumulator * 0.1D, 2)
                        row(tdsCol) = calculatedTDS
                        totalMemberTDS += calculatedTDS
                    Else
                        row(tdsCol) = 0D
                    End If
                Else
                    ' Threshold already crossed in a previous month, calculate 10% TDS on this month's interest directly
                    Dim calculatedTDS As Decimal = Math.Round(monthInterest * 0.1D, 2)
                    row(tdsCol) = calculatedTDS
                    totalMemberTDS += calculatedTDS
                End If
            Next

            row("TotalMemberTDS") = totalMemberTDS
        Next
    End Sub

    Protected Sub gvTDSReport_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvTDSReport.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim HeaderGrid As GridView = DirectCast(sender, GridView)
            Dim HeaderGridRow As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            ' Helper to create cells
            Dim CreateCell = Function(text As String, colspan As Integer) As TableCell
                                 Dim cell As New TableCell()
                                 cell.Text = text
                                 cell.ColumnSpan = colspan
                                 cell.HorizontalAlign = HorizontalAlign.Center
                                 cell.Font.Bold = True
                                 cell.BackColor = System.Drawing.Color.FromName("#f8f9fa")
                                 Return cell
                             End Function

            ' Fixed Column Spans
            HeaderGridRow.Cells.Add(CreateCell("Member Details", 7))

            Dim months() As String = {"April", "May", "June", "July", "August", "September", "October", "November", "December", "January", "February", "March"}
            For Each month As String In months
                HeaderGridRow.Cells.Add(CreateCell(month, 7)) ' 5 products + 1 total + 1 TDS
            Next

            HeaderGridRow.Cells.Add(CreateCell("Total", 2))

            gvTDSReport.Controls(0).Controls.AddAt(0, HeaderGridRow)
        End If
    End Sub

    Protected Sub gvTDSReport_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvTDSReport.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' TDS column indices: 13, 20, 27, 34, 41, 48, 55, 62, 69, 76, 83, 90, 92
            Dim tdsIndices() As Integer = {13, 20, 27, 34, 41, 48, 55, 62, 69, 76, 83, 90, 92}
            For Each idx In tdsIndices
                If idx < e.Row.Cells.Count Then
                    Dim cellValue As Decimal = 0D
                    If Decimal.TryParse(e.Row.Cells(idx).Text, NumberStyles.Any, CultureInfo.InvariantCulture, cellValue) Then
                        If cellValue > 0 Then
                            e.Row.Cells(idx).BackColor = System.Drawing.Color.Yellow
                            e.Row.Cells(idx).Font.Bold = True
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Protected Sub gvTDSReport_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvTDSReport.PageIndex = e.NewPageIndex
        If ViewState("TDSReportData") IsNot Nothing Then
            Dim dt As DataTable = DirectCast(ViewState("TDSReportData"), DataTable)
            gvTDSReport.DataSource = dt
            gvTDSReport.DataBind()
            
            ' Re-apply footers on pagination
            If dt.Rows.Count > 0 Then
                gvTDSReport.FooterRow.Cells(6).Text = "Grand Total:"
                For i As Integer = 7 To gvTDSReport.Columns.Count - 1
                    Dim boundField As BoundField = TryCast(gvTDSReport.Columns(i), BoundField)
                    If boundField IsNot Nothing AndAlso Not String.IsNullOrEmpty(boundField.DataField) Then
                        Dim colHeader As String = boundField.DataField
                        If dt.Columns.Contains(colHeader) Then
                            gvTDSReport.FooterRow.Cells(i).Text = Convert.ToDouble(dt.Compute("SUM(" & colHeader & ")", "")).ToString("N2")
                            gvTDSReport.FooterRow.Cells(i).HorizontalAlign = HorizontalAlign.Right
                        End If
                    End If
                Next
            End If
        Else
            BindTDSReport()
        End If
    End Sub

    Protected Sub btnVerifyMember_Click(sender As Object, e As EventArgs)
        BindMemberDetails()
    End Sub

    Private Sub BindMemberDetails()
        Dim memberNo As String = txtSearchMemberNo.Text.Trim()
        If String.IsNullOrEmpty(memberNo) Then Return

        Dim fy As String = ddlFY.SelectedValue
        Dim fyStart As String = fy
        If fy.Contains("-") Then fyStart = fy.Split("-"c)(0)
        
        Dim startDate As String = fyStart & "0401"
        Dim endDate As String = (CInt(fyStart) + 1) & "0331"

        Dim query As String = "SELECT " &
            "DATENAME(MONTH, act.date) as MonthName, " &
            "mast.product as Product, " &
            "act.date as Date, " &
            "act.acno as AcNo, " &
            "act.Narration, " &
            "act.Crc as Amount " &
            "FROM dbo.actrans act " &
            "INNER JOIN dbo.master mast ON act.acno = mast.acno " &
            "WHERE mast.cid = @cid " &
            "AND mast.product IN ('RD', 'FD', 'RID', 'SB', 'KMK') " &
            "AND act.Narration = 'By INTEREST' " &
            "AND CONVERT(VARCHAR(8), act.date, 112) BETWEEN @start AND @end " &
            "ORDER BY act.date, mast.product"

        Using cmd As New SqlCommand(query, con)
            cmd.Parameters.AddWithValue("@cid", memberNo)
            cmd.Parameters.AddWithValue("@start", startDate)
            cmd.Parameters.AddWithValue("@end", endDate)

            Dim dt As New DataTable()
            Dim da As New SqlDataAdapter(cmd)

            Try
                con.Open()
                da.Fill(dt)
                
                ViewState("MemberDetailsData") = dt
                gvMemberDetails.DataSource = dt
                gvMemberDetails.DataBind()

                If dt.Rows.Count > 0 Then
                    gvMemberDetails.FooterRow.Cells(4).Text = "Total:"
                    gvMemberDetails.FooterRow.Cells(5).Text = Convert.ToDouble(dt.Compute("SUM(Amount)", "")).ToString("N2")
                    gvMemberDetails.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                End If
            Catch ex As Exception
                ' Handle exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Protected Sub gvMemberDetails_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvMemberDetails.PageIndex = e.NewPageIndex
        If ViewState("MemberDetailsData") IsNot Nothing Then
            Dim dt As DataTable = DirectCast(ViewState("MemberDetailsData"), DataTable)
            gvMemberDetails.DataSource = dt
            gvMemberDetails.DataBind()
            
            ' Re-apply footers on pagination
            If dt.Rows.Count > 0 Then
                gvMemberDetails.FooterRow.Cells(4).Text = "Total:"
                gvMemberDetails.FooterRow.Cells(5).Text = Convert.ToDouble(dt.Compute("SUM(Amount)", "")).ToString("N2")
                gvMemberDetails.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
            End If
        Else
            BindMemberDetails()
        End If
    End Sub

    Private Shared ReadOnly SqlServerName As String = "localhost\SQLEXPRESS"

    ' EXPORT BCP logic
    Protected Sub btnExport_Click(sender As Object, e As EventArgs)
        Dim fyFull As String = ddlFY.SelectedValue ' e.g. 2025-2026
        Dim fyStart As String = ""
        If fyFull.Contains("-") Then
            fyStart = fyFull.Split("-"c)(0)
        Else
            fyStart = fyFull
        End If

        Dim startDate As String = fyStart & "0401"
        Dim endDate As String = (CInt(fyStart) + 1) & "0331"

        Dim stageTableName As String = "TDS_Export_Temp"
        Dim exportDir As String = Path.Combine(Path.GetTempPath(), "MultiBranchTDS")
        If Not Directory.Exists(exportDir) Then
            Directory.CreateDirectory(exportDir)
        End If

        Dim baseName As String = If(String.IsNullOrWhiteSpace(txtExportFileName.Text), "TDS_Export", txtExportFileName.Text.Trim())
        Dim fileName As String = $"{baseName}_{DateTime.Now:yyyyMMddHHmmss}.dat"
        Dim dataFile As String = Path.Combine(exportDir, fileName)

        Try
            Using curCon As New SqlConnection(con.ConnectionString)
                curCon.Open()

                ' 1. Create staging table structure based on the raw aggregate data only (No TDS)
                Dim setupSql As String = $"
                    IF OBJECT_ID('dbo.{stageTableName}', 'U') IS NOT NULL 
                        DROP TABLE dbo.{stageTableName};
                    
                    CREATE TABLE dbo.{stageTableName} (
                        MemberNo VARCHAR(50) NOT NULL,
                        FinancialYear VARCHAR(9) NOT NULL,
                        FirstName VARCHAR(255),
                        address VARCHAR(MAX),
                        pincode VARCHAR(50),
                        mobile VARCHAR(50),
                        dob VARCHAR(50),
                        PAN VARCHAR(20),
                        Apr_RD MONEY DEFAULT 0, Apr_FD MONEY DEFAULT 0, Apr_RID MONEY DEFAULT 0, Apr_SB MONEY DEFAULT 0, Apr_KMK MONEY DEFAULT 0, Apr_Total MONEY DEFAULT 0,
                        May_RD MONEY DEFAULT 0, May_FD MONEY DEFAULT 0, May_RID MONEY DEFAULT 0, May_SB MONEY DEFAULT 0, May_KMK MONEY DEFAULT 0, May_Total MONEY DEFAULT 0,
                        Jun_RD MONEY DEFAULT 0, Jun_FD MONEY DEFAULT 0, Jun_RID MONEY DEFAULT 0, Jun_SB MONEY DEFAULT 0, Jun_KMK MONEY DEFAULT 0, Jun_Total MONEY DEFAULT 0,
                        Jul_RD MONEY DEFAULT 0, Jul_FD MONEY DEFAULT 0, Jul_RID MONEY DEFAULT 0, Jul_SB MONEY DEFAULT 0, Jul_KMK MONEY DEFAULT 0, Jul_Total MONEY DEFAULT 0,
                        Aug_RD MONEY DEFAULT 0, Aug_FD MONEY DEFAULT 0, Aug_RID MONEY DEFAULT 0, Aug_SB MONEY DEFAULT 0, Aug_KMK MONEY DEFAULT 0, Aug_Total MONEY DEFAULT 0,
                        Sep_RD MONEY DEFAULT 0, Sep_FD MONEY DEFAULT 0, Sep_RID MONEY DEFAULT 0, Sep_SB MONEY DEFAULT 0, Sep_KMK MONEY DEFAULT 0, Sep_Total MONEY DEFAULT 0,
                        Oct_RD MONEY DEFAULT 0, Oct_FD MONEY DEFAULT 0, Oct_RID MONEY DEFAULT 0, Oct_SB MONEY DEFAULT 0, Oct_KMK MONEY DEFAULT 0, Oct_Total MONEY DEFAULT 0,
                        Nov_RD MONEY DEFAULT 0, Nov_FD MONEY DEFAULT 0, Nov_RID MONEY DEFAULT 0, Nov_SB MONEY DEFAULT 0, Nov_KMK MONEY DEFAULT 0, Nov_Total MONEY DEFAULT 0,
                        Dec_RD MONEY DEFAULT 0, Dec_FD MONEY DEFAULT 0, Dec_RID MONEY DEFAULT 0, Dec_SB MONEY DEFAULT 0, Dec_KMK MONEY DEFAULT 0, Dec_Total MONEY DEFAULT 0,
                        Jan_RD MONEY DEFAULT 0, Jan_FD MONEY DEFAULT 0, Jan_RID MONEY DEFAULT 0, Jan_SB MONEY DEFAULT 0, Jan_KMK MONEY DEFAULT 0, Jan_Total MONEY DEFAULT 0,
                        Feb_RD MONEY DEFAULT 0, Feb_FD MONEY DEFAULT 0, Feb_RID MONEY DEFAULT 0, Feb_SB MONEY DEFAULT 0, Feb_KMK MONEY DEFAULT 0, Feb_Total MONEY DEFAULT 0,
                        Mar_RD MONEY DEFAULT 0, Mar_FD MONEY DEFAULT 0, Mar_RID MONEY DEFAULT 0, Mar_SB MONEY DEFAULT 0, Mar_KMK MONEY DEFAULT 0, Mar_Total MONEY DEFAULT 0,
                        TotalInterest MONEY DEFAULT 0
                    );
                "
                Using cmd As New SqlCommand(setupSql, curCon)
                    cmd.ExecuteNonQuery()
                End Using

                ' 2. Process data in .NET to calculate TDS (We cannot do this cleanly in standard SQL pivot queries)
                Dim selectSql As String = GetLocalTDSQuery(fyFull)
                Dim dt As New DataTable()
                Using cmd As New SqlCommand(selectSql, curCon)
                    cmd.Parameters.AddWithValue("@start", startDate)
                    cmd.Parameters.AddWithValue("@end", endDate)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using

                ' 3. SqlBulkCopy the raw DataTable into the SQL Staging Table
                Using bulkCopy As New SqlBulkCopy(curCon)
                    bulkCopy.DestinationTableName = $"dbo.{stageTableName}"
                    ' Setup Column Mappings to match the exact names of the DataTable to the StagingTable
                    For Each column As DataColumn In dt.Columns
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName)
                    Next
                    bulkCopy.WriteToServer(dt)
                End Using
            End Using

            ' 4. BCP OUT
            Dim args As String = $"dbo.{stageTableName} out ""{dataFile}"" -n -T -S ""{SqlServerName}"" -d fiscusdb"
            RunBcp(args)

            ' 3. Drop staging table
            Using curCon As New SqlConnection(con.ConnectionString)
                curCon.Open()
                Dim dropSql As String = $"DROP TABLE dbo.{stageTableName};"
                Using cmd As New SqlCommand(dropSql, curCon)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' 4. Provide download
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
            Response.TransmitFile(dataFile)
            Response.End()

        Catch ex As Exception
            Dim safeMsg As String = ex.Message.Replace("'", "\'").Replace(vbCr, "\r").Replace(vbLf, "\n")
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", $"alert('Export failed:\n{safeMsg}');", True)
        End Try
    End Sub

    Protected Sub btnExportCSV_Click(sender As Object, e As EventArgs)
        Dim fyFull As String = ddlFY.SelectedValue
        Dim fyStart As String = ""
        If fyFull.Contains("-") Then
            fyStart = fyFull.Split("-"c)(0)
        Else
            fyStart = fyFull
        End If

        Dim startDate As String = fyStart & "0401"
        Dim endDate As String = (CInt(fyStart) + 1) & "0331"

        Dim baseName As String = If(String.IsNullOrWhiteSpace(txtExportFileName.Text), "TDS_Export", txtExportFileName.Text.Trim())
        Dim fileName As String = $"{baseName}_{DateTime.Now:yyyyMMddHHmmss}.csv"

        Try
            Dim dt As New DataTable()
            Dim query As String
            If ddlDataSource.SelectedValue = "Hub" Then
                query = "SELECT * FROM TDS_Consolidated_Data WHERE FinancialYear = @fy ORDER BY MemberNo"
            Else
                query = GetLocalTDSQuery(fyFull) & " ORDER BY m.MemberNo"
            End If

            Using curCon As New SqlConnection(con.ConnectionString)
                curCon.Open()
                Using cmd As New SqlCommand(query, curCon)
                    If ddlDataSource.SelectedValue = "Hub" Then
                        cmd.Parameters.AddWithValue("@fy", fyFull)
                    Else
                        cmd.Parameters.AddWithValue("@start", startDate)
                        cmd.Parameters.AddWithValue("@end", endDate)
                    End If
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            ' Apply the TDS rules logic dynamically to the exported table
            AppendTDSToDataTable(dt)

            ' Generate Formatted CSV matching GridView
            Dim sb As New StringBuilder()
            Dim monthPrefixes As String() = {"Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar"}
            Dim monthNames As String() = {"April", "May", "June", "July", "August", "September", "October", "November", "December", "January", "February", "March"}
            Dim products As String() = {"RD", "FD", "RID", "SB", "KMK"}

            ' --- Header Row 1: Groupings ---
            sb.Append("""Member Details"","""","""","""","""","""","""",") ' 7 cells for Member Details
            For Each mName As String In monthNames
                sb.Append($"""{mName}"","""","""","""","""","""","""",") ' 7 cells per month
            Next
            sb.AppendLine("""Total"",""""") ' 2 cells for final totals

            ' --- Header Row 2: Sub-headers ---
            sb.Append("""Member No"",""Name"",""Complete Address"",""Pincode"",""Mobile"",""Date of Birth"",""PAN No"",")
            For Each prefix As String In monthPrefixes
                For Each prod As String In products
                    sb.Append($"""{prod}"",")
                Next
                sb.Append("""Total"",""TDS"",")
            Next
            sb.AppendLine("""Grand Total"",""Total Member TDS""")

            ' --- Data Rows ---
            For Each row As DataRow In dt.Rows
                ' Member Details
                sb.Append($"""{row("MemberNo")}"",""{row("FirstName")}"",""{row("address")}"",""{row("pincode")}"",""{row("mobile")}"",""{row("dob")}"",""{row("PAN")}"",")

                ' Monthly Data
                For Each prefix As String In monthPrefixes
                    For Each prod As String In products
                        Dim val As Decimal = If(IsDBNull(row($"{prefix}_{prod}")), 0D, Convert.ToDecimal(row($"{prefix}_{prod}")))
                        sb.Append($"""{val:N2}"",")
                    Next
                    ' Monthly Total and TDS
                    Dim mTotal As Decimal = If(IsDBNull(row($"{prefix}_Total")), 0D, Convert.ToDecimal(row($"{prefix}_Total")))
                    Dim mTds As Decimal = If(IsDBNull(row($"{prefix}_TDS")), 0D, Convert.ToDecimal(row($"{prefix}_TDS")))
                    sb.Append($"""{mTotal:N2}"",""{mTds:N2}"",")
                Next

                ' Grand Totals
                Dim grandTotal As Decimal = If(IsDBNull(row("TotalInterest")), 0D, Convert.ToDecimal(row("TotalInterest")))
                Dim totalTds As Decimal = If(IsDBNull(row("TotalMemberTDS")), 0D, Convert.ToDecimal(row("TotalMemberTDS")))
                sb.AppendLine($"""{grandTotal:N2}"",""{totalTds:N2}""")
            Next

            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" & fileName)
            Response.Charset = ""
            Response.ContentType = "text/csv"
            Response.Output.Write(sb.ToString())
            Response.Flush()
            Response.End()

        Catch ex As Exception
            Dim safeMsg As String = ex.Message.Replace("'", "\'").Replace(vbCr, "\r").Replace(vbLf, "\n")
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", $"alert('CSV Export failed:\n{safeMsg}');", True)
        End Try
    End Sub

    ' IMPORT BCP logic

    Protected Sub btnBatchImport_Click(sender As Object, e As EventArgs)
        Dim tdsFolder As String = Server.MapPath("~/tds")

        If Not Directory.Exists(tdsFolder) Then
            lblImportStatus.Text = "The 'tds' folder does not exist in the root directory. Please create it and place .DAT files inside."
            lblImportStatus.ForeColor = System.Drawing.Color.Red
            litDashboardLog.Text = ""
            Return
        End If

        Dim datFiles As String() = Directory.GetFiles(tdsFolder, "*.dat")
        If datFiles.Length = 0 Then
            lblImportStatus.Text = "No .dat files found in the '~/tds' folder."
            lblImportStatus.ForeColor = System.Drawing.Color.Orange
            litDashboardLog.Text = ""
            Return
        End If

        Dim successCount As Integer = 0
        Dim errorCount As Integer = 0
        Dim logEntries As New List(Of String)()

        Dim stageTableName As String = "TDS_Consolidated_STAGE"
        Dim targetTableName As String = "TDS_Consolidated_Data" ' Must be created in advance on hub

        Dim uploadDir As String = Path.Combine(Path.GetTempPath(), "MultiBranchTDS_Import")
        If Not Directory.Exists(uploadDir) Then
            Directory.CreateDirectory(uploadDir)
        End If

        For Each sourceFile In datFiles
            Dim fileName As String = Path.GetFileName(sourceFile)
            Dim datFilePath As String = Path.Combine(uploadDir, fileName)

            Try
                ' Copy the file to temp location for BCP to securely access
                File.Copy(sourceFile, datFilePath, True)

                ' 1. Create Staging Table by copying structure from central Target Table
                '    and creating Target Table if it does not yet exist on this specific environment for testing
                Using currentCon As New SqlConnection(con.ConnectionString)
                    currentCon.Open()

                    Dim setupSql As String = $"
                        IF OBJECT_ID('dbo.{targetTableName}', 'U') IS NULL 
                        BEGIN
                            CREATE TABLE dbo.{targetTableName} (
                                MemberNo VARCHAR(50) NOT NULL,
                                FinancialYear VARCHAR(9) NOT NULL,
                                FirstName VARCHAR(255),
                                address VARCHAR(MAX),
                                pincode VARCHAR(50),
                                mobile VARCHAR(50),
                                dob VARCHAR(50),
                                PAN VARCHAR(20),
                                Apr_RD MONEY DEFAULT 0, Apr_FD MONEY DEFAULT 0, Apr_RID MONEY DEFAULT 0, Apr_SB MONEY DEFAULT 0, Apr_KMK MONEY DEFAULT 0, Apr_Total MONEY DEFAULT 0,
                                May_RD MONEY DEFAULT 0, May_FD MONEY DEFAULT 0, May_RID MONEY DEFAULT 0, May_SB MONEY DEFAULT 0, May_KMK MONEY DEFAULT 0, May_Total MONEY DEFAULT 0,
                                Jun_RD MONEY DEFAULT 0, Jun_FD MONEY DEFAULT 0, Jun_RID MONEY DEFAULT 0, Jun_SB MONEY DEFAULT 0, Jun_KMK MONEY DEFAULT 0, Jun_Total MONEY DEFAULT 0,
                                Jul_RD MONEY DEFAULT 0, Jul_FD MONEY DEFAULT 0, Jul_RID MONEY DEFAULT 0, Jul_SB MONEY DEFAULT 0, Jul_KMK MONEY DEFAULT 0, Jul_Total MONEY DEFAULT 0,
                                Aug_RD MONEY DEFAULT 0, Aug_FD MONEY DEFAULT 0, Aug_RID MONEY DEFAULT 0, Aug_SB MONEY DEFAULT 0, Aug_KMK MONEY DEFAULT 0, Aug_Total MONEY DEFAULT 0,
                                Sep_RD MONEY DEFAULT 0, Sep_FD MONEY DEFAULT 0, Sep_RID MONEY DEFAULT 0, Sep_SB MONEY DEFAULT 0, Sep_KMK MONEY DEFAULT 0, Sep_Total MONEY DEFAULT 0,
                                Oct_RD MONEY DEFAULT 0, Oct_FD MONEY DEFAULT 0, Oct_RID MONEY DEFAULT 0, Oct_SB MONEY DEFAULT 0, Oct_KMK MONEY DEFAULT 0, Oct_Total MONEY DEFAULT 0,
                                Nov_RD MONEY DEFAULT 0, Nov_FD MONEY DEFAULT 0, Nov_RID MONEY DEFAULT 0, Nov_SB MONEY DEFAULT 0, Nov_KMK MONEY DEFAULT 0, Nov_Total MONEY DEFAULT 0,
                                Dec_RD MONEY DEFAULT 0, Dec_FD MONEY DEFAULT 0, Dec_RID MONEY DEFAULT 0, Dec_SB MONEY DEFAULT 0, Dec_KMK MONEY DEFAULT 0, Dec_Total MONEY DEFAULT 0,
                                Jan_RD MONEY DEFAULT 0, Jan_FD MONEY DEFAULT 0, Jan_RID MONEY DEFAULT 0, Jan_SB MONEY DEFAULT 0, Jan_KMK MONEY DEFAULT 0, Jan_Total MONEY DEFAULT 0,
                                Feb_RD MONEY DEFAULT 0, Feb_FD MONEY DEFAULT 0, Feb_RID MONEY DEFAULT 0, Feb_SB MONEY DEFAULT 0, Feb_KMK MONEY DEFAULT 0, Feb_Total MONEY DEFAULT 0,
                                Mar_RD MONEY DEFAULT 0, Mar_FD MONEY DEFAULT 0, Mar_RID MONEY DEFAULT 0, Mar_SB MONEY DEFAULT 0, Mar_KMK MONEY DEFAULT 0, Mar_Total MONEY DEFAULT 0,
                                TotalInterest MONEY DEFAULT 0,
                                CONSTRAINT PK_TDS_Consolidated PRIMARY KEY (MemberNo, FinancialYear)
                            );
                        END

                        IF OBJECT_ID('dbo.{stageTableName}', 'U') IS NOT NULL 
                            DROP TABLE dbo.{stageTableName};
                        
                        SELECT TOP 0 * INTO dbo.{stageTableName} FROM dbo.{targetTableName};
                    "
                    Using cmd As New SqlCommand(setupSql, currentCon)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                ' 2. BCP IN to staging table
                Dim bcpArgs As String = $"dbo.{stageTableName} in ""{datFilePath}"" -n -T -S ""{SqlServerName}"" -d fiscusdb"
                RunBcp(bcpArgs)

                ' 3. MERGE from staging to active consolidated table
                Using currentCon As New SqlConnection(con.ConnectionString)
                    currentCon.Open()

                    Dim mergeSql As String = $"
                        MERGE INTO dbo.{targetTableName} AS Target
                        USING dbo.{stageTableName} AS Source
                        ON Target.MemberNo = Source.MemberNo AND Target.FinancialYear = Source.FinancialYear
                        WHEN MATCHED THEN
                            UPDATE SET 
                                Target.FirstName = ISNULL(Target.FirstName, Source.FirstName),
                                Target.address = ISNULL(Target.address, Source.address),
                                Target.pincode = ISNULL(Target.pincode, Source.pincode),
                                Target.mobile = ISNULL(Target.mobile, Source.mobile),
                                Target.dob = ISNULL(Target.dob, Source.dob),
                                Target.PAN = ISNULL(Target.PAN, Source.PAN),
                                
                                Target.Apr_RD = Target.Apr_RD + Source.Apr_RD, Target.Apr_FD = Target.Apr_FD + Source.Apr_FD, Target.Apr_RID = Target.Apr_RID + Source.Apr_RID, Target.Apr_SB = Target.Apr_SB + Source.Apr_SB, Target.Apr_KMK = Target.Apr_KMK + Source.Apr_KMK, Target.Apr_Total = Target.Apr_Total + Source.Apr_Total,
                                Target.May_RD = Target.May_RD + Source.May_RD, Target.May_FD = Target.May_FD + Source.May_FD, Target.May_RID = Target.May_RID + Source.May_RID, Target.May_SB = Target.May_SB + Source.May_SB, Target.May_KMK = Target.May_KMK + Source.May_KMK, Target.May_Total = Target.May_Total + Source.May_Total,
                                Target.Jun_RD = Target.Jun_RD + Source.Jun_RD, Target.Jun_FD = Target.Jun_FD + Source.Jun_FD, Target.Jun_RID = Target.Jun_RID + Source.Jun_RID, Target.Jun_SB = Target.Jun_SB + Source.Jun_SB, Target.Jun_KMK = Target.Jun_KMK + Source.Jun_KMK, Target.Jun_Total = Target.Jun_Total + Source.Jun_Total,
                                Target.Jul_RD = Target.Jul_RD + Source.Jul_RD, Target.Jul_FD = Target.Jul_FD + Source.Jul_FD, Target.Jul_RID = Target.Jul_RID + Source.Jul_RID, Target.Jul_SB = Target.Jul_SB + Source.Jul_SB, Target.Jul_KMK = Target.Jul_KMK + Source.Jul_KMK, Target.Jul_Total = Target.Jul_Total + Source.Jul_Total,
                                Target.Aug_RD = Target.Aug_RD + Source.Aug_RD, Target.Aug_FD = Target.Aug_FD + Source.Aug_FD, Target.Aug_RID = Target.Aug_RID + Source.Aug_RID, Target.Aug_SB = Target.Aug_SB + Source.Aug_SB, Target.Aug_KMK = Target.Aug_KMK + Source.Aug_KMK, Target.Aug_Total = Target.Aug_Total + Source.Aug_Total,
                                Target.Sep_RD = Target.Sep_RD + Source.Sep_RD, Target.Sep_FD = Target.Sep_FD + Source.Sep_FD, Target.Sep_RID = Target.Sep_RID + Source.Sep_RID, Target.Sep_SB = Target.Sep_SB + Source.Sep_SB, Target.Sep_KMK = Target.Sep_KMK + Source.Sep_KMK, Target.Sep_Total = Target.Sep_Total + Source.Sep_Total,
                                Target.Oct_RD = Target.Oct_RD + Source.Oct_RD, Target.Oct_FD = Target.Oct_FD + Source.Oct_FD, Target.Oct_RID = Target.Oct_RID + Source.Oct_RID, Target.Oct_SB = Target.Oct_SB + Source.Oct_SB, Target.Oct_KMK = Target.Oct_KMK + Source.Oct_KMK, Target.Oct_Total = Target.Oct_Total + Source.Oct_Total,
                                Target.Nov_RD = Target.Nov_RD + Source.Nov_RD, Target.Nov_FD = Target.Nov_FD + Source.Nov_FD, Target.Nov_RID = Target.Nov_RID + Source.Nov_RID, Target.Nov_SB = Target.Nov_SB + Source.Nov_SB, Target.Nov_KMK = Target.Nov_KMK + Source.Nov_KMK, Target.Nov_Total = Target.Nov_Total + Source.Nov_Total,
                                Target.Dec_RD = Target.Dec_RD + Source.Dec_RD, Target.Dec_FD = Target.Dec_FD + Source.Dec_FD, Target.Dec_RID = Target.Dec_RID + Source.Dec_RID, Target.Dec_SB = Target.Dec_SB + Source.Dec_SB, Target.Dec_KMK = Target.Dec_KMK + Source.Dec_KMK, Target.Dec_Total = Target.Dec_Total + Source.Dec_Total,
                                Target.Jan_RD = Target.Jan_RD + Source.Jan_RD, Target.Jan_FD = Target.Jan_FD + Source.Jan_FD, Target.Jan_RID = Target.Jan_RID + Source.Jan_RID, Target.Jan_SB = Target.Jan_SB + Source.Jan_SB, Target.Jan_KMK = Target.Jan_KMK + Source.Jan_KMK, Target.Jan_Total = Target.Jan_Total + Source.Jan_Total,
                                Target.Feb_RD = Target.Feb_RD + Source.Feb_RD, Target.Feb_FD = Target.Feb_FD + Source.Feb_FD, Target.Feb_RID = Target.Feb_RID + Source.Feb_RID, Target.Feb_SB = Target.Feb_SB + Source.Feb_SB, Target.Feb_KMK = Target.Feb_KMK + Source.Feb_KMK, Target.Feb_Total = Target.Feb_Total + Source.Feb_Total,
                                Target.Mar_RD = Target.Mar_RD + Source.Mar_RD, Target.Mar_FD = Target.Mar_FD + Source.Mar_FD, Target.Mar_RID = Target.Mar_RID + Source.Mar_RID, Target.Mar_SB = Target.Mar_SB + Source.Mar_SB, Target.Mar_KMK = Target.Mar_KMK + Source.Mar_KMK, Target.Mar_Total = Target.Mar_Total + Source.Mar_Total,
                                
                                Target.TotalInterest = Target.TotalInterest + Source.TotalInterest
                        WHEN NOT MATCHED THEN
                            INSERT (
                                MemberNo, FinancialYear, FirstName, address, pincode, mobile, dob, PAN,
                                Apr_RD, Apr_FD, Apr_RID, Apr_SB, Apr_KMK, Apr_Total,
                                May_RD, May_FD, May_RID, May_SB, May_KMK, May_Total,
                                Jun_RD, Jun_FD, Jun_RID, Jun_SB, Jun_KMK, Jun_Total,
                                Jul_RD, Jul_FD, Jul_RID, Jul_SB, Jul_KMK, Jul_Total,
                                Aug_RD, Aug_FD, Aug_RID, Aug_SB, Aug_KMK, Aug_Total,
                                Sep_RD, Sep_FD, Sep_RID, Sep_SB, Sep_KMK, Sep_Total,
                                Oct_RD, Oct_FD, Oct_RID, Oct_SB, Oct_KMK, Oct_Total,
                                Nov_RD, Nov_FD, Nov_RID, Nov_SB, Nov_KMK, Nov_Total,
                                Dec_RD, Dec_FD, Dec_RID, Dec_SB, Dec_KMK, Dec_Total,
                                Jan_RD, Jan_FD, Jan_RID, Jan_SB, Jan_KMK, Jan_Total,
                                Feb_RD, Feb_FD, Feb_RID, Feb_SB, Feb_KMK, Feb_Total,
                                Mar_RD, Mar_FD, Mar_RID, Mar_SB, Mar_KMK, Mar_Total, TotalInterest
                            )
                            VALUES (
                                Source.MemberNo, Source.FinancialYear, Source.FirstName, Source.address, Source.pincode, Source.mobile, Source.dob, Source.PAN,
                                Source.Apr_RD, Source.Apr_FD, Source.Apr_RID, Source.Apr_SB, Source.Apr_KMK, Source.Apr_Total,
                                Source.May_RD, Source.May_FD, Source.May_RID, Source.May_SB, Source.May_KMK, Source.May_Total,
                                Source.Jun_RD, Source.Jun_FD, Source.Jun_RID, Source.Jun_SB, Source.Jun_KMK, Source.Jun_Total,
                                Source.Jul_RD, Source.Jul_FD, Source.Jul_RID, Source.Jul_SB, Source.Jul_KMK, Source.Jul_Total,
                                Source.Aug_RD, Source.Aug_FD, Source.Aug_RID, Source.Aug_SB, Source.Aug_KMK, Source.Aug_Total,
                                Source.Sep_RD, Source.Sep_FD, Source.Sep_RID, Source.Sep_SB, Source.Sep_KMK, Source.Sep_Total,
                                Source.Oct_RD, Source.Oct_FD, Source.Oct_RID, Source.Oct_SB, Source.Oct_KMK, Source.Oct_Total,
                                Source.Nov_RD, Source.Nov_FD, Source.Nov_RID, Source.Nov_SB, Source.Nov_KMK, Source.Nov_Total,
                                Source.Dec_RD, Source.Dec_FD, Source.Dec_RID, Source.Dec_SB, Source.Dec_KMK, Source.Dec_Total,
                                Source.Jan_RD, Source.Jan_FD, Source.Jan_RID, Source.Jan_SB, Source.Jan_KMK, Source.Jan_Total,
                                Source.Feb_RD, Source.Feb_FD, Source.Feb_RID, Source.Feb_SB, Source.Feb_KMK, Source.Feb_Total,
                                Source.Mar_RD, Source.Mar_FD, Source.Mar_RID, Source.Mar_SB, Source.Mar_KMK, Source.Mar_Total, Source.TotalInterest
                            );
                        
                        -- Drop staging table
                        DROP TABLE dbo.{stageTableName};
                    "
                    Using cmd As New SqlCommand(mergeSql, currentCon)
                        cmd.CommandTimeout = 300
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                ' Move successfully processed file to a backup/archive subfolder
                Dim archiveFolder As String = Path.Combine(tdsFolder, "Processed")
                If Not Directory.Exists(archiveFolder) Then
                    Directory.CreateDirectory(archiveFolder)
                End If
                Dim archiveFileName = Path.Combine(archiveFolder, fileName)
                If File.Exists(archiveFileName) Then File.Delete(archiveFileName)
                File.Move(sourceFile, archiveFileName)

                successCount += 1
                logEntries.Add($"<div class='text-success'><i class='fas fa-check-circle'></i> <strong>{fileName}</strong> - Integrated Successfully</div>")

            Catch ex As Exception
                ' Important: Drop the staging table if the transaction failed, to prevent dirty data in next loop iteration
                Try
                    Using dropCon As New SqlConnection(con.ConnectionString)
                        dropCon.Open()
                        Using cmd As New SqlCommand($"IF OBJECT_ID('dbo.{stageTableName}', 'U') IS NOT NULL DROP TABLE dbo.{stageTableName};", dropCon)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                Catch ignore As Exception
                End Try

                errorCount += 1
                Dim safeEx As String = ex.Message.Replace("'", "\'").Replace(vbCr, "").Replace(vbLf, " ")
                logEntries.Add($"<div class='text-danger'><i class='fas fa-times-circle'></i> <strong>{fileName}</strong> - FAILED: {safeEx}</div>")
            Finally
                ' Try to clean up local temp file
                If File.Exists(datFilePath) Then
                    Try
                        File.Delete(datFilePath)
                    Catch ignore As Exception
                    End Try
                End If
            End Try
        Next

        lblImportStatus.Text = $"Batch Complete: {successCount} files processed successfully, {errorCount} errors."
        If errorCount = 0 Then
            lblImportStatus.ForeColor = System.Drawing.Color.Green
        Else
            lblImportStatus.ForeColor = System.Drawing.Color.Goldenrod
        End If

        litDashboardLog.Text = String.Join("", logEntries)

        If ddlDataSource.SelectedValue = "Hub" Then
            BindTDSReport() ' Refresh view
        End If
    End Sub

    ' RUN BCP SAFELY
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

End Class
