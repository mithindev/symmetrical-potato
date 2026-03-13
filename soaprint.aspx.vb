Imports System.Data.SqlClient
Imports System.Globalization

Public Class soaprint
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con1.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Me.IsPostBack Then

            Dim param1 As String = Session("soaprint")
            ' ViewState("RefUrl") = Request.UrlReferrer.ToString()
            Dim ptype As String = Request.QueryString("ptype")
            If String.IsNullOrEmpty(ptype) Then ptype = "c"

            If Not param1 = "" Then
                get_acc_details(param1, ptype)
                bind_grid(ptype)

            End If

        End If

    End Sub
    Private Sub bind_grid(ptype As String)


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
            dt.Columns.Add("type", GetType(String))


        End If



        Dim X As Integer = 1

        disp.EmptyDataText = "No Records Found"
        cmdi.Parameters.Clear()

        If Session("flt") = False Then

            If session_user_role = "Audit" Then
                If ptype = "d" Then
                    query = "SELECT  date,narration,due,drc as drd,crc as crd,type FROM dbo.[actransc] WHERE actransc.acno=@acno and scroll=@scroll  ORDER BY date,tid"
                Else
                    query = "SELECT  date,narration,due,drd,crd,type FROM dbo.[actransc] WHERE actransc.acno=@acno and scroll=@scroll  ORDER BY date,tid"
                End If
            Else
                If ptype = "d" Then
                    query = "SELECT  date,narration,due,drc as drd,crc as crd,type FROM dbo.[actrans] WHERE actrans.acno=@acno and scroll=@scroll  ORDER BY date,tid "
                Else
                    query = "SELECT  date,narration,due,drd,crd,type FROM dbo.[actrans] WHERE actrans.acno=@acno and scroll=@scroll  ORDER BY date,tid "
                End If
            End If
            lblstprd.Text = ""
        Else

            Dim dat As DateTime = DateTime.ParseExact(Session("frmdt"), "dd-MM-yyyy", CultureInfo.InvariantCulture)
            reformatted = dat.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            Dim dat1 As DateTime = DateTime.ParseExact(Session("todt"), "dd-MM-yyyy", CultureInfo.InvariantCulture)
            reformatted1 = dat1.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            lblstprd.Text = "Statement from " + Session("frmdt") + " To " + Session("todt")

            newrow = dt.NewRow

            newrow(0) = Session("frmdt")
            newrow(1) = "Opening Balance"
            newrow(2) = ""
            newrow(3) = 0
            newrow(4) = 0
            dbal = get_opening(Trim(Session("acno")), reformatted, ptype)
            newrow(5) = dbal
            newrow(6) = ""



            dt.Rows.Add(newrow)


            If session_user_role = "Audit" Then
                If ptype = "d" Then
                    query = "SELECT  date,narration,due,drc as drd,crc as crd,type FROM dbo.[actransc] WHERE actransc.acno=@acno and scroll=@scroll and CONVERT(VARCHAR(20), actransc.date, 112) between @frm and @to ORDER BY date,tid"
                Else
                    query = "SELECT  date,narration,due,drd,crd,type FROM dbo.[actransc] WHERE actransc.acno=@acno and scroll=@scroll and CONVERT(VARCHAR(20), actransc.date, 112) between @frm and @to ORDER BY date,tid"
                End If
            Else
                If ptype = "d" Then
                    query = "SELECT  date,narration,due,drc as drd,crc as crd,type FROM dbo.[actrans] WHERE actrans.acno=@acno and scroll=@scroll and CONVERT(VARCHAR(20), actrans.date, 112) between @frm and @to  ORDER BY date,tid "
                Else
                    query = "SELECT  date,narration,due,drd,crd,type FROM dbo.[actrans] WHERE actrans.acno=@acno and scroll=@scroll and CONVERT(VARCHAR(20), actrans.date, 112) between @frm and @to  ORDER BY date,tid "
                End If
            End If


        End If


        cmdi.Connection = con
        cmdi.Parameters.AddWithValue("@acno", Trim(Session("acno")))
        cmdi.Parameters.AddWithValue("@scroll", X)
        If Session("flt") = True Then
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
                    newrow(6) = reader(5).ToString


                    dt.Rows.Add(newrow)


                End While

            End If



            disp.DataSource = dt
            disp.DataBind()




        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            ' ds.Dispose()
            cmd.Dispose()
            con.Close()


        End Try




        trim_disp()




    End Sub

    Public Function get_opening(ByVal acn As String, ByVal dt As String, ptype As String)



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
        If ptype = "d" Then
            Return Session("cbal")
        Else
            Return Session("dbal")
        End If

    End Function

    Sub trim_disp()

        '' For i As Integer = disp.Rows.Count - 1 To 0 Step -1
        For i As Integer = 0 To disp.Rows.Count - 1


            Dim cr As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblcr"), Label)
            Dim dr As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbldr"), Label)
            Dim dat As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbldat"), Label)

            Dim cbal As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lblbal"), Label)
            Dim typ As Label = DirectCast(disp.Rows(i).Cells(0).FindControl("lbltyp"), Label)

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

            If typ.Text = "CASH" Then
                typ.Text = "CASH"
            Else
                typ.Text = "TRF"
            End If
            'If bal.Text = 0 Then
            '    bal.Text = ""
            'End If
            dat.Text = Convert.ToDateTime(dat.Text).ToString("dd/MM/yyyy") 'String.Format("{0:dd/mm/yyyy}", dat.Text)



        Next
        ' disp.PageIndex = disp.PageCount

        '  pgtot(disp.PageIndex) = total
    End Sub

    Function get_bal(ByVal acno As String, ptype As String)
        Dim bal As Double = 0

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    If ptype = "d" Then
                        query = "SELECT COALESCE(SUM(actransc.Crc) - SUM(actransc.Drc),0) AS bal FROM dbo.actransc WHERE actransc.acno = @acno and scroll=1"
                    Else
                        query = "SELECT COALESCE(SUM(actransc.Crd) - SUM(actransc.Drd),0) AS bal FROM dbo.actransc WHERE actransc.acno = @acno and scroll=1"
                    End If
                Else
                    If ptype = "d" Then
                        query = "SELECT COALESCE(SUM(actrans.Crc) - SUM(actrans.Drc),0) AS bal FROM dbo.actrans WHERE actrans.acno = @acno and scroll=1"
                    Else
                        query = "SELECT COALESCE(SUM(actrans.Crd) - SUM(actrans.Drd),0) AS bal FROM dbo.actrans WHERE actrans.acno = @acno and scroll=1"
                    End If
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
    Sub get_acc_details(ByVal acno As String, ptype As String)


        get_soa_details(acno)



        lblacn.Text = Session("acno") + "( " + Session("Product") + " )"

        lblcid.Text = Session("cid")
        lblbr.Text = get_home()



        lblrpt.Text = "Report Date : " + DateAndTime.Now.ToString
        
        Dim balType As String = ""
        If ptype = "d" Then balType = " (D-Interest)" Else balType = " (C-Interest)"

        Dim bal As String = FormatCurrency(get_bal(Session("acno"), ptype))

        lblbal.Text = "Effective Balance" + balType + " as on " + DateTime.Now.ToString + " is  " + bal

        get_profile(Session("cid"))

        lblname.Text = Session("firstname")
        lbllname.Text = Session("lastname")
        lbladd.Text = Session("address")
        lblmobile.Text = Session("mobile")








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
                            lblnominee.Text = "Yes"
                        End If


                    End Using


                Catch ex As Exception
                    Response.Write(ex.ToString)
                End Try
            End Using
            con.Close()

        End Using




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

    Private Sub soaprint_Unload(sender As Object, e As EventArgs) Handles Me.Unload

        Session("frmdt") = Nothing
        Session("todt") = Nothing
        Session("flt") = False

    End Sub

    Private Sub soaprint_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class