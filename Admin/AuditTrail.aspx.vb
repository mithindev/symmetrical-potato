Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports iTextSharp.text
Imports iTextSharp.text.pdf


' ==========================================================
' PDF PAGE EVENTS (FOOTER + WATERMARK)
' ==========================================================
Public Class AuditPdfPageEvent
    Inherits PdfPageEventHelper

    Private ReadOnly FooterText As String =
            "Karavalai Nidhi Ltd • Audit Trail"

    Private ReadOnly WatermarkText As String =
            "AUDIT USE ONLY | KVLTD"

    Public Overrides Sub OnEndPage(
            writer As PdfWriter,
            document As Document
        )

        ' ===== FOOTER =====
        Dim footerFont As Font =
                FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.GRAY)

        Dim footerPhrase As New Phrase(
                $"{FooterText} | Page {writer.PageNumber}",
                footerFont
            )

        ColumnText.ShowTextAligned(
                writer.DirectContent,
                Element.ALIGN_CENTER,
                footerPhrase,
                (document.Right + document.Left) / 2,
                document.Bottom - 15,
                0
            )

        ' ===== WATERMARK (BEHIND CONTENT) =====
        Dim cb As PdfContentByte = writer.DirectContentUnder

        Dim watermarkFont As Font =
                FontFactory.GetFont(
                    FontFactory.HELVETICA_BOLD,
                    48,
                    New BaseColor(220, 220, 220)
                )

        Dim watermarkPhrase As New Phrase(WatermarkText, watermarkFont)

        ColumnText.ShowTextAligned(
                cb,
                Element.ALIGN_CENTER,
                watermarkPhrase,
                (document.Right + document.Left) / 2,
                (document.Top + document.Bottom) / 2,
                45
            )

    End Sub

End Class

