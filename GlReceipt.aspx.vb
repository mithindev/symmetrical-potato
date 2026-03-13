Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services

Public Class GlReceipt
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then


            tdate.Text = Date.Today
            txtfocus(tdate)
            Session("tdat") = CDate(tdate.Text)



            ''    ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", tdate.ClientID), True)
        End If

    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click

        btn_update.Enabled = False

        set_changes()


    End Sub

    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)
        If tid = 0 Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
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


            MsgBox(ex.Message)

        Finally
            cmd.Dispose()
            con.Close()


        End Try

        query = ""


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
        cmd.Parameters.AddWithValue("@acn", "")
        cmd.Parameters.AddWithValue("@nar", nar)
        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
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
        cmd.Parameters.AddWithValue("@crd", txtamt.Text)
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", txtamt.Text)
        cmd.Parameters.AddWithValue("@narration", "By Cash")
        cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text))
        cmd.Parameters.AddWithValue("@type", "CASH")
        cmd.Parameters.AddWithValue("@suplimentry", txtled.Text)
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



        query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + txtled.Text + "'"
        cmd.CommandText = query

        countresult = cmd.ExecuteScalar()

        Session("tid") = Convert.ToString(countresult)

        update_suplementry(Session("tid"), txtled.Text, txtamt.Text, 0, txtnar.Text, "RECEIPT")



        'set_diff(ovr)

        'Dim nar As String = "TO INTEREST UPTO" + CStr(tdate.Text)
        'update_int(tid, CDbl(lblactualint_d.Text), 0, CDbl(lblactualint.Text), 0, nar, product, -ovr_c, -ovr_d)




        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transacion ID : " + Session("tid")
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
        clear_tab_recpt()

    End Sub

    ''Private Sub update_int(ByVal tid As Double, ByVal drd As Double, ByVal crd As Double, ByVal drc As Double, ByVal crc As Double, ByVal nar As String, ByVal supliment As String, ByVal cbal As Double, ByVal dbal As Double)


    ''    If con.State = ConnectionState.Closed Then con.Open()


    ''    cmdi.Connection = con


    ''    'Dim d As String = get_due(txtacn.Text, 0)

    ''    query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
    ''    query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"

    ''    cmdi.Parameters.Clear()

    ''    cmdi.CommandText = query
    ''    cmdi.Parameters.AddWithValue("@uiid", tid)
    ''    cmdi.Parameters.AddWithValue("@uidate", tdat)
    ''    cmdi.Parameters.AddWithValue("@uiacno", txtacn.Text)
    ''    cmdi.Parameters.AddWithValue("@uidrd", drd)
    ''    cmdi.Parameters.AddWithValue("@uicrd", crd)
    ''    cmdi.Parameters.AddWithValue("@uidrc", drc)
    ''    cmdi.Parameters.AddWithValue("@uicrc", crc)
    ''    cmdi.Parameters.AddWithValue("@uinarration", nar)
    ''    cmdi.Parameters.AddWithValue("@uidue", " ")
    ''    cmdi.Parameters.AddWithValue("@uitype", "INTR")
    ''    cmdi.Parameters.AddWithValue("@uisuplimentry", supliment)
    ''    cmdi.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
    ''    cmdi.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
    ''    cmdi.Parameters.AddWithValue("@uicbal", cbal)
    ''    cmdi.Parameters.AddWithValue("@uidbal", dbal)


    ''    Try
    ''        cmdi.ExecuteNonQuery()
    ''    Catch ex As Exception


    ''        'MsgBox(ex.Message)

    ''    Finally

    ''        cmdi.Dispose()
    ''        con.Close()
    ''    End Try


    ''End Sub

    ''Sub set_diff(ByVal ovr As Double)
    ''    If con.State = ConnectionState.Closed Then con.Open()

    ''    cmd.Connection = con

    ''    cmd.Parameters.Clear()
    ''    query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
    ''    query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
    ''    cmd.CommandText = query
    ''    cmd.Parameters.AddWithValue("@tid", tid)
    ''    cmd.Parameters.AddWithValue("@date", tdat)
    ''    cmd.Parameters.AddWithValue("@product", product)
    ''    cmd.Parameters.AddWithValue("@acno", txtacn.Text)
    ''    cmd.Parameters.AddWithValue("dr", IIf(ovr < 0, -ovr, 0))
    ''    cmd.Parameters.AddWithValue("cr", IIf(ovr > 0, ovr, 0))

    ''    Try

    ''        cmd.ExecuteNonQuery()

    ''    Catch ex As Exception
    ''        MsgBox(ex.ToString)
    ''    Finally

    ''        cmd.Dispose()
    ''        con.Close()


    ''    End Try
    ''End Sub

    Sub clear_tab_recpt()

        Session("tid") = Nothing
        txtnar.Text = ""
        txtled.Text = ""
        txtamt.Text = ""
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        txtfocus(tdate)





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



    Private Sub GlReceipt_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class