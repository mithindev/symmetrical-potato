Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Drawing

Public Class SoaLoan
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim dtdl As DataTable
    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim newrow As DataRow
    Dim yearpart As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Me.IsPostBack Then

            Dim param1 As String = Request.QueryString("acno")

            If Not param1 = "" Then
                Session("soaprint") = param1
                get_acc_details(param1)
                ViewState("RefUrl") = Request.UrlReferrer.ToString()
                bind_grid()
                LoadJewelImages(param1)
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

    Sub showjltab(ByVal acn As String)

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select itm,qty,gross,net from jlspec where jlspec.acno='" + acn + "'"
        'cmd.Connection = con
        'cmd.CommandText = query

        Dim ds As New DataSet
        Dim adapter As New SqlDataAdapter(query, con)
        adapter.Fill(ds)
        dispjl.DataSource = ds
        dispjl.DataBind()

        adapter.Dispose()

        query = "select tqty,tgross,tnet from jlstock where jlstock.acn='" + acn + "'"
        cmd.Connection = con
        cmd.CommandText = query
        Dim dr As SqlDataReader

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()

                lbltqty.Text = IIf(IsDBNull(dr(0)), 0, dr(0))
                lblgross.Text = Format(IIf(IsDBNull(dr(1)), 0, dr(1)), "##,##0.000")
                lblnet.Text = Format(IIf(IsDBNull(dr(2)), 0, dr(2)), "##,##0.000")
            End If
            dr.Close()

            Dim gr As Double = get_goldrate()

            Dim rpg As Double = Math.Round(CDbl(lblbal.Text) / CDbl(lblnet.Text))

            lblrpg.Text = FormatCurrency(rpg)

            If rpg > gr Then
                lblrpg.CssClass = "text-danger"
            Else
                lblrpg.CssClass = "text-success"
            End If



        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

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

                query = "SELECT COALESCE(SUM(actrans.Crd),0) AS bal FROM dbo.actrans WHERE actrans.acno = @acno and Type ='INTR' "
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

    Function get_last_transaction(ByVal acn As String)
        Dim oResult As Date

        If con.State = ConnectionState.Closed Then con.Open()


        cmdx.Connection = con
        cmdx.CommandText = "SELECT top 1 date  FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "'ORDER BY date DESC"

        Try
            oResult = cmdx.ExecuteScalar()

            If Not oResult = Date.MinValue Then
                'Dim tdat As Date = Convert.ToDateTime(tdate.Text)
                Session("prv_inton") = oResult
            Else
                Session("prv_inton") = Session("ac_date")

            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        Return Session("prv_inton")

    End Function

    Function get_bal(ByVal acno As String)
        Dim bal As Double = 0

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT COALESCE(SUM(actrans.Drd) - SUM(actrans.Crd),0) AS bal FROM dbo.actrans WHERE actrans.acno = @acno"
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


    Sub get_acc_details(ByVal acno As String)

        jltab.Visible = False
        dltab.Visible = False


        get_soa_details(acno)
        If IsNothing(Session("acno")) Then
            Response.Redirect("/soa.aspx?typ=l")
        End If



        If session_user_role = "Audit" Then
            shwtab.Visible = False
            pnldint.Visible = False
        Else
            shwtab.Visible = True
            pnldint.Visible = True
        End If

        lbldat.Text = Session("Date")

        lbllastrcpt.Text = CDate(get_last_transaction(acno))
        lblacn.Text = Session("acno")
        lblacno.Text = Session("acno")
        lblcid.Text = Session("cid")
        lblprod.Text = Session("Product")

        lblfc.Text = IIf(Session("fc") = 0, "No", "Yes")
        fc.Checked = IIf(Session("fc") = 0, False, True)
        If Not Session("agent") = "" Then
            ddagent.SelectedItem.Text = Session("agent")
        End If
        lblagent.Text = Session("agent")
        lblprd.Text = DateDiff(DateInterval.Day, CDate(lbllastrcpt.Text), Date.Today).ToString + " Days"



        lblamt.Text = FormatCurrency(Session("amount"))
        lblcroi.Text = Session("CInt")
        lblroi.Text = Session("dint")
        lblbal.Text = FormatCurrency(get_bal(Session("acno")))
        ''lblintpaid.Text = FormatCurrency(get_intpaid(Session("acno")))
        'Session("cld")

        If Session("cld") = 0 Then
            lblstus.Text = "Active"
            lblba.Attributes.Add("Class", "badge badge-outline badge-success")
        Else
            lblstus.Text = "Closed"
            lblba.Attributes.Add("Class", "badge badge-outline badge-danger")



        End If


        ' lblmdat.Text = Session("mdate")
        ' lblmamt.Text = FormatCurrency(Session("mamt"))

        txtcid.Text = Session("cid")
        '   txtdate.Text = Session("Date")
        '   txtwef.Text = Session("Date")
        '   txtmdate.Text = Session("mdate")
        '   txtpbon.Text = Session("pbi")
        lblEacn.Text = Session("acno")

        If Session("alert") = True Then
            ispb.Checked = True

        End If

        lblalert.Text = IIf(Session("alert") = True, "Yes", "No")
        'lblpbon.Text = Session("pbi")

        lblsch.Text = Session("sch")

        If Trim(Session("Product")) = "JL" Then
            showjltab(Session("acno"))
            jltab.Visible = True

        End If

        If Trim(Session("Product")) = "DL" Then
            showdltab(Session("acno"))
            dltab.Visible = True
        End If




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

                    '  lblsi.Text = cmd.ExecuteScalar

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

                query = "SELECT TOP 1 COALESCE(deposit,' ') AS sacn FROM dbo.dlspec WHERE dlspec.acn = @acno"
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



    End Sub

    Sub showdltab(ByVal acn As String)

        Dim dtdl As New DataTable


        Dim tbal As Double = 0
        If con.State = ConnectionState.Closed Then con.Open()

        query = "select deposit from dlspec where dlspec.acn='" + acn + "'"
        cmd.Connection = con
        cmd.CommandText = query

        Dim drdl As SqlDataReader
        Try
            drdl = cmd.ExecuteReader()

            If drdl.HasRows() Then
                While drdl.Read()
                    acn = IIf(IsDBNull(drdl(0)), 0, drdl(0))
                    get_dep_details(acn)
                    Session("cbal") = get_balance(acn)
                    tbal = tbal + Session("dbal")
                    If dtdl.Columns.Count = 0 Then
                        dtdl.Columns.Add("acn", GetType(String))

                        dtdl.Columns.Add("acdate", GetType(String))
                        dtdl.Columns.Add("depamt", GetType(Decimal))
                        dtdl.Columns.Add("curbal", GetType(Decimal))
                    End If


                    newrow = dtdl.NewRow

                    newrow(0) = acn
                    newrow(1) = String.Format("{0:dd-MM-yyyy}", Session("dl_date"))
                    newrow(2) = String.Format("{0:N}", CDbl(Session("dl_amt")))
                    newrow(3) = String.Format("{0:N}", CDbl(Session("dbal")))



                    dtdl.Rows.Add(newrow)













                End While
                drdl.Close()

                dispdl.DataSource = dtdl

                dispdl.DataBind()

                lblamt.Text = String.Format("{0:N}", tbal)
                ' dltab.Visible = True
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)

        End Try

    End Sub

    Sub get_dep_details(ByVal acn As String)

        If con1.State = ConnectionState.Closed Then con1.Open()

        query = "select date,amount from master where master.acno='" + acn + "'"
        cmd.Connection = con1
        cmd.CommandText = query


        Dim dr As SqlDataReader
        Try
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                dr.Read()

                Session("dl_date") = dr(0)
                Session("dl_amt") = dr(1)

            Else
                Session("dl_date") = Nothing
                Session("dl_amt") = 0
            End If

            dr.Close()

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            con1.Close()

        End Try


    End Sub
    Public Function get_balance(ByVal acn As String)

        Dim sql As String = String.Empty

        If con1.State = ConnectionState.Closed Then con1.Open()

        If session_user_role = "Audit" Then
            sql = "SELECT SUM([actransc].Drd) AS expr1,SUM([actransc].Crd) AS expr2,SUM([actransc].Drc) AS expr3,SUM([actransc].Crc) AS expr4,[actransc].acno FROM dbo.[actransc] WHERE [actransc].acno ='" + acn + "' GROUP BY [actransc].acno"
        Else
            sql = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' GROUP BY [actrans].acno"
        End If

        ' Dim sql As String = "select top 1 cbal,dbal from dbo.[actrans] where [actrans].acno='" + acn + "' ORDER BY date DESC"
        Dim ds_bal As New DataSet

        '        Dim Adapter As New SqlDataAdapter(sql, con)
        '       Adapter.Fill(ds_bal)
        cmd.Connection = con1
        cmd.CommandText = sql

        Try

            Dim reader1 As SqlDataReader = cmd.ExecuteReader()


            If reader1.HasRows Then
                reader1.Read()
                Session("dbal") = (reader1(1).ToString - reader1(0).ToString) '+ -(unclr_debit_d - unclr_credit_d)
                Session("cbal") = (reader1(3).ToString - reader1(2).ToString) '+ -(unclr_debit_c - unclr_credit_c)

                'lblccr.Text = FormatCurrency(reader1(3).ToString + unclr_credit_c)
                'lblcdr.Text = FormatCurrency(reader1(2).ToString + unclr_debit_c)
                'lblcbal.Text = FormatCurrency(cbal)

                'lbldcr.Text = FormatCurrency(reader1(1).ToString + unclr_credit_d)
                'lblddr.Text = FormatCurrency(reader1(0).ToString + unclr_debit_d)
                'lbldbal.Text = FormatCurrency(dbal)

                reader1.Close()


            Else
                Session("cbal") = 0
                Session("dbal") = 0
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            con1.Close()

        End Try

        Return Session("cbal")

    End Function

    Private Sub SoaDeposit_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub


    Private Sub bind_grid()


        Dim cbal As Double = 0
        Dim dbal As Double = 0
        Dim newrow As DataRow




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

        If session_user_role = "Audit" Then
            query = "SELECT  date,narration,due,drc,crc,cbal,tid,drd,crd FROM dbo.[actransc] WHERE actransc.acno=@acno and scroll=@scroll "
        Else
            query = "SELECT  date,narration,due,drc,crc,cbal,tid,drd,crd FROM dbo.[actrans] WHERE actrans.acno=@acno and scroll=@scroll "
        End If

        cmdi.Connection = con
        cmdi.Parameters.AddWithValue("@acno", Trim(Session("acno")))
        cmdi.Parameters.AddWithValue("@scroll", X)
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

    Private Sub LoadJewelImages(ByVal acn As String)
        phJewelImages.Controls.Clear()
        Dim con3 As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
        con3.Open()
        Dim q As String = "SELECT ImageData FROM jlspec WHERE acno = @acno AND ImageData IS NOT NULL"
        Dim cmd3 As New SqlClient.SqlCommand(q, con3)
        cmd3.Parameters.AddWithValue("@acno", Trim(acn))
        Try
            Dim dr3 As SqlDataReader = cmd3.ExecuteReader()
            Dim idx As Integer = 0
            While dr3.Read()
                Dim imgbytes As Byte() = CType(dr3.GetValue(0), Byte())
                Dim imagePath As String = String.Format("~/Captures/jl_{0}_{1}.png", Trim(acn), idx)
                File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                Dim imgUrl As String = String.Format("~/Captures/jl_{0}_{1}.png?{2}", Trim(acn), idx, DateTime.Now.Ticks.ToString())
                Dim resolvedUrl As String = ResolveUrl(imgUrl)
                Dim img As New System.Web.UI.WebControls.Image()
                img.ImageUrl = imgUrl
                img.Style("width") = "96px"
                img.Style("height") = "96px"
                img.Style("border-radius") = "8px"
                img.Style("object-fit") = "cover"
                img.Style("margin-right") = "8px"
                img.Style("cursor") = "pointer"
                img.CssClass = "img-thumbnail"
                img.Attributes.Add("onclick", String.Format("openJewelModal('{0}')", resolvedUrl))
                img.ToolTip = "Click to view larger"
                phJewelImages.Controls.Add(img)
                idx += 1
            End While
            dr3.Close()
        Catch ex As Exception
            ' silently fail — no jewel images shown on error
        Finally
            cmd3.Dispose()
            con3.Close()
        End Try
    End Sub

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


    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

        Dim agent As String
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    query = "update masterc set cid=@cid,alert=@alert,fc=@fc,agent=@agent where masterc.acno=@acno"
                Else
                    query = "update master set cid=@cid,alert=@alert,fc=@fc,agent=@agent where master.acno=@acno"
                End If


                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", Trim(lblacno.Text))
                cmd.Parameters.AddWithValue("@cid", Trim(txtcid.Text))
                cmd.Parameters.AddWithValue("@alert", IIf(ispb.Checked = True, 1, 0))
                cmd.Parameters.AddWithValue("@fc", IIf(fc.Checked = True, 1, 0))
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
                    get_acc_details(Trim(lblacno.Text))
                    bind_grid()



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



        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand

                query = " delete from dlspecc WHERE acn = @acn"
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
End Class