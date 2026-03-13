Imports System
Imports System.Data
    Imports System.Data.SqlClient
    Imports System.Configuration
    Imports System.IO
    Imports System.Web.Services
Imports System.Globalization
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class ProTrialBalance
        Inherits System.Web.UI.Page

        Dim newrow As DataRow
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim cmdi As New SqlClient.SqlCommand
        Dim con1 As New SqlClient.SqlConnection
        Dim cmd1 As New SqlClient.SqlCommand
        Dim ds As New DataSet
        Dim ds1 As New DataSet

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub
        Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
            disp.PageIndex = e.NewPageIndex
            disp.DataBind()
            disp_grid()

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.Open()

            If Not IsPostBack Then
                '   Dim script As String = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });"
                '   ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)
                'ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", (prod.ClientID), True))
                ' txtfocus(prod)
                txtdate.Text = Format(Now, "dd-MM-yyyy")
                bind_grid()



        End If
        End Sub


        Sub bind_grid()


            Dim dr1 As SqlDataReader

            Dim query As String = "SELECT shortname,PRDTYPE from products"

            cmd.Connection = con
            cmd.CommandText = query

            Try

                dr1 = cmd.ExecuteReader()

                If dr1.HasRows Then
                    Do While dr1.Read()

                        prod.Items.Add(dr1(0))

                    Loop


                End If

                prod.Items.Insert(0, "<-- Select -->")
                prod.Items.Item(0).Value = ""
                dr1.Close()


            Catch ex As Exception
                MsgBox(ex.ToString)
            Finally
                cmd.Dispose()

                con.Close()


            End Try




        End Sub


        Function get_disp_data() As DataTable

            Dim rt As Double = 0
            Dim rc As Integer = 0

            Dim xty As String = ""

            Dim fn As String
            Dim amtd As Double = 0


            dispadd.Visible = False
            disp.Visible = True



            Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
            Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)



            Dim dt_j As New DataTable

            If dt_j.Columns.Count = 0 Then
                dt_j.Columns.Add("acno", GetType(String))
                dt_j.Columns.Add("party", GetType(String))
                dt_j.Columns.Add("amt", GetType(Decimal))
                'dt_j.Columns.Add("net", GetType(Decimal))

            End If

            If con.State = ConnectionState.Closed Then con.Open()

        'query = "select acno,cid from master where CONVERT(VARCHAR(20), date, 112) <='" + reformatted + "'"

        ' query = " SELECT SUM(actrans.Drc) AS expr1, SUM(actrans.Crc) AS expr2,SUM(actrans.Drd) AS expr3, SUM(actrans.Crd) AS expr4, Master.date,Master.acno,Master.serial,member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.actrans ON master.acno = actrans.acno LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo "
        'query &= "WHERE CONVERT(VARCHAR(20), actrans.date, 112) <= '" + reformatted + "'AND master.product = '" + Trim(prod.SelectedItem.Text) + "' GROUP BY master.date,Master.acno, member.FirstName  ORDER BY master.serial"

        If Session("session_user_role") = "Audit" Then

            query = " SELECT SUM(actransc.Drc) AS expr1, SUM(actransc.Crc) AS expr2,SUM(actransc.Drd) AS expr3, SUM(actransc.Crd) AS expr4, Masterc.date,Masterc.acno,member.FirstName FROM dbo.masterc LEFT OUTER JOIN dbo.actransc ON masterc.acno = actransc.acno LEFT OUTER JOIN dbo.member ON masterc.cid = member.MemberNo "
            query &= " WHERE CONVERT(VARCHAR(20), actransc.date, 112) <= '" + reformatted + "'AND masterc.product = '" + Trim(prod.SelectedItem.Text) + "' GROUP BY masterc.date,Masterc.acno, member.FirstName  ORDER BY masterc.date,masterc.acno"


        Else
            query = " SELECT SUM(actrans.Drc) AS expr1, SUM(actrans.Crc) AS expr2,SUM(actrans.Drd) AS expr3, SUM(actrans.Crd) AS expr4, Master.date,Master.acno,member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.actrans ON master.acno = actrans.acno LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo "
            query &= "WHERE CONVERT(VARCHAR(20), actrans.date, 112) <= '" + reformatted + "'AND master.product = '" + Trim(prod.SelectedItem.Text) + "' GROUP BY master.date,Master.acno, member.FirstName  ORDER BY master.date,master.acno"

        End If

        '    query = " SELECT SUM(actrans.Drc) AS expr1, SUM(actrans.Crc) AS expr2,SUM(actrans.Drd) AS expr3, SUM(actrans.Crd) AS expr4, Master.date,Master.acno,member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.actrans ON master.acno = actrans.acno LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo "
        '   query &= "WHERE CONVERT(VARCHAR(20), actrans.date, 112) <= '" + reformatted + "'AND master.product = '" + Trim(prod.SelectedItem.Text) + "' GROUP BY master.date,Master.acno, member.FirstName  ORDER BY master.date,master.acno"



        cmd.Connection = con
            cmd.CommandText = query

            Dim drtb As SqlDataReader

            Try
                drtb = cmd.ExecuteReader()

                If drtb.HasRows() Then

                    While drtb.Read()

                        Session("acn") = IIf(IsDBNull(drtb(5)), "", drtb(5))
                        Session("amt") = IIf(IsDBNull(drtb(1)), "", drtb(1)) - IIf(IsDBNull(drtb(0)), "", drtb(0))
                        amtd = IIf(IsDBNull(drtb(3)), "", drtb(3)) - IIf(IsDBNull(drtb(2)), "", drtb(2))
                        fn = IIf(IsDBNull(drtb(6)), "", drtb(6))



                        Select Case Trim(prod.SelectedItem.Text)

                            Case "DL"
                                xty = "LOAN"
                            Case "DCL"
                                xty = "LOAN"
                            Case "JL"
                            xty = "LOAN"
                        Case "IGP"
                            xty = "LOAN"

                        Case "ML"
                            xty = "LOAN"
                            Case "DS"
                            '  Session("amt") = amtd
                            xty = "Deposit"
                            Case "FD"
                                xty = "Deposit"
                            '    Session("amt") = amtd
                        Case "KMK"
                                xty = "Deposit"

                         '       Session("amt") = amtd
                        Case "RD"
                                xty = "Deposit"
                        '        Session("amt") = amtd
                        Case "RID"
                                xty = "Deposit"
                        '        Session("amt") = amtd
                        Case "SB"
                                xty = "Deposit"
                            '        Session("amt") = amtd
                    End Select

                        If xty = "LOAN" Then

                            If Not Session("amt") >= 0 Then

                                newrow = dt_j.NewRow

                                newrow(0) = Session("acn")
                                newrow(1) = fn
                                newrow(2) = -(Session("amt"))
                            rt = rt + Session("amt")
                            rc = rc + 1
                                dt_j.Rows.Add(newrow)
                            End If






                        ElseIf xty = "Deposit" Then

                        If Not Session("amt") = 0 Then

                            newrow = dt_j.NewRow

                            newrow(0) = Session("acn")
                            newrow(1) = fn
                            newrow(2) = Session("amt")
                            rc = rc + 1
                            rt = rt + Session("amt")

                            dt_j.Rows.Add(newrow)
                        End If


                    End If

                    End While

                End If

                lblnoc.Text = rc
                lbltotal.Text = String.Format("{0:N}", rt)
                
            Catch ex As Exception
                MsgBox(ex.ToString)
            Finally

            cmd.Dispose()
                con.Close()

            End Try

            Return dt_j

        End Function
        
        Sub disp_grid()
            Dim dt_j As DataTable = get_disp_data()
            disp.DataSource = dt_j
            disp.DataBind()
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

        Sub generate_pdf(ByVal acno As String)

            Dim br As String = get_home()

            Dim sql As String = "SELECT Master.date,Master.acno,Master.product,Master.amount,Master.cint,Master.dint,Master.prd,Master.prdtype,Master.mdate,Master.mamt,Master.cid,Master.pb,Master.pbi FROM dbo.master WHERE master.acno = '" + acno + "' "

            Dim adapter As New SqlDataAdapter(sql, con)


            adapter.Fill(ds)

            If Not ds.Tables(0).Rows.Count = 0 Then

                Session("ac_date") = ds.Tables(0).Rows(0).Item(0)
                Dim acn As String = ds.Tables(0).Rows(0).Item(1)
                Session("product") = Trim(ds.Tables(0).Rows(0).Item(2))
                Session("amt") = ds.Tables(0).Rows(0).Item(3)
                Session("cintr") = ds.Tables(0).Rows(0).Item(4)
                Session("dint") = ds.Tables(0).Rows(0).Item(5)
                Session("prd") = ds.Tables(0).Rows(0).Item(6)
                Session("prdtyp") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(7)), "M", ds.Tables(0).Rows(0).Item(7))
                Session("mdt") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(8)), Session("ac_date"), ds.Tables(0).Rows(0).Item(8))
                Session("mamt") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(9)), "0", ds.Tables(0).Rows(0).Item(9)) 'ds.Tables(0).Rows(0).Item(9)
                Session("ncid") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(10)), " ", ds.Tables(0).Rows(0).Item(10)) 'ds.Tables(0).Rows(0).Item(10)
                Session("pb") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(11)), "0", ds.Tables(0).Rows(0).Item(11)) 'ds.Tables(0).Rows(0).Item(11)
                Session("pbi") = IIf(IsDBNull(ds.Tables(0).Rows(0).Item(12)), "", ds.Tables(0).Rows(0).Item(12)) 'ds.Tables(0).Rows(0).Item(11)

            End If

            ds.Dispose()
            adapter.Dispose()

            sql = "SELECT FirstName,lastname,address,mobile from dbo.member where MemberNo='" + Session("ncid") + "'"

            Dim adapter1 As New SqlDataAdapter(sql, con)

            adapter1.Fill(ds1)

            If Not ds1.Tables(0).Rows.Count = 0 Then
                Session("ac_name") = ds1.Tables(0).Rows(0).Item(0)
                Session("ac_lname") = ds1.Tables(0).Rows(0).Item(1)
                Session("address") = ds1.Tables(0).Rows(0).Item(2)
                Session("mobile") = IIf(IsDBNull(ds1.Tables(0).Rows(0).Item(3)), "", ds1.Tables(0).Rows(0).Item(3))


            End If
            ds1.Dispose()
            adapter1.Dispose()


            Dim dt As DataTable = New DataTable
            'dt.Columns.AddRange(New DataColumn() {New DataColumn("Date", GetType(System.DateTime)), New DataColumn("Particulars", GetType(System.String)), New DataColumn("Debit", GetType(System.String))})
            If dt.Columns.Count = 0 Then
                dt.Columns.Add("Date", GetType(String))
                dt.Columns.Add("Particulars", GetType(String))
                dt.Columns.Add("drd", GetType(String))
                dt.Columns.Add("crd", GetType(String))
                dt.Columns.Add("cbal", GetType(String))
            End If



            Dim cbal As Double = 0
            Dim newrow As DataRow


            Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
            Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


            Dim reader As SqlDataReader



            Dim X As Integer = 1


            cmdi.Parameters.Clear()


            query = "SELECT  date,narration,due,drc,crc,cbal,tid FROM dbo.[actrans] WHERE actrans.acno=@acno and scroll=@scroll and actrans.date<=@dt ORDER BY date"

            If con.State = ConnectionState.Closed Then con.Open()

            cmdi.Connection = con
            cmdi.Parameters.AddWithValue("@acno", acno)
            cmdi.Parameters.AddWithValue("@scroll", X)
            cmdi.Parameters.AddWithValue("@dt", reformatted)
            cmdi.CommandText = query

            Try

                reader = cmdi.ExecuteReader()



                If reader.HasRows() Then

                    While reader.Read()

                        newrow = dt.NewRow

                        newrow(0) = FormatDateTime(reader(0), DateFormat.ShortDate).ToString
                        newrow(1) = reader(1).ToString + reader(2).ToString
                        newrow(2) = IIf(reader(3).ToString = 0, "", reader(3).ToString)
                        newrow(3) = IIf(reader(4).ToString = 0, "", reader(4).ToString)
                        cbal = (reader(4).ToString - reader(3).ToString) + cbal
                        newrow(4) = FormatNumber(cbal, 2).ToString


                        dt.Rows.Add(newrow)


                    End While

                End If

                reader.Close()



            Catch ex As Exception
                MsgBox(ex.Message)

            Finally


                ds.Dispose()
                cmd.Dispose()

                ' con.Close()


            End Try

            Dim sb As StringBuilder = New StringBuilder()


            sb.Append("<h4>KARAVILAI BENEFIT FUND LTD</h4>")
            sb.Append(br + " Branch")
            sb.Append("<br/>")
            sb.Append("STATEMENT OF ACCOUNTS")
            sb.Append("<hr style='border-top: 1px dotted red';>")
            sb.Append("<table>")
            sb.Append("<tbody>")
            sb.Append("<tr style='height: 30px;'>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; '>Customer Id :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none; width: 300px;'>" + Session("ncid") + "</td>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; margin-top: 10px;'></p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none; width: 300px;'>&nbsp;</td>")
            sb.Append("</tr>")
            sb.Append("<tr style='height: 30px;'>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; margin-top: 10px;'>First Name :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;'>")
            sb.Append(Session("ac_name"))
            sb.Append("</td>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; '>Last Name :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;'>" + Session("ac_lname") + "</td>")
            sb.Append("</tr>")
            sb.Append("<tr style='height: 30px;'>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; margin-top: 0px;'>Address :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;'>" + Session("address") + "</td>")
            sb.Append("</tr>")
            sb.Append("<tr>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; '>Mobile No</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;'>" + Session("mobile") + "</td>")
            sb.Append("</tr>")
            sb.Append("</tbody>")
            sb.Append("</table>")

            sb.Append("<hr>")

            sb.Append("<p style='margin-left: 0px; font-size: medium; font-variant-caps: small-caps;'><strong>Account Details</strong></p>")
            sb.Append("<table style='margin-left: 15px;'>")
            sb.Append("<tbody>")
            sb.Append("<tr style='height: 30px;'>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; '>Account :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;width:100px'>" + Session("product") + "</td>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; '>Account No :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;width:100px '>" + acno + "</td>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps;width:120px '>Deposit Amount :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;'>")
            sb.Append(FormatCurrency(Session("amt")))
            sb.Append("</td>")
            sb.Append("</tr>")
            sb.Append("<tr style='height: 30px;'>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; '>Date :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;width:100px '>" + Convert.ToString(FormatDateTime(Session("ac_date"), DateFormat.ShortDate)) + "</td>")
            sb.Append("<td style='border: none; '>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps;width: 200px;'>ROI :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;width:100px'>" + Convert.ToString(Session("cintr")) + "</td>")
            sb.Append("<td style='border: none;'>")
            sb.Append("<p style='font-size: smaller; font-variant-caps: all-small-caps; '>Account Balance:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>")
            sb.Append("</td>")
            sb.Append("<td style='border: none;width:100px'>" + FormatCurrency(cbal) + "</td>")
            sb.Append("</tr>")
            sb.Append("</tbody>")
            sb.Append("</table>")
            sb.Append("<hr>")

            'Table start.
            sb.Append("<table cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 9pt;font-family:Arial;width:100%'>")
            ''Adding HeaderRow.
            sb.Append("<thead style='display: table-header-group;'>")
            sb.Append("<tr>")
            'For Each column As DataColumn In dt.Columns
            '    sb.Append(("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>" _
            '                    + (column.ColumnName + "</th>")))
            'Next
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:12%;text-aligh:center;>Date</th>")
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:40%;text-aligh:center;>Description</th>")
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:12%;text-aligh:center;>Debit</th>")
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:13%;text-aligh:center;>Credit</th>")
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc';width:13%;text-aligh:center;>Balance</th>")

            sb.Append("</tr>")
            sb.Append("</thead>")






            sb.Append("<tbody>")
            ''Adding DataRow.
            For Each row As DataRow In dt.Rows
                sb.Append("<tr>")
                'For Each column As DataColumn In dt.Columns
                ' sb.Append(("<td style='width:100px;border: 1px solid #ccc'>" _
                '                + (row(column.ColumnName).ToString + "</td>")))


                sb.Append("<td style='border: 1px solid #ccc;width:12%'>" + row(0).ToString + "</td>")
                sb.Append("<td style='border: 1px solid #ccc;width:40%'>" + row(1).ToString + "</td>")
                sb.Append("<td style='text-align:right;border: 1px solid #ccc;width:12%'>" + row(2).ToString + "</td>")
                sb.Append("<td style='text-align:right;border: 1px solid #ccc;width:13%'>" + row(3).ToString + "</td>")
                sb.Append("<td style='text-align:right;border: 1px solid #ccc;width:13%'>" + row(4).ToString + "</td>")
                ' Next
                sb.Append("</tr>")
            Next
            ''Table end.
            sb.Append("</tbody>")
            sb.Append("</table>")
        '    Dim converter As New HtmlToPdf

        ' Dim html As String = sb.ToString
        'Dim doc As PdfDocument = converter.ConvertHtmlString(html, "")
        'Dim prefix = Mid(br, 1, 1)


        'Dim cleanString As String = acno.Replace("/", "")

        'Dim fpath = prefix.ToString + cleanString

        'doc.Save("d:\statements\" + prod.Text + "\" + fpath + ".Pdf")




    End Sub



        Sub generate_soa()

            For i As Integer = 0 To disp.Rows.Count - 1


                Dim acno As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblacn"), Label)
                generate_pdf(acno.Text)


            Next
        'Response.Redirect("/admin/dashboard.aspx", False)

    End Sub

        'Function get_bal(ByVal acn As String)
        '    Dim bal As Double = 0

        '    Dim dr As SqlDataReader

        '    Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        '    Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)


        '    If con1.State = ConnectionState.Closed Then con1.Open()
        '    cmd1.Connection = con1
        '    query = "SELECT SUM(actrans.Drd) AS expr1,SUM(actrans.Crd) AS expr2 FROM dbo.actrans WHERE  CONVERT(VARCHAR(20), date, 112) <='" + reformatted + "'"
        '    cmd1.CommandText = query

        '    Try

        '        dr = cmd.ExecuteReader()

        '        If dr.HasRows() Then
        '            dr.Read()

        '            bal = IIf(IsDBNull(dr(1)), 0, dr(1)) - IIf(IsDBNull(dr(0)), 0, dr(0))
        '        End If


        '    Catch ex As Exception
        '        MsgBox(ex.ToString)
        '    Finally
        '        dr.Close()
        '        cmd1.Dispose()
        '        con1.Close()



        '    End Try



        '    Return bal
        'End Function
        'Function get_cust(ByVal cid As String)
        '    Dim fnx As String = ""

        '    If con1.State = ConnectionState.Closed Then con1.Open()

        '    cmdi.Connection = con1

        '    query = "select firstname from member where memberno='" + Trim(cid) + "'"
        '    cmdi.CommandText = query

        '    Try
        '        fnx = cmdi.ExecuteScalar()
        '    Catch ex As Exception
        '        MsgBox(ex.ToString)

        '    Finally
        '        cmdi.Dispose()
        '        con1.Close()

        '    End Try




        '    Return fnx

        'End Function


        Sub disp_gridadd()

            Dim rt As Double = 0
            Dim rc As Integer = 0

            Dim xty As String = ""

            Dim fn As String
            Dim add As String
            Dim amtd As Double = 0


            disp.Visible = False
            dispadd.Visible = True

            dispadd.DataSource = Nothing
            dispadd.DataBind()



            Dim dat As DateTime = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
            Dim reformatted As String = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)



            Dim dt_j As New DataTable

            If dt_j.Columns.Count = 0 Then
                dt_j.Columns.Add("acno", GetType(String))
                dt_j.Columns.Add("party", GetType(String))
                dt_j.Columns.Add("amt", GetType(Decimal))
                dt_j.Columns.Add("address", GetType(String))

            End If

            If con.State = ConnectionState.Closed Then con.Open()

            'query = "select acno,cid from master where CONVERT(VARCHAR(20), date, 112) <='" + reformatted + "'"

            ' query = " SELECT SUM(actrans.Drc) AS expr1, SUM(actrans.Crc) AS expr2,SUM(actrans.Drd) AS expr3, SUM(actrans.Crd) AS expr4, Master.date,Master.acno,Master.serial,member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.actrans ON master.acno = actrans.acno LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo "
            'query &= "WHERE CONVERT(VARCHAR(20), actrans.date, 112) <= '" + reformatted + "'AND master.product = '" + Trim(prod.SelectedItem.Text) + "' GROUP BY master.date,Master.acno, member.FirstName  ORDER BY master.serial"
            query = " SELECT SUM(actrans.Drc) AS expr1, SUM(actrans.Crc) AS expr2,SUM(actrans.Drd) AS expr3, SUM(actrans.Crd) AS expr4, Master.date,Master.acno,member.FirstName FROM dbo.master LEFT OUTER JOIN dbo.actrans ON master.acno = actrans.acno LEFT OUTER JOIN dbo.member ON master.cid = member.MemberNo "
            query &= "WHERE CONVERT(VARCHAR(20), actrans.date, 112) <= '" + reformatted + "'AND master.product = '" + Trim(prod.SelectedItem.Text) + "' GROUP BY master.date,Master.acno, member.FirstName  ORDER BY master.DATE,MASTER.ACNO"



            cmd.Connection = con
            cmd.CommandText = query

            Dim drtb As SqlDataReader

            Try
                drtb = cmd.ExecuteReader()

                If drtb.HasRows() Then

                    While drtb.Read()

                        Session("acn") = IIf(IsDBNull(drtb(5)), "", drtb(5))
                        Session("amt") = IIf(IsDBNull(drtb(1)), "", drtb(1)) - IIf(IsDBNull(drtb(0)), "", drtb(0))
                        amtd = IIf(IsDBNull(drtb(3)), "", drtb(3)) - IIf(IsDBNull(drtb(2)), "", drtb(2))
                        fn = IIf(IsDBNull(drtb(6)), "", drtb(6))
                        add = get_add(Session("acn"))


                        Select Case Trim(prod.SelectedItem.Text)

                            Case "DL"
                                xty = "LOAN"
                            Case "DCL"
                                xty = "LOAN"
                            Case "JL"
                                xty = "LOAN"
                            Case "ML"
                                xty = "LOAN"
                            Case "DS"
                                Session("amt") = amtd
                                xty = "Deposit"
                            Case "FD"
                                xty = "Deposit"
                                Session("amt") = amtd
                            Case "KMK"
                                xty = "Deposit"

                                Session("amt") = amtd
                            Case "RD"
                                xty = "Deposit"
                                Session("amt") = amtd
                            Case "RID"
                                xty = "Deposit"
                                Session("amt") = amtd
                            Case "SB"
                                xty = "Deposit"
                                Session("amt") = amtd
                        End Select

                        If xty = "LOAN" Then

                            If Not Session("amt") >= 0 Then

                                newrow = dt_j.NewRow

                                newrow(0) = Session("acn")
                                newrow(1) = fn
                                newrow(2) = -(Session("amt"))
                                newrow(3) = add
                                rt = rt + Session("amt")
                                rc = rc + 1
                                dt_j.Rows.Add(newrow)
                            End If






                        ElseIf xty = "Deposit" Then
                            If Not Session("amt") <= 0 Then

                                newrow = dt_j.NewRow

                                newrow(0) = Session("acn")
                                newrow(1) = fn
                                newrow(2) = Session("amt")
                                newrow(3) = add
                                rc = rc + 1
                                rt = rt + Session("amt")

                                dt_j.Rows.Add(newrow)
                            End If


                        End If

                    End While
                    dispadd.DataSource = dt_j
                    dispadd.DataBind()


                End If

                lblnoc.Text = rc
                lbltotal.Text = String.Format("{0:N}", rt)

            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try




        End Sub

        Function get_add(ByVal acn As String)

            Dim add As String = ""

            If con1.State = ConnectionState.Closed Then con1.Open()

            cmdi.Connection = con1


            query = "SELECT member.Address FROM dbo.master INNER JOIN dbo.member  ON master.cid = member.MemberNo WHERE master.acno = @acn"
            cmdi.Parameters.Clear()
            cmdi.CommandText = query
            cmdi.Parameters.AddWithValue("@acn", Trim(acn))
            Try
                add = cmdi.ExecuteScalar()
            Catch ex As Exception

                Response.Write(ex.ToString)
            Finally
                cmdi.Dispose()

            End Try


            Return add


        End Function

        Private Sub prod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles prod.SelectedIndexChanged


        End Sub

    Private Sub btn_inw_Click(sender As Object, e As EventArgs) Handles btn_inw.Click

        '  If isadd.Checked Then

        ' disp_gridadd()


        '   Else
        disp_grid()
        '
        'btn_r'ef.Visible = True
        'btnexport.Visible = True
        'End If


    End Sub

    Private Sub btn_ref_Click(sender As Object, e As EventArgs) Handles btn_ref.Click



            If con.State = ConnectionState.Closed Then con.Open()

        query = "select acno,cld from master  GROUP BY acno,cld"






        cmd.Connection = con
        cmd.CommandText = query


        Dim drtb As SqlDataReader

        Try
            drtb = cmd.ExecuteReader()

            If drtb.HasRows() Then

                While drtb.Read()


                    ' reform_ac(Trim(drtb(0)), Trim(drtb(1)))
                    update_ac(drtb(0), drtb(1))
                    '                
                End While
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

        btn_ref.Visible = False

        End Sub

    Sub reform_ac(ByVal acn As String, ByVal sch As String)
        Dim prefix As String = String.Empty
        Dim acpart As String

        prefix = "6320" + Mid(acn, 6, 2) + "0"
        acpart = Mid(acn, 1, 4)
        Session("acno") = prefix + acpart

        If con1.State = ConnectionState.Closed Then con1.Open()

        query = "UPDATE dbo.master SET sch = @sch WHERE acno = @acno"
        cmd1.Connection = con1
        cmd1.CommandText = query
        cmd1.Parameters.Clear()
        cmd1.Parameters.AddWithValue("@acno", Session("acno"))
        cmd1.Parameters.AddWithValue("@sch", sch)
        Try
            cmd1.ExecuteNonQuery()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        cmd1.Dispose()



    End Sub

    Sub update_ac(ByVal acn As String, ByVal cld As Boolean)

        If con1.State = ConnectionState.Closed Then con1.Open()
        cmdi.Parameters.Clear()
        cmdi.Connection = con1
        cmdi.CommandText = query
        query = "UPDATE dbo.jlstock SET cld =@cld WHERE acn = @acn"
        cmdi.Parameters.AddWithValue("@acn", acn)
        cmdi.Parameters.AddWithValue("@cld", cld)
        Try
            cmdi.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        'If con1.State = ConnectionState.Closed Then con1.Open()
        'cmdi.Parameters.Clear()
        'cmdi.Connection = con1
        'cmdi.CommandText = query
        'query = "UPDATE dbo.MASTER SET cld = 1 WHERE ACNO = @acn"
        'cmdi.Parameters.AddWithValue("@acn", acn)
        'Try
        '    cmdi.ExecuteNonQuery()
        'Catch ex As Exception
        '    Response.Write(ex.Message)
        'End Try



    End Sub

    Private Sub ExportGridToExcel()
            Response.Clear()
            Response.Buffer = True
            Response.ClearContent()
            Response.ClearHeaders()
            Response.Charset = ""
            Dim FileName As String = "Ledger" & DateTime.Now & ".xls"
            Dim strwritter As StringWriter = New StringWriter()
            Dim htmltextwrtter As HtmlTextWriter = New HtmlTextWriter(strwritter)
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment;filename=" & FileName)

            If disp.Visible = True Then
                disp.GridLines = GridLines.Both
                disp.HeaderStyle.Font.Bold = True
                disp.AllowPaging = False
                disp_grid()
                disp.RenderControl(htmltextwrtter)
                Response.Write(strwritter.ToString())
                Response.[End]()
            Else
                dispadd.GridLines = GridLines.Both
                dispadd.HeaderStyle.Font.Bold = True
                dispadd.AllowPaging = False
                disp_gridadd()
                dispadd.RenderControl(htmltextwrtter)
                Response.Write(strwritter.ToString())
                Response.[End]()
            End If

        End Sub

    ' Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    'End Sub

    '    Private Sub btnexport_Click(sender As Object, e As EventArgs) Handles btnexport.Click
    '        ExportGridToExcel()

    '    End Sub

    '    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click

    'End Sub

    '    Private Sub btnsoa_Click(sender As Object, e As EventArgs) Handles btnsoa.Click
    '        generate_soa()

    '    End Sub

    Private Sub ProTrialBalance_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

        If Session("session_user_role") = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"
        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Private Sub btnprnt_Click(sender As Object, e As EventArgs) Handles btnprnt.Click

        Dim stitle As String = String.Empty
        Dim msg As String = String.Empty
        disp.AllowPaging = False
        bind_grid()

        Select Case Trim(prod.SelectedItem.Text)
            Case "DS"
                stitle = "Daily Deposit"
            Case "FD"
                stitle = "Fixed Deposit"
            Case "KMK"
                stitle = "KMK Deposit"
            Case "RD"
                stitle = "Recurring Deposit"
            Case "RID"
                stitle = "Reinvestment Deposit"
            Case "SB"
                stitle = "Savings Deposit"
            Case "DCL"
                stitle = "Demand Cash Loan"
            Case "DL"
                stitle = "Demand Loan"
            Case "JL"
                stitle = "Jewel Loan"
            Case "ML"
                stitle = "Mortgage Loan"
        End Select


        ' Fetch complete raw data bypassing pagination limitations
        Dim pdfDt As DataTable = get_disp_data()

        If pdfDt Is Nothing OrElse pdfDt.Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('No records found to print.');", True)
            Return
        End If

        Dim pdfBytes As Byte() = GenerateTrialBalancePdf(pdfDt, stitle, txtdate.Text, lbltotal.Text)

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Disposition", "inline; filename=TrialBalance_" & prod.SelectedItem.Text & "_" & txtdate.Text & ".pdf")
        Response.BinaryWrite(pdfBytes)
        Response.End()
        
    End Sub

    Public Shared Function GenerateTrialBalancePdf(dt As DataTable, reportTitle As String, reportDate As String, grandTotal As String) As Byte()

        Using ms As New MemoryStream()

            Dim doc As New Document(PageSize.A4, 20, 20, 30, 40)

            Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
            
            Dim printAction As PdfAction = New PdfAction(PdfAction.PRINTDIALOG)
            writer.SetOpenAction(printAction)

            doc.Open()

            ' ===== TITLE =====
            Dim titleFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
            Dim subFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13, BaseColor.GRAY)

            Dim title As New Paragraph("Karavalai Nidhi Ltd • Multiple Ledger", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            title.SpacingAfter = 4

            Dim subtitle As New Paragraph($"Branch: Karavilai | {reportTitle} | As of: {reportDate}", subFont)
            subtitle.Alignment = Element.ALIGN_CENTER
            subtitle.SpacingAfter = 14

            doc.Add(title)
            doc.Add(subtitle)

            ' ===== TABLE =====
            Dim table As New PdfPTable(4)
            table.WidthPercentage = 100
            table.SetWidths({10.0F, 20.0F, 45.0F, 25.0F}) ' Match HTML GridView widths roughly
            table.HeaderRows = 1

            ' Header
            Dim headerFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE)
            Dim headers As String() = {"S.No", "Account No", "Customer Name", "Balance"}

            For Each headerText In headers
                Dim cell As New PdfPCell(New Phrase(headerText, headerFont))
                cell.BackgroundColor = New BaseColor(37, 99, 235) ' Fiscus blue header
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                cell.Padding = 6
                table.AddCell(cell)
            Next

            ' Data
            Dim cellFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 9)
            Dim numFont As Font = FontFactory.GetFont(FontFactory.COURIER, 9)
            Dim rowIndex As Integer = 1

            For Each row As DataRow In dt.Rows
                
                ' S.No
                Dim cellSno As New PdfPCell(New Phrase(rowIndex.ToString(), cellFont))
                cellSno.HorizontalAlignment = Element.ALIGN_CENTER
                cellSno.Padding = 5
                table.AddCell(cellSno)

                ' Acno
                Dim cellAcno As New PdfPCell(New Phrase(Convert.ToString(row("acno")), cellFont))
                cellAcno.HorizontalAlignment = Element.ALIGN_RIGHT
                cellAcno.Padding = 5
                table.AddCell(cellAcno)

                ' Party
                Dim cellParty As New PdfPCell(New Phrase(Convert.ToString(row("party")), cellFont))
                cellParty.HorizontalAlignment = Element.ALIGN_LEFT
                cellParty.Padding = 5
                table.AddCell(cellParty)

                ' Balance
                Dim amt As Decimal = Convert.ToDecimal(row("amt"))
                Dim formattedAmt As String = amt.ToString("N2")
                Dim cellAmt As New PdfPCell(New Phrase(formattedAmt, numFont))
                cellAmt.HorizontalAlignment = Element.ALIGN_RIGHT
                cellAmt.Padding = 5
                table.AddCell(cellAmt)

                rowIndex += 1
            Next

            ' Totals Row
            Dim cellEmpty1 As New PdfPCell(New Phrase("", cellFont))
            cellEmpty1.Border = Rectangle.NO_BORDER
            table.AddCell(cellEmpty1)

            Dim cellTotalLabel As New PdfPCell(New Phrase("TOTAL", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)))
            cellTotalLabel.HorizontalAlignment = Element.ALIGN_RIGHT
            cellTotalLabel.Padding = 5
            table.AddCell(cellTotalLabel)

            Dim cellTotalRecords As New PdfPCell(New Phrase((rowIndex - 1).ToString() & " Accounts", cellFont))
            cellTotalRecords.HorizontalAlignment = Element.ALIGN_LEFT
            cellTotalRecords.Padding = 5
            table.AddCell(cellTotalRecords)

            Dim cellGrandTotal As New PdfPCell(New Phrase(grandTotal, FontFactory.GetFont(FontFactory.COURIER_BOLD, 9)))
            cellGrandTotal.HorizontalAlignment = Element.ALIGN_RIGHT
            cellGrandTotal.Padding = 5
            table.AddCell(cellGrandTotal)

            doc.Add(table)
            doc.Close()

            Return ms.ToArray()

        End Using

    End Function

    Private Sub btnxl_Click(sender As Object, e As EventArgs) Handles btnxl.Click


        'Private Sub isadd_CheckedChanged(sender As Object, e As EventArgs) Handles isadd.CheckedChanged
        '    If isadd.Checked Then
        '        disp_gridadd()
        '        ' btnexport.Visible = True
        '    Else
        '        disp_grid()

        '    End If
        'End Sub

        Response.Clear()

        Response.Buffer = True



        Response.AddHeader("content-disposition", "attachment;filename=TrialBalance.xls")

        Response.Charset = ""

        Response.ContentType = "application/vnd.ms-excel"



        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)



        disp.AllowPaging = False

        bind_grid()




        'Change the Header Row back to white color

        '   disp.HeaderRow.Style.Add("background-color", "#FFFFFF")



        'Apply style to Individual Cells




        For i As Integer = 0 To disp.Rows.Count - 1

            Dim row As GridViewRow = disp.Rows(i)




            'Apply text style to each Row

            '  row.Attributes.Add("class", "textmode")



            'Apply style to Individual Cells of Alternating Row



        Next

        disp.RenderControl(hw)



        'style to format numbers to string

        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"

        Response.Write(style)

        Response.Output.Write(sw.ToString())

        Response.Flush()

        Response.End()

    End Sub


    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        ' Verifies that the control is rendered

    End Sub



    Private Sub btnprnt_Command(sender As Object, e As CommandEventArgs) Handles btnprnt.Command

    End Sub
End Class