' ==========================================================
' AUDIT TRAIL REPOSITORY
' ==========================================================
Public Class AuditTrailRepository

    Private Shared ReadOnly ConnStr As String =
            "Server=localhost\SQLEXPRESS;Database=fiscusdb;Integrated Security=True;"

    ' ----------------------------------------------------------
    ' GET ALL AUDIT TRAIL
    ' ----------------------------------------------------------
    Public Shared Function GetAllAuditTrail() As DataTable

        Dim dt As New DataTable()

        Using con As New SqlConnection(ConnStr)
            Using cmd As New SqlCommand("
                    SELECT *
                    FROM fiscusdb.dbo.AuditTrail
                    ORDER BY ChangedOn DESC", con)

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using

            End Using
        End Using

        Return dt

    End Function

    ' ----------------------------------------------------------
    ' GET AUDIT TRAIL BY DATE RANGE
    ' ----------------------------------------------------------
    Public Shared Function GetAuditTrailByDateRange(
            startDate As DateTime,
            endDate As DateTime
        ) As DataTable

        Dim dt As New DataTable()

        Using con As New SqlConnection(ConnStr)
            Using cmd As New SqlCommand("
                    SELECT *
                    FROM fiscusdb.dbo.AuditTrail
                    WHERE ChangedOn BETWEEN @StartDate AND @EndDate
                    ORDER BY ChangedOn DESC", con)

                cmd.Parameters.AddWithValue("@StartDate", startDate)
                cmd.Parameters.AddWithValue("@EndDate", endDate)

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using

            End Using
        End Using

        Return dt

    End Function

    ' ----------------------------------------------------------
    ' PDF GENERATION (CORE)
    ' ----------------------------------------------------------
    Public Shared Function GenerateAuditTrailPdf(dt As DataTable) As Byte()

        Using ms As New MemoryStream()

            Dim doc As New Document(PageSize.A4.Rotate(), 20, 20, 30, 40)

            Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
            writer.PageEvent = New AuditPdfPageEvent()

            doc.Open()

            ' ===== TITLE =====
            Dim titleFont As Font =
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)

            Dim subFont As Font =
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY)

            Dim title As New Paragraph("Karavalai Nidhi Ltd • Audit Trail Report", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            title.SpacingAfter = 4

            Dim subtitle As New Paragraph(
                    $"Generated on {DateTime.Now:dd-MMM-yyyy HH:mm}",
                    subFont
                )
            subtitle.Alignment = Element.ALIGN_CENTER
            subtitle.SpacingAfter = 14

            doc.Add(title)
            doc.Add(subtitle)

            ' ===== TABLE =====
            Dim table As New PdfPTable(dt.Columns.Count)
            table.WidthPercentage = 100
            table.HeaderRows = 1

            ' Header
            Dim headerFont As Font =
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE)

            For Each col As DataColumn In dt.Columns
                Dim cell As New PdfPCell(New Phrase(col.ColumnName, headerFont))
                cell.BackgroundColor = New BaseColor(37, 99, 235)
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                cell.Padding = 6
                table.AddCell(cell)
            Next

            ' Data
            Dim cellFont As Font =
                    FontFactory.GetFont(FontFactory.HELVETICA, 9)

            For Each row As DataRow In dt.Rows
                For Each col As DataColumn In dt.Columns
                    Dim cell As New PdfPCell(
                            New Phrase(Convert.ToString(row(col)), cellFont)
                        )
                    cell.Padding = 5
                    table.AddCell(cell)
                Next
            Next

            doc.Add(table)
            doc.Close()

            Return ms.ToArray()

        End Using

    End Function

    ' ----------------------------------------------------------
    ' PDF HELPERS
    ' ----------------------------------------------------------
    Public Shared Function GetAllAuditTrailPdf() As Byte()

        Dim dt As DataTable = GetAllAuditTrail()
        Return GenerateAuditTrailPdf(dt)

    End Function

    Public Shared Function GetAuditTrailPdfByDateRange(
            startDate As DateTime,
            endDate As DateTime
        ) As Byte()

        Dim dt As DataTable = GetAuditTrailByDateRange(startDate, endDate)
        Return GenerateAuditTrailPdf(dt)

    End Function

    ' ----------------------------------------------------------
    ' AUDIT SUMMARY (DASHBOARD)
    ' ----------------------------------------------------------
    Public Shared Function GetAuditSummary() As DataTable

        Dim dt As New DataTable()

        Using con As New SqlConnection(ConnStr)
            Using cmd As New SqlCommand("
                    SELECT
                        COUNT(*) AS TotalRecords,
                        MAX(ChangedOn) AS LastActivity,
                        (
                            SELECT TOP 1 ChangedBy
                            FROM fiscusdb.dbo.AuditTrail
                            GROUP BY ChangedBy
                            ORDER BY COUNT(*) DESC
                        ) AS MostActiveUser,
                        (
                            SELECT TOP 1 TableName
                            FROM fiscusdb.dbo.AuditTrail
                            GROUP BY TableName
                            ORDER BY COUNT(*) DESC
                        ) AS TopTable
                    FROM fiscusdb.dbo.AuditTrail
                ", con)

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using

            End Using
        End Using

        Return dt

    End Function

End Class

Partial Public Class AuditTrail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        If Not IsPostBack Then
            LoadAuditSummary()
            LoadAuditTrail()
        End If
    End Sub

    Private Sub LoadAuditTrail()

        Dim dt As DataTable = AuditTrailRepository.GetAllAuditTrail()

        ' Defensive check
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            gvAuditTrail.DataSource = Nothing
            gvAuditTrail.DataBind()
            Return
        End If

        ' Clone structure
        Dim top10 As DataTable = dt.Clone()

        ' Import first 10 rows
        For i As Integer = 0 To Math.Min(9, dt.Rows.Count - 1)
            top10.ImportRow(dt.Rows(i))
        Next

        gvAuditTrail.DataSource = top10
        gvAuditTrail.DataBind()

    End Sub


    Protected Sub btnDownload_Click(sender As Object, e As EventArgs)

        Dim pdfBytes As Byte()

        If rbAllData.Checked Then
            pdfBytes = AuditTrailRepository.GetAllAuditTrailPdf()
        Else
            Dim startDate As DateTime = DateTime.Parse(txtStartDate.Text)
            Dim endDate As DateTime = DateTime.Parse(txtEndDate.Text)

            pdfBytes = AuditTrailRepository.GetAuditTrailPdfByDateRange(startDate, endDate)
        End If

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=AuditTrail.pdf")
        Response.BinaryWrite(pdfBytes)
        Response.End()

    End Sub

    Private Sub LoadAuditSummary()

        Dim summary As DataTable = AuditTrailRepository.GetAuditSummary()

        If summary.Rows.Count = 1 Then
            Dim row = summary.Rows(0)

            lblTotalRecords.Text = row("TotalRecords").ToString()
            lblLastActivity.Text = Convert.ToDateTime(row("LastActivity")).ToString("dd-MMM-yyyy")
            lblTopTable.Text = row("TopTable").ToString()
        End If

    End Sub

End Class
