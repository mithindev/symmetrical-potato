<%@ Page Title="Loan Analysis" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="LoanAnalysis.aspx.vb" Inherits="Fiscus.LoanAnalysis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/daterangepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form id="loananalysis" runat="server">

        <asp:ScriptManager ID="sml" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Reports</a></li>
                <li class="breadcrumb-item active">Loan Analysis</li>
            </ol>
        </nav>

        <div class="card card-body ">

            
            <div id="smarttab">
                <ul class="nav" id="tabitm">
                    <li>
                        <a class="nav-link" href="#tab-1">
                            Jewel Loan
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#tab-2">New Account
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#tab-3">Closed Account
                        </a>
                    </li>
                    <li>
                        <a class="nav-link" href="#tab-4">Reminder
                        </a>
                    </li>
                    
                </ul>

                <div class="tab-content">
                    <div id="tab-1" class="tab-pane" role="tabpanel">


                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>

                        <div class="card-body ">
                            
                            <div class="form-group row border-bottom ">
                              <div  class ="input-daterange input-group ">
                                                             <span class="input-group-addon col-form-label  ">RPG&nbsp;&nbsp;</span>
                        <asp:DropDownList CssClass ="form-control" runat ="server" style="margin-left :-6.5px"  Width="110px" ID="fltrrpg" >
                            <asp:ListItem>Equal to</asp:ListItem>
                            <asp:ListItem>Less than</asp:ListItem>
                            <asp:ListItem>Greater than</asp:ListItem>
                        </asp:DropDownList>                 
                                                    <asp:TextBox ID="txtrpg" Style="margin-left: 0px" CssClass=" form-control " runat="server" Width="100px" ></asp:TextBox>
                                                    <span class="input-group-addon col-form-label " >Balance&nbsp;&nbsp;</span>
                                                    <asp:DropDownList CssClass ="form-control" runat ="server"   Width="130px" ID="ddbal" >
                                                    <asp:ListItem>Equal to</asp:ListItem>
                                                    <asp:ListItem>Less than</asp:ListItem>
                                                    <asp:ListItem>Greater than</asp:ListItem>
                                                    </asp:DropDownList>                 
                                                <asp:TextBox ID="txtamt" CssClass=" form-control  " Style="margin-left: 0px" runat="server" Width="100px" ></asp:TextBox>
                                                                                                        <span class="input-group-addon col-form-label col-sm-1"  >Name</span>
                                                
                                                                                                        <asp:TextBox ID="txtname" CssClass=" form-control  "  runat="server" Width="120px" ></asp:TextBox>

                                                     <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>      
                                          

                            </div>

                                </div>

                            <table style="border:0px;width:100%;margin-left:0px;float:left;margin-top:5px; "  class="table table-bordered ">
                                  <thead>

                                         <tr style="color:white">
                                         <th style="width:15%;">Loan Date</th>
                                         <th style="width:15%;">Account No</th>
                                         <th style="width:30%;">Name</th>
                                         <th style="width:10%;">Weight</th>

                                         <th style="width:15%;">Balance</th>
                                         <th style="width:10%;">Rate Per GM</th>
                                         </tr>
                                      </thead>
                                  </table>

                   
                        <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="50" EmptyDataText="No Data Found" 
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>

                                 
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("acno")%>' Style="text-align:left   ;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>

                                                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("name")%>' Style="text-align:left   ;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="30%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("tnet", "{0:#,###.000}")%>' Style="text-align:right    ;float:right   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>


                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("balance", "{0:N}")%>'   Style="text-align:right;float:right"></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("rpg", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>
                                 </Columns>
                            </asp:GridView>

                                                                              

                    <asp:UpdatePanel runat ="server">
                        <ContentTemplate>                            <table style="border:0px;width:100%;margin-left:0px;float :left " class ="table table-bordered  ">

                                                 <tbody>
                                                     <tr >
                                                         <td style="width:60%">Total</td>
                                                         <td style ="width:10%">
                                                         <asp:Label ID="lblwtsum" runat="server" Style="text-align:right;float:right "></asp:Label></td>

                                                         <td style ="width:15%">
                                                             
                                                         <asp:Label ID="lblsum" runat="server" Text="0" Style="text-align:right;float:right "></asp:Label></td>

                                                         <td style ="width:10%">
                                                             <asp:Label ID="lblrpg" runat="server" Style="text-align:right;float:right "></asp:Label>
                                                         </td>
                                                     </tr>
                                                 </tbody>
                                             </table>
                            </ContentTemplate>

                        </asp:UpdatePanel>

                                    
                            
                            </div>

                                
                            </ContentTemplate>
                        </asp:UpdatePanel>


                        </div> <!-- jl -->

                        
                    <div id="tab-2" class="tab-pane" role="tabpanel">

                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>

                            
                                 <div class="form-group row border-bottom ">
                              <div  class ="input-daterange input-group col-sm-10">
                                                             <span class="input-group-addon col-form-label col-sm-1 ">From</span>
                                                    <asp:TextBox ID="txtnewfrm" Style="margin-left: 0px" CssClass=" form-control " runat="server" Width="100px" ></asp:TextBox>
                                                    <span class="input-group-addon col-form-label col-sm-1" >To</span>
                                                    
                                                <asp:TextBox ID="txtnewto" CssClass=" form-control  " Style="margin-left: 0px" runat="server" Width="100px" ></asp:TextBox>
                                                                                                        <span class="input-group-addon col-form-label col-sm-1"  >Loan</span>
                                                
                                                                                                        <asp:TextBox ID="txtnewprd" CssClass=" form-control  "  runat="server" Width="120px" ></asp:TextBox>

                                                     <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_new" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>      
                                          

                            </div>

                                </div>


                                                                                             
                              <table style="border:0px;width:95%;margin-left:0px;float:left;margin-top:10px "  class="table table-bordered ">
                                  <thead>

                                         <tr style="color:white">
                                         <th style="width:8%;">S.No</th>
                                         <th style="width:13%;">Date</th>
                                         <th style="width:15%;">Loan No</th>
                                         <th style="width:30%;">Name</th>
                                         <th style="width:17%;">Amount</th>
                                         </tr>
                                      </thead>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvnew" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingnew" PageSize="50" EmptyDataText="No Data Found" EnableViewState="false" 
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "  >
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

                             
                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                                                                   
                                    <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>
                                             <table style="border:0px;width:95%;margin-left:0px;float:left " class="table table-bordered">
                                  <tbody>
                                      <tr>
                                          <td style="width:75%">Total</td>
                                          <td style="width:20%">
                                              <asp:Label ID="lblnewdsum" runat="server" style="float:right " ></asp:Label>
                                          </td>
                                         
                                      </tr>
                                    </tbody>

                                                 </table> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                                                                                                               

                                </ContentTemplate>
                            </asp:UpdatePanel>

                    </div> <!-- new Accoun -->
                    <div id="tab-3" class="tab-pane" role="tabpanel">


                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>

                            
                            <div class="form-group row border-bottom ">
                              <div  class ="input-daterange input-group col-sm-10">
                                                             <span class="input-group-addon col-form-label col-sm-1 ">From</span>
                                                    <asp:TextBox ID="txtcldfrm" Style="margin-left: 0px" CssClass=" form-control " runat="server" Width="100px" ></asp:TextBox>
                                                    <span class="input-group-addon col-form-label col-sm-1" >To</span>
                                                    
                                                <asp:TextBox ID="txtcldto" CssClass=" form-control  " Style="margin-left: 0px" runat="server" Width="100px" ></asp:TextBox>
                                                                                                        <span class="input-group-addon col-form-label col-sm-1"  >Loan</span>
                                                
                                                                                                        <asp:TextBox ID="txtcldprod" CssClass=" form-control  "  runat="server" Width="120px" ></asp:TextBox>

                                                     <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_cld" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>      
                                          

                            </div>

                                </div>

                        <table style="border:0px;width:95%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>

                                         <tr style="color:white">
                                         <th style="width:8%; ">S.No</th>
                                         <th style="width:13%; ">Date</th>
                                         <th style="width:15%;">Loan No</th>
                                         <th style="width:30%;">Name</th>
                                         <th style="width:17%;">Amount</th>
                                         <th style="width:17%;">Closing Balance</th>
                                         </tr>
                                      </thead>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvcld" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingcld" PageSize="50" EmptyDataText="No Data Found"  
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
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("crd", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="17%" />

                                        </asp:TemplateField>
                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                                                                   
                                    <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>
                                             <table style="border:1px;width:95%;margin-left:0px;float:left " class="table table-bordered ">
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

                                </ContentTemplate>
                        </asp:UpdatePanel>


                    </div> <!--Closed Acc -->
                    <div id="tab-4" class="tab-pane" role="tabpanel">

                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>

                                       <div class="form-group row border-bottom ">
                            <span class="input-group-addon col-form-label col-sm-1"  >Day</span>
                                                
                                                                                                        <asp:TextBox ID="txtday" CssClass=" form-control  "  runat="server" Width="120px" ></asp:TextBox>

                                                     <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btnlist" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>      
                                           <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btnsendsms" CssClass ="btn btn-outline-primary ">

                    
<svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-message-square"><path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"></path></svg>                                                                    
                                                                </asp:LinkButton>
                                                          </span>      
                                          

                            </div>

                                                       

                                                                                               
                              <table style="border:1px;width:95%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>

                                         <tr style="color:white">
                                          <th style="width:4%">
                                              <asp:CheckBox runat="server" ID="smshdrchk" AutoPostBack="true"/>
                                          </th>
                                         <th style="width:8%; ">S.No</th>
                                         <th style="width:13%; ">Date</th>
                                         <th style="width:15%;">Loan No</th>
                                         <th style="width:30%;">Name</th>
                                         <th style="width:17%;">Amount</th>
                                             
                                         </tr>
                                      </thead>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvsms" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="false"  PageSize="500" EmptyDataText="No Data Found"  
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="smschk" Checked="true"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" HorizontalAlign="Center"  />
                                </asp:TemplateField>

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

                                <asp:TemplateField Visible="false" >
                                            <ItemTemplate >
                                                <asp:Label ID="lblsms" runat="server" text='<%#Eval("mobile")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="17%" />

                                        </asp:TemplateField>
                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                                                                   
                                               </ContentTemplate>
                        </asp:UpdatePanel>

                    </div   > <!-- reminder -->
                    </div>
                </div>

            
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
  
}
    </style>

    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="js/daterangepicker.js" type="text/javascript"></script>
    <script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />


    <script type="text/javascript">


        $(document).ready(function () {

            $('#smarttab').smartTab({
                selected: 0, // Initial selected tab, 0 = first tab
               // theme: 'dark', // theme for the tab, related css need to include for other than default theme
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





        }




    </script>


</asp:Content>
