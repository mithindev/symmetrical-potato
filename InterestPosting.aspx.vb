Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Globalization


Public Class InterestPosting
    Inherits System.Web.UI.Page


    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd1 As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Public ach As String
    Public lod As String
    Dim countresult As Integer
    Public newrow As DataRow
    Dim oResultdate As Date
    Public dt As New DataTable
    Private WithEvents BGW As New BackgroundWorker
    Public Event DoWork As DoWorkEventHandler



    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub OnPagingsi(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        get_data_si()
        disp_si.PageIndex = e.NewPageIndex
        disp_si.DataBind()


    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        'disp4post(tdate.Text, txtproduct.Text)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.Open()

        'ScriptManager1.RegisterAsyncPostBackControl(btnSubmit)

        If Not IsPostBack Then

            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", tdate.ClientID), True)
            tdate.Text = Format(Now, "dd-MM-yyyy")
            bind_grid()



        End If
    End Sub

    Sub bind_grid()
        Dim dr As SqlDataReader

        Dim query As String = "SELECT shortname from products"

        cmd.Connection = con
        cmd.CommandText = query

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                Do While dr.Read()

                    prod.Items.Add(Trim(dr(0).ToString))
                Loop


            End If

            prod.Items.Insert(0, "<-- Select -->")
            prod.Items.Item(0).Value = ""
            dr.Close()


        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            cmd.Dispose()

            con.Close()


        End Try




    End Sub

    Sub update_fd_int()


        ' Me.ModalPopup1.Show()



        Dim rc As Integer = 0
        Dim di As New DataSet
        Dim reader As SqlDataReader
        Dim dr As SqlDataReader
        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()


        query = "SELECT tmpint.tdate,tmpint.acdate FROM dbo.tmpint WHERE MONTH(tdate)=@mp AND YEAR(tdate)=@yp AND DATEPART(dd, tdate) = @dt and dep=@dep and runson=@runson"


        cmd.Parameters.AddWithValue("@mp", Month(tdate.Text))
        cmd.Parameters.AddWithValue("@yp", Year(tdate.Text))
        cmd.Parameters.AddWithValue("@dt", txtday.Text)
        cmd.Parameters.AddWithValue("@dep", prod.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@runson", txtday.Text)
        cmd.CommandText = query
        cmd.Connection = con
        Try
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then

                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()


                Dim stitle = "Hi " + Session("sesusr")
                Dim msg = "Interest Already Updated !"
                Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

                '  ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
                btn_post.Enabled = False
                dr.Close()
                Exit Sub
            End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.Message)

        Finally
            'reader.Close()
            con.Close()
            cmd.Dispose()

        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.Parameters.Clear()

        ' query = "select date,acno from dbo.master where DATEPART(dd,date)=" + txtday.Text + "and product='" + Trim(prod.SelectedItem.Text) + "' and cld='0'"

        If inttyp.SelectedItem.Text = "Monthly" Then
            'query = "SELECT  master.date, Master.acno,Master.amount,master.cint,master.dint,master.prd,master.prdtype,master.mdate FROM dbo.master WHERE DATEPART(dd, master.date) = @dt AND master.product = @prd AND master.cld = @cld"
            query = "SELECT   master.date,  master.acno,  master.amount,  master.cint,  master.dint,  master.prd,  master.prdtype,  master.mdate FROM dbo.stdins LEFT OUTER JOIN dbo.master   ON stdins.acno = master.acno"
            query &= " WHERE stdins.sidate = @si AND stdins.srcproduct = @prd AND master.cld = @cld"

            cmd.Parameters.AddWithValue("@si", txtday.Text)
            cmd.Parameters.AddWithValue("@prd", prod.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@cld", 0)
        ElseIf inttyp.SelectedItem.Text = "Yearly" Then
            query = "SELECT  master.date, Master.acno,Master.amount,master.cint,master.dint,master.prd,master.prdtype,master.mdate FROM dbo.master WHERE  master.product = @prd AND master.cld = @cld"
            'cmd.Parameters.AddWithValue("@dt", txtday.Text)
            cmd.Parameters.AddWithValue("@prd", prod.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@cld", 0)

        End If
        cmd.CommandText = query

        Try

            reader = cmd.ExecuteReader()

            If reader.HasRows Then


                While reader.Read()
                    Session("ac_date") = reader(0)

                    If Not Convert.ToDateTime(tdate.Text).Equals(CType(Session("ac_date"), Date)) Then
                        Session("ac_date") = reader(0)
                        Session("acn") = reader(1).ToString
                        Session("amt") = reader(2).ToString
                        Session("cintr") = reader(3).ToString
                        Session("dint") = reader(4).ToString
                        Session("prd") = IIf(IsDBNull(reader(5)), 0, reader(5))
                        Session("prdtyp") = IIf(IsDBNull(reader(6)), "M", reader(6))
                        Session("mdt") = IIf(IsDBNull(reader(7)), Session("ac_date"), reader(7))

                        Dim x As Integer = Convert.ToDateTime(tdate.Text).CompareTo(CType(Session("mdt"), Date))

                        Dim ddif As Integer = DateDiff(DateInterval.Day, Convert.ToDateTime(tdate.Text), CType(Session("mdt"), Date))

                        If ddif >= 0 And ddif <= 3 Then x = 0

                        If Not x = 1 Then

                            Session("prv_inton") = get_prev_inton(Trim(Session("acn")))
                            If inttyp.SelectedItem.Text = "Yearly" Then

                                post_int_fd(Session("prv_inton"), Trim(Session("acn")), Session("amt"), Session("cintr"), Session("dint"))
                            Else
                                If Not Date.Today.Equals(Session("ac_date")) Then
                                    post_int_fd(Session("prv_inton"), Trim(Session("acn")), Session("amt"), Session("cintr"), Session("dint"))
                                End If
                            End If
                        Else

                            Dim ddiff As Integer = DateDiff(DateInterval.Day, CType(Session("mdt"), Date), Convert.ToDateTime(tdate.Text))

                            If ddiff = 1 Or runMaturedDepCheckBox.Checked Then
                                post_int_fd(Session("prv_inton"), Trim(Session("acn")), Session("amt"), Session("cintr"), Session("dint"))
                            End If

                        End If
                    End If

                End While

            End If
            '  Me.ModalPopup1.Hide()


        Catch ex As Exception
            Response.Write(ex.Message)
        End Try



    End Sub


    Function get_prev_inton_c(ByVal acn As String)

        If con1.State = ConnectionState.Closed Then con1.Open()

        Try
            cmdx.Parameters.Clear()
            cmdx.Connection = con1
            query = "SELECT top 1 date FROM dbo.actransc WHERE actransc.acno = @dcacn AND actransc.Type = @ty ORDER BY date DESC"

            cmdx.Parameters.AddWithValue("@dcacn", acn)
            cmdx.Parameters.AddWithValue("@ty", "INTR")
            cmdx.CommandText = query

            Dim drp As SqlDataReader = cmdx.ExecuteReader()


            If drp.HasRows() Then

                drp.Read()

                If IsDBNull(drp(0)) Then

                    Session("prv_inton") = Session("ac_date")


                Else
                    Session("prv_inton") = drp(0)
                End If

            Else

                Session("prv_inton") = Session("ac_date")

            End If
            drp.Close()

        Catch ex As Exception
            Response.Write(ex.Message)

        Finally
            cmdx.Dispose()
            con1.Close()

        End Try

        Return Session("prv_inton")


    End Function


    Function get_prev_inton(ByVal acn As String)

        If con1.State = ConnectionState.Closed Then con1.Open()

        Try
            cmdx.Parameters.Clear()
            cmdx.Connection = con1
            query = "SELECT top 1 date FROM dbo.actrans WHERE actrans.acno = @dcacn AND actrans.Type = @ty ORDER BY date DESC"

            cmdx.Parameters.AddWithValue("@dcacn", acn)
            cmdx.Parameters.AddWithValue("@ty", "INTR")
            cmdx.CommandText = query

            Dim drp As SqlDataReader = cmdx.ExecuteReader()


            If drp.HasRows() Then

                drp.Read()

                If IsDBNull(drp(0)) Then

                    Session("prv_inton") = Session("ac_date")


                Else
                    Session("prv_inton") = drp(0)
                End If

            Else

                Session("prv_inton") = Session("ac_date")

            End If
            drp.Close()

        Catch ex As Exception
            Response.Write(ex.Message)

        Finally
            cmdx.Dispose()
            con1.Close()

        End Try

        Return Session("prv_inton")


    End Function


    Sub update_data_si_c()

        Dim prodt = get_pro(txt_si_prod.Text)
        Dim dat As DateTime = DateTime.ParseExact(txt_si_date.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT tmpintc.dep, tmpintc.acn, tmpintc.camt, tmpintc.damt, stdinsc.product, stdinsc.siacno,tmpintc.tdate as acdate FROM dbo.tmpintc LEFT OUTER JOIN dbo.stdinsc  ON tmpintc.acn = stdinsc.acno "
                query &= "WHERE tmpintc.dep = @dep AND tmpintc.post = @post AND tmpintc.si = @si AND CONVERT(VARCHAR(20), tmpintc.tdate, 112) = @dt ORDER BY tmpintc.id"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@dep", txt_si_prod.Text)
                cmd.Parameters.AddWithValue("@post", 1)
                cmd.Parameters.AddWithValue("@si", 0)
                cmd.Parameters.AddWithValue("@dt", reformatted)

                Using reader As SqlDataReader = cmd.ExecuteReader
                    If reader.HasRows Then
                        Do While reader.Read


                            Using conx = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                                conx.Open()
                                Using cmdx = New SqlCommand
                                    cmdx.Connection = conx
                                    query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                                    query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"
                                    cmdx.CommandText = query
                                    cmdx.Parameters.Clear()
                                    cmdx.Parameters.AddWithValue("@id", Session("tid"))
                                    cmdx.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                                    cmdx.Parameters.AddWithValue("@acno", reader(1).ToString)
                                    cmdx.Parameters.AddWithValue("@drd", CDbl(reader(2).ToString))
                                    cmdx.Parameters.AddWithValue("@crd", 0)
                                    cmdx.Parameters.AddWithValue("@drc", CDbl(reader(2).ToString))
                                    cmdx.Parameters.AddWithValue("@crc", 0)
                                    cmdx.Parameters.AddWithValue("@narration", "To Transfer")
                                    cmdx.Parameters.AddWithValue("@due", reader(5).ToString)
                                    cmdx.Parameters.AddWithValue("@type", "TRF")
                                    cmdx.Parameters.AddWithValue("@suplimentry", prodt)
                                    cmdx.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                                    cmdx.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
                                    cmdx.Parameters.AddWithValue("@cbal", 0)
                                    cmdx.Parameters.AddWithValue("@dbal", 0)
                                    Try
                                        cmdx.ExecuteNonQuery()
                                    Catch ex As Exception

                                        Response.Write(ex.Message)
                                    End Try


                                End Using
                            End Using


                            Using coni = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                                coni.Open()
                                Using cmdi = New SqlCommand
                                    cmdi.Connection = coni
                                    query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                                    query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

                                    cmdi.CommandText = query
                                    cmdi.Parameters.Clear()

                                    cmdi.Parameters.AddWithValue("@id", Session("tid"))
                                    cmdi.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                                    cmdi.Parameters.AddWithValue("@acno", reader(5).ToString)
                                    cmdi.Parameters.AddWithValue("@drd", 0)
                                    cmdi.Parameters.AddWithValue("@crd", CDbl(reader(2).ToString))
                                    cmdi.Parameters.AddWithValue("@drc", 0)
                                    cmdi.Parameters.AddWithValue("@crc", CDbl(reader(2).ToString))
                                    cmdi.Parameters.AddWithValue("@narration", "By Transfer")
                                    cmdi.Parameters.AddWithValue("@due", reader(1).ToString)
                                    cmdi.Parameters.AddWithValue("@type", "TRF")
                                    cmdi.Parameters.AddWithValue("@suplimentry", "SAVINGS DEPOSIT")
                                    cmdi.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                                    cmdi.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
                                    cmdi.Parameters.AddWithValue("@cbal", 0)
                                    cmdi.Parameters.AddWithValue("@dbal", 0)


                                    Try
                                        cmdi.ExecuteNonQuery()
                                    Catch ex As Exception
                                        Response.Write(ex.ToString)
                                    End Try


                                End Using

                            End Using




                        Loop
                    End If


                End Using


            End Using
        End Using


    End Sub



    Sub get_data_si()

        Dim dat As DateTime = DateTime.ParseExact(txt_si_date.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If txt_si_prod.Text = "FD" Then

            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con
            cmd.Parameters.Clear()

            '     If Mid(reformatted, 5, 4) = "0331" Then
            '   query = "SELECT tmpint.dep, tmpint.acn, tmpint.camt, tmpint.damt, stdins.product, stdins.siacno,tmpint.tdate as acdate FROM dbo.tmpint LEFT OUTER JOIN dbo.stdins  ON tmpint.acn = stdins.acno "
            '  query &= "WHERE tmpint.dep = @dep  tmpint.post = @post AND tmpint.si=@si AND CONVERT(VARCHAR(20), tmpint.tdate, 112) = @dt ORDER BY tmpint.id"
            'End If

            query = "SELECT tmpint.dep, tmpint.acn, tmpint.camt, tmpint.damt, stdins.product, stdins.siacno,tmpint.tdate as acdate FROM dbo.tmpint LEFT OUTER JOIN dbo.stdins  ON tmpint.acn = stdins.acno "
            query &= "WHERE tmpint.dep = @dep AND tmpint.post = @post AND tmpint.si = @si AND CONVERT(VARCHAR(20), tmpint.tdate, 112) = @dt ORDER BY tmpint.id"

            Dim dsi As New DataSet

            Try
                Dim adapter As New SqlDataAdapter(query, con)

                adapter.SelectCommand.Parameters.AddWithValue("@dep", txt_si_prod.Text)
                adapter.SelectCommand.Parameters.AddWithValue("@post", 1)
                adapter.SelectCommand.Parameters.AddWithValue("@si", 0)
                adapter.SelectCommand.Parameters.AddWithValue("@dt", reformatted)

                adapter.Fill(dsi)

                disp_si.DataSource = dsi
                disp_si.DataBind()

                trim_disp_si()

                'lbl_si_cint_total.Text = String.Format("{0:N}", dsi.Tables(0).Compute("Sum(camt)", "0"))

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

        ElseIf txt_si_prod.Text = "RD" Then

            query = "SELECT  stdins.acno AS acn,stdins.siacno, master.amount AS camt, master.amount AS damt,master.date as acdate,stdins.sidate FROM dbo.stdins LEFT OUTER JOIN dbo.master  ON stdins.acno = master.acno WHERE stdins.srcproduct = 'RD'"
            query &= " AND sidate = @dt and stdins.cld= '0' and CONVERT(VARCHAR(20), master.mdate, 112) > @dmy"



            Dim dsi As New DataSet

            Try
                Dim adapter As New SqlDataAdapter(query, con)

                ' adapter.SelectCommand.Parameters.AddWithValue("@mp", Month(txt_si_date.Text))
                'adapter.SelectCommand.Parameters.AddWithValue("@yp", Year(txt_si_date.Text))
                adapter.SelectCommand.Parameters.AddWithValue("@dt", txt_si_day.Text)
                adapter.SelectCommand.Parameters.AddWithValue("@dmy", reformatted)

                adapter.Fill(dsi)

                disp_si.DataSource = dsi
                disp_si.DataBind()

                lbl_bycas.Text = "BALANCE"
                trim_disp_si()

            Catch ex As Exception
                Response.Write(ex.Message)
            End Try

        End If


    End Sub


    Function get_dint()


        Dim dr_roi As SqlDataReader
        If con1.State = ConnectionState.Closed Then con1.Open()
        query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"

        cmd.Parameters.Clear()
        cmd.Connection = con1


        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@prod", prod.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@prdtyp", Session("prdtyp"))
        cmd.Parameters.AddWithValue("@prdx", Session("prd"))
        cmd.Parameters.AddWithValue("@prdy", Session("prd"))



        Try

            dr_roi = cmd.ExecuteReader()

            If dr_roi.HasRows() Then
                'dr_roi.Read()

                While dr_roi.Read

                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)


                    Dim x As Long = FYFRM.CompareTo(CType(Session("ac_date"), Date))

                    If x = -1 Then


                        Dim y As Long = FYTO.CompareTo(CType(Session("ac_date"), Date))

                        If y = 1 Then
                            Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                            Session("dint") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))

                            Exit While

                        End If

                    End If
                End While


            End If

            dr_roi.Close()


        Catch ex As Exception

            MsgBox(ex.Message)
        Finally
            cmd.Dispose()
            con1.Close()



        End Try


        Return Session("cintr")

    End Function

    Sub get_accured_int_c(ByVal acno As String)

        query = "SELECT TOP 1 actransc.Crd,actransc.Crc,actransc.Type,actransc.acno,actransc.date FROM dbo.actransc"
        query &= " WHERE actransc.Type = 'INTR' AND actransc.acno = @acn ORDER BY actransc.date DESC "

        If con1.State = ConnectionState.Closed Then con1.Open()

        cmd1.Connection = con1
        cmd1.Parameters.Clear()
        cmd1.CommandText = query
        cmd1.Parameters.AddWithValue("@acn", acno)
        Try

            Dim dr As SqlDataReader

            dr = cmd1.ExecuteReader()
            If dr.HasRows Then
                dr.Read()
                Session("c_accured") = dr(1)
                Session("d_accured") = dr(0)
            Else
                Session("c_accured") = 0
                Session("d_accured") = 0


            End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Sub get_accured_int(ByVal acno As String)

        query = "SELECT TOP 1 actrans.Crd,actrans.Crc,actrans.Type,actrans.acno,actrans.date FROM dbo.actrans"
        query &= " WHERE actrans.Type = 'INTR' AND actrans.acno = @acn ORDER BY actrans.date DESC "

        If con1.State = ConnectionState.Closed Then con1.Open()

        cmd1.Connection = con1
        cmd1.Parameters.Clear()
        cmd1.CommandText = query
        cmd1.Parameters.AddWithValue("@acn", acno)
        Try

            Dim dr As SqlDataReader

            dr = cmd1.ExecuteReader()
            If dr.HasRows Then
                dr.Read()
                Session("c_accured") = dr(1)
                Session("d_accured") = dr(0)
            Else
                Session("c_accured") = 0
                Session("d_accured") = 0


            End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Sub post_int_fd(ByVal dt As Date, ByVal acno As String, ByVal depamt As Double, ByVal intc As Double, ByVal intd As Double)


        ' Session("cintr") = get_dint()

        ' If intc > Session("cintr") Then Session("cintr")

        Dim intc_amt As Double

        Dim intd_amt As Double

        Dim prd As Integer = DateDiff(DateInterval.Day, dt, Convert.ToDateTime(tdate.Text))

        Try


            intc_amt = Math.Round((depamt * intc) / 100 / 12, 2, MidpointRounding.AwayFromZero)
            intd_amt = Math.Round((depamt * intd) / 100 / 12, 2, MidpointRounding.AwayFromZero)

            intc_amt = Math.Round(intc_amt, 0, MidpointRounding.AwayFromZero)
            intd_amt = Math.Round(intd_amt, 0, MidpointRounding.AwayFromZero)


            If inttyp.SelectedItem.Text = "Yearly" Then
                '' intc_amt = Math.Round((intc_amt / 30) * prd, 0, MidpointRounding.AwayFromZero)
                ''intd_amt = Math.Round((intd_amt / 30) * prd, 0, MidpointRounding.AwayFromZero)


                intc_amt = Math.Round(((depamt * intc) / 100 / 365) * prd, 2, MidpointRounding.AwayFromZero)
                intd_amt = Math.Round(((depamt * intd) / 100 / 365) * prd, 2, MidpointRounding.AwayFromZero)

                intc_amt = Math.Round(intc_amt, 0, MidpointRounding.AwayFromZero)
                intd_amt = Math.Round(intd_amt, 0, MidpointRounding.AwayFromZero)
            Else

                If Not Month(tdate.Text) = 4 Then
                    intc_amt = intc_amt
                    intd_amt = intd_amt
                    prd = 30

                Else
                    prd = 30
                    get_accured_int(acno)


                    intc_amt = intc_amt - Session("c_accured")
                    intd_amt = intd_amt - Session("d_accured")

                End If

            End If


            If con1.State = ConnectionState.Closed Then con1.Open()

            cmd1.Connection = con1

            cmd1.Parameters.Clear()

            If Not intc_amt = 0 Then
                query = "insert into tmpint(tdate,acn,dep,acdate,amt,prd,cint,camt,dint,damt,runson)"
                query &= "values(@tdate,@acn,@dep,@acdate,@amt,@prd,@cint,@camt,@dint,@damt,@runson)"
                cmd1.CommandText = query
                cmd1.Parameters.AddWithValue("@tdate", Convert.ToDateTime(tdate.Text))
                cmd1.Parameters.AddWithValue("@acn", acno)
                cmd1.Parameters.AddWithValue("@dep", Trim(prod.SelectedItem.Text))
                cmd1.Parameters.AddWithValue("@acdate", CType(Session("ac_date"), Date))
                cmd1.Parameters.AddWithValue("@amt", Session("amt"))
                cmd1.Parameters.AddWithValue("@prd", prd)
                cmd1.Parameters.AddWithValue("@cint", intc)
                cmd1.Parameters.AddWithValue("@camt", intc_amt)
                cmd1.Parameters.AddWithValue("@dint", intd)
                cmd1.Parameters.AddWithValue("@damt", intd_amt)
                cmd1.Parameters.AddWithValue("@runson", txtday.Text)

                cmd1.ExecuteNonQuery()
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con1.Close()
            cmd1.Dispose()
            cmd1.Parameters.Clear()

        End Try

        post_int_fd_c(acno, intc_amt)


    End Sub
    Sub post_int_fd_c(ByVal acno As String, ByVal intamt As Double)

        Dim intcamt As Double
        Dim cont As Integer = 0
        Dim i As Integer = 0

        Dim camt As Double = 0


        Dim intdamt As Double
        Dim depamt As Double = 0


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                query = "select COALESCE(count(*),0) from masterc where cld=0 and parent=@acno"
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@acno", acno)
                cont = cmd.ExecuteScalar


            End Using

        End Using





        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Dim intc As Double
            Dim acn As String
            Using cmd = New SqlCommand
                query = "select acno,amount,cint,mdate from masterc where cld=0 and parent=@acno"
                cmd.Connection = con
                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@acno", acno)
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If reader.HasRows Then

                        Do While reader.Read
                            i = i + 1


                            acn = reader(0).ToString
                            depamt = CDbl(reader(1).ToString)
                            intc = CDbl(reader(2).ToString)

                            Session("prv_inton") = get_prev_inton_c(reader(0).ToString)


                            Dim prd As Integer = DateDiff(DateInterval.Day, Session("prv_inton"), Convert.ToDateTime(tdate.Text))

                            intcamt = Math.Round((depamt * intc) / 100 / 12, 2, MidpointRounding.AwayFromZero)
                            intdamt = Math.Round((depamt * intc / 100) / 12, 2, MidpointRounding.AwayFromZero)

                            intcamt = Math.Round(intcamt, 0, MidpointRounding.AwayFromZero)
                            intdamt = Math.Round(intdamt, 0, MidpointRounding.AwayFromZero)


                            If inttyp.SelectedItem.Text = "Yearly" Then
                                '' intc_amt = Math.Round((intc_amt / 30) * prd, 0, MidpointRounding.AwayFromZero)
                                ''intd_amt = Math.Round((intd_amt / 30) * prd, 0, MidpointRounding.AwayFromZero)


                                intcamt = Math.Round(((depamt * intc) / 100 / 365) * prd, 2, MidpointRounding.AwayFromZero)
                                intdamt = Math.Round(((depamt * intc / 100) / 365) * prd, 2, MidpointRounding.AwayFromZero)

                                intcamt = Math.Round(intcamt, 0, MidpointRounding.AwayFromZero)
                                intdamt = Math.Round(intdamt, 0, MidpointRounding.AwayFromZero)
                            Else

                                If Not Month(tdate.Text) = 4 Then
                                    intcamt = intcamt
                                    intdamt = intdamt
                                    prd = 30

                                Else
                                    prd = 30
                                    get_accured_int_c(acno)


                                    intcamt = intcamt - Session("c_accured")
                                    intdamt = intdamt - Session("d_accured")

                                End If

                            End If

                            camt = camt + intcamt


                            If i = cont Then

                                If intamt < camt Then
                                    intcamt = intcamt - (camt - intamt)
                                ElseIf intamt > camt Then
                                    intcamt = intamt + (intamt - camt)

                                End If
                            End If


                            Using con1 = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                                con1.Open()
                                Using cmd1 = New SqlCommand
                                    cmd1.Connection = con1
                                    If Not intcamt = 0 Then
                                        query = "insert into tmpintc(tdate,acn,dep,acdate,amt,prd,cint,camt,dint,damt,runson)"
                                        query &= "values(@tdate,@acn,@dep,@acdate,@amt,@prd,@cint,@camt,@dint,@damt,@runson)"
                                        cmd1.CommandText = query
                                        cmd1.Parameters.AddWithValue("@tdate", Convert.ToDateTime(tdate.Text))
                                        cmd1.Parameters.AddWithValue("@acn", acn)
                                        cmd1.Parameters.AddWithValue("@dep", Trim(prod.SelectedItem.Text))
                                        cmd1.Parameters.AddWithValue("@acdate", CType(Session("ac_date"), Date))
                                        cmd1.Parameters.AddWithValue("@amt", depamt)
                                        cmd1.Parameters.AddWithValue("@prd", prd)
                                        cmd1.Parameters.AddWithValue("@cint", intc)
                                        cmd1.Parameters.AddWithValue("@camt", intcamt)
                                        cmd1.Parameters.AddWithValue("@dint", intc)
                                        cmd1.Parameters.AddWithValue("@damt", intcamt)
                                        cmd1.Parameters.AddWithValue("@runson", txtday.Text)

                                        cmd1.ExecuteNonQuery()
                                    End If


                                End Using
                            End Using


                        Loop

                    End If


                End Using

            End Using
        End Using



    End Sub


    Sub longprocess()


    End Sub


    Private Sub bgw_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGW.DoWork





        BGW.ReportProgress(10)

        ' longprocess()



        BGW.CancelAsync()






    End Sub


    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ' lblprocess.Text = "Please Wait"
        btnSubmit.Enabled = False

        'blocker.Attributes.Add("display", "block")

        '  Me.ModalPopupt.Show()
        Select Case Trim(prod.SelectedItem.Text)
            Case "FD"

                lod = "D"
                update_fd_int()
                '  ipost.Visible = True
                '  TabContainer1.ActiveTab = ipost
                'txtproduct.Text = "FD"
                txtidate.Text = tdate.Text
                txtproduct.Text = "FD"
                disp4post(tdate.Text, "FD")
               ' TabContainer1.ActiveTab = ipost
            Case "KMK"
                lod = "D"


                update_kmk_int()



            Case "RD"
                lod = "D"
                update_rd_int()

            Case "RID"
                lod = "D"
                update_rid_int()
            Case "SB"
                lod = "D"
                update_sb_int()

            Case "DCL"
                lod = "D"
                update_loan_int()

            Case "JL"
                lod = "L"
                update_loan_int()
            Case "DL"
                lod = "L"

                update_loan_int()
            Case "ML"
                lod = "L"

                update_loan_int()
        End Select

        btnSubmit.Enabled = True
        '  Me.ModalPopupt.Hide()


        clear_form()

        ' System.Windows.Forms.Application.DoEvents()


        ' System.Threading.Thread.Sleep(5000)

        '  bgwcall()

        'BGW.WorkerSupportsCancellation = True
        'BGW.WorkerReportsProgress = True
        'AddHandler BGW.DoWork, AddressOf bgw_DoWork
        'AddHandler BGW.ProgressChanged, AddressOf bgw_ProgressChanged
        'AddHandler BGW.RunWorkerCompleted, AddressOf bgw_Completed

        '  System.Windows.Forms.Application.DoEvents()
        '        Upintup.Update()

        '        BGW.RunWorkerAsync()


        ' 
        '  blocker.Attributes.Add("display", "none")
    End Sub
    Sub update_dcl_int()



    End Sub
    Sub bgwcall()

    End Sub
    Sub update_rid_int()

        Dim cum_int As Integer
        Try
            If dt.Columns.Count = 0 Then
                dt.Columns.Add("acdate", GetType(Date))
                dt.Columns.Add("acn", GetType(String))
                dt.Columns.Add("amt", GetType(Decimal))
                dt.Columns.Add("prd", GetType(Integer))
                dt.Columns.Add("cint", GetType(Decimal))
                dt.Columns.Add("camt", GetType(Decimal))
                dt.Columns.Add("damt", GetType(Decimal))
            End If

            If con.State = ConnectionState.Closed Then con.Open()

            query = "select date,acno,amount,cint,mdate,dint from master where product='RID' and cld='0'  order by date"
            cmd.Connection = con
            cmd.CommandText = query


            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                While dr.Read
                    Dim x As Integer = Convert.ToDateTime(dr(4)).CompareTo(Convert.ToDateTime(txtto.Text))
                    If x < 0 Then GoTo nxt
                    Session("cintr") = dr(3)

                    Session("dint") = dr(5)
                    cum_int = get_rid_int(dr(1), dr(0), dr(2))
                    If Session("amt") = 0 Then GoTo nxt

                    newrow = dt.NewRow
                    newrow(0) = dr(0)
                    newrow(1) = dr(1)
                    newrow(4) = dr(3)
                    newrow(2) = Session("amt")
                    newrow(3) = Session("prd_buffer")
                    newrow(4) = cum_int
                    If cum_int = 0 Then GoTo nxt
                    dt.Rows.Add(newrow)

                    If con1.State = ConnectionState.Closed Then con1.Open()

                    cmd1.Connection = con1

                    cmd1.Parameters.Clear()

                    query = "insert into tmpint(tdate,acn,dep,acdate,amt,prd,cint,camt,dint,damt)"
                    query &= "values(@tdate,@acn,@dep,@acdate,@amt,@prd,@cint,@camt,@dint,@damt)"
                    cmd1.CommandText = query
                    cmd1.Parameters.AddWithValue("@tdate", Convert.ToDateTime(tdate.Text))
                    cmd1.Parameters.AddWithValue("@acn", dr(1))
                    cmd1.Parameters.AddWithValue("@dep", Trim(prod.SelectedItem.Text))
                    cmd1.Parameters.AddWithValue("@acdate", dr(0))
                    cmd1.Parameters.AddWithValue("@amt", Session("amt"))
                    cmd1.Parameters.AddWithValue("@prd", Session("prd_buffer"))
                    cmd1.Parameters.AddWithValue("@cint", Session("cintr"))
                    cmd1.Parameters.AddWithValue("@camt", cum_int)
                    cmd1.Parameters.AddWithValue("@dint", Session("dint"))
                    cmd1.Parameters.AddWithValue("@damt", Math.Round(Session("dintc")))

                    Try
                        cmd1.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)
                    Finally
                        con1.Close()
                        cmd1.Dispose()
                        cmd1.Parameters.Clear()

                    End Try
