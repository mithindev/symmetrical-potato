Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Drawing

Public Class SoaDeposit
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim yearpart As String
    Dim fltr As Boolean


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Me.IsPostBack Then

            Dim param1 As String = Request.QueryString("acno")
            ViewState("RefUrl") = Request.UrlReferrer.ToString()
            fltr = False
            Session("flt") = False

            If Not param1 = "" Then
                Session("soaprint") = param1
                get_acc_details(param1)
                bind_grid()
                get_Img()
            End If

        End If

    End Sub

    Protected Sub OnSelectedIndexChanged_gv_dep(ByVal sender As Object, ByVal e As EventArgs) Handles disp.SelectedIndexChanged
        'Dim pName As String = disp.SelectedRow.Cells(1).Text
        'MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)
        For Each row As GridViewRow In disp.Rows


            If row.RowIndex = disp.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#EAF2D3")

                ' txtlimit.Text = disp.SelectedRow.Cells(1).Text 'row.Cells.Item(0).Text
                'Dim dt As System.Web.UI.WebControls.Image = CType(disp.SelectedRow.Cells(1).FindControl("imgselct"), System.Web.UI.WebControls.Image)
                Dim tid As Label = CType(disp.SelectedRow.Cells(1).FindControl("lbltid"), Label)
                ' get_data(dt.Text)
                'dt.Visible = True
                ' get_ac_info(Trim(acn.Text))
                ' Me.popupdep.Show()

                show_vinfo(tid.Text)



            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#f0f4f5")
                row.ToolTip = "Click to select this row."
                ' Dim dt As System.Web.UI.WebControls.Image = CType(disp.SelectedRow.Cells(0).FindControl("imgselct"), System.Web.UI.WebControls.Image)
                ' get_data(dt.Text)
                ' dt.Visible = False

            End If
        Next
    End Sub

    Sub show_vinfo(ByVal tid As Double)


        query = "SELECT id,sesusr,entryat from actrans where tid=@tid "
        'Dim adapter As New SqlDataAdapter(sql, con)
        cmd.Parameters.Clear()

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@tid", tid)
        Dim dr As SqlDataReader

        dr = cmd.ExecuteReader()

        If dr.HasRows() Then
            dr.Read()

            '       lblvno.Text = IIf(IsDBNull(dr(1)), "", dr(1))
            '      lblentyby.Text = IIf(IsDBNull(dr(2)), "", dr(2))
            '     lblentyat.Text = IIf(IsDBNull(dr(3)), "", dr(3))

        End If

        ' pop_vinfo.Show()

        '   show_vinfo = "V.no :" + lblvno.Text + vbCrLf + "Entered By :" + lblentyby.Text + " @ " + lblentyat.Text

    End Sub


    Private Sub disp_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles disp.SelectedIndexChanging
        Dim x As Integer = disp.SelectedValue
    End Sub

    Protected Sub gv_dep_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles disp.RowDataBound
        'EAF2D3
        If e.Row.RowType = DataControlRowType.DataRow Then
            'e.Row.Attributes("onmouseover") = "this.style.backgroundColor='#EAF2D3';"
            'e.Row.Attributes("onmouseout") = "this.style.backgroundColor='#f0f4f5';"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(disp, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
            e.Row.Attributes("style") = "cursor:pointer"
            ' e.Row.ToolTip = "Click last column for selecting this row."
            If (e.Row.RowIndex Mod 2 <> 0) Then
                e.Row.Cells(5).BackColor = Drawing.ColorTranslator.FromHtml("#f6eec7")
                e.Row.Cells(6).BackColor = Drawing.ColorTranslator.FromHtml("#f6eec7")
                e.Row.Cells(7).BackColor = Drawing.ColorTranslator.FromHtml("#f6eec7")
            End If

        End If

    End Sub


    Function get_intpaid(ByVal acno As String)
        Dim bal As Double = 0

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    query = "SELECT COALESCE(SUM(actransc.Crd),0) AS bal FROM dbo.actransc WHERE actransc.acno = @acno and Type ='INTR' "
                Else
                    query = "SELECT COALESCE(SUM(actrans.Crd),0) AS bal FROM dbo.actrans WHERE actrans.acno = @acno and Type ='INTR' "
                End If

                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", acno)
                Try

                    bal = cmd.ExecuteScalar


                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()

        End Using


        Return bal

    End Function

    Function get_bal(ByVal acno As String)
        Dim bal As Double = 0

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    query = "SELECT COALESCE(SUM(actransc.Crd) - SUM(actransc.Drd),0) AS bal FROM dbo.actransc WHERE actransc.acno = @acno"
                Else
                    query = "SELECT COALESCE(SUM(actrans.Crd) - SUM(actrans.Drd),0) AS bal FROM dbo.actrans WHERE actrans.acno = @acno"
                End If

                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", acno)
                Try

                    bal = cmd.ExecuteScalar


                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()

        End Using


        Return bal

    End Function

    Sub get_acc_details(ByVal acno As String)



        get_soa_details(acno)


        If IsNothing(Session("acno")) Then
            Response.Redirect("/soa.aspx?typ=d")
        End If

        lbldat.Text = Session("Date")
        lblwef.Text = Session("Date")
        lblacn.Text = Session("acno")
        lblacno.Text = Session("acno")
        lblcid.Text = Session("cid")
        lblprod.Text = Session("Product")

        bind_knl(Trim(lblacno.Text))


        lblamt.Text = FormatCurrency(Session("amount"))
        lblcroi.Text = Session("CInt")
        lblroi.Text = Session("dint")
        lblbal.Text = FormatCurrency(get_bal(lblacn.Text))
        ''lblintpaid.Text = FormatCurrency(get_intpaid(Session("acno")))
        'Session("cld")

        If Session("cld") = 0 Then
            lblstus.Text = "Active"
            lblba.Attributes.Add("Class", "badge badge-outline badge-success")
        Else
            lblstus.Text = "Closed"
            lblba.Attributes.Add("Class", "badge badge-outline badge-danger")



        End If

        lblprd.Text = Session("prd").ToString + " M"
        If Not Session("mdate") = DateTime.MinValue.AddYears(1899) Then
            If Session("mdate") = DateTime.MinValue Then
                lblmdat.Text = ""
            Else

                lblmdat.Text = Session("mdate")
            End If
        Else
            lblmdat.Text = ""
        End If

        lblmamt.Text = FormatCurrency(Session("mamt"))

        txtcid.Text = Session("cid")


        txtwef.Text = Session("Date")
        txtmdate.Text = Session("mdate")

        If Not Session("mdate") = DateTime.MinValue.AddYears(1899) Then
            If Session("mdate") = DateTime.MinValue Then
                lblmdat.Text = ""
            Else

                lblmdat.Text = Session("mdate")
            End If
        Else
            txtmdate.Text = ""
        End If

        txtpbon.Text = Session("pbi")
        lblEacn.Text = Session("acno")




        If Session("pb") = True Then
            ispb.Checked = True

        End If

        lblpb.Text = IIf(Session("pb") = True, "Yes", "No")
        lblpbon.Text = Session("pbi")
        fc.Checked = IIf(Session("fc") = 0, False, True)
        If Not Session("agent") = "" Then
            ddagent.SelectedItem.Text = Session("agent")
        End If

        lblrenewal.Text = Session("renewacn")
        get_profile(Session("cid"))

        lblEname.Text = Session("firstname")
        lblElname.Text = Session("lastname")
        lblEadd.Text = Session("address")
        lblfname.Text = Session("firstname")
        lbllname.Text = Session("lastname")
        'HttpContext.Current.Session("dob") = IIf(IsDBNull(dr(3)), "", dr(3))
        'HttpContext.Current.Session("gender") = IIf(IsDBNull(dr(4)), "", dr(4))
        lbladd.Text = Session("address")
        'HttpContext.Current.Session("pincode") = IIf(IsDBNull(dr(6)), "", dr(6))
        'HttpContext.Current.Session("phone") = IIf(IsDBNull(dr(7)), "", dr(7))
        lblmobile.Text = Session("mobile")


        UPDATE_MAT_VAL_c()



        If CType(Session("Date"), Date).Equals(Date.Today) Then
            btn_delete.Visible = False
        Else
            btn_delete.Visible = False
        End If


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    query = "SELECT COALESCE(siacno,' ') AS acn FROM dbo.stdinsc WHERE stdinsc.acno = @acno"
                Else
                    query = "SELECT COALESCE(siacno,' ') AS acn FROM dbo.stdins WHERE stdins.acno = @acno"
                End If

                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", acno)
                Try

                    lblsi.Text = cmd.ExecuteScalar

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()

        End Using


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "select top 1 isnull(date,'') from actrans where acno=@acno order by date "
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", Trim(lblacn.Text))
                Dim dt = cmd.ExecuteScalar()
                txtdate.Text = dt
                lbldat.Text = dt

            End Using
            cmd.Dispose()

            con.Close()

        End Using


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    query = "SELECT COALESCE(acn,' ') AS sacn FROM dbo.dlspecc WHERE dlspecc.deposit = @acno"
                Else
                    query = "SELECT COALESCE(acn,' ') AS sacn FROM dbo.dlspec WHERE dlspec.deposit = @acno"
                End If


                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", acno)
                Try

                    lbldl.Text = cmd.ExecuteScalar

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()

        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT nominee,relation,address FROM dbo.nominee WHERE nominee.acno = @acno"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", acno)
                Try

                    Using dr As SqlDataReader = cmd.ExecuteReader

                        If dr.HasRows Then
                            dr.Read()
                            lblnomi.Text = IIf(IsDBNull(dr(0)), "", dr(0))
                            lblrel.Text = IIf(IsDBNull(dr(1)), "", dr(1))
                            lblnadd.Text = IIf(IsDBNull(dr(2)), "", dr(2))
                        End If


                    End Using


                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()

        End Using


        prntdate.Text = lbldat.Text
        prntwef.Text = lblwef.Text
        prntaccno.Text = lblacno.Text
        prntprod.Text = get_pro(Trim(lblprod.Text))

        If session_user_role = "Audit" Then
            prntroi.Text = Session("cint").ToString + "%"
        Else

            prntroi.Text = Session("dint").ToString + "%"
        End If
        Dim intr As Double = Math.Round(Session("amount") * Session("dint") / 100 / 12, 0)

        lblhome.Text = get_home()


        prntamt.Text = FormatCurrency(Session("amount"))
        prntaiw.Text = "( " + get_wrds(Session("amount")) + " )"
        Dim prd = " Months"

        If Trim(lblprod.Text) = "FD" Then
            Session("mamt") = Session("amount")
        End If
        prntprd.Text = Session("prd").ToString.PadRight(3) + prd
        prntmdt.Text = Session("mdate")
        prntmamt.Text = FormatCurrency(Session("mamt"))
        prntmaiw.Text = "( " + get_wrds(Session("mamt")) + " )"

        lblWefLeft.Text = prntwef.Text
        lblNameLeft.Text = Trim(Session("firstname")).PadRight(15, " ") + "(" + String.Empty.PadRight(3, " ") + Session("cid").ToString.PadRight(2, " ") + ")"
        lblAddressLeft.Text = Session("address")
        lblDateLeft.Text = prntdate.Text
        lblAccountNoLeft.Text = prntaccno.Text
        lblMaturityValueLeft.Text = prntmamt.Text
        lblMaturityDateLeft.Text = prntmdt.Text
        lblPeriodLeft.Text = prntprd.Text
        lblInterestRateLeft.Text = prntroi.Text

        prntcustomer.Text = Trim(Session("firstname")).PadRight(15, " ") + "(" + String.Empty.PadRight(3, " ") + Session("cid").ToString.PadRight(2, " ") + ")"
        prntfirstname.Text = Trim(Session("lastname")).PadRight(5, " ")

        prntadd.Text = Session("address")
        If Trim(lblprod.Text) = "FD" Then
            prntint.Text = "Monthly Interest of  " + FormatCurrency(intr) + " will be Transfered to SB Account No  " + lblsi.Text

        End If
        If Trim(lblprod.Text) = "RID" Then
            prntint.Text = "Total Interest Payable : " + FormatCurrency(CDbl(lblmamt.Text) - CDbl(lblamt.Text))
            
            ' RID Specific Layout changes
            lblMaturityValueLeft.Text = prntamt.Text ' Principal at top left
            lblMaturityValueLeftBottom.Text = prntmamt.Text ' Maturity Value at bottom left (above name)
            trMaturityValueLeftBottom.Visible = True
        Else
            trMaturityValueLeftBottom.Visible = False
        End If
        'prntnname.Text = lblnomi.Text
        ''rntnadd.Text = lblnadd.Text


    End Sub




    Function get_pro(ByVal prd As String)

        If con.State = ConnectionState.Closed Then con.Open()


        Dim prdname As String
        Dim query As String = String.Empty
        cmd.Connection = con
        query = "SELECT name from products where products.shortname='" + prd + "'"
        cmd.CommandText = query
        prdname = cmd.ExecuteScalar()


        Return prdname
    End Function

    Sub UPDATE_MAT_VAL_c()
        Dim PRIN As Integer
        Dim MN As Integer
        Dim MNT As Integer
        Dim PRAMT As Integer
        Dim PR As Integer
        PRIN = Session("amount")
        MN = Session("prd")

        If Trim(Session("product")) = "RD" Then

            PRAMT = PRIN
            For j = 1 To MN
                MNT = PRAMT * Session("CInt") / 100
                MNT = MNT / 12
                PRAMT = PRAMT + MNT
                PR = PRAMT + PRIN
                PRAMT = Format(PR, "#")
                PRAMT = Val(PR)
            Next
            PRAMT = PRAMT - PRIN
            lblmatc.Text = FormatCurrency(Math.Round(PRAMT))
        ElseIf Trim(Session("product")) = "RID" Then
            PRIN = Session("amount")
            MN = Session("prd")
            MN = MN / 3
            PRAMT = PRIN
            For j = 1 To MN
                MNT = PRAMT * Session("CInt") / 100
                MNT = MNT / 12
                PRAMT = PRAMT + (MNT * 3)
            Next
            ' lblmat.Text = FormatCurrency(Math.Round(PRAMT))
            lblmatc.Text = FormatCurrency(Math.Round(PRAMT))
        End If

    End Sub




    Private Sub SoaDeposit_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub


    Sub bind_knl(ByVal parent As String)



        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()


            query = "SELECT masterc.acno ,masterc.cid ,member.FirstName ,masterc.amount FROM dbo.masterc INNER JOIN dbo.member "
            query += " On masterc.cid = member.MemberNo WHERE masterc.parent = @parent ORDER BY masterc.serial "


            Dim ds As New DataSet

            Using adapter As New SqlDataAdapter(query, con)
                adapter.SelectCommand.Parameters.AddWithValue("@parent", parent)



                adapter.Fill(ds)

            End Using

            rpknl.DataSource = ds
            rpknl.DataBind()

        End Using


    End Sub


    Private Sub bind_grid()


        Dim cbal As Double = 0
        Dim dbal As Double = 0
        Dim newrow As DataRow
        Dim reformatted1 As String = String.Empty
        Dim reformatted As String = String.Empty


        Dim reader As SqlDataReader

        Dim dt As New DataTable

        If con.State = ConnectionState.Closed Then con.Open()
        If dt.Columns.Count = 0 Then
            dt.Columns.Add("date", GetType(String))
            dt.Columns.Add("narration", GetType(String))
            dt.Columns.Add("due", GetType(String))
            dt.Columns.Add("drd", GetType(Decimal))
            dt.Columns.Add("crd", GetType(Decimal))
            dt.Columns.Add("dbal", GetType(Decimal))
            dt.Columns.Add("tid", GetType(Integer))
            dt.Columns.Add("drc", GetType(Decimal))
            dt.Columns.Add("crc", GetType(Decimal))
            dt.Columns.Add("cbal", GetType(Decimal))

        End If



        Dim X As Integer = 1

        disp.EmptyDataText = "No Records Found"
        cmdi.Parameters.Clear()

        If fltr = False Then

            If session_user_role = "Audit" Then
                query = "SELECT  date,narration,due,drc,crc,cbal,tid,drd,crd FROM dbo.[actransc] WHERE actransc.acno=@acno and scroll=@scroll  ORDER BY date,tid"
            Else
                query = "SELECT  date,narration,due,drc,crc,cbal,tid,drd,crd FROM dbo.[actrans] WHERE actrans.acno=@acno and scroll=@scroll  ORDER BY date,tid "
            End If
            lblstprd.Text = ""
        Else
            Dim dat As DateTime = DateTime.ParseExact(Session("frmdt"), "dd-MM-yyyy", CultureInfo.InvariantCulture)
            reformatted = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            Dim dat1 As DateTime = DateTime.ParseExact(Session("todt"), "dd-MM-yyyy", CultureInfo.InvariantCulture)
            reformatted1 = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            lblstprd.Text = "Statement from " + txtfrm.Text + " To " + txtto.Text

            newrow = dt.NewRow

            newrow(0) = txtfrm.Text
            newrow(1) = "Opening Balance"
            newrow(2) = ""
            newrow(3) = 0
            newrow(4) = 0
            dbal = get_opening(lblacn.Text, reformatted)
            newrow(5) = dbal
            newrow(6) = 0
            newrow(7) = 0
            newrow(8) = 0
            newrow(9) = 0



            dt.Rows.Add(newrow)


            If session_user_role = "Audit" Then
                query = "SELECT  date,narration,due,drc,crc,cbal,tid,drd,crd FROM dbo.[actransc] WHERE actransc.acno=@acno and scroll=@scroll and CONVERT(VARCHAR(20), actransc.date, 112) between @frm and @to ORDER BY date,tid"
            Else
                query = "SELECT  date,narration,due,drc,crc,cbal,tid,drd,crd FROM dbo.[actrans] WHERE actrans.acno=@acno and scroll=@scroll and CONVERT(VARCHAR(20), actrans.date, 112) between @frm and @to  ORDER BY date,tid "
            End If


        End If


        cmdi.Connection = con
        cmdi.Parameters.AddWithValue("@acno", Trim(Session("acno")))
        cmdi.Parameters.AddWithValue("@scroll", X)
        If fltr = True Then
            cmdi.Parameters.AddWithValue("@frm", reformatted)
            cmdi.Parameters.AddWithValue("@to", reformatted1)
        End If
        cmdi.CommandText = query

        Try

            reader = cmdi.ExecuteReader()



            If reader.HasRows() Then

                While reader.Read()

                    newrow = dt.NewRow

                    newrow(0) = reader(0).ToString
                    newrow(1) = reader(1).ToString
                    newrow(2) = reader(2).ToString
                    newrow(3) = reader(3).ToString
                    newrow(4) = reader(4).ToString
                    dbal = (reader(4).ToString - reader(3).ToString) + dbal
                    newrow(5) = dbal
                    newrow(6) = reader(6).ToString
                    newrow(7) = reader(7).ToString
                    newrow(8) = reader(8).ToString
                    cbal = (reader(8).ToString - reader(7).ToString) + cbal
                    newrow(9) = cbal


                    dt.Rows.Add(newrow)


                End While

            End If



            disp.DataSource = dt
            disp.DataBind()

            rptab.DataSource = dt
            rptab.DataBind()



        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            ds.Dispose()
            cmd.Dispose()
            con.Close()


        End Try




        trim_disp()




    End Sub

    Private Sub get_Img()
        Dim imgx As Image = Nothing
        Dim imgbytes As Byte() = Nothing
        Dim stream As MemoryStream = Nothing

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select photo from kyc where kyc.memberno='" + Trim(txtcid.Text) + "'"
        cmd.Connection = con
        cmd.CommandText = query

        Try
            Dim dr As SqlDataReader = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()



                If Not IsDBNull(dr(0)) Then
                    imgbytes = CType(dr.GetValue(0), Byte())
                    stream = New MemoryStream(imgbytes, 0, imgbytes.Length)
                    imgx = Image.FromStream(stream)

                    Dim imagePath As String = String.Format("~/Captures/{0}.png", Trim(txtcid.Text))
                    File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                    imgCapture.ImageUrl = "~/captures/" + Trim(txtcid.Text) + ".png?" + DateTime.Now.Ticks.ToString()
                    imgCapture.Visible = True
                Else
                    imgCapture.Visible = False


                End If
            Else

                imgCapture.Visible = False


            End If
            dr.Close()


        Catch ex As Exception
            imgCapture.Visible = False
        End Try
    End Sub

    Public Function get_opening(ByVal acn As String, ByVal dt As String)



        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Parameters.Clear()


        Dim x As Integer = 1
        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno =@acno and scroll =@scroll and CONVERT(VARCHAR(20), actrans.date, 112) <= '" + dt + "' GROUP BY [actrans].acno"
        cmd.Parameters.AddWithValue("@acno", acn)
        cmd.Parameters.AddWithValue("@scroll", x)
        ' Dim sql As String = "select top 1 cbal,dbal from dbo.[actrans] where [actrans].acno='" + acn + "' ORDER BY date DESC"
        Dim ds_bal As New DataSet

        '        Dim Adapter As New SqlDataAdapter(sql, con)
        '       Adapter.Fill(ds_bal)
        cmd.Connection = con
        cmd.CommandText = sql

        Dim reader As SqlDataReader = cmd.ExecuteReader()


        If reader.HasRows Then
            reader.Read()
            Session("dbal") = reader(1).ToString - reader(0).ToString
            Session("cbal") = reader(3).ToString - reader(2).ToString



        Else
            Session("cbal") = 0
            Session("dbal") = 0
        End If
        Return Session("dbal")

    End Function

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        disp.PageIndex = e.NewPageIndex

        'If disp.PageIndex = 0 Then

        '    total = pgtot(disp.PageIndex).ToString
        'Else

        '    total = pgtot(disp.PageIndex - 1).ToString
        'End If


        bind_grid()
    End Sub

    Sub trim_disp()

        '' For i As Integer = disp.Rows.Count - 1 To 0 Step -1
        For i As Integer = 0 To rptab.Items.Count - 1

            Dim cr As Label = DirectCast(rptab.Items(i).FindControl("lblcr"), Label)
            Dim dr As Label = DirectCast(rptab.Items(i).FindControl("lbldr"), Label)
            Dim dat As Label = DirectCast(rptab.Items(i).FindControl("lbldat"), Label)

            Dim ccr As Label = DirectCast(rptab.Items(i).FindControl("lblccr"), Label)
            Dim cdr As Label = DirectCast(rptab.Items(i).FindControl("lblcdr"), Label)
            Dim cbal As Label = DirectCast(rptab.Items(i).FindControl("lblcbal"), Label)

            If i = 0 Then
                prntdate.Text = Convert.ToDateTime(dat.Text).ToString("dd/MM/yyyy") 'String.Format("{0:dd/mm/yyyy}", dat.Text)

            End If


            'ccr.BackColor = System.Drawing.ColorTranslator.FromHtml("#EAF2D3")
            'cdr.BackColor = System.Drawing.ColorTranslator.FromHtml("#EAF2D3")
            'cbal.BackColor = System.Drawing.ColorTranslator.FromHtml("#EAF2D3")
            If cr.Text = 0 Then
                cr.Text = ""
            Else
                Session("total") = Session("total") + CDbl(cr.Text)
            End If

            If dr.Text = 0 Then
                dr.Text = ""
            Else
                Session("total") = Session("total") - CDbl(dr.Text)
            End If

            If ccr.Text = 0 Then
                ccr.Text = ""
            Else
                Session("ctotal") = Session("ctotal") + CDbl(ccr.Text)
            End If

            If cdr.Text = 0 Then
                cdr.Text = ""
            Else
                Session("ctotal") = Session("ctotal") - CDbl(cdr.Text)
            End If
            'If bal.Text = 0 Then
            '    bal.Text = ""
            'End If
            dat.Text = Convert.ToDateTime(dat.Text).ToString("dd/MM/yyyy") 'String.Format("{0:dd/mm/yyyy}", dat.Text)



        Next
        ' disp.PageIndex = disp.PageCount

        '  pgtot(disp.PageIndex) = total
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click




    End Sub

    Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT firstname,lastname,address from member where member.memberno=@acno"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", Trim(txtcid.Text))
                Try

                    Using dr As SqlDataReader = cmd.ExecuteReader

                        If dr.HasRows Then
                            dr.Read()
                            lblEname.Text = IIf(IsDBNull(dr(0)), "", dr(0))
                            lblElname.Text = IIf(IsDBNull(dr(1)), "", dr(1))
                            lblEadd.Text = IIf(IsDBNull(dr(2)), "", dr(2))
                        End If


                    End Using


                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()

        End Using

    End Sub

    Private Sub txtdate_TextChanged(sender As Object, e As EventArgs) Handles txtdate.TextChanged



    End Sub

    Private Sub txtwef_TextChanged(sender As Object, e As EventArgs) Handles txtwef.TextChanged

        If Not txtwef.Text = "" Then

            txtmdate.Text = CDate(txtwef.Text).AddMonths(Session("prd"))
        End If


    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

        Dim agent As String
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If ispb.Checked = True Then
                    If session_user_role = "Audit" Then
                        query = "update masterc set cid=@cid,date=@date,mdate=@mdate,pb=@pb,pbi=@pbi where masterc.acno=@acno"
                    Else
                        query = "update master set cid=@cid,date=@date,mdate=@mdate,pb=@pb,pbi=@pbi,fc=@fc,agent=@agent where master.acno=@acno"
                    End If

                Else
                    If session_user_role = "Audit" Then
                        query = "update masterc set cid=@cid,date=@date,mdate=@mdate where masterc.acno=@acno"
                    Else
                        query = "update master set cid=@cid,date=@date,mdate=@mdate,fc=@fc,agent=@agent where master.acno=@acno"
                    End If

                End If

                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", Trim(lblacno.Text))
                cmd.Parameters.AddWithValue("@cid", Trim(txtcid.Text))
                cmd.Parameters.AddWithValue("@date", CDate(txtwef.Text))
                If Trim(lblprod.Text) = "SB" Then
                    cmd.Parameters.AddWithValue("@mdate", "")
                Else

                    cmd.Parameters.AddWithValue("@mdate", CDate(txtmdate.Text))
                End If

                cmd.Parameters.AddWithValue("@fc", IIf(fc.Checked = True, 1, 0))
                If ispb.Checked = True Then
                    cmd.Parameters.AddWithValue("@pb", 1)
                    cmd.Parameters.AddWithValue("@pbi", CDate(txtpbon.Text))
                End If
                If ddagent.SelectedItem.Text = "<--Select-->" Then
                    agent = ""
                Else
                    agent = ddagent.SelectedItem.Text
                End If

                cmd.Parameters.AddWithValue("@agent", agent)


                Try

                    cmd.ExecuteNonQuery()


                    lbluser.Text = Session("sesusr")
                    lblmsg.Text = "Account Updated !"
                    alertinfo.Visible = True


                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()

        End Using


    End Sub

    Sub clear_masterc(ByVal acno As String)

        Dim dat1 As DateTime = DateTime.ParseExact(Session("date"), "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = " delete from masterc WHERE masterc.acno = @acn AND CONVERT(VARCHAR(20), date, 112) = @dt"
                cmd.Connection = con
                cmd.CommandText = query

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acn", acno)
                cmd.Parameters.AddWithValue("@dt", reformattedto)
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try

            End Using
            con.Close()

        End Using

    End Sub
    Sub clear_transc(ByVal acno As String, ByVal reformattedto As String)
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = " delete from actransc WHERE acno = @acn AND CONVERT(VARCHAR(20), date, 112) = @dt"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acn", acno)
                cmd.Parameters.AddWithValue("@dt", reformattedto)
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)
                Finally



                End Try
            End Using
            con.Close()

        End Using


    End Sub
    Sub clear_supplementc(ByVal acno As String, ByVal reformattedto As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand


                query = " delete from suplementc WHERE acn = @acn AND CONVERT(VARCHAR(20), date, 112) = @dt"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acn", acno)
                cmd.Parameters.AddWithValue("@dt", reformattedto)
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)
                Finally
                    con.Close()

                End Try

            End Using
            con.Close()

        End Using

    End Sub
    Sub clear_stdins(ByVal acno As String, ByVal reformattedto As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand


                query = " delete from stdinsc WHERE acno = @acn "
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acn", acno)
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)
                Finally
                    con.Close()

                End Try

            End Using
            con.Close()

        End Using


    End Sub
    Sub clear_nominee(ByVal acno As String, ByVal reformattedto As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand


                query = " delete from nominee WHERE acno = @acn "
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acn", acno)
                cmd.Connection = con
                cmd.CommandText = query
                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)
                Finally
                    con.Close()

                End Try

            End Using
            con.Close()

        End Using


    End Sub


    Sub clear_c_det()

        Dim dat1 As DateTime = DateTime.ParseExact(Session("date"), "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "select acno from masterc where masterc.parent=@parent"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@parent", Session("acno"))
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If reader.HasRows Then
                        Do While reader.Read

                            clear_masterc(reader(0).ToString)
                            clear_transc(reader(0).ToString, reformattedto)
                            clear_supplementc(reader(0).ToString, reformattedto)
                            clear_stdins(reader(0).ToString, reformattedto)
                            clear_nominee(reader(0).ToString, reformattedto)


                        Loop
                    End If

                End Using
            End Using
            con.Close()

        End Using

    End Sub
    Private Sub btn_del_Click(sender As Object, e As EventArgs) Handles btn_del.Click

        If CType(Session("Date"), Date).Equals(Date.Today) Then
            '     btn_delete.Visible = False

            Dim dat1 As DateTime = DateTime.ParseExact(Session("date"), "dd-MM-yyyy", CultureInfo.InvariantCulture)
            Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand

                    query = " delete from master WHERE master.acno = @acn AND CONVERT(VARCHAR(20), date, 112) = @dt"
                    cmd.Connection = con
                    cmd.CommandText = query

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Parameters.AddWithValue("@dt", reformattedto)
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    End Try

                End Using
                con.Close()

            End Using

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand

                    query = " delete from actrans WHERE acno = @acn AND CONVERT(VARCHAR(20), date, 112) = @dt"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Parameters.AddWithValue("@dt", reformattedto)
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally



                    End Try
                End Using
                con.Close()

            End Using


            clear_c_det()

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand

                    query = " delete from trans WHERE trans.acno = @acn AND CONVERT(VARCHAR(20), date, 112) = @dt"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Parameters.AddWithValue("@dt", reformattedto)
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con.Close()
                    End Try
                End Using
                con.Close()

            End Using




            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand


                    query = " delete from suplement WHERE acn = @acn AND CONVERT(VARCHAR(20), date, 112) = @dt"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Parameters.AddWithValue("@dt", reformattedto)
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con.Close()

                    End Try

                End Using
                con.Close()

            End Using


            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand

                    query = " delete from diff WHERE acno = @acn AND CONVERT(VARCHAR(20), date, 112) = @dt"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Parameters.AddWithValue("@dt", reformattedto)
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con.Close()

                    End Try
                End Using
                con.Close()

            End Using

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand

                    query = " delete from jlspec WHERE acno = @acn "
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))

                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con.Close()

                    End Try

                End Using
                con.Close()

            End Using

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand

                    query = " delete from jlstock WHERE acn = @acn"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con.Close()

                    End Try
                End Using
                con.Close()

            End Using

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand

                    query = " delete from dlspec WHERE acn = @acn"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con.Close()

                    End Try
                End Using
                con.Close()

            End Using

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand


                    query = " delete from stdins WHERE acno = @acn "
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con.Close()

                    End Try

                End Using
                con.Close()

            End Using

            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd = New SqlCommand


                    query = " delete from DEPOBR WHERE lacno = @acn "
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@acn", Session("acno"))
                    cmd.Connection = con
                    cmd.CommandText = query
                    Try
                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con.Close()

                    End Try
                End Using
                con.Close()

            End Using


            lbluser.Text = Session("sesusr")
            lblmsg.Text = "Account Deleted !"
            alertinfo.Visible = True

        Else
            'btn_delete.Visible = False
            sesuser.Text = Session("sesusr")
            lblinfo.Text = "Account Can't be Deleted !"
            alertmsg.Visible = True

        End If



    End Sub

    Private Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click

        Dim refurl As Object = ViewState("RefUrl")

        If Not refurl = Nothing Then
            Response.Redirect(refurl.ToString)


        End If
    End Sub

    Private Sub btnfltr_Click(sender As Object, e As EventArgs) Handles btnfltr.Click

        Session("frmdt") = txtfrm.Text
        Session("todt") = txtto.Text
        Session("flt") = True
        fltr = True
        bind_grid()


    End Sub

    Function get_home()

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.CommandText = "SELECT TOP 1 branch FROM branch"

        Try
            home = cmd.ExecuteScalar


            If home Is Nothing Then
                home = "KARAVILAI"
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        Return home


    End Function

    Private Sub SoaDeposit_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        'Session("frmdt") = Nothing
        'Session("todt") = Nothing
        ' Session("flt") = False

        fltr = False

    End Sub
End Class