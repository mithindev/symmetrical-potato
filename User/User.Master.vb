Public Class User
    Inherits System.Web.UI.MasterPage



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("session_user_role") = "Audit" Then

            mnuFin.Style.Add("display", "block")
        Else
            mnuFin.Style.Add("display", "none")
        End If
    End Sub


End Class