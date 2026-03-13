Imports System.Data.SqlClient

Public Class Postal
    Inherits System.Web.UI.Page

    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public dt As New DataTable

    Dim newrow As DataRow



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Me.IsPostBack Then
            bind_ac()

            acchrg.Enabled = False


        End If

    End Sub


    Sub bind_ac()
        Dim ds_js As DataSet = Nothing

        acchrg.Items.Clear()

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()

            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "SELECT nature,chrgs FROM dbo.accharges ORDER BY nature"
                cmd.CommandText = query

                Using reader As SqlDataReader = cmd.ExecuteReader()

                    acchrg.DataSource = reader

                    acchrg.DataTextField = "nature"
                    acchrg.DataValueField = "chrgs"

                    acchrg.DataBind()

                    acchrg.Items.Insert(0, "<-- Select -->")
                    acchrg.Items.Item(0).Value = ""

                End Using



            End Using

            con.Close()

        End Using



    End Sub

    Protected Sub onchk(sender As Object, e As EventArgs)




    End Sub




    Private Sub Postal_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Sub showpostal(ByVal prod As String, ByVal frm As Integer, ByVal pto As Integer)

        Dim dt As DataTable = New DataTable


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "postal"
                cmd.Parameters.Clear()
                cmd.CommandTimeout = 600

                cmd.Parameters.Add(New SqlParameter("@prod", prod))
                cmd.Parameters.Add(New SqlParameter("@frm", frm))
                cmd.Parameters.Add(New SqlParameter("@to", pto))
                cmd.Parameters.Add(New SqlParameter("@nature", Trim(acchrg.SelectedItem.Text)))

                Try

                    Using Sda As New SqlDataAdapter(cmd)


                        Sda.Fill(dt)
                        rppostal.DataSource = dt
                        rppostal.DataBind()

                        If Not dt.Rows.Count = 0 Then

                            lbltotal.Text = FormatCurrency(dt.Compute("sum(chrgs)", String.Empty))
                            pnlbtn.Visible = True
                            Session("jspec") = dt

                        Else
                            lbltotal.Text = FormatCurrency(0)

                        End If






                    End Using

                Catch ex As Exception

                    Response.Write(ex.ToString)


                End Try


            End Using

        End Using




    End Sub


    Protected Sub ispostal_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim total As Double = 0
        For Each item As RepeaterItem In rppostal.Items
            Dim chk As CheckBox = CType(item.FindControl("ispostal"), CheckBox)
            Dim txtamt As Label = CType(item.FindControl("lblchrgs"), Label)



            If chk.Checked Then

                total += CInt(txtamt.Text)

            End If

            If total > 0 Then btnup.Enabled = True


            lbltotal.Text = FormatCurrency(total)



        Next

    End Sub




    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        If Not acchrg.SelectedItem.Value = "" Then
            Select Case acchrg.SelectedItem.Text
                Case "IN LAND"
                    showpostal(ddloan.SelectedItem.Text, 10, 12)
                Case "REGISTERED POST"
                    showpostal(ddloan.SelectedItem.Text, 2, 10)
                Case Else


            End Select

        End If
    End Sub

    Private Sub rppostal_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rppostal.ItemDataBound
        'Dim total As Double = 0
        'For Each item As RepeaterItem In rppostal.Items
        '    Dim txtpar As TextBox = CType(item.FindControl("txtpar"), TextBox)
        '    Dim txtamt As TextBox = CType(item.FindControl("txtamt"), TextBox)
        '    Dim txttotal As TextBox = CType(item.FindControl("txttotal"), TextBox)
        '    txtpar.Text = acchrg.SelectedItem.Text
        '    txtamt.Text = FormatNumber(CInt(acchrg.SelectedItem.Value))

        '    total += CInt(acchrg.SelectedItem.Value)

        '    txttotal.Text = FormatCurrency(total)



        'Next

    End Sub

    Private Sub btnup_Click(sender As Object, e As EventArgs) Handles btnup.Click

        If Session("sesusr") = Nothing Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            Log_out(Session("logtime").ToString, Session("sesusr"))
            Response.Redirect("..\login.aspx")


        End If


        If CDbl(lbltotal.Text) = 0 Then Exit Sub



        'Dim ovr_d As Double
        'Dim ovr_c As Double
        Dim led As String = String.Empty
        Dim cled As String = String.Empty

        Select Case ddloan.SelectedItem.Text
            Case "DL"
                led = "DL POSTAL"
                cled = "DEPOSIT LOAN"
            Case "DCL"
                led = "DCL POSTAL"
                cled = "DAILY COLLECTION LOAN"
            Case "JL"
                led = "JL POSTAL"
                cled = "JEWEL LOAN"
            Case "ML"
                led = "ML POSTAL"
                cled = "MORTGAGE LOAN"
        End Select


        Dim countresult As Integer
        Dim query As String = String.Empty


        If con.State = ConnectionState.Closed Then con.Open()


        cmd.Connection = con




        query = "INSERT INTO trans(date,acno,drd,crd,drc,crc,narration,due,type,suplimentry,sesusr,entryat,cbal,dbal)"
        query &= "VALUES(@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@suplimentry,@sesusr,@entryat,@cbal,@dbal)"

        '  Dim prod = get_pro(product)

        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@acno", "")
        cmd.Parameters.AddWithValue("@drd", 0)
        cmd.Parameters.AddWithValue("@crd", CDbl(lbltotal.Text))
        cmd.Parameters.AddWithValue("@drc", 0)
        cmd.Parameters.AddWithValue("@crc", CDbl(lbltotal.Text))
        cmd.Parameters.AddWithValue("@narration", "To Postage")
        cmd.Parameters.AddWithValue("@due", "")
        cmd.Parameters.AddWithValue("@type", "JOURNAL")
        cmd.Parameters.AddWithValue("@suplimentry", led)
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
        cmd.Connection = con

        query = "SELECT MAX(ID) from dbo.trans where trans.suplimentry='" + led + "'"
        'cmd.Parameters.AddWithValue("@led", x)
        cmd.CommandText = query


        Try

            countresult = cmd.ExecuteScalar()

            Session("tid") = Convert.ToString(countresult)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try


        updateTrans()
        update_suplementry(Session("tid"), led, lbltotal.Text, 0, "By Transfer", "JOURNAL")
        update_suplementry(Session("tid"), cled, 0, lbltotal.Text, "To Transfer", "JOURNAL")

        bind_ac()
        rppostal.DataSource = Nothing
        rppostal.DataBind()
        pnlbtn.Visible = False




        sesuser.Text = Session("sesusr")
        lblinfo.Text = "Updated ! Transaction Id : " + Session("tid")
        alertmsg.Visible = True


    End Sub

    Sub updateTrans()

        Dim led As String = String.Empty
        Dim cled As String = String.Empty

        Select Case ddloan.SelectedItem.Text
            Case "DL"
                led = "DL POSTAL"
                cled = "DEPOSIT LOAN"
            Case "DCL"
                led = "DCL POSTAL"
                cled = "DAILY COLLECTION LOAN"
            Case "JL"
                led = "JL POSTAL"
                cled = "JEWEL LOAN"
            Case "ML"
                led = "ML POSTAL"
                cled = "MORTGAGE LOAN"
        End Select


        For Each item As RepeaterItem In rppostal.Items
            Dim chk As CheckBox = CType(item.FindControl("ispostal"), CheckBox)
            Dim txtamt As Label = CType(item.FindControl("lblchrgs"), Label)
            Dim acno As Label = CType(item.FindControl("lblacno"), Label)
            Dim nature As Label = CType(item.FindControl("lblnature"), Label)

            If chk.Checked Then

                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Parameters.Clear()
                cmd.Connection = con
                query = "insert into actrans(id,date,acno,drd,crd,drc,crc,narration,due,type,sesusr,entryat,suplimentry)"
                query &= "values(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sesusr,@entryat,@suplimentry)"

                cmd.CommandText = query


                cmd.Parameters.AddWithValue("@id", Session("tid"))
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
                cmd.Parameters.AddWithValue("@acno", acno.Text)
                cmd.Parameters.AddWithValue("@drd", CInt(txtamt.Text))
                cmd.Parameters.AddWithValue("@crd", 0)
                cmd.Parameters.AddWithValue("@drc", CInt(txtamt.Text))
                cmd.Parameters.AddWithValue("@crc", 0)
                cmd.Parameters.AddWithValue("@narration", "To Postage")
                cmd.Parameters.AddWithValue("@due", nature.Text)
                cmd.Parameters.AddWithValue("suplimentry", cled)
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))

                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    MsgBox(ex.ToString)
                Finally
                    cmd.Parameters.Clear()
                    con.Close()
                    cmd.Dispose()

                End Try


                If con.State = ConnectionState.Closed Then con.Open()
                cmd.Parameters.Clear()
                cmd.Connection = con
                query = "insert into actransc(id,date,acno,drd,crd,drc,crc,narration,due,type,sesusr,entryat,suplimentry)"
                query &= "values(@id,@date,@acno,@drd,@crd,@drc,@crc,@narration,@due,@type,@sesusr,@entryat,@suplimentry)"

                cmd.CommandText = query


                cmd.Parameters.AddWithValue("@id", Session("tid"))
                cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
                cmd.Parameters.AddWithValue("@acno", acno.Text)
                cmd.Parameters.AddWithValue("@drd", CInt(txtamt.Text))
                cmd.Parameters.AddWithValue("@crd", 0)
                cmd.Parameters.AddWithValue("@drc", CInt(txtamt.Text))
                cmd.Parameters.AddWithValue("@crc", 0)
                cmd.Parameters.AddWithValue("@narration", "To Postage")
                cmd.Parameters.AddWithValue("@due", nature.Text)
                cmd.Parameters.AddWithValue("suplimentry", cled)
                cmd.Parameters.AddWithValue("@type", "TRF")
                cmd.Parameters.AddWithValue("@sesusr", Session("sesusr"))
                cmd.Parameters.AddWithValue("@entryat", Convert.ToDateTime(Now))

                Try
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    MsgBox(ex.ToString)
                Finally
                    cmd.Parameters.Clear()
                    con.Close()
                    cmd.Dispose()

                End Try

            End If





        Next

    End Sub

    Private Sub update_suplementry(ByVal tid As Double, ByVal ach As String, ByVal cr As Double, ByVal dr As Double, ByVal nar As String, ByVal typ As String)
        If tid = 0 Then Exit Sub

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        'cmd.CreateParameter()
        query = "INSERT INTO suplement(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@date,@tid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@tid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        If dr = 0 Then
            cmd.Parameters.AddWithValue("@nar", nar)
        Else
            cmd.Parameters.AddWithValue("@nar", nar)
        End If

        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()


        End Try

        query = ""

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        cmd.Parameters.Clear()
        'cmd.CreateParameter()
        query = "INSERT INTO suplementc(date,transid,achead,debit,credit,acn,narration,type)"
        query &= "VALUES(@date,@tid,@achead,@debit,@credit,@acn,@nar,@typ)"
        cmd.CommandText = query
        cmd.Parameters.Clear()

        cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtdate.Text))
        cmd.Parameters.AddWithValue("@tid", tid)
        cmd.Parameters.AddWithValue("@achead", ach)
        cmd.Parameters.AddWithValue("@debit", dr)
        cmd.Parameters.AddWithValue("@credit", cr)
        cmd.Parameters.AddWithValue("@acn", "")
        If dr = 0 Then
            cmd.Parameters.AddWithValue("@nar", nar)
        Else
            cmd.Parameters.AddWithValue("@nar", nar)
        End If

        cmd.Parameters.AddWithValue("@typ", typ)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception


            MsgBox(ex.Message)

        Finally
            cmd.Parameters.Clear()

            cmd.Dispose()
            con.Close()


        End Try

        query = ""



    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        pnladdpostal.Visible = True
        txtacn.Focus()



    End Sub

    Private Sub acchrg_TextChanged(sender As Object, e As EventArgs) Handles acchrg.TextChanged

    End Sub

    Private Sub ddloan_TextChanged(sender As Object, e As EventArgs) Handles ddloan.TextChanged
        If Not ddloan.SelectedItem.Value = "" Then
            acchrg.Enabled = True
            acchrg.Focus()


        End If
    End Sub

    Private Sub btn_add_Click(sender As Object, e As EventArgs) Handles btn_add.Click
        Dim account As String = Mid(txtacn.Text, 1, 2)

        Select Case Trim(ddloan.SelectedItem.Text)
            Case "DCL"
                If account = "61" Then
                    bind_newpostal()
                Else
                    txtacn.Text = ""
                    txtfirstname.Text = ""
                    txtacn.Focus()

                End If


            Case "DL"
                If account = "62" Then
                    bind_newpostal()

                Else
                txtacn.Text = ""
                    txtfirstname.Text = ""
                    txtacn.Focus()
                End If

            Case "JL"
                If account = "63" Then
                    bind_newpostal()

                Else
                txtacn.Text = ""
                    txtfirstname.Text = ""
                    txtacn.Focus()
                End If

            Case "ML"
                If account = "64" Then
                    bind_newpostal()

                Else
                txtacn.Text = ""
                    txtfirstname.Text = ""
                    txtacn.Focus()
                End If

        End Select

    End Sub

    Sub bind_newpostal()

        Dim dt_j As New DataTable
        If dt_j.Columns.Count = 0 Then
            dt_j.Columns.Add("date", GetType(Date))
            dt_j.Columns.Add("acno", GetType(String))
            dt_j.Columns.Add("FirstName", GetType(String))
            dt_j.Columns.Add("LastName", GetType(String))
            dt_j.Columns.Add("dago", GetType(Integer))
            dt_j.Columns.Add("nature", GetType(String))
            dt_j.Columns.Add("chrgs", GetType(Decimal))

        End If


        If dt.Columns.Count = 0 Then
            dt.Columns.Add("date", GetType(Date))
            dt.Columns.Add("acno", GetType(String))
            dt.Columns.Add("FirstName", GetType(String))
            dt.Columns.Add("LastName", GetType(String))
            dt.Columns.Add("dago", GetType(Integer))
            dt.Columns.Add("nature", GetType(String))
            dt.Columns.Add("chrgs", GetType(Decimal))

        End If



        If Session("jspec") Is Nothing = False Then

            dt_j = CType(Session("jspec"), DataTable)

            Dim count As Integer = dt_j.Rows.Count

            For Each row1 As DataRow In dt_j.Rows

                dt.ImportRow(row1)
            Next

        End If
        newrow = dt_j.NewRow
        newrow(0) = Date.Today
        newrow(1) = txtacn.Text
        newrow(2) = txtfirstname.Text
        newrow(3) = txtlname.Text
        newrow(4) = 0

        newrow(6) = FormatNumber(CDbl(acchrg.SelectedItem.Value))

        newrow(5) = acchrg.SelectedItem.Text

        dt_j.Rows.Add(newrow)

        dt.ImportRow(newrow)

        Session("jspec") = dt_j
        rppostal.DataSource = dt

        rppostal.DataBind()

        If Not dt_j.Rows.Count = 0 Then

          '  lbltotal.Text = 0.00 ' FormatCurrency(dt_j.Compute("sum(chrgs)", String.Empty))
            pnlbtn.Visible = True
            Session("jspec") = dt
            btnup.Enabled = False


        Else
            lbltotal.Text = FormatCurrency(0)

        End If


        txtacn.Text = ""
        txtfirstname.Text = ""
        txtlname.Text = ""
        pnladdpostal.Visible = False











    End Sub

    Private Sub btnclr_Click(sender As Object, e As EventArgs) Handles btnclr.Click
        Session("jspec") = Nothing

        rppostal.DataSource = Nothing
        rppostal.DataBind()
        pnlbtn.Visible = False




    End Sub

    Private Sub txtacn_TextChanged(sender As Object, e As EventArgs) Handles txtacn.TextChanged

        Dim custname As String = ""
        If con.State = ConnectionState.Closed Then con.Open()
        Dim cmdi As New SqlCommand



        query = "SELECT member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo WHERE master.acno = @acno"
        cmdi.Connection = con
        cmdi.CommandText = query
        cmdi.Parameters.Clear()
        cmdi.Parameters.AddWithValue("@acno", Trim(txtacn.Text))

        Try

            custname = cmdi.ExecuteScalar()
            If IsDBNull(custname) Then
                txtacn.Text = ""
                txtacn.Focus()

            Else
                txtfirstname.Text = custname

            End If


        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try




    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click

        Dim total As Double
        For Each item As RepeaterItem In rppostal.Items
            Dim chk As CheckBox = CType(item.FindControl("ispostal"), CheckBox)

            chk.Checked = True
            Dim txtamt As Label = CType(item.FindControl("lblchrgs"), Label)



            If chk.Checked Then

                total += CInt(txtamt.Text)

            End If





        Next

        lbltotal.Text = FormatCurrency(total)


    End Sub

    Private Sub btnUnselectAll_Click(sender As Object, e As EventArgs) Handles btnUnselectAll.Click
        For Each item As RepeaterItem In rppostal.Items
            Dim chk As CheckBox = CType(item.FindControl("ispostal"), CheckBox)

            chk.Checked = False
            '    Dim txtamt As Label = CType(item.FindControl("lblchrgs"), Label)








        Next
        lbltotal.Text = FormatCurrency(0)

    End Sub
End Class