Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization


Public Class DepositReceipt

    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim yearpart As String
    Dim sb_srch As Boolean

    Public Class ACMas

        Public dt As Date
        Public acno As String
        Public cid As String
        Public serial As Integer
        Public prod As String
        Public amt As Decimal
        Public cintr As Decimal
        Public dintr As Decimal
        Public cld As Integer
        Public prd As Integer
        Public prdtyp As String
        Public mdate As Date
        Public mamt As Decimal
        Public isren As Integer
        Public renacn As String
        Public parent As String




    End Class

    Public Class Actrans

        Public id As Integer
        Public dt As Date
        Public acno As String
        Public drd As Decimal
        Public crd As Decimal
        Public drc As Decimal
        Public crc As Decimal
        Public narration As String
        Public due As String
        Public typ As String
        Public suplimentery As String
        Public sesusr As String
        Public entryat As DateTime


    End Class


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then


            If Session("orgin") = "loananalysis" Then

                txtacn.Text = Session("auction_sb")
                tdate.Text = Date.Today
                get_ac_info(txtacn.Text)
                txtamt.Text = (Session("auction_diff"))
                txtnar.Text = "BY JL AUCTION A/C No " + Session("auctionacn").ToString
            Else
                If Not Session("acn") Is Nothing Then
                    txtacn.Text = Trim(CType(Session("acn"), String))

                    '  TabContainer1.Visible = True
                    ' txt_acn_srch.Text = ""
                    'listgid1.Items.Clear()
                    tdate.Text = Date.Today
                    ' closure_notice.Visible = False
                    get_ac_info(txtacn.Text)
                    '  txtfocus(tdate)



                    'bind_grid()
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtacn.ClientID), True)
                    ' sb_srch = False
                End If



            End If
        End If

    End Sub


    Public Function get_balance(ByVal acn As String)

        Dim x As Integer = 1
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno =@acno and scroll =@scroll GROUP BY [actrans].acno"
                cmd.Parameters.AddWithValue("@acno", acn)
                cmd.Parameters.AddWithValue("@scroll", x)
                cmd.CommandText = sql
                cmd.Connection = con

                Using reader1 As SqlDataReader = cmd.ExecuteReader
                    If reader1.HasRows Then
                        reader1.Read()
                        Session("dbal") = reader1(1).ToString - reader1(0).ToString
                        Session("cbal") = reader1(3).ToString - reader1(2).ToString



                    Else
                        Session("cbal") = 0
                        Session("dbal") = 0
                    End If



                End Using

            End Using
            con.Close()

        End Using






        Return Session("dbal")
    End Function
    Public Sub get_ac_info(ByVal acn As String)

        rdinfo.Visible = False

        Dim sql As String = "SELECT Master.date,Master.acno,Master.product,Master.amount,Master.cint,Master.dint,Master.prd,Master.prdtype,Master.mdate,Master.mamt,Master.cid,Master.isrenew FROM dbo.master WHERE master.acno = '" + acn + "' and cld = 0 "

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
            Session("renew") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(11)), 0, ds.Tables(0).Rows(0).Item(11))



            'ds.Tables(0).Rows(0).Item(9)
            Session("ncid") = ds.Tables(0).Rows(0).Item(10)

            txtacn.Enabled = False
            soa.Enabled = True
        Else


            txtacn.Enabled = True
            txtfocus(txtacn)

            Exit Sub

        End If

        sql = "SELECT FirstName,lastname,address,mobile,dob from dbo.member where MemberNo='" + Session("ncid") + "'"

        Dim adapter1 As New SqlDataAdapter(sql, con)

        adapter1.Fill(ds1)

        If Not ds1.Tables(0).Rows.Count = 0 Then
            Session("ac_name") = ds1.Tables(0).Rows(0).Item(0)
            Session("ac_lname") = ds1.Tables(0).Rows(0).Item(1)
            Session("address") = ds1.Tables(0).Rows(0).Item(2)
            Session("mobile") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(3)), "", ds1.Tables(0).Rows(0).Item(3))
            Session("dob") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(4)), "", ds1.Tables(0).Rows(0).Item(4))
            'custpict.ImageUrl = "~/ShowImage.ashx?id=" & Session("ncid")

        End If

        adapter1.Dispose()





        lblbal.Text = FormatCurrency(get_balance(acn))
        lblproduct.Text = Trim(Session("product"))
        lblname.Text = Session("ac_name")
        lbladd.Text = Session("address")
        lblmobile.Text = Session("mobile")
        get_Img(Session("ncid").ToString())
        pnlact.Visible = True
        lblamt.Text = FormatCurrency(Session("amt"))

        adapter.Dispose()


        '  bind_grid()

        Select Case Trim(Session("product"))
            Case "FD"
                If Not CDbl(lblamt.Text) = 0 Then
                    btn_up_rcpt.Visible = False
                End If
            Case "DS"
                If CDbl(lblbal.Text) = 0 Then
                    If CDbl(Session("amt")) = 0 Then
                        btn_up_rcpt.Visible = True
                    Else
                        btn_up_rcpt.Visible = False
                    End If
                Else
                    Dim X As Integer = CDbl(lblbal.Text) / CDbl(Session("amt"))
                    If Not X >= 300 Then
                        btn_up_rcpt.Visible = True
                    Else
                        btn_up_rcpt.Visible = False
                    End If
                End If
                GetTotalCreditInADay(acn)
            Case "RID"
                If Not CDbl(lblamt.Text) = 0 Then
                    btn_up_rcpt.Visible = False
                End If

            Case "KMK"
            Case "SB"
                'If Not CDbl(lblamt.Text) = 0 Then
                btn_up_rcpt.Visible = True
                    'End If
            Case "RD"
                rddue.Visible = True
                'nod.Visible = True
                txtnod.Text = 1
                rdinfo.Visible = True
                txtamt.Text = CDbl(Session("amt"))
                get_last_transaction(acn)
                calculate_penalty()


        End Select


        If String.Equals(Trim(Session("product")), "RD") Then

        End If

        ' ClientScript.RegisterStartupScript(Me.[GetType](), "SetFocus", "<script language=""Jscript"" > document.getElementById(""txtamt"").focus(); </Script>", True)
        '  ClientScript.RegisterStartupScript(Me.[GetType](), "Focus", "document.getElementById('<%=txtamt.ClientID()%>').focus();", True)
        ' ScriptManager1.SetFocus(txtamt)


        ' txtfocus(tdate)
        '
        'ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", tdate.ClientID), True)

        If Not IsNothing(Session.Item("diff_to_sb")) Then
            txtamt.Text = Session.Item("diff_to_sb")
        End If
    End Sub

    Private Sub get_Img(ByVal memberno As String)
        Dim imgbytes As Byte() = Nothing

        Using con2 As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con2.Open()

            Dim query As String = "select photo from kyc where kyc.memberno=@memberno"
            Using cmd2 As New SqlClient.SqlCommand(query, con2)
                cmd2.Parameters.AddWithValue("@memberno", Trim(memberno))

                Try
                    Dim dr As SqlDataReader = cmd2.ExecuteReader()

                    If dr.HasRows() Then
                        dr.Read()

                        If Not IsDBNull(dr(0)) Then
                            imgbytes = CType(dr.GetValue(0), Byte())

                            Dim imagePath As String = String.Format("~/Captures/{0}.png", Trim(memberno))
                            File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                            imgCapture.ImageUrl = "~/captures/" + Trim(memberno) + ".png?" + DateTime.Now.Ticks.ToString()
                            imgCapture.Visible = True
                        Else
                            imgCapture.Visible = False
                        End If
                    Else
                        imgCapture.Visible = False
                    End If
                    dr.Close()
                Catch ex As Exception
                    imgCapture.Visible = False
                Finally
                    cmd2.Dispose()
                    con2.Close()
                End Try
            End Using
        End Using
    End Sub



    Public Sub OpenNewDeposit(ByVal master As ACMas)



        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "INSERT INTO masterc(date,acno,cid,serial,product,amount,cint,dint,cld,prd,prdtype,mdate,mamt,isrenew,renewacn,parent)"
                query &= "VALUES(@date,@acno,@cid,@serial,@product,@amount,@cint,@dint,@cld,@prd,@prdtype,@mdate,@mamt,@isrenew,@renewacn,@parent)"

                cmd.CommandText = query
                cmd.Parameters.Clear()

                cmd.Parameters.AddWithValue("@date", master.dt)
                cmd.Parameters.AddWithValue("@acno", master.acno)
                cmd.Parameters.AddWithValue("@cid", master.cid)
                cmd.Parameters.AddWithValue("@serial", master.serial)
                cmd.Parameters.AddWithValue("@product", master.prod)
                cmd.Parameters.AddWithValue("@amount", master.amt)
                cmd.Parameters.AddWithValue("@cint", master.cintr)
                cmd.Parameters.AddWithValue("@dint", master.dintr)
                cmd.Parameters.AddWithValue("@cld", master.cld)
                cmd.Parameters.AddWithValue("@prd", master.prd)
                cmd.Parameters.AddWithValue("@prdtype", master.prdtyp)
                cmd.Parameters.AddWithValue("@mdate", master.mdate)
                cmd.Parameters.AddWithValue("@mamt", master.mamt)
                cmd.Parameters.AddWithValue("@isrenew", master.isren)
                cmd.Parameters.AddWithValue("@renewacn", master.renacn)
                cmd.Parameters.AddWithValue("@parent", master.parent)
                Try

                    cmd.ExecuteNonQuery()




                Catch ex As Exception

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
                Finally
                    con.Close()
                    cmd.Dispose()

                End Try

            End Using
            con.Close()

        End Using



    End Sub


    Sub get_last_transaction(ByVal acn As String)
        Dim oResult As Date

        If con.State = ConnectionState.Closed Then con.Open()


        cmdx.Connection = con
        cmdx.CommandText = "SELECT top 1 date  FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "'ORDER BY date DESC"

        oResult = cmdx.ExecuteScalar()

        If Not oResult = Date.MinValue Then
            Dim tdat As Date = Convert.ToDateTime(tdate.Text)
            Session("days_ago") = DateDiff(DateInterval.Day, oResult, tdat)
        Else
            Session("days_ago") = 1
        End If


    End Sub

    Sub GetTotalCreditInADay(ByVal acn As String)

        Session("totalCreditInADay") = 0

        If con.State = ConnectionState.Closed Then con.Open()
        Dim tdat As String = Convert.ToDateTime(tdate.Text).ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        cmdx.Connection = con
        cmdx.CommandText = "select SUM([actrans].Crc) as totalCred from dbo.actrans where acno = '" + acn + "' and CONVERT(VARCHAR(20), date, 112) = '" + tdat + "'"

        Dim result = cmdx.ExecuteScalar()
        If Not IsDBNull(result) Then
            Session("totalCreditInADay") = result
        End If

    End Sub

    Function get_collection(ByVal acn As String)

        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].type<>'INTR' "

        cmdi.Connection = con
        cmdi.CommandText = sql

        countresult = cmdi.ExecuteScalar()



        Return countresult


    End Function



    Function get_due(ByVal acn As String, ByVal curdue As Integer)

        Dim df As String = "dMMM-yyyy"
        Dim op As String
        Dim opt As DateTime

        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].type<>'INTR' "

        cmdi.Connection = con
        cmdi.CommandText = sql

        countresult = cmdi.ExecuteScalar()

        If countresult = 0 Then

            opt = Convert.ToDateTime(Session("ac_date"))
            op = opt.ToString("y")

        Else

            Dim curdue_period As Date = DateAdd(DateInterval.Month, (countresult), Session("ac_date"))

            op = curdue_period.ToString("y")


        End If

        Return op
    End Function

    Sub calculate_penalty()


        'Dim sql As String = "SELECT roi.penalty,roi.penaltyprd FROM dbo.roi WHERE roi.Product = '" + product + "'"
        'sql &= "AND roi.prddmy = '" + prdtyp + "'"
        'sql &= "AND roi.prdfrm <= " + prd
        'sql &= "AND roi.prdto >= " + prd


        'Dim penalty_ds As New DataSet

        'Dim penalty_adapter As New SqlDataAdapter(sql, con)


        'penalty_adapter.Fill(penalty_ds)

        'If Not penalty_ds.Tables(0).Rows.Count = 0 Then

        '    If days_ago >= CInt(IIf(IsDBNull(penalty_ds.Tables(0).Rows(0).Item(1)), 0, penalty_ds.Tables(0).Rows(0).Item(1))) Then

        '        Dim perhundred As Integer = IIf(IsDBNull(penalty_ds.Tables(0).Rows(0).Item(0)), 0, penalty_ds.Tables(0).Rows(0).Item(0))
        '        Dim penalprd As Integer = IIf(IsDBNull(penalty_ds.Tables(0).Rows(0).Item(1)), 0, penalty_ds.Tables(0).Rows(0).Item(1))

        '        txtpenalty.Text = ((amt * perhundred) / 100) * (Math.Round(days_ago / 30) - 1)
        '        rdpenal.Visible = True
        '    End If


        'End If

        rd_due.Text = Session("prd")


        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + txtacn.Text + "' AND [actrans].type<>'INTR' "

        cmdi.Connection = con
        cmdi.CommandText = sql

        countresult = cmdi.ExecuteScalar()

        countresult = IIf(IsDBNull(countresult), 0, countresult)

        rd_duepaid.Text = countresult
        rd_duebalance.Text = Session("prd") - countresult

        If rd_duebalance.Text <= 0 Then btn_up_rcpt.Enabled = False

        '   If rd_duebalance.Text >= 0 Then txtamt.Enabled = False

        Dim rdlate As Integer = DateDiff(DateInterval.Month, Session("ac_date"), Convert.ToDateTime(tdate.Text)) - CDbl(countresult)
        If rdlate <= rd_duebalance.Text Then
            rdlateby.Text = rdlate
        Else
            rdlateby.Text = rd_duebalance.Text
        End If

        ' If rdlate > Session("prd") - countresult Then rdlate = rd_duebalance.Text
        Dim pamt As Double = 0

        If rdlate > 0 Then
            lblcalc.Text = CStr(rdlate) + " Months @ 2%"

            'lblpen.Visible = True
            rdpenal.Visible = True
            txtpenalty.Visible = True
            lblhed.Text = "late by"
            Dim nod As Integer = CInt(txtnod.Text)
            If nod >= rdlate Then nod = rdlate

            For i As Integer = 1 To nod
                pamt = pamt + (Session("amt") / 100 * 2) * ((rdlate + 1) - i)
            Next

            txtpenalty.Text = pamt
            txtamt.Text = CDbl(txtamt.Text) + pamt
        Else

            Dim year = Convert.ToDateTime(tdate.Text).Year
            Dim mnt = Convert.ToDateTime(tdate.Text).Month

            Dim days_remain As Integer = DateTime.DaysInMonth(year, mnt)

            Dim rdlatecurrent As Integer = DateDiff(DateInterval.Month, Session("ac_date"), Convert.ToDateTime(tdate.Text).AddDays(days_remain)) - CDbl(countresult)

            lblhed.Text = "adv by"
            rdlateby.Text = (rdlatecurrent)
            txtpenalty.Text = 0

        End If

    End Sub

    Private Sub txtnod_TextChanged(sender As Object, e As EventArgs) Handles txtnod.TextChanged


        If txtnod.Text <= CDbl(rd_duebalance.Text) Then

            calculate_penalty()
            'txtfocus(txtamt)
            txtamt.Text = CDbl(txtnod.Text) * Session("amt") + CDbl(txtpenalty.Text)

            System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtamt)

            'denom_in.Visible = True
            'txtfocus(txt1k)
        Else
            txtnod.Text = CDbl(rd_duebalance.Text)
            calculate_penalty()
            txtamt.Text = CDbl(txtnod.Text) * Session("amt") + CDbl(txtpenalty.Text)
            ' txtfocus(txtnod)
        End If
    End Sub

    Private Sub txtacn_TextChanged(sender As Object, e As EventArgs) Handles txtacn.TextChanged

        'Session("acn") = txtacn.Text

        ' clear_tab_recpt()
        'txtacn.Text = CType(Session("acn"), String)
        ' TabContainer1.Visible = True
        ' txt_acn_srch.Text = ""
        'listgid1.Items.Clear()
        If Not Trim(txtacn.Text) = "" Then
            tdate.Text = Date.Today
            'alertmsg.Visible = False

            'closure_notice.Visible = False
            get_ac_info(txtacn.Text)

            'System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.tdate)

            If Trim(lblproduct.Text) = "RD" Then
                txtfocus(txtnod)

            Else
                txtfocus(txtamt)
            End If

        End If


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
            con.Close()

        End Using



        Return prdname
    End Function

    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, Optional ByVal notes As String = "")
        If tid = 0 Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        cmd.Connection = con

        If mop.SelectedItem.Text = "Cash" Then

            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type,notes)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ,@notes)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            cmd.Parameters.AddWithValue("@nar", "BY CASH")
            cmd.Parameters.AddWithValue("@typ", "RECEIPT")
            cmd.Parameters.AddWithValue("@notes", notes)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)


            End Try

        Else

            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type,notes)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ,@notes)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@notes", notes)

            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                cmd.Parameters.AddWithValue("@nar", "By Transfer " + bnk.SelectedItem.Text)
            Else
                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
                ' cmd.Parameters.AddWithValue("@nar", "By Transfer " + Trim(txtacn.Text))
                cmd.Parameters.AddWithValue("@nar", "By Transfer " + Trim(txtacn.Text) + " (" + Trim(txt_sb.Text) + ")")

            End If

            cmd.Parameters.AddWithValue("@typ", "JOURNAL")

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
            End Try

            cmd.Parameters.Clear()


            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type,notes)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ,@notes)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@notes", notes)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@achead", bnk.SelectedItem.Text)
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                cmd.Parameters.AddWithValue("@nar", "To Transfer " + bnk.SelectedItem.Text)
            Else
                If Left(Trim(txt_sb.Text), 2) = "79" Then
                    cmd.Parameters.AddWithValue("@achead", "SAVINGS DEPOSIT")
                Else
                    cmd.Parameters.AddWithValue("@achead", "KMK DEPOSIT")
                End If

                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                cmd.Parameters.AddWithValue("@nar", "To Transfer " + Trim(txt_sb.Text) + " (" + Trim(txtacn.Text) + ")")
            End If
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)


            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
            End Try

        End If

        query = ""



    End Sub

    Sub UPDATE_MAT_VAL_c()
        Dim PRIN As Integer
        Dim MN As Integer
        Dim MNT As Integer
        Dim PRAMT As Integer
        Dim PR As Integer
        PRIN = Session("splitamt")
        MN = Session("prd")

        If Trim(Session("product")) = "RD" Then

            PRAMT = PRIN
            For j = 1 To MN
                MNT = PRAMT * Session("cintr") / 100
                MNT = MNT / 12
                PRAMT = PRAMT + MNT
                PR = PRAMT + PRIN
                PRAMT = Format(PR, "#")
                PRAMT = Val(PR)
            Next
            PRAMT = PRAMT - PRIN
            Session("MAT") = Math.Round(PRAMT)

        ElseIf Trim(Session("product")) = "RID" Then
            PRIN = txtamt.Text
            MN = Session("prd")
            MN = MN / 3
            PRAMT = PRIN
            For j = 1 To MN
                MNT = PRAMT * Session("cintr") / 100
                MNT = MNT / 12
                PRAMT = PRAMT + (MNT * 3)
            Next
            ' lblmat.Text = FormatCurrency(Math.Round(PRAMT))
            Session("MAT") = Math.Round(PRAMT)
        End If

    End Sub

    Sub UPDATE_MAT_VAL()
        Dim PRIN As Integer
        Dim MN As Integer
        Dim MNT As Integer
        Dim PRAMT As Integer
        Dim PR As Integer
        PRIN = txtamt.Text
        MN = Session("prd")

        If Trim(Session("product")) = "RD" Then

            PRAMT = PRIN
            For j = 1 To MN
                MNT = PRAMT * Session("roid") / 100
                MNT = MNT / 12
                PRAMT = PRAMT + MNT
                PR = PRAMT + PRIN
                PRAMT = Format(PR, "#")
                PRAMT = Val(PR)
            Next
            PRAMT = PRAMT - PRIN
            Session("mamt") = Math.Round(PRAMT)
        ElseIf Trim(Session("product")) = "RID" Then
            PRIN = txtamt.Text
            MN = Session("prd")
            MN = MN / 3
            PRAMT = PRIN
            For j = 1 To MN
                MNT = PRAMT * Session("roid") / 100
                MNT = MNT / 12
                PRAMT = PRAMT + (MNT * 3)
            Next
            ' lblmat.Text = FormatCurrency(Math.Round(PRAMT))
            Session("mamt") = Math.Round(PRAMT)
        End If

    End Sub
    Private Sub set_changes()
        Dim query As String = String.Empty


        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            'log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("login.aspx")

        End If
        If session_user_role = "Audit" Then Exit Sub



        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con
        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"
        Dim prod = get_pro(Session("product"))
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", CDbl(txtamt.Text))


        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "By Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnar.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "By Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnar.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "By Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text) + " " + txt_sb.Text)
        End Select



        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)

        Finally

            cmd.Dispose()
            con.Close()


        End Try





        query = ""


        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.Parameters.Clear()
        query = "SELECT MAX(ID) from dbo.trans where trans.acno='" + txtacn.Text + "'"
        cmd.CommandText = query


        Try
            countresult = cmd.ExecuteScalar()

            Session("tid") = Convert.ToString(countresult)

        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
        End Try


        If Trim(Session("product")) = "RD" Then GoTo nxt
        If Trim(Session("product")) = "DS" Then GoTo nxt



        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        '' Dim prod = get_pro(Session("product"))

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", CDbl(txtamt.Text))


        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "By Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnar.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "By Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnar.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "By Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text) + " " + txt_sb.Text)
        End Select

        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)

        Finally

            cmd.Dispose()
            con.Close()


        End Try


