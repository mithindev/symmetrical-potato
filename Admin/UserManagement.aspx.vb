Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization



Public Class UserManagement
    Inherits System.Web.UI.Page

    Public closed As Integer = 0
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet

    Dim query As String

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = "Data Source=.\SQLEXPRESS;Initial Catalog=fiscusdb;integrated Security = true;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        con.Open()

        If Not Page.IsPostBack Then

            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtfn.ClientID), True)
            alertmsg.Visible = False

            bind_grid()
        End If

    End Sub

    Private Sub txtdob_TextChanged(sender As Object, e As EventArgs) Handles txtdob.TextChanged
        '   txtfocus(txtphone)
    End Sub
    Protected Sub OnPagingout(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        '  gvout.PageIndex = e.NewPageIndex
        get_log()

    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        ' disp.PageIndex = e.NewPageIndex

        'If disp.PageIndex = 0 Then

        '    total = pgtot(disp.PageIndex).ToString
        'Else

        '    total = pgtot(disp.PageIndex - 1).ToString
        'End If


        bind_grid()
    End Sub

    Sub bind_grid()

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select firstname,designation,username,lastlogin,loggedAs from staff"

        Dim ds As New DataTable

        Try
            Dim adapter As New SqlDataAdapter(query, con)
            adapter.Fill(ds)
            '  disp.DataSource = ds
            '  disp.DataBind()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



        If con.State = ConnectionState.Closed Then con.Open()


        Dim dr As SqlDataReader


        query = "select username from staff"

        cmd.Connection = con
        cmd.CommandText = query

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                Do While dr.Read()

                    ''  lstusr.Items.Add(Trim(dr(0).ToString))
                Loop


            End If

            'lstusr.Items.Insert(0, "<-- Select -->")
            'lstusr.Items.Item(0).Value = ""
            dr.Close()


        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()

            con.Close()


        End Try



    End Sub
    Private Sub btnclr_Click(sender As Object, e As EventArgs) Handles btnclr.Click
        txtfn.Text = ""
        txtln.Text = ""
        txtadd.Text = ""
        txtdob.Text = ""
        txtdoj.Text = ""
        gender.SelectedItem.Text = "<--Select-->"
        ' txtphone.Text = ""
        txtmobile.Text = ""
        txtemail.Text = ""
        txtdesi.SelectedItem.Text = "<--Select-->"
        txtuser.Text = ""
        txtpass.Text = ""
        txtcpass.Text = ""
        'txtfocus(txtfn)
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtfn.ClientID)
        alertmsg.Visible = False

    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click

        If con.State = ConnectionState.Closed Then con.Open()
        
        ' First, check if username already exists
        Dim oresult As String = ""
        If Not txtuser.Text = "" Then
            query = "select username from staff where username='" + txtuser.Text + "'"
            Dim checkCmd As New SqlCommand(query, con)
            Try
                oresult = checkCmd.ExecuteScalar()

                If Not oresult Is Nothing AndAlso Not oresult.ToString() = "" Then
                    alertmsg.Visible = True
                    lblinfo.Text = "Error: User Name already exists."
                    
                    txtuser.Text = ""
                    txtfocus(txtuser)
                    Return ' Exit the Sub completely so we don't insert duplicate
                End If
            Catch ex As Exception
                Response.Write(ex.ToString)
                Return
            End Try
        End If

        query = "insert into staff(FirstName,LastName, address,gender,dob,phone,mobile,email,Designation,doj,username,password,firstlogin)"
        query &= "values(@FirstName,@LastName, @address,@gender,@dob,@phone,@mobile,@email,@Designation,@doj,@username,@password,@firstlogin)"
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@FirstName", txtfn.Text)
        cmd.Parameters.AddWithValue("@LastName", txtln.Text)
        cmd.Parameters.AddWithValue("@address", txtadd.Text)
        cmd.Parameters.AddWithValue("@gender", gender.SelectedItem.Text)
        If Trim(txtdob.Text) = "" Then
            cmd.Parameters.AddWithValue("@dob", DBNull.Value)

        Else

            cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(txtdob.Text))
        End If
        cmd.Parameters.AddWithValue("@phone", "")
        cmd.Parameters.AddWithValue("@mobile", txtmobile.Text)
        cmd.Parameters.AddWithValue("@email", txtemail.Text)
        cmd.Parameters.AddWithValue("@Designation", txtdesi.SelectedItem.Text)
        If Trim(txtdoj.Text) = "" Then
            cmd.Parameters.AddWithValue("@doj", DBNull.Value)

        Else

            cmd.Parameters.AddWithValue("@doj", Convert.ToDateTime(txtdoj.Text))
        End If

        cmd.Parameters.AddWithValue("@username", txtuser.Text)
        cmd.Parameters.AddWithValue("@password", txtpass.Text)
        cmd.Parameters.AddWithValue("@firstlogin", 1)


        Try
            cmd.ExecuteNonQuery()

            alertmsg.Visible = True
            lblinfo.Text = "User Account Created Successfully!"
            
            btnclr_Click(sender, e)
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub

    ' txtuser_TextChanged removed to stop duplicate network calls
    'Sub disp_staff()
    '    Dim ds As New DataSet
    '    Dim adapter As New SqlDataAdapter
    '    Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
    '    Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
    '    If con.State = ConnectionState.Closed Then con.Open()
    '    cmd.Connection = con
    '    query = "select username,roles from usr where date=@dt"
    '    cmd.CommandText = query
    '    Try

    '        adapter = New SqlDataAdapter(query, con)
    '        adapter.SelectCommand.Parameters.Clear()
    '        adapter.SelectCommand.Parameters.AddWithValue("@dt", reformatted)
    '        adapter.Fill(ds)
    '        gvin.DataSource = ds
    '        gvin.DataBind()

    '    Catch ex As Exception
    '        Response.Write(ex.ToString)
    '    End Try

    'End Sub
    'Private Sub btn_inw_Click(sender As Object, e As ImageClickEventArgs) Handles btn_inw.Click

    '    disp_staff()

    'End Sub
    Sub get_log()

        Dim newrow As DataRow
        Dim dt As New DataTable
        Dim entyat As String
        Dim acno As String = ""
        Dim amt As Double = 0
        Dim type As String = ""
        Dim achead As String = ""

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("entryat", GetType(String))
            dt.Columns.Add("acno", GetType(String))
            dt.Columns.Add("type", GetType(String))
            dt.Columns.Add("achead", GetType(String))
            dt.Columns.Add("amt", GetType(Double))
        End If


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        query = "select cast(entryat as time) ,acno,drd,crd,narration,suplimentry from translog where date=@dt and sesusr=@usr"
        ' Dim dat As DateTime = DateTime.ParseExact(txtlogdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        '  Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        cmd.CommandText = query
        cmd.Parameters.Clear()
        ' cmd.Parameters.AddWithValue("@dt", reformatted)
        'cmd.Parameters.AddWithValue("@usr", lstusr.SelectedItem.Text)
        Try
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            If dr.HasRows Then
                While dr.Read()

                    entyat = Left(dr(0).ToString, 8)
                    acno = dr(1)
                    amt = IIf(dr(2) = 0, dr(3), dr(2))
                    achead = dr(5)
                    Select Case dr(4)
                        Case "TO CASH"
                            type = "PAYMENT"
                        Case "BY CASH"
                            type = "RECEIPT"
                        Case "TO INTEREST"
                            type = "PAYMENT"
                        Case "BY INTEREST"
                            type = "RECEIPT"
                        Case "By Transfer"
                            type = "RECEIPT"
                        Case "To Transfer"
                            type = "RECEIPT"
                    End Select

                    newrow = dt.NewRow
                    newrow(0) = entyat
                    newrow(1) = acno
                    newrow(2) = type
                    newrow(3) = achead
                    newrow(4) = amt
                    dt.Rows.Add(newrow)
                End While

                '     gvout.DataSource = dt
                '    gvout.DataBind()


            End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Sub

    Private Sub UserManagement_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class