nxt:
                End While
            End If
            dr.Close()

            ' disp.DataSource = dt
            ' disp.DataBind()
            Session("dt") = dt
            txtidate.Text = tdate.Text
            txtproduct.Text = prod.SelectedItem.Text
            'TabContainer1.ActiveTab = ipost
            'disp4post(txtidate.Text, txtproduct.Text)

        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()

        End Try



    End Sub

    Function get_rid_int(ByVal acn As String, ByVal dt As Date, ByVal amt As Double)

        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim fq As Integer = 0
        Dim prdx As Integer = 0
        Dim das As Decimal = 0.0
        Dim pamt As Double = 0
        pamt = get_ob(Trim(acn), reformatted)


        If Not pamt = 0 Then
            Session("amt") = pamt
            Session("prd_buffer") = DateDiff(DateInterval.Day, Convert.ToDateTime(txtfrm.Text), Convert.ToDateTime(txtto.Text)) + 1

        Else
            Session("amt") = amt
            pamt = amt
            Session("dbal") = amt
            Session("prd_buffer") = DateDiff(DateInterval.Day, dt, Convert.ToDateTime(txtto.Text)) + 1
            If Session("prd_buffer") < 0 Then Session("prd_buffer") = 0


        End If

        Dim curint As Integer = 0
        Dim cumint As Integer = 0

        'prd_buffer = prd_buffer / 30
        'prdx = prd_buffer Mod 3

        'fq = Math.Round(prd_buffer - prdx)

        ''   pamt = CDbl(lblbal.Text)

        'If Not fq = 0 Then

        '   For i = 1 To fq
        '    If Session("prd_buffer") < 0 Then Session("prd_buffer") = 0

        cumint = ((pamt * Session("cintr") / 100) / 365) * Session("prd_buffer")
        Session("dintc") = ((Session("dbal") * Session("dint") / 100) / 365) * Session("prd_buffer")
        '   cumint = (curint * prd_buffer)
        'Session("dbal")
        '  pamt = pamt + (curint * 3)

        ' Next
        '  End If


        'das = (prd_buffer - (fq * 3))

        '        cumint = Math.Round(cumint + (pamt * cintr / 100) / 365) * Math.Round((das * 30))



        Return cumint

    End Function

    Function due_paid(ByVal acn As String)
        If con1.State = ConnectionState.Closed Then con1.Open()



        Dim sql As String = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].type<>'INTR' "

        cmdi.Connection = con1
        cmdi.CommandText = sql
        Try

            countresult = cmdi.ExecuteScalar()

            countresult = IIf(IsDBNull(countresult), 0, countresult)

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Return countresult

    End Function

    Sub update_rd_int()
        Dim dupaid As Integer
        Dim sum As Double
        Dim pamt As Double = 0
        Dim mint As Double = 0
        Dim cum_int As Double = 0
        Dim cur_pr As Double = 0

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("acdate", GetType(Date))
            dt.Columns.Add("acn", GetType(String))
            dt.Columns.Add("amt", GetType(Decimal))
            dt.Columns.Add("prd", GetType(Integer))
            dt.Columns.Add("cint", GetType(Decimal))
            dt.Columns.Add("camt", GetType(Decimal))
            dt.Columns.Add("damt", GetType(Decimal))
        End If

        Try
            If con.State = ConnectionState.Closed Then con.Open()



            query = " SELECT  master.date,master.acno, master.amount,master.cint,SUM(actrans.Drd) AS debit,SUM(actrans.Crd) AS credit FROM dbo.master "
            query &= " INNER JOIN dbo.actrans  ON master.acno = actrans.acno WHERE master.cld = '0' AND master.product = 'RD'"
            query &= " GROUP BY master.date,master.acno, master.amount, master.cint ORDER BY master.date"


            cmd.Connection = con
            cmd.CommandText = query


            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                While dr.Read

                    If Not dr(5) - dr(4) = 0 Then

                        dupaid = due_paid(dr(1))
                        sum = dupaid * dr(2)
                        sum = (dr(5) - dr(4)) - sum
                        Session("cintr") = dr(3)

                        pamt = dr(2)
                        cur_pr = pamt
                        For i = 1 To dupaid
                            mint = (cur_pr * Session("cintr") / 100) / 12
                            mint = Math.Round(mint)
                            cum_int = cum_int + mint
                            cur_pr = cur_pr + mint + pamt
                        Next
                        cum_int = cum_int - sum
                        newrow = dt.NewRow
                        newrow(0) = dr(0)
                        newrow(1) = dr(1)
                        newrow(2) = pamt
                        newrow(3) = 12
                        newrow(4) = Session("cintr")
                        newrow(5) = cum_int
                        dt.Rows.Add(newrow)

                        If con1.State = ConnectionState.Closed Then con1.Open()

                        cmd1.Connection = con1

                        cmd1.Parameters.Clear()

                        query = "insert into tmpint(tdate,acn,dep,acdate,amt,prd,cint,camt,dint,damt)"
                        query &= "values(@tdate,@acn,@dep,@acdate,@amt,@prd,@cint,@camt,@dint,@damt)"
                        cmd1.CommandText = query
                        cmd1.Parameters.AddWithValue("@tdate", Convert.ToDateTime(tdate.Text))
                        cmd1.Parameters.AddWithValue("@acn", dr(1))
                        cmd1.Parameters.AddWithValue("@dep", Trim(prod.SelectedItem.Text))
                        cmd1.Parameters.AddWithValue("@acdate", dr(0))
                        cmd1.Parameters.AddWithValue("@amt", dr(2))
                        cmd1.Parameters.AddWithValue("@prd", "12")
                        cmd1.Parameters.AddWithValue("@cint", Session("cintr"))
                        cmd1.Parameters.AddWithValue("@camt", cum_int)
                        cmd1.Parameters.AddWithValue("@dint", Session("cintr"))
                        cmd1.Parameters.AddWithValue("@damt", cum_int)

                        Try
                            cmd1.ExecuteNonQuery()

                        Catch ex As Exception
                            Response.Write(ex.ToString)
                        Finally
                            con1.Close()
                            cmd1.Dispose()
                            cmd1.Parameters.Clear()
                            cum_int = 0


                        End Try


                    End If
                End While

            End If

            dr.Close()
            disp.DataSource = dt
            disp.DataBind()
            lblnet.Text = String.Format("{0:N}", dt.Compute("sum(camt)", ""))
            Session("dt") = dt
            txtidate.Text = tdate.Text
            txtproduct.Text = prod.SelectedItem.Text
            ' TabContainer1.ActiveTab = ipost


        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally
            con.Close()
            cmd.Dispose()

        End Try



    End Sub


    Sub update_loan_int()

        Dim x As Double

        Try
            If dt.Columns.Count = 0 Then
                dt.Columns.Add("acdate", GetType(Date))
                dt.Columns.Add("acn", GetType(String))
                dt.Columns.Add("amt", GetType(Decimal))
                dt.Columns.Add("prd", GetType(Integer))
                dt.Columns.Add("cint", GetType(Decimal))
                dt.Columns.Add("camt", GetType(Decimal))
                dt.Columns.Add("damt", GetType(Decimal))

            End If

            If con.State = ConnectionState.Closed Then con.Open()

            query = "SELECT master.date,master.acno, SUM(actrans.Drc) AS debit,SUM(actrans.Crc) AS credit,master.cld,master.product,master.sch FROM dbo.master "
            query &= " INNER JOIN dbo.actrans ON master.acno = actrans.acno WHERE master.cld = 0 AND master.product ='" + prod.SelectedItem.Text + "'"
            query &= " GROUP BY master.date,master.acno,master.product,master.cld,master.sch order by date"
            cmd.Connection = con
            cmd.CommandText = query


            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                While dr.Read()

                    If Not (dr(2) - dr(3)) = 0 Then

                        newrow = dt.NewRow
                        newrow(0) = dr(0)
                        newrow(1) = dr(1)
                        newrow(2) = dr(2) - dr(3)
                        Session("sch") = dr(6)

                        x = get_int(dr(2) - dr(3), dr(1), dr(0))
                        If x = 0 Then GoTo nxt1

                        newrow(3) = Session("prd_buffer")
                        Session("cintr") = IIf(Session("cintr") = Nothing, 0, Session("cintr"))
                        newrow(4) = Session("cintr")
                        newrow(5) = x
                        dt.Rows.Add(newrow)
                        If con1.State = ConnectionState.Closed Then con1.Open()

                        cmd1.Connection = con1

                        cmd1.Parameters.Clear()

                        query = "insert into tmpint(tdate,acn,dep,acdate,amt,prd,cint,camt,dint,damt)"
                        query &= "values(@tdate,@acn,@dep,@acdate,@amt,@prd,@cint,@camt,@dint,@damt)"
                        cmd1.CommandText = query
                        cmd1.Parameters.AddWithValue("@tdate", Convert.ToDateTime(tdate.Text))
                        cmd1.Parameters.AddWithValue("@acn", dr(1))
                        cmd1.Parameters.AddWithValue("@dep", Trim(prod.SelectedItem.Text))
                        cmd1.Parameters.AddWithValue("@acdate", dr(0))
                        cmd1.Parameters.AddWithValue("@amt", dr(2) - dr(3))
                        cmd1.Parameters.AddWithValue("@prd", Session("prd_buffer"))
                        cmd1.Parameters.AddWithValue("@cint", Session("cintr"))
                        cmd1.Parameters.AddWithValue("@camt", x)
                        If prod.SelectedItem.Text = "JL" Then
                            cmd1.Parameters.AddWithValue("@dint", 0)
                            cmd1.Parameters.AddWithValue("@damt", 0)
                        Else

                            cmd1.Parameters.AddWithValue("@dint", Session("dintr"))
                            cmd1.Parameters.AddWithValue("@damt", Session("totint_d"))
                        End If
                        Try
                            cmd1.ExecuteNonQuery()

                        Catch ex As Exception
                            Response.Write(ex.ToString)

                        Finally
                            con1.Close()
                            cmd1.Dispose()
                            cmd1.Parameters.Clear()

                        End Try
                    End If
