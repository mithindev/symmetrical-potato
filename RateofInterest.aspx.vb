Imports System.Data.SqlClient

Public Class RateofInterest
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

    End Sub

    Private Sub RateofInterest_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub ddprod_TextChanged(sender As Object, e As EventArgs) Handles ddprod.TextChanged
        Select Case ddprod.SelectedItem.Text
            Case "DS"
                upsch.Visible = False
                Session("prddmy") = "D"
            Case "FD"
                upsch.Visible = False
                Session("prddmy") = "M"
            Case "KMK"
                upsch.Visible = False
                Session("prddmy") = "M"
            Case "RD"
                upsch.Visible = False
                Session("prddmy") = "M"
            Case "RID"
                upsch.Visible = False
                Session("prddmy") = "M"
            Case "SB"
                upsch.Visible = False
                Session("prddmy") = "M"
            Case "DCL"
                upsch.Visible = True
                Session("prddmy") = "D"
            Case "DL"
                upsch.Visible = True
                Session("prddmy") = "D"
            Case "JL"
                upsch.Visible = True
                Session("prddmy") = "D"
            Case "ML"
                upsch.Visible = True
                Session("prddmy") = "D"
        End Select
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                Try
                    cmd.Connection = con

                    query = "insert into roi(product,fyfrm,fyto,prdfrm,prddmy,prdto,prdtdmy,cint,dint,minsc,penalcut,agst) "
                    query += " VALUES(@product,@fyfrm,@fyto,@prdfrm,@prddmy,@prdto,@prdtdmy,@cint,@dint,@minsc,@penalcut,@agst)"
                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@product", ddprod.SelectedValue)
                    cmd.Parameters.AddWithValue("@fyfrm", CDate(txtfrm.Text))
                    cmd.Parameters.AddWithValue("@fyto", CDate(txtto.Text))
                    cmd.Parameters.AddWithValue("@prdfrm", txtprdfrm.Text)
                    cmd.Parameters.AddWithValue("@prddmy", Session("prddmy"))
                    cmd.Parameters.AddWithValue("@prdto", txtprdto.Text)
                    cmd.Parameters.AddWithValue("@prdtdmy", Session("prddmy"))
                    cmd.Parameters.AddWithValue("@cint", txtcint.Text)
                    cmd.Parameters.AddWithValue("@dint", txtdint.Text)
                    cmd.Parameters.AddWithValue("@minsc", ddshare.SelectedValue)
                    cmd.Parameters.AddWithValue("@penalcut", txtpenel.Text)
                    If ddprod.SelectedItem.Text = "DL" Then
                        cmd.Parameters.AddWithValue("@agst", ddagst.SelectedItem.Value)
                    Else
                        cmd.Parameters.AddWithValue("@agst", ddsch.SelectedValue)
                    End If

                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cmd.Dispose()
                    con.Close()
                End Try
            End Using
        End Using

        clear()
    End Sub

    Private Sub btnclr_Click(sender As Object, e As EventArgs) Handles btnclr.Click
        clear()
    End Sub

    Sub clear()
        ddprod.SelectedIndex = 0
        txtfrm.Text = DateTime.Today
        txtto.Text = DateTime.Today
        txtprdfrm.Text = ""
        txtprdto.Text = ""
        txtcint.Text = ""
        txtdint.Text = ""
        ddshare.SelectedIndex = 0
        txtpenel.Text = ""
        ddagst.SelectedIndex = 0
        ddsch.SelectedIndex = 0
    End Sub

End Class