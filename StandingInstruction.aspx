<%@ Page Title="Standing Instruction" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="StandingInstruction.aspx.vb" Inherits="Fiscus.StandingInstruction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="pettycash" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Master</a></li>
						<li class="breadcrumb-item active" >Standing Instruction</li>
					</ol>
				</nav>
	
			

    
    
        		<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								    <div id="smarttab">
                                        <ul class="nav" id="tabitm"  >
           <li id="cTab">
               <a class="nav-link" href="#tab-1">
                    Standing Instruction
                   </a>
           </li>
           <li id="jTab">
               <a class="nav-link" href="#tab-2">
                  Register
               </a>
           </li>
            
    </ul>
                                       
										<div class="tab-content">
                                           <div id="tab-1" class="tab-pane" role="tabpanel">

                                               <asp:UpdatePanel runat="server" >
                                                   <ContentTemplate>

                                                   
                                               <div class="card card-body ">

                                                   <div class="form-group row ">
                                                       <label class="col-form-label col-sm-2">Deposit Account No</label>
                                                       <div class="col-sm-3">

                                                            <asp:TextBox  ID="txtsiacn"  runat="server" CssClass="form-control"  AutoPostBack="True"></asp:TextBox>
                               
                                                       </div>
                                                   </div>

                                                        <div class="form-group row ">
                                                       <label class="col-form-label col-sm-2">Runs On</label>
                                                       <div class="col-sm-3">

                                                            <asp:TextBox  ID="txtday"  runat="server" CssClass="form-control"  AutoPostBack="True"></asp:TextBox>
                               
                                                       </div>
                                                   </div>
        
                                                   <div class="form-group row ">
                                                       <label class="col-form-label col-sm-2">Amount</label>
                                                       <div class="col-sm-3">

                                                            <asp:TextBox  ID="txtamt"  runat="server" CssClass="form-control"  AutoPostBack="True"></asp:TextBox>
                               
                                                       </div>
                                                   </div>

                                                   <div class="form-group row ">
                                                       <label class="col-form-label col-sm-2">SB Account No</label>
                                                       <div class="col-sm-3">

                                                            <asp:TextBox  ID="txtsbacn"  runat="server" CssClass="form-control"  AutoPostBack="True"></asp:TextBox>
                               
                                                       </div>
                                                   </div>

                                                   <div class="form-group row  ">
                                                       <label class="col-form-label col-sm-2" >Customer Name</label>
                                                        <div class="col-sm-3">
                                        <asp:Label ID="cust" runat ="server"></asp:Label>

                                                                   </div>
                                                   </div>

                                                   <div class="form-group row border-top "></div>
                                                   <div class="col-md-4"></div>
                                                   <div class="col-md-4">
                                                       <asp:Button ID="btn_si_upclr" CssClass="btn btn-outline-secondary   small" runat="server" Text="Clear" CausesValidation="False" />
                <asp:Button ID="btn_si_update" CssClass="btn btn-outline-primary small" runat="server" Text="Save" />
                <asp:Button ID="btn_si_can" CssClass="btn btn-outline-danger  small" runat="server" Text="Cancel"  Visible="false" />
                                                   </div>
                                                   </div>
                                                       </ContentTemplate>
                                               </asp:UpdatePanel>
                                               </div>

                                            <div id="tab-2" class="tab-pane" role="tabpanel" >

                                                <div class ="card card-body">
                                                       
                                                    <asp:UpdatePanel runat="server" >
                                                        <ContentTemplate>

                                          
                                                      <div class="form-group row ">
                                                    <label class="col-sm-1 col-form-label ">Deposit</label>
                                                    <div class="col-sm-2">
                                                                
                                                         <asp:TextBox ID="txtpro" style="margin-left:-7px"   CssClass="form-control " runat="server" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                    </div>
                                                    <label class="col-sm-1 col-form-label ">Day</label>
                                                    <div class="col-sm-3">
                                                        <div class="input-group ">
                                                         <asp:TextBox ID="txtdayd" CssClass="form-control  "   Style="margin-left: -7px" runat="server" Height="37px" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                            <span class="input-group-btn input-group-append">
                                                                <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                                
                                                                
                                                            </span>
                                                            </div>
                                                    </div>
                                                    
                                                </div>
                                                                                               <div>
                              <table style="border:1px;width:90%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>
                                      
                                         
                                             <th style="width:10%;text-align:center;font-variant:small-caps ">S.No</th>
                                         <th style="width:10%;text-align:center;">Runs On</th>
                                         <th style="width:15%;text-align:center;">Transfer From</th>
                                         <th style="width:15%;text-align:center;">Transfer To</th>
                                         <th style="width:20%;text-align:center;">Amount</th>
                                         
                                      </thead>
                                  </table>

                        <asp:GridView  runat="server" ID="gvin" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="50" EmptyDataText="No Data Found" 
                             Width="90%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>

                                <asp:TemplateField HeaderText="S No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="10%" />
            </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("day", "{0:##}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("frm")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("to")%>' Style="text-align:right  ;float:right "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("amt", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>

                                                        </Columns>
                            </asp:GridView>


                                                                                </div>

                                                            
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


        <script src="../js/jquery.js" type="text/javascript"></script>
  <script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
  
	<script type="text/javascript">




        $(document).ready(function () {



            
            
        $('#smarttab').smartTab({
            selected: 0, // Initial selected tab, 0 = first tab
          //  theme: 'dark', // theme for the tab, related css need to include for other than default theme
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
                keyNavigation: true, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
                keyLeft: [37], // Left key code
                keyRight: [39] // Right key code
            }
        });


           

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        // Place here the first init of the autocomplete
            InitAutoComplacn();
            InitAutoComplSB();

    });


        function InitializeRequest(sender, args) {
        }





        

 

    

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            
            InitAutoComplacn();
            InitAutoComplSB();

            
            
        }


       
        function InitAutoComplSB() {
            $("#<%=txtsiacn.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/membersearch.asmx/GetDeposit") %>',
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
                                    image: item.ImageURL
                                }
                            }))
                        }
                    });
                },
                minLength: 1,
                focus: function (event, ui) {

                    $("#<%=txtsiacn.ClientID %>").val(ui.item.memberno);
                    return false;

                },
                open: function () {
                    $("ul.ui-menu").width($(this).innerWidth() + 210);

                },

				select: function (event, ui) {

                    $("#<%=txtsiacn.ClientID %>").val(ui.item.memberno);
                    $("#<%=cust.ClientID %>").text(ui.item.firstname);


                    return false;
                }
            })
                .autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>")
                        .append("<div style='container'>")
                        .append("<div class='img-div column'>")
                        .append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
                        .append("</div")
                        .append("<div class='info-div column'>")
                        .append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
                        .append("<div class='userinfo'>" + item.lastname + "</div>")
                        .append("<div class='userinfo'>" + item.address + "</div>")
                        .append("</br>")
                        .append("</div")
                        .append("</div>")
                        .appendTo(ul);
                };
        }

      

        function InitAutoComplacn() {

       

            $("#<%=txtsbacn.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/membersearch.asmx/GetSbAc") %>',
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
                                    product: item.Product
								}
							}))
						}
					});
				},
                minLength: 1,
                focus: function (event, ui) {

                    $("#<%=txtsbacn.ClientID %>").val(ui.item.memberno);
                    return false;

                },
                open: function () {
                    $("ul.ui-menu").width($(this).innerWidth() + 180);

                },

				select: function (event, ui) {

                    $("#<%=txtsbacn.ClientID %>").val(ui.item.memberno);
                    $("#<%=cust.ClientID %>").text(ui.item.firstname);



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
                            .append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
                            .append("<div class='userinfo'>" + item.lastname + "<div class='product text-muted'>" + item.product + "</div></div>")
                            .append("<div class='userinfo'>" + item.address + "</div>")
                            .append("</br>")
                            .append("</div")
                            .append("</div>")
                            .appendTo(ul);
                    };
        }

        

    </script>

    
    <style>


        .product {
			z-index: 10;  
			position: absolute;  
			right: 10px;  
			top: 21px;
			font-size:12px;
			
	    }
	    .memno {
			z-index: 10;  
			position: absolute;  
			right: 10px;  
			top: 10px;
			font-size:12px;
			color:#000;
	    }

		.img-div{
			display:inline-block;
			vertical-align:top;
		}
		.info-div{
			display:inline-block;
		}

        .username
        {
            display: inline-block;
            font-weight: bold;
            margin-bottom: 0em;
        }
        .userimage
        {
            float: left;
            max-height: 48px;
            max-width: 48px;
            margin-right:10px;
        }
        .userinfo
        {

            margin: 0px;
            padding: 0px;
			font-size:10px;
        }
    </style>
    

</asp:Content>