nxt1:
                End While

            End If

            Session("dt") = dt
            lblnet.Text = String.Format("{0:N}", dt.Compute("sum(camt)", ""))
            lblbyc.Text = String.Format("{0:N}", dt.Compute("sum(damt)", ""))
            txtidate.Text = tdate.Text
            txtproduct.Text = prod.SelectedItem.Text
            'disp.DataSource = dt
            'disp.DataBind()
            ' TabContainer1.ActiveTab = ipost
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub
    Sub update_sb_int()
        If dt.Columns.Count = 0 Then
            dt.Columns.Add("acdate", GetType(Date))
            dt.Columns.Add("acn", GetType(String))
            dt.Columns.Add("amt", GetType(Decimal))
            dt.Columns.Add("prd", GetType(Integer))
            dt.Columns.Add("cint", GetType(Decimal))
            dt.Columns.Add("camt", GetType(Decimal))
            dt.Columns.Add("damt", GetType(Decimal))
        End If

        If con.State = ConnectionState.Closed Then con.Open()
        query = "select date,acno,amount,cint from master where product='SB' and cld='0'  "
        cmd.Connection = con
        cmd.CommandText = query
        Try
            Dim drm As SqlDataReader
            drm = cmd.ExecuteReader()
            If drm.HasRows() Then
                While drm.Read()
                    Session("ac_date") = Convert.ToDateTime(drm(0))
                    Session("acn") = drm(1)
                    Session("amt") = drm(2)
                    sb_closure(Trim(drm(1)), drm(3))
                End While
            End If
            drm.Close()


            Session("dt") = dt
            ' disp.DataSource = dt
            ' disp.DataBind()
            Session("dt") = dt
            lblnet.Text = String.Format("{0:N}", dt.Compute("sum(camt)", ""))
            txtidate.Text = tdate.Text
            txtproduct.Text = prod.SelectedItem.Text
            ' TabContainer1.ActiveTab = ipost

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub

    Sub update_kmk_int()


        If dt.Columns.Count = 0 Then
            dt.Columns.Add("acdate", GetType(Date))
            dt.Columns.Add("acn", GetType(String))
            dt.Columns.Add("amt", GetType(Decimal))
            dt.Columns.Add("prd", GetType(Integer))
            dt.Columns.Add("cint", GetType(Decimal))
            dt.Columns.Add("camt", GetType(Decimal))
            dt.Columns.Add("damt", GetType(Decimal))

        End If

        If con.State = ConnectionState.Closed Then con.Open()
        query = "select date,acno,amount,cint from master where product='KMK' and cld='0'"
        cmd.Connection = con
        cmd.CommandText = query
        Try
            Dim drm As SqlDataReader
            drm = cmd.ExecuteReader()
            If drm.HasRows() Then
                While drm.Read()
                    Session("ac_date") = drm(0)
                    Session("acn") = drm(1)
                    Session("amt") = drm(2)
                    kmk_closure(Trim(drm(1)), drm(3))

                    'lblprocessid.Text = Session("acn")
                    'lblprocess.Text = Session("acn")



                End While
            End If
            drm.Close()
            Session("dt") = dt
            '  disp.DataSource = dt
            ' disp.DataBind()





        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub
    Sub Refreshpg()
        'Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        'ipost.Visible = True
        'TabContainer1.ActiveTab = ipost
        ''txtproduct.Text = "FD"
        'txtidate.Text = tdate.Text
        'txtproduct.Text = "KMK"
        'disp4post(tdate.Text, "KMK")
        'TabContainer1.ActiveTab = ipost
        ' Me.ModalPopupt.Show()

        'lblprocess.Text = "Completed"


    End Sub
    Public Function get_balance(ByVal acn As String)

        If con1.State = ConnectionState.Closed Then con1.Open()
        cmdi.Parameters.Clear()


        Dim x As Integer = 1
        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno =@acno and scroll =@scroll GROUP BY [actrans].acno"
        cmdi.Parameters.AddWithValue("@acno", acn)
        cmdi.Parameters.AddWithValue("@scroll", x)
        ' Dim sql As String = "select top 1 cbal,dbal from dbo.[actrans] where [actrans].acno='" + acn + "' ORDER BY date DESC"
        Dim ds_bal As New DataSet

        '        Dim Adapter As New SqlDataAdapter(sql, con)
        '       Adapter.Fill(ds_bal)
        cmdi.Connection = con1
        cmdi.CommandText = sql

        Dim reader1 As SqlDataReader = cmdi.ExecuteReader()


        If reader1.HasRows Then
            reader1.Read()
            Session("dbal") = reader1(1).ToString - reader1(0).ToString
            Session("cbal") = reader1(3).ToString - reader1(2).ToString
        Else
            Session("cbal") = 0
            Session("dbal") = 0
        End If
        cmdi.Dispose()

        reader1.Close()

        con1.Close()


        Return Session("dbal")
    End Function


    Sub trim_disp_si()

        Dim Isinsufficiant As Boolean = False

        Dim int_total As Double = 0
        Dim bycas As Double = 0

        If txt_si_prod.Text = "FD" Then


            For i As Integer = 0 To disp_si.Rows.Count - 1
                Dim camt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_cint"), Label)
                Dim damt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_dint"), Label)

                damt.Text = String.Format("{0:N}", CDbl(damt.Text) - CDbl(camt.Text))
                bycas = bycas + CDbl(damt.Text)

                int_total = int_total + CDbl(camt.Text)
            Next

            lbl_si_cint_total.Text = String.Format("{0:N}", int_total)
            lbl_si_bycas_total.Text = String.Format("{0:N}", bycas)
        Else

            For i As Integer = 0 To disp_si.Rows.Count - 1
                Dim camt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_cint"), Label)
                Dim damt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_dint"), Label)
                Dim sbac As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblsiacno"), Label)



                damt.Text = String.Format("{0:N}", get_balance(sbac.Text))


                int_total = int_total + CDbl(camt.Text)

                If int_total = 0 Then
                    btn_si_up.Enabled = False
                Else
                    btn_si_up.Enabled = True
                End If


                If CDbl(damt.Text) - 20 < CDbl(camt.Text) Then
                    '  camt.ForeColor = Drawing.Color.Red
                    damt.ForeColor = Drawing.Color.Red
                    sbac.ForeColor = Drawing.Color.Red

                    Isinsufficiant = True
                Else
                    '  btn_si_up.Enabled = True
                End If

            Next

            lbl_si_cint_total.Text = String.Format("{0:N}", int_total)
            lbl_si_bycas_total.Text = "" 'String.Format("{0:N}", bycas)

        End If

        If Isinsufficiant Then btn_si_up.Enabled = False


    End Sub
    Sub trim_disp()
        Dim int_total As Double = 0

        For i As Integer = 0 To disp.Rows.Count - 1
            Dim camt As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblicamt"), Label)

            int_total = int_total + CDbl(camt.Text)
        Next

        lblnet.Text = String.Format("{0:N}", int_total)

        If int_total = 0 Then

            btn_post.Enabled = False
        Else
            btn_post.Enabled = True
        End If

    End Sub


    Sub disp4post(ByVal dat As String, ByVal dep As String)

        'Dim txtbyc As New Label
        'txtbyc.ID = "damt"
        Session("dt") = Nothing
        dt.Rows.Clear()


        If dt.Columns.Count = 0 Then
            dt.Columns.Add("acdate", GetType(Date))
            dt.Columns.Add("acn", GetType(String))
            dt.Columns.Add("name", GetType(String))
            dt.Columns.Add("amt", GetType(Decimal))
            dt.Columns.Add("prd", GetType(Integer))
            dt.Columns.Add("cint", GetType(Decimal))
            dt.Columns.Add("camt", GetType(Decimal))
            dt.Columns.Add("damt", GetType(Decimal))

        End If

        'Dim byctmp As New TemplateField
        'byctmp.ItemTemplate = txtbyc
        'Me.disp.Columns.Add(byctmp)

        'Dim camt As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblicamt"), Label)

        'If dep = "KMK" Or dep = "SB" Then

        If dep = "RD" Or dep = "JL" Then
            ' disp.Columns(8).Visible = False
            ' dtab.Visible = False
            'ctab.Visible = True
            '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showCtab", "showTab();", True)

        Else
            'disp.Columns(8).Visible = True
            'dtab.Visible = True
            'ctab.Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showCtab", "showTab();", True)



        End If
        Dim dsi As New DataSet
        Dim dat1 As DateTime = DateTime.ParseExact(dat, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        If con1.State = ConnectionState.Closed Then con1.Open()

        ' txtidate.Text = tdate.Text
        '  txtproduct.Text = prod.SelectedItem.Text

        ''query = "select acdate,acn,amt,prd,cint,camt,damt from tmpint WHERE convert(varchar(20), tdate, 112) ='" + reformatted + "' and dep='" + dep + "' and post=0 ORDER BY acdate,id"

        query = " SELECT tmpint.acdate,tmpint.acn,member.FirstName,tmpint.amt,tmpint.prd,tmpint.cint,tmpint.camt,tmpint.damt FROM dbo.tmpint "
        query += " INNER JOIN dbo.master ON tmpint.acn = master.acno INNER JOIN dbo.member  ON master.cid = member.MemberNo "
        query += " WHERE tmpint.tdate = @date AND tmpint.dep = @dep1 AND tmpint.post = 0 ORDER BY  tmpint.id,tmpint.acdate"
        cmd.Parameters.Clear()

        cmd.Connection = con1
        cmd.CommandText = query

        cmd.Parameters.AddWithValue("@dep1", dep)
        cmd.Parameters.AddWithValue("@date", reformatted)


        Try
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                While dr.Read()

                    newrow = dt.NewRow
                    newrow(0) = dr(0)
                    newrow(1) = dr(1)
                    newrow(2) = dr(2)
                    newrow(3) = dr(3)
                    newrow(4) = dr(4)
                    newrow(5) = dr(5)
                    newrow(6) = dr(6)
                    newrow(7) = dr(7)
                    dt.Rows.Add(newrow)
                End While
            End If



            Session("dt") = dt
            disp.DataSource = dt
            disp.DataBind()
            lblnet.Text = String.Format("{0:N}", dt.Compute("sum(camt)", ""))
            '  If dep = "KMK" Or dep = "SB" Then
            lblbyc.Text = String.Format("{0:N}", dt.Compute("sum(damt)", ""))
            ' End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            cmd.Dispose()
            con.Close()

        End Try

        '  trim_disp()

    End Sub

    Sub clear_form()
        txtday.Text = ""
        txtfrm.Text = ""
        txtto.Text = ""
        prod.SelectedIndex = 0
        inttyp.SelectedIndex = 0
        '        txtfocus(tdate)
        ' TabContainer1.ActiveTab = ipost
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        txtfocus(txtidate)

    End Sub

    Private Sub btn_post_Click(sender As Object, e As EventArgs) Handles btn_post.Click
        btn_post.Enabled = False
        btn_post_clr.Enabled = False

        Select Case Trim(txtproduct.Text)
            Case "DS"
                ach = "DAILY DEPOSIT"
                lod = "D"
            Case "FD"
                lod = "D"
                ach = "FIXED DEPOSIT"
            Case "KMK"
                lod = "D"
                ach = "KMK DEPOSIT"
            Case "RD"
                lod = "D"
                ach = "RECURRING DEPOSIT"
            Case "RID"
                lod = "D"
                ach = "REINVESTMENT DEPOSIT"
            Case "SB"
                lod = "D"
                ach = "SAVINGS DEPOSIT"
            Case "DCL"
                lod = "L"
                ach = "DAILY COLLECTION LOAN"
            Case "JL"
                lod = "L"
                ach = "JEWEL LOAN"
            Case "DL"
                lod = "L"
                ach = "DEPOSIT LOAN"
            Case "ML"
                lod = "L"
                ach = "MORTGAGE LOAN"
        End Select

        If con.State = ConnectionState.Closed Then con.Open()

        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        ' Dim prod = get_pro(product)
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtidate.Text))
        cmd.Parameters.AddWithValue("@acno", "")
        If lod = "D" Then
            cmd.Parameters.AddWithValue("@drd", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To INTEREST")
            cmd.Parameters.AddWithValue("@due", "")
        ElseIf lod = "L" Then
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@narration", "By INTEREST")
            cmd.Parameters.AddWithValue("@due", "")
        End If
        cmd.Parameters.AddWithValue("@type", "INTR")
        cmd.Parameters.AddWithValue("@suplimentry", txtproduct.Text + " INTEREST")
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)


        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
            con.Close()
            cmd.Dispose()


        End Try


        query = ""


        If con.State = ConnectionState.Closed Then con.Open()

        Dim countresult As Integer = 0

        query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + txtproduct.Text + " INTEREST" + "'"

        cmd.Connection = con
        cmd.CommandText = query
        Try
            countresult = cmd.ExecuteScalar()
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            con.Close()
            cmd.Dispose()

        End Try

        Session("tid") = Convert.ToString(countresult)




        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()

        cmd.Connection = con
        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@sudt", Convert.ToDateTime(txtidate.Text))
        cmd.Parameters.AddWithValue("@transid", Session("tid"))
        cmd.Parameters.AddWithValue("@achead", txtproduct.Text + " INTEREST")
        If lod = "D" Then
            cmd.Parameters.AddWithValue("@debit", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "To INTEREST")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
        ElseIf lod = "L" Then
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "By INTEREST")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
        End If
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.ToString)

        Finally
            cmd.Dispose()

            con.Close()



        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()


        cmd.Connection = con
        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@sudt", Convert.ToDateTime(txtidate.Text))
        cmd.Parameters.AddWithValue("@transid", Session("tid"))
        cmd.Parameters.AddWithValue("@achead", ach)
        If lod = "D" Then
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "By INTEREST")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
        ElseIf lod = "L" Then
            cmd.Parameters.AddWithValue("@debit", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "To INTEREST")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")

        End If

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.ToString)

        Finally
            cmd.Dispose()

            con.Close()



        End Try


        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()

        cmd.Connection = con
        query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@sudt", Convert.ToDateTime(txtidate.Text))
        cmd.Parameters.AddWithValue("@transid", Session("tid"))
        cmd.Parameters.AddWithValue("@achead", txtproduct.Text + " INTEREST")
        If lod = "D" Then
            cmd.Parameters.AddWithValue("@debit", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "To INTEREST")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
        ElseIf lod = "L" Then
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "By INTEREST")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
        End If
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.ToString)

        Finally
            cmd.Dispose()

            con.Close()



        End Try

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Parameters.Clear()


        cmd.Connection = con
        query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@sudt", Convert.ToDateTime(txtidate.Text))
        cmd.Parameters.AddWithValue("@transid", Session("tid"))
        cmd.Parameters.AddWithValue("@achead", ach)
        If lod = "D" Then
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "By INTEREST")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")
        ElseIf lod = "L" Then
            cmd.Parameters.AddWithValue("@debit", CDbl(lblnet.Text))
            cmd.Parameters.AddWithValue("@credit", 0)
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "To INTEREST")
            cmd.Parameters.AddWithValue("@typ", "JOURNAL")

        End If

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.ToString)

        Finally
            cmd.Dispose()

            con.Close()



        End Try






        If txtproduct.Text = "KMK" Or txtproduct.Text = "SB" Then

            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()


            cmd.Connection = con
            query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sudt", Convert.ToDateTime(txtidate.Text))
            cmd.Parameters.AddWithValue("@transid", Session("tid"))
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", CDbl(lblbyc.Text))
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "BY CASH")
            cmd.Parameters.AddWithValue("@typ", "RECEIPT")



            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                MsgBox(ex.ToString)

            Finally
                cmd.Dispose()

                con.Close()



            End Try

            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()


            cmd.Connection = con
            query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
            query &= "VALUES(@sudt,@transid,@achead,@debit,@credit,@usacn,@nar,@typ)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@sudt", Convert.ToDateTime(txtidate.Text))
            cmd.Parameters.AddWithValue("@transid", Session("tid"))
            cmd.Parameters.AddWithValue("@achead", ach)
            cmd.Parameters.AddWithValue("@debit", 0)
            cmd.Parameters.AddWithValue("@credit", CDbl(lblbyc.Text))
            cmd.Parameters.AddWithValue("@usacn", "")
            cmd.Parameters.AddWithValue("@nar", "BY CASH")
            cmd.Parameters.AddWithValue("@typ", "RECEIPT")



            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                MsgBox(ex.ToString)

            Finally
                cmd.Dispose()

                con.Close()



            End Try

        End If



        If con.State = ConnectionState.Closed Then con.Open()
        Dim dat As DateTime = DateTime.ParseExact(txtidate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
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

                query = "update chklst set intpost=1 WHERE convert(varchar(20), date, 112) ='" + reformatted + "'"
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

            Else

                dr.Close()
                cmd.Dispose()

                query = "insert into chklst(date,intpost)"
                query &= "values (@date,@intpost)"

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                cmd.Parameters.AddWithValue("@intpost", 1)
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



            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


        transfer2actransc()
        transfer2actrans()
        txt_si_date.Text = txtidate.Text
        get_data_si()
        txtidate.Text = ""
        txtproduct.Text = ""
        lblnet.Text = ""
        lbl_bycas.Text = ""
        btn_post.Enabled = True
        btn_post_clr.Enabled = True
        disp.DataSource = Nothing
        disp.DataBind()
        'TabContainer1.ActiveTab = stdins
        txtfocus(txt_si_date)

    End Sub
    Sub transfer2actransc()
        Dim ach = get_pro(txtproduct.Text)
        Dim dat1 As DateTime = DateTime.ParseExact(txtidate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        'acn = row1("acn")
        'lonint = row1("camt")
        'BYC = row1("damt")

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = " SELECT tmpintc.acn,tmpintc.camt,tmpintc.damt FROM dbo.tmpintc "
                query += " WHERE tmpintc.tdate = @date AND tmpintc.dep = @dep1 AND tmpintc.post = 0 ORDER BY  tmpintc.id,tmpintc.acdate"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@date", reformatted)
                cmd.Parameters.AddWithValue("@dep1", txtproduct.Text)

                Using reader As SqlDataReader = cmd.ExecuteReader
                    If reader.HasRows Then
                        Do While reader.Read

                            Using con1 = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                                con1.Open()
                                Using cmd1 = New SqlCommand
                                    cmd1.Connection = con1

                                    query = "INSERT INTO actransc(Id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                                    query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"


                                    cmd1.CommandText = query
                                    cmd1.Parameters.Clear()
                                    cmd1.Parameters.AddWithValue("@uiid", Session("tid"))
                                    cmd1.Parameters.AddWithValue("@uidate", Convert.ToDateTime(txtidate.Text))
                                    cmd1.Parameters.AddWithValue("@uiacno", reader(0).ToString)

                                    If lod = "D" Then
                                        cmd1.Parameters.AddWithValue("@uidrd", 0)
                                        cmd1.Parameters.AddWithValue("@uicrd", CDbl(reader(1).ToString))
                                        cmd1.Parameters.AddWithValue("@uidrc", 0)
                                        cmd1.Parameters.AddWithValue("@uicrc", CDbl(reader(1).ToString))
                                        cmd1.Parameters.AddWithValue("@uinarration", "By INTEREST")
                                    Else
                                        If Trim(ach) = "JEWEL LOAN" Then
                                            cmd1.Parameters.AddWithValue("@uidrd", 0)
                                        Else
                                            cmd1.Parameters.AddWithValue("@uidrd", CDbl(reader(2).ToString))
                                        End If
                                        cmd1.Parameters.AddWithValue("@uicrd", 0)
                                        cmd1.Parameters.AddWithValue("@uidrc", CDbl(reader(1).ToString))
                                        cmd1.Parameters.AddWithValue("@uicrc", 0)
                                        cmd1.Parameters.AddWithValue("@uinarration", "To INTEREST")

                                    End If
                                    cmd1.Parameters.AddWithValue("@uidue", " ")
                                    cmd1.Parameters.AddWithValue("@uitype", "INTR")
                                    cmd1.Parameters.AddWithValue("@uisuplimentry", Trim(ach))
                                    cmd1.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
                                    cmd1.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
                                    cmd1.Parameters.AddWithValue("@uicbal", 0)
                                    cmd1.Parameters.AddWithValue("@uidbal", 0)


                                    Try
                                        cmd1.ExecuteNonQuery()
                                    Catch ex As Exception
                                        Response.Write(ex.ToString)
                                    End Try


                                    If txtproduct.Text = "KMK" Or txtproduct.Text = "SB" Then

                                        If CDbl(reader(2).ToString) > 0 Then

                                            Using conx = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                                                con1.Open()
                                                Using cmdx = New SqlCommand
                                                    cmdx.Connection = con1
                                                    query = "INSERT INTO actransc(Id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                                                    query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"

                                                    cmdx.Parameters.Clear()

                                                    cmdx.CommandText = query
                                                    cmdx.Parameters.AddWithValue("@uiid", Session("tid"))
                                                    cmdx.Parameters.AddWithValue("@uidate", Convert.ToDateTime(txtidate.Text))
                                                    cmdx.Parameters.AddWithValue("@uiacno", Trim(reader(0).ToString))


                                                    cmdx.Parameters.AddWithValue("@uidrd", 0)
                                                    cmdx.Parameters.AddWithValue("@uicrd", CDbl(reader(2).ToString))
                                                    cmdx.Parameters.AddWithValue("@uidrc", 0)
                                                    cmdx.Parameters.AddWithValue("@uicrc", CDbl(reader(2).ToString))
                                                    cmdx.Parameters.AddWithValue("@uinarration", "By CASH")
                                                    cmdx.Parameters.AddWithValue("@uidue", " ")
                                                    cmdx.Parameters.AddWithValue("@uitype", "CASH")
                                                    cmdx.Parameters.AddWithValue("@uisuplimentry", Trim(ach))
                                                    cmdx.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
                                                    cmdx.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
                                                    cmdx.Parameters.AddWithValue("@uicbal", 0)
                                                    cmdx.Parameters.AddWithValue("@uidbal", 0)


                                                    Try
                                                        cmdx.ExecuteNonQuery()
                                                    Catch ex As Exception
                                                        Response.Write(ex.ToString)



                                                    End Try

                                                End Using

                                            End Using



                                            Using conx = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                                                conx.Open()
                                                Using cmdx = New SqlCommand
                                                    cmdx.Connection = con1

                                                    cmdx.Connection = con

                                                    cmdx.Parameters.Clear()
                                                    query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
                                                    query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
                                                    cmdx.CommandText = query
                                                    cmdx.Parameters.AddWithValue("@tid", Session("tid"))
                                                    cmdx.Parameters.AddWithValue("@date", Convert.ToDateTime(txtidate.Text))
                                                    cmdx.Parameters.AddWithValue("@product", txtproduct.Text)
                                                    cmdx.Parameters.AddWithValue("@acno", reader(0).ToString)
                                                    cmdx.Parameters.AddWithValue("cr", 0)
                                                    cmdx.Parameters.AddWithValue("dr", CDbl(reader(2).ToString))


                                                    Try

                                                        cmdx.ExecuteNonQuery()

                                                    Catch ex As Exception
                                                        Response.Write(ex.ToString)

                                                    End Try

                                                End Using
                                            End Using
                                        End If



                                    End If


                                End Using
                            End Using



                        Loop
                    End If
                End Using

            End Using

        End Using



        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                query = "UPDATE TMPINTc SET tmpintc.post=@po,tmpintc.transid=@tid FROM dbo.tmpintc WHERE convert(varchar(20), tdate, 112) ='" + reformatted + "' and dep='" + txtproduct.Text + "'"
                cmd.Connection = con
                cmd.Parameters.AddWithValue("@po", 1)
                cmd.Parameters.AddWithValue("@tid", Session("tid"))
                cmd.CommandText = query

                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    Response.Write(ex.ToString)

                End Try


            End Using
        End Using






    End Sub

    Sub transfer2actrans()

        Dim acn As String
        Dim lonint As Double
        Dim BYC As Double
        Dim ach = get_pro(txtproduct.Text)

        dt = CType(Session("dt"), DataTable)
        For Each row1 As DataRow In dt.Rows

            acn = row1("acn")
            lonint = row1("camt")
            BYC = row1("damt")



            'For i As Integer = 0 To disp.Rows.Count - 1

            '    Dim camt As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblicamt"), Label)
            '    Dim acnx As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblacn"), Label)




            If con.State = ConnectionState.Closed Then con.Open()


            cmd.Connection = con


            cmd.Parameters.Clear()

            'Dim d As String = get_due(txtacn.Text, 0)

            query = "INSERT INTO actrans(Id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@uiid", Session("tid"))
            cmd.Parameters.AddWithValue("@uidate", Convert.ToDateTime(txtidate.Text))
            cmd.Parameters.AddWithValue("@uiacno", Trim(acn))

            If lod = "D" Then
                cmd.Parameters.AddWithValue("@uidrd", 0)


                If Trim(ach) = "SAVINGS DEPOSIT" Or Trim(ach) = "KMK DEPOSIT" Then
                    If BYC = 0 Then
                        cmd.Parameters.AddWithValue("@uicrd", lonint)
                    Else

                        cmd.Parameters.AddWithValue("@uicrd", lonint)

                    End If
                Else
                    cmd.Parameters.AddWithValue("@uicrd", BYC)
                End If
                'cmd.Parameters.AddWithValue("@uicrd", BYC)

                cmd.Parameters.AddWithValue("@uidrc", 0)
                    cmd.Parameters.AddWithValue("@uicrc", lonint)
                    cmd.Parameters.AddWithValue("@uinarration", "By INTEREST")
                Else
                    If Trim(ach) = "JEWEL LOAN" Then
                    cmd.Parameters.AddWithValue("@uidrd", 0)
                Else
                    cmd.Parameters.AddWithValue("@uidrd", BYC)
                End If
                cmd.Parameters.AddWithValue("@uicrd", 0)
                cmd.Parameters.AddWithValue("@uidrc", lonint)
                cmd.Parameters.AddWithValue("@uicrc", 0)
                cmd.Parameters.AddWithValue("@uinarration", "To INTEREST")

            End If
            cmd.Parameters.AddWithValue("@uidue", " ")
            cmd.Parameters.AddWithValue("@uitype", "INTR")
            cmd.Parameters.AddWithValue("@uisuplimentry", Trim(ach))
            cmd.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
            cmd.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
            cmd.Parameters.AddWithValue("@uicbal", 0)
            cmd.Parameters.AddWithValue("@uidbal", 0)


            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)



            Finally

                cmd.Dispose()
                con.Close()
            End Try


            If txtproduct.Text = "KMK" Or txtproduct.Text = "SB" Then

                If BYC > 0 Then

                    If con.State = ConnectionState.Closed Then con.Open()


                    cmd.Connection = con


                    cmd.Parameters.Clear()

                    'Dim d As String = get_due(txtacn.Text, 0)

                    query = "INSERT INTO actrans(Id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                    query &= "VALUES(@uiid,@uidate,@uiacno,@uidrd,@uicrd,@uidrc,@uicrc,@uinarration,@uidue,@uitype,@uisuplimentry,@uisesusr,@uientryat,@uicbal,@uidbal)"


                    cmd.CommandText = query
                    cmd.Parameters.AddWithValue("@uiid", Session("tid"))
                    cmd.Parameters.AddWithValue("@uidate", Convert.ToDateTime(txtidate.Text))
                    cmd.Parameters.AddWithValue("@uiacno", Trim(acn))


                    cmd.Parameters.AddWithValue("@uidrd", 0)
                    cmd.Parameters.AddWithValue("@uicrd", BYC)
                    cmd.Parameters.AddWithValue("@uidrc", 0)
                    cmd.Parameters.AddWithValue("@uicrc", BYC)
                    cmd.Parameters.AddWithValue("@uinarration", "By CASH")
                    cmd.Parameters.AddWithValue("@uidue", " ")
                    cmd.Parameters.AddWithValue("@uitype", "CASH")
                    cmd.Parameters.AddWithValue("@uisuplimentry", Trim(ach))
                    cmd.Parameters.AddWithValue("@uisesusr", Session("sesusr"))
                    cmd.Parameters.AddWithValue("@uientryat", Convert.ToDateTime(Now))
                    cmd.Parameters.AddWithValue("@uicbal", 0)
                    cmd.Parameters.AddWithValue("@uidbal", 0)


                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        Response.Write(ex.ToString)



                    End Try









                    If con.State = ConnectionState.Closed Then con.Open()

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
                    query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
                    cmd.CommandText = query
                    cmd.Parameters.AddWithValue("@tid", Session("tid"))
                    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtidate.Text))
                    cmd.Parameters.AddWithValue("@product", txtproduct.Text)
                    cmd.Parameters.AddWithValue("@acno", Trim(acn))
                    cmd.Parameters.AddWithValue("cr", 0)
                    cmd.Parameters.AddWithValue("dr", BYC)


                    Try

                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)

                    End Try

                End If


            End If



        Next
        Dim dat As DateTime = DateTime.ParseExact(txtidate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()
        query = "UPDATE TMPINT SET tmpint.post=@po,tmpint.transid=@tid FROM dbo.tmpint WHERE convert(varchar(20), tdate, 112) ='" + reformatted + "' and dep='" + txtproduct.Text + "'"

        cmd.Connection = con
        cmd.Parameters.Clear()

        'cmd.Parameters.AddWithValue("@mp", Month(tdate.Text))
        'cmd.Parameters.AddWithValue("@yp", Year(tdate.Text))
        'cmd.Parameters.AddWithValue("@dt", txtday.Text)
        'cmd.Parameters.AddWithValue("@dep", prod.SelectedItem.Text)
        cmd.Parameters.AddWithValue("@po", 1)
        cmd.Parameters.AddWithValue("@tid", Session("tid"))
        cmd.CommandText = query

        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()

        End Try


        'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        '' sb.Append("<div class=" + """container""" + ">")
        'sb.Append("<div class=" + """alert alert-dismissable alert-danger """ + "role=" + """alert""" + ">")
        'sb.Append("<a class=" + """close""" + "data-dismiss=" + """alert""" + ">X</a>")
        'sb.Append("<strong>INTEREST Updated  </strong> ")
        'sb.Append("</div>")
        '' sb.Append("</div>")

        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())

        'disp.DataSource = Nothing
        'disp.DataBind()


        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "INTEREST Posting Completed.Transaction ID : " + Session("tid")
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
    End Sub

    Private Sub txtproduct_TextChanged(sender As Object, e As EventArgs) Handles txtproduct.TextChanged
        txtproduct.Text = txtproduct.Text.ToUpper



        disp4post(txtidate.Text, txtproduct.Text)
    End Sub

    Private Sub inttyp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles inttyp.SelectedIndexChanged

        If inttyp.SelectedItem.Text = "Monthly" Then
            ' intprd.Visible = False
            prd_cap.Visible = False
            fd_day.Visible = True
            runMaturedDepCheckBox.Checked = False
            If session_user_role = "Admin" Then
                runMaturedDepPanel.Visible = True
            End If
            ' cap_fd_day.Visible = True
            'txtfocus(txtday)
            System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtday)

        ElseIf inttyp.SelectedItem.Text = "Yearly" Then
            'intprd.Visible = False
            prd_cap.Visible = False
            fd_day.Visible = False
            'cap_fd_day.Visible = False
            System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.btnSubmit)

        Else
            'intprd.Visible = True
            prd_cap.Visible = True
            fd_day.Visible = False
            'cap_fd_day.Visible = False
            System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtfrm)



        End If
    End Sub

    Private Sub prod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles prod.SelectedIndexChanged

        Select Case prod.SelectedItem.Text
            Case "FD"

                inttyp.Items.Clear()
                inttyp.Items.Add("<-Select->")
                inttyp.Items(0).Value = ""
                inttyp.Items.Add("Monthly")
                inttyp.Items.Add("Yearly")
                inttyp.Items(0).Value = ""
            Case "KMK"
                inttyp.Items.Clear()
                inttyp.Items.Add("<-Select->")
                inttyp.Items(0).Value = ""

                inttyp.Items.Add("Quaterly")

            Case "RD"
                inttyp.Items.Clear()
                inttyp.Items.Add("<-Select->")
                inttyp.Items(0).Value = ""
                inttyp.Items.Add("Yearly")

            Case "RID"
                inttyp.Items.Clear()
                inttyp.Items.Add("<-Select->")
                inttyp.Items(0).Value = ""
                inttyp.Items.Add("Quaterly")

            Case "SB"
                inttyp.Items.Clear()
                inttyp.Items.Add("<-Select->")
                inttyp.Items(0).Value = ""
                inttyp.Items.Add("Half Yearly")


            Case "JL"
                inttyp.Items.Clear()
                inttyp.Items.Add("<-Select->")
                inttyp.Items(0).Value = ""
                inttyp.Items.Add("Yearly")
            Case "DL"
                inttyp.Items.Clear()
                inttyp.Items.Add("<-Select->")
                inttyp.Items(0).Value = ""
                inttyp.Items.Add("Yearly")
            Case "ML"
                inttyp.Items.Clear()
                inttyp.Items.Add("<-Select->")
                inttyp.Items(0).Value = ""
                inttyp.Items.Add("Yearly")





        End Select


    End Sub

    Private Sub txt_si_prod_TextChanged(sender As Object, e As EventArgs) Handles txt_si_prod.TextChanged


    End Sub

    Sub chk_lst()

        If con.State = ConnectionState.Closed Then con.Open()

        Dim dat As DateTime = DateTime.ParseExact(txt_si_date.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)



        query = "select stdinsrd from chklst WHERE convert(varchar(20), date, 112) ='" + reformatted + "'"
        cmd.Connection = con
        cmd.CommandText = query

        Try

            countresult = cmd.ExecuteScalar()

            If Day(txt_si_date.Text) = txt_si_day.Text Then

                If countresult = 0 Then
                    get_data_si()
                Else


                    Dim stitle = "Hi " + Session("sesusr")
                    Dim msg = "Standing Instruction Already Completed."
                    Dim fnc As String = "showToastnOK('" + stitle + "','" + msg + "');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
                    txtfocus(txt_si_date)

                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
                    btn_si_up.Enabled = False
                End If
            Else
                get_data_si()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            cmd.Dispose()
            con.Close()

        End Try


    End Sub
    Sub up_chklst(ByVal prod As String)

        If con.State = ConnectionState.Closed Then con.Open()

        Dim dat As DateTime = DateTime.ParseExact(txt_si_date.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
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
                If prod = "FD" Then
                    query = "update chklst set stdinsfd=1 WHERE convert(varchar(20), date, 112) ='" + reformatted + "'"
                ElseIf prod = "RD" Then
                    query = "update chklst set stdinsrd=1 WHERE convert(varchar(20), date, 112) ='" + reformatted + "'"

                End If

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

            Else

                dr.Close()
                cmd.Dispose()

                If prod = "FD" Then
                    query = "insert into chklst(date,stdinsfd)"
                    query &= "values (@date,@intpost)"
                ElseIf prod = "RD" Then
                    query = "insert into chklst(date,stdinsrd)"
                    query &= "values (@date,@intpost)"

                End If
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                cmd.Parameters.AddWithValue("@intpost", 1)
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

            End If

        Catch ex As Exception

            Response.Write(ex.ToString)


        End Try

    End Sub

    Private Sub btn_si_up_Click(sender As Object, e As EventArgs) Handles btn_si_up.Click

        If txt_si_prod.Text = "FD" Then


            Dim countresult As Integer
            Dim prodt = get_pro(txt_si_prod.Text)
            If con.State = ConnectionState.Closed Then con.Open()
            cmd.Connection = con
            cmd.Parameters.Clear()
            query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal,denom)"
            query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal,@denom)"
            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", "")
            cmd.Parameters.AddWithValue("@drd", CDbl(lbl_si_cint_total.Text))
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", CDbl(lbl_si_cint_total.Text))
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Transfer")
            cmd.Parameters.AddWithValue("@due", "")
            cmd.Parameters.AddWithValue("@type", "TRF")
            cmd.Parameters.AddWithValue("@suplimentry", prodt)
            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
            cmd.Parameters.AddWithValue("@cbal", 0)
            cmd.Parameters.AddWithValue("@dbal", 0)
            cmd.Parameters.AddWithValue("@denom", 1)
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)

            Finally
                cmd.Dispose()
                con.Close()
            End Try


            If con.State = ConnectionState.Closed Then con.Open()


            query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + Trim(prodt) + "'"
            cmd.Connection = con
            cmd.CommandText = query
            countresult = cmd.ExecuteScalar()
            Try
                Session("tid") = Convert.ToString(countresult)
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try



            If txt_si_prod.Text = "FD" Then

                update_suplementry(Session("tid"), prodt, 0, CDbl(lbl_si_cint_total.Text), "TO TRANSFER", "TRF")

                update_suplementry(Session("tid"), "SAVINGS DEPOSIT", CDbl(lbl_si_cint_total.Text), 0, "BY TRANSFER", "TRF")

                update_suplementryc(Session("tid"), prodt, 0, CDbl(lbl_si_cint_total.Text), "TO TRANSFER", "TRF")

                update_suplementryc(Session("tid"), "SAVINGS DEPOSIT", CDbl(lbl_si_cint_total.Text), 0, "BY TRANSFER", "TRF")


                up_chklst("FD")

                If Not CDbl(lbl_si_bycas_total.Text) = 0 Then

                    update_suplementry(Session("tid"), "SAVINGS DEPOSIT", CDbl(lbl_si_bycas_total.Text), 0, "BY CASH", "RECEIPT")
                    update_suplementryc(Session("tid"), "SAVINGS DEPOSIT", CDbl(lbl_si_bycas_total.Text), 0, "BY CASH", "RECEIPT")
                End If
            ElseIf txt_si_prod.Text = "RD" Then

                up_chklst("RD")
                update_suplementry(Session("tid"), "SAVINGS DEPOSIT", 0, CDbl(lbl_si_cint_total.Text), "TO TRANSFER", "TRF")

                update_suplementry(Session("tid"), prodt, CDbl(lbl_si_cint_total.Text), 0, "BY TRANSFER", "TRF")

                update_suplementryc(Session("tid"), "SAVINGS DEPOSIT", 0, CDbl(lbl_si_cint_total.Text), "TO TRANSFER", "TRF")

                update_suplementryc(Session("tid"), prodt, CDbl(lbl_si_cint_total.Text), 0, "BY TRANSFER", "TRF")

            End If


            update_data_si_c()

            For i As Integer = 0 To disp_si.Rows.Count - 1




                Dim fdacn As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblacn"), Label)
                Dim sbacn As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblsiacno"), Label)
                Dim camt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_cint"), Label)
                Dim damt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_dint"), Label)


                If con.State = ConnectionState.Closed Then con.Open()

                cmd.Connection = con
                cmd.Parameters.Clear()


                query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@id", Session("tid"))
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                cmd.Parameters.AddWithValue("@acno", fdacn.Text)
                cmd.Parameters.AddWithValue("@drd", CDbl(camt.Text) + CDbl(damt.Text))
                cmd.Parameters.AddWithValue("@crd", 0)
                cmd.Parameters.AddWithValue("@drc", CDbl(camt.Text))
                cmd.Parameters.AddWithValue("@crc", 0)
                cmd.Parameters.AddWithValue("@narration", "To Transfer")
                cmd.Parameters.AddWithValue("@due", sbacn.Text)
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@suplimentry", prodt)
                cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
                cmd.Parameters.AddWithValue("@cbal", 0)
                cmd.Parameters.AddWithValue("@dbal", 0)


                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception


                    Response.Write(ex.Message)

                Finally
                    cmd.Dispose()
                    con.Close()
                End Try

                If con.State = ConnectionState.Closed Then con.Open()

                cmd.Connection = con
                cmd.Parameters.Clear()


                query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


                cmd.CommandText = query
                cmd.Parameters.AddWithValue("@id", Session("tid"))
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                cmd.Parameters.AddWithValue("@acno", sbacn.Text)
                cmd.Parameters.AddWithValue("@drd", 0)
                cmd.Parameters.AddWithValue("@crd", CDbl(camt.Text))
                cmd.Parameters.AddWithValue("@drc", 0)
                cmd.Parameters.AddWithValue("@crc", CDbl(camt.Text))
                cmd.Parameters.AddWithValue("@narration", "By Transfer")
                cmd.Parameters.AddWithValue("@due", fdacn.Text)
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@suplimentry", "SAVINGS DEPOSIT")
                cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
                cmd.Parameters.AddWithValue("@cbal", 0)
                cmd.Parameters.AddWithValue("@dbal", 0)


                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception


                    MsgBox(ex.Message)

                Finally
                    cmd.Dispose()
                    con.Close()
                End Try


                If Not CDbl(damt.Text) = 0 Then

                    If con.State = ConnectionState.Closed Then con.Open()

                    cmd.Parameters.Clear()

                    cmd.Connection = con
                    query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                    query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


                    cmd.CommandText = query
                    cmd.Parameters.AddWithValue("@id", Session("tid"))
                    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                    cmd.Parameters.AddWithValue("@acno", sbacn.Text)
                    cmd.Parameters.AddWithValue("@drd", 0)
                    cmd.Parameters.AddWithValue("@crd", CDbl(damt.Text))
                    cmd.Parameters.AddWithValue("@drc", 0)
                    cmd.Parameters.AddWithValue("@crc", CDbl(damt.Text))
                    cmd.Parameters.AddWithValue("@narration", "By CASH")
                    cmd.Parameters.AddWithValue("@due", fdacn.Text)
                    cmd.Parameters.AddWithValue("@type", "CASH")
                    cmd.Parameters.AddWithValue("@suplimentry", "SAVINGS DEPOSIT")
                    cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                    cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
                    cmd.Parameters.AddWithValue("@cbal", 0)
                    cmd.Parameters.AddWithValue("@dbal", 0)


                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception


                        MsgBox(ex.Message)

                    Finally
                        cmd.Dispose()
                        con.Close()
                    End Try

                    If con.State = ConnectionState.Closed Then con.Open()

                    cmd.Parameters.Clear()

                    cmd.Connection = con
                    query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
                    query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


                    cmd.CommandText = query
                    cmd.Parameters.AddWithValue("@id", Session("tid"))
                    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
                    cmd.Parameters.AddWithValue("@acno", sbacn.Text)
                    cmd.Parameters.AddWithValue("@drd", 0)
                    cmd.Parameters.AddWithValue("@crd", CDbl(damt.Text))
                    cmd.Parameters.AddWithValue("@drc", 0)
                    cmd.Parameters.AddWithValue("@crc", CDbl(damt.Text))
                    cmd.Parameters.AddWithValue("@narration", "By CASH")
                    cmd.Parameters.AddWithValue("@due", fdacn.Text)
                    cmd.Parameters.AddWithValue("@type", "CASH")
                    cmd.Parameters.AddWithValue("@suplimentry", "SAVINGS DEPOSIT")
                    cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                    cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
                    cmd.Parameters.AddWithValue("@cbal", 0)
                    cmd.Parameters.AddWithValue("@dbal", 0)


                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception


                        MsgBox(ex.Message)

                    Finally
                        cmd.Dispose()
                        con.Close()
                    End Try

                    If con.State = ConnectionState.Closed Then con.Open()

                    cmd.Connection = con

                    cmd.Parameters.Clear()
                    query = "INSERT INTO diff(tid,date,product,acno,dr,cr)"
                    query &= "VALUES(@tid,@date,@product,@acno,@dr,@cr)"
                    cmd.CommandText = query
                    cmd.Parameters.AddWithValue("@tid", Session("tid"))
                    cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtidate.Text))
                    cmd.Parameters.AddWithValue("@product", "FD")
                    cmd.Parameters.AddWithValue("@acno", fdacn.Text)
                    cmd.Parameters.AddWithValue("cr", 0)
                    cmd.Parameters.AddWithValue("dr", CDbl(damt.Text))


                    Try

                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        Response.Write(ex.ToString)

                    End Try






                End If







            Next


            Dim dat As DateTime = DateTime.ParseExact(txt_si_date.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
            Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            If con.State = ConnectionState.Closed Then con.Open()
            query = "UPDATE TMPINT SET tmpint.si=@po,tmpint.sitid=@tid FROM dbo.tmpint WHERE convert(varchar(20), tdate, 112) ='" + reformatted + "' and dep='" + txt_si_prod.Text + "'"

            cmd.Connection = con
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@po", 1)
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.CommandText = query

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.Message)
            Finally
                cmd.Dispose()
                con.Close()

            End Try
            If con.State = ConnectionState.Closed Then con.Open()
            query = "UPDATE TMPINTc SET tmpintc.si=@po,tmpintc.sitid=@tid FROM dbo.tmpintc WHERE convert(varchar(20), tdate, 112) ='" + reformatted + "' and dep='" + txt_si_prod.Text + "'"

            cmd.Connection = con
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@po", 1)
            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.CommandText = query

            Try
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.Message)
            Finally
                cmd.Dispose()
                con.Close()

            End Try

            If CDbl(lbl_si_bycas_total.Text) <> 0 Then

            End If



            Dim stitle = "Hi " + Session("sesusr")
            Dim msg = "Standing Instruction  Completed.Transaction ID : " + Session("tid")
            Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
            'Me.ModalPopup1.Show()

            disp_si.DataSource = Nothing
            disp_si.DataBind()
            txt_si_date.Text = ""
            txt_si_prod.Text = ""
            lbl_si_bycas_total.Text = 0
            lbl_si_cint_total.Text = 0


        ElseIf txt_si_prod.Text = "RD" Then
            update_si_rd()

        End If



    End Sub

    Function get_due(ByVal acn As String, ByVal curdue As Integer)

        Dim df As String = "dMMM-yyyy"
        Dim op As String
        Dim opt As DateTime

        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "SELECT COUNT(*) FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND [actrans].type<>'INTR' "

        cmdi.Connection = con
        cmdi.CommandText = sql

        countresult = cmdi.ExecuteScalar()

        If countresult = 0 Then

            opt = Convert.ToDateTime(Session("ac_date"))
            op = opt.ToString("y")

        Else

            Dim curdue_period As Date = DateAdd(DateInterval.Month, (countresult), Session("ac_date"))

            op = curdue_period.ToString("y")


        End If

        Return op
    End Function


    Sub update_si_rd()
        Dim countresult As Integer
        Dim prodt = get_pro(txt_si_prod.Text)
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal,denom)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal,@denom)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@acno", "")
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", CDbl(lbl_si_cint_total.Text))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", CDbl(lbl_si_cint_total.Text))
        cmd.Parameters.AddWithValue("@narration", "By Transfer")
        cmd.Parameters.AddWithValue("@due", "")
        cmd.Parameters.AddWithValue("@type", "TRF")
        cmd.Parameters.AddWithValue("@suplimentry", prodt)
        cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
        cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
        cmd.Parameters.AddWithValue("@cbal", 0)
        cmd.Parameters.AddWithValue("@dbal", 0)
        cmd.Parameters.AddWithValue("@denom", 1)
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            cmd.Dispose()
            con.Close()
        End Try


        If con.State = ConnectionState.Closed Then con.Open()


        query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + Trim(prodt) + "'"
        cmd.Connection = con
        cmd.CommandText = query
        countresult = cmd.ExecuteScalar()
        Try
            Session("tid") = Convert.ToString(countresult)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try



        If Mid(txt_si_date.Text, 1, 2) = txt_si_day.Text Then
            up_chklst("RD")
        End If
        update_suplementry(Session("tid"), "SAVINGS DEPOSIT", 0, CDbl(lbl_si_cint_total.Text), "TO TRANSFER", "TRF")

        update_suplementry(Session("tid"), prodt, CDbl(lbl_si_cint_total.Text), 0, "BY TRANSFER", "TRF")

        update_suplementryc(Session("tid"), "SAVINGS DEPOSIT", 0, CDbl(lbl_si_cint_total.Text), "TO TRANSFER", "TRF")

        update_suplementryc(Session("tid"), prodt, CDbl(lbl_si_cint_total.Text), 0, "BY TRANSFER", "TRF")


        For i As Integer = 0 To disp_si.Rows.Count - 1

            Dim rdacn As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblacn"), Label)
            Dim sbacn As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblsiacno"), Label)
            Dim camt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_cint"), Label)
            Dim damt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_dint"), Label)
            Dim acdate As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblacdate"), Label)



            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()

            Session("ac_date") = Convert.ToDateTime(acdate.Text)
            Dim d As String = get_due(rdacn.Text, i - 1)

            query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"


            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", rdacn.Text)
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", CDbl(camt.Text))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", CDbl(camt.Text))
            cmd.Parameters.AddWithValue("@narration", "By Transfer")
            cmd.Parameters.AddWithValue("@due", (d + " " + Convert.ToString(sbacn.Text)))
            cmd.Parameters.AddWithValue("@type", "TRF")
            cmd.Parameters.AddWithValue("@sup", "RECURRING DEPOSIT")
            cmd.Parameters.AddWithValue("@usr", Session("sesusr"))
            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
            cmd.Parameters.AddWithValue("@cbal", 0)
            cmd.Parameters.AddWithValue("@dbal", 0)


            Try
                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                cmd.CommandText = query
                cmd.ExecuteNonQuery()

            Catch EX As Exception

                Response.Write(EX.Message)

            Finally

                cmd.Parameters.Clear()

                cmd.Dispose()
                con.Close()

            End Try


            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Connection = con
            cmd.Parameters.Clear()


            query = "INSERT INTO actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@id", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", sbacn.Text)
            cmd.Parameters.AddWithValue("@drd", CDbl(camt.Text))
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", CDbl(camt.Text))
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Transfer")
            cmd.Parameters.AddWithValue("@due", rdacn.Text)
            cmd.Parameters.AddWithValue("@type", "TRF")
            cmd.Parameters.AddWithValue("@suplimentry", "SAVINGS DEPOSIT")
            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
            cmd.Parameters.AddWithValue("@cbal", 0)
            cmd.Parameters.AddWithValue("@dbal", 0)


            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                MsgBox(ex.Message)

            Finally
                cmd.Dispose()
                con.Close()
            End Try




        Next


        For i As Integer = 0 To disp_si.Rows.Count - 1

            Dim rdacn As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblacn"), Label)
            Dim sbacn As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblsiacno"), Label)
            Dim camt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_cint"), Label)
            Dim damt As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lbl_dint"), Label)
            Dim acdate As Label = DirectCast(disp_si.Rows(i).Cells(0).FindControl("lblacdate"), Label)



            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Parameters.Clear()

            Session("ac_date") = Convert.ToDateTime(acdate.Text)
            Dim d As String = get_due(rdacn.Text, i - 1)

            query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@tid,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sup,@usr,@entryat,@cbal,@dbal)"


            cmd.Parameters.AddWithValue("@tid", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", rdacn.Text)
            cmd.Parameters.AddWithValue("@drd", 0)
            cmd.Parameters.AddWithValue("@crd", CDbl(camt.Text))
            cmd.Parameters.AddWithValue("@drc", 0)
            cmd.Parameters.AddWithValue("@crc", CDbl(camt.Text))
            cmd.Parameters.AddWithValue("@narration", "By Transfer")
            cmd.Parameters.AddWithValue("@due", (d + " " + Convert.ToString(sbacn.Text)))
            cmd.Parameters.AddWithValue("@type", "TRF")
            cmd.Parameters.AddWithValue("@sup", "RECURRING DEPOSIT")
            cmd.Parameters.AddWithValue("@usr", Session("sesusr"))
            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
            cmd.Parameters.AddWithValue("@cbal", 0)
            cmd.Parameters.AddWithValue("@dbal", 0)


            Try
                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Connection = con
                cmd.CommandText = query
                cmd.ExecuteNonQuery()

            Catch EX As Exception

                Response.Write(EX.Message)

            Finally

                cmd.Parameters.Clear()

                cmd.Dispose()
                con.Close()

            End Try


            If con.State = ConnectionState.Closed Then con.Open()

            cmd.Connection = con
            cmd.Parameters.Clear()


            query = "INSERT INTO actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
            query &= "VALUES(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@id", Session("tid"))
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(tdate.Text))
            cmd.Parameters.AddWithValue("@acno", sbacn.Text)
            cmd.Parameters.AddWithValue("@drd", CDbl(camt.Text))
            cmd.Parameters.AddWithValue("@crd", 0)
            cmd.Parameters.AddWithValue("@drc", CDbl(camt.Text))
            cmd.Parameters.AddWithValue("@crc", 0)
            cmd.Parameters.AddWithValue("@narration", "To Transfer")
            cmd.Parameters.AddWithValue("@due", rdacn.Text)
            cmd.Parameters.AddWithValue("@type", "TRF")
            cmd.Parameters.AddWithValue("@suplimentry", "SAVINGS DEPOSIT")
            cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
            cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))
            cmd.Parameters.AddWithValue("@cbal", 0)
            cmd.Parameters.AddWithValue("@dbal", 0)


            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception


                MsgBox(ex.Message)

            Finally
                cmd.Dispose()
                con.Close()
            End Try




        Next


        Dim stitle = "Hi " + Session("sesusr")
        Dim msg = "Standing Instruction  Completed.Transaction ID : " + Session("tid")
        Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        disp_si.DataSource = Nothing
        disp_si.DataBind()
        txt_si_date.Text = ""
        txt_si_prod.Text = ""
        lbl_si_bycas_total.Text = 0
        lbl_si_cint_total.Text = 0



    End Sub
    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.Parameters.Clear()

        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@transid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        cmd.Parameters.AddWithValue("@nar", nar)
        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
            cmd.Dispose()
            con.Close()


        End Try

        query = ""



    End Sub


    Private Sub update_suplementryc(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)

        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.Parameters.Clear()

        query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@sdt,@transid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(tdate.Text))
        cmd.Parameters.AddWithValue("@transid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        cmd.Parameters.AddWithValue("@nar", nar)
        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
            cmd.Dispose()
            con.Close()


        End Try

        query = ""



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
    Public Function get_ob(ByVal acn As String, ByVal reformatted As String)



        If con1.State = ConnectionState.Closed Then con1.Open()


        Dim sql As String = "SELECT SUM([actrans].Drd) AS expr1,SUM([actrans].Crd) AS expr2,SUM([actrans].Drc) AS expr3,SUM([actrans].Crc) AS expr4,[actrans].acno FROM dbo.[actrans] WHERE [actrans].acno ='" + acn + "' AND CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' GROUP BY [actrans].acno"
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
                Session("dbal") = reader1(1) - reader1(0)
                Session("cbal") = reader1(3) - reader1(2)



            Else
                Session("cbal") = 0
                Session("dbal") = 0
            End If

            reader1.Close()

        Catch ex As Exception
            Response.Write(ex.Message)

        Finally
            cmd.Dispose()
            con1.Close()


        End Try


        Return Session("cbal")

    End Function

    Function get_kmkmin(ByVal frmdt As String, ByVal tdt As String)

        Dim kmkmin As Integer = 0

        Dim dat As DateTime = DateTime.ParseExact(frmdt, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(tdt, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()



        Dim ob As Double = get_ob(Session("acn"), reformatted)
        Dim day_total As Double = 0

        kmkmin = ob

        If con1.State = ConnectionState.Closed Then con1.Open()

        cmd.Parameters.Clear()


        Dim dr As SqlDataReader

        query = "SELECT actrans.Drc, actrans.Crc, actrans.date FROM dbo.actrans WHERE actrans.acno = @acn AND CONVERT(VARCHAR(20), date, 112) BETWEEN @fdt AND @tdt"

        cmd.Parameters.AddWithValue("@acn", Session("acn"))
        cmd.Parameters.AddWithValue("@fdt", reformatted)
        cmd.Parameters.AddWithValue("@tdt", reformatted1)
        cmd.CommandText = query
        cmd.Connection = con1

        Try

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                ' dr.Read()

                While dr.Read()

                    day_total = ob + IIf(IsDBNull(dr(1)), 0, dr(1)) - IIf(IsDBNull(dr(0)), 0, dr(0))

                    If day_total < kmkmin Then kmkmin = day_total
                    ob = day_total

                End While

            End If

            dr.Close()



        Catch ex As Exception
            Response.Write(ex.Message)

        Finally
            con1.Close()
            cmd.Dispose()

        End Try


        Return kmkmin
    End Function
    Sub sb_closure(ByVal acn As String, ByVal cintr As Double)
        Dim kmkmin4int As Integer = 0
        Dim cur_kmkmin As Integer = 0
        Dim curmin As Integer = 0
        Dim tdta As Date
        Dim frmdt As String
        Dim tdt As String
        Dim fdt As String
        Dim cur_int As Double = 0
        Dim cum_int As Double = 0
        Dim cum_d As Double = 0
        Dim cur_int_d As Double = 0
        Session("prdtyp") = "0"
        Session("prd") = DateDiff(DateInterval.Day, Session("ac_date"), Convert.ToDateTime(txtto.Text))
        'cintr = get_dint()



        Session("prv_inton") = Convert.ToDateTime(txtfrm.Text) 'get_prev_inton(acn)
        Session("voucher_date") = Convert.ToDateTime(txtto.Text)

        While Session("prv_inton") <= Session("voucher_date")

            frmdt = Session("prv_inton")
            '  tdt = DateAdd(DateInterval.Day, 6, prv_inton)

            Dim days As Integer = DateTime.DaysInMonth(Year(Session("prv_inton")), Month(Session("prv_inton")))

            '' tdta = DateAdd(DateInterval.Day, days, Session("prv_inton"))
            ''  tdt = tdta

            tdt = DateSerial(Year(frmdt), Month(frmdt) + 1, 0)
            fdt = DateSerial(Year(frmdt), Month(frmdt), 0)
            tdta = tdt
            '   kmkmin4int = get_kmkmin(frmdt, tdt)
            kmkmin4int = get_kmkmin(Convert.ToDateTime(fdt).AddDays(11), tdt)

            If Not kmkmin4int = 0 Then cur_kmkmin = kmkmin4int

            If Not cur_kmkmin <= 200 Then

                '   cur_int = (cur_kmkmin * cintr / 100) / 12
                '  cum_int = cum_int + cur_int




                If cur_kmkmin < 100000 Then

                    cur_int = (cur_kmkmin * cintr / 100) / 12
                    cum_int = cum_int + cur_int
                    cur_int = 0
                Else

                    cur_int = (100000 * cintr / 100) / 12
                    cum_int = cum_int + cur_int
                    cur_int_d = ((cur_kmkmin - 100000) * cintr / 100) / 12
                    cum_d = cum_d + cur_int_d

                End If
                If Session("prv_inton") > Session("voucher_date") Then
                    cum_int = cum_int - cur_int
                    cum_d = cum_d - cur_int_d
                End If
                cur_int = 0
                cur_int_d = 0
                Session("prv_inton") = tdta.AddDays(1) 'tdt 'Co
                ' Session("prv_inton") = tdt
            Else
                Session("prv_inton") = Convert.ToDateTime(tdt).AddDays(1)
            End If





        End While


        cum_int = Math.Round(cum_int)
        cum_d = Math.Round(cum_d)
        If Not cum_int = 0 Then
            Dim dayInterval = DateDiff(DateInterval.Day, Convert.ToDateTime(txtfrm.Text), Session("voucher_date"))
            newrow = dt.NewRow()
            newrow(0) = Session("ac_date")
            newrow(1) = acn
            newrow(2) = Session("amt")
            newrow(3) = dayInterval
            newrow(4) = cintr
            newrow(5) = cum_int
            newrow(6) = cum_d
            dt.Rows.Add(newrow)

            If con1.State = ConnectionState.Closed Then con1.Open()

            cmd1.Connection = con1

            cmd1.Parameters.Clear()

            query = "insert into tmpint(tdate,acn,dep,acdate,amt,prd,cint,camt,dint,damt)"
            query &= "values(@tdate,@acn,@dep,@acdate,@amt,@prd,@cint,@camt,@dint,@damt)"
            cmd1.CommandText = query
            cmd1.Parameters.AddWithValue("@tdate", Convert.ToDateTime(tdate.Text))
            cmd1.Parameters.AddWithValue("@acn", acn)
            cmd1.Parameters.AddWithValue("@dep", Trim(prod.SelectedItem.Text))
            cmd1.Parameters.AddWithValue("@acdate", Session("ac_date"))
            cmd1.Parameters.AddWithValue("@amt", Session("amt"))
            cmd1.Parameters.AddWithValue("@prd", dayInterval)
            cmd1.Parameters.AddWithValue("@cint", cintr)
            cmd1.Parameters.AddWithValue("@camt", cum_int)
            cmd1.Parameters.AddWithValue("@dint", cintr)
            cmd1.Parameters.AddWithValue("@damt", cum_d)

            Try
                cmd1.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con1.Close()
                cmd1.Dispose()
                cmd1.Parameters.Clear()

            End Try
        End If

    End Sub
    Sub kmk_closure(ByVal acn As String, ByVal cintr As Double)

        Dim kmkmin4int As Integer = 0
        Dim cur_kmkmin As Integer = 0
        Dim curmin As Integer = 0

        Dim frmdt As String
        Dim tdt As String
        Dim cur_int As Double = 0
        Dim cur_int_d As Double = 0
        Dim cum_int As Double = 0
        Dim cum_d As Double = 0
        Session("prdtyp") = "0"
        Session("prd") = DateDiff(DateInterval.Day, Session("ac_date"), Convert.ToDateTime(txtto.Text))
        'cintr = get_dint()



        Session("prv_inton") = Convert.ToDateTime(txtfrm.Text).AddDays(-1) 'get_prev_inton(acn)
        Session("voucher_date") = Convert.ToDateTime(txtto.Text)

        While (Session("prv_inton") <= Session("voucher_date"))

            frmdt = Convert.ToDateTime(Session("prv_inton")).AddDays(1)
            tdt = Convert.ToDateTime(frmdt).AddDays(7) 'DateAdd(DateInterval.Day, 6, Session("prv_inton"))

            kmkmin4int = get_kmkmin(frmdt, tdt)
            If Not kmkmin4int = 0 Then cur_kmkmin = kmkmin4int

            If Not cur_kmkmin <= 200 Then


                If cur_kmkmin < 100000 Then

                    cur_int = (cur_kmkmin * cintr / 100) / 12 / 4
                    cum_int = cum_int + cur_int
                Else

                    cur_int = (100000 * cintr / 100) / 12 / 4
                    cum_int = cum_int + cur_int
                    cur_int_d = ((cur_kmkmin - 100000) * cintr / 100) / 12 / 4
                    cum_d = cum_d + cur_int_d

                End If




                If Session("prv_inton") > Session("voucher_date") Then
                    cum_int = cum_int - cur_int
                    cum_d = cum_d - cur_int_d
                End If
                cur_int = 0
                cur_int_d = 0
                Session("prv_inton") = tdt 'Convert.ToDateTime(tdt).AddDays(1)
            Else
                Session("prv_inton") = Convert.ToDateTime(tdt).AddDays(1)
            End If



        End While


        cum_int = Math.Round(cum_int)
        cum_d = Math.Round(cum_d)
        If Not cum_int = 0 Then
            newrow = dt.NewRow()
            newrow(0) = Session("ac_date")
            newrow(1) = acn
            newrow(2) = Session("amt")
            newrow(3) = DateDiff(DateInterval.Day, Convert.ToDateTime(txtfrm.Text), Session("voucher_date"))
            newrow(4) = cintr
            newrow(5) = cum_int
            newrow(6) = cum_d
            dt.Rows.Add(newrow)

            If con1.State = ConnectionState.Closed Then con1.Open()

            cmd1.Connection = con1

            cmd1.Parameters.Clear()

            query = "insert into tmpint(tdate,acn,dep,acdate,amt,prd,cint,camt,dint,damt)"
            query &= "values(@tdate,@acn,@dep,@acdate,@amt,@prd,@cint,@camt,@dint,@damt)"
            cmd1.CommandText = query
            cmd1.Parameters.AddWithValue("@tdate", Convert.ToDateTime(tdate.Text))
            cmd1.Parameters.AddWithValue("@acn", acn)
            cmd1.Parameters.AddWithValue("@dep", Trim(prod.SelectedItem.Text))
            cmd1.Parameters.AddWithValue("@acdate", Session("ac_date"))
            cmd1.Parameters.AddWithValue("@amt", cur_kmkmin)
            cmd1.Parameters.AddWithValue("@prd", DateDiff(DateInterval.Day, Convert.ToDateTime(txtfrm.Text), Session("voucher_date")))
            cmd1.Parameters.AddWithValue("@cint", cintr)
            cmd1.Parameters.AddWithValue("@camt", cum_int)
            cmd1.Parameters.AddWithValue("@dint", cintr)
            cmd1.Parameters.AddWithValue("@damt", cum_d)

            Try
                cmd1.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                con1.Close()
                cmd1.Dispose()
                cmd1.Parameters.Clear()

            End Try

        End If

    End Sub
    Function prv_int(ByVal acn As String, ByVal ac_date As Date)
        Dim oresult As Date

        If con1.State = ConnectionState.Closed Then con1.Open()
        cmd.Connection = con1
        cmd.Parameters.Clear()

        query = "SELECT TOP 1 actrans.date FROM dbo.actrans WHERE actrans.Drc > 0 AND actrans.Type = 'INTR' AND actrans.acno = @acn ORDER BY actrans.date DESC"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@acn", acn)

        Try
            oresult = cmd.ExecuteScalar()
            If oresult = Date.MinValue Then
                Session("prv_inton") = ac_date
            Else
                Session("prv_inton") = oresult
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con1.Close()

        End Try

        Return Session("prv_inton")


    End Function
    Function get_share_value(ByVal cid As String)
        Dim shr As Integer = 0
        If con1.State = ConnectionState.Closed Then con1.Open()

        query = "SELECT SUM(share.allocation) AS expr1 FROM dbo.share WHERE share.cid = @cid"
        cmd1.Connection = con1
        cmd1.Parameters.Clear()
        cmd1.CommandText = query
        cmd1.Parameters.AddWithValue("@cid", cid)

        Try
            Dim obj As Object = cmd1.ExecuteScalar()
            If Not IsDBNull(obj) Then
                shr = obj
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        con1.Close()
        cmd1.Dispose()


        Return shr
    End Function


    Function get_int(ByVal cbal As Double, ByVal acn As String, ByVal ac_date As Date)

        Session("totint") = 0
        Try
            Session("prv_inton") = prv_int(acn, ac_date)

            If prod.SelectedItem.Text = "DCL" Then


                Session("prd") = DateDiff(DateInterval.Day, ac_date, CDate(tdate.Text))
                'Else

                If Session("prd") < 120 Then


                    GoTo nxt
                Else

                    Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(tdate.Text))

                    If Session("prd_buffer") > 120 Then Session("prd_buffer") = Session("prd_buffer") - 120

                End If

            Else
                Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(tdate.Text))
            End If

            '   End If
            '  prd_buffer_d = DateDiff(DateInterval.Day, prv_dinton, CDate(tdate.Text))

            'If prod.SelectedItem.Text = "DCL" Then

            '    If Session("prd_buffer") <= 120 Then

            '        GoTo nxt
            '    Else
            '        Session("prd_buffer") = Session("prd_buffer") - 120
            '    End If
            'End If



            Dim shrval As Integer = get_share_value(acn)



            '   If Session("prd_buffer") = 0 Then Session("prd_buffer") = 1
            ' If prd_buffer_d = 0 Then prd_buffer_d = 7
            If prod.SelectedItem.Text = "ML" Then

                'query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod  order by fyfrm"


                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy AND minsc=@shr AND agst=@agst order by fyfrm"



            ElseIf prod.SelectedItem.Text = "JL" Then
                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod and roi.agst=@agst AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
            ElseIf prod.SelectedItem.Text = "DL" Then
                GoTo nxt1
            Else
                query = "SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy order by fyfrm"
            End If
            Dim dr_roi As SqlDataReader


            If con1.State = ConnectionState.Closed Then con1.Open()

            cmd.Parameters.Clear()
            cmd.Connection = con1


            cmd.CommandText = query
            cmd.Parameters.AddWithValue("@prod", prod.SelectedItem.Text)
            '       If Not prod.SelectedItem.Text = "ML" Then
            cmd.Parameters.AddWithValue("@prdtyp", "D")
            cmd.Parameters.AddWithValue("@prdx", Session("prd_buffer"))
            cmd.Parameters.AddWithValue("@prdy", Session("prd_buffer"))
            'End If

            If prod.SelectedItem.Text = "JL" Or prod.SelectedItem.Text = "ML" Then
                cmd.Parameters.AddWithValue("@agst", Session("sch"))
            End If
            If shrval < 500 Then
                cmd.Parameters.AddWithValue("@shr", 49)
            Else
                cmd.Parameters.AddWithValue("@shr", 51)
            End If



            dr_roi = cmd.ExecuteReader()

            If dr_roi.HasRows() Then
                ' dr_roi.Read()

                While dr_roi.Read

                    Dim FYFRM As Date = dr_roi(2)
                    Dim FYTO As Date = dr_roi(3)


                    Dim x As Long = FYFRM.CompareTo(ac_date)

                    If x = -1 Then


                        Dim y As Long = FYTO.CompareTo(ac_date)

                        If y = 1 Then
                            Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
                            Session("dintr") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))
                            Exit While

                        End If

                    End If
                End While


            End If

            dr_roi.Close()


