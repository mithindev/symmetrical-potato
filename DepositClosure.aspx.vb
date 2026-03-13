Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices.ComTypes
Imports System.Web.Services
Imports System.Windows.Forms
Public Class DepositClosure
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand

    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim oResult As String
    Dim oResultdate As Date
    Dim monthpart As String
    Dim yearpart As String

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)

    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        con.Open()

        If Not Page.IsPostBack Then


            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtacn.ClientID), True)

            If Not session_user_role = "Admin" Then
                lbl2bpaid.Enabled = False
                lblpenal.Enabled = False
            Else
                lbl2bpaid.Enabled = True
                lblpenal.Enabled = True

            End If


        End If

    End Sub




    Public Function get_ob(ByVal acn As String, ByVal frm As Date)

        Dim dat As DateTime = DateTime.ParseExact(frm, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' GROUP BY [actrans].acno"
        ' Dim sql As String = "select top 1 cbal,dbal from dbo.[actrans] where [actrans].acno='" + acn + "' ORDER BY date DESC"
        Dim ds_bal As New DataSet

        '        Dim Adapter As New SqlDataAdapter(sql, con)
        '       Adapter.Fill(ds_bal)
        cmd.Connection = con
        cmd.CommandText = sql


        Try

            Dim reader1 As SqlDataReader = cmd.ExecuteReader()


            If reader1.HasRows Then
                reader1.Read()
                Session("dbal") = reader1(1) - reader1(0)
                Session("cbal") = reader1(3) - reader1(2)



            Else
                Session("cbal") = 0
                Session("dbal") = 0
            End If

            reader1.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()


        End Try


        Return Session("cbal")

    End Function

    Sub update_cld(ByVal acn As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "UPDATE masterc SET cld = @tscroll where (masterc.acno= @tacn )"



                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@tscroll", 1)
                cmd.Parameters.AddWithValue("@tacn", acn)
                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)


                End Try
            End Using
        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con


                query = "UPDATE stdinsc SET cld = @tscroll where (stdinsc.acno= @tacn )"


                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@tscroll", 1)
                cmd.Parameters.AddWithValue("@tacn", acn)
                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)


                End Try
            End Using
        End Using



    End Sub

    Public Sub get_Cbalance(ByVal acn As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                Dim sql As String = "SELECT SUM([actransc].Drd) AS expr1,SUM([actransc].Crd) AS expr2,SUM([actransc].Drc) AS expr3,SUM([actransc].Crc) AS expr4,[actransc].acno FROM dbo.[actransc] WHERE [actransc].acno ='" + acn + "' GROUP BY [actransc].acno"
                cmd.CommandText = sql
                Using reader As SqlDataReader = cmd.ExecuteReader

                    If reader.HasRows Then
                        reader.Read()
                        Session("dbal") = reader(1).ToString - reader(0).ToString
                        Session("cbal") = reader(3).ToString - reader(2).ToString



                    Else
                        Session("cbal") = 0
                        Session("dbal") = 0
                    End If

                End Using

            End Using
        End Using


    End Sub




    Public Function get_balance(ByVal acn As String)

        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' GROUP BY [actrans].acno"
        ' Dim sql As String = "select top 1 cbal,dbal from dbo.[actrans] where [actrans].acno='" + acn + "' ORDER BY date DESC"
        Dim ds_bal As New DataSet

        '        Dim Adapter As New SqlDataAdapter(sql, con)
        '       Adapter.Fill(ds_bal)
        cmd.Connection = con
        cmd.CommandText = sql

        Dim reader As SqlDataReader = cmd.ExecuteReader()


        If reader.HasRows Then
            reader.Read()
            Session("dbal") = reader(1).ToString - reader(0).ToString
            Session("cbal") = reader(3).ToString - reader(2).ToString



        Else
            Session("cbal") = 0
            Session("dbal") = 0
        End If

        reader.Close()

        Return Session("dbal")



    End Function


    Public Sub get_ac_info(ByVal acn As String)


        Session("voucher_date") = Convert.ToDateTime(tdate.Text)
        ' closure_notice.Visible = False
        Try
            If con.State = ConnectionState.Closed Then con.Open()


            Dim sql As String = "SELECT Master.date,Master.acno,Master.product,Master.amount,Master.cint,Master.dint,Master.prd,Master.prdtype,Master.mdate,Master.mamt,Master.cid,Master.cld FROM dbo.master WHERE master.acno = '" + acn + "' and cld=0 "
            Dim adapter As New SqlDataAdapter(sql, con)
            adapter.Fill(ds)
            If Not ds.Tables(0).Rows.Count = 0 Then
                Session("ac_date") = ds.Tables(0).Rows(0).Item(0)
                Session("acn") = ds.Tables(0).Rows(0).Item(1)
                Session("product") = Trim(ds.Tables(0).Rows(0).Item(2))
                Session("amt") = ds.Tables(0).Rows(0).Item(3)
                Session("cintr") = ds.Tables(0).Rows(0).Item(4)
                Session("dint") = ds.Tables(0).Rows(0).Item(5)
                Session("prd") = ds.Tables(0).Rows(0).Item(6)
                Session("prdtyp") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(7)), "M", ds.Tables(0).Rows(0).Item(7))
                Session("mdt") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(8)), Session("ac_date"), ds.Tables(0).Rows(0).Item(8))
                Session("mamt") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(9)), 0, ds.Tables(0).Rows(0).Item(9))
                Session("ncid") = ds.Tables(0).Rows(0).Item(10)
                Session("cld") = ds.Tables(0).Rows(0).Item(11)
                soa.Enabled = True

            Else
                ' closure_notice.Visible = True
                'lblmat.Text = "Invalid Account or Already Closed."
                Exit Sub

            End If

        Finally
            con.Close()
        End Try

        If Session("cld") = True Then

            'closure_notice.Visible = True
            'lblmat.Text = "Account Already Closed."
            Exit Sub

        End If


        Try
            query = "SELECT FirstName,lastname,address,mobile from dbo.member where MemberNo='" + Session("ncid") + "'"
            Dim adapter1 As New SqlDataAdapter(query, con)
            adapter1.Fill(ds1)
            If Not ds.Tables(0).Rows.Count = 0 Then
                Session("ac_name") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(0)), "", ds1.Tables(0).Rows(0).Item(0))
                Session("ac_lname") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(1)), "", ds1.Tables(0).Rows(0).Item(1))
                Session("address") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(2)), "", ds1.Tables(0).Rows(0).Item(2))
                Session("mobile") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(3)), "", ds1.Tables(0).Rows(0).Item(3))
                'custpict.ImageUrl = "~/ShowImage.ashx?id=" & Session("ncid")
            End If
            adapter1.Dispose()


        Finally

            con.Close()

        End Try


        ds.Dispose()
        ds1.Dispose()
        cmd.Dispose()
        con.Close()




        lblbal.Text = FormatCurrency(get_balance(acn))
        Session("bal") = lblbal.Text
        MaturityLabel.Text = Session("mdt")
        lblproduct.Text = Trim(Session("product"))
        lblname.Text = Session("ac_name")
        lblamt.Text = FormatCurrency(Session("amt"))
        txtamt.Text = Session("amt")







        If Session("mdt") > Convert.ToDateTime(tdate.Text) Then

            txtacn.Enabled = False

            calculate_preclosure()


            txtfocus(tdate)


        Else

            Session("paydt") = getpaydate()

            If Convert.ToDateTime(Session("paydt")) > Convert.ToDateTime(tdate.Text) Then


                Dim stitle = "Hi " + Session("sesusr")
                Dim msg = "Account can be Closed on or After " + CStr(Session("paydt"))
                Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


            Else
                calculate_preclosure()

                ' TabPanel1.Visible = True
                'TabContainer1.Visible = True
                'tabpanel2.Visible = True
                'tabpanel3.Visible = True
                'denom_tab.Visible = False
                txtacn.Enabled = False
                txtfocus(tdate)

            End If


        End If

        pnlint.Visible = True

        chk_4_extra()


    End Sub

    Sub chk_4_extra()


        If con1.State = ConnectionState.Closed Then con1.Open()

        query = "select acn from dlspec where dlspec.deposit='" + txtacn.Text + "'"
        cmd.Connection = con1
        cmd.CommandText = query
        Try
            'Dim isstdins As String = cmd.ExecuteScalar()

            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader
            If dr.HasRows Then

                Do While dr.Read

                    Session("dl") = IIf(IsDBNull(dr(0)), 0, dr(0))
                    chk_dl_bal()

                Loop
            End If




        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            con1.Close()
            cmd.Dispose()

        End Try



    End Sub

    Sub chk_dl_bal()


        Dim dl_bal As Double = get_balance(Session("dl"))
        Session("dl_bal") = dl_bal
        If dl_bal < 0 Then

            btn_up_rcpt.Visible = False

            alertmsg.Visible = True
            lblinfo.Text = "Active Loan on this Deposit. Loan Balance :" + FormatCurrency(dl_bal)

        Else
            'lbl_ad_dl.ForeColor = Color.Green
            btn_up_rcpt.Visible = True
            alertmsg.Visible = False


        End If


    End Sub
    Function get_penalcut()



        Dim i As String = 1
        query = "Select CInt, dint, FYFRM, FYTO, penalcut FROM dbo.roi WHERE roi.Product = @prod And roi.prddmy =  @prdtyp And roi.prdfrm <=  @prdx And roi.prdto >=  @prdy order by fyfrm"

        Try



            Dim dr_roi As SqlDataReader


            If con1.State = ConnectionState.Closed Then con1.Open()

            cmdi.Parameters.Clear()
            cmdi.Connection = con1


            cmdi.CommandText = query
            cmdi.Parameters.AddWithValue("@prod", Session("product"))
            cmdi.Parameters.AddWithValue("@prdtyp", Session("prdtyp"))
            cmdi.Parameters.AddWithValue("@prdx", CInt(Session("prd_buffer_d")))
            cmdi.Parameters.AddWithValue("@prdy", CInt(Session("prd_buffer_d")))




            dr_roi = cmdi.ExecuteReader()

            If dr_roi.HasRows() Then
                dr_roi.Read()

                While dr_roi.Read

                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)


                    Dim x As Long = FYFRM.CompareTo(Session("ac_date"))

                    If x = -1 Then


                        Dim y As Long = FYTO.CompareTo(Session("ac_date"))

                        If y = 1 Then
                            Session("penal_cintr") = IIf(IsDBNull(dr_roi(4)), 0, dr_roi(4))

                            'dint = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                            Exit While

                        End If

                    End If
                End While

                dr_roi.Close()

            End If
        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            cmdi.Dispose()
            con1.Close()

        End Try



        Return Session("penal_cintr")
    End Function
    Function get_dint()


        Dim dr_roi As SqlDataReader
        If con.State = ConnectionState.Closed Then con.Open()
        query = "Select CInt, dint, FYFRM, FYTO FROM dbo.roi WHERE roi.Product = @prod And roi.prddmy =  @prdtyp And roi.prdfrm <=  @prdx And roi.prdto >=  @prdy order by fyfrm"

        cmd.Parameters.Clear()
        cmd.Connection = con


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@prod", Session("product"))
        cmd.Parameters.AddWithValue("@prdtyp", Session("prdtyp"))
        cmd.Parameters.AddWithValue("@prdx", Session("prd"))
        cmd.Parameters.AddWithValue("@prdy", Session("prd"))



        Try

            dr_roi = cmd.ExecuteReader()

            If dr_roi.HasRows() Then
                'dr_roi.Read()

                While dr_roi.Read

                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)


                    Dim x As Long = FYFRM.CompareTo(Session("ac_date"))

                    If x = -1 Then


                        Dim y As Long = FYTO.CompareTo(Session("ac_date"))

                        If y = 1 Then
                            Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                            Session("dint") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                            Exit While

                        End If

                    End If
                End While


            End If


        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()



        End Try


        Return Session("cintr")

    End Function
    Sub ds_closure_c(ByVal acn As String)
        ' cintr = get_penalcut()

        Dim prod = Trim(get_pro(Session("product")))
        Session("acn_c") = acn


        '        Session("ac_date") = CDate(reader(1).ToString)
        '       Session("amount") = reader(2).ToString
        '      Session("mdate") = CDate(reader(3).ToString)
        '     Session("cintr") = CDbl(reader(4).ToString)
        '    Session("prd") = CDbl(reader(5).ToString)

        Session("penel_int") = 0
        Session("c2bepaid") = 0
        Session("days_ago") = DateDiff(DateInterval.Day, Session("ac_date"), Convert.ToDateTime(tdate.Text))

        get_Cbalance(acn)


        Select Case Session("days_ago")

            Case Is <= 90

                Session("penel_int") = Session("dbal") * 3 / 100
                lblpenalroi.Text = "3%"


            Case Is <= 180
                Session("penel_int") = Session("dbal") * 2 / 100
                lblpenalroi.Text = "2%"

            Case Is <= 270
                Session("penel_int") = FormatCurrency(Session("dbal") * 1 / 100, 0)
                lblpenalroi.Text = "1%"
                lblpenalroi_d.Text = "1%"


            Case Is <= 330
                Session("c2bepaid") = 0
                Session("penel_int") = 0

            Case Is >= 365

                Session("due") = Session("amt") * 300

                If Session("dbal") < CDbl(Session("due")) Then
                    Session("c2bepaid") = 0
                Else
                    Session("c2bepaid") = (Session("amt") / 10) * 120


                End If

                '   lbl2bpaid.Text = c2bepaid + cbal - penel_int


        End Select

        '  lblpenal.Text = Session("penel_int")
        ' lblpenal_d.Text = Session("penel_int")

        ' lbl2bpaid.Text = Session("c2bepaid")
        'lbl2bpaid_d.Text = Session("c2bepaid")

        Session("pre_amt") = Session("c2bepaid") + Session("dbal") - Session("penel_int")




        If Not CDbl(Session("c2bepaid")) = 0 Then
            ' Session("cbal") = Session("cbal") + CDbl(Session("2bpaid_d"))
            'Session("dbal") = Session("dbal") + CDbl(Session("2bpaid_d"))
            update_int_C(Session("tid"), 0, CDbl(Session("c2bepaid")), 0, CDbl(Session("c2bepaid")), "BY INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, CDbl(Session("c2bepaid")), 0, "BY INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", 0, CDbl(Session("c2bepaid")), "To INTEREST", "INTR")

        End If


        If Not CDbl(Session("penel_int")) = 0 Then

            update_int_C(Session("tid"), CDbl(Session("penel_int")), 0, CDbl(Session("penel_int")), 0, "To INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("penel_int")), "To INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", CDbl(Session("penel_int")), 0, "BY INTEREST", "INTR")

        End If


        update_cld(acn)





        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con



        query = "INSERT INTO actransc(id, Date, acno, drd, crd, drc, crc, narration, due, Type, suplimentry, sesusr, entryat, cbal, dbal)"
        query &= "VALUES(@id,@Date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@drd", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crc", 0)

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "To CASH")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To TRANSFER")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()

        End Try


        update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("pre_amt")), "To CASH", "PAYMENT")

    End Sub

    Sub ds_closure()
        ' cintr = get_penalcut()
        Session("penel_int") = 0
        Session("c2bepaid") = 0
        Session("days_ago") = DateDiff(DateInterval.Day, Session("ac_date"), Convert.ToDateTime(tdate.Text))
        Dim totalCol As Integer = GetTotalCollection()
        Dim period As Integer = 0
        Integer.TryParse(Session("prd"), period)
        If period <= 4 Then
            Select Case totalCol

                Case Is <= 25
                    GetDS_Penal_AmtInfo(1.5, True)
                Case Is <= 50
                    GetDS_Penal_AmtInfo(1, True)
                Case Is <= 75
                    GetDS_Penal_AmtInfo(0.5, True)
                Case Is <= 85
                    GetDS_Penal_AmtInfo(0, False)
                Case Is <= 99
                    GetDS_Penal_AmtInfo(10, False)
                Case Is >= 100
                    GetDS_Penal_AmtInfo(20, False)
            End Select
        ElseIf period <= 8 Then
            Select Case totalCol

                Case Is <= 30
                    GetDS_Penal_AmtInfo(1.5, True)
                Case Is <= 60
                    GetDS_Penal_AmtInfo(1, True)
                Case Is <= 90
                    GetDS_Penal_AmtInfo(0.5, True)
                Case Is <= 120
                    GetDS_Penal_AmtInfo(0, False)
                Case Is <= 150
                    GetDS_Penal_AmtInfo(35, False)
                Case Is <= 180
                    GetDS_Penal_AmtInfo(45, False)
                Case Is <= 199
                    GetDS_Penal_AmtInfo(55, False)
                Case Is >= 200
                    GetDS_Penal_AmtInfo(60, False)
            End Select
        Else
            Select Case totalCol

                Case Is <= 50
                    GetDS_Penal_AmtInfo(1.5, True)
                Case Is <= 100
                    GetDS_Penal_AmtInfo(1, True)
                Case Is <= 150
                    GetDS_Penal_AmtInfo(0.5, True)
                Case Is <= 175
                    GetDS_Penal_AmtInfo(0, False)
                Case Is <= 200
                    GetDS_Penal_AmtInfo(75, False)
                Case Is <= 225
                    GetDS_Penal_AmtInfo(85, False)
                Case Is <= 250
                    GetDS_Penal_AmtInfo(95, False)
                Case Is <= 275
                    GetDS_Penal_AmtInfo(105, False)
                Case Is <= 299
                    GetDS_Penal_AmtInfo(115, False)
                Case Is >= 300
                    GetDS_Penal_AmtInfo(130, False)
            End Select
        End If

        lblpenal.Text = Session("penel_int")
        lblpenal_d.Text = Session("penel_int")

        lbl2bpaid.Text = Session("c2bepaid")
        lbl2bpaid_d.Text = Session("c2bepaid")

        txtamt.Text = CDbl(lbl2bpaid_d.Text) + Session("dbal") - CDbl(lblpenal_d.Text)
        Session("camt") = CDbl(lbl2bpaid.Text) + Session("cbal") - CDbl(lblpenal.Text)
    End Sub

    Sub GetDS_Penal_AmtInfo(ByVal val As Double, ByVal isInterest As Boolean)
        If isInterest Then
            Session("penel_int") = Session("dbal") * val / 100
            lblpenalroi.Text = val.ToString() + "%"
        Else
            Session("c2bepaid") = (Session("amt") / 10) * val
            Session("penel_int") = 0
        End If
    End Sub

    Function GetTotalCollection()
        Dim totalCol As Integer = 0
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                Dim reformatted As String = Convert.ToDateTime(Session("ac_date")).ToString("yyyyMMdd", CultureInfo.InvariantCulture)

                query = "select COUNT(acno) from dbo.actrans where acno = @acn and CONVERT(VARCHAR(20), date, 112) >= @acdt"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acn", Session("acn"))
                cmd.Parameters.AddWithValue("@acdt", reformatted)

                totalCol = cmd.ExecuteScalar()
            End Using
        End Using
        Return totalCol
    End Function

    Function get_prev_inton_c(ByVal acn As String)
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con


                query = "Select top 1 Date FROM dbo.actransc WHERE actransc.acno = @dcacn AND actransc.Type = @ty ORDER BY date DESC"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@dcacn", acn)
                cmd.Parameters.AddWithValue("@ty", "INTR")

                oResultdate = cmd.ExecuteScalar()

                If oResultdate = Date.MinValue Then
                    Session("prv_inton") = Session("ac_date")
                Else
                    Session("prv_inton") = oResultdate

                End If



            End Using
        End Using

        Return Session("prv_inton")


    End Function
    Function get_prev_inton()

        If con.State = ConnectionState.Closed Then con.Open()

        Try

            query = "SELECT top 1 date FROM dbo.actrans WHERE actrans.acno = @dcacn AND actrans.Type = @ty ORDER BY date DESC"

            cmd.Parameters.AddWithValue("@dcacn", txtacn.Text)
            cmd.Parameters.AddWithValue("@ty", "INTR")
            cmd.CommandText = query

            oResultdate = cmd.ExecuteScalar()

            If oResultdate = Date.MinValue Then
                Session("prv_inton") = Session("ac_date")


            Else
                Session("prv_inton") = oResultdate


            End If

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()

        End Try

        Return Session("prv_inton")


    End Function

    Function get_IntpaidD_c(ByVal acn As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT COALESCE(SUM([crd]),0) FROM dbo.actransc WHERE actransc.acno = @acn AND actransc.Type = @tyd"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acn", acn)
                cmd.Parameters.AddWithValue("@tyd", "INTR")
                cmd.CommandText = query

                Try
                    Session("intpaid_d") = cmd.ExecuteScalar()
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
        End Using


        Return Session("intpaid_d")

    End Function


    Function get_intpaid_d()

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT COALESCE(SUM([crd]),0) FROM dbo.actrans WHERE actrans.acno = @acn AND actrans.Type = @tyd"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                cmd.Parameters.AddWithValue("@tyd", "INTR")
                cmd.CommandText = query

                Try
                    Session("intpaid_d") = cmd.ExecuteScalar()
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
        End Using


        Return Session("intpaid_d")


    End Function

    Function get_intpaid_c(ByVal acn As String)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmdx = New SqlCommand
                cmdx.Connection = con
                query = "SELECT COALESCE (SUM([crc]),0) FROM dbo.actransc WHERE actransc.acno = @acnc AND actransc.Type = @tyc"
                cmd.Parameters.Clear()
                cmdx.Parameters.AddWithValue("@acnc", acn)
                cmdx.Parameters.AddWithValue("@tyc", "INTR")
                cmdx.CommandText = query

                Try
                    cmdx.CommandText = query

                    Session("intpaid") = cmdx.ExecuteScalar()

                Catch ex As Exception
                    Response.Write(ex.ToString)


                End Try
            End Using

        End Using



        Return Session("intpaid")
    End Function


    Function get_intpaid()

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmdx = New SqlCommand
                cmdx.Connection = con
                query = "SELECT COALESCE (SUM([crc]),0) FROM dbo.actrans WHERE actrans.acno = @acnc AND actrans.Type = @tyc"
                cmd.Parameters.Clear()
                cmdx.Parameters.AddWithValue("@acnc", txtacn.Text)
                cmdx.Parameters.AddWithValue("@tyc", "INTR")
                cmdx.CommandText = query

                Try
                    cmdx.CommandText = query

                    Session("intpaid") = cmdx.ExecuteScalar()

                Catch ex As Exception
                    Response.Write(ex.ToString)


                End Try
            End Using

        End Using



        Return Session("intpaid")
    End Function



    Sub get_extra_roi()

        Dim cintr As Double = Session("cintr")
        Dim dint As Double = Session("dint")



        Dim ds As New DataSet

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@prod", "FD")
                cmd.Parameters.AddWithValue("@prdtyp", "M")
                cmd.Parameters.AddWithValue("@prdx", Session("prd"))
                cmd.Parameters.AddWithValue("@prdy", Session("prd"))

                Using dr_roi As SqlDataReader = cmd.ExecuteReader

                    If dr_roi.HasRows() Then
                        dr_roi.Read()

                        While dr_roi.Read

                            Dim FYFRM As Date = dr_roi(2)
                            Dim FYTO As Date = dr_roi(3)

                            ' Session("ac_date") = CDate(ddt.Text)
                            Dim x As Long = FYFRM.CompareTo(Session("ac_date"))

                            If x = -1 Then


                                Dim y As Long = FYTO.CompareTo(Session("ac_date"))

                                If y = 1 Then

                                    Session("cintr_pre_diff") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                                    Session("dint_pre_diff") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))
                                    Session("cintr_pre_diff") = cintr - CDbl(Session("cintr_pre_diff"))
                                    Session("dint_pre_diff") = dint - CDbl(Session("dint_pre_diff"))


                                    Exit While

                                End If

                            End If
                        End While



                    End If

                End Using

            End Using
        End Using


    End Sub


    Sub getint()

        Dim ds As New DataSet

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@prod", "FD")
                cmd.Parameters.AddWithValue("@prdtyp", "M")
                cmd.Parameters.AddWithValue("@prdx", Session("prd_buffer"))
                cmd.Parameters.AddWithValue("@prdy", Session("prd_buffer"))

                Using dr_roi As SqlDataReader = cmd.ExecuteReader

                    If dr_roi.HasRows() Then
                        dr_roi.Read()

                        While dr_roi.Read

                            Dim FYFRM As Date = dr_roi(2)
                            Dim FYTO As Date = dr_roi(3)

                            ' Session("ac_date") = CDate(ddt.Text)
                            Dim x As Long = FYFRM.CompareTo(Session("ac_date"))

                            If x = -1 Then


                                Dim y As Long = FYTO.CompareTo(Session("ac_date"))

                                If y = 1 Then

                                    Session("cintr_pre") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                                    Session("dint_pre") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                                    Exit While

                                End If

                            End If
                        End While



                    End If

                End Using

            End Using
        End Using

    End Sub

    Sub get_maturity_interist_with_plan_fd()

        Dim acno As String = txtacn.Text
        Dim wef As Date
        Dim mdate As Date
        Dim plan As String = ""
        Dim tenure As Integer
        Dim today As Date = Date.Today

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()

        ' GET MASTER DETAILS
        cmd.CommandText = "
        SELECT [date], [mdate], [plan], [prd], [prdtype], [amount]
        FROM dbo.master
        WHERE acno = @acno AND product = 'FD'"


        cmd.Parameters.AddWithValue("@acno", acno)

        Dim dr As SqlDataReader = cmd.ExecuteReader()

        If dr.Read() Then
            wef = CDate(dr("date"))
            mdate = CDate(dr("mdate"))
            plan = If(IsDBNull(dr("plan")), "", dr("plan").ToString().ToLower())
            tenure = CInt(dr("prd"))
            Session("amt") = CDbl(dr("amount"))
        Else
            dr.Close()
            Exit Sub
        End If
        dr.Close()

        '  GET ROI SLAB
        Dim roiCint As Decimal = 0D
        Dim roiDint As Decimal = 0D

        Dim monthsCompleted As Integer = DateDiff(DateInterval.Month, wef, mdate)

        'Adjust month logic like your reference
        If mdate.Day < wef.Day Then monthsCompleted -= 1
        If monthsCompleted < 1 Then monthsCompleted = 1
        If monthsCompleted > tenure Then monthsCompleted = tenure

        cmd.Parameters.Clear()

        Dim q2 As String =
        "SELECT TOP 1 cint, dint
        FROM dbo.roi
        WHERE product='FD'
        AND @startDate BETWEEN fyfrm AND fyto
        AND @months BETWEEN prdfrm AND prdto
        ORDER BY id DESC"

        Using cmd2 As New SqlCommand(q2, con)

            cmd2.Parameters.AddWithValue("@startDate", wef)
            cmd2.Parameters.AddWithValue("@months", monthsCompleted)

            Using dr2 = cmd2.ExecuteReader()

                If dr2.Read() Then
                    roiCint = CDec(dr2("cint"))
                    roiDint = CDec(dr2("dint"))
                Else
                    Throw New Exception("ROI slab not found.")
                End If

            End Using
        End Using

        '  GET ADDITIONAL PLAN INTEREST
        Dim addInt As Decimal = 0D

        If plan <> "" Then

            Dim planList As String() = plan.Split(","c)

            Dim q3 As String =
    "SELECT TOP 1 renew, srcitizen, bnktrf 
     FROM dbo.goldrate
     WHERE [date] <= @startDate
     ORDER BY [date] DESC"

            Using cmd3 As New SqlCommand(q3, con)

                cmd3.Parameters.AddWithValue("@startDate", wef)

                Using dr3 = cmd3.ExecuteReader()

                    If dr3.Read() Then

                        For Each p As String In planList

                            Select Case p.Trim()

                                Case "renew"
                                    addInt += If(IsDBNull(dr3("renew")), 0D, CDec(dr3("renew")))

                                Case "senior"
                                    addInt += If(IsDBNull(dr3("srcitizen")), 0D, CDec(dr3("srcitizen")))

                                Case "transfer"
                                    addInt += If(IsDBNull(dr3("bnktrf")), 0D, CDec(dr3("bnktrf")))

                            End Select

                        Next

                    End If

                End Using
            End Using

        End If


        ' FINAL INTEREST

        Session("cintr") = roiCint + addInt
        Session("dint") = roiDint + addInt

        Dim script2 As String = "alert('dint: " &
        Convert.ToDouble(Session("dint")).ToString(System.Globalization.CultureInfo.InvariantCulture) &
        " | Months: " &
        monthsCompleted.ToString() &
        " | AddInt: " &
        addInt.ToString(System.Globalization.CultureInfo.InvariantCulture) &
        "');"

        ScriptManager.RegisterStartupScript(Page, Page.GetType(),
               Guid.NewGuid().ToString(),
               script2,
               True)

    End Sub

    Sub get_preclosure_interest_with_plan_fd(preclMonth As Integer)

        Dim acno As String = txtacn.Text
        Dim wef As Date
        Dim plan As String = ""
        Dim roiCint As Double = 0
        Dim roiDint As Double = 0
        Dim addInt As Double = 0

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()

        ' 1️⃣ GET WEF + PLAN
        cmd.CommandText = "
        SELECT [date], [plan] 
        FROM dbo.master 
        WHERE acno = @acno AND product = 'FD'"

        cmd.Parameters.AddWithValue("@acno", acno)

        Dim dr As SqlDataReader = cmd.ExecuteReader()

        If dr.Read() Then
            wef = CDate(dr("date"))
            plan = If(IsDBNull(dr("plan")), "", dr("plan").ToString().ToLower())
        Else
            dr.Close()
            Exit Sub
        End If

        dr.Close()

        ' 2️⃣ GET ROI BASED ON PRE-CLOSURE MONTHS

        'Adjust month logic properly (like maturity logic)
        Dim monthsCompleted As Integer = preclMonth

        If CDate(tdate.Text).Day < wef.Day Then monthsCompleted -= 1
        If monthsCompleted < 1 Then monthsCompleted = 1

        cmd.Parameters.Clear()

        Dim q2 As String =
        "SELECT TOP 1 cint, dint
        FROM dbo.roi
        WHERE product='FD'
        AND @startDate BETWEEN fyfrm AND fyto
        AND @months BETWEEN prdfrm AND prdto
        ORDER BY id DESC"

        Using cmd2 As New SqlCommand(q2, con)

            cmd2.Parameters.AddWithValue("@startDate", wef)
            cmd2.Parameters.AddWithValue("@months", monthsCompleted)

            Using dr2 = cmd2.ExecuteReader()

                If dr2.Read() Then
                    roiCint = CDec(dr2("cint"))
                    roiDint = CDec(dr2("dint"))
                Else
                    Throw New Exception("Pre-closure ROI slab not found.")
                End If

            End Using
        End Using

        ' 3️⃣ GET ADDITIONAL INTEREST FROM GOLD RATE
        If plan <> "" Then

            Dim planList As String() = plan.Split(","c)

            Dim q3 As String =
    "SELECT TOP 1 renew, srcitizen, bnktrf 
     FROM dbo.goldrate
     WHERE [date] <= @startDate
     ORDER BY [date] DESC"

            Using cmd3 As New SqlCommand(q3, con)

                cmd3.Parameters.AddWithValue("@startDate", wef)

                Using dr3 = cmd3.ExecuteReader()

                    If dr3.Read() Then

                        For Each p As String In planList

                            Select Case p.Trim()

                                Case "renew"
                                    addInt += If(IsDBNull(dr3("renew")), 0D, CDec(dr3("renew")))

                                Case "senior"
                                    addInt += If(IsDBNull(dr3("srcitizen")), 0D, CDec(dr3("srcitizen")))

                                Case "transfer"
                                    addInt += If(IsDBNull(dr3("bnktrf")), 0D, CDec(dr3("bnktrf")))

                            End Select

                        Next

                    End If

                End Using
            End Using

        End If

        ' 4️⃣ STORE FINAL PRE-CLOSURE ROI
        Session("precls_croi") = roiCint + addInt
        Session("precls_droi") = roiDint + addInt



        Dim script2 As String = "alert('PreClosure ROI: " &
        Convert.ToDouble(Session("precls_croi")).ToString(System.Globalization.CultureInfo.InvariantCulture) &
        " | Months: " &
        monthsCompleted.ToString() &
        " | AddInt: " &
        addInt.ToString(System.Globalization.CultureInfo.InvariantCulture) &
        "');"

        ScriptManager.RegisterStartupScript(Page, Page.GetType(),
               Guid.NewGuid().ToString(),
               script2,
               True)


    End Sub

    Sub fd_closure()

        Dim intc_amt As Integer
        Dim intd_amt As Integer

        Dim aintc As Integer
        Dim aintd As Integer
        Dim tdays As Integer
        Dim days As Integer

        get_maturity_interist_with_plan_fd()

        Dim x As Integer = Session("mdt").CompareTo(CDate(tdate.Text))

        Dim preclMonth = DateDiff(DateInterval.Month, Session("ac_date"), CDate(tdate.Text))


        If Not x <= 0 Then

            'getPreClsROI(preclMonth)
            get_preclosure_interest_with_plan_fd(preclMonth)

            Session("prv_inton") = get_prev_inton()
            tdays = DateDiff(DateInterval.Day, Session("ac_date"), Session("prv_inton"))
            days = tdays Mod (30)
            Session("days_ago") = (tdays - days) / 30

            Dim scriptDebug1 As String = "alert('DEBUG INFO:\n" &
            "days_ago " & Session("days_ago").ToString() & "\n" &
        "');"

            ScriptManager.RegisterStartupScript(Page, Page.GetType(),
        Guid.NewGuid().ToString(),
        scriptDebug1,
        True)

            Session("prd_buffer") = Session("days_ago") 'CInt((Session("days_ago") / 365) * 12)
            getint()
            get_extra_roi()

            Session("penalcut") = 1
            Session("penal_cintr") = Session("precls_croi") - Session("penalcut") '+ Session("cintr_pre_diff")
            Session("penal_dint") = Session("precls_droi") - Session("penalcut") '+ Session("dint_pre_diff")

            aintc = Math.Round((Session("cbal") * Session("cintr") / 100 / 12) * Session("prd_buffer"))
            aintd = Math.Round((Session("cbal") * Session("dint") / 100 / 12) * Session("prd_buffer"))
            intc_amt = Math.Round((Session("cbal") * Session("penal_cintr") / 100 / 12) * Session("prd_buffer"))
            intd_amt = Math.Round((Session("cbal") * Session("penal_dint") / 100 / 12) * Session("prd_buffer"))
            Session("intpaid") = get_intpaid()
            Session("intpaid_d") = get_intpaid_d()

            lblperiod.Text = Convert.ToString(Session("prd_buffer")) + " Months"
            lblpenalperiod.Text = Convert.ToString(Session("prd_buffer")) + " Months"
            lblroi.Text = String.Format("{0:N}", Session("cintr"))
            lblpenalroi.Text = String.Format("{0:N}", Session("penal_cintr"))
            lblactualint.Text = String.Format("{0:N}", aintc)
            lblpenalint.Text = String.Format("{0:N}", intc_amt)
            lblintpaid.Text = String.Format("{0:N}", Session("intpaid"))
            lblpenalintpaid.Text = String.Format("{0:N}", Session("intpaid"))
            lbl2bpaid.Text = String.Format("{0:N}", (aintc - Session("intpaid")))
            lblpenal2bpaid.Text = String.Format("{0:N}", (intc_amt - Session("intpaid")))
            lblpenal.Text = String.Format("{0:N}", aintc - intc_amt)

            If aintc - Session("intpaid") < 0 Then

            End If

            lblperiod_d.Text = lblperiod.Text
            lblpenalperiod_d.Text = CStr(preclMonth) + " Months"

            lblroi_d.Text = String.Format("{0:N}", Session("dint"))
            lblpenalroi_d.Text = String.Format("{0:N}", Session("penal_dint"))


            lblpenalint_d.Text = String.Format("{0:N}", intd_amt)
            lblactualint_d.Text = String.Format("{0:N}", aintd)
            lblintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))
            lblpenalintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))
            lbl2bpaid_d.Text = String.Format("{0:N}", aintd - Session("intpaid_d"))
            lblpenal2bpaid_d.Text = String.Format("{0:N}", intd_amt - Session("intpaid_d"))
            lblpenal_d.Text = String.Format("{0:N}", aintd - intd_amt)

            txtamt.Text = String.Format("{0:N}", Session("dbal") - CDbl(lblpenal_d.Text) + CDbl(lbl2bpaid_d.Text))
            Session("camt") = String.Format("{0:N}", Session("cbal") - CDbl(lblpenal.Text) + CDbl(lbl2bpaid.Text))


        Else
            lblperiod_d.Text = 0
            lblperiod.Text = 0
            lblpenalperiod_d.Text = 0
            lblpenalperiod.Text = 0
            lblroi_d.Text = String.Format("{0:N}", 0)
            lblroi.Text = String.Format("{0:N}", 0)
            lblpenalroi_d.Text = String.Format("{0:N}", 0)
            lblpenalroi.Text = String.Format("{0:N}", 0)
            Session("totalint_d") = Math.Round((Session("dbal") * Session("dint") / 100) / 12) * Session("prd_buffer")
            Session("totint_penal_d") = Math.Round((Session("dbal") * Session("penal_dint") / 100) / 12) * Session("prd_buffer")

            lblpenalint_d.Text = String.Format("{0:N}", 0)
            lblpenalint.Text = String.Format("{0:N}", 0)
            lblactualint_d.Text = String.Format("{0:N}", 0)
            lblactualint.Text = String.Format("{0:N}", 0)
            lblintpaid_d.Text = String.Format("{0:N}", 0)
            lblintpaid.Text = String.Format("{0:N}", 0)
            lblpenalintpaid_d.Text = String.Format("{0:N}", 0)
            lblpenalintpaid.Text = String.Format("{0:N}", 0)
            lbl2bpaid_d.Text = String.Format("{0:N}", 0)
            lbl2bpaid.Text = String.Format("{0:N}", 0)
            lblpenal2bpaid_d.Text = String.Format("{0:N}", 0)
            lblpenal2bpaid.Text = String.Format("{0:N}", 0)
            lblpenal_d.Text = String.Format("{0:N}", 0)
            lblpenal.Text = String.Format("{0:N}", 0)

            txtamt.Text = String.Format("{0:N}", Session("dbal"))
            Session("camt") = String.Format("{0:N}", Session("cbal"))

        End If


    End Sub

    Sub rd_closure()

        Dim due As Integer = 0


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        Dim mint As Double
        Dim cur_pr As Double
        Dim cum_int As Double = 0
        Dim i As Integer

        'Dim sql As String = "SELECT COUNT(*)  AS duep FROM dbo.[actrans] WHERE [actrans].acno ='" + txtacn.Text + "' AND [actrans].Type = 'CASH'"

        query = "SELECT COUNT(*) AS acno FROM dbo.actrans WHERE actrans.acno = @acno AND (actrans.Type = @typ OR actrans.Type=@typ1)"
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@typ", "CASH")
        cmd.Parameters.AddWithValue("@typ1", "TRF")


        cmd.Connection = con
        cmd.CommandText = query
        Try
            oResult = cmd.ExecuteScalar()


            If Not IsDBNull(oResult) Then

                due = oResult

            Else
                due = 0
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally

            cmd.Dispose()
            con.Close()


        End Try

        ''  Dim x As Integer = CDate(tdate.Text).CompareTo(Session("mdt"))

        If Not CDate(tdate.Text).CompareTo(Session("mdt")) Then

            If Not CType(Session("prd"), Integer) <= due Then GoTo NXT

            get_last_transaction(Trim(txtacn.Text))

            If Session("days_ago") < 30 Then GoTo NXT

            Session("prv_inton") = get_prev_inton()

            Dim paid_due As Integer = DateDiff(DateInterval.Month, Session("ac_date"), Session("prv_inton"))
            due = DateDiff(DateInterval.Month, Session("prv_inton"), Session("mdt"))

            due = Math.Round(due)
            Dim paid_amt As Double = paid_due * Session("amt")
            Dim pamt As Double = Session("amt")
            cur_pr = pamt + paid_amt
            For i = 1 To due
                mint = (cur_pr * Session("cintr") / 100) / 12
                mint = Math.Round(mint)
                cum_int = cum_int + mint
                cur_pr = cur_pr + mint + pamt
            Next
            cum_int = Math.Round(cum_int)

            lblperiod.Text = CStr(due) + " Months"
            lblperiod_d.Text = CStr(due) + " Months"
            lblroi.Text = CStr(Session("cintr")) + "% "
            lblroi_d.Text = CStr(Session("dint")) + "% "
            lblactualint.Text = String.Format("{0:N}", cum_int)
            txtamt.Text = CDbl(lblbal.Text) + cum_int

            If CDbl(txtamt.Text) < Session("mamt") Then
                txtamt.Text = Session("mamt")
                lblactualint.Text = Session("mamt") - CDbl(lblbal.Text)
                lbl2bpaid.Text = lblactualint.Text
                lbl2bpaid_d.Text = lblactualint.Text
            ElseIf CDbl(txtamt.Text) > Session("mamt") Then
                txtamt.Text = Session("mamt")
                lblactualint.Text = Session("mamt") - CDbl(lblbal.Text)
                lbl2bpaid.Text = lblactualint.Text
                lbl2bpaid_d.Text = lblactualint.Text
            End If
            Session("camt") = txtamt.Text
        Else