nxt:




        If CDbl(lblamt.Text) = 0 Then



            Select Case Trim(lblproduct.Text)
                Case "DS"
                    Session("amt") = txtamt.Text
                    Session("roic") = Session("cintr")
                    Session("roid") = Session("dint")
                    Session("mamt") = 0

                Case "FD"

                    ' Session("rebate") = drx(0)
                    'Session("src") = drx(1)
                    'Session("renewal") = drx(2)
                    'Session("spl") = drx(3)
                    'Session("istrf") = drx(4)
                    'Session("isrenewal") = drx(5)

                    Session("amt") = CDbl(txtamt.Text)
                    Session("roic") = CDbl(Session("cintr"))
                    Session("roid") = CDbl(Session("dint"))

                    Session("mamt") = txtamt.Text

                Case "KMK"
                    Session("amt") = txtamt.Text
                    Session("roic") = Session("cintr")
                    Session("roid") = Session("dint")
                    Session("mamt") = 0
                Case "RD"


                    Session("amt") = txtamt.Text
                    Session("roic") = Session("cintr")
                    Session("roid") = Session("dint")
                    UPDATE_MAT_VAL()
                   ' Session("mamt") = Session("mat")

                Case "RID"

                    Session("amt") = txtamt.Text
                    'Session("roic") = Session("cintr")
                    '//Session("roid") = Session("dint")
                    '//Session("mamt") = Session("mat")



                    ' Session("amt") = CDbl(txtamt.Text)
                    Session("roic") = CDbl(Session("cintr"))
                    Session("roid") = CDbl(Session("dint"))

                    UPDATE_MAT_VAL()

                    'Session("mamt") = txtamt.Text


                Case "SB"
                    Session("amt") = txtamt.Text
                    Session("roic") = Session("cintr")
                    Session("roid") = Session("dint")
                    Session("mamt") = 0


            End Select

            If con.State = ConnectionState.Closed Then con.Open()

            query = "UPDATE master SET amount = @amt,mamt=@mmat,cint=@cint,dint=@dint where (master.acno= @acn)"
            cmd.CommandText = query
            cmd.Connection = con
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@amt", txtamt.Text)
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            cmd.Parameters.AddWithValue("@mmat", Session("mamt"))
            cmd.Parameters.AddWithValue("@cint", Session("roic"))
            cmd.Parameters.AddWithValue("@dint", Session("roid"))
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
            Finally
                con.Close()
                cmd.Dispose()

            End Try

            If con.State = ConnectionState.Closed Then con.Open()

            If mop.SelectedItem.Text = "Cash" Then
                If CDbl(txtamt.Text) > 18000 Then
                    Session("amt") = 18000
                Else
                    Session("amt") = txtamt.Text
                End If
            Else
                Session("amt") = txtamt.Text
            End If


            If Trim(lblproduct.Text) = "FD" Then
                If mop.SelectedItem.Text = "Cash" Then
                    If CDbl(txtamt.Text) > 18000 Then
                        Session("mamt") = 18000
                    Else
                        Session("mamt") = txtamt.Text
                    End If
                Else
                    Session("mamt") = txtamt.Text
                End If

            End If

            query = "UPDATE masterc SET amount = @amt,mamt=@mmat,cint=@cint,dint=@dint where (masterc.acno= @acn)"
            cmd.CommandText = query
            cmd.Connection = con
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@amt", Session("amt"))
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            cmd.Parameters.AddWithValue("@mmat", Session("mamt"))
            cmd.Parameters.AddWithValue("@cint", Session("roic"))
            cmd.Parameters.AddWithValue("@dint", Session("roid"))
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
            Finally
                con.Close()
                cmd.Dispose()

            End Try
            Session("cintr") = Session("roic")
            Session("dint") = Session("roid")
        End If

        'UpdateDepRoi()

        If Trim(Session("product")) = "RD" Then
            If Session("amt") = 0 Then Session("amt") = CDbl(txtamt.Text)
            update_suplementry(Session("tid"), prod, CDbl(txtnod.Text) * Session("amt"), txtnar.Text)

            update_rd()
        ElseIf Trim(Session("product")) = "DS" Then
            If Session("amt") = 0 Then Session("amt") = CDbl(txtamt.Text)
            update_suplementry(Session("tid"), prod, CDbl(txtamt.Text), txtnar.Text)

            update_ds()

        Else
            update_suplementry(Session("tid"), prod, txtamt.Text, txtnar.Text)

        End If

        If mop.SelectedItem.Text = "Account" Then
            update_sb()
        End If

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con


        If txtpenalty.Text = "" Then txtpenalty.Text = 0

        If Not txtpenalty.Text = 0 Then

            update_suplementry(Session("tid"), "RD DEFAULT INTEREST", txtpenalty.Text, txtnar.Text)


        End If

        Select Case Trim(mop.SelectedItem.Text)
            Case "Cash"
                If CDbl(txtamt.Text) > 18000 Then
                    If lblamt.Text = 0 Then
                        set_changes_c()
                    Else
                        set_changes_trans()
                    End If

                Else

                    set_changes_trans()

                End If


            Case "Account"

                set_changes_nocash()
                update_sb_C()


            Case "Transfer"

                set_changes_nocash()

        End Select







        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transaction Completed. Id # " + Session("tid")
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

    End Sub

    Function chk4srcitizen(ByVal cid As String)
        Session("srcitizen") = 0

        If Not Trim(Session("dob")) = "" Then

            Dim x As Integer = DateDiff(DateInterval.Year, CDate(Session("dob")), Today)

            If x >= 60 Then Session("srcitizen") = Session("src")
        Else
            Session("srcitizen") = 0
        End If


        Return Session("srcitizen")


    End Function
    Function chk4transfer(ByVal cid As String)

        Session("rebate") = 0

        query = "SELECT TOP 1 isnull(goldrate.bnktrf,0),isnull(goldrate.srcitizen,0),isnull(goldrate.renew,0),isnull(goldrate.deposit,0),isbnktrf,isrenew FROM dbo.goldrate  ORDER BY date DESC "

        If con.State = ConnectionState.Closed Then con.Open()


        cmdx.CommandText = query
        cmdx.Connection = con

        Dim drx As SqlDataReader

        Try
            drx = cmdx.ExecuteReader()
            If drx.HasRows Then
                drx.Read()
                Session("rebate") = drx(0)
                Session("src") = drx(1)
                Session("renewal") = drx(2)
                Session("spl") = drx(3)
                Session("istrf") = drx(4)
                Session("isrenewal") = drx(5)
            Else
                Session("src") = 0
                Session("rebate") = 0
                Session("renewal") = 0
                Session("spl") = 0
                Session("istrf") = 0
                Session("isrenewal") = 0

            End If




        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
        End Try


        cmdx.Dispose()


        Return Session("rebate")


    End Function

    Sub update_ds()
        Dim due As String = ""
        Dim query As String
        Dim i As Integer
        Dim nod As Integer = 0

        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Parameters.Clear()

        cmd.Connection = con

        If Not CDbl(lblamt.Text) = 0 Then
            Session("amt") = CInt(lblamt.Text) 'CDbl(txtamt.Text) / CDbl(txtnod.Text)
            nod = CDbl(txtamt.Text) / CInt(lblamt.Text)
        Else
            Session("amt") = txtamt.Text
            nod = 1
        End If




        Dim d As String = get_collection(txtacn.Text)

        For i = 1 To nod Step 1

            due = "Collection :" + CStr((d + i))
            If Not String.IsNullOrWhiteSpace(Trim(txtnar.Text)) Then
                due += " - " + Trim(txtnar.Text)
            End If

            query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"


            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", txtacn.Text)
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", Session("amt"))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", Session("amt"))
            If mop.SelectedItem.Text = "Cash" Then
                cmd.Parameters.AddWithValue("@narration", "By Cash")
            End If
            If mop.SelectedItem.Text = "Account" Then
                cmd.Parameters.AddWithValue("@narration", "BY Transfer (" + Trim(txt_sb.Text) + ")")
            End If
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@narration", "By Transfer " + bnk.SelectedItem.Text)
            End If

            cmd.Parameters.AddWithValue("@due", due)
            cmd.Parameters.AddWithValue("@type", "CASH")
            cmd.Parameters.AddWithValue("@sup", "DAILY DEPOSIT")
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

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + EX.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)

            Finally

                cmd.Parameters.Clear()

                cmd.Dispose()
                con.Close()

            End Try




        Next
    End Sub

    Sub update_rd()

        Dim due As String = ""
        Dim query As String
        Dim i As Integer

        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Parameters.Clear()

        cmd.Connection = con

        If Not CDbl(lblamt.Text) = 0 Then
            Session("amt") = CInt(lblamt.Text) 'CDbl(txtamt.Text) / CDbl(txtnod.Text)
        Else
            Session("amt") = txtamt.Text
        End If
        For i = 1 To txtnod.Text Step 1


            Dim d As String = get_due(txtacn.Text, i - 1)
            If Not String.IsNullOrWhiteSpace(Trim(txtnar.Text)) Then
                d += " - " + Trim(txtnar.Text)
            End If

            query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"


            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", txtacn.Text)
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", Session("amt"))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", Session("amt"))
            cmd.Parameters.AddWithValue("@narration", "By Cash")
            cmd.Parameters.AddWithValue("@due", d)
            cmd.Parameters.AddWithValue("@type", "CASH")
            cmd.Parameters.AddWithValue("@sup", "RECURRING DEPOSIT")
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

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + EX.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)

            Finally

                cmd.Parameters.Clear()

                cmd.Dispose()
                con.Close()

            End Try




        Next

        If con.State = ConnectionState.Closed Then con.Open()

        query = "UPDATE trans SET due=@nar where id=@id"
        cmd.Parameters.Clear()
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@nar", "Paid Upto " + due)
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
        End Try
        'query = ""
        'cmd.CommandText = Nothing
        'cmd.Connection = Nothing

        'Query = "SELECT MAX(ID) from dbo.trans where trans.acno='" + txtacn.Text + "'"
        'cmd.Connection = con
        'cmd.CommandText = query

        'countresult = cmd.ExecuteScalar()

        'tid = Convert.ToString(countresult)

        'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

        'sb.Append("<div class=" + """alert alert-dismissable alert-info """ + ">")
        'sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
        'sb.Append("<strong>Updated !</strong> Transaction Id is " + tid)
        'sb.Append("</div>")
        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())

    End Sub
    Sub update_sb()
        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Parameters.Clear()

        cmd.Connection = con




        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"


        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
        cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@crc", 0)
        cmd.Parameters.AddWithValue("@narration", "To Transfer")
        cmd.Parameters.AddWithValue("@due", txtacn.Text)
        cmd.Parameters.AddWithValue("@type", "TRF")
        If Left(txt_sb.Text, 2) = "79" Then
            cmd.Parameters.AddWithValue("@sup", "SAVINGS DEPOSIT")
        Else
            cmd.Parameters.AddWithValue("@sup", "KMK DEPOSIT")
        End If
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

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + EX.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)

        Finally

            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()

        End Try


    End Sub


    Sub update_sb_C()
        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Parameters.Clear()

        cmd.Connection = con




        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"


        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
        cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@crc", 0)
        cmd.Parameters.AddWithValue("@narration", "To Transfer")
        cmd.Parameters.AddWithValue("@due", txtacn.Text)
        cmd.Parameters.AddWithValue("@type", "TRF")
        If Left(txt_sb.Text, 2) = "79" Then
            cmd.Parameters.AddWithValue("@sup", "SAVINGS DEPOSIT")
        Else
            cmd.Parameters.AddWithValue("@sup", "KMK DEPOSIT")
        End If
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

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + EX.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)

        Finally

            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()

        End Try


    End Sub

    Private Sub tdate_TextChanged(sender As Object, e As EventArgs) Handles tdate.TextChanged
        On Error GoTo nxt
        get_ac_info(txtacn.Text)
        'txtfocus(txtnod)

        Exit Sub

