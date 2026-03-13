<%@ Page Title="Stock Book" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="StockBook.aspx.vb" Inherits="Fiscus.StockBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../css/daterangepicker.css" />
    <style>
          @media print {

            .reportheader{
                display:block;
            }
        }
            

            .reportheader{
                display:none;
            }

    </style>
    <script src="../js/jquery.js" type="text/javascript"></script>
     <script src="../js/printThis.js" type="text/javascript"></script>

    <script type="text/javascript">
        function doprnt(br, rpthead) {
            var rh = "<h4>Karavilai Nidhi Limited</h4><span>Branch:" + br + "</span><span>" + rpthead + "</span>"

            $('#vouchprint').printThis({
                importCSS: true,
                importStyle: true,         // import style tags
                printContainer: true,
                header: rh
            });

        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="stockbook" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Books</a></li>
						<li class="breadcrumb-item active" >Stock Book</li>
					</ol>
				</nav>

          <div class="card card-body ">
              <div class="d-flex justify-content-between align-items-baseline">
                      <h6 class="card-title mb-0">Stock Book</h6>
                      <div class="dropdown mb-2">
                        <button class="btn p-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-more-horizontal icon-lg text-muted pb-3px"><circle cx="12" cy="12" r="1"></circle><circle cx="19" cy="12" r="1"></circle><circle cx="5" cy="12" r="1"></circle></svg>
                        </button>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton" style="">
                          <asp:LinkButton ID="btnprnt" runat="server" class="dropdown-item d-flex align-items-center"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-printer icon-sm mr-2"><polyline points="6 9 6 2 18 2 18 9"></polyline><path d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2"></path><rect x="6" y="14" width="12" height="8"></rect></svg> <span class="">Print</span></asp:LinkButton>
                          <asp:LinkButton ID="btnxl" runat="server" class="dropdown-item d-flex align-items-center"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-download icon-sm mr-2"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path><polyline points="7 10 12 15 17 10"></polyline><line x1="12" y1="15" x2="12" y2="3"></line></svg> <span class="">Download</span></asp:LinkButton>
                        </div>
                      </div>
                    </div>
              <div id="smarttab">
                                        <ul class="nav" id="tabitm"  >
           <li id="cTab">
               <a class="nav-link" href="#tab-1">
                   Stock
                   </a>
           </li>
           <li id="jTab">
               <a class="nav-link" href="#tab-2">
                  Inward
               </a>
           </li>
                                            <li id="oTab">
               <a class="nav-link" href="#tab-3">
                  Outward
               </a>
           </li>
          
    </ul>
                                       
										<div class="tab-content">
                                           <div id="tab-1" class="tab-pane" role="tabpanel">

                                               <div class="card-body ">

                                                   <div id="vouchprint">
                                                       <asp:UpdatePanel runat="server" >
                                                           <ContentTemplate>

                                                           
                                                          
                              <table style="border:0px;width:90%;margin-left:0px;float:left "  class="table table-bordered  ">
                                  <thead>

                                         <tr>
                                         <th style="width:10%;text-align:center;color:white;">S.No</th>
                                         <th style="width:15%;text-align:center;color:white; ">Date</th>
                                         <th style="width:15%;text-align:center;color:white;">Loan No</th>
                                         <th style="width:10%;text-align:center;color:white;">Qty</th>
                                         <th style="width:20%;text-align:center;color:white;">Weight</th>
                                         <th style="width:20%;text-align:center;color:white;">Total</th>
                                         </tr>
                                      </thead>
                                  </table>

                       <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="50" OnRowDataBound ="disp_RowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" 
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
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acn")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("tqty")%>' Style="text-align:right  ;float:right "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("tnet")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("total")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>
                                 </Columns>
                            </asp:GridView>
                                                               </ContentTemplate>
                                                       </asp:UpdatePanel>

                                                                        </div>        
           
                                               </div>

                                                <div class="form-group row  border-bottom">
      
                  </div>
                                               </div>
                                            <div id="tab-2" class="tab-pane " role="tabpanel" >

                                                                    <div class="card-body ">

                                                                        <asp:UpdatePanel runat="server" >
                                                                            <ContentTemplate>

                                                      <div class="form-group row  border-bottom">

                                                   
                                                       <label  class=" col-form-label text-primary  ">From&nbsp;&nbsp;</label>
                                                       <asp:TextBox ID="txtfrm" CssClass="col-sm-2 form-control "   runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <label  class=" col-form-label text-primary  ">&nbsp;&nbsp;To</label>
                                                   
                                                       <div class="col-sm-3">
                                                           <div class="input-group">
                                                        <asp:TextBox ID="txtto" CssClass=" form-control "  runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                           </div>
            
                                                   </div>
                                                   

                                                        
                                                        
                
               
                
                                                         

                                               
                      </div>

                                                          
                                                  <table style="border:0px;width:90%;margin-left:0px;float:left "  class="table table-bordered  ">
                                  <thead>

                                         <tr>
                                             <th style="width:10%;text-align:center;color:white;">S.No</th>
                                         <th style="width:15%;text-align:center;color:white; ">Date</th>
                                         <th style="width:15%;text-align:center;color:white;">Loan No</th>
                                         <th style="width:10%;text-align:center;color:white;">Qty</th>
                                         <th style="width:20%;text-align:center;color:white;">Weight</th>
                                             <th style="width:20%;text-align:center;color:white;">Total</th>
                                         </tr>
                                      </thead>
                                  </table>


                                                                         <asp:GridView  runat="server" ID="gvin" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingin" PageSize="50" EmptyDataText="No Data Found" OnRowDataBound="gvin_RowDataBound"  OnSelectedIndexChanged="OnSelectedIndexChangedgvin" 
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
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acn")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("tqty")%>' Style="text-align:right  ;float:right "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("tnet")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("total")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>
                                 </Columns>
                            </asp:GridView>

                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>


</div>
                                            </div>

                                            <div id="tab-3" class="tab-pane" role="tabpanel">

                                                                    <div class="card-body ">

                                                                        <asp:UpdatePanel runat="server" >
                                                                            <ContentTemplate>

                                                                            

                                                                                                <div class="form-group row  border-bottom">

                                                   
                                                       <label  class=" col-form-label text-primary  ">From&nbsp;&nbsp;</label>
                                                       <asp:TextBox ID="txtfrmout" CssClass="col-sm-2 form-control "   runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <label  class=" col-form-label text-primary  ">&nbsp;&nbsp;To</label>
                                                   
                                                       <div class="col-sm-3">
                                                           <div class="input-group">
                                                        <asp:TextBox ID="txttoout" CssClass=" form-control "  runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="srch_out" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                           </div>
            
                                                   </div>
                                                   

                                                        
                                                        
                
               
                
                                                         

                                               
                      </div>


                                                          
                              <table style="border:0px;width:90%;margin-left:0px;float:left "  class="table table-bordered  ">
                                  <thead>

                                         <tr>
                                             <th style="width:10%;text-align:center;color:white;">S.No</th>
                                         <th style="width:15%;text-align:center;color:white; ">Date</th>
                                         <th style="width:15%;text-align:center;color:white;">Loan No</th>
                                         <th style="width:10%;text-align:center;color:white;">Qty</th>
                                         <th style="width:20%;text-align:center;color:white;">Weight</th>
                                             <th style="width:20%;text-align:center;color:white;">Total</th>
                                         </tr>
                                      </thead>
                                  </table>

                <asp:GridView  runat="server" ID="gvout" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingout" PageSize="50"  EmptyDataText="No Data Found" OnRowDataBound="gvout_RowDataBound" OnSelectedIndexChanged="OnSelectedIndexChangedgvout" 
                             Width="90%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "  >
                            <Columns>

                                <asp:TemplateField HeaderText="S No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="10%" />
            </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acn")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("tqty")%>' Style="text-align:right  ;float:right "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("tnet")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("total")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

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


        <asp:LinkButton ID="lnkdummy1" runat="server" ></asp:LinkButton>        
            
                <asp:Panel  ID="pnldep" runat="server" CssClass="modalPopup" style="display:none"  Width="50%">
                   
                     <div class="msgbox">

               <div style="float:left" >
                    <img src="Images/fiscus.png" style="margin-left:1px;height:35px"  />  
               </div>

                  <div style="float:left;margin-left:10px">
                      <span class="caption ">                        Jewel Details</span>

                    </div>
                            <div style="float:right;margin-right:0px">
                                                  
                    <asp:ImageButton ID="dep_closed" ImageUrl="~/Images/RedCircleX.png" runat="server" Height="30px" Width="30px" />
                            </div>

                         
                         <div class="div-spread " >
                             <table style="margin-left:10px;width:100%" class="customers "> 
                                 <tbody>
                                     <tr style="text-align:center;">
                                     <th style="text-align:center;width:35%;font-variant:small-caps;font-size:medium" >Particulars</th>
                                    <th style="text-align:center;width:15%;font-variant:small-caps;font-size:medium" >Quantity</th>   
                                    <th style="text-align:center;width:25%;font-variant:small-caps;font-size:medium" >Gross Weight</th>
                                     <th style="text-align:center;width:25%;font-variant:small-caps;font-size:medium" >Net Weight</th>
                                         <th></th> 

                                      </tr>
                                                                       </tbody>
                             </table>
                              
                               <asp:GridView  runat="server" ID="dispjl" AutoGenerateColumns="false" style="margin-top:-2px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true"  OnPageIndexChanging="OnPaging" PageSize="15"
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered   "  >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblitm" runat="server" text='<%#Eval("itm")%>' Style="text-align:left  "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                     
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblqty" runat="server" text='<%#Eval("qty")%>' Style="text-align:right;float:right"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                    
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblgross" runat="server" text='<%#Eval("gross")%>' Style="text-align:right;float:right"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblnet" runat="server" text='<%#Eval("net")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>

                   </Columns> 
                                </asp:GridView>

                             
                                 <table style="margin-left:10px;width:100%" class="customers "> 
                                 <tbody>
                                     <tr style="text-align:center;">
                                     <td style="text-align:center;width:35%" >
                                        <p>Total</p>

                                     </td>
                                    <td style="text-align:center;width:15% " >
                                     <asp:Label runat="server" ID="lbltqty" style="float:right;text-align:right " Text="0"></asp:Label>
                                    </td>   
                                    <td style="text-align:right ;width:25% " >
                                        <asp:Label runat="server" ID="lblgross" style="float:right;text-align:right " Text="0"></asp:Label>
                                    </td>
                                     <td style="text-align:center;width:25%" >
                                         <asp:Label runat="server" ID="lblnet" style="float:right;text-align:right " Text="0"></asp:Label>
                                         </td>
                                          

                                            </tr>
                                     </tbody>
                                     </table> 


            

                         </div>                        

                         </div> 
                    </asp:Panel>


        </form>


       
      <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/daterangepicker.js" type="text/javascript" ></script>
	<script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
    

    <script type="text/javascript">


        $(document).ready(function () {

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
                    keyNavigation: true, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
                    keyLeft: [37], // Left key code
                    keyRight: [39] // Right key code
                }
            });

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

            $("#<%=txtfrm.ClientID%>").focus(function () {


                $("#<%=txtfrm.ClientID%>").daterangepicker({
                  "singleDatePicker": true,
                  "startDate": moment(),
                  "endDate": moment(),
                  "autoUpdateInput": true,

                  "autoApply": true,
                  locale: {
                      format: 'DD-MM-YYYY'
                  },
              }, function (start, end, label) {

                  $("#<%=txtfrm.ClientID%>").val(start.format('DD-MM-YYYY'));
                        

                    });
                
              




            });

            $("#<%=txtto.ClientID%>").focus(function () {


                $("#<%=txtto.ClientID%>").daterangepicker({
                        "singleDatePicker": true,
                        "startDate": moment(),
                        "endDate": moment(),
                        "autoUpdateInput": true,

                        "autoApply": true,
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                    }, function (start, end, label) {

                    $("#<%=txtto.ClientID%>").val(start.format('DD-MM-YYYY'));


                });






            });

            
            $("#<%=txtfrmout.ClientID%>").focus(function () {


                $("#<%=txtfrmout.ClientID%>").daterangepicker({
                          "singleDatePicker": true,
                          "startDate": moment(),
                          "endDate": moment(),
                          "autoUpdateInput": true,

                          "autoApply": true,
                          locale: {
                              format: 'DD-MM-YYYY'
                          },
                      }, function (start, end, label) {

                          $("#<%=txtfrmout.ClientID%>").val(start.format('DD-MM-YYYY'));
                        

                    });
                
              




            });

            $("#<%=txttoout.ClientID%>").focus(function () {


                $("#<%=txttoout.ClientID%>").daterangepicker({
                        "singleDatePicker": true,
                        "startDate": moment(),
                        "endDate": moment(),
                        "autoUpdateInput": true,

                        "autoApply": true,
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                    }, function (start, end, label) {

                    $("#<%=txttoout.ClientID%>").val(start.format('DD-MM-YYYY'));


                });






            });

        } 
               

    </script>

</asp:Content>