nxt1:
            If prod.SelectedItem.Text = "DL" Then get_dl_int(acn)



            If Session("prv_inton").Equals(ac_date) Then

                '  If Session("prd_buffer") <= 7 Then Session("prd_buffer") = 7


                ' If prd_buffer <= 7 Then prd_buffer = 7

                ' If prd_buffer_d <= 7 Then prd_buffer_d = 7

            End If


            If prod.SelectedItem.Text = "DCL" Then

                Session("cintr") = 15
                Session("dintr") = 27.6
                Session("totint") = Math.Round((((cbal) * Session("cintr") / 100) / 365) * Session("prd_buffer"))
                Session("totint_d") = Math.Round((((cbal) * Session("dintr") / 100) / 365) * Session("prd_buffer"))

            Else

                Dim dbal As Double = get_balance(acn)
                If dbal < 0 Then dbal = dbal * -1
                '  If Session("prd_buffer") = 1 Then Session("prd_buffer") = 0
                Session("totint") = Math.Round((((cbal) * Session("cintr") / 100) / 365) * Session("prd_buffer"))
                Session("totint_d") = Math.Round((((dbal) * Session("dintr") / 100) / 365) * Session("prd_buffer"))

            End If

            If Not Session("prd_buffer") = 0 Then

                '    If Session("totint") < 5 Then Session("totint") = 5
                '   If Session("totint_d") < 5 Then Session("totint_d") = 5
            End If



        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con1.Close()

        End Try
