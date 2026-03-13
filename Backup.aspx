<%@ Page Title="Backup" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Backup.aspx.vb" Inherits="Fiscus.Backup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/daterangepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form runat="server" >
         <asp:ScriptManager ID="SM3" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Home</a></li>
						<li class="breadcrumb-item active" >BackUp</li>
					</ol>
				</nav>
	
          
                        <div class="row">
                            <div class="card card-body">
                                <h6 class="card-title text-primary ">DataBase Backup</h6>
                                
                            
                             <div class="form-group row">
                                 <label class="col-sm-2 col-form-label text-primary ">Backup Date</label>
                                 <div class="col-sm-2">
                                     <asp:TextBox  ID="txtdt" runat="server"   Width="130px" CssClass="datepicker form-control" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                 </div>

                                 <label class="col-sm-2 col-form-label text-primary ">Backup Location</label>
                                 <div class="col-sm-2">
                                    <asp:TextBox  ID="txtpath" runat="server"   Width="300px" CssClass="form-control "  data-validation-engine="validate[required]" data-errormessage-value-missing="Path Missing" Text="E:\Backup" ReadOnly  ></asp:TextBox>
                                 </div>

                            </div>

                            <div class="form-group row border-bottom "></div>
                            <div class="form-group row ">
                                <div class="col-sm-5"></div>
                                <div class="col-sm-5">
                                    <asp:Button ID="btn_bkup" runat="server" CssClass =" btn btn-outline-primary " Text="BACKUP"   />
                                </div>
                            </div>

                  </div>
                            </div>

    </form>
    <script src="js/jquery.js"></script>
    <script src="js/daterangepicker.js"></script>
    <script  type="text/javascript"  >

        $(document).ready(function () {


            $("#<%=txtdt.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,    
                showDropdowns: false,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=txtdt.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

        })

    </script>

</asp:Content>
