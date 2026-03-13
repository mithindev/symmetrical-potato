<%@ Page Title="Deposit Analysis" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="DepositAnalysis.aspx.vb" Inherits="Fiscus.DepositAnalysis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/daterangepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="despanalysis" runat="server">

        <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

        <nav class="page-breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Reports</a></li>
                <li class="breadcrumb-item active">Deposit Analysis</li>
            </ol>
        </nav>

        <div class="card card-body ">
            <div id="smarttab">
                <ul class="nav" id="tabitm">
                    <li>
                        <a class="nav-link" href="#tab-1">Maturity Analysis
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#tab-2">RD Over Due
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#tab-3">Minimum Balance
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#tab-4">New Account
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#tab-5">Closed Account
                        </a>
                    </li>
                </ul>

                <div class="tab-content">
                    <div id="tab-1" class="tab-pane" role="tabpanel">

                        <div class="card-body ">

                            <asp:UpdatePanel runat="server" >
                                <ContentTemplate>

                                
                            <div class="form-group row ">
                                <label class="col-sm-1 col-form-label text-primary ">From</label>

                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtfrm" Style="margin-left: -7px" Height="37px" CssClass="form-control " runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                </div>

                                <label class="col-sm-1 col-form-label text-primary ">To</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtto" CssClass="datepicker form-control  " Height="37px" Style="margin-left: -7px" runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                </div>
                                <label class="col-sm-1 col-form-label text-primary ">Deposit</label>
                                <div class="col-sm-2">
                                    <div class="input-group">

                                        <asp:TextBox ID="txtprod" Height="37px" CssClass="form-control " runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
                                        <span class="input-group-btn input-group-append">
                                            <asp:LinkButton runat="server" ID="btn_inw" CssClass="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                            </asp:LinkButton>
                                        </span>

                                    </div>

                                </div>
                                <asp:ImageButton runat="server" Style="margin-left: 0px" Width="50px" Height="37px" ToolTip="Click Here to Send SMS" ImageUrl="~/Images/sndsms.jpg" ID="btn_sms" CssClass="btn btn-outline-secondary  btn" CausesValidation="False" Visible="false" />
                            </div>

                            <table style="width: 95%; margin-left: 0px; float: left" class="table table-bordered   ">
                                <thead>

                                    <tr style="color: white">
                                        <th style="width: 8%;">S.No</th>
                                        <th style="width: 13%;">Matured On</th>
                                        <th style="width: 15%;">Deposit No</th>
                                        <th style="width: 30%;">Name</th>
                                        <th style="width: 17%;">Amount</th>
                                        <th style="width: 17%;">Maturity Amount</th>
                                    </tr>
                                </thead>
                            </table>

                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>

                                                <asp:GridView  runat="server" ID="gvin" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingin" PageSize="50" EmptyDataText="No Data Found" OnRowDataBound="gvin_RowDataBound"  OnSelectedIndexChanged="OnSelectedIndexChangedgvin" 
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>