nxt:
        Return Session("totint")

    End Function
    Sub get_dl_int(ByVal acn As String)


        Dim mnt As Integer

        Dim pro As String = get_dep_agst(acn)



        Session("prd_buffer") = DateDiff(DateInterval.Day, Session("prv_inton"), CDate(tdate.Text))
        Session("prd_buffer_d") = DateDiff(DateInterval.Day, Session("prv_dinton"), CDate(tdate.Text))

        '  If Trim(acn) = "62201800089" Then
        'acn = "62201800089"
        'End If



        If Session("prd_buffer_d") > 30 Then

            mnt = Math.Round(CType(Session("prd_buffer_d"), Integer) / 30)
            mnt = Session("agst_prd")
        Else
            mnt = 1
        End If

        query = "select cint,dint from master where acno='" + acn + "'" '"SELECT cint,dint,FYFRM,FYTO FROM dbo.roi WHERE roi.Product = @prod AND roi.prddmy =  @prdtyp AND roi.prdfrm <=  @prdx AND roi.prdto >=  @prdy and agst=@agst  order by fyfrm"

        Dim dr_roi As SqlDataReader


        If con1.State = ConnectionState.Closed Then con1.Open()

        cmd1.Parameters.Clear()
        cmd1.CommandText = query
        cmd1.Connection = con1






        dr_roi = cmd1.ExecuteReader()

        If dr_roi.HasRows() Then
            dr_roi.Read()

            Session("cintr") = IIf(IsDBNull(dr_roi(0)), 0, dr_roi(0))
            Session("dintr") = IIf(IsDBNull(dr_roi(1)), 0, dr_roi(1))


        End If

        dr_roi.Close()

        cmd.Dispose()

        con1.Close()




    End Sub
    Sub get_agst_info(ByVal acn As String)

        If con1.State = ConnectionState.Closed Then con.Open()

        cmd1.Parameters.Clear()
        cmd1.Connection = con1

        query = "SELECT DATE,PRD FROM MASTER where acno=@acn"


        cmd1.CommandText = query
        cmd1.Parameters.AddWithValue("@acn", acn)

        Dim dr1 As SqlDataReader

        Try
            dr1 = cmd1.ExecuteReader
            If dr1.HasRows Then
                dr1.Read()
                Session("agst_acdate") = dr1(0)
                Session("agst_prd") = dr1(1)
            End If

            dr1.Close()
            cmd1.Dispose()
            con1.Close()


        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try

    End Sub
    Function get_dep_agst(ByVal acn As String)
        Dim pro As String = ""
        Dim PROX As String = ""

        query = "select deposit from dlspec where acn=@acn ORDER BY dlspec.deposit "
        If con1.State = ConnectionState.Closed Then con1.Open()
        cmd1.Connection = con1
        cmd1.CommandText = query
        cmd1.Parameters.Clear()
        cmd1.Parameters.AddWithValue("@acn", acn)
        Try
            pro = cmd1.ExecuteScalar()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        Select Case Mid(pro, 1, 2)
            Case 74
                PROX = "DS"
            Case 75
                PROX = "FD"
            Case 76
                PROX = "KMK"
            Case 77
                PROX = "RD"
            Case 78
                PROX = "RID"
            Case 79
                PROX = "SB"
        End Select

        get_agst_info(pro)

        Return PROX

    End Function



    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        disp4post(txtidate.Text, txtproduct.Text)
    End Sub

    Protected Sub disp_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles disp.PageIndexChanging
        disp.PageIndex = e.NewPageIndex
        'disp.DataBind()
        disp.DataSource = Session("dt")
        disp.DataBind()

    End Sub

    Private Sub btnclr_Click(sender As Object, e As EventArgs) Handles btnclr.Click

    End Sub

    Private Sub btn_post_clr_Click(sender As Object, e As EventArgs) Handles btn_post_clr.Click
        If con.State = ConnectionState.Closed Then con.Open()

        Dim dat As DateTime = DateTime.ParseExact(txtidate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "delete from tmpintc WHERE tmpintc.dep = @dep  AND CONVERT(VARCHAR(20), tmpintc.tdate, 112) = @dt"
                cmd.Parameters.AddWithValue("@dep", txtproduct.Text)
                cmd.Parameters.AddWithValue("@dt", reformatted)
                cmd.CommandText = query
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try




            End Using

        End Using


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()


        query = "delete from tmpint WHERE tmpint.dep = @dep  AND CONVERT(VARCHAR(20), tmpint.tdate, 112) = @dt"
        cmd.Parameters.AddWithValue("@dep", txtproduct.Text)
        cmd.Parameters.AddWithValue("@dt", reformatted)
        cmd.CommandText = query
        Try
            cmd.ExecuteNonQuery()
            disp.DataSource = Nothing
            disp.DataBind()
            lblnet.Text = ""
            lbl_bycas.Text = ""
            txtidate.Text = ""
            txtproduct.Text = ""
            txtfocus(txtidate)
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



    End Sub

    Private Sub btn_si_Click(sender As Object, e As EventArgs) Handles btn_si.Click
        If txt_si_prod.Text = "FD" Then
            get_data_si()
        ElseIf txt_si_prod.Text = "RD" Then
            chk_lst()

        End If

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
                row.BackColor = Drawing.ColorTranslator.FromHtml("#f3f5f6")
                row.ToolTip = String.Empty
                ' txtlimit.Text = disp.SelectedRow.Cells(1).Text 'row.Cells.Item(0).Text
                '     Dim dt As System.Web.UI.WebControls.Image = CType(gv_loan.SelectedRow.Cells(1).FindControl("imgselct"), System.Web.UI.WebControls.Image)
                Dim acn As Label = CType(disp.SelectedRow.Cells(0).FindControl("lblacn"), Label)
                Dim amt As Label = CType(disp.SelectedRow.Cells(0).FindControl("lblicamt"), Label)
                Dim by_cas As Label = CType(disp.SelectedRow.Cells(0).FindControl("LBLbycas"), Label)
                ' get_data(dt.Text)
                '   dt.Visible = True
                ''     get_ac_info(Trim(acn.Text))
                ''    Me.popupdep.Show()

                btn_post.Visible = False
                btn_post_clr.Visible = False
                int_revised.Visible = True

                int_rev_acn.Text = acn.Text
                int_rev_amt.Text = amt.Text

                If txtproduct.Text = "KMK" Or txtproduct.Text = "SB" Or txtproduct.Text = "ML" Or txtproduct.Text = "DL" Or txtproduct.Text = "DCL" Or txtproduct.Text = "FD" Or txtproduct.Text = "RID" Then



                    byc_rev.Visible = True
                    lbl_bycas.Visible = True
                    byc_caption.Visible = True
                    byc_rev.Text = by_cas.Text
                Else
                    byc_rev.Visible = False
                    lbl_bycas.Visible = False
                    byc_caption.Visible = False
                End If

                ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", int_rev_amt.ClientID), True)
                '  txtfocus(int_rev_amt)

            Else
                'row.BackColor = Drawing.ColorTranslator.FromHtml("#f0f4f5")
                row.ToolTip = "Click to select this row."
                'Dim dt As System.Web.UI.WebControls.Image = CType(gv_loan.SelectedRow.Cells(0).FindControl("imgselct"), System.Web.UI.WebControls.Image)
                ' get_data(dt.Text)
                'dt.Visible = False

            End If
        Next
    End Sub

    Private Sub btn_revised_Click(sender As Object, e As EventArgs) Handles btn_revised.Click
        btn_post.Visible = True
        btn_post_clr.Visible = True


        ' If con.State = ConnectionState.Closed Then con.Open()

        Dim dat As DateTime = DateTime.ParseExact(txtidate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()


        'query = "delete from tmpint WHERE tmpint.dep = @dep  AND CONVERT(VARCHAR(20), tmpint.tdate, 112) = @dt"
        query = "UPDATE dbo.tmpint SET camt = @amt, damt = @damt WHERE CONVERT(VARCHAR(20), tmpint.tdate, 112) = @dt AND acn = @acn"

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@dt", reformatted)
        cmd.Parameters.AddWithValue("@amt", CDbl(int_rev_amt.Text))

        If txtproduct.Text = "KMK" Or txtproduct.Text = "SB" Or txtproduct.Text = "ML" Or txtproduct.Text = "DL" Or txtproduct.Text = "DCL" Or txtproduct.Text = "FD" Or txtproduct.Text = "RID" Then

            cmd.Parameters.AddWithValue("@damt", CDbl(byc_rev.Text))
        Else
            If txtproduct.Text = "JL" Or txtproduct.Text = "jl" Then


                cmd.Parameters.AddWithValue("@damt", 0)
            Else
                cmd.Parameters.AddWithValue("@damt", CDbl(int_rev_amt.Text))

            End If

        End If

        cmd.Parameters.AddWithValue("acn", int_rev_acn.Text)

        Try
            cmd.ExecuteNonQuery()
            disp4post(txtidate.Text, txtproduct.Text)
            int_rev_acn.Text = ""
            int_rev_amt.Text = ""
            byc_rev.Text = ""
            int_revised.Visible = False
            btn_post.Visible = True
            btn_post_clr.Visible = True

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



    End Sub

    ''Private Sub bgw_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles 
    ''    Dim sqldatasourceenumerator1 As SqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance
    ''    datatable1 = sqldatasourceenumerator1.GetDataSources()
    ''End Sub

    ''Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
    ''    ProgressBar1.Value = e.ProgressPercentage
    ''End Sub

    ''Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    ''    BackgroundWorker1.RunWorkerAsync()
    ''End Sub

    ''Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
    ''    DataGridView1.DataSource = datatable1
    ''    MsgBox("Done........")
    ''End Sub

    Private Sub InterestPosting_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btn_post_Command(sender As Object, e As CommandEventArgs) Handles btn_post.Command

    End Sub
End Class