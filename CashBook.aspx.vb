Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization

Public Class CashBook
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Dim newrow As DataRow

    Dim countresult As Integer
    Public dt_dl As New DataTable
    Public dt As New DataTable
    Public dtc As New DataTable
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim yearpart As String


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        con.Open()



        If Not Page.IsPostBack Then

            txtdate.Text = Date.Today
            bind_grid()


            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtdate.ClientID), True)



        End If

    End Sub

    Sub bind_grid()


        Dim ds As New DataSet
        Dim ds1 As New DataSet

        If con.State = ConnectionState.Closed Then con.Open()

        Dim dat1 As DateTime = DateTime.ParseExact(Today, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        cmd.Parameters.Clear()

        Dim qResult As Integer = 0

        query = "select count(*) as expr1 from trans where scroll='0'"
        cmd.Connection = con
        cmd.CommandText = query

        Try
            qResult = cmd.ExecuteScalar()

            If reformatted = reformatted1 Then

                If qResult > 0 Then

                    disp.DataSource = Nothing
                    disp.DataBind()
                    dispdebit.DataSource = Nothing
                    dispdebit.DataBind()

                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

                    sb.Append("<div class=" + """alert alert-dismissable alert-danger """ + ">")
                    sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
                    sb.Append("<strong>Vouchers are Pending to Authorize</strong> ")
                    sb.Append("</div>")
                    ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
                    btn_up_pd.Enabled = False
                    Exit Sub

                End If
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()

        End Try

        If con.State = ConnectionState.Closed Then con.Open()


        query = "SELECT   products.shortname, trans.acno,  trans.drc,  trans.crc,  member.FirstName,  trans.suplimentry,  trans.Id,  trans.date FROM dbo.trans "
        query &= " FULL OUTER JOIN dbo.products   ON trans.suplimentry = products.name FULL OUTER JOIN dbo.master   ON master.acno = trans.acno FULL OUTER JOIN dbo.member  ON master.cid = member.MemberNo "
        query &= " where CONVERT(VARCHAR(20), trans.date, 112) ='" + reformatted + "' and trans.type='" + "PAYMENT" + "' ORDER BY trans.Id"


        ' query = "select id,acno as acn,suplimentry,crc from trans where CONVERT(VARCHAR(20), date, 112) ='" + reformatted + "' and trans.crd > '0' and trans.type='CASH' "
        'cmd.Parameters.AddWithValue("@date", reformatted)
        'cmd.Parameters.AddWithValue("@cnt", 0)

        cmd.CommandText = query
        cmd.Connection = con

        Session("dispsrc") = "TRANS"
        Try


            Dim adapter As New SqlDataAdapter(query, con)

            adapter.Fill(ds)

            If Not ds.Tables(0).Rows.Count = 0 Then

                disp.DataSource = ds
                disp.DataBind()
            Else
                adapter.Dispose()
                ds.Dispose()

                'query = "select TRANSid AS id,acn,achead AS suplimentry,credit AS crc from suplement where CONVERT(VARCHAR(20), date, 112) ='" + reformatted + "' and suplement.type='" + "RECEIPT" + "'"
                ''cmd.Parameters.AddWithValue("@date", reformatted)
                ''cmd.Parameters.AddWithValue("@cnt", 0)

                query = "SELECT   products.shortname, trans.acno,  trans.drc,  trans.crc,  member.FirstName,  trans.suplimentry,  trans.Id,  trans.date FROM dbo.trans "
                query &= " FULL OUTER JOIN dbo.products   ON trans.suplimentry = products.name FULL OUTER JOIN dbo.master   ON master.acno = trans.acno FULL OUTER JOIN dbo.member  ON master.cid = member.MemberNo "
                query &= " where CONVERT(VARCHAR(20), trans.date, 112) ='" + reformatted + "' and trans.type='" + "RECEIPT" + "' ORDER BY trans.Id"

                cmd.CommandText = query
                cmd.Connection = con

                Session("dispsrc") = "SUPLEMENT"

                If session_user_role = "Audit" Then
                    query = " SELECT   products.shortname,  suplementc.acn, suplementc.debit, suplementc.credit, member.FirstName,suplementc.achead,suplementc.transid "
                    query &= " FROM dbo.suplementc FULL OUTER JOIN dbo.products  ON suplementc.achead = products.name FULL OUTER JOIN dbo.masterc ON masterc.acno = suplementc.acn FULL OUTER JOIN dbo.member ON masterc.cid = member.MemberNo "
                    query &= " where CONVERT(VARCHAR(20), suplementc.date, 112) ='" + reformatted + "' and suplementc.type='" + "RECEIPT" + "' ORDER BY suplementc.tId"


                Else
                    query = " SELECT   products.shortname,  suplement.acn, suplement.debit, suplement.credit, member.FirstName,suplement.achead,suplement.transid "
                    query &= " FROM dbo.suplement FULL OUTER JOIN dbo.products  ON suplement.achead = products.name FULL OUTER JOIN dbo.master ON master.acno = suplement.acn FULL OUTER JOIN dbo.member ON master.cid = member.MemberNo "
                    query &= " where CONVERT(VARCHAR(20), suplement.date, 112) ='" + reformatted + "' and suplement.type='" + "RECEIPT" + "' ORDER BY suplement.tId"

                End If

                cmd.CommandText = query
                cmd.Connection = con


                'Dim adapter2 As New SqlDataAdapter(query, con)

                'adapter2.Fill(ds1)

                ''  If Not ds1.Tables(0).Rows.Count = 0 Then

                'dispdebit.DataSource = ds1
                'dispdebit.DataBind()

                ' End If

                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader()
                If dr.HasRows() Then

                    While dr.Read


                        If dtc.Columns.Count = 0 Then

                            dtc.Columns.Add("id", GetType(String))

                            dtc.Columns.Add("dep", GetType(String))
                            dtc.Columns.Add("acn", GetType(String))
                            dtc.Columns.Add("achead", GetType(String))
                            dtc.Columns.Add("crc", GetType(String))

                            'dt.Columns.Add("acn", GetType(String))
                        End If

                        newrow = dtc.NewRow

                        newrow(0) = dr(6)


                        If IsDBNull(dr(0)) Then

                            newrow(1) = IIf(IsDBNull(dr(1)), " ", dr(1))
                            newrow(2) = "GL"
                            newrow(3) = dr(5)
                        Else
                            newrow(1) = IIf(IsDBNull(dr(1)), " ", dr(1))
                            newrow(2) = IIf(IsDBNull(dr(0)), " ", dr(0))
                            newrow(3) = dr(4)
                        End If
                        newrow(4) = String.Format("{0:N}", IIf(dr(3) = 0, " ", dr(3)))


                        dtc.Rows.Add(newrow)
                    End While

                End If


                disp.DataSource = dtc
                disp.DataBind()


            End If


        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()


        End Try


        If con.State = ConnectionState.Closed Then con.Open()

        ' query = "select id,acno as acn,suplimentry,drc from trans where CONVERT(VARCHAR(20), date, 112) ='" + reformatted + "' and trans.drd > '0' and trans.type='CASH' "

        query = "SELECT   products.shortname, trans.acno,  trans.drc,  trans.crc,  member.FirstName,  trans.suplimentry,  trans.Id,  trans.date FROM dbo.trans "
        query &= " FULL OUTER JOIN dbo.products   ON trans.suplimentry = products.name FULL OUTER JOIN dbo.master   ON master.acno = trans.acno FULL OUTER JOIN dbo.member  ON master.cid = member.MemberNo "
        query &= " where CONVERT(VARCHAR(20), trans.date, 112) ='" + reformatted + "' and trans.type='" + "PAYMENT" + "' ORDER BY trans.Id"


        cmd.CommandText = query
        cmd.Connection = con

        Try

            Dim adapter1 As New SqlDataAdapter(query, con)

            adapter1.Fill(ds1)

            If Not ds1.Tables(0).Rows.Count = 0 Then
                Session("dispsrc") = "TRANS"
                dispdebit.DataSource = ds1
                dispdebit.DataBind()
            Else


                ''query = "select TRANSid AS id,acn,achead AS suplimentry,DEBIT AS drc from suplement where CONVERT(VARCHAR(20), date, 112) ='" + reformatted + "' and suplement.type='" + "PAYMENT" + "'"
                'cmd.Parameters.AddWithValue("@date", reformatted)
                'cmd.Parameters.AddWithValue("@cnt", 0)

                If session_user_role = "Audit" Then
                    query = " SELECT   products.shortname,  suplementc.acn, suplementc.debit, suplementc.credit, member.FirstName,suplementc.achead,suplementc.transid "
                    query &= " FROM dbo.suplementc FULL OUTER JOIN dbo.products  ON suplementc.achead = products.name FULL OUTER JOIN dbo.masterc ON masterc.acno = suplementc.acn FULL OUTER JOIN dbo.member ON masterc.cid = member.MemberNo "
                    query &= " where CONVERT(VARCHAR(20), suplementc.date, 112) ='" + reformatted + "' and suplementc.type='" + "PAYMENT" + "' ORDER BY suplementc.tId"

                Else

                    query = " SELECT   products.shortname,  suplement.acn, suplement.debit, suplement.credit, member.FirstName,suplement.achead,suplement.transid "
                    query &= " FROM dbo.suplement FULL OUTER JOIN dbo.products  ON suplement.achead = products.name FULL OUTER JOIN dbo.master ON master.acno = suplement.acn FULL OUTER JOIN dbo.member ON master.cid = member.MemberNo "
                    query &= " where CONVERT(VARCHAR(20), suplement.date, 112) ='" + reformatted + "' and suplement.type='" + "PAYMENT" + "' ORDER BY suplement.tId"

                End If


                cmd.CommandText = query
                cmd.Connection = con


                'Dim adapter2 As New SqlDataAdapter(query, con)

                'adapter2.Fill(ds1)

                ''  If Not ds1.Tables(0).Rows.Count = 0 Then

                'dispdebit.DataSource = ds1
                'dispdebit.DataBind()

                ' End If

                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader()
                If dr.HasRows() Then

                    While dr.Read


                        If dt.Columns.Count = 0 Then

                            dt.Columns.Add("id", GetType(String))

                            dt.Columns.Add("dep", GetType(String))
                            dt.Columns.Add("acn", GetType(String))
                            dt.Columns.Add("achead", GetType(String))
                            dt.Columns.Add("drc", GetType(String))

                            'dt.Columns.Add("acn", GetType(String))
                        End If

                        newrow = dt.NewRow

                        newrow(0) = dr(6)


                        If IsDBNull(dr(0)) Then

                            newrow(1) = IIf(IsDBNull(dr(1)), " ", dr(1))
                            newrow(2) = "GL"
                            newrow(3) = dr(5)
                        Else
                            newrow(1) = IIf(IsDBNull(dr(1)), " ", dr(1))
                            newrow(2) = IIf(IsDBNull(dr(0)), " ", dr(0))
                            newrow(3) = dr(4)
                        End If
                        newrow(4) = String.Format("{0:N}", IIf(dr(2) = 0, " ", dr(2)))


                        dt.Rows.Add(newrow)
                    End While

                End If


                dispdebit.DataSource = dt
                dispdebit.DataBind()


            End If



        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()


        End Try



        'pnlcb.Visible = True
        get_opening()
        get_denom()




    End Sub

    Sub get_denom()

        Dim dat As Date = DateAdd(DateInterval.Day, -1, CDate(txtdate.Text))
        dat = DateTime.ParseExact(dat, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim reader As SqlDataReader

        Dim count_1k As Integer = 0
        Dim count_500 As Integer = 0
        Dim count_200 As Integer = 0
        Dim count_100 As Integer = 0
        Dim count_50 As Integer = 0
        Dim count_20 As Integer = 0
        Dim count_10 As Integer = 0
        Dim count_coin As Integer = 0
        Dim count_others As Integer = 0

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select in1k,in500,in200,in100,in50,in20,in10,incoin,inothers from closingdenom WHERE CONVERT(VARCHAR(20), date, 112) ='" + reformatted + "'"
        cmd.Connection = con
        cmd.CommandText = query

        Try

            reader = cmd.ExecuteReader()

            If reader.HasRows() Then
                reader.Read()
                count_1k = IIf(IsDBNull(reader(0)) = True, 0, reader(0))
                count_500 = IIf(IsDBNull(reader(1)) = True, 0, reader(1))
                count_200 = IIf(IsDBNull(reader(2)) = True, 0, reader(2))
                count_100 = IIf(IsDBNull(reader(3)) = True, 0, reader(3))
                count_50 = IIf(IsDBNull(reader(4)) = True, 0, reader(4))
                count_20 = IIf(IsDBNull(reader(5)) = True, 0, reader(5))
                count_10 = IIf(IsDBNull(reader(6)) = True, 0, reader(6))
                count_coin = IIf(IsDBNull(reader(7)) = True, 0, reader(7))
                count_others = IIf(IsDBNull(reader(8)) = True, 0, reader(8))
            End If

        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        Dim dat1 As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        query = "SELECT "
        query &= "SUM(isnull(cast(denom.in1k as float),0)) AS expr1, "
        query &= "SUM(isnull(cast(denom.in500 as float),0)) AS expr2, "
        query &= "SUM(isnull(cast(denom.in200 as float),0)) AS expr3, "
        query &= "SUM(isnull(cast(denom.in100 as float),0)) AS expr4, "
        query &= "SUM(isnull(cast(denom.in50 as float),0)) AS expr5, "
        query &= "SUM(isnull(cast(denom.in20 as float),0)) AS expr6, "
        query &= "SUM(isnull(cast(denom.in10 as float),0)) AS expr7, "
        query &= "SUM(isnull(cast(denom.incoin as float),0)) AS expr8, "
        query &= "SUM(isnull(cast(denom.b1k as float),0)) AS expr9, "
        query &= "SUM(isnull(cast(denom.b500 as float),0)) AS expr10, "
        query &= "SUM(isnull(cast(denom.b100 as float),0)) AS expr11, "
        query &= "SUM(isnull(cast(denom.b200 as float),0)) AS expr12, "
        query &= "SUM(isnull(cast(denom.b50 as float),0)) AS expr13, "
        query &= "SUM(isnull(cast(denom.b20 as float),0)) AS expr14, "
        query &= "SUM(isnull(cast(denom.b10 as float),0)) AS expr15, "
        query &= "SUM(isnull(cast(denom.bcoin as float),0)) AS expr16 "
        query &= "FROM dbo.denom WHERE CONVERT(VARCHAR(20), date, 112) ='" + reformatted1 + "'"
        cmd.Connection = con
        cmd.CommandText = query


        Try

            reader = cmd.ExecuteReader()

            If reader.HasRows() Then
                reader.Read()
                count_1k = count_1k + (IIf(IsDBNull(reader(0)), 0, reader(0)) - IIf(IsDBNull(reader(8)), 0, reader(8)))
                count_500 = count_500 + (IIf(IsDBNull(reader(1)), 0, reader(1)) - IIf(IsDBNull(reader(9)), 0, reader(9))) '(reader(1) - reader(8))
                count_200 = count_200 + (IIf(IsDBNull(reader(2)), 0, reader(2)) - IIf(IsDBNull(reader(10)), 0, reader(10)))
                count_100 = count_100 + (IIf(IsDBNull(reader(3)), 0, reader(3)) - IIf(IsDBNull(reader(11)), 0, reader(11))) '(reader(2) - reader(9))
                count_50 = count_50 + (IIf(IsDBNull(reader(4)), 0, reader(4)) - IIf(IsDBNull(reader(12)), 0, reader(12))) '(reader(3) - reader(10))
                count_20 = count_20 + (IIf(IsDBNull(reader(5)), 0, reader(5)) - IIf(IsDBNull(reader(13)), 0, reader(13))) '(reader(4) - reader(11))
                count_10 = count_10 + (IIf(IsDBNull(reader(6)), 0, reader(6)) - IIf(IsDBNull(reader(14)), 0, reader(14))) '(reader(5) - reader(12))
                count_coin = count_coin + (IIf(IsDBNull(reader(7)), 0, reader(7)) - IIf(IsDBNull(reader(15)), 0, reader(15))) '(reader(6) - reader(13))

            End If

        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try


        lbl_count_1k.Text = count_1k
        lbl_count_500.Text = count_500
        lbl_count_200.Text = count_200
        lbl_count_100.Text = count_100
        lbl_count_50.Text = count_50
        lbl_count_20.Text = count_20
        lbl_count_10.Text = count_10
        lbl_count_coin.Text = count_coin
        lblothers.Text = String.Format("{0:N}", count_others)
        txt1k.Text = count_1k
        txt500.Text = count_500
        txt100.Text = count_100
        txt200.Text = count_200
        txt50.Text = count_50
        txt20.Text = count_20
        txt10.Text = count_10
        txtcoin.Text = count_coin



        lbl_val_1k.Text = String.Format("{0:N}", count_1k * 2000)
        lbl_val_500.Text = String.Format("{0:N}", count_500 * 500)
        lbl_val_200.Text = String.Format("{0:N}", count_200 * 200)
        lbl_val_100.Text = String.Format("{0:N}", count_100 * 100)
        lbl_val_50.Text = String.Format("{0:N}", count_50 * 50)
        lbl_val_20.Text = String.Format("{0:N}", count_20 * 20)
        lbl_val_10.Text = String.Format("{0:N}", count_10 * 10)
        lbl_val_coins.Text = String.Format("{0:N}", count_coin)

        lbl_pd_1k.Text = lbl_val_1k.Text
        lbl_pd_500.Text = lbl_val_500.Text
        lbl_pd_200.Text = lbl_val_200.Text
        lbl_pd_100.Text = lbl_val_100.Text
        lbl_pd_50.Text = lbl_val_50.Text
        lbl_pd_20.Text = lbl_val_20.Text
        lbl_pd_10.Text = lbl_val_10.Text
        lbl_pd_coin.Text = lbl_val_coins.Text


        lbl_denom_total.Text = String.Format("{0:N}", (count_1k * 2000) + (count_500 * 500) + (count_200 * 200) + (count_100 * 100) + (count_50 * 50) + (count_20 * 20) + (count_10 * 10) + (count_coin) + (count_others))
        lbl_pd_total.Text = lbl_denom_total.Text
    End Sub
    Sub get_opening()

        Dim sum_credit As Double
        Dim sum_debit As Double
        Dim opening As Double
        Dim closing_bal As Double

        Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        Dim sdr As SqlDataReader

        If con.State = ConnectionState.Closed Then con.Open()


        If session_user_role = "Audit" Then
            query = "SELECT SUM(suplementc.debit) AS expr1, SUM(suplementc.credit) AS expr2 FROM dbo.suplementc WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
        Else
            query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"
        End If





        cmd.CommandText = query

        Try

            sdr = cmd.ExecuteReader()

            If sdr.HasRows Then

                sdr.Read()

                sum_credit = IIf(IsDBNull(sdr(1)), 0, sdr(1))
                sum_debit = IIf(IsDBNull(sdr(0)), 0, sdr(0))



            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally






            cmd.Dispose()
            con.Close()



        End Try

        Dim oResult As Double = 0

        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 105) <'" + Convert.ToDateTime(txtdate.Text)

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select sum(ledger.ob) as expr1 from ledger"

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

        Finally
            cmd.Dispose()
            con.Close()


        End Try

        If con.State = ConnectionState.Closed Then con.Open()


        If CType(Session("dispsrc"), String) = "TRANS" Then
            query = "select sum(crc) as expr1,sum(drc) as expr2 from dbo.trans where convert(varchar(20),date,112) ='" + reformatted + "' and trans.type='CASH' "
            '  query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) <='" + reformatted + "' and scroll='1'"
        Else
            If session_user_role = "Audit" Then
                query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplementc where convert(varchar(20),date,112) ='" + reformatted + "' and suplementc.type<>'" + "JOURNAL" + "'"
            Else
                query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplement where convert(varchar(20),date,112) ='" + reformatted + "' and suplement.type<>'" + "JOURNAL" + "'"
            End If

        End If


        cmd.CommandText = query
        cmd.Connection = con

        Dim dr As SqlDataReader

        Dim trans_credit As Double = 0

        Dim trans_debit As Double = 0

        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()

                trans_credit = IIf(IsDBNull(dr(0)), 0, dr(0))

                trans_debit = IIf(IsDBNull(dr(1)), 0, dr(1))



            End If

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()

        End Try



        opening = opening + (sum_credit - sum_debit)

        If opening < 0 Then
            lbl_opening_debit.Text = String.Format("{0:N}", -opening)
        Else

            lbl_opening_credit.Text = String.Format("{0:N}", opening)
        End If

        lbl_sum_credit.Text = String.Format("{0:N}", trans_credit)
        lbl_sum_debit.Text = String.Format("{0:N}", trans_debit)


        closing_bal = opening + (trans_credit - trans_debit)

        If closing_bal < 0 Then
            lbl_closing_debit.Text = String.Format("{0:N}", -closing_bal)
        Else

            lbl_closing_credit.Text = String.Format("{0:N}", closing_bal)
            lbldenomcb.Text = FormatCurrency(closing_bal)
        End If


    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        disp.PageIndex = e.NewPageIndex
        dispdebit.PageIndex = e.NewPageIndex
        'If disp.PageIndex = 0 Then

        '    total = pgtot(disp.PageIndex).ToString
        'Else

        '    total = pgtot(disp.PageIndex - 1).ToString
        'End If


        bind_grid()
    End Sub

    Private Sub txtdate_TextChanged(sender As Object, e As EventArgs) Handles txtdate.TextChanged
        'On Error GoTo nxt


        Exit Sub

nxt:
        txtfocus(txtdate)

    End Sub

    Sub update_closing()

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()

        query = "INSERT INTO closingdenom(date,in1k,in500,in200,in100,in50,in20,in10,incoin,inothers)"
        query &= "values(@date,@in1k,@in500,@in200,@in100,@in50,@in20,@in10,@incoin,@inothers)"
        cmd.CommandText = query
        cmd.Connection = con
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@in1k", txt1k.Text)
        cmd.Parameters.AddWithValue("@in500", txt500.Text)
        cmd.Parameters.AddWithValue("@in200", txt200.Text)
        cmd.Parameters.AddWithValue("@in100", txt100.Text)
        cmd.Parameters.AddWithValue("@in50", txt50.Text)
        cmd.Parameters.AddWithValue("@in20", txt20.Text)
        cmd.Parameters.AddWithValue("@in10", txt10.Text)
        cmd.Parameters.AddWithValue("@incoin", txtcoin.Text)
        cmd.Parameters.AddWithValue("@inothers", txtother.Text)
        Try
            cmd.ExecuteNonQuery()

            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

            sb.Append("<div class=" + """alert alert-dismissable alert-primary """ + ">")
            sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
            sb.Append("<strong>Denomination Updated</strong> ")
            sb.Append("</div>")
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()
        End Try

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        query = "INSERT INTO translog SELECT * FROM trans "
        cmd.CommandText = query

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        query = "TRUNCATE TABLE dbo.trans"
        cmd.CommandText = query
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



        btn_up_pd.Enabled = False
        txt1k.Enabled = False
        txt500.Enabled = False
        txt100.Enabled = False
        txt200.Enabled = False
        txt50.Enabled = False
        txt20.Enabled = False
        txt10.Enabled = False
        txtcoin.Enabled = False
        Response.Redirect("/admin/dashboard.aspx")
        Response.Redirect(Request.Url.AbsoluteUri)

    End Sub

    Private Sub btn_up_pd_Click(sender As Object, e As EventArgs) Handles btn_up_pd.Click


        If CDbl(lbl_closing_credit.Text) = CDbl(lbl_pd_total.Text) Then

            update_closing()

            btn_supliment.Visible = True
        Else
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

            sb.Append("<div class=" + """alert alert-dismissable alert-danger """ + ">")
            sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
            sb.Append("<strong>Cash Difference Occured !</strong> ")
            sb.Append("</div>")
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
        End If
    End Sub

    Private Sub txt1k_TextChanged(sender As Object, e As EventArgs) Handles txt1k.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt500)
        'txtfocus(txt500)
    End Sub

    Private Sub txt500_TextChanged(sender As Object, e As EventArgs) Handles txt500.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt200)
        'txtfocus(Me.txt100)

    End Sub

    Private Sub txt100_TextChanged(sender As Object, e As EventArgs) Handles txt100.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt50)

    End Sub

    Private Sub txt50_TextChanged(sender As Object, e As EventArgs) Handles txt50.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt20)


    End Sub

    Private Sub txt20_TextChanged(sender As Object, e As EventArgs) Handles txt20.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt10)


    End Sub

    Private Sub txt10_TextChanged(sender As Object, e As EventArgs) Handles txt10.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtcoin)

    End Sub

    Private Sub txtcoin_TextChanged(sender As Object, e As EventArgs) Handles txtcoin.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtother)


    End Sub


    Sub get_total()

        Dim x As Double = 0
        If Not txt1k.Text = "" Then
            lbl_pd_1k.Text = FormatNumber(CDbl(txt1k.Text) * 2000)
            x = x + CDbl(lbl_pd_1k.Text)
        End If
        If Not txt500.Text = "" Then
            lbl_pd_500.Text = FormatNumber(CDbl(txt500.Text) * 500)
            x = x + CDbl(lbl_pd_500.Text)
        End If

        If Not txt200.Text = "" Then
            lbl_pd_200.Text = FormatNumber(CDbl(txt200.Text) * 200)
            x = x + CDbl(lbl_pd_200.Text)
        End If


        If Not txt100.Text = "" Then
            lbl_pd_100.Text = FormatNumber(CDbl(txt100.Text) * 100)
            x = x + CDbl(lbl_pd_100.Text)
        End If
        If Not txt50.Text = "" Then
            lbl_pd_50.Text = FormatNumber(CDbl(txt50.Text) * 50)
            x = x + CDbl(lbl_pd_50.Text)
        End If
        If Not txt20.Text = "" Then
            lbl_pd_20.Text = FormatNumber(CDbl(txt20.Text) * 20)
            x = x + CDbl(lbl_pd_20.Text)
        End If
        If Not txt10.Text = "" Then
            lbl_pd_10.Text = FormatNumber(CDbl(txt10.Text) * 10)
            x = x + CDbl(lbl_pd_10.Text)

        End If

        If Not txtcoin.Text = "" Then
            lbl_pd_coin.Text = FormatNumber(CDbl(txtcoin.Text))
            x = x + CDbl(lbl_pd_coin.Text)
        End If

        If Not txtother.Text = "" Then
            lbl_pd_others.Text = FormatNumber(CDbl(txtother.Text))
            x = x + CDbl(lbl_pd_others.Text)
        End If

        lbl_pd_total.Text = FormatNumber(x)

    End Sub


    Private Sub txtother_TextChanged(sender As Object, e As EventArgs) Handles txtother.TextChanged

        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.btn_up_pd)


    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click

        dt.Clear()
        dtc.Clear()

        bind_grid()

    End Sub

    Private Sub txt200_TextChanged(sender As Object, e As EventArgs) Handles txt200.TextChanged
        get_total()
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt100)
    End Sub

    Private Sub CashBook_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class