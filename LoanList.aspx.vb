Imports System.Data.SqlClient

Public Class LoanList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            get_acc_details()
            ViewState("RefUrl") = Request.UrlReferrer.ToString()


        End If


    End Sub
    Sub get_acc_details()


        'Select Case Session("prod")
        '    Case "DCL"
        '        btnDCL.CssClass = "text-primary"
        '        btnDL.CssClass = "text-dark"
        '        btnJL.CssClass = "text-dark"
        '        btnML.CssClass = "text-dark"


        '    Case "DL"
        '        btnDCL.CssClass = "text-dark"
        '        btnDL.CssClass = "text-primary"
        '        btnJL.CssClass = "text-dark"
        '        btnML.CssClass = "text-dark"


        '    Case "JL"
        '        btnDCL.CssClass = "text-dark"
        '        btnDL.CssClass = "text-dark"
        '        btnJL.CssClass = "text-primary"
        '        btnML.CssClass = "text-dark"


        '    Case "ML"
        '        btnDCL.CssClass = "text-dark"
        '        btnDL.CssClass = "text-dark"
        '        btnJL.CssClass = "text-dark"
        '        btnML.CssClass = "text-primary"



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
                cmd.CommandText = "Loanlist"
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
                        ' get_locked(dt)
                        If dt.Rows.Count > 0 Then
                            lblDepBal.Text = FormatCurrency(dt.Compute("sum(bal)", String.Empty))
                        Else
                            lblDepBal.Text = FormatCurrency(0)
                        End If

                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try


            End Using

        End Using

        If Session("prod") = "JL" Then

            trim_APlan()

        End If


        cld = 1

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "Loanlist"
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

                        'Session("depbal") =


                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try


            End Using

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

        End Using





    End Sub
    Function get_goldrate()
        Dim gr As Double = 0


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                cmd.CommandText = "Select top 1 COALESCE(rate,0) from dbo.goldrate ORDER BY goldrate.rate DESC"
                cmd.Parameters.Clear()


                Try
                    gr = cmd.ExecuteScalar

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()

        End Using

        Return gr


    End Function


    Sub trim_APlan()

        Dim gr As Double = 0
        gr = get_goldrate()


        Dim nw As Double = 0


        Dim rpitem As RepeaterItem

        For Each rpitem In rpAPlan.Items

            nw = 0

            Dim rpg As Label = DirectCast(rpitem.FindControl("lblrpg"), Label)
            Dim acn As Label = DirectCast(rpitem.FindControl("lblacn"), Label)
            Dim bal As Label = DirectCast(rpitem.FindControl("lblbal"), Label)

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand
                    cmd.Connection = con

                    cmd.CommandText = "select COALESCE(tnet,0) from jlstock where jlstock.acn=@acno"
                    cmd.Parameters.Clear()

                    cmd.Parameters.AddWithValue("@acno", Trim(acn.Text))
                    Try
                        nw = cmd.ExecuteScalar
                        If Not IsDBNull(nw) Then
                            rpg.Text = FormatCurrency(Math.Round(CDbl(bal.Text) / nw))

                            If CDbl(rpg.Text) > gr Then

                                rpg.CssClass = "text-danger"
                            Else
                                rpg.CssClass = "text-success"

                            End If
                        End If



                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    End Try

                End Using
                con.Close()

            End Using





        Next

    End Sub
    Private Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click
        Dim refurl As Object = ViewState("RefUrl")

        If Not refurl = Nothing Then
            Response.Redirect(refurl.ToString)


        End If
    End Sub

    Private Sub LoanList_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btnDCL_Click(sender As Object, e As EventArgs) Handles btnDCL.Click
        Session("dep") = "Daily Collection Loan"
        Session("prod") = "DCL"
        get_acc_details()

    End Sub

    Private Sub btnDL_Click(sender As Object, e As EventArgs) Handles btnDL.Click
        Session("dep") = "Deposit Loan"
        Session("prod") = "DL"
        get_acc_details()

    End Sub

    Private Sub btnJL_Click(sender As Object, e As EventArgs) Handles btnJL.Click
        Session("dep") = "Jewel Loan"
        Session("prod") = "JL"
        get_acc_details()

    End Sub

    Private Sub btnML_Click(sender As Object, e As EventArgs) Handles btnML.Click
        Session("dep") = "Mortgage Loan"
        Session("prod") = "ML"
        get_acc_details()

    End Sub
End Class