NXT:
            ' Session("prv_inton") = get_prev_inton()


            ' Dim paid_due As Integer = DateDiff(DateInterval.Month, Session("ac_date"), Session("prv_inton"))


            Dim acno As String = txtacn.Text
            Dim startDate As Date
            Dim prd As Integer

            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Parameters.Clear()

            Dim q1 As String = "SELECT date, prd FROM dbo.master WHERE acno=@acno AND product='RD'"
            Using cmd1 As New SqlCommand(q1, con)
                cmd1.Parameters.AddWithValue("@acno", acno)
                Using dr = cmd1.ExecuteReader()
                    If dr.Read() Then
                        startDate = CDate(dr("date"))
                        prd = CInt(dr("prd"))
                    Else
                        Throw New Exception("RD master record not found.")
                    End If
                End Using
            End Using

            Dim today As Date = CDate(Session("mdt"))
            Dim monthsCompleted As Integer = due

            If today.Day < startDate.Day Then monthsCompleted -= 1
            If monthsCompleted < 1 Then monthsCompleted = 1
            If monthsCompleted > prd Then monthsCompleted = prd

            Dim roiRate As Decimal = 0D

            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Parameters.Clear()

            Dim q2 As String =
            "SELECT TOP 1 cint
             FROM dbo.roi
             WHERE product='RD'
             AND @startDate BETWEEN fyfrm AND fyto
             AND @months BETWEEN prdfrm AND prdto
             ORDER BY id DESC"

            Using cmd2 As New SqlCommand(q2, con)
                cmd2.Parameters.AddWithValue("@startDate", startDate)
                cmd2.Parameters.AddWithValue("@months", monthsCompleted)

                Dim result = cmd2.ExecuteScalar()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    roiRate = CDec(result)
                Else
                    Throw New Exception("ROI slab not found.")
                End If
            End Using

            Dim penalcut As Decimal = 2D
            Dim penal_cintr As Decimal = roiRate - penalcut

            If Session("dint") = 0 Then Session("dint") = Session("cintr")

            Session("penal_cintr") = roiRate - penalcut
            Session("penal_dint") = roiRate - penalcut

            'Select Case due
            '    Case Is <= 3
            '        If due >= 4 Then
            '            Session("penalcut") = 4
            '            Session("penal_cintr") = Session("cintr") - Session("penalcut")
            '            Session("penal_dint") = Session("dint") - Session("penalcut")
            '        Else
            '            Session("penal_cintr") = 0 'Session("cintr") - Session("penalcut")
            '            Session("penal_dint") = 0 'Session("dint") - Session("penalcut")
            '        End If

            '    Case Is <= 6
            '        If due >= 7 Then
            '            Session("penalcut") = 4
            '            Session("penal_cintr") = Session("cintr") - Session("penalcut")
            '            Session("penal_dint") = Session("dint") - Session("penalcut")
            '        Else
            '            Session("penal_cintr") = 0 'Session("cintr") - Session("penalcut")
            '            Session("penal_dint") = 0 'Session("dint") - Session("penalcut")
            '        End If


            '    Case Is <= 24
            '        Session("penalcut") = 4
            '        Session("penal_cintr") = Session("cintr") - Session("penalcut")
            '        Session("penal_dint") = Session("dint") - Session("penalcut")

            '    Case Is <= 36
            '        Session("penalcut") = 3
            '        Session("penal_cintr") = Session("cintr") - Session("penalcut")
            '        Session("penal_dint") = Session("dint") - Session("penalcut")

            '    Case Is <= 48
            '        Session("penalcut") = 2
            '        Session("penal_cintr") = Session("cintr") - Session("penalcut")
            '        Session("penal_dint") = Session("dint") - Session("penalcut")

            '    Case Is <= 60
            '        Session("penalcut") = 1
            '        Session("penal_cintr") = Session("cintr") - Session("penalcut")
            '        Session("penal_dint") = Session("dint") - Session("penalcut")
            'End Select





            Dim pamt As Double = Session("amt")
            cur_pr = pamt
            For i = 1 To due
                mint = (cur_pr * Session("cintr") / 100) / 12
                ' mint = Math.Round(mint)
                cum_int = cum_int + mint
                cur_pr = cur_pr + Math.Round(mint) + pamt
            Next

            lblactualint.Text = String.Format("{0:N}", Math.Round(cum_int))

            cur_pr = pamt
            mint = 0
            cum_int = 0
            For i = 1 To due
                mint = (cur_pr * Session("dint") / 100) / 12
                ' mint = Math.Round(mint)
                cum_int = cum_int + mint
                cur_pr = cur_pr + Math.Round(mint) + pamt
            Next

            lblactualint_d.Text = String.Format("{0:N}", Math.Round(cum_int))
            Dim normalInterest_d As Decimal = Math.Round(cum_int, 2)

            cur_pr = pamt
            mint = 0
            cum_int = 0

            For i = 1 To due
                mint = (cur_pr * Session("penal_cintr") / 100) / 12
                mint = mint
                cum_int = cum_int + mint
                cur_pr = cur_pr + Math.Round(mint) + pamt
            Next
            lblpenalint.Text = String.Format("{0:N}", Math.Round(cum_int))
            Dim penalInterest_d As Decimal = Math.Round(cum_int, 2)


            cur_pr = pamt
            mint = 0
            cum_int = 0

            For i = 1 To due
                mint = (cur_pr * Session("penal_dint") / 100) / 12
                '  mint = Math.Round(mint)
                cum_int = cum_int + mint
                cur_pr = cur_pr + Math.Round(mint) + pamt
            Next
            lblpenalint_d.Text = String.Format("{0:N}", Math.Round(cum_int))

            Session("intpaid") = get_intpaid()
            Session("intpaid_d") = get_intpaid_d()

            Dim interestPaid_d As Decimal = CDec(Session("intpaid_d"))

            Dim normalToBePaid As Decimal = normalInterest_d - interestPaid_d
            Dim penalToBePaid As Decimal = penalInterest_d - interestPaid_d
            Dim penaltyLoss As Decimal = normalToBePaid - penalToBePaid

            lblroi.Text = CStr(Session("cintr")) + "%"
            lblroi_d.Text = CStr(Session("dint")) + "%"

            lblpenalroi.Text = CStr(Session("penal_cintr")) + "%"
            lblpenalroi_d.Text = CStr(Session("penal_dint")) + "%"

            lblintpaid.Text = String.Format("{0:N}", Session("intpaid"))
            lblpenalintpaid.Text = String.Format("{0:N}", Session("intpaid"))
            lblintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))
            lblpenalintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))


            lbl2bpaid.Text = String.Format("{0:N}", (CDbl(lblactualint.Text) - CDbl(Session("intpaid"))))
            lbl2bpaid_d.Text = String.Format("{0:N}", (CDbl(lblactualint_d.Text) - CDbl(Session("intpaid_d"))))

            lblpenal2bpaid.Text = String.Format("{0:N}", (CDbl(lblpenalint.Text) - CDbl(Session("intpaid"))))
            lblpenal2bpaid_d.Text = String.Format("{0:N}", (CDbl(lblpenalint_d.Text) - CDbl(Session("intpaid_d"))))

            lblpenal.Text = String.Format("{0:N}", (CDbl(lbl2bpaid.Text) - CDbl(lblpenal2bpaid.Text)))
            lblpenal_d.Text = String.Format("{0:N}", (CDbl(lbl2bpaid_d.Text) - CDbl(lblpenal2bpaid_d.Text)))

            lblperiod.Text = CStr(due) + " Months"
            lblpenalperiod.Text = CStr(due) + " Months"
            lblperiod_d.Text = CStr(due) + " Months"
            lblpenalperiod_d.Text = CStr(due) + " Months"

            If CDbl(lblpenal2bpaid.Text) < 0 Then
                txtamt.Text = String.Format("{0:N}", CDbl(Session("dbal")) + (CDbl(lblpenal2bpaid.Text)))
            Else
                'txtamt.Text = String.Format("{0:N}", CDbl(Session("dbal")) + (CDbl(lblpenalint_d.Text) - (CDbl(lblpenal2bpaid_d.Text))))
                txtamt.Text = String.Format("{0:N}", CDbl(Session("dbal")) + (CDbl(lblpenal2bpaid.Text))) '+ CDbl(lblpenal.Text)))
            End If
            Session("camt") = txtamt.Text

            ' <-- WORKING -->
            'lblperiod_d_w.Text = CStr(due) + " Months"
            'lblpenalperiod_d_w.Text = CStr(due) + " Months"

        End If

    End Sub

    Sub rd_closure_c(ByVal acn As String)

        Dim prod = Trim(get_pro(Session("product")))
        Session("acn_c") = acn

        '        Session("ac_date") = CDate(reader(1).ToString)
        '       Session("amount") = reader(2).ToString
        '      Session("mdate") = CDate(reader(3).ToString)
        '     Session("cintr") = CDbl(reader(4).ToString)

        Dim due As Integer = 0


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        Dim mint As Double
        Dim cur_pr As Double
        Dim cum_int As Double = 0
        Dim i As Integer

        'Dim sql As String = "SELECT COUNT(*)  AS duep FROM dbo.[actrans] WHERE [actrans].acno ='" + txtacn.Text + "' AND [actrans].Type = 'CASH'"

        query = "SELECT COUNT(*) AS acno FROM dbo.actransc WHERE actransc.acno = @acno AND (actransc.Type = @typ OR actransc.Type=@typ1)"
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@typ", "CASH")
        cmd.Parameters.AddWithValue("@typ1", "TRF")


        cmd.Connection = con
        cmd.CommandText = query
        Try
            oResult = cmd.ExecuteScalar()


            If Not IsDBNull(oResult) Then

                due = oResult

            Else
                due = 0
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally

            cmd.Dispose()
            con.Close()


        End Try

        ''  Dim x As Integer = CDate(tdate.Text).CompareTo(Session("mdt"))

        If Not CDate(tdate.Text).CompareTo(Session("mdate")) Then

            If Not CType(Session("prd"), Integer) <= due Then GoTo NXT

            get_last_transaction_c(acn)

            If Session("days_ago") < 30 Then GoTo NXT

            Session("prv_inton") = get_prev_inton_c(acn)

            Dim paid_due As Integer = DateDiff(DateInterval.Month, Session("ac_date"), Session("prv_inton"))
            due = DateDiff(DateInterval.Month, Session("prv_inton"), Session("mdate"))

            due = Math.Round(due)
            Dim paid_amt As Double = paid_due * Session("amount")
            Dim pamt As Double = Session("amount")
            cur_pr = pamt + paid_amt
            For i = 1 To due
                mint = (cur_pr * Session("cintr") / 100) / 12
                mint = Math.Round(mint)
                cum_int = cum_int + mint
                cur_pr = cur_pr + mint + pamt
            Next
            cum_int = Math.Round(cum_int)

            'lblperiod.Text = CStr(due) + " Months"
            'lblperiod_d.Text = CStr(due) + " Months"
            'lblroi.Text = CStr(Session("cintr")) + "% "
            'lblroi_d.Text = CStr(Session("dint")) + "% "
            'l'blactualint.Text = String.Format("{0:N}", cum_int)
            'txtamt.Text = CDbl(lblbal.Text) + cum_int
            get_Cbalance(acn)

            If CDbl(txtamt.Text) < Session("mamt") Then
                Session("pre_amt") = Session("mamt")
                Session("2bpaid") = Session("mamt") - CDbl(Session("dbal"))
                Session("penel") = 0
                'Session("2bpaid") = lblactualint.Text
                'lbl2bpaid_d.Text = lblactualint.Text
            ElseIf CDbl(txtamt.Text) > Session("mamt") Then
                Session("pre_amt") = Session("mamt")
                ' lblactualint.Text = 
                Session("2bpaid") = Session("mamt") - CDbl(Session("2bpaid"))
                Session("penel") = 0

                'lbl2bpaid_d.Text = lblactualint.Text
            End If

        Else

