$filePath = "c:\Users\mithi\Desktop\Fiscus\DayBook.aspx.vb"
$lines = Get-Content $filePath

# Replace lines 1 to 7 (index 0 to 6)
$imports = @"
Imports System.Globalization
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
"@

# The Sub generate_pdf spans from line 556 to 737 (index 555 to 736)
$newMethod = @"
    Private Sub btnprnt_Click(sender As Object, e As EventArgs) Handles btnprnt.Click
        show_led()
        get_opening()
        
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('No records found to print.');", True)
            Return
        End If

        Dim pdfBytes As Byte() = GenerateDayBookPdf(dt, txtfrm.Text, txtto.Text, Session("ob"), Session("dr"), Session("cr"), Session("cb"), get_home())

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "inline; filename=DayBook_" & txtfrm.Text & "_to_" & txtto.Text & ".pdf")
        Response.BinaryWrite(pdfBytes)
        Response.End()
    End Sub

    Public Shared Function GenerateDayBookPdf(dtData As DataTable, fromDate As String, toDate As String, ob As String, dr As String, cr As String, cb As String, branchName As String) As Byte()
        Using ms As New MemoryStream()
            Dim doc As New Document(PageSize.A4, 20, 20, 30, 40)
            Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
            Dim printAction As PdfAction = New PdfAction(PdfAction.PRINTDIALOG)
            writer.SetOpenAction(printAction)
            doc.Open()

            Dim titleFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
            Dim subFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13, BaseColor.GRAY)

            Dim title As New Paragraph("Karavilai Nidhi Ltd " & Chr(149) & " Day Book", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            title.SpacingAfter = 4

            Dim subtitle As New Paragraph("Branch: " & branchName & " | From: " & fromDate & " To: " & toDate, subFont)
            subtitle.Alignment = Element.ALIGN_CENTER
            subtitle.SpacingAfter = 14

            doc.Add(title)
            doc.Add(subtitle)

            Dim table As New PdfPTable(5)
            table.WidthPercentage = 100
            table.SetWidths(New Single() {12.0F, 10.0F, 42.0F, 18.0F, 18.0F}) ' Date, V.No, Particulars, DR, CR
            table.HeaderRows = 1

            Dim headerFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE)
            Dim headers As String() = {"Date", "V.No", "Particulars", "Debit", "Credit"}

            For Each headerText As String In headers
                Dim cell As New PdfPCell(New Phrase(headerText, headerFont))
                cell.BackgroundColor = New BaseColor(37, 99, 235)
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                cell.Padding = 6
                table.AddCell(cell)
            Next

            Dim cellFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 9)
            Dim numFont As Font = FontFactory.GetFont(FontFactory.COURIER, 9)

            ' Opening Balance Row
            Dim cellBlank1 As New PdfPCell(New Phrase("", cellFont))
            Dim cellBlank2 As New PdfPCell(New Phrase("", cellFont))
            Dim cellOBLabel As New PdfPCell(New Phrase("Opening Balance", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)))
            Dim cellOBDebit As New PdfPCell(New Phrase("", numFont))
            Dim cellOBCredit As New PdfPCell(New Phrase(ob, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))

            table.AddCell(cellBlank1)
            table.AddCell(cellBlank2)
            table.AddCell(cellOBLabel)
            table.AddCell(cellOBDebit)
            cellOBCredit.HorizontalAlignment = Element.ALIGN_RIGHT
            table.AddCell(cellOBCredit)

            ' Data Rows
            Dim i As Integer = 1
            For Each row As DataRow In dtData.Rows
                Dim dtVal As String = Convert.ToDateTime(row("date")).ToString("dd-MM-yyyy")
                
                Dim cDate As New PdfPCell(New Phrase(dtVal, cellFont))
                cDate.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cDate)

                Dim cVno As New PdfPCell(New Phrase(i.ToString(), cellFont))
                cVno.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cVno)

                Dim part As String = Convert.ToString(row("achead")) & "    " & Convert.ToString(row("nar"))
                Dim cPart As New PdfPCell(New Phrase(part, cellFont))
                table.AddCell(cPart)

                Dim drVal As String = Convert.ToString(row("dr")).Replace(" ", "")
                Dim cDr As New PdfPCell(New Phrase(drVal, numFont))
                cDr.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cDr)

                Dim crVal As String = Convert.ToString(row("cr")).Replace(" ", "")
                Dim cCr As New PdfPCell(New Phrase(crVal, numFont))
                cCr.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cCr)

                i += 1
            Next

            ' Totals
            Dim cTotalLabel As New PdfPCell(New Phrase("Transaction", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)))
            Dim cTotalDr As New PdfPCell(New Phrase(dr, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))
            Dim cTotalCr As New PdfPCell(New Phrase(cr, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))

            cTotalDr.HorizontalAlignment = Element.ALIGN_RIGHT
            cTotalCr.HorizontalAlignment = Element.ALIGN_RIGHT

            Dim cE1 As New PdfPCell(New Phrase(""))
            cE1.Border = Rectangle.NO_BORDER
            Dim cE2 As New PdfPCell(New Phrase(""))
            cE2.Border = Rectangle.NO_BORDER
            
            table.AddCell(cE1)
            table.AddCell(cE2)
            cTotalLabel.Border = Rectangle.NO_BORDER
            cTotalLabel.HorizontalAlignment = Element.ALIGN_RIGHT
            table.AddCell(cTotalLabel)
            table.AddCell(cTotalDr)
            table.AddCell(cTotalCr)

            ' Closing Balance
            Dim cCBLabel As New PdfPCell(New Phrase("Closing Balance", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)))
            Dim cCBCredit As New PdfPCell(New Phrase(cb, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))
            cCBCredit.HorizontalAlignment = Element.ALIGN_RIGHT

            Dim cE3 As New PdfPCell(New Phrase(""))
            cE3.Border = Rectangle.NO_BORDER
            Dim cE4 As New PdfPCell(New Phrase(""))
            cE4.Border = Rectangle.NO_BORDER
            Dim cE5 As New PdfPCell(New Phrase(""))

            table.AddCell(cE3)
            table.AddCell(cE4)
            cCBLabel.Border = Rectangle.NO_BORDER
            cCBLabel.HorizontalAlignment = Element.ALIGN_RIGHT
            table.AddCell(cCBLabel)
            table.AddCell(cE5)
            table.AddCell(cCBCredit)

            doc.Add(table)
            doc.Close()

            Return ms.ToArray()
        End Using
    End Function
"@

# Create new array of lines
$newLines = @()
$newLines += $imports.Split([Environment]::NewLine)
# Add from line 8 to 555
for ($i = 7; $i -lt 555; $i++) {
    $newLines += $lines[$i]
}
$newLines += $newMethod.Split([Environment]::NewLine)
# Add from line 738 to end
for ($i = 737; $i -lt $lines.Length; $i++) {
    $newLines += $lines[$i]
}

Set-Content $filePath -Value $newLines
Write-Output "Successfully modified DayBook.aspx.vb"
