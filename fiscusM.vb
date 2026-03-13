Imports System.Globalization
Imports System.Threading
Imports System.Web.Services
Imports System
Imports System.Net
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net.Http
Imports System.Threading.Tasks

Module fiscusM

    Public issrvc As Boolean = False

    Public smsbal As String = "0"

    Public home As String = "Karavilai"

    Public summary_on As Boolean = False
    Public prnt4sc As Boolean = False


    Public Session_user As String
    Public session_user_role As String
    Public sms_balance As String

    Public query As String

    Public Enum TemplateID As Long
        Debit = 1707173183202982850
        Credit = 1707173183538705904
        JewelDue = 1707173183556924507
        DepositMaturity = 1707173183585151184
        JewelRenewRelease = 1707173183607889797
        RDPaymentLate = 1707173184377436756
    End Enum

    Public Sub InitializeCulture()

        Dim ci As CultureInfo = New CultureInfo("en-IN")
        ci.NumberFormat.CurrencySymbol = "₹"

        Thread.CurrentThread.CurrentCulture = ci

    End Sub

    Public Function sms_bal()
        ' Check if balance is already cached for 6 hours
        If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Cache("SMS_Balance") IsNot Nothing Then
            Return HttpContext.Current.Cache("SMS_Balance").ToString()
        End If

        Dim bal As String = "0"

        Dim smsAPIKey As String = "j7c0tIJ4AAcSBr8v"
        Dim smsURL As String = "https://sms.textspeed.in/vb/http-credit.php?"
        Dim createdURL As String = smsURL + "apikey=" + smsAPIKey + "&route_id=1&format=json"

        Try
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
            Dim myReq As HttpWebRequest = HttpWebRequest.Create(createdURL)
            Dim myResp As HttpWebResponse = myReq.GetResponse()
            Dim respStreamReader As System.IO.StreamReader = New System.IO.StreamReader(myResp.GetResponseStream())
            Dim responseString As String = respStreamReader.ReadToEnd()
            
            Dim jresult As JObject = JObject.Parse(responseString)

            If jresult("status").ToString().ToLower() = "success" Then
                bal = jresult("balance").ToString()
                
                ' Cache the balance for 6 hours
                If HttpContext.Current IsNot Nothing Then
                    HttpContext.Current.Cache.Insert("SMS_Balance", bal, Nothing, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration)
                End If
            End If

            respStreamReader.Close()
            myResp.Close()
        Catch ex As Exception
            ' Error handling
        End Try

        Return bal
    End Function
    
    Public Sub Send_sms(ByVal mobileno As String, ByVal msgdata As String, ByVal templateType As TemplateID)
        If Not Trim(mobileno) = "" Then
            Dim smsAPIKey As String = "j7c0tIJ4AAcSBr8v"
            Dim smsTemplateId = CType(templateType, Long).ToString()

            Dim smsURL As String = "https://sms.textspeed.in/vb/apikey.php?"
            Dim smsRecipients As String = HttpUtility.UrlEncode("91" + Trim(mobileno)) '; //who     will get the message
            Dim smsMessageData As String = Trim(msgdata) '; //body     of message
            Dim smsSender As String = "KBFLTD"
            Dim createdURL As String = smsURL + "apikey=" + smsAPIKey + "&senderid=" + smsSender
            createdURL += "&templateid=" + smsTemplateId
            createdURL += "&number=" + smsRecipients
            createdURL += "&message=" + smsMessageData + " - Karavilai Nidhi Ltd."

            Dim result As String = GetRequest(createdURL).Result
            Console.WriteLine(result)
        End If
    End Sub

    Async Function GetRequest(ByVal url As String) As Task(Of String)
        Using client As New HttpClient()
            Try
                ' Send the GET request
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                Dim response As HttpResponseMessage = Await client.GetAsync(url)
                response.EnsureSuccessStatusCode()

                ' Read the response content
                Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                Return responseBody
            Catch ex As Exception
                ' Handle errors
                Console.WriteLine($"Error: {ex.Message}")
                Return Nothing
            End Try
        End Using
    End Function

    Public Function poke(ByVal usr As String)
        Dim clr As Boolean = False

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "update seslog set sessioncld=1 where sesusr='" + usr + "'"
                cmd.CommandText = query

                Try
                    cmd.ExecuteNonQuery()
                    clr = True

                Catch ex As Exception

                End Try

            End Using
        End Using




        Return clr
    End Function


    Function get_acnprefix(ByVal dep As String)

        Dim acnprefix As String = "00"
        Dim oResult As String = String.Empty





        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "select acnprefix from products where shortname='" + dep + "'"
                cmd.CommandText = query

                oResult = cmd.ExecuteScalar()

                If oResult IsNot Nothing Then
                    acnprefix = oResult

                End If




            End Using
        End Using

        Return acnprefix
    End Function



    Public Function Get_Rnd_KMK(ByVal typ As Integer, ByVal amt As Double)
        Dim noa As Integer = 0