NXT:
            ' Session("prv_inton") = get_prev_inton()


            ' Dim paid_due As Integer = DateDiff(DateInterval.Month, Session("ac_date"), Session("prv_inton"))

            If Session("dint") = 0 Then Session("dint") = Session("cintr")

            Select Case due

                Case Is <= 3
                    If due >= 4 Then
                        Session("penalcut") = 4
                        Session("penal_cintr") = Session("cintr") - Session("penalcut")
                        Session("penal_dint") = Session("dint") - Session("penalcut")
                    Else
                        Session("penal_cintr") = 0 'Session("cintr") - Session("penalcut")
                        Session("penal_dint") = 0 'Session("dint") - Session("penalcut")
                    End If

                Case Is <= 6
                    If due >= 7 Then
                        Session("penalcut") = 4
                        Session("penal_cintr") = Session("cintr") - Session("penalcut")
                        Session("penal_dint") = Session("dint") - Session("penalcut")
                    Else
                        Session("penal_cintr") = 0 'Session("cintr") - Session("penalcut")
                        Session("penal_dint") = 0 'Session("dint") - Session("penalcut")
                    End If


                Case Is <= 24
                    Session("penalcut") = 4
                    Session("penal_cintr") = Session("cintr") - Session("penalcut")
                    Session("penal_dint") = Session("dint") - Session("penalcut")

                Case Is <= 36
                    Session("penalcut") = 3
                    Session("penal_cintr") = Session("cintr") - Session("penalcut")
                    Session("penal_dint") = Session("dint") - Session("penalcut")

                Case Is <= 48
                    Session("penalcut") = 2
                    Session("penal_cintr") = Session("cintr") - Session("penalcut")
                    Session("penal_dint") = Session("dint") - Session("penalcut")

                Case Is <= 60
                    Session("penalcut") = 1
                    Session("penal_cintr") = Session("cintr") - Session("penalcut")
                    Session("penal_dint") = Session("dint") - Session("penalcut")
            End Select





            Dim pamt As Double = Session("amount")
            cur_pr = pamt
            For i = 1 To due
                mint = (cur_pr * Session("cintr") / 100) / 12
                ' mint = Math.Round(mint)
                cum_int = cum_int + mint
                cur_pr = cur_pr + Math.Round(mint) + pamt
            Next

            Session("actint") = String.Format("{0:N}", Math.Round(cum_int))


            cur_pr = pamt
            mint = 0
            cum_int = 0

            For i = 1 To due
                mint = (cur_pr * Session("penal_cintr") / 100) / 12
                mint = mint
                cum_int = cum_int + mint
                cur_pr = cur_pr + Math.Round(mint) + pamt
            Next
            Session("penelint") = String.Format("{0:N}", Math.Round(cum_int))


            Session("intpaid") = get_intpaid_c(acn)
            Session("intpaid_d") = get_IntpaidD_c(acn)



            ' lblintpaid.Text = String.Format("{0:N}", Session("intpaid"))
            ' lblpenalintpaid.Text = String.Format("{0:N}", Session("intpaid"))
            ' lblintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))
            ' lblpenalintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))


            Session("2bpaid") = Session("actint") - CDbl(Session("intpaid"))
            '    lbl2bpaid_d.Text = String.Format("{0:N}", (CDbl(lblactualint_d.Text) - CDbl(Session("intpaid_d"))))

            Session("penal2bpaid") = String.Format("{0:N}", (Session("penelint") - CDbl(Session("intpaid"))))
            'lblpenal2bpaid_d.Text = String.Format("{0:N}", (CDbl(lblpenalint_d.Text) - CDbl(Session("intpaid_d"))))

            Session("penal") = CDbl(Session("2bpaid")) - CDbl(Session("penal2bpaid"))
            'lblpenal_d.Text = String.Format("{0:N}", (CDbl(lbl2bpaid_d.Text) - CDbl(lblpenal2bpaid_d.Text)))

            ' lblperiod.Text = CStr(due) + " Months"
            ' lblpenalperiod.Text = CStr(due) + " Months"
            ' lblperiod_d.Text = CStr(due) + " Months"
            ' lblpenalperiod_d.Text = CStr(due) + " Months"
            get_Cbalance(acn)

            If CDbl(Session("penal2bpaid")) < 0 Then
                Session("pre_amt") = CDbl(Session("dbal")) + CDbl(Session("penal2bpaid"))
            Else
                'txtamt.Text = String.Format("{0:N}", CDbl(Session("dbal")) + (CDbl(lblpenalint_d.Text) - (CDbl(lblpenal2bpaid_d.Text))))
                Session("pre_amt") = String.Format("{0:N}", CDbl(Session("dbal")) + (CDbl(Session("penal2bpaid")))) '+ CDbl(lblpenal.Text)))
            End If

        End If




        If Not CDbl(Session("2bpaid")) = 0 Then
            Session("cbal") = Session("cbal") + CDbl(Session("2bpaid_d"))
            Session("dbal") = Session("dbal") + CDbl(Session("2bpaid_d"))
            update_int_C(Session("tid"), 0, CDbl(Session("2bpaid")), 0, CDbl(Session("2bpaid")), "BY INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, CDbl(Session("2bpaid")), 0, "BY INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", 0, CDbl(Session("2bpaid")), "TO INTEREST", "INTR")

        End If


        If Not CDbl(Session("penal")) = 0 Then

            update_int_C(Session("tid"), CDbl(Session("penal")), 0, CDbl(Session("penal")), 0, "TO INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("penal")), "TO INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", CDbl(Session("penal")), 0, "BY INTEREST", "INTR")

        End If


        update_cld(acn)





        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con



        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@drd", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crc", 0)

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "TO CASH")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To TRANSFER")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()



        End Try


        update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("pre_amt")), "TO CASH", "PAYMENT")
    End Sub

    Sub get_maturity_interist_with_plan()

        Dim acno As String = txtacn.Text
        Dim wef As Date
        Dim mdate As Date
        Dim plan As String = ""
        Dim tenure As Integer
        Dim today As Date = Date.Today

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()

        ' GET MASTER DETAILS
        cmd.CommandText = "
