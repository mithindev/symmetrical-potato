Imports System
Imports System.Data
    Imports System.Data.SqlClient
    Imports System.Configuration
    Imports System.IO
    Imports System.Globalization
    Imports System.Web.Services
    Imports iTextSharp.text
    Imports iTextSharp.text.pdf

Public Class Ledger
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public dt As New DataTable
    Dim newrow As DataRow


    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        show_led()
        disp.PageIndex = e.NewPageIndex
        disp.DataBind()


    End Sub

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then



            '            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", glh.ClientID), True)
        End If

    End Sub


    Sub show_led()


        disp.DataSource = Nothing
        disp.DataBind()


        Dim ccr As Double = 0
        Dim cdr As Double = 0

        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        If Session("session_user_role") = "Audit" Then
            query = "select date,narration,debit,credit,acn from suplementc where achead=@achead and CONVERT(VARCHAR(20), date, 112) between @frm and @to ORDER BY date,tid "
        Else
            query = "select date,narration,debit,credit,acn from suplement where achead=@achead and CONVERT(VARCHAR(20), date, 112) between @frm and @to ORDER BY date,tid "
        End If


        Dim ds As New DataSet
        Try
            'Dim adapter As New SqlDataAdapter(query, con)
            cmd.Connection = con
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@achead", txtprod.Text)
            cmd.Parameters.AddWithValue("@frm", reformatted)
            cmd.Parameters.AddWithValue("@to", reformatted1)

            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                While dr.Read

                    If dt.Columns.Count = 0 Then
                        dt.Columns.Add("date", GetType(Date))
                        dt.Columns.Add("narration", GetType(String))
                        dt.Columns.Add("debit", GetType(String))
                        dt.Columns.Add("credit", GetType(String))
                        dt.Columns.Add("balance", GetType(String))
                    End If




                    newrow = dt.NewRow
                    Dim particular As String = dr(1)
                    If Not dr(1).ToString().Contains("(") Then
                        particular = dr(1) + " " + dr(4)
                    End If
                    newrow(0) = dr(0)
                    newrow(1) = particular
                    newrow(2) = String.Format("{0:N}", IIf(dr(2) = 0, "", dr(2)))
                    cdr = cdr + dr(2)
                    newrow(3) = String.Format("{0:N}", IIf(dr(3) = 0, "", dr(3)))
                    ccr = ccr + dr(3)
                    Session("closing") = Session("closing") + dr(3) - dr(2)
                    newrow(4) = String.Format("{0:N}", Session("closing"))
                    dt.Rows.Add(newrow)
                End While


                newrow = dt.NewRow
                dt.Rows.Add(newrow)
                '  dt.Rows.Add(newrow)

                newrow = dt.NewRow

                newrow(1) = "   TOTAL   "
                newrow(2) = String.Format("{0:N}", cdr)
                newrow(3) = String.Format("{0:N}", ccr)
                dt.Rows.Add(newrow)
                disp.DataSource = dt
                disp.DataBind()
            End If


        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()


        End Try
        lblnet.Text = String.Format("{0:N}", Session("closing"))
        'trim_disp()
        '

    End Sub

    Sub trim_disp()


        For i As Integer = 0 To disp.Rows.Count - 1
            Dim debit As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbldebit"), Label)
            Dim credit As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblcredit"), Label)
            ' Dim narx As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblnar"), Label)
            '  Dim acnx As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblacn"), Label)

            Dim cr As Double = IIf(credit.Text = "", 0, credit.Text)
            Dim dr As Double = IIf(debit.Text = "", 0, debit.Text)

            Session("closing") = Session("closing") + cr - dr

            'If CDbl(debit.Text) = 0 Then debit.Text = ""
            'If CDbl(credit.Text) = 0 Then credit.Text = ""


        Next



    End Sub

    Sub get_opening()

        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()
        If Session("session_user_role") = "Audit" Then
            query = "select SUM(suplementc.debit) AS expr1,SUM(suplementc.credit) AS expr2 from suplementc where CONVERT(VARCHAR(20), date, 112) < @dt and achead=@glh"
        Else
            query = "select SUM(suplement.debit) AS expr1,SUM(suplement.credit) AS expr2 from suplement where CONVERT(VARCHAR(20), date, 112) < @dt and achead=@glh"

        End If

        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@dt", reformatted)
        cmd.Parameters.AddWithValue("@glh", txtprod.Text)

        Try
            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()

                Dim debit As Double = IIf(IsDBNull(dr(0)), 0, dr(0))
                Dim credit As Double = IIf(IsDBNull(dr(1)), 0, dr(1))
                Session("closing") = credit - debit 'dr(1) - dr(0)

            Else
                Session("closing") = 0
            End If
            dr.Close()

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()


        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select ob from ledger where ledger='" + txtprod.Text + "'"
        cmd.Connection = con
        cmd.CommandText = query

        Try
            Dim ob As Double = cmd.ExecuteScalar()

            If Not IsDBNull(ob) Then
                Session("closing") = Session("closing") + ob
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try
        lbl_ob.Text = String.Format("{0:N}", Session("closing"))

    End Sub


    Function get_home()

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = "SELECT TOP 1 branch FROM branch"

        Try
            home = cmd.ExecuteScalar


            If home Is Nothing Then
                home = "KARAVILAI"
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        Return home


    End Function

    Sub generate_pdf()

        Dim br As String = get_home()



        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim rundt As DateTime = dat
        Dim reformatted As String = rundt.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)




        Dim sb As StringBuilder = New StringBuilder()


        sb.Append("<h4>KARAVILAI BENEFIT FUND LTD</h4>")
        sb.Append(br + " Branch")
        sb.Append("<br/>")

        sb.Append("Account :&nbsp;&nbsp;&nbsp;<b>" + txtprod.Text + "</b><br/>")


        sb.Append("Statements Of Account From " + dat.ToShortDateString + "   To   " + dat1.ToShortDateString)


        sb.Append("<br/>")
        sb.Append("<br/>")
        sb.Append("<table cellpadding='5' cellspacing='0' style='border: 0px solid #ccc;font-size: 9pt;font-family:Arial;width:85%;page-break-after: always;'>")
        sb.Append("<thead style='display:table-header-group;'>")
        sb.Append("<tr>")
        'For Each column As DataColumn In dt.Columns
        '    sb.Append(("<th style='background-color: #B8DBFD;border: 0px solid #ccc'>" _
        '                    + (column.ColumnName + "</th>")))
        'Next
        sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:12%;text-aligh:center;>Date</th>")
        sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:52%;text-aligh:center;>Particulars</th>")
        sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:13%;text-aligh:center;>Debit</th>")
        sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:13%;text-aligh:center;>Credit</th>")

        sb.Append("</tr>")
        sb.Append("</thead>")






        sb.Append("<tbody>")


        While (rundt <= dat1)

            Dim i = 1
            reformatted = rundt.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            get_opening1(rundt, rundt)




            sb.Append("<tr>")
            sb.Append("<td style='border: 0px solid #ccc;width:12%;'>" + rundt.ToShortDateString + "</td>")
            'sb.Append("<td style='border: % solid #ccc;width:10%;'></td>")
            sb.Append("<td style='text-align:left;border: 0px solid #ccc;width:52%'> Opening Balance</td>")
            sb.Append("<td style='text-align:right;border: 0px solid #ccc;width:13%'>" + Session("dob") + "</td>")
            sb.Append("<td style='text-align:right;border: 0px solid #ccc;width:13%'>" + Session("cob") + "</td>")
            sb.Append("</tr>")


            If con.State = ConnectionState.Closed Then con.Open()

            query = "select date,transid,achead,narration,acn ,debit,credit from suplement where MONTH(date)=@mnt and YEAR(date)=@yr and achead=@achead   ORDER BY date  "

            Dim ds As New DataSet
            Try
                'Dim adapter As New SqlDataAdapter(query, con)
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()

                'cmd.Parameters.AddWithValue("@achead", txtprod.text)
                cmd.Parameters.AddWithValue("@mnt", rundt.Month)
                cmd.Parameters.AddWithValue("@yr", rundt.Year)
                cmd.Parameters.AddWithValue("@achead", txtprod.Text)



                Dim dr As SqlDataReader

                dr = cmd.ExecuteReader()

                If dr.HasRows() Then
                    While dr.Read


                        sb.Append("<tr>")
                        sb.Append("<td style='border: 0px solid #ccc;width:12%'>" + dr(0) + "</td>")
                        'sb.Append("<td style='border: 0px solid #ccc;width:10%'>" + CStr(i) + "</td>")
                        sb.Append("<td style='text-align:left;border: 0px solid #ccc;width:52%'>" + Convert.ToString(dr(3)) + "    " + Convert.ToString(dr(4)) + "</td>")
                        sb.Append("<td style='text-align:right;border: 0px solid #ccc;width:13%'>" + String.Format("{0:N}", IIf(dr(5) = 0, " ", dr(5))) + "</td>")
                        sb.Append("<td style='text-align:right;border: 0px solid #ccc;width:13%'>" + String.Format("{0:N}", IIf(dr(6) = 0, " ", dr(6))) + "</td>")
                        sb.Append("</tr>")
                        i = i + 1
                    End While
                End If
                i = 1
                dr.Close()
                sb.Append("<tr>")
                sb.Append("<td>&nbsp;</td>")
                sb.Append("</tr>")

                sb.Append("<tr>")
                sb.Append("<td style='border: 0px solid #ccc;width:12%'></td>")
                ' sb.Append("<td style='border: 0px solid #ccc;width:10%'></td>")
                sb.Append("<td style='text-align:left;border: 0px solid #ccc;width:52%'>Transaction </td>")
                sb.Append("<td style='text-align:right;border: 0px solid #ccc;width:13%'>" + Session("dr") + "</td>")
                sb.Append("<td style='text-align:right;border: 0px solid #ccc;width:13%'>" + Session("cr") + "</td>")
                sb.Append("</tr>")

                sb.Append("<tr>")
                sb.Append("<td style='border: 0px solid #ccc;width:12%'></td>")
                ' sb.Append("<td style='border: 0px solid #ccc;width:10%'></td>")
                sb.Append("<td style='text-align:left;border: 0px solid #ccc;width:52%'>Closing Balance </td>")
                sb.Append("<td style='text-align:right;border: 0px solid #ccc;width:13%'>" + Session("dcb") + "</td>")
                sb.Append("<td style='text-align:right;border: 0px solid #ccc;width:13%'>" + Session("ccb") + "</td>")
                sb.Append("</tr>")
                sb.Append("<tr>")
                sb.Append("<td>&nbsp;</td>")
                sb.Append("</tr>")
                sb.Append("<tr>")
                sb.Append("<td>&nbsp;</td>")
                sb.Append("</tr>")

                'For Each column As DataColumn In dt.Columns
                ' sb.Append(("<td style='width:100px;border: 0px solid #ccc'>" _
                '                + (row(column.ColumnName).ToString + "</td>")))


                ' Next

                ''Table end.



            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally

                cmd.Dispose()
                con.Close()


            End Try
            rundt = rundt.AddMonths(1)




        End While

        sb.Append("</tbody>")
        sb.Append("</table>")


        '  Dim converter As New HtmlToPdf
        '
        ' Dim html As String = sb.ToString
        'Dim doc As PdfDocument = converter.ConvertHtmlString(html, "")
        Dim prefix = Mid(br, 1, 1)
        Dim fpath = prefix.ToString + Mid(reformatted, 1, 6).ToString


        Using sw As StreamWriter = New StreamWriter("d:\statements\" + fpath + ".html")

            Using writer As HtmlTextWriter = New HtmlTextWriter(sw)
                writer.RenderBeginTag(HtmlTextWriterTag.Html)
                writer.RenderBeginTag(HtmlTextWriterTag.Head)
                writer.Write("")
                writer.RenderEndTag()
                writer.RenderBeginTag(HtmlTextWriterTag.Body)
                writer.Write(sb.ToString)
                writer.RenderEndTag()
                writer.RenderEndTag()
            End Using
        End Using
        'Dim cleanString As String = acno.Replace("/", "")



        ''    doc.Save("d:\statements\" + fpath + ".Pdf")
        'Dim renderer = New IronPdf.HtmlToPdf()
        'Dim pdf = renderer.RenderHTMLFileAsPdf("d:\statements\" + fpath + ".html")

        'Dim html = sb.ToString
        'renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print

        'Dim document = renderer.RenderHtmlAsPdf(html)
        'pdf.SaveAs("d:\statements\" + fpath + ".Pdf")
        System.Diagnostics.Process.Start("d:\statements\" + fpath + ".html")


        sb.Clear()





    End Sub
    Sub get_opening1(ByVal fdt As Date, ByVal tdt As Date)

        Dim sum_credit As Double
        Dim sum_debit As Double
        Dim opening As Double
        Dim closing_bal As Double

        Dim fd As Integer
        Dim fy As Integer



        Dim dat As DateTime = DateTime.ParseExact(fdt, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        Dim dat1 As DateTime = DateTime.ParseExact(tdt, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim sdr As SqlDataReader

        If con.State = ConnectionState.Closed Then con.Open()


        If Session("session_user_role") = "Audit" Then
            query = "SELECT SUM(suplementc.debit) AS expr1, SUM(suplementc.credit) AS expr2 FROM dbo.suplementc WHERE CONVERT(VARCHAR(20), date, 112) < @frm and achead=@achead"
        Else
            query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) < @frm and achead=@achead"

        End If


        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' "
        cmd.Parameters.Clear()

        If fdt.Month = 1 Then

            fd = 12
            fy = fdt.Year - 1

        Else
            fd = fdt.Month - 1
            fy = fdt.Year

        End If
        cmd.Parameters.AddWithValue("@frm", reformatted)
        cmd.Parameters.AddWithValue("@yr", fy)
        cmd.Parameters.AddWithValue("@achead", txtprod.Text)



        cmd.CommandText = query

        Try

            sdr = cmd.ExecuteReader()

            If sdr.HasRows Then

                sdr.Read()

                sum_credit = IIf(IsDBNull(sdr(1)), 0, sdr(1))
                sum_debit = IIf(IsDBNull(sdr(0)), 0, sdr(0))



            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally






            cmd.Dispose()
            con.Close()



        End Try

        Dim oResult As Double = 0

        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 105) <'" + Convert.ToDateTime(txtdate.Text)

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select ledger.ob as expr1 from ledger where ledger=@ledger "

        cmd.CommandText = query
        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@ledger", txtprod.Text)

        Try

            oResult = cmd.ExecuteScalar()


            If Not IsNothing(oResult) Then
                opening = oResult
            Else
                opening = 0
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()


        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        If Session("session_user_role") = "Audit" Then
            query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplementc where MONTH(date)=@mnt and YEAR(date)=@yr and achead=@achead "
        Else
            query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplement where MONTH(date)=@mnt and YEAR(date)=@yr and achead=@achead "

        End If



        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@mnt", fdt.Month)


        cmd.Parameters.AddWithValue("@yr", fdt.Year)
        cmd.Parameters.AddWithValue("@achead", txtprod.Text)



        cmd.CommandText = query
        cmd.Connection = con

        Dim dr As SqlDataReader

        Dim trans_credit As Double = 0

        Dim trans_debit As Double = 0

        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()

                trans_credit = IIf(IsDBNull(dr(0)), 0, dr(0))

                trans_debit = IIf(IsDBNull(dr(1)), 0, dr(1))



            End If

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()

        End Try



        opening = opening + (sum_credit - sum_debit)


        If opening < 0 Then
            Session("dob") = String.Format("{0:N}", -opening)
            Session("cob") = ""
        Else

            Session("cob") = String.Format("{0:N}", opening)
            Session("dob") = ""
        End If

        Session("cr") = String.Format("{0:N}", trans_credit)
        Session("dr") = String.Format("{0:N}", trans_debit)


        closing_bal = opening + (trans_credit - trans_debit)

        If closing_bal < 0 Then
            Session("dcb") = String.Format("{0:N}", -closing_bal)
            Session("ccb") = ""
        Else

            Session("ccb") = String.Format("{0:N}", closing_bal)
            Session("dcb") = ""
        End If


    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        get_opening()

        ' generate_pdf()

        show_led()

    End Sub

    Private Sub btnprnt_Click(sender As Object, e As EventArgs) Handles btnprnt.Click
        If String.IsNullOrEmpty(txtfrm.Text) OrElse String.IsNullOrEmpty(txtto.Text) Then
            Dim fnc As String = "showToastnOK('Error', 'Please select a valid date range to print.');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
            Return
        End If
        If String.IsNullOrEmpty(txtprod.Text) Then
            Dim fnc As String = "showToastnOK('Error', 'Please select a Ledger to print.');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
            Return
        End If

        get_opening()
        show_led()

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('No records found to print.');", True)
            Return
        End If

        Dim openingBalance As Double = 0
        If Session("closing") IsNot Nothing Then
            Double.TryParse(Session("closing").ToString(), openingBalance)
        End If

        Dim oDr As String = If(openingBalance < 0, String.Format("{0:N}", -openingBalance), "")
        Dim oCr As String = If(openingBalance >= 0, String.Format("{0:N}", openingBalance), "")

        Dim pdfBytes As Byte() = GenerateLedgerPdf(dt, txtfrm.Text, txtto.Text, txtprod.Text, oDr, oCr, get_home())

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "inline; filename=Ledger_" & txtprod.Text & "_" & txtfrm.Text & "_to_" & txtto.Text & ".pdf")
        Response.BinaryWrite(pdfBytes)
        Response.End()
    End Sub

    Public Shared Function GenerateLedgerPdf(dtData As DataTable, fromDate As String, toDate As String, ledgerName As String, oDr As String, oCr As String, branchName As String) As Byte()
        Using ms As New MemoryStream()
            Dim doc As New Document(PageSize.A4, 20, 20, 30, 40)
            Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
            Dim printAction As PdfAction = New PdfAction(PdfAction.PRINTDIALOG)
            writer.SetOpenAction(printAction)
            doc.Open()

            Dim titleFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
            Dim subFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13, BaseColor.GRAY)

            Dim title As New Paragraph("Karavilai Nidhi Ltd " & Chr(149) & " General Ledger", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            title.SpacingAfter = 4

            Dim subtitle As New Paragraph("Branch: " & branchName & " | Ledger: " & ledgerName, subFont)
            subtitle.Alignment = Element.ALIGN_CENTER
            subtitle.SpacingAfter = 4

            Dim dateSubtitle As New Paragraph("From: " & fromDate & " To: " & toDate, subFont)
            dateSubtitle.Alignment = Element.ALIGN_CENTER
            dateSubtitle.SpacingAfter = 14

            doc.Add(title)
            doc.Add(subtitle)
            doc.Add(dateSubtitle)

            Dim table As New PdfPTable(5)
            table.WidthPercentage = 100
            table.SetWidths(New Single() {15.0F, 40.0F, 15.0F, 15.0F, 15.0F}) ' Date, Particulars, Debit, Credit, Balance
            table.HeaderRows = 1

            Dim headerFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE)
            Dim headers As String() = {"Date", "Particulars", "Debit", "Credit", "Balance"}

            For Each headerText As String In headers
                Dim cell As New PdfPCell(New Phrase(headerText, headerFont))
                cell.BackgroundColor = New BaseColor(37, 99, 235)
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                cell.Padding = 6
                table.AddCell(cell)
            Next

            Dim cellFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 9)
            Dim numFont As Font = FontFactory.GetFont(FontFactory.COURIER, 9)
            Dim obLabelFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)
            Dim lightBlue As New BaseColor(219, 234, 254)

            ' Opening Balance Row
            Dim cellBlank1 As New PdfPCell(New Phrase("", cellFont))
            Dim cellOBLabel As New PdfPCell(New Phrase("Opening Balance", obLabelFont))
            cellOBLabel.HorizontalAlignment = Element.ALIGN_LEFT
            cellOBLabel.Padding = 5

            Dim celloDrValue As New PdfPCell(New Phrase(oDr, numFont))
            celloDrValue.HorizontalAlignment = Element.ALIGN_RIGHT
            celloDrValue.Padding = 5

            Dim celloCrValue As New PdfPCell(New Phrase(oCr, numFont))
            celloCrValue.HorizontalAlignment = Element.ALIGN_RIGHT
            celloCrValue.Padding = 5

            table.AddCell(cellBlank1)
            table.AddCell(cellOBLabel)
            table.AddCell(celloDrValue)
            table.AddCell(celloCrValue)
            table.AddCell(New PdfPCell(New Phrase("", numFont)))

            ' Data Rows
            For i As Integer = 0 To dtData.Rows.Count - 1
                Dim row As DataRow = dtData.Rows(i)
                Dim dtVal As String = ""
                If Not IsDBNull(row("date")) AndAlso Not String.IsNullOrEmpty(row("date").ToString()) Then
                    dtVal = Convert.ToDateTime(row("date")).ToString("dd-MM-yyyy")
                End If

                Dim partStr As String = Convert.ToString(row("narration"))

                If partStr.Trim() = "TOTAL" Then
                    Dim cBlankCell As New PdfPCell(New Phrase("", cellFont))
                    cBlankCell.BackgroundColor = lightBlue
                    table.AddCell(cBlankCell)

                    Dim cPartCell As New PdfPCell(New Phrase("Total", obLabelFont))
                    cPartCell.HorizontalAlignment = Element.ALIGN_RIGHT
                    cPartCell.Padding = 4
                    cPartCell.BackgroundColor = lightBlue
                    table.AddCell(cPartCell)

                    Dim dVal As String = Convert.ToString(row("debit")).Replace(" ", "")
                    Dim cDrCell As New PdfPCell(New Phrase(dVal, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))
                    cDrCell.HorizontalAlignment = Element.ALIGN_RIGHT
                    cDrCell.Padding = 4
                    cDrCell.BackgroundColor = lightBlue
                    table.AddCell(cDrCell)

                    Dim cVal As String = Convert.ToString(row("credit")).Replace(" ", "")
                    Dim cCrCell As New PdfPCell(New Phrase(cVal, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))
                    cCrCell.HorizontalAlignment = Element.ALIGN_RIGHT
                    cCrCell.Padding = 4
                    cCrCell.BackgroundColor = lightBlue
                    table.AddCell(cCrCell)

                    Dim cBalBlankCell As New PdfPCell(New Phrase("", cellFont))
                    cBalBlankCell.BackgroundColor = lightBlue
                    table.AddCell(cBalBlankCell)
                ElseIf String.IsNullOrEmpty(partStr) AndAlso String.IsNullOrEmpty(dtVal) Then
                     ' skip completely empty rows added before total
                     Continue For
                Else
                    Dim cDateCell As New PdfPCell(New Phrase(dtVal, cellFont))
                    cDateCell.HorizontalAlignment = Element.ALIGN_CENTER
                    cDateCell.Padding = 4
                    table.AddCell(cDateCell)

                    Dim cPartCell As New PdfPCell(New Phrase(partStr, cellFont))
                    cPartCell.Padding = 4
                    table.AddCell(cPartCell)

                    Dim dVal As String = Convert.ToString(row("debit")).Replace(" ", "")
                    Dim cDrCell As New PdfPCell(New Phrase(dVal, numFont))
                    cDrCell.HorizontalAlignment = Element.ALIGN_RIGHT
                    cDrCell.Padding = 4
                    table.AddCell(cDrCell)

                    Dim cVal As String = Convert.ToString(row("credit")).Replace(" ", "")
                    Dim cCrCell As New PdfPCell(New Phrase(cVal, numFont))
                    cCrCell.HorizontalAlignment = Element.ALIGN_RIGHT
                    cCrCell.Padding = 4
                    table.AddCell(cCrCell)
                    
                    Dim bVal As String = ""
                    If row.Table.Columns.Contains("balance") AndAlso Not IsDBNull(row("balance")) Then
                        bVal = Convert.ToString(row("balance")).Replace(" ", "")
                    End If
                    Dim cBalCell As New PdfPCell(New Phrase(bVal, numFont))
                    cBalCell.HorizontalAlignment = Element.ALIGN_RIGHT
                    cBalCell.Padding = 4
                    table.AddCell(cBalCell)
                End If
            Next

            doc.Add(table)
            doc.Close()
            writer.Close()

            Return ms.ToArray()
        End Using
    End Function

    Private Sub Ledger_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If Session("session_user_role") = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class