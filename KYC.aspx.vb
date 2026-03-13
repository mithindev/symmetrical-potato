Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Net.Mail

Public Class KYC
    Inherits System.Web.UI.Page

    Protected Friend WithEvents txtname As System.Web.UI.WebControls.TextBox



    Dim ncid As String
    Dim filename As String
    Dim oResult As String
    Dim query As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'For Getting: On Page2.aspx 
        '  ncid = Session("id4kyc").ToString


        ' Response.Write(ncid)
        If Not Me.IsPostBack Then
            txtcid.Text = Session("cid")

            If Not txtcid.Text = "" Then
                txtcid_TextChanged(sender, e)



            End If



            If Request.InputStream.Length > 0 Then
                Using reader As New StreamReader(Request.InputStream)
                    Dim hexString As String = Server.UrlEncode(reader.ReadToEnd())
                    Dim imageName As String = DateTime.Now.ToString("dd-MM-yy hh-mm-ss")
                    Dim imagePath As String = String.Format("~/Captures/{0}.png", "webcam")
                    File.WriteAllBytes(Server.MapPath(imagePath), ConvertHexToBytes(hexString))
                    Session("CapturedImage") = ResolveUrl(imagePath)
                    Session("upimg") = ConvertHexToBytes(hexString)


                End Using
            End If
            'txtfocus(txtcid)


        End If


        ' txtnominee.Focus()



    End Sub

    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub

    Private Shared Function ConvertHexToBytes(hex As String) As Byte()
        Dim bytes As Byte() = New Byte(hex.Length / 2 - 1) {}
        For i As Integer = 0 To hex.Length - 1 Step 2
            bytes(i / 2) = Convert.ToByte(hex.Substring(i, 2), 16)
        Next
        Return bytes
    End Function


    <WebMethod()>
    Public Shared Function SaveCapturedImage(ByVal data As String) As Boolean
        Dim fileName As String = "ImgCaptured" 'DateTime.Now.ToString("dd-MM-yy hh-mm-ss")

        'Convert Base64 Encoded string to Byte Array.
        Dim imageBytes() As Byte = Convert.FromBase64String(data.Split(",")(1))
        HttpContext.Current.Session("upimg") = imageBytes

        'Save the Byte Array as Image File.
        Dim filePath As String = HttpContext.Current.Server.MapPath(String.Format("~/Captures/{0}.jpg", fileName))
        File.WriteAllBytes(filePath, imageBytes)
        Return True
    End Function


    '   Private Sub grpid_Click(sender As Object, e As ImageClickEventArgs) Handles grpid.Click
    '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "showmodalpop()", True)


    ' End Sub







    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click

        If cp.Checked = True Then
            If Not txtotp.Text = Session("otp") Then
                lblotp.Visible = True
                Exit Sub
            End If
        End If

        Dim qresult As Integer
        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        Using conLocal As New SqlConnection(connString)
            conLocal.Open()

            Dim img_photo() As Byte = DirectCast(Context.Session("upimg"), Byte())
            Dim img_spec() As Byte = DirectCast(Context.Session("imgspec"), Byte())
            Dim img_poi() As Byte = DirectCast(Context.Session("imgpoi"), Byte())
            Dim img_poa() As Byte = DirectCast(Context.Session("imgpoa"), Byte())
            Dim isact = IIf(isactiv.Checked = True, 1, 0)

            Dim queryUpdateMember = "update member set active=@active where memberno=@mem"
            Using cmdLocal As New SqlCommand(queryUpdateMember, conLocal)
                cmdLocal.Parameters.AddWithValue("@active", isact)
                cmdLocal.Parameters.AddWithValue("@mem", Trim(txtcid.Text))
                Try
                    cmdLocal.ExecuteNonQuery()
                Catch ex As Exception
                    ' Log error if needed
                End Try
            End Using

            Dim qryCount = "select count(*) as expr1 from kyc where kyc.memberno=@mem"
            Using cmdCount As New SqlCommand(qryCount, conLocal)
                cmdCount.Parameters.AddWithValue("@mem", Trim(txtcid.Text))
                qresult = Convert.ToInt32(cmdCount.ExecuteScalar())
            End Using

            If qresult = 0 Then
                Dim qryInsert = "INSERT INTO kyc(Memberno,Parent,Nominee,Relationship,photo,specimen,pan,poi,poi_no,poi_by,poi_on,poa,poa_no,poa_by,poa_on,appno,cp,poi_pic,poa_pic)"
                qryInsert &= " VALUES(@Memberno,@Parent,@Nominee,@Relationship,@imagephoto,@specimen,@pan,@poi,@poi_no,@poi_by,@poi_on,@poa,@poa_no,@poa_by,@poa_on,@appno,@cp,@poi_pic,@poa_pic)"

                Using cmdInsert As New SqlCommand(qryInsert, conLocal)
                    cmdInsert.Parameters.AddWithValue("@Memberno", txtcid.Text.Trim())
                    cmdInsert.Parameters.AddWithValue("@Parent", txtgrpid.Text.Trim())
                    cmdInsert.Parameters.AddWithValue("@Nominee", txtnominee.Text.Trim())
                    cmdInsert.Parameters.AddWithValue("@Relationship", txtrelation.Text.Trim())

                    If img_photo Is Nothing Then
                        cmdInsert.Parameters.Add("@imagephoto", SqlDbType.Image).Value = DBNull.Value
                    Else
                        cmdInsert.Parameters.AddWithValue("@imagephoto", img_photo)
                    End If

                    If img_spec Is Nothing Then
                        cmdInsert.Parameters.Add("@specimen", SqlDbType.Image).Value = DBNull.Value
                    Else
                        cmdInsert.Parameters.AddWithValue("@specimen", img_spec)
                    End If

                    cmdInsert.Parameters.AddWithValue("@pan", txtpan.Text.Trim())
                    cmdInsert.Parameters.AddWithValue("@appno", txtappno.Text.Trim())
                    cmdInsert.Parameters.AddWithValue("@cp", IIf(cp.Checked, 1, 0))
                    cmdInsert.Parameters.AddWithValue("@poi", idprof.SelectedItem.Text)
                    cmdInsert.Parameters.AddWithValue("@poi_no", txtiddocno.Text.Trim())
                    cmdInsert.Parameters.AddWithValue("@poi_by", DBNull.Value)
                    cmdInsert.Parameters.AddWithValue("@poi_on", DBNull.Value)
                    cmdInsert.Parameters.AddWithValue("@poa", addprof.SelectedItem.Text)
                    cmdInsert.Parameters.AddWithValue("@poa_no", txtadd_doc.Text.Trim())
                    cmdInsert.Parameters.AddWithValue("@poa_by", DBNull.Value)
                    cmdInsert.Parameters.AddWithValue("@poa_on", DBNull.Value)
                    
                    If img_poi Is Nothing Then
                        cmdInsert.Parameters.Add("@poi_pic", SqlDbType.VarBinary).Value = DBNull.Value
                    Else
                        cmdInsert.Parameters.AddWithValue("@poi_pic", img_poi)
                    End If

                    If img_poa Is Nothing Then
                        cmdInsert.Parameters.Add("@poa_pic", SqlDbType.VarBinary).Value = DBNull.Value
                    Else
                        cmdInsert.Parameters.AddWithValue("@poa_pic", img_poa)
                    End If
                    
                    Try
                        cmdInsert.ExecuteNonQuery()
                        ShowSuccessMessage()
                    Catch ex As Exception
                        Response.Write(ex.Message)
                    End Try
                End Using
            Else
                Dim qryUpdateKyc = "UPDATE KYC set Parent=@Parent,Nominee=@Nominee,Relationship=@Relationship,"
                
                If Not img_photo Is Nothing Then
                    qryUpdateKyc &= "photo=@imagephoto,"
                End If
                
                If Not img_spec Is Nothing Then
                    qryUpdateKyc &= "specimen=@specimen,"
                End If

                qryUpdateKyc &= "pan=@pan,poi=@poi,poi_no=@poi_no,poi_by=@poi_by,poi_on=@poi_on,poa=@poa,poa_no=@poa_no,poa_by=@poa_by,appno=@appno,cp=@cp,"
                
                If Not img_poi Is Nothing Then
                    qryUpdateKyc &= "poi_pic=@poi_pic,"
                End If
                
                If Not img_poa Is Nothing Then
                    qryUpdateKyc &= "poa_pic=@poa_pic,"
                End If
                
                qryUpdateKyc = qryUpdateKyc.TrimEnd(","c) & " where (kyc.memberno=@Memberno )"

                Using cmdUpdate As New SqlCommand(qryUpdateKyc, conLocal)
                    cmdUpdate.Parameters.AddWithValue("@Memberno", txtcid.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@Parent", txtgrpid.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@Nominee", txtnominee.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@Relationship", txtrelation.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@cp", IIf(cp.Checked, 1, 0))

                    If Not img_photo Is Nothing Then
                        cmdUpdate.Parameters.AddWithValue("@imagephoto", img_photo)
                    End If

                    If Not img_spec Is Nothing Then
                        cmdUpdate.Parameters.AddWithValue("@specimen", img_spec)
                    End If

                    cmdUpdate.Parameters.AddWithValue("@pan", txtpan.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@appno", txtappno.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@poi", idprof.SelectedItem.Text)
                    cmdUpdate.Parameters.AddWithValue("@poi_no", txtiddocno.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@poi_by", DBNull.Value)
                    cmdUpdate.Parameters.AddWithValue("@poi_on", DBNull.Value)
                    cmdUpdate.Parameters.AddWithValue("@poa", addprof.SelectedItem.Text)
                    cmdUpdate.Parameters.AddWithValue("@poa_no", txtadd_doc.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@poa_by", DBNull.Value)
                    cmdUpdate.Parameters.AddWithValue("@poa_on", DBNull.Value)

                    If Not img_poi Is Nothing Then
                        cmdUpdate.Parameters.AddWithValue("@poi_pic", img_poi)
                    End If

                    If Not img_poa Is Nothing Then
                        cmdUpdate.Parameters.AddWithValue("@poa_pic", img_poa)
                    End If

                    Try
                        cmdUpdate.ExecuteNonQuery()
                        ShowSuccessMessage()
                    Catch ex As Exception
                        Response.Write(ex.Message)
                    End Try
                End Using
            End If
        End Using
    End Sub

    Private Sub ShowSuccessMessage()
        Dim stitle = "KYC Updated !"
        Dim msg = "KYC for Member " + txtcid.Text + " is Updated Sucessfully."
        Dim fnc = "showToastOK('" + stitle + "','" + msg + "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showtoastok", fnc, True)
        Clear_tab()
    End Sub

    Protected Sub btn_up_poi_Click(sender As Object, e As EventArgs) Handles btn_up_poi.Click
        If poi_file.HasFile Then
            Dim img_poi As Byte() = poi_file.FileBytes
            Session("imgpoi") = img_poi
            Dim base64String As String = Convert.ToBase64String(img_poi, 0, img_poi.Length)
            imgPOI.ImageUrl = "data:image/png;base64," & base64String
        End If
    End Sub

    Protected Sub btn_poi_can_Click(sender As Object, e As EventArgs) Handles btn_poi_can.Click
        Try
            Dim base64 As String = hfCapturedPOI.Value
            If base64.Contains(",") Then
                base64 = base64.Split(","c)(1)
            End If
            Dim img_poi As Byte() = Convert.FromBase64String(base64)
            Session("imgpoi") = img_poi
            imgPOI.ImageUrl = hfCapturedPOI.Value
        Catch ex As Exception
            ' Handle error
        End Try
    End Sub

    Protected Sub btn_up_poa_Click(sender As Object, e As EventArgs) Handles btn_up_poa.Click
        If poa_file.HasFile Then
            Dim img_poa As Byte() = poa_file.FileBytes
            Session("imgpoa") = img_poa
            Dim base64String As String = Convert.ToBase64String(img_poa, 0, img_poa.Length)
            imgPOA.ImageUrl = "data:image/png;base64," & base64String
        End If
    End Sub

    Protected Sub btn_poa_can_Click(sender As Object, e As EventArgs) Handles btn_poa_can.Click
        Try
            Dim base64 As String = hfCapturedPOA.Value
            If base64.Contains(",") Then
                base64 = base64.Split(","c)(1)
            End If
            Dim img_poa As Byte() = Convert.FromBase64String(base64)
            Session("imgpoa") = img_poa
            imgPOA.ImageUrl = hfCapturedPOA.Value
        Catch ex As Exception
            ' Handle error
        End Try
    End Sub





    'Private Sub btnselect_Click(sender As Object, e As EventArgs) Handles btnselect.Click
    'txtgrpid.Text = Left(listgid1.SelectedItem.Text, 6)
    'txtfirstname.Text = ""
    'listgid1.Items.Clear()
    'txtpan.Focus()

    'End Sub



    Sub Clear_tab()

        txtotp.Text = ""
        otp.Visible = False
        lblotp.Visible = False
        cp.Checked = False
        isactiv.Checked = False
        txtcid.Text = ""
        txtgrpid.Text = ""
        txtnominee.Text = ""
        txtcname.Text = ""
        txtrelation.Text = ""
        txtappno.Text = ""
        Session("cid4") = Nothing
        txtpan.Text = ""
        imgCapture.ImageUrl = Nothing
        imgsign.ImageUrl = Nothing
        idprof.SelectedIndex = 0
        addprof.SelectedIndex = 0
        txtadd.Text = ""
        txtadd_doc.Text = ""
        txtiddocno.Text = ""
        txtfocus(txtcid)

        System.Web.UI.ScriptManager.GetCurrent(Me).SetFocus(Me.txtcid)

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showopen", "SCTOP()", True)

    End Sub

    Private Sub txtname_TextChanged(sender As Object, e As EventArgs) Handles txtname.TextChanged


    End Sub
    Sub fectch_cust(ByVal cid As String)

        imgCapture.ImageUrl = Nothing

        Dim imgx As System.Drawing.Image = Nothing
        Dim imgbytes As Byte() = Nothing
        Dim stream As MemoryStream = Nothing
        Dim isact = 0
        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        Using conn As New SqlConnection(connString)
            conn.Open()
            Dim qry = "select firstname,address,active from member where member.memberno=@cid"
            Using cmdFetch As New SqlCommand(qry, conn)
                cmdFetch.Parameters.AddWithValue("@cid", cid)
                Using drc As SqlDataReader = cmdFetch.ExecuteReader()
                    If drc.HasRows() Then
                        drc.Read()
                        txtcname.Text = IIf(IsDBNull(drc(0)), "", drc(0))
                        txtadd.Text = IIf(IsDBNull(drc(1)), "", drc(1))
                        ' Fix InvalidCastException by checking for DBNull
                        isact = IIf(IsDBNull(drc(2)), 0, drc(2))
                    End If
                End Using
            End Using

            If isact = 0 Then
                isactiv.Checked = False
            Else
                isactiv.Checked = True
            End If

            qry = "select Memberno,Parent,Nominee,Relationship,photo,specimen,pan,poi,poi_no,poi_by,poi_on,poa,poa_no,poa_by,poa_on,appno,cp,poi_pic,poa_pic from kyc where kyc.memberno=@cid"
            Using cmdKyc As New SqlCommand(qry, conn)
                cmdKyc.Parameters.AddWithValue("@cid", cid)
                Using dr As SqlDataReader = cmdKyc.ExecuteReader()
                    If dr.HasRows() Then
                        dr.Read()

                        txtgrpid.Text = IIf(IsDBNull(dr(1)), "", dr(1))
                        txtnominee.Text = IIf(IsDBNull(dr(2)), "", dr(2))
                        txtrelation.Text = IIf(IsDBNull(dr(3)), "", dr(3))
                        txtpan.Text = IIf(IsDBNull(dr(6)), "", dr(6))
                        idprof.SelectedItem.Text = IIf(IsDBNull(dr(7)), "", dr(7))
                        txtiddocno.Text = IIf(IsDBNull(dr(8)), "", dr(8))
                        addprof.SelectedItem.Text = IIf(IsDBNull(dr(11)), "", dr(11))
                        txtadd_doc.Text = IIf(IsDBNull(dr(12)), "", dr(12))
                        txtappno.Text = IIf(IsDBNull(dr(15)), "", dr(15))
                        Dim cpv As Boolean = IIf(IsDBNull(dr(16)), False, dr(16))
                        If cpv = False Then
                            cp.Checked = False
                        Else
                            cp.Checked = True
                        End If

                        If Not IsDBNull(dr(4)) Then
                            imgbytes = CType(dr.GetValue(4), Byte())
                            Dim imagePath As String = String.Format("~/Captures/{0}.png", cid)
                            File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                            imgCapture.ImageUrl = ResolveUrl("~/Captures/" & cid & ".png?" & DateTime.Now.Ticks.ToString())
                        Else
                            imgCapture.ImageUrl = ResolveUrl("~/images/user.png")
                        End If

                        If Not IsDBNull(dr(5)) Then
                            imgbytes = CType(dr.GetValue(5), Byte())
                            Dim imagePath As String = String.Format("~/Captures/{0}_spec.png", cid)
                            File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                            imgsign.ImageUrl = ResolveUrl("~/Captures/" & cid & "_spec.png?" & DateTime.Now.Ticks.ToString())
                        End If

                        If Not IsDBNull(dr(17)) Then
                            imgbytes = CType(dr.GetValue(17), Byte())
                            Dim imagePath As String = String.Format("~/Captures/{0}_poi.png", cid)
                            File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                            imgPOI.ImageUrl = ResolveUrl("~/Captures/" & cid & "_poi.png?" & DateTime.Now.Ticks.ToString())
                        End If

                        If Not IsDBNull(dr(18)) Then
                            imgbytes = CType(dr.GetValue(18), Byte())
                            Dim imagePath As String = String.Format("~/Captures/{0}_poa.png", cid)
                            File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                            imgPOA.ImageUrl = ResolveUrl("~/Captures/" & cid & "_poa.png?" & DateTime.Now.Ticks.ToString())
                        End If
                    Else
                        imgCapture.ImageUrl = ResolveUrl("~/images/user.png")
                    End If
                End Using
            End Using
        End Using
    End Sub


    Private Sub btn_upimg_Click(sender As Object, e As EventArgs) Handles btn_upimg.Click

        imgCapture.ImageUrl = ""


        img_file.PostedFile.SaveAs(Server.MapPath("~/captures/" + Trim(txtcid.Text) + ".png"))

        HttpContext.Current.Session("upimg") = img_file.FileBytes
        imgCapture.Visible = True

        imgCapture.ImageUrl = "~/captures/" + Trim(txtcid.Text) + ".png?" + DateTime.Now.Ticks.ToString()







    End Sub

    'Private Sub btn_up_spec_Click(sender As Object, e As EventArgs) Handles btn_up_spec.Click
    '    imgsignup.PostedFile.SaveAs(Server.MapPath("~/captures/spec.png"))
    '    imgsign.ImageUrl = "~/captures/spec.png"
    '    Session("imgspec") = imgsignup.FileBytes
    '    'pnlspec.Visible = False




    'End Sub


    'Private Sub btn_img_upload_Click(sender As Object, e As EventArgs) Handles btn_img_upload.Click

    'End Sub



    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Clear_tab()

    End Sub

    Private Sub txtcid_TextChanged(sender As Object, e As EventArgs) Handles txtcid.TextChanged

        'get_data()
        If Not txtcid.Text = "" Then
            fectch_cust(txtcid.Text)
        End If


    End Sub

    'Private Sub btn_bulk_Click(sender As Object, e As EventArgs) Handles btn_bulk.Click

    '    Dim mem As String = String.Empty

    '    If con1.State = ConnectionState.Closed Then con1.Open()
    '    cmd.Connection = con1
    '    Dim qry = "SELECT member.MemberNo FROM dbo.member LEFT OUTER JOIN dbo.KYC  ON member.MemberNo = KYC.MemberNo WHERE  KYC.Photo IS NULL"

    '    cmd.CommandText = qry

    '    Try

    '        Dim dr As SqlDataReader
    '        dr = cmd.ExecuteReader()

    '        If dr.HasRows() Then

    '            While dr.Read()

    '                mem = dr(0)

    '                If con.State = ConnectionState.Closed Then con.Open()

    '                Dim filePath As String = Server.MapPath("~\captures\" + mem + ".JPG")

    '                Dim filename As String = Path.GetFileName(filePath)

    '                If IO.File.Exists(filename) Then
    '                    Dim fs As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)

    '                    Dim br As BinaryReader = New BinaryReader(fs)

    '                    Dim bytes As Byte() = br.ReadBytes(Convert.ToInt32(fs.Length))

    '                    br.Close()

    '                    fs.Close()

    '                    Dim strQuery As String = "UPDATE KYC SET PHOTO=@DATA WHERE MemberNo=@MEM"

    '                    Dim cmd As SqlCommand = New SqlCommand(strQuery)
    '                    cmd.Connection = con
    '                    cmd.Parameters.AddWithValue("@MEM", mem)
    '                    cmd.Parameters.Add("@Data", SqlDbType.Binary).Value = bytes

    '                    Try
    '                        cmd.ExecuteNonQuery()

    '                    Catch ex As Exception
    '                        Response.Write(ex.ToString)
    '                    End Try

    '                End If
    '            End While

    '        End If
    '    Catch ex As Exception
    '        Response.Write(ex.ToString)
    '    End Try



    'End Sub

    Private Sub cp_CheckedChanged(sender As Object, e As EventArgs) Handles cp.CheckedChanged

        If cp.Checked = True Then
            otp.Visible = True
            sendotp()
            txtotp.Focus()


        Else
            otp.Visible = False
        End If

    End Sub


    Private Function populate_body() As String

        Dim connString As String = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        Dim branchName As String = ""

        Using conn As New SqlConnection(connString)
            conn.Open()
            Dim qry = "select branch from dbo.branch "
            Using cmdLocal As New SqlCommand(qry, conn)
                Try
                    branchName = cmdLocal.ExecuteScalar()
                Catch e As Exception
                    Response.Write(e.ToString)
                End Try
            End Using
        End Using

        Session("branch") = branchName + " Branch"


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
        Dim reader As StreamReader = New StreamReader(Server.MapPath("partner.html"))
        body = reader.ReadToEnd
        body = body.Replace("{branch}", Session("branch"))
        body = body.Replace("{otp}", strrandom)
        body = body.Replace("{lblacn}", txtcid.Text)
        body = body.Replace("{lblname}", txtcname.Text)
        body = body.Replace("{lbladd}", txtadd.Text)

        Return body

    End Function
    Private Sub sendotp()

        Dim subject As String = "OTP for Correction"
        Dim recepientEmail As String = ConfigurationManager.AppSettings("recepientEmailid1")
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
            'response.write(ex.ToString)
            If (Not ClientScript.IsStartupScriptRegistered("alert")) Then
                Page.ClientScript.RegisterStartupScript _
                (Me.GetType(), "alert", "chkpass();", True)
            End If

        End Try


    End Sub



    Private Sub KYC_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub

    Protected Sub btn_up_sig_Click(sender As Object, e As EventArgs) Handles btn_up_sig.Click
        If sig_file.HasFile Then
            Try
                Dim cid As String = Trim(txtcid.Text)
                Dim fileName As String = cid & "_spec.png"
                Dim filePath As String = Server.MapPath("~/Captures/" & fileName)

                sig_file.PostedFile.SaveAs(filePath)
                Session("imgspec") = sig_file.FileBytes

                imgsign.ImageUrl = ResolveUrl("~/Captures/" & fileName & "?" & DateTime.Now.Ticks.ToString())
            Catch ex As Exception
                ' Handle upload error
            End Try
        End If
    End Sub

    Protected Sub btn_sig_can_Click(sender As Object, e As EventArgs) Handles btn_sig_can.Click
        If Not String.IsNullOrEmpty(hfCapturedSignature.Value) Then
            Try
                Dim base64String As String = hfCapturedSignature.Value.Split(","c)(1)
                Dim imageBytes() As Byte = Convert.FromBase64String(base64String)
                Session("imgspec") = imageBytes

                Dim cid As String = Trim(txtcid.Text)
                Dim fileName As String = cid & "_spec.png"
                Dim filePath As String = Server.MapPath("~/Captures/" & fileName)
                File.WriteAllBytes(filePath, imageBytes)

                imgsign.ImageUrl = ResolveUrl("~/Captures/" & fileName & "?" & DateTime.Now.Ticks.ToString())
            Catch ex As Exception
                ' Handle conversion error
            End Try
        End If
    End Sub

    Private Sub btn_img_can_Click(sender As Object, e As EventArgs) Handles btn_img_can.Click
        If Not String.IsNullOrEmpty(hfCapturedImage.Value) Then
            Try
                Dim base64String As String = hfCapturedImage.Value.Split(","c)(1)
                Dim imageBytes() As Byte = Convert.FromBase64String(base64String)
                Session("upimg") = imageBytes

                Dim fileName As String = "ImgCaptured.jpg"
                Dim filePath As String = Server.MapPath("~/Captures/" & fileName)
                File.WriteAllBytes(filePath, imageBytes)

                imgCapture.Visible = True
                imgCapture.ImageUrl = "~/Captures/" & fileName & "?" & DateTime.Now.Ticks.ToString()
            Catch ex As Exception
                ' Handle conversion error
            End Try
        End If
    End Sub
End Class