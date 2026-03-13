Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization

Public Class LoanOpening
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand

    Public cid As String
    Dim oResult As String


    Dim newrow As DataRow
    Public dt As New DataTable
    Public dtdl As New DataTable
    Public dc As New DataColumn
    Public tqty As Integer
    Public tgross As Decimal
    Public tnet As Decimal


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()


        If Not Page.IsPostBack Then

            Session("Tabin") = 0
            ' alertmsg.Visible = False
            ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "ShowCTab()", True)


            txtadate.Text = Format(Now, "dd-MM-yyyy")

            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            btn_nxt_ms.Enabled = False
            btn_nxt_jsp.Enabled = False

            txtfocus(txtcid)
            tqty = 0
            tgross = 0
            tnet = 0
            Me.Page.SetFocus(txtcid)




        End If

    End Sub


    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        disp.PageIndex = e.NewPageIndex
        disp.DataBind()
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

                    clea()


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
                    If Not imgbytes.Length = 0 Then
                        stream = New MemoryStream(imgbytes, 0, imgbytes.Length)
                        imgx = Image.FromStream(stream)
                        '  imgx.Save("../captures/webcam.png", System.Drawing.Imaging.ImageFormat.Png)


                        Dim imagePath As String = String.Format("~/Captures/{0}.png", Trim(txtcid.Text))
                        File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                        'Session("CapturedImage") = ResolveUrl(imagePath)
                        imgCapture.ImageUrl = "~/captures/" + Trim(txtcid.Text) + ".png?" + DateTime.Now.Ticks.ToString()
                        imgCapture.Visible = True
                    End If
                Else
                    imgCapture.Visible = False


                End If
            Else

                imgCapture.Visible = False


            End If
            dr.Close()


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try


    End Sub
    Public Function byteArrayToImage(ByVal byteArrayIn As Byte()) As Image
        Using mStream As New MemoryStream(byteArrayIn)
            Return Image.FromStream(mStream)
        End Using
    End Function
    Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged
        'display_address(txtcid.Text)
        fetch_cust()
        txtadate.Text = Format(Now, "dd-MM-yyyy")
        '  System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtadate)

        txtfocus(deptyp)

    End Sub




    Private Sub btn_nxt_jsp_Click(sender As Object, e As EventArgs) Handles btn_nxt_jsp.Click

        ' Guard: no ROI was found for the selected date
        If Trim(txtcint.Text) = "0" AndAlso Trim(txtdint.Text) = "0" Then
            Dim stitle As String = "Hi " & Session("sesusr")
            Dim msg As String = "No ROI available for the date: " & Trim(txtadate.Text)
            Dim fnc As String = "showToastnOK('" & stitle & "','" & msg & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "toastNoRoi", fnc, True)
            btn_nxt_jsp.Enabled = True
            Exit Sub
        End If

        btn_nxt_jsp.Enabled = False

        If isrebate.Checked = True Then
            If ssbr.SelectedValue <> "" And ssdep.SelectedValue <> "" And Trim(txtrebateacn.Text) <> "" Then

                open_loan()
                clea()

            End If

        Else
            open_loan()
            clea()


        End If




    End Sub
    Private Sub chkOtherBranchShare_CheckedChanged(sender As Object, e As EventArgs) Handles chkOtherBranchShare.CheckedChanged
        get_int()
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

        If con.State = ConnectionState.Closed Then con.Open()

        Dim shrval As Integer = get_share_value(txtcid.Text)
        cmd.Connection = con

        Dim dr_roi As SqlDataReader


        If Trim(deptyp.SelectedItem.Text) = "DCL" Then
            txtcint.Text = "15.00"
            txtdint.Text = "27.60"
        End If

        If deptyp.SelectedItem.Text = "ML" Then

            query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy AND minsc=@shr AND agst=@sch order by fyfrm"


            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()
            cmd.Connection = con


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@prod", deptyp.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@prdtyp", "D")
            cmd.Parameters.AddWithValue("@prdx", 10)
            cmd.Parameters.AddWithValue("@prdy", 10)
            cmd.Parameters.AddWithValue("@sch", sch.SelectedItem.Text)
            ' If shrval < 500 Then
            '     cmd.Parameters.AddWithValue("@shr", 49)
            ' Else
            '     cmd.Parameters.AddWithValue("@shr", 51)
            ' End If

            If shrval < 100 AndAlso Not chkOtherBranchShare.Checked Then
                cmd.Parameters.AddWithValue("@shr", 49)
            Else
                cmd.Parameters.AddWithValue("@shr", 51)
            End If

        Else
            query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"


            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()
            cmd.Connection = con


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@prod", deptyp.SelectedItem.Text)
            If deptyp.SelectedItem.Text = "DL" Then
                cmd.Parameters.AddWithValue("@prdtyp", "M")
            Else
                cmd.Parameters.AddWithValue("@prdtyp", "D")
            End If
            cmd.Parameters.AddWithValue("@prdx", 10)
            cmd.Parameters.AddWithValue("@prdy", 10)

        End If




        dr_roi = cmd.ExecuteReader()

        Dim roiFound As Boolean = False
        Dim parsedAdate As DateTime = DateTime.ParseExact(Trim(txtadate.Text), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)

        If dr_roi.HasRows() Then

            While dr_roi.Read

                Dim FYFRM As Date = dr_roi(2)
                Dim FYTO As Date = dr_roi(3)

                Dim x As Long = FYFRM.CompareTo(parsedAdate)

                If x < 0 Then

                    Dim y As Long = FYTO.CompareTo(parsedAdate)

                    If y > 0 Then
                        Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                        Session("dint") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                        txtcint.Text = Session("cintr")
                        txtdint.Text = Session("dint")
                        roiFound = True

                        Exit While
                    End If

                End If
            End While

        End If

        dr_roi.Close()

        If Not roiFound Then
            txtcint.Text = "0"
            txtdint.Text = "0"
        End If




    End Sub


    Sub get_dl_int()
        Try


            query = "SELECT cint,dint,FYFRM,FYTO,prdfrm,prdto FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND agst=@agst  order by fyfrm desc"

            Dim dr_roi As SqlDataReader


            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()
            cmd.Connection = con


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@prod", "DL")
            cmd.Parameters.AddWithValue("@prdtyp", "M")
            'cmd.Parameters.AddWithValue("@prdx", 1)
            'cmd.Parameters.AddWithValue("@prdy", 1)
            cmd.Parameters.AddWithValue("@agst", Session("depagst"))




            dr_roi = cmd.ExecuteReader()

            If dr_roi.HasRows() Then
                ' dr_roi.Read()

                Dim parsedDepDate As DateTime
                If TypeOf Session("depdate") Is DateTime Then
                    parsedDepDate = CType(Session("depdate"), DateTime)
                Else
                    parsedDepDate = DateTime.Parse(Session("depdate").ToString())
                End If

                While dr_roi.Read

                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)

                    Dim x As Long = FYFRM.CompareTo(parsedDepDate)

                    If x = -1 Then

                        If dr_roi(4) <= Session("agst_prd") And dr_roi(5) = Session("agst_prd") Then
                            Dim y As Long = FYTO.CompareTo(parsedDepDate)

                            If y = 1 Then
                                txtcint.Text = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                                txtdint.Text = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))
                                Dim lbres As Double
                                Dim txtd As Double
                                Double.TryParse(lbld.Text, lbres)
                                Double.TryParse(txtdint.Text, txtd)
                                If lbres < txtd Then
                                    lblc.Text = txtcint.Text
                                    lbld.Text = txtdint.Text
                                End If
                                Exit While
                            End If
                        End If
                    End If
                End While


            End If

            dr_roi.Close()




        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub

    'Sub get_service_charge()

    '    Dim oresult As Double = 0

    '    If con.State = ConnectionState.Closed Then con.Open()

    '    query = "SELECT service.srvchr FROM dbo.service WHERE service.lon = @lon AND service.frm <= @frm AND service.[to] >= @to"

    '    cmd.Connection = con
    '    cmd.CommandText = query

    '    cmd.Parameters.AddWithValue("@lon", deptyp.SelectedItem.Text)
    '    cmd.Parameters.AddWithValue("@frm", txtamt.Text)
    '    cmd.Parameters.AddWithValue("@to", txtamt.Text)

    '    oresult = cmd.ExecuteScalar()

    '    txtserchrg.Text = Format(oresult, "0.00")


    'End Sub

    'Private Sub txtamt_TextChanged(sender As Object, e As EventArgs) Handles txtamt.TextChanged

    '    Dim tmplamt As Double

    '    tmplamt = Math.Round(CDbl(Session("lamt")))
    '    Try
    '        If deptyp.SelectedItem.Text = "JL" Or deptyp.SelectedItem.Text = "DL" Then
    '            If CDbl(txtamt.Text) <= CDbl(tmplamt) Then
    '                '  txtamt.Text = Format(txtamt.Text, "0.00")
    '                lblrvamt.Visible = False
    '                get_int()
    '                get_service_charge()

    '                System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(btn_update)

    '            Else
    '                lblrvamt.Visible = True
    '                lblrvamt.Text = "Loan Amount must be Less than " + CStr(tmplamt)
    '                txtfocus(txtamt)
    '            End If

    '        Else
    '            ' txtamt.Text = Format(txtamt.Text, "0.00")
    '            lblrvamt.Visible = False
    '            get_int()
    '            get_service_charge()


    '        End If

    '    Catch ex As Exception
    '        Response.Write(ex.ToString)
    '    End Try

    'End Sub

    'Sub get_bal()

    'End Sub
    Sub get_acinfo()

        If con.State = ConnectionState.Closed Then con.Open()


        query = "select date,amount,PRD from dbo.master where acno='" + txtacn.Text + "'"

        cmd.Connection = con
        cmd.CommandText = query

        Dim rds As SqlDataReader

        Try
            rds = cmd.ExecuteReader()

            If rds.HasRows() Then
                rds.Read()

                txtddt.Text = rds(0)
                Session("depdate") = rds(0)
                txtdepamt.Text = Format(rds(1), "0")
                Session("agst_prd") = rds(2)


            End If


        Catch ex As Exception

            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()
        End Try


        If con.State = ConnectionState.Closed Then con.Open()

        Dim x As Integer = 1
        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno =@acno and scroll =@scroll GROUP BY [actrans].acno"
        cmd.Parameters.AddWithValue("@acno", txtacn.Text)
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
            txtacbal.Text = reader(1).ToString - reader(0).ToString
            ' cbal = reader(3).ToString - reader(2).ToString



        Else

            txtacbal.Text = "0"
        End If






    End Sub
    Private Sub txtacn_TextChanged(sender As Object, e As EventArgs) Handles txtacn.TextChanged

        get_acinfo()


    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click

        If Not CDbl(txtacbal.Text) = 0 Then
            btn_nxt_ds.Enabled = True
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

            newrow(0) = txtacn.Text
            newrow(1) = txtddt.Text 'DateTime.Parse(String.Format("{0}:dd-mm-yyyy", txtddt.Text))
            newrow(2) = String.Format("{0:N}", CDbl(txtdepamt.Text))
            newrow(3) = String.Format("{0:N}", CDbl(txtacbal.Text))

            If Session("cum_bal") Is Nothing Then
                tqty = CDbl(txtacbal.Text)
            Else
                tqty = CType(Session("cum_bal"), Double) + CDbl(txtacbal.Text)
            End If


            dt_j.Rows.Add(newrow)
            dtdl.ImportRow(newrow)

            dispdl.DataSource = dtdl
            dispdl.DataBind()
            Session("dspec") = dt_j
            Session("cum_bal") = tqty


            lblamt.Text = Format(tqty, "0.00")
            'lbltqty.Text = tqtyvi
            'lblgross.Text = Format(tgross, "##,##0.000") 'String.Format("{0:#,###.###}", tgross)
            'lblnet.Text = Format(tnet, "##,##0.000") 'String.Format("{0:#,###.###}", tnet)
            'lblval.Text = String.Format("{0:N}", (tnet * ratepergm))

            Select Case Mid(txtacn.Text, 1, 2)
                Case 74
                    Session("depagst") = "DS"
                Case 75
                    Session("depagst") = "FD"
                Case 76
                    Session("depagst") = "KMK"
                Case 77
                    Session("depagst") = "RD"
                Case 78
                    Session("depagst") = "RID"
                Case 79
                    Session("depagst") = "SB"
            End Select
            ' Session("depagst") =
            get_dl_int()

            txtacn.Text = ""
            txtacbal.Text = ""
            txtddt.Text = ""
            txtdepamt.Text = ""

        Else
            txtacn.Text = ""
            txtacbal.Text = ""
            txtddt.Text = ""
            txtdepamt.Text = ""


        End If

        Me.Page.SetFocus(txtacn.ClientID)

        Session("Tabin") = 2

    End Sub

    Private Sub btn_nxt_ds_Click(sender As Object, e As EventArgs) Handles btn_nxt_ds.Click

        btn_nxt_ds.Enabled = False

        'loantab.Visible = True
        'TabContainer1.ActiveTab = loantab
        'lamt = CDbl(lblamt.Text)
        'txtfocus(txtamt)
        open_loan()
        clea()


    End Sub


    Function get_acnprefix(ByVal dep As String)

        Dim acnprefix As String = "00"

        query = "select acnprefix from products where shortname='" + dep + "'"

        cmd.Connection = con
        cmd.CommandText = query

        oResult = cmd.ExecuteScalar()

        If oResult IsNot Nothing Then
            acnprefix = oResult

        End If






        Return acnprefix
    End Function
    Private Sub update_int(ByVal tid As Double, ByVal drd As Double, ByVal crd As Double, ByVal drc As Double, ByVal crc As Double, ByVal nar As String, ByVal supliment As String, ByVal cbal As Double, ByVal dbal As Double, ByVal typ As String)
        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con


        'Dim d As String = get_due(txtacn.Text, 0)

        query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"

        cmd.Parameters.Clear()

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@uiid", tid)
        cmd.Parameters.AddWithValue("@uidate", Session("ac_date"))
        cmd.Parameters.AddWithValue("@uiacno", Session("acn"))
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


            'response.write(ex.Message)

        Finally

            cmd.Dispose()
            con.Close()
        End Try


    End Sub

    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()

        cmd.Connection = con
        query = "INSERT INTO suplement(date,id,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@sudt", Session("ac_date"))
        cmd.Parameters.AddWithValue("@transid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@usacn", Session("acn"))
        cmd.Parameters.AddWithValue("@nar", nar)
        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            Response.Write(ex.Message)

        Finally
            cmd.Dispose()

            con.Close()



        End Try

        query = ""



    End Sub


    Sub set_diff(ByVal ovr As Double)
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con

        cmd.Parameters.Clear()
        query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
        query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@tid", 0)
        cmd.Parameters.AddWithValue("@date", Session("ac_date"))
        cmd.Parameters.AddWithValue("@product", deptyp.Text)
        cmd.Parameters.AddWithValue("@acno", Session("acn"))
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

    Sub open_loan()

        Dim ALERT As Integer = 0


        If session_user_role = "Audit" Then Exit Sub

        get_int()
        Dim c_int As Double
        Dim d_int As Double

        If Not Double.TryParse(lblc.Text, c_int) Then
            c_int = CDbl(txtcint.Text)
        End If
        If Not Double.TryParse(lbld.Text, d_int) Then
            d_int = CDbl(txtdint.Text)
        End If


        If con.State = ConnectionState.Closed Then con.Open()

        Dim acnp As String = get_acnprefix(deptyp.SelectedItem.Text)


        Dim dat As DateTime = DateTime.ParseExact(CStr("01-01-") + CStr(DateAndTime.Year(Convert.ToDateTime(txtadate.Text))), "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        Session("aid") = Trim(acnp) + CStr(DateAndTime.Year(Convert.ToDateTime(txtadate.Text)))



        cmd.Connection = con
        cmd.CommandText = "SELECT TOP 1 serial FROM masterc where product=@prd and masterc.date>=@dt ORDER BY date DESC,masterc.serial DESC"
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@prd", deptyp.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@dt", reformatted)

        Try
            oResult = cmd.ExecuteScalar()

            If oResult IsNot Nothing Then

                Session("serial") = Int(oResult.ToString) + 1

            Else
                Session("serial") = 1
            End If


            Session("acn") = Session("aid") + String.Format("{0:00000}", Session("serial"))


        Catch ex As Exception

            Response.Write(ex.ToString)

        End Try

        Session("ac_date") = txtadate.Text



        Dim query As String = String.Empty

        cmd.Parameters.Clear()
        cmd.Connection = con

        query = "INSERT INTO masterc(date,acno,cid,serial,product,amount,cint,dint,cld,prd,prdtype,sch,alert,parent,IsOtherBranchShareholder)"
        query &= " VALUES(@date,@acno,@cid,@serial,@product,@amount,@cint,@dint,@cld,@prd,@prdtype,@sch,@alert,@parent,@IsOtherBranchShareholder)"

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtadate.Text))
        cmd.Parameters.AddWithValue("@acno", Session("acn"))
        cmd.Parameters.AddWithValue("@cid", Trim(txtcid.Text))
        cmd.Parameters.AddWithValue("@serial", Session("serial"))
        cmd.Parameters.AddWithValue("@product", deptyp.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@amount", 0)
        cmd.Parameters.AddWithValue("@cint", c_int)
        cmd.Parameters.AddWithValue("@dint", d_int)
        cmd.Parameters.AddWithValue("@cld", 0)
        cmd.Parameters.AddWithValue("@prd", 0)
        cmd.Parameters.AddWithValue("@prdtype", "D")
        If deptyp.SelectedItem.Text = "ML" Then
            cmd.Parameters.AddWithValue("@sch", sch.SelectedItem.Text)
        Else
            cmd.Parameters.AddWithValue("@sch", "")
        End If

        If issms.Checked = True Then
            cmd.Parameters.AddWithValue("@alert", 1)
        Else
            cmd.Parameters.AddWithValue("@alert", 0)
        End If
        cmd.Parameters.AddWithValue("@parent", Session("acn"))
        cmd.Parameters.AddWithValue("@IsOtherBranchShareholder", chkOtherBranchShare.Checked)

        '  cmd.Parameters.AddWithValue("@mdate", txtadate.Text)
        '  cmd.Parameters.AddWithValue("@mamt", 0)

        Try

            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



        query = ""

        '   Dim query As String = String.Empty

        cmd.Parameters.Clear()
        cmd.Connection = con

        query = "INSERT INTO master(date,acno,cid,serial,product,amount,cint,dint,cld,prd,prdtype,sch,alert,IsOtherBranchShareholder)"
        query &= " VALUES(@date,@acno,@cid,@serial,@product,@amount,@cint,@dint,@cld,@prd,@prdtype,@sch,@alert,@IsOtherBranchShareholder)"

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtadate.Text))
        cmd.Parameters.AddWithValue("@acno", Session("acn"))
        cmd.Parameters.AddWithValue("@cid", Trim(txtcid.Text))
        cmd.Parameters.AddWithValue("@serial", Session("serial"))
        cmd.Parameters.AddWithValue("@product", deptyp.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@amount", 0)
        cmd.Parameters.AddWithValue("@cint", c_int)
        cmd.Parameters.AddWithValue("@dint", d_int)
        cmd.Parameters.AddWithValue("@cld", 0)
        cmd.Parameters.AddWithValue("@prd", 0)
        cmd.Parameters.AddWithValue("@prdtype", "D")
        If deptyp.SelectedItem.Text = "ML" Then
            cmd.Parameters.AddWithValue("@sch", sch.SelectedItem.Text)
        Else
            cmd.Parameters.AddWithValue("@sch", "")
        End If

        If issms.Checked = True Then
            cmd.Parameters.AddWithValue("@alert", 1)
        Else
            cmd.Parameters.AddWithValue("@alert", 0)
        End If
        cmd.Parameters.AddWithValue("@IsOtherBranchShareholder", chkOtherBranchShare.Checked)

        '  cmd.Parameters.AddWithValue("@mdate", txtadate.Text)
        '  cmd.Parameters.AddWithValue("@mamt", 0)

        Try

            cmd.ExecuteNonQuery()

            If deptyp.SelectedItem.Text = "JL" Then

                update_jlspec()

            ElseIf deptyp.SelectedItem.Text = "DL" Then

                update_dlspec()

            ElseIf deptyp.SelectedItem.Text = "DCL" Then
                '  update_dcl()


            End If


        Catch ex As Exception

            Response.Write(ex.ToString)

        Finally


        End Try

        If isrebate.Checked = True Then

            query = "insert into DEPOBR(lacno,branch,dep,depacno)"
            query &= " values(@lacno,@branch,@dep,@depacno)"


            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Connection = con

            cmd.Parameters.Clear()

            cmd.CommandText = query

            cmd.Parameters.AddWithValue("@lacno", Session("acn"))
            cmd.Parameters.AddWithValue("@branch", ssbr.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@dep", ssdep.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@depacno", txtrebateacn.Text)

            Try

                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try


        End If


        'If Not CDbl(txtserchrg.Text) = 0 Then


        '    update_service_charges()
        'End If


        'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

        ''sb.Append("<div class=" + """alert alert-dismissable alert-info """ + ">")
        ''sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
        ''sb.Append("<strong>Account Created !</strong> Account No is " + acn)
        ''sb.Append("</div>")
        'sb.Append("<script type = 'text/javascript'>")
        'sb.Append("notif({")
        'sb.Append("type:" + "info" + ",")
        'sb.Append(" msg: " + "Account Created." + "<br>" + "Account No is" + acn + ",")
        'sb.Append("position:" + "right" + ",")
        'sb.Append("width: 500,")
        'sb.Append("height: 60,")
        'sb.Append("autohide: false")
        'sb.Append("});")
        'sb.Append("</script>")

        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "accreated(" + acn + ")", True)



        ''lblacn.Text = "Account No :  " + Session("acn")
        ''Me.ModalPopup1.Show()


        Dim stitle As String = "Hi " + Session("sesusr")
        Dim msg As String = "Loan Account No  " + Session("acn").ToString + " Created.</b>&nbsp;&nbsp;&nbsp;<a href=\loanpayment.aspx\>&nbsp;&nbsp;Click here to Update the Payment</a>"


        '  msg = "Member No:&nbsp;&nbsp;&nbsp;<b>" + Session("ncid") + "</b>&nbsp;&nbsp;&nbsp;<a href=\kyc.aspx\>&nbsp;&nbsp;Click here to Update the KYC</a>"


        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


    End Sub
    'Sub update_service_charges()

    '    If con.State = ConnectionState.Closed Then con.Open()

    '    cmd.Parameters.Clear()

    '    query = "insert into diff(tid,date,product,acno,dr,cr)"
    '    query &= "values(@tid,@date,@product,@acmo,@dr,@cr)"


    '    cmd.Connection = con
    '    cmd.CommandText = query

    '    cmd.Parameters.AddWithValue("@tid", 0)
    '    cmd.Parameters.AddWithValue("@date", Session("ac_date"))
    '    cmd.Parameters.AddWithValue("@product", deptyp.SelectedItem.Text)
    '    cmd.Parameters.AddWithValue("@acmo", Session("acn"))
    '    cmd.Parameters.AddWithValue("@dr", 0)
    '    cmd.Parameters.AddWithValue("@cr", CDbl(txtserchrg.Text))

    '    Try
    '        cmd.ExecuteNonQuery()

    '    Catch ex As Exception

    '        Response.Write(ex.ToString)
    '    Finally
    '        con.Close()
    '        cmd.Dispose()

    '    End Try


    'End Sub
    Sub update_jlspec()

        Dim dt_j As New DataTable

        If Session("jspec") Is Nothing = False Then
            dt_j = CType(Session("jspec"), DataTable)

            Dim count As Integer = dt_j.Rows.Count

            For Each row1 As DataRow In dt_j.Rows

                Dim jlspec As String = row1(0).ToString
                Dim qty As Integer = row1(1).ToString
                Dim gross As Decimal = row1(2).ToString
                Dim net As Decimal = row1(3).ToString
                Dim imgdata As Byte() = Nothing
                If dt_j.Columns.Contains("imagedata") AndAlso Not IsDBNull(row1("imagedata")) Then
                    imgdata = DirectCast(row1("imagedata"), Byte())
                End If

                cmd.Parameters.Clear()

                If con.State = ConnectionState.Closed Then con.Open()

                query = "INSERT INTO jlspec(acno,itm,qty,gross,net,ImageData)"
                query &= "Values(@acn,@itm,@qty,@gross,@net,@ImageData)"

                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@acn", Session("acn"))
                cmd.Parameters.AddWithValue("@itm", jlspec)
                cmd.Parameters.AddWithValue("@qty", qty)
                cmd.Parameters.AddWithValue("@gross", gross)
                cmd.Parameters.AddWithValue("@net", net)
                If imgdata Is Nothing Then
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary, -1).Value = DBNull.Value
                Else
                    cmd.Parameters.AddWithValue("@ImageData", imgdata)
                End If

                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)

                Finally
                    con.Close()
                    cmd.Dispose()

                End Try

            Next

            cmd.Parameters.Clear()

            If con.State = ConnectionState.Closed Then con.Open()


            query = "insert into jlstock(date,acn,tqty,tgross,tnet)"
            query &= "values(@date,@acn,@tqty,@tgross,@tnet)"

            cmd.Connection = con
            cmd.CommandText = query

            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtadate.Text))
            cmd.Parameters.AddWithValue("@acn", Session("acn"))

            If (Session("tqt") Is Nothing) = False Then
                cmd.Parameters.AddWithValue("@tqty", CType(Session("tqt"), Integer))
            Else
                cmd.Parameters.AddWithValue("@tqty", 0)
            End If

            If (Session("tgross") Is Nothing) = False Then
                cmd.Parameters.AddWithValue("@tgross", CType(Session("tgross"), Decimal))
            Else
                cmd.Parameters.AddWithValue("@tgross", 0)
            End If

            If (Session("tnet") Is Nothing) = False Then
                cmd.Parameters.AddWithValue("@tnet", CType(Session("tnet"), Decimal))
            Else
                cmd.Parameters.AddWithValue("@tnet", 0)
            End If

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)

            Finally
                con.Close()
                cmd.Dispose()



            End Try


        End If

    End Sub

    Sub update_dlspec()
        Dim dt_j As New DataTable

        If Session("dspec") Is Nothing = False Then
            dt_j = CType(Session("dspec"), DataTable)

            Dim count As Integer = dt_j.Rows.Count

            For Each row1 As DataRow In dt_j.Rows

                Dim dlspec As String = row1(0).ToString
                cmd.Parameters.Clear()

                If con.State = ConnectionState.Closed Then con.Open()

                query = "INSERT INTO dlspec(acn,deposit)"
                query &= "Values(@acn,@itm)"

                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@acn", Session("acn"))
                cmd.Parameters.AddWithValue("@itm", dlspec)

                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)

                Finally
                    con.Close()
                    cmd.Dispose()

                End Try

            Next

            cmd.Parameters.Clear()
        End If


    End Sub

    Sub clea()

        txtcof.Text = ""
        'txtserchrg.Text = ""
        'txtintc.Text = ""
        'txtintd.Text = ""
        'txtamt.Text = ""
        'txtcof.Text = ""
        btn_nxt_ms.Text = "Next"
        ' loantab.Visible = False
        imgCapture.Visible = False
        ' If dltab.Visible = True Then

        Session("dspec") = Nothing

        dispdl.DataSource = Nothing
        dispdl.DataBind()

        lblamt.Text = ""

        '     dltab.Visible = False
        ' End If


        ' If jeweltab.Visible = True Then

        Session("jspec") = Nothing
        Session("tqt") = Nothing
        Session("tgross") = Nothing
        Session("tnet") = Nothing

        ' lblrate.Text = ""
        ' lblval.Text = ""
        lbltqty.Text = ""
        lblgross.Text = ""
        lblnet.Text = ""
        disp.DataSource = Nothing
        disp.DataBind()

        '    jeweltab.Visible = False

        '        End If

        txtrebateacn.Text = ""
        ssbr.SelectedValue = ""
        ssdep.SelectedValue = ""
        rebate.Visible = False
        isrebate.Checked = False
        issms.Checked = False
        txtcid.Text = ""
        txtname.Text = ""
        txtadd.Text = ""
        txtcint.Text = ""
        txtdint.Text = ""
        deptyp.SelectedIndex = 0
        Session("Tabin") = 0
        btn_nxt_jsp.Enabled = True
        btn_nxt_ds.Enabled = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "ShowCTab()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Showhome", "ShowDLS(0)", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
    End Sub

    Private Sub deptyp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles deptyp.SelectedIndexChanged

        If Not deptyp.SelectedItem.Text = "Select" Then
            btn_nxt_ms.Enabled = True


            If Trim(deptyp.SelectedItem.Text) = "DCL" Or Trim(deptyp.SelectedItem.Text) = "ML" Then

                Session("Tabin") = 0
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showctab", "ShowCTab()", True)
                If Trim(deptyp.SelectedItem.Text) = "ML" Then
                    pnlsch.Visible = True
                    chkOtherBranchShare.Visible = True
                Else
                    pnlsch.Visible = False
                    chkOtherBranchShare.Visible = False
                    chkOtherBranchShare.Checked = False
                End If
                btn_nxt_ms.Text = "Update"
            Else
                pnlsch.Visible = False
                chkOtherBranchShare.Visible = False
                chkOtherBranchShare.Checked = False

                If Trim(deptyp.SelectedItem.Text) = "JL" Then
                    get_ratepergram()

                    Session("Tabin") = 1
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showjltab", "ShowJlTab()", True)

                End If
                If Trim(deptyp.SelectedItem.Text) = "DL" Then
                    Session("Tabin") = 2
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showdltab", "ShowDlTab()", True)

                End If

                btn_nxt_ms.Text = "Next"
            End If
            get_int()
            txtfocus(btn_nxt_ms)
        End If

    End Sub

    Private Sub deptyp_TextChanged(sender As Object, e As EventArgs) Handles deptyp.TextChanged





        If Not deptyp.SelectedItem.Text = "Select" Then
            btn_nxt_ms.Enabled = True
            get_int()

            If Trim(deptyp.SelectedItem.Text) = "DCL" Or Trim(deptyp.SelectedItem.Text) = "ML" Then
                btn_nxt_ms.Text = "Update"
            Else
                btn_nxt_ms.Text = "Next"
            End If
            txtfocus(btn_nxt_ms)
        End If
    End Sub

    Private Sub btn_clr_ms_Click(sender As Object, e As EventArgs) Handles btn_clr_ms.Click
        txtcid.Text = ""
        txtname.Text = ""
        txtadd.Text = ""
        deptyp.SelectedIndex = 0
        imgCapture.Visible = False
        btn_nxt_ms.Enabled = False
        txtfocus(txtcid)

    End Sub


    Private Sub btn_clr_ds_Click(sender As Object, e As EventArgs) Handles btn_clr_ds.Click
        btn_nxt_ds.Enabled = False
        dispdl.DataSource = Nothing
        dispdl.DataBind()
        Session("dspec") = Nothing
        Session("cum_bal") = Nothing
        lblc.Text = ""
        lbld.Text = ""
        lblamt.Text = 0
        txtacn.Text = ""
        txtacbal.Text = ""
        txtddt.Text = ""
        txtdepamt.Text = ""
        txtfocus(txtacn)




    End Sub

    Private Sub btn_clr_jsp_Click(sender As Object, e As EventArgs) Handles btn_clr_jsp.Click
        btn_nxt_jsp.Enabled = False

        tqty = 0
        tnet = 0
        tgross = 0
        disp.DataSource = Nothing
        disp.DataBind()
        Session("jspec") = Nothing
        Session("tnet") = Nothing
        Session("tgross") = Nothing
        Session("tqt") = Nothing


        lbltqty.Text = 0
        lblgross.Text = "0" 'Format(tgross, "##,##0.000") 'String.Format("{0:#,###.###}", tgross)
        lblnet.Text = "0" 'Format(tnet, "##,##0.000") 'String.Format("{0:#,###.###}", tnet)
        'lblval.Text = "0"

        txtqty.Text = ""
        txtgross.Text = ""
        txtnet.Text = ""

        txtfocus(jwlspecs)

    End Sub


    Private Sub sch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sch.SelectedIndexChanged
        get_int()

    End Sub



    Private Sub isrebate_CheckedChanged(sender As Object, e As EventArgs) Handles isrebate.CheckedChanged

        If isrebate.Checked = True Then

            rebate.Visible = True
            Me.Page.SetFocus(isrebate.ClientID)

        Else
            rebate.Visible = False
        End If

    End Sub

    Private Sub issms_CheckedChanged(sender As Object, e As EventArgs) Handles issms.CheckedChanged

    End Sub

    Private Sub btnAddJewel_Click(sender As Object, e As EventArgs) Handles btnAddJewel.Click

        Dim dt_j As New DataTable

        If dt_j.Columns.Count = 0 Then
            dt_j.Columns.Add("itm", GetType(String))
            dt_j.Columns.Add("qty", GetType(Integer))
            dt_j.Columns.Add("gross", GetType(Decimal))
            dt_j.Columns.Add("net", GetType(Decimal))
            dt_j.Columns.Add("imagedata", GetType(Byte()))
        End If

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("itm", GetType(String))
            dt.Columns.Add("qty", GetType(Integer))
            dt.Columns.Add("gross", GetType(Decimal))
            dt.Columns.Add("net", GetType(Decimal))
            dt.Columns.Add("base64image", GetType(String))
        End If

        If Session("jspec") Is Nothing = False Then
            dt_j = CType(Session("jspec"), DataTable)
            For Each row1 As DataRow In dt_j.Rows
                Dim r As DataRow = dt.NewRow()
                r("itm") = row1("itm")
                r("qty") = row1("qty")
                r("gross") = row1("gross")
                r("net") = row1("net")
                If dt_j.Columns.Contains("imagedata") AndAlso Not IsDBNull(row1("imagedata")) Then
                    Dim imgdata As Byte() = DirectCast(row1("imagedata"), Byte())
                    Dim base64String As String = Convert.ToBase64String(imgdata)
                    r("base64image") = "data:image/jpeg;base64," & base64String
                Else
                    r("base64image") = "" ' Or placeholder image
                End If
                dt.Rows.Add(r)
            Next
        End If
        
        If Trim(jwlspecs.Text) = "" Or Trim(txtnet.Text) = "" Or Trim(txtgross.Text) = "" Or Trim(txtqty.Text) = "" Then
            Exit Sub
        End If

        newrow = dt_j.NewRow

        newrow(0) = jwlspecs.Text
        newrow(1) = txtqty.Text
        newrow(2) = txtgross.Text
        newrow(3) = txtnet.Text
        
        If Not Session("CurrentJewelImage") Is Nothing Then
            newrow(4) = DirectCast(Session("CurrentJewelImage"), Byte())
            Session("CurrentJewelImage") = Nothing
            imgJewelCapture.Visible = False
            imgJewelCapture.ImageUrl = ""
        Else
            newrow(4) = DBNull.Value
        End If

        If Session("tqt") Is Nothing Then
            tqty = CInt(txtqty.Text)
        Else
            tqty = CType(Session("tqt"), Integer) + CInt(txtqty.Text)
        End If

        If Session("tgross") Is Nothing Then
            tgross = Convert.ToDecimal(txtgross.Text)
        Else
            tgross = CType(Session("tgross"), Decimal) + Convert.ToDecimal(txtgross.Text)
        End If

        If Session("tnet") Is Nothing Then
            tnet = Convert.ToDecimal(txtnet.Text)
        Else
            tnet = CType(Session("tnet"), Decimal) + Convert.ToDecimal(txtnet.Text)
        End If

        dt_j.Rows.Add(newrow)
        
        Dim dispRow As DataRow = dt.NewRow()
        dispRow("itm") = newrow("itm")
        dispRow("qty") = newrow("qty")
        dispRow("gross") = newrow("gross")
        dispRow("net") = newrow("net")
        If Not IsDBNull(newrow("imagedata")) Then
            Dim imgdata As Byte() = DirectCast(newrow("imagedata"), Byte())
            Dim base64String As String = Convert.ToBase64String(imgdata)
            dispRow("base64image") = "data:image/jpeg;base64," & base64String
        Else
            dispRow("base64image") = ""
        End If
        dt.Rows.Add(dispRow)

        disp.DataSource = dt
        disp.DataBind()
        Session("jspec") = dt_j
        Session("tnet") = tnet
        Session("tgross") = tgross
        Session("tqt") = tqty

        lbltqty.Text = tqty
        lblgross.Text = Format(tgross, "##,##0.000") 
        lblnet.Text = Format(tnet, "##,##0.000") 
        lblval.Text = String.Format("{0:N}", (tnet * Session("ratepergm")))

        txtqty.Text = ""
        txtgross.Text = ""
        txtnet.Text = ""
        btn_nxt_jsp.Enabled = True
        jwlspecs.Text = ""

        Me.Page.SetFocus(jwlspecs.ClientID)

    End Sub

    Protected Sub disp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles disp.RowCommand
        If e.CommandName = "RemoveJewel" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim dt_j As DataTable = CType(Session("jspec"), DataTable)
            
            Dim remQty As Integer = CInt(dt_j.Rows(index)("qty"))
            Dim remGross As Decimal = Convert.ToDecimal(dt_j.Rows(index)("gross"))
            Dim remNet As Decimal = Convert.ToDecimal(dt_j.Rows(index)("net"))
            
            dt_j.Rows(index).Delete()
            dt_j.AcceptChanges()
            
            Session("tqt") = CType(Session("tqt"), Integer) - remQty
            Session("tgross") = CType(Session("tgross"), Decimal) - remGross
            Session("tnet") = CType(Session("tnet"), Decimal) - remNet
            
            tqty = Session("tqt")
            tgross = Session("tgross")
            tnet = Session("tnet")
            
            lbltqty.Text = tqty
            lblgross.Text = Format(tgross, "##,##0.000")
            lblnet.Text = Format(tnet, "##,##0.000")
            lblval.Text = String.Format("{0:N}", (tnet * Session("ratepergm")))
            
            Session("jspec") = dt_j
            disp.DataSource = dt_j
            disp.DataBind()
            
            If dt_j.Rows.Count = 0 Then
                btn_nxt_jsp.Enabled = False
            End If
        End If
    End Sub

    Private Sub btn_upimg_Click(sender As Object, e As EventArgs) Handles btn_upimg.Click
        If img_file.HasFile Then
            Session("CurrentJewelImage") = img_file.FileBytes
            
            ' Temporarily save to show preview 
            Dim previewPath As String = "~/Captures/temppreview_" & Session.SessionID & ".png"
            img_file.PostedFile.SaveAs(Server.MapPath(previewPath))
            imgJewelCapture.ImageUrl = previewPath & "?" & DateTime.Now.Ticks.ToString()
            imgJewelCapture.Visible = True
        End If
    End Sub
    
    Private Sub btn_img_can_Click(sender As Object, e As EventArgs) Handles btn_img_can.Click
        imgJewelCapture.Visible = True
        imgJewelCapture.ImageUrl = "~/Captures/temppreview_" & Session.SessionID & ".png?" & DateTime.Now.Ticks.ToString()
    End Sub

    <WebMethod()>
    Public Shared Function SaveCapturedImage(ByVal data As String) As Boolean
        Try
            Dim imageBytes() As Byte = Convert.FromBase64String(data.Split(",")(1))
            HttpContext.Current.Session("CurrentJewelImage") = imageBytes
            
            ' Save a temp preview
            Dim fileName As String = "temppreview_" & HttpContext.Current.Session.SessionID & ".png"
            Dim filePath As String = HttpContext.Current.Server.MapPath(String.Format("~/Captures/{0}", fileName))
            File.WriteAllBytes(filePath, imageBytes)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Sub get_ratepergram()

        Dim rate As Double

        If con.State = ConnectionState.Closed Then con.Open()

        query = "Select rate from dbo.goldrate"

        cmd.Connection = con
        cmd.CommandText = query



        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = "select top 1 COALESCE( rate,0) from goldrate ORDER BY goldrate.rate DESC"
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    rate = cmd.ExecuteScalar()

                    If Not rate = 0 Then
                        Session("ratepergm") = rate
                        lblrate.Text = String.Format("{0:N}", Session("ratepergm"))
                    Else
                        Session("ratepergm") = rate
                        lblrate.Text = String.Format("{0:N}", Session("ratepergm"))

                    End If

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()


        End Using



    End Sub



    Private Sub btn_nxt_ms_Click(sender As Object, e As EventArgs) Handles btn_nxt_ms.Click

        If Not txtname.Text = "" Then


            Select Case deptyp.SelectedItem.Text

                Case "DCL"
                    ' loantab.Visible = True
                    ' TabContainer1.ActiveTab = loantab
                    ' txtfocus(txtamt)
                    open_loan()
                    clea()

                Case "DL"

                    Session("Tabin") = 2

                    ''ScriptManager.RegisterStartupScript(Me.GetType(), Guid.NewGuid().ToString(), "ShowDLS()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowStatus", "ShowDLS(2)", True)




                Case "JL"
                    Session("Tabin") = 1
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowStatus", "ShowDLS(1)", True)
                Case "ML"
                    '  loantab.Visible = True
                    '  TabContainer1.ActiveTab = loantab
                    '  txtfocus(txtamt)

                    open_loan()
                    clea()


            End Select

        Else
            'lblcid404.Text = "Invalid Customer Id"
            'lblcid404.Visible = True
            'txtfocus(txtcid)
        End If


    End Sub

    Private Sub LoanOpening_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub jladd_Click(sender As Object, e As EventArgs) Handles jladd.Click

        If Not Trim(txtjwlname.Text) = "" Then
            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()

            query = "insert into jewel(jlname)"
            query &= "values(@jlname)"


            cmd.Connection = con
            cmd.CommandText = query

            cmd.Parameters.AddWithValue("@jlname", txtjwlname.Text)

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception

                Response.Write(ex.ToString)
            Finally
                con.Close()
                cmd.Dispose()

            End Try
        End If

    End Sub
End Class