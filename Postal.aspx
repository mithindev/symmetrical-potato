<%@ Page Title="Postal" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Postal.aspx.vb" Inherits="Fiscus.Postal" EnableEventValidation="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link rel="stylesheet" href="css/daterangepicker.css" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form class="frmpostal" runat="server" >
		<asp:ScriptManager ID="SM1" runat="server" AsyncPostBackTimeout="36000" ></asp:ScriptManager>
                       
				<nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Transaction</a></li>
						<li class="breadcrumb-item active" >Postal</li>
					</ol>
				</nav>

        <asp:UpdatePanel runat="server" >
            <ContentTemplate>

        <asp:UpdatePanel ID="alertmsg" runat="server" Visible="false">
			<ContentTemplate>
		<div class="alert alert-success  alert-dismissible fade show" role="alert">
	<strong>Hi 
		<asp:Label ID="sesuser" runat="server" ></asp:Label> <asp:Label id="lblinfo" runat="server"></asp:Label>
				<button type="button" class="close" data-dismiss="alert" aria-label="Close">

		<span aria-hidden="true">&times;</span>
	</button>
				
	</div>

			</ContentTemplate>
			</asp:UpdatePanel>
                
            </ContentTemplate>
        </asp:UpdatePanel>

			<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								<h6 class="card-title">Postal Entry</h6>
								    <div class="form-group row border-sm-top">
                       <label class="col-form-label col-sm-1 ">Date</label>
                       <div class="col-md-2">
                           <asp:TextBox CssClass="form-control" ID="txtdate" runat="server" ></asp:TextBox>
                       </div>
                           <label class="col-form-label col-md-1 ">Loan</label>
                       
                       <div class="col-md-2">
                           <asp:UpdatePanel runat="server" >
                               <ContentTemplate>

                               
                           <asp:DropDownList CssClass="form-control " runat="server" ID="ddloan" AutoPostBack="true" >
                               <asp:ListItem Value=""><-Select-></asp:ListItem>
                               <asp:ListItem Value="DCL">DCL</asp:ListItem>
                               <asp:ListItem Value="DL">DL</asp:ListItem>
                               <asp:ListItem Value="JL">JL</asp:ListItem>
                               <asp:ListItem Value="ML">ML</asp:ListItem>

                           </asp:DropDownList>
                                   </ContentTemplate>
                           </asp:UpdatePanel>
                       </div>
										   
										 <label class="col-form-label col-md-1 ">Particulars</label>
                       
                       <div class="col-md-3">

                           <asp:UpdatePanel runat="server" ID="pnlsrch" >
                               <ContentTemplate>
                                      <div class="input-group ">
                               
                           <asp:DropDownList CssClass="form-control " runat="server" ID="acchrg" AutoPostBack="true" >

                           </asp:DropDownList>
                                   <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                             
                                                          </span>
                                   </div>
                                   </ContentTemplate>
                           </asp:UpdatePanel>
                       </div>

                                        <div class="col-md-2">
                                            
                                            <asp:LinkButton runat="server" id="btnadd" CssClass ="btn btn-outline-primary ">

                                                                   
<svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-plus"><line x1="12" y1="5" x2="12" y2="19"></line><line x1="5" y1="12" x2="19" y2="12"></line></svg>
                                                                </asp:LinkButton>
                                                
                           
                                        </div>
											
                       </div>


                                  <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlsrch">
<ProgressTemplate>
    <div id="blocker">
        <div style="margin:0 auto ">
            <div class="sk-circle">
  <div class="sk-circle1 sk-child"></div>
  <div class="sk-circle2 sk-child"></div>
  <div class="sk-circle3 sk-child"></div>
  <div class="sk-circle4 sk-child"></div>
  <div class="sk-circle5 sk-child"></div>
  <div class="sk-circle6 sk-child"></div>
  <div class="sk-circle7 sk-child"></div>
  <div class="sk-circle8 sk-child"></div>
  <div class="sk-circle9 sk-child"></div>
  <div class="sk-circle10 sk-child"></div>
  <div class="sk-circle11 sk-child"></div>
  <div class="sk-circle12 sk-child"></div>
</div>
            Processing.....
        </div>
    </div>
