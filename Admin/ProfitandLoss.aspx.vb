Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Public Class ProfitandLoss

    Inherits System.Web.UI.Page
    Public dt As New DataTable
    Public dtd As New DataTable
    Public pnl As Double = 0


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
        pnl = GetNetProfitLoss(fromDate, toDate)

        ' Add Net Profit/Loss to appropriate side
        If pnl < 0 Then
            Dim newrow As DataRow = dt.NewRow()
            newrow(0) = "Profit & Loss (Net Loss)"
            newrow(1) = -pnl
            dt.Rows.Add(newrow)
        ElseIf pnl > 0 Then
            Dim newrow As DataRow = dtd.NewRow()
            newrow(0) = "Profit & Loss (Net Profit)"
            newrow(1) = pnl
            dtd.Rows.Add(newrow)
        End If

        ' Consolidated query for all Income and Expenditure accounts
        Dim query As String = "SELECT l.ledger, l.under, ISNULL(SUM(s.credit), 0) - ISNULL(SUM(s.debit), 0) as Balance " &
                             "FROM ledger l " &
                             "INNER JOIN suplement s ON l.ledger = s.achead " &
                             "WHERE l.under IN ('Income', 'Expenditure') AND s.date >= @frm AND s.date <= @to " &
                             "GROUP BY l.ledger, l.under " &
                             "HAVING SUM(s.credit) <> SUM(s.debit)"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@frm", fromDate)
                cmd.Parameters.AddWithValue("@to", toDate)
                con.Open()
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim ach As String = dr("ledger").ToString()
                        Dim under As String = dr("under").ToString()
                        Dim bal As Double = Convert.ToDouble(dr("Balance"))

                        If under = "Income" Then
                            Dim r As DataRow = dt.NewRow()
                            r(0) = ach
                            r(1) = bal ' In P&L, Income is on Credit side
                            dt.Rows.Add(r)
                        Else ' Expenditure
                            Dim r As DataRow = dtd.NewRow()
                            r(0) = ach
                            r(1) = -bal ' Expenditure is on Debit side
                            dtd.Rows.Add(r)
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
        btntrf.Visible = True
    End Sub

    Function GetNetProfitLoss(ByVal fromDate As DateTime, ByVal toDate As DateTime) As Double
        Dim netPL As Double = 0
        Dim query As String = "SELECT ISNULL(SUM(credit) - SUM(debit), 0) FROM suplement s " &
                             "INNER JOIN ledger l ON s.achead = l.ledger " &
                             "WHERE l.under IN ('Income', 'Expenditure') AND s.date >= @frm AND s.date <= @to"

        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@frm", fromDate)
                cmd.Parameters.AddWithValue("@to", toDate)
                con.Open()
                netPL = Convert.ToDouble(cmd.ExecuteScalar())
            End Using
        End Using
        Return netPL
    End Function


    Private Sub txtto_TextChanged(sender As Object, e As EventArgs) Handles txtto.TextChanged


    End Sub

    Protected Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        BindReport()
    End Sub


    ' Removed get_bal, profitloss, and get_opening as they are now integrated into BindReport and GetNetProfitLoss







    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String, Optional ByVal extCon As SqlConnection = Nothing)
        Dim query As String = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type) VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"

        Dim action As Action(Of SqlCommand) = Sub(cmd)
                                                   cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtto.Text))
                                                   cmd.Parameters.AddWithValue("@transid", tid)
                                                   cmd.Parameters.AddWithValue("@achead", ach)
                                                   cmd.Parameters.AddWithValue("@debit", dr)
                                                   cmd.Parameters.AddWithValue("@credit", cr)
                                                   cmd.Parameters.AddWithValue("@acn", "")
                                                   cmd.Parameters.AddWithValue("@nar", nar)
                                                   cmd.Parameters.AddWithValue("@typ", typ)
                                                   cmd.ExecuteNonQuery()
                                               End Sub

        If extCon IsNot Nothing Then
            Using cmd As New SqlCommand(query, extCon)
                action(cmd)
            End Using
        Else
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                Using cmd As New SqlCommand(query, con)
                    con.Open()
                    action(cmd)
                End Using
            End Using
        End If
    End Sub



    Private Sub btntrf_Click(sender As Object, e As EventArgs) Handles btntrf.Click
        System.Threading.Thread.Sleep(5000)



        post_credit()
        post_debit()

        Response.Redirect("..\login.aspx")



    End Sub

    Sub post_credit()
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            For i As Integer = 0 To disp.Rows.Count - 1
                Dim achead As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbl_typ_supl"), Label)
                Dim credit As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbl_dr"), Label)

                If achead.Text = "Profit & Loss" OrElse achead.Text.Contains("Profit & Loss") Then Continue For

                Dim queryIns As String = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal,denom) " &
                                        "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal,@denom)"

                Using cmd As New SqlCommand(queryIns, con)
                    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtto.Text))
                    cmd.Parameters.AddWithValue("@acno", "")
                    cmd.Parameters.AddWithValue("@drd", CDbl(credit.Text))
                    cmd.Parameters.AddWithValue("@crd", 0)
                    cmd.Parameters.AddWithValue("@drc", CDbl(credit.Text))
                    cmd.Parameters.AddWithValue("@crc", 0)
                    cmd.Parameters.AddWithValue("@narration", "To Transfer")
                    cmd.Parameters.AddWithValue("@due", "")
                    cmd.Parameters.AddWithValue("@type", "TRF")
                    cmd.Parameters.AddWithValue("@suplimentry", achead.Text)
                    cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                    cmd.Parameters.AddWithValue("@entryat", DateTime.Now)
                    cmd.Parameters.AddWithValue("@cbal", 0)
                    cmd.Parameters.AddWithValue("@dbal", 0)
                    cmd.Parameters.AddWithValue("@denom", 1)
                    cmd.ExecuteNonQuery()
                End Using

                Dim tid As Integer
                Dim queryMax As String = "SELECT MAX(ID) from dbo.trans where trans.suplimentry=@ach"
                Using cmd As New SqlCommand(queryMax, con)
                    cmd.Parameters.AddWithValue("@ach", Trim(achead.Text))
                    tid = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                update_suplementry(tid, achead.Text, 0, CDbl(credit.Text), "Transfered to P&L ", "JOURNAL", con)
                update_suplementry(tid, "Profit & Loss A/C", CDbl(credit.Text), 0, "Transfered to P&L ", "JOURNAL", con)
            Next
        End Using
    End Sub



    Sub post_debit()
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            For i As Integer = 0 To disp_debit.Rows.Count - 1
                Dim achead As Label = DirectCast(disp_debit.Rows(i).Cells(0).FindControl("lbl_typ_supl"), Label)
                Dim debit As Label = DirectCast(disp_debit.Rows(i).Cells(0).FindControl("lbl_dr"), Label)

                If achead.Text = "Profit & Loss" OrElse achead.Text.Contains("Profit & Loss") Then Continue For

                Dim queryIns As String = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal,denom) " &
                                        "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal,@denom)"

                Using cmd As New SqlCommand(queryIns, con)
                    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtto.Text))
                    cmd.Parameters.AddWithValue("@acno", "")
                    cmd.Parameters.AddWithValue("@drd", 0)
                    cmd.Parameters.AddWithValue("@crd", CDbl(debit.Text))
                    cmd.Parameters.AddWithValue("@drc", 0)
                    cmd.Parameters.AddWithValue("@crc", CDbl(debit.Text))
                    cmd.Parameters.AddWithValue("@narration", "By Transfer")
                    cmd.Parameters.AddWithValue("@due", "")
                    cmd.Parameters.AddWithValue("@type", "TRF")
                    cmd.Parameters.AddWithValue("@suplimentry", achead.Text)
                    cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                    cmd.Parameters.AddWithValue("@entryat", DateTime.Now)
                    cmd.Parameters.AddWithValue("@cbal", 0)
                    cmd.Parameters.AddWithValue("@dbal", 0)
                    cmd.Parameters.AddWithValue("@denom", 1)
                    cmd.ExecuteNonQuery()
                End Using

                Dim tid As Integer
                Dim queryMax As String = "SELECT MAX(ID) from dbo.trans where trans.suplimentry=@ach"
                Using cmd As New SqlCommand(queryMax, con)
                    cmd.Parameters.AddWithValue("@ach", Trim(achead.Text))
                    tid = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                update_suplementry(tid, achead.Text, CDbl(debit.Text), 0, "Transfered to P&L ", "JOURNAL", con)
                update_suplementry(tid, "Profit & Loss A/C", 0, CDbl(debit.Text), "Transfered to P&L ", "JOURNAL", con)
            Next
        End Using
    End Sub




End Class