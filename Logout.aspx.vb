Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1))
            Response.Cache.SetNoStore()
            Log_out(Session("logtime").ToString, Session("sesusr"))
            FormsAuthentication.SignOut()
            Session.Abandon()

        Catch ex As Exception
            Response.Redirect("Login.aspx")
        End Try



    End Sub

End Class