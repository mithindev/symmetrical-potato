Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Globalization
Imports System.Drawing.Imaging

Public Class LoanReceipt
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdy As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Dim newrow As DataRow

    Dim countresult As Integer
    Public dt_dl As New DataTable
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim yearpart As String
    Public dt As New DataTable
    Public dtdl As New DataTable
    Public dc As New DataColumn
    Public tqty As Decimal
    Public tgross As Decimal
    Public tnet As Decimal
    Dim sb_srch As Boolean
    Dim stitle As String = String.Empty
    Dim msg As String = String.Empty
    Dim fnc As String = String.Empty





    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then

            If Not session_user_role = "Admin" Then
                lblactualint.Enabled = False
                lblactualint_d.Enabled = False
            Else
                lblactualint.Enabled = True
                lblactualint_d.Enabled = True

            End If


            If Session("orgin") = "loananalysis" Then
                txtacn.Text = Session("auctionacn")

                get_ac_info(txtacn.Text)
                txtamt.Text = Session("os")
                txtothers.Text = Session("ovr")


            Else

                If Not Session("acn") = Nothing Then
                    '    txtacn.Text = Session("acn")
                    '   get_ac_info(Session("acn"))

                End If
                ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtacn.ClientID), True)



            End If

        End If

    End Sub


    Public Sub get_unclr()

        Session("unclr_credit_c") = 0
        Session("unclr_debit_c") = 0

        Session("unclr_credit_d") = 0
        Session("unclr_debit_d") = 0

        If con.State = ConnectionState.Closed Then con.Open()


        'Dim sql As String = "SELECT SUM([trans].Drd) AS expr1,SUM([trans].Crd) AS expr2, FROM dbo.[trans] WHERE [trans].acno ='" + acn + "' GROUP BY [trans].acno"
        Dim sql As String = "SELECT SUM([trans].Drd) AS expr1,SUM([trans].Crd) AS expr2,SUM([trans].Drc) AS expr3,SUM([trans].Crc) AS expr4 FROM dbo.[trans] WHERE [trans].acno ='" + txtacn.Text + "' and scroll='0' GROUP BY [trans].acno "
        cmd.Connection = con
        cmd.CommandText = sql

        Try
            Dim reader_uc As SqlDataReader = cmd.ExecuteReader()

            If reader_uc.HasRows() Then

                reader_uc.Read()
                Session("unclr_credit_d") = CDbl(reader_uc(1).ToString)
                Session("unclr_debit_d") = CDbl(reader_uc(0).ToString)
                Session("unclr_credit_c") = CDbl(reader_uc(3).ToString)
                Session("unclr_debit_c") = CDbl(reader_uc(2).ToString)
            End If

            reader_uc.Close()


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



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
    Public Function get_balance(ByVal acn As String)


        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' GROUP BY [actrans].acno"
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
                Session("dbal") = (reader1(1).ToString - reader1(0).ToString) + -(Session("unclr_debit_d") - Session("unclr_credit_d"))
                Session("cbal") = (reader1(3).ToString - reader1(2).ToString) + -(Session("unclr_debit_c") - Session("unclr_credit_c"))

                lblccr.Text = FormatCurrency(reader1(3).ToString + Session("unclr_credit_c"))
                lblcdr.Text = FormatCurrency(reader1(2).ToString + Session("unclr_debit_c"))
                lblcbal.Text = FormatCurrency(Session("cbal"))

                lbldcr.Text = FormatCurrency(reader1(1).ToString + Session("unclr_credit_d"))
                lblddr.Text = FormatCurrency(reader1(0).ToString + Session("unclr_debit_d"))
                lbldbal.Text = FormatCurrency(Session("dbal"))

                reader1.Close()


            Else
                Session("cbal") = 0
                Session("dbal") = 0
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally

        End Try

        Return Session("dbal")

    End Function
    Public Sub get_ac_info(ByVal acn As String)
        txtins.Text = 0
        txtothers.Text = 0

        Dim sql As String = "SELECT Master.date,Master.acno,Master.product,Master.amount,Master.cint,Master.dint,Master.prd,Master.prdtype,Master.mdate,Master.mamt,Master.cid,Master.sch,Master.IsOtherBranchShareholder FROM dbo.master WHERE master.acno = '" + acn + "' and cld='0'"

        Dim adapter As New SqlDataAdapter(sql, con)
        ' closure_notice.Visible = False

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
                Session("prdtyp") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(7)), "D", ds.Tables(0).Rows(0).Item(7))
                Session("mdt") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(8)), Session("ac_date"), ds.Tables(0).Rows(0).Item(8))
                'mamt = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(9)), 0, ds.Tables(0).Rows(0).Item(9))
                '   mdt = ds.Tables(0).Rows(0).Item(8)
                '   mamt = ds.Tables(0).Rows(0).Item(9)
                Session("ncid") = ds.Tables(0).Rows(0).Item(10)
                Session("scheme") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(11)), "", ds.Tables(0).Rows(0).Item(11))
                Session("IsOtherBranchShareholder") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(12)), False, ds.Tables(0).Rows(0).Item(12))



                txtacn.Enabled = False
                soa.Enabled = True

                If CBool(Session("IsOtherBranchShareholder")) Then
                    lblShareHolderNew.Visible = True
                Else
                    lblShareHolderNew.Visible = False
                End If

                '  get_unclr()

                pnlact.Visible = True
                pnlint.Visible = True
                pnlbtn.Visible = True


            Else
                ' closure_notice.Visible = True
                stitle = "Hi " + Session("sesusr")
                msg = "Invalid Account or Already Closed"
                fnc = "showToastnOK('" + stitle + "','" + msg + "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


                Exit Sub
            End If

        Catch ex As Exception
            ' Response.Write(ex.ToString)
            stitle = "Hi " + Session("sesusr")
            msg = "An error occurred fetching account info. Please check logs."
            '"Invalid Account or Already Closed"
            fnc = "showToastnOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

        Finally

            cmd.Dispose()
            con.Close()


        End Try


        If con.State = ConnectionState.Closed Then con.Open()

        sql = "SELECT FirstName,lastname,address,mobile from dbo.member where MemberNo='" + Session("ncid") + "'"

        Dim adapter1 As New SqlDataAdapter(sql, con)

        Try

            adapter1.Fill(ds1)

            If Not ds1.Tables(0).Rows.Count = 0 Then
                Session("ac_name") = ds1.Tables(0).Rows(0).Item(0)
                Session("ac_lname") = ds1.Tables(0).Rows(0).Item(1)
                Session("address") = ds1.Tables(0).Rows(0).Item(2)
                Session("mobile") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(3)), "", ds1.Tables(0).Rows(0).Item(3))
                
                lblname.Text = Session("ac_name").ToString() & " " & Session("ac_lname").ToString()
                lbladd.Text = Session("address").ToString()
                lblmobile.Text = Session("mobile").ToString()
                
                LoadJewelImages(acn)
                get_Img(Session("ncid").ToString())

            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



        adapter1.Dispose()


        lblbal.Text = FormatCurrency(get_balance(acn))
        lblproduct.Text = Session("product")
        lblname.Text = Session("ac_name")
        lblsch.Text = Session("scheme")
        lblamt.Text = FormatCurrency(Session("amt"))
        Session("mdt") = get_last_transaction(Session("acn"))


        adapter.Dispose()
        If Session("product") = "JL" Then showjltab(Session("acn").ToString)


        If Session("product") = "DL" Then

            get_dl_int()

        Else


            If Session("product") = "JL" Then

                Dim iseligible = IIf(Session("scheme") = "PRIME", 0, 1)
                If Session("scheme") = "PRIME ULTRA" Then iseligible = 0

                If iseligible = 1 Then
                    check4rebate()
                Else
                    Session("rebate") = 0
                End If

            End If


            get_cintr()
            get_int()

        End If



        ' ClientScript.RegisterStartupScript(Me.[GetType](), "SetFocus", "<script language=""Jscript"" > document.getElementById(""txtamt"").focus(); </Script>", True)
        '  ClientScript.RegisterStartupScript(Me.[GetType](), "Focus", "document.getElementById('<%=txtamt.ClientID()%>').focus();", True)
        ' ScriptManager1.SetFocus(txtamt)

        chk4auction()

        txtfocus(txtamt)





    End Sub

    Private Sub get_Img(ByVal memberno As String)
        Dim imgx As Image = Nothing
        Dim imgbytes As Byte() = Nothing
        Dim stream As MemoryStream = Nothing

        Dim con2 As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
        con2.Open()

        Dim query As String = "select photo from kyc where kyc.memberno=@memberno"
        Dim cmd2 As New SqlClient.SqlCommand(query, con2)
        cmd2.Parameters.AddWithValue("@memberno", Trim(memberno))

        Try
            Dim dr As SqlDataReader = cmd2.ExecuteReader()

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
            imgCapture.Visible = False
        Finally
            cmd2.Dispose()
            con2.Close()
        End Try
    End Sub

    Private Sub LoadJewelImages(ByVal acn As String)
        phJewelImages.Controls.Clear()
        Dim con3 As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
        con3.Open()
        Dim q As String = "SELECT ImageData FROM jlspec WHERE acno = @acno AND ImageData IS NOT NULL"
        Dim cmd3 As New SqlClient.SqlCommand(q, con3)
        cmd3.Parameters.AddWithValue("@acno", Trim(acn))
        Try
            Dim dr3 As SqlDataReader = cmd3.ExecuteReader()
            Dim idx As Integer = 0
            While dr3.Read()
                Dim imgbytes As Byte() = CType(dr3.GetValue(0), Byte())
                Dim imagePath As String = String.Format("~/Captures/jl_{0}_{1}.png", Trim(acn), idx)
                File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                Dim imgUrl As String = String.Format("~/Captures/jl_{0}_{1}.png?{2}", Trim(acn), idx, DateTime.Now.Ticks.ToString())
                Dim resolvedUrl As String = ResolveUrl(imgUrl)
                Dim img As New System.Web.UI.WebControls.Image()
                img.ImageUrl = imgUrl
                img.Style("width") = "60px"
                img.Style("height") = "60px"
                img.Style("border-radius") = "50%"
                img.Style("object-fit") = "cover"
                img.Style("margin-right") = "6px"
                img.Style("cursor") = "pointer"
                img.CssClass = "img-thumbnail"
                img.Attributes.Add("onclick", String.Format("openJewelModal('{0}')", resolvedUrl))
                img.ToolTip = "Click to view larger"
                phJewelImages.Controls.Add(img)
                idx += 1
            End While
            dr3.Close()
        Catch ex As Exception
            ' silently fail — no jewel images shown on error
        Finally
            cmd3.Dispose()
            con3.Close()
        End Try
    End Sub

    Function calculate_penalty(ByVal acn As String, ByVal acdt As Date)





        If con1.State = ConnectionState.Closed Then con1.Open()


        Dim sql As String = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].type<>'INTR' "

        cmdi.Connection = con1
        cmdi.CommandText = sql

        countresult = cmdi.ExecuteScalar()

        countresult = IIf(IsDBNull(countresult), 0, countresult)


        Dim rdlate As Integer = DateDiff(DateInterval.Month, acdt, Convert.ToDateTime(tdate.Text)) - CDbl(countresult)


        Return rdlate

    End Function

    Function chk4obdep(ByVal acn As String)

        Dim isobdep As Integer = 1

        query = "select lacno,depacno from DEPOBR where lacno='" + acn + "'"
        If con1.State = ConnectionState.Closed Then con1.Open()
        cmdy.Connection = con1
        cmdy.CommandText = query
        Dim dry As SqlDataReader
        dry = cmdy.ExecuteReader

        If dry.HasRows Then isobdep = 0


        Return isobdep

    End Function

    Function chk4deposit(ByVal parentid As String)


        Dim hasdeposit As Integer = 1


        query = "SELECT KYC.MemberNo, master.product,  master.acno,master.date FROM dbo.KYC INNER JOIN dbo.master   ON KYC.MemberNo = master.cid "
        query &= " WHERE KYC.Parent = @gid AND master.cld = 0 AND (master.product = 'RID' OR master.product= 'FD')"

        cmdx.Parameters.Clear()

        cmdx.CommandText = query
        cmdx.Parameters.AddWithValue("@gid", parentid)

        Dim drX As SqlDataReader

        drX = cmdx.ExecuteReader()

        If drX.HasRows() Then
            drX.Read()
            hasdeposit = 0
        Else
            hasdeposit = chk4obdep(Trim(txtacn.Text))
        End If
        drX.Close()

        Return hasdeposit

    End Function

    Sub check4rebate()

        Session("rdduepending") = 1


        Dim rdlate As Integer = 1
        Dim parentid As String
        If con.State = ConnectionState.Closed Then con.Open()


        query = "SELECT COALESCE(KYC.Parent,'N') FROM dbo.kyc where kyc.MemberNo=@cid"

        cmd.Parameters.Clear()
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@cid", Trim(Session("ncid")))
        cmd.Connection = con
        Try

            parentid = cmd.ExecuteScalar()

            If Trim(parentid) = "N" Then parentid = Trim(Session("ncid"))

            If Len(Trim(parentid)) < 10 Then parentid = Trim(Session("ncid"))


            query = "SELECT KYC.MemberNo, master.product,  master.acno,master.date FROM dbo.KYC INNER JOIN dbo.master   ON KYC.MemberNo = master.cid "
            query &= " WHERE KYC.Parent = @gid AND master.product = 'RD' AND master.cld = 0"

            cmd.Parameters.Clear()

            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@gid", parentid)

            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then

                While dr.Read()

                    rdlate = calculate_penalty(dr(2), dr(3))

                    If rdlate <= 1 Then Session("rdduepending") = 0


                End While

            Else


                Session("rdduepending") = chk4deposit(parentid)


            End If





            dr.Close()


            cmd.Dispose()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


        If Session("rdduepending") = 0 Then

            query = "SELECT TOP 1 isnull(goldrate.rebate,0) FROM dbo.goldrate  ORDER BY date DESC "

            cmd.CommandText = query
            cmd.Connection = con
            Try
                Session("rebate") = cmd.ExecuteScalar


            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try


        Else
            Session("rebate") = 0

        End If


    End Sub
    Sub chk4auction()
        Dim recovry As Integer
        If con.State = ConnectionState.Closed Then con.Open()
        query = "SELECT jlauction.cid,SUM(jlauction.debit) AS expr1,SUM(jlauction.credit) AS expr2 FROM dbo.jlauction WHERE jlauction.cid = @cid GROUP BY jlauction.cid"

        cmdy.Connection = con
        cmdy.CommandText = query
        cmdy.Parameters.Clear()
        cmdy.Parameters.AddWithValue("@cid", Session("ncid"))
        Try
            Dim dras As SqlDataReader

            dras = cmdy.ExecuteReader()

            If dras.HasRows() Then
                dras.Read()

                recovry = dras(1) - dras(2)
                If recovry > 0 Then
                    jlrecovery.Visible = True
                    lbl_acn_loss.Text = FormatCurrency(recovry)
                    '  txtrecovery.Text = 0
                    txtfocus(txtamt)
                End If

            End If
            cmdy.Dispose()

            dras.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)

        End Try
    End Sub
    Function get_dep_agst()
        Dim pro As String = ""
        Dim PROX As String = ""

        query = "select deposit from dlspec where acn=@acn ORDER BY dlspec.deposit DESC"
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@acn", txtacn.Text)
        Try
            pro = cmd.ExecuteScalar()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Select Case Mid(pro, 1, 2)
            Case 74
                PROX = "DS"
            Case 75
                PROX = "FD"
            Case 76
                PROX = "KMK"
            Case 77
                PROX = "RD"
            Case 78
                PROX = "RID"
            Case 79
                PROX = "SB"
        End Select

        get_agst_info(pro)

        Return PROX

    End Function

    Sub get_agst_info(ByVal acn As String)

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()
        cmd.Connection = con

        query = "SELECT DATE,PRD FROM MASTER where acno=@acn"


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@acn", acn)

        Dim dr As SqlDataReader

        Try
            dr = cmd.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                Session("agst_acdate") = dr(0)
                Session("agst_prd") = dr(1)
            End If

            dr.Close()


        Catch ex As Exception
            Response.Write(ex.ToString.ToString)
        End Try

    End Sub
    Sub get_dl_int()
        Dim transDone As Boolean = False

        Try
            transDone = prv_int()
            Dim pro As String = get_dep_agst()

            Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(tdate.Text))
            Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("prv_dinton"), CDate(tdate.Text))


            'If Session("prd_buffer_d") > 30 Then

            '    mnt = Math.Round(CType(Session("prd_buffer_d"), Integer) / 30)
            '    mnt = Session("agst_prd")
            'Else
            '    mnt = 1
            'End If

            If Session("prd_buffer").ToString = 0 Then Session("prd_buffer") = 7
            If Session("prd_buffer_d").ToString = 0 Then Session("prd_buffer_d") = 7

            'query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy and agst=@agst  order by fyfrm desc"
            query = "SELECT cint,dint,FYFRM,FYTO,prdfrm,prdto FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND agst=@agst  order by fyfrm desc"

            Dim dr_roi As SqlDataReader


            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()
            cmd.Connection = con


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@prod", Session("product"))
            cmd.Parameters.AddWithValue("@prdtyp", "M")
            'cmd.Parameters.AddWithValue("@prdx", mnt)
            'cmd.Parameters.AddWithValue("@prdy", mnt)
            cmd.Parameters.AddWithValue("@agst", pro)




            dr_roi = cmd.ExecuteReader()

            If dr_roi.HasRows() Then
                dr_roi.Read()

                While dr_roi.Read

                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)


                    Dim x As Long = FYFRM.CompareTo(Session("agst_acdate"))

                    If x = -1 Then

                        If dr_roi(4) <= Session("agst_prd") And dr_roi(5) = Session("agst_prd") Then
                            Dim y As Long = FYTO.CompareTo(Session("agst_acdate"))

                            If y = 1 Then
                                Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                                Session("dint") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                                Exit While

                            End If
                        End If
                    End If
                End While


            End If

            dr_roi.Close()



            If Not transDone And Session("prv_inton").Equals(Session("ac_date")) Then

                If Session("prd_buffer") <= 7 Then Session("prd_buffer") = 7

                If Session("prd_buffer_d") <= 7 Then Session("prd_buffer_d") = 7

                ' If prd_buffer <= 7 Then prd_buffer = 7

                ' If prd_buffer_d <= 7 Then prd_buffer_d = 7

            End If


            If transDone And Session("prv_inton").Equals(Convert.ToDateTime(tdate.Text)) Then
                Session("prd_buffer_d") = 0
                Session("prd_buffer") = 0
            Else
                If Session("totint") < 5 Then Session("totint") = 5
                If Session("totalint_d") < 5 Then Session("totalint_d") = 5
            End If



            Session("totint") = Math.Round((((-Session("cbal")) * Session("cintr") / 100) / 365) * Session("prd_buffer"))
            Session("totalint_d") = Math.Round((((-Session("dbal")) * Session("dint") / 100) / 365) * Session("prd_buffer_d"))


            lblperiod.Text = CStr(Session("prd_buffer")) + " Days"
            lblroi.Text = CStr(Session("cintr")) + "%"
            lblactualint.Text = FormatCurrency(Session("totint"))

            lblperiod_d.Text = CStr(Session("prd_buffer_d")) + " Days"
            lblroi_d.Text = CStr(Session("dint")) + "%"
            lblactualint_d.Text = FormatCurrency(Session("totalint_d"))

            txtamt.Text = (-(Session("dbal") - Session("totalint_d")))
            ' lblos.Text = (-(Session("dbal") - Session("totalint_d")))

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub


    Sub get_cintr()

        Dim dr_roi As SqlDataReader

        Try
            Dim transDone As Boolean = prv_int()
            Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(tdate.Text))
            Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("prv_dinton"), CDate(tdate.Text))

            If Session("prd_buffer").ToString = 0 Then Session("prd_buffer") = 7
            If Session("prd_buffer_d").ToString = 0 Then Session("prd_buffer_d") = 7

            If Session("product") = "JL" Or Session("product") = "ML" Then
                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.agst=@agst AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
            Else
                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND  roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
            End If



            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()
            cmd.Connection = con


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@prod", Session("product"))
            cmd.Parameters.AddWithValue("@prdtyp", Session("prdtyp"))
            cmd.Parameters.AddWithValue("@prdx", Session("prd_buffer"))
            cmd.Parameters.AddWithValue("@prdy", Session("prd_buffer"))


            If Session("product") = "JL" Or Session("product") = "ML" Then
                cmd.Parameters.AddWithValue("@agst", Session("scheme"))
            End If



            dr_roi = cmd.ExecuteReader()

            If dr_roi.HasRows() Then
                ' dr_roi.Read()

                While dr_roi.Read

                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)


                    Dim x As Long = FYFRM.CompareTo(Session("ac_date"))

                    If x = -1 Then


                        Dim y As Long = FYTO.CompareTo(Session("ac_date"))

                        If y = 1 Then
                            Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                            ' Session("dint") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                            Exit While

                        End If

                    End If
                End While


            End If


            dr_roi.Close()

            ''   Session("cintr") = Session("cintr") - Session("rebate")


            If Not transDone And Session("prv_inton").Equals(Session("ac_date")) Then

                If Session("prd_buffer") <= 7 Then Session("prd_buffer") = 7

                If Session("prd_buffer_d") <= 7 Then Session("prd_buffer_d") = 7

                ' If prd_buffer <= 7 Then prd_buffer = 7

                ' If prd_buffer_d <= 7 Then prd_buffer_d = 7

            End If

            Session("totint") = Math.Round((((-Session("cbal")) * Session("cintr") / 100) / 365) * Session("prd_buffer"))
            '   Session("totalint_d") = Math.Round((((-Session("dbal")) * Session("dint") / 100) / 365) * Session("prd_buffer_d"))

            If Session("totint") < 5 Then Session("totint") = 5
            If Session("totalint_d") < 5 Then Session("totalint_d") = 5
            lblperiod.Text = CStr(Session("prd_buffer")) + " Days"
            lblroi.Text = CStr(Session("cintr")) + "%"
            lblactualint.Text = FormatCurrency(Session("totint"))

            'lblperiod_d.Text = CStr(Session("prd_buffer_d")) + " Days"
            'lblroi_d.Text = CStr(Session("dint")) + "%"
            'lblactualint_d.Text = FormatCurrency(Session("totalint_d"))

            'txtamt.Text = (-(Session("dbal") - Session("totalint_d")))


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub

    Function get_share_value(ByVal cid As String)
        Dim shr As Integer = 0
        If con.State = ConnectionState.Closed Then con.Open()

        query = "SELECT SUM(share.allocation) AS expr1 FROM dbo.share WHERE share.cid = @cid"
        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@cid", cid)

        Try
            Dim obj As Object = cmd.ExecuteScalar()
            If Not IsDBNull(obj) Then
                shr = obj
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
        Return shr
    End Function


    Sub get_int()
        Dim dr_roi As SqlDataReader
        Try
            Dim transDone As Boolean = prv_int()
            If lblproduct.Text = "DCL" Then
                Session("prd_buffer") = DateDiff(DateInterval.Day, Session("ac_date"), CDate(tdate.Text))
                Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("ac_date"), CDate(tdate.Text))
            Else

                Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(tdate.Text))
                Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("prv_dinton"), CDate(tdate.Text))
            End If


            '      If Session("prd_buffer").ToString = 0 Then Session("prd_buffer") = 7
            '    If Session("prd_buffer_d").ToString = 0 Then Session("prd_buffer_d") = 7


            If Session("product") = "ML" Then

                Dim shrval As Integer = get_share_value(Session("ncid"))

            '     Dim scriptDebug As String =
            '     "alert('DEBUG INFO:\n" &
            '     "shrval: " & shrval.ToString() & "\n" &
            '     "');"

            ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid().ToString(), scriptDebug, True)


                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy AND minsc=@shr AND agst=@sch order by fyfrm"


                If con.State = ConnectionState.Closed Then con.Open()

                cmd.Parameters.Clear()
                cmd.Connection = con


                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@prod", Session("product"))
                cmd.Parameters.AddWithValue("@prdtyp", "D")
                cmd.Parameters.AddWithValue("@prdx", 10)
                cmd.Parameters.AddWithValue("@prdy", 10)
                cmd.Parameters.AddWithValue("@sch", Session("scheme"))

                ' If shrval < 500 Then
                '     cmd.Parameters.AddWithValue("@shr", 49)
                ' Else
                '     cmd.Parameters.AddWithValue("@shr", 51)
                ' End If

                If shrval < 100 AndAlso Not CBool(Session("IsOtherBranchShareholder")) Then
                    cmd.Parameters.AddWithValue("@shr", 99)
                Else
                    cmd.Parameters.AddWithValue("@shr", 101)
                End If

                dr_roi = cmd.ExecuteReader()

                If dr_roi.HasRows() Then
                    '  dr_roi.Read()

                    While dr_roi.Read

                        Dim FYFRM As Date = dr_roi(2)
                        Dim FYTO As Date = dr_roi(3)


                        Dim x As Long = FYFRM.CompareTo(Session("ac_date"))

                        If x = -1 Then


                            Dim y As Long = FYTO.CompareTo(Session("ac_date"))

                            If y = 1 Then

                                If Session("product") = "ML" Then
                                    Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                                End If
                                Session("dint") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                                Exit While

                            End If

                        End If
                    End While



                    dr_roi.Close()
                End If


            Else

                If Session("product") = "JL" Then
                    query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.agst=@agst AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
                Else
                    query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
                End If



                If con.State = ConnectionState.Closed Then con.Open()

                cmd.Parameters.Clear()
                cmd.Connection = con


                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@prod", Session("product"))
                cmd.Parameters.AddWithValue("@prdtyp", "D")
                cmd.Parameters.AddWithValue("@prdx", Session("prd_buffer_d"))
                cmd.Parameters.AddWithValue("@prdy", Session("prd_buffer_d"))
                If Session("product") = "JL" Then
                    cmd.Parameters.AddWithValue("@agst", Session("scheme"))
                End If






                dr_roi = cmd.ExecuteReader()

                If dr_roi.HasRows() Then
                    '  dr_roi.Read()

                    While dr_roi.Read

                        Dim FYFRM As Date = dr_roi(2)
                        Dim FYTO As Date = dr_roi(3)


                        Dim x As Long = FYFRM.CompareTo(Session("ac_date"))

                        If x = -1 Then


                            Dim y As Long = FYTO.CompareTo(Session("ac_date"))

                            If y = 1 Then

                                If Session("product") = "ML" Then
                                    Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                                End If
                                Session("dint") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                                Exit While

                            End If

                        End If
                    End While



                    dr_roi.Close()
                End If

            End If


            If Not transDone And Session("prv_inton").Equals(Session("ac_date")) Then

                If Session("prd_buffer") <= 7 Then Session("prd_buffer") = 7

                If Session("prd_buffer_d") <= 7 Then Session("prd_buffer_d") = 7

                ' If prd_buffer <= 7 Then prd_buffer = 7

                ' If prd_buffer_d <= 7 Then prd_buffer_d = 7





            End If

            If lblproduct.Text = "DCL" Then
                If Session("prd_buffer") <= 120 Then
                    Session("cbalx") = 0
                    Session("dbalx") = 0
                Else
                    '' Session("prd_buffer_d") = Session("prd_buffer_d") - 120
                    ''Session("prd_buffer") = Session("prd_buffer") - 120
                    Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(tdate.Text))
                    Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("prv_dinton"), CDate(tdate.Text))
                    If Session("prd_buffer") > 120 Then
                        Session("prd_buffer") = Session("prd_buffer") - 120

                    End If

                    If Session("prd_buffer_d") > 120 Then
                        Session("prd_buffer_d") = Session("prd_buffer_d") - 120

                    End If

                    Session("cbalx") = Session("cbal")
                    Session("dbalx") = Session("dbal")
                End If

            Else
                Session("cbalx") = Session("cbal")
                Session("dbalx") = Session("dbal")


            End If

            If Session("rebate") = 0 Then

                lblrebateC.Text = ""
                lblrebated.Text = ""
            Else
                lblrebateC.Text = "-" + Session("rebate").ToString + " %"
                lblrebated.Text = "-" + Session("rebate").ToString + " %"
            End If


            If Session("product") = "JL" Then

                Session("cintr") = Session("cintr") - Session("rebate")
                Session("dint") = Session("dint") - Session("rebate")
            Else
                lblrebateC.Text = ""
                lblrebated.Text = ""

            End If

            Session("totint") = Math.Round((((-Session("cbalx")) * Session("cintr") / 100) / 365) * Session("prd_buffer"))
            Session("totalint_d") = Math.Round((((-Session("dbalx")) * Session("dint") / 100) / 365) * Session("prd_buffer_d"))

            ' If Session("totint") < 5 Then Session("totint") = 5
            ' If Session("totalint_d") < 5 Then Session("totalint_d") = 5
            lblperiod.Text = CStr(Session("prd_buffer")) + " Days"
            lblroi.Text = CStr(Session("cintr")) + "%"
            lblactualint.Text = FormatCurrency(Session("totint"))

            lblperiod_d.Text = CStr(Session("prd_buffer_d")) + " Days"
            lblroi_d.Text = CStr(Session("dint")) + "%"
            lblactualint_d.Text = FormatCurrency(Session("totalint_d"))

            txtamt.Text = (-(Session("dbal") - Session("totalint_d")))
            '   lblos.Text = String.Format("{0:N}", (-(Session("dbal") - Session("totalint_d"))))

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


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
            rds.Close()


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
            lblamt.Text = Session("cbal")
        Else
            lblamt.Text = CType(Session("cum_bal"), Double) + CType(Session("cba"), Double)
        End If


        '  dt_j.Rows.Add(newrow)
        ' dtdl.ImportRow(newrow)

        ' dispdl.DataSource = dtdl
        'dispdl.DataBind()
        ' Session("dspec") = dt_j
        ' Session("cum_bal") = CDbl(lblamt.Text)


        'lblamt.Text = Format(tqty, "0.00")
        'lbltqty.Text = tqtyvi
        'lblgross.Text = Format(tgross, "##,##0.000") 'String.Format("{0:#,###.###}", tgross)
        'lblnet.Text = Format(tnet, "##,##0.000") 'String.Format("{0:#,###.###}", tnet)
        'lblval.Text = String.Format("{0:N}", (tnet * ratepergm))

        Session("ac_date") = Nothing
        Session("amt") = 0
        Session("cbal") = 0


    End Sub

    'Private Sub bind_grid()

    '    Dim ds_trans As New DataSet


    '    disp.EmptyDataText = "No Records Found"

    '    Dim sql As String = "SELECT format(date,'dd/MM/yyyy') as ddate,narration,drc,crc,cbal FROM dbo.[actrans] WHERE actrans.acno='" + txtacn.Text + "' order by actrans.date,id"

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
    '        con.Close()



    '    End Try

    '    trim_disp()


    '    If product = "JL" Then

    '        jltab.Visible = True

    '        disp.EmptyDataText = "No Records Found"
    '        Dim query As String = "SELECT itm,qty,gross,net FROM dbo.[jlspec] WHERE jlspec.acno='" + txtacn.Text + "'"
    '        Dim adapter1 As New SqlDataAdapter(query, con)
    '        Try
    '            adapter1.Fill(ds_trans)
    '            dispjl.DataSource = ds_trans
    '            dispjl.DataBind()
    '        Catch ex As Exception
    '            response.write(ex.ToString)
    '        Finally
    '            ds_trans.Dispose()
    '            adapter.Dispose()
    '        End Try
    '        If con.State = ConnectionState.Closed Then con.Open()
    '        query = "select tqty,tgross,tnet from dbo.jlstock where jlstock.acn='" + txtacn.Text + "'"
    '        cmd.Connection = con
    '        cmd.CommandText = query
    '        Dim reader As SqlDataReader
    '        Try
    '            reader = cmd.ExecuteReader()
    '            If reader.HasRows() Then
    '                reader.Read()
    '                lbltqty.Text = reader(0)
    '                lblgross.Text = reader(1)
    '                lblnet.Text = reader(2)
    '            End If
    '        Catch ex As Exception
    '            response.write(ex.ToString)
    '        End Try
    '    End If


    '    If product = "DL" Then
    '        If con.State = ConnectionState.Closed Then con.Open()

    '        dltab.Visible = True
    '        cmd.Connection = con
    '        dispdl.EmptyDataText = "No Records Found"
    '        Dim query As String = "SELECT deposit FROM dbo.[dlspec] WHERE dlspec.acn='" + txtacn.Text + "'"
    '        cmd.CommandText = query

    '        Dim reader As SqlDataReader

    '        Try
    '            reader = cmd.ExecuteReader()

    '            If reader.HasRows Then
    '                While reader.Read()

    '                    add_table(reader(0))

    '                End While

    '            End If

    '            '    dispdl.DataSource = dt_dl
    '            '   dispdl.DataBind()



    '        Catch ex As Exception
    '            response.write(ex.ToString)
    '        Finally
    '            ds_trans.Dispose()
    '            adapter.Dispose()
    '        End Try



    '    End If


    'End Sub

    'Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
    '    disp.PageIndex = e.NewPageIndex
    '    disp.DataBind()
    'End Sub

    'Sub trim_disp()
    '    Dim total As Double = 0
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
    'End Sub


    Function prv_int()
        Dim oresult As Date
        Dim result As Boolean = False

        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Connection = con
        cmdi.Parameters.Clear()

        query = "SELECT TOP 1 actrans.date FROM dbo.actrans WHERE actrans.Drd > 0 AND actrans.Type = 'INTR' AND actrans.acno = @acn ORDER BY actrans.date DESC"
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acn", txtacn.Text)

        Try
            oresult = cmdi.ExecuteScalar()
            If oresult = Date.MinValue Then
                Session("prv_dinton") = Session("ac_date")
                result = False
            Else
                Session("prv_dinton") = oresult
                result = True
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmdi.Dispose()
            con.Close()

        End Try


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()

        query = "SELECT TOP 1 actrans.date FROM dbo.actrans WHERE actrans.Drc > 0 AND actrans.Type = 'INTR' AND actrans.acno = @acn ORDER BY actrans.date DESC"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@acn", txtacn.Text)

        Try
            oresult = cmd.ExecuteScalar()
            If oresult = Date.MinValue Then
                Session("prv_inton") = Session("ac_date")
                result = False
            Else
                Session("prv_inton") = oresult
                result = True
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()

        End Try
        Return result


    End Function
    Function get_last_transaction(ByVal acn As String)
        Dim oResult As Date


        cmdx.Connection = con
        cmdx.CommandText = "SELECT top 1 date  FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "'ORDER BY date DESC"

        oResult = cmdx.ExecuteScalar()

        If Not oResult = Date.MinValue Then
            'Dim tdat As Date = Convert.ToDateTime(tdate.Text)
            Session("prv_inton") = oResult
        Else
            Session("prv_inton") = tdate.Text
        End If

        Return Session("prv_inton")

    End Function

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



    Private Sub txtacn_TextChanged(sender As Object, e As EventArgs) Handles txtacn.TextChanged
        ' TabContainer1.Visible = True
        ' txt_acn_srch.Text = ""
        'listgid1.Items.Clear()
        tdate.Text = Date.Today
        Session("tdat") = CDate(tdate.Text)
        Session("acn") = Trim(txtacn.Text)
        get_ac_info(txtacn.Text)
        txtfocus(tdate)

    End Sub

    Function get_pro(ByVal prd As String)

        Dim prdname As String
        Dim query As String = String.Empty
        cmd.Connection = con
        query = "SELECT name from products where products.shortname='" + prd + "'"
        cmd.CommandText = query
        prdname = cmd.ExecuteScalar()


        Return prdname
    End Function
    Private Sub update_suplementry4JRNL(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)

        If tid = 0 Then Exit Sub
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con


        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.Clear()

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

        If tid = 0 Then Exit Sub
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con


        query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.Clear()

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
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            If mop.SelectedItem.Text = "Transfer" Then
                cmd.Parameters.AddWithValue("@acn", txtacn.Text)
                cmd.Parameters.AddWithValue("@nar", "By Transfer " + bnk.SelectedItem.Text)
            Else
                cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
                cmd.Parameters.AddWithValue("@nar", "By Transfer " + Trim(txtacn.Text) + " (" + Trim(txt_sb.Text) + ")")

            End If

            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
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

            'cmd.Parameters.AddWithValue("@nar", "To Transfer")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            If Not Session("credit_d") = cr Then


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
                    cmd.Parameters.AddWithValue("@nar", "To Cash " + txtacn.Text)
                Else
                    cmd.Parameters.AddWithValue("@achead", bnk.SelectedItem.Text)
                    cmd.Parameters.AddWithValue("@nar", "To Transfer " + txtacn.Text)
                End If

                cmd.Parameters.AddWithValue("@debit", Session("credit_d") - cr)
                    cmd.Parameters.AddWithValue("@credit", 0)
                    cmd.Parameters.AddWithValue("@acn", txt_sb.Text)

                cmd.Parameters.AddWithValue("@typ", "PAYMENT")
                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    End Try


                End If



            End If

        query = ""


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        If mop.SelectedItem.Text = "Cash" Then
            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.Clear()

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

            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@transid", tid)
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", cr)
            cmd.Parameters.AddWithValue("@acn", txt_sb.Text)
            cmd.Parameters.AddWithValue("@nar", "By Transfer")
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
            cmd.Parameters.AddWithValue("@credit", 0)

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
    Private Sub set_changes()


        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            ' log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("..\login.aspx")
        End If

        If session_user_role = "Audit" Then Exit Sub

        Dim ovr_d As Double
        Dim ovr_c As Double
        Dim ovr_rec As Double
        Dim ovr_auction As Double = 0

        Dim query As String = String.Empty

        cmd.Connection = con
        If txt_jla.Text = "" Then txt_jla.Text = 0

        If txtothers.Text = "" Then txtothers.Text = 0
        If txtins.Text = "" Then txtins.Text = 0




        ovr_d = -(Session("dbal") - CDbl(lblactualint_d.Text))

        ovr_c = -(Session("cbal") - CDbl(lblactualint.Text))

        If Trim(txt_jla.Text) = "" Then txt_jla.Text = 0


        If Not CDbl(txtins.Text) = 0 Then
            txt_jla.Text = CDbl(txt_jla.Text) + CDbl(txtins.Text)
        Else
            ovr_auction = txt_jla.Text
        End If

        If Not CDbl(txtothers.Text) = 0 Then
            txt_jla.Text = CDbl(txt_jla.Text) + CDbl(txtothers.Text)
        Else
            ovr_auction = txt_jla.Text
        End If

        ovr_rec = CDbl(txt_jla.Text)

        Session("ovr") = ovr_d - ovr_c

        If txtamt.Text < (ovr_d - ovr_c) Then
            Session("ovr") = txtamt.Text
        End If


        If Session("product") = "JL" Then

            If CDbl(txtamt.Text) - CDbl(txt_jla.Text) < CDbl(lblactualint.Text) Then

                '  Session("ovr") = CDbl(txtamt.Text)
                Session("credit_c") = CDbl(txtamt.Text) - Session("ovr") - CDbl(txt_jla.Text)

                If Session("credit_c") < 0 Then
                    Session("ovr") = Session("ovr") + Session("credit_c")
                    Session("credit_c") = 0
                End If


                'CDbl(lblactualint_d.Text) - CDbl(lblactualint.Text)

                If CDbl(txtamt.Text) < ovr_d - ovr_c Then
                    Session("credit_c") = 0

                End If

                Session("credit_d") = CDbl(txtamt.Text)



            Else

                Session("credit_c") = CDbl(txtamt.Text) - Session("ovr") - CDbl(txt_jla.Text)
                Session("credit_d") = CDbl(txtamt.Text)
            End If

        Else
            Session("credit_c") = CDbl(txtamt.Text) - Session("ovr") - CDbl(txt_jla.Text)
            Session("credit_d") = CDbl(txtamt.Text)
        End If



        Dim d As String = get_due(txtacn.Text, 0)

        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        Dim prod = get_pro(Session("product"))

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", Session("credit_d"))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", Session("credit_c"))

        'cmd.Parameters.AddWithValue("@narration", "BY CASH")
        'cmd.Parameters.AddWithValue("@due", " ")
        'cmd.Parameters.AddWithValue("@type", "CASH")


        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "By Cash")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "By Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "By Transfer")
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + txt_sb.Text)
        End Select




        cmd.Parameters.AddWithValue("@suplimentry", prod)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", ovr_rec)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)


        End Try

        query = ""



        query = "SELECT MAX(ID) from dbo.trans where trans.acno='" + txtacn.Text + "'"
        cmd.CommandText = query

        countresult = cmd.ExecuteScalar()

        Session("tid") = Convert.ToString(countresult)


        Dim nar As String = "To Interest Upto " + CStr(tdate.Text)
        If Not CDbl(lblactualint_d.Text) = 0 Then
            update_int(Session("tid"), CDbl(lblactualint_d.Text), 0, CDbl(lblactualint.Text), 0, nar, prod, -ovr_c, -ovr_d)
            nar = "By Interest Upto  " + CStr(tdate.Text) + " " '+ txtacn.Text
            update_suplementry4JRNL(Session("tid"), Session("product").ToString + " INTEREST", CDbl(lblactualint.Text), 0, nar, "JOURNAL")
            nar = "To Interest Upto  " + CStr(tdate.Text) + " " '+ txtacn.Text
            update_suplementry4JRNL(Session("tid"), prod, 0, CDbl(lblactualint.Text), nar, "JOURNAL")
        End If


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", Session("credit_d") - ovr_rec)
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", Session("credit_c"))
        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "BY CASH")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "By TRANSFER")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "By TRANSFER")
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


        End Try

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@id", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", Session("credit_d") - ovr_rec)
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", Session("credit_c"))
        Select Case mop.SelectedItem.Text

            Case "Cash"
                cmd.Parameters.AddWithValue("@narration", "BY CASH")
                cmd.Parameters.AddWithValue("@type", "CASH")
                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", " ")
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text))
                End If
            Case "Transfer"
                cmd.Parameters.AddWithValue("@narration", "By TRANSFER")
                cmd.Parameters.AddWithValue("@type", "TRF")

                If Trim(txtnotes.Text = "") Then
                    cmd.Parameters.AddWithValue("@due", bnk.SelectedItem.Text)
                Else
                    cmd.Parameters.AddWithValue("@due", Trim(txtnotes.Text) + " " + bnk.SelectedItem.Text)
                End If
            Case "Account"
                cmd.Parameters.AddWithValue("@narration", "By TRANSFER")
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


        End Try








        If Not CType(Session("ovr"), Integer) = 0 Then


            set_diff(Session("ovr"))

        End If

        If Not CDbl(txtins.Text) = 0 Then
            set_diff(txtins.Text)

            update_int(Session("tid"), txtins.Text, 0, 0, 0, "To Insurance", prod, 0, 0)
            update_int(Session("tid"), 0, txtins.Text, 0, 0, "By CASH", prod, 0, 0)

        End If

        If Not CDbl(txtothers.Text) = 0 Then
            set_diff(txtothers.Text)
        End If



        If mop.SelectedItem.Text = "Account" Then
            update_sb()
        End If


        If Not ovr_auction = 0 Then

            set_diff(ovr_auction)
            query = "insert into jlauction(date,cid,acno,credit)"
            query &= " values(@date,@cid,@acno,@credit)"
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con
            cmd.CommandText = query
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@cid", Session("ncid"))
            cmd.Parameters.AddWithValue("@acno", txtacn.Text)
            If txtothers.Text = "0" Then
                cmd.Parameters.AddWithValue("@credit", CDbl(txt_jla.Text))
            Else
                Dim ar As Double = 0

                ar = CDbl(txt_jla.Text) - CDbl(txtothers.Text)
                cmd.Parameters.AddWithValue("@credit", ar)
            End If
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

        End If



        If Not Session("credit_c") = 0 Then
            update_suplementry(Session("tid"), prod, Session("credit_c"), 0, "BY CASH", "RECEIPT")
        End If



        If CDbl(txtamt.Text) = -(CType(Session("dbal"), Double) - CDbl(lblactualint_d.Text)) + CDbl(txt_jla.Text) Then

            query = "UPDATE MASTER SET cld=1 where acno='" + txtacn.Text + "'"
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con
            cmd.CommandText = query
            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            If Session("product") = "JL" Then
                query = "UPDATE jlstock SET cld=@cld,cldon=@cldon where acn=@acno"
                cmd.Connection = con
                cmd.CommandText = query

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@cld", 1)
                cmd.Parameters.AddWithValue("@cldon", CDate(tdate.Text))
                cmd.Parameters.AddWithValue("@acno", txtacn.Text)


                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)


                End Try

            End If


        End If




        stitle = "Hi " + Session("sesusr")
        msg = "Transaction Completed. ID # " + Session("tid")
        fnc = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


        'clear_tab_recpt()
        If mop.SelectedItem.Text = "Cash" Then
            clear_tab_recpt()
        Else
            prepare_print()
        End If


    End Sub

    Sub prepare_print()



        Dim othersAmt As Double = 0
        If Not String.IsNullOrEmpty(txtothers.Text) Then
            Double.TryParse(txtothers.Text, othersAmt)
        End If

        Dim mainReceiptAmt As Double = CDbl(txtamt.Text)
        If (mop.SelectedItem.Text = "Transfer" OrElse mop.SelectedItem.Text = "Account") AndAlso othersAmt > 0 Then
            mainReceiptAmt = mainReceiptAmt - othersAmt
        End If

        camt.Value = FormatCurrency(Session("credit_c"))
        cword.Value = get_wrds(Session("credit_c"))
        damt.Value = FormatCurrency(mainReceiptAmt)
        dword.Value = get_wrds(mainReceiptAmt)


        lblcpt.Text = "RECEIPT"
        lblcptr.Text = "CUSTOMER COPY"
        pvno.Text = Session("tid")
        pdate.Text = tdate.Text
        pbranch.Text = get_home()
        pacno.Text = txtacn.Text
        pglh.Text = get_pro(lblproduct.Text)
        pcid.Text = get_memberno(txtacn.Text)
        pcname.Text = lblname.Text
        pamt.Text = FormatCurrency(mainReceiptAmt)
        paiw.Text = get_wrds(mainReceiptAmt)
        premit.Text = pcname.Text

        ovr_acn.Text = txt_sb.Text

        If (mop.SelectedItem.Text = "Transfer" OrElse mop.SelectedItem.Text = "Account") AndAlso othersAmt > 0 Then
            ovr_aiw.Text = get_wrds(othersAmt)
            ovr_amt.Text = FormatCurrency(othersAmt)
            If mop.SelectedItem.Text = "Account" Then
                ovr_type.Text = "PAYMENT"
            Else
                ovr_type.Text = "RD RECEIPT"
            End If
        Else
            ovr_aiw.Text = get_wrds(Session("ovr"))
            ovr_amt.Text = FormatCurrency(Session("ovr"))
            If mop.SelectedItem.Text = "Account" Then
                ovr_type.Text = "PAYMENT"
            Else
                ovr_type.Text = "PAYMENT"
            End If
        End If

        ovr_branch.Text = pbranch.Text
        ovr_cid.Text = get_memberno(txt_sb.Text)
        ovr_name.Text = pcname.Text
        ovr_sign.Text = pcname.Text
        ovr_no.Text = Session("tid")
        ovr_copy.Text = "OFFICE COPY"
        ovr_dt.Text = pdate.Text


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
        pcamt.Text = FormatCurrency(mainReceiptAmt)
        pcaiw.Text = get_wrds(mainReceiptAmt)
        pcremit.Text = pccname.Text
        ' clear_tab_recpt()


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
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
        cmd.Parameters.AddWithValue("@drd", Session("credit_c"))
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", Session("credit_c"))
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

            Response.Write(EX.ToString)

        Finally

            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()

        End Try

        Dim othersAmt As Double = 0
        If Not String.IsNullOrEmpty(txtothers.Text) Then
            Double.TryParse(txtothers.Text, othersAmt)
        End If

        If othersAmt > 0 Then
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Parameters.Clear()
            cmd.Connection = con
            query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
            cmd.Parameters.AddWithValue("@drd", othersAmt)
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", othersAmt)
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Transfer Others")
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
        End If

        If Not Session("ovr") = 0 Then

            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Parameters.Clear()
            cmd.Connection = con
            query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
            cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text) - Session("credit_c"))
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", CDbl(txtamt.Text) - Session("credit_c"))
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Cash")
            cmd.Parameters.AddWithValue("@due", txtacn.Text)
            cmd.Parameters.AddWithValue("@type", "CASH")
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


        End If

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()
        cmd.Connection = con
        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
        cmd.Parameters.AddWithValue("@drd", CDbl(txtamt.Text) - othersAmt)
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", CDbl(txtamt.Text) - othersAmt)
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

            Response.Write(EX.ToString)

        Finally

            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()

        End Try

        If othersAmt > 0 Then
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Parameters.Clear()
            cmd.Connection = con
            query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", txt_sb.Text)
            cmd.Parameters.AddWithValue("@drd", othersAmt)
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", othersAmt)
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Transfer Others")
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
        End If

    End Sub
    Private Sub update_int(ByVal tid As Double, ByVal drd As Double, ByVal crd As Double, ByVal drc As Double, ByVal crc As Double, ByVal nar As String, ByVal supliment As String, ByVal cbal As Double, ByVal dbal As Double)


        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Connection = con
        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"
        cmdi.Parameters.Clear()
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@uiid", tid)
        cmdi.Parameters.AddWithValue("@uidate", Convert.ToDateTime(tdate.Text))
        cmdi.Parameters.AddWithValue("@uiacno", txtacn.Text)
        cmdi.Parameters.AddWithValue("@uidrd", drd)
        cmdi.Parameters.AddWithValue("@uicrd", crd)
        cmdi.Parameters.AddWithValue("@uidrc", drc)
        cmdi.Parameters.AddWithValue("@uicrc", crc)
        cmdi.Parameters.AddWithValue("@uinarration", nar)
        cmdi.Parameters.AddWithValue("@uidue", " ")
        cmdi.Parameters.AddWithValue("@uitype", "INTR")
        cmdi.Parameters.AddWithValue("@uisuplimentry", supliment)
        cmdi.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
        cmdi.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
        cmdi.Parameters.AddWithValue("@uicbal", 0)
        cmdi.Parameters.AddWithValue("@uidbal", 0)


        Try
            cmdi.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)

        Finally

            cmdi.Dispose()
            con.Close()
        End Try



        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Connection = con
        query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"
        cmdi.Parameters.Clear()
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@uiid", tid)
        cmdi.Parameters.AddWithValue("@uidate", Convert.ToDateTime(tdate.Text))
        cmdi.Parameters.AddWithValue("@uiacno", txtacn.Text)
        cmdi.Parameters.AddWithValue("@uidrd", drd)
        cmdi.Parameters.AddWithValue("@uicrd", crd)
        cmdi.Parameters.AddWithValue("@uidrc", drc)
        cmdi.Parameters.AddWithValue("@uicrc", crc)
        cmdi.Parameters.AddWithValue("@uinarration", nar)
        cmdi.Parameters.AddWithValue("@uidue", " ")
        cmdi.Parameters.AddWithValue("@uitype", "INTR")
        cmdi.Parameters.AddWithValue("@uisuplimentry", supliment)
        cmdi.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
        cmdi.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
        cmdi.Parameters.AddWithValue("@uicbal", 0)
        cmdi.Parameters.AddWithValue("@uidbal", 0)


        Try
            cmdi.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.ToString)

        Finally

            cmdi.Dispose()
            con.Close()
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

    Private Sub btn_up_rcpt_Click(sender As Object, e As EventArgs) Handles btn_up_rcpt.Click
        'txtamt.ReadOnly = True
        btn_up_rcpt.Enabled = False


        Dim others As Double = 0

        If Not txt_jla.Text = "" Then
            others = CDbl(txtins.Text) + CDbl(txtothers.Text) + CDbl(txt_jla.Text)
        Else
            others = CDbl(txtins.Text) + CDbl(txtothers.Text)
        End If

        If (CDbl(txtamt.Text) <= -(CDbl(lbldbal.Text) - CDbl(lblactualint_d.Text)) + others) Then

            If CDbl(txtamt.Text) = 0 Then Exit Sub


            set_changes()


            'System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt1k)

        Else


            stitle = "Hi " + Session("sesusr")
            msg = "Excess receipt . Receipt should be less than  " + FormatCurrency(-(CDbl(lbldbal.Text) - CDbl(lblactualint_d.Text) - 1) + others)
            fnc = "showToastnOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


        End If

    End Sub
    Sub clear_tab_recpt()
        btn_up_rcpt.Enabled = True


        Session("tid") = Nothing
        txtnotes.Text = ""
        lblrebateC.Text = ""
        lblrebated.Text = ""
        txtacn.Text = ""
        lblamt.Text = ""
        lblbal.Text = ""
        lblname.Text = ""
        lblproduct.Text = ""
        lblsch.Text = ""
        txtamt.Text = ""
        txtacn.Enabled = True
        txtfocus(txtacn)
        '  btn_auction.Enabled = False
        Session("jspec") = Nothing
        Session("tnet") = Nothing
        Session("tgross") = Nothing
        Session("tqt") = Nothing

        jlrecovery.Visible = False
        lbl_acn_loss.Text = ""
        txt_jla.Text = ""
        txtnar.Text = ""
        pnltran.Visible = False
        pnlact.Visible = False
        pnlbtn.Visible = False
        pnlint.Visible = False
        '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)



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

    '            response.write(ex.ToString)
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
        'get_ac_info(txtacn.Text)

        Session("tdat") = CDate(tdate.Text)
        If Not Trim(txtacn.Text) = "" Then
            get_int()
        End If

        txtfocus(txtamt)

        Exit Sub
