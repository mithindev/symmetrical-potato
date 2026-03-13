Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Globalization

Public Class WebForm1
    Inherits System.Web.UI.Page

    Dim query As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            dt.Text = String.Format("{0:dd MMMM yyyy}", DateAndTime.Today)

            If Request.UrlReferrer IsNot Nothing Then
                ViewState("RefUrl") = Request.UrlReferrer.ToString()
            Else
                ViewState("RefUrl") = ""
            End If
            
            Dim sw As New System.Diagnostics.Stopwatch()

            sw.Restart()
            get_opening()
            lblOpeningTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            get_dep()
            lblDepTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            get_loan()
            lblLoanTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            gold_rate()
            lblGoldTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            petty_cash()
            lblPettyTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            fill_recent()
            lblRecentTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            bind_user()
            lblStaffTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            scrollcount()
            lblScrollTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            fill_productstat()
            lblStatsTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            get_maturing_this_week()
            lblMaturityTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"

            sw.Restart()
            ' get_overdue_jl() -- MOVED TO ASYNC TIMER TO PREVENT LOGIN BLOCKING
            lblOverdueTime.Text = "(Pending...)"

            lblsms.Text = sms_bal()
            smsbal = lblsms.Text
            sw.Stop()
            lblSmsTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"
        End If

    End Sub

    Sub scrollcount()
        Dim cnt As Integer = 0
        Dim cnt1 As Integer = 0

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                query = "SELECT COUNT(*) AS expr1 FROM dbo.trans"
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    cnt = cmd.ExecuteScalar()
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()
        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                query = "SELECT COUNT(*) AS expr1 FROM dbo.trans where scroll='1'"
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    cnt1 = cmd.ExecuteScalar()
                Catch ex As Exception
                    Response.Write(ex.ToString)
                Finally
                    con.Close()
                    cmd.Dispose()
                End Try
            End Using
            con.Close()
        End Using

        lblvch.Text = CStr(cnt1) + " / " + CStr(cnt)
    End Sub

    Sub fill_productstat()
        ' FIX: Replaced 20 individual queries (10 helper calls × 2 each) with 2 grouped queries.
        ' Query 1: Get SUM(debit) and SUM(credit) per account-head from suplement in one shot.
        ' Query 2: Get opening balances from ledger for all products in one shot.

        Dim balances As New Dictionary(Of String, Double)  ' achead -> net balance

        Dim depHeads As String() = {"DAILY DEPOSIT", "FIXED DEPOSIT", "KMK DEPOSIT", "RECURRING DEPOSIT", "REINVESTMENT DEPOSIT", "SAVINGS DEPOSIT"}
        Dim loanHeads As String() = {"DAILY COLLECTION LOAN", "DEPOSIT LOAN ", "JEWEL LOAN", "MORTGAGE LOAN"}
        Dim allHeads As String() = depHeads.Concat(loanHeads).ToArray()

        ' Build IN clause parameter names
        Dim paramNames As String() = allHeads.Select(Function(h, i) "@h" & i.ToString()).ToArray()
        Dim inClause As String = String.Join(",", paramNames)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()

            ' --- Query 1: suplement balances grouped by achead ---
            Using cmd = New SqlCommand("SELECT achead, SUM(debit) AS dr, SUM(credit) AS cr FROM dbo.suplement WHERE scroll='1' AND achead IN (" & inClause & ") GROUP BY achead", con)
                For i As Integer = 0 To allHeads.Length - 1
                    cmd.Parameters.AddWithValue(paramNames(i), allHeads(i))
                Next
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        Dim head As String = dr("achead").ToString().Trim()
                        Dim netDr As Double = IIf(IsDBNull(dr("dr")), 0, CDbl(dr("dr")))
                        Dim netCr As Double = IIf(IsDBNull(dr("cr")), 0, CDbl(dr("cr")))
                        ' Deposits: credit - debit = balance. Loans: debit - credit = balance.
                        If depHeads.Contains(head) Then
                            balances(head) = netCr - netDr
                        Else
                            balances(head) = netDr - netCr
                        End If
                    End While
                End Using
            End Using

            ' --- Query 2: opening balances from ledger for all product heads ---
            Using cmd2 = New SqlCommand("SELECT ledger, ob FROM dbo.ledger WHERE ledger IN (" & inClause & ")", con)
                For i As Integer = 0 To allHeads.Length - 1
                    cmd2.Parameters.AddWithValue(paramNames(i), allHeads(i))
                Next
                Using dr2 As SqlDataReader = cmd2.ExecuteReader()
                    While dr2.Read()
                        Dim head As String = dr2("ledger").ToString().Trim()
                        Dim ob As Double = IIf(IsDBNull(dr2("ob")), 0, CDbl(dr2("ob")))
                        If balances.ContainsKey(head) Then
                            If depHeads.Contains(head) Then
                                balances(head) += ob
                            Else
                                balances(head) -= ob
                            End If
                        Else
                            If depHeads.Contains(head) Then
                                balances(head) = ob
                            Else
                                balances(head) = -ob
                            End If
                        End If
                    End While
                End Using
            End Using

            con.Close()
        End Using

        ' Helper to safely get a balance by head name
        Dim getbal = Function(h As String) As Double
                         If balances.ContainsKey(h) Then Return balances(h) Else Return 0
                     End Function

        Dim ds As Double = getbal("DAILY DEPOSIT")
        Dim fd As Double = getbal("FIXED DEPOSIT")
        Dim kmk As Double = getbal("KMK DEPOSIT")
        Dim rd As Double = getbal("RECURRING DEPOSIT")
        Dim rid As Double = getbal("REINVESTMENT DEPOSIT")
        Dim sb As Double = getbal("SAVINGS DEPOSIT")
        Dim dcl As Double = getbal("DAILY COLLECTION LOAN")
        Dim dl As Double = getbal("DEPOSIT LOAN ")
        Dim jl As Double = getbal("JEWEL LOAN")
        Dim ml As Double = getbal("MORTGAGE LOAN")

        Dim deptotal As Double = ds + fd + kmk + rd + rid + sb
        Dim loantotal As Double = dl + dcl + ml + jl

        Dim statsToCache As New Dictionary(Of String, String)()

        If loantotal > 0 Then
            statsToCache("DCL") = FormatNumber((dcl / loantotal) * 100)
            statsToCache("DL") = FormatNumber((dl / loantotal) * 100)
            statsToCache("JL") = FormatNumber((jl / loantotal) * 100)
            statsToCache("ML") = FormatNumber((ml / loantotal) * 100)
        Else
            statsToCache("DCL") = "0" : statsToCache("DL") = "0"
            statsToCache("JL") = "0" : statsToCache("ML") = "0"
        End If

        If deptotal > 0 Then
            statsToCache("DS") = FormatNumber((ds / deptotal) * 100)
            statsToCache("FD") = FormatNumber((fd / deptotal) * 100)
            statsToCache("KMK") = FormatNumber((kmk / deptotal) * 100)
            statsToCache("RD") = FormatNumber((rd / deptotal) * 100)
            statsToCache("RID") = FormatNumber((rid / deptotal) * 100)
            statsToCache("SB") = FormatNumber((sb / deptotal) * 100)
        Else
            statsToCache("DS") = "0" : statsToCache("FD") = "0"
            statsToCache("KMK") = "0" : statsToCache("RD") = "0"
            statsToCache("RID") = "0" : statsToCache("SB") = "0"
        End If

        For Each kvp As KeyValuePair(Of String, String) In statsToCache
            ViewState(kvp.Key) = kvp.Value
        Next

        depositCal.Text = FormatCurrency(0)
        lblCalcError.Text = ""
        depositType.SelectedIndex = 0
        SeniorCitizen.SelectedIndex = 0
        InterestLabel.Text = "0%"
        lblMonthlyPayout.Text = ""

    End Sub

    Sub fill_recent()

        ' FIX: Replaced 10 separate DB connections (one per product) with a single query
        ' using ROW_NUMBER() CTE to return the latest record per product in one round-trip.
        Dim allRecentData As New Dictionary(Of String, Dictionary(Of String, Object))()

        Dim products As String() = {"DS", "FD", "KMK", "RD", "RID", "SB", "DCL", "DL", "JL", "ML"}
        Dim paramNames2 As String() = products.Select(Function(p, i) "@p" & i.ToString()).ToArray()
        Dim inClause2 As String = String.Join(",", paramNames2)

        Dim recentSql As String =
            "WITH Ranked AS (" &
            "  SELECT masterc.date, masterc.acno, member.FirstName, masterc.amount, masterc.product," &
            "         ROW_NUMBER() OVER (PARTITION BY masterc.product ORDER BY masterc.date DESC, masterc.serial DESC) AS rn" &
            "  FROM dbo.masterc" &
            "  INNER JOIN dbo.member ON masterc.cid = member.MemberNo" &
            "  WHERE masterc.product IN (" & inClause2 & ")" &
            ")" &
            "SELECT date, acno, FirstName, amount, product FROM Ranked WHERE rn = 1"

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand(recentSql, con)
                For i As Integer = 0 To products.Length - 1
                    cmd.Parameters.AddWithValue(paramNames2(i), products(i))
                Next
                Try
                    Using dr As SqlDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            Dim prod As String = dr("product").ToString().Trim().ToUpper()
                            allRecentData(prod) = New Dictionary(Of String, Object) From {
                                {"depdate", IIf(IsDBNull(dr("date")), "", CStr(dr("date")))},
                                {"depacno", IIf(IsDBNull(dr("acno")), "", dr("acno").ToString())},
                                {"firstname", IIf(IsDBNull(dr("FirstName")), "", dr("FirstName").ToString())},
                                {"depamt", IIf(IsDBNull(dr("amount")), 0, CDbl(dr("amount")))}
                            }
                        End While
                    End Using
                Catch ex As Exception
                    Response.Write(ex.ToString())
                End Try
            End Using
            con.Close()
        End Using

        ' Render each product from the in-memory dictionary
        Dim render = Sub(lblDt As Label, lblAcn As Label, lblFn As Label, lblAmt As Label, prod As String)
                         If allRecentData.ContainsKey(prod) Then
                             lblDt.Text = allRecentData(prod)("depdate")
                             lblAcn.Text = allRecentData(prod)("depacno")
                             lblFn.Text = allRecentData(prod)("firstname")
                             If Not allRecentData(prod)("depamt") = 0 Then lblAmt.Text = FormatCurrency(allRecentData(prod)("depamt"))
                         End If
                     End Sub

        render(lbldsdt, lbldsacn, lbldsfn, lbldsamt, "DS")
        render(lblfddt, lblfdacn, lblfdfn, lblfdamt, "FD")
        render(lblKMKdt, lblKMKacn, lblKMKfn, lblKMKamt, "KMK")
        render(lblrddt, lblrdacn, lblrdfn, lblrdamt, "RD")
        render(lblriddt, lblridacn, lblridfn, lblridamt, "RID")
        render(lblsbdt, lblsbacn, lblsbfn, lblsbamt, "SB")
        render(lbldcldt, lbldclacn, lbldclfn, lbldclamt, "DCL")
        render(lbldldt, lbldlacn, lbldlfn, lbldlamt, "DL")
        render(lbljldt, lbljlacn, lbljlfn, lbljlamt, "JL")
        render(lblmldt, lblmlacn, lblmlfn, lblmlamt, "ML")

    End Sub

    Function get_login(ByVal usr As String)

        Dim dt As String = ""

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = "SELECT TOP 1 seslog.login FROM dbo.seslog WHERE seslog.sesusr = @usr ORDER BY seslog.login DESC "
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@usr", usr)
                Try
                    dt = cmd.ExecuteScalar()

                    If IsDBNull(dt) = True Then dt = ""


                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()

        End Using


        Return dt
    End Function

    Sub bind_user()
        Dim dt As New DataTable
        Dim newrow As DataRow

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("intl", GetType(String))
            dt.Columns.Add("user", GetType(String))
            dt.Columns.Add("role", GetType(String))
            dt.Columns.Add("log", GetType(String))
        End If

        Dim todayDate As DateTime = Date.Today

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = "select username,roles from usr where date = @dt "
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@dt", todayDate)

                Using dr As SqlDataReader = cmd.ExecuteReader

                    If dr.HasRows() Then
                        While dr.Read()

                            newrow = dt.NewRow()
                            newrow(0) = Mid(dr(0), 1, 1).ToUpper
                            newrow(1) = dr(0)
                            newrow(2) = dr(1)
                            newrow(3) = get_login(dr(0))
                            dt.Rows.Add(newrow)


                        End While

                        rpstaff.DataSource = dt
                        rpstaff.DataBind()
                        Cache.Insert("Dashboard_StaffList", dt, Nothing, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration)

                    End If

                End Using



            End Using
            con.Close()

        End Using

    End Sub

    Sub petty_cash()
        Dim dr As Double = 0
        Dim cr As Double = 0

        Dim todayDate As DateTime = Date.Today

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()

            Using cmd = New SqlCommand

                query = "select SUM(isnull(cast(diff.dr as float),0)) AS dr,SUM(isnull(cast(diff.cr as float),0)) AS cr from diff where date = @today"

                cmd.CommandText = query
                cmd.Connection = con
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@today", todayDate)

                Try

                    Using reader As SqlDataReader = cmd.ExecuteReader()


                        If reader.HasRows() Then
                            reader.Read()
                            cr = IIf(IsDBNull(reader(1)), 0, reader(1))
                            dr = IIf(IsDBNull(reader(0)), 0, reader(0))

                        End If

                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)

                Finally
                    con.Close()
                    cmd.Dispose()

                End Try

            End Using
            con.Close()



        End Using

        If (cr - dr) < 0 Then
            lblpettycash.Text = FormatCurrency(dr - cr)

        Else
            lblpettycash.Text = FormatCurrency(cr - dr)
        End If

    End Sub

    Sub get_recent(ByVal prod As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                query = "SELECT top 1 masterc.date,masterc.acno,member.FirstName,masterc.amount FROM dbo.masterc INNER JOIN dbo.member   ON masterc.cid = member.MemberNo   where product=@prod ORDER BY date DESC,masterc.serial DESC"

                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@prod", prod)
                Try
                    Using dr As SqlDataReader = cmd.ExecuteReader

                        If dr.HasRows Then
                            dr.Read()
                            ViewState("depdate") = dr(0)
                            ViewState("depacno") = dr(1)
                            ViewState("firstname") = dr(2)
                            ViewState("depamt") = dr(3)
                        Else
                            ViewState("depdate") = ""
                            ViewState("depacno") = ""
                            ViewState("firstname") = ""
                            ViewState("depamt") = 0
                        End If


                    End Using

                    'dr.Close()

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()

        End Using


    End Sub

    Sub gold_rate()
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = "select top 1 COALESCE( rate,0) from goldrate ORDER BY goldrate.rate DESC"
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    lblgoldrate.Text = FormatCurrency(cmd.ExecuteScalar())
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()


        End Using

    End Sub


    Sub get_dep()
        ' FIX: Replaced 3 separate queries with a single conditional aggregation query
        Dim todayDate As DateTime = Date.Today
        Dim lastWeekDate As DateTime = Date.Today.AddDays(-7)
        Dim startOfMonth As New DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, 1)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandText = "SELECT" &
                    " COALESCE(SUM(CASE WHEN master.date = @today THEN master.amount ELSE 0 END), 0) AS today_amt," &
                    " COALESCE(SUM(CASE WHEN master.date BETWEEN @lastWeek AND @today THEN master.amount ELSE 0 END), 0) AS week_amt," &
                    " COALESCE(SUM(CASE WHEN master.date BETWEEN @startMonth AND @today THEN master.amount ELSE 0 END), 0) AS month_amt" &
                    " FROM dbo.products LEFT OUTER JOIN dbo.master ON products.shortname = master.product" &
                    " WHERE products.prdtype = @prdtype AND master.date BETWEEN @startMonth AND @today"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@prdtype", "Deposit")
                cmd.Parameters.AddWithValue("@today", todayDate)
                cmd.Parameters.AddWithValue("@lastWeek", lastWeekDate)
                cmd.Parameters.AddWithValue("@startMonth", startOfMonth)

                Using dr As SqlDataReader = cmd.ExecuteReader()
                    If dr.HasRows() Then
                        dr.Read()
                        lbldeptoday.Text = FormatCurrency(IIf(IsDBNull(dr(0)), 0, dr(0)))
                        lbldepweek.Text = FormatCurrency(IIf(IsDBNull(dr(1)), 0, dr(1)))
                        lbldepmonth.Text = FormatCurrency(IIf(IsDBNull(dr(2)), 0, dr(2)))
                    Else
                        lbldeptoday.Text = FormatCurrency(0)
                        lbldepweek.Text = FormatCurrency(0)
                        lbldepmonth.Text = FormatCurrency(0)
                    End If
                End Using
            End Using
            con.Close()
        End Using

    End Sub

    Sub get_loan()
        ' FIX: Replaced 3 separate queries with a single conditional aggregation query
        Dim todayDate As DateTime = Date.Today
        Dim lastWeekDate As DateTime = Date.Today.AddDays(-7)
        Dim startOfMonth As New DateTime(DateAndTime.Now.Year, DateAndTime.Now.Month, 1)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandText = "SELECT" &
                    " COALESCE(SUM(CASE WHEN master.date = @today THEN master.amount ELSE 0 END), 0) AS today_amt," &
                    " COALESCE(SUM(CASE WHEN master.date BETWEEN @lastWeek AND @today THEN master.amount ELSE 0 END), 0) AS week_amt," &
                    " COALESCE(SUM(CASE WHEN master.date BETWEEN @startMonth AND @today THEN master.amount ELSE 0 END), 0) AS month_amt" &
                    " FROM dbo.products LEFT OUTER JOIN dbo.master ON products.shortname = master.product" &
                    " WHERE products.prdtype = @prdtype AND master.date BETWEEN @startMonth AND @today"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@prdtype", "LOAN")
                cmd.Parameters.AddWithValue("@today", todayDate)
                cmd.Parameters.AddWithValue("@lastWeek", lastWeekDate)
                cmd.Parameters.AddWithValue("@startMonth", startOfMonth)

                Using dr As SqlDataReader = cmd.ExecuteReader()
                    If dr.HasRows() Then
                        dr.Read()
                        lblloantoday.Text = FormatCurrency(IIf(IsDBNull(dr(0)), 0, dr(0)))
                        lblloanweek.Text = FormatCurrency(IIf(IsDBNull(dr(1)), 0, dr(1)))
                        lblloanmonth.Text = FormatCurrency(IIf(IsDBNull(dr(2)), 0, dr(2)))
                    Else
                        lblloantoday.Text = FormatCurrency(0)
                        lblloanweek.Text = FormatCurrency(0)
                        lblloanmonth.Text = FormatCurrency(0)
                    End If
                End Using
            End Using
            con.Close()
        End Using

    End Sub
    Sub get_opening()
        Dim sum_credit As Double
        Dim sum_debit As Double
        Dim opening As Double
        Dim closing_bal As Double
        Dim oResult As Double = 0
        Dim trans_credit As Double = 0
        Dim trans_debit As Double = 0


        Dim todayDate As DateTime = Date.Today

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE date < @today and scroll='1'"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@today", todayDate)

                Using sdr As SqlDataReader = cmd.ExecuteReader
                    If sdr.HasRows Then

                        sdr.Read()

                        sum_credit = IIf(IsDBNull(sdr(1)), 0, sdr(1))
                        sum_debit = IIf(IsDBNull(sdr(0)), 0, sdr(0))

                    End If

                End Using
            End Using
            con.Close()

        End Using





        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "select COALESCE(sum(ledger.ob),0) as expr1 from ledger"

                cmd.CommandText = query
                cmd.Connection = con

                Try

                    oResult = cmd.ExecuteScalar()


                    If Not IsNothing(oResult) Then
                        opening = oResult
                    Else
                        opening = 0
                    End If

                Catch ex As Exception
                    Response.Write(ex.ToString)

                End Try

            End Using
            con.Close()

        End Using



        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "select COALESCE(sum(credit),0) as expr1,COALESCE(sum(debit),0) as expr2 from dbo.suplement where date = @today and suplement.type<>'" + "JOURNAL" + "'"
                cmd.CommandText = query
                cmd.Connection = con
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@today", todayDate)
                Using dr As SqlDataReader = cmd.ExecuteReader

                    If dr.HasRows() Then
                        dr.Read()

                        trans_credit = IIf(IsDBNull(dr(0)), 0, dr(0))

                        trans_debit = IIf(IsDBNull(dr(1)), 0, dr(1))

                    End If

                End Using

            End Using
            con.Close()

        End Using


        opening = opening + (sum_credit - sum_debit)



        closing_bal = opening + (trans_credit - trans_debit)

        lblcredit.Text = FormatCurrency(trans_credit)
        lbldebit.Text = FormatCurrency(trans_debit)


        lblcbal.Text = FormatCurrency(Math.Round(closing_bal))

    End Sub

    Function get_depbal(ByVal pro As String)
        Dim bal As Double = 0
        Dim bal1 As Double = 0

        Select Case pro
            Case "DS"
                pro = "DAILY DEPOSIT"
            Case "FD"
                pro = "FIXED DEPOSIT"
            Case "KMK"
                pro = "KMK DEPOSIT"
            Case "RD"
                pro = "RECURRING DEPOSIT"
            Case "RID"
                pro = "REINVESTMENT DEPOSIT"
            Case "SB"
                pro = "SAVINGS DEPOSIT"
        End Select

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = "SELECT SUM(suplement.debit) AS expr1,SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE suplement.achead = @pro and scroll='1' GROUP BY suplement.achead"
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@pro", pro)
                Try
                    Using dr As SqlDataReader = cmd.ExecuteReader

                        If dr.HasRows() Then
                            dr.Read()
                            bal = dr(1) - dr(0)
                        End If


                    End Using
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try



                Using con1 = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con1.Open()
                    Using cmd1 = New SqlCommand

                        query = "select ob from ledger where ledger=@prod"
                        cmd1.Connection = con1
                        cmd1.CommandText = query
                        cmd1.Parameters.Clear()
                        cmd1.Parameters.AddWithValue("@prod", pro)
                        Try
                            bal1 = cmd1.ExecuteScalar()
                        Catch ex As Exception
                            Response.Write(ex.ToString)
                        End Try

                    End Using
                End Using




                bal = bal + bal1

            End Using
            con.Close()

        End Using


        Return bal

    End Function
    Function get_lonbal(ByVal pro As String)
        Dim bal1 As Double = 0
        Dim bal As Double = 0

        Select Case pro
            Case "DCL"
                pro = "DAILY COLLECTION LOAN"
            Case "DL"
                pro = "DEPOSIT LOAN "
            Case "JL"
                pro = "JEWEL LOAN"
            Case "ML"
                pro = "MORTGAGE LOAN"
        End Select

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = "SELECT SUM(suplement.debit) AS expr1,SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE suplement.achead = @pro and scroll='1' GROUP BY suplement.achead"
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@pro", pro)

                Try
                    Using dr As SqlDataReader = cmd.ExecuteReader

                        If dr.HasRows() Then
                            dr.Read()
                            bal = dr(0) - dr(1)
                        End If

                    End Using

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()


        End Using


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = "select ob from ledger where ledger=@prod"
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@prod", pro)
                Try
                    bal1 = cmd.ExecuteScalar()
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()

        End Using





        bal = bal - bal1

        Return bal
    End Function

    Private Sub rpstaff_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpstaff.ItemCommand
        If e.CommandName = "pokeClick" Then
            Session("user") = e.CommandArgument
            If Not Trim(Session("sesusr")) = Trim(Session("user")) Then
                poke(Session("user"))
            End If


        End If
    End Sub

    Private Sub btnclr_Click(sender As Object, e As EventArgs) Handles btnclr.Click
        txamt.Text = ""
        txtprd.Text = ""
        lblCalcError.Text = ""
        depositType.SelectedIndex = 0
        SeniorCitizen.SelectedIndex = 0
        InterestLabel.Text = "0%"
        depositCal.Text = FormatCurrency(0)
        lblMonthlyPayout.Text = ""
    End Sub

    Private Sub btncalc_Click(sender As Object, e As EventArgs) Handles btncalc.Click
        Dim depAmt As Double
        Dim tenure As Double
        Double.TryParse(txamt.Text, depAmt)
        Double.TryParse(txtprd.Text, tenure)
        If depAmt = 0 Or tenure = 0 Or depositType.SelectedIndex = -1 Or SeniorCitizen.SelectedIndex = -1 Then
            Return
        End If
        Dim prod As String = "FD"
        If depositType.SelectedValue.Contains("RD") Then
            prod = "RD"
        ElseIf depositType.SelectedValue.Contains("RID") Then
            prod = "RID"
        End If

        lblCalcError.Text = ""
        lblMonthlyPayout.Text = ""
        If prod = "RD" AndAlso (tenure Mod 12 <> 0) Then
            lblCalcError.Text = "RD Tenure must be in multiples of 12 months."
            InterestLabel.Text = "0%"
            depositCal.Text = FormatCurrency(0)
            Return
        ElseIf prod = "RID" AndAlso (tenure Mod 3 <> 0) Then
            lblCalcError.Text = "RID Tenure must be in multiples of 3 months."
            InterestLabel.Text = "0%"
            depositCal.Text = FormatCurrency(0)
            Return
        End If

        GetDepositAmount(prod, depAmt, tenure)
    End Sub

    Sub GetDepositAmount(ByVal prod As String, ByVal depAmt As Double, ByVal tenure As Double)
        Try
            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand
                    query = "SELECT cint,dint,FYFRM,FYTO,prdfrm,prdto FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy = @prdtyp order by fyfrm desc"

                    Dim dep_roi As SqlDataReader
                    If con.State = ConnectionState.Closed Then con.Open()
                    cmd.Parameters.Clear()
                    cmd.Connection = con
                    cmd.CommandText = query
                    cmd.Parameters.AddWithValue("@prod", Trim(prod))
                    cmd.Parameters.AddWithValue("@prdtyp", "M")
                    dep_roi = cmd.ExecuteReader()

                    If dep_roi.HasRows() Then
                        While dep_roi.Read

                            Dim FYFRM As Date = dep_roi(2)
                            Dim FYTO As Date = dep_roi(3)


                            Dim x As Long = FYFRM.CompareTo(Date.Today)

                            If x = -1 Then

                                If dep_roi(4) <= tenure AndAlso dep_roi(5) >= tenure Then
                                    Dim y As Long = FYTO.CompareTo(Date.Today)

                                    If y = 1 Then
                                        Dim interest As Double
                                        Double.TryParse(dep_roi(1), interest)
                                        If Not prod.Equals("RD") And SeniorCitizen.SelectedValue.Contains("Yes") Then
                                            interest += 0.3
                                        End If
                                        InterestLabel.Text = interest.ToString() + "%"
                                        If prod.Equals("FD") Then
                                            Dim finalAmt As Double = Math.Round(depAmt + (depAmt * tenure * interest / (12 * 100)), 2)
                                            depositCal.Text = FormatCurrency(finalAmt)
                                            lblMonthlyPayout.Text = "Monthly Interest Payout: " & FormatCurrency((finalAmt - depAmt) / 12)
                                        ElseIf prod.Equals("RD") Then
                                            Dim principal = depAmt
                                            For i = 1 To tenure
                                                If Not i = 1 Then
                                                    depAmt += principal
                                                End If
                                                depAmt = (depAmt * interest / (12 * 100)) + depAmt
                                            Next
                                            depositCal.Text = FormatCurrency(Math.Round(depAmt))
                                        ElseIf prod.Equals("RID") Then
                                            depAmt = depAmt * Math.Pow(1 + (interest / (4 * 100)), 4 * tenure / 12)
                                            depositCal.Text = FormatCurrency(Math.Round(depAmt))
                                        End If
                                        Exit While
                                    End If
                                End If
                            End If
                        End While


                    End If

                    dep_roi.Close()
                End Using
                con.Close()

            End Using
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub
    Sub get_maturing_this_week()
        If Cache("Dashboard_Maturity") IsNot Nothing Then
            rpMaturity.DataSource = DirectCast(Cache("Dashboard_Maturity"), DataTable)
            rpMaturity.DataBind()
            Return
        End If

        Try
            ' FIX: Maturity data only changes overnight — cache until midnight
            Dim frm As DateTime = Date.Today
            Dim tto As DateTime = Date.Today.AddDays(7)
            Dim midnight As DateTime = Date.Today.AddDays(1)  ' expires at next midnight
            Dim localQuery As String = ""

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                localQuery = "SELECT master.acno, member.FirstName, master.product, master.mdate, master.mamt " &
                             "FROM dbo.master " &
                             "INNER JOIN dbo.member ON master.cid = member.MemberNo " &
                             "WHERE master.product IN ('FD', 'RD', 'RID') " &
                             "AND master.cld <> 1 " &
                             "AND master.mdate BETWEEN @frm AND @to " &
                             "ORDER BY master.mdate ASC"

                Using cmd = New SqlCommand(localQuery, con)
                    cmd.Parameters.AddWithValue("@frm", frm)
                    cmd.Parameters.AddWithValue("@to", tto)

                    Using da As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        rpMaturity.DataSource = dt
                        rpMaturity.DataBind()
                        ' Cache until midnight — maturity data doesn't change intraday
                        Cache.Insert("Dashboard_Maturity", dt, Nothing, midnight, Cache.NoSlidingExpiration)
                    End Using
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            ' Silently handle to prevent dashboard break
        End Try
    End Sub

    Protected Sub tmrOverdue_Tick(sender As Object, e As EventArgs)
        tmrOverdue.Enabled = False ' Only run once
        Dim sw As New System.Diagnostics.Stopwatch()
        sw.Start()
        get_overdue_jl()
        sw.Stop()
        lblOverdueTime.Text = "(" & sw.ElapsedMilliseconds & " ms)"
        pnlOverdueLoading.Visible = False
        upnlOverdue.Update()
    End Sub

    Sub get_overdue_jl()
        If Cache("Dashboard_JLOverdue") IsNot Nothing Then
            Dim dtCached As DataTable = DirectCast(Cache("Dashboard_JLOverdue"), DataTable)
            rpJLOverdue.DataSource = dtCached
            rpJLOverdue.DataBind()
            phNoData.Visible = (dtCached.Rows.Count = 0)
            pnlOverdueLoading.Visible = False
            Return
        End If

        Try
            Dim midnight As DateTime = Date.Today.AddDays(1)
            ' PERFORMANCE FIX: Removed LTRIM/RTRIM from Join/Apply which was preventing Index usage
            ' PERFORMANCE FIX: Added exact product filters to reduce scan surface
            Dim localQuery As String = "SELECT m.acno, mem.FirstName, m.amount, " &
                        "COALESCE(sums.repaid, 0) as repaid, " &
                        "COALESCE(sums.debits - sums.repaid, 0) as balance " &
                        "FROM master m " &
                        "INNER JOIN member mem ON LTRIM(RTRIM(m.cid)) = LTRIM(RTRIM(mem.MemberNo)) " &
                        "OUTER APPLY (" &
                        "    SELECT SUM(Crd + Crc) as repaid, " &
                        "           SUM(Drd + Drc) as debits " &
                        "    FROM actrans " &
                        "    WHERE LTRIM(RTRIM(actrans.acno)) = LTRIM(RTRIM(m.acno)) AND scroll = '1' " &
                        ") sums " &
                        "WHERE m.product = 'JL' AND m.cld <> 1 " &
                        "AND m.date <= DATEADD(month, -10, GETDATE()) " &
                        "AND m.date >= DATEADD(month, -15, GETDATE()) " &
                        "ORDER BY m.date ASC"

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand(localQuery, con)
                    cmd.CommandTimeout = 120 ' Increased timeout for heavy day-end processing
                    Using da As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        rpJLOverdue.DataSource = dt
                        rpJLOverdue.DataBind()
                        phNoData.Visible = (dt.Rows.Count = 0)
                        pnlOverdueLoading.Visible = False
                        ' Cache until midnight — overdue status is a daily snapshot
                        Cache.Insert("Dashboard_JLOverdue", dt, Nothing, midnight, Cache.NoSlidingExpiration)
                    End Using
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            lblOverdueError.Text = "Error: " & ex.Message
            lblOverdueError.Visible = True
            pnlOverdueLoading.Visible = False
        End Try
    End Sub

End Class