SELECT [date], [mdate], [plan], [prd], [prdtype], [amount]
FROM dbo.master
WHERE acno = @acno AND product = 'RID'"


        cmd.Parameters.AddWithValue("@acno", acno)

        Dim dr As SqlDataReader = cmd.ExecuteReader()

        If dr.Read() Then
            wef = CDate(dr("date"))
            mdate = CDate(dr("mdate"))
            plan = If(IsDBNull(dr("plan")), "", dr("plan").ToString().ToLower())
            tenure = CInt(dr("prd"))
            Session("amt") = CDbl(dr("amount"))
        Else
            dr.Close()
            Exit Sub
        End If
        dr.Close()

        '========================
        '  GET ROI SLAB
        '========================
        Dim roiCint As Decimal = 0D
        Dim roiDint As Decimal = 0D

        Dim monthsCompleted As Integer = DateDiff(DateInterval.Month, wef, mdate)

        'Adjust month logic like your reference
        If mdate.Day < wef.Day Then monthsCompleted -= 1
        If monthsCompleted < 1 Then monthsCompleted = 1
        If monthsCompleted > tenure Then monthsCompleted = tenure

        cmd.Parameters.Clear()

        Dim q2 As String =
"SELECT TOP 1 cint, dint
 FROM dbo.roi
 WHERE product='RID'
 AND @startDate BETWEEN fyfrm AND fyto
 AND @months BETWEEN prdfrm AND prdto
 ORDER BY id DESC"

        Using cmd2 As New SqlCommand(q2, con)

            cmd2.Parameters.AddWithValue("@startDate", wef)
            cmd2.Parameters.AddWithValue("@months", monthsCompleted)

            Using dr2 = cmd2.ExecuteReader()

                If dr2.Read() Then
                    roiCint = CDec(dr2("cint"))
                    roiDint = CDec(dr2("dint"))
                Else
                    Throw New Exception("ROI slab not found.")
                End If

            End Using
        End Using

        '========================
        ' 3️⃣ GET ADDITIONAL PLAN INTEREST
        '========================
        Dim addInt As Double = 0

        'If plan <> "" Then

        '    cmd.Parameters.Clear()
        '    cmd.CommandText = "
        'SELECT TOP 1 renew, srcitizen, bnktrf 
        'FROM dbo.goldrate
        'WHERE [date] <= @startDate
        'ORDER BY [date] DESC"

        '    cmd.Parameters.AddWithValue("@startDate", wef)

        '    dr = cmd.ExecuteReader()

        '    If dr.Read() Then

        '        Select Case plan
        '            Case "renew"
        '                addInt = If(IsDBNull(dr("renew")), 0, CDbl(dr("renew")))

        '            Case "senior"
        '                addInt = If(IsDBNull(dr("srcitizen")), 0, CDbl(dr("srcitizen")))

        '            Case "transfer"
        '                addInt = If(IsDBNull(dr("bnktrf")), 0, CDbl(dr("bnktrf")))

        '            Case Else
        '                addInt = 0
        '        End Select

        '    End If

        '    dr.Close()
        'End If

        If plan <> "" Then

            Dim planList As String() = plan.Split(","c)

            Dim q3 As String =
    "SELECT TOP 1 renew, srcitizen, bnktrf 
     FROM dbo.goldrate
     WHERE [date] <= @startDate
     ORDER BY [date] DESC"

            Using cmd3 As New SqlCommand(q3, con)

                cmd3.Parameters.AddWithValue("@startDate", wef)

                Using dr3 = cmd3.ExecuteReader()

                    If dr3.Read() Then

                        For Each p As String In planList

                            Select Case p.Trim()

                                Case "renew"
                                    addInt += If(IsDBNull(dr3("renew")), 0D, CDec(dr3("renew")))

                                Case "senior"
                                    addInt += If(IsDBNull(dr3("srcitizen")), 0D, CDec(dr3("srcitizen")))

                                Case "transfer"
                                    addInt += If(IsDBNull(dr3("bnktrf")), 0D, CDec(dr3("bnktrf")))

                            End Select

                        Next

                    End If

                End Using
            End Using

        End If


        ' FINAL INTEREST

        Session("cintr") = roiCint + addInt
        Session("dint") = roiDint + addInt

    End Sub

    Sub get_preclosure_interest_with_plan(preclMonth As Integer)

        Dim acno As String = txtacn.Text
        Dim wef As Date
        Dim plan As String = ""
        Dim roiCint As Double = 0
        Dim roiDint As Double = 0
        Dim addInt As Double = 0

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()

        ' 1️⃣ GET WEF + PLAN
        cmd.CommandText = "
        SELECT [date], [plan] 
        FROM dbo.master 
        WHERE acno = @acno AND product = 'RID'"

        cmd.Parameters.AddWithValue("@acno", acno)

        Dim dr As SqlDataReader = cmd.ExecuteReader()

        If dr.Read() Then
            wef = CDate(dr("date"))
            plan = If(IsDBNull(dr("plan")), "", dr("plan").ToString().ToLower())
        Else
            dr.Close()
            Exit Sub
        End If

        dr.Close()

        ' 2️⃣ GET ROI BASED ON PRE-CLOSURE MONTHS

        'Adjust month logic properly (like maturity logic)
        Dim monthsCompleted As Integer = preclMonth

        If CDate(tdate.Text).Day < wef.Day Then monthsCompleted -= 1
        If monthsCompleted < 1 Then monthsCompleted = 1

        cmd.Parameters.Clear()

        Dim q2 As String =
