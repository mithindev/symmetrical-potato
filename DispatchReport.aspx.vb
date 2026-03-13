Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization




Public Class DispatchReport
    Inherits System.Web.UI.Page

    Public closed As Integer = 0
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
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

        If Not Page.IsPostBack Then

            '  ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtcid.ClientID), True)
            ' bind_grid()


        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        disp.PageIndex = e.NewPageIndex
        bind_grid()

    End Sub

    Sub bind_grid()

        Dim prod As String = String.Empty

        Select Case Trim(txtproduct.Text)
            Case "JL"
                prod = "JEWEL LOAN"
            Case "DL"
                prod = "DEPOSIT LOAN"

            Case "ML"
                prod = "MORTGAGE LOAN"
            Case "DCL"
                prod = "DAILY COLLECTION LOAN "


        End Select

        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        '  query = "select date,acn,tqty,tnet from jlstock where CONVERT(VARCHAR(20), date, 112) between @frm and @to order by date,acn"

        'query = "SELECT actrans.date,actrans.acno,actrans.Drd,actrans.Due FROM dbo.actrans WHERE actrans.Narration LIKE '%postage%' and CONVERT(VARCHAR(20), date, 112) between @frm and @to order by date,acno"

        query = "SELECT actrans.date, actrans.acno,member.FirstName,actrans.Drd,actrans.Due FROM dbo.actrans LEFT OUTER JOIN dbo.master ON actrans.acno = master.acno "
        query &= " LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo WHERE actrans.Narration LIKE '%postage%' and CONVERT(VARCHAR(20), actrans.date, 112) between @frm and @to and suplimentry=@prod order by actrans.date,actrans.acno"

        cmd.CommandText = query
        cmd.Connection = con
        Dim ds As New DataTable

        Try
            Dim adapter As New SqlDataAdapter(query, con)
            adapter.SelectCommand.Parameters.Clear()
            adapter.SelectCommand.Parameters.AddWithValue("@frm", reformatted)
            adapter.SelectCommand.Parameters.AddWithValue("@to", reformatted1)
            adapter.SelectCommand.Parameters.AddWithValue("@prod", prod)

            adapter.Fill(ds)
            disp.DataSource = ds
            disp.DataBind()

            Dim sum As Object = ds.Compute("sum(drd)", "")

            If Not sum.ToString = "" Then

                lbltotal.Text = FormatCurrency(sum.ToString)
            Else
                lbltotal.Text = "0"
            End If



        Catch ex As Exception
            Response.Write(ex.ToString)

        End Try



    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        bind_grid()

    End Sub


    Private Sub DispatchReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class