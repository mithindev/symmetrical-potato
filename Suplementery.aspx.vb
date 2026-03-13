Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Web.Services
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization

Public Class Suplementery
    Inherits System.Web.UI.Page
    Dim con As New SqlClient.SqlConnection
    Dim con1 As New SqlClient.SqlConnection
    Dim cmd As New SqlClient.SqlCommand
    Dim cmdi As New SqlClient.SqlCommand
    Dim cmdx As New SqlClient.SqlCommand
    Dim newrow As DataRow
    Public dr_sum As Double = 0
    Public cr_sum As Double = 0
    Public dt As New DataTable
    Public dtx As New DataTable
    Dim countresult As Integer
    Public dt_dl As New DataTable
    Dim ds As New DataSet
    Dim ds1 As New DataSet
    Dim monthpart As String
    Dim yearpart As String


    Public Sub txtfocus(ByRef ctrl As System.Web.UI.WebControls.WebControl)
        ClientScript.RegisterStartupScript(Me.GetType(), "focus", String.Format("function pageLoad() {{document.getElementById('{0}').focus();}}", ctrl.ClientID), True)


    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'rddue.Visible = False
        ' rdpenalty.Visible = False

        con.ConnectionString = ConfigurationManager.ConnectionStrings("fiscusdbConnectionString").ConnectionString

        con.Open()



        If Not Page.IsPostBack Then

            bind_sum()




        End If



    End Sub


    Sub bind_sum()





        Dim dt As New DataSet
        query = "SELECT   trans.suplimentry,  SUM(trans.Drc) AS dr,  SUM(trans.Crc) AS cr FROM dbo.trans WHERE trans.Type = 'CASH' GROUP BY trans.suplimentry "
        query &= " ; SELECT   trans.id,trans.acno,  trans.Drc,  trans.Crc,  trans.Narration,  trans.suplimentry,  member.FirstName FROM dbo.trans "
        query &= " INNER JOIN dbo.master   ON trans.acno = master.acno INNER JOIN dbo.member   ON master.cid = member.MemberNo WHERE trans.Type = 'CASH' "

        cmd.Connection = con
        cmd.CommandText = query

        Try

            Using Sda As New SqlDataAdapter(cmd)


                Sda.Fill(dt)


            End Using

        Catch ex As Exception

            Response.Write(ex.ToString)


        End Try

        'suplimentry

        dt.Relations.Add(New DataRelation("suplimentry", dt.Tables(0).Columns("suplimentry"), dt.Tables(1).Columns("suplimentry")))

        rpsuplement.DataSource = dt.Tables(0)
        rpsuplement.DataBind()

        If Not dt.Tables(0).Rows.Count = 0 Then
            lbltdr.Text = FormatCurrency(dt.Tables(0).Compute("sum(dr)", ""))
            lblrcr.Text = FormatCurrency(dt.Tables(0).Compute("sum(cr)", ""))
        Else
            lbltdr.Text = "0.00"
            lblrcr.Text = "0.00"
        End If


    End Sub

    Protected Sub OnItemDataBound(sender As Object, e As RepeaterItemEventArgs)

        If e.Item.ItemType = ListItemType.Item OrElse
                            e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim drv As DataRowView = TryCast(e.Item.DataItem, DataRowView)
            Dim ChildRep As Repeater = TryCast(e.Item.FindControl("rpbrkup"), Repeater)
            ChildRep.DataSource = drv.CreateChildView("suplimentry")
            ChildRep.DataBind()
        End If
    End Sub



    Private Sub Suplementery_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If session_user_role = "Admin" Then
            Me.MasterPageFile = "~/admin/Admin.Master"

        Else
            Me.MasterPageFile = "~/User/User.Master"
        End If

    End Sub
End Class