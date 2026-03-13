Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Imports System.Net



Public Class DepositAnalysis
    Inherits System.Web.UI.Page

    Public closed As Integer = 0
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection

    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim newrow As DataRow
    Public dt As New DataTable

    Dim query As String


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.Open()

        If Not Page.IsPostBack Then

            '  ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtcid.ClientID), True)
            ' bind_grid()
            '  gvin.DataSource = Nothing
            ' gvin.DataBind()


        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvmin.PageIndex = e.NewPageIndex

        dt = Session("minbal")
        gvmin.DataSource = dt
        gvmin.DataBind()
    End Sub

    Protected Sub OnPagingcld(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvcld.PageIndex = e.NewPageIndex
        disp_cld()

    End Sub
    Protected Sub OnPagingnew(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvnew.PageIndex = e.NewPageIndex
        disp_new()

    End Sub
    Protected Sub OnPagingin(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvin.PageIndex = e.NewPageIndex
        disp_mat()

    End Sub
    Sub bind_grid()

        Dim sum As Decimal = 0

        Dim dt As New DataTable
        If dt.Columns.Count = 0 Then
            dt.Columns.Add("acno", GetType(String))
            dt.Columns.Add("Firstname", GetType(String))
            dt.Columns.Add("balance", GetType(Decimal))
        End If


        Dim dat As DateTime = DateTime.ParseExact(txtasat.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()


        If session_user_role = "Audit" Then
            query = " SELECT masterc.acno,member.FirstName,SUM(actransc.Drd) AS dr,SUM(actransc.Crd) AS cr FROM dbo.masterc"
            query &= " INNER JOIN dbo.member ON masterc.cid = member.MemberNo INNER JOIN dbo.actransc   ON masterc.acno = actransc.acno"
            query &= " WHERE masterc.product = @prd AND masterc.cld = '0' AND CONVERT(VARCHAR(20), actransc.date, 112) <= @dt GROUP BY masterc.acno, member.FirstName"

        Else
            query = " SELECT master.acno,member.FirstName,SUM(actrans.Drd) AS dr,SUM(actrans.Crd) AS cr FROM dbo.master"
            query &= " INNER JOIN dbo.member ON master.cid = member.MemberNo INNER JOIN dbo.actrans   ON master.acno = actrans.acno"
            query &= " WHERE master.product = @prd AND master.cld = '0' AND CONVERT(VARCHAR(20), actrans.date, 112) <= @dt GROUP BY master.acno, member.FirstName"

        End If


        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@prd", txtprd.Text)
        cmd.Parameters.AddWithValue("@dt", reformatted)
        Try
            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                While dr.Read()

                    sum = dr(3) - dr(2)

                    If sum <= CDbl(txtbal.Text) Then
                        newrow = dt.NewRow
                        newrow(0) = dr(0)
                        newrow(1) = dr(1)
                        newrow(2) = sum
                        dt.Rows.Add(newrow)

                    End If
                    sum = 0

                End While
            End If
            Session("minbal") = dt
            gvmin.DataSource = dt
            gvmin.DataBind()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub

    Sub disp_mat()

        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        If session_user_role = "Audit" Then
            query = "SELECT  masterc.mdate,  masterc.acno,  member.FirstName,  masterc.amount,  masterc.mamt FROM dbo.masterc INNER JOIN dbo.member   ON masterc.cid = member.MemberNo "
            query &= " WHERE CONVERT(VARCHAR(20), masterc.mdate, 112) BETWEEN @frm AND @to AND masterc.product = @prod and masterc.cld=@cld order by mdate"
        Else
            query = "SELECT  master.mdate,  master.acno,  member.FirstName,  master.amount,  master.mamt FROM dbo.master INNER JOIN dbo.member   ON master.cid = member.MemberNo "
            query &= " WHERE CONVERT(VARCHAR(20), master.mdate, 112) BETWEEN @frm AND @to AND master.product = @prod and master.cld=@cld order by mdate"
        End If

        cmd.Connection = con
        ' cmd.CommandText = query

        Try
            Dim ds As New DataSet

            Dim adapter As New SqlDataAdapter(query, con)
            adapter.SelectCommand.Parameters.Clear()
            adapter.SelectCommand.Parameters.AddWithValue("@frm", reformatted)
            adapter.SelectCommand.Parameters.AddWithValue("@to", reformatted1)
            adapter.SelectCommand.Parameters.AddWithValue("@prod", txtprod.Text)
            adapter.SelectCommand.Parameters.AddWithValue("@cld", 0)

            adapter.Fill(ds)

            gvin.DataSource = ds
            gvin.DataBind()

            lbldsum.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(amount)", ""))
            lblmsum.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(mamt)", ""))


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub
    Protected Sub gvin_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#A1DCF2';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='#f0f4f5';"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvin, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
            e.Row.Attributes("style") = "cursor:pointer"
            ' e.Row.ToolTip = "Click last column for selecting this row."

            ' e.Row.Cells[6].Controls.Dispose()

            '  firstCell.Controls.Clear();

        End If
    End Sub
    Protected Sub OnSelectedIndexChangedgvin(ByVal sender As Object, ByVal e As EventArgs) Handles gvin.SelectedIndexChanged
        'Dim pName As String = disp.SelectedRow.Cells(1).Text
        'MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)
        For Each row As GridViewRow In gvin.Rows
            If row.RowIndex = gvin.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
                ' txtlimit.Text = disp.SelectedRow.Cells(1).Text 'row.Cells.Item(0).Text
                Dim dt As CheckBox = DirectCast(gvin.SelectedRow.Cells(0).FindControl("itmchk"), CheckBox)

                If dt.Checked Then
                    dt.Checked = False
                Else
                    dt.Checked = True
                End If
            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#f0f4f5")
                row.ToolTip = "Click to select this row."
            End If
        Next
    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        disp_mat()

    End Sub

    Protected Sub bind_rddue()

        Dim acdat As Date

        Dim curdue_period As String
        Dim rdlate As Integer

        Dim newrow As DataRow
        If con.State = ConnectionState.Closed Then con.Open()

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("acno", GetType(String))
            dt.Columns.Add("Firstname", GetType(String))
            dt.Columns.Add("amount", GetType(Decimal))
            dt.Columns.Add("dupaid", GetType(String))
            dt.Columns.Add("pending", GetType(Integer))

        End If

        If session_user_role = "Audit" Then
            query = " SELECT masterc.date,masterc.acno, member.FirstName, masterc.amount, masterc.prd FROM dbo.masterc INNER JOIN dbo.member   ON masterc.cid = member.MemberNo"
            query &= " WHERE masterc.product =@prd AND masterc.cld =@cld ORDER BY masterc.date"

        Else
            query = " SELECT master.date,master.acno, member.FirstName, master.amount, master.prd FROM dbo.master INNER JOIN dbo.member   ON master.cid = member.MemberNo"
            query &= " WHERE master.product =@prd AND master.cld =@cld ORDER BY master.date"

        End If

        cmd.Connection = con
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@prd", "RD")
        cmd.Parameters.AddWithValue("@cld", 0)


        Try
            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                While dr.Read()

                    acdat = dr(0)
                    ' paid = dr(5) - dr(4)

                    curdue_period = get_due(dr(1), dr(0))
                    rdlate = calculate_penalty(dr(1), dr(0))

                    If Not rdlate <= 0 Then
                        newrow = dt.NewRow
                        newrow(0) = dr(1)
                        newrow(1) = dr(2)
                        newrow(2) = dr(3)
                        newrow(3) = curdue_period
                        newrow(4) = rdlate
                        dt.Rows.Add(newrow)
                    End If


                End While

            End If
            gvout.DataSource = dt
            gvout.DataBind()

            Session("rddue") = dt

            dr.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub

    Protected Sub btn_rd_Click(sender As Object, e As EventArgs) Handles btn_rd.Click
        System.Threading.Thread.Sleep(5000)

        bind_rddue()

    End Sub

    Function get_due(ByVal acn As String, ByVal acdt As Date)

        Dim df As String = "MMM-yyyy"
        Dim op As String = ""
        Dim opt As DateTime
        Dim sql As String
        If con1.State = ConnectionState.Closed Then con1.Open()

        If session_user_role = "Audit" Then

            sql = "SELECT COUNT(*) FROM dbo.[actransc] WHERE [actransc].acno ='" + acn + "' AND [actransc].type<>'INTR' "
        Else
            sql = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].type<>'INTR' "
        End If


        cmdi.Connection = con1
        cmdi.CommandText = sql

        Try
            countresult = cmdi.ExecuteScalar()
            If countresult = 0 Then
                opt = Convert.ToDateTime(acdt)
                op = opt.ToString(df)
            Else
                Dim curdue_period As Date = DateAdd(DateInterval.Month, (countresult), acdt)

                op = curdue_period.ToString("MMMM,yyyy", CultureInfo.InvariantCulture)
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Return op
    End Function

    Function calculate_penalty(ByVal acn As String, ByVal acdt As Date)


        'Dim sql As String = "SELECT roi.penalty,roi.penaltyprd FROM dbo.roi WHERE roi.Product = '" + product + "'"
        'sql &= "AND roi.prddmy = '" + prdtyp + "'"
        'sql &= "AND roi.prdfrm <= " + prd
        'sql &= "AND roi.prdto >= " + prd


        'Dim penalty_ds As New DataSet

        'Dim penalty_adapter As New SqlDataAdapter(sql, con)


        'penalty_adapter.Fill(penalty_ds)

        'If Not penalty_ds.Tables(0).Rows.Count = 0 Then

        '    If days_ago >= CInt(IIf(IsDBNull(penalty_ds.Tables(0).Rows(0).Item(1)), 0, penalty_ds.Tables(0).Rows(0).Item(1))) Then

        '        Dim perhundred As Integer = IIf(IsDBNull(penalty_ds.Tables(0).Rows(0).Item(0)), 0, penalty_ds.Tables(0).Rows(0).Item(0))
        '        Dim penalprd As Integer = IIf(IsDBNull(penalty_ds.Tables(0).Rows(0).Item(1)), 0, penalty_ds.Tables(0).Rows(0).Item(1))

        '        txtpenalty.Text = ((amt * perhundred) / 100) * (Math.Round(days_ago / 30) - 1)
        '        rdpenal.Visible = True
        '    End If


        'End If

        Dim sql As String

        If con1.State = ConnectionState.Closed Then con1.Open()

        If session_user_role = "Audit" Then
            sql = "SELECT COUNT(*) FROM dbo.[actransc] WHERE [actransc].acno ='" + acn + "' AND [actransc].type<>'INTR' "
        Else
            sql = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].type<>'INTR' "
        End If


        cmdi.Connection = con1
        cmdi.CommandText = sql

        countresult = cmdi.ExecuteScalar()

        countresult = IIf(IsDBNull(countresult), 0, countresult)


        Dim rdlate As Integer = DateDiff(DateInterval.Month, acdt, Convert.ToDateTime(txtdue.Text)) - CDbl(countresult)


        Return rdlate

    End Function

    Protected Sub gvout_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvout.PageIndexChanging

        gvout.PageIndex = e.NewPageIndex

        dt = Session("rddue")
        gvout.DataSource = dt
        gvout.DataBind()

    End Sub

    Private Sub btn_minbal_Click(sender As Object, e As EventArgs) Handles btn_minbal.Click
        System.Threading.Thread.Sleep(5000)
        bind_grid()


    End Sub
    Sub disp_new()

        Dim dat As DateTime = DateTime.ParseExact(txtnewfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtnewto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        If session_user_role = "Audit" Then
            query = "SELECT  masterc.date,  masterc.acno,  member.FirstName,  masterc.amount,  masterc.mamt FROM dbo.masterc INNER JOIN dbo.member   ON masterc.cid = member.MemberNo "
            query &= "WHERE CONVERT(VARCHAR(20), masterc.date, 112) BETWEEN @frm AND @to AND masterc.product = @prod and masterc.cld=@cld order by date"
        Else
            query = " SELECT master.date,master.acno,member.FirstName,member.LastName,member.Address,isnull(KYC.PAN,'') as PAN ,master.amount,master.mamt FROM dbo.master"
            query &= " INNER JOIN dbo.member  ON master.cid = member.MemberNo INNER JOIN dbo.KYC ON member.MemberNo = KYC.MemberNo WHERE master.product = @prod"
            query &= " AND CONVERT(VARCHAR(20), master.date, 112) BETWEEN @frm AND @to and master.cld=@cld"

        End If


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
            lblnewmsum.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(mamt)", ""))


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub

    Private Sub btn_new_Click(sender As Object, e As EventArgs) Handles btn_new.Click
        disp_new()

    End Sub

    Sub disp_cld()

        Dim dat As DateTime = DateTime.ParseExact(txtcldfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtcldto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()


        If session_user_role = "Audit" Then
            query = " SELECT MAX(actransc.date) AS date,actransc.acno,member.FirstName,masterc.amount,actransc.Drd FROM dbo.actransc"
            query &= " INNER JOIN dbo.masterc ON actransc.acno = masterc.acno INNER JOIN dbo.member ON masterc.cid = member.MemberNo"
            query &= " WHERE CONVERT(VARCHAR(20), actransc.date, 112) BETWEEN @frm AND @to AND masterc.product = @prd AND masterc.cld = @cld AND actransc.Drd > 0 AND actransc.Type = 'CASH'"
            query &= " GROUP BY actransc.acno,member.FirstName,actransc.Drd,masterc.amount ORDER BY date"

        Else

            query = " SELECT MAX(actrans.date) AS date,actrans.acno,member.FirstName,masterc.amount,actrans.Drd FROM dbo.actrans"
            query &= " INNER JOIN dbo.master ON actrans.acno = master.acno INNER JOIN dbo.member ON master.cid = member.MemberNo"
            query &= " WHERE CONVERT(VARCHAR(20), actrans.date, 112) BETWEEN @frm AND @to AND master.product = @prd AND master.cld = @cld AND actrans.Drd > 0 AND actrans.Type = 'CASH'"
            query &= " GROUP BY actrans.acno,member.FirstName,actrans.Drd,master.amount ORDER BY date"

        End If


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
            lblmcld.Text = String.Format("{0:N}", ds.Tables(0).Compute("sum(drd)", ""))


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub


    Private Sub btn_cld_Click(sender As Object, e As EventArgs) Handles btn_cld.Click
        disp_cld()

    End Sub

    Private Sub btn_sms_Click(sender As Object, e As EventArgs) Handles btn_sms.Click

        Dim mobile As String
        Dim txtmsg As String
        'Dim pName As String = disp.SelectedRow.Cells(1).Text
        'MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)


        For i As Integer = 0 To gvin.Rows.Count - 1
            Dim dt As CheckBox = DirectCast(gvin.Rows(i).Cells(0).FindControl("itmchk"), CheckBox)
            Dim acn As Label = DirectCast(gvin.Rows(i).Cells(0).FindControl("lblsl"), Label)
            Dim dat As Label = DirectCast(gvin.Rows(i).Cells(0).FindControl("lbldat"), Label)


            If dt.Checked Then
                mobile = get_mobile(acn.Text)
                If Not mobile = "" Then
                    'mobile = mobile
                    'txtmsg = "Dear Customer Your Deposit A/c " + acn.Text + " Matured On " + dat.Text + " Please Contact Your Branch."
                    txtmsg = "Your Deposit A/C " + "xxxxxxx" + Right(Trim(acn.Text), 4) + " will be Matured on " + dat.Text + ". Please Contact Branch"
                    Send_sms(mobile, txtmsg, fiscusM.TemplateID.DepositMaturity)
                    txtmsg = ""
                    mobile = ""
                    acn.ForeColor = Drawing.Color.Green
                Else
                    acn.ForeColor = Drawing.Color.Red
                End If

            End If

        Next
    End Sub
    Function get_mobile(ByVal acn As String)
        Dim mobile As String = ""
        If con.State = ConnectionState.Closed Then con.Open()

        If session_user_role = "Audit" Then
            query = "SELECT member.Mobile FROM dbo.masterc INNER JOIN dbo.member   ON masterc.cid = member.MemberNo WHERE masterc.acno = @acn"
        Else
            query = "SELECT member.Mobile FROM dbo.master INNER JOIN dbo.member   ON master.cid = member.MemberNo WHERE master.acno = @acn"
        End If


        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@acn", acn)
        Try
            mobile = cmd.ExecuteScalar()

            If Not IsDBNull(mobile) Then
                If Len(mobile) < 10 Then mobile = ""
            Else
                mobile = ""
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


        Return mobile
    End Function



    Private Sub btn_rd_sms_Click(sender As Object, e As EventArgs) Handles btn_rd_sms.Click

        Dim mobile As String
        Dim txtmsg As String
        'Dim pName As String = disp.SelectedRow.Cells(1).Text
        'MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)
        Try

            For i As Integer = 0 To gvout.Rows.Count - 1
                Dim dt As CheckBox = DirectCast(gvout.Rows(i).Cells(0).FindControl("itmchk"), CheckBox)
                Dim acn As Label = DirectCast(gvout.Rows(i).Cells(0).FindControl("lblsl"), Label)
                Dim due As Label = DirectCast(gvout.Rows(i).Cells(0).FindControl("lblshr"), Label)


                If dt.Checked Then
                    mobile = get_mobile(acn.Text)
                    If Not mobile = "" Then
                        ' mobile = "91" & mobile
                        txtmsg = "Dear Customer, Your RD A/c " + acn.Text + " is late by " + due.Text + " Months. Please pay Immediately"
                        Send_sms(mobile, txtmsg, fiscusM.TemplateID.RDPaymentLate)
                        txtmsg = ""
                        mobile = ""
                        acn.ForeColor = Drawing.Color.Green
                    Else
                        acn.ForeColor = Drawing.Color.Red
                    End If

                End If

            Next

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub


    Private Sub DepositAnalysis_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btnxl_Click(sender As Object, e As EventArgs) Handles btnxl.Click


        'Private Sub isadd_CheckedChanged(sender As Object, e As EventArgs) Handles isadd.CheckedChanged
        '    If isadd.Checked Then
        '        disp_gridadd()
        '        ' btnexport.Visible = True
        '    Else
        '        disp_grid()

        '    End If
        'End Sub

        Response.Clear()

        Response.Buffer = True



        Response.AddHeader("content-disposition", "attachment;filename=NewAccount.xls")

        Response.Charset = ""

        Response.ContentType = "application/vnd.ms-excel"



        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)



        gvnew.AllowPaging = False

        disp_new()





        'Change the Header Row back to white color

        '   disp.HeaderRow.Style.Add("background-color", "#FFFFFF")



        'Apply style to Individual Cells




        For i As Integer = 0 To gvnew.Rows.Count - 1

            Dim row As GridViewRow = gvnew.Rows(i)
        Next

        gvnew.RenderControl(hw)



        'style to format numbers to string

        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"

        Response.Write(style)

        Response.Output.Write(sw.ToString())

        Response.Flush()

        Response.End()

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

End Class