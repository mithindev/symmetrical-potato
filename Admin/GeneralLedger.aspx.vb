Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Public Class GeneralLedger


    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim countresult As Integer
    Dim ds As New DataSet



    Protected Sub disp_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            '    ' e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            '    ' e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(disp, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"

            '    e.Row.ToolTip = "Click to select this row."
            '    ' e.Row.ToolTip = "Click last column for selecting this row."
        End If
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ''Dim pName As String = disp.SelectedRow.Cells(1).Text
        ''MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)
        For Each row As GridViewRow In disp.Rows
            If row.RowIndex = disp.SelectedIndex Then
                ' row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
                Dim led As Label = DirectCast(disp.SelectedRow.Cells(0).FindControl("lblached"), Label)

                Session("led2edit") = led.Text

                get_data()

            Else
                ' row.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
                'row.ToolTip = "Click to select this row."
            End If
        Next
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        bind_grid()
        disp.PageIndex = e.NewPageIndex
        disp.DataBind()
    End Sub

    Sub get_data()
        If con.State = ConnectionState.Closed Then con.Open()
        Dim led As String = CType(Session("led2edit"), String)

        query = "select ledger,under,ob from ledger where ledger='" + Trim(led) + "'"

        Dim dr As SqlDataReader

        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()
                txtled.Text = IIf(IsDBNull(dr(0)), "", dr(0))
                txtob.Text = dr(2)
                undr.SelectedItem.Text = dr(1)
            End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Sub
    Sub bind_grid()

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select ledger,under,ob from ledger order by ledger"

        Dim ds As New DataSet

        Dim adapter As New SqlDataAdapter(query, con)
        adapter.Fill(ds)

        disp.DataSource = ds
        disp.DataBind()

    End Sub
    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then

            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtled.ClientID), True)
            bind_grid()

        End If

    End Sub

    Private Sub btn_clr_Click(sender As Object, e As EventArgs) Handles btn_clr.Click

        txtled.Text = ""
        txtob.Text = ""
        undr.SelectedIndex = 0
        txtfocus(txtled)
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        Dim isedit As Boolean = False
        Dim msg As String = String.Empty



        If con.State = ConnectionState.Closed Then con.Open()
        If Session("led2edit") = Nothing Then
            query = "insert into ledger(ledger,under,ob)"
            query &= "values(@ledger,@under,@ob)"
            isedit = False
        Else
            Dim led As String = CType(Session("led2edit"), String)
            query = "UPDATE ledger SET ledger=@ledger,under=@under,ob=@ob where ledger='" + led + "'"

            isedit = True
        End If
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@ledger", txtled.Text)
        cmd.Parameters.AddWithValue("@under", undr.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@ob", CDbl(txtob.Text))
        cmd.CommandText = query
        cmd.Connection = con
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Dim stitle As String = "Hi  " + Session("sesusr")
        If isedit = True Then
            msg = "Ledger Updated Sucessfully !"
        Else
            msg = "Ledger Created Sucessfully !"
        End If
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

        bind_grid()
        txtled.Text = ""
        txtob.Text = ""
        undr.SelectedIndex = 0
        txtfocus(txtled)
    End Sub
End Class
