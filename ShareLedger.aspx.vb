Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging




Public Class ShareLedger
    Inherits System.Web.UI.Page

    Public closed As Integer = 0
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Public ds_trans As New DataSet
    Dim countresult As Integer
    Dim ds As New DataSet
    Dim ds1 As New DataSet

    Dim query As String

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        If Not Page.IsPostBack Then

            ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", txtcid.ClientID), True)


        End If

    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        disp.PageIndex = e.NewPageIndex
        bind_grid()
    End Sub
    Sub bind_grid()
        If con.State = ConnectionState.Closed Then con.Open()

        If Session("sesusr") = "RAM KUMAR" Then
            query = "select date,sl,shrfrm,shrto,allocation,shrval,trffrm,trfcid from share where cid='" + txtcid.Text + "' order by date"
        Else
            query = "select date,sl,shrfrm,shrto,allocation,shrval from share where cid='" + txtcid.Text + "' and showall= 0 order by date"
        End If
        cmd.Connection = con
        cmd.CommandText = query
        Try
            Dim ds As New DataTable
            Dim adapter As New SqlDataAdapter(query, con)

            adapter.Fill(ds)
            disp.DataSource = ds
            disp.DataBind()

            Dim sum As Object = ds.Compute("sum(allocation)", "")

            If Not sum.ToString = "" Then

                lblshrnos.Text = sum.ToString
            Else
                lblshrnos.Text = "0"
            End If


            sum = ds.Compute("sum(shrval)", "")

            If Not sum.ToString = "" Then

                lblnetshrval.Text = FormatCurrency(sum.ToString)
            Else
                lblnetshrval.Text = "0.00"
            End If

            'lblshrnos.Text = shrno
            ' lblnetshrval.Text = FormatCurrency(shrval)
            'trim_disp()


        Catch ex As Exception
            Response.Write(ex.ToString)

        End Try
    End Sub


    Sub get_member(ByVal cid As String)
        If con.State = ConnectionState.Closed Then con.Open()

        query = "select lastname,dob,gender,pincode,phone,mobile,email from member where member.memberno='" + Trim(cid) + "'" '
        cmd.Connection = con
        cmd.CommandText = query
        Dim dr As SqlDataReader
        Try
            dr = cmd.ExecuteReader()
            If dr.HasRows() Then
                dr.Read()
                lblconame.Text = IIf(IsDBNull(dr(0)), "", dr(0))

            End If
            dr.Close()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try


        Dim imgx As Image = Nothing
        Dim imgbytes As Byte() = Nothing
        Dim stream As MemoryStream = Nothing

        If con.State = ConnectionState.Closed Then con.Open()

        query = "select  photo,specimen,parent,pan,poi,poi_no,poi_by,poi_on,poa,poa_no,poa_by,poa_on from dbo.[kyc] where MemberNo='" + cid + "'"
        cmd.Connection = con
        cmd.CommandText = query


        Try
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            reader.Read()
            If reader.HasRows Then
                If Not IsDBNull(reader(0)) Then
                    imgbytes = CType(reader.GetValue(0), Byte())
                    stream = New MemoryStream(imgbytes, 0, imgbytes.Length)
                    imgx = Image.FromStream(stream)
                    '  imgx.Save("../captures/webcam.png", System.Drawing.Imaging.ImageFormat.Png)

                    Dim imagePath As String = String.Format("~/Captures/{0}.png", "webcam")
                    File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                    'Session("CapturedImage") = ResolveUrl(imagePath)
                    memimg.ImageUrl = "../captures/webcam.png"
                End If


            Else
                memimg.ImageUrl = "../Images/user.png"

            End If


                reader.Close()
            bind_grid()


        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            cmd.Dispose()
            con.Close()

        End Try



    End Sub


    Sub fetch_cust()
        Dim ds As New DataSet
        'Dim i As Integer

        If con.State = ConnectionState.Closed Then con.Open()


        Dim sql As String = "select FirstName,LastName,Address,mobile from member where MemberNo = '" + Trim(txtcid.Text) + "'"
        Dim adapter As New SqlDataAdapter(sql, con)

        Try

            '  lblcid404.Visible = False
            adapter.Fill(ds)

            If Not ds.Tables(0).Rows.Count = 0 Then

                lblfname.Text = Trim(ds.Tables(0).Rows(0).ItemArray(0).ToString)
                lblconame.Text = Trim(ds.Tables(0).Rows(0).ItemArray(1).ToString)
                lbladd.Text = Trim(ds.Tables(0).Rows(0).ItemArray(2).ToString)
                lblpin.Text = Trim(ds.Tables(0).Rows(0).ItemArray(3).ToString)

            Else
                ' lblcid404.Visible = True
                txtfocus(txtcid)
                Exit Sub
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)

        Finally

            con.Close()

        End Try

    End Sub


    'Private Sub toshow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles toshow.SelectedIndexChanged

    '    If toshow.SelectedItem.Text = "Active Deposits" Then
    '        closed = 0
    '        get_deposits()

    '    ElseIf toshow.SelectedItem.Text = "Closed Deposits" Then
    '        closed = 1
    '        get_deposits()

    '    End If
    '    End Sub


    Private Sub ShareLedger_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

        If session_user_role IsNot Nothing Then

            If session_user_role = "Admin" Then
                Me.MasterPageFile = "~/admin/Admin.Master"

            Else
                Me.MasterPageFile = "~/User/User.Master"
            End If
        Else
            Response.Redirect("/Login.aspx")

        End If

    End Sub

    Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged

        If Not Trim(txtcid.Text) = "" Then
            fetch_cust()
            get_member(txtcid.Text)
        End If


    End Sub
End Class