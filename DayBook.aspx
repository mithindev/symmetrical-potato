<%@ Page Title="DayBook" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="DayBook.aspx.vb" Inherits="Fiscus.DayBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/daterangepicker.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <form id="daybook" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Books</a></li>
						<li class="breadcrumb-item active" >Day Book</li>
					</ol>
				</nav>

          <div class="card card-body ">
              
                                               <div class="form-group row  border-bottom">

                                                   
                                                       <label  class=" col-form-label text-primary  ">From&nbsp;&nbsp;</label>
                                                       <asp:TextBox ID="txtfrm" CssClass="col-sm-2 form-control "   runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <label  class=" col-form-label text-primary  ">&nbsp;&nbsp;To</label>
                                                   
                                                       <div class="col-sm-3">
                                                           <div class="input-group">
                                                        <asp:TextBox ID="txtto" CssClass=" form-control "  runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_inw"  CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton runat="server" id="btnprnt" CssClass ="btn btn-outline-primary " style="margin-left: 2px;" OnClientClick="document.forms[0].target = '_blank'; setTimeout(function(){document.forms[0].target='';}, 100);">
                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-printer"><polyline points="6 9 6 2 18 2 18 9"></polyline><path d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2"></path><rect x="6" y="14" width="12" height="8"></rect></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                           </div>
            
                                                   </div>
                      </div>

               
                                   
              
                            <asp:Panel ID="pnlcb" runat="server" Visible="false" >

                                    
                                 <table style="width:100%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>
                                         <tr>
                                         <th style="width:12%;text-align:center; ">Date</th>
                                             <th style="width:8%;text-align:center;">T.ID</th>
                                            <th style="width:40%;text-align:center;">Account Head</th>
                                      <th style="width:20%;text-align:center;">Debit</th>
                                         <th style="width:20%;text-align:center;">Credit</th>
                                                                                  </tr>
                                      </thead>
                                  </table> 

                                      <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-top:-1px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging ="OnPaging" PageSize ="40" Width="100%"  
                               Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "   >
                                <Columns>
                                    <asp:TemplateField >
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dt" runat="server" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center ;font-size:small"></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="12%" />
                                                                                </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                  
                                    
                                    <asp:Label ID="lbl_tid" runat="server" text='<%#Eval("transid")%>' Style="text-align:left ;float:left; font-size:small   "></asp:Label>
                                        <asp:LinkButton runat="server" ID="lnkbrkup" Text="  >" ></asp:LinkButton>
                                    </ItemTemplate>
                                        <ItemStyle Width ="8%" />

                                        </asp:TemplateField> 
                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_typ_supl" runat="server" Width="100%" text='<%#Eval("achead")%>' Style="text-align:left ;float:left;  "></asp:Label>
                                    <asp:Label ID="lbl_nar" runat="server" text='<%#Eval("nar")%>' Style="text-align:left ;float:left;margin-top:5px;font-size:smaller   "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="40%" Height="20px" />



                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dr" runat="server" text='<%#Eval("DR", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="20%" />
                                     
                                   
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cr" runat="server" text='<%#Eval("CR", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="20%" />

                                       
                                    </asp:TemplateField>

                                   
                                    

                                </Columns>

                                </asp:GridView> 

                            <table class="table" style="border:none ;width:100%;margin-left:0px;float:left " >
                                  <tbody>
                                         <tr>
                                         <td style="width:60%;text-align:right ;border:none ;">Opening Balance</td>
                                         <td style="width:20%;text-align:center;border:none ">
                                             <asp:Label ID="lbl_opening_debit" Style="text-align:right;float:right;border:none " CssClass="col-form-label text-danger " runat ="server" ></asp:Label>
                                                                                    </td>
                                                                                
                                             <td style="width:20%;text-align:right ;border:none">
                                                 <asp:Label ID="lbl_opening_credit" Style="text-align:right;float:right " CssClass="col-form-label text-success " runat ="server" ></asp:Label>
                                              </td>
                                         </tr>

                                         <tr >
                                         <td style="width:60%;text-align:right ;border:none"> Total</td>
                                         <td style="width:20%;text-align:right ;border:none">
                                             <asp:Label ID="lbl_sum_debit" Style="text-align:right;float:right " CssClass="col-form-label " runat ="server" ></asp:Label>
                                                                                     </td>
                                         
                                             <td style="width:20%;text-align:right ;float:right;border:none ">
                                                 <asp:Label ID="lbl_sum_credit" Style="text-align:right;float:right " CssClass="col-form-label " runat ="server" ></asp:Label>
                                              </td>
                                         </tr>

                                         <tr >
                                         <td style="width:60%;text-align:right ;border:none">Closing Balance</td>
                                         <td style="width:20%;text-align:right ;border:none">
                                             <asp:Label ID="lbl_closing_debit" Style="text-align:right;float:right " CssClass="col-form-label text-danger " runat ="server" ></asp:Label>
                                                                                     </td>
                                        
                                         
                                             <td style="width:20%;text-align:center;border:none">
                                                 <asp:Label ID="lbl_closing_credit" CssClass="col-form-label text-success " Style="text-align:right;float:right " runat ="server" ></asp:Label>
                                              </td>
                                         </tr>


                                      </tbody>
                                  </table> 



               
                                </asp:Panel>
                                                          

              </div>
         
         <div class="modal" tabindex="-1" role="dialog" id="knlmodal"  data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-xl " role="document"  style="overflow-y: initial !important">
                <div class="modal-content">
                    <div class="modal-body" style=" height: 80vh;overflow-y: auto;">

                        <div style="overflow:auto !important">
                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>
                                <asp:Repeater ID="rpknl" runat="server" >
                                    <HeaderTemplate>

                                 
                       <table class="table table-bordered">
                           <thead>
                               <tr>
                                   <th>Account No</th>
                                   <th>Member No</th>
                                   <th>Customer Name</th>
                                   <th>Debit</th>
                                   <th>Credit</th>
                                </tr>
                           </thead>
                           
                           </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>
                                          <td><asp:Label runat="server" ID="lblacn"  text='<%#Eval("acno")%>'></asp:Label></td>
                                          <td><asp:Label runat="server" ID="lblmem"  text='<%#Eval("cid")%>'></asp:Label></td>
                                          <td><asp:Label runat="server" ID="lblname" text='<%#Eval("firstname")%>'></asp:Label></td>
                                          <td><asp:Label runat="server" ID="lbldr" Style="text-align:right;float:right " text='<%#Eval("debit", "{0:#,##,###.00}")%>'></asp:Label></td>
                                          <td><asp:Label runat="server" ID="lblcr" Style="text-align:right;float:right "  text='<%#Eval("credit", "{0:#,##,###.00}")%>'></asp:Label></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                       </table> 
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                            </div>


                        <div class="form-group row border-top-primary">
                            <div class="col-md-6">

                            </div>
                            <a class="btn btn-outline-dark" href="#" onclick="hidebrkup();">Close</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>


     
             
         </form>

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


            
            




        function hidebrkup() {
            $('#knlmodal').toggle();
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
        }

        


    </script>
</asp:Content>
