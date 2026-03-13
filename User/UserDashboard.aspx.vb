Imports System.Data.SqlClient
Imports System.Globalization

Partial Public Class UserDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            dt.Text = String.Format("{0:dd MMMM yyyy}", DateAndTime.Today)

            ViewState("RefUrl") = Request.UrlReferrer.ToString()
            get_opening()
            gold_rate()
            petty_cash()
            fill_recent()
            bind_user()
            scrollcount()

            If session_user_role = "Audit" Then
                pnlpc.Visible = False

            End If

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

    Sub bind_user()

        Dim dt As New DataTable
        Dim newrow As DataRow

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("intl", GetType(String))
            dt.Columns.Add("user", GetType(String))
            dt.Columns.Add("role", GetType(String))
            dt.Columns.Add("log", GetType(String))
        End If

        Dim dat As DateTime = DateTime.ParseExact(DateAndTime.Today, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = "select username,roles from usr where CONVERT(VARCHAR(20), date, 112) =@dt "
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@dt", reformatted)

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


                    End If

                End Using



            End Using
            con.Close()

        End Using

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

    Sub fill_recent()

        get_recent("DS")

        lbldsdt.Text = Session("depdate")
        lbldsacn.Text = Session("depacno")
        lbldsfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lbldsamt.Text = FormatCurrency(Session("depamt"))


        get_recent("FD")

        lblfddt.Text = Session("depdate")
        lblfdacn.Text = Session("depacno")
        lblfdfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lblfdamt.Text = FormatCurrency(Session("depamt"))

        get_recent("KMK")

        lblKMKdt.Text = Session("depdate")
        lblKMKacn.Text = Session("depacno")
        lblKMKfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lblKMKamt.Text = FormatCurrency(Session("depamt"))

        get_recent("RD")

        lblrddt.Text = Session("depdate")
        lblrdacn.Text = Session("depacno")
        lblrdfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lblrdamt.Text = FormatCurrency(Session("depamt"))
        get_recent("RID")

        lblriddt.Text = Session("depdate")
        lblridacn.Text = Session("depacno")
        lblridfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lblridamt.Text = FormatCurrency(Session("depamt"))

        get_recent("SB")

        lblsbdt.Text = Session("depdate")
        lblsbacn.Text = Session("depacno")
        lblsbfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lblsbamt.Text = FormatCurrency(Session("depamt"))

        get_recent("DCL")

        lbldcldt.Text = Session("depdate")
        lbldclacn.Text = Session("depacno")
        lbldclfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lbldclamt.Text = FormatCurrency(Session("depamt"))
        get_recent("DL")

        lbldldt.Text = Session("depdate")
        lbldlacn.Text = Session("depacno")
        lbldlfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lbldlamt.Text = FormatCurrency(Session("depamt"))
        get_recent("jL")

        lbljldt.Text = Session("depdate")
        lbljlacn.Text = Session("depacno")
        lbljlfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lbljlamt.Text = FormatCurrency(Session("depamt"))
        get_recent("ML")

        lblmldt.Text = Session("depdate")
        lblmlacn.Text = Session("depacno")
        lblmlfn.Text = Session("firstname")
        If Not Session("depamt") = 0 Then lblmlamt.Text = FormatCurrency(Session("depamt"))



    End Sub

    Sub petty_cash()

        Dim dr As Double = 0
        Dim cr As Double = 0

        Dim dat As DateTime = DateTime.ParseExact(DateAndTime.Today, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()

            Using cmd = New SqlCommand

                query = "select SUM(isnull(cast(diff.dr as float),0)) AS dr,SUM(isnull(cast(diff.cr as float),0)) AS cr from diff where CONVERT(VARCHAR(20), date, 112) ='" + reformatted + "'"



                cmd.CommandText = query
                cmd.Connection = con

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
                            Session("depdate") = dr(0)
                            Session("depacno") = dr(1)
                            Session("firstname") = dr(2)
                            Session("depamt") = dr(3)
                        Else
                            Session("depdate") = ""
                            Session("depacno") = ""
                            Session("firstname") = ""
                            Session("depamt") = 0
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


    Sub get_opening()

        Dim sum_credit As Double
        Dim sum_debit As Double
        Dim opening As Double
        Dim closing_bal As Double
        Dim oResult As Double = 0
        Dim trans_credit As Double = 0
        Dim trans_debit As Double = 0


        Dim dat As DateTime = DateTime.ParseExact(DateAndTime.Today, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)



        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    query = "SELECT SUM(suplementc.debit) AS expr1, SUM(suplementc.credit) AS expr2 FROM dbo.suplementc WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
                Else
                    query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
                End If

                cmd.CommandText = query

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



        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 105) <'" + Convert.ToDateTime(txtdate.Text)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    query = "select COALESCE(sum(credit),0) as expr1,COALESCE(sum(debit),0) as expr2 from dbo.suplementc where convert(varchar(20),date,112) ='" + reformatted + "' and suplementc.type<>'" + "JOURNAL" + "'"
                Else
                    query = "select COALESCE(sum(credit),0) as expr1,COALESCE(sum(debit),0) as expr2 from dbo.suplement where convert(varchar(20),date,112) ='" + reformatted + "' and suplement.type<>'" + "JOURNAL" + "'"
                End If

                cmd.CommandText = query
                cmd.Connection = con
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


    Private Sub UserDashboard_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            If session_user_role = "Cashier" Then Response.Redirect("DashBoard-Cash.aspx")
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class