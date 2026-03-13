<%@ Page Title="Journal Book" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="JournalBook.aspx.vb" Inherits="Fiscus.JournalBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/daterangepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <form id="journal" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Books</a></li>
						<li class="breadcrumb-item active" >Journal</li>
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
                       <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                           </div>
            
                                                   </div>
                      </div>

               
                                   
              
                            <asp:Panel ID="pnlcb" runat="server" Visible="true" >

                                    
                                 <table style="width:100%;margin-left:0px;float:left "  class="table table-bordered  ">
                                  <thead>
                                         <tr>
                                         <th style="width:15%;">Date</th>
                                             
                                            <th style="width:45%;">Account Head</th>
                                      <th style="width:20%;">Debit</th>
                                         <th style="width:20%;">Credit</th>
                                                                                  </tr>
                                      </thead>
                                  </table> 

                                      <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-left:0px;" 
                            AllowPaging ="true" OnPageIndexChanging ="OnPaging" PageSize ="40" Width="100%"  
                               Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "   >
                                <Columns>
                                    <asp:TemplateField >
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dt" runat="server" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center ;font-size:small"></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="15%" />
                                                                                </asp:TemplateField>

                               
                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_typ_supl" runat="server" Width="100%" text='<%#Eval("achead")%>' Style="text-align:left ;float:left;  "></asp:Label>
                                    <asp:Label ID="lbl_nar" runat="server" text='<%#Eval("nar")%>' Style="text-align:left ;float:left;margin-top:2px;font-size:smaller   "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="45%" Height="20px" />



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

                            <table class="table table-bordered" style="border:none ;width:100%;margin-left:0px;float:left " >
                                  <tbody>
                                     

                                         <tr >
                                         <td style="width:60%;text-align:right ;border:none"> Total</td>
                                         <td style="width:20%;text-align:right ;border:none">
                                             <asp:Label ID="lbl_sum_debit" Style="text-align:right;float:right " CssClass="col-form-label " runat ="server" ></asp:Label>
                                                                                     </td>
                                         
                                             <td style="width:20%;text-align:right ;float:right;border:none ">
                                                 <asp:Label ID="lbl_sum_credit" Style="text-align:right;float:right " CssClass="col-form-label " runat ="server" ></asp:Label>
                                              </td>
                                         </tr>

                                   


                                      </tbody>
                                  </table> 



               
                                </asp:Panel>
                                                          

              </div>
         

     
             
         </form>

    
	
    <script src="js/daterangepicker.js" type="text/javascript"></script>

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
