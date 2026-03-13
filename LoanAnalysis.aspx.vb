Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization

Public Class LoanAnalysis

    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Public newrow As DataRow
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Public dt As New DataTable
    Public dtx As New DataTable
    Public dv As DataView
    'Protected Sub cur_sel_CheckedChanged(sender As Object, e As EventArgs)



    '    For i As Integer = 0 To gv_auction.Rows.Count - 1

    '        Dim rb As RadioButton = DirectCast(gv_auction.Rows(i).Cells(0).FindControl("cur_sel"), RadioButton)
    '        Dim os As Label = DirectCast(gv_auction.Rows(i).Cells(0).FindControl("lblos"), Label)
    '        Dim acn As Label = DirectCast(gv_auction.Rows(i).Cells(0).FindControl("lblacno"), Label)
    '        Dim acval As Label = DirectCast(gv_auction.Rows(i).Cells(0).FindControl("lblacval"), Label)
    '        Dim lbldif As Label = DirectCast(gv_auction.Rows(i).Cells(0).FindControl("lbldif"), Label)

    '        If rb IsNot Nothing Then

    '            If rb.Checked Then

    '                Dim hf As HiddenField = DirectCast(gv_auction.Rows(i).Cells(0).FindControl("hf"), HiddenField)
    '                If hf IsNot Nothing Then
    '                    ViewState("id") = hf.Value

    '                    ' Dim x As Integer = CInt(hf.Value)

    '                    ''  showvinfo(hf.Value)
    '                    vin.Visible = True
    '                    lbl_acno.Text = acn.Text
    '                    lbl_os.Text = os.Text
    '                    lbl_av.Text = acval.Text
    '                    lbl_diff.Text = lbldif.Text
    '                    txtothers.Text = 0
    '                    'System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.lbl_av)
    '                    txtfocus(txtothers)
    '                End If


    '                Exit For

    '            End If

    '        End If

    '    Next

    'End Sub

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.Open()

        If Not Page.IsPostBack Then

            '  bind_auction()
            '  add_sb()


            '   bind_grid()

        End If

    End Sub

    'Sub add_sb()
    '    If con.State = ConnectionState.Closed Then con.Open()
    '    query = "SELECT master.acno,member.FirstName FROM dbo.master INNER JOIN dbo.member  ON master.cid = member.MemberNo WHERE master.product = 'SB' AND master.cld = 0"
    '    cmd.Connection = con
    '    cmd.CommandText = query

    '    Try

    '        Dim ds As New DataSet
    '        Dim adapter As New SqlDataAdapter(query, con)
    '        adapter.Fill(ds)

    '        sbac.DataSource = ds
    '        sbac.DataTextField = "acno"
    '        sbac.DataValueField = "Firstname"

    '        sbac.DataBind()
    '        sbac.Items.Insert(0, "<-Select->")


    '        adapter.Dispose()
    '        cmd.Dispose()
    '        con.Close()

    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Sub
    'Sub bind_auction()
    '    Dim dr As SqlDataReader
    '    Dim bal As Double = 0
    '    Dim rpg As Integer = 0
    '    Dim int As Double = 0





    '    If con.State = ConnectionState.Closed Then con.Open()
    '    cmd.Connection = con
    '    ''   query = "SELECT jlauctionbrkup.acno, member.FirstName,  SUM(jlauctionbrkup.weight) AS expr1,  SUM(jlauctionbrkup.val) AS expr2 FROM dbo.jlauctionbrkup "
    '    ''  query &= " INNER JOIN dbo.member   ON jlauctionbrkup.cid = member.MemberNo GROUP BY jlauctionbrkup.acno,member.FirstName where jlauctionbrkup.cld=@cld"
    '    query = "SELECT jlauctionbrkup.acno, member.FirstName,  SUM(jlauctionbrkup.weight) AS expr1,  SUM(jlauctionbrkup.val) AS expr2"
    '    query &= " FROM dbo.jlauctionbrkup LEFT OUTER JOIN dbo.member   ON jlauctionbrkup.cid = member.MemberNo WHERE jlauctionbrkup.cld = 0"
    '    query &= " GROUP BY jlauctionbrkup.acno,member.FirstName"

    '    If dtx.Columns.Count = 0 Then
    '        dtx.Columns.Add("acno", GetType(String))
    '        dtx.Columns.Add("name", GetType(String))
    '        dtx.Columns.Add("os", GetType(Decimal))
    '        dtx.Columns.Add("acval", GetType(Decimal))
    '        dtx.Columns.Add("diff", GetType(Integer))
    '    End If
    '    cmd.CommandText = query
    '    cmd.Parameters.Clear()
    '    cmd.Parameters.AddWithValue("@cld", 0)
    '    Try
    '        dr = cmd.ExecuteReader()

    '        If dr.HasRows() Then



    '            While dr.Read()


    '                bal = get_os(Trim(dr(0)))

    '                newrow = dtx.NewRow
    '                newrow(0) = dr(0)
    '                newrow(1) = dr(1)
    '                newrow(2) = bal
    '                newrow(3) = dr(3)
    '                newrow(4) = dr(3) - bal
    '                dtx.Rows.Add(newrow)






    '            End While
    '            gv_auction.DataSource = dtx
    '            gv_auction.DataBind()
    '            lbl_aw_aval.Text = FormatCurrency(dtx.Compute("sum(acval)", ""))
    '            lbl_aw_bal.Text = FormatCurrency(dtx.Compute("sum(os)", ""))
    '            lbl_aw_diff.Text = FormatCurrency(dtx.Compute("sum(diff)", ""))

    '        Else
    '            lbl_aw_aval.Text = 0
    '            lbl_aw_bal.Text = 0
    '            lbl_aw_diff.Text = 0
    '        End If


    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try


    'End Sub

    Function get_os(ByVal acn As String)
        Dim os As Double = 0
        Dim ac_bal As Double = 0

        If con1.State = ConnectionState.Closed Then con1.Open()
        query = "SELECT SUM(actrans.Drd) AS expr1,SUM(actrans.Crd) AS expr2 FROM dbo.actrans WHERE actrans.acno = @acno GROUP BY actrans.acno"
        cmd.Connection = con1
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@acno", acn)

        Dim drx As SqlDataReader

        Try
            drx = cmd.ExecuteReader()

            If drx.HasRows() Then
                drx.Read()
                ac_bal = drx(0) - drx(1)
            End If
            drx.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            con1.Close()
            cmd.Dispose()

        End Try
        Dim acdate As Date
        If con1.State = ConnectionState.Closed Then con1.Open()
        query = "select date from master where master.acno='" + Trim(acn) + "'"
        cmdi.Connection = con1
        cmdi.CommandText = query

        Try
            acdate = cmdi.ExecuteScalar()

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmdi.Dispose()

        End Try

        Dim oresult As Date
        Dim prv As Date

        If con1.State = ConnectionState.Closed Then con1.Open()
        cmdi.Connection = con1
        cmdi.Parameters.Clear()

        query = "SELECT TOP 1 actrans.date FROM dbo.actrans WHERE actrans.Drd > 0 AND actrans.Type = 'INTR' AND actrans.acno = @acn ORDER BY actrans.date DESC"
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acn", acn)

        Try
            oresult = cmdi.ExecuteScalar()
            If oresult = Date.MinValue Then
                prv = acdate
            Else
                prv = oresult

            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmdi.Dispose()
            con1.Close()

        End Try

        os = get_int(prv, ac_bal, acn)
        os = os + ac_bal

        Return os
    End Function

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        disp.PageIndex = e.NewPageIndex
        dv = Session("jlexceed")
        disp.DataSource = dv
        disp.DataBind()


    End Sub
    Protected Sub OnPaginggv(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        '  gv_auction.PageIndex = e.NewPageIndex
        '  bind_auction()


    End Sub
    Sub bind_grid()
        On Error Resume Next

        Dim fltrpg As String = ""
        Dim fltbal As String = ""
        Dim fltname As String = ""

        Dim dr As SqlDataReader
        Dim bal As Double = 0
        Dim rpg As Integer = 0
        Dim int As Double = 0
        If con.State = ConnectionState.Closed Then con.Open()
        Dim cmd As New SqlCommand("jlexceed")
        cmd.Connection = con
        cmd.CommandType = CommandType.StoredProcedure

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("date", GetType(Date))
            dt.Columns.Add("acno", GetType(String))
            dt.Columns.Add("name", GetType(String))
            dt.Columns.Add("tnet", GetType(Decimal))
            dt.Columns.Add("balance", GetType(Decimal))
            dt.Columns.Add("rpg", GetType(Integer))
        End If

        dr = cmd.ExecuteReader()

        If dr.HasRows() Then

            While dr.Read()

                bal = dr(3) - dr(4)
                If Not bal = 0 Then
                    int = get_int(dr(0), bal, dr(1))


                    newrow = dt.NewRow
                    newrow(0) = dr(0)
                    newrow(1) = dr(1)
                    newrow(2) = dr(2)
                    newrow(3) = dr(6)
                    newrow(4) = bal + int
                    If Not dr(6) = 0 Then
                        rpg = Math.Round((bal + int) / dr(6))
                    Else
                        rpg = bal + int
                    End If
                    newrow(5) = rpg
                    dt.Rows.Add(newrow)

                End If




            End While

            dv = New DataView(dt)

            If Not txtrpg.Text = "" Then

                Select Case fltrrpg.SelectedItem.Text
                    Case "Equal to"
                        fltrpg = "rpg=" + txtrpg.Text
                    Case "Less than"
                        fltrpg = "rpg<" + txtrpg.Text
                    Case "Greater than"
                        fltrpg = "rpg>" + txtrpg.Text
                End Select
            Else
                fltrpg = "rpg>0"
            End If

            If Not txtamt.Text = "" Then

                Select Case ddbal.SelectedItem.Text
                    Case "Equal to"
                        fltbal = "balance=" + txtamt.Text
                    Case "Less than"
                        fltbal = "balance<" + txtamt.Text

                    Case "Greater than"
                        fltbal = "balance>" + txtamt.Text

                End Select
            Else
                fltbal = "balance>0"
            End If

            If Not txtname.Text = "" Then
                fltname = String.Concat("CONVERT(", "name", ",System.String) LIKE '", txtname.Text, "%'")
            Else
                fltname = ""
            End If


            If txtname.Text = "" Then
                Dim x As String = fltrpg & " And " & fltbal  '& " And " & fltname

                dv.RowFilter = x 'fltrpg & " and " & fltbal
            Else
                Dim x As String = fltrpg & " And " & fltbal & " And " & fltname
                dv.RowFilter = x 'fltrpg & " and " & fltbal & " and " & fltname
            End If

            '                If txtamt.Text = "" And txtname.Text = "" And txtrpg.Text = "" Then
            'dv.RowFilter = Nothing
            ' End If



            lblsum.Text = FormatNumber(dv.Table.Compute("sum(balance)", dv.RowFilter).ToString)
            lblwtsum.Text = dv.Table.Compute("sum(tnet)", dv.RowFilter).ToString
            lblrpg.Text = FormatNumber(Math.Round(CDbl(lblsum.Text) / CDbl(lblwtsum.Text)))

            Session("jlexceed") = dv
            disp.DataSource = dv
            disp.DataBind()

        End If







    End Sub

    Sub prv_int(ByVal acn As String, ByVal ac_date As Date)
        Dim oresult As Date

        If con1.State = ConnectionState.Closed Then con1.Open()
        cmdi.Connection = con1
        cmdi.Parameters.Clear()

        query = "SELECT TOP 1 actrans.date FROM dbo.actrans WHERE actrans.Drd > 0 AND actrans.Type = 'INTR' AND actrans.acno = @acn ORDER BY actrans.date DESC"
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acn", acn)

        Try
            oresult = cmdi.ExecuteScalar()
            If oresult = Date.MinValue Then
                Session("prv_dinton") = ac_date
            Else
                Session("prv_dinton") = oresult

            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmdi.Dispose()
            con1.Close()

        End Try

    End Sub

    Function get_int(ByVal dat As Date, ByVal bal As Double, ByVal acn As String)

        Dim dint As Decimal

        prv_int(acn, dat)

        Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("prv_dinton"), Today)

        If Session("prd_buffer_d") = 0 Then Session("prd_buffer_d") = 7

        query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"




        Dim dr_roi As SqlDataReader


        If con1.State = ConnectionState.Closed Then con1.Open()

        cmdi.Parameters.Clear()
        cmdi.Connection = con1


        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@prod", "JL")
        cmdi.Parameters.AddWithValue("@prdtyp", "D")
        cmdi.Parameters.AddWithValue("@prdx", Session("prd_buffer_d"))
        cmdi.Parameters.AddWithValue("@prdy", Session("prd_buffer_d"))



        Try

            dr_roi = cmdi.ExecuteReader()

            If dr_roi.HasRows() Then
                dr_roi.Read()

                While dr_roi.Read

                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)


                    Dim x As Long = FYFRM.CompareTo(dat)

                    If x = -1 Then


                        Dim y As Long = FYTO.CompareTo(dat)

                        If y = 1 Then
                            Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                            dint = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                            Exit While

                        End If

                    End If
                End While


            End If




            If Session("prv_dinton").Equals(dat) Then


                If Session("prd_buffer_d") <= 7 Then Session("prd_buffer_d") = 7

                ' If prd_buffer <= 7 Then prd_buffer = 7

                ' If prd_buffer_d <= 7 Then prd_buffer_d = 7

            End If

            Session("totalint_d") = Math.Round((((bal) * dint / 100) / 365) * Session("prd_buffer_d"))



            ' If totint < 5 Then totint = 5
            '  If totalint_d < 5 Then totalint_d = 5


        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            con1.Close()
            cmdi.Dispose()

        End Try

        Return Session("totalint_d")

    End Function




    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click

        System.Threading.Thread.Sleep(5000)
        bind_grid()

    End Sub

    Sub disp_new()

        Dim dat As DateTime = DateTime.ParseExact(txtnewfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtnewto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        query = "SELECT  master.date,  master.acno,  member.FirstName,  master.amount FROM dbo.master INNER JOIN dbo.member   ON master.cid = member.MemberNo "
        query &= "WHERE CONVERT(VARCHAR(20), master.date, 112) BETWEEN @frm AND @to AND master.product = @prod and master.cld=@cld order by date"
        cmd.Connection = con
        ' cmd.CommandText = query

        Try
            Dim ds As New DataSet

            Dim adapter As New SqlDataAdapter(query, con)
            adapter.SelectCommand.Parameters.Clear()
            adapter.SelectCommand.Parameters.AddWithValue("@frm", reformatted)
            adapter.SelectCommand.Parameters.AddWithValue("@to", reformatted1)
            adapter.SelectCommand.Parameters.AddWithValue("@prod", txtnewprd.Text)
            adapter.SelectCommand.Parameters.AddWithValue("@cld", 0)

            adapter.Fill(ds)

            gvnew.DataSource = ds
            gvnew.DataBind()

            lblnewdsum.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(amount)", ""))
            ' lblnewmsum.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(mamt)", ""))


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub

    Private Sub btn_new_Click(sender As Object, e As EventArgs) Handles btn_new.Click
        disp_new()

    End Sub

    Sub sms_send()

        Dim msg As String = ""
        Dim ac As String = ""


        For i As Integer = 0 To gvsms.Rows.Count - 1
            Dim checkBox As CheckBox = DirectCast(gvsms.Rows(i).Cells(0).FindControl("smschk"), CheckBox)
            If Not checkBox.Checked Then
                Continue For
            End If
            Dim mobile As Label = DirectCast(gvsms.Rows(i).Cells(0).FindControl("lblsms"), Label)
            Dim acn As Label = DirectCast(gvsms.Rows(i).Cells(0).FindControl("lblsl"), Label)

            ac = "XXXXXXX" + Right(Trim(acn.Text), 4)

            msg = "Your Jewel Loan A/C " + ac + " is Due on next 2 Days. Please ignore if already Paid"

            Send_sms(mobile.Text, msg, fiscusM.TemplateID.JewelDue)
        Next


        gvsms.DataSource = Nothing
        gvsms.DataBind()




    End Sub

    Sub disp_sms()

        'Dim dat As DateTime = DateTime.ParseExact(txtcldfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        'Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        'Dim dat1 As DateTime = DateTime.ParseExact(txtcldto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        'Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        query = " SELECT master.date,master.acno,member.FirstName,master.amount,member.Mobile FROM dbo.master"
        query &= " INNER JOIN dbo.member  ON master.cid = member.MemberNo WHERE DATEPART(d, master.date) = @day AND master.product = 'JL' AND master.alert = 1 AND master.cld = 0 "


        cmd.Connection = con
        ' cmd.CommandText = query

        Try
            Dim ds As New DataSet

            Dim adapter As New SqlDataAdapter(query, con)
            adapter.SelectCommand.Parameters.Clear()
            adapter.SelectCommand.Parameters.AddWithValue("@day", txtday.Text)

            adapter.Fill(ds)

            gvsms.DataSource = ds
            gvsms.DataBind()

            ''lbldcld.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(amount)", ""))
            ''lblmcld.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(crd)", ""))


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub

    Sub disp_cld()

        Dim dat As DateTime = DateTime.ParseExact(txtcldfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtcldto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        query = " SELECT MAX(actrans.date) AS date,actrans.acno,member.FirstName,master.amount,actrans.crd FROM dbo.actrans"
        query &= " INNER JOIN dbo.master ON actrans.acno = master.acno INNER JOIN dbo.member ON master.cid = member.MemberNo"
        query &= " WHERE CONVERT(VARCHAR(20), actrans.date, 112) BETWEEN @frm AND @to AND master.product = @prd AND master.cld = @cld AND actrans.crd > 0 AND actrans.Type = 'CASH'"
        query &= " GROUP BY actrans.acno,member.FirstName,actrans.crd,master.amount ORDER BY date"

        cmd.Connection = con
        ' cmd.CommandText = query

        Try
            Dim ds As New DataSet

            Dim adapter As New SqlDataAdapter(query, con)
            adapter.SelectCommand.Parameters.Clear()
            adapter.SelectCommand.Parameters.AddWithValue("@frm", reformatted)
            adapter.SelectCommand.Parameters.AddWithValue("@to", reformatted1)
            adapter.SelectCommand.Parameters.AddWithValue("@prd", txtcldprod.Text)
            adapter.SelectCommand.Parameters.AddWithValue("@cld", 1)

            adapter.Fill(ds)

            gvcld.DataSource = ds
            gvcld.DataBind()

            lbldcld.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(amount)", ""))
            lblmcld.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(crd)", ""))


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub


    Private Sub btn_cld_Click(sender As Object, e As EventArgs) Handles btn_cld.Click
        disp_cld()

    End Sub

    Protected Sub OnPagingcld(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvcld.PageIndex = e.NewPageIndex
        disp_cld()

    End Sub
    Protected Sub OnPagingnew(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvnew.PageIndex = e.NewPageIndex
        disp_new()

    End Sub

    Function get_cid(ByVal acn As String)
        Dim cid As String = ""

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select cid from master where master.acno='" + acn + "'"
        cmd.Connection = con
        cmd.CommandText = query
        Try
            cid = cmd.ExecuteScalar()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        Return cid

    End Function

    Private Sub btnsendsms_Click(sender As Object, e As EventArgs) Handles btnsendsms.Click

        sms_send()



    End Sub

    Private Sub btnlist_Click(sender As Object, e As EventArgs) Handles btnlist.Click
        disp_sms()
        smshdrchk.Checked = True
    End Sub

    Private Sub LoanAnalysis_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Protected Sub smshdrchk_CheckedChanged(sender As Object, e As EventArgs) Handles smshdrchk.CheckedChanged
        For i As Integer = 0 To gvsms.Rows.Count - 1
            Dim checkBox As CheckBox = DirectCast(gvsms.Rows(i).Cells(0).FindControl("smschk"), CheckBox)
            checkBox.Checked = smshdrchk.Checked
        Next
    End Sub
End Class