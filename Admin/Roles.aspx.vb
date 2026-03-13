Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Public Class Roles
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public dt As New DataTable
    Public newrow As DataRow
    Dim query As String

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        If Not Page.IsPostBack Then
            get_users()
            bind_grid()


            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", usr.ClientID), True)


        End If


    End Sub
    Sub get_users()


        If con.State = ConnectionState.Closed Then con.Open()
        query = "select username from staff order by username"

        cmd.CommandText = query
        cmd.Connection = con

        Try
            Dim ds As New DataSet
            Dim adapter As New SqlDataAdapter(query, con)
            adapter.Fill(ds)


            usr.DataValueField = "username"
            usr.DataTextField = "username"
            usr.DataSource = ds
            usr.DataBind()
            usr.Items.Insert(0, "<-- Select -->")

            usr.Items(0).Value = ""
            usrrole.Items.Insert(0, "<-- Select -->")
            usrrole.Items(0).Value = ""
            usrrole.Items.Insert(1, "Admin")
            usrrole.Items.Insert(2, "Audit")
            usrrole.Items.Insert(3, "Cashier")
            usrrole.Items.Insert(4, "Clerical")

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvin.PageIndex = e.NewPageIndex
        gvin.DataSource = Session("dt")
        gvin.DataBind()

    End Sub
    Sub bind_grid()

        gvin.DataSource = Nothing

        gvin.DataBind()

    End Sub


    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click

        If usr.SelectedIndex = 0 Or usrrole.SelectedIndex = 0 Then Exit Sub

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("username", GetType(String))
            dt.Columns.Add("role", GetType(String))
        End If

        If Session("dt") Is Nothing = False Then
            dt = CType(Session("dt"), DataTable)
        End If
        newrow = dt.NewRow
        newrow(0) = usr.SelectedItem.Text
        newrow(1) = usrrole.SelectedItem.Text
        dt.Rows.Add(newrow)

        gvin.DataSource = dt
        gvin.DataBind()

        Session("dt") = dt
        usr.SelectedIndex = 0
        usrrole.SelectedIndex = 0

    End Sub

    Private Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click

        gvin.DataSource = Nothing
        gvin.DataBind()
        Session("dt") = Nothing
        txtfocus(usr)
    End Sub

    Private Sub btn_up_Click(sender As Object, e As EventArgs) Handles btn_up.Click


        If Session("dt") Is Nothing = False Then
            dt = CType(Session("dt"), DataTable)

            'Dim count As Integer = dt_j.Rows.Count

            For Each row1 As DataRow In dt.Rows

                Dim user As String = row1(0).ToString
                Dim userrole As String = row1(1).ToString
                cmd.Parameters.Clear()

                If con1.State = ConnectionState.Closed Then con1.Open()

                query = "INSERT INTO usr(date,username,roles)"
                query &= " Values(@dt,@user,@role)"

                cmd.Connection = con1
                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@dt", DateAndTime.Today)
                cmd.Parameters.AddWithValue("@user", user)
                cmd.Parameters.AddWithValue("@role", userrole)

                Try

                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    MsgBox(ex.ToString)

                Finally
                    '  con.Close()
                    ' cmd.Dispose()

                End Try

            Next
            chk4backup()

            'Dim webClient As New System.Net.WebClient
            'webClient.DownloadFile("http://bulksms.mysmsmantra.com:8080/WebSMS/balance.jsp?username=Ingenious&password=1574491715", "D:\result.txt")
            ''Session("smsbal") = result


            'Using sr As StreamReader = New StreamReader("d:\result.txt")
            '    Dim n As Integer = 0
            '    Dim ntline As String = Nothing
            '    Do While (sr.Peek() >= 0) And (n <= 8)
            '        sr.ReadLine()
            '        n += 1
            '    Loop
            '    If sr.Peek() >= 0 Then
            '        ntline = sr.ReadLine()
            '    End If
            '    Session("smsbal") = Right(Trim(ntline), 4)
            'End Using

            'If System.IO.File.Exists("d:\result.txt") Then System.IO.File.Delete("d:\result.txt")

            ''Dim bearerToken As String = DirectCast(result.StartsWith(SelectToken("access_token"),



        End If

    End Sub

    Sub chk4backup()

        If con1.State = ConnectionState.Closed Then con1.Open()
        Dim dat As Date = DateTime.ParseExact(DateAndTime.Today.AddDays(-1), "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        query = "select bakup from chklst where CONVERT(VARCHAR(20),date, 112) = @dt"
        cmd.Connection = con1
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@dt", reformatted)
        Try
            Dim rslt As Integer = cmd.ExecuteScalar()
            If rslt = 0 Then
                Session("bdt") = dat
                Response.Redirect("..\backup.aspx")
            Else
                Session("dt") = Nothing
                Response.Redirect("..\admin\dashboard.aspx")

            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub
End Class

