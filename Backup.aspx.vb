Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Public Class Backup
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        con.Open()

        If Not Session("bdt") Is Nothing Then
            txtdt.Text = CType(Session("bdt"), Date)
        End If

    End Sub


    Protected Sub btn_bkup_Click(sender As Object, e As EventArgs) Handles btn_bkup.Click
        System.Threading.Thread.Sleep(3000)




        query = "BACKUP DATABASE fiscusdb TO DISK = @path WITH NAME = N'fiscusdb-Full Database backup', NOFORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, STATS = 1"

        Dim path As String = txtpath.Text + "\fiscusdb.bak"

        If con.State = ConnectionState.Closed Then con.Open()

        Try
            cmd.Connection = con
            cmd.CommandText = query
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@path", path)

            cmd.ExecuteNonQuery()


            up_chklst()


            'Response.Write("OK")
            txtdt.Text = ""
            txtpath.Text = ""
            ' Response.Redirect("/admin/dashboard.aspx")

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
        If session_user_role = "Admin" Then
            Response.Redirect("admin/dashboard.aspx", False)
        Else

            Response.Redirect("User/UserDashboard.aspx", False)
        End If

        HttpContext.Current.ApplicationInstance.CompleteRequest()

    End Sub

    Sub up_chklst()

        If con.State = ConnectionState.Closed Then con.Open()

        Dim dat As DateTime = DateTime.ParseExact(txtdt.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)



        query = "select date from chklst WHERE convert(varchar(20), date, 112) ='" + reformatted + "'"
        cmd.Connection = con
        cmd.CommandText = query

        Dim dr As SqlDataReader

        Try
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                dr.Close()
                cmd.Dispose()

                query = "update chklst set bakup=1 WHERE convert(varchar(20), date, 112) ='" + reformatted + "'"

                cmd.Connection = con
                cmd.CommandText = query

                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    MsgBox(ex.ToString)
                Finally
                    con.Close()
                    cmd.Dispose()


                End Try

            Else

                dr.Close()
                cmd.Dispose()


                query = "insert into chklst(date,bakup)"
                query &= "values (@date,@bakup)"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdt.Text))
                cmd.Parameters.AddWithValue("@bakup", 1)
                cmd.Connection = con
                cmd.CommandText = query

                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    MsgBox(ex.ToString)
                Finally
                    con.Close()
                    cmd.Dispose()


                End Try

            End If

        Catch ex As Exception

            Response.Write(ex.ToString)


        End Try

    End Sub



End Class