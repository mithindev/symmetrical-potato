Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Net.Mail

Public Class DepositOpening
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim query As String
    Dim imgfn As String
    Dim oResult As String
    Dim ncid As String
    Dim serial As Integer

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)

    End Sub

    Protected Sub Page_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim ex As Exception = Server.GetLastError()
        Response.Clear()
        Response.Write("REAL ERROR: " & ex.ToString())
        Response.End()
    End Sub

    Private Function populate_body()

        Dim oResult As String = ""
        Dim qry = "select branch from dbo.branch "
        Dim cmdx As New SqlCommand
        cmdx.CommandType = CommandType.Text
        cmdx.CommandText = qry
        If con.State = ConnectionState.Closed Then con.Open()
        cmdx.Connection = con
        Try


            oResult = cmdx.ExecuteScalar()

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
        Dim reader As StreamReader = New StreamReader(Server.MapPath("/admin/Campagin.html"))
        body = reader.ReadToEnd
        body = body.Replace("{branch}", Session("branch"))
        body = body.Replace("{otp}", strrandom)
        body = body.Replace("{dt}", ddt.Text)
        body = body.Replace("{cid}", txtcid.Text)
        body = body.Replace("{name}", txtname.Text)
        body = body.Replace("{address}", txtcof.Text + vbCrLf + txtadd.Text)
        body = body.Replace("{prod}", deptyp.SelectedItem.Text)
        body = body.Replace("{prd}", prd.Text + "M")
        body = body.Replace("{isren}", IIf(isrenew.Checked, "Yes", "No"))
        body = body.Replace("{cint}", cintr.Text)
        body = body.Replace("{dint}", dint.Text)
        Return body

    End Function
    Protected Sub sendotp()

        Dim subject As String = "OTP for Correction"
        Dim recepientEmail As String = ConfigurationManager.AppSettings("recepientEmailid")
        Dim body As String = populate_body()
        Dim mailMessage As MailMessage = New MailMessage
        mailMessage.From = New MailAddress(ConfigurationManager.AppSettings("UserName"))
        mailMessage.Subject = subject
        mailMessage.Body = body
        mailMessage.IsBodyHtml = True
        mailMessage.To.Add(New MailAddress(recepientEmail))
        Dim smtp As SmtpClient = New SmtpClient
        smtp.Host = ConfigurationManager.AppSettings("Host")
        smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings("EnableSsl"))
        Dim NetworkCred As System.Net.NetworkCredential = New System.Net.NetworkCredential
        NetworkCred.UserName = ConfigurationManager.AppSettings("UserName")
        NetworkCred.Password = ConfigurationManager.AppSettings("Password")
        smtp.UseDefaultCredentials = True
        smtp.Credentials = NetworkCred
        smtp.Port = Integer.Parse(ConfigurationManager.AppSettings("Port"))
        Try
            smtp.Send(mailMessage)
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then
            Session("Tabin") = 0


            get_products()

            cintr.Text = 0
            dint.Text = 0

        End If

    End Sub

    Sub get_products()

        Dim dr As SqlDataReader

        Dim query As String = "SELECT shortname from products where products.prdtype='" + "Deposit" + "'"

        cmd.Connection = con
        cmd.CommandText = query

        dr = cmd.ExecuteReader()

        If dr.HasRows Then
            Do While dr.Read()

                deptyp.Items.Add(dr(0).ToString)
            Loop

        End If


        deptyp.Items.Insert(0, "<-Select->")

        dr.Close()


    End Sub

    Sub getint()

        If con.State = ConnectionState.Closed Then con.Open()

        Dim startDate As Date = CDate(ddt.Text)
        Dim tenure As Integer = CInt(prd.Text)

        Dim roiCint As Decimal = 0D
        Dim roiDint As Decimal = 0D

        '  GET ROI SLAB
        Dim q As String =
        "SELECT TOP 1 cint, dint
         FROM dbo.roi
         WHERE product = @prod
         AND @startDate BETWEEN fyfrm AND fyto
         AND @months BETWEEN prdfrm AND prdto
         ORDER BY id DESC"

        Using cmd2 As New SqlCommand(q, con)

            cmd2.Parameters.AddWithValue("@prod", deptyp.SelectedItem.Text)
            cmd2.Parameters.AddWithValue("@startDate", startDate)
            cmd2.Parameters.AddWithValue("@months", tenure)

            Using dr2 = cmd2.ExecuteReader()
                If dr2.Read() Then
                    roiCint = CDec(dr2("cint"))
                    roiDint = CDec(dr2("dint"))
                Else
                    cintr.Text = 0
                    dint.Text = 0
                    Exit Sub
                End If
            End Using
        End Using

        '  PLAN EXTRA INTEREST
        Dim addInt As Decimal = 0D

        If chkRenewPlan.Checked Or chkSeniorPlan.Checked Or chkTransferPlan.Checked Then

            Dim q2 As String =
        "SELECT TOP 1 renew, srcitizen, bnktrf
         FROM dbo.goldrate
         WHERE [date] <= @startDate
         ORDER BY [date] DESC"

            Using cmd3 As New SqlCommand(q2, con)

                cmd3.Parameters.AddWithValue("@startDate", startDate)

                Using dr3 = cmd3.ExecuteReader()

                    If dr3.Read() Then

                        If chkRenewPlan.Checked Then
                            addInt += If(IsDBNull(dr3("renew")), 0D, CDec(dr3("renew")))
                        End If

                        If chkSeniorPlan.Checked Then
                            addInt += If(IsDBNull(dr3("srcitizen")), 0D, CDec(dr3("srcitizen")))
                        End If

                        If chkTransferPlan.Checked Then
                            addInt += If(IsDBNull(dr3("bnktrf")), 0D, CDec(dr3("bnktrf")))
                        End If

                    End If

                End Using
            End Using
        End If

        ' FINAL INTEREST
        cintr.Text = (roiCint + addInt).ToString()
        dint.Text = (roiDint + addInt).ToString()

        cintr.Focus()


        Dim scriptDebug1 As String = "alert('DEBUG INFO:\n" &
        "cint " & cintr.Text.ToString() & "\n" &
        "addInt " & addInt.ToString() & "\n" &
    "');"

        ScriptManager.RegisterStartupScript(Page, Page.GetType(),
    Guid.NewGuid().ToString(),
    scriptDebug1,
    True)

    End Sub



    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showDLStab", "ShowDLS(1)", True)

        If Trim(deptyp.SelectedItem.Text) = "FD" Or Trim(deptyp.SelectedItem.Text) = "RD" Then
            btn_da1_nxt.Text = "Next"
            Session("Tabin") = 1
        Else
            btn_da1_nxt.Text = "Save"
        End If

        txtfocus(txtgrpid)

    End Sub


    Sub open_deposit(ByVal Product As String)

        If session_user_role = "Audit" Then Exit Sub

        Dim acnp As String = get_acnprefix(deptyp.SelectedItem.Text)

        Session("aid") = Trim(acnp) '+ CStr(DateAndTime.Year(Convert.ToDateTime(ddt.Text)))

        cmd.Connection = con
        cmd.CommandText = "SELECT TOP 1 serial FROM masterc where product='" + deptyp.SelectedItem.Text + "'ORDER BY serial DESC"

        oResult = cmd.ExecuteScalar()

        Try
            If oResult IsNot Nothing Then
                serial = Int(oResult.ToString) + 1
            Else
                serial = 1
            End If

            Session("acn") = Session("aid") + String.Format("{0:000000}", serial)

            Dim cdt As Date = ddt.Text
            If Not Trim(deptyp.SelectedItem.Text) = "SB" Then
                Session("prdtyp") = "M"
                Session("mdt") = cdt.AddMonths(prd.Text)
            Else
                Session("prdtyp") = "M"
                Session("mdt") = DBNull.Value
            End If


        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        If deptyp.SelectedItem.Text = "RD" Then

            Dim p As Decimal = 0 'Convert.ToDecimal(amt.Text)
            '  Dim due As Decimal = Convert.ToDecimal(amt.Text)

            Dim drate As Double = Convert.ToDouble(dint.Text)
            Dim mnth As Integer = prd.Text
            Dim inta As Decimal
            Dim mat As Decimal = 0

            For i = 1 To mnth

                p = p + Session("due")
                inta = ((p * drate / 100) / 12)
                p = p + inta
            Next

            '   mamt = CDbl(amt.Text)

        Else

        End If

        If Trim(deptyp.Text) = "RD" Then

            If con.State = ConnectionState.Closed Then con.Open()
            Dim sqlquery As String = "UPDATE dbo.KYC SET Parent = @parent WHERE MemberNo = @member"
            cmd.Connection = con
            cmd.CommandText = sqlquery
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@parent", txtgrpid.Text)
            cmd.Parameters.AddWithValue("@member", txtcid.Text)
            Try

                cmd.ExecuteNonQuery()

                '  lblacn.Text = " Account No is  " + acn

            Catch ex As Exception
                Response.Write(ex.Message)
            Finally
                con.Close()
                cmd.Dispose()
                cmd = New SqlCommand
            End Try


        End If

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con

        Dim rnew As Integer = 0
        If chkRenewPlan.Checked = True Then rnew = 1

        Dim plan As String = ""

        If chkRenewPlan.Checked Then plan &= "Renew,"
        If chkSeniorPlan.Checked Then plan &= "Senior,"
        If chkTransferPlan.Checked Then plan &= "Transfer,"
        If chkRegularPlan.Checked Then plan &= "Regular,"

        If plan.EndsWith(",") Then
            plan = plan.Substring(0, plan.Length - 1)
        End If


        Dim query As String = String.Empty

        query = "INSERT INTO master(date,acno,cid,serial,product,amount,cint,dint,cld,prd,prdtype,mdate,mamt,isrenew,renewacn,[plan])"
        query &= "VALUES(@date,@acno,@cid,@serial,@product,@amount,@cint,@dint,@cld,@prd,@prdtype,@mdate,@mamt,@isrenew,@renewacn,@plan)"

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(ddt.Text))
        cmd.Parameters.AddWithValue("@acno", Session("acn"))
        cmd.Parameters.AddWithValue("@cid", txtcid.Text)
        cmd.Parameters.AddWithValue("@serial", serial)
        cmd.Parameters.AddWithValue("@product", deptyp.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@amount", 0)
        cmd.Parameters.AddWithValue("@cint", cintr.Text)
        cmd.Parameters.AddWithValue("@dint", dint.Text)
        cmd.Parameters.AddWithValue("@cld", 0)
        cmd.Parameters.AddWithValue("@prd", prd.Text)
        cmd.Parameters.AddWithValue("@prdtype", Session("prdtyp"))
        cmd.Parameters.AddWithValue("@mdate", Session("mdt"))
        cmd.Parameters.AddWithValue("@mamt", 0)
        cmd.Parameters.AddWithValue("@isrenew", rnew)
        cmd.Parameters.AddWithValue("@renewacn", Trim(txtrenewacn.Text))
        cmd.Parameters.AddWithValue("@plan", plan)
        Try

            cmd.ExecuteNonQuery()

            '  lblacn.Text = " Account No is  " + 

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
            cmd.Dispose()
            cmd = New SqlCommand

        End Try


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con

        ' Dim rnew As Integer = 0
        ' If isrenew.Checked = True Then rnew = 1

        query = "INSERT INTO masterc(date,acno,cid,serial,product,amount,cint,dint,cld,prd,prdtype,mdate,mamt,isrenew,renewacn,parent,[plan])"
        query &= "VALUES(@date,@acno,@cid,@serial,@product,@amount,@cint,@dint,@cld,@prd,@prdtype,@mdate,@mamt,@isrenew,@renewacn,@parent,@plan)"

        cmd.CommandText = query
        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(ddt.Text))
        cmd.Parameters.AddWithValue("@acno", Session("acn"))
        cmd.Parameters.AddWithValue("@cid", txtcid.Text)
        cmd.Parameters.AddWithValue("@serial", serial)
        cmd.Parameters.AddWithValue("@product", deptyp.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@amount", 0)
        cmd.Parameters.AddWithValue("@cint", cintr.Text)
        cmd.Parameters.AddWithValue("@dint", dint.Text)
        cmd.Parameters.AddWithValue("@cld", 0)
        cmd.Parameters.AddWithValue("@prd", prd.Text)
        cmd.Parameters.AddWithValue("@prdtype", Session("prdtyp"))
        cmd.Parameters.AddWithValue("@mdate", Session("mdt"))
        cmd.Parameters.AddWithValue("@mamt", 0)
        cmd.Parameters.AddWithValue("@isrenew", rnew)
        cmd.Parameters.AddWithValue("@renewacn", Trim(txtrenewacn.Text))
        cmd.Parameters.AddWithValue("@parent", Session("acn"))
        cmd.Parameters.AddWithValue("@plan", plan)

        Try

            cmd.ExecuteNonQuery()

            '  lblacn.Text = " Account No is  " + acn

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
            cmd.Dispose()
            cmd = New SqlCommand

        End Try

        If Not txtsiacn.Text = "" Then

            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con
            cmd.Parameters.Clear()

            query = "insert into stdins(srcproduct,acno,product,siacno,sidate)"
            query &= "values(@srcproduct,@acno,@product,@siacno,@sidate)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@srcproduct", deptyp.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@acno", Session("acn"))
            cmd.Parameters.AddWithValue("@product", "SB")
            cmd.Parameters.AddWithValue("@siacno", txtsiacn.Text)
            cmd.Parameters.AddWithValue("@sidate", Left(ddt.Text, 2))

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()
                cmd = New SqlCommand

            End Try

            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con
            cmd.Parameters.Clear()

            query = "insert into stdinsc(srcproduct,acno,product,siacno,sidate)"
            query &= "values(@srcproduct,@acno,@product,@siacno,@sidate)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@srcproduct", deptyp.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@acno", Session("acn"))
            cmd.Parameters.AddWithValue("@product", "SB")
            cmd.Parameters.AddWithValue("@siacno", txtsiacn.Text)
            cmd.Parameters.AddWithValue("@sidate", Left(ddt.Text, 2))

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()
                cmd = New SqlCommand

            End Try


        End If

        If con.State = ConnectionState.Closed Then con.Open()
        query = "insert into nominee(acno,nominee,address,relation,age,dob,gurd,gurdadd)"
        query &= "values(@acno,@nominee,@address,@relation,@age,@dob,@gurd,@gurdadd)"
        cmd.Parameters.Clear()
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@acno", Session("acn"))
        cmd.Parameters.AddWithValue("@nominee", txtnominee.Text)
        cmd.Parameters.AddWithValue("@address", txtnadd.Text)
        cmd.Parameters.AddWithValue("@relation", txtrelation.Text)
        cmd.Parameters.AddWithValue("@age", txtage.Text)
        cmd.Parameters.AddWithValue("@dob", txtdob.Text)
        cmd.Parameters.AddWithValue("@gurd", txtgurd.Text)
        cmd.Parameters.AddWithValue("@gurdadd", txtgrdadd.Text)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            cmd = New SqlCommand
            con.Close()
        End Try



        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Account Opened. Account No " + Session("acn") + " &nbsp;&nbsp;&nbsp;<a href=\depositreceipt.aspx\>&nbsp;&nbsp;Click here to Update the Receipt</a>"
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

        clear_frm()
    End Sub
    Sub clear_frm()
        Session("renewalint") = 0
        Session("campain") = 0
        isrenew.Checked = False
        txtname.Text = ""
        txtadd.Text = ""
        txtcof.Text = ""
        txtcid.Text = ""
        prd.Text = ""
        cintr.Text = 0
        dint.Text = 0
        imgCapture.Visible = False

        chkRenewPlan.Checked = False
        chkSeniorPlan.Checked = False
        chkTransferPlan.Checked = False
        chkRegularPlan.Checked = False


        ' prddmy.SelectedIndex = 0
        txtsiacn.Text = ""
        cust.Text = ""
        txtnominee.Text = ""
        txtnadd.Text = ""
        txtrelation.Text = ""
        txtage.Text = ""
        txtdob.Text = ""
        txtgurd.Text = ""
        txtgrdadd.Text = ""
        txtsiacn.Text = ""
        cust.Text = ""
        txtgrpid.Text = ""
        lblgrp.InnerHtml = ""
        Session("Tabin") = 0

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showCtab", "ShowCTab()", True)
        txtcid.Focus()

    End Sub

    Sub fetch_cust()

        Dim imgx As Image = Nothing
        Dim imgbytes As Byte() = Nothing
        Dim stream As MemoryStream = Nothing

        Dim ds As New DataSet
        'Dim i As Integer

        If con.State = ConnectionState.Closed Then con.Open()
        Dim act = False

        Dim sql As String = "select FirstName,LastName,Address,active from member where MemberNo = '" + Trim(txtcid.Text) + "'"


        Try

            Dim adapter As New SqlDataAdapter(sql, con)
            '  lblcid404.Visible = False
            adapter.Fill(ds)

            If Not ds.Tables(0).Rows.Count = 0 Then

                act = ds.Tables(0).Rows(0).ItemArray(3)

                If act = True Then
                    txtname.Text = Trim(ds.Tables(0).Rows(0).ItemArray(0).ToString)
                    txtcof.Text = Trim(ds.Tables(0).Rows(0).ItemArray(1).ToString)
                    txtadd.Text = Trim(ds.Tables(0).Rows(0).ItemArray(2).ToString)
                Else
                    Dim stitle = "Hi " + Session("sesusr")
                    Dim msg = "In Active Member "
                    Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

                    clear_frm()

                End If



            Else
                '  lblcid404.Visible = True
                txtfocus(txtcid)
                Exit Sub
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally

            con.Close()

        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select photo from kyc where kyc.memberno='" + Trim(txtcid.Text) + "'"
        cmd.Connection = con
        cmd.CommandText = query

        Try
            Dim dr As SqlDataReader = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()



                If Not IsDBNull(dr(0)) Then
                    imgbytes = CType(dr.GetValue(0), Byte())
                    stream = New MemoryStream(imgbytes, 0, imgbytes.Length)
                    imgx = Image.FromStream(stream)
                    '  imgx.Save("../captures/webcam.png", System.Drawing.Imaging.ImageFormat.Png)


                    Dim imagePath As String = String.Format("~/Captures/{0}.png", Trim(txtcid.Text))
                    File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                    'Session("CapturedImage") = ResolveUrl(imagePath)
                    imgCapture.ImageUrl = "~/captures/" + Trim(txtcid.Text) + ".png?" + DateTime.Now.Ticks.ToString()
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
        End Try


    End Sub

    Private Sub btnclr_Click(sender As Object, e As EventArgs) Handles btnclr.Click
        clear_frm()
        'Me.ModalPopup1.Show()

    End Sub




    Private Sub deptyp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles deptyp.SelectedIndexChanged
        'If Trim(deptyp.SelectedItem.Text) = "FD" Or Trim(deptyp.SelectedItem.Text) = "RD" Then
        Session("Tabin") = 1
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showNtab", "ShowNTab()", True)

        btnsave.Text = "Next"

        get_grpid(txtcid.Text)
        txtfocus(ddt)

        '  End If


    End Sub

    Private Sub btn_si_update_Click(sender As Object, e As EventArgs) Handles btn_si_update.Click

        btn_si_update.Enabled = False
        open_deposit(deptyp.SelectedItem.Text)

    End Sub


    Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged

        fetch_cust()

        ddt.Text = Format(Now, "dd-MM-yyyy")
        'System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.ddt)
        txtfocus(deptyp)

    End Sub

    Private Sub btn_da1_clr_Click(sender As Object, e As EventArgs) Handles btn_da1_clr.Click
        txtnominee.Text = ""
        txtnadd.Text = ""
        txtrelation.Text = ""
        txtage.Text = ""
        txtdob.Text = ""
        txtgurd.Text = ""
        txtgrdadd.Text = ""
        txtfocus(txtnominee)

    End Sub

    Private Sub btn_da1_nxt_Click(sender As Object, e As EventArgs) Handles btn_da1_nxt.Click
        ' Session("Tabin") = 1

        If Trim(deptyp.SelectedItem.Text) = "FD" Or Trim(deptyp.SelectedItem.Text) = "RD" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showdltab", "ShowSTab()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showDLStab", "ShowDLS(2)", True)
            txtfocus(txtsiacn)
        Else
            btn_da1_nxt.Enabled = False
            open_deposit(Trim(deptyp.SelectedItem.Text))

        End If
    End Sub

    Public Sub get_grpid(ByVal cid As String)

        'grptr.Visible = True
        Dim sql As String = "SELECT KYC.Parent, member.FirstName  FROM dbo.member INNER JOIN dbo.KYC  ON member.MemberNo = KYC.Parent WHERE KYC.MemberNo ='" + Trim(cid) + "'"
        Dim adapter As New SqlDataAdapter(sql, con)


        adapter.Fill(ds)

        If Not ds.Tables(0).Rows.Count = 0 Then

            txtgrpid.Text = ds.Tables(0).Rows(0).Item(0)
            lblgrp.InnerHtml = ds.Tables(0).Rows(0).Item(1)
        End If

    End Sub



    Public Sub get_ac_info(ByVal acn As String)



        Dim sql As String = "SELECT Master.date,Master.acno,Master.product,Master.amount,Master.cint,Master.dint,Master.prd,Master.prdtype,Master.mdate,Master.mamt,Master.cid FROM dbo.master WHERE master.acno = '" + acn + "'and cld='0'"

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

                'TabPanel1.Visible = True
                'TabContainer1.Visible = True
                'tabpanel2.Visible = True
                'tabpanel3.Visible = True
                'denom_tab.Visible = False
                'txtacn.Enabled = False


            Else
                '                closure_notice.Visible = True
                '               lblmat.Text = "Invalid Account or Already Closed"

            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
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

                imgCapture.ImageUrl = "~/ShowImage.ashx?id=" & Session("ncid")
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            con.Close()

            adapter1.Dispose()

        End Try

    End Sub


    Private Sub txtgrpid_TextChanged(sender As Object, e As EventArgs) Handles txtgrpid.TextChanged
        If Not Trim(txtgrpid.Text) = "" Then

            Dim sql As String = "SELECT  member.FirstName  FROM dbo.member where MemberNo ='" + Trim(txtgrpid.Text) + "'"
            Dim adapter As New SqlDataAdapter(sql, con)


            adapter.Fill(ds)

            If Not ds.Tables(0).Rows.Count = 0 Then

                ''txtgrpid.Text = ds.Tables(0).Rows(0).Item(0)
                lblgrp.InnerHtml = Trim(ds.Tables(0).Rows(0).Item(0))
            Else
                txtgrpid.Text = ""
                lblgrp.InnerHtml = ""
                txtfocus(txtgrpid)
            End If
        End If

    End Sub


    Private Sub chkRenewPlan_CheckedChanged(sender As Object, e As EventArgs) _
    Handles chkRenewPlan.CheckedChanged

        Session("renewalint") = 0

        If chkRenewPlan.Checked Then
            txtrenewacn.Visible = True
            txtrenewacn.Focus()
        Else
            txtrenewacn.Visible = False
            prd.Focus()
        End If

    End Sub


    Private Sub prd_TextChanged(sender As Object, e As EventArgs) Handles prd.TextChanged


        getint()
    End Sub

    Private Sub txtsiacn_TextChanged(sender As Object, e As EventArgs) Handles txtsiacn.TextChanged

    End Sub

    Private Sub DepositOpening_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btn_si_upclr_Click(sender As Object, e As EventArgs) Handles btn_si_upclr.Click
        txtsiacn.Text = ""
        cust.Text = ""
    End Sub



    Private Sub txtotp_TextChanged(sender As Object, e As EventArgs) Handles txtotp.TextChanged

        If Not Trim(txtotp.Text) = "" Then
            If Trim(txtotp.Text) = Session("otp") Then
                btn_si_update.Enabled = True
            Else
                txtotp.Text = "Invalid OTP"

            End If
        End If

    End Sub
End Class