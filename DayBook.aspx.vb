Imports System.Globalization

Imports System

Imports System.Data

Imports System.Data.SqlClient

Imports System.Configuration

Imports System.IO

Imports iTextSharp.text

Imports iTextSharp.text.pdf



Public Class DayBook
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection

    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Public dt As New DataTable
    Dim newrow As DataRow
    Protected Sub signout(sender As Object, e As EventArgs)

        FormsAuthentication.SignOut()

        Session.Abandon()
        Log_out(Session("logtime").ToString, Session("sesusr"))
        Response.Redirect("..\login.aspx")

    End Sub


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        con.Open()

        If Not Page.IsPostBack Then

            ' bind_grid()


            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtfrm.ClientID), True)
        End If

    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        show_led()
        disp.PageIndex = e.NewPageIndex
        disp.DataBind()

    End Sub

    Sub show_led()
        If String.IsNullOrEmpty(txtfrm.Text) OrElse String.IsNullOrEmpty(txtto.Text) Then
            Return
        End If

        Dim dat As DateTime
        Dim dat1 As DateTime

        If Not DateTime.TryParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, dat) Then Return
        If Not DateTime.TryParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, dat1) Then Return

        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        If session_user_role = "Audit" Then
            query = "select date,transid,achead,narration,acn ,debit,credit,type from suplementc where CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' ORDER BY tid  "
        Else
            query = "select date,transid,achead,narration,acn ,debit,credit,type from suplement where CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' ORDER BY tid  "
        End If


        dt.Rows.Clear()
        If dt.Columns.Count = 0 Then
            dt.Columns.Add("date", GetType(Date))
            dt.Columns.Add("transid", GetType(String))
            dt.Columns.Add("achead", GetType(String))
            dt.Columns.Add("nar", GetType(String))
            dt.Columns.Add("dr", GetType(String))
            dt.Columns.Add("cr", GetType(String))
        End If

        Dim ds As New DataSet
        Try
            'Dim adapter As New SqlDataAdapter(query, con)
            cmd.Connection = con
            cmd.CommandText = query
            'cmd.Parameters.AddWithValue("@achead", glh.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@frm", reformatted)
            cmd.Parameters.AddWithValue("@to", reformatted1)

            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                pnlcb.Visible = True

                While dr.Read

                    If False Then ' Skip old redundant check
                    End If


                    Dim nam As String = get_name(dr(4))

                    newrow = dt.NewRow

                    newrow(0) = dr(0)
                    newrow(1) = dr(1)
                    newrow(2) = dr(2)

                    If Trim(dr(7)) = "RECEIPT" Or Trim(dr(7)) = "PAYMENT" Then
                        newrow(3) = dr(3) + " " + dr(4) + Trim(nam)
                    Else

                        newrow(3) = dr(3) + " " + Trim(nam)
                    End If
                    newrow(4) = String.Format("{0:N}", IIf(dr(5) = 0, " ", dr(5)))
                    newrow(5) = String.Format("{0:N}", IIf(dr(6) = 0, " ", dr(6)))
                    dt.Rows.Add(newrow)
                End While
            End If


            disp.DataSource = dt
            disp.DataBind()



        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()


        End Try
        '   lblnet.Text = String.Format("{0:N}", closing)
        'trim_disp()
        '

    End Sub

    Function get_name(ByVal acnox As String)

        Dim nam As String = ""
        If session_user_role = "Audit" Then
            query = "SELECT   member.FirstName FROM dbo.masterc LEFT OUTER JOIN dbo.member  ON masterc.cid = member.MemberNo WHERE masterc.acno = '" + acnox + "'"
        Else
            query = "SELECT   member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.member  ON master.cid = member.MemberNo WHERE master.acno = '" + acnox + "'"
        End If


        If con1.State = ConnectionState.Closed Then con1.Open()
        cmdi.Connection = con1
        cmdi.CommandText = query
        Try

            nam = cmdi.ExecuteScalar()

            If IsDBNull(nam) Then
                nam = " "
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con1.Close()
            cmdi.Dispose()
        End Try
        Return nam
    End Function
    Sub get_opening()
        If String.IsNullOrEmpty(txtfrm.Text) OrElse String.IsNullOrEmpty(txtto.Text) Then
            Return
        End If

        Dim sum_credit As Double
        Dim sum_debit As Double
        Dim opening As Double
        Dim closing_bal As Double

        Dim dat As DateTime
        Dim dat1 As DateTime

        If Not DateTime.TryParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, dat) Then Return
        If Not DateTime.TryParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, dat1) Then Return

        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim sdr As SqlDataReader

        If con.State = ConnectionState.Closed Then con.Open()



        If session_user_role = "Audit" Then
            query = "SELECT SUM(suplementc.debit) AS expr1, SUM(suplementc.credit) AS expr2 FROM dbo.suplementc WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
        Else

            query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
        End If


        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' "
        'cmd.Parameters.Clear()
        'cmd.Parameters.AddWithValue("@frm", reformatted)

        'cmd.Parameters.AddWithValue("@to", reformattedto)


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

        query = "select sum(ledger.ob) as expr1 from ledger"

        cmd.CommandText = query
        cmd.Connection = con

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

        If session_user_role = "Audit" Then
            query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplementc where  CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' "
        Else
            query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplement where  CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' "
        End If

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@frm", reformatted)

        cmd.Parameters.AddWithValue("@to", reformattedto)


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
            lbl_opening_debit.Text = String.Format("{0:N}", -opening)
            lbl_opening_credit.Text = ""
            Session("ob") = String.Format("{0:N}", -opening) & " Dr"
        Else
            lbl_opening_debit.Text = ""
            lbl_opening_credit.Text = String.Format("{0:N}", opening)
            Session("ob") = String.Format("{0:N}", opening) & " Cr"
        End If

        lbl_sum_credit.Text = String.Format("{0:N}", trans_credit)
        lbl_sum_debit.Text = String.Format("{0:N}", trans_debit)
        Session("cr") = String.Format("{0:N}", trans_credit)
        Session("dr") = String.Format("{0:N}", trans_debit)


        closing_bal = opening + (trans_credit - trans_debit)

        If closing_bal < 0 Then
            lbl_closing_debit.Text = String.Format("{0:N}", -closing_bal)
            lbl_closing_credit.Text = ""
            Session("cb") = String.Format("{0:N}", -closing_bal) & " Dr"
        Else
            lbl_closing_debit.Text = ""
            lbl_closing_credit.Text = String.Format("{0:N}", closing_bal)
            Session("cb") = String.Format("{0:N}", closing_bal) & " Cr"
        End If


    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        If String.IsNullOrEmpty(txtfrm.Text) OrElse String.IsNullOrEmpty(txtto.Text) Then
            Dim fnc As String = "showToastnOK('Error', 'Please select a valid date range.');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
            Return
        End If
        show_led()

        get_opening()
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

    Sub get_opening1(ByVal fdt As Date, ByVal tdt As Date)

        Dim sum_credit As Double
        Dim sum_debit As Double
        Dim opening As Double
        Dim closing_bal As Double

        Dim reformatted As String = fdt.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        Dim reformattedto As String = tdt.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim sdr As SqlDataReader

        If con.State = ConnectionState.Closed Then con.Open()


        If session_user_role = "Audit" Then

            query = "SELECT SUM(suplementc.debit) AS expr1, SUM(suplementc.credit) AS expr2 FROM dbo.suplementc WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
        Else

            query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
        End If
        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' "
        'cmd.Parameters.Clear()
        'cmd.Parameters.AddWithValue("@frm", reformatted)

        'cmd.Parameters.AddWithValue("@to", reformattedto)


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

        query = "select sum(ledger.ob) as expr1 from ledger"

        cmd.CommandText = query
        cmd.Connection = con

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

        If session_user_role = "Audit" Then

            query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplementc where  CONVERT(VARCHAR(20), date, 112) between @frm and @to  and type <> 'JOURNAL' "
        Else
            query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplement where  CONVERT(VARCHAR(20), date, 112) between @frm and @to  and type <> 'JOURNAL' "

        End If





        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@frm", reformatted)

        cmd.Parameters.AddWithValue("@to", reformattedto)


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
            Session("ob") = String.Format("{0:N}", -opening)
        Else

            Session("ob") = String.Format("{0:N}", opening)
        End If

        Session("cr") = String.Format("{0:N}", trans_credit)
        Session("dr") = String.Format("{0:N}", trans_debit)


        closing_bal = opening + (trans_credit - trans_debit)

        If closing_bal < 0 Then
            Session("cb") = String.Format("{0:N}", -closing_bal)
        Else

            Session("cb") = String.Format("{0:N}", closing_bal)
        End If


    End Sub



    Private Sub btnprnt_Click(sender As Object, e As EventArgs) Handles btnprnt.Click
        If String.IsNullOrEmpty(txtfrm.Text) OrElse String.IsNullOrEmpty(txtto.Text) Then
            Dim fnc As String = "showToastnOK('Error', 'Please select a valid date range to print.');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
            Return
        End If
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



            ' Colors
            Dim redColor As New BaseColor(220, 38, 38)
            Dim greenColor As New BaseColor(22, 163, 74)
            Dim lightBlue As New BaseColor(219, 234, 254)

            ' Opening Balance Row
            Dim cellBlank1 As New PdfPCell(New Phrase("", cellFont))
            Dim cellBlank2 As New PdfPCell(New Phrase("", cellFont))
            Dim obLabelFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)
            Dim cellOBLabel As New PdfPCell(New Phrase("Opening Balance", obLabelFont))
            cellOBLabel.HorizontalAlignment = Element.ALIGN_RIGHT
            cellOBLabel.Padding = 5

            Dim cellOBValue As New PdfPCell(New Phrase(ob, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9, greenColor)))
            cellOBValue.HorizontalAlignment = Element.ALIGN_RIGHT
            cellOBValue.Padding = 5

            table.AddCell(cellBlank1)
            table.AddCell(cellBlank2)
            table.AddCell(cellOBLabel)
            
            If Not String.IsNullOrEmpty(ob) AndAlso ob.Contains("Dr") Then
                table.AddCell(cellOBValue)
                table.AddCell(New PdfPCell(New Phrase("", numFont)))
            Else
                table.AddCell(New PdfPCell(New Phrase("", numFont)))
                table.AddCell(cellOBValue)
            End If

            ' Data Rows
            Dim k As Integer = 1
            For Each row As DataRow In dtData.Rows
                Dim dtVal As String = Convert.ToDateTime(row("date")).ToString("dd-MM-yyyy")
                
                Dim cDateCell As New PdfPCell(New Phrase(dtVal, cellFont))
                cDateCell.HorizontalAlignment = Element.ALIGN_CENTER
                cDateCell.Padding = 4
                table.AddCell(cDateCell)

                Dim cVnoCell As New PdfPCell(New Phrase(k.ToString(), cellFont))
                cVnoCell.HorizontalAlignment = Element.ALIGN_CENTER
                cVnoCell.Padding = 4
                table.AddCell(cVnoCell)

                Dim partStr As String = Convert.ToString(row("achead")) & vbCrLf & "   " & Convert.ToString(row("nar"))
                Dim cPartCell As New PdfPCell(New Phrase(partStr, cellFont))
                cPartCell.Padding = 4
                table.AddCell(cPartCell)

                Dim dVal As String = Convert.ToString(row("dr")).Replace(" ", "")
                Dim cDrCell As New PdfPCell(New Phrase(dVal, numFont))
                cDrCell.HorizontalAlignment = Element.ALIGN_RIGHT
                cDrCell.Padding = 4
                table.AddCell(cDrCell)

                Dim cVal As String = Convert.ToString(row("cr")).Replace(" ", "")
                Dim cCrCell As New PdfPCell(New Phrase(cVal, numFont))
                cCrCell.HorizontalAlignment = Element.ALIGN_RIGHT
                cCrCell.Padding = 4
                table.AddCell(cCrCell)

                k += 1
            Next

            ' Totals Row
            Dim cTotalLabelCell As New PdfPCell(New Phrase("Total", obLabelFont))
            cTotalLabelCell.HorizontalAlignment = Element.ALIGN_RIGHT
            cTotalLabelCell.Padding = 5
            cTotalLabelCell.BackgroundColor = lightBlue

            Dim cTotalDrCell As New PdfPCell(New Phrase(dr, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))
            cTotalDrCell.HorizontalAlignment = Element.ALIGN_RIGHT
            cTotalDrCell.Padding = 5
            cTotalDrCell.BackgroundColor = lightBlue

            Dim cTotalCrCell As New PdfPCell(New Phrase(cr, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))
            cTotalCrCell.HorizontalAlignment = Element.ALIGN_RIGHT
            cTotalCrCell.Padding = 5
            cTotalCrCell.BackgroundColor = lightBlue

            Dim ce1_ As New PdfPCell(New Phrase(""))
            ce1_.Border = Rectangle.NO_BORDER
            Dim ce2_ As New PdfPCell(New Phrase(""))
            ce2_.Border = Rectangle.NO_BORDER
            
            table.AddCell(ce1_)
            table.AddCell(ce2_)
            table.AddCell(cTotalLabelCell)
            table.AddCell(cTotalDrCell)
            table.AddCell(cTotalCrCell)

            ' Closing Balance Row
            Dim cCBLabelCell As New PdfPCell(New Phrase("Closing Balance", obLabelFont))
            cCBLabelCell.HorizontalAlignment = Element.ALIGN_RIGHT
            cCBLabelCell.Padding = 5

            Dim cCBValue As New PdfPCell(New Phrase(cb, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9, greenColor)))
            cCBValue.HorizontalAlignment = Element.ALIGN_RIGHT
            cCBValue.Padding = 5

            Dim ce3_ As New PdfPCell(New Phrase(""))
            ce3_.Border = Rectangle.NO_BORDER
            Dim ce4_ As New PdfPCell(New Phrase(""))
            ce4_.Border = Rectangle.NO_BORDER

            table.AddCell(ce3_)
            table.AddCell(ce4_)
            table.AddCell(cCBLabelCell)

            If Not String.IsNullOrEmpty(cb) AndAlso cb.Contains("Dr") Then
                table.AddCell(cCBValue)
                table.AddCell(New PdfPCell(New Phrase("", numFont)))
            Else
                table.AddCell(New PdfPCell(New Phrase("", numFont)))
                table.AddCell(cCBValue)
            End If



            doc.Add(table)

            doc.Close()



            Return ms.ToArray()

        End Using

    End Function

    Private Sub DayBook_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub disp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles disp.RowCommand

        If Not e.CommandName = "Page" Then
            Dim rowSelect As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)
            Dim rowindex As Integer = rowSelect.RowIndex

            Dim dat As String = TryCast(rowSelect.FindControl("lbl_dt"), Label).Text
            Dim id As String = TryCast(rowSelect.FindControl("lbl_tid"), Label).Text

            show_brkup(id, dat)
        End If


    End Sub


    Sub show_brkup(ByVal id As String, ByVal dat As String)


        Dim tdat As DateTime = DateTime.ParseExact(dat, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = tdat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()

            query = "Select masterc.cid,member.FirstName,masterc.acno,actransc.Drd As debit,actransc.Crd As credit FROM dbo.actransc INNER JOIN dbo.masterc  On actransc.acno = masterc.acno "
            query += "INNER Join dbo.member ON masterc.cid = member.MemberNo WHERE actransc.Id = @id And CONVERT(VARCHAR(20), actransc.date, 112) = @dt"


            Dim ds As New DataSet

            Using adapter As New SqlDataAdapter(query, con)
                adapter.SelectCommand.Parameters.Clear()


                adapter.SelectCommand.Parameters.AddWithValue("@id", CInt(id))
                adapter.SelectCommand.Parameters.AddWithValue("@dt", reformatted)

                adapter.Fill(ds)

            End Using

            rpknl.DataSource = ds
            rpknl.DataBind()

        End Using

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Show", "<script> $('#knlmodal').toggle();</script>", False)




    End Sub

End Class
