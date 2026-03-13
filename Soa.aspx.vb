Public Class Soa
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Soa_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub txtacn_TextChanged(sender As Object, e As EventArgs) Handles txtacn.TextChanged

        If Not Trim(txtacn.Text) = "" Then

            Dim x As String = Mid(Trim(txtacn.Text), 1, 1)

            If x = "6" Then
                Response.Redirect("soaloan.aspx?acno=" + Trim(txtacn.Text))
            Else
                Response.Redirect("soadeposit.aspx?acno=" + Trim(txtacn.Text))
            End If


        End If
    End Sub

    Private Sub soa_Click(sender As Object, e As EventArgs) Handles soa.Click
        If Not Trim(txtacn.Text) = "" Then

            Dim x As String = Mid(Trim(txtacn.Text), 1, 1)

            If x = "6" Then
                Response.Redirect("soaloan.aspx?acno=" + Trim(txtacn.Text))
            Else
                Response.Redirect("soadeposit.aspx?acno=" + Trim(txtacn.Text))
            End If


        End If
    End Sub
End Class