</ProgressTemplate>
</asp:UpdateProgress>




                   <asp:Updatepanel runat="server" >
                       <ContentTemplate>

                       

			<asp:Updatepanel runat="server"	id="pnladdpostal" Visible="false" >
                <ContentTemplate>
                    <div class="card card-body">
                    <div class="form-group row ">
                        <label class="col-md-2 col-form-label ">Account No</label>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtacn" runat="server" CssClass="form-control "></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:Updatepanel runat="server" >
                                <ContentTemplate>
                                    <asp:TextBox runat="server" CssClass="form-control " ID="txtfirstname"></asp:TextBox>
                            <asp:TextBox runat="server" CssClass="form-control " ID="txtlname" Visible="false" ></asp:TextBox>
                                </ContentTemplate>
                            </asp:Updatepanel>
                            
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btn_add" CssClass="btn btn-outline-primary " Text="ADD" />
                        </div>
                    </div>

                        </div>
                </ContentTemplate>
			</asp:Updatepanel>
                                
                          </ContentTemplate>
                   </asp:Updatepanel>

                                <div class="col-md-auto" >
                                    <asp:Button CssClass="btn btn-outline-primary" Text="Select All" runat="server" ID="btnSelectAll" />
                                    <asp:Button CssClass="btn btn-outline-primary" Text="Clear" runat="server" ID="btnUnselectAll" />
                                </div>
                                <br />
        <asp:UpdatePanel runat="server" >
            <ContentTemplate>
                <asp:Repeater runat="server" ID="rppostal">
                    <HeaderTemplate>
                              <table class="table table-bordered " >
                                  <thead>
                                    <tr >
                                        <th style="text-align:center;width:8%"></th>
                                        <th style="text-align:center;width:20%">Account No</th>
                                        <th style="text-align:center;width:34%">Customer Name</th>
                                        <th style="text-align:center;width:20%">Particulars</th>
                                        <th style="text-align:center;width:10%">Amount</th>
                                        
                                    </tr>
        </thead>

                    </HeaderTemplate>
                    <ItemTemplate>

                        <tr>
                            
                            <td style="text-align:center">
                                <asp:UpdatePanel runat="server" >
                                    <ContentTemplate>
                                        <asp:CheckBox runat="server" Checked="false" ID="ispostal" AutoPostBack="true" CssClass="form-check" OnCheckedChanged="ispostal_checkedChanged" /> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                        
                                 
                               </td>
                            <td><asp:Label ID="lblacno" runat="server" Text='<%# Eval("acno") %>'></asp:Label> </td>
                            <td><asp:Label runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                <br /> 
                                <asp:Label runat="server" Text='<%# Eval("LastName") %>' ></asp:Label>
                            </td>
                            
                            <td><asp:Label ID="lblnature" runat="server" Text='<%# Eval("nature") %>'></asp:Label></td>
                            <td style="text-align:right "><asp:Label runat="server" Text='<%# Eval("chrgs")  %>' id="lblchrgs"></asp:Label></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>

                                <asp:UpdatePanel runat="server" >
                                    <ContentTemplate>

                                <asp:Panel runat="server" ID="pnlbtn" Visible="false" >

                                 <asp:UpdatePanel runat="server" >
                                            <ContentTemplate>
                                       

                                <div class="form-group row ">

                <label class="col-md-1 ">Total Postal</label>
                                    
                                                <asp:Label runat="server" CssClass="text-primary col-md-2" id="lbltotal" Font-Size="Medium" Text="0.00"></asp:Label>
                                            
                                    
            </div>
       </ContentTemplate>
                                        </asp:UpdatePanel>

                                <div class="form-group row border-top ">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4">
                                        
                                            <asp:Button CssClass="btn btn-outline-secondary " Text="Clear" runat="server" ID="btnclr" />
                                            <asp:Button CssClass="btn btn-outline-primary  " Text="Update" runat="server" ID="btnup" />
                                        
                                    </div>
                                </div>
                                    </asp:Panel>
                                        </ContentTemplate>
                                </asp:UpdatePanel>

                </div>
							</div>
						</div>
		
                </div>

             
		</form>
      <script src="../js/jquery.js" type="text/javascript"></script>
     
    
	<script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
   
	<script src="js/daterangepicker.js" type="text/javascript" ></script>

    
        <script type="text/javascript">


            $(document).ready(function () {


            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            // Place here the first init of the autocomplete
            InitAutoCompl();

        });


        function InitializeRequest(sender, args) {
            }

        function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete
                InitAutoCompl();
        }

        function InitAutoCompl() {


                $("#<%=txtdate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=txtdate.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

            $("#<%=txtacn.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/membersearch.asmx/GetLoan") %>',
						data: "{ 'input': '" + request.term + "'}",
						dataType: "json",
						type: "POST",
						contentType: "application/json; charset=utf-8",
						success: function (data) {
							response($.map(data.d, function (item) {
								return {
									memberno: item.Memberno,
									firstname: item.Firstname,
									lastname: item.Lastname,
									address: item.Address,
                                    image: item.ImageURL,
                                    product: item.Product,
                                    amt: item.Amount,
                                    
								}
							}))
						}
					});
				},
                minLength: 1,
                focus: function (event, ui) {

                    $("#<%=txtacn.ClientID %>").val(ui.item.memberno);
                    return false;

                },
                open: function () {
                    $("ul.ui-menu").width($(this).innerWidth() + 210);

                },

				select: function (event, ui) {

                    $("#<%=txtacn.ClientID %>").val(ui.item.memberno);

                    $("#<%=txtfirstname.ClientID %>").val(ui.item.firstname);
                    $("#<%=txtlname.ClientID %>").val(ui.item.lastname);


                      return false;
                  }
              })
                  .autocomplete("instance")._renderItem = function (ul, item) {
                      return $("<li>")
                          .append("<div>")
                          .append("<div class='img-div column'>")
                          .append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
                          .append("</div")
                          .append("<div class='info-div column'>")
                          .append("<div class='username'>" + item.firstname + "</div>")
                          .append("<div class='userinfo'>" + item.lastname + "</div>")
                          .append("<div class='userinfo'>" + item.address + "</div>")
                          .append("<div class='userinfo1'>" + item.amt + " </div>")
                          .append("</br>")
                          .append("</div")
                          .append("</div>")
                          .appendTo(ul);
                  };
          
        }


        </script>

</asp:Content>