nxt:
        Dim acc As Integer = 0
        Dim acn As String = String.Empty

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "SELECT TOP 1 acno FROM master where cid like 'KNL%' and product='KMK' ORDER BY NEWID()"

                cmd.CommandText = query

                acn = cmd.ExecuteScalar




            End Using
        End Using


        acc = CHK_RND_CHK(acn, typ, amt)

        If acc <> 0 Then
            noa += 1
            If noa <= 30 Then
                GoTo nxt
            Else
                GoTo nxt1
            End If


        End If

nxt1:

        Return acn

    End Function

    Public Function CHK_RND_CHK(ByVal acn As String, ByVal typ As Integer, ByVal amt As Double)

        Dim occ = 0

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "select count(actrans.acno) from actrans where acno='" + acn + "' and  convert(varchar(20), actrans.date, 102) = convert(varchar(20), getdate(), 102)"
                cmd.CommandText = query
                occ = cmd.ExecuteScalar

            End Using
        End Using


        If occ = 0 Then

            If typ = 0 Then
                Dim bal As Double = 0

                Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
                    con.Open()
                    Using cmd = New SqlCommand
                        cmd.Connection = con
                        If session_user_role = "Audit" Then
                            query = "SELECT COALESCE(SUM(actransc.Crd) - SUM(actransc.Drd),0) AS bal FROM dbo.actransc WHERE actransc.acno = @acno"
                        Else
                            query = "SELECT COALESCE(SUM(actrans.Crd) - SUM(actrans.Drd),0) AS bal FROM dbo.actrans WHERE actrans.acno = @acno"
                        End If

                        cmd.CommandText = query
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@acno", acn)
                        Try

                            bal = cmd.ExecuteScalar


                        Catch ex As Exception
                            HttpContext.Current.Response.Write(ex.ToString)
                        End Try

                    End Using


                End Using



                If (bal - 200) >= amt Then
                    occ = 0
                Else
                    occ = 1
                End If



            End If

        End If



        Return occ

    End Function




    Public Function Get_Rnd_SB(ByVal typ As Integer, ByVal amt As Double)

        Dim noa As Integer = 0

nxt:
        Dim acc As Integer = 0
        Dim acn As String = String.Empty

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "SELECT TOP 1 acno FROM master where cid like 'KNL%' and product='SB' ORDER BY NEWID()"

                cmd.CommandText = query

                acn = cmd.ExecuteScalar




            End Using
        End Using


        acc = CHK_RND_CHK(acn, typ, amt)

        If acc <> 0 Then
            noa += 1
            If noa <= 30 Then
                GoTo nxt
            Else
                GoTo nxt1
            End If


        End If

nxt1:

        Return acn

    End Function

    Public Function Get_Rnd_Member()
        Dim cid As String = String.Empty
        Dim count As Integer = 0

