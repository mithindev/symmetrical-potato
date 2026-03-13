Imports System.Data.SqlClient
Imports System.Globalization

Public Class login
    Inherits System.Web.UI.Page
    Dim ncid As String
    Dim filename As String

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ' Initial load logic if any
        End If

    End Sub


    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        Dim reformatted As String = DateTime.Today.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using conLocal As New SqlConnection(connString)
            conLocal.Open()
            Dim queryLogin = " SELECT staff.username,staff.password,[usr].date,[usr].roles,staff.passchangeon FROM dbo.staff"
            queryLogin &= " INNER JOIN dbo.[usr] ON staff.username = [usr].username"
            queryLogin &= " WHERE staff.username = @user COLLATE Latin1_General_CS_AS AND CONVERT(VARCHAR(20), [usr].date, 112) = @dt"

            Using cmdLocal As New SqlCommand(queryLogin, conLocal)
                cmdLocal.Parameters.AddWithValue("@user", txtuser.Text)
                cmdLocal.Parameters.AddWithValue("@dt", reformatted)

                Try
                    Using dr As SqlDataReader = cmdLocal.ExecuteReader()
                        If dr.HasRows Then
                            dr.Read()

                            If txtpass.Text = dr(1).ToString() Then
                                Session("session_user_role") = dr(3)
                                Session("sesusr") = dr(0).ToString.ToUpper
                                session_user_role = dr(3)
                                Session("logtime") = DateTime.Now

                                FormsAuthentication.SetAuthCookie(dr(0).ToString(), False)

                                add2log()

                                If IsDBNull(dr(4)) Then
                                    Response.Redirect("Changepassword.aspx", False)
                                    Context.ApplicationInstance.CompleteRequest()
                                    Exit Sub
                                End If

                                Dim x As Integer = DateTime.Today.CompareTo(CDate(dr(4)).AddDays(30))
                                If x = -1 Then
                                    Dim returnUrl As String = Request.QueryString("ReturnUrl")
                                    If Not String.IsNullOrEmpty(returnUrl) Then
                                        Response.Redirect(returnUrl, False)
                                        Context.ApplicationInstance.CompleteRequest()
                                        Exit Sub
                                    Else
                                        Select Case Session("session_user_role").ToString
                                            Case "Clerical", "Audit"
                                                Response.Redirect("user/userdashboard.aspx", False)
                                            Case "Admin"
                                                Response.Redirect("/admin/dashboard.aspx", False)
                                            Case "Cashier"
                                                Response.Redirect("/user/dashboard-cash.aspx", False)
                                        End Select
                                        Context.ApplicationInstance.CompleteRequest()
                                        Exit Sub
                                    End If
                                Else
                                    Response.Redirect("Changepassword.aspx", False)
                                    Context.ApplicationInstance.CompleteRequest()
                                    Exit Sub
                                End If
                            Else
                                lblerrmsg.Text = "Invalid UserName or Password"
                                lblerrmsg.Visible = True
                                txtfocus(txtuser)
                            End If
                        Else
                            chk4admin()
                        End If
                    End Using
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
        End Using
    End Sub

    Sub chk4admin()
        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        Using conLocal As New SqlConnection(connString)
            conLocal.Open()
            Dim queryAdmin = "select username,password from staff where username=@user and password=@pass and Designation=@desi"
            Using cmdLocal As New SqlCommand(queryAdmin, conLocal)
                cmdLocal.Parameters.AddWithValue("@user", txtuser.Text)
                cmdLocal.Parameters.AddWithValue("@pass", txtpass.Text)
                cmdLocal.Parameters.AddWithValue("@desi", "Manager")

                Try
                    Using dr As SqlDataReader = cmdLocal.ExecuteReader
                        If dr.HasRows Then
                            dr.Close()
                            chk4roles()
                        Else
                            lblerrmsg.Text = "Contact your Admin."
                            lblerrmsg.Visible = True
                            txtfocus(txtuser)
                        End If
                    End Using
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
        End Using
    End Sub

    Sub chk4roles()
        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        Dim reformatted As String = DateTime.Today.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using conLocal As New SqlConnection(connString)
            conLocal.Open()
            Dim queryRoles = "SELECT COUNT(*) AS rc FROM dbo.usr WHERE CONVERT(VARCHAR(20),date, 112) = @dt"
            Using cmdLocal As New SqlCommand(queryRoles, conLocal)
                cmdLocal.Parameters.AddWithValue("@dt", reformatted)
                Try
                    Dim xcount As Integer = Convert.ToInt32(cmdLocal.ExecuteScalar())

                    If xcount = 0 Then
                        Session("session_user_role") = "Admin"
                        Session("sesusr") = txtuser.Text
                        session_user_role = "Admin"
                        Session("logtime") = DateTime.Now
                        FormsAuthentication.SetAuthCookie(txtuser.Text, False)
                        addadmin(txtuser.Text, "Admin")
                        add2log()

                        Response.Redirect("\admin\roles.aspx", False)
                        Context.ApplicationInstance.CompleteRequest()
                        Exit Sub
                    Else
                        lblerrmsg.Text = "Contact your Admin."
                        lblerrmsg.Visible = True
                        txtfocus(txtuser)
                    End If
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
        End Using
    End Sub

    Function chk4Login() As Boolean
        Dim isAllowed As Boolean = False
        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        Using conLocal As New SqlConnection(connString)
            conLocal.Open()
            Dim queryCheck = "select login,sessioncld from seslog where sesusr=@sesusr and sessioncld=0 ORDER BY LOGIN DESC"
            Using cmdLocal As New SqlCommand(queryCheck, conLocal)
                cmdLocal.Parameters.AddWithValue("@sesusr", Trim(txtuser.Text))

                Using reader As SqlDataReader = cmdLocal.ExecuteReader()
                    If reader.HasRows Then
                        isAllowed = False
                    Else
                        isAllowed = True
                    End If
                End Using
            End Using
        End Using

        Return isAllowed
    End Function


    Sub add2log()
        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        Using conLocal As New SqlConnection(connString)
            conLocal.Open()
            Dim queryLog = "INSERT INTO seslog(sesusr,role,login) Values(@user,@role,@log)"
            Using cmdLocal As New SqlCommand(queryLog, conLocal)
                cmdLocal.Parameters.AddWithValue("@user", Session("sesusr").ToString)
                cmdLocal.Parameters.AddWithValue("@role", Session("session_user_role").ToString)
                cmdLocal.Parameters.AddWithValue("@log", CType(Session("logtime"), DateTime))

                Try
                    cmdLocal.ExecuteNonQuery()
                Catch ex As Exception
                    ' Log error appropriately if needed
                    ' MsgBox is not recommended for web apps
                End Try
            End Using
        End Using
    End Sub
    
    Sub addadmin(ByVal usr As String, ByVal rol As String)
        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        Using conLocal As New SqlConnection(connString)
            conLocal.Open()
            Dim queryAddAdmin = "INSERT INTO usr(date,username,roles) Values(@dt,@user,@role)"
            Using cmdLocal As New SqlCommand(queryAddAdmin, conLocal)
                cmdLocal.Parameters.AddWithValue("@dt", DateAndTime.Now)
                cmdLocal.Parameters.AddWithValue("@user", usr)
                cmdLocal.Parameters.AddWithValue("@role", rol)

                Try
                    cmdLocal.ExecuteNonQuery()
                Catch ex As Exception
                    ' Log error appropriately if needed
                    ' MsgBox is not recommended for web apps
                End Try
            End Using
        End Using
    End Sub


End Class