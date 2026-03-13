Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Imports System.Net.Mail
Imports System.Net
Imports System.Drawing
Imports System.Net.Configuration

Public Class Scroll
    Inherits System.Web.UI.Page


    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand

    'Dim imgfn As String
    Dim oResult As String
    Dim pending_vouchers As Integer = 0
    ' Dim passed As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        If Not Page.IsPostBack Then

            '  Session("passed") = 0

            bind_rp()


        End If
    End Sub
    Sub show_passed_v()

        bind_rp()


    End Sub





    Sub bind_rp()


        rpscroll.DataSource = Nothing
        rpscroll.DataBind()

        If inclpass.Checked Then
            Session("passed_v") = 1
        Else
            Session("passed_v") = 0
        End If


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "getscroll"
                cmd.Parameters.Clear()

                cmd.Parameters.Add(New SqlParameter("@cld", Session("passed_v")))

                Try

                    Using Sda As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable
                        Sda.Fill(dt)


                        rpscroll.DataSource = dt
                        rpscroll.DataBind()
                        'get_locked(dt)


                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try


            End Using
            con.Close()

        End Using

    End Sub
    Sub show_passed_p()
        pending_vouchers = 1
        Session("passed_v") = 1

        bind_rp()

    End Sub




    Sub showvinfo(ByVal tid As String)

        ' ddtid.Enabled = False


        Session("MOBILE") = Nothing

        btn_authorize.Enabled = True
        vinfo.Visible = True
        Dim dr As Double
        Dim cr As Double
        Dim drc As Double
        Dim crc As Double



        Dim ds As New DataSet

        ' lblvno.Text = tid


        cmd.Connection = con

        query = "select suplimentry,DATE,sesusr,acno,type,drd,crd,drc,crc,scroll from dbo.trans where trans.id='" + tid + "'"
        cmd.CommandText = query


        Dim Adapter As New SqlDataAdapter(query, con)
        Adapter.Fill(ds)


        If Not ds.Tables(0).Rows.Count = 0 Then

            lblprod.Text = ds.Tables(0).Rows(0).ItemArray(0).ToString
            Session("prod") = ds.Tables(0).Rows(0).ItemArray(0).ToString
            Session("ach") = ds.Tables(0).Rows(0).ItemArray(0).ToString
            lbldt.Text = Convert.ToDateTime(ds.Tables(0).Rows(0).ItemArray(1))
            lblentryby.Text = ds.Tables(0).Rows(0).ItemArray(2).ToString
            'lbldt.Font.Size = 8
            lblacn.Text = ds.Tables(0).Rows(0).ItemArray(3).ToString
            Session("acn") = ds.Tables(0).Rows(0).ItemArray(3).ToString
            lbltyp.Text = ds.Tables(0).Rows(0).ItemArray(4).ToString
            dr = ds.Tables(0).Rows(0).ItemArray(5).ToString
            cr = ds.Tables(0).Rows(0).ItemArray(6).ToString
            drc = ds.Tables(0).Rows(0).ItemArray(7).ToString
            crc = ds.Tables(0).Rows(0).ItemArray(8).ToString
            Session("passed") = ds.Tables(0).Rows(0).ItemArray(9).ToString
            If CDbl(dr) = 0 Then
                lblamt.Text = FormatCurrency(cr)
                lblcamt.Text = FormatCurrency(crc)
                Session("vtype") = "CREDIT"
                Session("amt") = FormatNumber(crc)
                Session("damt") = FormatNumber(cr)
            Else
                lblamt.Text = FormatCurrency(dr)
                lblcamt.Text = FormatCurrency(drc)
                Session("vtype") = "DEBIT"
                Session("amt") = FormatNumber(drc)
                Session("damt") = FormatNumber(dr)
            End If



        End If

        query = "select cid from dbo.master where acno='" + lblacn.Text + "'"
        cmd.CommandType = CommandType.Text
        cmd.CommandText = query

        Try
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            oResult = cmd.ExecuteScalar()
            Session("memberno") = oResult
        Catch e As Exception

            Response.Write(e.ToString)
        End Try

        If oResult IsNot Nothing Then

            query = "select firstname from dbo.member where MemberNo='" + oResult + "'"
            cmd.CommandType = CommandType.Text
            cmd.CommandText = query
            Try
                If Not con.State = ConnectionState.Open Then
                    con.Open()
                End If
                oResult = cmd.ExecuteScalar()
                If oResult IsNot Nothing Then
                    lblname.Text = oResult.ToString

                End If
            Catch e As Exception
                Response.Write(e.ToString)

            End Try

            If Session("passed_v") = 0 Then

                query = "select COALESCE(MOBILE,'0000') from dbo.member where MemberNo='" + Session("memberno") + "'"
                cmd.CommandType = CommandType.Text
                cmd.CommandText = query
                Try
                    If Not con.State = ConnectionState.Open Then
                        con.Open()
                    End If
                    oResult = cmd.ExecuteScalar()


                    If IsDBNull(oResult.ToString) Then

                        lblmobile.Text = ""

                    Else
                        If oResult IsNot Nothing Then

                            Session("MOBILE") = Trim(oResult.ToString)
                            If Len(Session("MOBILE")) = 10 Then


                                Session("MOBILE") = Session("MOBILE").ToString
                                lblmobile.Text = Session("MOBILE")
                            Else
                                lblmobile.Text = ""
                            End If

                        End If

                    End If
                Catch e As Exception
                    Response.Write(e.ToString)

                End Try



            End If


            Dim act = Mid(Trim(Session("memberno")), 1, 5)

            If act = "KBF09" Then lblmobile.Text = ""




            If Trim(lblmobile.Text) = "" Then
                chksms.Checked = False
                chksms.Enabled = False

            Else
                chksms.Checked = True
                chksms.Enabled = True

            End If


        End If
        If Session("passed") = "True" Then
            'sendotp()
            vinfo.Visible = True
            btn_authorize.Enabled = False

            'btnpnl.Visible = False


        End If




    End Sub


    Sub clea()


        btnpnl.Visible = True
        vinfo.Visible = False

        lblmobile.Text = ""
        chksms.Checked = False
        chksms.Enabled = False
        lblacn.Text = ""
        lblamt.Text = ""
        lbldt.Text = ""
        lblentryby.Text = ""
        lblname.Text = ""
        lblprod.Text = ""
        lbltyp.Text = ""
        ' lblvno.Text = ""
        lblcap.Visible = False
        'lblotp.Visible = False
        txtreason.Text = ""
        vin.Visible = False
        pnlotp.Visible = False
        btnpnl.Visible = True

        ' ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "none", "showdet();", True)

        pnlscrldata.Visible = True
        upscrlrejct.Visible = False



    End Sub

    Public Function get_balance4dl(ByVal acn As String)

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




    Function chk_4_extra(ByVal acn As String)

        Dim bal As Double = 0

        If con1.State = ConnectionState.Closed Then con1.Open()

        query = "select acn from dlspec where dlspec.deposit='" + acn + "'"
        cmd.Connection = con1
        cmd.CommandText = query
        Try
            'Dim isstdins As String = cmd.ExecuteScalar()

            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader
            If dr.HasRows Then

                Do While dr.Read

                    Session("dl") = IIf(IsDBNull(dr(0)), 0, dr(0))
                    bal = bal + get_balance4dl(Session("dl"))

                Loop
            End If



        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con1.Close()
            cmd.Dispose()

        End Try


        Return bal

    End Function

    Sub transfer2transaction(ByVal tid As Integer)



        query = "SELECT * FROM dbo.[trans] where trans.id=@xid"

        'query = "INSERT into dbo.transaction(id,date,acno,drd,crd,drc,crc,narration,due,type,scroll,suplimentry,sesusr,entryat,cbal,dbal)"
        'query &= "Select id,date,acno,drd,crd,drc,crc,narration,due,type,scroll,suplimentry,sesusr,entryat,cbal,dbal from trans where trans.id=@xid"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@xid", tid)
        cmd.Connection = con

        Try
            If Not con.State = ConnectionState.Closed Then
                con.Open()
            End If

            Dim reader As SqlDataReader

            reader = cmd.ExecuteReader()
            If reader.HasRows Then

                While reader.Read()

                    Session("tid") = reader.GetSqlValue(0).ToString
                    Session("tdat") = reader.GetSqlValue(1)
                    Session("acn") = reader.GetSqlValue(2).ToString
                    Session("drd") = reader.GetSqlValue(3).ToString
                    Session("crd") = reader.GetSqlValue(4).ToString
                    Session("drc") = reader.GetSqlValue(5).ToString
                    Session("crc") = reader.GetSqlValue(6).ToString
                    Session("narration") = reader.GetSqlValue(7).ToString
                    Session("due") = reader.GetSqlValue(8).ToString
                    Session("type") = reader.GetSqlValue(9).ToString
                    Session("scrl") = reader.GetSqlValue(10).ToString
                    Session("suplimentry") = reader.GetSqlValue(11).ToString
                    Session("sesusr") = reader.GetSqlValue(12).ToString
                    Session("entryat") = reader.GetSqlValue(13)
                    Session("cbal") = 0 'reader.GetSqlValue(14).ToString
                    Session("dbal") = reader.GetSqlValue(15).ToString

                End While


            End If

            reader.Close()

            If Not Trim(Session("acn")) = "" Then


                'trans()


            End If


            'End If



        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            con.Close()

        End Try





    End Sub

    'Sub trans()

    '    con.Close()
    '    cmd.Dispose()

    '    con.Open()

    '    Dim dt As Date = CType(Session("tdat"), Date)

    '    query = "INSERT INTO dbo.[actrans](id,date,acno,drd,crd,drc,crc,narration,due,type,scroll,suplimentry,sesusr,entryat,cbal,dbal)"
    '    query &= "values(@tnid,@tndate,@tnacno,@tndrd,@tncrd,@tndrc,@tncrc,@tnnarration,@tndue,@tntype,@tnscroll,@tnsuplimentry,@tnsesusr,@tnentryat,@tncbal,@tndbal)"
    '    cmd.CommandText = query
    '    cmd.Parameters.AddWithValue("@tnid", Session("tid"))
    '    cmd.Parameters.AddWithValue("@tndate", dt)
    '    cmd.Parameters.AddWithValue("@tnacno", Session("acn"))
    '    cmd.Parameters.AddWithValue("@tndrd", Session("drd"))
    '    cmd.Parameters.AddWithValue("@tncrd", Session("crd") - Session("dbal"))
    '    cmd.Parameters.AddWithValue("@tndrc", Session("drc"))
    '    cmd.Parameters.AddWithValue("@tncrc", Session("crc"))
    '    cmd.Parameters.AddWithValue("@tnnarration", Session("narration"))
    '    cmd.Parameters.AddWithValue("@tndue", Session("due"))
    '    cmd.Parameters.AddWithValue("@tntype", Session("type"))
    '    cmd.Parameters.AddWithValue("@tnscroll", Session("scrl"))
    '    cmd.Parameters.AddWithValue("@tnsuplimentry", Session("suplimentry"))
    '    cmd.Parameters.AddWithValue("@tnsesusr", Session("sesusr"))
    '    cmd.Parameters.AddWithValue("@tnentryat", Session("entryat"))
    '    cmd.Parameters.AddWithValue("@tncbal", 0)
    '    cmd.Parameters.AddWithValue("@tndbal", 0)
    '    cmd.Connection = con

    '    Try

    '        If Not con.State = ConnectionState.Open Then
    '            con.Open()
    '        End If


    '        cmd.ExecuteNonQuery()


    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try

    'End Sub
    Sub update_scroll(ByVal tid As Integer)

        Dim trx As SqlTransaction = Nothing

        Try
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If

            trx = con.BeginTransaction("ScrollUpdate")

            ' Disable Triggers to prevent audit log performance hits and lock timeouts
            cmd = New SqlCommand()
            cmd.Connection = con
            cmd.Transaction = trx
            cmd.CommandText = "DISABLE TRIGGER ALL ON trans; " & _
                              "DISABLE TRIGGER ALL ON actrans; " & _
                              "DISABLE TRIGGER ALL ON suplement; " & _
                              "DISABLE TRIGGER ALL ON actransc; " & _
                              "DISABLE TRIGGER ALL ON suplementc;"
            cmd.ExecuteNonQuery()

            ' Perform Updates
            cmd.CommandText = "UPDATE trans SET scroll = 1 WHERE id = @id; " & _
                              "UPDATE actrans SET scroll = 1 WHERE id = @id; " & _
                              "UPDATE suplement SET scroll = 1 WHERE transid = @id; " & _
                              "UPDATE actransc SET scroll = 1 WHERE id = @id; " & _
                              "UPDATE suplementc SET scroll = 1 WHERE transid = @id;"
            cmd.Parameters.AddWithValue("@id", tid)
            cmd.ExecuteNonQuery()

            ' Re-enable Triggers
            cmd.CommandText = "ENABLE TRIGGER ALL ON trans; " & _
                              "ENABLE TRIGGER ALL ON actrans; " & _
                              "ENABLE TRIGGER ALL ON suplement; " & _
                              "ENABLE TRIGGER ALL ON actransc; " & _
                              "ENABLE TRIGGER ALL ON suplementc;"
            cmd.ExecuteNonQuery()

            trx.Commit()

        Catch ex As Exception
            If trx IsNot Nothing Then
                Try
                    trx.Rollback()
                Catch rx As Exception
                    Response.Write("Rollback error: " & rx.ToString())
                End Try
            End If
            Response.Write(ex.ToString())
        Finally
            If cmd IsNot Nothing Then
                cmd.Dispose()
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

    End Sub


    Private Sub btn_rject_Click(sender As Object, e As EventArgs) Handles btn_rject.Click


        If Session("passed") = "True" Then
            sendotp()
            vinfo.Visible = False
            pnlotp.Visible = True
            btnpnl.Visible = False
            txtotp.Visible = True
            lbloptcaption.Visible = True
            btnpnl.Visible = False


        Else
            vinfo.Visible = False
            lbloptcaption.Visible = False
            txtotp.Visible = False
            pnlotp.Visible = True
            btnpnl.Visible = False

        End If
        '   ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "none", "showdet();", True)
        pnlscrldata.Visible = False
        upscrlrejct.Visible = True

        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtreason)

    End Sub

    Private Sub btnpay_Click(sender As Object, e As EventArgs) Handles btnpay.Click

        If Not Session("passed") = "True" Then GoTo nxt
        If txtotp.Text = Session("otp") Then

