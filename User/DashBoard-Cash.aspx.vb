Imports System.Data.SqlClient
Imports System.Globalization

Public Class DashBoard_Cash
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then

            dt.Text = String.Format("{0:dd MMMM yyyy}", DateAndTime.Today)

            ViewState("RefUrl") = Request.UrlReferrer.ToString()
            get_opening()
            bind_rp()
            bind_user()

            scrollcount()

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
                query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
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
                query = "select COALESCE(sum(credit),0) as expr1,COALESCE(sum(debit),0) as expr2 from dbo.suplement where convert(varchar(20),date,112) ='" + reformatted + "' and suplement.type<>'" + "JOURNAL" + "'"
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

    Sub bind_rp()


        rpscroll.DataSource = Nothing
        rpscroll.DataBind()

        Session("passed_v") = 0


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "getDenom"
                cmd.Parameters.Clear()

                cmd.Parameters.Add(New SqlParameter("@cld", Session("passed_v")))

                Try

                    Using Sda As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable
                        Sda.Fill(dt)


                        rpscroll.DataSource = dt
                        rpscroll.DataBind()
                        'get_locked(dt)

                        If dt.Rows.Count = 0 Then pnldenom.Visible = False

                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try


            End Using
            con.Close()

        End Using

    End Sub

    Private Sub rpscroll_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpscroll.ItemCommand
        If e.CommandName = "ViewClick" Then
            ''MsgBox(e.CommandArgument)
            Session("tid") = e.CommandArgument

            Response.Redirect("../denomination.aspx")



        End If
    End Sub

    Private Sub rpstaff_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpstaff.ItemCommand
        If e.CommandName = "pokeClick" Then
            Session("user") = e.CommandArgument
            If Not Trim(Session("sesusr")) = Trim(Session("user")) Then
                poke(Session("user"))
            End If


        End If
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

End Class