nxt:
        txtfocus(tdate)
    End Sub




    Private Sub txtamt_TextChanged(sender As Object, e As EventArgs) Handles txtamt.TextChanged

        If Not lblproduct.Text = "DS" Then
            txtfocus(txtnar)
        Else
            If Not CDbl(lblbal.Text) = 0 Then
                Dim nod As Integer = (CDbl(lblbal.Text) + CDbl(txtamt.Text)) / CDbl(lblamt.Text)
                Dim totalNod As Integer = 300
                If CDbl(Session("prd")) <= 4 Then
                    totalNod = 100
                ElseIf CDbl(Session("prd")) <= 8 Then
                    totalNod = 200
                End If
                If nod <= totalNod Then
                    If IsTransactionAcceptable() Then
                        txtfocus(txtnar)
                    End If
                Else
                    Dim stitle = "Hi " + Session("sesusr")
                    Dim msg = "No of Dues Exceeds.."
                    Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
                    txtamt.Text = ""
                    txtamt.Focus()
                End If
            Else
                txtfocus(txtnar)
            End If

        End If



    End Sub

    Function IsTransactionAcceptable()
        Dim allowable As Boolean = True
        Dim totalCredit As Double = 0
        Double.TryParse(Session("totalCreditInADay"), totalCredit)
        If CDbl(lblamt.Text) > 0 Then
            If CDbl(txtamt.Text) > CDbl(lblamt.Text) * 5 Or totalCredit + CDbl(txtamt.Text) > CDbl(lblamt.Text) * 5 Then
                Dim stitle = "Hi " + Session("sesusr")
                Dim msg = "Entered amount exceeds the daily allowed limit"
                Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
                txtamt.Text = ""
                txtamt.Focus()
                allowable = False
            End If
        End If
        Return allowable
    End Function

    Private Sub mop_TextChanged(sender As Object, e As EventArgs) Handles mop.TextChanged

        If mop.SelectedItem.Text = "Transfer" Then

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
                '  pnltran.Visible = True
                bnk.Visible = True
                lbl.Visible = True
                ' pnlsbtrf.Visible = False
                txt_sb.Visible = False
                'btn_trf.Visible = False
                lblsb.Visible = False
                lbl_sb_bal.Text = ""


            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
            Finally

            End Try

        ElseIf mop.SelectedItem.Text = "Account" Then
            '  pnltran.Visible = False
            bnk.Visible = False
            lbl.Visible = False
            '   pnlsbtrf.Visible = True
            txt_sb.Visible = True
            '  btn_trf.Visible = True
            lblsb.Visible = True
            txtfocus(txt_sb)
        Else
            '  pnltran.Visible = False
            bnk.Visible = False
            lbl.Visible = False
            lblsb.Visible = False
            ' pnlsbtrf.Visible = False
            txt_sb.Visible = False
            lbl_sb_bal.Text = ""
            ' btn_trf.Visible = False
            txtfocus(bnk)
        End If
    End Sub

    Private Sub btn_up_rcpt_Click(sender As Object, e As EventArgs) Handles btn_up_rcpt.Click
        'txtamt.ReadOnly = True


        If mop.SelectedItem.Text = "Account" Then

            If CDbl(txtamt.Text) > CDbl(lbl_sb_bal.Text) - 100 Then
                btn_up_rcpt.Enabled = False
                txt_sb.Text = ""
                txt_sb.Focus()

                Exit Sub

            End If


        End If


        btn_up_rcpt.Enabled = False

        If lblproduct.Text = "DS" Then
            If Not lblamt.Text = 0 Then
                Dim x As Integer = txtamt.Text Mod lblamt.Text
                If x <> 0 Then

                    Dim stitle = "Hi " + Session("sesusr")
                    Dim msg = "Amount should be Multiple of <b>" + FormatCurrency(lblamt.Text) + "</b>"
                    Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

                    Exit Sub

                Else

                    If Not CDbl(lblbal.Text) = 0 Then
                        Dim nod As Integer = (CDbl(lblbal.Text) + CDbl(txtamt.Text)) / CDbl(lblamt.Text)
                        If nod <= 300 Then
                            set_changes()
                        Else


                            Dim stitle = "Hi " + Session("sesusr")
                            Dim msg = "No of Dues Exceeds.."
                            Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


                            txtamt.Text = ""
                        End If
                    Else
                        set_changes()
                    End If
                End If
            Else
                set_changes()
            End If
        Else

            set_changes()


        End If

        If mop.SelectedItem.Text = "Cash" Then
            clear_tab_recpt()
        Else
            prepare_print()
        End If






    End Sub


    Sub prepare_print()


        lblcpt.Text = "RECEIPT"
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
        pnote.Text = txtnar.Text
        pcnote.Text = txtnar.Text
        premit.Text = pcname.Text

        If mop.SelectedItem.Text = "Account" Then
            pnar.Text = "By Transfer " + txt_sb.Text
        Else
            pnar.Text = "By Transfer " + bnk.SelectedItem.Text
        End If


        lblccpt.Text = "PAYMENT"
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
            pcnar.Text = "To Transfer for " + txtacn.Text
        Else
            pcacno.Text = txtacn.Text
            pcglh.Text = bnk.SelectedItem.Text
            pccid.Text = get_memberno(txtacn.Text)
            pccname.Text = lblname.Text
            pcnar.Text = "To Transfer for " + txtacn.Text
        End If
        pcamt.Text = FormatCurrency(txtamt.Text)
        pcaiw.Text = get_wrds(txtamt.Text)
        pcremit.Text = pccname.Text
        clear_tab_recpt()


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
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
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
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
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
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
        End Try

        Return home


    End Function

    Private Sub update_suplementry_c(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, Optional ByVal notes As String = "")
        If tid = 0 Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        cmd.Connection = con

        If mop.SelectedItem.Text = "Cash" Then

            query = "INSERT INTO suplementc (date,transid,achead,debit,credit,acn,narration,type,notes)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ,@notes)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", Session("acn"))
            cmd.Parameters.AddWithValue("@nar", "By Cash")
            cmd.Parameters.AddWithValue("@typ", "RECEIPT")
            cmd.Parameters.AddWithValue("@notes", notes)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)


            End Try

        Else

            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type,notes)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ,@notes)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", Session("acn"))
            cmd.Parameters.AddWithValue("@nar", "By Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            cmd.Parameters.AddWithValue("@notes", notes)
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
            End Try

            cmd.Parameters.Clear()


            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type,notes)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ,@notes)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@notes", notes)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@achead", bnk.SelectedItem.Text)
                cmd.Parameters.AddWithValue("@acn", Session("acn"))
            Else
                If Left(Trim(txt_sb.Text), 2) = "79" Then
                    cmd.Parameters.AddWithValue("@achead", "SAVINGS DEPOSIT")
                Else
                    cmd.Parameters.AddWithValue("@achead", "KMK DEPOSIT")
                End If

                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
            End If
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)

            cmd.Parameters.AddWithValue("@nar", "To Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
            End Try

        End If

        query = ""



    End Sub

    'Sub UpdateDepRoi()

    '    If CDbl(lblamt.Text) = 0 Then



    '        Select Case Trim(lblproduct.Text)
    '            Case "DS"
    '                Session("amt") = txtamt.Text
    '                Session("roic") = Session("cintr")
    '                Session("roid") = Session("dint")
    '                Session("mamt") = 0
    '            Case "FD"
    '                Session("transfer") = chk4transfer(Trim(Session("ncid")))
    '                Session("srcitzen") = chk4srcitizen(Trim(Session("ncid")))

    '                Session("amt") = CDbl(txtamt.Text)
    '                Session("roic") = CDbl(Session("cintr")) + CDbl(Session("transfer")) + CDbl(Session("srcitzen"))
    '                Session("roid") = CDbl(Session("dint")) + CDbl(Session("transfer")) + CDbl(Session("srcitzen"))
    '                Session("mamt") = txtamt.Text

    '            Case "KMK"
    '                Session("amt") = txtamt.Text
    '                Session("roic") = Session("cintr")
    '                Session("roid") = Session("dint")
    '                Session("mamt") = 0
    '            Case "RD"
    '                UPDATE_MAT_VAL_c()

    '                Session("amt") = txtamt.Text
    '                Session("roic") = Session("cintr")
    '                Session("roid") = Session("dint")
    '                Session("mamt") = Session("mat")

    '            Case "RID"
    '                UPDATE_MAT_VAL_c()
    '                Session("amt") = txtamt.Text
    '                Session("roic") = Session("cintr")
    '                Session("roid") = Session("dint")
    '                Session("mamt") = Session("mat")

    '            Case "SB"
    '                Session("amt") = txtamt.Text
    '                Session("roic") = Session("cintr")
    '                Session("roid") = Session("dint")
    '                Session("mamt") = 0


    '        End Select

    '        If con.State = ConnectionState.Closed Then con.Open()

    '        query = "UPDATE masterc SET amount = @amt,mamt=@mmat,cint=@cint,dint=@dint where (masterc.acno= @acn)"
    '        cmd.CommandText = query
    '        cmd.Connection = con
    '        cmd.Parameters.Clear()
    '        cmd.Parameters.AddWithValue("@amt", txtamt.Text)
    '        cmd.Parameters.AddWithValue("@acn", txtacn.Text)
    '        cmd.Parameters.AddWithValue("@mmat", Session("mamt"))
    '        cmd.Parameters.AddWithValue("@cint", Session("roic"))
    '        cmd.Parameters.AddWithValue("@dint", Session("roid"))
    '        Try
    '            cmd.ExecuteNonQuery()

    '        Catch ex As Exception
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
    '        Finally
    '            con.Close()
    '            cmd.Dispose()

    '        End Try


    '    End If



    'End Sub

    Sub set_changes_nocash()

        Session("total_amt") = CDbl(txtamt.Text)

        If CDbl(txtpenalty.Text) = 0 Then
            Session("total_amt") = CDbl(txtamt.Text)
        Else
            Session("total_amt") = CDbl(txtamt.Text) - CDbl(txtpenalty.Text)
        End If
        Session("acn") = txtacn.Text


        Session("splitamt") = Session("total_amt")

        Dim actrans As New Actrans
        actrans.id = Session("tid")
        actrans.dt = CDate(tdate.Text)
        actrans.acno = txtacn.Text
        actrans.drd = 0
        actrans.crd = Session("splitamt")
        actrans.drc = 0
        actrans.crc = Session("splitamt")
        actrans.narration = "By Transfer"
        If mop.SelectedItem.Text = "Account" Then
            actrans.due = txt_sb.Text
        ElseIf mop.SelectedItem.Text = "Transfer" Then
            actrans.due = bnk.SelectedItem.Text
        End If
        actrans.typ = "TRF"
        actrans.suplimentery = get_pro(Session("product"))
        actrans.sesusr = Session("sesusr")
        actrans.entryat = Convert.ToDateTime(Now)
        update_actrans(actrans)
        '   
        update_suplementry_c(Session("tid"), actrans.suplimentery, actrans.crc, txtnar.Text)
        If Not CDbl(txtpenalty.Text) = 0 Then
            update_suplementry_c(Session("tid"), "RD DEFAULT INTEREST", txtpenalty.Text, txtnar.Text)
        End If






    End Sub
    Sub set_changes_trans()

        If CDbl(txtpenalty.Text) = 0 Then
            Session("total_amt") = CDbl(txtamt.Text)
        Else
            Session("total_amt") = CDbl(txtamt.Text) - CDbl(txtpenalty.Text)
        End If

        If Session("total_amt") <= 18000 Then
            Session("splitamt") = Session("total_amt")
            Session("total_amt") = Session("total_amt") - Session("splitamt")
        Else
            Session("splitamt") = 18000
            Session("total_amt") = Session("total_amt") - Session("splitamt")
        End If

        Dim actrans As New Actrans
        actrans.id = Session("tid")
        actrans.dt = CDate(tdate.Text)
        actrans.acno = Trim(txtacn.Text)
        actrans.drd = 0
        actrans.crd = Session("splitamt")
        actrans.drc = 0
        actrans.crc = Session("splitamt")
        actrans.narration = "By Cash"
        actrans.due = ""
        actrans.typ = "CASH"
        actrans.suplimentery = get_pro(Session("product"))
        actrans.sesusr = Session("sesusr")
        actrans.entryat = Convert.ToDateTime(Now)
        update_actrans(actrans)
        '   
        update_suplementry_c(Session("tid"), actrans.suplimentery, actrans.crc, txtnar.Text)
        If Not CDbl(txtpenalty.Text) = 0 Then
            update_suplementry_c(Session("tid"), "RD DEFAULT INTEREST", txtpenalty.Text, txtnar.Text)
        End If




        Do Until Session("total_amt") = 0

            If Session("total_amt") < 18000 Then
                Session("splitamt") = Session("total_amt")
                Session("total_amt") = Session("total_amt") - Session("splitamt")

            Else
                Session("splitamt") = 18000
                Session("total_amt") = Session("total_amt") - Session("splitamt")

            End If
            If Session("product") = "KMK" Then
                Session("acn") = Get_Rnd_KMK(1, Session("splitamt"))

            Else
                Session("acn") = Get_Rnd_SB(1, Session("splitamt"))
            End If


            actrans.id = Session("tid")
            actrans.dt = CDate(tdate.Text)
            actrans.acno = Session("acn")
            actrans.drd = 0
            actrans.crd = Session("splitamt")
            actrans.drc = 0
            actrans.crc = Session("splitamt")
            actrans.narration = "By Cash"
            actrans.due = ""
            actrans.typ = "CASH"
            actrans.suplimentery = get_pro(Session("product"))
            actrans.sesusr = Session("sesusr")
            actrans.entryat = Convert.ToDateTime(Now)
            update_actrans(actrans)
            '   update_suplementry_c(Session("tid"), "RD DEFAULT INTEREST", txtpenalty.Text)
            update_suplementry_c(Session("tid"), actrans.suplimentery, actrans.crc, txtnar.Text)


        Loop


    End Sub

    Sub StandingIns_C()

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con



        query = "insert into stdinsc(srcproduct,acno,product,siacno,sidate)"
        query &= "values(@srcproduct,@acno,@product,@siacno,@sidate)"

        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@srcproduct", lblproduct.Text)
        cmd.Parameters.AddWithValue("@acno", Session("acn"))
        cmd.Parameters.AddWithValue("@product", "SB")
        cmd.Parameters.AddWithValue("@siacno", Session("sbacn"))
        cmd.Parameters.AddWithValue("@sidate", Left(Trim(Session("ac_date")), 2))

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
        Finally
            con.Close()
            cmd.Dispose()

        End Try

    End Sub
    Function get_sb(ByVal cid As String)

        Dim sba As String
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        query = "select acno from masterc where cid ='" + cid + "' and product='SB' "

        cmd.Parameters.Clear()
        cmd.CommandText = query

        sba = cmd.ExecuteScalar

        Return sba



    End Function
    Sub update_masterc()


        If CDbl(lblamt.Text) = 0 Then
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Parameters.Clear()
            Session("transfer") = chk4transfer(Trim(Session("ncid")))
            Session("srcitzen") = chk4srcitizen(Trim(Session("ncid")))

            If Trim(Session("product")) = "RD" Or Trim(Session("product")) = "RID" Then

                If mop.SelectedItem.Text = "Transfer" Then
                    query = "UPDATE masterc SET amount = @amt,mamt=@mmat,cint=@cint,dint=@dint where (master.acno= @acn)"

                    If Trim(Session("product")) = "RD" Then
                        Session("roi") = CDbl(Session("cint"))
                    Else
                        Session("roi") = CDbl(Session("cint")) + CDbl(Session("srcitzen")) + CDbl(Session("transfer"))
                    End If

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@amt", txtamt.Text)
                    cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                    cmd.Parameters.AddWithValue("@mmat", Session("mat"))
                    cmd.Parameters.AddWithValue("@cint", Session("roi"))
                    cmd.Parameters.AddWithValue("@dint", Session("roi"))

                Else
                    query = "UPDATE masterc SET amount = @amt,mamt=@mmat where (master.acno= @acn)"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@amt", txtamt.Text)
                    cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                    cmd.Parameters.AddWithValue("@mmat", Session("mat"))

                End If



                cmd.CommandText = query
                cmd.Connection = con

            Else

                If CDbl(txtamt.Text) = 0 Then
                    If lblproduct.Text = "FD" Then
                        If CDbl(txtamt.Text) <= 18000 Then
                            Session("amt") = txtamt.Text
                            Session("mat") = txtamt.Text
                        Else
                            Session("amt") = 18000
                            Session("mat") = 18000

                        End If
                    Else
                        Session("mat") = 0
                    End If
                End If

                If mop.SelectedItem.Text = "Transfer" Then

                    query = "UPDATE masterc SET amount = @amt,cint=@cint,dint=@dint,mamt=@mmat where (master.acno= @acn)"
                    If Session("product") = "FD" Then
                        Session("roi") = CDbl(Session("cint")) + CDbl(Session("srcitzen")) + CDbl(Session("transfer"))
                    Else
                        Session("roi") = CDbl(Session("cint")) + CDbl(Session("srcitzen"))
                    End If

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@amt", txtamt.Text)
                    cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                    cmd.Parameters.AddWithValue("@cint", Session("roi"))
                    cmd.Parameters.AddWithValue("@dint", Session("roi"))
                    cmd.Parameters.AddWithValue("@mmat", Session("mat"))

                Else

                    cmd.Parameters.Clear()

                    query = "UPDATE masterc SET amount = @amt,cint=@cint,dint=@dint,mamt=@mmat where (master.acno= @acn)"
                    Session("roi") = CDbl(Session("cint")) + CDbl(Session("srcitzen"))

                    cmd.Parameters.AddWithValue("@amt", txtamt.Text)
                    cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                    cmd.Parameters.AddWithValue("@cint", Session("roi"))
                    cmd.Parameters.AddWithValue("@dint", Session("roi"))
                    cmd.Parameters.AddWithValue("@mmat", Session("mat"))

                End If





                cmd.CommandText = query
                cmd.Connection = con

            End If

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.ToString.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)
            Finally
                con.Close()
                cmd.Dispose()

            End Try
        End If

    End Sub

    Sub set_changes_c()

        Dim x As String



        If Session("product") = "SB" Or Session("product") = "KMK" Then
            set_changes_trans()

        Else

            If Session("product") = "RD" Or Session("product") = "RID" Then
                If CDbl(lblamt.Text = 0) Then
                    Session("splitamt") = CDbl(txtamt.Text)
                    UPDATE_MAT_VAL_c()
                Else
                    update_masterc()
                End If


            End If

            Session("acn") = txtacn.Text
            Session("total_amt") = CDbl(txtamt.Text)
            Session("splitamt") = 18000
            Session("total_amt") = Session("total_amt") - Session("splitamt")
            UPDATE_MAT_VAL_c()

            'UpdateDepRoi()

            'Session("sbacn") = Get_Rnd_SB()
            'StandingIns_C()

            Dim FirstEntry As New Actrans
            FirstEntry.id = Session("tid")
            FirstEntry.dt = CDate(tdate.Text)
            FirstEntry.acno = Session("acn")
            FirstEntry.drd = 0
            FirstEntry.crd = Session("splitamt")
            FirstEntry.drc = 0
            FirstEntry.crc = Session("splitamt")
            FirstEntry.narration = "By Cash"
            FirstEntry.due = ""
            FirstEntry.typ = "CASH"
            FirstEntry.suplimentery = get_pro(Session("product"))
            FirstEntry.sesusr = Session("sesusr")
            FirstEntry.entryat = Convert.ToDateTime(Now)

            update_actrans(FirstEntry)
            update_suplementry_c(Session("tid"), FirstEntry.suplimentery, FirstEntry.crc)

            Do Until Session("total_amt") = 0

                If Session("total_amt") < 18000 Then
                    Session("splitamt") = Session("total_amt")
                    Session("total_amt") = Session("total_amt") - Session("splitamt")

                Else
                    Session("splitamt") = 18000
                    Session("total_amt") = Session("total_amt") - Session("splitamt")

                End If

                Session("acn") = GetNewDepositAcno(Session("product"))
                Dim acmas As New ACMas
                acmas.dt = CDate(tdate.Text)
                acmas.acno = Session("acn")
                acmas.cid = Get_Rnd_Member()
                acmas.serial = Session("serial")
                acmas.prod = Session("product")
                acmas.amt = Session("splitamt")
                acmas.cintr = Session("cintr")
                acmas.dintr = Session("dint")
                acmas.cld = 0
                acmas.prd = CDbl(Session("prd"))
                acmas.prdtyp = Session("prdtyp")
                acmas.mdate = Session("mdt")
                acmas.mamt = 0
                acmas.isren = 0
                acmas.renacn = ""
                acmas.parent = txtacn.Text
                OpenNewDeposit(acmas)
                If Trim(lblproduct.Text) = "FD" Then
                    x = get_sb(acmas.cid)
                    If Not x = Nothing Then
                        Session("sbacn") = x
                    End If

                    StandingIns_C()
                    UPDATE_MAT_VAL_c()
                End If

                Dim actrans As New Actrans
                actrans.id = Session("tid")
                actrans.dt = CDate(tdate.Text)
                actrans.acno = Session("acn")
                actrans.drd = 0
                actrans.drc = 0
                actrans.crd = Session("splitamt")
                actrans.crc = Session("splitamt")
                actrans.narration = "By Cash"
                actrans.due = ""
                actrans.typ = "CASH"
                actrans.suplimentery = get_pro(Session("product"))
                actrans.sesusr = Session("sesusr")
                actrans.entryat = Convert.ToDateTime(Now)
                '  UpdateDepRoi()
                update_actrans(actrans)
                update_suplementry_c(Session("tid"), actrans.suplimentery, actrans.crc)


            Loop




        End If







    End Sub

    Sub update_actrans(ByVal actrans As Actrans)


        Dim prod = get_pro(Session("product"))

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"
                cmd.Parameters.Clear()
                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@id", actrans.id)
                cmd.Parameters.AddWithValue("@date", CDate(actrans.dt))
                cmd.Parameters.AddWithValue("@acno", actrans.acno)
                cmd.Parameters.AddWithValue("@drd", actrans.drd)
                cmd.Parameters.AddWithValue("@crd", actrans.crd)
                cmd.Parameters.AddWithValue("@drc", actrans.drc)
                cmd.Parameters.AddWithValue("@crc", actrans.crc)

                Select Case mop.SelectedItem.Text

                    Case "Cash"
                        cmd.Parameters.AddWithValue("@narration", "By Cash")
                        cmd.Parameters.AddWithValue("@type", "CASH")
                        If Trim(txtnar.Text = "") Then
                            cmd.Parameters.AddWithValue("@due", " ")
                        Else
                            cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text))
                        End If
                    Case "Transfer"
                        cmd.Parameters.AddWithValue("@narration", "By Transfer")
                        cmd.Parameters.AddWithValue("@type", "TRF")

                        If Trim(txtnar.Text = "") Then
                            cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                        Else
                            cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text) + " " + bnk.SelectedItem.Text)
                        End If
                    Case "Account"
                        cmd.Parameters.AddWithValue("@narration", "By Transfer")
                        cmd.Parameters.AddWithValue("@type", "TRF")
                        cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text) + " " + txt_sb.Text)
                End Select
                cmd.Parameters.AddWithValue("@suplimentry", prod)
                cmd.Parameters.AddWithValue("@sesusr", actrans.sesusr)
                cmd.Parameters.AddWithValue("@entryat", actrans.entryat)
                cmd.Parameters.AddWithValue("@cbal", 0)
                cmd.Parameters.AddWithValue("@dbal", 0)

                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast" + Guid.NewGuid().ToString(), "showToastnOK('Error', '" + ex.Message.ToString().Replace("'", "").Replace(vbCrLf, " ") + "');", True)

                Finally
                    cmd.Dispose()
                    con.Close()
                End Try


            End Using

        End Using












    End Sub


    Sub clear_tab_recpt()

        'lblpen.Visible = False
        'rdpenal.Visible = False
        Session("tid") = Nothing
        txtacn.Text = ""
        lblamt.Text = ""
        lblbal.Text = ""
        lblname.Text = ""
        lbladd.Text = ""
        lblmobile.Text = ""
        pnlact.Visible = False
        imgCapture.Visible = False
        lblproduct.Text = ""
        txtnod.Text = ""
        txtpenalty.Text = ""
        txtnar.Text = ""
        rddue.Visible = False
        rdpenal.Visible = False
        txtamt.Text = ""
        rdinfo.Visible = False
        ' TabContainer1.Visible = False
        txtacn.Enabled = True

        'pnltran.Visible = False
        sb_srch = False
        'pnlsbtrf.Visible = False
        lbl_sb_bal.Text = ""
        txt_sb.Text = ""
        btn_up_rcpt.Enabled = True
        Session("acn") = Nothing

        ' Dim cs As ClientScriptManager = Me.ClientScript
        'cs.RegisterStartupScript(Me.GetType(), "test", "SCTOP()", True)

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        txtfocus(txtacn)

    End Sub

    Private Sub btn_up_can_Click(sender As Object, e As EventArgs) Handles btn_up_can.Click
        clear_tab_recpt()

    End Sub

    Private Sub txt_sb_TextChanged(sender As Object, e As EventArgs) Handles txt_sb.TextChanged

        Dim bal As Double = 0
        Double.TryParse(get_balance(Trim(txt_sb.Text)), bal)
        lbl_sb_bal.Text = FormatCurrency(bal)

        If txtamt.Text = 0 Then
            txt_sb.Text = ""
            txtfocus(txtamt)
        End If

        If Trim(Left(txt_sb.Text, 2)) = "79" Or Trim(Left(txt_sb.Text, 2)) = "76" Then



            'Dim abal As Decimal = bal - 100

            'If CDbl(txtamt.Text) <= abal Then
            '    lbl_sb_bal.ForeColor = Color.Green
            '    btn_up_rcpt.Enabled = True
            'Else

            '    lbl_sb_bal.ForeColor = Color.Red
            '    btn_up_rcpt.Enabled = False
            'End If

        Else
            txt_sb.Text = ""
        End If
    End Sub


    Public Function GetNewDepositAcno(ByVal Product As String)

        Dim acn As String = String.Empty
        Dim acnp As String = get_acnprefix(Product)
        Dim oResult As String = String.Empty
        Dim serial As Integer = 0


        acn = Trim(acnp) '+ CStr(DateAndTime.Year(Convert.ToDateTime(ddt.Text)))

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "SELECT TOP 1 serial FROM masterc where product='" + Product + "'ORDER BY serial DESC"

                cmd.CommandText = query

                Try
                    oResult = cmd.ExecuteScalar()

                Catch ex As Exception

                End Try
                If oResult IsNot Nothing Then

                    serial = Int(oResult.ToString) + 1

                Else
                    serial = 1
                End If

                Session("serial") = serial

                acn += String.Format("{0:000000}", serial)



            End Using
        End Using


        Return acn


    End Function


    Private Sub DepositReceipt_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
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

    Private Sub btn_up_rcpt_Disposed(sender As Object, e As EventArgs) Handles btn_up_rcpt.Disposed

    End Sub

    Private Sub btntog_Click(sender As Object, e As EventArgs) Handles btntog.Click
        clear_tab_recpt()
        pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
        pnltrans.Style.Add(HtmlTextWriterStyle.Display, "block")
        txtacn.Focus()

    End Sub

    Protected Sub btn_reprint_submit_Click(sender As Object, e As EventArgs) Handles btn_reprint_submit.Click
        Dim reprintDateStr As String = txtReprintDate.Text.Trim()
        Dim transId As String = txtReprintTransID.Text.Trim()

        If String.IsNullOrEmpty(reprintDateStr) OrElse String.IsNullOrEmpty(transId) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", "showToastnOK('Error', 'Please enter both Date and Transaction ID.');", True)
            Return
        End If

        Dim reprintDate As DateTime
        If Not DateTime.TryParseExact(reprintDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, reprintDate) Then
            ' Depending on date picker format, fallback to standard date parsing or dd-MM-yyyy
            If Not DateTime.TryParseExact(reprintDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, reprintDate) Then
                If Not DateTime.TryParse(reprintDateStr, reprintDate) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", "showToastnOK('Error', 'Invalid Date format.');", True)
                    Return
                End If
            End If
        End If

        RecreateReceipts(reprintDate, transId)
    End Sub

    Private Sub RecreateReceipts(reprintDate As DateTime, transId As String)
        Dim dtReceipts As New DataTable()
        dtReceipts.Columns.Add("Branch", GetType(String))
        dtReceipts.Columns.Add("TransID", GetType(String))
        dtReceipts.Columns.Add("Date", GetType(String))
        dtReceipts.Columns.Add("AccountNo", GetType(String))
        dtReceipts.Columns.Add("AccountHead", GetType(String))
        dtReceipts.Columns.Add("MemberNo", GetType(String))
        dtReceipts.Columns.Add("MemberName", GetType(String))
        dtReceipts.Columns.Add("AmountFormatted", GetType(String))
        dtReceipts.Columns.Add("Narration", GetType(String))
        dtReceipts.Columns.Add("AmountInWords", GetType(String))
        dtReceipts.Columns.Add("RemitterName", GetType(String))
        dtReceipts.Columns.Add("ReceiptType", GetType(String))
        dtReceipts.Columns.Add("UserNote", GetType(String))

        Dim sql As String = "SELECT achead, acn, credit, debit, narration, notes FROM suplement WHERE CAST(date AS DATE) = CAST(@date AS DATE) AND transid = @transid AND (credit > 0 OR debit > 0)"

        Using conReprint As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            Using cmdReprint As New SqlCommand(sql, conReprint)
                cmdReprint.Parameters.AddWithValue("@date", reprintDate)
                cmdReprint.Parameters.AddWithValue("@transid", transId)

                Try
                    conReprint.Open()
                    Using reader As SqlDataReader = cmdReprint.ExecuteReader()
                        While reader.Read()
                            Dim acn As String = reader("acn").ToString()
                            Dim achead As String = reader("achead").ToString()
                            Dim credit As Decimal = If(IsDBNull(reader("credit")), 0D, Convert.ToDecimal(reader("credit")))
                            Dim debit As Decimal = If(IsDBNull(reader("debit")), 0D, Convert.ToDecimal(reader("debit")))
                            Dim narration As String = reader("narration").ToString()

                            ' Determine receipt type and amount based on which column has a value
                            Dim receiptType As String = If(credit > 0, "RECEIPT", "PAYMENT")
                            Dim amount As Decimal = If(credit > 0, credit, debit)

                            Dim memberNo As String = get_memberno(acn)
                            Dim memberName As String = get_membername(acn)
                            Dim amountWords As String = get_wrds(amount.ToString())

                            Dim row As DataRow = dtReceipts.NewRow()
                            row("Branch") = get_home()
                            row("TransID") = transId
                            row("Date") = reprintDate.ToString("dd-MM-yyyy")
                            row("AccountNo") = acn
                            row("AccountHead") = achead
                            row("MemberNo") = memberNo
                            row("MemberName") = memberName
                            row("AmountFormatted") = FormatCurrency(amount)
                            row("Narration") = narration
                            row("AmountInWords") = amountWords
                            row("RemitterName") = memberName
                            row("ReceiptType") = receiptType
                            row("UserNote") = If(IsDBNull(reader("notes")), "", reader("notes").ToString())

                            dtReceipts.Rows.Add(row)
                        End While
                    End Using
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", "showToastnOK('Error', '" & ex.Message.Replace("'", "") & "');", True)
                    Return
                End Try
            End Using
        End Using

        If dtReceipts.Rows.Count > 0 Then
            rptReprint.DataSource = dtReceipts
            rptReprint.DataBind()
            
            pnlReprintResults.Visible = True

            ' Close modal using plain JS (Bootstrap plugin may not be available after UpdatePanel postback)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModal",
                "document.getElementById('reprintModal').classList.remove('show');" &
                "document.getElementById('reprintModal').style.display='none';" &
                "document.body.classList.remove('modal-open');" &
                "var bd=document.querySelector('.modal-backdrop'); if(bd) bd.parentNode.removeChild(bd);", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", "showToastnOK('Info', 'No receipts found for the given Date and Transaction ID.');", True)
        End If
    End Sub

    Protected Sub btnReprintClose_Click(sender As Object, e As EventArgs) Handles btnReprintClose.Click
        pnlReprintResults.Visible = False
        txtacn.Focus()
    End Sub

End Class
