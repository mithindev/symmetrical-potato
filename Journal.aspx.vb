Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization


Public Class Journal
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public dt As New DataTable
    Dim newrow As DataRow
    Public tqty As Integer
    Public dcl_chrg As Integer
    Public dl_chrg As Integer
    Public jl_chrg As Integer
    Public ml_chrg As Integer
    Public home As String


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then
            '
            'bind_grid()

            tdate.Text = Date.Today
            txtfocus(tdate)
            Session("tdat") = CDate(tdate.Text)




            ''    ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", tdate.ClientID), True)
        End If

    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click

        set_changes()


    End Sub

    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)
        If tid = 0 Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        'cmd.CreateParameter()
        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@date,@tid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@tid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        If dr = 0 Then
            cmd.Parameters.AddWithValue("@nar", nar)
        Else
            cmd.Parameters.AddWithValue("@nar", nar)
        End If

        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()


        End Try

        query = ""

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        'cmd.CreateParameter()
        query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@date,@tid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@tid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        If dr = 0 Then
            cmd.Parameters.AddWithValue("@nar", nar)
        Else
            cmd.Parameters.AddWithValue("@nar", nar)
        End If

        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()


        End Try

        query = ""



    End Sub
    Private Sub set_changes()
        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            Log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("..\login.aspx")

        End If
        'Dim ovr_d As Double
        'Dim ovr_c As Double
        Dim countresult As Integer
        Dim query As String = String.Empty

        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con




        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        '  Dim prod = get_pro(product)

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", "")
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", CDbl(txtamt.Text))
        cmd.Parameters.AddWithValue("@narration", txtnar.Text)
        cmd.Parameters.AddWithValue("@due", "")
        cmd.Parameters.AddWithValue("@type", "JOURNAL")
        cmd.Parameters.AddWithValue("@suplimentry", txtled.Text)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)



        Finally

            con.Close()
            cmd.Dispose()


        End Try

        query = ""

        Dim x As String = txtled.Text



        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con

        query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + x + "'"
        'cmd.Parameters.AddWithValue("@led", x)
        cmd.CommandText = query


        Try

            countresult = cmd.ExecuteScalar()

            Session("tid") = Convert.ToString(countresult)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try



        '  If drac.SelectedItem.Text = "GL" Then
        update_suplementry(Session("tid"), txtled_d.Text, 0, CDbl(txtamt.Text), txtnar.Text, "JOURNAL")
        ' Else

        ' trim_disp()


        ' End If


        update_suplementry(Session("tid"), txtled.Text, CDbl(txtamt.Text), 0, txtnar.Text, "JOURNAL")


        'set_diff(ovr)

        lblglhcr.Text = txtled.Text
        'If drac.SelectedItem.Text = "GL" Then
        lblglhdr.Text = txtled_d.Text
        'Else
        'lblglhdr.Text = Session("pro")
        'End If

        lbldr.Text = FormatNumber(txtamt.Text)

        lblcr.Text = FormatNumber(txtamt.Text)
        lbltcr.Text = FormatCurrency(txtamt.Text)
        lbltdr.Text = FormatCurrency(txtamt.Text)
        pnar.Text = txtnar.Text
        paiw.Text = get_wrds(txtamt.Text)
        pvno.Text = Session("tid")
        pdate.Text = tdate.Text
        pbranch.Text = get_home()






        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transacion ID : " + Session("tid")
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)



        '  pnlvch.Visible = False
        '  pnlprnt.Visible = True


        pnlvch.Style.Add("display", "none")
        pnlprnt.Style.Add("display", "block")


        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)

        'Catch ex As Exception

        '    MsgBox(ex.Message)
        'End Try



    End Sub


    Function get_home()

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = "Select TOP 1 branch FROM branch"

        Try
            home = cmd.ExecuteScalar


            If home Is Nothing Then
                home = "KARAVILAI"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return home


    End Function


    Sub clear_tab_recpt()

        txtamt.Text = ""
        txtnar.Text = ""
        'gldr.Visible = True

        txtled.Text = ""
        txtled_d.Text = ""
        Session("tid") = Nothing




        'TabContainer1.Visible = False
        ' 





    End Sub



    Private Sub tdate_TextChanged(sender As Object, e As EventArgs) Handles tdate.TextChanged
        'get_ac_info(txtacn.Text)
        Session("tdat") = CDate(tdate.Text)

        'txtfocus(glh)
    End Sub

    Private Function TabPanel1() As Object
        Throw New NotImplementedException
    End Function
    'Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
    '    dispgv.PageIndex = e.NewPageIndex
    '    dispgv.DataBind()
    'End Sub
    'Sub bind_ac()





    '    If Not txtled.Text = "TDS RECEIVABLE" Then

    '        Dim ds_js As DataSet = Nothing

    '        If con.State = ConnectionState.Closed Then con.Open()

    '        query = "SELECT nature,chrgs FROM dbo.accharges ORDER BY nature"
    '        cmd.Connection = con
    '        cmd.CommandText = query
    '        Try

    '            Dim reader As SqlDataReader = cmd.ExecuteReader()




    '            acchrg.DataSource = reader

    '            acchrg.DataTextField = "nature"
    '            acchrg.DataValueField = "chrgs"

    '            acchrg.DataBind()

    '            acchrg.Items.Insert(0, "<-- Select -->")
    '            acchrg.Items.Item(0).Value = ""



    '        Catch ex As Exception
    '            MsgBox(ex.ToString())

    '        Finally
    '            cmd.Dispose()
    '            con.Close()



    '        End Try

    '    Else
    '        acchrg.Items.Insert(0, "<-- Select -->")
    '        acchrg.Items.Item(0).Value = ""
    '        acchrg.Items.Insert(0, "TDS RECEIVABLE")
    '        acchrg.Items.Item(0).Value = ""



    '    End If

    'End Sub
    'Private Sub drac_TextChanged(sender As Object, e As EventArgs) Handles drac.TextChanged
    '    If Not drac.SelectedIndex = 0 Then
    '        If drac.SelectedItem.Text = "GL" Then
    '            'bind_grid_dr()

    '            gldr.Visible = True
    '            txtfocus(txtled_d)

    '        ElseIf drac.SelectedItem.Text = "Account" Then
    '                bind_ac()

    '            gldr_ac.Visible = True
    '            txtfocus(txtcid)

    '        End If
    '    End If

    'End Sub


    'Function get_tds_rec(ByVal cid As String)
    '    Dim bal As Decimal = 0
    '    query = "SELECT memberno,isnull(SUM(tdsbrkup.debit),0)-isnull(SUM(tdsbrkup.credit),0)  AS balance FROM dbo.tdsbrkup WHERE dbo.tdsbrkup.name=@cid GROUP BY memberno "

    '    If con.State = ConnectionState.Closed Then con.Open()
    '    cmd.Connection = con
    '    cmd.CommandText = query
    '    cmd.Parameters.Clear()
    '    cmd.Parameters.AddWithValue("@cid", txtdr_nar.Text)
    '    Try
    '        Dim dr As SqlDataReader
    '        dr = cmd.ExecuteReader()

    '        If dr.HasRows() Then
    '            dr.Read()
    '            Session("tds_cid") = dr(0)
    '            bal = dr(1)
    '        End If

    '    Catch ex As Exception
    '        Response.Write(ex.ToString)
    '    End Try

    '    Return bal

    'End Function


    'Private Sub acchrg_SelectedIndexChanged(sender As Object, e As EventArgs) Handles acchrg.SelectedIndexChanged

    '    If txtled.Text = "TDS RECEIVABLE" Then
    '        txtdr.Text = get_tds_rec(txtcid.Text)

    '    Else

    '        txtdr.Text = acchrg.SelectedValue.ToString

    '    End If

    '    txtfocus(txtdr_nar)
    'End Sub

    'Private Sub j_add_Click(sender As Object, e As EventArgs) Handles j_add.Click


    '    Dim dt_j As New DataTable

    '    If dt_j.Columns.Count = 0 Then
    '        dt_j.Columns.Add("acn", GetType(String))
    '        dt_j.Columns.Add("nature", GetType(String))
    '        dt_j.Columns.Add("nar", GetType(String))
    '        dt_j.Columns.Add("amt", GetType(Decimal))
    '        dt_j.Columns.Add("cid", GetType(String))

    '    End If

    '    If dt.Columns.Count = 0 Then
    '        dt.Columns.Add("acn", GetType(String))
    '        dt.Columns.Add("nature", GetType(String))
    '        dt.Columns.Add("nar", GetType(String))
    '        dt.Columns.Add("amt", GetType(Decimal))
    '        dt.Columns.Add("cid", GetType(String))
    '    End If



    '    If Session("jspec") Is Nothing = False Then
    '        dt_j = CType(Session("jspec"), DataTable)

    '        Dim count As Integer = dt_j.Rows.Count

    '        For Each row1 As DataRow In dt_j.Rows

    '            dt.ImportRow(row1)
    '        Next

    '    End If
    '    newrow = dt_j.NewRow

    '    newrow(0) = txtcid.Text
    '    newrow(1) = acchrg.SelectedItem.Text
    '    newrow(2) = If(txtdr_nar.Text = "", " ", txtdr_nar.Text)
    '    newrow(3) = txtdr.Text



    '    Dim x As String = Left(txtcid.Text, 2)

    '    Select Case x
    '        Case "61"
    '            If Session("dcl") Is Nothing Then
    '                dcl_chrg = CInt(txtdr.Text)
    '                Session("dcl") = dcl_chrg

    '            Else
    '                dcl_chrg = CType(Session("dcl"), Integer) + CInt(txtdr.Text)
    '                Session("dcl") = dcl_chrg

    '            End If
    '        Case "62"
    '            If Session("dl") Is Nothing Then
    '                dl_chrg = CInt(txtdr.Text)
    '                Session("dl") = dl_chrg

    '            Else
    '                dl_chrg = CType(Session("dl"), Integer) + CInt(txtdr.Text)
    '                Session("dl") = dl_chrg

    '            End If

    '        Case "63"
    '            If Session("jl") Is Nothing Then
    '                jl_chrg = CInt(txtdr.Text)
    '                Session("jl") = jl_chrg

    '            Else
    '                jl_chrg = CType(Session("jl"), Integer) + CInt(txtdr.Text)
    '                Session("jl") = jl_chrg

    '            End If

    '        Case "64"
    '            If Session("ml") Is Nothing Then
    '                ml_chrg = CInt(txtdr.Text)
    '                Session("ml") = ml_chrg

    '            Else

    '                ml_chrg = CType(Session("ml"), Integer) + CInt(txtdr.Text)
    '                Session("ml") = ml_chrg

    '            End If


    '    End Select


    '    If Session("tqt") Is Nothing Then
    '        tqty = CInt(txtdr.Text)
    '    Else
    '        tqty = CType(Session("tqt"), Integer) + CInt(txtdr.Text)
    '    End If


    '    dt_j.Rows.Add(newrow)

    '    dt.ImportRow(newrow)

    '    dispgv.DataSource = dt
    '    dispgv.DataBind()
    '    Session("tqt") = tqty
    '    Session("jspec") = dt_j

    '    txtamt.Text = tqty

    '    txtcid.Text = ""
    '    txtdr_nar.Text = ""
    '    txtdr.Text = ""
    '    acchrg.SelectedIndex = 0
    '    txtfocus(txtcid)
    'End Sub

    'Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged

    '    get_ac_info()

    'End Sub

    'Sub get_ac_info()


    '    Dim dsd As New DataSet


    '    Dim sql As String = "SELECT Master.date,Master.acno,Master.product,Master.amount,Master.cint,Master.dint,Master.prd,Master.prdtype,Master.mdate,Master.mamt,Master.cid FROM dbo.master WHERE master.acno = '" + txtcid.Text + "' and cld='0'"

    '    Dim adapter As New SqlDataAdapter(sql, con)

    '    Try

    '        adapter.Fill(dsd)

    '        If Not dsd.Tables(0).Rows.Count = 0 Then
    '            txtfocus(acchrg)
    '        Else
    '            txtcid.Text = ""
    '            txtfocus(txtcid)
    '        End If


    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    Finally
    '        con.Close()

    '    End Try


    'End Sub

    'Sub trim_disp()


    '    If txtled.Text = "TDS RECEIVABLE" Then

    '        For i As Integer = 0 To dispgv.Rows.Count - 1


    '            Dim lblacn As Label = DirectCast(dispgv.Rows(i).Cells(0).FindControl("lblacn"), Label)
    '            Dim lblnature As Label = DirectCast(dispgv.Rows(i).Cells(0).FindControl("lblnature"), Label)
    '            Dim lblnar As Label = DirectCast(dispgv.Rows(i).Cells(0).FindControl("lblnar"), Label)
    '            Dim lblchrg As Label = DirectCast(dispgv.Rows(i).Cells(0).FindControl("lblchrg"), Label)

    '            If con.State = ConnectionState.Closed Then con.Open()
    '            cmd.Parameters.Clear()

    '            cmd.Connection = con
    '            query = "insert into actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,sesusr,entryat,suplimentry)"
    '            query &= "values(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sesusr,@entryat,@suplimentry)"

    '            cmd.CommandText = query


    '            cmd.Parameters.AddWithValue("@id", Session("tid"))
    '            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
    '            cmd.Parameters.AddWithValue("@acno", lblacn.Text)
    '            cmd.Parameters.AddWithValue("@drd", CInt(lblchrg.Text))
    '            cmd.Parameters.AddWithValue("@crd", 0)
    '            cmd.Parameters.AddWithValue("@drc", CInt(lblchrg.Text))
    '            cmd.Parameters.AddWithValue("@crc", 0)
    '            If Right(Trim(txtled.Text), 6) = "POSTAL" Then
    '                cmd.Parameters.AddWithValue("@narration", "To CASH")
    '                cmd.Parameters.AddWithValue("@due", lblnature.Text + " " + lblnar.Text)
    '                cmd.Parameters.AddWithValue("suplimentry", "SAVINGS DEPOSIT")
    '            Else
    '                cmd.Parameters.AddWithValue("@narration", "To " + lblnature.Text)
    '                cmd.Parameters.AddWithValue("@due", lblnar.Text)
    '                cmd.Parameters.AddWithValue("suplimentry", "SAVINGS DEPOSIT")

    '            End If
    '            cmd.Parameters.AddWithValue("@type", "TRF")
    '            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
    '            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))

    '            Try
    '                cmd.ExecuteNonQuery()

    '            Catch ex As Exception
    '                MsgBox(ex.ToString)
    '            Finally
    '                cmd.Parameters.Clear()
    '                con.Close()
    '                cmd.Dispose()

    '            End Try

    '            If con.State = ConnectionState.Closed Then con.Open()
    '            cmd.Parameters.Clear()

    '            cmd.Connection = con
    '            query = "insert into actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,sesusr,entryat,suplimentry)"
    '            query &= "values(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sesusr,@entryat,@suplimentry)"

    '            cmd.CommandText = query


    '            cmd.Parameters.AddWithValue("@id", Session("tid"))
    '            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
    '            cmd.Parameters.AddWithValue("@acno", lblacn.Text)
    '            cmd.Parameters.AddWithValue("@drd", CInt(lblchrg.Text))
    '            cmd.Parameters.AddWithValue("@crd", 0)
    '            cmd.Parameters.AddWithValue("@drc", CInt(lblchrg.Text))
    '            cmd.Parameters.AddWithValue("@crc", 0)
    '            If Right(Trim(txtled.Text), 6) = "POSTAL" Then
    '                cmd.Parameters.AddWithValue("@narration", "To CASH")
    '                cmd.Parameters.AddWithValue("@due", lblnature.Text + " " + lblnar.Text)
    '                cmd.Parameters.AddWithValue("suplimentry", "SAVINGS DEPOSIT")
    '            Else
    '                cmd.Parameters.AddWithValue("@narration", "To " + lblnature.Text)
    '                cmd.Parameters.AddWithValue("@due", lblnar.Text)
    '                cmd.Parameters.AddWithValue("suplimentry", "SAVINGS DEPOSIT")

    '            End If
    '            cmd.Parameters.AddWithValue("@type", "TRF")
    '            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
    '            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))

    '            Try
    '                cmd.ExecuteNonQuery()

    '            Catch ex As Exception
    '                MsgBox(ex.ToString)
    '            Finally
    '                cmd.Parameters.Clear()
    '                con.Close()
    '                cmd.Dispose()

    '            End Try






    '        Next

    '        update_suplementry(Session("tid"), "SAVINGS DEPOSIT", 0, txtamt.Text, "To Transfer", "JOURNAL")


    '    Else


    '        Dim pro As String = ""
    '        For i As Integer = 0 To dispgv.Rows.Count - 1


    '            Dim lblacn As Label = DirectCast(dispgv.Rows(i).Cells(0).FindControl("lblacn"), Label)
    '            Dim lblnature As Label = DirectCast(dispgv.Rows(i).Cells(0).FindControl("lblnature"), Label)
    '            Dim lblnar As Label = DirectCast(dispgv.Rows(i).Cells(0).FindControl("lblnar"), Label)
    '            Dim lblchrg As Label = DirectCast(dispgv.Rows(i).Cells(0).FindControl("lblchrg"), Label)

    '            If con.State = ConnectionState.Closed Then con.Open()
    '            cmd.Parameters.Clear()

    '            cmd.Connection = con
    '            query = "insert into actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,sesusr,entryat,suplimentry)"
    '            query &= "values(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sesusr,@entryat,@suplimentry)"

    '            cmd.CommandText = query
    '            Select Case Mid(lblacn.Text, 1, 2)
    '                Case 61
    '                    pro = "DAILY COLLECTION LOAN"
    '                Case 62
    '                    pro = "DEPOSIT LOAN"
    '                Case 63
    '                    pro = "JEWEL LOAN"
    '                Case 64
    '                    pro = "MORTGAGE LOAN"

    '            End Select

    '            cmd.Parameters.AddWithValue("@id", Session("tid"))
    '            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
    '            cmd.Parameters.AddWithValue("@acno", lblacn.Text)
    '            cmd.Parameters.AddWithValue("@drd", CInt(lblchrg.Text))
    '            cmd.Parameters.AddWithValue("@crd", 0)
    '            cmd.Parameters.AddWithValue("@drc", CInt(lblchrg.Text))
    '            cmd.Parameters.AddWithValue("@crc", 0)
    '            If Right(Trim(txtled.Text), 6) = "POSTAL" Then
    '                cmd.Parameters.AddWithValue("@narration", "To Postage")
    '                cmd.Parameters.AddWithValue("@due", lblnature.Text + " " + lblnar.Text)
    '                cmd.Parameters.AddWithValue("suplimentry", pro)
    '            Else
    '                cmd.Parameters.AddWithValue("@narration", "To " + lblnature.Text)
    '                cmd.Parameters.AddWithValue("@due", lblnar.Text)
    '                cmd.Parameters.AddWithValue("suplimentry", pro)

    '            End If
    '            cmd.Parameters.AddWithValue("@type", "TRF")
    '            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
    '            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))

    '            Try
    '                cmd.ExecuteNonQuery()

    '            Catch ex As Exception
    '                MsgBox(ex.ToString)
    '            Finally
    '                cmd.Parameters.Clear()
    '                con.Close()
    '                cmd.Dispose()

    '            End Try

    '            If con.State = ConnectionState.Closed Then con.Open()
    '            cmd.Parameters.Clear()

    '            cmd.Connection = con
    '            query = "insert into actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,sesusr,entryat,suplimentry)"
    '            query &= "values(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sesusr,@entryat,@suplimentry)"

    '            cmd.CommandText = query
    '            Select Case Mid(lblacn.Text, 1, 2)
    '                Case 61
    '                    pro = "DAILY COLLECTION LOAN"

    '                Case 62
    '                    pro = "DEPOSIT LOAN"
    '                Case 63
    '                    pro = "JEWEL LOAN"
    '                Case 64
    '                    pro = "MORTGAGE LOAN"

    '            End Select
    '            Session("pro") = pro
    '            cmd.Parameters.AddWithValue("@id", Session("tid"))
    '            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
    '            cmd.Parameters.AddWithValue("@acno", lblacn.Text)
    '            cmd.Parameters.AddWithValue("@drd", CInt(lblchrg.Text))
    '            cmd.Parameters.AddWithValue("@crd", 0)
    '            cmd.Parameters.AddWithValue("@drc", CInt(lblchrg.Text))
    '            cmd.Parameters.AddWithValue("@crc", 0)
    '            If Right(Trim(txtled.Text), 6) = "POSTAL" Then
    '                cmd.Parameters.AddWithValue("@narration", "To Postage")
    '                cmd.Parameters.AddWithValue("@due", lblnature.Text + " " + lblnar.Text)
    '                cmd.Parameters.AddWithValue("suplimentry", pro)
    '            Else
    '                cmd.Parameters.AddWithValue("@narration", "To " + lblnature.Text)
    '                cmd.Parameters.AddWithValue("@due", lblnar.Text)
    '                cmd.Parameters.AddWithValue("suplimentry", pro)

    '            End If
    '            cmd.Parameters.AddWithValue("@type", "TRF")
    '            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
    '            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))

    '            Try
    '                cmd.ExecuteNonQuery()

    '            Catch ex As Exception
    '                MsgBox(ex.ToString)
    '            Finally
    '                cmd.Parameters.Clear()
    '                con.Close()
    '                cmd.Dispose()

    '            End Try


    '        Next
    '        ' disp.PageIndex = disp.PageCount

    '        dcl_chrg = CType(Session("dcl"), Integer)
    '        dl_chrg = CType(Session("dl"), Integer)
    '        jl_chrg = CType(Session("jl"), Integer)
    '        ml_chrg = CType(Session("ml"), Integer)

    '        If Not dcl_chrg = 0 Then
    '            update_suplementry(Session("tid"), "DAILY COLLECTION LOAN", 0, dcl_chrg, "To Transfer", "JOURNAL")
    '        End If

    '        If Not dl_chrg = 0 Then
    '            update_suplementry(Session("tid"), "DEPOSIT LOAN", 0, dl_chrg, "To Transfer", "JOURNAL")

    '        End If

    '        If Not jl_chrg = 0 Then
    '            update_suplementry(Session("tid"), "JEWEL LOAN", 0, jl_chrg, "To Transfer", "JOURNAL")

    '        End If

    '        If Not ml_chrg = 0 Then
    '            update_suplementry(Session("tid"), "MORTGAGE LOAN", 0, ml_chrg, "To Transfer", "JOURNAL")

    '        End If
    '    End If

    '    '  pgtot(disp.PageIndex) = total
    'End Sub



    Private Sub btn_clr_Click(sender As Object, e As EventArgs) Handles btn_clr.Click
        clear_tab_recpt()

    End Sub


    Private Sub Journal_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
    If session_user_role = "Admin" Then
        Me.MasterPageFile = "~/admin/Admin.Master"

    Else
        Me.MasterPageFile = "~/User/User.Master"
    End If

End Sub

    Private Sub btntog_Click(sender As Object, e As EventArgs) Handles btntog.Click
        pnlvch.Style.Add("display", "block")
        pnlprnt.Style.Add("display", "none")
        clear_tab_recpt()
        txtfocus(tdate)

    End Sub
End Class