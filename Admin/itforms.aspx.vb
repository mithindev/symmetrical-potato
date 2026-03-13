Imports System.Globalization
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data
Imports System.Drawing
Public Class itforms

    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim dt As New DataTable
    Dim newrow As DataRow
    Dim query As String

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub signout(sender As Object, e As EventArgs)

        FormsAuthentication.SignOut()

        Session.Abandon()
        Log_out(Session("logtime").ToString, Session("sesusr"))
        Response.Redirect("..\login.aspx")

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = "Data Source=.\SQLEXPRESS;Initial Catalog=fiscusdb;integrated Security = true;;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        con.Open()



    End Sub


    Sub gvin_RowDataBound()

    End Sub

    Sub OnSelectedIndexChangedgvin()

    End Sub


    Protected Sub OnPagingin(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvin.PageIndex = e.NewPageIndex
        bind_grid()

    End Sub

    Private Sub btn_inw_Click(sender As Object, e As ImageClickEventArgs) Handles btn_inw.Click

        If con.State = ConnectionState.Closed Then con.Open()

        query = "TRUNCATE TABLE dbo.tmp15gh"
        cmd.Connection = con
        cmd.CommandText = query
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text.ToString, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text.ToString, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim prod As String = "FD"
        Dim qResult As Integer = 0
        Dim queryString As String = "base15g " & "'" & reformatted & "'" & "," & "'" & reformatted1 & "'" & "," & prod
        cmd.CommandText = queryString
        cmd.Connection = con

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        prod = "RD"

        queryString = "base15g " & "'" & reformatted & "'" & "," & "'" & reformatted1 & "'" & "," & prod
        cmd.CommandText = queryString
        cmd.Connection = con

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


        prod = "RID"

        queryString = "base15g " & "'" & reformatted & "'" & "," & "'" & reformatted1 & "'" & "," & prod
        cmd.CommandText = queryString
        cmd.Connection = con

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        bind_grid()



    End Sub

    Sub bind_grid()


        Dim dl As New DataTable
        query = " SELECT tmp15gh.Memberno,tmp15gh.Firstname,SUM(tmp15gh.interest) AS interest FROM dbo.tmp15gh "
        query &= " GROUP BY tmp15gh.Memberno,tmp15gh.Firstname HAVING SUM(tmp15gh.interest) >= 5000"
        cmd.CommandText = query

        Dim Adapter = New SqlDataAdapter(query, con)
        Adapter.Fill(dl)
        gvin.DataSource = dl

        gvin.DataBind()


    End Sub

    Sub bind_savedforms()

        Dim dl As New DataTable
        '        query = " SELECT tmp15gh.Memberno,tmp15gh.Firstname,SUM(tmp15gh.interest) AS interest FROM dbo.tmp15gh "
        '       query &= " GROUP BY tmp15gh.Memberno,tmp15gh.Firstname HAVING SUM(tmp15gh.interest) >= 5000"

        query = " SELECT UID,name,pan,estimateincome from form15gh order by uid"

        cmd.CommandText = query

        Dim Adapter = New SqlDataAdapter(query, con)
        Adapter.Fill(dl)
        gv_saved.DataSource = dl

        gv_saved.DataBind()

    End Sub
    Protected Sub paid_chked(sender As Object, e As EventArgs)

        txttdspaid.Text = 0
        Dim count As Integer = 0

        For i As Integer = 0 To disp_as.Rows.Count - 1

            Dim rb As CheckBox = DirectCast(disp_as.Rows(i).Cells(0).FindControl("paidchk"), CheckBox)

            If rb IsNot Nothing Then

                If rb.Checked Then

                    Dim cid As Label = DirectCast(disp_as.Rows(i).Cells(0).FindControl("lblmno"), Label)
                    Dim fn As Label = DirectCast(disp_as.Rows(i).Cells(0).FindControl("lblname"), Label)
                    Dim INCOME As Label = DirectCast(disp_as.Rows(i).Cells(0).FindControl("lblint"), Label)
                    Dim tds As Label = DirectCast(disp_as.Rows(i).Cells(0).FindControl("lbltds"), Label)

                    txttdspaid.Text = CDbl(txttdspaid.Text) + CDbl(tds.Text)
                    count = count + 1

                End If

            End If

        Next
        '  txttdspaid.Text = total
        If txttotal.Text = "" Then txttotal.Text = 0
        txttotal.Text = CDbl(txttdspaid.Text)

        If CDbl(txttdspaid.Text) > 0 Then
            pnltdspaid.Visible = True

        Else
            pnltdspaid.Visible = False
        End If


    End Sub

    Protected Sub tds_chked(sender As Object, e As EventArgs)

        Dim total As Decimal = 0
        Dim count As Integer = 0

        For i As Integer = 0 To gvin.Rows.Count - 1

            Dim rb As CheckBox = DirectCast(gvin.Rows(i).Cells(0).FindControl("itmchk"), CheckBox)

            If rb IsNot Nothing Then

                If rb.Checked Then

                    Dim hf As HiddenField = DirectCast(gvin.Rows(i).Cells(0).FindControl("hf"), HiddenField)
                    Dim fn As Label = DirectCast(gvin.Rows(i).Cells(0).FindControl("lblshrfrm"), Label)
                    Dim INCOME As Label = DirectCast(gvin.Rows(i).Cells(0).FindControl("lblshrto"), Label)
                    'If hf IsNot Nothing Then
                    ' ViewState("id") = hf.Value
                    If rb.Checked = True Then
                        total = total + CDbl(INCOME.Text)
                        count = count + 1
                    End If
                    'End If

                End If

            End If

        Next

        lbldsum.Text = FormatCurrency(total)
        lblmsum.Text = count
        If lblmsum.Text <> 0 Then
            btn_update_26as.Visible = True
        Else
            btn_update_26as.Visible = False
        End If

    End Sub


    Protected Sub cur_sel_CheckedChanged(sender As Object, e As EventArgs)



        For i As Integer = 0 To gvin.Rows.Count - 1

            Dim rb As RadioButton = DirectCast(gvin.Rows(i).Cells(0).FindControl("cur_sel"), RadioButton)

            If rb IsNot Nothing Then

                If rb.Checked Then

                    Dim hf As HiddenField = DirectCast(gvin.Rows(i).Cells(0).FindControl("hf"), HiddenField)
                    Dim fn As Label = DirectCast(gvin.Rows(i).Cells(0).FindControl("lblshrfrm"), Label)
                    Dim INCOME As Label = DirectCast(gvin.Rows(i).Cells(0).FindControl("lblshrto"), Label)
                    If hf IsNot Nothing Then
                        ViewState("id") = hf.Value

                        ' Dim x As Integer = CInt(hf.Value)
                        'disp.Visible = False
                        'showvinfo(hf.Value)
                        'prepare_sms()
                        prepare_income_details(hf.Value, fn.Text, INCOME.Text)

                        tabcontainer1.ActiveTab = tab_income
                        tabcontainer1.ActiveTab.Focus()
                    End If


                    Exit For

                End If

            End If

        Next

    End Sub

    Sub prepare_income_details(ByVal memno As String, ByVal name As String, ByVal INCOME As String)

        'lblmember.Text = memno
        'lblname.Text = name

        txtuid.Text = memno
        txtname.Text = name
        txtname.Enabled = False
        txtstatus.Text = "Individual"
        txtstatus.Enabled = False
        txtpy.Text = (Mid(txtfrm.Text, 7, 4) - 1)
        txtpy.Enabled = False
        txtres.Text = "Resident"
        txtres.Enabled = False
        txtstate.Text = "Tamil Nadu"
        txtstate.Enabled = False
        txttax.Text = "No"
        txttax.Enabled = False
        txtincome.Text = INCOME
        txtincome.Enabled = False
        txtincomepaid.Text = INCOME
        txtincomepaid.Enabled = False
        txtincomepaiddate.Text = "31/03/2018"
        txtincomepaiddate.Enabled = False
        txt15gamt.Text = INCOME
        txt15gamt.Enabled = False
        txt15gno.Text = 0
        txttaxyr.Enabled = False
        txt15gno.Enabled = False
        txttotalincome.Text = INCOME
        txttotalincome.Enabled = False
        txt_dcl_date.Text = "31/03/2018"
        txt_dcl_date.Enabled = False

        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        query = " SELECT   master.acno, master.product,SUM(actrans.Crc) AS amount FROM dbo.master LEFT OUTER JOIN dbo.actrans "
        query &= " ON master.acno = actrans.acno WHERE actrans.date BETWEEN @fdmy AND @tdmy "
        query &= " AND actrans.Type = 'INTR' AND master.cid = @cid AND master.product <> 'SB' AND master.product <> 'KMK'"
        query &= " GROUP BY master.acno, master.product HAVING SUM(actrans.Crc) <> 0 "



        Dim dl As New DataTable
        If con.State = ConnectionState.Closed Then con.Open()

        Dim Adapter = New SqlDataAdapter(query, con)
        Adapter.SelectCommand.Parameters.AddWithValue("@cid", memno)
        Adapter.SelectCommand.Parameters.AddWithValue("@fdmy", reformatted)
        Adapter.SelectCommand.Parameters.AddWithValue("@tdmy", reformatted1)

        Adapter.Fill(dl)

        lbl_incom_total.Text = FormatNumber(dl.Compute("SUM(amount)", String.Empty), 2)
        gv_income.DataSource = dl

        gv_income.DataBind()



    End Sub

    Private Sub formg_CheckedChanged(sender As Object, e As EventArgs) Handles formg.CheckedChanged
        If formg.Checked Then
            formh.Checked = False
            lbldob.Visible = False
            txtdob.Visible = False
        Else
            formh.Checked = True
            lbldob.Visible = True
            txtdob.Visible = True
        End If


    End Sub

    Private Sub formh_CheckedChanged(sender As Object, e As EventArgs) Handles formh.CheckedChanged
        If formh.Checked Then
            formg.Checked = False
            lbldob.Visible = True
            txtdob.Visible = True
        Else
            formg.Checked = True
            lbldob.Visible = False
            txtdob.Visible = False
        End If


    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        query = " INSERT INTO dbo.FORM15GH (FORMTYPE, UID, MEMBERNO, NAME, PAN, STATUS, [Residential Status], [Previous year], Flat, Road, area, premises, town, state, pin, email, stdcode, phone, mobile, assessed, [if yes], estimateincome, [total income], noof15g, [aggregate income], [15gdate], [income paid], paidon, [record type], dob) "
        query &= "VALUES (@formtype, @uid, @memberno, @name, @pan, @status, @residential, @py, @flat, @road, @area, @premises, @town, @state, @pin, @email, @std, @phone, @mobile, @assessed, @ifyes, @estimateincome, @totalincome, @noof15g, @agg, @15gdate, @incomepaid, @paidon, @record, @dob)"

        '  query = "UPDATE dbo.FORM15GH SET [income paid] = @incomepaid,[aggregate income] = @agg,estimateincome = @estimateincome,[total income] = @totalincome WHERE MEMBERNO = @memberno"

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.CommandText = query

        If formg.Checked = True Then
            cmd.Parameters.AddWithValue("@formtype", "15G")
        Else
            cmd.Parameters.AddWithValue("@formtype", "15H")
        End If

        cmd.Parameters.AddWithValue("@uid", txtuid.Text)
        cmd.Parameters.AddWithValue("@memberno", txtuid.Text)
        cmd.Parameters.AddWithValue("@name", txtname.Text)
        cmd.Parameters.AddWithValue("@pan", txtpan.Text)
        cmd.Parameters.AddWithValue("@status", txtstatus.Text)
        cmd.Parameters.AddWithValue("@residential", txtres.Text)
        cmd.Parameters.AddWithValue("@py", txtpy.Text)
        cmd.Parameters.AddWithValue("@flat", txtdoor.Text)
        cmd.Parameters.AddWithValue("@road", txtroad.Text)
        cmd.Parameters.AddWithValue("@area", txtarea.Text)
        cmd.Parameters.AddWithValue("@premises", txtpremises.Text)
        cmd.Parameters.AddWithValue("@town", txtcity.Text)
        cmd.Parameters.AddWithValue("@state", txtstate.Text)
        cmd.Parameters.AddWithValue("@pin", txtpin.Text)
        cmd.Parameters.AddWithValue("@email", txtemial.Text)
        cmd.Parameters.AddWithValue("@std", txtstd.Text)
        cmd.Parameters.AddWithValue("@phone", txtphone.Text)
        cmd.Parameters.AddWithValue("@mobile", txtmobile.Text)
        cmd.Parameters.AddWithValue("@assessed", txttax.Text)
        cmd.Parameters.AddWithValue("@ifyes", txttaxyr.Text)
        cmd.Parameters.AddWithValue("@estimateincome", CDbl(txt15gamt.Text))
        cmd.Parameters.AddWithValue("@totalincome", CDbl(txtincome.Text))
        cmd.Parameters.AddWithValue("@noof15g", CInt(txt15gno.Text))
        cmd.Parameters.AddWithValue("@agg", CDbl(txttotalincome.Text))
        cmd.Parameters.AddWithValue("@15gdate", Convert.ToDateTime(txt_dcl_date.Text))
        cmd.Parameters.AddWithValue("@incomepaid", CDbl(txtincomepaid.Text))
        cmd.Parameters.AddWithValue("@paidon", Convert.ToDateTime(txtincomepaiddate.Text))
        cmd.Parameters.AddWithValue("@record", "A")
        If Not txtdob.Text = "" Then
            cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(txtdob.Text))
        Else
            cmd.Parameters.AddWithValue("@dob", txtdob.Text)
        End If


        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


        For i As Integer = 0 To gv_income.Rows.Count - 1



            Dim product As Label = DirectCast(gv_income.Rows(i).Cells(0).FindControl("lblproduct"), Label)
            Dim acno As Label = DirectCast(gv_income.Rows(i).Cells(0).FindControl("lblacno"), Label)
            Dim amt As Label = DirectCast(gv_income.Rows(i).Cells(0).FindControl("lblamt"), Label)

            query = " INSERT INTO incomedetails(MEMBERNO,PRODUCT,acno,amount) "
            query &= "values(@MEMBERNO,@PRODUCT,@acno,@amount)"

            cmd.CommandText = query

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@memberno", txtuid.Text)
            cmd.Parameters.AddWithValue("@product", product.Text)
            cmd.Parameters.AddWithValue("@acno", acno.Text)
            cmd.Parameters.AddWithValue("@amount", CDbl(amt.Text))

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.Message)
            End Try

        Next


        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

        sb.Append("<div class=" + """alert alert-dismissable alert-primary """ + ">")
        sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
        sb.Append("<strong>Updated !</strong> ")
        sb.Append("</div>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())


        pnlbrnch.Visible = False
        txtvky.Text = ""
        txtmkm.Text = ""
        txtal.Text = ""
        txtpdm.Text = ""
        txttdspaid.Text = ""
        txttotal.Text = ""
        tdate.Text = ""
        ddbank.Items.Clear()
        ddled.Items.Clear()
        pnltdspaid.Visible = False
        txtqtr.Text = ""
        txtyr.Text = ""
        disp_as.DataSource = Nothing
        disp_as.DataBind()

        tabcontainer1.ActiveTabIndex = 1
        tabcontainer1.ActiveTab.Focus()
        clea()
        '' Response.Redirect("~/admin/dashboard.aspx")


    End Sub

    Sub clea()
        txtuid.Text = ""
        txtuid.Text = ""
        txtname.Text = ""
        txtpan.Text = ""
        txtstatus.Text = ""
        txtres.Text = ""
        txtpy.Text = ""
        txtdoor.Text = ""
        txtroad.Text = ""
        txtarea.Text = ""
        txtpremises.Text = ""
        txtcity.Text = ""
        txtstate.Text = ""
        txtpin.Text = ""
        txtemial.Text = ""
        txtstd.Text = ""
        txtphone.Text = ""
        txtmobile.Text = ""
        txttax.Text = ""
        txttaxyr.Text = ""
        txt15gamt.Text = ""
        txtincome.Text = ""
        txt15gno.Text = ""
        txttotalincome.Text = ""
        txt_dcl_date.Text = ""
        txtincomepaid.Text = ""
        txtincomepaiddate.Text = ""

        txtdob.Text = ""

        txtdob.Visible = False
        lbldob.Visible = False
        formg.Checked = False
        formh.Checked = False
        gv_income.DataSource = Nothing
        gv_income.DataBind()
        bind_savedforms()
        tabcontainer1.ActiveTab = formtab




    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        clea()

    End Sub

    Private Sub btn_update_26as_Click(sender As Object, e As EventArgs) Handles btn_update_26as.Click
        Dim total As Decimal = 0
        Dim count As Integer = 0
        Dim pan As String = Nothing

        For i As Integer = 0 To gvin.Rows.Count - 1

            Dim rb As CheckBox = DirectCast(gvin.Rows(i).Cells(0).FindControl("itmchk"), CheckBox)

            If rb IsNot Nothing Then

                If rb.Checked Then

                    Dim hf As HiddenField = DirectCast(gvin.Rows(i).Cells(0).FindControl("hf"), HiddenField)
                    Dim fn As Label = DirectCast(gvin.Rows(i).Cells(0).FindControl("lblshrfrm"), Label)
                    Dim INCOME As Label = DirectCast(gvin.Rows(i).Cells(0).FindControl("lblshrto"), Label)
                    If hf IsNot Nothing Then
                        ViewState("id") = hf.Value

                        If con.State = ConnectionState.Closed Then con.Open()

                        query = "select pan from kyc where MemberNo=@cid"
                        cmd.Connection = con
                        cmd.CommandText = query
                        Try
                            pan = cmd.ExecuteScalar()


                        Catch ex As Exception
                            Response.Write(ex.ToString)
                        End Try


                        update_tds(hf.Value, fn.Text, INCOME.Text, pan)

                        pan = Nothing

                        rb.Checked = False
                    End If

                End If

            End If

        Next

        btn_update_26as.Enabled = False

        tabcontainer1.ActiveTab = f26as
        tabcontainer1.Focus()

        Response.Redirect("~/admin/itforms.aspx")



    End Sub

    Sub update_tds(ByVal cid As String, ByVal name As String, ByVal intrest As Decimal, ByVal pan As String)

        Dim qtr As String = Nothing
        Dim fq As Integer = Convert.ToDateTime(txtfrm.Text).Month
        Dim tq As Integer = Convert.ToDateTime(txtto.Text).Month
        Dim yr As String = Convert.ToDateTime(txtfrm.Text).Year
        Dim tds As Decimal = Math.Round((intrest / 100) * 10)
        If pan = Nothing Then pan = ""

        Select Case fq

            Case 4
                If tq = 6 Then qtr = "Q1"

            Case 7
                If tq = 9 Then qtr = "Q2"
            Case 10
                If tq = 12 Then qtr = "Q3"
            Case 1
                If tq = 3 Then qtr = "Q4"
            Case Else
                qtr = ""

        End Select




        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con

        query = "insert into tds(quater,tdsyear,memberno,name,pan,interest,tds)"
        query &= " values(@quater,@tdsyear,@memberno,@name,@pan,@interest,@tds)"
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@quater", qtr)
        cmd.Parameters.AddWithValue("@tdsyear", yr)
        cmd.Parameters.AddWithValue("@memberno", cid)
        cmd.Parameters.AddWithValue("@name", name)
        cmd.Parameters.AddWithValue("@interest", intrest)
        cmd.Parameters.AddWithValue("@tds", tds)
        cmd.Parameters.AddWithValue("@pan", pan)
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Sub btn_as_Click(sender As Object, e As ImageClickEventArgs) Handles btn_as.Click

        bind_as()


        ddbank.Items.Clear()
        Dim dr As SqlDataReader

        query = "SELECT ledger.ledger FROM dbo.ledger WHERE ledger.printorder = '1' "

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.CommandText = query

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                Do While dr.Read()

                    ddbank.Items.Add(dr(0))

                Loop
            End If
            ddbank.Items.Insert(0, "<-- Select -->")
            ddbank.Items.Insert(1, "HEAD OFFICE")
            ddbank.Items.Item(0).Value = ""
            dr.Close()


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        ddled.Items.Clear()
        '  Dim dr As SqlDataReader

        query = "SELECT ledger.ledger FROM dbo.ledger WHERE ledger.printorder = '0' "

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.CommandText = query

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                Do While dr.Read()

                    ddled.Items.Add(dr(0))

                Loop
            End If
            ddled.Items.Insert(0, "<-- Select -->")
            'ddled.Items.Insert(1, "HEAD OFFICE")
            ddled.Items.Item(0).Value = ""
            dr.Close()


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


        If con.State = ConnectionState.Closed Then con.Open()
        query = "select branchcode from branch "
        cmd.Connection = con
        cmd.CommandText = query

        Try
            Dim br As String = cmd.ExecuteScalar
            Session("br") = br
            If br = "01" Then pnlbrnch.Visible = True
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub
    Sub bind_as()
        Dim dl As New DataTable
        If con.State = ConnectionState.Closed Then con.Open()

        If txtqtr.Text = "" Then
            query = "select memberno,name,pan,interest,tds,paidon from tds where tdsyear=@yr"
        Else
            query = "select memberno,name,pan,interest,tds,paidon from tds where tdsyear=@yr and quater=@qtr"
        End If


        Dim Adapter = New SqlDataAdapter(query, con)
        Adapter.SelectCommand.Parameters.AddWithValue("@qtr", txtqtr.Text)
        Adapter.SelectCommand.Parameters.AddWithValue("@yr", txtyr.Text)


        Adapter.Fill(dl)

        lblinttotal.Text = FormatNumber(dl.Compute("SUM(interest)", String.Empty), 2)
        lbltdstotal.Text = FormatNumber(dl.Compute("SUM(tds)", String.Empty), 2)
        disp_as.DataSource = dl

        disp_as.DataBind()


    End Sub

    Protected Sub ExportToExcel(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            'To Export all pages
            disp_as.AllowPaging = False
            bind_as()


            disp_as.HeaderRow.BackColor = Color.White
            For Each cell As TableCell In disp_as.HeaderRow.Cells
                cell.BackColor = disp_as.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In disp_as.Rows
                row.BackColor = Color.White
                For Each cell As TableCell In row.Cells
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = disp_as.AlternatingRowStyle.BackColor
                    Else
                        cell.BackColor = disp_as.RowStyle.BackColor
                    End If
                    cell.CssClass = "textmode"
                Next
            Next

            disp_as.RenderControl(hw)
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using
    End Sub

    Private Sub btnexp_Click(sender As Object, e As EventArgs) Handles btnexp.Click
        ExportToExcel(sender, e)

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub

    Private Sub txtvky_TextChanged(sender As Object, e As EventArgs) Handles txtvky.TextChanged
        If txtvky.Text = "" Then txtvky.Text = 0
        If txtmkm.Text = "" Then txtmkm.Text = 0
        If txtal.Text = "" Then txtal.Text = 0
        If txtpdm.Text = "" Then txtpdm.Text = 0
        txttotal.Text = CDbl(txttdspaid.Text) + CDbl(txtvky.Text) + CDbl(txtmkm.Text) + CDbl(txtal.Text) + CDbl(txtpdm.Text)
        txtfocus(txtmkm)
    End Sub

    Private Sub txtmkm_TextChanged(sender As Object, e As EventArgs) Handles txtmkm.TextChanged
        If txtvky.Text = "" Then txtvky.Text = 0
        If txtmkm.Text = "" Then txtmkm.Text = 0
        If txtal.Text = "" Then txtal.Text = 0
        If txtpdm.Text = "" Then txtpdm.Text = 0
        txttotal.Text = CDbl(txttdspaid.Text) + CDbl(txtvky.Text) + CDbl(txtmkm.Text) + CDbl(txtal.Text) + CDbl(txtpdm.Text)
        txtfocus(txtal)
    End Sub

    Private Sub txtal_TextChanged(sender As Object, e As EventArgs) Handles txtal.TextChanged
        If txtvky.Text = "" Then txtvky.Text = 0
        If txtmkm.Text = "" Then txtmkm.Text = 0
        If txtal.Text = "" Then txtal.Text = 0
        If txtpdm.Text = "" Then txtpdm.Text = 0
        txttotal.Text = CDbl(txttdspaid.Text) + CDbl(txtvky.Text) + CDbl(txtmkm.Text) + CDbl(txtal.Text) + CDbl(txtpdm.Text)
        txtfocus(txtpdm)
    End Sub

    Private Sub txtpdm_TextChanged(sender As Object, e As EventArgs) Handles txtpdm.TextChanged
        If txtvky.Text = "" Then txtvky.Text = 0
        If txtmkm.Text = "" Then txtmkm.Text = 0
        If txtal.Text = "" Then txtal.Text = 0
        If txtpdm.Text = "" Then txtpdm.Text = 0
        txttotal.Text = CDbl(txttdspaid.Text) + CDbl(txtvky.Text) + CDbl(txtmkm.Text) + CDbl(txtal.Text) + CDbl(txtpdm.Text)
        'txtfocus(txtmkm)
    End Sub

    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)

        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con
        cmd.Parameters.Clear()
        'cmd.CreateParameter()


        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@date,@tid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@tid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        If dr = 0 Then
            cmd.Parameters.AddWithValue("@nar", "By Transfer")
        Else
            cmd.Parameters.AddWithValue("@nar", nar)
        End If

        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()


        End Try

        query = ""



    End Sub
    Private Sub set_changes()
        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            Log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("..\login.aspx")

        End If
        'Dim ovr_d As Double
        'Dim ovr_c As Double
        Dim countresult As Integer
        Dim query As String = String.Empty

        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con


        Dim txtnar = "TDS Remitted for " + txtqtr.Text + " " + txtyr.Text

        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        '  Dim prod = get_pro(product)

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", "")
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", CDbl(txttotal.Text))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", CDbl(txttotal.Text))
        cmd.Parameters.AddWithValue("@narration", txtnar)
        cmd.Parameters.AddWithValue("@due", "")
        cmd.Parameters.AddWithValue("@type", "JOURNAL")
        cmd.Parameters.AddWithValue("@suplimentry", ddbank.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)



        Finally

            con.Close()
            cmd.Dispose()


        End Try

        query = ""

        Dim x As String = ddbank.SelectedItem.Text



        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con

        query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + x + "'"
        'cmd.Parameters.AddWithValue("@led", x)
        cmd.CommandText = query


        Try

            countresult = cmd.ExecuteScalar()

            Session("tid") = Convert.ToString(countresult)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try



        '  If drac.SelectedItem.Text = "GL" Then
        update_suplementry(Session("tid"), ddled.SelectedItem.Text, 0, CDbl(txttotal.Text), txtnar, "JOURNAL")
        'Else

        'trim_disp()


        'End If


        update_suplementry(Session("tid"), ddbank.SelectedItem.Text, CDbl(txttotal.Text), 0, txtnar, "JOURNAL")


        update_suplementry(Session("tid"), ddled.SelectedItem.Text, CDbl(txttotal.Text), 0, txtnar, "JOURNAL")

        If Session("br") = "01" Then
            update_suplementry(Session("tid"), "VKY BRANCH", 0, CDbl(txtvky.Text), txtnar, "JOURNAL")
            update_suplementry(Session("tid"), "MKM BRANCH", 0, CDbl(txtmkm.Text), txtnar, "JOURNAL")
            update_suplementry(Session("tid"), "ALENCODE BRANCH", 0, CDbl(txtal.Text), txtnar, "JOURNAL")
            update_suplementry(Session("tid"), "PADMANABHAPURAM BRANCH", 0, CDbl(txtpdm.Text), txtnar, "JOURNAL")
        End If

        update_suplementry(Session("tid"), "TDS RECEIVABLE", 0, CDbl(txttdspaid.Text), txtnar, "JOURNAL")


        'set_diff(ovr)




        '  btn_prnt.Visible = True
        For i As Integer = 0 To disp_as.Rows.Count - 1

            Dim rb As CheckBox = DirectCast(disp_as.Rows(i).Cells(0).FindControl("paidchk"), CheckBox)

            If rb IsNot Nothing Then

                If rb.Checked Then

                    Dim cid As Label = DirectCast(disp_as.Rows(i).Cells(0).FindControl("lblmno"), Label)
                    Dim fn As Label = DirectCast(disp_as.Rows(i).Cells(0).FindControl("lblname"), Label)
                    Dim INCOME As Label = DirectCast(disp_as.Rows(i).Cells(0).FindControl("lblint"), Label)
                    Dim tds As Label = DirectCast(disp_as.Rows(i).Cells(0).FindControl("lbltds"), Label)

                    query = "insert into tdsbrkup(date,memberno,name,debit,credit,narr) values(@date,@memberno,@name,@debit,@credit,@narr)"
                    If con.State = ConnectionState.Closed Then con.Open()
                    cmd.Connection = con
                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                    cmd.Parameters.AddWithValue("@memberno", cid.Text)
                    cmd.Parameters.AddWithValue("@name", fn.Text)
                    cmd.Parameters.AddWithValue("@debit", CDbl(tds.Text))
                    cmd.Parameters.AddWithValue("@credit", 0)
                    cmd.Parameters.AddWithValue("@narr", txtnar)

                    Try
                        cmd.ExecuteNonQuery()


                    Catch ex As Exception

                        Response.Write(ex.ToString)
                    End Try

                End If

            End If

        Next






        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

        sb.Append("<div class=" + """alert alert-dismissable alert-primary """ + ">")
        sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
        sb.Append("<strong>Updated !</strong> Transaction Id is " + Session("tid"))
        sb.Append("</div>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())



        'Catch ex As Exception

        '    MsgBox(ex.Message)
        'End Try



    End Sub



    Private Sub btn_upadate_tds_Click(sender As Object, e As EventArgs) Handles btn_upadate_tds.Click
        set_changes()

    End Sub


    Sub bind_tds()
        Dim dl As New DataTable
        If con.State = ConnectionState.Closed Then con.Open()

        query = "SELECT tdsbrkup.date,tdsbrkup.memberno,tdsbrkup.name,SUM(tdsbrkup.debit) AS debit,SUM(tdsbrkup.credit) AS credit,SUM(tdsbrkup.debit)-SUM(tdsbrkup.credit)  AS balance "
        query &= " FROM dbo.tdsbrkup GROUP BY tdsbrkup.date,tdsbrkup.memberno,tdsbrkup.name "



        Dim Adapter = New SqlDataAdapter(query, con)
        Adapter.SelectCommand.Parameters.AddWithValue("@qtr", txtqtr.Text)
        Adapter.SelectCommand.Parameters.AddWithValue("@yr", txtyr.Text)


        Adapter.Fill(dl)

        disp_tds.DataSource = dl

        disp_tds.DataBind()
        If Not dl.Rows.Count = 0 Then
            lblcr.Text = FormatNumber(dl.Compute("SUM(credit)", String.Empty), 2)
            lbldr.Text = FormatNumber(dl.Compute("SUM(debit)", String.Empty), 2)
            lblbal.Text = FormatNumber(dl.Compute("SUM(balance)", String.Empty), 2)
        End If
    End Sub



End Class