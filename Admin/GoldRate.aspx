
<%@ Page Title="Gold Rate" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="GoldRate.aspx.vb" Inherits="Fiscus.GoldRate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../css/daterangepicker.css" rel="stylesheet" />
    <link href="../css/ValidationEngine.css" rel="stylesheet" />
    <link href="../css/validationEngine.jquery.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Master</a></li>
						<li class="breadcrumb-item active" >Gold Rate</li>
					</ol>
				</nav>
	
			

	<form id="frmGoldRate" name="frmGoldRate" runat="server" >
        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
    
   


				<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								    <div id="smarttab">
        <ul class="nav">
           <li>
               <a class="nav-link" href="#tab-1">
                    New Rate
               </a>
           </li>
           <li>
               <a class="nav-link" href="#tab-2">
                  Previous Rate
               </a>
           </li>
    </ul>

										<div class="tab-content">
           <div id="tab-1" class="tab-pane" role="tabpanel">
               
                   <div class="form-group row">
										<label class="col-sm-1 col-form-label" for="txtcid">Date</label>
										<div class="col-sm-2">
                                            <asp:TextBox ID="txtfrm" CssClass=" form-control "  runat="server"    data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
								</div>
               </div>

               <h6 class="card-title text-primary">Rate Per Gram</h6>
               <div class="form-group row">
                   								
                   <label class="col-sm-1 col-form-label" for="txtcid">Prime</label>
                   <div class="col-sm-2">
                                            <asp:TextBox ID="txtrate" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]"></asp:TextBox>
								</div>
                   <label class="col-sm-1 col-form-label" for="txtcid">Prime Plus</label>
                   <div class="col-sm-2">
                                            <asp:TextBox ID="txtplusrate" CssClass=" form-control "  runat="server"    data-validation-engine="validate[required,funcCall[chkdecim[]]" data-errormessage-value-missing=""></asp:TextBox>
								</div>
                   <label class="col-sm-1 col-form-label" for="txtcid">Prime Ultra</label>
                   <div class="col-sm-2">
                                              <asp:TextBox ID="txtultra" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]"></asp:TextBox>
								</div>
               </div>
               		<div class="form-group border-bottom"></div>

               <h6 class="card-title text-primary">Loan Limit</h6>

                <div class="form-group row">
                   								
                   <label class="col-sm-1 col-form-label" for="txtcid">Limit in %</label>
                   <div class="col-sm-3">
                                         <asp:TextBox ID="txtlimit" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]" ToolTip="in %"></asp:TextBox>
								</div>
               </div>
               		

                <div class="form-group row">
                   								
                   <label class="col-sm-1 col-form-label " for="txtcid">Prime Plus</label>
                   <div class="col-sm-2">
                                            <asp:TextBox ID="txtlmtpp" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]" ToolTip="in %"></asp:TextBox>
								</div>
                   <label class="col-sm-1 col-form-label" for="txtcid">If RD</label>
                   <div class="col-sm-2">
                                            <asp:TextBox ID="txtlmtppwrd" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]" ToolTip="in %"></asp:TextBox>

								</div>
                   <label class="col-sm-1 col-form-label" for="txtcid">Prime Ultra</label>
                   <div class="col-sm-2">
                                           <asp:TextBox ID="txtultralimit" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]"></asp:TextBox>
								</div>
               </div>
               <div class="form-group border-bottom"></div>
               <h6 class="card-title text-primary">Special Schemes</h6>
               <div class="form-group row">
                   								
                   <label class="col-sm-1 col-form-label " for="txtcid">Sr. Citizen</label>
                   <div class="col-sm-2">
                                           <asp:TextBox ID="txtsrc" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]" ToolTip="in %"></asp:TextBox>
								</div>
                   <label class="col-sm-1 col-form-label" for="txtcid">Transfer</label>
                   <div class="col-sm-3">
                       <div class="input-group mb-3">
                           <div class="input-group-prepend">
                                   <div class="input-group-text" style="height:36px">

                               <asp:CheckBox runat="server" ID="istrf" CssClass="form-check" />
                                       </div>
                           </div>
                                            <asp:TextBox ID="txttrf" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]" ToolTip="in %"></asp:TextBox>
                           </div>
								</div>
                   <label class="col-sm-1 col-form-label" for="txtcid">Renewal</label>
                   <div class="col-sm-3">
                       <div class="input-group mb-3">
                           <div class="input-group-prepend">
                                   <div class="input-group-text" style="height:36px">

                               <asp:CheckBox runat="server" ID="isren" CssClass="form-check" />
                                       </div>
                           </div>

                   <asp:TextBox ID="txtrenew" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]" ToolTip="in %"></asp:TextBox>
                       </div>
								</div>
               </div>

                 <div class="form-group row">
                   								
                   <label class="col-sm-1 col-form-label " for="txtcid">Jewel Loan</label>
                   <div class="col-sm-2">
              
                 <asp:TextBox ID="txtrebate" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]" ToolTip="in %"></asp:TextBox>
                       </div>

                     <label class="col-sm-1 col-form-label " for="txtcid">Deposits</label>
                   <div class="col-sm-2">
              
                 <asp:TextBox ID="txtdep" CssClass="form-control " runat="server" Width="150px" data-validation-engine="validate[required,funcCall[chkdecim[]]" ToolTip="in %"></asp:TextBox>
                       </div>


                     </div>

               
                  <asp:Panel runat="server"  ID="pnlotp" Visible="false">
                      
                      
            <div class="form-group border-bottom"></div>
                      <div class="form-group row">

                          <label class="col-sm-1 col-form-label text-secondary" for="txtcid">Enter OTP</label>
                          <div class="col-sm-2">
                              <asp:TextBox ID="txtotp" CssClass="form-control "   runat="server"    data-validation-engine="validate[required]" data-errormessage-value-missing="Enter Valid OTP"></asp:TextBox>
                          </div>
                          <div class="col-sm-2">
                              <asp:Button  runat="server"  style="display:inline-block;float:left "  ID="btnotp" CssClass="btn btn-outline-primary " Text ="Submit"  />
                      </div>
                           </div>   
                              
                            </asp:Panel>
                   <div class="form-group border-bottom"></div>
										<div class="form-group row">
											
											<div class="col-sm-4"></div>
											<div class="col-sm-4">
		

         <asp:Button  runat="server"   ID="btn_clr" CssClass="btn btn-outline-secondary  " Text ="Clear" CausesValidation="false"  OnClientClick  ="return detach();" />
                        <asp:Button  runat="server"   ID="btn_update" CssClass="btn btn-outline-primary " Text ="Update"  />                        
    </div>
                                            </div>
    
              
         </div>
											<div id="tab-2" class="tab-pane" role="tabpanel">

                                                                       <div >
                              <table style="width:90%;margin-left:0px;float:left "  class="table table-bordered">
                                  <thead>

                                         <th style="width:17%;text-align:center; ">Date</th>
                                         <th style="width:22%;text-align:center;">Rate per GM</th>
                                         <th style="width:22%;text-align:center;">Loan Limit in %</th>
                                         <th style="width:22%;text-align:center;">PRIME PLUS</th>
                                         <th style="width:22%;text-align:center;">PRIME PLUS W RD</th>
                                      </thead>
                                  </table>
                <asp:UpdatePanel  runat ="server"  >
                    <ContentTemplate>
                        <asp:GridView  runat="server" ID="disp"  AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="15" OnRowDataBound="disp_RowDataBound" OnSelectedIndexChanged ="OnSelectedIndexChanged" 
                             Width="90%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "    >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="17%" />

                                </asp:TemplateField>
                                     
                                   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblrt" runat="server" text='<%#Eval("rate", "{0:C}")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                                                           <ItemStyle Width="22%" />

                                                </asp:TemplateField>

   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblnar" runat="server" text='<%#Eval("limit", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="22%" />

                                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblnar" runat="server" text='<%#Eval("loanlimit", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="22%" />

                                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblnar" runat="server" text='<%#Eval("loanlimitwrd", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="22%" />

                                                </asp:TemplateField>


                                </Columns> 
                                                </asp:GridView>
                        </ContentTemplate>
                                                            </asp:UpdatePanel>
            
                
                        </div>
        
        </div>
    </div>
        

         </div>
										</div>
								</div>
							</div>
						</div>
					
     
        
    
    </form>
    <link href="../css/daterangepicker.css" rel="stylesheet" />

  
	    <script src="../js/jquery.min.js" type="text/javascript" ></script>
    <script src="../js/daterangepicker.js" type="text/javascript"></script>
	<script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
    <script src="../js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="../js/jquery.validationEngine.js" type="text/javascript" ></script>

	<script type="text/javascript">

        $(document).ready(function () {


            
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            $("#frmGoldRate").validationEngine('attach');
            $("#frmGoldRate").validationEngine({ promptPosition: "topleft", scroll: false, focusFirstField: true, showArrow: true });
            $("#frmGoldRate").validationEngine('attach', { promptPosition: "topleft", scroll: false,showArrow:true });

            // Place here the first init of the autocomplete
            InitAutoCompl();
            $('#smarttab').smartTab({
                selected: 0, // Initial selected tab, 0 = first tab
               // theme: 'dark', // theme for the tab, related css need to include for other than default theme
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
        });

        function InitializeRequest(sender, args) {
        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            InitAutoCompl();
        }

        function InitAutoCompl() {

            $("#<%=txtfrm.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,    
                showDropdowns: false,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=txtfrm.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

        }

        function DateFormat(field, rules, i, options) {
            var regex = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
            if (!regex.test(field.val())) {
                return "Please enter date in dd/MM/yyyy format."
            }
        };

        function chkdecim(field, rules, i, options) {
            var regex = /^\d+\.?\d{0,2}$/;
            if (!regex.test(field.val())) {
                return "Please enter a Valid Number."
            }
        }

        function detach() {
            $("#frmgoldrate").validationEngine('detach');

        }


        

    </script>

</asp:Content>
