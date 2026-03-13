Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services

Public Class Product
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.prodname)


        End If

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()


    End Sub

    Private Sub btn_clr_Click(sender As Object, e As EventArgs) Handles btn_clr.Click

        prodname.Text = ""
        shrtname.Text = ""
        acnprefix.Text = ""
        typ.Text = "Select"
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.prodname)

    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        Dim query As String = String.Empty

        cmd.Connection = con



        query = "INSERT INTO products(name,shortname,acnprefix,prdtype)"
        query &= "VALUES(@name,@shortname,@acnprefix,@prdtype)"


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@name", prodname.Text)
        cmd.Parameters.AddWithValue("@shortname", shrtname.Text)
        cmd.Parameters.AddWithValue("@acnprefix", acnprefix.Text)
        cmd.Parameters.AddWithValue("@prdtype", typ.Text)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)


        End Try

        prodname.Text = ""
        shrtname.Text = ""
        acnprefix.Text = ""
        typ.Text = "Select"
        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.prodname)

        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

        sb.Append("<div class=" + """alert alert-dismissable alert-primary """ + ">")
        sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
        sb.Append("<strong>Updated !</strong> Product Updated." + Session("tid"))
        sb.Append("</div>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())

    End Sub
End Class