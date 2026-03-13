<%@ Page Title="Balance Sheet" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="BalanceSheet.aspx.vb" Inherits="Fiscus.BalanceSheet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/daterangepicker.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="bs" runat="server">

        <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

        <nav class="page-breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Financial Reports</a></li>
                <li class="breadcrumb-item active">Balance Sheet</li>
            </ol>
        </nav>

          <asp:UpdatePanel runat="server" >
              <ContentTemplate>
                  <div class="card card-body ">
                      <h6 class="card-title text-primary ">Balance Sheet</h6>
                  

                                                <asp:UpdatePanel runat="server" ID="pnlbtn">
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
                                                                     		<div class="col-sm-2">
                                            <div class="form-check form-check-inline">
											<label class="form-check-label">
												<asp:CheckBox  runat="server"  ID="ndh3"   />
												NDH-3
											<i class="input-frame"></i></label>
										</div>
					   </div>
                      </div>

                                                        </ContentTemplate>
                                                </asp:UpdatePanel>
                  
                      <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlbtn">
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
<br/>
            Processing.....
        </div>
    </div>
</ProgressTemplate>
</asp:UpdateProgress>
                      
                                <table style="width:100%;"  class="table table-bordered ">
                                  <thead>
                                         <tr>
                                            <th style="width:30%;">Liabilities</th>
                                         <th style="width:20%;">credit</th>
                                            <th style="width:30%;">Assets</th>
                                         <th style="width:20%;">Debit</th>
                                                                                  </tr>
                                      </thead>
                                  </table> 
                     

                                    <div >
                                      <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="display:inline-block;float:left" 
                            AllowPaging ="true" OnPageIndexChanging ="OnPaging" PageSize ="40"  Width="50%"   EnableViewState="false" EmptyDataText="No Records Found"
                               Visible="true" ShowHeaderWhenEmpty="true" ShowHeader="false"  CssClass="table table-bordered"   >
                                <Columns>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_typ_supl" runat="server" Width="100%" text='<%#Eval("achead")%>' Style="text-align:left ;float:left;font-size:smaller  "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="30%" />



                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dr" runat="server" text='<%#Eval("credit", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="20%" />
                                     
                                   
                                    </asp:TemplateField>


                                   
                                    

                                </Columns>

                                </asp:GridView> 


                                     <asp:GridView  runat="server" ID="disp_debit" AutoGenerateColumns="false" style="float:left ;" 
                            AllowPaging ="true" OnPageIndexChanging ="OnPagingdebit" PageSize ="40" Width="50%"  EmptyDataText="NO Data"
                               Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  " >
                                <Columns>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_typ_supl" runat="server" Width="100%" text='<%#Eval("achead")%>' Style="text-align:left ;float:left;font-size:smaller  "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="30%" />



                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dr" runat="server" text='<%#Eval("debit", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="20%" />
                                     
                                   
                                    </asp:TemplateField>


                                   
                                    

                                </Columns>

                                </asp:GridView> 

                                        </div>
                                                             <asp:UpdatePanel runat ="server">
                        <ContentTemplate>                            <table style="width:100%;" class ="table table-bordered  ">

                                                 <tbody>
                                                     <tr >
                                                         <td style="width:30%">Total</td>
                                                       
                                                         <td style ="width:20%">
                                                             
                                                         <asp:Label ID="lblcr" runat="server" Text="0" Style="text-align:right;float:right "></asp:Label></td>
                                                         <td style ="width:30%"></td> 

                                                         <td style ="width:10%">
                                                             <asp:Label ID="lbldr" runat="server" Style="text-align:right;float:right "></asp:Label>
                                                         </td>
                                                     </tr>
                                                 </tbody>
                                             </table>
                            </ContentTemplate>

                        </asp:UpdatePanel>

                  </div>

                  <asp:Panel Visible="false"  runat="server" >
                       <asp:UpdatePanel id="updatepanel3" runat="server" >
                            <ContentTemplate>
                                                                                                                  <div class="input-daterange input-group" id="datepicker1">
                                                             <span class="input-group-addon" style="margin-left:5px;width:50px; height: 32px">From</span>
                                                             <asp:TextBox ID="txtfrm1" style="margin-left:-7px;"   CssClass="datepicker  form-control " runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                             <span class="input-group-addon" style="width: 40px; height: 32px">To</span>
                                                             <asp:TextBox ID="txtto1" CssClass="datepicker form-control  "   Style="margin-left: -7px" runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                                                  <span class ="input-group-btn " >
    <asp:ImageButton runat="server" style="margin-left:0px" Width ="45px" Height="34px"  ImageUrl="../images/search_logo.png" id="btn_show" CssClass="btn btn-outline-secondary  "   />
                                                                                      <asp:ImageButton runat="server" style="margin-left:0px" Width ="45px" Height="34px"  ImageUrl="~/Images/sms.png"  id="btn_exp" CssClass="btn btn-outline-secondary  "   />
                                                                                      </span>
                                                         </div>

                            </ContentTemplate>
                         </asp:UpdatePanel>
                                                                                                              
                                        
                                               <asp:updatePanel ID="UpdatePanel2" runat="server" Visible="true" >
                                <ContentTemplate >

                                     <section id="content1">
                                 <table style="width:100%;"  class="table table-bordered">
                                  <tbody>
                                         <tr>
                                            <td style="width:40%;">AC HEAD</td>
                                         <td style="width:15%;">Opening</td>
                                             <td style="width:15%;">Debit</td>
                                             <td style="width:15%;">Credit</td>
                                             <td style="width:15%;">Closing</td>
                                                                                  </tr>
                                      </tbody>
                                  </table> 
               



                                      <asp:GridView  runat="server" ID="disp_ndh3" AutoGenerateColumns="false" style="margin-top:-0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging ="OnPaging" PageSize ="100" Width="100%"   EnableViewState="false" EmptyDataText="No Records Found"
                               Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="customers " AlternatingRowStyle-CssClass="customers alt"   >
                                <Columns>
                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_typ_supl" runat="server" Width="100%" text='<%#Eval("achead")%>' Style="text-align:left ;float:left;font-size:smaller  "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="40%" />

                                   </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_ob" runat="server" text='<%#Eval("ob", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="15%" />
                                     
                                   
                                    </asp:TemplateField>


                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dr" runat="server" text='<%#Eval("debit", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="15%" />
                                     
                                   
                                    </asp:TemplateField>

                                   <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cr" runat="server" text='<%#Eval("credit", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="15%" />
                                     
                                   
                                    </asp:TemplateField>

                                    
                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cb" runat="server" text='<%#Eval("cb", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="15%" />
                                     
                                   
                                    </asp:TemplateField>


                                </Columns>

                                </asp:GridView> 


                        


                                                             <asp:UpdatePanel runat ="server" Visible="false" >
                        <ContentTemplate>                            <table style="width:100%;" class ="table table-bordered ">

                                                 <tbody>
                                                     <tr >
                                                         <td style="width:40%">Total</td>
                                                       
                                                         <td style ="width:15%">
                                                             
                                                         <asp:Label ID="Label1" runat="server" Text="0" Style="text-align:right;float:right "></asp:Label></td>
                                                         <td style ="width:40%"></td> 

                                                         <td style ="width:20%">
                                                             <asp:Label ID="Label2" runat="server" Style="text-align:right;float:right "></asp:Label>
                                                         </td>
                                                     </tr>
                                                 </tbody>
                                             </table>
                            </ContentTemplate>

                        </asp:UpdatePanel>


              

                  </ContentTemplate>
              </asp:UpdatePanel>

                      </asp:Panel>
                  </ContentTemplate>
              </asp:UpdatePanel>
        </form>

    
   
	
    <script src="../js/daterangepicker.js" type="text/javascript" ></script>

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
