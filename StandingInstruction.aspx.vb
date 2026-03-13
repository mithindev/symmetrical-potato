Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services

Public Class StandingInstruction
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public dt As New DataTable
    Public newrow As DataRow
    Dim query As String


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString '"Data Source=.\SQLEXPRESS;Initial Catalog=fiscusdb;integrated Security = true;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        con.Open()

        If Not Page.IsPostBack Then

            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtsiacn.ClientID), True)



        End If


    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvin.PageIndex = e.NewPageIndex
        bind_grid()

    End Sub
    Sub bind_grid()

        If con.State = ConnectionState.Closed Then con.Open()

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("day", GetType(Integer))
            dt.Columns.Add("frm", GetType(String))
            dt.Columns.Add("to", GetType(String))
            dt.Columns.Add("amt", GetType(Decimal))

        End If
        If txtdayd.Text = "" And txtpro.Text = "" Then
            query = "select srcproduct,acno,siacno,sidate,amount from stdins where cld='0' order by sidate"
        ElseIf txtdayd.Text <> "" And txtpro.Text = "" Then
            query = "select srcproduct,acno,siacno,sidate,amount from stdins where sidate ='" + CDbl(txtdayd.Text) + "' and cld='0' order by sidate"
        ElseIf txtdayd.Text <> "" And txtpro.Text <> "" Then
            query = "select srcproduct,acno,siacno,sidate,amount from stdins where sidate ='" + txtdayd.Text + "' and srcproduct='" + Trim(txtpro.Text) + "' and cld='0' order by sidate"
        End If

        cmd.Connection = con
        cmd.CommandText = query
        Try
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                While dr.Read
                    newrow = dt.NewRow
                    If txtpro.Text = "FD" Then
                        newrow(0) = dr(3)
                        newrow(1) = dr(1)
                        newrow(2) = dr(2)
                        newrow(3) = IIf(IsDBNull(dr(4)), 0, dr(4))
                    Else
                        newrow(0) = dr(3)
                        newrow(1) = dr(2)
                        newrow(2) = dr(1)
                        newrow(3) = IIf(IsDBNull(dr(4)), 0, dr(4))
                    End If
                    dt.Rows.Add(newrow)
                End While
            End If

            gvin.DataSource = dt
            gvin.DataBind()
            dr.Close()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub btn_si_update_Click(sender As Object, e As EventArgs) Handles btn_si_update.Click

        update_si()


    End Sub


    Sub update_si()

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select sidate from stdins where acno=@src and siacno=@des"
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@src", txtsiacn.Text)
        cmd.Parameters.AddWithValue("@des", txtsbacn.Text)

        Try
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then

                query = "UPDATE stdins SET srcproduct=@srcp,acno=@src,product=@desp,siacno=@des,sidate=@sidate,amount=@amt where acno=@src and siacno=@des"
            Else
                query = "insert into stdins(srcproduct,acno,product,siacno,sidate,amount)"
                query &= "values(@srcp,@src,@desp,@des,@sidate,@amt)"

            End If
            dr.Close()

            cmd.Parameters.Clear()

            Dim src As String = Mid(txtsiacn.Text, 1, 2)

            If src = "75" Then
                src = "FD"
            ElseIf src = "77" Then
                src = "RD"
            End If


            Dim des As String = Mid(txtsbacn.Text, 1, 2)


            If des = "79" Then
                des = "SB"
            Else
                Exit Sub
            End If
            cmd.Connection = con
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@srcp", src)
            cmd.Parameters.AddWithValue("@src", txtsiacn.Text)
            cmd.Parameters.AddWithValue("@desp", des)
            cmd.Parameters.AddWithValue("@des", txtsbacn.Text)
            cmd.Parameters.AddWithValue("@sidate", txtday.Text)
            cmd.Parameters.AddWithValue("@amt", txtamt.Text)

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try


            Dim stitle As String = "Standing Instruction"
            Dim msg As String = "Standing Instruction updated for " + txtsiacn.Text
            Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)



            clea()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Sub

    Sub clea()
        txtsiacn.Text = ""
        txtsbacn.Text = ""
        cust.Text = ""
        txtday.Text = ""
        txtamt.Text = ""
        btn_si_can.Visible = False
        txtfocus(txtsiacn)
    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        bind_grid()

    End Sub

    Private Sub txtsiacn_TextChanged(sender As Object, e As EventArgs) Handles txtsiacn.TextChanged
        If con.State = ConnectionState.Closed Then con.Open()

        query = "select acno,siacno,sidate,COALESCE(stdins.amount,0) from stdins where acno=@src"

        cmd.Connection = con
        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@src", Trim(txtsiacn.Text))
        Dim dr As SqlDataReader
        dr = cmd.ExecuteReader()
        If dr.HasRows() Then
            dr.Read()
            txtsbacn.Text = dr(1)
            txtday.Text = dr(2)
            txtamt.Text = dr(3)
            btn_si_can.Visible = True
        End If

        dr.Close()


    End Sub

    Private Sub btn_si_can_Click(sender As Object, e As EventArgs) Handles btn_si_can.Click
        If con.State = ConnectionState.Closed Then con.Open()

        query = " DELETE FROM stdins WHERE acno = @acn"

        cmd.Connection = con
        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@acn", txtsiacn.Text)

        cmd.ExecuteNonQuery()

        Dim stitle As String = "Standing Instruction"
        Dim msg As String = "Standing Instruction Deleted for " + txtsiacn.Text
        Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)




        clea()


    End Sub

    Private Sub StandingInstruction_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class