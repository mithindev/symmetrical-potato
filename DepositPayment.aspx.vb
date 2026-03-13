Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Public Class DepositPayment
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand

    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim yearpart As String

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
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{ var elem = document.getElementById('{0}'); if(elem) elem.focus(); }}", ctrl.ClientID), True)


    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load




        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then

            If Session("orgin") = "loananalysis" Then
                txtacn.Text = Session("auction_sb")
                tdate.Text = Date.Today
                get_ac_info(txtacn.Text)
                txtamt.Text = -(Session("auction_diff"))
                txtnar.Text = "TO JL AUCTION A/C No " + Session("auctionacn").ToString
            Else

                'bind_grid()

                ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtacn.ClientID), True)

            End If
        End If




    End Sub



    Public Function get_balance(ByVal acn As String)


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()


        Dim x As Integer = 1
        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno =@acno and scroll =@scroll GROUP BY [actrans].acno"
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@scroll", x)
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
        Return Session("dbal")

    End Function
    Public Sub get_ac_info(ByVal acn As String)

        Dim sql As String = "SELECT Master.date,Master.acno,Master.product,Master.amount,Master.cint,Master.dint,Master.prd,Master.prdtype,Master.mdate,Master.mamt,Master.cid,master.cld FROM dbo.master WHERE master.acno = '" + acn + "' and cld=0"



        Try
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
                Session("mdt") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(8)), Nothing, ds.Tables(0).Rows(0).Item(8))
                Session("mamt") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(9)), 0, ds.Tables(0).Rows(0).Item(9))
                Session("ncid") = ds.Tables(0).Rows(0).Item(10)
                Session("cld") = ds.Tables(0).Rows(0).Item(11)
                txtacn.Enabled = False
                soa.Enabled = True
                pnlact.Visible = True

            Else
                '    closure_notice.Visible = True
                '   lblmat.Text = "Invalid Account or Already Closed."

                txtacn.Enabled = True
                txtfocus(txtacn)

                Exit Sub


            End If
        Catch ex As Exception
            MsgBox(ex.ToString)

        End Try


        If Session("cld") = True Then

            ' closure_notice.Visible = True
            ' lblmat.Text = "Account Already Closed."
            Exit Sub

        End If



        sql = "SELECT FirstName,lastname,address,mobile from dbo.member where MemberNo='" + Session("ncid") + "'"

        Dim adapter1 As New SqlDataAdapter(sql, con)

        adapter1.Fill(ds1)

        If Not ds1.Tables(0).Rows.Count = 0 Then
            Session("ac_name") = ds1.Tables(0).Rows(0).Item(0)
            Session("ac_lname") = ds1.Tables(0).Rows(0).Item(1)
            Session("address") = ds1.Tables(0).Rows(0).Item(2)
            Session("mobile") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(3)), " ", ds1.Tables(0).Rows(0).Item(3))

            get_Img(Session("ncid").ToString())
        End If

        adapter1.Dispose()

        ds1.Dispose()




        lblbal.Text = FormatCurrency(get_balance(acn))
        lblproduct.Text = Trim(Session("product"))
        lblname.Text = Session("ac_name").ToString() & " " & Session("ac_lname").ToString()
        lbladd.Text = Session("address").ToString()
        lblmobile.Text = Session("mobile").ToString()
        lblamt.Text = FormatCurrency(Session("amt"))






        Select Case Session("product")

            Case "DS"
                btn_up_rcpt.Visible = False
            Case "FD"
                btn_up_rcpt.Visible = False
            Case "RD"
                btn_up_rcpt.Visible = False
            Case "RID"
                btn_up_rcpt.Visible = False


        End Select


        txtfocus(tdate)

        If Not IsNothing(Session.Item("diff_to_sb")) Then
            txtamt.Text = Session.Item("diff_to_sb")
        End If

    End Sub

    Private Sub get_Img(ByVal memberno As String)
        Dim query As String
        Dim dr As SqlDataReader
        Dim imgbytes As Byte()
        Dim stream As MemoryStream
        Dim imgx As Image

        Try
            query = "select photo from kyc where kyc.memberno=@memberno"

            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con
            cmd.CommandText = query
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@memberno", memberno)
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()
                If Not IsDBNull(dr(0)) Then
                    imgbytes = CType(dr.GetValue(0), Byte())
                    stream = New MemoryStream(imgbytes, 0, imgbytes.Length)
                    imgx = Image.FromStream(stream)

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
        End Try
    End Sub


    Sub get_last_transaction(ByVal acn As String)
        Dim oResult As Date


        cmdx.Connection = con
        cmdx.CommandText = "SELECT top 1 date  FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "'ORDER BY date DESC"

        oResult = cmdx.ExecuteScalar()

        If Not oResult = Date.MinValue Then
            Dim tdat As Date = Convert.ToDateTime(tdate.Text)
            Session("days_ago") = DateDiff(DateInterval.Month, oResult, tdat)
        Else
            Session("days_ago") = 1
        End If


    End Sub

    Function get_due(ByVal acn As String, ByVal curdue As Integer)

        Dim df As String = "dMMM-yyyy"
        Dim op As String
        Dim opt As DateTime

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

        Return op
    End Function

    ''Sub calculate_penalty()


    ''    Dim sql As String = "SELECT roi.penalty,roi.penaltyprd FROM dbo.roi WHERE roi.Product = '" + product + "'"
    ''    sql &= "AND roi.prddmy = '" + prdtyp + "'"
    ''    sql &= "AND roi.prdfrm <= " + prd
    ''    sql &= "AND roi.prdto >= " + prd


    ''    Dim penalty_ds As New DataSet

    ''    Dim penalty_adapter As New SqlDataAdapter(sql, con)


    ''    penalty_adapter.Fill(penalty_ds)

    ''    If Not penalty_ds.Tables(0).Rows.Count = 0 Then

    ''        If days_ago >= ds.Tables(0).Rows(0).Item(1) Then

    ''            Dim perhundred As Integer = penalty_ds.Tables(0).Rows(0).Item(0)
    ''            Dim penalprd As Integer = penalty_ds.Tables(0).Rows(0).Item(1)

    ''            txtpenalty.Text = ((amt * perhundred) / 100) * days_ago
    ''            rdpenal.Visible = True
    ''        End If


    ''    End If


    ''End Sub


    Private Sub txtacn_TextChanged(sender As Object, e As EventArgs) Handles txtacn.TextChanged
        '    TabContainer1.Visible = True
        ' txt_acn_srch.Text = ""
        'listgid1.Items.Clear()
        tdate.Text = Date.Today

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


    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double)

        If tid = 0 Then Exit Sub
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con


        If mop.SelectedItem.Text = "Cash" Then
            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            cmd.Parameters.AddWithValue("@nar", "TO CASH")
            cmd.Parameters.AddWithValue("@typ", "PAYMENT")

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else

            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)
            'cmd.Parameters.AddWithValue("@acn", txtacn.Text)

            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                cmd.Parameters.AddWithValue("@nar", "To Transfer " + bnk.SelectedItem.Text)
            Else
                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
                cmd.Parameters.AddWithValue("@nar", "To Transfer " + Trim(txtacn.Text) + " (" + Trim(txt_sb.Text) + ")")

            End If


            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

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
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            '    cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@nar", "By Transfer ")
            Else
                cmd.Parameters.AddWithValue("@nar", "By Transfer " + Trim(txt_sb.Text) + " (" + Trim(txtacn.Text) + ")")
            End If

            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try




            End If
            query = ""



    End Sub
    Private Sub set_changes()
        Dim query As String = String.Empty

        cmd.Connection = con

        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            ' log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("..\login.aspx")

        End If

        If session_user_role = "Audit" Then Exit Sub

        Dim d As String = get_due(txtacn.Text, 0)

        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        Dim prod = get_pro(Session("product"))

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", txtamt.Text)
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", txtamt.Text)
        cmd.Parameters.AddWithValue("@crc", 0)



        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "To Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnar.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnar.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
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


            MsgBox(ex.Message)


        End Try

        query = ""



        query = "SELECT MAX(ID) from dbo.trans where trans.acno='" + txtacn.Text + "'"
        cmd.CommandText = query

        countresult = cmd.ExecuteScalar()

        Session("tid") = Convert.ToString(countresult)


        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        ''Dim prod = get_pro(Session("product"))

        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", txtamt.Text)
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", txtamt.Text)
        cmd.Parameters.AddWithValue("@crc", 0)



        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "To Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnar.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnar.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
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


            MsgBox(ex.Message)


        End Try






        update_suplementry(Session("tid"), prod, txtamt.Text)

        If mop.SelectedItem.Text = "Account" Then
            update_sb()
        End If


        If mop.SelectedItem.Text = "Cash" Then


            set_changes_trans()

        End If


        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transaction Completed. ID # " + Session("tid")
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

        If mop.SelectedItem.Text = "Cash" Then
            clear_tab_recpt()
        Else
            prepare_print()
        End If


    End Sub

    Sub prepare_print()


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
            pcnar.Text = "By Transfer from " + txtacn.Text
        Else
            pcacno.Text = txtacn.Text
            pcglh.Text = bnk.SelectedItem.Text
            pccid.Text = get_memberno(txtacn.Text)
            pccname.Text = lblname.Text
            pcnar.Text = "By Transfer from " + txtacn.Text
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


    Sub set_changes_trans()

        Session("total_amt") = CDbl(txtamt.Text)

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
        actrans.acno = txtacn.Text
        actrans.drd = Session("splitamt")
        actrans.crd = 0
        actrans.drc = Session("splitamt")
        actrans.crc = 0
        actrans.narration = "To Cash"
        actrans.due = ""
        actrans.typ = "CASH"
        actrans.suplimentery = get_pro(Session("product"))
        actrans.sesusr = Session("sesusr")
        actrans.entryat = Convert.ToDateTime(Now)
        update_actrans(actrans)
        '   
        update_suplementry_c(Session("tid"), actrans.suplimentery, actrans.drc)




        Do Until Session("total_amt") = 0

            If Session("total_amt") < 18000 Then
                Session("splitamt") = Session("total_amt")
                Session("total_amt") = Session("total_amt") - Session("splitamt")

            Else
                Session("splitamt") = 18000
                Session("total_amt") = Session("total_amt") - Session("splitamt")

            End If
            If Session("product") = "KMK" Then
                Session("acn") = Get_Rnd_KMK(0, Session("splitamt"))

            Else
                Session("acn") = Get_Rnd_SB(0, Session("splitamt"))
            End If


            actrans.id = Session("tid")
            actrans.dt = CDate(tdate.Text)
            actrans.acno = Session("acn")
            actrans.drd = Session("splitamt")
            actrans.crd = 0
            actrans.drc = Session("splitamt")
            actrans.crc = 0
            actrans.narration = "To Cash"
            actrans.due = ""
            actrans.typ = "CASH"
            actrans.suplimentery = get_pro(Session("product"))
            actrans.sesusr = Session("sesusr")
            actrans.entryat = Convert.ToDateTime(Now)
            update_actrans(actrans)
            '   update_suplementry_c(Session("tid"), "RD DEFAULT INTEREST", txtpenalty.Text)
            update_suplementry_c(Session("tid"), actrans.suplimentery, actrans.drc)


        Loop


    End Sub
    Private Sub update_suplementry_c(ByVal tid As Double, ByVal ach As String, ByVal cr As Double)
        If tid = 0 Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        cmd.Connection = con

        If mop.SelectedItem.Text = "Cash" Then

            query = "INSERT INTO suplementc (date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@acn", Session("acn"))
            cmd.Parameters.AddWithValue("@nar", "To Cash")
            cmd.Parameters.AddWithValue("@typ", "RECEIPT")

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                Response.Write(ex.Message)


            End Try

        Else

            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@acn", Session("acn"))
            cmd.Parameters.AddWithValue("@nar", "By Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try

            cmd.Parameters.Clear()


            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
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
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)

            cmd.Parameters.AddWithValue("@nar", "To Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

        End If

        query = ""



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
                        cmd.Parameters.AddWithValue("@narration", "To Cash")
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
                    Response.Write(ex.Message)

                Finally
                    cmd.Dispose()
                    con.Close()
                End Try


            End Using

        End Using












    End Sub


    Private Sub btn_up_rcpt_Click(sender As Object, e As EventArgs) Handles btn_up_rcpt.Click

        'txtamt.ReadOnly = True
        btn_up_rcpt.Enabled = False


        set_changes()


    End Sub
    Sub clear_tab_recpt()
        Session("tid") = Nothing
        txtnar.Text = ""
        txtacn.Text = ""
        lblamt.Text = ""
        lblbal.Text = ""
        lblname.Text = ""
        lblproduct.Text = ""
        txtamt.Text = ""
        txtnar.Text = ""

        lbladd.Text = ""
        lblmobile.Text = ""
        pnlact.Visible = False

        ' pnlsbtrf.Visible = False
        lbl_sb_bal.Text = ""
        txt_sb.Text = ""
        'pnltran.Visible = False
        'TabContainer1.Visible = False
        txtacn.Enabled = True
        btn_up_rcpt.Enabled = True
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
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
    '        cmd.Parameters.AddWithValue("@narration", "To Cash")
    '        cmd.Parameters.AddWithValue("@due", d)
    '        cmd.Parameters.AddWithValue("@type", "CASH")


    '        Try
    '            cmd.ExecuteNonQuery()

    '            cmd.Parameters.Clear()


    '        Catch ex As Exception

    '            MsgBox(ex.Message)
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
        get_ac_info(txtacn.Text)
        txtfocus(txtamt)
    End Sub


    Private Sub btn_up_can_Click(sender As Object, e As EventArgs) Handles btn_up_can.Click
        clear_tab_recpt()
        btn_up_rcpt.Visible = True
        txtfocus(txtacn)
    End Sub

    Private Sub txtamt_TextChanged(sender As Object, e As EventArgs) Handles txtamt.TextChanged

        If CDbl(txtamt.Text) <= (CDbl(lblbal.Text) - 10) Then
            btn_up_rcpt.Enabled = True
            txtfocus(btn_up_rcpt)
        Else
            btn_up_rcpt.Enabled = False


            'sesuser.Text = Session("sesusr")
            'lblinfo.Text =
            'alertmsg.Visible = True

            Dim stitle = "Hi " + Session("sesusr")
            Dim msg = "Insufficient balance. Amount should be less than " + FormatCurrency((CDbl(lblbal.Text) - 10))
            Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


            txtfocus(txtamt)
        End If

    End Sub


    Private Sub mop_SelectedIndexChanged(sender As Object, e As EventArgs) Handles mop.SelectedIndexChanged
        mop_TextChanged(sender, e)

    End Sub

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

                bnk.Visible = True
                lbl.Visible = True

                txt_sb.Visible = False

                lblsb.Visible = False
                lbl_sb_bal.Text = ""

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally

            End Try

        ElseIf mop.SelectedItem.Text = "Account" Then

            bnk.Visible = False
            lbl.Visible = False
            txt_sb.Visible = True
            lblsb.Visible = True
            txtfocus(txt_sb)
        Else
            bnk.Visible = False
            lbl.Visible = False
            lblsb.Visible = False
            txt_sb.Visible = False
        End If
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
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@narration", "By Transfer")
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

            Response.Write(EX.ToString)

        Finally

            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()

        End Try


    End Sub

    Private Sub DepositPayment_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
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

    Private Sub txt_sb_TextChanged(sender As Object, e As EventArgs) Handles txt_sb.TextChanged
        Dim bal As Double = get_balance(Trim(txt_sb.Text))

        lbl_sb_bal.Text = FormatCurrency(bal)
        If bal < 0 Then
            lbl_sb_bal.CssClass = "col-sm-2 text-danger"
        Else
            lbl_sb_bal.CssClass = "col-sm-2 text-success"
        End If

    End Sub

    Private Sub btntog_Click(sender As Object, e As EventArgs) Handles btntog.Click
        clear_tab_recpt()
        pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
        pnltrans.Style.Add(HtmlTextWriterStyle.Display, "block")
        txtacn.Focus()

    End Sub
End Class