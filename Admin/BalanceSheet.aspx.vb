Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Public Class BalanceSheet

    Inherits System.Web.UI.Page
    Public dt As New DataTable
    Public dtd As New DataTable
    Public pandl As Double = 0



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ' bind_grid()
        End If
    End Sub


    Protected Sub OnPagingdebit(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        disp_debit.PageIndex = e.NewPageIndex
        BindReport()
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        disp.PageIndex = e.NewPageIndex
        BindReport()
    End Sub

    Sub BindReport()
        Dim fromDate As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim toDate As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

        ' Clear data
        dt.Clear()
        dtd.Clear()

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("achead", GetType(String))
            dt.Columns.Add("credit", GetType(Decimal))
        End If
        If dtd.Columns.Count = 0 Then
            dtd.Columns.Add("achead", GetType(String))
            dtd.Columns.Add("debit", GetType(Decimal))
        End If

        ' Fetch P&L
        pandl = GetProfitLoss(toDate)

        ' Add P&L to appropriate side
        If pandl > 0 Then
            Dim newrow As DataRow = dt.NewRow()
            newrow(0) = "Profit & Loss"
            newrow(1) = pandl
            dt.Rows.Add(newrow)
        ElseIf pandl < 0 Then
            Dim newrow As DataRow = dtd.NewRow()
            newrow(0) = "Profit & Loss"
            newrow(1) = -pandl
            dtd.Rows.Add(newrow)
        End If

        ' Add Cash In Hand to Assets
        Dim cashRow As DataRow = dtd.NewRow()
        cashRow(0) = "Cash In Hand"
        cashRow(1) = get_opening(toDate)
        dtd.Rows.Add(cashRow)

        ' Consolidated query for all ledger accounts
        Dim query As String = "WITH Movement AS (" &
                             "    SELECT achead, SUM(ISNULL(debit,0)) as RangeDebit, SUM(ISNULL(credit,0)) as RangeCredit " &
                             "    FROM suplement WHERE date >= @frm AND date <= @to GROUP BY achead" &
                             ") " &
                             "SELECT l.ledger, l.under, ISNULL(l.ob, 0) as ob, " &
                             "ISNULL(m.RangeDebit, 0) as RangeDebit, ISNULL(m.RangeCredit, 0) as RangeCredit " &
                             "FROM ledger l LEFT JOIN Movement m ON l.ledger = m.achead " &
                             "WHERE l.under IN ('Liabilities', 'Assets')"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@frm", fromDate)
                cmd.Parameters.AddWithValue("@to", toDate)
                con.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim ach As String = dr("ledger").ToString()
                        Dim under As String = dr("under").ToString()
                        Dim ob As Double = Convert.ToDouble(dr("ob"))
                        Dim rDebit As Double = Convert.ToDouble(dr("RangeDebit"))
                        Dim rCredit As Double = Convert.ToDouble(dr("RangeCredit"))

                        Dim bal As Double
                        If ndh3.Checked Then
                            bal = rCredit + rDebit
                        Else
                            bal = ob - rDebit + rCredit
                        End If

                        If bal <> 0 Then
                            If under = "Liabilities" Then
                                Dim r As DataRow = dt.NewRow()
                                r(0) = ach
                                r(1) = bal
                                dt.Rows.Add(r)
                            Else ' Assets
                                Dim r As DataRow = dtd.NewRow()
                                r(0) = ach
                                r(1) = -bal ' Assets side displays positive values for debit balances
                                dtd.Rows.Add(r)
                            End If
                        End If
                    End While
                End Using
            End Using
        End Using

        ' Bind GridViews
        disp.DataSource = dt
        disp.DataBind()
        disp_debit.DataSource = dtd
        disp_debit.DataBind()

        ' Update Totals
        lblcr.Text = String.Format("{0:N}", dt.Compute("sum(credit)", ""))
        lbldr.Text = String.Format("{0:N}", dtd.Compute("sum(debit)", ""))
    End Sub


    Private Sub txtto_TextChanged(sender As Object, e As EventArgs) Handles txtto.TextChanged


    End Sub




    Function GetProfitLoss(ByVal toDate As DateTime) As Double
        Dim pl As Double = 0
        Dim query As String = "SELECT ISNULL(SUM(credit) - SUM(debit), 0) FROM suplement s " &
                             "INNER JOIN ledger l ON s.achead = l.ledger " &
                             "WHERE l.under IN ('Income', 'Expenditure') AND s.date <= @dt"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@dt", toDate)
                con.Open()
                pl = Convert.ToDouble(cmd.ExecuteScalar())
            End Using
        End Using
        Return pl
    End Function


    Function get_opening(ByVal toDate As DateTime) As Double
        Dim opening As Double = 0
        Dim query As String = "SELECT " &
                             "(SELECT ISNULL(SUM(ob), 0) FROM ledger) + " &
                             "ISNULL((SELECT SUM(credit) - SUM(debit) FROM suplement WHERE date <= @dt AND scroll = '1'), 0)"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@dt", toDate)
                con.Open()
                opening = Convert.ToDouble(cmd.ExecuteScalar())
            End Using
        End Using
        Return opening
    End Function



    Protected Sub ExportToExcel(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            'To Export all pages
            ' disp.AllowPaging = False
            ' Me.BindGrid()

            disp.HeaderRow.BackColor = Drawing.Color.White
            For Each cell As TableCell In disp.HeaderRow.Cells
                cell.BackColor = disp.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In disp.Rows
                row.BackColor = Drawing.Color.White
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
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub

    Private Sub btn_exp_Click(sender As Object, e As ImageClickEventArgs) Handles btn_exp.Click
        ExportToExcel(sender, e)

    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        BindReport()
    End Sub


    'Private Sub btn_txt_Click(sender As Object, e As ImageClickEventArgs) Handles btn_txt.Click


    '    Dim line As String = String.Empty

    '    If con1.State = ConnectionState.Closed Then con1.Open()

    '    query = "SELECT branchcode FROM BRANCH "

    '    'cmd.CommandType = CommandType.Text
    '    cmdi.Connection = con1
    '    cmdi.CommandText = query
    '    Try


    '        oresult = cmdi.ExecuteScalar()

    '    Catch ex As Exception

    '        MsgBox(ex.ToString)
    '    End Try


    '    oresult = "KBF" + oresult

    '    Session("oresult") = oresult


    '    For i As Integer = 0 To disp.Rows.Count - 1
    '        Dim led As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbl_typ_supl"), Label)
    '        Dim amt As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbl_cr"), Label)
    '        line &= CStr(txtfrm.Text) + "^"
    '        line &= CStr(txtto.Text) + "^"
    '        line &= CStr(oresult) + "^"
    '        line &= CStr("Liabilities") + "^"
    '        line &= CStr(led.Text) + "^"
    '        line &= CStr(amt.Text) + vbCrLf
    '    Next
    '    '  txttdspaid.Text = total
    '    For i As Integer = 0 To disp_debit.Rows.Count - 1
    '        Dim led1 As Label = DirectCast(disp_debit.Rows(i).Cells(0).FindControl("lblached"), Label)
    '        Dim amt1 As Label = DirectCast(disp_debit.Rows(i).Cells(0).FindControl("lbl_dr"), Label)
    '        line &= CStr(txtfrm.Text) + "^"
    '        line &= CStr(txtto.Text) + "^"
    '        line &= CStr(oresult) + "^"
    '        line &= CStr("Assets") + "^"
    '        line &= CStr(led1.Text) + "^"
    '        line &= CStr(amt1.Text) + vbCrLf
    '    Next


    '    Dim strFile As String = "d:\" + Session("oresult") + "BS.txt"
    '    Dim sw As StreamWriter
    '    Try
    '        If (Not File.Exists(strFile)) Then
    '            sw = File.CreateText(strFile)
    '            sw.WriteLine(line)
    '            sw.Close()
    '        Else

    '            File.Delete(strFile)
    '            '    sw = File.CreateText(strFile)
    '            '  sw.WriteLine(line)
    '        End If
    '        ' sw.WriteLine("Error Message in  Occured at-- " & DateTime.Now)

    '    Catch ex As IOException
    '        MsgBox("Error writing to log file.")
    '    End Try



    'End Sub
End Class