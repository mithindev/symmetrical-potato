Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services

Public Class GlPayment

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
        cmd.Parameters.AddWithValue("@drd", txtamt.Text)
        cmd.Parameters.AddWithValue("@crd", 0)
        cmd.Parameters.AddWithValue("@drc", txtamt.Text)
        cmd.Parameters.AddWithValue("@crc", 0)
        cmd.Parameters.AddWithValue("@narration", "To Cash")
        cmd.Parameters.AddWithValue("@due", Trim(txtnar.Text))
        cmd.Parameters.AddWithValue("@type", "CASH")
        cmd.Parameters.AddWithValue("@suplimentry", txtled.text)
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

        update_suplementry(Session("tid"), txtled.Text, 0, txtamt.Text, txtnar.Text, "PAYMENT")



        'set_diff(ovr)

        'Dim nar As String = "TO INTEREST UPTO" + CStr(tdate.Text)
        'update_int(tid, CDbl(lblactualint_d.Text), 0, CDbl(lblactualint.Text), 0, nar, product, -ovr_c, -ovr_d)



        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transacion ID : " + Session("tid")
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)



        clear_tab_recpt()

    End Sub


    Sub clear_tab_recpt()
        Session("tid") = Nothing
        txtnar.Text = ""
        txtled.Text = ""
        txtamt.Text = ""
        'TabContainer1.Visible = False
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        txtfocus(tdate)





    End Sub



    Private Sub tdate_TextChanged(sender As Object, e As EventArgs) Handles tdate.TextChanged
        'get_ac_info(txtacn.Text)
        Session("tdat") = CDate(tdate.Text)

        txtfocus(txtled)
    End Sub



    Private Sub GlPayment_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class