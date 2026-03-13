Imports System.Globalization
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO


Public Class JournalBook
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection

    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Public dt As New DataTable
    Dim newrow As DataRow


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        con.Open()

        If Not Page.IsPostBack Then

            ' bind_grid()


            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtfrm.ClientID), True)
        End If

    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        show_led()
        disp.PageIndex = e.NewPageIndex
        disp.DataBind()

    End Sub

    Sub show_led()

        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted1 As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select date,achead,narration,acn ,debit,credit from suplement where CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' and type='JOURNAL' ORDER BY tid  "

        Dim ds As New DataSet
        Try
            'Dim adapter As New SqlDataAdapter(query, con)
            cmd.Connection = con
            cmd.CommandText = query
            'cmd.Parameters.AddWithValue("@achead", glh.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@frm", reformatted)
            cmd.Parameters.AddWithValue("@to", reformatted1)

            Dim dr As SqlDataReader

            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                While dr.Read

                    If dt.Columns.Count = 0 Then
                        dt.Columns.Add("date", GetType(Date))
                        dt.Columns.Add("achead", GetType(String))
                        dt.Columns.Add("nar", GetType(String))
                        dt.Columns.Add("dr", GetType(String))
                        dt.Columns.Add("cr", GetType(String))
                        'dt.Columns.Add("acn", GetType(String))
                    End If


                    Dim nam As String = get_name(dr(3))

                    newrow = dt.NewRow

                    newrow(0) = dr(0)
                    newrow(1) = dr(1)
                    newrow(2) = dr(2) + " " + dr(3) + " " + nam
                    newrow(3) = String.Format("{0:N}", IIf(dr(4) = 0, "", dr(4)))
                    newrow(4) = String.Format("{0:N}", IIf(dr(5) = 0, "", dr(5)))
                    dt.Rows.Add(newrow)
                End While
            End If


            disp.DataSource = dt
            disp.DataBind()



        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()


        End Try
        '   lblnet.Text = String.Format("{0:N}", closing)
        'trim_disp()
        '

    End Sub

    Function get_name(ByVal acnox As String)

        Dim nam As String = ""
        query = "SELECT   member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.member  ON master.cid = member.MemberNo WHERE master.acno = '" + acnox + "'"

        If con1.State = ConnectionState.Closed Then con1.Open()
        cmdi.Connection = con1
        cmdi.CommandText = query
        Try

            nam = cmdi.ExecuteScalar()

            If IsDBNull(nam) Then
                nam = " "
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            con1.Close()
            cmdi.Dispose()
        End Try
        Return nam
    End Function
    Sub get_opening()

        Dim sum_credit As Double
        Dim sum_debit As Double
        Dim opening As Double
        ' Dim closing_bal As Double

        Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim sdr As SqlDataReader

        If con.State = ConnectionState.Closed Then con.Open()



        query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) <'" + reformatted + "' and scroll='1'"

        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' "
        'cmd.Parameters.Clear()
        'cmd.Parameters.AddWithValue("@frm", reformatted)

        'cmd.Parameters.AddWithValue("@to", reformattedto)


        cmd.CommandText = query

        Try

            sdr = cmd.ExecuteReader()

            If sdr.HasRows Then

                sdr.Read()

                sum_credit = IIf(IsDBNull(sdr(1)), 0, sdr(1))
                sum_debit = IIf(IsDBNull(sdr(0)), 0, sdr(0))



            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally






            cmd.Dispose()
            con.Close()



        End Try

        Dim oResult As Double = 0

        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 105) <'" + Convert.ToDateTime(txtdate.Text)

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select sum(ledger.ob) as expr1 from ledger"

        cmd.CommandText = query
        cmd.Connection = con

        Try

            oResult = cmd.ExecuteScalar()


            If Not IsNothing(oResult) Then
                opening = oResult
            Else
                opening = 0
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()


        End Try

        If con.State = ConnectionState.Closed Then con.Open()


        query = "select sum(credit) as expr1,sum(debit) as expr2 from dbo.suplement where  CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' and type='JOURNAL' "
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@frm", reformatted)

        cmd.Parameters.AddWithValue("@to", reformattedto)


        cmd.CommandText = query
        cmd.Connection = con

        Dim dr As SqlDataReader

        Dim trans_credit As Double = 0

        Dim trans_debit As Double = 0

        Try
            dr = cmd.ExecuteReader()

            If dr.HasRows() Then
                dr.Read()

                trans_credit = IIf(IsDBNull(dr(0)), 0, dr(0))

                trans_debit = IIf(IsDBNull(dr(1)), 0, dr(1))



            End If

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            cmd.Dispose()
            con.Close()

        End Try





        lbl_sum_credit.Text = String.Format("{0:N}", trans_credit)
        lbl_sum_debit.Text = String.Format("{0:N}", trans_debit)





    End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click
        show_led()

        get_opening()
    End Sub

    Private Sub JournalBook_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class