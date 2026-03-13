Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization

Public Class InterestReport
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd1 As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim countresult As Integer
    Dim oResultdate As Date

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.Open()

        If Not IsPostBack Then
            '   Dim script As String = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });"
            '   ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)

            damt_header.Visible = False
            damt_foot.Visible = False

            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtidate.ClientID), True)
            ' tdate.Text = Format(Now, "dd-MM-yyyy")
            ' bind_grid()


        End If
    End Sub

    Sub onPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        disp4post(txtidate.Text, txtproduct.Text)
        disp.PageIndex = e.NewPageIndex
        disp.DataBind()

    End Sub


    Sub disp4post(ByVal dt As String, ByVal dep As String)

        Dim int_total As Double = 0
        Dim int_d_total As Double = 0

        If damt_header.Visible = True Then
            disp.Columns(7).Visible = True
        Else
            disp.Columns(7).Visible = False
        End If

        Dim dsi As New DataSet
        Dim dat As DateTime = DateTime.ParseExact(dt, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        If con.State = ConnectionState.Closed Then con.Open()

        ' txtidate.Text = tdate.Text
        '  txtproduct.Text = prod.SelectedItem.Text

        'query = "select acdate,acn,amt,prd,cint,camt from tmpint WHERE convert(varchar(20), tdate, 112) ='" + reformatted + "' and dep='" + dep + "'"
        If session_user_role = "Audit" Then
            query = "SELECT tmpintc.acdate,tmpintc.acn,member.FirstName,tmpintc.amt,tmpintc.prd,tmpintc.cint,tmpintc.camt,tmpintc.damt FROM dbo.tmpintc"
            query &= " LEFT OUTER JOIN dbo.masterc  ON tmpintc.acn = masterc.acno LEFT OUTER JOIN dbo.member  ON masterc.cid = member.MemberNo "
            query &= "WHERE CONVERT(VARCHAR(20), tmpintc.tdate, 112) ='" + reformatted + "' and dep='" + dep + "'"


        Else
            query = "SELECT tmpint.acdate,tmpint.acn,member.FirstName,tmpint.amt,tmpint.prd,tmpint.cint,tmpint.camt,tmpint.damt FROM dbo.tmpint"
            query &= " LEFT OUTER JOIN dbo.master  ON tmpint.acn = master.acno LEFT OUTER JOIN dbo.member  ON master.cid = member.MemberNo "
            query &= "WHERE CONVERT(VARCHAR(20), tmpint.tdate, 112) ='" + reformatted + "' and dep='" + dep + "'"

        End If




        'cmd.CommandText = query
        Try

            Dim adapter As New SqlDataAdapter(query, con)

            adapter.Fill(dsi)

            disp.DataSource = dsi
            disp.DataBind()

            If dsi.Tables(0).Rows.Count = 0 Then
                int_total = 0
                int_d_total = 0
            Else
                int_total = dsi.Tables(0).Compute("SUM(camt)", String.Empty)
                int_d_total = dsi.Tables(0).Compute("SUM(damt)", String.Empty)
            End If




            lblnet.Text = String.Format("{0:N}", int_total)
            lbldnet.Text = String.Format("{0:N}", int_d_total)

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        '  trim_disp()

    End Sub

    Sub trim_disp()
        Dim int_total As Double = 0
        Dim int_d_total As Double = 0

        For i As Integer = 0 To disp.Rows.Count - 1
            Dim camt As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblicamt"), Label)
            Dim damt As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblidamt"), Label)
            int_total = int_total + CDbl(camt.Text)
            int_d_total = int_d_total + CDbl(damt.Text)
        Next

        lblnet.Text = String.Format("{0:N}", int_total)
        lbldnet.Text = String.Format("{0:N}", int_d_total)




    End Sub

    Private Sub txtidate_TextChanged(sender As Object, e As EventArgs) Handles txtidate.TextChanged

    End Sub

    Private Sub txtproduct_TextChanged(sender As Object, e As EventArgs) Handles txtproduct.TextChanged

    End Sub

    Private Sub btn_new_Click(sender As Object, e As EventArgs) Handles btn_new.Click

        damt_foot.Visible = False

        damt_header.Visible = False
        disp4post(txtidate.Text, txtproduct.Text)
    End Sub

    Private Sub disp_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles disp.RowCreated
        'If dint_foot.Visible = True Then
        '    e.Row.Cells(7).Visible = True
        '    e.Row.Cells(8).Visible = True
        'Else
        '    e.Row.Cells(7).Visible = True
        '    e.Row.Cells(8).Visible = True
        'End If

    End Sub

    Private Sub btn_cv_Click(sender As Object, e As EventArgs) Handles btn_cv.Click

        '78


        damt_foot.Visible = True
        damt_header.Visible = True
        disp4post(txtidate.Text, txtproduct.Text)


    End Sub

    Private Sub InterestReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class