nxt:
        txtfocus(tdate)

    End Sub

    Private Function TabPanel1() As Object
        Throw New NotImplementedException
    End Function


    Private Sub btn_up_can_Click(sender As Object, e As EventArgs) Handles btn_up_can.Click

        ' TabContainer1.Visible = False
        txtacn.Text = ""
        txtacn.Enabled = True
        clear_tab_recpt()

        txtfocus(txtacn)

    End Sub

    Private Sub lblactualint_d_TextChanged(sender As Object, e As EventArgs) Handles lblactualint_d.TextChanged


        txtamt.Text = -(CDbl(lblbal.Text) - CDbl(lblactualint_d.Text))


    End Sub

    Sub showjltab(ByVal acn As String)

        ''   auctiontab.Visible = True

        'If con1.State = ConnectionState.Closed Then con1.Open()

        'query = "select itm,qty,gross,net from jlspec where jlspec.acno='" + acn + "'"
        ''cmd.Connection = con
        ''cmd.CommandText = query

        'Dim ds As New DataSet
        'Dim adapter As New SqlDataAdapter(query, con1)
        'adapter.Fill(ds)
        'dispjl.DataSource = ds
        'dispjl.DataBind()

        ''jwlspecs.DataSource = ds
        ''jwlspecs.DataTextField = "itm"
        ''jwlspecs.DataValueField = "gross"

        ''jwlspecs.DataBind()
        ''jwlspecs.Items.Insert(0, "<-Select->")

        'adapter.Dispose()

        'query = "select tqty,tgross,tnet from jlstock where jlstock.acn='" + acn + "'"
        'cmd.Connection = con1
        'cmd.CommandText = query
        'Dim dr As SqlDataReader

        'Try

        '    dr = cmd.ExecuteReader()

        '    If dr.HasRows() Then
        '        dr.Read()

        '        lbltqty.Text = IIf(IsDBNull(dr(0)), 0, dr(0))
        '        lblgross.Text = Format(IIf(IsDBNull(dr(1)), 0, dr(1)), "##,##0.000")
        '        lblnet.Text = Format(IIf(IsDBNull(dr(2)), 0, dr(2)), "##,##0.000")
        '    End If
        '    dr.Close()


        'Catch ex As Exception
        '    Response.Write(ex.ToString)
        'End Try


        'lbl_ad_mamt.Text = -(Math.Round(CDbl(lbl_ad_bal.Text) / CDbl(lblnet.Text)))
        'lbl_ad_mamt.ForeColor = Color.Red

        ' jltab.Visible = True

    End Sub

    'Private Sub jwlspecs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles jwlspecs.SelectedIndexChanged

    '    txtweight.Text = jwlspecs.SelectedValue.ToString
    '    txtfocus(txtrpg)

    'End Sub

    'Private Sub jwlspecs_TextChanged(sender As Object, e As EventArgs) Handles jwlspecs.TextChanged
    '    txtweight.Text = jwlspecs.SelectedValue.ToString
    '    txtfocus(txtrpg)
    'End Sub

    'Private Sub txtrpg_TextChanged(sender As Object, e As EventArgs) Handles txtrpg.TextChanged
    '    txtnet.Text = String.Format("{0:N}", CDbl(txtrpg.Text) * CDbl(txtweight.Text))
    'End Sub

    'Private Sub j_add_Click(sender As Object, e As EventArgs) Handles j_add.Click



    '    Dim dt_j As New DataTable

    '    If dt_j.Columns.Count = 0 Then
    '        dt_j.Columns.Add("itm", GetType(String))
    '        dt_j.Columns.Add("qty", GetType(Decimal))
    '        dt_j.Columns.Add("gross", GetType(Integer))
    '        dt_j.Columns.Add("net", GetType(Double))

    '    End If

    '    If dt.Columns.Count = 0 Then
    '        dt.Columns.Add("itm", GetType(String))
    '        dt.Columns.Add("qty", GetType(Decimal))
    '        dt.Columns.Add("gross", GetType(Integer))
    '        dt.Columns.Add("net", GetType(Double))

    '    End If



    '    If Session("jspec") Is Nothing = False Then
    '        dt_j = CType(Session("jspec"), DataTable)

    '        Dim count As Integer = dt_j.Rows.Count

    '        For Each row1 As DataRow In dt_j.Rows
    '            dt.ImportRow(row1)
    '        Next

    '    End If
    '    newrow = dt_j.NewRow

    '    newrow(0) = jwlspecs.SelectedItem.Text
    '    newrow(1) = txtweight.Text
    '    newrow(2) = txtrpg.Text
    '    newrow(3) = txtnet.Text

    '    If Session("tqt") Is Nothing Then
    '        tqty = Convert.ToDecimal(txtweight.Text)
    '    Else
    '        tqty = CType(Session("tqt"), Decimal) + Convert.ToDecimal(txtweight.Text)
    '    End If

    '    If Session("tgross") Is Nothing Then
    '        tgross = Convert.ToDecimal(txtrpg.Text)
    '    Else
    '        tgross = CType(Session("tgross"), Decimal) + Convert.ToDecimal(txtrpg.Text)
    '    End If

    '    If Session("tnet") Is Nothing Then
    '        tnet = Convert.ToDecimal(txtnet.Text)

    '    Else
    '        tnet = CType(Session("tnet"), Decimal) + Convert.ToDecimal(txtnet.Text)
    '    End If

    '    dt_j.Rows.Add(newrow)
    '    dt.ImportRow(newrow)

    '    gvauction.DataSource = dt
    '    gvauction.DataBind()
    '    Session("jspec") = dt_j
    '    Session("tnet") = tnet
    '    Session("tgross") = tgross
    '    Session("tqt") = tqty


    '    lblwt.Text = Format(tqty, "##,##0.000")
    '    'lblgross.Text = Format(tgross, "##,##0.000") 'String.Format("{0:#,###.###}", tgross)
    '    lblauctionvalue.Text = Format(tnet, "##,##0.00") 'String.Format("{0:#,###.###}", tnet)
    '    'lblauctionvalue.Text = String.Format("{0:N}", (tnet * Session("ratepergm")))

    '    lblval.Text = String.Format("{0:N}", CDbl(lblauctionvalue.Text) - CDbl(lblos.Text))

    '    If CDbl(lblval.Text) < 0 Then
    '        lblval.ForeColor = Color.Red
    '        lblval.Font.Bold = True
    '    Else
    '        lblval.ForeColor = Color.Green
    '    End If

    '    txtweight.Text = ""
    '    txtrpg.Text = ""
    '    txtnet.Text = ""
    '    btn_auction.Enabled = True
    '    jwlspecs.SelectedIndex = 0
    '    txtfocus(jwlspecs)




    'End Sub



    'Private Sub btn_auction_Click(sender As Object, e As EventArgs) Handles btn_auction.Click

    '    If CDbl(lblval.Text) <> 0 Then

    '        If con.State = ConnectionState.Closed Then con.Open()

    '        Dim dt_j As New DataTable

    '        If Session("jspec") Is Nothing = False Then
    '            dt_j = CType(Session("jspec"), DataTable)

    '            Dim count As Integer = dt_j.Rows.Count

    '            For Each row1 As DataRow In dt_j.Rows
    '                Dim jlspec As String = row1(0).ToString
    '                Dim wt As Decimal = row1(1).ToString
    '                Dim rpg As Double = row1(2).ToString
    '                Dim net As Double = row1(3).ToString
    '                cmd.Parameters.Clear()

    '                query = "insert into jlauctionbrkup(date,cid,acno,itm,weight,rpg,val)"
    '                query &= "values(@date,@cid,@acno,@itm,@weight,@rpg,@val)"

    '                cmd.Connection = con
    '                cmd.CommandText = query
    '                cmd.Parameters.Clear()
    '                cmd.Parameters.AddWithValue("@date", DateAndTime.Today)
    '                cmd.Parameters.AddWithValue("@cid", lbl_ad_acid.Text)
    '                cmd.Parameters.AddWithValue("@acno", txtacn.Text)
    '                cmd.Parameters.AddWithValue("@itm", jlspec)
    '                cmd.Parameters.AddWithValue("@weight", wt)
    '                cmd.Parameters.AddWithValue("@rpg", rpg)
    '                cmd.Parameters.AddWithValue("@val", net)

    '                Try
    '                    cmd.ExecuteNonQuery()

    '                Catch ex As Exception
    '                    Response.Write(ex.ToString)
    '                End Try
    '            Next
    '        End If


    '    End If

    '    clear_tab_recpt()


    'End Sub



    Private Sub txtamt_TextChanged(sender As Object, e As EventArgs) Handles txtamt.TextChanged
        ' lblnettotal.Text = FormatCurrency(CDbl(txtamt.Text) + CDbl(txtrecovery.Text))
        'txtfocus(txt_jla)
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txt_jla)
    End Sub

    Private Sub txt_jla_TextChanged(sender As Object, e As EventArgs) Handles txt_jla.TextChanged
        lbl_actual.Text = FormatCurrency(CDbl(txtamt.Text) - CDbl(txt_jla.Text))
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtnar)
    End Sub

    Private Sub txtothers_TextChanged(sender As Object, e As EventArgs) Handles txtothers.TextChanged
        If Not txtothers.Text = "" Then
            txtamt.Text = CDbl(txtamt.Text) + CDbl(txtothers.Text)
        End If

    End Sub

    Private Sub txtins_TextChanged(sender As Object, e As EventArgs) Handles txtins.TextChanged
        If Not txtins.Text = "" Then
            txtamt.Text = CDbl(txtamt.Text) + CDbl(txtins.Text)
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
                ' btn_trf.Visible = False
                lblsb.Visible = False
                bnk.Focus()


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
            'btn_trf.Visible = True
            lblsb.Visible = True
            txt_sb.Focus()


        Else
            pnltran.Visible = False
            bnk.Visible = False
            lbl.Visible = False
            lblsb.Visible = False
            pnlsbtrf.Visible = False
            txt_sb.Visible = False
            '   btn_trf.Visible = False
        End If
    End Sub

    Public Sub btnCalcPostal_Click(sender As Object, e As EventArgs)
        If String.IsNullOrEmpty(txtPostalDate.Text) Then
            Dim stitle As String = "Hi " & Session("sesusr").ToString()
            Dim msg As String = "Please select a target date for Postal Purpose calculation."
            Dim fnc As String = "showToastnOK('" & stitle & "','" & msg & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
            Exit Sub
        End If

        Dim targetDate As Date
        Try
            targetDate = CDate(txtPostalDate.Text)
        Catch ex As Exception
            Dim stitle As String = "Hi " & Session("sesusr").ToString()
            Dim msg As String = "Invalid date format."
            Dim fnc As String = "showToastnOK('" & stitle & "','" & msg & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
            Exit Sub
        End Try

        Dim dr_roi As SqlDataReader = Nothing
        Dim local_prd_buffer_d As Integer = 0
        Dim local_dint As Double = 0
        Dim local_totalint_d As Double = 0

        Try
            If lblproduct.Text = "DCL" Then
                local_prd_buffer_d = DateDiff(DateInterval.Day, CDate(Session("ac_date")), targetDate)
            Else
                If Not IsNothing(Session("prv_dinton")) Then
                    local_prd_buffer_d = DateDiff(DateInterval.Day, CDate(Session("prv_dinton")), targetDate)
                Else
                    local_prd_buffer_d = DateDiff(DateInterval.Day, CDate(Session("ac_date")), targetDate)
                End If
            End If

            If local_prd_buffer_d <= 0 Then
                If local_prd_buffer_d = 0 Then local_prd_buffer_d = 7
            End If

            Dim query As String
            If Session("product") = "JL" Then
                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.agst=@agst AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
            Else
                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
            End If

            Dim cmd_local As New SqlClient.SqlCommand()
            If con.State = ConnectionState.Closed Then con.Open()
            cmd_local.Connection = con
            cmd_local.CommandText = query
            cmd_local.Parameters.AddWithValue("@prod", Session("product"))
            cmd_local.Parameters.AddWithValue("@prdtyp", "D")
            cmd_local.Parameters.AddWithValue("@prdx", local_prd_buffer_d)
            cmd_local.Parameters.AddWithValue("@prdy", local_prd_buffer_d)
            If Session("product") = "JL" Then
                cmd_local.Parameters.AddWithValue("@agst", Session("scheme"))
            End If

            dr_roi = cmd_local.ExecuteReader()
            If dr_roi.HasRows() Then
                While dr_roi.Read()
                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)
                    Dim x As Long = FYFRM.CompareTo(CDate(Session("ac_date")))
                    If x = -1 Then
                        Dim y As Long = FYTO.CompareTo(CDate(Session("ac_date")))
                        If y = 1 Then
                            local_dint = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))
                            Exit While
                        End If
                    End If
                End While
            End If
            dr_roi.Close()

            If Session("product") = "JL" Then
                local_dint = local_dint - IIf(IsNothing(Session("rebate")), 0, CDbl(Session("rebate")))
            End If

            Dim local_dbalx As Double = CDbl(Session("dbal"))
            If lblproduct.Text = "DCL" Then
                If local_prd_buffer_d > 120 Then
                    local_prd_buffer_d = local_prd_buffer_d - 120
                End If
            End If

            local_totalint_d = Math.Round((((-local_dbalx) * local_dint / 100) / 365) * local_prd_buffer_d)

            lblPostalCredit.Text = lbldcr.Text
            lblPostalDebit.Text = lblddr.Text
            lblPostalBalance.Text = lbldbal.Text
            lblPostalPeriod.Text = local_prd_buffer_d.ToString() & " Days"
            lblPostalROI.Text = local_dint.ToString() & "%"
            lblPostalInterest.Text = FormatCurrency(local_totalint_d)

            Dim finalAmount As Double = -(local_dbalx - local_totalint_d)
            lblPostalFinalAmount.Text = FormatCurrency(finalAmount)

        Catch ex As Exception
            If Not dr_roi Is Nothing AndAlso Not dr_roi.IsClosed Then dr_roi.Close()
            Response.Write(ex.ToString())
        End Try
    End Sub

    Private Sub LoanReceipt_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub soa_Click(sender As Object, e As EventArgs) Handles soa.Click
        If Not Trim(txtacn.Text) = "" Then

            Response.Redirect("soaloan.aspx?acno=" + Trim(txtacn.Text))

        End If
    End Sub

    Private Sub lblactualint_TextChanged(sender As Object, e As EventArgs) Handles lblactualint.TextChanged

    End Sub

    Private Sub txt_sb_TextChanged(sender As Object, e As EventArgs) Handles txt_sb.TextChanged
        If txtamt.Text = 0 Then
            txt_sb.Text = ""
            txtfocus(txtamt)
        End If
        If Trim(Left(txt_sb.Text, 2)) = "79" Or Trim(Left(txt_sb.Text, 2)) = "76" Then

            lbl_sb_bal.Text = FormatCurrency(get_balance_SB(Trim(txt_sb.Text)))
            Dim abal As Decimal = CDbl(lbl_sb_bal.Text) - 100


            If CDbl(txtamt.Text) <= abal Then

                    lbl_sb_bal.ForeColor = Color.Green

                Else
                    lbl_sb_bal.ForeColor = Color.Red
                    btn_up_rcpt.Enabled = False
                End If

        Else
                txt_sb.Text = ""
        End If
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

        If chkServiceChargeReceipt.Checked Then
            ' FETCH SERVICE CHARGE FROM DIFF TABLE
            Dim sqlDiff As String = "SELECT acno, product, cr FROM diff WHERE tid = @tid AND CAST(date AS DATE) = CAST(@date AS DATE) AND cr > 0"
            Using conDiff As New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                Using cmdDiff As New SqlCommand(sqlDiff, conDiff)
                    cmdDiff.Parameters.AddWithValue("@tid", transId)
                    cmdDiff.Parameters.AddWithValue("@date", reprintDate)

                    Try
                        conDiff.Open()
                        Using reader As SqlDataReader = cmdDiff.ExecuteReader()
                            If reader.Read() Then
                                Dim acn As String = reader("acno").ToString()
                                Dim amount As Decimal = Convert.ToDecimal(reader("cr"))
                                
                                Dim memberNo As String = get_memberno(acn)
                                Dim memberName As String = get_membername(acn)
                                Dim amountWords As String = get_wrds(amount)
                                
                                Dim row As DataRow = dtReceipts.NewRow()
                                row("Branch") = get_home()
                                row("TransID") = transId
                                row("Date") = reprintDate.ToString("dd-MM-yyyy")
                                row("AccountNo") = acn
                                row("AccountHead") = "SERVICE CHARGES"
                                row("MemberNo") = memberNo
                                row("MemberName") = memberName
                                row("AmountFormatted") = FormatCurrency(amount)
                                row("Narration") = "Service Charge Collected"
                                row("AmountInWords") = amountWords
                                row("RemitterName") = memberName
                                row("ReceiptType") = "SERVICE CHARGE RECEIPT"
                                
                                dtReceipts.Rows.Add(row)
                            Else
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", "showToastnOK('Info', 'No service found for that transaction');", True)
                                Return
                            End If
                        End Using
                    Catch ex As Exception
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", "showToastnOK('Error', '" & ex.Message.Replace("'", "") & "');", True)
                        Return
                    End Try
                End Using
            End Using
        Else
            ' STANDARD RECREATION FROM SUPLEMENT
            Dim sql As String = "SELECT achead, acn, credit, debit, narration FROM suplement WHERE CAST(date AS DATE) = CAST(@date AS DATE) AND transid = @transid AND (credit > 0 OR debit > 0) AND narration NOT LIKE 'By Interest%' AND narration NOT LIKE 'To Interest%'"
            
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
                                
                                Dim receiptType As String = If(credit > 0, "RECEIPT", "PAYMENT")
                                Dim amount As Decimal = If(credit > 0, credit, debit)
                                
                                Dim memberNo As String = get_memberno(acn)
                                Dim memberName As String = get_membername(acn)
                                Dim amountWords As String = get_wrds(amount)
                                
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
                                
                                dtReceipts.Rows.Add(row)
                            End While
                        End Using
                    Catch ex As Exception
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", "showToastnOK('Error', '" & ex.Message.Replace("'", "") & "');", True)
                        Return
                    End Try
                End Using
            End Using
        End If

        If dtReceipts.Rows.Count > 0 Then
            rptReprint.DataSource = dtReceipts
            rptReprint.DataBind()
            
            pnlReprintResults.Visible = True

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
        rptReprint.DataSource = Nothing
        rptReprint.DataBind()
    End Sub
End Class