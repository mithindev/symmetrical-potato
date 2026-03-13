Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Public Class BankBook

    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Public newrow As DataRow
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Public dt As New DataTable
    Public dtd As New DataTable



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.Open()

        If Not Page.IsPostBack Then


            '   bind_grid()

        End If


    End Sub


    Function get_opening(ByVal reform As String)

        Dim sum_credit As Double
        Dim sum_debit As Double
        Dim opening As Double

        'Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        'Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        'Dim dat1 As DateTime = DateTime.ParseExact(txtto.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        'Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim sdr As SqlDataReader

        If con.State = ConnectionState.Closed Then con.Open()



        query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) <'" + reform + "' and scroll='1'"

        'query = "SELECT SUM(suplement.debit) AS expr1, SUM(suplement.credit) AS expr2 FROM dbo.suplement WHERE CONVERT(VARCHAR(20), date, 112) between @frm and @to and scroll='1' "
        'cmd.Parameters.Clear()
        'cmd.Parameters.AddWithValue("@frm", reformatted)

        'cmd.Parameters.AddWithValue("@to", reformattedto)

        cmd.Connection = con
        cmd.CommandText = query

        Try

            sdr = cmd.ExecuteReader()

            If sdr.HasRows Then

                sdr.Read()

                sum_credit = IIf(IsDBNull(sdr(1)), 0, sdr(1))
                sum_debit = IIf(IsDBNull(sdr(0)), 0, sdr(0))



            End If

            sdr.Close()

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




        opening = opening + (sum_credit - sum_debit)


        Return opening

    End Function

    Private Sub btn_show_Click(sender As Object, e As EventArgs) Handles btn_show.Click

        bind_grid_ndh3()



    End Sub


    Sub bind_grid_ndh3()
        Dim dat As DateTime = DateTime.ParseExact(txtfrm1.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        Dim dat1 As DateTime = DateTime.ParseExact(txtto1.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformattedto As String = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        Dim ob As Double

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("achead", GetType(String))
            dt.Columns.Add("ob", GetType(Decimal))
            dt.Columns.Add("debit", GetType(Decimal))
            dt.Columns.Add("credit", GetType(Decimal))
            dt.Columns.Add("cb", GetType(Decimal))

        End If



        query = " SELECT suplement.achead,SUM(suplement.credit) AS credit,SUM(suplement.debit) AS debit FROM dbo.ledger "
        query &= " LEFT OUTER JOIN dbo.suplement ON ledger.ledger = suplement.achead WHERE CONVERT(VARCHAR(20), suplement.date, 112) BETWEEN @frm AND @to and  ledger.printorder=1 "
        query &= " GROUP BY suplement.achead ORDER BY suplement.achead "



        If con.State = ConnectionState.Closed Then con.Open()

        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@frm", reformatted)
        cmd.Parameters.AddWithValue("@to", reformattedto)

        Try
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                While dr.Read()

                    newrow = dt.NewRow
                    newrow(0) = dr(0)
                    ob = get_led_ob(dr(0), reformatted)
                    newrow(1) = ob
                    newrow(2) = dr(1)
                    newrow(3) = dr(2)
                    newrow(4) = ob - dr(1) + dr(2)
                    dt.Rows.Add(newrow)

                End While
            End If
            Session("dt") = dt
            disp_ndh3.DataSource = dt
            disp_ndh3.DataBind()
            'lblcr.Text = String.Format("{0:N}", dt.Compute("sum(credit)", ""))
            dr.Close()



        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub



    Function get_led_ob(ByVal ach As String, ByVal dt As String)

        Dim x As Double = 0

        query = " SELECT suplement.achead,SUM(suplement.debit) AS debit,  SUM(suplement.credit) AS credit FROM dbo.suplement"
        query &= " WHERE suplement.date < @dt AND suplement.achead = @ach GROUP BY suplement.achead "

        If con1.State = ConnectionState.Closed Then con1.Open()

        cmdi.Connection = con1
        cmdi.CommandType = CommandType.Text
        cmdi.CommandText = query

        cmdi.Parameters.Clear()
        cmdi.Parameters.AddWithValue("@ach", ach)
        cmdi.Parameters.AddWithValue("@dt", dt)



        Dim dr1 As SqlDataReader

        Try
            dr1 = cmdi.ExecuteReader()

            If dr1.HasRows() Then

                While dr1.Read()

                    x = dr1(2) - dr1(1)
                End While

            End If


            cmdi.Dispose()
            dr1.Close()


        Catch ex As Exception
            Response.Write(ex.ToString)


        End Try

        cmdi.Parameters.Clear()

        Dim lob As Double = 0
        query = "SELECT ledger.ob FROM dbo.ledger WHERE ledger.ledger = @ach"
        cmdi.Connection = con1
        cmdi.CommandText = query
        cmdi.Parameters.AddWithValue("@ach", ach)

        Try
            lob = cmdi.ExecuteScalar()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        x = x + lob

        get_led_ob = x
    End Function


    Protected Sub ExportToExcel(sender As Object, e As EventArgs)

        disp_ndh3.AllowPaging = False
        bind_grid_ndh3()

        Response.Clear()
        Response.Buffer = True
        'Response.ContentType = "application/vnd.ms-excel"
        'Response.AddHeader("Content-Disposition", "attachment; filename=Consolidated Report.xlsx")

        Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

        Response.AppendHeader("content-disposition", "attachment; filename=ndh3.xlsx")
        Response.Charset = ""
        Me.EnableViewState = False
        Dim oStringWriter As New System.IO.StringWriter()
        Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)
        disp_ndh3.Visible = True
        disp_ndh3.RenderControl(oHtmlTextWriter)
        Response.Write(oStringWriter.ToString)
        'Response.Flush()
        Response.End()


    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        'Tell the compiler that the control is rendered
        'explicitly by overriding; the VerifyRenderingInServerForm event.
        '   Return



    End Sub
    Private Sub btn_exp_Click(sender As Object, e As ImageClickEventArgs) ' Handles btn_exp.Click
        ExportToExcel(sender, e)
    End Sub

    Private Sub BankBook_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

    End Sub
End Class