nxt:
        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                query = "SELECT TOP 1 member.MemberNo FROM member where MemberNo like 'KNL%' ORDER BY NEWID()"

                cmd.CommandText = query
                cid = cmd.ExecuteScalar



            End Using
        End Using


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "select count(masterc.cid) from masterc where cid='" + cid + "' and  convert(varchar(20), masterc.date, 102) = convert(varchar(20), getdate(), 102)"
                cmd.CommandText = query
                count = cmd.ExecuteScalar



            End Using
        End Using


        If count <> 0 Then GoTo nxt



        Return cid


    End Function

    Public Sub get_soa_details(ByVal acno As String)

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                If session_user_role = "Audit" Then
                    query = "select date,acno,cid,product,amount,cint,dint,cld,prd,mdate,mamt,sch,pb,pbi,renewacn,alert from masterc where acno=@acno"
                Else
                    query = "select date,acno,cid,product,amount,cint,dint,cld,prd,mdate,mamt,sch,pb,pbi,renewacn,alert,fc,agent from master where acno=@acno"
                End If

                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@acno", acno)
                Try

                    Using dr = cmd.ExecuteReader

                        If dr.HasRows Then
                            dr.Read()

                            HttpContext.Current.Session("date") = IIf(IsDBNull(dr(0)), "", dr(0))
                            HttpContext.Current.Session("acno") = IIf(IsDBNull(dr(1)), "", dr(1))
                            HttpContext.Current.Session("cid") = IIf(IsDBNull(dr(2)), "", dr(2))
                            HttpContext.Current.Session("product") = IIf(IsDBNull(dr(3)), "", dr(3))
                            HttpContext.Current.Session("amount") = IIf(IsDBNull(dr(4)), "", dr(4))
                            HttpContext.Current.Session("cint") = IIf(IsDBNull(dr(5)), "", dr(5))
                            HttpContext.Current.Session("dint") = IIf(IsDBNull(dr(6)), "", dr(6))
                            HttpContext.Current.Session("cld") = IIf(IsDBNull(dr(7)), "", dr(7))
                            HttpContext.Current.Session("prd") = IIf(IsDBNull(dr(8)), "", dr(8))
                            HttpContext.Current.Session("mdate") = IIf(IsDBNull(dr(9)), DateTime.MinValue, dr(9))
                            HttpContext.Current.Session("mamt") = IIf(IsDBNull(dr(10)), 0, dr(10))
                            HttpContext.Current.Session("sch") = IIf(IsDBNull(dr(11)), "", dr(11))
                            HttpContext.Current.Session("pb") = IIf(IsDBNull(dr(12)), 0, dr(12))
                            HttpContext.Current.Session("pbi") = IIf(IsDBNull(dr(13)), "", dr(13))
                            HttpContext.Current.Session("renewacn") = IIf(IsDBNull(dr(14)), "", dr(14))
                            HttpContext.Current.Session("alert") = IIf(IsDBNull(dr(15)), "", dr(15))
                            If Not session_user_role = "Audit" Then
                                HttpContext.Current.Session("fc") = IIf(IsDBNull(dr(16)), 0, dr(16))
                                HttpContext.Current.Session("agent") = IIf(IsDBNull(dr(17)), "", dr(17))
                            End If

                        End If
                    End Using

                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

            End Using
        End Using



    End Sub


    Public Sub get_profile(ByVal cid As String)

        Dim imgx As Image = Nothing
        Dim imgbytes As Byte() = Nothing
        Dim stream As MemoryStream = Nothing


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "select memberno,firstname,lastname,dob,gender,address,pincode,phone,mobile from member where memberno=@cid"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@cid", cid)

                Using dr = cmd.ExecuteReader

                    If dr.HasRows Then

                        dr.Read()

                        HttpContext.Current.Session("memberno") = IIf(IsDBNull(dr(0)), "", dr(0))
                        HttpContext.Current.Session("firstname") = IIf(IsDBNull(dr(1)), "", dr(1))
                        HttpContext.Current.Session("lastname") = IIf(IsDBNull(dr(2)), "", dr(2))
                        HttpContext.Current.Session("dob") = IIf(IsDBNull(dr(3)), "", dr(3))
                        HttpContext.Current.Session("gender") = IIf(IsDBNull(dr(4)), "", dr(4))
                        HttpContext.Current.Session("address") = IIf(IsDBNull(dr(5)), "", dr(5))
                        HttpContext.Current.Session("pincode") = IIf(IsDBNull(dr(6)), "", dr(6))
                        HttpContext.Current.Session("phone") = IIf(IsDBNull(dr(7)), "", dr(7))
                        HttpContext.Current.Session("mobile") = IIf(IsDBNull(dr(8)), "", dr(8))




                    End If



                End Using

            End Using
        End Using


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "select parent,nominee,relationship,pan,appno,cp,photo from kyc where memberno=@cid"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@cid", cid)

                Using dr = cmd.ExecuteReader

                    If dr.HasRows Then

                        dr.Read()
                        HttpContext.Current.Session("parent") = IIf(IsDBNull(dr(0)), "", dr(0))
                        HttpContext.Current.Session("nominee") = IIf(IsDBNull(dr(1)), "", dr(1))
                        HttpContext.Current.Session("relation") = IIf(IsDBNull(dr(2)), "", dr(2))
                        HttpContext.Current.Session("pan") = IIf(IsDBNull(dr(3)), "", dr(3))
                        HttpContext.Current.Session("appno") = IIf(IsDBNull(dr(4)), "", dr(4))
                        HttpContext.Current.Session("cp") = IIf(IsDBNull(dr(5)), 0, dr(5))

                        If Not IsDBNull(dr(6)) Then

                            imgbytes = CType(dr.GetValue(6), Byte())
                            If Not imgbytes.Length = 0 Then
                                stream = New MemoryStream(imgbytes, 0, imgbytes.Length)
                                imgx = Image.FromStream(stream)
                                '  imgx.Save("../captures/webcam.png", System.Drawing.Imaging.ImageFormat.Png)

                                Dim imagePath As String = String.Format("~/Captures/{0}.png", cid)
                                File.WriteAllBytes(HttpContext.Current.Server.MapPath(imagePath), imgbytes)
                                'Session("CapturedImage") = ResolveUrl(imagePath)
                                HttpContext.Current.Session("photo") = imagePath
                            Else
                                HttpContext.Current.Session("photo") = "~/Images/user.png"
                            End If
                        Else
                            HttpContext.Current.Session("photo") = "~/Images/user.png"

                        End If


                    End If
                End Using

            End Using

        End Using

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                query = "SELECT share.cid,SUM(share.allocation) AS nosh ,SUM(share.shrval) AS shval FROM dbo.share WHERE share.cid = @cid GROUP BY share.cid"
                cmd.CommandText = query
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@cid", cid)

                Using dr = cmd.ExecuteReader

                    If dr.HasRows Then
                        dr.Read()


                        HttpContext.Current.Session("share") = IIf(IsDBNull(dr(1)), 0, dr(1))
                        HttpContext.Current.Session("shareval") = IIf(IsDBNull(dr(2)), 0, dr(2))
                    Else

                        HttpContext.Current.Session("share") = 0
                        HttpContext.Current.Session("shareval") = 0
                    End If

                End Using
            End Using
        End Using




    End Sub


    Public Sub Log_out(ByVal log As String, ByVal usr As String)
        Dim con As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString
        con.Open()

        query = "update seslog set logout=@dt,sessioncld=@scld  WHERE CONVERT(VARCHAR(20), seslog.login, 120) = @sdt and sesusr=@usr"
        cmd.Connection = con
        cmd.CommandText = query
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@dt", DateAndTime.Now)
        cmd.Parameters.AddWithValue("@scld", 1)
        cmd.Parameters.AddWithValue("@sdt", Convert.ToDateTime(log))
        cmd.Parameters.AddWithValue("@usr", usr)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


    Public Function AmountInWords(ByVal nAmount As String, Optional ByVal wAmount _
                 As String = vbNullString, Optional ByVal nSet As Object = Nothing) As String
        'Let's make sure entered value is numeric
        If Not IsNumeric(nAmount) Then Return "Please enter numeric values only."

        Dim tempDecValue As String = String.Empty : If InStr(nAmount, ".") Then _
            tempDecValue = nAmount.Substring(nAmount.IndexOf("."))
        nAmount = Replace(nAmount, tempDecValue, String.Empty)

        Try
            Dim intAmount As Long = nAmount
            If intAmount > 0 Then
                nSet = IIf((intAmount.ToString.Trim.Length / 3) _
                    > (CLng(intAmount.ToString.Trim.Length / 3)), _
                  CLng(intAmount.ToString.Trim.Length / 3) + 1, _
                    CLng(intAmount.ToString.Trim.Length / 3))
                Dim eAmount As Long = Microsoft.VisualBasic.Left(intAmount.ToString.Trim, _
                  (intAmount.ToString.Trim.Length - ((nSet - 1) * 3)))
                Dim multiplier As Long = 10 ^ (((nSet - 1) * 3))

                Dim Ones() As String = _
                {"", "One", "Two", "Three", _
                  "Four", "Five", _
                  "Six", "Seven", "Eight", "Nine"}
                Dim Teens() As String = {"", _
                "Eleven", "Twelve", "Thirteen", _
                  "Fourteen", "Fifteen", _
                  "Sixteen", "Seventeen", "Eighteen", "Nineteen"}
                Dim Tens() As String = {"", "Ten", _
                "Twenty", "Thirty", _
                  "Forty", "Fifty", "Sixty", _
                  "Seventy", "Eighty", "Ninety"}
                Dim HMBT() As String = {"", "", _
                "Thousand", "Lakh", _
                  "Billion", "Trillion", _
                  "Quadrillion", "Quintillion"}

                intAmount = eAmount

                Dim nHundred As Integer = intAmount \ 100 : intAmount = intAmount Mod 100
                Dim nTen As Integer = intAmount \ 10 : intAmount = intAmount Mod 10
                Dim nOne As Integer = intAmount \ 1

                If nHundred > 0 Then wAmount = wAmount & _
                Ones(nHundred) & " Hundred " 'This is for hundreds                
                If nTen > 0 Then 'This is for tens and teens
                    If nTen = 1 And nOne > 0 Then 'This is for teens 
                        wAmount = wAmount & Teens(nOne) & " "
                    Else 'This is for tens, 10 to 90
                        wAmount = wAmount & Tens(nTen) & IIf(nOne > 0, "-", " ")
                        If nOne > 0 Then wAmount = wAmount & Ones(nOne) & " "
                    End If
                Else 'This is for ones, 1 to 9
                    If nOne > 0 Then wAmount = wAmount & Ones(nOne) & " "
                End If
                wAmount = wAmount & HMBT(nSet) & " "
                wAmount = AmountInWords(CStr(CLng(nAmount) - _
                  (eAmount * multiplier)).Trim & tempDecValue, wAmount, nSet - 1)
            Else
                If Val(nAmount) = 0 Then nAmount = nAmount & _
                tempDecValue : tempDecValue = String.Empty
                If (Math.Round(Val(nAmount), 2) * 100) > 0 Then wAmount = _
                  Trim(AmountInWords(CStr(Math.Round(Val(nAmount), 2) * 100), _
                  wAmount.Trim & " Paise And ", 1)) & " Paise"
            End If
        Catch ex As Exception
            MsgBox("Error Encountered: " & ex.Message, _
              "Convert Numbers To Words ")
        End Try

        'Trap null values
        If IsNothing(wAmount) = True Then wAmount = String.Empty Else wAmount = _
          IIf(InStr(wAmount.Trim.ToLower, " "), _
          wAmount.Trim, wAmount.Trim & " ")

        'Display the result
        Return wAmount
    End Function


    Public Function get_ones(ByVal ic As Double)

        Dim X As String = String.Empty



        Select Case ic
            Case 1
                X = "One"
            Case 2
                X = "Two"
            Case 3
                X = "Three"
            Case 4
                X = "Four"
            Case 5
                X = "Five"
            Case 6
                X = "Six"
            Case 7
                X = "Seven"
            Case 8
                X = "Eight"
            Case 9
                X = "Nine"
            Case 10
                X = "Ten"
            Case 11
                X = "Eleven"
            Case 12
                X = "Twelve"
            Case 13
                X = "Thirteen"
            Case 14
                X = "Fourteen"
            Case 15
                X = "Fifteen"
            Case 16
                X = "Sixteen"
            Case 17
                X = "Seventeen"
            Case 18
                X = "Eighteen"
            Case 19
                X = "Ninteen"
        End Select
        get_ones = X
    End Function
    Public Function get_tens(ByVal ic As Double)

        Dim X As String = String.Empty


        Select Case ic
            Case Is <= 29
                X = "Twenty"
            Case Is <= 39
                X = "Thirty"
            Case Is <= 49
                X = "Fourty"
            Case Is <= 59
                X = "Fifty"
            Case Is <= 69
                X = "Sixty"
            Case Is <= 79
                X = "Seventy"
            Case Is <= 89
                X = "Eighty"
            Case Is <= 99
                X = "Ninty"
        End Select

        get_tens = X
    End Function

    Public Function get_wrds(ByVal fig As Double)
        On Error Resume Next
        Dim w As String = ""
        Dim X As String = Format(fig, "0")
        Dim fl As String = Len(Trim(X))
        Dim X1 As String = Right(X, 2)
        Dim X2 As String = String.Empty
        Dim x3 As String = String.Empty
        Dim x4 As String = String.Empty
        Dim x5 As String = String.Empty
        Dim wrdo As String = String.Empty
        Dim wrdt As String = String.Empty
        Dim wrdh As String = String.Empty
        Dim wrdtho As String = String.Empty
        Dim wrdl As String = String.Empty
        Dim wrdc As String = String.Empty


        Dim Empty As String = String.Empty


        If X1 <= 19 Then
            wrdo = get_ones(X1)
        ElseIf X1 > 19 Then
            wrdo = get_ones(Right(X1, 1))
            wrdt = get_tens(X1)
        End If
        w = w + wrdt + wrdo
        If fl < 3 Then GoTo nxt
        X2 = Right(X, 3)

        wrdh = get_ones(Left(X2, 1))
        If Not wrdh = Empty Then
            If w = Empty And wrdh = "One" Then
                w = wrdh + " Hundred  " + w
            Else
                If w = "" Then
                    w = wrdh + " Hundred " + w
                Else
                    w = wrdh + " Hundred  and " + w
                End If
            End If
        Else
            w = wrdh + w
        End If


        If fl < 4 Then GoTo nxt
        If fl <= 4 Then
            x3 = Left(Right(X, 5), 1)
        Else
            x3 = Left(Right(X, 5), 2)
        End If
        If x3 <= 19 Then
            wrdtho = get_ones(Left(x3, 2))
            If wrdtho = "" Then
                wrdtho = Empty
            Else
                wrdtho = wrdtho + " Thousand "
            End If

            If Not wrdtho = Empty Then
                w = wrdtho + w
            End If
        ElseIf x3 > 19 Then
            wrdtho = get_ones(Right(x3, 1))
            wrdtho = get_tens(Left(x3, 2)) + wrdtho + " Thousand "
            If Not wrdtho = Empty Then
                w = wrdtho + w
            End If

        End If


        If fl < 6 Then GoTo nxt
        If fl <= 6 Then
            x4 = Left(Right(X, 7), 1)
        Else
            x4 = Left(Right(X, 7), 2)
        End If
        If x4 <= 19 Then
            wrdl = get_ones(Left(x4, 2))
            If wrdl <> "" Then wrdl = wrdl + " Lakh "
        ElseIf x4 > 19 Then
            wrdl = get_ones(Right(x4, 1))
            wrdl = get_tens(Left(x4, 2)) + wrdl + " Lakh "
        End If
        w = wrdl + w

        If fl < 8 Then GoTo nxt
        If fl <= 8 Then
            x5 = Left(Right(X, 9), 1)
        Else
            x5 = Left(Right(X, 9), 2)
        End If
        If x5 <= 19 Then
            wrdc = get_ones(Left(x5, 2))
            If wrdc <> "" Then wrdc = wrdc + " Crore "
        ElseIf x5 > 19 Then
            wrdc = get_ones(Right(x5, 1))
            wrdc = get_tens(Left(x5, 2)) + wrdc + " Crore "
        End If
        w = wrdc + w