<asp:TemplateField>
   <HeaderTemplate>
    <asp:CheckBox runat="server" ID="cbHeader"  />
   </HeaderTemplate>
   <ItemTemplate>
       <div class="checkbox">
    <asp:CheckBox runat="server" ID="itmchk" CssClass ="checkbox " style="margin-top:-5px;margin-left:1px;float:left " />
           </div>
   </ItemTemplate>
                           <ItemStyle Width="3%" HorizontalAlign="Center"  />

  </asp:TemplateField>
                                <asp:TemplateField>
   <ItemTemplate>

                                         <%# Container.DataItemIndex + 1 %>

   </ItemTemplate>
                           <ItemStyle Width="5%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("mdate", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="13%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acno")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("Firstname")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="30%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("amount", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="17%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("mamt", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="17%" />

                                        </asp:TemplateField>
                                 </Columns>
                            </asp:GridView>

                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <table style="border: 1px; width: 95%; margin-left: 0px; float: left" class="table table-bordered ">
                                        <tbody>
                                            <tr>
                                                <td style="width: 66%">Total</td>
                                                <td style="width: 17%">
                                                    <asp:Label ID="lbldsum" runat="server" Style="float: right"></asp:Label>
                                                </td>
                                                <td style="width: 17%">
                                                    <asp:Label ID="lblmsum" runat="server" Style="float: right"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>

                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                                </ContentTemplate>
                            </asp:UpdatePanel>


                        </div>

                    </div>
                    <div id="tab-2" class="tab-pane" role="tabpanel">


                        <asp:UpdatePanel runat="server" ID="uprd">
                            <ContentTemplate>

                        <div class="form-group row ">
                               <label class="col-sm-1 col-form-label text-primary ">As At</label>
                                <div class="col-sm-3">
                                    <div class="input-group">

                                        <asp:TextBox ID="txtdue" Height="37px" CssClass="form-control " runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
                                        <span class="input-group-btn input-group-append">
                                            <asp:LinkButton runat="server" ID="btn_rd" CssClass="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                            </asp:LinkButton>
                                        </span>

                                    </div>

                                </div>

                            <div class="col-sm-2">
                                <asp:ImageButton runat="server" style="margin-left:0px" Width ="50px" Height="37px"  ToolTip="Click Here to Send SMS"  ImageUrl="~/Images/sndsms.jpg" id="btn_rd_sms" CssClass="btn btn-outline-secondary  btn" CausesValidation="False"  Visible="false"  />
                            </div>
                             
                        </div>

                              <table style="border:0px;width:95%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>

                                         <tr style="color:white">
                                         <th style="width:8%;">S.No</th>
                                         <th style="width:13%;">Account No</th>
                                         <th style="width:30%;">Name</th>
                                         <th style="width:17%;">Amount</th>
                                         <th style="width:25%;">Last Due On</th>
                                         <th style="width:10%;">Due Pending</th>
                                         </tr>
                                      </thead>
                                  </table>

                                      <asp:UpdatePanel runat="server"  >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvout" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="gvout_PageIndexChanging"  PageSize="50" EmptyDataText="No Data Found"  EnableViewState="false" 
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "  >
                            <Columns>

<asp:TemplateField>
   <HeaderTemplate>
    <asp:CheckBox runat="server" ID="cbHeader" CssClass ="checkbox " />
   </HeaderTemplate>
   <ItemTemplate>
    <asp:CheckBox runat="server" ID="itmchk" CssClass ="checkbox " style="margin-top:-5px;margin-left:-45px;float:left " />
   </ItemTemplate>
                           <ItemStyle Width="3%" HorizontalAlign="Center"  />

  </asp:TemplateField>
                                <asp:TemplateField>
   <ItemTemplate>

                                         <%# Container.DataItemIndex + 1 %>

   </ItemTemplate>
                           <ItemStyle Width="5%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acno")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="13%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("Firstname")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="30%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("amount", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="17%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lbldp" runat="server" text='<%#Eval("dupaid")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="25%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("pending")%>' Style="text-align:right   ;float:right   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>


                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                             
                                
                                    <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>
                                             <table style="border:1px;width:95%;margin-left:0px;float:left " class="table table-bordered">
                                  <tbody>
                                      <tr>
                                          <td style="width:66%">Total</td>
                                          <td style="width:17%">
                                              <asp:Label ID="Label1" runat="server" style="float:right " ></asp:Label>
                                          </td>
                                          <td style="width:17%">
                                              <asp:Label ID="Label2" runat="server" style="float:right "></asp:Label>
                                          </td>
                                      </tr>
                                    </tbody>

                                                 </table> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                                                                                                                     
                                                                                
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdateProgress ID="UpdateProgress1" runat="server"
            AssociatedUpdatePanelID="uprd">
                   <ProgressTemplate>
                       
        <div  class="loader ">    
            
        </div>    
    </ProgressTemplate>   
                   
                            </asp:UpdateProgress>

                    </div> <!-- Rd -->
                    <div id="tab-3" class="tab-pane" role="tabpanel">

                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>

                            

                                        <div class="form-group row ">

                                            <label class="col-sm-1 col-form-label text-primary ">As At</label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtasat" style="margin-left:-7px"  Height="37px"  CssClass="datepicker  form-control " runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-1 col-form-label text-primary ">Deposit</label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtprd" style="margin-left:-7px"   CssClass="form-control " Height="37px" runat="server" Width="120px" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit type Missing"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-1 col-form-label text-primary ">Balance</label>
                                            <div class="col-sm-2">
                                                     <div class="input-group">

                                        <asp:TextBox ID="txtbal" Height="37px" CssClass="form-control " runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
                                        <span class="input-group-btn input-group-append">
                                            <asp:LinkButton runat="server" ID="btn_minbal" CssClass="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                            </asp:LinkButton>
                                        </span>

                                    </div>
                                            </div>
                                        </div>

                                        <div >
                              <table style="border:1px;width:80%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>

                                         <tr style="color:white">
                                         <th style="width:10%;">S.No</th>
                                         <th style="width:15%;">Account No</th>
                                         <th style="width:35%;">Name</th>
                                         <th style="width:20%;">Balance</th>
                                                                                  </tr>
                                      </thead>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvmin" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging"   PageSize="50" EmptyDataText="No Data Found"  EnableViewState="false" 
                             Width="80%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "  >
                            <Columns>

                                <asp:TemplateField>
   <ItemTemplate>

                                         <%# Container.DataItemIndex + 1 %>

   </ItemTemplate>
                           <ItemStyle Width="10%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acno")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("Firstname")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="35%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("balance", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>

                       

                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                                                                   
                                                                                </div>

                                </ContentTemplate>
                        </asp:UpdatePanel>

                    


                    </div><!-- minimum -->
                    <div id="tab-4" class="tab-pane" role="tabpanel">
                        <div class="d-flex justify-content-between align-items-baseline">
                      <h6 class="card-title mb-0">&nbsp;</h6>
                      <div class="dropdown mb-2">
                        <button class="btn p-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-more-horizontal icon-lg text-muted pb-3px"><circle cx="12" cy="12" r="1"></circle><circle cx="19" cy="12" r="1"></circle><circle cx="5" cy="12" r="1"></circle></svg>
                        </button>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton" style="">
                          <asp:LinkButton ID="btnxl" runat="server" class="dropdown-item d-flex align-items-center"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-download icon-sm mr-2"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path><polyline points="7 10 12 15 17 10"></polyline><line x1="12" y1="15" x2="12" y2="3"></line></svg> <span class="">Download</span></asp:LinkButton>
                        </div>
                      </div>
                    </div>
                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>

                                            <div class="form-group row ">
                                <label class="col-sm-1 col-form-label text-primary ">From</label>

                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtnewfrm" Style="margin-left: -7px" Height="37px" CssClass="form-control " runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                </div>

                                <label class="col-sm-1 col-form-label text-primary ">To</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtnewto" CssClass="datepicker form-control  " Height="37px" Style="margin-left: -7px" runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                </div>
                                <label class="col-sm-1 col-form-label text-primary ">Deposit</label>
                                <div class="col-sm-2">
                                    <div class="input-group">

                                        <asp:TextBox ID="txtnewprd" Height="37px" CssClass="form-control " runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
                                        <span class="input-group-btn input-group-append">
                                            <asp:LinkButton runat="server" ID="btn_new" CssClass="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                            </asp:LinkButton>
                                        </span>

                                    </div>

                                </div>
                                <asp:ImageButton runat="server" Style="margin-left: 0px" Width="50px" Height="37px" ToolTip="Click Here to Send SMS" ImageUrl="~/Images/sndsms.jpg" ID="ImageButton1" CssClass="btn btn-outline-secondary  btn" CausesValidation="False" Visible="false" />
                            </div>

                                <table style="border:0px;width:100%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>

                                         <tr style="color:white">
                                         <th style="width:8%;">S.No</th>
                                         <th style="width:13%;">Date</th>
                                         <th style="width:12%;">Deposit No</th>
                                         <th style="width:35%;">Name</th>
                                         <th style="width:13%;">PAN</th>
                                         <th style="width:13%;">Amount</th>
                                         <th style="width:13%;">Maturity Amount</th>
                                         </tr>
                                      </thead>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvnew" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingnew" PageSize="50" EmptyDataText="No Data Found" EnableViewState="false" 
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "  >
                            <Columns>

                                <asp:TemplateField>
   <ItemTemplate>

                                         <%# Container.DataItemIndex + 1 %>

   </ItemTemplate>
                           <ItemStyle Width="8%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="13%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acno")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("Firstname")%>' Style="text-align:left;float:left  "></asp:Label>
                                                <br />
                                                 <asp:Label ID="Label3" runat="server" text='<%#Eval("LastName")%>' Style="text-align:left;float:left;font-size:11px;  "></asp:Label>
                                                <br />
                                                 <asp:Label ID="Label4" runat="server" text='<%#Eval("Address")%>' Style="text-align:left;float:left;font-size:11px;  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="30%" />

                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("pan")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("amount", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("mamt", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                 </Columns>
                            </asp:GridView>

                                        
                                             <table style="border:0px;width:100%;margin-left:0px;float:left " class="table table-bordered ">
                                  <tbody>
                                      <tr>
                                          <td style="width:70%">Total</td>
                                          <td style="width:15%">
                                              <asp:Label ID="lblnewdsum" runat="server" style="float:right " ></asp:Label>
                                          </td>
                                          <td style="width:15%">
                                              <asp:Label ID="lblnewmsum" runat="server" style="float:right "></asp:Label>
                                          </td>
                                      </tr>
                                    </tbody>

                                                 </table> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                                      

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div><!-- new ac -->
                    <div id="tab-5" class="tab-pane" role="tabpanel">

                                                                    <div class="form-group row ">
                                <label class="col-sm-1 col-form-label text-primary ">From</label>

                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtcldfrm" Style="margin-left: -7px" Height="37px" CssClass="form-control " runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                </div>

                                <label class="col-sm-1 col-form-label text-primary ">To</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtcldto" CssClass="datepicker form-control  " Height="37px" Style="margin-left: -7px" runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                </div>
                                <label class="col-sm-1 col-form-label text-primary ">Deposit</label>
                                <div class="col-sm-2">
                                    <div class="input-group">

                                        <asp:TextBox ID="txtcldprod" Height="37px" CssClass="form-control " runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
                                        <span class="input-group-btn input-group-append">
                                            <asp:LinkButton runat="server" ID="btn_cld" CssClass="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                            </asp:LinkButton>
                                        </span>

                                    </div>

                                </div>
                                <asp:ImageButton runat="server" Style="margin-left: 0px" Width="50px" Height="37px" ToolTip="Click Here to Send SMS" ImageUrl="~/Images/sndsms.jpg" ID="ImageButton2" CssClass="btn btn-outline-secondary  btn" CausesValidation="False" Visible="false" />
                            </div>

                         <table style="border:0px;width:95%;margin-left:0px;float:left "  class="table table-bordered">
                                  <thead>

                                         <tr style="color:white">
                                         <th style="width:8%;">S.No</th>
                                         <th style="width:13%;">Date</th>
                                         <th style="width:15%;">Deposit No</th>
                                         <th style="width:30%;">Name</th>
                                         <th style="width:17%;">Amount</th>
                                         <th style="width:17%;">Maturity Amount</th>
                                         </tr>
                                      </thead>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvcld" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingcld" PageSize="50" EmptyDataText="No Data Found" EnableViewState="false" 
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>

                                <asp:TemplateField>
   <ItemTemplate>

                                         <%# Container.DataItemIndex + 1 %>

   </ItemTemplate>
                           <ItemStyle Width="8%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="13%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acno")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("Firstname")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="30%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("amount", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="17%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("drd", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="17%" />

                                        </asp:TemplateField>
                                 </Columns>
                            </asp:GridView>

                                         
                                             <table style="border:0px;width:95%;margin-left:0px;float:left " class="table table-bordered ">
                                  <tbody>
                                      <tr>
                                          <td style="width:66%">Total</td>
                                          <td style="width:17%">
                                              <asp:Label ID="lbldcld" runat="server" style="float:right " ></asp:Label>
                                          </td>
                                          <td style="width:17%">
                                              <asp:Label ID="lblmcld" runat="server" style="float:right "></asp:Label>
                                          </td>
                                      </tr>
                                    </tbody>

                                                 </table> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                                      
                    </div><!-- closed ac -->
                </div>
                <!--  tab pane-->
            </div>
            <!-- smart tab -->
        </div>

    </form>

    <style>
       

        .loader{
  position: fixed;
  left: 0px;
  top: 0px;
  width: 100%;
  height: 100%;
  z-index: 9999;
  background: url('../Images/813.gif') 
              50% 50% no-repeat rgba(0,0,0,0.5);
}
    </style>

    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/daterangepicker.js" type="text/javascript" ></script>
    <script src="js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="css/smart_tab.min.css" />


    <script type="text/javascript">


        $(document).ready(function () {

            $('#smarttab').smartTab({
                selected: 0, // Initial selected tab, 0 = first tab
                //theme: 'dark', // theme for the tab, related css need to include for other than default theme
                orientation: 'horizontal', // Nav menu orientation. horizontal/vertical
                justified: true, // Nav menu justification. true/false
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
                    "autoUpdateInput": true,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtto.ClientID%>").val(start.format('DD-MM-YYYY'));


                });
                  });




            $("#<%=txtnewfrm.ClientID%>").focus(function () {


                $("#<%=txtnewfrm.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtnewfrm.ClientID%>").val(start.format('DD-MM-YYYY'));
                    

                });
              });


            $("#<%=txtnewto.ClientID%>").focus(function () {


                $("#<%=txtnewto.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtnewto.ClientID%>").val(start.format('DD-MM-YYYY'));


                });
                });




            $("#<%=txtcldfrm.ClientID%>").focus(function () {


                $("#<%=txtcldfrm.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtcldfrm.ClientID%>").val(start.format('DD-MM-YYYY'));
                    

                });
              });


            $("#<%=txtcldto.ClientID%>").focus(function () {


                $("#<%=txtcldto.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtcldto.ClientID%>").val(start.format('DD-MM-YYYY'));


                });
                });




            $("#<%=txtdue.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                 $("#<%=txtdue.ClientID%>").val(start.format('DD-MM-YYYY'));
             });


            $("#<%=txtasat.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                 $("#<%=txtasat.ClientID%>").val(start.format('DD-MM-YYYY'));
             });



        }




    </script>

</asp:Content>
