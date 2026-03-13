<%@ Page Title="Field Collection" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="FieldCollection.aspx.vb" Inherits="Fiscus.FieldCollection" EnableEventValidation="false"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/daterangepicker.css" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <form class="frmfc" runat="server" >
		<asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
                       
				<nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Transaction</a></li>
						<li class="breadcrumb-item active" >Field Collection</li>
					</ol>
				</nav>


    

		 	<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">

								    <div id="smarttab">
        <ul class="nav">
           <li>
               <a class="nav-link" href="#tab-1">
                    Summary
               </a>
           </li>
           <li>
               <a class="nav-link" href="#tab-2">
                  Transfer
               </a>
           </li>
			<li>
               <a class="nav-link" href="#tab-3">
                  DL Transfer
               </a>
           </li>
    </ul>

										<div class="tab-content">
           <div id="tab-1" class="tab-pane" role="tabpanel">

               

               <div class="d-flex justify-content-between align-items-baseline">
                        <div class="form-group row border-sm-top">
                       <label class="col-form-label col-md-1 ">Date</label>
                       <div class="col-md-3">
                           <asp:TextBox CssClass="form-control" ID="txtdate" runat="server" ></asp:TextBox>
                       </div>
                           <label class="col-form-label col-md-1 ">Collected By</label>
                       <div class="col-md-1"></div>
                       <div class="col-md-4">
                           <asp:UpdatePanel runat="server" >
                               <Triggers>
                                   <asp:PostBackTrigger ControlID="agent" />
                               </Triggers>
                               <ContentTemplate>

                               
                           <asp:DropDownList CssClass="form-control" runat="server" ID="agent" AutoPostBack="true" >

                           </asp:DropDownList>
                                   </ContentTemplate>
                           </asp:UpdatePanel>
                       </div>
                   </div>
                      <div class="dropdown mb-2">
                        <button class="btn p-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-more-horizontal icon-lg text-muted pb-3px"><circle cx="12" cy="12" r="1"></circle><circle cx="19" cy="12" r="1"></circle><circle cx="5" cy="12" r="1"></circle></svg>
                        </button>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton" style="">
                          <asp:LinkButton ID="btnprnt" runat="server" class="dropdown-item d-flex align-items-center"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-printer icon-sm mr-2"><polyline points="6 9 6 2 18 2 18 9"></polyline><path d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2"></path><rect x="6" y="14" width="12" height="8"></rect></svg> <span class="">Print</span></asp:LinkButton>
                            
                        </div>
                      </div>
                    </div>

                 

               <asp:Panel runat="server" ID="pnlcol">
                   <table class="table table-bordered">
                       <thead>
                       <tr>
                           <th style="text-align:center;width:10%">S.No</th>
                           <th style="text-align:center;width:15%">Account</th>
                           <th style="text-align:center;width:15%">Account No</th>
                           <th style="text-align:center;width:45%">Customer Name</th>
                           <th style="text-align:center;width:15%">Amount</th>
                           
                       </tr>
                           </thead>
                   </table>
                               <asp:UpdatePanel  runat ="server"  >
                    <ContentTemplate>
                        <asp:GridView  runat="server" ID="disp"  AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left" 
                            AllowPaging ="false"  OnSelectedIndexChanged="OnSelectedIndexChanged_gv_grp" OnRowDataBound="gv_dep_RowDataBound" 
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table  table-hover  table-bordered  "    >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                <%# Container.DataItemIndex + 1 %>

                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />

                                </asp:TemplateField>
                                     
                                   <asp:TemplateField>

                                            <ItemTemplate>

                                                <asp:Label ID="lblprd" runat="server" text='<%#Eval("product")%>' Style="text-align:left  ;float:left  "></asp:Label>
                                                <asp:Label ID="lblid" runat="server" text='<%#Eval("ID")%>' Style="text-align:left  ;float:left" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>

   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblacno" runat="server" text='<%#Eval("acno")%>' Style="text-align:left ;float:left  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" text='<%#Eval("FirstName")%>' Style="text-align:left ;float:left  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="45%" />

                                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblnar" runat="server" text='<%#Eval("credit", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>


                                </Columns> 
                                                </asp:GridView>
                        <table class="table table-bordered ">
                            <tr>
                                <td style="width:85%;text-align:center">Total</td>
                                <td style="width:15%;text-align:right"><asp:Label ID="lbltotal" runat="server" CssClass="text-success"></asp:Label> </td>
                            </tr>
                        </table>
                        </ContentTemplate>
                                                            </asp:UpdatePanel>
    
                   
               </asp:Panel>

               <asp:UpdatePanel runat="server" >
                   <ContentTemplate>

               
                <asp:UpdatePanel runat="server"  ID="pnldel" Visible="false" >
                    <ContentTemplate>
               <div class ="card card-body ">
                        <asp:Panel runat="server" >

                            <div class="form-group row ">
                                <label class="col-form-label">Account No</label>
                                <asp:Label runat="server" ID="txtacn" CssClass =" col-md-3"></asp:Label> 
                                <div class ="col-md-1 ">
                                    <asp:Button runat="server" CssClass="btn btn-outline-danger " Text="Delete" ID="btn_del" />
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>               
              
            
                   </ContentTemplate>
               </asp:UpdatePanel>
           </div>

                                            <div id="tab-2" class="tab-pane" role="tabpanel" >
                                                

                                                 <div class="form-group row border-sm-top">
                       <label class="col-form-label col-md-1 ">Date</label>
                       <div class="col-md-2">
                           <asp:TextBox CssClass="form-control" ID="txttdate" runat="server" ></asp:TextBox>
                       </div>
                           <label class="col-form-label col-md-1 ">Account</label>
                       <div class="col-md-1"></div>
                       <div class="col-md-2">
                           <asp:UpdatePanel runat="server" >
                               <ContentTemplate>

                               
                           <asp:DropDownList CssClass="form-control " runat="server" ID="ddprod" AutoPostBack="true" >
                               <asp:ListItem><-select-></asp:ListItem>
                               <asp:ListItem>DS</asp:ListItem>
                               <asp:ListItem>RD</asp:ListItem>
                               <asp:ListItem>SB</asp:ListItem>
                               <asp:ListItem>DCL</asp:ListItem>
                           </asp:DropDownList>
                                   </ContentTemplate>
                           </asp:UpdatePanel>
                       </div>
                   </div>

                                                      <asp:Panel runat="server" ID="pnltrf">
                   <table class="table table-bordered">
                       <thead>
                       <tr>
                           <th style="text-align:center;width:10%">S.No</th>
                           <th style="text-align:center;width:15%">Account</th>
                           <th style="text-align:center;width:15%">Account No</th>
                           <th style="text-align:center;width:45%">Customer Name</th>
                           <th style="text-align:center;width:15%">Amount</th>
                           
                       </tr>
                           </thead>
                   </table>
                               <asp:UpdatePanel  runat ="server"  >
                    <ContentTemplate>
                        <asp:GridView  runat="server" ID="dispt"  AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left" 
                            AllowPaging ="false" 
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table  table-hover  table-bordered  "    >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                <%# Container.DataItemIndex + 1 %>

                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />

                                </asp:TemplateField>
                                     
                                   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblprd" runat="server" text='<%#Eval("product")%>' Style="text-align:left  ;float:left  "></asp:Label>
                                                </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>

   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblacno" runat="server" text='<%#Eval("acno")%>' Style="text-align:left ;float:left  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" text='<%#Eval("name")%>' Style="text-align:left ;float:left  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="45%" />

                                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblcr" runat="server" text='<%#Eval("credit", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>


                                </Columns> 
                                                </asp:GridView>
                        <table class="table table-bordered ">
                            <tr>
                                <td style="width:85%;text-align:center">Total</td>
                                <td style="width:15%;text-align:right"><asp:Label ID="lblttotal" runat="server" CssClass="text-success"></asp:Label> </td>
                            </tr>
                        </table>
                        </ContentTemplate>
                                                            </asp:UpdatePanel>
    
                                                          <div class="form-group row border-top  ">
                       <div class="col-md-4"></div>
                       <div class="col-md-4">
                           <asp:Button CssClass="btn btn-outline-primary " runat="server" id="btntrf" Text ="Transfer" />
                           <asp:Button CssClass="btn btn-outline-danger  " runat="server"  ID="btnclr" Text ="Cancel" />
                       </div>
                   </div>
               </asp:Panel>

                                            </div>

                                                  <div id="tab-3" class="tab-pane" role="tabpanel" >
                                                

                                                 <div class="form-group row border-sm-top">
                       <label class="col-form-label col-md-1 ">Date</label>
                       <div class="col-md-2">
                           <asp:TextBox CssClass="form-control" ID="txtddate" runat="server" ></asp:TextBox>
                       </div>
                          
                   </div>

                                                      <asp:Panel runat="server" ID="pnldl">
                   <table class="table table-bordered">
                       <thead>
                       <tr>
                           <th style="text-align:center;width:10%">S.No</th>
                           <th style="text-align:center;width:15%">Account</th>
                           <th style="text-align:center;width:15%">Account No</th>
                           <th style="text-align:center;width:45%">Customer Name</th>
                           <th style="text-align:center;width:15%">Amount</th>
                           
                       </tr>
                           </thead>
                   </table>
                               <asp:UpdatePanel  runat ="server"  >
                    <ContentTemplate>
                        <asp:GridView  runat="server" ID="dispdl"  AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left" 
                            AllowPaging ="false" 
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table  table-hover  table-bordered  "    >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                <%# Container.DataItemIndex + 1 %>

                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />

                                </asp:TemplateField>
                                     
                                   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblprd" runat="server" text='<%#Eval("product")%>' Style="text-align:left  ;float:left  "></asp:Label>
                                                </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>

   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblacno" runat="server" text='<%#Eval("acno")%>' Style="text-align:left ;float:left  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" text='<%#Eval("name")%>' Style="text-align:left ;float:left  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="45%" />

                                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblcr" runat="server" text='<%#Eval("credit", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>


                                </Columns> 
                                                </asp:GridView>
                        <table class="table table-bordered ">
                            <tr>
                                <td style="width:85%;text-align:center">Total</td>
                                <td style="width:15%;text-align:right"><asp:Label ID="lbldl" runat="server" CssClass="text-success"></asp:Label> </td>
                            </tr>
                        </table>
                        </ContentTemplate>
                                                            </asp:UpdatePanel>
    
                                                          <div class="form-group row border-top  ">
                       <div class="col-md-4"></div>
                       <div class="col-md-4">
                           <asp:Button CssClass="btn btn-outline-primary " runat="server" id="btn_dltrf" Text ="Transfer" />
                           <asp:Button CssClass="btn btn-outline-danger  " runat="server"  ID="btn_dlclr" Text ="Cancel" />
                       </div>
                   </div>
               </asp:Panel>

                                            </div>
											</div>
										</div>
								</div>
							</div>
						</div>
				 </div>
		 
		     <script src="../js/jquery.min.js" type="text/javascript" ></script>
    <script src="../js/daterangepicker.js" type="text/javascript"></script>
	<script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
    <script >
		    $(document).ready(function () {


            
            
         
            // Place here the first init of the autocomplete
            //InitAutoCompl();
                $('#smarttab').smartTab({

                selected: 0, // Initial selected tab, 0 = first tab
                //theme: 'dark', // theme for the tab, related css need to include for other than default theme
                orientation: 'horizontal', // Nav menu orientation. horizontal/vertical
                justified: false, // Nav menu justification. true/false
                autoAdjustHeight: false, // Automatically adjust content height
                backButtonSupport: true, // Enable the back button support
                enableURLhash: true, // Enable selection of the tab based on url hash
                transition: {
                    animation: 'slide-swing', // Effect on navigation, none/fade/slide-horizontal/slide-vertical/slide-swing
                    speed: '400', // Transion animation speed
                    easing: '' // Transition animation easing. Not supported without a jQuery easing plugin
                },
                keyboardSettings: {
                    keyNavigation: false, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
                    keyLeft: [37], // Left key code
                    keyRight: [39] // Right key code
                }
                });



                $(document).ready(function () {


                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    prm.add_initializeRequest(InitializeRequest);
                    prm.add_endRequest(EndRequest);
                    InitDatePick();
                    

                });

                function InitializeRequest(sender, args) {
                }
                function EndRequest(sender, args) {
                    // after update occur on UpdatePanel re-init the Autocomplete

                    InitDatePick();
                }
        });

        function InitDatePick() {

            $("#<%=txtdate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,    
                showDropdowns: false,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                 $("#<%=txtdate.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

            $("#<%=txttdate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,    
                showDropdowns: false,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                 $("#<%=txttdate.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

            $("#<%=txtddate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,    
                showDropdowns: false,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                 $("#<%=txtddate.ClientID%>").val(start.format('DD-MM-YYYY'));
             });

        }

    </script>
   </form>

</asp:Content>