nxt:
        w = "Rupees " + w + " Only"
        get_wrds = w

    End Function


    Public Function get_wrds4share(ByVal fig As Double)
        On Error Resume Next
        Dim w As String = ""
        Dim X As String = Format(fig, "0")
        Dim fl As String = Len(Trim(X))
        Dim X1 As String = Right(X, 2)
        Dim X2 As String = String.Empty
        Dim x3 As String = String.Empty
        Dim x4 As String = String.Empty
        Dim x5 As String = String.Empty
        Dim wrdo As String = String.Empty
        Dim wrdt As String = String.Empty
        Dim wrdh As String = String.Empty
        Dim wrdtho As String = String.Empty
        Dim wrdl As String = String.Empty
        Dim wrdc As String = String.Empty


        Dim Empty As String = String.Empty


        If X1 <= 19 Then
            wrdo = get_ones(X1)
        ElseIf X1 > 19 Then
            wrdo = get_ones(Right(X1, 1))
            wrdt = get_tens(X1)
        End If
        w = w + wrdt + wrdo
        If fl < 3 Then GoTo nxt
        X2 = Right(X, 3)

        wrdh = get_ones(Left(X2, 1))
        If Not wrdh = Empty Then
            If w = Empty And wrdh = "One" Then
                w = wrdh + " Hundred  " + w
            Else
                If w = "" Then
                    w = wrdh + " Hundred " + w
                Else
                    w = wrdh + " Hundred  and " + w
                End If
            End If
        Else
            w = wrdh + w
        End If


        If fl < 4 Then GoTo nxt
        If fl <= 4 Then
            x3 = Left(Right(X, 5), 1)
        Else
            x3 = Left(Right(X, 5), 2)
        End If
        If x3 <= 19 Then
            wrdtho = get_ones(Left(x3, 2))
            If wrdtho = "" Then
                wrdtho = Empty
            Else
                wrdtho = wrdtho + " Thousand "
            End If

            If Not wrdtho = Empty Then
                w = wrdtho + w
            End If
        ElseIf x3 > 19 Then
            wrdtho = get_ones(Right(x3, 1))
            wrdtho = get_tens(Left(x3, 2)) + wrdtho + " Thousand "
            If Not wrdtho = Empty Then
                w = wrdtho + w
            End If

        End If


        If fl < 6 Then GoTo nxt
        If fl <= 6 Then
            x4 = Left(Right(X, 7), 1)
        Else
            x4 = Left(Right(X, 7), 2)
        End If
        If x4 <= 19 Then
            wrdl = get_ones(Left(x4, 2))
            If wrdl <> "" Then wrdl = wrdl + " Lakh "
        ElseIf x4 > 19 Then
            wrdl = get_ones(Right(x4, 1))
            wrdl = get_tens(Left(x4, 2)) + wrdl + " Lakh "
        End If
        w = wrdl + w

        If fl < 8 Then GoTo nxt
        If fl <= 8 Then
            x5 = Left(Right(X, 9), 1)
        Else
            x5 = Left(Right(X, 9), 2)
        End If
        If x5 <= 19 Then
            wrdc = get_ones(Left(x5, 2))
            If wrdc <> "" Then wrdc = wrdc + " Crore "
        ElseIf x5 > 19 Then
            wrdc = get_ones(Right(x5, 1))
            wrdc = get_tens(Left(x5, 2)) + wrdc + " Crore "
        End If
        w = wrdc + w




nxt:
        'w = "Rupees " + w + " Only"
        get_wrds4share = w

    End Function

End Module
