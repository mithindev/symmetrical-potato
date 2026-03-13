<%@ Page Title="Interest Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="InterestReport.aspx.vb" Inherits="Fiscus.InterestReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="intreport" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Reports</a></li>
						<li class="breadcrumb-item active" >Interest Report</li>
					</ol>
				</nav>

          <div class="card card-body ">
              
                  
                                           

                                               <div class="form-group row  border-bottom">

                                                   
                                                       <label  class=" col-form-label text-primary  ">Date&nbsp;&nbsp;</label>
                                                       <asp:TextBox ID="txtidate" CssClass="col-sm-2 form-control "   runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <label  class=" col-form-label text-primary  ">&nbsp;&nbsp;Product</label>
                                                   
                                                       <div class="col-sm-4">
                                                           <div class="input-group">
                                                        <asp:TextBox ID="txtproduct" CssClass=" form-control "  runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_new" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                               <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_cv" CssClass ="btn btn-outline-primary ">
                           <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-refresh-cw"><polyline points="23 4 23 10 17 10"></polyline><polyline points="1 20 1 14 7 14"></polyline><path d="M3.51 9a9 9 0 0 1 14.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0 0 20.49 15"></path></svg>
                                                                    
                                                                </asp:LinkButton>
                                                          </span>
                                                           </div>
            
                                                   </div>
                      </div>


               <table style="margin-left:0px;width:100%" class="table table-bordered"> 
                                 <thead>
                                     
                                     <th style="width:15%;" >Date</th>
                                        <th style="text-align:center;width:15%;" >Account No</th>  
                                        <th style="text-align:center;width:28%;" >Customer Name</th>  
                                        
                                    <th style="text-align:center;width:15%;" >Amount</th>
                                     <th style="text-align:center;width:13%;" >Period</th>
                                     <th style="text-align:center;width:13%;" >ROI</th>
                                     <th style="text-align:center;width:15%;" >Interest</th> 
                                     <th style="text-align:center;width:15%;" runat="server" id="damt_header" >By Cash</th> 
                                            
                        
                                    </thead>
                             </table>
                             
                                 <asp:UpdatePanel ID="jspc" runat="server">
                                 <ContentTemplate >
                             
                               <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-top:-2px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true"  OnPageIndexChanging="OnPaging" PageSize="100"
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblitm" runat="server" text='<%#Eval("acdate", "{0:dd-MM-yyyy}")%>' Style="text-align:center   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                     
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblacn" runat="server" text='<%#Eval("acn")%>' Style="text-align:left ;float:left "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblfn" runat="server" text='<%#Eval("FirstName")%>' Style="text-align:left ;float:left "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="28%" />
                                </asp:TemplateField>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblgross" runat="server" text='<%#Eval("amt", "{0:N}")%>' Style="text-align:right;float:right"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lbliprd" runat="server" text='<%#Eval("prd")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="13%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblicint" runat="server" text='<%#Eval("cint", "{0:N}")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="13%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblicamt" runat="server" text='<%#Eval("camt", "{0:N}")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                         

                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                  <asp:Label ID="lblidamt" runat="server" text='<%#Eval("damt", "{0:N}")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>



                   </Columns> 
                                </asp:GridView>
                                 
                                     
                                      </ContentTemplate>
                                 </asp:UpdatePanel>

                                 <table style="margin-left:0px;width:100%" class="table table-condensed  "> 
                                 <tbody>
                                     <tr style="text-align:center;">
                                     <td style="text-align:center;width:88%" >
                                        <p>Total</p>

                                     </td>
                                      <td style="text-align:center;width:15%" >
                                          <asp:UpdatePanel runat="server" >
                                              <ContentTemplate>
                                         <asp:Label runat="server" ID="lblnet" style="float:right;text-align:right " Text="0"></asp:Label>
                                                  </ContentTemplate>
                                              </asp:UpdatePanel>
                                         </td>
                                        
                                            <td style="text-align:center;width:15%"  id="damt_foot" runat="server" >
                                          <asp:UpdatePanel runat="server" >
                                              <ContentTemplate>
                                         <asp:Label runat="server" ID="lbldnet" style="float:right;text-align:right " Text="0"></asp:Label>
                                                  </ContentTemplate>
                                              </asp:UpdatePanel>
                                         </td>

                                            </tr>
                                     </tbody>
                                     </table> 

              </div>
        </form>

    
      <script src="../js/jquery.js" type="text/javascript"></script>
    
	<script src="js/daterangepicker.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="css/daterangepicker.css" />
    

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


            $("#<%=txtidate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=txtidate.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

          
        }



    </script>


</asp:Content>