nxt:
            If con.State = ConnectionState.Closed Then con.Open()
            query = "insert into reject(date,tid,acn,name,achead,vtype,amount,sesusr,entryat,rejectby,rejectat,reason)"
            query &= "values(@date,@tid,@acn,@name,@achead,@vtype,@amount,@sesusr,@entryat,@rejectby,@rejectat,@reason)"
            cmd.Connection = con
            cmd.CommandText = query
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(lbldt.Text))
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@acn", lblacn.Text)
            cmd.Parameters.AddWithValue("@name", lblname.Text)
            cmd.Parameters.AddWithValue("@achead", lblprod.Text)
            cmd.Parameters.AddWithValue("@vtype", lbltyp.Text)
            cmd.Parameters.AddWithValue("@amount", CDbl(lblamt.Text))
            cmd.Parameters.AddWithValue("@sesusr", lblentryby.Text)
            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(lbldt.Text))
            cmd.Parameters.AddWithValue("@rejectby", Session("sesusr"))
            cmd.Parameters.AddWithValue("@rejectat", DateAndTime.Now)
            cmd.Parameters.AddWithValue("@reason", txtreason.Text)

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)


            End Try



            Dim dat1 As DateTime = DateTime.ParseExact(lbldt.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
            Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            If con.State = ConnectionState.Closed Then con.Open()
            query = " delete from trans WHERE trans.id = @tid AND CONVERT(VARCHAR(20), date, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try



            If con.State = ConnectionState.Closed Then con.Open()
            query = " delete from actrans WHERE actrans.id = @tid AND CONVERT(VARCHAR(20), date, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try



            If con.State = ConnectionState.Closed Then con.Open()
            query = " delete from actransc WHERE actransc.id = @tid AND CONVERT(VARCHAR(20), date, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try



            'If con.State = ConnectionState.Closed Then con.Open()
            'query = " delete from trans WHERE trans.id = @tid AND CONVERT(VARCHAR(20), date, 112) = @dt"
            'cmd.Parameters.Clear()
            'cmd.Parameters.AddWithValue("@tid", ddtid.SelectedItem.Text)
            'cmd.Parameters.AddWithValue("@dt", reformattedto)
            'cmd.Connection = con
            'cmd.CommandText = query
            'Try
            '    cmd.ExecuteNonQuery()

            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            'Finally
            '    con.Close()
            '    cmd.Dispose()

            'End Try



            If con.State = ConnectionState.Closed Then con.Open()
            query = " delete from suplement WHERE transid = @tid AND CONVERT(VARCHAR(20), date, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try

            If con.State = ConnectionState.Closed Then con.Open()
            query = " delete from suplementc WHERE transid = @tid AND CONVERT(VARCHAR(20), date, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try


            If con.State = ConnectionState.Closed Then con.Open()
            query = " delete from diff WHERE diff.tid = @tid AND CONVERT(VARCHAR(20), date, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try

            If Not Trim(lblacn.Text) = "" Then

                If con.State = ConnectionState.Closed Then con.Open()


                If Session("vtype") = "DEBIT" And (Trim(Session("prod")) = "JEWEL LOAN" Or Trim(Session("prod")) = "DAILY COLLECTION LOAN" Or Trim(Session("prod")) = "DEPOSIT LOAN" Or Trim(Session("prod")) = "MORTGAGE LOAN") Then

                    query = " UPDATE master set cld=0,amount=0 where master.acno=@acno"
                Else
                    query = " UPDATE master set cld=0 where master.acno=@acno"

                End If

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", lblacn.Text)
                cmd.Parameters.AddWithValue("@dt", reformattedto)
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)
                Finally
                    con.Close()
                    cmd.Dispose()

                End Try

            End If

            If Not Trim(lblacn.Text) = "" Then

                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd = New SqlCommand
                        cmd.Connection = con
                        query = "select masterc.acno from masterc where masterc.parent=@acno"
                        cmd.CommandText = query
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@acno", lblacn.Text)
                        Using reader As SqlDataReader = cmd.ExecuteReader
                            If reader.HasRows Then
                                Do While reader.Read


                                    Using con1 = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                                        con1.Open()
                                        Using cmd1 = New SqlCommand
                                            cmd1.Connection = con1
                                            query = " UPDATE masterc set cld=0 where masterc.acno=@acno"
                                            cmd1.Parameters.Clear()
                                            cmd1.Parameters.AddWithValue("@acno", reader(0).ToString)
                                            cmd1.CommandText = query
                                            cmd1.ExecuteNonQuery()

                                        End Using
                                    End Using
                                Loop


                            End If
                        End Using
                    End Using
                    con.Close()

                End Using

            End If


            If con.State = ConnectionState.Closed Then con.Open()

            query = "update collection set trf=0 WHERE tid = @tid AND CONVERT(VARCHAR(20), trfon, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try



            If con.State = ConnectionState.Closed Then con.Open()

            query = "update tmpint set post=0 WHERE transid = @tid AND CONVERT(VARCHAR(20), tdate, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try

            If con.State = ConnectionState.Closed Then con.Open()
            query = "update tmpintc set post=0 WHERE transid = @tid AND CONVERT(VARCHAR(20), tdate, 112) = @dt"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@dt", reformattedto)
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try



            clea()

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "none", "showdet();", True)

            bind_rp()


        Else

            txtotp.Text = "invalid OTP"
            txtotp.ForeColor = Drawing.Color.Red
        End If

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

            Response.Write(e.ToString)
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
        Dim reader As StreamReader = New StreamReader(Server.MapPath("scroll.html"))
        body = reader.ReadToEnd
        body = body.Replace("{branch}", Session("branch"))
        body = body.Replace("{otp}", strrandom)
        body = body.Replace("{lblvno}", Session("tid"))
        body = body.Replace("{lbldt}", lbldt.Text)
        body = body.Replace("{lblentryby}", lblentryby.Text)
        body = body.Replace("{lblprod}", lblprod.Text)
        body = body.Replace("{lblacn}", lblacn.Text)
        body = body.Replace("{lblname}", lblname.Text)
        body = body.Replace("{lbltyp}", lbltyp.Text)
        body = body.Replace("{lblamt}", lblamt.Text)
        body = body.Replace("{lblcamt}", lblcamt.Text)
        Return body

    End Function

    Protected Sub sendotp()

        Dim subject As String = "OTP for Correction"
        Dim recepientEmail As String = ConfigurationManager.AppSettings("recepientEmailid")
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




    Protected Sub sendotp1()

        Dim subject As String = "OTP for Correction"
        Dim recepientEmail As String = ConfigurationManager.AppSettings("recepientEmailid")
        Dim body As String = populate_body()
        Dim mailMessage As MailMessage = New MailMessage
        mailMessage.From = New MailAddress(ConfigurationManager.AppSettings("UserName"))
        mailMessage.Subject = subject
        mailMessage.Body = body
        mailMessage.IsBodyHtml = True
        mailMessage.To.Add(New MailAddress(recepientEmail))
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls
        Dim smtp As SmtpClient = New SmtpClient(ConfigurationManager.AppSettings("Host"), Integer.Parse(ConfigurationManager.AppSettings("Port")))

        'smtp.Host = ConfigurationManager.AppSettings("Host")
        smtp.EnableSsl = False
        'Convert.ToBoolean(ConfigurationManager.AppSettings("EnableSsl"))
        Dim NetworkCred As System.Net.NetworkCredential = New System.Net.NetworkCredential
        NetworkCred.UserName = ConfigurationManager.AppSettings("UserName")
        NetworkCred.Password = ConfigurationManager.AppSettings("Password")
        smtp.UseDefaultCredentials = False
        smtp.Credentials = NetworkCred
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        ' smtp.Port = Integer.Parse(ConfigurationManager.AppSettings("Port"))
        Try
            ' smtp.Send(mailMessage)
        Catch ex As Exception
            'MsgBox(ex.ToString)
            Console.WriteLine(ex.ToString)

            Dim al = "mailerror('" + ex.Message.ToString + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", al, True)
        End Try




    End Sub


    Sub prepare_sms()

        Dim msgdata As String = ""



        ' Session("vtype") = "CREDIT"
        ' Session("amt") = FormatNumber(crc)
        ' Session("vtype") = "DEBIT"
        '  Session("amt") = FormatNumber(drc)
        'Session("MOBILE") = oResult.ToString
        Session("ACNO") = "XXXXXXX" + Right(Trim(lblacn.Text), 4)
        Session("ON") = lbldt.Text

        Session("bal") = FormatNumber(get_balance(lblacn.Text))



        If Session("vtype") = "DEBIT" Then

            msgdata = "Your A/C " + Session("ACNO") + " Debited INR " + Session("damt") + " on " + Session("ON") + ".A/C Balance INR " + Session("bal")

        ElseIf Session("vtype") = "CREDIT" Then
            msgdata = "Your A/C " + Session("ACNO") + " Credited INR " + Session("damt") + " on " + Session("ON") + ".A/C Balance INR " + Session("bal")
        End If

        If Not Trim(lblmobile.Text) = "" Then
            If Len(Trim(lblmobile.Text)) = 10 Then
                If chksms.Checked = True Then
                    Send_sms(lblmobile.Text, msgdata, If(Session("vtype") = "CREDIT", fiscusM.TemplateID.Credit, fiscusM.TemplateID.Debit))
                End If
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

        Return Session("cbal")
    End Function

    Private Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click

        clea()
        '        bind_grid()
        '   ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "none", "showdet();", True)


    End Sub



    Private Sub inclpass_CheckedChanged(sender As Object, e As EventArgs) Handles inclpass.CheckedChanged
        clea()
        bind_rp()


    End Sub

    Private Sub btn_authorize_Click(sender As Object, e As EventArgs) Handles btn_authorize.Click


        If Session("ach") = "RECURRING DEPOSIT" Or Session("ach") = "FIXED DEPOSIT" Or Session("ach") = "REINVESTMENT DEPOSIT" Then

            If Session("vtype") = "CREDIT" Then GoTo nxt
            Dim lod As Double = chk_4_extra(Session("acn"))
            If lod < 0 Then
                Dim stitle = "Hi " + Session("sesusr")
                Dim msg = "Please Close the Deposit Loan and then Approve."
                Dim fnc = "showToastnOK('" + stitle + "','" + msg + "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

                Exit Sub

            End If

        End If

