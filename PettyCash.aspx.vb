Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization

Public Class PettyCash
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Dim newrow As DataRow
    Public vtype As String = ""
    Dim countresult As Integer
    Public dt_dl As New DataTable
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim yearpart As String


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        con.Open()



        If Not Page.IsPostBack Then

            txtcr.Text = 0
            txtdr.Text = 0
            txtdate.Text = Date.Today
            bind_grid()


            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtdate.ClientID), True)

        End If

    End Sub

    Public Function get_balance(ByVal acn As String)

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()


        Dim x As Integer = 1
        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno =@acno and scroll =@scroll GROUP BY [actrans].acno"
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@scroll", x)
        ' Dim sql As String = "select top 1 cbal,dbal from dbo.[actrans] where [actrans].acno='" + acn + "' ORDER BY date DESC"
        Dim ds_bal As New DataSet

        '        Dim Adapter As New SqlDataAdapter(sql, con)
        '       Adapter.Fill(ds_bal)
        cmd.Connection = con
        cmd.CommandText = sql

        Dim reader As SqlDataReader = cmd.ExecuteReader()


        If reader.HasRows Then
            reader.Read()
            Session("dbal") = reader(1).ToString - reader(0).ToString
            Session("cbal") = reader(3).ToString - reader(2).ToString



        Else
            Session("cbal") = 0
            Session("dbal") = 0
        End If

        reader.Close()

        Return Session("dbal")
    End Function
    Sub bind_grid()
        Dim ds As New DataSet
        Dim ds1 As New DataSet

        If con.State = ConnectionState.Closed Then con.Open()


        Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        cmd.Parameters.Clear()

        Dim qResult As Integer = 0

        ' Dim query1 As String = "select tid,product,acno,dr,cr from diff where CONVERT(VARCHAR(20), date, 112) ='" + reformatted + "' order by tid"
        Dim query1 As String = " SELECT diff.tid,diff.product,diff.acno,member.FirstName,diff.Dr,diff.cr FROM dbo.diff LEFT OUTER JOIN dbo.master  ON diff.acno = master.acno "
        query1 &= "INNER JOIN dbo.member   ON master.cid = member.MemberNo WHERE CONVERT(VARCHAR(20), diff.date, 112)='" + reformatted + "' order by tid"
        ''
        cmd.Connection = con
        'cmd.CommandText = query

        Try

            Dim adapter As New SqlDataAdapter(query1, con)

            adapter.Fill(ds)
            disp.DataSource = ds
            disp.DataBind()



        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()

        End Try


        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()


        query = "select SUM(isnull(cast(diff.dr as float),0)) AS dr,SUM(isnull(cast(diff.cr as float),0)) AS cr from diff where CONVERT(VARCHAR(20), date, 112) ='" + reformatted + "'"

        Dim reader As SqlDataReader


        cmd.CommandText = query
        cmd.Connection = con

        Try
            reader = cmd.ExecuteReader()


            If reader.HasRows() Then
                reader.Read()
                lbl_sum_cr.Text = String.Format("{0:N}", IIf(IsDBNull(reader(1)), 0, reader(1)))
                lbl_sum_dr.Text = String.Format("{0:N}", IIf(IsDBNull(reader(0)), 0, reader(0)))

            End If

        Catch ex As Exception

            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()

        End Try


        Dim x As Double = 0

        x = CDbl(lbl_sum_cr.Text) - CDbl(lbl_sum_dr.Text)


        If x < 0 Then

            lbl_bal_cr.Text = String.Format("{0:N}", -x)
            lbltrf.Text = FormatCurrency(-x)

        Else
            lbl_bal_dr.Text = String.Format("{0:N}", x)
            lbltrf.Text = FormatCurrency(x)


        End If


        If Not CInt(x) = 0 Then
            '  diff_tans.Visible = True
            txtfocus(sbacno)
        Else
            ' diff_tans.Visible = False
        End If


    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        disp.PageIndex = e.NewPageIndex

        'If disp.PageIndex = 0 Then

        '    total = pgtot(disp.PageIndex).ToString
        'Else

        '    total = pgtot(disp.PageIndex - 1).ToString
        'End If


        bind_grid()
    End Sub

    Private Sub txtdate_TextChanged(sender As Object, e As EventArgs) Handles txtdate.TextChanged

    End Sub
    Function get_cust(ByVal acn As String)
        Dim custname As String = ""
        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Parameters.Clear()

        query = "SELECT member.FirstName,member.memberno FROM dbo.master LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo WHERE master.acno = @acno"
        cmdi.Connection = con
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acno", acn)

        Try

            Using reader As SqlDataReader = cmdi.ExecuteReader
                If reader.HasRows Then
                    reader.Read()
                    custname = IIf(IsDBNull(reader(0)), "", reader(0).ToString)
                    Session("cid") = IIf(IsDBNull(reader(1)), "", reader(1).ToString)
                End If
            End Using




        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Return custname

    End Function

    Private Sub btn_trans_Click(sender As Object, e As EventArgs) Handles btn_trans.Click

        If session_user_role = "Audit" Then Exit Sub



        If Trim(lbl_bal_cr.Text) = "" Then
            ' Session("diff_to_sb") = lbl_bal_dr.Text
            '    Response.Redirect("depositpayment.aspx")
            ' Server.Transfer("depositpayment.aspx", True)
            ' txtamt.Text = lbl_bal_dr.Text
            vtype = "RECEIPT"
            lblcpt.Text = "RECEIPT"

        Else
            'Session("diff_to_sb") = lbl_bal_cr.Text
            'Response.Redirect("depositreceipt.aspx")
            'Server.Transfer("depositreceipt.aspx", True)
            ' txtamt.Text = lbl_bal_cr.Text
            vtype = "PAYMENT"
            lblcpt.Text = "PAYMENT"
        End If





        Dim query As String = String.Empty


        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con


        ' Dim d As String = get_due(txtacn.Text, 0)

        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal,denom)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal,@denom)"

        Dim prod = "SAVINGS DEPOSIT"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@acno", sbacno.Text)
        If vtype = "RECEIPT" Then
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@narration", "By Cash")
        Else
            cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Cash")
        End If

        cmd.Parameters.AddWithValue("@due", " ")
        cmd.Parameters.AddWithValue("@type", "CASH")
        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)
        cmd.Parameters.AddWithValue("@denom", 1)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()


        End Try





        query = ""


        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.Parameters.Clear()
        query = "SELECT MAX(ID) from dbo.trans where trans.acno='" + sbacno.Text + "'"
        cmd.CommandText = query


        Try
            countresult = cmd.ExecuteScalar()

            Session("tid") = Convert.ToString(countresult)
        Catch ex As Exception

            Response.Write(ex.ToString)
        End Try





        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con


        ' Dim d As String = get_due(txtacn.Text, 0)

        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        ''Dim prod = "SAVINGS DEPOSIT"
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@acno", sbacno.Text)
        If vtype = "RECEIPT" Then
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@narration", "By Cash")
        Else
            cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Cash")
        End If

        cmd.Parameters.AddWithValue("@due", " ")
        cmd.Parameters.AddWithValue("@type", "CASH")
        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()


        End Try

        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con




        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        ''Dim prod = "SAVINGS DEPOSIT"
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@acno", sbacno.Text)
        If vtype = "RECEIPT" Then
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@narration", "By Cash")
        Else
            cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", CDbl(txtamt.Text))
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Cash")
        End If

        cmd.Parameters.AddWithValue("@due", " ")
        cmd.Parameters.AddWithValue("@type", "CASH")
        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()


        End Try





        update_suplementry(CType(Session("tid"), Integer), prod, txtamt.Text)

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con

        cmd.Parameters.Clear()
        query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
        query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@product", "SB")
        cmd.Parameters.AddWithValue("@acno", sbacno.Text)

        If vtype = "PAYMENT" Then
            cmd.Parameters.AddWithValue("dr", 0)
            cmd.Parameters.AddWithValue("cr", CDbl(txtamt.Text))
        Else
            cmd.Parameters.AddWithValue("cr", 0)
            cmd.Parameters.AddWithValue("dr", CDbl(txtamt.Text))

        End If

        Try

            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally

            cmd.Dispose()
            con.Close()
        End Try

        'prepare_print()


        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transacion ID : <b>" + Session("tid") + "</b>"
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)





        pvno.Text = Session("tid")
        pdate.Text = txtdate.Text
        pacno.Text = sbacno.Text
        pglh.Text = prod
        pamt.Text = FormatCurrency(txtamt.Text)
        pcname.Text = get_cust(sbacno.Text)
        pcid.Text = Session("cid")
        paiw.Text = get_wrds(txtamt.Text)
        premit.Text = pcname.Text
        pbranch.Text = get_home()

        pnltrf.Style.Add("display", "none")
        pnlpr.Style.Add("display", "block")

        clear_tab()

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
    Sub clear_tab()

        'diff_tans.Visible = False
        lblacbal.Text = ""
        lblname.Text = ""
        txtamt.Text = ""
        sbacno.Text = ""
        bind_grid()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)

    End Sub
    Private Sub update_suplementry(ByVal tid As Integer, ByVal ach As String, ByVal cr As Double)



        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            Log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("..\login.aspx")

        End If
        If tid = 0 Then Exit Sub
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@transid", Session("tid"))
        cmd.Parameters.AddWithValue("@achead", ach)
        If vtype = "RECEIPT" Then
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", sbacno.Text)
            cmd.Parameters.AddWithValue("@nar", "By Cash")
            cmd.Parameters.AddWithValue("@typ", "RECEIPT")
        Else
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@acn", sbacno.Text)
            cmd.Parameters.AddWithValue("@nar", "To Cash")
            cmd.Parameters.AddWithValue("@typ", "PAYMENT")

        End If
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)


        End Try

        query = ""


        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"

        cmd.CommandText = query
        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@transid", Session("tid"))
        cmd.Parameters.AddWithValue("@achead", ach)
        If vtype = "RECEIPT" Then
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", sbacno.Text)
            cmd.Parameters.AddWithValue("@nar", "By Cash")
            cmd.Parameters.AddWithValue("@typ", "RECEIPT")
        Else
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@acn", sbacno.Text)
            cmd.Parameters.AddWithValue("@nar", "To Cash")
            cmd.Parameters.AddWithValue("@typ", "PAYMENT")

        End If
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()



        End Try

        query = ""


    End Sub



    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        lbl_bal_cr.Text = ""
        lbl_bal_dr.Text = ""
        lbl_sum_cr.Text = ""
        lbl_sum_dr.Text = ""
        bind_grid()

    End Sub


    Function get_name(ByVal acn As String)
        Dim fname As String = String.Empty


        query = "SELECT  member.FirstName FROM dbo.master INNER JOIN dbo.member  ON master.cid = member.MemberNo WHERE master.acno = @acno"

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con

        cmd.Parameters.Clear()

        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@acno", acn)

        Try

            fname = cmd.ExecuteScalar()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Return fname



    End Function


    Private Sub btn_add_diff_Click(sender As Object, e As EventArgs) Handles btn_add_diff.Click

        If session_user_role = "Audit" Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()

        If txtcr.Text = "" Then txtcr.Text = 0
        If txtdr.Text = "" Then txtdr.Text = 0

        cmd.Connection = con

        cmd.Parameters.Clear()
        query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
        query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@tid", 0)
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@product", txtac.Text)
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("dr", CDbl(txtdr.Text))
        cmd.Parameters.AddWithValue("cr", CDbl(txtcr.Text))



        Try

            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally

            cmd.Dispose()
            con.Close()


        End Try

        txtac.Text = ""
        txtacn.Text = ""
        txtname.Text = ""
        txtdr.Text = ""
        txtcr.Text = ""
        '  add_entry.Visible = False
        bind_grid()

    End Sub



    Private Sub sbacno_TextChanged(sender As Object, e As EventArgs) Handles sbacno.TextChanged


        lblname.Text = get_name(Trim(sbacno.Text))
        lblacbal.Text = FormatCurrency(get_balance(Trim(sbacno.Text)))

        If Trim(lbl_bal_cr.Text) = "" Then
            ' Session("diff_to_sb") = lbl_bal_dr.Text
            '    Response.Redirect("depositpayment.aspx")
            ' Server.Transfer("depositpayment.aspx", True)
            txtamt.Text = lbl_bal_dr.Text
        Else
            'Session("diff_to_sb") = lbl_bal_cr.Text
            'Response.Redirect("depositreceipt.aspx")
            'Server.Transfer("depositreceipt.aspx", True)
            txtamt.Text = lbl_bal_cr.Text
        End If

        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtamt.ClientID)

    End Sub

    Private Sub PettyCash_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btntog_Click(sender As Object, e As EventArgs) Handles btntog.Click


        pnltrf.Style.Add("display", "block")
        pnlpr.Style.Add("display", "none")



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        clear_tab()

    End Sub
End Class