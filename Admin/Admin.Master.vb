Public Class Admin
    Inherits System.Web.UI.MasterPage


    'Protected Sub signout(sender As Object, e As EventArgs)

    '    FormsAuthentication.SignOut()

    '    Session.Abandon()
    '    Log_out(Session("logtime").ToString, Session("sesusr"))
    '    Response.Redirect("\login.aspx")

    'End Sub

    Sub signout(sender As Object, e As EventArgs)


    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Dim so As New HtmlAnchor

        ' AddHandler so.ServerClick, AddressOf signout

        'Me.Page.ClientScript.GetPostBackEventReference(so, "")

        If System.Web.HttpContext.Current.User.Identity.IsAuthenticated Then
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1))
            Response.Cache.SetNoStore()

        Else
            Response.Redirect("Login.aspx", False)

        End If




    End Sub


End Class