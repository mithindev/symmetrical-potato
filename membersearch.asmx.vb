Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class membersearch
    Inherits System.Web.Services.WebService
    Public Class UserDetails

        Public Memberno As String
        Public Firstname As String
        Public Lastname As String
        Public Address As String
        Public ImageURL As String
        Public Amount As String
    End Class
    Public Class UserDetails1
        Public Memberno As String
        Public Firstname As String
        Public Lastname As String
        Public Address As String
        Public ImageURL As String
        Public Product As String
        Public Amount As String
    End Class
    Public Class Jewel
        Public Jewel As String
    End Class
    Dim con As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd1 As New SqlClient.SqlCommand

    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetSbAc(ByVal input As String) As List(Of UserDetails1)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con


                Dim mobilesearch As String = ""
                If input.Length > 6 Then mobilesearch = " OR member.mobile Like @search + '%'"

                If session_user_role = "Audit" Then
                    query = "SELECT masterc.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,masterc.product,products.prdtype FROM dbo.masterc"
                    query += " INNER Join dbo.member ON masterc.cid = member.MemberNo INNER JOIN dbo.products ON masterc.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') AND masterc.product = @typ AND masterc.cld = 0 ORDER by FirstName"

                Else
                    query = "SELECT master.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,master.product,products.prdtype FROM dbo.master"
                    query += " INNER Join dbo.member ON master.cid = member.MemberNo INNER JOIN dbo.products ON master.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') AND (master.product = @typ or master.product='KMK') AND master.cld = 0 ORDER by FirstName"

                End If

                cmd.CommandText = query

                cmd.Parameters.AddWithValue("@search", input)
                cmd.Parameters.AddWithValue("@typ", "SB")

                Dim result As New List(Of UserDetails1)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New UserDetails1
                        r.Memberno = IIf(IsDBNull(dr(0)), "", dr(0))
                        r.Firstname = IIf(IsDBNull(dr(2)), "", Trim(dr(2)))
                        r.Lastname = IIf(IsDBNull(dr(3)), "", Trim(dr(3)))
                        r.Address = IIf(IsDBNull(dr(4)), "", dr(4))
                        r.ImageURL = get_userimg(dr(1))
                        r.Product = IIf(IsDBNull(dr(5)), "", dr(5))


                        result.Add(r)


                    End While
                    Return result
                End Using


            End Using
            con.Close()

        End Using

    End Function


    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetNewLoan(ByVal input As String) As List(Of UserDetails1)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                Dim mobilesearch As String = ""
                If input.Length > 6 Then mobilesearch = " OR member.mobile Like @search + '%'"

                If session_user_role = "Audit" Then
                    query = "SELECT masterc.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,masterc.product,products.prdtype,masterc.amount FROM dbo.masterc"
                    query += " INNER Join dbo.member ON masterc.cid = member.MemberNo INNER JOIN dbo.products ON masterc.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') AND products.prdtype = @typ AND masterc.cld = 0 AND masterc.amount = 0 ORDER by FirstName,product"
                Else
                    query = "SELECT master.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,master.product,products.prdtype,master.amount FROM dbo.master"
                    query += " INNER Join dbo.member ON master.cid = member.MemberNo INNER JOIN dbo.products ON master.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') AND products.prdtype = @typ AND master.cld = 0 AND master.amount = 0 ORDER by FirstName,product"
                End If
                cmd.CommandText = query

                cmd.Parameters.AddWithValue("@search", input)
                cmd.Parameters.AddWithValue("@typ", "LOAN")

                Dim result As New List(Of UserDetails1)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New UserDetails1
                        r.Memberno = IIf(IsDBNull(dr(0)), "", dr(0))
                        r.Firstname = IIf(IsDBNull(dr(2)), "", Trim(dr(2)))
                        r.Lastname = IIf(IsDBNull(dr(3)), "", Trim(dr(3)))
                        r.Address = IIf(IsDBNull(dr(4)), "", dr(4))
                        r.ImageURL = get_userimg(dr(1))
                        r.Product = IIf(IsDBNull(dr(5)), "", dr(5))
                        r.Amount = IIf(IsDBNull(dr(7)), 0, dr(7))

                        result.Add(r)


                    End While
                    Return result
                End Using


            End Using

            con.Close()

        End Using

    End Function


    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetSOA(ByVal input As String) As List(Of UserDetails1)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                Dim mobilesearch As String = ""
                If input.Length > 6 Then mobilesearch = " OR member.mobile Like @search + '%'"

                If session_user_role = "Audit" Then
                    query = "SELECT masterc.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,masterc.product,products.prdtype,masterc.amount,masterc.date FROM dbo.masterc"
                    query += " INNER Join dbo.member ON masterc.cid = member.MemberNo INNER JOIN dbo.products ON masterc.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') and masterc.acno like '7%'  ORDER by FirstName,product,masterc.date"
                Else
                    query = "SELECT master.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,master.product,products.prdtype,master.amount,master.date FROM dbo.master"
                    query += " INNER Join dbo.member ON master.cid = member.MemberNo INNER JOIN dbo.products ON master.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%')  and master.acno like '7%' ORDER by FirstName,product,master.date"
                End If
                cmd.CommandText = query

                cmd.Parameters.AddWithValue("@search", input)


                Dim result As New List(Of UserDetails1)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New UserDetails1
                        r.Memberno = IIf(IsDBNull(dr(0)), "", dr(0))
                        r.Firstname = IIf(IsDBNull(dr(2)), "", Trim(dr(2)))
                        r.Lastname = IIf(IsDBNull(dr(3)), "", Trim(dr(3)))
                        r.Address = IIf(IsDBNull(dr(4)), "", dr(4))
                        r.ImageURL = get_userimgnavbar(dr(1))
                        r.Product = IIf(IsDBNull(dr(5)), "", dr(5))
                        r.Amount = dr(5).ToString + " - " + dr(0).ToString + " - " + CDate(dr(8)).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + FormatCurrency(dr(7))


                        result.Add(r)


                    End While
                    Return result
                End Using


            End Using
            con.Close()

        End Using

    End Function



    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetLoan(ByVal input As String) As List(Of UserDetails1)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                Dim mobilesearch As String = ""
                If input.Length > 6 Then mobilesearch = " OR member.mobile Like @search + '%'"

                If session_user_role = "Audit" Then
                    query = "SELECT masterc.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,masterc.product,products.prdtype,masterc.amount,master.cdate FROM dbo.masterc"
                    query += " INNER Join dbo.member ON masterc.cid = member.MemberNo INNER JOIN dbo.products ON masterc.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') AND products.prdtype = @typ AND masterc.cld = 0 ORDER by FirstName,product,masterc.date"

                Else
                    query = "SELECT master.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,master.product,products.prdtype,master.amount,master.date FROM dbo.master"
                    query += " INNER Join dbo.member ON master.cid = member.MemberNo INNER JOIN dbo.products ON master.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') AND products.prdtype = @typ AND master.cld = 0 ORDER by FirstName,product,master.date"

                End If

                cmd.CommandText = query

                cmd.Parameters.AddWithValue("@search", input)
                cmd.Parameters.AddWithValue("@typ", "LOAN")

                Dim result As New List(Of UserDetails1)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New UserDetails1
                        r.Memberno = IIf(IsDBNull(dr(0)), "", dr(0))
                        r.Firstname = IIf(IsDBNull(dr(2)), "", Trim(dr(2)))
                        r.Lastname = IIf(IsDBNull(dr(3)), "", Trim(dr(3)))
                        r.Address = IIf(IsDBNull(dr(4)), "", dr(4))
                        r.ImageURL = get_userimg(dr(1))
                        r.Product = IIf(IsDBNull(dr(5)), "", dr(5))
                        r.Amount = dr(5).ToString + " - " + Trim(dr(0).ToString) + " - " + CDate(dr(8)).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + FormatCurrency(dr(7))


                        result.Add(r)


                    End While
                    Return result
                End Using


            End Using
            con.Close()

        End Using

    End Function


    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetDeposit(ByVal input As String) As List(Of UserDetails1)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                Dim mobilesearch As String = ""
                If input.Length > 6 Then mobilesearch = " OR member.mobile Like @search + '%'"

                If session_user_role = "Audit" Then
                    query = "SELECT masterc.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,masterc.product,products.prdtype,masterc.amount,masterc.date FROM dbo.masterc"
                    query += " INNER Join dbo.member ON masterc.cid = member.MemberNo INNER JOIN dbo.products ON masterc.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') AND products.prdtype = @typ AND masterc.cld = 0 ORDER by FirstName,product"

                Else

                    query = "SELECT master.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,master.product,products.prdtype,master.amount,master.date FROM dbo.master"
                    query += " INNER Join dbo.member ON master.cid = member.MemberNo INNER JOIN dbo.products ON master.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') AND products.prdtype = @typ AND master.cld = 0 ORDER by FirstName,product"

                End If

                cmd.CommandText = query

                cmd.Parameters.AddWithValue("@search", input)
                cmd.Parameters.AddWithValue("@typ", "Deposit")

                Dim result As New List(Of UserDetails1)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New UserDetails1
                        r.Memberno = IIf(IsDBNull(dr(0)), "", dr(0))
                        r.Firstname = IIf(IsDBNull(dr(2)), "", Trim(dr(2)))
                        r.Lastname = IIf(IsDBNull(dr(3)), "", Trim(dr(3)))
                        r.Address = IIf(IsDBNull(dr(4)), "", dr(4))
                        r.ImageURL = get_userimg(dr(1))
                        r.Product = IIf(IsDBNull(dr(5)), "", dr(5))
                        r.Amount = dr(5).ToString + " - " + Trim(dr(0).ToString) + " - " + CDate(dr(8)).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + FormatCurrency(dr(7))
                        result.Add(r)


                    End While
                    Return result
                End Using


            End Using

            con.Close()

        End Using

    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetLedger(ByVal input As String) As List(Of Jewel)
        Dim result As New List(Of Jewel)






        query = "select jewel.jlname from dbo.jewel ORDER BY jewel.jlname"
        cmd.Connection = con
        cmd.CommandText = query

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                cmd.CommandText = "select ledger from dbo.ledger  where ledger like '%'+ @search +'%'  ORDER BY ledger.ledger"
                cmd.Parameters.AddWithValue("@search", input)

                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New Jewel

                        r.Jewel = IIf(IsDBNull(dr(0)), "", dr(0))
                        result.Add(r)


                    End While
                    Return result
                End Using
            End Using
            con.Close()

        End Using




    End Function



    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetJewel(ByVal input As String) As List(Of Jewel)
        Dim result As New List(Of Jewel)






        query = "select jewel.jlname from dbo.jewel ORDER BY jewel.jlname"
        cmd.Connection = con
        cmd.CommandText = query

        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con

                cmd.CommandText = "select jewel.jlname from dbo.jewel  where jlname like '%'+ @search +'%'  ORDER BY jewel.jlname"
                cmd.Parameters.AddWithValue("@search", input)

                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New Jewel

                        r.Jewel = IIf(IsDBNull(dr(0)), "", dr(0))
                        result.Add(r)


                    End While
                    Return result
                End Using
            End Using
            con.Open()

        End Using




    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetUsers(ByVal input As String) As List(Of UserDetails)




        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                Dim mobilesearch As String = ""
                If input.Length > 6 Then mobilesearch = " OR mobile like @search +'%'"
                cmd.CommandText = "select memberno,firstname,lastname,address from member where (firstname like @search +'%'" & mobilesearch & " OR phone like @search +'%') and active=1  ORDER BY firstname"
                cmd.Parameters.AddWithValue("@search", input)
                Dim result As New List(Of UserDetails)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New UserDetails
                        r.Memberno = IIf(IsDBNull(dr(0)), "", dr(0))
                        r.Firstname = IIf(IsDBNull(dr(1)), "", Trim(dr(1)))
                        r.Lastname = IIf(IsDBNull(dr(2)), "", Trim(dr(2)))
                        r.Address = IIf(IsDBNull(dr(3)), "", dr(3))
                        r.ImageURL = get_userimg(dr(0))

                        result.Add(r)


                    End While
                    Return result
                End Using


            End Using
            con.Close()

        End Using














    End Function

    Function get_userimgnavbar(ByVal memno As String)
        Dim imgx As Image = Nothing
        Dim imgbytes As Byte() = Nothing
        Dim stream As MemoryStream = Nothing


        Dim imageurl As String


        Using con1 = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con1.Open()
            Using cmd1 = New SqlCommand

                cmd1.Connection = con1
                cmd1.CommandText = "select photo from kyc where memberno='" + memno + "'"

                Using drx As SqlDataReader = cmd1.ExecuteReader()

                    Try
                        If drx.HasRows() Then
                            drx.Read()



                            If Not IsDBNull(drx(0)) Then
                                imgbytes = CType(drx.GetValue(0), Byte())
                                If Not imgbytes.Length = 0 Then
                                    stream = New IO.MemoryStream(imgbytes, 0, imgbytes.Length)
                                    imgx = Image.FromStream(stream)
                                    '  imgx.Save("../captures/webcam.png", System.Drawing.Imaging.ImageFormat.Png)

                                    Dim imagePath As String = String.Format("~/Captures/{0}.png", memno)
                                    File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                                    'Session("CapturedImage") = ResolveUrl(imagePath)
                                    imageurl = "../captures/" + memno + ".png"
                                Else
                                    imageurl = "../Images/user.png"
                                End If
                            Else

                                imageurl = "../Images/user.png"

                            End If



                        Else
                            imageurl = "../Images/user.png"

                        End If
                    Catch ex As Exception
                        imageurl = "../Images/user.png"
                    End Try
                    Return imageurl

                End Using

            End Using
            con1.Dispose()

        End Using





    End Function



    Function get_userimg(ByVal memno As String)
        Dim imgx As Image = Nothing
        Dim imgbytes As Byte() = Nothing
        Dim stream As MemoryStream = Nothing


        Dim imageurl As String





        Using con1 = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con1.Open()
            Using cmd1 = New SqlCommand

                cmd1.Connection = con1
                cmd1.CommandText = "select photo from kyc where memberno='" + memno + "'"

                Using drx As SqlDataReader = cmd1.ExecuteReader()

                    Try
                        If drx.HasRows() Then
                            drx.Read()
                            If Not IsDBNull(drx(0)) Then
                                imgbytes = CType(drx.GetValue(0), Byte())
                                If Not imgbytes.Length = 0 Then
                                    stream = New IO.MemoryStream(imgbytes, 0, imgbytes.Length)
                                    imgx = Image.FromStream(stream)
                                    '  imgx.Save("../captures/webcam.png", System.Drawing.Imaging.ImageFormat.Png)

                                    Dim imagePath As String = String.Format("~/Captures/{0}.png", memno)
                                    File.WriteAllBytes(Server.MapPath(imagePath), imgbytes)
                                    'Session("CapturedImage") = ResolveUrl(imagePath)
                                    imageurl = "captures/" + memno + ".png"
                                Else
                                    imageurl = "../Images/user.png"
                                End If
                            Else

                                imageurl = "../Images/user.png"

                            End If



                        Else
                            imageurl = "../Images/user.png"

                        End If
                    Catch ex As Exception
                        imageurl = "../Images/user.png"
                    End Try
                    Return imageurl

                End Using

            End Using
            con1.Dispose()

        End Using





    End Function



    <WebMethod()>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetSOALoan(ByVal input As String) As List(Of UserDetails1)


        Using con = New SqlConnection(ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString)
            con.Open()
            Using cmd = New SqlCommand
                cmd.Connection = con
                Dim mobilesearch As String = ""
                If input.Length > 6 Then mobilesearch = " OR member.mobile Like @search + '%'"

                If session_user_role = "Audit" Then
                    query = "SELECT masterc.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,masterc.product,products.prdtype,masterc.amount,masterc.date FROM dbo.masterc"
                    query += " INNER Join dbo.member ON masterc.cid = member.MemberNo INNER JOIN dbo.products ON masterc.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') and masterc.acno like '6%' ORDER by FirstName,product,masterc.date"
                Else
                    query = "SELECT master.acno,member.MemberNo,member.FirstName,member.LastName,member.Address,master.product,products.prdtype,master.amount,master.date FROM dbo.master"
                    query += " INNER Join dbo.member ON master.cid = member.MemberNo INNER JOIN dbo.products ON master.product = products.shortname"
                    query += " WHERE (member.FirstName Like @search + '%'" & mobilesearch & " OR member.phone Like @search + '%') and master.acno like '6%'  ORDER by FirstName,product,master.date"
                End If
                cmd.CommandText = query

                cmd.Parameters.AddWithValue("@search", input)


                Dim result As New List(Of UserDetails1)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read
                        Dim r As New UserDetails1
                        r.Memberno = IIf(IsDBNull(dr(0)), "", dr(0))
                        r.Firstname = IIf(IsDBNull(dr(2)), "", Trim(dr(2)))
                        r.Lastname = IIf(IsDBNull(dr(3)), "", Trim(dr(3)))
                        r.Address = IIf(IsDBNull(dr(4)), "", dr(4))
                        r.ImageURL = get_userimgnavbar(dr(1))
                        r.Product = IIf(IsDBNull(dr(5)), "", dr(5))
                        r.Amount = dr(5).ToString + " - " + dr(0).ToString + " - " + CDate(dr(8)).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " + FormatCurrency(dr(7))


                        result.Add(r)


                    End While
                    Return result
                End Using


            End Using
            con.Close()

        End Using

    End Function


End Class