Imports System.Data.SqlClient

Public Class GroupAccounts
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            ViewState("RefUrl") = Request.UrlReferrer.ToString()
            show_grpAcc()

        End If
    End Sub

    Protected Sub OnItemDataBound(sender As Object, e As RepeaterItemEventArgs)

        If e.Item.ItemType = ListItemType.Item OrElse
                            e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim drv As DataRowView = TryCast(e.Item.DataItem, DataRowView)
            Dim ChildRep As Repeater = TryCast(e.Item.FindControl("rpbrkup"), Repeater)
            ChildRep.DataSource = drv.CreateChildView("cid")
            ChildRep.DataBind()
        End If
    End Sub


    Sub show_grpAcc()

        Dim cld As Integer = 0
        lbldep.Text = Session("dep")

        Dim dt As New DataSet
        lblcid.Text = Session("cid")
        lblfname.Text = Session("firstname")
        lbllname.Text = Session("lastname")
        lbladd.Text = Session("address")



        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "GrpTotal"
                cmd.Parameters.Clear()
                cmd.Parameters.Add(New SqlParameter("@parent", Session("cid")))
                cmd.Parameters.Add(New SqlParameter("@prod", Session("prod")))
                cmd.Parameters.Add(New SqlParameter("@cld", cld))

                Try

                    Using Sda As New SqlDataAdapter(cmd)


                        Sda.Fill(dt)


                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try


            End Using

        End Using

        dt.Relations.Add(New DataRelation("cid", dt.Tables(0).Columns("cid"), dt.Tables(1).Columns("cid")))

        rpSgrp.DataSource = dt.Tables(0)
        rpSgrp.DataBind()



        If Not dt.Tables(0).Rows.Count = 0 Then
            lblDepBal.Text = FormatCurrency(dt.Tables(0).Compute("sum(samt)", String.Empty))
            lbldl.Text = String.Format("{0:00}", dt.Tables(0).Compute("sum(noa)", String.Empty))
            lblcDep.Text = FormatCurrency(dt.Tables(0).Compute("sum(bal)", String.Empty))

        Else
            lblDepBal.Text = FormatCurrency(0)
            lbldl.Text = 0
            lblcDep.Text = FormatCurrency(0)
        End If



    End Sub

    Private Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click


        Dim refurl As Object = ViewState("RefUrl")

        If Not refurl = Nothing Then
            Response.Redirect(refurl.ToString)


        End If
    End Sub

    Private Sub btnDS_Click(sender As Object, e As EventArgs) Handles btnDS.Click

        Session("dep") = "Daily Deposit"
        lbldep.Text = Session("dep")
        Session("prod") = "DS"
        show_grpAcc()


    End Sub

    Private Sub btnFD_Click(sender As Object, e As EventArgs) Handles btnFD.Click
        Session("dep") = "Fixed Deposit"
        lbldep.Text = Session("dep")
        Session("prod") = "FD"
        show_grpAcc()


    End Sub

    Private Sub btnKMK_Click(sender As Object, e As EventArgs) Handles btnKMK.Click
        Session("dep") = "KMK Deposit"
        lbldep.Text = Session("dep")
        Session("prod") = "KMK"
        show_grpAcc()

    End Sub

    Private Sub btnRD_Click(sender As Object, e As EventArgs) Handles btnRD.Click
        Session("dep") = "Recurring Deposit"
        lbldep.Text = Session("dep")
        Session("prod") = "RD"
        show_grpAcc()

    End Sub

    Private Sub btnRID_Click(sender As Object, e As EventArgs) Handles btnRID.Click
        Session("dep") = "Reinvestment Deposit"
        lbldep.Text = Session("dep")
        Session("prod") = "RID"
        show_grpAcc()

    End Sub

    Private Sub btnSB_Click(sender As Object, e As EventArgs) Handles btnSB.Click
        Session("dep") = "Savings Deposit"
        lbldep.Text = Session("dep")
        Session("prod") = "SB"
        show_grpAcc()

    End Sub

    Private Sub btnDCL_Click(sender As Object, e As EventArgs) Handles btnDCL.Click
        Session("dep") = "Daily Collection"
        lbldep.Text = Session("dep")
        Session("prod") = "DCL"
        show_grpAcc()

    End Sub

    Private Sub btnDL_Click(sender As Object, e As EventArgs) Handles btnDL.Click
        Session("dep") = "Deposit Loan"
        lbldep.Text = Session("dep")
        Session("prod") = "DL"
        show_grpAcc()

    End Sub

    Private Sub btnJL_Click(sender As Object, e As EventArgs) Handles btnJL.Click
        Session("dep") = "Jewel Loan"
        lbldep.Text = Session("dep")
        Session("prod") = "JL"
        show_grpAcc()

    End Sub

    Private Sub btnML_Click(sender As Object, e As EventArgs) Handles btnML.Click
        Session("dep") = "Mortgage Loan"
        lbldep.Text = Session("dep")
        Session("prod") = "ML"
        show_grpAcc()

    End Sub

    Private Sub GroupAccounts_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class