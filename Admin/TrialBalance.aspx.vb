Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Public Class TrialBalance

    Inherits System.Web.UI.Page
    Public newrow As DataRow
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Public dt As New DataTable
    Public dtd As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ' bind_grid()
        End If
    End Sub

    Protected Sub OnPagingdebit(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        disp_debit.PageIndex = e.NewPageIndex
        bind_grid()

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        disp.PageIndex = e.NewPageIndex
        bind_grid()

    End Sub
    Sub bind_grid()
        Dim fromDate As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim toDate As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("achead", GetType(String))
            dt.Columns.Add("credit", GetType(Decimal))
        Else
            dt.Clear()
        End If

        Dim ob As Double = get_opening(fromDate)

        newrow = dt.NewRow
        newrow(0) = "Opening Balance"
        newrow(1) = ob
        dt.Rows.Add(newrow)

        Dim query As String = "SELECT achead, SUM(credit) AS credit FROM dbo.suplement WHERE date >= @frm AND date <= @to GROUP BY achead HAVING SUM(credit) > 0 ORDER BY achead"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@frm", fromDate)
                cmd.Parameters.AddWithValue("@to", toDate)
                con.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        newrow = dt.NewRow
                        newrow(0) = dr(0)
                        newrow(1) = dr(1)
                        dt.Rows.Add(newrow)
                    End While
                End Using
            End Using
        End Using

        disp.DataSource = dt
        disp.DataBind()
        lblcr.Text = String.Format("{0:N}", dt.Compute("sum(credit)", ""))

        If dtd.Columns.Count = 0 Then
            dtd.Columns.Add("achead", GetType(String))
            dtd.Columns.Add("debit", GetType(Decimal))
        Else
            dtd.Clear()
        End If

        query = "SELECT achead, SUM(debit) AS debit FROM dbo.suplement WHERE date >= @frm AND date <= @to GROUP BY achead HAVING SUM(debit) > 0 ORDER BY achead"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@frm", fromDate)
                cmd.Parameters.AddWithValue("@to", toDate)
                con.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        newrow = dtd.NewRow
                        newrow(0) = dr(0)
                        newrow(1) = dr(1)
                        dtd.Rows.Add(newrow)
                    End While
                End Using
            End Using
        End Using

        Dim nextDay As DateTime = toDate.AddDays(1)
        ob = get_opening(nextDay)

        newrow = dtd.NewRow
        newrow(0) = "Closing Balance"
        newrow(1) = ob
        dtd.Rows.Add(newrow)

        disp_debit.DataSource = dtd
        disp_debit.DataBind()
        lbldr.Text = String.Format("{0:N}", dtd.Compute("sum(debit)", ""))
    End Sub


    Protected Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        'System.Threading.Thread.Sleep(5000)
        bind_grid()

    End Sub

    Function get_opening(ByVal targetDate As DateTime) As Double
        Dim opening As Double = 0
        Dim query As String = "SELECT " &
                             "(SELECT ISNULL(SUM(ob), 0) FROM ledger) + " &
                             "ISNULL((SELECT SUM(credit) - SUM(debit) FROM suplement WHERE date < @dt AND scroll = '1'), 0)"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@dt", targetDate)
                con.Open()
                opening = Convert.ToDouble(cmd.ExecuteScalar())
            End Using
        End Using
        Return opening
    End Function


    Private Sub btn_show_Click(sender As Object, e As ImageClickEventArgs) Handles btn_show.Click

        bind_grid_ndh3()



    End Sub


    Sub bind_grid_ndh3()
        Dim fromDate As DateTime = DateTime.ParseExact(txtfrm1.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim toDate As DateTime = DateTime.ParseExact(txtto1.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("achead", GetType(String))
            dt.Columns.Add("ob", GetType(Decimal))
            dt.Columns.Add("debit", GetType(Decimal))
            dt.Columns.Add("credit", GetType(Decimal))
            dt.Columns.Add("cb", GetType(Decimal))
        Else
            dt.Clear()
        End If

        Dim query As String = "WITH PreRangeTotals AS (" &
                             "    SELECT achead, SUM(credit) - SUM(debit) as PreBal" &
                             "    FROM suplement WHERE date < @frm GROUP BY achead" &
                             ")," &
                             "RangeTotals AS (" &
                             "    SELECT achead, SUM(debit) as RangeDebit, SUM(credit) as RangeCredit" &
                             "    FROM suplement WHERE date >= @frm AND date <= @to GROUP BY achead" &
                             ")" &
                             "SELECT " &
                             "    l.ledger as achead," &
                             "    ISNULL(l.ob, 0) + ISNULL(p.PreBal, 0) as ob," &
                             "    ISNULL(r.RangeDebit, 0) as debit," &
                             "    ISNULL(r.RangeCredit, 0) as credit," &
                             "    (ISNULL(l.ob, 0) + ISNULL(p.PreBal, 0)) - ISNULL(r.RangeDebit, 0) + ISNULL(r.RangeCredit, 0) as cb " &
                             "FROM ledger l " &
                             "LEFT JOIN PreRangeTotals p ON l.ledger = p.achead " &
                             "LEFT JOIN RangeTotals r ON l.ledger = r.achead " &
                             "WHERE l.ob <> 0 OR p.achead IS NOT NULL OR r.achead IS NOT NULL " &
                             "ORDER BY l.ledger"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@frm", fromDate)
                cmd.Parameters.AddWithValue("@to", toDate)
                con.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        newrow = dt.NewRow
                        newrow("achead") = dr("achead")
                        newrow("ob") = dr("ob")
                        newrow("debit") = dr("debit")
                        newrow("credit") = dr("credit")
                        newrow("cb") = dr("cb")
                        dt.Rows.Add(newrow)
                    End While
                End Using
            End Using
        End Using

        Session("dt") = dt
        disp_ndh3.DataSource = dt
        disp_ndh3.DataBind()
    End Sub



    Protected Sub ExportToExcel(sender As Object, e As EventArgs)

        disp_ndh3.AllowPaging = False
        bind_grid_ndh3()

        Response.Clear()
        Response.Buffer = True
        'Response.ContentType = "application/vnd.ms-excel"
        'Response.AddHeader("Content-Disposition", "attachment; filename=Consolidated Report.xlsx")

        Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

        Response.AppendHeader("content-disposition", "attachment; filename=ndh3.xlsx")
        Response.Charset = ""
        Me.EnableViewState = False
        Dim oStringWriter As New System.IO.StringWriter()
        Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)
        disp_ndh3.Visible = True
        disp_ndh3.RenderControl(oHtmlTextWriter)
        Response.Write(oStringWriter.ToString)
        'Response.Flush()
        Response.End()


    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        'Tell the compiler that the control is rendered
        'explicitly by overriding; the VerifyRenderingInServerForm event.
        '   Return



    End Sub
    Private Sub btn_exp_Click(sender As Object, e As ImageClickEventArgs) Handles btn_exp.Click
        ExportToExcel(sender, e)
    End Sub
End Class