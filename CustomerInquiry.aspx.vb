

Imports System.Data.SqlClient

Public Class CustomerInquiry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            If Not Session("cid") = Nothing Then
                txtcid.Text = Session("cid")
                txtcid_TextChanged(sender, e)

            End If
        End If

    End Sub

    Private Sub CustomerInquiry_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged

        imgCapture.ImageUrl = Nothing
        Dim DS As Double = 0
        Dim FD As Double = 0
        Dim KMK As Double = 0
        Dim RD As Double = 0
        Dim RID As Double = 0
        Dim SB As Double = 0
        Dim GBal As Double = 0
        Dim CDS As Double = 0
        Dim CFD As Double = 0
        Dim CKMK As Double = 0
        Dim CRD As Double = 0
        Dim CRID As Double = 0
        Dim CSB As Double = 0
        Dim CDBal As Double = 0
        Dim CDL As Double = 0
        Dim CDCL As Double = 0
        Dim CJL As Double = 0
        Dim CML As Double = 0
        Dim ClBal As Double = 0


        upprog.Visible = True
        If Not Trim(txtcid.Text) = "" And Trim(txtcid.Text).Length = 10 Then


            alertmsg.Visible = False
            get_profile(Trim(txtcid.Text))

            Session("cid") = Trim(txtcid.Text)
            lblfname.Text = Session("firstname")
            lbllname.Text = Session("lastname")
            lbldob.Text = Session("dob")
            lblgen.Text = Session("gender")
            lbladd.Text = Session("address")
            lblpin.Text = Session("pincode")
            lblphone.Text = Session("phone")
            lblmobile.Text = Session("mobile")



            imgCapture.ImageUrl = Session("photo")



            lblgrp.Text = Session("parent")
            lblnominee.Text = Session("nominee")
            lblrelation.Text = Session("relation")
            lblpan.Text = Session("pan")
            lblappno.Text = Session("appno")
            If Session("cp") = 1 Then
                upcp.Visible = True
            Else
                upcp.Visible = False

            End If

            lblshare.Text = Session("share")
            lblshrval.Text = FormatCurrency(Session("shareval"))





            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand
                    cmd.Connection = con
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "GrpAcc"
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add(New SqlClient.SqlParameter("@grpid", Trim(txtcid.Text)))

                    Using dr = cmd.ExecuteReader

                        If dr.HasRows Then

                            Do While dr.Read()
                                Select Case Trim(dr(2))
                                    Case "DS"
                                        DS += dr(4)
                                        GBal += dr(4)

                                    Case "FD"
                                        FD += dr(4)
                                        GBal += dr(4)


                                    Case "RD"
                                        RD += dr(4)
                                        GBal += dr(4)


                                    Case "RID"
                                        RID += dr(4)
                                        GBal += dr(4)


                                    Case "KMK"
                                        KMK += dr(4)
                                        GBal = +dr(4)

                                    Case "SB"
                                        SB += dr(4)
                                        GBal += dr(4)

                                End Select

                            Loop

                        End If
                    End Using
                End Using
                con.Close()

            End Using


            lblgDS.Text = FormatNumber(DS)

            lblgFD.Text = FormatNumber(FD)
            lblgKMK.Text = FormatNumber(KMK)
            lblgRD.Text = FormatNumber(RD)
            lblgRID.Text = FormatNumber(RID)
            lblgSB.Text = FormatNumber(SB)
            lblgTotal.Text = FormatNumber(GBal)
            lblGBal.Text = FormatCurrency(GBal)


            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand
                    cmd.Connection = con

                    query = "SELECT  master.product,(SUM(actrans.Crd) - SUM(actrans.Drd)) AS BAL FROM dbo.master INNER JOIN dbo.actrans "
                    query += " ON master.acno = actrans.acno WHERE master.cid = @cid AND master.cld = 0 GROUP BY master.product"
                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@cid", Trim(txtcid.Text))

                    Using dr = cmd.ExecuteReader

                        If dr.HasRows Then

                            Do While dr.Read()

                                Select Case Trim(dr(0))

                                    Case "DS"
                                        CDS += dr(1)
                                        CDBal += dr(1)
                                    Case "FD"
                                        CFD += dr(1)
                                        CDBal += dr(1)
                                    Case "KMK"
                                        CKMK += dr(1)
                                        CDBal += dr(1)
                                    Case "RD"
                                        CRD += dr(1)
                                        CDBal += dr(1)
                                    Case "RID"
                                        CRID += dr(1)
                                        CDBal += dr(1)
                                    Case "SB"
                                        CSB += dr(1)
                                        CDBal += dr(1)
                                    Case "DCL"
                                        CDCL += -dr(1)
                                        ClBal += -dr(1)
                                    Case "DL"
                                        CDL += -dr(1)
                                        ClBal += -dr(1)
                                    Case "JL"
                                        CJL += -dr(1)
                                        ClBal += -dr(1)
                                    Case "ML"
                                        CML += -dr(1)
                                        ClBal += -dr(1)

                                End Select
                            Loop

                        End If
                    End Using
                End Using
                con.Close()

            End Using


            lblcDS.Text = FormatNumber(CDS)
            lblcFD.Text = FormatNumber(CFD)
            lblcKMK.Text = FormatNumber(CKMK)
            lblcRD.Text = FormatNumber(CRD)
            lblcRID.Text = FormatNumber(CRID)
            lblcSB.Text = FormatNumber(CSB)


            lblcDCL.Text = FormatNumber(CDCL)
            lblcDL.Text = FormatNumber(CDL)
            lblcJL.Text = FormatNumber(CJL)
            lblcML.Text = FormatNumber(CML)
            'lbltLoan.Text = FormatCurrency(ClBal)
            lblcLtotal.Text = FormatNumber(ClBal)
            'lblcLBal.Text = FormatCurrency(ClBal)


            lblcTotalBal.Text = FormatCurrency(CDBal - ClBal)
            lblcDep.Text = FormatCurrency(CDBal)
            lbltLoan.Text = FormatCurrency(ClBal)
            lblcDTotal.Text = FormatCurrency(CDBal)
            lblcLBal.Text = FormatCurrency(ClBal)
            lblcTotal.Text = FormatCurrency(CDBal)


            upCust.Visible = True
            upcustprofile.Visible = True
            upinvdet.Visible = True



        Else
            upCust.Visible = False
            upcustprofile.Visible = False
            upinvdet.Visible = False

            sesuser.Text = Session("sesusr")
            alertmsg.Visible = True




        End If

        upprog.Visible = False


    End Sub

    Private Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click
        Session("cid") = Nothing
        txtcid.Text = ""
        upCust.Visible = False
        upcustprofile.Visible = False
        upinvdet.Visible = False
        alertmsg.Visible = False
        sesuser.Text = ""
        imgCapture.ImageUrl = Nothing
    End Sub

    Private Sub btn_cDS_Click(sender As Object, e As EventArgs) Handles btn_cDS.Click
        Session("dep") = "Daily Deposit"
        Session("prod") = "DS"
        Response.Redirect("DepositList.aspx")


    End Sub

    Private Sub btn_cFD_Click(sender As Object, e As EventArgs) Handles btn_cFD.Click
        Session("dep") = "Fixed Deposit"
        Session("prod") = "FD"
        Response.Redirect("DepositList.aspx")


    End Sub

    Private Sub btn_cKMK_Click(sender As Object, e As EventArgs) Handles btn_cKMK.Click

        Session("dep") = "KMK Deposit"
        Session("prod") = "KMK"
        Response.Redirect("DepositList.aspx")


    End Sub

    Private Sub btn_cRD_Click(sender As Object, e As EventArgs) Handles btn_cRD.Click
        Session("dep") = "Recurring Deposit"
        Session("prod") = "RD"
        Response.Redirect("DepositList.aspx")


    End Sub

    Private Sub btn_cRID_Click(sender As Object, e As EventArgs) Handles btn_cRID.Click
        Session("dep") = "Reinvestment Deposit"
        Session("prod") = "RID"
        Response.Redirect("DepositList.aspx")


    End Sub

    Private Sub btn_cSB_Click(sender As Object, e As EventArgs) Handles btn_cSB.Click
        Session("dep") = "Savings Deposit"
        Session("prod") = "SB"
        Response.Redirect("DepositList.aspx")


    End Sub

    Private Sub btn_cDCL_Click(sender As Object, e As EventArgs) Handles btn_cDCL.Click
        Session("dep") = "Daily Collection Loan"
        Session("prod") = "DCL"
        Response.Redirect("LoanList.aspx")

    End Sub

    Private Sub btn_CDL_Click(sender As Object, e As EventArgs) Handles btn_CDL.Click
        Session("dep") = "Deposit Loan"
        Session("prod") = "DL"
        Response.Redirect("LoanList.aspx")

    End Sub

    Private Sub btn_cJL_Click(sender As Object, e As EventArgs) Handles btn_cJL.Click
        Session("dep") = "Jewel Loan"
        Session("prod") = "JL"
        Response.Redirect("LoanList.aspx")

    End Sub

    Private Sub btn_CML_Click(sender As Object, e As EventArgs) Handles btn_CML.Click
        Session("dep") = "Mortgage Loan"
        Session("prod") = "ML"
        Response.Redirect("LoanList.aspx")

    End Sub

    Private Sub btn_gDS_Click(sender As Object, e As EventArgs) Handles btn_gDS.Click
        Session("dep") = "Daily Deposit"
        Session("prod") = "DS"
        Response.Redirect("GroupAccounts.aspx")

    End Sub

    Private Sub btn_gFD_Click(sender As Object, e As EventArgs) Handles btn_gFD.Click
        Session("dep") = "Fixed Deposit"
        Session("prod") = "FD"
        Response.Redirect("GroupAccounts.aspx")

    End Sub

    Private Sub btn_gKMK_Click(sender As Object, e As EventArgs) Handles btn_gKMK.Click
        Session("dep") = "KMK Deposit"
        Session("prod") = "KMK"
        Response.Redirect("GroupAccounts.aspx")

    End Sub

    Private Sub btn_gRD_Click(sender As Object, e As EventArgs) Handles btn_gRD.Click
        Session("dep") = "Recurring Deposit"
        Session("prod") = "RD"
        Response.Redirect("GroupAccounts.aspx")

    End Sub

    Private Sub btn_gRID_Click(sender As Object, e As EventArgs) Handles btn_gRID.Click
        Session("dep") = "Reinvestment Deposit"
        Session("prod") = "RID"
        Response.Redirect("GroupAccounts.aspx")

    End Sub

    Private Sub btn_gSB_Click(sender As Object, e As EventArgs) Handles btn_gSB.Click
        Session("dep") = "Savings Deposit"
        Session("prod") = "SB"
        Response.Redirect("GroupAccounts.aspx")

    End Sub
End Class