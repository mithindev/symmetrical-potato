Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Web.Security

Public Class customer
    Inherits System.Web.UI.Page



    Dim cid As Integer
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim oResult As String
    Dim brcode As String
    Dim isedit As Boolean
    Dim stitle As String
    Dim msg As String



    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString


        If Not Page.IsPostBack Then
            con.Open()
            newmember()
            'txtcid.Enabled = False


            txtFirstName.Focus()

        End If


    End Sub
    Sub newmember()
        cmd.Connection = con

        brcode = get_brcode()

        If session_user_role = "Audit" Then
            cmd.CommandText = "SELECT TOP 1 serial FROM member where member.memberno like '" + "KNL" + brcode + "%' ORDER BY serial DESC"
        Else
            cmd.CommandText = "SELECT TOP 1 serial FROM member where member.memberno like '" + "KBF" + brcode + "%' ORDER BY serial DESC"
        End If

        Try
            oResult = cmd.ExecuteScalar()

            If oResult IsNot Nothing Then
                cid = Int(oResult.ToString) + 1
            Else
                cid = 1
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        If session_user_role = "Audit" Then
            Session("ncid") = "KNL" + brcode + String.Format("{0:00000}", cid)
            Session("lcid") = "KNL" + brcode + String.Format("{0:00000}", cid - 1)
        Else
            Session("ncid") = "KBF" + brcode + String.Format("{0:00000}", cid)
            Session("lcid") = "KBF" + brcode + String.Format("{0:00000}", cid - 1)
        End If

        txtcid.Text = Session("ncid")

        get_lastmember(Session("lcid"))

    End Sub


    Sub get_lastmember(ByVal cid As String)

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = "SELECT firstname from member where memberno='" + cid + "' "

        Try
            brcode = cmd.ExecuteScalar


            If brcode Is Nothing Then
                brcode = ""
                alertmsg.Visible = False
            Else
                lblinfo.Text = "Recent Member : " + cid + "  -  " + brcode
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Function get_brcode()

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = "SELECT TOP 1 branchcode FROM branch"

        Try
            brcode = cmd.ExecuteScalar


            If brcode Is Nothing Then
                brcode = "01"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return brcode


    End Function

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click


        btnsave.Enabled = False

        brcode = get_brcode()


        cmd.Connection = con
        '  cmd.CommandText = "SELECT TOP 1 serial FROM member where member.memberno like '" + "KBF" + brcode + "%' ORDER BY serial DESC"
        If session_user_role = "Audit" Then
            cmd.CommandText = "SELECT TOP 1 serial FROM member where member.memberno like '" + "KNL" + brcode + "%' ORDER BY serial DESC"
        Else
            cmd.CommandText = "SELECT TOP 1 serial FROM member where member.memberno like '" + "KBF" + brcode + "%' ORDER BY serial DESC"
        End If



        Try
            oResult = cmd.ExecuteScalar()


            If oResult IsNot Nothing Then
                cid = Int(oResult.ToString) + 1
            Else
                cid = 1
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Dim query As String = String.Empty

        If Session("ncid") = Trim(txtcid.Text) Then

            query = "INSERT INTO member(Memberno,FirstName,Lastname,dob,gender,address,pincode,phone,mobile,email,serial)"
            query &= "VALUES(@Memberno,@FirstName,@lastname,@dob,@gender,@address,@pincode,@phone,@mobile,@email,@serial)"
            If session_user_role = "Audit" Then
                Session("ncid") = "KNL" + brcode + String.Format("{0:00000}", cid)
            Else
                Session("ncid") = "KBF" + brcode + String.Format("{0:00000}", cid)
            End If
            isedit = False

        Else
                query = "update member set FirstName=@Firstname,Lastname=@lastname,dob=@dob,gender=@gender,address=@address,pincode=@pincode,phone=@phone,mobile=@mobile,email=@email where memberno=@memberno"
            Session("ncid") = txtcid.Text
            isedit = True
        End If


        cmd.CommandText = query
        ' cmd.Parameters.AddWithValue("@Id", cid)
        cmd.Parameters.AddWithValue("@Memberno", Session("ncid"))
        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
        cmd.Parameters.AddWithValue("@lastname", txtLastName.Text)
        cmd.Parameters.AddWithValue("@dob", txtDOB.Text)
        cmd.Parameters.AddWithValue("@gender", SelectGender.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@address", txtAddress.Text)
        cmd.Parameters.AddWithValue("@pincode", txtPincode.Text)
        cmd.Parameters.AddWithValue("@phone", txtphone.Text)
        cmd.Parameters.AddWithValue("@mobile", txtMobile.Text)
        cmd.Parameters.AddWithValue("@email", txtEmail.Text)
        cmd.Parameters.AddWithValue("@serial", cid)

        Try
            cmd.ExecuteNonQuery()









            Session("cid") = Session("ncid")

            ' alertmsg.Visible = True
            Dim url As String = "KYC.aspx"
            If isedit = False Then
                stitle = "Member Created !"
            Else
                stitle = "Member Updated !"
            End If


            msg = "Member No:&nbsp;&nbsp;&nbsp;<b>" + Session("ncid") + "</b>&nbsp;&nbsp;&nbsp;<a href=\kyc.aspx\>&nbsp;&nbsp;Click here to Update the KYC</a>"


            Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"

            clear_form()

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)




        Catch ex As Exception

            Response.Write(ex.Message.ToString)


        End Try









    End Sub

    Private Sub btnsave_Command(sender As Object, e As CommandEventArgs) Handles btnsave.Command

    End Sub

    Private Sub btnsave_DataBinding(sender As Object, e As EventArgs) Handles btnsave.DataBinding

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        clear_form()




    End Sub
    Sub clear_form()
        txtcid.Text = ""
        txtcid.Enabled = True
        btnsave.Enabled = True
        txtAddress.Text = ""
        txtDOB.Text = ""
        SelectGender.SelectedIndex = 0
        txtEmail.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtMobile.Text = ""
        txtphone.Text = ""
        txtPincode.Text = ""
        txtEmail.Text = ""


        newmember()
        txtFirstName.Focus()

    End Sub




    Sub get_member(ByVal cid As String)


        '  alertmsg.Visible = False

        If con.State = ConnectionState.Closed Then con.Open()

        Dim qry As String = "Select firstname,lastname,dob,gender,address,pincode,phone,mobile,email from member where member.memberno='" + Trim(cid) + "'"
        cmd.Connection = con
        cmd.CommandText = qry
        Dim dr As SqlDataReader
        Try
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                dr.Read()

                txtFirstName.Text = Trim(IIf(IsDBNull(dr(0)), "", dr(0)))
                txtLastName.Text = Trim(IIf(IsDBNull(dr(1)), "", dr(1)))
                txtDOB.Text = Trim(IIf(IsDBNull(dr(2)), "", dr(2)))
                'SelectGender.SelectedItem.Text = Trim(IIf(IsDBNull(dr(3)), "", dr(3)))
                If Not IsDBNull(dr(3)) Then
                    If dr(3) = "Male" Then SelectGender.SelectedIndex = 1
                    If dr(3) = "FeMale" Then SelectGender.SelectedIndex = 2
                    If dr(3) = "Others" Then SelectGender.SelectedIndex = 3
                End If
                txtAddress.Text = Trim(IIf(IsDBNull(dr(4)), "", dr(4)))
                txtPincode.Text = Trim(IIf(IsDBNull(dr(5)), "", dr(5)))
                txtphone.Text = Trim(IIf(IsDBNull(dr(6)), "", dr(6)))
                txtMobile.Text = Trim(IIf(IsDBNull(dr(7)), "", dr(7)))
                txtEmail.Text = Trim(IIf(IsDBNull(dr(8)), "", dr(8)))


            End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try





    End Sub


    Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged

        get_member(txtcid.Text)


    End Sub

    Private Sub customer_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class