"SELECT TOP 1 cint, dint
 FROM dbo.roi
 WHERE product='RID'
 AND @startDate BETWEEN fyfrm AND fyto
 AND @months BETWEEN prdfrm AND prdto
 ORDER BY id DESC"

        Using cmd2 As New SqlCommand(q2, con)

            cmd2.Parameters.AddWithValue("@startDate", wef)
            cmd2.Parameters.AddWithValue("@months", monthsCompleted)

            Using dr2 = cmd2.ExecuteReader()

                If dr2.Read() Then
                    roiCint = CDec(dr2("cint"))
                    roiDint = CDec(dr2("dint"))
                Else
                    Throw New Exception("Pre-closure ROI slab not found.")
                End If

            End Using
        End Using

        '========================
        ' 3️⃣ GET ADDITIONAL INTEREST FROM GOLD RATE
        '========================
        'If plan <> "" Then

        '    cmd.Parameters.Clear()
        '    cmd.CommandText = "
        'SELECT TOP 1 renew, srcitizen, bnktrf 
        'FROM dbo.goldrate
        'WHERE [date] <= @startDate
        'ORDER BY [date] DESC"

        '    cmd.Parameters.AddWithValue("@startDate", wef)

        '    dr = cmd.ExecuteReader()

        '    If dr.Read() Then

        '        Select Case plan
        '            Case "renew"
        '                addInt = If(IsDBNull(dr("renew")), 0, CDbl(dr("renew")))

        '            Case "senior"
        '                addInt = If(IsDBNull(dr("srcitizen")), 0, CDbl(dr("srcitizen")))

        '            Case "transfer"
        '                addInt = If(IsDBNull(dr("bnktrf")), 0, CDbl(dr("bnktrf")))

        '            Case Else
        '                addInt = 0
        '        End Select

        '    End If

        '    dr.Close()
        'End If

        If plan <> "" Then

            Dim planList As String() = plan.Split(","c)

            Dim q3 As String =
    "SELECT TOP 1 renew, srcitizen, bnktrf 
     FROM dbo.goldrate
     WHERE [date] <= @startDate
     ORDER BY [date] DESC"

            Using cmd3 As New SqlCommand(q3, con)

                cmd3.Parameters.AddWithValue("@startDate", wef)

                Using dr3 = cmd3.ExecuteReader()

                    If dr3.Read() Then

                        For Each p As String In planList

                            Select Case p.Trim()

                                Case "renew"
                                    addInt += If(IsDBNull(dr3("renew")), 0D, CDec(dr3("renew")))

                                Case "senior"
                                    addInt += If(IsDBNull(dr3("srcitizen")), 0D, CDec(dr3("srcitizen")))

                                Case "transfer"
                                    addInt += If(IsDBNull(dr3("bnktrf")), 0D, CDec(dr3("bnktrf")))

                            End Select

                        Next

                    End If

                End Using
            End Using

        End If

        '========================
        ' 4️⃣ STORE FINAL PRE-CLOSURE ROI
        '========================
        Session("precls_croi") = roiCint + addInt
        Session("precls_droi") = roiDint + addInt



        '    Dim script2 As String = "alert('PreClosure ROI: " &
        'Convert.ToDouble(Session("precls_croi")).ToString(System.Globalization.CultureInfo.InvariantCulture) &
        '" | Months: " &
        'monthsCompleted.ToString() &
        '" | AddInt: " &
        'addInt.ToString(System.Globalization.CultureInfo.InvariantCulture) &
        '"');"

        '    ScriptManager.RegisterStartupScript(Page, Page.GetType(),
        '        Guid.NewGuid().ToString(),
        '        script2,
        '        True)


    End Sub

    Sub rid_closure()

        Dim bal As Double = get_balance(txtacn.Text)

        Dim PRIN As Integer
        Dim MN As Integer
        Dim MNT As Integer
        Dim PRAMT As Integer
        ' Dim PR As Integer

        Dim fq As Integer = 0
        Dim prdx As Double = 0
        Dim curint As Double = 0
        Dim curint_d As Double = 0
        Dim curint_penal As Double = 0
        Dim curint_penal_d As Double = 0
        Dim cumint As Double = 0
        Dim cumint_d As Double = 0
        Dim cumint_penal As Double = 0
        Dim cumint_penal_d As Double = 0

        Dim pamt As Double = Session("amt")
        Dim pamt_d As Double = Session("amt")
        Dim pamt_penal As Double = Session("amt")
        Dim pamt_penal_d As Double = Session("amt")

        Dim days As Double = 0

        Dim das As Decimal = 0.0

        get_maturity_interist_with_plan()
        Dim preclMonth = DateDiff(DateInterval.Month, Session("ac_date"), CDate(tdate.Text))

        get_preclosure_interest_with_plan(preclMonth)

        Session("penal_cintr") = Session("precls_croi")
        Session("penal_dint") = Session("precls_droi")

        Session("prd") = DateDiff(DateInterval.Month, Session("ac_date"), Session("voucher_date"))
        Session("prdtyp") = "M"

        Session("prv_inton") = get_prev_inton()

        Dim x = CDate(tdate.Text).CompareTo(Session("mdt"))

        If Not x = -1 Then

            ScriptManager.RegisterStartupScript(Page, Page.GetType(),
                "Alert" & Guid.NewGuid().ToString(),
                "alert('inside');",
                True)

            days = DateDiff(DateInterval.Month, Session("ac_date"), CDate(tdate.Text)) * 30

            Dim scriptDebug1 As String = "alert('DEBUG INFO:\n" &
            "Total Days: " & days.ToString() & "\n" &
        "');"

            ScriptManager.RegisterStartupScript(Page, Page.GetType(),
        Guid.NewGuid().ToString(),
        scriptDebug1,
        True)

            PRIN = Session("amt")
            MN = Session("prd")
            MN = MN / 3
            PRAMT = PRIN
            For j = 1 To MN
                MNT = PRAMT * Session("cintr") / 100
                MNT = MNT / 12
                PRAMT = PRAMT + (MNT * 3)
            Next
            Session("camt") = Math.Round(PRAMT)

            Session("prd_buffer") = DateDiff(DateInterval.Month, Session("prv_inton"), Session("mdt"))

            prdx = Session("prd_buffer") Mod 3

            fq = Math.Round(Session("prd_buffer") - prdx)

            pamt = CDbl(lblbal.Text)

            Dim script5 As String = "alert('fq: " &
            Convert.ToDouble(fq).ToString(System.Globalization.CultureInfo.InvariantCulture) &
            "');"

            ScriptManager.RegisterStartupScript(Page, Page.GetType(),
            Guid.NewGuid().ToString(),
            script5,
            True)

            If Not fq = 0 Then

                For i = 1 To fq
                    curint = (pamt * Session("cintr") / 100) / 12
                    curint_penal = (pamt_penal * Session("penal_cintr") / 100) / 12
                    curint_d = (pamt_d * Session("dint") / 100) / 12
                    curint_penal_d = (pamt_penal_d * Session("penal_dint") / 100) / 12

                    cumint = cumint + (curint * 3)
                    cumint_d = cumint_d + (curint_d * 3)
                    cumint_penal = cumint_penal + (curint_penal * 3)
                    cumint_penal_d = cumint_penal_d + (curint_penal_d * 3)

                    pamt = pamt + (curint * 3)
                    pamt_d = pamt_d + (curint_d * 3)
                    pamt_penal = pamt_penal + (curint_penal * 3)
                    pamt_penal_d = pamt_penal_d + (curint_penal_d * 3)
                Next
            End If


            lblroi.Text = String.Format("{0:N}", Session("cintr"))
            lblpenalroi.Text = String.Format("{0:N}", Session("penal_cintr"))
            lblroi_d.Text = String.Format("{0:N}", Session("dint"))
            lblpenalroi_d.Text = String.Format("{0:N}", Session("penal_cintr"))

            'lblactualint.Text = String.Format("{0:N}", cumint)
            'lblactualint_d.Text = String.Format("{0:N}", cumint_d)

            txtamt.Text = String.Format("{0:N}", Session("mamt"))
            lbl2bpaid.Text = String.Format("{0:N}", Session("camt") - CDbl(Session("cbal")))
            lbl2bpaid_d.Text = String.Format("{0:N}", Session("mamt") - bal)


            lblpenalint.Text = String.Format("{0:N}", 0)
            lblpenalint_d.Text = String.Format("{0:N}", 0)

            lblintpaid.Text = String.Format("{0:N}", 0)
            lblpenalintpaid.Text = String.Format("{0:N}", 0)

            lblintpaid_d.Text = String.Format("{0:N}", 0)
            lblpenalintpaid_d.Text = String.Format("{0:N}", 0)

            lblpenal2bpaid.Text = String.Format("{0:N}", 0)
            lblpenal2bpaid_d.Text = String.Format("{0:N}", 0)

            lblpenal.Text = String.Format("{0:N}", 0)
            lblpenal_d.Text = String.Format("{0:N}", 0)

            lblperiod.Text = days & " Days (" & preclMonth & " M)"
            lblperiod_d.Text = days & " Days (" & preclMonth & " M)"

            lblpenalperiod.Text = days & " Days (" & preclMonth & " M)"
            lblpenalperiod_d.Text = days & " Days (" & preclMonth & " M)"


        Else

            '<--- REAL PRE CLOSURE --->

            Dim remprd As Double = 0


            Session("penal_cintr") = Session("cintr") - 1
            Session("penal_dint") = Session("dint") - 1
            Session("prd_buffer") = DateDiff(DateInterval.Day, Session("ac_date"), Session("voucher_date"))
            Dim penalDays = DateDiff(DateInterval.Day, Session("ac_date"), CDate(tdate.Text))
            days = Session("prd_buffer")
            Dim prdDays As Integer = CInt(Session("prd_buffer"))
            Dim penalDaysVal As Integer = CInt(penalDays)

            Dim prdMonths As Decimal = Math.Round(prdDays / 30D, 2)
            Dim penalMonths As Decimal = Math.Round(penalDaysVal / 30D, 2)

            lblperiod.Text = prdDays & " Days (" & prdMonths & " M)"
            lblperiod_d.Text = prdDays & " Days (" & prdMonths & " M)"

            lblpenalperiod.Text = penalDaysVal & " Days (" & penalMonths & " M)"
            lblpenalperiod_d.Text = penalDaysVal & " Days (" & penalMonths & " M)"


            Session("intpaid") = get_intpaid()
            Session("intpaid_d") = get_intpaid_d()

            Dim cintr As Double = Session("cintr")
            Dim dintr As Double = Session("dint")

            ''---------soe
            If (Session("prd_buffer") > 30) Then
                prdx = Session("prd_buffer") / 30
                remprd = prdx Mod 3


                If Not prdx = remprd Then
                    fq = Format((prdx - remprd) / 3, "#")
                Else
                    fq = 0
                End If
                ' prdx = Session("prd_buffer") Mod 3
                das = Format(prdx - (fq * 3), "0.00")
            Else
                If Session("prd_buffer") <= 15 Then
                    das = 0
                    fq = 0
                Else
                    fq = 0
                    das = Format(Session("prd_buffer") / 30, "0.00")


                End If
            End If


            '  fq = Math.Round(Session("prd_buffer") - Session("prdx"))
            Session("amt") = CDbl(lblamt.Text)
            pamt = Session("amt")

            For i = 1 To fq
                curint = (pamt * cintr / 100) / 12
                curint_penal = (pamt_penal * Session("penal_cintr") / 100) / 12
                curint_d = (pamt_d * dintr / 100) / 12
                curint_penal_d = (pamt_penal_d * Session("penal_dint") / 100) / 12

                cumint = cumint + (curint * 3)
                cumint_d = cumint_d + (curint_d * 3)
                cumint_penal = cumint_penal + (curint_penal * 3)
                cumint_penal_d = cumint_penal_d + (curint_penal_d * 3)

                pamt = pamt + (curint * 3)
                pamt_d = pamt_d + (curint_d * 3)
                pamt_penal = pamt_penal + (curint_penal * 3)
                pamt_penal_d = pamt_penal_d + (curint_penal_d * 3)

            Next
            pamt = Math.Round(pamt)
            pamt_d = Math.Round(pamt_d)
            pamt_penal = Math.Round(pamt_penal)
            pamt_penal_d = Math.Round(pamt_penal_d)

            Dim int4day As Double = (pamt * cintr / 100) / 365
            int4day = int4day * (das * 30)
            cumint = Math.Round(cumint + int4day)
            cumint_d = Math.Round(cumint_d + (((pamt_d * dintr / 100) / 365) * (das * 30)))
            cumint_penal = Math.Round(cumint_penal + (((pamt_penal * Session("penal_cintr") / 100) / 365) * (das * 30)))
            cumint_penal_d = Math.Round(cumint_penal_d + (((pamt_penal_d * Session("penal_dint") / 100) / 365) * (das * 30)))


            lblroi.Text = String.Format("{0:N}", cintr)
            lblpenalroi.Text = String.Format("{0:N}", Session("penal_cintr"))
            lblroi_d.Text = String.Format("{0:N}", dintr)
            lblpenalroi_d.Text = String.Format("{0:N}", Session("penal_dint"))

            lblactualint.Text = String.Format("{0:N}", cumint)
            lblactualint_d.Text = String.Format("{0:N}", cumint_d)

            'lblpenalint.Text = String.Format("{0:N}", cumint_penal)
            'lblpenalint_d.Text = String.Format("{0:N}", cumint_penal_d)

            lblintpaid.Text = String.Format("{0:N}", Session("intpaid_d"))
            lblpenalintpaid.Text = String.Format("{0:N}", Session("intpaid_d"))

            lblintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))
            lblpenalintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))

            lbl2bpaid.Text = String.Format("{0:N}", (cumint - Session("intpaid")))
            lbl2bpaid_d.Text = String.Format("{0:N}", (cumint_d - Session("intpaid_d")))

            lblpenal2bpaid.Text = String.Format("{0:N}", (cumint_penal - Session("intpaid_d")))
            lblpenal2bpaid_d.Text = String.Format("{0:N}", (cumint_penal_d - Session("intpaid_d")))

            lblpenal.Text = String.Format("{0:N}", (cumint - cumint_penal))
            lblpenal_d.Text = String.Format("{0:N}", (cumint_d - cumint_penal_d))

            txtamt.Text = String.Format("{0:N}", (CDbl(lblbal.Text) + (cumint_penal_d - Session("intpaid_d"))))
            Session("camt") = String.Format("{0:N}", (CDbl(Session("cbal")) + (cumint_penal - Session("intpaid"))))


        End If

        ' NEW CUMULATIVE INTEREST ENGINE

        Dim total_days As Integer = days
        Dim q_no As Integer = total_days \ 90
        Dim d_no As Integer = total_days Mod 90
        Dim principal_amount As Double = CDbl(Session("amt"))

        Dim accumulated_interest_penal As Double = 0D
        Dim rate_of_interest_penal As Double = CDbl(Session("penal_dint"))   ' keep in %

        Dim rate_of_interest_maturity As Double = CDbl(Session("cintr"))
        Dim accumulated_interest_maturity As Double = 0D

        Dim quater_cmp_amount_penal As Double = 0D
        Dim quater_cmp_amount_maturity As Double = 0D

        If total_days <= 90 Then

            accumulated_interest_penal = (principal_amount * rate_of_interest_penal / 100D) * total_days / 365D
            accumulated_interest_maturity = (principal_amount * rate_of_interest_maturity / 100D) * total_days / 365D

        Else

            quater_cmp_amount_penal = (((400D + rate_of_interest_penal) / 400D) ^ q_no) * principal_amount

            accumulated_interest_penal = quater_cmp_amount_penal - principal_amount

            quater_cmp_amount_maturity = (((400D + rate_of_interest_maturity) / 400D) ^ q_no) * principal_amount

            accumulated_interest_maturity = quater_cmp_amount_maturity - principal_amount

            If d_no > 0 Then
                Dim d_interest_penal As Double = (quater_cmp_amount_penal * rate_of_interest_penal / 100D) * d_no / 365D

                accumulated_interest_penal += d_interest_penal

                Dim d_interest_maturity As Double = (quater_cmp_amount_maturity * rate_of_interest_maturity / 100D) * d_no / 365D

                accumulated_interest_maturity += d_interest_maturity
            End If

        End If

        accumulated_interest_penal = Math.Round(accumulated_interest_penal, 2)
        accumulated_interest_maturity = Math.Round(accumulated_interest_maturity, 2)

        lblpenalint.Text = String.Format("{0:N}", accumulated_interest_penal)
        lblpenalint_d.Text = String.Format("{0:N}", accumulated_interest_penal)

        lblactualint.Text = String.Format("{0:N}", accumulated_interest_maturity)
        lblactualint_d.Text = String.Format("{0:N}", accumulated_interest_maturity)

        Dim scriptDebug As String = "alert('DEBUG INFO:\n" &
            "Total Days: " & total_days.ToString() & "\n" &
            "Quarter No: " & q_no.ToString() & "\n" &
            "Remaining Days: " & d_no.ToString() & "\n\n" &
            "Principal: " & principal_amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) & "\n" &
            "ROI (%): " & rate_of_interest_penal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) & "\n\n" &
            "Quarter Comp Amount: " & quater_cmp_amount_penal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) & "\n" &
            "Accumulated Interest: " & accumulated_interest_penal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) & "\n" &
            "ROI  MATURE(%): " & rate_of_interest_maturity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) & "\n\n" &
            "Quarter Comp Amount MATURE: " & quater_cmp_amount_maturity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) & "\n" &
            "Accumulated Interest MATURE: " & accumulated_interest_maturity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) &
        "');"

        ScriptManager.RegisterStartupScript(Page, Page.GetType(),
        Guid.NewGuid().ToString(),
        scriptDebug,
        True)



    End Sub
    Sub sb_closure_c(ByVal acn As String)

        Session("voucher_date") = tdate.Text
        Session("acn_c") = acn

        Dim prod = Trim(get_pro(Session("product")))
        Dim kmkmin4int As Integer = 0
        Dim cur_kmkmin As Integer = 0
        Dim curmin As Integer = 0
        Dim x As Integer
        Dim frmdt As Date
        Dim tdt As Date
        Dim cur_int As Double = 0
        Dim cum_int As Double = 0

        ' Session("cintr") = get_dint()


        Session("prv_inton") = get_prev_inton_c(acn)
        '   prv_inton = DateAdd(DateInterval.Day, 1, prv_inton)

        While (Session("prv_inton") <= Session("voucher_date"))

            frmdt = Session("prv_inton")
            frmdt = frmdt.AddDays(1)

            tdt = DateAdd(DateInterval.Month, 1, Session("prv_inton"))

            kmkmin4int = get_kmkminc(frmdt.AddDays(10), tdt)
            If Not kmkmin4int <= 0 Then cur_kmkmin = kmkmin4int

            If Not cur_kmkmin <= 200 Then

                cur_int = (cur_kmkmin * Session("cintr") / 100) / 12

                cum_int = cum_int + cur_int
                cur_kmkmin = 0
            End If
            Session("prv_inton") = Session("prv_inton").AddMonths(1)


        End While

        '    If Session("prv_inton") > Session("voucher_date") Then
        ' cum_int = cum_int - cur_int
        ' End If

        frmdt = Session("prv_inton")
        tdt = Session("voucher_date")

        x = frmdt.CompareTo(tdt)

        If x = 1 Then
            cum_int = cum_int - cur_int
        End If

        Session("2bpaid") = String.Format("{0:n}", Math.Round(cum_int))
        'lbl2bpaid_d.Text = String.Format("{0:n}", Math.Round(cum_int))
        get_Cbalance(acn)


        Session("pre_amt") = String.Format("{0:n}", (Session("dbal") + Math.Round(cum_int)))


        If Not CDbl(Session("2bpaid")) = 0 Then
            Session("cbal") = Session("cbal") + CDbl(Session("2bpaid_d"))
            Session("dbal") = Session("dbal") + CDbl(Session("2bpaid_d"))
            update_int_C(Session("tid"), 0, CDbl(Session("2bpaid")), 0, CDbl(Session("2bpaid")), "BY INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, CDbl(Session("2bpaid")), 0, "BY INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", 0, CDbl(Session("2bpaid")), "TO INTEREST", "INTR")

        End If



        update_cld(acn)





        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con



        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@drd", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crc", 0)

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "TO CASH")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To TRANSFER")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()



        End Try


        update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("pre_amt")), "TO CASH", "PAYMENT")

    End Sub


    Sub sb_closure()
        Dim kmkmin4int As Integer = 0
        Dim cur_kmkmin As Integer = 0
        Dim curmin As Integer = 0
        Dim x As Integer
        Dim frmdt As Date
        Dim tdt As Date
        Dim cur_int As Double = 0
        Dim cum_int As Double = 0

        Session("cintr") = get_dint()


        Session("prv_inton") = get_prev_inton()
        '   prv_inton = DateAdd(DateInterval.Day, 1, prv_inton)

        While (Session("prv_inton") <= Session("voucher_date"))

            frmdt = Session("prv_inton")
            frmdt = frmdt.AddDays(1)

            tdt = DateAdd(DateInterval.Month, 1, Session("prv_inton"))

            kmkmin4int = get_kmkmin(frmdt.AddDays(10), tdt)
            If Not kmkmin4int <= 0 Then cur_kmkmin = kmkmin4int

            If Not cur_kmkmin <= 200 Then

                cur_int = (cur_kmkmin * Session("cintr") / 100) / 12

                cum_int = cum_int + cur_int
                cur_kmkmin = 0
            End If
            Session("prv_inton") = Session("prv_inton").AddMonths(1)


        End While

        '    If Session("prv_inton") > Session("voucher_date") Then
        ' cum_int = cum_int - cur_int
        ' End If

        frmdt = Session("prv_inton")
        tdt = Session("voucher_date")

        x = frmdt.CompareTo(tdt)

        If x = 1 Then
            cum_int = cum_int - cur_int
        End If


        lbl2bpaid.Text = String.Format("{0:n}", Math.Round(cum_int))
        lbl2bpaid_d.Text = String.Format("{0:n}", Math.Round(cum_int))

        txtamt.Text = String.Format("{0:n}", (CDbl(lblbal.Text) + Math.Round(cum_int)))
        Session("camt") = txtamt.Text

    End Sub
    Sub calculate_preclosure()

        Select Case Session("product")
            Case "DS"
                ds_closure()
            Case "FD"
                fd_closure()

            Case "KMK"

                kmk_closure()

            Case "RD"
                rd_closure()

            Case "RID"
                rid_closure()

            Case "SB"

                sb_closure()



        End Select

    End Sub

    Function get_kmkminc(ByVal frmdt As Date, ByVal tdt As Date)

        Dim kmkmin As Integer = 0


        Dim ob As Double = get_ob(txtacn.Text, frmdt)
        Dim day_total As Double = 0

        kmkmin = ob

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()


        Dim dr As SqlDataReader

        query = "SELECT SUM([actransc].Drc) AS expr1,SUM([actransc].crc) AS expr2,date FROM dbo.actransc WHERE actransc.acno = @acn AND actransc.date BETWEEN @fdt AND @tdt GROUP BY DATE"

        cmd.Parameters.AddWithValue("@acn", txtacn.Text)
        cmd.Parameters.AddWithValue("@fdt", frmdt)
        cmd.Parameters.AddWithValue("@tdt", tdt)
        cmd.CommandText = query
        cmd.Connection = con

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                ' dr.Read()

                While dr.Read()

                    day_total = ob + IIf(IsDBNull(dr(1)), 0, dr(1)) - IIf(IsDBNull(dr(0)), 0, dr(0))

                    If day_total <= kmkmin Then kmkmin = day_total
                    ob = day_total

                End While

            End If

            dr.Close()



        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()

        End Try


        Return kmkmin
    End Function

    Function get_kmkmin(ByVal frmdt As Date, ByVal tdt As Date)

        Dim kmkmin As Integer = 0


        Dim ob As Double = get_ob(txtacn.Text, frmdt)
        Dim day_total As Double = 0

        kmkmin = ob

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()


        Dim dr As SqlDataReader

        query = "SELECT SUM([actrans].Drc) AS expr1,SUM([actrans].crc) AS expr2,date FROM dbo.actrans WHERE actrans.acno = @acn AND actrans.date BETWEEN @fdt AND @tdt GROUP BY DATE"

        cmd.Parameters.AddWithValue("@acn", txtacn.Text)
        cmd.Parameters.AddWithValue("@fdt", frmdt)
        cmd.Parameters.AddWithValue("@tdt", tdt)
        cmd.CommandText = query
        cmd.Connection = con

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                ' dr.Read()

                While dr.Read()

                    day_total = ob + IIf(IsDBNull(dr(1)), 0, dr(1)) - IIf(IsDBNull(dr(0)), 0, dr(0))

                    If day_total <= kmkmin Then kmkmin = day_total
                    ob = day_total

                End While

            End If

            dr.Close()



        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()

        End Try


        Return kmkmin
    End Function

    Sub kmk_closure()

        Dim kmkmin4int As Integer = 0
        Dim cur_kmkmin As Integer = 0
        Dim curmin As Integer = 0
        Dim x As Integer
        Dim frmdt As Date
        Dim tdt As Date
        Dim cur_int As Double = 0
        Dim cum_int As Double = 0
        Session("cintr") = get_dint()



        Session("prv_inton") = get_prev_inton()
        Session("prv_inton") = DateAdd(DateInterval.Day, 1, Session("prv_inton"))

        While (Session("prv_inton") <= Session("voucher_date"))

            frmdt = Session("prv_inton")
            tdt = frmdt.AddDays(7) 'DateAdd(DateInterval.Day, 6, Session("prv_inton"))

            kmkmin4int = get_kmkmin(frmdt, tdt)
            If Not kmkmin4int = 0 Then cur_kmkmin = kmkmin4int

            If Not cur_kmkmin <= 200 Then
                cur_int = 0
                cur_int = (cur_kmkmin * Session("cintr") / 100) / 12 / 4

                cum_int = cum_int + cur_int

                Session("prv_inton") = tdt
            Else
                Session("prv_inton") = tdt.AddDays(1)
            End If



        End While

        frmdt = Session("prv_inton")
        tdt = Session("voucher_date")

        x = frmdt.CompareTo(tdt)

        If x = 1 Then
            cum_int = cum_int - cur_int
        End If

        lbl2bpaid.Text = String.Format("{0:n}", Math.Round(cum_int))
        lbl2bpaid_d.Text = String.Format("{0:n}", Math.Round(cum_int))

        txtamt.Text = String.Format("{0:n}", (Session("bal") + Math.Round(cum_int)))
        Session("camt") = txtamt.Text




    End Sub


    Sub kmk_closure_c(ByVal acn As String)
        Session("acn_c") = acn
        Session("voucher_date") = tdate.Text
        Dim prod = Trim(get_pro(Session("product")))

        Dim kmkmin4int As Integer = 0
        Dim cur_kmkmin As Integer = 0
        Dim curmin As Integer = 0
        Dim x As Integer
        Dim frmdt As Date
        Dim tdt As Date
        Dim cur_int As Double = 0
        Dim cum_int As Double = 0
        'Session("cintr") = get_d()



        Session("prv_inton") = get_prev_inton_c(acn)
        Session("prv_inton") = DateAdd(DateInterval.Day, 1, Session("prv_inton"))

        While (Session("prv_inton") <= Session("voucher_date"))

            frmdt = Session("prv_inton")
            tdt = frmdt.AddDays(7) 'DateAdd(DateInterval.Day, 6, Session("prv_inton"))

            kmkmin4int = get_kmkminc(frmdt, tdt)
            If Not kmkmin4int = 0 Then cur_kmkmin = kmkmin4int

            If Not cur_kmkmin <= 200 Then
                cur_int = 0
                cur_int = (cur_kmkmin * Session("cintr") / 100) / 12 / 4

                cum_int = cum_int + cur_int

                Session("prv_inton") = tdt
            Else
                Session("prv_inton") = tdt.AddDays(1)
            End If



        End While

        frmdt = Session("prv_inton")
        tdt = Session("voucher_date")

        x = frmdt.CompareTo(tdt)

        If x = 1 Then
            cum_int = cum_int - cur_int
        End If

        'get_Cbalance(acn)


        Session("2bpaid") = String.Format("{0:n}", Math.Round(cum_int))
        'lbl2bpaid_d.Text = String.Format("{0:n}", Math.Round(cum_int))
        get_Cbalance(acn)


        Session("pre_amt") = String.Format("{0:n}", (Session("dbal") + Math.Round(cum_int)))


        If Not CDbl(Session("2bpaid")) = 0 Then
            Session("cbal") = Session("cbal") + CDbl(Session("2bpaid_d"))
            Session("dbal") = Session("dbal") + CDbl(Session("2bpaid_d"))
            update_int_C(Session("tid"), 0, CDbl(Session("2bpaid")), 0, CDbl(Session("2bpaid")), "BY INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, CDbl(Session("2bpaid")), 0, "BY INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", 0, CDbl(Session("2bpaid")), "TO INTEREST", "INTR")

        End If



        update_cld(acn)





        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con



        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@drd", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crc", 0)

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "TO CASH")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To TRANSFER")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()



        End Try


        update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("pre_amt")), "TO CASH", "PAYMENT")




    End Sub

    Function getpaydate()

        'allowaftr
        Dim ds As New DataSet
        Dim clo As Integer
        Dim alafter As String
        ' Maturity Date
        'Last Receipt
        If con.State = ConnectionState.Closed Then con.Open()

        Try

            'Dim sql As String = "select cint,dint from roi where Product = '" + Trim(deptyp.SelectedItem.Text) + "'" And "fyfrm >= '" + ddt.Text + "'" And "fyto <= '" + ddt.Text + "'"
            Dim sql As String = "SELECT closure,allowaftr from roi where roi.product = '" + Session("product") + "'"
            sql &= "AND roi.prddmy = '" + Session("prdtyp").ToString + "'"
            sql &= "AND roi.prdfrm <= " + Session("prd").ToString
            sql &= "AND roi.prdto >= " + Session("prd").ToString

            cmd.Connection = con
            cmd.CommandText = sql

            Dim Adapter As New SqlDataAdapter(sql, con)
            Adapter.Fill(ds)

            If Not ds.Tables(0).Rows.Count = 0 Then

                clo = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(0)), 0, ds.Tables(0).Rows(0).Item(0))
                alafter = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(1)), 0, ds.Tables(0).Rows(0).Item(1))

                If Trim(alafter) = "Maturity Date" Then

                    Session("paydt") = Session("mdt").AddDays(clo)
                ElseIf Trim(alafter) = "Last Receipt" Then

                    Session("paydt") = Session("mdt").AddDays(clo - Session("days_ago"))


                End If

            End If
        Finally
            cmd.Dispose()
            con.Close()

        End Try

        Return Session("paydt")

    End Function

    Sub getPreClsROI(ByVal month As Integer)
        If con.State = ConnectionState.Closed Then con.Open()
        Try
            Dim preClsDate = CDate(Session("ac_date"))
            Dim sql As String = "SELECT cint,dint from roi where roi.product = '" + Session("product") + "'"
            sql &= "AND roi.prddmy = '" + Session("prdtyp").ToString + "'"
            sql &= "AND roi.prdfrm <= " + month.ToString
            sql &= "AND roi.prdto >= " + month.ToString
            sql &= "AND '" + preClsDate.Year.ToString() + "-" + preClsDate.Month.ToString() + "-" + preClsDate.Day.ToString() + "' BETWEEN roi.fyfrm AND roi.fyto"

            cmd.Connection = con
            cmd.CommandText = sql

            Using reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Session("precls_croi") = reader("cint")
                    Session("precls_droi") = reader("dint")
                End If
            End Using
        Catch ex As Exception
            Console.WriteLine(ex)
        Finally
            cmd.Dispose()
            con.Close()
        End Try
    End Sub
    'Private Sub bind_grid()

    '    Dim ds_trans As New DataSet

    '    If con.State = ConnectionState.Closed Then con.Open()


    '    disp.EmptyDataText = "No Records Found"

    '    Dim sql As String = "SELECT date,narration,drd,crd,cbal FROM dbo.[actrans] WHERE actrans.acno='" + txtacn.Text + "'"

    '    Dim adapter As New SqlDataAdapter(sql, con)

    '    Try

    '        adapter.Fill(ds_trans)



    '        disp.DataSource = ds_trans
    '        disp.DataBind()


    '    Catch ex As Exception
    '        response.write(ex.ToString)

    '    Finally
    '        ds_trans.Dispose()
    '        adapter.Dispose()
    '        cmd.Dispose()
    '        con.Close()


    '    End Try

    '    trim_disp()


    'End Sub

    'Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
    '    disp.PageIndex = e.NewPageIndex

    '    total = pgtot(disp.PageIndex - 1).ToString

    '    bind_grid()
    'End Sub

    'Sub trim_disp()

    '    For i As Integer = 0 To disp.Rows.Count - 1

    '        Dim cr As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblcr"), Label)
    '        Dim dr As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbldr"), Label)
    '        Dim bal As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblbal"), Label)


    '        If cr.Text = 0 Then
    '            cr.Text = ""
    '        Else
    '            total = total + CDbl(cr.Text)
    '        End If

    '        If dr.Text = 0 Then
    '            dr.Text = ""
    '        Else
    '            total = total - CDbl(dr.Text)
    '        End If

    '        'If bal.Text = 0 Then
    '        '    bal.Text = ""
    '        'End If
    '        bal.Text = String.Format("{0:N}", total)



    '    Next
    '    pgtot(disp.PageIndex) = total
    'End Sub

    Sub get_last_transaction(ByVal acn As String)

        If con.State = ConnectionState.Closed Then con.Open()


        Dim oResult As Date

        Try

            cmdx.Connection = con
            cmdx.CommandText = "SELECT top 1 date  FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' and type<>'INTR' ORDER BY date DESC"

            oResult = cmdx.ExecuteScalar()

            If Not oResult = Date.MinValue Then
                Dim tdat As Date = Convert.ToDateTime(tdate.Text)
                Session("days_ago") = DateDiff(DateInterval.Day, oResult, tdat)
            Else
                Session("days_ago") = 0
            End If

        Finally
            cmd.Dispose()
            con.Close()


        End Try

    End Sub

    Sub get_last_transaction_c(ByVal acn As String)

        If con.State = ConnectionState.Closed Then con.Open()


        Dim oResult As Date

        Try

            cmdx.Connection = con
            cmdx.CommandText = "SELECT top 1 date  FROM dbo.[actransc] WHERE [actransc].acno ='" + acn + "' and type='CASH' ORDER BY date DESC"

            oResult = cmdx.ExecuteScalar()

            If Not oResult = Date.MinValue Then
                Dim tdat As Date = Convert.ToDateTime(tdate.Text)
                Session("days_ago") = DateDiff(DateInterval.Day, oResult, tdat)
            Else
                Session("days_ago") = 0
            End If

        Finally
            cmd.Dispose()
            con.Close()


        End Try

    End Sub

    Function get_due(ByVal acn As String, ByVal curdue As Integer)

        If con.State = ConnectionState.Closed Then con.Open()



        Dim df As String = "dMMM-yyyy"
        Dim op As String
        Dim opt As DateTime
        Try
            Dim sql As String = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].Type = 'CASH'"

            cmdi.Connection = con
            cmdi.CommandText = sql

            countresult = cmdi.ExecuteScalar()

            If countresult = 0 Then

                opt = Convert.ToDateTime(Session("ac_date"))
                op = opt.ToString("y")

            Else

                Dim curdue_period As Date = DateAdd(DateInterval.Month, (countresult + curdue), Session("ac_date"))

                op = curdue_period.ToString("y")


            End If

        Finally
            cmd.Dispose()
            con.Close()

        End Try


        Return op
    End Function

    Sub calculate_penalty()


        If con.State = ConnectionState.Closed Then con.Open()
        Try
            Dim sql As String = "SELECT roi.penalty,roi.penaltyprd FROM dbo.roi WHERE roi.Product = '" + Session("product") + "'"
            sql &= "AND roi.prddmy = '" + Session("prdtyp").ToString + "'"
            sql &= "AND roi.prdfrm <= " + Session("prd").ToString
            sql &= "AND roi.prdto >= " + Session("prd").ToString


            Dim penalty_ds As New DataSet

            Dim penalty_adapter As New SqlDataAdapter(sql, con)


            penalty_adapter.Fill(penalty_ds)

            If Not penalty_ds.Tables(0).Rows.Count = 0 Then

                If Session("days_ago") >= ds.Tables(0).Rows(0).Item(1) Then

                    Dim perhundred As Integer = penalty_ds.Tables(0).Rows(0).Item(0)
                    Dim penalprd As Integer = penalty_ds.Tables(0).Rows(0).Item(1)

                    '    txtpenalty.Text = ((amt * perhundred) / 100) * days_ago
                    '   rdpenal.Visible = True
                End If


            End If
        Finally
            cmd.Dispose()
            con.Close()

        End Try

    End Sub



    Private Sub txtacn_TextChanged(sender As Object, e As EventArgs) Handles txtacn.TextChanged



        get_ac_info(txtacn.Text)
        txtfocus(tdate)

    End Sub

    Function get_pro(ByVal prd As String)

        Dim prdname As String = ""

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT name from products where products.shortname='" + prd + "'"
                cmd.CommandText = query
                prdname = cmd.ExecuteScalar()

            End Using
        End Using



        Return prdname
    End Function

    Sub set_diff(ByVal ovr As Double)
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con

        cmd.Parameters.Clear()
        query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
        query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@product", Session("product"))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("dr", IIf(ovr < 0, -ovr, 0))
        cmd.Parameters.AddWithValue("cr", IIf(ovr > 0, ovr, 0))

        Try

            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally

            cmd.Dispose()
            con.Close()


        End Try


    End Sub
    Private Sub update_int(ByVal tid As Double, ByVal drd As Double, ByVal crd As Double, ByVal drc As Double, ByVal crc As Double, ByVal nar As String, ByVal supliment As String, ByVal cbal As Double, ByVal dbal As Double)


        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con




        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"

        cmd.Parameters.Clear()

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@uiid", tid)
        cmd.Parameters.AddWithValue("@uidate", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@uiacno", txtacn.Text)
        cmd.Parameters.AddWithValue("@uidrd", drd)
        cmd.Parameters.AddWithValue("@uicrd", crd)
        cmd.Parameters.AddWithValue("@uidrc", drc)
        cmd.Parameters.AddWithValue("@uicrc", crc)
        cmd.Parameters.AddWithValue("@uinarration", nar)
        cmd.Parameters.AddWithValue("@uidue", " ")
        cmd.Parameters.AddWithValue("@uitype", "INTR")
        cmd.Parameters.AddWithValue("@uisuplimentry", supliment)
        cmd.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@uicbal", cbal)
        cmd.Parameters.AddWithValue("@uidbal", dbal)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()
        End Try


    End Sub

    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)



        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        If mop.SelectedItem.Text = "Cash" Then
            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            cmd.Parameters.AddWithValue("@nar", nar)
            cmd.Parameters.AddWithValue("@typ", typ)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try
        Else
            cmd.Parameters.Clear()

            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            Else
                If typ = "INTR" Then
                    cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                Else
                    cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
                End If

            End If
            If typ = "INTR" Then
                cmd.Parameters.AddWithValue("@nar", nar)
            Else
                If mop.SelectedItem.Text = "Account" Then
                    '  cmd.Parameters.AddWithValue("@nar", nar + " (" + Trim(txtacn.Text) + ")")
                    cmd.Parameters.AddWithValue("@nar", "To Transfer " + Trim(txtacn.Text) + " (" + Trim(txt_sb.Text) + ")")
                Else
                    cmd.Parameters.AddWithValue("@nar", nar + " (" + Trim(bnk.SelectedItem.Text) + ")")
                End If


            End If

            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try


            If typ = "INTR" Then GoTo nxt



            cmd.Parameters.Clear()


            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@achead", bnk.SelectedItem.Text)
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            Else
                If Left(Trim(txt_sb.Text), 2) = "79" Then
                    cmd.Parameters.AddWithValue("@achead", "SAVINGS DEPOSIT")
                Else
                    cmd.Parameters.AddWithValue("@achead", "KMK DEPOSIT")
                End If
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            End If
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", dr)
            If typ = "JOURNAL" Then
                If mop.SelectedItem.Text = "Transfer" Then
                    cmd.Parameters.AddWithValue("@nar", "By Transfer ")
                Else
                    cmd.Parameters.AddWithValue("@nar", "By Transfer " + Trim(txt_sb.Text) + " (" + Trim(txtacn.Text) + ")")
                End If

            Else

                cmd.Parameters.AddWithValue("@nar", nar)
            End If

            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            If Not Session("camt") = CDbl(txtamt.Text) Then


                cmd.Parameters.Clear()

                query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
                cmd.Parameters.AddWithValue("@transid", tid)
                If mop.SelectedItem.Text = "Account" Then
                    If Left(Trim(txt_sb.Text), 2) = "79" Then
                        cmd.Parameters.AddWithValue("@achead", "SAVINGS DEPOSIT")
                    Else
                        cmd.Parameters.AddWithValue("@achead", "KMK DEPOSIT")
                    End If
                    cmd.Parameters.AddWithValue("@nar", "By Cash " + "(" + txtacn.Text + ")")
                Else
                    cmd.Parameters.AddWithValue("@achead", bnk.SelectedItem.Text)
                    cmd.Parameters.AddWithValue("@nar", "By Transfer " + txtacn.Text)
                End If

                cmd.Parameters.AddWithValue("@debit", 0)
                cmd.Parameters.AddWithValue("@credit", CDbl(txtamt.Text) - Session("camt"))
                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)

                cmd.Parameters.AddWithValue("@typ", "RECEIPT")
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try


            End If


        End If

