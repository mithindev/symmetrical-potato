Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Net.Configuration
Imports System.Net.Mail
Imports System.Net

Public Class LoanPayment
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Dim newrow As DataRow

    Dim countresult As Integer
    Public dt_dl As New DataTable
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim yearpart As String
    Dim sb_srch As Boolean
    Dim stitle As String
    Dim msg As String
    Dim fnc As String
    Dim oResult As String


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then
            'bind_grid()
            btn_up_rcpt.Enabled = False

            If Not Session("acn") Is Nothing Then
                txtacn.Text = Trim(CType(Session("acn"), String))
                tdate.Text = Date.Today

                get_ac_info(txtacn.Text)



            Else
                tdate.Text = Date.Today
                ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtacn.ClientID), True)

            End If





        End If

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
        Return Session("cbal")


    End Function
    Public Sub get_ac_info(ByVal acn As String)


        'closure_notice.Visible = False

        Dim sql As String = "SELECT Master.date,Master.acno,Master.product,Master.amount,Master.cint,Master.dint,Master.prd,Master.prdtype,Master.mdate,Master.mamt,Master.cid,master.alert FROM dbo.master WHERE master.acno = '" + acn + "'and cld='0'"

        If con.State = ConnectionState.Closed Then con.Open()

        Dim adapter As New SqlDataAdapter(sql, con)

        Try
            adapter.Fill(ds)

            If Not ds.Tables(0).Rows.Count = 0 Then

                Session("ac_date") = ds.Tables(0).Rows(0).Item(0)
                Session("acn") = ds.Tables(0).Rows(0).Item(1)
                Session("product") = Trim(ds.Tables(0).Rows(0).Item(2))
                Session("amt") = ds.Tables(0).Rows(0).Item(3)
                Session("cintr") = ds.Tables(0).Rows(0).Item(4)
                Session("dint") = ds.Tables(0).Rows(0).Item(5)
                Session("prd") = ds.Tables(0).Rows(0).Item(6)
                Session("prdtyp") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(7)), "", ds.Tables(0).Rows(0).Item(7))
                '   mdt = ds.Tables(0).Rows(0).Item(8)
                '   mamt = ds.Tables(0).Rows(0).Item(9)
                Session("ncid") = ds.Tables(0).Rows(0).Item(10)
                Session("alert") = ds.Tables(0).Rows(0).Item(11)
                txtacn.Enabled = False

                If Session("product") = "JL" Then
                    pnlsch.Visible = True
                    chk4cp()
                    get_scheme()
                End If

                If Session("amt") = 0 Then
                    btn_up_rcpt.Enabled = True
                    soa.Enabled = True
                Else
                    btn_up_rcpt.Enabled = False
                End If

                soa.Enabled = True


            Else
                'closure_notice.Visible = True
                'lblmat.Text = "Invalid Account or Already Closed"

            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con.Close()

        End Try


        sql = "SELECT FirstName,lastname,address,mobile from dbo.member where MemberNo='" + Trim(Session("ncid")) + "'"
        If con.State = ConnectionState.Closed Then con.Open()

        Dim adapter1 As New SqlDataAdapter(sql, con)

        Try

            adapter1.Fill(ds1)

            If Not ds1.Tables(0).Rows.Count = 0 Then
                Session("ac_name") = Trim(ds1.Tables(0).Rows(0).Item(0))
                Session("ac_lname") = ds1.Tables(0).Rows(0).Item(1)
                Session("address") = ds1.Tables(0).Rows(0).Item(2)
                Session("mobile") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(3)), " ", ds1.Tables(0).Rows(0).Item(3))

                '   custpict.ImageUrl = "~/ShowImage.ashx?id=" & Session("ncid")
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            con.Close()

            adapter1.Dispose()

        End Try

        lblname.Text = Session("ac_name")




        lblbal.Text = FormatCurrency(get_balance(acn))
        lblproduct.Text = Trim(Session("product"))
        lblname.Text = Session("ac_name")
        lblamt.Text = FormatCurrency(Session("amt"))
        'sessmdt = get_last_transaction(Session("acn"))
        'lbl_ad_mdt.Text = mdt

        'lbl_ad_mamt.Text = ""


        adapter.Dispose()




        ' ClientScript.RegisterStartupScript(Me.[GetType](), "SetFocus", "<script language=""Jscript"" > document.getElementById(""txtamt"").focus(); </Script>", True)
        '  ClientScript.RegisterStartupScript(Me.[GetType](), "Focus", "document.getElementById('<%=txtamt.ClientID()%>').focus();", True)
        ' ScriptManager1.SetFocus(txtamt)


        txtamt.Focus()


    End Sub

    Sub chk4cp()

        query = "select COALESCE(CP,0) from kyc where MemberNo='" + Session("ncid") + "'"

        If con1.State = ConnectionState.Closed Then con1.Open()
        cmdi.Connection = con1
        cmdi.CommandText = query

        Try

            Session("cp") = cmdi.ExecuteScalar


        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmdi.Dispose()

        End Try


    End Sub


    Sub get_scheme()

        Dim dr As SqlDataReader

        query = "SELECT roi.agst FROM dbo.roi WHERE roi.Product = 'JL' GROUP BY roi.agst"

        If con1.State = ConnectionState.Closed Then con1.Open()

        cmdi.Connection = con1
        cmdi.CommandText = query

        Try

            dr = cmdi.ExecuteReader()

            If dr.HasRows Then
                Do While dr.Read()


                    If Trim(dr(0)) = "PRIME ULTRA" Then

                        If Session("cp") = 1 Then
                            schlst.Items.Add(Trim(dr(0)))
                        End If

                    Else
                        schlst.Items.Add(Trim(dr(0)))
                    End If

                Loop


            End If

            schlst.Items.Insert(0, "<-- Select -->")
            schlst.Items.Item(0).Value = ""
            dr.Close()


        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()

            con.Close()
        End Try

        If Session("cp") = 1 Then
            schlst.Items.Clear()
            schlst.Items.Add("PRIME ULTRA")
        Else
            schlst.SelectedIndex = 2
        End If

        Select Case Trim(schlst.SelectedItem.Text)
            Case "PRIME"
                lblcard.Text = "Issue Green Card"
                pnlgreen.Visible = True
                pnlBlue.Visible = False
                pnlpink.Visible = False
                pnlwhite.Visible = False

            Case "PRIME SPECIAL"
                lblcard.Text = "Issue Blue Card"
                pnlgreen.Visible = False
                pnlBlue.Visible = True
                pnlpink.Visible = False
                pnlwhite.Visible = False

            Case "PRIME PLUS"
                lblcard.Text = "Issue Pink Card"
                pnlgreen.Visible = False
                pnlBlue.Visible = False
                pnlpink.Visible = True
                pnlwhite.Visible = False

            Case "PRIME ULTRA"
                lblcard.Text = "Issue White Card"
                pnlgreen.Visible = False
                pnlBlue.Visible = False
                pnlpink.Visible = False
                pnlwhite.Visible = True


        End Select

        get_int()
        lblroi.Text = "@ " + CStr(Session("dint")) + "%"



    End Sub


    Sub add_table(ByVal acnx As String)


        Dim dtdl As New DataTable
        query = "select date,amount from dbo.master where acno='" + acnx + "'"
        If con1.State = ConnectionState.Closed Then con1.Open()

        cmdi.Connection = con1
        cmdi.CommandText = query

        Dim rds As SqlDataReader

        Try
            rds = cmdi.ExecuteReader()

            If rds.HasRows() Then
                rds.Read()

                Session("ac_date") = rds(0)
                Session("amt") = Format(rds(1), "0.00")



            End If


        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            con1.Close()
            cmdi.Dispose()
        End Try

        Dim oresult As Double = 0

        If con1.State = ConnectionState.Closed Then con1.Open()

        cmdi.Connection = con1

        query = "SELECT TOP 1 actrans.cbal FROM dbo.actrans WHERE actrans.acno ='" + txtacn.Text + "' ORDER BY actrans.tid DESC"
        cmdi.CommandText = query

        Try


            oresult = cmdi.ExecuteScalar()

            If Not oresult = 0 Then
                Session("cbal") = Format(oresult, "0.00")
            Else
                Session("cbal") = "0.00"
            End If




        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            con1.Close()
            cmdi.Dispose()

        End Try


        Dim dt_j As New DataTable

        If dt_j.Columns.Count = 0 Then
            dt_j.Columns.Add("acn", GetType(String))
            dt_j.Columns.Add("acdate", GetType(String))
            dt_j.Columns.Add("depamt", GetType(Decimal))
            dt_j.Columns.Add("curbal", GetType(Decimal))

        End If

        If dtdl.Columns.Count = 0 Then
            dtdl.Columns.Add("acn", GetType(String))
            dtdl.Columns.Add("acdate", GetType(String))
            dtdl.Columns.Add("depamt", GetType(Decimal))
            dtdl.Columns.Add("curbal", GetType(Decimal))

        End If



        If Session("dspec") Is Nothing = False Then
            dt_j = CType(Session("dspec"), DataTable)

            Dim count As Integer = dt_j.Rows.Count

            For Each row1 As DataRow In dt_j.Rows

                dtdl.ImportRow(row1)
            Next

        End If

        newrow = dt_j.NewRow

        newrow(0) = acnx
        newrow(1) = Session("ac_date").ToShortDateString  'DateTime.Parse(String.Format("{0}:dd-mm-yyyy", ac_date))
        newrow(2) = Session("amt")
        newrow(3) = Session("cbal")

        If Session("cum_bal") Is Nothing Then
            lblamt.Text = CDbl(Session("cbal"))
        Else
            lblamt.Text = CType(Session("cum_bal"), Double) + CDbl(Session("cbal"))
        End If


        dt_j.Rows.Add(newrow)
        dtdl.ImportRow(newrow)

        ' dispdl.DataSource = dtdl
        ' dispdl.DataBind()
        Session("dspec") = dt_j
        Session("cum_bal") = CDbl(lblamt.Text)


        'lblamt.Text = Format(tqty, "0.00")
        'lbltqty.Text = tqtyvi
        'lblgross.Text = Format(tgross, "##,##0.000") 'String.Format("{0:#,###.###}", tgross)
        'lblnet.Text = Format(tnet, "##,##0.000") 'String.Format("{0:#,###.###}", tnet)
        'lblval.Text = String.Format("{0:N}", (tnet * ratepergm))

        Session("ac_date") = Nothing
        Session("amt") = 0
        Session("cbal") = 0


    End Sub




    Private Sub txtacn_TextChanged(sender As Object, e As EventArgs) Handles txtacn.TextChanged

        If Not Trim(txtacn.Text) = "" Then
            get_ac_info(txtacn.Text)
            txtfocus(txtamt)
        End If

        ' txtfocus(tdate)

    End Sub

    Function get_pro(ByVal prd As String)

        If con.State = ConnectionState.Closed Then con.Open()




        Dim prdname As String
        Dim query As String = String.Empty
        cmd.Connection = con
        query = "SELECT name from products where products.shortname='" + prd + "'"
        cmd.CommandText = query

        prdname = cmd.ExecuteScalar()


        Return prdname
    End Function

    'Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double)
    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)
        If tid = 0 Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()

        If mop.SelectedItem.Text = "Cash" Then
            cmd.Connection = con
            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sudt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@usacn", txtacn.Text)
            cmd.Parameters.AddWithValue("@nar", nar)
            cmd.Parameters.AddWithValue("@typ", typ)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                Response.Write(ex.ToString)

            Finally
                cmd.Dispose()

                con.Close()



            End Try
        Else

            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
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

            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", dr)
            '    cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@nar", "By Transfer ")
            Else
                cmd.Parameters.AddWithValue("@nar", "By Transfer " + txt_sb.Text + " (" + Trim(txtacn.Text) + ")")

            End If

            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try




        End If
        query = ""


        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()

        If mop.SelectedItem.Text = "Cash" Then
            cmd.Connection = con
            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sudt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@usacn", txtacn.Text)
            cmd.Parameters.AddWithValue("@nar", nar)
            cmd.Parameters.AddWithValue("@typ", typ)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                Response.Write(ex.ToString)

            Finally
                cmd.Dispose()

                con.Close()



            End Try
        Else

            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
            'cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            Else
                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
            End If
            cmd.Parameters.AddWithValue("@nar", "To Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            cmd.Parameters.Clear()


            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
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
            '    cmd.Parameters.AddWithValue("@acn", txtacn.Text)
            cmd.Parameters.AddWithValue("@nar", "By Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try




        End If
        query = ""

    End Sub
    Private Sub update_int(ByVal tid As Double, ByVal drd As Double, ByVal crd As Double, ByVal drc As Double, ByVal crc As Double, ByVal nar As String, ByVal supliment As String, ByVal cbal As Double, ByVal dbal As Double, ByVal typ As String)

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        query = "INSERT INTO actrans(Id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@uiid", tid)
        cmd.Parameters.AddWithValue("@uidate", Session("ac_date"))
        cmd.Parameters.AddWithValue("@uiacno", txtacn.Text)
        cmd.Parameters.AddWithValue("@uidrd", drd)
        cmd.Parameters.AddWithValue("@uicrd", crd)
        cmd.Parameters.AddWithValue("@uidrc", drc)
        cmd.Parameters.AddWithValue("@uicrc", crc)
        cmd.Parameters.AddWithValue("@uinarration", nar)
        cmd.Parameters.AddWithValue("@uidue", " ")
        cmd.Parameters.AddWithValue("@uitype", typ)
        cmd.Parameters.AddWithValue("@uisuplimentry", supliment)
        cmd.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@uicbal", cbal)
        cmd.Parameters.AddWithValue("@uidbal", dbal)
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)


        End Try


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        query = "INSERT INTO actransc(Id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@uiid", tid)
        cmd.Parameters.AddWithValue("@uidate", Session("ac_date"))
        cmd.Parameters.AddWithValue("@uiacno", txtacn.Text)
        cmd.Parameters.AddWithValue("@uidrd", drd)
        cmd.Parameters.AddWithValue("@uicrd", crd)
        cmd.Parameters.AddWithValue("@uidrc", drc)
        cmd.Parameters.AddWithValue("@uicrc", crc)
        cmd.Parameters.AddWithValue("@uinarration", nar)
        cmd.Parameters.AddWithValue("@uidue", " ")
        cmd.Parameters.AddWithValue("@uitype", typ)
        cmd.Parameters.AddWithValue("@uisuplimentry", supliment)
        cmd.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@uicbal", cbal)
        cmd.Parameters.AddWithValue("@uidbal", dbal)
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)


        End Try


    End Sub

    Sub update_service_charges()

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()

        query = "insert into diff(tid,date,product,acno,dr,cr)"
        query &= "values(@tid,@date,@product,@acmo,@dr,@cr)"


        cmd.Connection = con
        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@product", lblproduct.Text)
        cmd.Parameters.AddWithValue("@acmo", txtacn.Text)
        cmd.Parameters.AddWithValue("@dr", 0)
        cmd.Parameters.AddWithValue("@cr", CDbl(txtsc.Text))

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try

    End Sub


    Sub get_int()

        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con

        Dim dr_roi As SqlDataReader


        query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.agst=@sch AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"


        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()
        cmd.Connection = con


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@prod", "JL")
        cmd.Parameters.AddWithValue("@sch", schlst.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@prdtyp", "D")

        cmd.Parameters.AddWithValue("@prdx", 10)
        cmd.Parameters.AddWithValue("@prdy", 10)






        dr_roi = cmd.ExecuteReader()

        If dr_roi.HasRows() Then
            '    dr_roi.Read()

            While dr_roi.Read

                Dim FYFRM As Date = dr_roi(2)
                Dim FYTO As Date = dr_roi(3)


                Dim x As Long = FYFRM.CompareTo(Date.Today)

                If x = -1 Then


                    Dim y As Long = FYTO.CompareTo(Date.Today)

                    If y = 1 Then
                        Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                        Session("dint") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))



                        Exit While
                    Else

                        Session("cintr") = 0 'IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                        Session("dint") = 0 'IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))


                    End If

                End If
            End While


        End If


        dr_roi.Close()




    End Sub

    Private Sub set_changes()


        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            '   log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("..\login.aspx")

        End If

        If session_user_role = "Audit" Then Exit Sub


        Dim query As String = String.Empty

        cmd.Connection = con


        ' Dim d As String = get_due(txtacn.Text, 0)

        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        Dim prod = get_pro(Session("product"))
        cmd.Parameters.Clear()


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", txtamt.Text)
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", txtamt.Text)
        cmd.Parameters.AddWithValue("@crc", 0)

        'cmd.Parameters.AddWithValue("@narration", "TO CASH")
        'cmd.Parameters.AddWithValue("@due", " ")
        'cmd.Parameters.AddWithValue("@type", "CASH")

        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "To Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text) = "" Then
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
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()


        End Try



        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()

        If lblproduct.Text = "JL" Then

            get_int()
            cmd.Parameters.Clear()
            query = "UPDATE master SET amount = @amt,cint=@cint,dint=@dint,sch=@sch where (master.acno= @acn )"
            cmd.Parameters.AddWithValue("@amt", txtamt.Text)
            cmd.Parameters.AddWithValue("@cint", Session("cintr"))
            cmd.Parameters.AddWithValue("@dint", Session("dint"))
            cmd.Parameters.AddWithValue("@sch", schlst.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)

        Else
            cmd.Parameters.Clear()
            query = "UPDATE master SET amount = @amt where (master.acno= @acn )"
            cmd.Parameters.AddWithValue("@amt", txtamt.Text)
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)
        End If

        cmd.CommandText = query
        cmd.Connection = con

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try


        If lblproduct.Text = "JL" Then

            get_int()
            cmd.Parameters.Clear()
            query = "UPDATE masterc SET amount = @amt,cint=@cint,dint=@dint,sch=@sch where (masterc.acno= @acn )"
            cmd.Parameters.AddWithValue("@amt", txtamt.Text)
            cmd.Parameters.AddWithValue("@cint", Session("cintr"))
            cmd.Parameters.AddWithValue("@dint", Session("dint"))
            cmd.Parameters.AddWithValue("@sch", schlst.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)

        Else
            cmd.Parameters.Clear()
            query = "UPDATE masterc SET amount = @amt where (masterc.acno= @acn )"
            cmd.Parameters.AddWithValue("@amt", txtamt.Text)
            cmd.Parameters.AddWithValue("@acn", txtacn.Text)
        End If
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.CommandText = query
        cmd.Connection = con

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try



        query = ""


        If con.State = ConnectionState.Closed Then con.Open()



        query = "SELECT MAX(ID) from dbo.trans where trans.acno='" + txtacn.Text + "'"
        cmd.CommandText = query
        cmd.Connection = con

        Try
            countresult = cmd.ExecuteScalar()
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try

        Session("tid") = Convert.ToString(countresult)
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con


        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= " VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        ''Dim prod = get_pro(Session("product"))
        cmd.Parameters.Clear()


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", txtamt.Text)
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", txtamt.Text)
        cmd.Parameters.AddWithValue("@crc", 0)

        'cmd.Parameters.AddWithValue("@narration", "TO CASH")
        'cmd.Parameters.AddWithValue("@due", " ")
        'cmd.Parameters.AddWithValue("@type", "CASH")

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
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()


        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con


        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= " VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        ''Dim prod = get_pro(Session("product"))
        cmd.Parameters.Clear()


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", txtamt.Text)
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", txtamt.Text)
        cmd.Parameters.AddWithValue("@crc", 0)

        'cmd.Parameters.AddWithValue("@narration", "TO CASH")
        'cmd.Parameters.AddWithValue("@due", " ")
        'cmd.Parameters.AddWithValue("@type", "CASH")

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
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()


        End Try




        update_suplementry(Session("tid"), prod, 0, txtamt.Text, "To Cash", "PAYMENT")


        If mop.SelectedItem.Text = "Account" Then
            update_sb()
        End If


        If lblproduct.Text = "DCL" Then update_dcl()

        If Not CDbl(txtsc.Text) = 0 Then
            update_service_charges()
        End If






        stitle = "Hi " + Session("sesusr")
        msg = "Payment Completed. Transaction ID # " + Session("tid")
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
    Sub update_sb()
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        cmd.Connection = con
        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"
        cmd.Parameters.Clear()
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


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        cmd.Connection = con
        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"
        cmd.Parameters.Clear()
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
    Sub update_dcl()

        If con.State = ConnectionState.Closed Then con.Open()

        Session("totint") = CDbl(txtamt.Text) * 10 / 100

        Session("totalint_d") = Math.Round((CDbl(txtamt.Text) * CDbl(Session("cintr")) / 100) / 365 * 120)

        Session("cintr") = Session("totalint_d")
        Session("dint") = Session("totint") - Session("totalint_d")

        update_int(Session("tid"), Session("dint"), 0, Session("cintr"), 0, "TO INTEREST", "DAILY COLLECTION LOAN", -(CDbl(txtamt.Text) + CDbl(Session("cintr"))), -(CDbl(Session("dint")) + CDbl(txtamt.Text)), "INTR")
        update_int(Session("tid"), 0, Session("dint"), 0, Session("cintr"), "By Cash", "DAILY COLLECTION LOAN", 0, 0, "CASH")

        update_suplementry(Session("tid"), "DAILY COLLECTION LOAN", 0, Session("cintr"), "TO INTEREST", "JOURNAL")
        update_suplementry(Session("tid"), "DCL INTEREST", Session("cintr"), 0, "BY INTEREST", "JOURNAL")
        update_suplementry(Session("tid"), "DAILY COLLECTION LOAN", Session("cintr"), 0, "By Cash", "CASH")
        set_diff(Session("dint"))


        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        Dim prod = get_pro(Session("product"))
        cmd.Parameters.Clear()


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", Session("dint"))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", Session("cintr"))
        cmd.Parameters.AddWithValue("@narration", "By Cash")
        cmd.Parameters.AddWithValue("@due", " ")
        cmd.Parameters.AddWithValue("@type", "CASH")
        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()


        End Try

    End Sub

    Sub set_diff(ByVal ovr As Double)
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.Parameters.Clear()
        query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
        query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@product", lblproduct.Text)
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

    Private Sub btn_up_rcpt_Click(sender As Object, e As EventArgs) Handles btn_up_rcpt.Click

        If Trim(txtacn.Text) = "" Then Exit Sub
        If lblproduct.Text = "JL" Then
            If schlst.SelectedItem.Text = "PRIME SPECIAL" Then
                If txtotp.Text = "" Then
                    pnlotp.Visible = True
                    schlst.Enabled = False
                    txtamt.Enabled = False
                    txtsc.Enabled = False
                    mop.Enabled = False
                    sendotp()
                    Return
                ElseIf Not txtotp.Text.Equals(Session("otp")) Then
                    Return
                End If
            End If
        End If

        btn_up_rcpt.Enabled = False

        If CInt(lblamt.Text) = 0 Then

            If txtamt.Text = "" Then Exit Sub
            If txtsc.Text = "" Then Exit Sub


            set_changes()

        End If

    End Sub

    Protected Sub sendotp()

        Dim subject As String = "OTP for Prime Special Loan"
        Dim recepientEmail As String = ConfigurationManager.AppSettings("recepientEmailid1")
        Dim body As String = populate_body()
        Dim SmtpSection As SmtpSection = CType(ConfigurationManager.GetSection("system.net/mailSettings/smtp"), SmtpSection)
        Using mm As MailMessage = New MailMessage(SmtpSection.From, recepientEmail)
            mm.Subject = subject
            mm.Body = body
            mm.IsBodyHtml = True
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
            Dim smtp As SmtpClient = New SmtpClient
            smtp.Host = SmtpSection.Network.Host
            smtp.EnableSsl = SmtpSection.Network.EnableSsl
            Dim networkCred As NetworkCredential = New NetworkCredential(SmtpSection.Network.UserName, SmtpSection.Network.Password)
            smtp.UseDefaultCredentials = SmtpSection.Network.DefaultCredentials
            smtp.Credentials = networkCred
            smtp.Port = SmtpSection.Network.Port
            smtp.Send(mm)
        End Using


    End Sub

    Private Function populate_body()

        query = "select branch from dbo.branch "
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con

        cmd.CommandType = CommandType.Text
        cmd.CommandText = query

        Try
            oResult = cmd.ExecuteScalar()

        Catch e As Exception

            MsgBox(e.ToString)
        End Try

        Session("branch") = oResult + " Branch"


        Dim charArr As Char() = "0123456789".ToCharArray()
        Dim strrandom As String = String.Empty
        Dim objran As New Random()
        Dim noofcharacters As Integer = 6 'Convert.ToInt32(txtCharacters.Text)
        For i As Integer = 0 To noofcharacters - 1
            'It will not allow Repetation of Characters
            Dim pos As Integer = objran.[Next](1, charArr.Length)
            If Not strrandom.Contains(charArr.GetValue(pos).ToString()) Then
                strrandom += charArr.GetValue(pos)
            Else
                i -= 1
            End If
        Next
        Session("otp") = strrandom

        Dim body As String = String.Empty
        Dim reader As StreamReader = New StreamReader(Server.MapPath("JewelLoan.html"))
        body = reader.ReadToEnd
        body = body.Replace("{branch}", Session("branch"))
        body = body.Replace("{otp}", strrandom)
        body = body.Replace("{lbldt}", DateTime.Today)
        body = body.Replace("{lblentryby}", Session("sesusr"))
        body = body.Replace("{lblacn}", txtacn.Text)
        body = body.Replace("{lblname}", lblname.Text)
        body = body.Replace("{lblamt}", txtamt.Text)
        Return body

    End Function

    Sub clear_tab_recpt()
        Session("tid") = Nothing
        lblcard.Text = ""
        pnlgreen.Visible = False

        pnlpink.Visible = False
        pnlwhite.Visible = False
        pnlsbtrf.Visible = False
        pnltran.Visible = False
        pnlsch.Visible = False
        txt_sb.Text = ""
        txtacn.Text = ""
        lblamt.Text = ""
        lblbal.Text = ""
        lblname.Text = ""
        lblproduct.Text = ""
        txtamt.Text = ""
        mop.SelectedIndex = 0
        txtacn.Enabled = True
        txtfocus(txtacn)
        sch.Visible = True
        schlst.DataSource = Nothing
        schlst.DataBind()
        Session("acn") = Nothing
        btn_up_rcpt.Enabled = True
        pnlotp.Visible = False
        schlst.Enabled = True
        txtamt.Enabled = True
        txtsc.Enabled = True
        mop.Enabled = True
        txtotp.Text = ""

        schlst.Items.Clear()
        '
        txtacn.Focus()

    End Sub


    Private Sub tdate_TextChanged(sender As Object, e As EventArgs) Handles tdate.TextChanged
        If Not Trim(txtacn.Text) = "" Then
            get_ac_info(txtacn.Text)
            txtfocus(txtamt)
        End If

    End Sub

    Private Sub txtamt_TextChanged(sender As Object, e As EventArgs) Handles txtamt.TextChanged

        If lblproduct.Text = "JL" Then

            If Not schlst.SelectedItem.Text = "PRIME" And Not schlst.SelectedItem.Text = "PRIME SPECIAL" Then

                chk_sch_limit()
            Else
                chk_limit()

            End If


        Else

            get_service_charge()

        End If


    End Sub

    Sub chk_sch_limit()

        Dim availed As Double = 0
        Dim loan_limit As Double = 0
        Dim parentid As String
        Dim available_limit As Double = 0
        If con.State = ConnectionState.Closed Then con.Open()


        query = "SELECT COALESCE(KYC.Parent,'N') FROM dbo.kyc where kyc.MemberNo=@cid"



        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@cid", Trim(Session("ncid")))
        cmd.Connection = con
        Try

            parentid = cmd.ExecuteScalar()

            'If IsDBNull(parentid) Then parentid = Trim(lbl_ad_acid.Text)
            If Trim(parentid) = "N" Then parentid = Trim(Session("ncid"))

            If Not parentid = "N" Then

                'cmd.Dispose()

                'query = "SELECT master.cid, master.product,  master.acno FROM dbo.master "
                'query &= " WHERE master.cid = @gid AND master.product = 'RD' AND master.cld = 0"

                query = "SELECT KYC.MemberNo, master.product,  master.acno FROM dbo.KYC INNER JOIN dbo.master   ON KYC.MemberNo = master.cid "
                query &= " WHERE KYC.Parent = @gid AND master.product = 'RD' AND master.cld = 0"


                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@gid", parentid)

                Dim dr As SqlDataReader

                dr = cmd.ExecuteReader()

                If dr.HasRows() Then

                    query = "Select top 1 loanlimitwrd from dbo.goldrate ORDER BY goldrate.date DESC"
                Else
                    query = "Select top 1 loanlimit from dbo.goldrate ORDER BY goldrate.date DESC"

                End If
                dr.Close()

            Else
                query = "Select top 1 loanlimit from dbo.goldrate ORDER BY goldrate.date DESC"
            End If


            If schlst.SelectedItem.Text = "PRIME ULTRA" Then
                query = "Select top 1 ultralimit from dbo.goldrate ORDER BY goldrate.date DESC"
            End If

            cmd.Dispose()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


        Try
            cmd.Connection = con
            cmd.CommandText = query
            loan_limit = cmd.ExecuteScalar()

            If Not IsDBNull(loan_limit) Then
                Session("loan_limit") = loan_limit
            End If

            cmd.Parameters.Clear()

            query = " SELECT COALESCE(SUM(actrans.Drd)-SUM(actrans.Crd),0 ) AS BAL FROM dbo.master INNER JOIN dbo.actrans  ON master.acno = actrans.acno "
            query += " WHERE master.cid = @cid AND master.cld = 0 AND master.sch = @sch and master.product= 'JL' "

            cmd.CommandText = query

            'SELECT COALESCE(SUM(master.amount ),0) AS expr1 FROM dbo.master WHERE master.cid = @cid AND master.cld = 0 AND master.sch = @sch"
            cmd.Parameters.AddWithValue("@cid", Trim(Session("ncid")))
            cmd.Parameters.AddWithValue("@sch", Trim(schlst.SelectedItem.Text))

            availed = cmd.ExecuteScalar()


            available_limit = loan_limit - availed

            If available_limit < CDbl(txtamt.Text) Then
                schlst.Items.Clear()
                schlst.Items.Add("Select")
                schlst.Items.Add("PRIME")
                schlst.Items.Add("PRIME SPECIAL")
                schlst.SelectedIndex = 1
                schlst_TextChanged(schlst, EventArgs.Empty)
                txtotp.Text = ""



                stitle = "Hi " + Session("sesusr")

                msg = "Limit Exceeded to PRIME PLUS. Scheme changed to <b>PRIME</b>"
                Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
                txtamt.Text = ""
                txtamt.Focus()

            Else
                chk_limit()


            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try
    End Sub
    Sub chk_limit()
        Dim rate As Double
        Dim drx As SqlDataReader

        '  lbl_loan_limit.Visible = False
        If con.State = ConnectionState.Closed Then con.Open()

        query = "Select top 1 rate,ultra from dbo.goldrate ORDER BY goldrate.rate DESC"

        cmd.Connection = con
        cmd.CommandText = query

        Try


            drx = cmd.ExecuteReader()

            If drx.HasRows() Then
                drx.Read()
                If schlst.SelectedItem.Text = "PRIME ULTRA" Then
                    Session("ratepergm") = drx(1)
                Else
                    Session("ratepergm") = drx(0)
                End If



            End If


            drx.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try


        If con.State = ConnectionState.Closed Then con.Open()

        query = "Select tnet from dbo.jlstock where jlstock.acn='" + txtacn.Text + "'"

        cmd.Connection = con
        cmd.CommandText = query

        Try

            rate = cmd.ExecuteScalar()

            If Not rate = 0 Then
                Session("ratepergm") = rate * Session("ratepergm")
            Else
                rate = 0
                Session("ratepergm") = rate * Session("ratepergm")
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try


        If CDbl(txtamt.Text) <= Session("ratepergm") Then

            get_service_charge()


        Else

            stitle = "Hi " + Session("sesusr")

            msg = "Loan Amount Must be Less than <b>" + String.Format("{0:C}", Session("ratepergm")) + "</b>"
            Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
            txtamt.Text = ""
            txtamt.Focus()


        End If




    End Sub
    Sub get_service_charge()

        Dim oresult As Double = 0
        Dim alert As Double = 0
        Dim ty = 0

        If con.State = ConnectionState.Closed Then con.Open()

        If Session("cp") = 1 Then ty = 1



        query = "SELECT service.srvchr FROM dbo.service WHERE service.lon = @lon AND service.frm <= @frm AND service.[to] >= @to AND type=@type"

        cmd.Connection = con
        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@lon", lblproduct.Text)
        cmd.Parameters.AddWithValue("@frm", txtamt.Text)
        cmd.Parameters.AddWithValue("@to", txtamt.Text)
        cmd.Parameters.AddWithValue("@type", ty)

        oresult = cmd.ExecuteScalar()

        If IsDBNull(oresult) Then oresult = 0


        If Session("alert") = True Then alert = 1



        txtsc.Text = oresult + alert


    End Sub

    Private Sub btn_up_can_Click(sender As Object, e As EventArgs) Handles btn_up_can.Click
        clear_tab_recpt()
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
                pnltran.Visible = True
                bnk.Visible = True
                lbl.Visible = True
                pnlsbtrf.Visible = False
                txt_sb.Visible = False
                'btn_trf.Visible = False
                lblsb.Visible = False

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally

            End Try

        ElseIf mop.SelectedItem.Text = "Account" Then
            pnltran.Visible = False
            bnk.Visible = False
            lbl.Visible = False
            pnlsbtrf.Visible = True
            txt_sb.Visible = True
            '   btn_trf.Visible = True
            lblsb.Visible = True
            txt_sb.Focus()

        Else
            pnltran.Visible = False
            bnk.Visible = False
            lbl.Visible = False
            lblsb.Visible = False
            pnlsbtrf.Visible = False
            txt_sb.Visible = False
            ' btn_trf.Visible = False
        End If
    End Sub

    Private Sub LoanPayment_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub schlst_SelectedIndexChanged(sender As Object, e As EventArgs) Handles schlst.SelectedIndexChanged


    End Sub

    Private Sub schlst_TextChanged(sender As Object, e As EventArgs) Handles schlst.TextChanged
        Select Case Trim(schlst.SelectedItem.Text)
            Case "PRIME"
                lblcard.Text = "Issue Green Card"
                pnlgreen.Visible = True
                pnlBlue.Visible = False
                pnlpink.Visible = False
                pnlwhite.Visible = False

            Case "PRIME SPECIAL"
                lblcard.Text = "Issue Blue Card"
                pnlgreen.Visible = False
                pnlBlue.Visible = True
                pnlpink.Visible = False
                pnlwhite.Visible = False

            Case "PRIME PLUS"
                lblcard.Text = "Issue Pink Card"
                pnlgreen.Visible = False
                pnlBlue.Visible = False
                pnlpink.Visible = True
                pnlwhite.Visible = False

            Case "PRIME ULTRA"
                lblcard.Text = "Issue White Card"
                pnlgreen.Visible = False
                pnlBlue.Visible = False
                pnlpink.Visible = False
                pnlwhite.Visible = True
        End Select


        get_int()
        lblroi.Text = "@ " + CStr(Session("dint")) + "%"


    End Sub

    Private Sub soa_Click(sender As Object, e As EventArgs) Handles soa.Click
        If Not Trim(txtacn.Text) = "" Then



            Response.Redirect("soaloan.aspx?acno=" + Trim(txtacn.Text))

        End If
    End Sub

    Private Sub btntog_Click(sender As Object, e As EventArgs) Handles btntog.Click

        pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
        pnltrans.Style.Add(HtmlTextWriterStyle.Display, "block")
        txtacn.Focus()


    End Sub
End Class