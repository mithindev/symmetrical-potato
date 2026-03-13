Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging




Public Class ShareHolders
    Inherits System.Web.UI.Page

    Public closed As Integer = 0
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet

    Dim query As String
    Protected Sub signout(sender As Object, e As EventArgs)

        FormsAuthentication.SignOut()

        Session.Abandon()
        Log_out(Session("logtime").ToString, Session("sesusr"))
        Response.Redirect("..\login.aspx")

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then

            '  ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtcid.ClientID), True)
            bind_grid()


        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        disp.PageIndex = e.NewPageIndex
        bind_grid()

    End Sub
    Sub bind_grid()

        If con.State = ConnectionState.Closed Then con.Open()

        Dim ds As New DataTable

        ' query = "SELECT   share.date,share.sl,member.FirstName,member.LastName,member.Address,share.shrval,share.allocation FROM dbo.share INNER JOIN dbo.member   ON share.cid = member.MemberNo ORDER BY share.sl"hare

        If Session("sesusr") = "RAM KUMAR" Then
            query = "SELECT   share.cid,member.FirstName,SUM(share.allocation) AS allocation, SUM(share.shrval) AS shrval FROM dbo.share INNER JOIN dbo.member  ON share.cid = member.MemberNo GROUP BY share.cid, member.FirstName Order by share.cid"
        Else
            If Trim(txtfrm.Text) <> "" And Trim(txtto.Text) <> "" Then
                query = "SELECT   share.cid,member.FirstName,SUM(share.allocation) AS allocation, SUM(share.shrval) AS shrval FROM dbo.share  INNER JOIN dbo.member  ON share.cid = member.MemberNo WHERE share.showall = 0 and allocation between " + txtfrm.Text + " and " + txtto.Text + " GROUP BY share.cid, member.FirstName Order by share.cid"
            Else


                query = "SELECT   share.cid,member.FirstName,SUM(share.allocation) AS allocation, SUM(share.shrval) AS shrval FROM dbo.share  INNER JOIN dbo.member  ON share.cid = member.MemberNo WHERE share.showall = 0  GROUP BY share.cid, member.FirstName Order by share.cid"
            End If
        End If


        Try
            Dim adapter As New SqlDataAdapter(query, con)
            adapter.Fill(ds)
            disp.DataSource = ds
            disp.DataBind()
            Dim sum As Object = ds.Compute("sum(allocation)", "")

            If Not sum.ToString = "" Then

                lblshrnos.Text = sum.ToString
            Else
                lblshrnos.Text = "0"
            End If


            sum = ds.Compute("sum(shrval)", "")

            If Not sum.ToString = "" Then

                lblnetshrval.Text = FormatCurrency(sum.ToString)
            Else
                lblnetshrval.Text = "0.00"
            End If


        Catch ex As Exception

            Response.Write(ex.ToString)

        End Try

    End Sub


    Private Sub ShareHolders_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub txtto_TextChanged(sender As Object, e As EventArgs) Handles txtto.TextChanged

        bind_grid()


    End Sub

    Private Sub txtfrm_TextChanged(sender As Object, e As EventArgs) Handles txtfrm.TextChanged
        txtto.Focus()

    End Sub



    Private Sub btn_prnt_Click(sender As Object, e As EventArgs) Handles btn_prnt.Click
        disp.AllowPaging = False
        bind_grid()
        'ExportToExcel(sender, e)

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "printshare", "printshare()", True)

    End Sub

    Private Sub btn_xl_Click(sender As Object, e As EventArgs) Handles btn_xl.Click
        disp.AllowPaging = False
        bind_grid()

        ExportToExcel(sender, e)
    End Sub


    Protected Sub ExportToExcel(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Report.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            'To Export all pages
            '   disp.AllowPaging = False
            '   bind_grid()


            disp.HeaderRow.BackColor = Color.White
            For Each cell As TableCell In disp.HeaderRow.Cells
                cell.BackColor = disp.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In disp.Rows
                row.BackColor = Color.White
                For Each cell As TableCell In row.Cells
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = disp.AlternatingRowStyle.BackColor
                    Else
                        cell.BackColor = disp.RowStyle.BackColor
                    End If
                    cell.CssClass = "textmode"
                Next
            Next

            disp.RenderControl(hw)
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "printshare", "printshare()", True)

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
End Class