nxt:

        query = ""




    End Sub
    Private Sub set_changes()


        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            'log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("..\login.aspx")

        End If

        If session_user_role = "Audit" Then Exit Sub


        Session("drd") = txtamt.Text '(totint_penal_d - intpaid_d) + dbal
        Session("drc") = txtamt.Text  '(totint_penal - intpaid) + cbal
        Session("crd") = 0
        Session("crc") = 0
        ' cbal = 0
        ' dbal = 0 'dbal - (totint_penal_d - intpaid_d)


        Dim prod = Trim(get_pro(Session("product")))


        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con


        'Dim d As String = get_due(txtacn.Text, 0)

        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(Session("camt")))
        cmd.Parameters.AddWithValue("@crc", 0)

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "To Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()



        End Try






        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()

        query = "UPDATE master SET cld = @tscroll where master.acno= @tacn"
        cmd.Parameters.Clear()
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@tscroll", 1)
        cmd.Parameters.AddWithValue("@tacn", txtacn.Text)
        cmd.Connection = con

        Try

            If con.State = ConnectionState.Closed Then con.Open()



            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()

        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()

        query = "UPDATE stdins SET cld = @tscroll where stdins.acno= @tacn"
        cmd.Parameters.Clear()
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@tscroll", 1)
        cmd.Parameters.AddWithValue("@tacn", txtacn.Text)
        cmd.Connection = con

        Try
            If con.State = ConnectionState.Closed Then con.Open()



            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()

        End Try



        query = ""
        If con.State = ConnectionState.Closed Then con.Open()
        query = "SELECT MAX(ID) from dbo.trans where trans.acno='" + txtacn.Text + "'"
        cmd.CommandText = query
        Try
            countresult = cmd.ExecuteScalar()
        Finally
            cmd.Dispose()
            con.Close()
        End Try
        Session("tid") = Convert.ToString(countresult)





        If Not CDbl(lbl2bpaid.Text) = 0 Then

            Session("cbal") = Session("cbal") + CDbl(lbl2bpaid.Text)
            Session("dbal") = Session("dbal") + CDbl(lbl2bpaid_d.Text)
            update_int(Session("tid"), 0, CDbl(lbl2bpaid_d.Text), 0, CDbl(lbl2bpaid.Text), "By Interest", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry(Session("tid"), prod, CDbl(lbl2bpaid.Text), 0, "By Interest", "INTR")
            update_suplementry(Session("tid"), Trim(Session("product")) + " INTEREST", 0, CDbl(lbl2bpaid.Text), "To Interest", "INTR")

        End If

        If Not CDbl(lblpenal.Text) = 0 Then

            update_int(Session("tid"), CDbl(lblpenal_d.Text), 0, CDbl(lblpenal.Text), 0, "To Interest", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry(Session("tid"), prod, 0, CDbl(lblpenal.Text), "To Interest", "INTR")
            update_suplementry(Session("tid"), Trim(Session("product")) + " INTEREST", CDbl(lblpenal.Text), 0, "By Interest", "INTR")

        End If







        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con


        'Dim d As String = get_due(txtacn.Text, 0)

        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(Session("camt")))
        cmd.Parameters.AddWithValue("@crc", 0)

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "To Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()



        End Try







        'If prod = "RECURRING DEPOSIT" Then

        '    If con.State = ConnectionState.Closed Then con.Open()
        '    cmd.Connection = con
        '    cmd.Parameters.Clear()

        '    query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        '    query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        '    cmd.CommandText = query
        '    cmd.Parameters.AddWithValue("@id", Session("tid"))
        '    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        '    cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        '    cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text))
        '    cmd.Parameters.AddWithValue("@crd", 0)
        '    cmd.Parameters.AddWithValue("@drc", CDbl(txtamt.Text))
        '    cmd.Parameters.AddWithValue("@crc", 0)
        '    cmd.Parameters.AddWithValue("@narration", "TO CASH")
        '    cmd.Parameters.AddWithValue("@due", " ")
        '    cmd.Parameters.AddWithValue("@type", "CASH")
        '    cmd.Parameters.AddWithValue("@suplimentry", prod)
        '    cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        '    cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        '    cmd.Parameters.AddWithValue("@cbal", 0)
        '    cmd.Parameters.AddWithValue("@dbal", 0)


        '    Try
        '        cmd.ExecuteNonQuery()
        '    Catch ex As Exception


        '        response.write(ex.Message)

        '    Finally

        '        cmd.Dispose()
        '        con.Close()



        '    End Try

        'End If

        If mop.SelectedItem.Text = "Cash" Then
            update_suplementry(Session("tid"), prod, 0, Session("camt"), "To Cash", "PAYMENT")
        Else
            update_suplementry(Session("tid"), prod, 0, Session("camt"), "To Transfer", "JOURNAL")
        End If




        Session("ovr") = Session("camt") - CDbl(txtamt.Text)

        If Not Session("ovr") = 0 Then
            set_diff(Session("ovr"))
        End If

        If mop.SelectedItem.Text = "Account" Then
            update_sb()
        End If


        set_changes_c()




        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

        'sb.Append("<div class=" + """alert alert-dismissable alert-primary """ + ">")
        'sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")tid
        'sb.Append("<strong>Updated !</strong> Transaction Id is " + tid)
        'sb.Append("</div>")
        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())


        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transaction Completed. ID #" + Session("tid")
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


        If mop.SelectedItem.Text = "Cash" Then
            clear_tab_recpt()
        Else
            prepare_print()
        End If


    End Sub
    Sub prepare_print()

        Dim diffAmt As Boolean = False
        If Not CDbl(txtamt.Text) - CDbl(Session("camt")) = 0 Then
            diffAmtPrint.Visible = True
            diffAmt = True
        Else
            diffAmtPrint.Visible = False
        End If
        camt.Value = FormatCurrency(Session("camt"))
        cword.Value = get_wrds(Session("camt"))
        damt.Value = FormatCurrency(txtamt.Text)
        dword.Value = get_wrds(txtamt.Text)

        lblcpt.Text = "PAYMENT"
        lblcptr.Text = "CUSTOMER COPY"
        pvno.Text = Session("tid")
        pdate.Text = tdate.Text
        pbranch.Text = get_home()
        pacno.Text = txtacn.Text
        pglh.Text = get_pro(lblproduct.Text)
        pcid.Text = get_memberno(txtacn.Text)
        pcname.Text = lblname.Text
        pamt.Text = FormatCurrency(txtamt.Text)
        paiw.Text = get_wrds(txtamt.Text)
        premit.Text = pcname.Text

        If mop.SelectedItem.Text = "Account" Then
            pnar.Text = "To Transfer " + txt_sb.Text
        Else
            pnar.Text = "To Transfer " + bnk.SelectedItem.Text
        End If


        lblccpt.Text = "RECEIPT"
        lblccptr.Text = "CUSTOMER COPY"
        pcvno.Text = Session("tid")
        pcdate.Text = tdate.Text
        pcbranch.Text = get_home()
        If mop.SelectedItem.Text = "Account" Then
            pcacno.Text = txt_sb.Text
            If Left(txt_sb.Text, 2) = "79" Then
                pcglh.Text = "SAVINGS DEPOSIT"
            Else
                pcglh.Text = "KMK DEPOSIT"
            End If

            pccid.Text = get_memberno(txt_sb.Text)
            pccname.Text = get_membername(txt_sb.Text)
            pcnar.Text = "By Transfer  " + txtacn.Text
        Else
            pcacno.Text = txtacn.Text
            pcglh.Text = bnk.SelectedItem.Text
            pccid.Text = get_memberno(txtacn.Text)
            pccname.Text = lblname.Text
            pcnar.Text = "By Transfer  " + txtacn.Text
        End If
        pcamt.Text = FormatCurrency(txtamt.Text)
        pcaiw.Text = get_wrds(txtamt.Text)
        pcremit.Text = pccname.Text
        ' clear_tab_recpt()
        If diffAmt Then
            Dim diff = Math.Max(CDbl(txtamt.Text), CDbl(Session("camt"))) - Math.Min(CDbl(txtamt.Text), CDbl(Session("camt")))
            lblccdpt.Text = "RECEIPT"
            lblccdptr.Text = lblccptr.Text
            pcdvno.Text = Session("tid")
            pcddate.Text = tdate.Text
            pcdbranch.Text = get_home()
            pcdacno.Text = pcacno.Text
            pcdglh.Text = pcglh.Text
            pcdcid.Text = pccid.Text
            pcdcname.Text = pccname.Text
            pcdnar.Text = "By Cash  " + txtacn.Text
            pcdamt.Text = FormatCurrency(diff)
            pcdaiw.Text = get_wrds(diff)
            pcdremit.Text = pccname.Text
        End If


        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "block")
        pnltrans.Style.Add(HtmlTextWriterStyle.Display, "none")
    End Sub

    Function get_membername(ByVal acn As String)
        Dim cid As String = Nothing
        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Parameters.Clear()

        query = "SELECT member.firstname FROM dbo.master LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo WHERE master.acno = @acno"
        cmdi.Connection = con
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acno", acn)
        Try

            cid = cmdi.ExecuteScalar()



        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Return cid

    End Function

    Function get_memberno(ByVal acn As String)
        Dim cid As String = Nothing
        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Parameters.Clear()

        query = "SELECT member.memberno FROM dbo.master LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo WHERE master.acno = @acno"
        cmdi.Connection = con
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acno", acn)
        Try

            cid = cmdi.ExecuteScalar()



        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Return cid

    End Function
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
    Private Sub btn_up_rcpt_Click(sender As Object, e As EventArgs) Handles btn_up_rcpt.Click
        'txtamt.ReadOnly = True
        btn_up_rcpt.Enabled = False

        set_changes()


    End Sub
    Sub clear_tab_recpt()
        Session("tid") = Nothing
        alertmsg.Visible = False
        txtacn.Text = ""
        lblamt.Text = ""
        lblbal.Text = ""
        MaturityLabel.Text = ""
        lblname.Text = ""
        lblproduct.Text = ""
        ' txtnod.Text = ""
        txtamt.Text = ""
        pnlint.Visible = False
        'TabContainer1.Visible = False
        txtacn.Enabled = True
        txtfocus(txtacn)
        'pnltran.Visible = False

        'pnlsbtrf.Visible = False
        bnk.Visible = False
        lbl.Visible = False

        mop.SelectedItem.Text = "Cash"
        bnk.Items.Clear()

        lbl_sb_bal.Text = ""
        txt_sb.Text = ""

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        txtfocus(txtacn)



    End Sub
    'Sub update_rd()

    '    Dim query As String
    '    Dim i As Integer



    '    cmd.Connection = con

    '    For i = 1 To txtnod.Text Step 1
    '        Dim d As String = get_due(txtacn.Text, i - 1)

    '        Query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type)"
    '        Query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type)"

    '        cmd.CommandText = Query
    '        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
    '        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
    '        cmd.Parameters.AddWithValue("@drd", 0)
    '        cmd.Parameters.AddWithValue("@crd", amt)
    '        cmd.Parameters.AddWithValue("@drc", 0)
    '        cmd.Parameters.AddWithValue("@crc", amt)
    '        cmd.Parameters.AddWithValue("@narration", "BY CASH")
    '        cmd.Parameters.AddWithValue("@due", d)
    '        cmd.Parameters.AddWithValue("@type", "CASH")


    '        Try
    '            cmd.ExecuteNonQuery()

    '            cmd.Parameters.Clear()


    '        Catch ex As Exception

    '            response.write(ex.Message)
    '        End Try

    '    Next


    '    query = ""
    '    cmd.CommandText = Nothing
    '    cmd.Connection = Nothing

    '    Query = "SELECT MAX(ID) from dbo.trans where trans.acno='" + txtacn.Text + "'"
    '    cmd.Connection = con
    '    cmd.CommandText = query

    '    countresult = cmd.ExecuteScalar()

    '    tid = Convert.ToString(countresult)

    '    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

    '    sb.Append("<div class=" + """alert alert-dismissable alert-info """ + ">")
    '    sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
    '    sb.Append("<strong>Updated !</strong> Transaction Id is " + tid)
    '    sb.Append("</div>")
    '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())

    'End Sub



    Private Sub tdate_TextChanged(sender As Object, e As EventArgs) Handles tdate.TextChanged
        On Error GoTo nxt

        get_ac_info(txtacn.Text)
        ' txtfocus(txtnod)
        ' txtfocus(btn_up_rcpt)
        Exit Sub
