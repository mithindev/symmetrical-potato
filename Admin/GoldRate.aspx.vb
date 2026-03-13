Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Globalization
Imports System.Net.Mail
Public Class GoldRate


    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim countresult As Integer
    Dim ds As New DataSet


    Protected Sub disp_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' e.Row.Attributes("onmouseover") = "this.style.backgroundColor='aquamarine';"
            ' e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(disp, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
            e.Row.Attributes("style") = "cursor:pointer"
            ' e.Row.ToolTip = "Click last column for selecting this row."
        End If
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Dim pName As String = disp.SelectedRow.Cells(1).Text
        'MsgBox(Convert.ToString("<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   ") & pName)
        For Each row As GridViewRow In disp.Rows
            If row.RowIndex = disp.SelectedIndex Then
                row.BackColor = Drawing.ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
                ' txtlimit.Text = disp.SelectedRow.Cells(1).Text 'row.Cells.Item(0).Text
                Dim dt As Label = DirectCast(disp.SelectedRow.Cells(0).FindControl("lbldat"), Label)
                get_data(dt.Text)

            Else
                row.BackColor = Drawing.ColorTranslator.FromHtml("#f0f4f5")
                row.ToolTip = "Click to select this row."
            End If
        Next
    End Sub
    Sub get_data(ByVal dt As String)
        Dim dat As DateTime = DateTime.ParseExact(dt, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

        If con.State = ConnectionState.Closed Then con.Open()
        Dim qry = "select date,rate,limit from goldrate  where CONVERT(VARCHAR(20), date, 112)='" + reformatted + "'"
        cmd.Connection = con
        cmd.CommandText = qry

        Dim dr As SqlDataReader
        Try
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                dr.Read()
                txtfrm.Text = dr(0)
                txtrate.Text = FormatNumber(dr(1))
                txtlimit.Text = FormatNumber(dr(2))

                txtfocus(txtrate)

            End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)

        bind_grid()
        disp.PageIndex = e.NewPageIndex
        disp.DataBind()
    End Sub

    Sub bind_grid()

        If con.State = ConnectionState.Closed Then con.Open()

        Dim qry = "select date,rate,limit,loanlimit,loanlimitwrd from goldrate order by date desc"

        Dim ds As New DataSet

        Dim adapter As New SqlDataAdapter(qry, con)
        adapter.Fill(ds)

        disp.DataSource = ds
        disp.DataBind()

    End Sub
    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then

            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtfrm.ClientID), True)
            bind_grid()

            txtotp.Visible = False
            btnotp.Visible = False

        End If

    End Sub

    Private Sub btn_clr_Click(sender As Object, e As EventArgs) Handles btn_clr.Click
        txtfrm.Text = ""
        txtrate.Text = ""
        txtlimit.Text = ""
        txtfocus(txtfrm)
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click

        populate_body()
        sendotp()
        pnlotp.Visible = True
        txtotp.Visible = True
        btnotp.Visible = True
        txtfocus(txtotp)



    End Sub

    Private Function populate_body()

        Dim oResult As String = ""
        Dim qry = "select branch from dbo.branch "
        cmd.CommandType = CommandType.Text
        cmd.CommandText = qry
        If con.State = ConnectionState.Closed Then con.Open()
        cmd.Connection = con
        Try


            oResult = cmd.ExecuteScalar()

        Catch e As Exception

            MsgBox(e.ToString)
        End Try

        Session("branch") = oResult + " Branch"


        Dim charArr As Char() = "0123456789".ToCharArray()
        Dim strrandom As String = String.Empty
        Dim objran As New Random()
        Dim noofcharacters As Integer = 6 'Convert.ToInt32(txtCharacters.Text)
        For i As Integer = 0 To noofcharacters - 1
            'It will not allow Repetation of Characters
            Dim pos As Integer = objran.[Next](1, charArr.Length)
            If Not strrandom.Contains(charArr.GetValue(pos).ToString()) Then
                strrandom += charArr.GetValue(pos)
            Else
                i -= 1
            End If
        Next
        Session("otp") = strrandom

        Dim body As String = String.Empty
        Dim reader As StreamReader = New StreamReader(Server.MapPath("goldrate.html"))
        body = reader.ReadToEnd
        body = body.Replace("{branch}", Session("branch"))
        body = body.Replace("{otp}", strrandom)
        body = body.Replace("{dt}", txtfrm.Text)
        body = body.Replace("{rpg}", FormatCurrency(txtrate.Text))
        body = body.Replace("{prpg}", FormatCurrency(txtplusrate.Text))
        body = body.Replace("{urpg}", FormatCurrency(txtultra.Text))
        body = body.Replace("{lonlimit}", txtlimit.Text)
        body = body.Replace("{pplimit}", FormatCurrency(txtlmtpp.Text))
        body = body.Replace("{pplwrd}", FormatCurrency(txtlmtppwrd.Text))
        Return body

    End Function
    Protected Sub sendotp()

        Dim subject As String = "OTP for Correction"
        Dim recepientEmail As String = ConfigurationManager.AppSettings("recepientEmailid")
        Dim body As String = populate_body()
        Dim mailMessage As MailMessage = New MailMessage
        mailMessage.From = New MailAddress(ConfigurationManager.AppSettings("UserName"))
        mailMessage.Subject = subject
        mailMessage.Body = body
        mailMessage.IsBodyHtml = True
        mailMessage.To.Add(New MailAddress(recepientEmail))
        Dim smtp As SmtpClient = New SmtpClient
        smtp.Host = ConfigurationManager.AppSettings("Host")
        smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings("EnableSsl"))
        Dim NetworkCred As System.Net.NetworkCredential = New System.Net.NetworkCredential
        NetworkCred.UserName = ConfigurationManager.AppSettings("UserName")
        NetworkCred.Password = ConfigurationManager.AppSettings("Password")
        smtp.UseDefaultCredentials = True
        smtp.Credentials = NetworkCred
        smtp.Port = Integer.Parse(ConfigurationManager.AppSettings("Port"))
        Try
            smtp.Send(mailMessage)
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


    End Sub



    Private Sub btnotp_Click(sender As Object, e As EventArgs) Handles btnotp.Click
        If txtotp.Text = Session("otp") Then
            Dim hasdate As DateTime
            If con.State = ConnectionState.Closed Then con.Open()
            Dim dat As DateTime = DateTime.ParseExact(txtfrm.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
            Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            Dim qry = "select date from goldrate  where CONVERT(VARCHAR(20), date, 112)='" + reformatted + "'"
            cmd.Connection = con
            cmd.CommandText = qry

            Try
                hasdate = cmd.ExecuteScalar()

            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try

            If hasdate = Date.MinValue Then
                qry = "insert into goldrate(date,rate,plusrate,limit,loanlimit,loanlimitwrd,rebate,srcitizen,bnktrf,renew,ultra,ultralimit,deposit,isbnktrf,isrenew)"
                qry &= "values(@date,@rate,@plusrate,@limit,@loanlimit,@loanlimitwrd,@rebate,@srcitizen,@bnktrf,@renew,@ultra,@ultralimit,@deposit,@isbnktrf,@isrenew)"
            Else

                qry = "UPDATE goldrate SET date=@date,rate=@rate,plusrate=@pluseratelimit=@limit,loanlimit=@loanlimit,loanlimitwrd=@loanlimitwrd,rebate=@rebate,srcitizen=@srcitizen,bnktrf=@bnktrf,renew=@renew,ultra=@ultra,ultralimit=@ultralimit,deposit=@deposit,isbnktrf=@isbnktrf,isrenew=@isrenew where CONVERT(VARCHAR(20), date, 112)='" + reformatted + "'"


            End If
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@date", Convert.ToDateTime(txtfrm.Text))
            cmd.Parameters.AddWithValue("@rate", CDbl(txtrate.Text))
            cmd.Parameters.AddWithValue("@plusrate", CDbl(txtplusrate.Text))
            cmd.Parameters.AddWithValue("@limit", CDbl(txtlimit.Text))
            cmd.Parameters.AddWithValue("@loanlimit", CDbl(txtlmtpp.Text))
            cmd.Parameters.AddWithValue("@loanlimitwrd", CDbl(txtlmtppwrd.Text))
            cmd.Parameters.AddWithValue("@rebate", CDbl(txtrebate.Text))
            cmd.Parameters.AddWithValue("@srcitizen", CDbl(txtsrc.Text))
            cmd.Parameters.AddWithValue("@bnktrf", CDbl(txttrf.Text))
            cmd.Parameters.AddWithValue("@renew", CDbl(txtrenew.Text))
            cmd.Parameters.AddWithValue("@ultra", CDbl(txtultra.Text))
            cmd.Parameters.AddWithValue("@ultralimit", CDbl(txtultralimit.Text))
            cmd.Parameters.AddWithValue("@deposit", CDbl(txtdep.Text))
            cmd.Parameters.AddWithValue("@isbnktrf", IIf(istrf.Checked = True, 1, 0))
            cmd.Parameters.AddWithValue("@isrenew", IIf(isren.Checked = True, 1, 0))

            cmd.CommandText = qry
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try


            txtfrm.Text = ""
            txtlimit.Text = ""
            txtplusrate.Text = ""
            txtrate.Text = ""
            txtlmtpp.Text = ""
            txtlmtppwrd.Text = ""
            txtrebate.Text = ""
            txtsrc.Text = ""
            txttrf.Text = ""
            txtotp.Text = ""
            txtrenew.Text = ""
            txtultra.Text = ""
            txtultralimit.Text = ""
            txtdep.Text = ""
            pnlotp.Visible = False
            Dim stitle As String = "Hi  " + Session("sesusr")
            Dim msg As String = "Gold Rate Updated Sucessfully !"
            Dim fnc As String = "showToastOK('" + stitle + "','" + msg + "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoast", fnc, True)

            bind_grid()
            txtfocus(txtfrm)

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)
        Else
            txtotp.Text = "Invalid OTP"
            txtotp.CssClass = "form-control danger"


        End If
    End Sub

    Private Sub btn_update_Command(sender As Object, e As CommandEventArgs) Handles btn_update.Command

    End Sub
End Class
