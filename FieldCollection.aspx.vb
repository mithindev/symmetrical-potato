Imports System.Data.SqlClient
Imports System.Globalization

Public Class FieldCollection
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Me.IsPostBack Then
            'bind_grid()
            bind_agent()
            bind_dl()


        End If
    End Sub
    Sub bind_agent()
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con
                query = "select agent from collection group by agent"
                cmd.CommandText = query
                cmd.Parameters.Clear()

                Using dr As SqlDataReader = cmd.ExecuteReader()

                    If dr.HasRows Then
                        Do While dr.Read()

                            agent.Items.Add(Trim(dr(0)))

                        Loop


                    End If

                    agent.Items.Insert(0, "<-- Select -->")



                End Using

            End Using



        End Using





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
        End If

    End Sub
    Protected Sub OnSelectedIndexChanged_gv_grp(ByVal sender As Object, ByVal e As EventArgs) Handles disp.SelectedIndexChanged

        For Each row As GridViewRow In disp.Rows
            If row.RowIndex = disp.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#000")
                row.ToolTip = String.Empty
                ' txtlimit.Text = disp.SelectedRow.Cells(1).Text 'row.Cells.Item(0).Text
                '     Dim dt As System.Web.UI.WebControls.Image = CType(gv_loan.SelectedRow.Cells(1).FindControl("imgselct"), System.Web.UI.WebControls.Image)
                Dim acn As Label = CType(disp.SelectedRow.Cells(0).FindControl("lblacno"), Label)
                Dim lblid As Label = CType(disp.SelectedRow.Cells(0).FindControl("lblid"), Label)

                Session("id") = lblid.Text
                pnldel.Visible = True
                txtacn.Text = acn.Text



                ' Dim by_cas As Label = CType(disp.SelectedRow.Cells(0).FindControl("LBLbycas"), Label)
                ' get_data(dt.Text)
                '   dt.Visible = True
                ''     get_ac_info(Trim(acn.Text))
                ''    Me.popupdep.Show()

                'btn_post.Visible = False
                'btn_post_clr.Visible = False
                'int_revised.Visible = True

                'int_rev_acn.Text = acn.Text
                'int_rev_amt.Text = amt.Text

                'If txtproduct.Text = "KMK" Or txtproduct.Text = "SB" Or txtproduct.Text = "ML" Or txtproduct.Text = "DL" Or txtproduct.Text = "DCL" Or txtproduct.Text = "FD" Or txtproduct.Text = "RID" Then



                '    byc_rev.Visible = True
                '    lbl_bycas.Visible = True
                '    byc_caption.Visible = True
                '    byc_rev.Text = by_cas.Text
                'Else
                '    byc_rev.Visible = False
                '    lbl_bycas.Visible = False
                '    byc_caption.Visible = False
                'End If

                'ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", int_rev_amt.ClientID), True)
                ''  txtfocus(int_rev_amt)

            Else
                'row.BackColor = Drawing.ColorTranslator.FromHtml("#f0f4f5")
                row.ToolTip = "Click to select this row."
                'Dim dt As System.Web.UI.WebControls.Image = CType(gv_loan.SelectedRow.Cells(0).FindControl("imgselct"), System.Web.UI.WebControls.Image)
                ' get_data(dt.Text)
                'dt.Visible = False

            End If
        Next
    End Sub


    Sub bind_dl()
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()

            Dim qry = " SELECT  collection.product , collection.acno ,member.FirstName As name ,sum(collection.credit) as credit FROM dbo.collection "
            qry += " INNER Join dbo.master   ON collection.acno = master.acno INNER JOIN dbo.member   ON master.cid = member.MemberNo "
            qry += " WHERE collection.product ='DL' and collection.trf= 0  GROUP BY collection.product,collection.acno,FirstName  "


            Dim ds As New DataSet

            Using adapter As New SqlDataAdapter(qry, con)
                adapter.SelectCommand.Parameters.AddWithValue("@Prod", ddprod.SelectedItem.Text)



                adapter.Fill(ds)

            End Using

            dispdl.DataSource = ds
            dispdl.DataBind()

            If Not ds.Tables(0).Rows.Count = 0 Then
                lbldl.Text = FormatCurrency(ds.Tables(0).Compute("sum(credit)", ""))
            Else
                lbldl.Text = FormatCurrency(0)

            End If





        End Using
    End Sub

    Sub bind_tgrid()

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()

            Dim qry = "SELECT  collection.product ,collection.acno ,member.FirstName as name ,collection.credit FROM dbo.collection "
            qry += " INNER JOIN dbo.master   ON collection.acno = master.acno INNER JOIN dbo.member   ON master.cid = member.MemberNo "
            qry += " WHERE collection.product = @prod and collection.trf= 0 "


            Dim ds As New DataSet

            Using adapter As New SqlDataAdapter(qry, con)
                adapter.SelectCommand.Parameters.AddWithValue("@Prod", ddprod.SelectedItem.Text)



                adapter.Fill(ds)

            End Using

            dispt.DataSource = ds
            dispt.DataBind()

            If Not ds.Tables(0).Rows.Count = 0 Then
                lblttotal.Text = FormatCurrency(ds.Tables(0).Compute("sum(credit)", ""))
            Else
                lblttotal.Text = FormatCurrency(0)

            End If





        End Using

    End Sub
    Sub bind_grid()
        If txtdate.Text = "" Then Exit Sub

        Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()

            Dim qry = "SELECT collection.id, collection.product ,collection.acno ,member.FirstName ,COALESCE(collection.credit,0) as credit FROM dbo.collection "
            qry += " INNER JOIN dbo.master   ON collection.acno = master.acno INNER JOIN dbo.member   ON master.cid = member.MemberNo "
            qry += " WHERE CONVERT(VARCHAR(20), collection.date, 112) = @date and collection.agent=@agent "


            Dim ds As New DataSet

            Using adapter As New SqlDataAdapter(qry, con)
                adapter.SelectCommand.Parameters.AddWithValue("@date", reformatted)
                adapter.SelectCommand.Parameters.AddWithValue("@agent", agent.SelectedItem.Text)


                adapter.Fill(ds)
            End Using

            disp.DataSource = ds
            disp.DataBind()
            Dim total As Double = IIf(IsDBNull(ds.Tables(0).Compute("sum(credit)", "")), 0, ds.Tables(0).Compute("sum(credit)", ""))

            lbltotal.Text = FormatCurrency(total)



        End Using

    End Sub

    Private Sub FieldCollection_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub agent_TextChanged(sender As Object, e As EventArgs) Handles agent.TextChanged
        bind_grid()

    End Sub

    Private Sub ddprod_TextChanged(sender As Object, e As EventArgs) Handles ddprod.TextChanged
        bind_tgrid()
    End Sub

    Function get_pro(ByVal prd As String)

        Dim prdname As String
        Dim query As String = String.Empty
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con
                query = "SELECT name from products where products.shortname='" + prd + "'"
                cmd.CommandText = query
                prdname = cmd.ExecuteScalar()

            End Using

            con.Close()


        End Using




        Return prdname
    End Function
    Private Sub btntrf_Click(sender As Object, e As EventArgs) Handles btntrf.Click

        If Session("sesusr") = Nothing Then Exit Sub
        Session("prod") = get_pro(ddprod.SelectedItem.Text)
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con


                query = "insert into trans(date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                query &= "values(@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                cmd.Parameters.AddWithValue("@acno", "")
                cmd.Parameters.AddWithValue("@drd", 0)
                cmd.Parameters.AddWithValue("@drc", 0)
                cmd.Parameters.AddWithValue("@crd", CDbl(lblttotal.Text))
                cmd.Parameters.AddWithValue("@crc", CDbl(lblttotal.Text))
                cmd.Parameters.AddWithValue("@Narration", "By CASH")
                cmd.Parameters.AddWithValue("@due", "TRANS")
                cmd.Parameters.AddWithValue("@type", "CASH")
                cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                cmd.Parameters.AddWithValue("@entryat", Date.Now)
                cmd.ExecuteNonQuery()


            End Using
        End Using

        Dim countresult As String


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                cmd.Connection = con

                cmd.Parameters.Clear()
                query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + Trim(Session("prod")) + "'"
                cmd.CommandText = query


                Try
                    countresult = cmd.ExecuteScalar()

                    Session("tid") = Convert.ToString(countresult)
                Catch ex As Exception

                    Response.Write(ex.ToString)

                Finally
                    con.Close()
                    cmd.Dispose()

                End Try

            End Using
        End Using


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                Try

                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txttdate.Text))
                    cmd.Parameters.AddWithValue("@transid", Session("tid"))
                    cmd.Parameters.AddWithValue("@achead", Session("prod"))
                    cmd.Parameters.AddWithValue("@debit", 0)
                    cmd.Parameters.AddWithValue("@credit", CDbl(lblttotal.Text))
                    cmd.Parameters.AddWithValue("@acn", "")
                    cmd.Parameters.AddWithValue("@nar", "BY CASH")
                    cmd.Parameters.AddWithValue("@typ", "RECEIPT")

                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try

            End Using

        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                Try

                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txttdate.Text))
                    cmd.Parameters.AddWithValue("@transid", Session("tid"))
                    cmd.Parameters.AddWithValue("@achead", Session("prod"))
                    cmd.Parameters.AddWithValue("@debit", 0)
                    cmd.Parameters.AddWithValue("@credit", CDbl(lblttotal.Text))
                    cmd.Parameters.AddWithValue("@acn", "")
                    cmd.Parameters.AddWithValue("@nar", "BY CASH")
                    cmd.Parameters.AddWithValue("@typ", "RECEIPT")

                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try

            End Using

        End Using

        Dim nod

        For i As Integer = 0 To dispt.Rows.Count - 1


            Dim prod As Label = DirectCast(dispt.Rows(i).Cells(0).FindControl("lblprd"), Label)
            Dim acno As Label = DirectCast(dispt.Rows(i).Cells(0).FindControl("lblacno"), Label)
            Dim cr As Label = DirectCast(dispt.Rows(i).Cells(0).FindControl("lblcr"), Label)


            If prod.Text = "DS" Then
                Session("amt") = get_ds(acno.Text)

                If Not CDbl(cr.Text) = 0 Then
                    ' Session("amt") = CInt(cr.Text) 'CDbl(txtamt.Text) / CDbl(txtnod.Text)
                    nod = CDbl(cr.Text) / CInt(Session("amt"))
                Else
                    Session("amt") = cr.Text
                    nod = 1
                End If



                Dim due As String
                Dim d As String = get_collection(acno.Text)

                For x = 1 To nod Step 1

                    due = "Collection :" + CStr((d + x))


                    Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                        con.Open()
                        Using cmd As New SqlCommand
                            cmd.Connection = con




                            query = "insert into actrans(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                            query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                            cmd.CommandText = query
                            cmd.Parameters.Clear()
                            cmd.Parameters.AddWithValue("@id", Session("tid"))
                            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                            cmd.Parameters.AddWithValue("@acno", acno.Text)
                            cmd.Parameters.AddWithValue("@drd", 0)
                            cmd.Parameters.AddWithValue("@drc", 0)
                            cmd.Parameters.AddWithValue("@crd", CDbl(Session("amt")))
                            cmd.Parameters.AddWithValue("@crc", CDbl(Session("amt")))
                            cmd.Parameters.AddWithValue("@Narration", "By CASH")
                            cmd.Parameters.AddWithValue("@due", due)
                            cmd.Parameters.AddWithValue("@type", "CASH")
                            cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                            cmd.Parameters.AddWithValue("@entryat", Date.Now)
                            cmd.ExecuteNonQuery()


                        End Using
                    End Using

                    Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                        con.Open()
                        Using cmd As New SqlCommand
                            cmd.Connection = con




                            query = "insert into actransc(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                            query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                            cmd.CommandText = query
                            cmd.Parameters.Clear()
                            cmd.Parameters.AddWithValue("@id", Session("tid"))
                            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                            cmd.Parameters.AddWithValue("@acno", acno.Text)
                            cmd.Parameters.AddWithValue("@drd", 0)
                            cmd.Parameters.AddWithValue("@drc", 0)
                            cmd.Parameters.AddWithValue("@crd", CDbl(Session("amt")))
                            cmd.Parameters.AddWithValue("@crc", CDbl(Session("amt")))
                            cmd.Parameters.AddWithValue("@Narration", "By CASH")
                            cmd.Parameters.AddWithValue("@due", due)
                            cmd.Parameters.AddWithValue("@type", "CASH")
                            cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                            cmd.Parameters.AddWithValue("@entryat", Date.Now)
                            cmd.ExecuteNonQuery()


                        End Using
                    End Using
                Next

            ElseIf prod.Text = "DCL" Then

                chk4int(acno.Text, "DAILY COLLECTION LOAN", "DCL INTEREST")
                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand
                        cmd.Connection = con




                        query = "insert into actrans(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                        query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                        cmd.CommandText = query
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@id", Session("tid"))
                        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                        cmd.Parameters.AddWithValue("@acno", acno.Text)
                        cmd.Parameters.AddWithValue("@drd", 0)
                        cmd.Parameters.AddWithValue("@drc", 0)
                        cmd.Parameters.AddWithValue("@crd", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@crc", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@Narration", "By CASH")
                        cmd.Parameters.AddWithValue("@due", "")
                        cmd.Parameters.AddWithValue("@type", "CASH")
                        cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                        cmd.Parameters.AddWithValue("@entryat", Date.Now)
                        cmd.ExecuteNonQuery()


                    End Using
                End Using

                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand
                        cmd.Connection = con




                        query = "insert into actransc(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                        query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                        cmd.CommandText = query
                        cmd.Parameters.Clear()

                        cmd.Parameters.AddWithValue("@id", Session("tid"))
                        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                        cmd.Parameters.AddWithValue("@acno", acno.Text)
                        cmd.Parameters.AddWithValue("@drd", 0)
                        cmd.Parameters.AddWithValue("@drc", 0)
                        cmd.Parameters.AddWithValue("@crd", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@crc", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@Narration", "By CASH")
                        cmd.Parameters.AddWithValue("@due", "")
                        cmd.Parameters.AddWithValue("@type", "CASH")
                        cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                        cmd.Parameters.AddWithValue("@entryat", Date.Now)
                        cmd.ExecuteNonQuery()


                    End Using
                End Using
            Else
                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand
                        cmd.Connection = con




                        query = "insert into actrans(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                        query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                        cmd.CommandText = query
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@id", Session("tid"))
                        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                        cmd.Parameters.AddWithValue("@acno", acno.Text)
                        cmd.Parameters.AddWithValue("@drd", 0)
                        cmd.Parameters.AddWithValue("@drc", 0)
                        cmd.Parameters.AddWithValue("@crd", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@crc", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@Narration", "By CASH")
                        cmd.Parameters.AddWithValue("@due", "")
                        cmd.Parameters.AddWithValue("@type", "CASH")
                        cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                        cmd.Parameters.AddWithValue("@entryat", Date.Now)
                        cmd.ExecuteNonQuery()


                    End Using
                End Using

                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand
                        cmd.Connection = con




                        query = "insert into actransc(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                        query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                        cmd.CommandText = query
                        cmd.Parameters.Clear()

                        cmd.Parameters.AddWithValue("@id", Session("tid"))
                        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                        cmd.Parameters.AddWithValue("@acno", acno.Text)
                        cmd.Parameters.AddWithValue("@drd", 0)
                        cmd.Parameters.AddWithValue("@drc", 0)
                        cmd.Parameters.AddWithValue("@crd", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@crc", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@Narration", "By CASH")
                        cmd.Parameters.AddWithValue("@due", "")
                        cmd.Parameters.AddWithValue("@type", "CASH")
                        cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                        cmd.Parameters.AddWithValue("@entryat", Date.Now)
                        cmd.ExecuteNonQuery()


                    End Using
                End Using
            End If


            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd As New SqlCommand
                    cmd.Connection = con
                    query = "UPDATE collection SET TRF = @trf,tid=@tid,trfon=@trfon WHERE collection.acno=@acno and trf=0"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@trf", 1)
                    cmd.Parameters.AddWithValue("@acno", acno.Text)
                    cmd.Parameters.AddWithValue("@tid", Session("tid"))
                    cmd.Parameters.AddWithValue("@trfon", Convert.ToDateTime(txttdate.Text))
                    cmd.CommandText = query
                    cmd.ExecuteNonQuery()

                End Using
            End Using


        Next

        bind_tgrid()


        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transaction Completed. Id # <b>" + Session("tid") + "</b>"
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)


    End Sub

    Sub chk4int(ByVal acn As String, ByVal prod As String, ByVal ach As String)

        prv_int(acn)
        get_balance(acn)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con

                query = "select date,cint,dint from master where acno='" + acn + "'"
                cmd.CommandText = query
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If reader.HasRows Then
                        reader.Read()
                        Session("ac_date") = reader(0)
                        Session("cint") = reader(1)
                        Session("dint") = reader(2)

                        If prod = "DAILY COLLECTION LOAN" Then
                            Session("total_prd") = DateDiff(DateInterval.Day, Session("ac_date"), CDate(txttdate.Text))

                            If Session("total_prd") > 120 Then
                                Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(txttdate.Text))
                                Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("prv_dinton"), CDate(txttdate.Text))
                                If Session("prd_buffer") > 120 Then
                                    Session("prd_buffer") = Session("prd_buffer") - 120

                                End If

                                If Session("prd_buffer_d") > 120 Then
                                    Session("prd_buffer_d") = Session("prd_buffer_d") - 120

                                End If

                                Session("totint") = Math.Round((((-Session("cbal")) * Session("cint") / 100) / 365) * Session("prd_buffer"))
                                Session("totalint_d") = Math.Round((((-Session("dbal")) * Session("dint") / 100) / 365) * Session("prd_buffer_d"))
                                update_int(acn, Session("totint"), Session("totalint_d"), prod, ach)
                            End If

                        Else
                            Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(txttdate.Text))
                            Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("prv_dinton"), CDate(txttdate.Text))

                            Session("totint") = Math.Round((((-Session("cbal")) * Session("cint") / 100) / 365) * Session("prd_buffer"))
                            Session("totalint_d") = Math.Round((((-Session("dbal")) * Session("dint") / 100) / 365) * Session("prd_buffer_d"))
                            update_int(acn, Session("totint"), Session("totalint_d"), prod, ach)
                        End If

                    End If


                End Using
            End Using
        End Using


    End Sub

    Sub update_int(ByVal acn As String, ByVal intc As Decimal, ByVal intd As Decimal, ByVal prod As String, ByVal ach As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con




                query = "insert into actrans(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@id", Session("tid"))
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                cmd.Parameters.AddWithValue("@acno", acn)
                cmd.Parameters.AddWithValue("@crd", 0)
                cmd.Parameters.AddWithValue("@crc", 0)
                cmd.Parameters.AddWithValue("@drd", intd)
                cmd.Parameters.AddWithValue("@drc", intc)
                cmd.Parameters.AddWithValue("@Narration", "To Interest")
                cmd.Parameters.AddWithValue("@due", "upto " + txtdate.Text)
                cmd.Parameters.AddWithValue("@type", "INTR")
                cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                cmd.Parameters.AddWithValue("@entryat", Date.Now)
                cmd.ExecuteNonQuery()


            End Using
        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con




                query = "insert into actransc(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                cmd.CommandText = query
                cmd.Parameters.Clear()

                cmd.Parameters.AddWithValue("@id", Session("tid"))
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                cmd.Parameters.AddWithValue("@acno", acn)
                cmd.Parameters.AddWithValue("@crd", 0)
                cmd.Parameters.AddWithValue("@crc", 0)
                cmd.Parameters.AddWithValue("@drd", intd)
                cmd.Parameters.AddWithValue("@drc", intc)
                cmd.Parameters.AddWithValue("@Narration", "To Interest  ")
                cmd.Parameters.AddWithValue("@due", "upto  " + txttdate.Text)
                cmd.Parameters.AddWithValue("@type", "INTR")
                cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                cmd.Parameters.AddWithValue("@entryat", Date.Now)
                cmd.ExecuteNonQuery()


            End Using
        End Using


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                Try

                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txttdate.Text))
                    cmd.Parameters.AddWithValue("@transid", Session("tid"))
                    cmd.Parameters.AddWithValue("@achead", ach)
                    cmd.Parameters.AddWithValue("@debit", 0)
                    cmd.Parameters.AddWithValue("@credit", intc)
                    cmd.Parameters.AddWithValue("@acn", acn)
                    cmd.Parameters.AddWithValue("@nar", "By Interest   " + acn)
                    cmd.Parameters.AddWithValue("@typ", "JOURNAL")

                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try

            End Using

        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                Try

                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txttdate.Text))
                    cmd.Parameters.AddWithValue("@transid", Session("tid"))
                    cmd.Parameters.AddWithValue("@achead", prod)
                    cmd.Parameters.AddWithValue("@debit", intc)
                    cmd.Parameters.AddWithValue("@credit", 0)
                    cmd.Parameters.AddWithValue("@acn", acn)
                    cmd.Parameters.AddWithValue("@nar", "To Interest   " + acn)
                    cmd.Parameters.AddWithValue("@typ", "JOURNAL")

                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try

            End Using

        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                Try

                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txttdate.Text))
                    cmd.Parameters.AddWithValue("@transid", Session("tid"))
                    cmd.Parameters.AddWithValue("@achead", "DCL INTEREST")
                    cmd.Parameters.AddWithValue("@debit", 0)
                    cmd.Parameters.AddWithValue("@credit", intc)
                    cmd.Parameters.AddWithValue("@acn", acn)
                    cmd.Parameters.AddWithValue("@nar", "By Interest  " + acn)
                    cmd.Parameters.AddWithValue("@typ", "JOURNAL")

                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try

            End Using

        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                Try

                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txttdate.Text))
                    cmd.Parameters.AddWithValue("@transid", Session("tid"))
                    cmd.Parameters.AddWithValue("@achead", "DAILY COLLECTION LOAN")
                    cmd.Parameters.AddWithValue("@debit", intc)
                    cmd.Parameters.AddWithValue("@credit", 0)
                    cmd.Parameters.AddWithValue("@acn", acn)
                    cmd.Parameters.AddWithValue("@nar", "To Interest   " + acn)
                    cmd.Parameters.AddWithValue("@typ", "JOURNAL")

                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try

            End Using

        End Using


    End Sub

    Public Sub get_balance(ByVal acn As String)


        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' GROUP BY [actrans].acno"
        ' Dim sql As String = "select top 1 cbal,dbal from dbo.[actrans] where [actrans].acno='" + acn + "' ORDER BY date DESC"
        Dim ds_bal As New DataSet

        '        Dim Adapter As New SqlDataAdapter(sql, con)
        '       Adapter.Fill(ds_bal)
        cmd.Connection = con
        cmd.CommandText = sql

        Try

            Dim reader1 As SqlDataReader = cmd.ExecuteReader()


            If reader1.HasRows Then
                reader1.Read()
                Session("dbal") = (reader1(1).ToString - reader1(0).ToString)
                Session("cbal") = (reader1(3).ToString - reader1(2).ToString)


                reader1.Close()


            Else
                Session("cbal") = 0
                Session("dbal") = 0
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally

        End Try



    End Sub
    Sub prv_int(ByVal acn As String)
        Dim oresult As Date

        If con.State = ConnectionState.Closed Then con.Open()
        cmdi.Connection = con
        cmdi.Parameters.Clear()

        query = "SELECT TOP 1 actrans.date FROM dbo.actrans WHERE actrans.Drd > 0 AND actrans.Type = 'INTR' AND actrans.acno = @acn ORDER BY actrans.date DESC"
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@acn", acn)

        Try
            oresult = cmdi.ExecuteScalar()
            If oresult = Date.MinValue Then
                Session("prv_dinton") = Session("ac_date")
            Else
                Session("prv_dinton") = oresult

            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmdi.Dispose()
            con.Close()

        End Try


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()

        query = "SELECT TOP 1 actrans.date FROM dbo.actrans WHERE actrans.Drc > 0 AND actrans.Type = 'INTR' AND actrans.acno = @acn ORDER BY actrans.date DESC"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@acn", acn)

        Try
            oresult = cmd.ExecuteScalar()
            If oresult = Date.MinValue Then
                Session("prv_inton") = Session("ac_date")

            Else

                Session("prv_inton") = oresult

            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()

        End Try



    End Sub

    Function get_ds(ByVal acn As String)
        Dim amt = 0

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con

                Dim sql As String = "SELECT COALESCE(amount,0) from master where master.acno=@acno"

                cmd.Connection = con
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", acn)
                amt = cmd.ExecuteScalar()
            End Using
        End Using



        Return amt

    End Function


    Function get_collection(ByVal acn As String)

        Dim countresult


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con

                Dim sql As String = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].type<>'INTR' "

                cmd.Connection = con
                cmd.CommandText = sql
                countresult = cmd.ExecuteScalar()
            End Using
        End Using



        Return countresult


    End Function

    Private Sub btn_del_Click(sender As Object, e As EventArgs) Handles btn_del.Click

        If con.State = ConnectionState.Closed Then con.Open()
        query = " delete from collection WHERE collection.id = @tid "
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@tid", Session("id"))

        cmd.Connection = con
        cmd.CommandText = query
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con.Close()
            cmd.Dispose()

        End Try
        pnldel.Visible = False
        bind_grid()


    End Sub

    Private Sub txtdate_TextChanged(sender As Object, e As EventArgs) Handles txtdate.TextChanged
        If Not agent.SelectedIndex = 0 Then
            bind_grid()

        End If
    End Sub

    Private Sub btn_dltrf_Click(sender As Object, e As EventArgs) Handles btn_dltrf.Click
        If Session("sesusr") = Nothing Then Exit Sub
        Session("prod") = get_pro("DL")
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con


                query = "insert into trans(date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                query &= "values(@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtddate.Text))
                cmd.Parameters.AddWithValue("@acno", "")
                cmd.Parameters.AddWithValue("@drd", 0)
                cmd.Parameters.AddWithValue("@drc", 0)
                cmd.Parameters.AddWithValue("@crd", CDbl(lbldl.Text))
                cmd.Parameters.AddWithValue("@crc", CDbl(lbldl.Text))
                cmd.Parameters.AddWithValue("@Narration", "By CASH")
                cmd.Parameters.AddWithValue("@due", "TRANS")
                cmd.Parameters.AddWithValue("@type", "CASH")
                cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                cmd.Parameters.AddWithValue("@entryat", Date.Now)
                cmd.ExecuteNonQuery()


            End Using
        End Using

        Dim countresult As String


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                cmd.Connection = con

                cmd.Parameters.Clear()
                query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + Trim(Session("prod")) + "'"
                cmd.CommandText = query


                Try
                    countresult = cmd.ExecuteScalar()

                    Session("tid") = Convert.ToString(countresult)
                Catch ex As Exception

                    Response.Write(ex.ToString)

                Finally
                    con.Close()
                    cmd.Dispose()

                End Try

            End Using
        End Using


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                Try

                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtddate.Text))
                    cmd.Parameters.AddWithValue("@transid", Session("tid"))
                    cmd.Parameters.AddWithValue("@achead", Session("prod"))
                    cmd.Parameters.AddWithValue("@debit", 0)
                    cmd.Parameters.AddWithValue("@credit", CDbl(lbldl.Text))
                    cmd.Parameters.AddWithValue("@acn", "")
                    cmd.Parameters.AddWithValue("@nar", "BY CASH")
                    cmd.Parameters.AddWithValue("@typ", "RECEIPT")

                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try

            End Using

        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd As New SqlCommand

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
                query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
                Try

                    cmd.CommandText = query
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(txtddate.Text))
                    cmd.Parameters.AddWithValue("@transid", Session("tid"))
                    cmd.Parameters.AddWithValue("@achead", Session("prod"))
                    cmd.Parameters.AddWithValue("@debit", 0)
                    cmd.Parameters.AddWithValue("@credit", CDbl(lbldl.Text))
                    cmd.Parameters.AddWithValue("@acn", "")
                    cmd.Parameters.AddWithValue("@nar", "BY CASH")
                    cmd.Parameters.AddWithValue("@typ", "RECEIPT")

                    cmd.ExecuteNonQuery()
                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try

            End Using

        End Using



        For i As Integer = 0 To dispdl.Rows.Count - 1


            Dim prod As Label = DirectCast(dispdl.Rows(i).Cells(0).FindControl("lblprd"), Label)
            Dim acno As Label = DirectCast(dispdl.Rows(i).Cells(0).FindControl("lblacno"), Label)
            Dim cr As Label = DirectCast(dispdl.Rows(i).Cells(0).FindControl("lblcr"), Label)


            chk4int(acno.Text, "DEPOSIT LOAN", "DL INTEREST")
            Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand
                        cmd.Connection = con


                    query = "insert into actrans(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                        query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                        cmd.CommandText = query
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@id", Session("tid"))
                        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                        cmd.Parameters.AddWithValue("@acno", acno.Text)
                        cmd.Parameters.AddWithValue("@drd", 0)
                        cmd.Parameters.AddWithValue("@drc", 0)
                        cmd.Parameters.AddWithValue("@crd", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@crc", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@Narration", "By CASH")
                        cmd.Parameters.AddWithValue("@due", "")
                        cmd.Parameters.AddWithValue("@type", "CASH")
                        cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                        cmd.Parameters.AddWithValue("@entryat", Date.Now)
                        cmd.ExecuteNonQuery()


                    End Using
                End Using

                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand
                        cmd.Connection = con


                    query = "insert into actransc(id,date,acno,drd,drc,crd,crc,Narration,due,type,suplimentry,sesusr,entryat)"
                        query &= "values(@id,@date,@acno,@drd,@drc,@crd,@crc,@Narration,@due,@type,@suplimentry,@sesusr,@entryat)"

                        cmd.CommandText = query
                        cmd.Parameters.Clear()

                        cmd.Parameters.AddWithValue("@id", Session("tid"))
                        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txttdate.Text))
                        cmd.Parameters.AddWithValue("@acno", acno.Text)
                        cmd.Parameters.AddWithValue("@drd", 0)
                        cmd.Parameters.AddWithValue("@drc", 0)
                        cmd.Parameters.AddWithValue("@crd", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@crc", CDbl(cr.Text))
                        cmd.Parameters.AddWithValue("@Narration", "By CASH")
                        cmd.Parameters.AddWithValue("@due", "")
                        cmd.Parameters.AddWithValue("@type", "CASH")
                        cmd.Parameters.AddWithValue("@suplimentry", Session("prod"))
                        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                        cmd.Parameters.AddWithValue("@entryat", Date.Now)
                        cmd.ExecuteNonQuery()


                    End Using
                End Using


                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                con.Open()
                Using cmd As New SqlCommand
                    cmd.Connection = con
                    query = "UPDATE collection SET TRF = @trf,tid=@tid,trfon=@trfon WHERE collection.acno=@acno and trf=0"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@trf", 1)
                    cmd.Parameters.AddWithValue("@acno", acno.Text)
                    cmd.Parameters.AddWithValue("@tid", Session("tid"))
                    cmd.Parameters.AddWithValue("@trfon", Convert.ToDateTime(txttdate.Text))
                    cmd.CommandText = query
                    cmd.ExecuteNonQuery()

                End Using
            End Using


        Next

        bind_dl()


        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Transaction Completed. Id # <b>" + Session("tid") + "</b>"
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

    End Sub
End Class