nxt:
        txtfocus(tdate)

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub


    Private Sub btn_up_can_Click(sender As Object, e As EventArgs) Handles btn_up_can.Click
        clear_tab_recpt()
    End Sub

    Private Sub lbl2bpaid_TextChanged(sender As Object, e As EventArgs) Handles lbl2bpaid.TextChanged

        If Not lbl2bpaid.Text = "" Then

            get_balance(Trim(txtacn.Text))

            lbl2bpaid.Text = FormatNumber(lbl2bpaid.Text)
            'lbl2bpaid_d.Text = lbl2bpaid.Text
            Session("camt") = (Session("cbal") + CDbl(lbl2bpaid.Text) - CDbl(lblpenal.Text))

        End If
        txtamt.Focus()

    End Sub



    Private Sub mop_TextChanged(sender As Object, e As EventArgs) Handles mop.TextChanged
        If mop.SelectedItem.Text = "Transfer" Then

            If Session("dl_bal") < 0 Then
                btn_up_rcpt.Visible = True
            End If


            bnk.Items.Clear()
            Dim dr As SqlDataReader

            query = "SELECT ledger.ledger FROM dbo.ledger WHERE ledger.printorder = '1' "

            If con.State = ConnectionState.Closed Then con.Open()

            cmdi.Connection = con
            cmdi.CommandText = query

            Try

                dr = cmdi.ExecuteReader()

                If dr.HasRows Then
                    Do While dr.Read()

                        bnk.Items.Add(dr(0))

                    Loop
                End If
                bnk.Items.Insert(0, "<-- Select -->")
                bnk.Items.Insert(1, "HEAD OFFICE")
                bnk.Items.Item(0).Value = ""
                dr.Close()
                ' pnltran.Visible = True
                bnk.Visible = True
                lbl.Visible = True
                'pnlsbtrf.Visible = False
                txt_sb.Visible = False
                'btn_trf.Visible = False
                lblsb.Visible = False
                lbl_sb_bal.Text = ""
            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally

            End Try

            bnk.Focus()

        ElseIf mop.SelectedItem.Text = "Account" Then

            bnk.Visible = False
            lbl.Visible = False
            ' pnlsbtrf.Visible = True
            txt_sb.Visible = True
            'btn_trf.Visible = True
            lbl_sb_bal.Text = ""
            lblsb.Visible = True
            txt_sb.Focus()

            If Session("dl_bal") < 0 Then
                btn_up_rcpt.Visible = True
            End If

        Else

            If Session("dl_bal") < 0 Then
                btn_up_rcpt.Visible = False
            End If

            'pnltran.Visible = False
            bnk.Visible = False
            lbl.Visible = False
            lblsb.Visible = False
            'pnlsbtrf.Visible = False
            txt_sb.Visible = False
            ' btn_trf.Visible = False
            lbl_sb_bal.Text = ""
            mop.Focus()

        End If
    End Sub


    Sub update_sb()


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        cmd.Connection = con
        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal,scroll)"
        query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal,@scroll)"
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", CDbl(Session("camt")))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", CDbl(Session("camt")))
        cmd.Parameters.AddWithValue("@narration", "By Transfer ")
        cmd.Parameters.AddWithValue("@due", txtacn.Text)
        cmd.Parameters.AddWithValue("@type", "TRF")
        cmd.Parameters.AddWithValue("@sup", "SAVINGS DEPOSIT")
        cmd.Parameters.AddWithValue("@usr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)
        If Session("dl_bal") < 0 Then
            cmd.Parameters.AddWithValue("@scroll", 1)
        Else
            cmd.Parameters.AddWithValue("@scroll", 0)
        End If
        Try
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con
            cmd.CommandText = query
            cmd.ExecuteNonQuery()
        Catch EX As Exception
            Response.Write(EX.ToString)
        Finally
            cmd.Parameters.Clear()
            cmd.Dispose()
            con.Close()
        End Try

        If Not Session("ovr") = 0 Then
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Parameters.Clear()
            cmd.Connection = con
            query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", CDbl(txtamt.Text) - CDbl(Session("camt")))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", CDbl(txtamt.Text) - CDbl(Session("camt")))
            cmd.Parameters.AddWithValue("@narration", "By CASH ")
            cmd.Parameters.AddWithValue("@due", txtacn.Text)
            cmd.Parameters.AddWithValue("@type", "CASH")
            cmd.Parameters.AddWithValue("@sup", "SAVINGS DEPOSIT")
            cmd.Parameters.AddWithValue("@usr", Session("sesusr"))
            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
            cmd.Parameters.AddWithValue("@cbal", 0)
            cmd.Parameters.AddWithValue("@dbal", 0)
            Try
                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                cmd.CommandText = query
                cmd.ExecuteNonQuery()
            Catch EX As Exception
                Response.Write(EX.ToString)
            Finally
                cmd.Parameters.Clear()
                cmd.Dispose()
                con.Close()
            End Try


        End If


    End Sub

    Private Sub lblpenal_TextChanged(sender As Object, e As EventArgs) Handles lblpenal.TextChanged
        If Not lblpenal.Text = "" Then
            get_balance(Trim(txtacn.Text))
            lblpenal.Text = FormatCurrency(lblpenal.Text)
            'lblpenal_d.Text = lblpenal.Text
            Session("camt") = (Session("cbal") + CDbl(lbl2bpaid.Text) - CDbl(lblpenal.Text))
        End If



        txtamt.Focus()

    End Sub

    Private Sub DepositClosure_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub soa_Click(sender As Object, e As EventArgs) Handles soa.Click
        If Not Trim(txtacn.Text) = "" Then

            Response.Redirect("soadeposit.aspx?acno=" + Trim(txtacn.Text))

        End If
    End Sub

    Sub set_changes_c()

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT acno,date,amount,mdate,cint,prd from masterc where masterc.parent='" & Trim(txtacn.Text) + "'"
                cmd.CommandText = query

                Using reader As SqlDataReader = cmd.ExecuteReader

                    If reader.HasRows Then
                        Do While reader.Read

                            Session("ac_date") = CDate(reader(1).ToString)
                            Session("amount") = reader(2).ToString
                            Session("mdate") = CDate(reader(3).ToString)
                            Session("cintr") = CDbl(reader(4).ToString)
                            Session("prd") = CDbl(reader(5).ToString)

                            Select Case Trim(lblproduct.Text)
                                Case "DS"
                                    ds_closure_c(Trim(reader(0).ToString))

                                Case "FD"

                                    fd_closure_c(Trim(reader(0).ToString))
                                Case "KMK"
                                    kmk_closure_c((Trim(reader(0).ToString)))
                                Case "RD"
                                    rd_closure_c(Trim(reader(0).ToString))
                                Case "RID"
                                    rid_closure_c(Trim(reader(0).ToString))

                                Case "SB"
                                    sb_closure_c(Trim(reader(0).ToString))

                            End Select
                        Loop
                    End If

                End Using


            End Using
        End Using



    End Sub




    Sub fd_closure_c(ByVal acn As String)

        'get_soa_details(acn)

        '  Session("ac_date") = reader(1).ToString
        '  Session("amount") = reader(2).ToString
        '  Session("mdate") = reader(3).ToString

        get_Cbalance(acn)
        Session("acn_c") = acn


        Dim intc_amt As Integer
        Dim intd_amt As Integer
        Dim tdays As Integer
        Dim days As Integer
        Dim prod = Trim(get_pro(Session("product")))

        Dim x As Integer = CDate(Session("mdate")).CompareTo(CDate(tdate.Text))

        '== chk for prematurity 
        If Not x <= 0 Then



            Session("prv_inton") = get_prev_inton()
            tdays = DateDiff(DateInterval.Day, Session("ac_date"), Session("prv_inton"))
            days = tdays Mod (30)
            Session("days_ago") = (tdays - days) / 30
            Session("prd_buffer") = Session("days_ago")
            Session("prd_buffer_d") = Session("prd_buffer")
            getint()

            Session("penalcut") = get_penalcut()
            Session("penal_cintr") = Session("cintr_pre") - Session("penalcut")
            Session("penal_dint") = Session("dint_pre") - Session("penalcut")
            intc_amt = Math.Round((Session("cbal") * Session("cintr") / 100 / 12) * Session("prd_buffer"))
            intd_amt = Math.Round((Session("cbal") * Session("penal_dint") / 100 / 12) * Session("prd_buffer"))
            Session("totint") = intc_amt
            Session("totint_penal") = intd_amt
            Session("intpaid") = get_intpaid_c(acn)
            Session("intpaid_d") = get_IntpaidD_c(acn)

            '   lblperiod.Text = Convert.ToString(Session("prd_buffer")) + " Months"
            '   lblpenalperiod.Text = Convert.ToString(Session("prd_buffer")) + " Months"
            '  lblroi.Text = String.Format("{0:N}", Session("cintr"))
            '  lblpenalroi.Text = String.Format("{0:N}", Session("penal_cintr"))
            '  lblactualint.Text = String.Format("{0:N}", Session("totint"))
            '  lblpenalint.Text = String.Format("{0:N}", Session("totint_penal"))
            '  lblintpaid.Text = String.Format("{0:N}", Session("intpaid"))
            '  lblpenalintpaid.Text = String.Format("{0:N}", Session("intpaid"))
            '  lbl2bpaid.Text = String.Format("{0:N}", (Session("totint") - Session("intpaid")))
            ' lblpenal2bpaid.Text = String.Format("{0:N}", (Session("totint_penal") - Session("intpaid")))
            ' lblpenal.Text = String.Format("{0:N}", (Session("totint") - Session("totint_penal")))
            ' lblperiod_d.Text = lblperiod.Text
            ' lblpenalperiod_d.Text = lblperiod.Text
            ' lblroi_d.Text = String.Format("{0:N}", Session("dint"))
            ' lblpenalroi_d.Text = String.Format("{0:N}", Session("penal_dint"))
            Session("totalint_d") = intc_amt 'Math.Round((Session("dbal") * Session("dint") / 100) / 12) * Session("prd_buffer")
            Session("totint_penal_d") = intd_amt 'Math.Round((Session("dbal") * Session("penal_dint") / 100) / 12) * Session("prd_buffer")
            'lblpenalint_d.Text = String.Format("{0:N}", Session("totint_penal_d"))
            'lblactualint_d.Text = String.Format("{0:N}", Session("totalint_d"))
            'lblintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))
            'lblpenalintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))
            Session("2bpaid_d") = String.Format("{0:N}", (Session("totalint_d") - Session("intpaid_d")))
            'lblpenal2bpaid_d.Text = String.Format("{0:N}", (Session("totint_penal_d") - Session("intpaid_d")))
            Session("penal_d") = String.Format("{0:N}", (Session("totalint_d") - Session("totint_penal_d")))


            Session("pre_amt") = String.Format("{0:N}", Session("dbal") - CDbl(Session("penal_d")) + CDbl(Session("2bpaid_d")))


        Else

            lblperiod_d.Text = 0
            lblperiod.Text = 0
            lblpenalperiod_d.Text = 0
            lblpenalperiod.Text = 0
            lblroi_d.Text = String.Format("{0:N}", 0)
            lblroi.Text = String.Format("{0:N}", 0)
            lblpenalroi_d.Text = String.Format("{0:N}", 0)
            lblpenalroi.Text = String.Format("{0:N}", 0)
            Session("totalint_d") = Math.Round((Session("dbal") * Session("dint") / 100) / 12) * Session("prd_buffer")
            Session("totint_penal_d") = Math.Round((Session("dbal") * Session("penal_dint") / 100) / 12) * Session("prd_buffer")

            lblpenalint_d.Text = String.Format("{0:N}", 0)
            lblpenalint.Text = String.Format("{0:N}", 0)
            lblactualint_d.Text = String.Format("{0:N}", 0)
            lblactualint.Text = String.Format("{0:N}", 0)
            lblintpaid_d.Text = String.Format("{0:N}", 0)
            lblintpaid.Text = String.Format("{0:N}", 0)
            lblpenalintpaid_d.Text = String.Format("{0:N}", 0)
            lblpenalintpaid.Text = String.Format("{0:N}", 0)
            lbl2bpaid_d.Text = String.Format("{0:N}", 0)
            lbl2bpaid.Text = String.Format("{0:N}", 0)
            lblpenal2bpaid_d.Text = String.Format("{0:N}", 0)
            lblpenal2bpaid.Text = String.Format("{0:N}", 0)
            lblpenal_d.Text = String.Format("{0:N}", 0)
            lblpenal.Text = String.Format("{0:N}", 0)

            Session("pre_amt") = String.Format("{0:N}", Session("dbal"))


        End If

        update_cld(acn)

        If Not CDbl(Session("2bpaid_d")) = 0 Then
            Session("cbal") = Session("cbal") + CDbl(Session("2bpaid_d"))
            Session("dbal") = Session("dbal") + CDbl(Session("2bpaid_d"))
            update_int_C(Session("tid"), 0, CDbl(Session("2bpaid_d")), 0, CDbl(Session("2bpaid_d")), "BY INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, CDbl(Session("2bpaid_d")), 0, "BY INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", 0, CDbl(Session("2bpaid_d")), "TO INTEREST", "INTR")

        End If

        If Not CDbl(Session("penal_d")) = 0 Then

            update_int_C(Session("tid"), CDbl(Session("penal_d")), 0, CDbl(Session("penal_d")), 0, "TO INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("penal_d")), "TO INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", CDbl(Session("penal_d")), 0, "BY INTEREST", "INTR")

        End If








        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con



        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@drd", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crc", 0)

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "To Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()



        End Try


        update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("pre_amt")), "TO CASH", "PAYMENT")







    End Sub

    Private Sub update_suplementry_C(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)



        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        If mop.SelectedItem.Text = "Cash" Then
            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", Session("acn_c"))
            cmd.Parameters.AddWithValue("@nar", nar)
            cmd.Parameters.AddWithValue("@typ", typ)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try
        Else
            cmd.Parameters.Clear()

            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", 0)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@acn", Session("acn_c"))
            Else
                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
            End If

            cmd.Parameters.AddWithValue("@nar", "TO Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            cmd.Parameters.Clear()


            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@achead", bnk.SelectedItem.Text)
                cmd.Parameters.AddWithValue("@acn", Session("acn_c"))
            Else
                cmd.Parameters.AddWithValue("@achead", "SAVINGS DEPOSIT")
                cmd.Parameters.AddWithValue("@acn", Session("acn_c"))
            End If
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", dr)

            cmd.Parameters.AddWithValue("@nar", "By Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try




        End If
        query = ""




    End Sub

    Private Sub update_int_C(ByVal tid As Double, ByVal drd As Double, ByVal crd As Double, ByVal drc As Double, ByVal crc As Double, ByVal nar As String, ByVal supliment As String, ByVal cbal As Double, ByVal dbal As Double)


        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con




        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"

        cmd.Parameters.Clear()

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@uiid", tid)
        cmd.Parameters.AddWithValue("@uidate", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@uiacno", Session("acn_c"))
        cmd.Parameters.AddWithValue("@uidrd", drd)
        cmd.Parameters.AddWithValue("@uicrd", crd)
        cmd.Parameters.AddWithValue("@uidrc", drc)
        cmd.Parameters.AddWithValue("@uicrc", crc)
        cmd.Parameters.AddWithValue("@uinarration", nar)
        cmd.Parameters.AddWithValue("@uidue", " ")
        cmd.Parameters.AddWithValue("@uitype", "INTR")
        cmd.Parameters.AddWithValue("@uisuplimentry", supliment)
        cmd.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@uicbal", cbal)
        cmd.Parameters.AddWithValue("@uidbal", dbal)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()
        End Try


    End Sub

    Sub rid_closure_c(ByVal acn As String)
        Session("acn_c") = acn

        Dim prod = Trim(get_pro(Session("product")))
        Dim fq As Integer = 0
        Dim prdx As Double = 0
        Dim curint As Double = 0
        Dim curint_d As Double = 0
        Dim curint_penal As Double = 0
        Dim curint_penal_d As Double = 0
        Dim cumint As Double = 0
        Dim cumint_d As Double = 0
        Dim cumint_penal As Double = 0
        Dim cumint_penal_d As Double = 0

        '  Session("ac_date") = CDate(reader(1).ToString)
        '  Session("amount") = reader(2).ToString
        '  Session("mdate") = CDate(reader(3).ToString)
        '  Session("cintr") = CDbl(reader(4).ToString)

        Dim pamt As Double = Session("amount")
        Dim pamt_d As Double = Session("amount")
        Dim pamt_penal As Double = Session("amount")
        Dim pamt_penal_d As Double = Session("amount")

        Dim das As Decimal = 0.0

        Session("prd") = DateDiff(DateInterval.Month, Session("ac_date"), CDate(tdate.Text))
        Session("prdtyp") = "M"

        Session("cintr") = get_dint()


        Session("prv_inton") = get_prev_inton_c(acn)
        ' penalcut = get_penalcut()

        Dim x = CDate(tdate.Text).CompareTo(Session("mdate"))

        If Not x = -1 Then

            Session("penal_cintr") = Session("cintr")
            Session("penal_dint") = Session("cintr")
            Session("prd_buffer") = DateDiff(DateInterval.Month, Session("prv_inton"), Session("mdate"))

            '            Session("prd_buffer") = Session("prd_buffer") / 30
            prdx = Session("prd_buffer") Mod 3

            fq = Math.Round(Session("prd_buffer") - prdx)

            ' pamt = CDbl(lblbal.Text)

            If Not fq = 0 Then

                For i = 1 To fq
                    curint = (pamt * Session("cintr") / 100) / 12
                    curint_penal = (pamt_penal * Session("penal_cintr") / 100) / 12
                    curint_d = (pamt_d * Session("cintr") / 100) / 12
                    curint_penal_d = (pamt_penal_d * Session("penal_cintr") / 100) / 12

                    cumint = cumint + (curint * 3)
                    cumint_d = cumint_d + (curint_d * 3)
                    cumint_penal = cumint_penal + (curint_penal * 3)
                    cumint_penal_d = cumint_penal_d + (curint_penal_d * 3)

                    pamt = pamt + (curint * 3)
                    pamt_d = pamt_d + (curint_d * 3)
                    pamt_penal = pamt_penal + (curint_penal * 3)
                    pamt_penal_d = pamt_penal_d + (curint_penal_d * 3)

                Next
            End If




            lblroi.Text = String.Format("{0:N}", Session("cintr"))
            lblpenalroi.Text = String.Format("{0:N}", Session("penal_cintr"))
            lblroi_d.Text = String.Format("{0:N}", Session("cintr"))
            lblpenalroi_d.Text = String.Format("{0:N}", Session("penal_cintr"))

            lblactualint.Text = String.Format("{0:N}", cumint)
            lblactualint_d.Text = String.Format("{0:N}", cumint_d)
            Session("penel") = 0

            Session("pre_amt") = String.Format("{0:N}", Session("mamt"))
            Session("2bpaid") = String.Format("{0:N}", Session("mamt") - CDbl(lblbal.Text))
            Session("2bpaid_d") = String.Format("{0:N}", Session("mamt") - CDbl(lblbal.Text))

            '

        Else

            Dim remprd As Double = 0

            Session("penal_cintr") = Session("cintr") - 1
            Session("penal_dint") = Session("dint") - 1
            Session("prd_buffer") = DateDiff(DateInterval.Day, Session("ac_date"), Session("voucher_date"))

            lblperiod.Text = CStr(Session("prd_buffer")) + " Days"
            lblperiod_d.Text = CStr(Session("prd_buffer")) + " Days"

            Session("intpaid") = get_intpaid_c(acn)
            Session("intpaid_d") = get_IntpaidD_c(acn)


            ''---------soe
            If (Session("prd_buffer") > 30) Then
                prdx = Session("prd_buffer") / 30
                remprd = prdx Mod 3


                If Not prdx = remprd Then
                    fq = Format((prdx - remprd) / 3, "#")
                Else
                    fq = 0
                End If
                ' prdx = Session("prd_buffer") Mod 3
                das = Format(prdx - (fq * 3), "0.00")
            Else
                If Session("prd_buffer") <= 15 Then
                    das = 0
                    fq = 0
                Else
                    fq = 0
                    das = Format(Session("prd_buffer") / 30, "0.00")


                End If
            End If


            '  fq = Math.Round(Session("prd_buffer") - Session("prdx"))
            '  Session("amt") = CDbl(lblamt.Text)
            ' pamt = Session("amount")

            For i = 1 To fq
                curint = (pamt * Session("cintr") / 100) / 12
                curint_penal = (pamt_penal * Session("penal_cintr") / 100) / 12
                curint_d = (pamt_d * Session("cintr") / 100) / 12
                curint_penal_d = (pamt_penal_d * Session("penal_dint") / 100) / 12

                cumint = cumint + (curint * 3)
                cumint_d = cumint_d + (curint_d * 3)
                cumint_penal = cumint_penal + (curint_penal * 3)
                cumint_penal_d = cumint_penal_d + (curint_penal_d * 3)

                pamt = pamt + (curint * 3)
                pamt_d = pamt_d + (curint_d * 3)
                pamt_penal = pamt_penal + (curint_penal * 3)
                pamt_penal_d = pamt_penal_d + (curint_penal_d * 3)

            Next
            pamt = Math.Round(pamt)
            pamt_d = Math.Round(pamt_d)
            pamt_penal = Math.Round(pamt_penal)
            pamt_penal_d = Math.Round(pamt_penal_d)

            Dim int4day As Double = (pamt * Session("cintr") / 100) / 365
            int4day = int4day * (das * 30)
            cumint = Math.Round(cumint + int4day)
            cumint_d = Math.Round(cumint_d + (((pamt_d * Session("cintr") / 100) / 365) * (das * 30)))
            cumint_penal = Math.Round(cumint_penal + (((pamt_penal * Session("penal_cintr") / 100) / 365) * (das * 30)))
            cumint_penal_d = Math.Round(cumint_penal_d + (((pamt_penal_d * Session("penal_dint") / 100) / 365) * (das * 30)))

            ' lblroi.Text = String.Format("{0:N}", Session("cintr"))
            ' lblpenalroi.Text = String.Format("{0:N}", Session("penal_cintr"))
            ' lblroi_d.Text = String.Format("{0:N}", Session("cintr"))
            ' lblpenalroi_d.Text = String.Format("{0:N}", Session("penal_cintr"))

            'lblactualint.Text = String.Format("{0:N}", cumint)
            'lblactualint_d.Text = String.Format("{0:N}", cumint_d)

            'lblpenalint.Text = String.Format("{0:N}", cumint_penal)
            'lblpenalint_d.Text = String.Format("{0:N}", cumint_penal)

            'lblintpaid.Text = String.Format("{0:N}", Session("intpaid_d"))
            'lblpenalintpaid.Text = String.Format("{0:N}", Session("intpaid_d"))

            'lblintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))
            'lblpenalintpaid_d.Text = String.Format("{0:N}", Session("intpaid_d"))

            Session("2bpaid") = String.Format("{0:N}", (cumint - Session("intpaid")))
            'lbl2bpaid_d.Text = String.Format("{0:N}", (cumint_d - Session("intpaid_d")))

            'lblpenal2bpaid.Text = String.Format("{0:N}", (cumint_penal - Session("intpaid_d")))
            'lblpenal2bpaid_d.Text = String.Format("{0:N}", (cumint_penal - Session("intpaid_d")))

            Session("penal") = String.Format("{0:N}", (cumint - cumint_penal))
            'lblpenal_d.Text = String.Format("{0:N}", (cumint_d - cumint_penal))

            Session("pre_amt") = CDbl(Session("amount")) + (cumint_penal - Session("intpaid_d")) + Session("intpaid_d")









        End If

        If Not CDbl(Session("2bpaid")) = 0 Then
            Session("cbal") = Session("cbal") + CDbl(Session("2bpaid_d"))
            Session("dbal") = Session("dbal") + CDbl(Session("2bpaid_d"))
            update_int_C(Session("tid"), 0, CDbl(Session("2bpaid")), 0, CDbl(Session("2bpaid")), "BY INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, CDbl(Session("2bpaid")), 0, "BY INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", 0, CDbl(Session("2bpaid")), "TO INTEREST", "INTR")

        End If

        If Not CDbl(Session("penal")) = 0 Then

            update_int_C(Session("tid"), CDbl(Session("penal")), 0, CDbl(Session("penal")), 0, "TO INTEREST", Trim(Session("product")) + " INTEREST", 0, 0)
            update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("penal")), "TO INTEREST", "INTR")
            update_suplementry_C(Session("tid"), Trim(Session("product")) + " INTEREST", CDbl(Session("penal")), 0, "BY INTEREST", "INTR")

        End If


        update_cld(acn)





        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con



        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@drd", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(Session("pre_amt")))
        cmd.Parameters.AddWithValue("@crc", 0)

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "TO CASH")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To TRANSFER")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()



        End Try


        update_suplementry_C(Session("tid"), prod, 0, CDbl(Session("pre_amt")), "TO CASH", "PAYMENT")





    End Sub

    Private Sub txt_sb_TextChanged(sender As Object, e As EventArgs) Handles txt_sb.TextChanged

        Dim bal As Double = get_balance(Trim(txt_sb.Text))

        lbl_sb_bal.Text = FormatCurrency(bal)

        If bal < 0 Then
            lbl_sb_bal.CssClass = "col-sm-2 text-danger"
        Else
            lbl_sb_bal.CssClass = "col-sm-2 text-success"
        End If

    End Sub

    Private Sub lbl2bpaid_d_TextChanged(sender As Object, e As EventArgs) Handles lbl2bpaid_d.TextChanged
        If Not lbl2bpaid_d.Text = "" Then

            get_balance(Trim(txtacn.Text))
            lbl2bpaid_d.Text = FormatNumber(lbl2bpaid_d.Text)
            'lbl2bpaid_d.Text = lbl2bpaid.Text
            txtamt.Text = String.Format("{0:n}", (Session("dbal") + CDbl(lbl2bpaid_d.Text) - CDbl(lblpenal_d.Text)))

        End If
        txtamt.Focus()

    End Sub

    Private Sub lblpenal_d_TextChanged(sender As Object, e As EventArgs) Handles lblpenal_d.TextChanged
        If Not lblpenal.Text = "" Then
            get_balance(Trim(txtacn.Text))
            lblpenal.Text = FormatCurrency(lblpenal.Text)
            lblpenal_d.Text = lblpenal.Text
            txtamt.Text = String.Format("{0:n}", (Session("dbal") + CDbl(lbl2bpaid_d.Text) - CDbl(lblpenal_d.Text)))
        End If

        txtamt.Focus()

    End Sub

    Private Sub btntog_Click(sender As Object, e As EventArgs) Handles btntog.Click
        clear_tab_recpt()
        pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
        pnltrans.Style.Add(HtmlTextWriterStyle.Display, "block")
        txtacn.Focus()

    End Sub
End Class