nxt:
        If Session("passed_v") = 0 Then
            btn_authorize.Enabled = False
            update_scroll(Session("tid"))
            If chksms.Checked = True Then prepare_sms()
            clea()
            If Not Session("src") = "lnk" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "none", "showdet();", True)
                Session("src") = Nothing
            End If
            bind_rp()
        End If


    End Sub

    Private Sub rpscroll_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpscroll.ItemDataBound
        For Each Items As RepeaterItem In rpscroll.Items

            Dim typ As Label = Items.FindControl("lblltyp")
            Dim camt As Label = Items.FindControl("lblcamt")
            Dim damt As Label = Items.FindControl("lbldamt")
            If Trim(typ.Text) = "RECEIPT" Then
                typ.ForeColor = Color.DarkGreen
                damt.ForeColor = Color.DarkGreen
                camt.ForeColor = Color.DarkGreen
                camt.Text = FormatNumber(camt.Text)
                damt.Text = FormatNumber(damt.Text)
            Else
                typ.ForeColor = Color.DarkRed
                damt.ForeColor = Color.DarkRed
                camt.ForeColor = Color.DarkRed
                camt.Text = FormatNumber(camt.Text)
                damt.Text = FormatNumber(damt.Text)
            End If

        Next


    End Sub

    Private Sub rpscroll_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpscroll.ItemCommand

        If e.CommandName = "ViewClick" Then
            ''MsgBox(e.CommandArgument)
            showvinfo(e.CommandArgument)
            Session("tid") = e.CommandArgument
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "none", "showdet();", True)
            pnlscrldata.Visible = False
            upscrlrejct.Visible = True


        End If
        If e.CommandName = "ApproveClick" Then
            showvinfo(e.CommandArgument)
            Session("tid") = e.CommandArgument
            Session("src") = "lnk"
            For i = 0 To rpscroll.Items.Count - 1 Step 1
                Dim sms As CheckBox = rpscroll.Items(i).FindControl("smschk")
                Dim id As Label = rpscroll.Items(i).FindControl("lblid")
                Dim acn As Label = rpscroll.Items(i).FindControl("lbllacno")
                Dim ach As Label = rpscroll.Items(i).FindControl("lbllach")

                Session("acn") = acn.Text
                Session("ach") = Trim(ach.Text)
                If CDbl(id.Text) = CDbl(e.CommandArgument) Then
                    If sms.Checked = True Then
                        chksms.Checked = True

                    Else
                        chksms.Checked = False

                    End If
                End If
            Next

            btn_authorize_Click(btn_authorize, e)





        End If

        If e.CommandName = "RejectClick" Then
            showvinfo(e.CommandArgument)

            Session("tid") = e.CommandArgument
            btn_rject_Click(btn_rject, e)


        End If

    End Sub

    Private Sub btn_authorize_Command(sender As Object, e As CommandEventArgs) Handles btn_authorize.Command

    End Sub
End Class