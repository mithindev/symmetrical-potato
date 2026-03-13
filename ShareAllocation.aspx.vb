Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Text.RegularExpressions


Public Class ShareAllocation

    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdc As New SqlClient.SqlCommand
    Dim msg As String = String.Empty
    Dim stitle As String = String.Empty

    Dim query As String
    Dim imgfn As String
    Dim oResult As String
    Dim ncid As String
    Dim serial As Integer

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()




        If Not Page.IsPostBack Then
            get_certificate()
            txtcno.Text = " Certificate No : "
            txtcno.Text = Session("share_certificate_no").ToString
            'ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtcid.ClientID), True)
            txtcid.Focus()

            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        End If

    End Sub

    Private Sub update_suplementryex(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)
        cmd.Connection = con
        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query

        cmd.Parameters.Clear()


        cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtadate.Text))
        cmd.Parameters.AddWithValue("@transid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        cmd.Parameters.AddWithValue("@nar", nar)
        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)


        End Try

        query = ""

        cmd.Connection = con
        query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query

        cmd.Parameters.Clear()


        cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtadate.Text))
        cmd.Parameters.AddWithValue("@transid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        cmd.Parameters.AddWithValue("@nar", nar)
        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)

        End Try

        query = ""


    End Sub

    Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged
        fetch_cust()
        txtadate.Text = Format(Now, "dd-MM-yyyy")
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtadate)


    End Sub
    Sub fetch_cust()
        Dim ds As New DataSet
        'Dim i As Integer

        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "select FirstName,LastName,Address from member where MemberNo = '" + Trim(txtcid.Text) + "'"
        Dim adapter As New SqlDataAdapter(sql, con)

        Try

            adapter.Fill(ds)

            If Not ds.Tables(0).Rows.Count = 0 Then

                txtname.Text = Trim(ds.Tables(0).Rows(0).ItemArray(0).ToString)
                'lbllname.Text = Trim(ds.Tables(0).Rows(0).ItemArray(1).ToString)
                txtadd.Text = Trim(ds.Tables(0).Rows(0).ItemArray(2).ToString)


            Else
                txtfocus(txtcid)
                Exit Sub
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally

            con.Close()

        End Try

    End Sub

    Sub set_changes_transfer()


        If session_user_role = "Audit" Then Exit Sub


        If con.State = ConnectionState.Closed Then con.Open()
        'Dim countresult As Integer

        query = "SELECT TOP 1 share.sl, share.shrto FROM share ORDER BY share.sl DESC"
        cmd.CommandText = query
        cmd.Connection = con

        Dim dr As SqlDataReader

        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                dr.Read()

                Session("share_certificate_no") = IIf(IsDBNull(dr(0)), 1, dr(0)) + 1
                '  share_from = IIf(IsDBNull(dr(1)), 1, dr(1)) + 1
                '  share_to = IIf(IsDBNull(dr(1)), 1, dr(1)) + CDbl(txtnos.Text)
            Else
                Session("share_certificate_no") = 1
                ' share_from = 1
                ' share_to = CDbl(txtnos.Text)
            End If

            dr.Close()

        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()


        End Try



        If CDbl(txtnos.Text) < Session("total_shr") Then

            Session("share_from") = Session("share_to_4m") - CDbl(txtnos.Text) + 1
            Session("share_to") = Session("share_to_4m")
            Session("share_to_4m") = Session("share_from") - 1

            If con.State = ConnectionState.Closed Then con.Open()


            query = "UPDATE share SET shrfrm=@snof,shrto=@snot,allocation=@nos,shrval=@sval where (share.sl= @sl)"

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@sl", Session("share_certificate_no_4m"))
            cmd.Parameters.AddWithValue("@snof", Session("share_from_4m"))
            cmd.Parameters.AddWithValue("@snot", Session("share_to_4m"))
            cmd.Parameters.AddWithValue("@nos", (Session("total_shr") - (CDbl(txtnos.Text))))
            cmd.Parameters.AddWithValue("@sval", ((Session("total_shr") - (CDbl(txtnos.Text))) * 10))

            cmd.Connection = con
            cmd.CommandText = query

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)

            Finally
                cmd.Dispose()
                con.Close()

            End Try

            If con.State = ConnectionState.Closed Then con.Open()



            query = "insert into share(date,sl,shrfrm,shrto,cid,allocation,pershr,shrval,trffrm,trfcid)"
            query &= "values(@date,@sl,@shrfrm,@shrto,@cid,@allocation,@pershr,@shrval,@trffrm,@trfcid)"

            cmd.CommandText = query
            cmd.Connection = con
            cmd.Parameters.Clear()

            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@sl", Session("share_certificate_no"))
            cmd.Parameters.AddWithValue("@shrfrm", Session("share_from"))
            cmd.Parameters.AddWithValue("@shrto", Session("share_to"))
            cmd.Parameters.AddWithValue("@cid", txtcid.Text)
            cmd.Parameters.AddWithValue("@allocation", CDbl(txtnos.Text))
            cmd.Parameters.AddWithValue("@pershr", CDbl(txtshrval.Text))
            cmd.Parameters.AddWithValue("@shrval", CDbl(txtval.Text))

            cmd.Parameters.AddWithValue("@trffrm", Session("share_certificate_no_4m"))
            cmd.Parameters.AddWithValue("@trfcid", txtshrcert.Text)


            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                cmd.Dispose()
                con.Close()

            End Try


            msg = "Share Transfered from : " + Convert.ToString(Session("share_certificate_no_4m")) + "Certificate No : " + Convert.ToString(Session("share_certificate_no"))
            stitle = "hi " + Session("sesusr")
            Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

            clear_rcpt()

        Else

            stitle = "Hi  " + Session("sesusr")
            msg = "No of Share Must be Greater than " + Convert.ToString(txtnos.Text)
            Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

            txtfocus(txtshrcert)

        End If





    End Sub



    Sub get_certificate()
        If con.State = ConnectionState.Closed Then con.Open()


        query = "SELECT TOP 1 share.sl, share.shrto FROM share ORDER BY share.sl DESC"
        cmd.CommandText = query
        cmd.Connection = con

        Dim dr As SqlDataReader

        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                dr.Read()

                Session("share_certificate_no") = IIf(IsDBNull(dr(0)), 1, dr(0)) + 1
                'Session("share_from") = IIf(IsDBNull(dr(1)), 1, dr(1)) + 1
                'Session("share_to") = IIf(IsDBNull(dr(1)), 1, dr(1)) + CDbl(txtnos.Text)
            Else
                Session("share_certificate_no") = 1
                ' Session("share_from") = 1
                ' Session("share_to") = CDbl(txtnos.Text)
            End If

            dr.Close()

        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()


        End Try

        get_recent(CDbl(Session("share_certificate_no")) - 1)


    End Sub

    Sub get_recent(ByVal cno As Integer)

        Dim brcode As String = String.Empty

        query = "SELECT member.FirstName FROM dbo.share INNER JOIN dbo.member   ON share.cid = member.MemberNo WHERE share.sl =@sl"
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@sl", cno)


        Try
            brcode = cmd.ExecuteScalar


            If brcode Is Nothing Then
                brcode = ""
                alertmsg.Visible = False
            Else
                lblinfo.Text = "Recent Share : " + cno.ToString + "  -  " + Trim(brcode)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Sub set_changes()

        If Session("sesusr") = Nothing Then Exit Sub
        If session_user_role = "Audit" Then Exit Sub


        If con.State = ConnectionState.Closed Then con.Open()
        Dim countresult As Integer

        '  get_certificate()

        query = "SELECT TOP 1 share.sl, share.shrto FROM share ORDER BY share.sl DESC"
        cmd.CommandText = query
        cmd.Connection = con

        Dim dr As SqlDataReader

        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                dr.Read()

                Session("share_certificate_no") = IIf(IsDBNull(dr(0)), 1, dr(0)) + 1
                Session("share_from") = IIf(IsDBNull(dr(1)), 1, dr(1)) + 1
                Session("share_to") = IIf(IsDBNull(dr(1)), 1, dr(1)) + CDbl(txtnos.Text)
            Else
                Session("share_certificate_no") = 1
                Session("share_from") = 1
                Session("share_to") = CDbl(txtnos.Text)
            End If

            dr.Close()

        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()


        End Try

        If con.State = ConnectionState.Closed Then con.Open()



        query = "insert into share(date,sl,shrfrm,shrto,cid,allocation,pershr,shrval,trffrm,trfcid)"
        query &= "values(@date,@sl,@shrfrm,@shrto,@cid,@allocation,@pershr,@shrval,@trffrm,@trfcid)"

        cmd.CommandText = query
        cmd.Connection = con
        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtadate.Text))
        cmd.Parameters.AddWithValue("@sl", Session("share_certificate_no"))
        cmd.Parameters.AddWithValue("@shrfrm", Session("share_from"))
        cmd.Parameters.AddWithValue("@shrto", Session("share_to"))
        cmd.Parameters.AddWithValue("@cid", txtcid.Text)
        cmd.Parameters.AddWithValue("@allocation", CDbl(txtnos.Text))
        cmd.Parameters.AddWithValue("@pershr", CDbl(txtshrval.Text))
        cmd.Parameters.AddWithValue("@shrval", CDbl(txtval.Text))
        If Not Session("share_certificate_no_4m") Is Nothing Then
            cmd.Parameters.AddWithValue("@trffrm", Session("share_certificate_no_4m"))
        Else
            cmd.Parameters.AddWithValue("@trffrm", 0)
        End If
        cmd.Parameters.AddWithValue("@trfcid", txtshrcert.Text)


        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()

        End Try

        lblcerno.Text = Session("share_certificate_no")
        lbldisno.Text = Session("share_from").ToString + " - " + Session("share_to").ToString
        lblshr.Text = txtnos.Text + "( " + get_wrds4share(CDbl(txtnos.Text)) + " )"
        lblname.Text = txtname.Text

        lblprntcid1.Text = Regex.Replace(txtcid.Text, "^.*KBF[\s\-\:\.]*", "", RegexOptions.IgnoreCase).Trim()

        lbldt.Text = txtadate.Text

        Try
            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()


            query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

            '  Dim prod = get_pro(product)

            cmd.CommandText = query
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@acno", txtcid.Text)
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", CDbl(txtval.Text))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", CDbl(txtval.Text))
            cmd.Parameters.AddWithValue("@due", " Share Certificate No: " + Session("share_certificate_no").ToString)

            cmd.Parameters.AddWithValue("@narration", "By Cash")
            cmd.Parameters.AddWithValue("@type", "CASH")
            cmd.Parameters.AddWithValue("@suplimentry", "SHARE")
            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr").ToString)
            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
            cmd.Parameters.AddWithValue("@cbal", 0)
            cmd.Parameters.AddWithValue("@dbal", 0)



            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)


        End Try

        query = ""



        query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + "SHARE" + "'"
        cmd.CommandText = query

        countresult = cmd.ExecuteScalar()

        Session("tid") = Convert.ToString(countresult)

        Dim nar As String = "C.No :" + Convert.ToString(Session("share_certificate_no")) + " " + txtname.Text

        update_suplementry(Session("tid"), "SHARE", CDbl(txtval.Text), 0, nar, "RECEIPT")
        Select Case Trim(mop.SelectedItem.Text)
            Case "Cash"

            Case "Transfer"

            Case "Account"
                update_sb(nar)


        End Select


        ' Session("tid") is already set — Denomination.aspx will auto-load the voucher
        Response.Redirect("~/Denomination.aspx")


    End Sub


    Sub update_sb(ByVal nar As String)


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        cmd.Connection = con
        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtadate.Text))
        cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
        cmd.Parameters.AddWithValue("@drd", CDbl(txtval.Text))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(txtval.Text))
        cmd.Parameters.AddWithValue("@crc", 0)
        cmd.Parameters.AddWithValue("@narration", "To Transfer")
        cmd.Parameters.AddWithValue("@due", nar)
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
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtadate.Text))
        cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
        cmd.Parameters.AddWithValue("@drd", CDbl(txtval.Text))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(txtval.Text))
        cmd.Parameters.AddWithValue("@crc", 0)
        cmd.Parameters.AddWithValue("@narration", "To Transfer")
        cmd.Parameters.AddWithValue("@due", nar)
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



    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)

        If tid = 0 Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        If mop.SelectedItem.Text = "Cash" Then
            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.Clear()

            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", "")
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
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@acn", "")
                cmd.Parameters.AddWithValue("@nar", "By Transfer " + bnk.SelectedItem.Text)
            Else
                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
                cmd.Parameters.AddWithValue("@nar", "By Transfer " + "Share " + "(" + nar + ")")

            End If

            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            cmd.Parameters.Clear()


            query = "INSERT INTO suplement(Date, transid, achead, debit, credit, acn, narration, Type)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@achead", bnk.SelectedItem.Text)
                cmd.Parameters.AddWithValue("@acn", "")
                cmd.Parameters.AddWithValue("@nar", "To Transfer " + bnk.SelectedItem.Text + " " + nar)
            Else
                If Left(Trim(txt_sb.Text), 2) = "79" Then

                    cmd.Parameters.AddWithValue("@achead", "SAVINGS DEPOSIT")
                Else
                    cmd.Parameters.AddWithValue("@achead", "KMK DEPOSIT")
                End If

                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
                cmd.Parameters.AddWithValue("@nar", "To Transfer " + Trim(txt_sb.Text) + nar)
            End If
            cmd.Parameters.AddWithValue("@debit", cr)
            cmd.Parameters.AddWithValue("@credit", 0)

            'cmd.Parameters.AddWithValue("@nar", "To Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try




        End If

        query = ""


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        If mop.SelectedItem.Text = "Cash" Then
            query = "INSERT INTO suplementc(Date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.Clear()

            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", dr)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", "")
            cmd.Parameters.AddWithValue("@nar", nar)
            cmd.Parameters.AddWithValue("@typ", typ)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                Response.Write(ex.ToString)


            End Try

        Else
            cmd.Parameters.Clear()

            query = "INSERT INTO suplementc(Date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", "")
            cmd.Parameters.AddWithValue("@nar", "By Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            cmd.Parameters.Clear()


            query = "INSERT INTO suplementc(Date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@xdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@xdt", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@achead", bnk.SelectedItem.Text)
                cmd.Parameters.AddWithValue("@acn", "")
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

            cmd.Parameters.AddWithValue("@nar", "To Transfer" + nar)
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

        End If

        query = ""



    End Sub


    Sub clear_rcpt()

        txtcid.Text = ""
        txtname.Text = ""
        txtadd.Text = ""
        txtadate.Text = ""
        txtnos.Text = ""
        txtval.Text = ""
        txtshrcert.Text = ""
        txtshrcert.Visible = False

        '        trfsrc.Visible = False

        btn_update.Enabled = True
        btn_nxt_ms.Enabled = True

        btn_update.Visible = False
        btn_nxt_ms.Visible = True

        get_certificate()

        txtcno.Text = Session("share_certificate_no").ToString
        txtcid.Focus()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)


    End Sub

    Private Sub shrtyp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles shrtyp.SelectedIndexChanged
        If shrtyp.SelectedItem.Text = "New" Then

            'trans.Visible = False
            'trfsrc.Visible = False
            trf.Visible = False
            txtshrcert.Visible = False
            'txtfocus(txtshrcert)
        ElseIf shrtyp.SelectedItem.Text = "Transfer" Then
            'trans.Visible = True
            'trfsrc.Visible = True
            'up_trf.Visible = True
            trf.Visible = True
            txtshrcert.Visible = True


            System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtshrcert)

        End If

    End Sub

    Private Sub btn_clr_ms_Click(sender As Object, e As EventArgs) Handles btn_clr_ms.Click
        clear_rcpt()

    End Sub

    Private Sub txtcno_TextChanged(sender As Object, e As EventArgs) Handles txtcno.TextChanged

        If Not Trim(txtcno.Text) = "" Then

            If con.State = ConnectionState.Closed Then con.Open()


            '  get_certificate()

            query = "Select Date,sl,cid,allocation,pershr,shrval,ISNULL(trffrm,'0'),shrfrm,shrto FROM share where sl = " + Trim(txtcno.Text)
            cmd.CommandText = query
            cmd.Connection = con

            Dim dr As SqlDataReader

            Try
                dr = cmd.ExecuteReader()

                If dr.HasRows Then
                    dr.Read()

                    txtcid.Text = dr(2).ToString
                    lblprntcid1.Text = Regex.Replace(dr(2).ToString, "^.*KBF[\s\-\:\.]*", "", RegexOptions.IgnoreCase).Trim()


                    txtshrval.Text = dr(4).ToString
                    txtnos.Text = dr(3).ToString
                    txtadate.Text = Format(dr(0), "dd-MM-yyyy").ToString
                    txtval.Text = dr(5).ToString
                    
                    lblcerno.Text = dr(1).ToString()
                    lbldisno.Text = dr(7).ToString() + " - " + dr(8).ToString()
                    lblshr.Text = txtnos.Text + "( " + get_wrds4share(CDbl(txtnos.Text)) + " )"
                    lbldt.Text = txtadate.Text
                    
                    If dr(6).ToString <> "0" Then
                        txtshrcert.Visible = True
                        txtshrcert.Text = dr(6).ToString
                        shrtyp.SelectedIndex = 2
                    Else
                        shrtyp.SelectedIndex = 1
                    End If
                End If
                
                dr.Close()
                
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try
            fetch_cust()
            lblname.Text = txtname.Text

            btn_nxt_ms.Visible = False


            btn_update.Visible = True

            txtfocus(txtcid)


        End If
    End Sub



    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click


        btn_update.Enabled = False


        If con.State = ConnectionState.Closed Then con.Open()



        query = " UPDATE dbo.share SET cid = @cid,allocation = @allocation,pershr = @pershr,shrval = @shrval,trffrm = @trffrm,trfcid = @trfcid WHERE sl = @sl "


        cmd.CommandText = query
        cmd.Connection = con
        cmd.Parameters.Clear()


        cmd.Parameters.AddWithValue("@sl", Trim(txtcno.Text))
        cmd.Parameters.AddWithValue("@cid", txtcid.Text)
        cmd.Parameters.AddWithValue("@allocation", CDbl(txtnos.Text))
        cmd.Parameters.AddWithValue("@pershr", CDbl(txtshrval.Text))
        cmd.Parameters.AddWithValue("@shrval", CDbl(txtval.Text))
        If Not Session("share_certificate_no_4m") Is Nothing Then
            cmd.Parameters.AddWithValue("@trffrm", Session("share_certificate_no_4m"))
        Else
            cmd.Parameters.AddWithValue("@trffrm", 0)
        End If
        cmd.Parameters.AddWithValue("@trfcid", txtshrcert.Text)


        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()

        End Try


        stitle = "Hi " + Session("sesusr")
        msg = "Certificate No : " + txtcno.Text + "&nbsp; Updated !"
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast1", fnc, True)




        clear_rcpt()

    End Sub
    Private Sub txtnos_TextChanged(sender As Object, e As EventArgs) Handles txtnos.TextChanged
        If Not txtnos.Text = "" Then
            txtval.Text = String.Format("{0:N}", CDbl(txtnos.Text) * CDbl(txtshrval.Text))
            'txtfocus(shrtyp)
            System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.shrtyp)

        End If
    End Sub

    Private Sub btn_nxt_ms_Click(sender As Object, e As EventArgs) Handles btn_nxt_ms.Click
        btn_nxt_ms.Enabled = False


        If shrtyp.SelectedItem.Text = "New" Then
            set_changes()
        Else
            set_changes_transfer()
        End If


    End Sub

    Public Function get_balance_SB(ByVal acn As String)

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
            Session("sb_dbal") = reader(1).ToString - reader(0).ToString
            Session("sb_cbal") = reader(3).ToString - reader(2).ToString



        Else
            Session("sb_cbal") = 0
            Session("sb_dbal") = 0
        End If
        Return Session("sb_dbal")
    End Function

    Private Sub txt_sb_TextChanged(sender As Object, e As EventArgs) Handles txt_sb.TextChanged

        txt_sb.Text = Trim(txt_sb.Text)
        If txt_sb.Text = "" Then txt_sb.Text = 0


        If CDbl(txtval.Text) = 0 Then
            txt_sb.Text = ""
            'txtfocus(txtamt)
        End If
        If Trim(Left(txt_sb.Text, 2)) = "79" Or Trim(Left(txt_sb.Text, 2)) = "76" Then

            lbl_sb_bal.Text = FormatCurrency(get_balance_SB(Trim(txt_sb.Text)))
            Dim abal As Decimal = CDbl(lbl_sb_bal.Text) - 100


            If CDbl(txtval.Text) <= abal Then

                lbl_sb_bal.ForeColor = Color.Green

            Else
                lbl_sb_bal.ForeColor = Color.Red
                btn_nxt_ms.Enabled = False
            End If

        Else
            txt_sb.Text = ""
        End If
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


    Private Sub ShareAllocation_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btntog_Click(sender As Object, e As EventArgs) Handles btntog.Click

        pnlprnt.Style.Add(HtmlTextWriterStyle.Display, "none")
        pnlshr.Style.Add(HtmlTextWriterStyle.Display, "block")
        txtcid.Focus()

    End Sub
End Class