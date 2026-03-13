Imports System.Data.SqlClient

Public Class DepositList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then

            get_acc_details()
            ViewState("RefUrl") = Request.UrlReferrer.ToString()


        End If

    End Sub

    Sub get_acc_details()


        'Select Case Session("prod")
        '    Case "DS"
        '        btnDS.CssClass = "text-primary"
        '        btnFD.CssClass = "text-dark"
        '        btnKMK.CssClass = "text-dark"
        '        btnRD.CssClass = "text-dark"
        '        btnRID.CssClass = "text-dark"
        '        btnSB.CssClass = "text-dark"

        '    Case "FD"
        '        btnDS.CssClass = "text-dark"
        '        btnFD.CssClass = "text-primary"
        '        btnKMK.CssClass = "text-dark"
        '        btnRD.CssClass = "text-dark"
        '        btnRID.CssClass = "text-dark"
        '        btnSB.CssClass = "text-dark"

        '    Case "KMK"
        '        btnDS.CssClass = "text-dark"
        '        btnFD.CssClass = "text-dark"
        '        btnKMK.CssClass = "text-primary"
        '        btnRD.CssClass = "text-dark"
        '        btnRID.CssClass = "text-dark"
        '        btnSB.CssClass = "text-dark"

        '    Case "RD"
        '        btnDS.CssClass = "text-dark"
        '        btnFD.CssClass = "text-dark"
        '        btnKMK.CssClass = "text-dark"
        '        btnRD.CssClass = "text-primary"
        '        btnRID.CssClass = "text-dark"
        '        btnSB.CssClass = "text-dark"

        '    Case "RID"
        '        btnDS.CssClass = "text-dark"
        '        btnFD.CssClass = "text-dark"
        '        btnKMK.CssClass = "text-dark"
        '        btnRD.CssClass = "text-dark"
        '        btnRID.CssClass = "text-primary"
        '        btnSB.CssClass = "text-dark"

        '    Case "SB"
        '        btnDS.CssClass =
        '        btnFD.CssClass = "text-dark"
        '        btnKMK.CssClass = "text-dark"
        '        btnRD.CssClass = "text-dark"
        '        btnRID.CssClass = "text-dark"
        '        btnSB.CssClass = "text-primary"


        'End Select


        lblcid.Text = Session("cid")
        ' = Trim(txtcid.Text)
        lblfname.Text = Session("firstname")
        lbllname.Text = Session("lastname")
        lbldep.Text = Session("dep")

        Dim cld As Integer = 0


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "Prolist"
                cmd.Parameters.Clear()
                cmd.Parameters.Add(New SqlParameter("@cid", Session("cid")))
                cmd.Parameters.Add(New SqlParameter("@prod", Session("prod")))
                cmd.Parameters.Add(New SqlParameter("@cld", cld))

                Try

                    Using Sda As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable
                        Sda.Fill(dt)
                        lblapC.Text = dt.Rows.Count

                        rpAPlan.DataSource = dt
                        rpAPlan.DataBind()
                        get_locked(dt)


                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try


            End Using
            con.Close()

        End Using

        cld = 1

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "Prolist"
                cmd.Parameters.Clear()
                cmd.Parameters.Add(New SqlParameter("@cid", Session("cid")))
                cmd.Parameters.Add(New SqlParameter("@prod", Session("prod")))
                cmd.Parameters.Add(New SqlParameter("@cld", cld))

                Try

                    Using Sda As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable
                        Sda.Fill(dt)
                        lblEpc.Text = dt.Rows.Count

                        rpEPlan.DataSource = dt
                        rpEPlan.DataBind()


                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try


            End Using
            con.Close()

        End Using

        Dim cm As Double = 0


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                cmd.CommandText = "SELECT COALESCE(SUM(master.amount),0) AS bal FROM dbo.master WHERE master.product = @prod AND master.cid = @mem  and MONTH(master.date) = @mn AND YEAR(master.date) = @yr"
                cmd.Parameters.Clear()

                cmd.Parameters.AddWithValue("@mem", Session("cid"))
                cmd.Parameters.AddWithValue("@prod", Session("prod"))
                cmd.Parameters.AddWithValue("@mn", Date.Now.Month)
                cmd.Parameters.AddWithValue("@yr", Date.Now.Year)

                Try

                    cm = cmd.ExecuteScalar

                    If Not IsDBNull(cm) Then
                        lblcDep.Text = FormatCurrency(cm)
                    Else
                        lblcDep.Text = FormatCurrency(cm)
                    End If


                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()

        End Using





    End Sub

    Sub get_locked(ByVal dt As DataTable)

        Session("dlbal") = 0
        Session("depbal") = 0



        Dim dbal As Double = 0

        Dim acno As String = String.Empty

        Session("depbal") = dt.Compute("sum(bal)", String.Empty)



        If Not dt.Rows.Count = 0 Then

            For Each row As DataRow In dt.Rows
                acno = row("acno")

                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd = New SqlCommand
                        cmd.Connection = con

                        cmd.CommandText = "SELECT COALESCE(SUM(actrans.Drd) - SUM(actrans.Crd), 0) AS bal FROM dbo.dlspec INNER JOIN dbo.actrans   ON dlspec.acn = actrans.acno WHERE dlspec.deposit = @acno "
                        cmd.Parameters.Clear()

                        cmd.Parameters.Add(New SqlParameter("@acno", acno))


                        Try

                            dbal = cmd.ExecuteScalar()

                            If Not IsDBNull(dbal) Then
                                Session("dlbal") += dbal
                            End If


                        Catch ex As Exception
                            Response.Write(ex.ToString)
                        End Try

                    End Using
                    con.Close()

                End Using


            Next
            lblDepBal.Text = FormatCurrency(Session("depbal") - Session("dlbal"))
            lbldl.Text = FormatCurrency(Session("dlbal"))

        Else
            lblDepBal.Text = FormatCurrency(0)

            lbldl.Text = FormatCurrency(0)

        End If

    End Sub

    Private Sub DepositList_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btnDS_Click(sender As Object, e As EventArgs) Handles btnDS.Click

        Session("dep") = "Daily Deposit"
        Session("prod") = "DS"
        get_acc_details()

    End Sub

    Private Sub btnFD_Click(sender As Object, e As EventArgs) Handles btnFD.Click

        Session("dep") = "Fixed Deposit"
        Session("prod") = "FD"
        get_acc_details()

    End Sub

    Private Sub btnKMK_Click(sender As Object, e As EventArgs) Handles btnKMK.Click
        Session("dep") = "KMK Deposit"
        Session("prod") = "KMK"
        get_acc_details()

    End Sub

    Private Sub btnRD_Click(sender As Object, e As EventArgs) Handles btnRD.Click
        Session("dep") = "Recurring Deposit"
        Session("prod") = "RD"
        get_acc_details()

    End Sub

    Private Sub btnRID_Click(sender As Object, e As EventArgs) Handles btnRID.Click
        Session("dep") = "Reinvestment Deposit"
        Session("prod") = "RID"
        get_acc_details()

    End Sub

    Private Sub btnSB_Click(sender As Object, e As EventArgs) Handles btnSB.Click
        Session("dep") = "Savings Deposit"
        Session("prod") = "SB"
        get_acc_details()

    End Sub

    Private Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click
        Dim refurl As Object = ViewState("RefUrl")

        If Not refurl = Nothing Then
            Response.Redirect(refurl.ToString)


        End If
    End Sub
End Class