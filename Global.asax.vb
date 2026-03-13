
Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
    End Sub

    Protected Sub Application_PreRequestHandlerExecute(sender As Object, e As EventArgs)
        If TypeOf Context.Handler Is IRequiresSessionState OrElse TypeOf Context.Handler Is IReadOnlySessionState Then

            If Session.IsNewSession OrElse Session.Count < 1 Then

                If Not Context.Request.Url.AbsoluteUri.Contains("Login.aspx") Then
                    Context.Response.Redirect("/Login.aspx")
                End If
            End If
        End If
    End Sub
    Sub Session_End(sender As Object, e As EventArgs)

        ' Server.Transfer("../Login.aspx")
        'If Context.Session IsNot Nothing Then
        '    If Session.IsNewSession Then
        '        Dim newSessionIdCookie As HttpCookie = Request.Cookies("ASP.NET_SessionId")
        '        If newSessionIdCookie IsNot Nothing Then
        '            Dim NewSessionIdCookieValue As String = newSessionIdCookie.Value
        '            If NewSessionIdCookieValue <> String.Empty Then
        '                Response.Redirect("../Login.aspx")
        '            End If
        '        End If
        '    End If
        'End If



    End Sub

    Protected Sub Page_Init(sender As Object, e As EventArgs)


    End Sub


    Protected Sub FormsAuthentication_OnAuthenticate(sender As Object, e As FormsAuthenticationEventArgs)
        'On Error Resume Next

        If FormsAuthentication.CookiesSupported = True Then


            If Not IsNothing(Request.Cookies(FormsAuthentication.FormsCookieName)) Then

                Dim username As String = FormsAuthentication.Decrypt(Request.Cookies(FormsAuthentication.FormsCookieName).Value).Name
                If username = Nothing Then
                    Response.Redirect("../login.aspx")
                End If
                Dim roles As String = session_user_role
                If Not roles = Nothing Then
                    e.User = New System.Security.Principal.GenericPrincipal(New System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(";"))
                End If



            End If

        End If



    End Sub

End Class