Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization




Public Class StockBook
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
            bind_grid()



        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        disp.PageIndex = e.NewPageIndex
        bind_grid()

    End Sub

    Protected Sub OnPagingout(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvout.PageIndex = e.NewPageIndex
        disp_out()


    End Sub

    Protected Sub OnPagingin(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        gvin.PageIndex = e.NewPageIndex
        disp_in()

    End Sub
    Sub bind_grid()
        Dim sum As Decimal = 0
        If con.State = ConnectionState.Closed Then con.Open()

        query = "select date,acn,tqty,tnet from jlstock where cld='0' order by date"
        cmd.Connection = con
        cmd.CommandText = query
        Dim ds As New DataTable
        Dim dr As SqlDataReader

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("date", GetType(Date))
            dt.Columns.Add("acn", GetType(String))
            dt.Columns.Add("tqty", GetType(Integer))
            dt.Columns.Add("tnet", GetType(Decimal))
            dt.Columns.Add("total", GetType(Decimal))
        End If



        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then

                Do While dr.Read()
                    newrow = dt.NewRow

                    newrow(0) = dr(0)
                    newrow(1) = dr(1)
                    newrow(2) = dr(2)
                    newrow(3) = Format(dr(3), "#,##0.000")
                    sum = sum + dr(3)
                    newrow(4) = Format(sum, "#,##0.000")
                    dt.Rows.Add(newrow)
                Loop

                disp.DataSource = dt
                disp.DataBind()
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click

        disp_in()




    End Sub
    Sub disp_in()
        Dim sum As Decimal = 0


        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select date,acn,tqty,tnet from jlstock where CONVERT(VARCHAR(20), date, 112) between @frm and @to order by date,acn"

        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@frm", reformatted)
        cmd.Parameters.AddWithValue("@to", reformatted1)
        Dim ds As New DataTable
        Dim dr As SqlDataReader

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("date", GetType(Date))
            dt.Columns.Add("acn", GetType(String))
            dt.Columns.Add("tqty", GetType(Integer))
            dt.Columns.Add("tnet", GetType(Decimal))
            dt.Columns.Add("total", GetType(Decimal))
        End If



        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then

                Do While dr.Read()
                    newrow = dt.NewRow

                    newrow(0) = dr(0)
                    newrow(1) = dr(1)
                    newrow(2) = dr(2)
                    newrow(3) = Format(dr(3), "#,##0.000")
                    sum = sum + dr(3)
                    newrow(4) = Format(sum, "#,##0.000")
                    dt.Rows.Add(newrow)
                Loop

                gvin.DataSource = dt
                gvin.DataBind()
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Sub

    Sub disp_out()
        Dim sum As Decimal = 0


        Dim dat As DateTime = DateTime.ParseExact(txtfrmout.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txttoout.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select date,acn,tqty,tnet from jlstock where CONVERT(VARCHAR(20), cldon, 112) between @frm and @to order by date,acn"

        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@frm", reformatted)
        cmd.Parameters.AddWithValue("@to", reformatted1)
        Dim ds As New DataTable
        Dim dr As SqlDataReader

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("date", GetType(Date))
            dt.Columns.Add("acn", GetType(String))
            dt.Columns.Add("tqty", GetType(Integer))
            dt.Columns.Add("tnet", GetType(Decimal))
            dt.Columns.Add("total", GetType(Decimal))
        End If



        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then

                Do While dr.Read()
                    newrow = dt.NewRow

                    newrow(0) = dr(0)
                    newrow(1) = dr(1)
                    newrow(2) = dr(2)
                    newrow(3) = Format(dr(3), "#,##0.000")
                    sum = sum + dr(3)
                    newrow(4) = Format(sum, "#,##0.000")
                    dt.Rows.Add(newrow)
                Loop

                gvout.DataSource = dt
                gvout.DataBind()
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Sub

    Private Sub srch_out_Click(sender As Object, e As EventArgs) Handles srch_out.Click
        disp_out()

    End Sub
    Sub get_data(ByVal acn As String)


        If con.State = ConnectionState.Closed Then con.Open()

        query = "select itm,qty,gross,net from jlspec where jlspec.acno='" + acn + "'"
        'cmd.Connection = con
        'cmd.CommandText = query

        Dim ds As New DataSet
        Dim adapter As New SqlDataAdapter(query, con)
        adapter.Fill(ds)
        dispjl.DataSource = ds
        dispjl.DataBind()

        adapter.Dispose()

        query = "select tqty,tgross,tnet from jlstock where jlstock.acn='" + acn + "'"
        cmd.Connection = con
        cmd.CommandText = query
        Dim dr As SqlDataReader

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()

                lbltqty.Text = IIf(IsDBNull(dr(0)), 0, dr(0))
                lblgross.Text = Format(IIf(IsDBNull(dr(1)), 0, dr(1)), "##,##0.000")
                lblnet.Text = Format(IIf(IsDBNull(dr(2)), 0, dr(2)), "##,##0.000")
            End If
            dr.Close()


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        '  Me.popupdep.Show()


    End Sub

    Protected Sub disp_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            ' e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(disp, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
            e.Row.Attributes("style") = "cursor:pointer"
            ' e.Row.ToolTip = "Click last column for selecting this row."
        End If
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Dim pName As String = disp.SelectedRow.Cells(1).Text
        'MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)
        For Each row As GridViewRow In disp.Rows
            If row.RowIndex = disp.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
                ' txtlimit.Text = disp.SelectedRow.Cells(1).Text 'row.Cells.Item(0).Text
                Dim dt As Label = DirectCast(disp.SelectedRow.Cells(0).FindControl("lblsl"), Label)
                get_data(dt.Text)

            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#f0f4f5")
                row.ToolTip = "Click to select this row."
            End If
        Next
    End Sub

    Protected Sub gvin_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            ' e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvin, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
            e.Row.Attributes("style") = "cursor:pointer"
            ' e.Row.ToolTip = "Click last column for selecting this row."
        End If
    End Sub
    Protected Sub OnSelectedIndexChangedgvin(ByVal sender As Object, ByVal e As EventArgs)
        'Dim pName As String = disp.SelectedRow.Cells(1).Text
        'MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)
        For Each row As GridViewRow In gvin.Rows
            If row.RowIndex = gvin.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
                ' txtlimit.Text = disp.SelectedRow.Cells(1).Text 'row.Cells.Item(0).Text
                Dim dt As Label = DirectCast(gvin.SelectedRow.Cells(0).FindControl("lblsl"), Label)
                get_data(dt.Text)

            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#f0f4f5")
                row.ToolTip = "Click to select this row."
            End If
        Next
    End Sub

    Protected Sub gvout_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            ' e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvout, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
            e.Row.Attributes("style") = "cursor:pointer"
            ' e.Row.ToolTip = "Click last column for selecting this row."
        End If
    End Sub
    Protected Sub OnSelectedIndexChangedgvout(ByVal sender As Object, ByVal e As EventArgs)
        'Dim pName As String = disp.SelectedRow.Cells(1).Text
        'MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)
        For Each row As GridViewRow In gvout.Rows
            If row.RowIndex = gvout.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
                ' txtlimit.Text = disp.SelectedRow.Cells(1).Text 'row.Cells.Item(0).Text
                Dim dt As Label = DirectCast(gvout.SelectedRow.Cells(0).FindControl("lblsl"), Label)
                get_data(dt.Text)

            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#f0f4f5")
                row.ToolTip = "Click to select this row."
            End If
        Next
    End Sub


    Private Sub dep_closed_Click(sender As Object, e As ImageClickEventArgs) Handles dep_closed.Click
        '  Me.popupdep.Hide()

    End Sub
    Private Sub btnprnt_Click(sender As Object, e As EventArgs) Handles btnprnt.Click

        Dim stitle As String = get_home()
        Dim msg As String = "STOCK BOOK"
        disp.AllowPaging = False
        bind_grid()





        Dim fnc As String = "doprnt('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "doprint", fnc, True)
    End Sub
    Function get_home()

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = "SELECT TOP 1 branch FROM branch"

        Try
            home = cmd.ExecuteScalar


            If home Is Nothing Then
                home = "KARAVILAI"
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        Return home


    End Function
    Private Sub btnxl_Click(sender As Object, e As EventArgs) Handles btnxl.Click


        'Private Sub isadd_CheckedChanged(sender As Object, e As EventArgs) Handles isadd.CheckedChanged
        '    If isadd.Checked Then
        '        disp_gridadd()
        '        ' btnexport.Visible = True
        '    Else
        '        disp_grid()

        '    End If
        'End Sub

        Response.Clear()

        Response.Buffer = True



        Response.AddHeader("content-disposition", "attachment;filename=Stock Book.xls")

        Response.Charset = ""

        Response.ContentType = "application/vnd.ms-excel"



        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)



        disp.AllowPaging = False

        bind_grid()




        'Change the Header Row back to white color

        '   disp.HeaderRow.Style.Add("background-color", "#FFFFFF")



        'Apply style to Individual Cells




        For i As Integer = 0 To disp.Rows.Count - 1

            Dim row As GridViewRow = disp.Rows(i)




            'Apply text style to each Row

            '  row.Attributes.Add("class", "textmode")



            'Apply style to Individual Cells of Alternating Row



        Next

        disp.RenderControl(hw)



        'style to format numbers to string

        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"

        Response.Write(style)

        Response.Output.Write(sw.ToString())

        Response.Flush()

        Response.End()


    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub


End Class
