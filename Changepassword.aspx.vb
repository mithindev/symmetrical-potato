Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Imports System.Web.Security

Public Class Changepassword
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub
    Protected Sub signout(sender As Object, e As EventArgs)

        FormsAuthentication.SignOut()

        Session.Abandon()
        Log_out(Session("logtime").ToString, Session("sesusr"))
        Response.Redirect("login.aspx")

    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then

            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtpass.ClientID), True)
            txtuser.Text = Session("sesusr")
        End If






    End Sub


    Private Sub btn_cp_Click(sender As Object, e As EventArgs) Handles btn_cp.Click
        Dim dat As Date = DateTime.ParseExact(DateAndTime.Today, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        ' query = " SELECT staff.username,staff.password,[user].date,[user].roles,staff.passchangeon FROM dbo.staff"
        ' query &= " INNER JOIN dbo.[user] ON staff.username = [user].username"
        ' query &= " WHERE staff.username = @user AND CONVERT(VARCHAR(20), [user].date, 112) = @dt"

        query = "update staff set firstlogin=@firstlogin, password=@user,passchangeon=@dt where staff.username='" + txtuser.Text + "'"

        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@user", txtpass.Text)
        cmd.Parameters.AddWithValue("@dt", DateAndTime.Today.AddDays(30))
        cmd.Parameters.AddWithValue("@firstlogin", 0)


        Try
            If Not Trim(txtpass.Text) = "" Then
                If txtcpass.Text = txtpass.Text Then
                    cmd.ExecuteNonQuery()
                    Response.Redirect("login.aspx")
                Else
                    lblerrmsg.Text = "Password Should Match"
                    lblerrmsg.Visible = True
                    txtfocus(txtpass)
                End If
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub

    Private Sub btn_clr_Click(sender As Object, e As EventArgs) Handles btn_clr.Click
        txtpass.Text = ""
        txtcpass.Text = ""
        txtfocus(txtpass)
    End Sub
End Class