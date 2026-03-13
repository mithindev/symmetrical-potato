Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.Drawing.Printing


Public Class denomination
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim newrow As DataRow
    Dim home As String
    Public dt As New DataTable



    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        bind_grid()

    End Sub


    Sub bind_rp()


        rpscroll.DataSource = Nothing
        rpscroll.DataBind()

        If inclpass.Checked Then
            rpScrollClass.Attributes.Add("style", "background-color: lightgoldenrodyellow;")
            Session("passed_v") = 1
        Else
            rpScrollClass.Attributes.Add("style", "background-color: white;")
            Session("passed_v") = 0
        End If


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "getDenom"
                cmd.Parameters.Clear()

                cmd.Parameters.Add(New SqlParameter("@cld", Session("passed_v")))

                Try

                    Using Sda As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable
                        Sda.Fill(dt)


                        rpscroll.DataSource = dt
                        rpscroll.DataBind()
                        'get_locked(dt)


                    End Using

                Catch ex As Exception

                    Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)


                End Try


            End Using
            con.Close()

        End Using

    End Sub

    Sub bind_grid()


        ' ddtid.Items.Clear()

        Dim ds As New DataSet

        If inclpass.Checked = False Then
            query = "SELECT ID FROM dbo.trans WHERE trans.denom <>'" + "1" + "' and type='CASH' or type='TRF'"
        Else
            query = "SELECT ID FROM dbo.trans WHERE trans.denom ='" + "1" + "' and type='CASH' or type='TRF'"
        End If

        'query = "SELECT  trans.Id,  trans.acno,  member.FirstName AS suplimentry,  trans.Drd,  trans.Crd,  trans.denom,  trans.Type FROM dbo.master LEFT OUTER JOIN dbo.trans   ON master.acno = trans.acno LEFT OUTER JOIN dbo.member   ON master.cid = member.MemberNo WHERE trans.denom = 0 AND trans.Type = 'CASH'"
        Dim adapter As New SqlDataAdapter(query, con)

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
            cmd.CommandText = query

        cmd.Parameters.Clear()

        Dim dr As SqlDataReader

        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                Do While dr.Read()

                    '  ddtid.Items.Add(dr(0).ToString)
                Loop

            End If


            ' ddtid.Items.Insert(0, "<-Select->")

            dr.Close()

        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)

        Finally
            ds.Dispose()
            adapter.Dispose()


        End Try




    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        con.Open()

        If Not Page.IsPostBack Then

            txtdate.Text = Today.Date
            denom_out.Visible = False
            ViewState("RefUrl") = Request.UrlReferrer.ToString()
            bind_grid()
            bind_rp()
            If Not Session("tid") = Nothing Then


                get_voucher(Session("tid"))
                pnllst.Style.Add(HtmlTextWriterStyle.Display, "none")
                pnldenom.Style.Add(HtmlTextWriterStyle.Display, "block")

            Else

                pnllst.Style.Add(HtmlTextWriterStyle.Display, "block")
                pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

            End If




        End If

    End Sub


    Sub get_voucher(ByVal tid As String)

        issrvc = False
        If con.State = ConnectionState.Closed Then con.Open()

        Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        'If con.State = ConnectionState.Closed Then con.Open()
        'query = "select date,vno,in1k,in500,in100,in50,in20,in10,incoin,b1k,b500,b100,b50,b20,b10,bcoin,shared from denom  where CONVERT(VARCHAR(20), date, 112)='" + reformatted + "' and vno='" + Trim(lbltid.Text) + "'"

        If CDate(txtdate.Text).Equals(Today) Then
            query = "select date,acno,drd,crd,drc,crc,suplimentry,narration,due,denom,type from trans where id=@id and (type=@ty or type=@ty1 or type=@ty2)"
        Else
            query = "select date,acno,drd,crd,drc,crc,suplimentry,narration,due,type from actrans where id=@id and (type=@ty or type=@ty1 or type=@ty2) and CONVERT(VARCHAR(20), date, 112)='" + reformatted + "'"
        End If

        cmd.Parameters.Clear()
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@id", tid)
        cmd.Parameters.AddWithValue("@ty", "CASH")
        cmd.Parameters.AddWithValue("@ty1", "TRF")
        cmd.Parameters.AddWithValue("@ty2", "JOURNAL")
        Try
            Dim dr As SqlDataReader
            Dim den As Integer
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                dr.Read()
                pnlvdetails.Visible = True
                If Not Session("shared_total") = 0 Then shrd.Visible = False 'Made hidden As per request
                Session("debit_c") = dr(4)
                Session("debit_d") = dr(2)
                Session("credit_c") = dr(5)
                Session("credit_d") = dr(3)
                If CDate(txtdate.Text).Equals(Today) Then
                    den = dr(9)
                    Session("voucherType") = dr(10).ToString().Trim()
                Else
                    den = 1
                    Session("voucherType") = dr(9).ToString().Trim()
                End If
                ' lbltid.Text = txtacn.Text
                tdate.Text = dr(0)
                lblacn.Text = dr(1)
                lbltrans.Text = dr(7)



                If lbltrans.Text = "To Cash" Then
                    txtamt.Text = dr(2)
                    damt.Value = FormatCurrency(dr(2))
                    camt.Value = FormatCurrency(dr(4))
                    lbltrans.Text = "PAYMENT"
                    lblnature.Text = "To CASH"
                    trans.Value = "PAYMENT"
                    lblnature.ForeColor = Color.DarkRed
                    Session("mode") = "PAYMENT"
                    cword.Value = get_wrds(camt.Value)
                    dword.Value = get_wrds(txtamt.Text)
                ElseIf lbltrans.Text = "To Transfer" Then
                    txtamt.Text = dr(2)
                    damt.Value = FormatCurrency(dr(2))
                    camt.Value = FormatCurrency(dr(4))
                    lbltrans.Text = "PAYMENT"
                    lblnature.Text = "To Transfer"
                    trans.Value = "PAYMENT"
                    lblnature.ForeColor = Color.DarkRed
                    Session("mode") = "PAYMENT"
                    cword.Value = get_wrds(camt.Value)
                    dword.Value = get_wrds(txtamt.Text)
                ElseIf lbltrans.Text = "By Cash" Then
                    txtamt.Text = dr(3)
                    damt.Value = FormatCurrency(dr(3))
                    camt.Value = FormatCurrency(dr(5))
                    lbltrans.Text = "RECEIPT"
                    lblnature.Text = "By CASH"
                    trans.Value = "RECEIPT"
                    lblnature.ForeColor = Color.DarkGreen
                    Session("mode") = "RECEIPT"
                    cword.Value = get_wrds(camt.Value)
                    dword.Value = get_wrds(txtamt.Text)
                ElseIf lbltrans.Text = "By Transfer" Then
                    txtamt.Text = dr(3)
                    damt.Value = FormatCurrency(dr(3))
                    camt.Value = FormatCurrency(dr(5))
                    lbltrans.Text = "RECEIPT"
                    lblnature.Text = "By Transfer"
                    trans.Value = "RECEIPT"
                    lblnature.ForeColor = Color.DarkGreen
                    Session("mode") = "RECEIPT"
                    cword.Value = get_wrds(camt.Value)
                    dword.Value = get_wrds(txtamt.Text)
                Else

                    If dr(2) = 0 Then
                        txtamt.Text = dr(3)
                        lbltrans.Text = "RECEIPT"

                    Else
                        txtamt.Text = dr(2)
                        lbltrans.Text = "PAYMENT"


                    End If

                End If


                Session("prod") = dr(6)
                lblproduct.Text = dr(6)

                If Trim(dr(6)) = "SHARE" Then
                    lblacn.Text = ""
                    lblname.Text = get_name(dr(1))
                    pcid.Text = dr(1)

                Else
                    lblname.Text = get_cust(lblacn.Text)
                    pcid.Text = get_memberno(Trim(lblacn.Text))
                End If

                Session("nar") = IIf(IsDBNull(dr(7)), " ", dr(7))
                Session("due_info") = Session("nar") + " " + IIf(IsDBNull(dr(8)), " ", dr(8))

                Dim retrievedDueValue As String = ""
                If Not IsDBNull(dr(8)) Then
                    Dim fullDue As String = dr(8).ToString()
                    Dim match = System.Text.RegularExpressions.Regex.Match(fullDue, ".*-\s*(.*)")
                    If match.Success Then
                        retrievedDueValue = match.Groups(1).Value.Trim()
                    Else
                        retrievedDueValue = fullDue.Trim()
                    End If
                End If

                remit.Text = lblname.Text
                dr.Close()

                lbldue.Text = retrievedDueValue
                plbldue.Text = retrievedDueValue

                get_over(tdate.Text, Session("tid"))

                pvno.Text = tid
                pdate.Text = tdate.Text
                pacno.Text = lblacn.Text
                pcname.Text = lblname.Text
                'pcid.Text = lblm
                pamt.Text = FormatCurrency(txtamt.Text)
                pglh.Text = lblproduct.Text
                tglh.Value = lblproduct.Text





                pbranch.Text = get_home()
                pnar.Text = Session("due_info")
                premit.Text = remit.Text

                lblcpt.Text = lbltrans.Text
                lblcptr.Text = "OFFICE COPY"


                If Not Session("shared_total") = 0 Then
                    If lbltrans.Text = "RECEIPT" Then
                        lblsharedtotal.Text = FormatCurrency(Session("shared_total") + CDbl(txtamt.Text))
                    Else
                        lblsharedtotal.Text = FormatCurrency(Session("shared_total") - CDbl(txtamt.Text))
                    End If

                    If Not txtsrvchr.Text = "" Then
                        lblsharedtotal.Text = FormatCurrency(Session("shared_total")) + CDbl(txtamt.Text) - CDbl(txtsrvchr.Text)

                    End If

                End If

                If Not Session("voucherType") = Nothing And Not String.IsNullOrWhiteSpace(Session("voucherType")) And Session("voucherType") = "JOURNAL" Then
                    cashDenomInput.Visible = False
                    denom_in.Visible = True
                    pnlvdetails.Visible = True
                    btn_prnt.Visible = True
                    btn_denom_update.Visible = False
                    jpbranch.Text = pbranch.Text
                    jpvno.Text = pvno.Text
                    jpdate.Text = pdate.Text
                    fetchJournalInfo()
                Else
                    cashDenomInput.Visible = True
                    If den = 0 Then

                        denom_in.Visible = True
                        pnlvdetails.Visible = True

                        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt1k.ClientID)
                    Else
                        '  prepare_print()
                        ' lblcap.Text = "Hi " + Session("sesusr").ToString

                        'Me.ModalPopup1.Show()
                        ' Me.prnt_ModalPopupExtender.Show()
                        'prnt.Visible = True
                        pnlvdetails.Visible = True
                        ' denom_out.Visible = True
                        'TabContainer1.ActiveTab = vch

                        'clear_tab()
                        load_denom()

                    End If
                End If

            Else

            End If

            btn_denom_update.Enabled = True

        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        Finally
            cmd.Dispose()
            con.Close()

        End Try


        ' txtfocus(remit)
    End Sub

    Sub load_denom()

        Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()
        query = "select date,vno,in1k,in500,in200,in100,in50,in20,in10,incoin,b1k,b500,b200,b100,b50,b20,b10,bcoin,shared from denom  where CONVERT(VARCHAR(20), date, 112)='" + reformatted + "' and vno='" + Trim(Session("tid")) + "'"

        cmd.Connection = con
        cmd.CommandText = query
        Try
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                dr.Read()

                If lbltrans.Text = "RECEIPT" Then

                    txt1k.Text = dr(2)
                    txt500.Text = dr(3)
                    txt200.Text = dr(4)
                    txt100.Text = dr(5)
                    txt50.Text = dr(6)
                    txt20.Text = dr(7)
                    txt10.Text = dr(8)
                    txtcoin.Text = dr(9)
                    txtb1k.Text = dr(10)
                    txtb500.Text = dr(11)
                    txtb200.Text = dr(12)
                    txtb100.Text = dr(13)
                    txtb50.Text = dr(14)
                    txtb20.Text = dr(15)
                    txtb10.Text = dr(16)
                    txtbcoin.Text = dr(17)

                Else


                    txtb1k.Text = dr(2)
                    txtb500.Text = dr(3)
                    txtb200.Text = dr(4)
                    txtb100.Text = dr(5)
                    txtb50.Text = dr(6)
                    txtb20.Text = dr(7)
                    txtb10.Text = dr(8)
                    txtbcoin.Text = dr(9)

                    txt1k.Text = dr(10)
                    txt500.Text = dr(11)
                    txt200.Text = dr(12)
                    txt100.Text = dr(13)
                    txt50.Text = dr(14)
                    txt20.Text = dr(15)
                    txt10.Text = dr(16)
                    txtcoin.Text = dr(17)

                End If
                '  btn_rp.Visible = True
                get_bal_total()
                get_total()

                If Not CDbl(lbltotal.Text) = 0 Then
                    btn_join.Enabled = False
                    If CDbl(lblbtotal.Text) = 0 Then
                        'btn_denom_update.Enabled = False
                    Else
                        btn_bal_update.Enabled = False
                        btn_denom_update.Text = "BALANCE"
                    End If


                End If

            End If
        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try


    End Sub

    Sub fetchJournalInfo()

        Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()
        query = "select achead,debit,credit,narration from suplement where CONVERT(VARCHAR(20), date, 112)='" + reformatted + "' and transid='" + Trim(Session("tid")) + "' and type ='JOURNAL'"

        cmd.Connection = con
        cmd.CommandText = query
        Try
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            While dr.Read()
                Dim achead As String = dr("achead").ToString()
                Dim debit As Decimal = Convert.ToDecimal(dr("debit"))
                Dim credit As Decimal = Convert.ToDecimal(dr("credit"))
                Dim narration As String = dr("narration").ToString()
                If debit = 0 Then
                    lblglhcr.Text = achead
                    lblcr.Text = credit
                    lbltcr.Text = FormatCurrency(credit)
                    jpaiw.Text = get_wrds(lbltcr.Text)
                Else
                    lblglhdr.Text = achead
                    lbldr.Text = debit
                    lbltdr.Text = FormatCurrency(debit)
                End If
                jpnar.Text = narration
            End While
            dr.Close()
        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try


    End Sub

    Sub get_over(ByVal dt As String, ByVal acnx As String)
        Dim dat As DateTime = DateTime.ParseExact(dt, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        If con.State = ConnectionState.Closed Then con.Open()

        query = "select dr,cr,product from diff where CONVERT(VARCHAR(20), date, 112)=@dt and tid=@tid"

        cmd.CommandText = query
        cmd.Connection = con
        cmd.Parameters.AddWithValue("@dt", reformatted)
        cmd.Parameters.AddWithValue("@tid", acnx)

        Try
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()

                If lbltrans.Text = "PAYMENT" Then
                    If Trim(dr(2)) = "JL" Or Trim(dr(2)) = "ML" Or Trim(dr(2)) = "DL" Or Trim(dr(2)) = "DCL" Then
                        lblsc.Visible = True
                        txtsrvchr.Visible = True
                        txtsrvchr.Text = String.Format("{0:N}", dr(1))
                        srvchr.Value = String.Format("{0:N}", dr(1))
                        srcwrds.Value = get_wrds(dr(1))




                    Else
                        lblsc.Visible = True
                        txtsrvchr.Visible = True
                        txtsrvchr.Text = String.Format("{0:N}", 0)
                    End If

                End If

            Else
                txtsrvchr.Text = ""
                lblsc.Visible = False
                txtsrvchr.Visible = False
            End If
            dr.Close()

        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try

    End Sub

    Function get_memberno(ByVal acn As String)
        Dim cid As String = Nothing
        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Parameters.Clear()

        query = "SELECT member.memberno FROM dbo.master LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo WHERE master.acno = @acno"
        cmdi.Connection = con
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acno", acn)
        Try

            cid = cmdi.ExecuteScalar()



        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try

        Return cid

    End Function

    Function get_name(ByVal cid As String)
        Dim custname As String = ""
        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Parameters.Clear()

        query = "SELECT member.FirstName FROM dbo.member  WHERE MemberNo = @acno"
        cmdi.Connection = con
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acno", cid)
        Try

            custname = cmdi.ExecuteScalar()



        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try

        Return custname

    End Function


    Function get_cust(ByVal acn As String)
        Dim custname As String = ""
        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Parameters.Clear()

        query = "SELECT member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo WHERE master.acno = @acno"
        cmdi.Connection = con
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acno", acn)
        Try

            custname = cmdi.ExecuteScalar()



        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try

        Return custname

    End Function

    Function get_product(ByVal acn As String) As String
        Dim product As String = ""
        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Parameters.Clear()

        query = "SELECT Product FROM dbo.master WHERE acno = @acno"
        cmdi.Connection = con
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acno", acn)
        Try
            Dim obj = cmdi.ExecuteScalar()
            If obj IsNot Nothing Then
                product = obj.ToString()
            End If
        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try

        Return product

    End Function
    Private Sub txt1k_TextChanged(sender As Object, e As EventArgs) Handles txt1k.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt500)
        'txtfocus(txt500)
    End Sub

    Private Sub txt500_TextChanged(sender As Object, e As EventArgs) Handles txt500.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt200)
        'txtfocus(Me.txt100)

    End Sub

    Private Sub txt100_TextChanged(sender As Object, e As EventArgs) Handles txt100.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt50)

    End Sub

    Private Sub txt50_TextChanged(sender As Object, e As EventArgs) Handles txt50.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt20)


    End Sub

    Private Sub txt20_TextChanged(sender As Object, e As EventArgs) Handles txt20.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt10)


    End Sub

    Private Sub txt10_TextChanged(sender As Object, e As EventArgs) Handles txt10.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtcoin)

    End Sub

    Private Sub txtcoin_TextChanged(sender As Object, e As EventArgs) Handles txtcoin.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.btn_denom_update.ClientID)


    End Sub

    Private Sub btn_denom_clr_Click(sender As Object, e As EventArgs) Handles btn_denom_clr.Click
        txt1k.Text = ""
        txt500.Text = ""
        txt100.Text = ""
        txt50.Text = ""
        txt20.Text = ""
        txt10.Text = ""
        txtcoin.Text = ""
        lbl1k.Text = "0.00"
        lbl500.Text = "0.00"
        lbl100.Text = "0.00"
        lbl50.Text = "0.00"
        lbl20.Text = "0.00"
        lbl10.Text = "0.00"
        lblcoin.Text = "0.00"
        lbltotal.Text = "0.00"
        ' ModalPopup1.Show()
        txtfocus(txt1k)
    End Sub
    Sub get_total()

        Dim x As Double = 0
        If Not txt1k.Text = "" Then
            lbl1k.Text = FormatNumber(CDbl(txt1k.Text) * 2000)
            x = x + CDbl(lbl1k.Text)
        Else
            lbl1k.Text = 0
            x = x + CDbl(lbl1k.Text)

        End If
        p2kcount.Text = txt1k.Text
        p2kval.Text = FormatNumber(lbl1k.Text)

        If Not txt500.Text = "" Then
            lbl500.Text = FormatNumber(CDbl(txt500.Text) * 500)
            x = x + CDbl(lbl500.Text)
        Else
            lbl500.Text = 0 'FormatNumber(CDbl(txt500.Text) * 500)
            x = x + CDbl(lbl500.Text)
        End If

        p500count.Text = txt500.Text
        p500val.Text = FormatNumber(lbl500.Text)


        If Not txt200.Text = "" Then
            lbl200.Text = FormatNumber(CDbl(txt200.Text) * 200)
            x = x + CDbl(lbl200.Text)
        Else
            lbl200.Text = 0 'FormatNumber(CDbl(txt200.Text) * 200)
            x = x + CDbl(lbl200.Text)
        End If
        p200count.Text = txt200.Text
        p200val.Text = lbl200.Text



        If Not txt100.Text = "" Then
            lbl100.Text = FormatNumber(CDbl(txt100.Text) * 100)
            x = x + CDbl(lbl100.Text)
        Else
            lbl100.Text = 0 'FormatNumber(CDbl(txt100.Text) * 100)
            x = x + CDbl(lbl100.Text)
        End If

        p100count.Text = txt100.Text
        p100val.Text = lbl100.Text

        If Not txt50.Text = "" Then
            lbl50.Text = FormatNumber(CDbl(txt50.Text) * 50)
            x = x + CDbl(lbl50.Text)
        Else
            lbl50.Text = 0 'FormatNumber(CDbl(txt50.Text) * 50)
            x = x + CDbl(lbl50.Text)
        End If
        p50count.Text = txt50.Text
        p50val.Text = lbl50.Text


        If Not txt20.Text = "" Then
            lbl20.Text = FormatNumber(CDbl(txt20.Text) * 20)
            x = x + CDbl(lbl20.Text)
        Else
            lbl20.Text = 0 'FormatNumber(CDbl(txt20.Text) * 20)
            x = x + CDbl(lbl20.Text)
        End If

        p20count.Text = txt20.Text
        p20val.Text = lbl20.Text

        If Not txt10.Text = "" Then
            lbl10.Text = FormatNumber(CDbl(txt10.Text) * 10)
            x = x + CDbl(lbl10.Text)
        Else
            lbl10.Text = 0 'FormatNumber(CDbl(txt10.Text) * 10)
            x = x + CDbl(lbl10.Text)
        End If

        p10count.Text = txt10.Text
        p10val.Text = lbl10.Text


        If Not txtcoin.Text = "" Then
            lblcoin.Text = FormatNumber(CDbl(txtcoin.Text))
            x = x + CDbl(lblcoin.Text)
        Else
            lblcoin.Text = 0 'FormatNumber(CDbl(txtcoin.Text))
            x = x + CDbl(lblcoin.Text)
        End If

        pcoin.Text = lblcoin.Text
        lbltotal.Text = FormatNumber(x)
        ptotal.Text = lbltotal.Text
        paiw.Text = get_wrds(ptotal.Text)

        paiw.Text = get_wrds(ptotal.Text)
        dword.Value = paiw.Text


        'If CDbl(txtamt.Text) = x Then
        '    btn_denom_update.Enabled = True
        'Else
        '    btn_denom_update.Enabled = False
        'End If

    End Sub
    Sub get_bal_total()

        Dim x As Double = 0
        If Not txtb1k.Text = "" Then
            lblb1k.Text = FormatNumber(CDbl(txtb1k.Text) * 1000)
            x = x + CDbl(lblb1k.Text)
        Else
            lblb1k.Text = 0 'FormatNumber(CDbl(txtb1k.Text) * 1000)
            x = x + CDbl(lblb1k.Text)
        End If
        If Not txtb500.Text = "" Then
            lblb500.Text = FormatNumber(CDbl(txtb500.Text) * 500)
            x = x + CDbl(lblb500.Text)
        Else
            lblb500.Text = 0 'FormatNumber(CDbl(txtb500.Text) * 500)
            x = x + CDbl(lblb500.Text)
        End If

        If Not txtb200.Text = "" Then
            lblb200.Text = FormatNumber(CDbl(txtb200.Text) * 200)
            x = x + CDbl(lblb200.Text)
        Else
            lblb200.Text = 0 'FormatNumber(CDbl(txtb200.Text) * 200)
            x = x + CDbl(lblb200.Text)
        End If

        If Not txtb100.Text = "" Then
            lblb100.Text = FormatNumber(CDbl(txtb100.Text) * 100)
            x = x + CDbl(lblb100.Text)
        Else
            lblb100.Text = 0 'FormatNumber(CDbl(txtb100.Text) * 100)
            x = x + CDbl(lblb100.Text)

        End If
        If Not txtb50.Text = "" Then
            lblb50.Text = FormatNumber(CDbl(txtb50.Text) * 50)
            x = x + CDbl(lblb50.Text)
        Else
            lblb50.Text = 0 'FormatNumber(CDbl(txtb50.Text) * 50)
            x = x + CDbl(lblb50.Text)
        End If
        If Not txtb20.Text = "" Then
            lblb20.Text = FormatNumber(CDbl(txtb20.Text) * 20)
            x = x + CDbl(lblb20.Text)
        Else
            lblb20.Text = 0 'FormatNumber(CDbl(txtb20.Text) * 20)
            x = x + CDbl(lblb20.Text)
        End If
        If Not txtb10.Text = "" Then
            lblb10.Text = FormatNumber(CDbl(txtb10.Text) * 10)
            x = x + CDbl(lblb10.Text)
        Else
            lblb10.Text = 0 'FormatNumber(CDbl(txtb10.Text) * 10)
            x = x + CDbl(lblb10.Text)

        End If

        If Not txtbcoin.Text = "" Then
            lblbcoin.Text = FormatNumber(CDbl(txtbcoin.Text))
            x = x + CDbl(lblbcoin.Text)
        Else
            lblbcoin.Text = 0 'FormatNumber(CDbl(txtbcoin.Text))
            x = x + CDbl(lblbcoin.Text)

        End If

        lblbtotal.Text = FormatNumber(x)

    End Sub


    Private Sub btn_denom_update_Click(sender As Object, e As EventArgs) Handles btn_denom_update.Click

        amt_nt.Visible = False

        'response.write(txtamt.Text + txtnod.Text)

        If txtsrvchr.Text = "" Then
            Session("amt") = 0
        Else
            Session("amt") = CDbl(txtsrvchr.Text)
        End If
        Session("amt") = CDbl(txtamt.Text) - Session("amt")

        If lbltrans.Text = "PAYMENT" Then

            Session("amt") = -(Session("amt"))

        End If

        If Not Session("shared_total") = 0 Then Session("amt") = Session("amt") + Session("shared_total")

        If Session("amt") < 0 Then Session("amt") = -(Session("amt"))

        If Session("amt") = CDbl(lbltotal.Text) Then
            ' shared_total = 0
            set_changes()
            Session("shared_total") = 0
            disp.DataSource = Nothing
            disp.DataBind()
            dt = Nothing
            Session("jspec") = Nothing
            pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

            pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "block")
            If Not Session("voucherType") = Nothing And Not String.IsNullOrWhiteSpace(Session("voucherType")) And Session("voucherType") = "JOURNAL" Then
                printOCBtn.Visible = False
                printCCBtn.Visible = False
                printJrnlBtn.Visible = True
                journalVouchPrint.Visible = True
                denVouchPrint.Visible = False
            Else
                printOCBtn.Visible = True
                printCCBtn.Visible = True
                printJrnlBtn.Visible = False
                journalVouchPrint.Visible = False
                denVouchPrint.Visible = True
            End If

        Else
            'ModalPopup2.Show()
            If CDbl(lbltotal.Text) >= CDbl(Session("amt")) Then

                lbldenombal.Text = FormatCurrency(CDbl(lbltotal.Text) - Session("amt"))
                '  lbl_denom_msg_cap.Text = "Denomination"
                '  lblmsg_denom.Text = "Balance To be Given " + lbldenombal.Text
                '  MPbal.Show()
                denom_in.Visible = False
                denom_out.Visible = True

                'System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtb1k)
                txtfocus(txtb1k)
            Else
                amt_nt.Visible = True
                amt_nt.Text = "Enter Denomination for " + FormatCurrency(Session("amt"))
                txtfocus(txt1k)
            End If
        End If



    End Sub

    Private Sub txtb1k_TextChanged(sender As Object, e As EventArgs) Handles txtb1k.TextChanged
        get_bal_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtb500)

    End Sub

    Private Sub txtb500_TextChanged(sender As Object, e As EventArgs) Handles txtb500.TextChanged
        get_bal_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtb200)

    End Sub

    Private Sub txtb100_TextChanged(sender As Object, e As EventArgs) Handles txtb100.TextChanged
        get_bal_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtb50)

    End Sub

    Private Sub txtb50_TextChanged(sender As Object, e As EventArgs) Handles txtb50.TextChanged
        get_bal_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtb20)

    End Sub

    Private Sub txtbcoin_TextChanged(sender As Object, e As EventArgs) Handles txtbcoin.TextChanged
        get_bal_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.btn_bal_clr)

    End Sub

    Private Sub btn_bal_update_Click(sender As Object, e As EventArgs) Handles btn_bal_update.Click
        bal_nt.Visible = False

        ' Dim amt As Double
        ' amt = (IIf(txtsrvchr.Text = "", 0, CDbl(txtsrvchr.Text)))
        ' amt = CDbl(txtamt.Text) - amt
        If CDbl(lblbtotal.Text) = CDbl(lbldenombal.Text) Then
            set_changes()
            Session("shared_total") = 0
            disp.DataSource = Nothing
            disp.DataBind()
            dt = Nothing
            pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

            pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "block")

        Else
            bal_nt.Visible = True

            bal_nt.Text = "Enter Balance denomination for " + FormatCurrency(lbldenombal.Text)
            txtfocus(txtb1k)
        End If
    End Sub
    Sub set_changes()

        Try

            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con

            '   shared_total = 0

            If Not txtsrvchr.Text = "" Then
                Dim xamt As Double = CDbl(txtamt.Text) - CDbl(txtsrvchr.Text)
                If CDbl(lbltotal.Text) = xamt Then prnt4sc = True
            End If
            query = "INSERT INTO denom(date,vno,in1k,in500,in200,in100,in50,in20,in10,incoin,b1k,b500,b200,b100,b50,b20,b10,bcoin)"
            query &= "VALUES(@vdate,@vno,@1k,@500,@200,@100,@50,@20,@10,@coin,@b1k,@b500,@b200,@b100,@b50,@b20,@b10,@bcoin)"

            cmd.CommandText = query

            If lbltrans.Text = "RECEIPT" Then
                cmd.Parameters.AddWithValue("@vdate", Convert.ToDateTime(tdate.Text))
                cmd.Parameters.AddWithValue("@vno", CInt(Session("tid")))
                cmd.Parameters.AddWithValue("@1k", Trim(txt1k.Text))
                cmd.Parameters.AddWithValue("@500", Trim(txt500.Text))
                cmd.Parameters.AddWithValue("@200", Trim(txt200.Text))
                cmd.Parameters.AddWithValue("@100", Trim(txt100.Text))
                cmd.Parameters.AddWithValue("@50", Trim(txt50.Text))
                cmd.Parameters.AddWithValue("@20", Trim(txt20.Text))
                cmd.Parameters.AddWithValue("@10", Trim(txt10.Text))
                cmd.Parameters.AddWithValue("@coin", Trim(txtcoin.Text))
                cmd.Parameters.AddWithValue("@b1k", Trim(txtb1k.Text))
                cmd.Parameters.AddWithValue("@b500", Trim(txtb500.Text))
                cmd.Parameters.AddWithValue("@b200", Trim(txtb200.Text))
                cmd.Parameters.AddWithValue("@b100", Trim(txtb100.Text))
                cmd.Parameters.AddWithValue("@b50", Trim(txtb50.Text))
                cmd.Parameters.AddWithValue("@b20", Trim(txtb20.Text))
                cmd.Parameters.AddWithValue("@b10", Trim(txtb10.Text))
                cmd.Parameters.AddWithValue("@bcoin", Trim(txtbcoin.Text))
            Else
                cmd.Parameters.AddWithValue("@vdate", Convert.ToDateTime(tdate.Text))
                cmd.Parameters.AddWithValue("@vno", CInt(Session("tid")))
                cmd.Parameters.AddWithValue("@1k", Trim(txtb1k.Text))
                cmd.Parameters.AddWithValue("@500", Trim(txtb500.Text))
                cmd.Parameters.AddWithValue("@200", Trim(txtb200.Text))
                cmd.Parameters.AddWithValue("@100", Trim(txtb100.Text))
                cmd.Parameters.AddWithValue("@50", Trim(txtb50.Text))
                cmd.Parameters.AddWithValue("@20", Trim(txtb20.Text))
                cmd.Parameters.AddWithValue("@10", Trim(txtb10.Text))
                cmd.Parameters.AddWithValue("@coin", Trim(txtbcoin.Text))
                cmd.Parameters.AddWithValue("@b1k", Trim(txt1k.Text))
                cmd.Parameters.AddWithValue("@b500", Trim(txt500.Text))
                cmd.Parameters.AddWithValue("@b200", Trim(txt200.Text))
                cmd.Parameters.AddWithValue("@b100", Trim(txt100.Text))
                cmd.Parameters.AddWithValue("@b50", Trim(txt50.Text))
                cmd.Parameters.AddWithValue("@b20", Trim(txt20.Text))
                cmd.Parameters.AddWithValue("@b10", Trim(txt10.Text))
                cmd.Parameters.AddWithValue("@bcoin", Trim(txtcoin.Text))

            End If


            cmd.ExecuteNonQuery()


        Catch ex As Exception
            Response.Write(ex.Message)


        Finally
            con.Close()
            cmd.Dispose()

        End Try

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        query = "update trans set trans.denom=@denom where trans.id=@id"
        cmd.Parameters.AddWithValue("@denom", 1)
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.CommandText = query
        cmd.Connection = con
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        Finally
            con.Close()
            cmd.Dispose()

        End Try

        For i As Integer = 0 To disp.Rows.Count - 1


            Dim vno As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblitm"), Label)
            '  Dim dr As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblnet"), Label)
            ' Dim dat As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbldat"), Label)
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Parameters.Clear()
            query = "update trans set trans.denom=@denom where trans.id=@id"
            cmd.Parameters.AddWithValue("@denom", 1)
            cmd.Parameters.AddWithValue("@id", vno.Text)
            cmd.CommandText = query
            cmd.Connection = con
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
            Finally
                con.Close()
                cmd.Dispose()

            End Try


        Next


        If Not Session("shared_total") = 0 Then

            If con.State = ConnectionState.Closed Then con.Open()

            query = "delete from tmpbrkupprnt"
            cmd.Connection = con
            cmd.CommandText = query

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
            End Try
            For i As Integer = 0 To disp.Rows.Count - 1


                Dim vno As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblitm"), Label)
                Dim dr As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblnet"), Label)

                query = "insert into tmpbrkupprnt(vno,amount,total)"
                query &= "values(@vno,@amount,@total)"
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@vno", vno.Text)
                cmd.Parameters.AddWithValue("@amount", dr.Text)
                cmd.Parameters.AddWithValue("@total", CDbl(lblsharedtotal.Text))
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
                End Try

                query = "update denom SET shared=@svno  where denom.vno=@vno"
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@svno", vno.Text)
                cmd.Parameters.AddWithValue("@vno", Session("tid"))
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
                End Try


            Next

            query = "insert into tmpbrkupprnt(vno,amount,total)"
            query &= "values(@vno,@amount,@total)"
            cmd.Connection = con
            cmd.CommandText = query
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@vno", Session("tid"))
            If lbltrans.Text = "RECEIPT" Then
                cmd.Parameters.AddWithValue("@amount", CDbl(txtamt.Text))
            Else
                If Not txtsrvchr.Text = "" Then
                    txtamt.Text = CDbl(txtamt.Text) - CDbl(txtsrvchr.Text)
                    cmd.Parameters.AddWithValue("@amount", -CDbl(txtamt.Text))
                Else
                    cmd.Parameters.AddWithValue("@amount", -CDbl(txtamt.Text))
                End If
            End If
            cmd.Parameters.AddWithValue("@total", CDbl(lblsharedtotal.Text))
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
            End Try



            summary_on = True
            ' shared_total = 0
            lblsharedtotal.Text = ""
        End If



        ' prepare_print()

        '  


        clear_tab()
        bind_grid()

        'prnt.Visible = True
        ' TabContainer1.ActiveTab = vch

    End Sub

    Sub clear_tab()
        '  issrvc = False
        ' btn_rp.Visible = False
        Session("tid") = Nothing
        lblsc.Visible = False
        txtsrvchr.Visible = False
        Session("due_info") = Nothing
        txt1k.Text = ""
        txt500.Text = ""
        txt100.Text = ""
        txt200.Text = ""
        txt50.Text = ""
        txt20.Text = ""
        txt10.Text = ""
        txtcoin.Text = ""
        lbl1k.Text = "0.00"
        lbl500.Text = "0.00"
        lbl200.Text = "0.00"
        lbl100.Text = "0.00"
        lbl50.Text = "0.00"
        lbl20.Text = "0.00"
        lbl10.Text = "0.00"
        lblcoin.Text = "0.00"
        lbltotal.Text = "0.00"
        lblbtotal.Text = "0.00"
        txtb1k.Text = ""
        txtb500.Text = ""
        txtb100.Text = ""
        txtb200.Text = ""
        txtb50.Text = ""
        txtb20.Text = ""
        txtb10.Text = ""
        txtbcoin.Text = ""
        lblb1k.Text = "0.00"
        lblb500.Text = "0.00"
        lblb100.Text = "0.00"
        lblb200.Text = "0.00"
        lblb50.Text = "0.00"
        lblb20.Text = "0.00"
        lblb10.Text = "0.00"
        lblbcoin.Text = "0.00"


        'txtacn.Text = ""
        tdate.Text = ""
        lblacn.Text = ""
        lblproduct.Text = ""
        lblname.Text = ""
        lbltrans.Text = ""
        lblnature.Text = ""
        txtsrvchr.Text = ""

        'lbltid.Text = ""
        remit.Text = ""
        txtamt.Text = ""
        'Session("jspec") = Nothing
        'dt = Nothing
        If Session("shared_total") = 0 Then
            shrd.Visible = False
            disp.DataSource = Nothing
            disp.DataBind()
        End If
        ' denom.Visible = False
        ' denom_in.Visible = False
        denom_out.Visible = False
        '  pnlvdetails.Visible = False
        ' TabContainer1.Visible = False
        ' txtfocus(txtacn)
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
    Sub prepare_print()



        home = get_home()

        Session("cid") = get_memberno(lblacn.Text)

        If Session("cid") = Nothing Then Session("cid") = " "

        If con.State = ConnectionState.Closed Then con.Open()

        query = "delete from tmpprint"
        cmd.Connection = con
        cmd.CommandText = query

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try

        query = "delete  from prntdenom"
        cmd.Connection = con
        cmd.CommandText = query

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        Finally
            con.Close()
            cmd.Dispose()

        End Try

        ' Dim byword As String = txtamt.Text)
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.Parameters.Clear()

        query = "insert into tmpprint(tid,branch,vhead,date,product,acn,cid,fname,bywords,amt,remittedby,loanamt,schrg,memberno)"
        query &= "values(@vno,@branch,@vhead,@date,@product,@acn,@cid,@fname,@bywords,@amt,@remittedby,@loanamt,@schrg,@memberno)"
        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@vno", Session("tid"))
        cmd.Parameters.AddWithValue("@branch", home)

        If lbltrans.Text = "RECEIPT" Then
            cmd.Parameters.AddWithValue("@vhead", "RECEIPT OFFICE COPY")
            cmd.Parameters.AddWithValue("@bywords", AmountInWords(Session("credit_c")) + " Only")
            cmd.Parameters.AddWithValue("@amt", Session("credit_c"))

        Else
            If txtsrvchr.Text = "" Then
                cmd.Parameters.AddWithValue("@vhead", "PAYMENT OFFICE COPY")
                cmd.Parameters.AddWithValue("@bywords", AmountInWords(Session("debit_c")) + " Only")
                cmd.Parameters.AddWithValue("@amt", Session("debit_c"))
            Else
                Session("debit_c") = CDbl(txtamt.Text)
                cmd.Parameters.AddWithValue("@vhead", "PAYMENT OFFICE COPY")
                cmd.Parameters.AddWithValue("@bywords", AmountInWords(Session("debit_c")) + " Only")
                cmd.Parameters.AddWithValue("@amt", Session("debit_c"))
            End If
        End If

        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@product", Trim(lblproduct.Text))
        cmd.Parameters.AddWithValue("@acn", lblacn.Text)
        cmd.Parameters.AddWithValue("@cid", Session("due_info"))
        cmd.Parameters.AddWithValue("@fname", Trim(lblname.Text))
        cmd.Parameters.AddWithValue("@remittedby", Trim(remit.Text))
        cmd.Parameters.AddWithValue("@loanamt", txtamt.Text)

        If txtsrvchr.Text = "" Then
            cmd.Parameters.AddWithValue("@schrg", 0)
        Else
            cmd.Parameters.AddWithValue("@schrg", CDbl(txtsrvchr.Text))
        End If
        cmd.Parameters.AddWithValue("@memberno", Session("cid"))


        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try

        cmd.Parameters.Clear()


        query = "insert into tmpprint(tid,branch,vhead,date,product,acn,cid,fname,bywords,amt,remittedby,loanamt,schrg,memberno)"
        query &= "values(@vno,@branch,@vhead,@date,@product,@acn,@cid,@fname,@bywords,@amt,@remittedby,@loanamt,@schrg,@memberno)"
        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@vno", Session("tid"))
        If lbltrans.Text = "RECEIPT" Then
            cmd.Parameters.AddWithValue("@vhead", "RECEIPT CUSTOMER COPY")
            cmd.Parameters.AddWithValue("@bywords", AmountInWords(Session("credit_d")) + " Only")
            cmd.Parameters.AddWithValue("@amt", Session("credit_d"))
        Else

            If Trim(lblproduct.Text) = "JEWEL LOAN" Then

                If txtsrvchr.Text = "" Then
                    cmd.Parameters.AddWithValue("@vhead", "PAYMENT CUSTOMER COPY")
                    cmd.Parameters.AddWithValue("@bywords", AmountInWords(Session("debit_d")) + " Only")
                    cmd.Parameters.AddWithValue("@amt", Session("debit_d"))
                Else
                    If CDbl(lbltotal.Text) = CDbl(txtamt.Text) Then
                        cmd.Parameters.AddWithValue("@bywords", AmountInWords(CDbl(lbltotal.Text)) + " Only")
                        cmd.Parameters.AddWithValue("@amt", CDbl(lbltotal.Text))
                        cmd.Parameters.AddWithValue("@vhead", "PAYMENT CUSTOMER COPY")
                    Else
                        If Session("shared_total") = 0 Then
                            cmd.Parameters.AddWithValue("@bywords", AmountInWords(CDbl(txtamt.Text)) + " Only")
                            cmd.Parameters.AddWithValue("@amt", CDbl(txtamt.Text))
                            cmd.Parameters.AddWithValue("@vhead", "PAYMENT CUSTOMER COPY")
                        Else
                            Dim lblt As Double = CDbl(txtamt.Text)
                            cmd.Parameters.AddWithValue("@bywords", AmountInWords(lblt) + " Only")
                            cmd.Parameters.AddWithValue("@amt", CDbl(lblt))
                            cmd.Parameters.AddWithValue("@vhead", "PAYMENT CUSTOMER COPY")
                            prnt4sc = True
                        End If
                    End If
                End If
            Else
                cmd.Parameters.AddWithValue("@vhead", "PAYMENT CUSTOMER COPY")
                cmd.Parameters.AddWithValue("@bywords", AmountInWords(Session("debit_d")) + " Only")
                cmd.Parameters.AddWithValue("@amt", Session("debit_d"))
            End If



        End If
        cmd.Parameters.AddWithValue("@branch", home)
        ' cmd.Parameters.AddWithValue("@vhead", "RECEIPT CUSTOMER COPY")
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@product", lblproduct.Text)
        cmd.Parameters.AddWithValue("@acn", lblacn.Text)
        cmd.Parameters.AddWithValue("@cid", Session("due_info"))
        cmd.Parameters.AddWithValue("@fname", lblname.Text)
        'cmd.Parameters.AddWithValue("@bywords", byword)
        'cmd.Parameters.AddWithValue("@amt", txtamt.Text)
        cmd.Parameters.AddWithValue("@remittedby", remit.Text)

        cmd.Parameters.AddWithValue("@loanamt", txtamt.Text)


        If txtsrvchr.Text = "" Then
            cmd.Parameters.AddWithValue("@schrg", 0)
        Else
            cmd.Parameters.AddWithValue("@schrg", CDbl(txtsrvchr.Text))
        End If
        cmd.Parameters.AddWithValue("@memberno", Session("cid"))

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try
        cmd.Parameters.Clear()

        If Not txtsrvchr.Text = "" Then
            issrvc = True
            query = "insert into tmpprint(tid,branch,vhead,date,product,acn,cid,fname,bywords,amt,remittedby,memberno)"
            query &= "values(@vno,@branch,@vhead,@date,@product,@acn,@cid,@fname,@bywords,@amt,@remittedby,@memberno)"
            cmd.CommandText = query

            cmd.Parameters.AddWithValue("@vno", Session("tid"))
            cmd.Parameters.AddWithValue("@vhead", "RECEIPT CUSTOMER COPY")
            cmd.Parameters.AddWithValue("@bywords", AmountInWords(txtsrvchr.Text) + " Only")
            cmd.Parameters.AddWithValue("@amt", CDbl(txtsrvchr.Text))
            cmd.Parameters.AddWithValue("@branch", home)
            ' cmd.Parameters.AddWithValue("@vhead", "RECEIPT CUSTOMER COPY")
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@product", lblproduct.Text)
            cmd.Parameters.AddWithValue("@acn", lblacn.Text)
            cmd.Parameters.AddWithValue("@cid", " ")
            cmd.Parameters.AddWithValue("@fname", lblname.Text)
            'cmd.Parameters.AddWithValue("@bywords", byword)
            'cmd.Parameters.AddWithValue("@amt", txtamt.Text)
            cmd.Parameters.AddWithValue("@remittedby", remit.Text)
            cmd.Parameters.AddWithValue("@memberno", Session("cid"))
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
            End Try
            cmd.Parameters.Clear()

        End If



        query = "insert into prntdenom(date,vno,in1k,val1k,in500,val500,in200,val200,in100,val100,in50,val50,in20,val20,in10,val10,incoin,total)"
        query &= "values(@date,@vno,@in1k,@val1k,@in500,@val500,@in200,@val200,@in100,@val100,@in50,@val50,@in20,@val20,@in10,@val10,@incoin,@total)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@vno", Session("tid"))
        cmd.Parameters.AddWithValue("@in1k", CDbl(IIf(txt1k.Text = "", 0, txt1k.Text)))
        cmd.Parameters.AddWithValue("@val1k", CDbl(lbl1k.Text))
        cmd.Parameters.AddWithValue("@in500", CDbl(IIf(txt500.Text = "", 0, txt500.Text)))
        cmd.Parameters.AddWithValue("@val500", CDbl(lbl500.Text))
        cmd.Parameters.AddWithValue("@in200", CDbl(IIf(txt200.Text = "", 0, txt200.Text)))
        cmd.Parameters.AddWithValue("@val200", CDbl(lbl200.Text))
        cmd.Parameters.AddWithValue("@in100", CDbl(IIf(txt100.Text = "", 0, txt100.Text)))
        cmd.Parameters.AddWithValue("@val100", CDbl(lbl100.Text))
        cmd.Parameters.AddWithValue("@in50", CDbl(IIf(txt50.Text = "", 0, txt50.Text)))
        cmd.Parameters.AddWithValue("@val50", CDbl(lbl50.Text))
        cmd.Parameters.AddWithValue("@in20", CDbl(IIf(txt20.Text = "", 0, txt20.Text)))
        cmd.Parameters.AddWithValue("@val20", CDbl(lbl20.Text))
        cmd.Parameters.AddWithValue("@in10", CDbl(IIf(txt10.Text = "", 0, txt10.Text)))
        cmd.Parameters.AddWithValue("@val10", CDbl(lbl10.Text))
        cmd.Parameters.AddWithValue("@incoin", CDbl(IIf(txtcoin.Text = "", 0, txtcoin.Text)))
        cmd.Parameters.AddWithValue("@total", CDbl(lbltotal.Text))

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)
        End Try




    End Sub


    'Private Sub btn_cc_Click(sender As Object, e As EventArgs) Handles btn_cc.Click

    '    Dim path As String = "c:\appfolder\cc.pdf"



    '    Dim reportPath As String = Nothing
    '    Try
    '        Dim reportDocument As New ReportDocument



    '        If Session("shared_total") = 0 Then



    '            If prnt4sc = True Then



    '                If Trim(Session("prod")) = "JEWEL LOAN" Then

    '                    reportPath = Server.MapPath("~/RPT/rcptjL.rpt")

    '                Else
    '                    reportPath = Server.MapPath("~/RPT/rcpt.rpt")
    '                End If




    '                'reportsource.Report.FileName

    '                If reportDocument.IsLoaded Then
    '                    reportDocument.Close()
    '                    reportDocument.Refresh()
    '                End If

    '                reportDocument.Load(reportPath)
    '                crv.PDFOneClickPrinting = False
    '                crv.HasToggleGroupTreeButton = False
    '                crv.BestFitPage = True
    '                crv.HasPrintButton = True
    '                crv.HasRefreshButton = True
    '                crv.HasCrystalLogo = False
    '                crv.HasToggleGroupTreeButton = False
    '                crv.ReportSource = reportDocument


    '                crv.RefreshReport()
    '                '       reportDocument.PrintOptions.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0)
    '                If Trim(Session("prod")) = "JEWEL LOAN" Then
    '                    ''  reportDocument.PrintToPrinter(1, False, 3, 3)
    '                    reportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
    '                    Dim Options As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '                    reportDocument.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5
    '                    reportDocument.PrintOptions.ApplyPageMargins(New CrystalDecisions.Shared.PageMargins(0, 0, 0, 0))
    '                    Options.DiskFileName = "c:\appfolder\cc.pdf"
    '                    reportDocument.ExportOptions.DestinationOptions = Options
    '                    reportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '                    Dim Options1 As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '                    Options1.UsePageRange = True
    '                    If Options1.UsePageRange Then
    '                        Options1.FirstPageNumber = 3
    '                        Options1.LastPageNumber = 3
    '                    End If
    '                    reportDocument.ExportOptions.FormatOptions = Options1
    '                    reportDocument.Export()
    '                    reportDocument.Dispose()
    '                    Dim doc = PdfDocument.Load(path)
    '                    Dim prntdoc = doc.CreatePrintDocument()
    '                    prntdoc.DocumentName = "cc.pdf"
    '                    prntdoc.PrinterSettings.PrintFileName = "cc.pdf"
    '                    prntdoc.PrintController = New StandardPrintController()
    '                    prntdoc.Print()
    '                    prntdoc.Dispose()
    '                    doc.Dispose()


    '                Else
    '                    ''  reportDocument.PrintToPrinter(1, False, 2, 2)
    '                    reportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
    '                    Dim Options As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '                    reportDocument.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5
    '                    reportDocument.PrintOptions.ApplyPageMargins(New CrystalDecisions.Shared.PageMargins(0, 0, 0, 0))

    '                    Options.DiskFileName = "c:\appfolder\cc.pdf"
    '                    reportDocument.ExportOptions.DestinationOptions = Options
    '                    reportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '                    Dim Options1 As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '                    Options1.UsePageRange = True

    '                    If Options1.UsePageRange Then
    '                        Options1.FirstPageNumber = 2
    '                        Options1.LastPageNumber = 2
    '                    End If
    '                    reportDocument.ExportOptions.FormatOptions = Options1
    '                    reportDocument.Export()
    '                    reportDocument.Dispose()

    '                    Dim doc = PdfDocument.Load(path)
    '                    Dim prntdoc = doc.CreatePrintDocument()
    '                    prntdoc.DocumentName = "cc.pdf"
    '                    prntdoc.PrinterSettings.PrintFileName = "cc.pdf"
    '                    prntdoc.PrintController = New StandardPrintController()
    '                    prntdoc.Print()
    '                    prntdoc.Dispose()
    '                    doc.Dispose()



    '                End If
    '                prnt4sc = False
    '            Else
    '                If Session("mode") = "RECEIPT" Then
    '                    reportPath = Server.MapPath("~/RPT/rcpt.rpt") 'reportsource.Report.FileName
    '                    reportDocument.Load(reportPath)
    '                    crv.PDFOneClickPrinting = False
    '                    crv.HasToggleGroupTreeButton = False
    '                    crv.BestFitPage = True
    '                    crv.HasPrintButton = True
    '                    crv.HasRefreshButton = True
    '                    crv.HasCrystalLogo = False
    '                    crv.HasToggleGroupTreeButton = False
    '                    crv.ReportSource = reportDocument
    '                    crv.RefreshReport()
    '                    reportDocument.PrintOptions.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0) '"M200 Series(Network)"
    '                    'System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0)
    '                    '   reportDocument.PrintToPrinter(1, False, 2, 2)
    '                    reportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
    '                    Dim Options As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '                    reportDocument.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5
    '                    reportDocument.PrintOptions.ApplyPageMargins(New CrystalDecisions.Shared.PageMargins(0, 0, 0, 0))

    '                    Options.DiskFileName = "c:\appfolder\cc.pdf"
    '                    reportDocument.ExportOptions.DestinationOptions = Options
    '                    reportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '                    Dim Options1 As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '                    Options1.UsePageRange = True
    '                    If Options1.UsePageRange Then
    '                        Options1.FirstPageNumber = 2
    '                        Options1.LastPageNumber = 2
    '                    End If
    '                    reportDocument.ExportOptions.FormatOptions = Options1
    '                    reportDocument.Export()
    '                    reportDocument.Dispose()

    '                    Dim doc = PdfDocument.Load(path)
    '                    Dim prntdoc = doc.CreatePrintDocument()
    '                    prntdoc.DocumentName = "cc.pdf"
    '                    prntdoc.PrinterSettings.PrintFileName = "cc.pdf"
    '                    prntdoc.PrintController = New StandardPrintController()
    '                    prntdoc.Print()
    '                    prntdoc.Dispose()
    '                    doc.Dispose()


    '                    'Dim info = New ProcessStartInfo("c:\appfolder\cc.pdf")
    '                    'info.Verb = "Print"
    '                    'info.UseShellExecute = True
    '                    'info.CreateNoWindow = True

    '                    'info.WindowStyle = ProcessWindowStyle.Hidden
    '                    'Process.Start(info)

    '                End If

    '                If issrvc Then
    '                    reportPath = Server.MapPath("~/RPT/rcptjl.rpt") 'reportsource.Report.FileName
    '                    reportDocument.Load(reportPath)
    '                    crv.PDFOneClickPrinting = False
    '                    crv.HasToggleGroupTreeButton = False
    '                    crv.BestFitPage = True
    '                    crv.HasPrintButton = True
    '                    crv.HasRefreshButton = True
    '                    crv.HasCrystalLogo = False
    '                    crv.HasToggleGroupTreeButton = False
    '                    crv.ReportSource = reportDocument
    '                    crv.RefreshReport()
    '                    reportDocument.PrintOptions.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0) '"M200 Series(Network)"
    '                    'System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0)
    '                    ' reportDocument.PrintToPrinter(1, False, 3, 3)
    '                    '--------------------------
    '                    '==== destination
    '                    reportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
    '                    Dim Options As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '                    reportDocument.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5
    '                    reportDocument.PrintOptions.ApplyPageMargins(New CrystalDecisions.Shared.PageMargins(0, 0, 0, 0))

    '                    Options.DiskFileName = "c:\appfolder\cc.pdf"
    '                    reportDocument.ExportOptions.DestinationOptions = Options
    '                    reportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '                    Dim Options1 As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '                    Options1.UsePageRange = True
    '                    If Options1.UsePageRange Then
    '                        Options1.FirstPageNumber = 3
    '                        Options1.LastPageNumber = 3
    '                    End If
    '                    reportDocument.ExportOptions.FormatOptions = Options1
    '                    reportDocument.Export()
    '                    reportDocument.Dispose()

    '                    Dim doc = PdfDocument.Load(path)
    '                    Dim prntdoc = doc.CreatePrintDocument()
    '                    prntdoc.DocumentName = "cc.pdf"
    '                    prntdoc.PrinterSettings.PrintFileName = "cc.pdf"
    '                    prntdoc.PrintController = New StandardPrintController()
    '                    prntdoc.Print()
    '                    prntdoc.Dispose()
    '                    doc.Dispose()

    '                    '-----------------------------
    '                    issrvc = False
    '                Else

    '                    'reportDocument.PrintToPrinter(1, False, 2, 2)
    '                End If
    '            End If
    '        Else


    '            If prnt4sc = True Then
    '                reportPath = Server.MapPath("RPT/rcpt4scwDenom.rpt")
    '                prnt4sc = False
    '            Else
    '                reportPath = Server.MapPath("RPT/rcptwDenom.rpt") 'reportsource.Report.FileName
    '            End If
    '            reportDocument.Load(reportPath)
    '            crv.PDFOneClickPrinting = False
    '            crv.HasToggleGroupTreeButton = False
    '            crv.BestFitPage = True
    '            crv.HasPrintButton = True
    '            crv.HasRefreshButton = True
    '            crv.HasCrystalLogo = False
    '            crv.HasToggleGroupTreeButton = False
    '            crv.ReportSource = reportDocument
    '            crv.RefreshReport()
    '            ' If issrvc Then
    '            reportDocument.PrintOptions.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0) '"M200 Series(Network)"
    '            'System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0)
    '            '   reportDocument.PrintToPrinter(1, False, 2, 2)
    '            reportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
    '            Dim Options As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '            reportDocument.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5
    '            reportDocument.PrintOptions.ApplyPageMargins(New CrystalDecisions.Shared.PageMargins(0, 0, 0, 0))

    '            Options.DiskFileName = "c:\appfolder\cc.pdf"
    '            reportDocument.ExportOptions.DestinationOptions = Options
    '            reportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '            Dim Options1 As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '            Options1.UsePageRange = True
    '            If Options1.UsePageRange Then
    '                Options1.FirstPageNumber = 2
    '                Options1.LastPageNumber = 2
    '            End If
    '            reportDocument.ExportOptions.FormatOptions = Options1
    '            reportDocument.Export()
    '            reportDocument.Dispose()

    '            Dim doc = PdfDocument.Load(path)
    '            Dim prntdoc = doc.CreatePrintDocument()
    '            prntdoc.DocumentName = "cc.pdf"
    '            prntdoc.PrinterSettings.PrintFileName = "cc.pdf"
    '            prntdoc.PrintController = New StandardPrintController()
    '            prntdoc.Print()
    '            prntdoc.Dispose()
    '            doc.Dispose()



    '        End If

    '        If summary_on = True Then
    '            reportPath = Server.MapPath("RPT/rcptbrkup.rpt") 'reportsource.Report.FileName
    '            reportDocument.Load(reportPath)
    '            crv.PDFOneClickPrinting = False
    '            crv.HasToggleGroupTreeButton = False
    '            crv.BestFitPage = True
    '            crv.HasPrintButton = True
    '            crv.HasRefreshButton = True
    '            crv.HasCrystalLogo = False
    '            crv.HasToggleGroupTreeButton = False
    '            crv.ReportSource = reportDocument
    '            crv.RefreshReport()
    '            '    reportDocument.PrintOptions.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0) '"M200 Series(Network)"
    '            '   reportDocument.PrintToPrinter(1, False, 1, 1)
    '            reportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
    '            Dim Options As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '            reportDocument.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5
    '            reportDocument.PrintOptions.ApplyPageMargins(New CrystalDecisions.Shared.PageMargins(0, 0, 0, 0))

    '            Options.DiskFileName = "c:\appfolder\cc.pdf"
    '            reportDocument.ExportOptions.DestinationOptions = Options
    '            reportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '            Dim Options1 As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '            Options1.UsePageRange = True
    '            If Options1.UsePageRange Then
    '                Options1.FirstPageNumber = 2
    '                Options1.LastPageNumber = 2
    '            End If
    '            reportDocument.ExportOptions.FormatOptions = Options1
    '            reportDocument.Export()
    '            reportDocument.Dispose()

    '            Dim doc = PdfDocument.Load(path)
    '            Dim prntdoc = doc.CreatePrintDocument()
    '            prntdoc.DocumentName = "cc.pdf"
    '            prntdoc.PrinterSettings.PrintFileName = "cc.pdf"
    '            prntdoc.PrintController = New StandardPrintController()
    '            prntdoc.Print()
    '            prntdoc.Dispose()
    '            doc.Dispose()

    '            summary_on = False
    '            Session("shared_total") = 0
    '        End If



    '    Catch ex As Exception
    '        Response.Write(ex.ToString)

    '    End Try

    '    Me.ModalPopup1.Show()

    'End Sub

    'Private Sub btn_oc_Click(sender As Object, e As EventArgs) Handles btn_oc.Click

    '    'Dim doc As New PdfDocument()
    '    Dim Width As Integer = Integer.Parse((Math.Round((14.8 * 0.393701) * 100, 0, MidpointRounding.AwayFromZero)).ToString())
    '    Dim Height As Integer = Integer.Parse((Math.Round((21 * 0.393701) * 100, 0, MidpointRounding.AwayFromZero)).ToString())


    '    Dim Path As String = "c:\appfolder\cc.pdf"
    '    Dim Path1 As String = "c:\appfolder\cc.pdf"
    '    Dim reportpath As String
    '    Try
    '        Dim reportDocument As New ReportDocument

    '        '    Dim reportPath As String = Server.MapPath("RPT/rcptwDenom.rpt") 'reportsource.Report.FileName
    '        If Session("mode") = "PAYMENT" Then
    '            If prnt4sc = False Then
    '                reportpath = Server.MapPath("~/RPT/rcpt.rpt") 'reportsource.Report.FileName
    '            Else
    '                If Trim(Session("prod")) = "JEWEL LOAN" Then
    '                    reportpath = Server.MapPath("~/RPT/rcptjl.rpt")
    '                Else
    '                    reportpath = Server.MapPath("~/RPT/rcpt.rpt")
    '                End If 'reportsource.Report.FileName
    '            End If
    '        Else
    '            reportpath = Server.MapPath("~/RPT/rcptwDenom.rpt") 'reportsource.Report.FileName
    '        End If
    '        reportDocument.Load(reportpath)
    '        crv.PDFOneClickPrinting = False
    '        crv.HasToggleGroupTreeButton = False
    '        crv.BestFitPage = True
    '        crv.HasPrintButton = True
    '        crv.HasRefreshButton = True
    '        crv.HasCrystalLogo = False
    '        crv.HasToggleGroupTreeButton = False
    '        crv.ReportSource = reportDocument
    '        reportDocument.PrintOptions.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0) ' "M200 Series(Network)"
    '        'System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0)
    '        'reportDocument.PrintToPrinter(1, False, 1, 1)
    '        reportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
    '        Dim Options As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '        reportDocument.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5
    '        reportDocument.PrintOptions.ApplyPageMargins(New CrystalDecisions.Shared.PageMargins(0, 0, 0, 0))
    '        Options.DiskFileName = "c:\appfolder\cc.pdf"
    '        reportDocument.ExportOptions.DestinationOptions = Options
    '        reportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '        Dim Options1 As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '        Options1.UsePageRange = True
    '        If Options1.UsePageRange Then
    '            Options1.FirstPageNumber = 1
    '            Options1.LastPageNumber = 1
    '        End If
    '        reportDocument.ExportOptions.FormatOptions = Options1
    '        reportDocument.Export()
    '        reportDocument.Dispose()

    '        ' ''doc.LoadFromFile("c:\appfolder\Oc.pdf")
    '        ' ''doc.Print()


    '        Dim doc = PdfDocument.Load(Path)
    '        Dim prntdoc = doc.CreatePrintDocument()
    '        prntdoc.DocumentName = "cc.pdf"
    '        prntdoc.PrinterSettings.PrintFileName = "cc.pdf"

    '        prntdoc.DefaultPageSettings.PaperSize = New Printing.PaperSize("Fiscus", Width, Height)
    '        prntdoc.PrintController = New StandardPrintController()
    '        prntdoc.Print()
    '        prntdoc.Dispose()
    '        doc.Dispose()
    '        crv.RefreshReport()

    '        If summary_on = True Then
    '            reportpath = Server.MapPath("RPT/rcptbrkup.rpt") 'reportsource.Report.FileName
    '            reportDocument.Load(reportpath)
    '            crv.PDFOneClickPrinting = False
    '            crv.HasToggleGroupTreeButton = False
    '            crv.BestFitPage = True
    '            crv.HasPrintButton = True
    '            crv.HasRefreshButton = True
    '            crv.HasCrystalLogo = False
    '            crv.HasToggleGroupTreeButton = False
    '            crv.ReportSource = reportDocument
    '            crv.RefreshReport()
    '            reportDocument.PrintOptions.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Item(0) ' "M200 Series(Network)"
    '            '    reportDocument.PrintToPrinter(1, False, 1, 1)
    '            reportDocument.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile
    '            Dim OptionsOC As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '            reportDocument.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5
    '            reportDocument.PrintOptions.ApplyPageMargins(New CrystalDecisions.Shared.PageMargins(0, 0, 0, 0))
    '            Options.DiskFileName = "c:\appfolder\cc.pdf"
    '            reportDocument.ExportOptions.DestinationOptions = OptionsOC
    '            reportDocument.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
    '            Dim Optionsoc1 As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
    '            Optionsoc1.UsePageRange = True
    '            If Optionsoc1.UsePageRange Then
    '                Optionsoc1.FirstPageNumber = 1
    '                Optionsoc1.LastPageNumber = 1
    '            End If
    '            reportDocument.ExportOptions.FormatOptions = Optionsoc1
    '            reportDocument.Export()
    '            reportDocument.Dispose()

    '            ' ''doc.LoadFromFile("c:\appfolder\Oc1.pdf")
    '            ' ''doc.Print()
    '            Dim doc1 = PdfDocument.Load(Path)
    '            Dim prntdoc1 = doc1.CreatePrintDocument()
    '            prntdoc1.DocumentName = "cc.pdf"
    '            prntdoc1.PrinterSettings.PrintFileName = "cc.pdf"
    '            prntdoc.DefaultPageSettings.PaperSize = New Printing.PaperSize("Fiscus", 148, 210)
    '            prntdoc1.PrintController = New StandardPrintController()
    '            prntdoc1.Print()
    '            prntdoc1.Dispose()
    '            doc1.Dispose()

    '            summary_on = False
    '            Session("shared_total") = 0
    '        End If

    '    Catch ex As Exception
    '        Response.Write(ex.ToString)

    '    End Try
    '    Me.ModalPopup1.Show()

    'End Sub

    'Private Sub btnpay_Click(sender As Object, e As EventArgs) Handles btnpay.Click
    '    txtfocus(txtacn)
    'End Sub

    Private Sub txtb20_TextChanged(sender As Object, e As EventArgs) Handles txtb20.TextChanged
        get_bal_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtb10)

    End Sub

    Private Sub txtb10_TextChanged(sender As Object, e As EventArgs) Handles txtb10.TextChanged
        get_bal_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtbcoin)

    End Sub

    Private Sub btn_join_Click(sender As Object, e As EventArgs) Handles btn_join.Click

        If lbltrans.Text = "RECEIPT" Then
            Session("shared_total") = Session("shared_total") + CDbl(txtamt.Text)
        Else

            If txtsrvchr.Text = "" Then
                Session("shared_total") = Session("shared_total") - CDbl(txtamt.Text)
            Else
                Session("shared_total") = Session("shared_total") - CDbl(txtamt.Text) + CDbl(txtsrvchr.Text)
            End If

        End If


        Dim dt_j As New DataTable

        If dt_j.Columns.Count = 0 Then
            dt_j.Columns.Add("vno", GetType(String))
            dt_j.Columns.Add("amt", GetType(Decimal))
        End If

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("vno", GetType(String))
            dt.Columns.Add("amt", GetType(Decimal))
        End If


        If Session("jspec") Is Nothing = False Then
            dt_j = CType(Session("jspec"), DataTable)

            Dim count As Integer = dt_j.Rows.Count

            For Each row1 As DataRow In dt_j.Rows

                dt.ImportRow(row1)
            Next

        End If
        newrow = dt_j.NewRow

        newrow(0) = Session("tid")
        If lbltrans.Text = "RECEIPT" Then
            newrow(1) = String.Format("{0:N}", txtamt.Text)
        Else
            If txtsrvchr.Text = "" Then
                newrow(1) = String.Format("{0:N}", -CDbl(txtamt.Text))
            Else
                newrow(1) = String.Format("{0:N}", -(CDbl(txtamt.Text) - CDbl(txtsrvchr.Text)))
            End If

        End If

        lblsharedtotal.Text = FormatCurrency(Session("shared_total"))

        dt_j.Rows.Add(newrow)
        dt.ImportRow(newrow)

        'If Not txtsrvchr.Text = "" Then
        '    newrow = dt_j.NewRow

        '    newrow(0) = txtacn.Text
        '    newrow(1) = 0 - CDbl(txtsrvchr.Text)
        '    shared_total = shared_total - CDbl(txtsrvchr.Text)
        '    lblsharedtotal.Text = FormatCurrency(shared_total)
        '    dt_j.Rows.Add(newrow)

        '    dt.ImportRow(newrow)

        'End If

        disp.DataSource = dt
        disp.DataBind()
        Session("jspec") = dt_j




        ' prepare_print()

        pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

        pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "block")


        ' txtlastprint.Text = "Voucher No. <STRONG>" + txtacn.Text + "</strong>    Ready To Print."
        '  txtlastprint.Text = "Voucher No. <STRONG>" + txtacn.Text + "</strong>    Ready To Print."
        '  Me.ModalPopup1.Show()
        ' clear_tab()
        bind_grid()
        '  prnt.Visible = True
        '  TabContainer1.ActiveTab = vch

    End Sub

    Public Sub New()

    End Sub





    Private Sub txt200_TextChanged(sender As Object, e As EventArgs) Handles txt200.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt100)
    End Sub

    Private Sub txtb200_TextChanged(sender As Object, e As EventArgs) Handles txtb200.TextChanged
        get_bal_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtb100)
    End Sub



    Private Sub denomination_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btntog_Click(sender As Object, e As EventArgs) Handles btntog.Click
        Dim refurl As Object = ViewState("RefUrl")

        tglh.Value = ""
        srcwrds.Value = ""
        srvchr.Value = ""


        If Not session_user_role = "Cashier" Then
            bind_rp()

            denom_out.Visible = False


            pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

            pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
            pnllst.Style.Add(HtmlTextWriterStyle.Display, "block")
        Else
            denom_out.Visible = False
            pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

            pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
            pnllst.Style.Add(HtmlTextWriterStyle.Display, "block")
            Response.Redirect(refurl.ToString)
        End If




    End Sub

    Private Sub btn_denom_update_Command(sender As Object, e As CommandEventArgs) Handles btn_denom_update.Command

    End Sub

    Private Sub inclpass_CheckedChanged(sender As Object, e As EventArgs) Handles inclpass.CheckedChanged

        bind_rp()


    End Sub

    Private Sub disp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles disp.SelectedIndexChanged

    End Sub

    Private Sub rpscroll_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpscroll.ItemCommand
        If e.CommandName = "ViewClick" Then
            ''MsgBox(e.CommandArgument)
            Session("tid") = e.CommandArgument
            get_voucher(e.CommandArgument)
            If Session("voucherType") = Nothing Or String.IsNullOrWhiteSpace(Session("voucherType")) Or Not Session("voucherType") = "JOURNAL" Then
                If inclpass.Checked Then
                    load_denom()
                    btn_denom_update.Visible = False
                    btn_prnt.Visible = True
                Else
                    btn_denom_update.Visible = True
                    btn_prnt.Visible = False

                End If
            End If
            pnllst.Style.Add(HtmlTextWriterStyle.Display, "none")
            pnldenom.Style.Add(HtmlTextWriterStyle.Display, "block")




        End If
    End Sub

    Private Sub txtdate_TextChanged(sender As Object, e As EventArgs) Handles txtdate.TextChanged

        If CDate(txtdate.Text) <> DateTime.Today Then

            inclpass.Checked = True
            bind_rp_passed()

        End If

    End Sub

    Sub bind_rp_passed()


        rpscroll.DataSource = Nothing
        rpscroll.DataBind()

        If inclpass.Checked Then
            Session("passed_v") = 1
        Else
            Session("passed_v") = 0
        End If
        Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "getDenomPassed"
                cmd.Parameters.Clear()

                cmd.Parameters.Add(New SqlParameter("@dt", reformatted))

                Try

                    Using Sda As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable
                        Sda.Fill(dt)


                        rpscroll.DataSource = dt
                        rpscroll.DataBind()
                        'get_locked(dt)


                    End Using

                Catch ex As Exception

                    Dim msg = ex.ToString().Replace("'", "").Replace(vbCrLf, " ")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "errorMessage", "showToastnOK('Error', '" & msg & "');", True)


                End Try


            End Using
            con.Close()

        End Using

    End Sub

    Private Sub inclpass_Load(sender As Object, e As EventArgs) Handles inclpass.Load

    End Sub

    Private Sub btn_can_Click(sender As Object, e As EventArgs) Handles btn_can.Click
        Dim refurl As Object = ViewState("RefUrl")


        If Not session_user_role = "Cashier" Then
            bind_rp()
            denom_in.Visible = True
            denom_out.Visible = False


            pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

            pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
            pnllst.Style.Add(HtmlTextWriterStyle.Display, "block")
        Else
            denom_in.Visible = True
            denom_out.Visible = False
            pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

            pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
            pnllst.Style.Add(HtmlTextWriterStyle.Display, "block")
            Response.Redirect(refurl.ToString)
        End If


    End Sub

    Private Sub btn_prnt_Click(sender As Object, e As EventArgs) Handles btn_prnt.Click
        pnldenom.Style.Add(HtmlTextWriterStyle.Display, "none")

        pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "block")
        If Not Session("voucherType") = Nothing And Not String.IsNullOrWhiteSpace(Session("voucherType")) And Session("voucherType") = "JOURNAL" Then
            printOCBtn.Visible = False
            printCCBtn.Visible = False
            printJrnlBtn.Visible = True
            journalVouchPrint.Visible = True
            denVouchPrint.Visible = False
        Else
            printOCBtn.Visible = True
            printCCBtn.Visible = True
            printJrnlBtn.Visible = False
            journalVouchPrint.Visible = False
            denVouchPrint.Visible = True
        End If
    End Sub

    Private Sub rpscroll_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpscroll.ItemDataBound
        For Each Items As RepeaterItem In rpscroll.Items

            Dim typ As Label = Items.FindControl("lblltyp")
            Dim camt As Label = Items.FindControl("lblamt")

            If Trim(typ.Text) = "RECEIPT" Then
                typ.ForeColor = Color.DarkGreen

                camt.Text = FormatNumber(camt.Text)
                camt.ForeColor = Color.DarkGreen

            Else
                typ.ForeColor = Color.DarkRed
                camt.Text = FormatNumber(camt.Text)
                camt.ForeColor = Color.DarkRed
            End If

        Next


    End Sub

    Private Sub denomination_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        '  Session("tid") = Nothing

    End Sub
End Class
