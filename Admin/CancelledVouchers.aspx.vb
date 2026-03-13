Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization

Public Class CancelledVouchers

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
    Public dv As DataView
    Protected Sub signout(sender As Object, e As EventArgs)

        FormsAuthentication.SignOut()

        Session.Abandon()
        Log_out(Session("logtime").ToString, Session("sesusr"))
        Response.Redirect("..\login.aspx")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString '"Data Source=.\SQLEXPRESS;Initial Catalog=fiscusdb;integrated Security = true;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true"
        con.Open()


        If Not Page.IsPostBack Then


            '   bind_grid()

        End If

    End Sub

    Protected Sub OnPagingnew(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvnew.PageIndex = e.NewPageIndex
        bind_grid()

    End Sub


    Sub bind_grid()

        Dim dat As DateTime = DateTime.ParseExact(txtnewfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtnewto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Close()
        query = "select date,tid,vtype,achead,amount,rejectby from reject where CONVERT(VARCHAR(20), date, 112) BETWEEN @frm AND @to "

        Try
            Dim ds As New DataSet

            Dim adapter As New SqlDataAdapter(query, con)
            adapter.SelectCommand.Parameters.Clear()
            adapter.SelectCommand.Parameters.AddWithValue("@frm", reformatted)
            adapter.SelectCommand.Parameters.AddWithValue("@to", reformattedto)

            adapter.Fill(ds)
            gvnew.DataSource = ds
            gvnew.DataBind()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Sub

    Private Sub btn_new_Click(sender As Object, e As EventArgs) Handles btn_new.Click
        bind_grid()

    End Sub

    Private Sub CancelledVouchers_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class