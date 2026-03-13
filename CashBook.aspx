<%@ Page Title="Cash Book" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="CashBook.aspx.vb" Inherits="Fiscus.CashBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/daterangepicker.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <form id="intpost" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Books</a></li>
						<li class="breadcrumb-item active" >Cash Book</li>
					</ol>
				</nav>

          <div class="card card-body ">
              <div id="smarttab">
                                        <ul class="nav" id="tabitm"  >
           <li id="cTab">
               <a class="nav-link" href="#tab-1">
                   Cash Book
                   </a>
           </li>
           <li id="jTab">
               <a class="nav-link" href="#tab-2">
                  Denomination
               </a>
           </li>
          
    </ul>
                                       
										<div class="tab-content">
                                           <div id="tab-1" class="tab-pane" role="tabpanel">

              <div class="form-group row  border-bottom">
                  <label class="col-sm-1 col-form-label text-primary ">Date</label>
                <div class="col-sm-3">
                    <div class="input-group">

                          <asp:TextBox  ID="txtdate" runat="server" AutoCompleteType="None"   Width ="120px" CssClass="form-control "  data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
            <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                      
                    </div>
                </div>

              </div>

              <table style="border:1px;width:99%;margin-left:0px;margin-top:5px "  class="table table-bordered ">
                                  <tbody>
                                         <tr style="color:black">
                                         <td style="width:5%;text-align:center;color:black;font-variant:small-caps;background-color:#0599ce; ">T.Id</td>
                                             <td style="width:10%;text-align:center;color:black;font-variant:small-caps;background-color:#0599ce ">Account No</td>
                                         <td style="width:23%;text-align:center;color:black;font-variant:small-caps;background-color:#0599ce">Account head</td>
                                         <td style="width:12%;text-align:center;color:black;font-variant:small-caps;background-color:#0599ce">credit</td>
                                         <td style="width:5%;text-align:center;color:black;font-variant:small-caps;background-color:slateblue ">T.Id</td>
                                             <td style="width:10%;text-align:center;color:black;font-variant:small-caps;background-color:slateblue ">Account No</td>
                                         <td style="width:23%;text-align:center;color:black;font-variant:small-caps;background-color:slateblue ">account head</td>
                                             <td style="width:12%;text-align:center;color:black;font-variant:small-caps;background-color:slateblue ">debit</td>
                                         </tr>
                                      </tbody>
                                  </table> 
              

                                                   
                              <div>
                                             <asp:UpdatePanel ID="cbcr" runat="server" >
                                    <ContentTemplate >
                                        
                            <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="display:inline-block;float:left "
                            AllowPaging ="true" OnPageIndexChanging ="OnPaging" PageSize ="50" Width="48%" 
                               Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "   >
                                <Columns>
                                    <asp:TemplateField >
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cr_id" runat="server" text='<%#Eval("id")%>' Style="text-align:right;float:right;font-size:small"></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="5%" />
                                                                                </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            
                                                
                                    <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("acn")%>' Width="100%"  Style="text-align:center   ;float:left  ;font-size:small "></asp:Label>
                                    <asp:Label ID="lbl_shrt" runat="server" text='<%#Eval("dep")%>' Width="100%"  Style="text-align:center  ;float:left   ;font-size:small   "></asp:Label>
                                            
                                        </ItemTemplate>
                                         <ItemStyle Width ="10%"  Height="15px"/>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("achead")%>' Style="text-align:left ;float:left;font-size:small "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="22%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("crc", "{0:N}")%>' Style="text-align:right;float:right ;font-size:small "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="12%" />
                                    </asp:TemplateField>

                                </Columns>

                                </asp:GridView> 
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                                     <asp:UpdatePanel ID="cbdr" runat="server" >
                                     <ContentTemplate >
                            <asp:GridView  runat="server" ID="dispdebit" AutoGenerateColumns="false"  style="float:left;"
                            AllowPaging ="true" OnPageIndexChanging ="OnPaging" PageSize ="50"  width="50%" 
                               Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "  >
                                <Columns>
                                    <asp:TemplateField >
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dr_id" runat="server" text='<%#Eval("id")%>' Style="text-align:right;float:right;font-size:small"></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="5%" />
                                                                                </asp:TemplateField>


                                     <asp:TemplateField>
                                        <ItemTemplate>
                                             
                                    <asp:Label ID="lbl_cr_suplw" runat="server" text='<%#Eval("acn")%>' Width="100%"  Style="text-align:center   ;float:left   ;font-size:small "></asp:Label>
                                           <asp:Label ID="lbl_shrt" runat="server" text='<%#Eval("dep")%>' Width="100%" Style="text-align:center  ;float:left  ;font-size:small   "></asp:Label>
                                        </ItemTemplate>
                                         <ItemStyle Width ="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>

                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dr_supl" runat="server" text='<%#Eval("achead")%>' Style="text-align:left ;float:left;font-size:small "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="23%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("drc", "{0:N}")%>' Style="text-align:right;float:right ;font-size:small "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="12%" />
                                    </asp:TemplateField>

                                </Columns>

                                </asp:GridView> 
                                         </ContentTemplate> 

                                     </asp:UpdatePanel> 
              
                       </div>
                      

                                <table  style="border:none;width:100%;margin-left:0px;float:left " >
                                  <tbody>
                                         <tr style="height:25px">
                                         <td style="width:38%;border:none ;">Opening Balance</td>
                                         <td style="width:12%;font-variant:small-caps">
                                             <asp:Label ID="lbl_opening_credit" Style="text-align:right;float:right " runat ="server" ></asp:Label>
                                                                                     </td>
                                         
                                         <td style="width:38%;font-variant:small-caps"></td>
                                             <td style="width:12%;">
                                                 <asp:Label ID="lbl_opening_debit" Style="text-align:right;float:right " runat ="server" ></asp:Label>
                                              </td>
                                         </tr>

                                         <tr style="height:25px">
                                         <td style="width:38%;">Credit total</td>
                                         <td style="width:12%;">
                                             <asp:Label ID="lbl_sum_credit" Style="text-align:right;float:right " runat ="server" ></asp:Label>
                                                                                     </td>
                                         
                                         <td style="width:38%;text-align:center">Debit total</td>
                                             <td style="width:12%;text-align:right ;float:right; ">
                                                 <asp:Label ID="lbl_sum_debit" Style="text-align:right;float:right " runat ="server" ></asp:Label>
                                              </td>
                                         </tr>

                                         <tr style="height:25px">
                                         <td style="width:38%;">Closing Balance</td>
                                         <td style="width:12%;">
                                             <asp:Label ID="lbl_closing_credit" Style="text-align:right;float:right " CssClass="col-form-label text-success " runat ="server" ></asp:Label>
                                                                                     </td>
                                         
                                         <td style="width:38%;"></td>
                                             <td >
                                                 <asp:Label ID="lbl_closing_debit" Style="text-align:right;float:right " CssClass="col-form-label text-danger " runat ="server" ></asp:Label>
                                              </td>
                                         </tr>


                                      </tbody>
                                  </table> 
                                               </div>
                                            <div id="tab-2" class="tab-pane" role="tabpanel">
                           
                                                                             <div class="form-group row">
                  <label class="col-md-2">Closing Balance</label>
                  <div class="col-md-3">
                      <asp:Label ID="lbldenomcb" Font-Bold="true" Font-Size="Large"  CssClass="text-primary" runat="server" ></asp:Label>
                  </div>
                     </div>
                                                <div class="row">
                           

					<div class="col-md-6 grid-margin stretch-card">
            <div class="card">
              <div class="card-body">
                 
								<h6 class="card-title text-primary">As Per System</h6>

                  <table   class="table table-bordered">
                                
                                        <tbody>
                                        
                                        <tr ><td style="width:60px">2000 </td>
                                            <td style="text-align:center;width:50px">X</td>
                                            <td ><asp:Label runat="server" ID="lbl_count_1k" Text="0" style="text-align:right;float:right"></asp:Label></td>
                                            <td style="text-align:center;width:30%">=</td><td ><asp:Label runat="server" ID="lbl_val_1k" Text="0" style="text-align:right;float:right;"></asp:Label></td>

                                        </tr>
                                        <tr ><td style="width:60px"> 500 </td><td style="text-align:center;width:50px">X</td><td ><asp:Label runat="server" ID="lbl_count_500" Text="0" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td><asp:Label runat="server" ID="lbl_val_500" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>
                                        <tr ><td style="width:60px"> 200 </td><td style="text-align:center;width:50px">X</td><td ><asp:Label runat="server" ID="lbl_count_200" Text="0" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td><asp:Label runat="server" ID="lbl_val_200" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>
                                        <tr ><td style="width:60px"> 100 </td><td style="text-align:center;width:50px">X</td><td ><asp:Label runat="server" ID="lbl_count_100" Text="0" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td><asp:Label runat="server" ID="lbl_val_100" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>
                                        <tr ><td style="width:60px">  50 </td><td style="text-align:center;width:50px">X</td><td ><asp:Label runat="server" ID="lbl_count_50" Text="0" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td><asp:Label runat="server" ID="lbl_val_50" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>
                                        <tr ><td style="width:60px">  20 </td><td style="text-align:center;width:50px">X</td><td ><asp:Label runat="server" ID="lbl_count_20" Text="0" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td><asp:Label runat="server" ID="lbl_val_20" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>
                                        <tr ><td style="width:60px">  10 </td><td style="text-align:center;width:50px">X</td><td ><asp:Label runat="server" ID="lbl_count_10" Text="0" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td><asp:Label runat="server" ID="lbl_val_10" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>
                                        <tr ><td style="width:60px"> COINS</td><td style="text-align:center;width:50px">+</td><td ><asp:Label runat="server" ID="lbl_count_coin" Text="0" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td ><asp:Label runat="server" ID="lbl_val_coins" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>
                                         <tr ><td style="width:60px"> Others</td><td style="text-align:center;width:50px">+</td><td ><asp:Label runat="server" ID="Label2" Text="0" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td ><asp:Label runat="server" ID="lblothers" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>
                                        <tr ><td style="width:0px"> Total</td><td style="text-align:center;width:50px"></td><td ><asp:Label runat="server" ID="Label1" Text="" style="text-align:right;float:right"></asp:Label></td><td style="text-align:center;width:50px">=</td><td ><asp:Label runat="server" ID="lbl_denom_total" Text="0" style="text-align:right;float:right;"></asp:Label></td></tr>


                                    </tbody>
                                </table>


				</div>
                </div>
                        </div>
                                                        <div class="col-md-6 grid-margin stretch-card">
            <div class="card">
              <div class="card-body">
								<h6 class="card-title text-primary ">As Per Physical</h6>


                  <asp:UpdatePanel runat="server" >
                      <ContentTemplate>

                      
                   <table style="" class="table table-bordered">
                                
                                        <tbody>
                                        
                                        <tr style="height:10px" >
                                            <td style="width:60px">2000 </td>
                                            <td style="text-align:center;width:50px">X</td>
                                            <td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txt1k" Text="0" style="text-align:right;"></asp:TextBox>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                            </td>
                                            <td style="text-align:right;width:50px">=</td>
                                            <td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:Label runat="server" ID="lbl_pd_1k" Text="" Width="110px" style="text-align:right;"></asp:Label>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel> 
                                            </td>          
                                            </tr>
                                            <tr >
                                            <td style="width:60px"> 500 </td><td style="text-align:center;width:50px">X</td>
                                            <td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txt500" Text="0" style="text-align:right;"></asp:TextBox>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>            
                                            </td>
                                            <td style="text-align:right ;width:50px">=</td>
                                            <td>
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:Label runat="server" ID="lbl_pd_500" Text="0" Width="110px" style="text-align:right;"></asp:Label>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel> 
                                            </td>
                                            </tr>

<tr >
                                            <td style="width:60px"> 200 </td><td style="text-align:center;width:50px">X</td>
                                            <td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txt200" Text="0" style="text-align:right;"></asp:TextBox>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>            
                                            </td>
                                            <td style="text-align:right ;width:50px">=</td>
                                            <td>
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:Label runat="server" ID="lbl_pd_200" Text="0" Width="110px" style="text-align:right;"></asp:Label>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel> 
                                            </td>
                                            </tr>








                                            <tr >
                                            <td style="width:60px"> 100 </td><td style="text-align:center;width:50px">X</td><td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txt100" Text="0" style="text-align:right;"></asp:TextBox>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel>
                                            </td>
                                            <td style="text-align:right ;width:50px">=</td><td>
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:Label runat="server" ID="lbl_pd_100" Text="0" Width="110px" style="text-align:right;"></asp:Label>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel> 
                                            </td>
                                            </tr>
                                            <tr ><td style="width:60px">  50 </td><td style="text-align:center;width:50px">X</td><td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txt50" Text="0" style="text-align:right;"></asp:TextBox>
                                            </ContentTemplate>
                                            </asp:UpdatePanel> 
                                            </td><td style="text-align:right;width:50px">=</td><td>
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:Label runat="server" ID="lbl_pd_50" Text="0" Width="110px" style="text-align:right;"></asp:Label>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel> 
                                            </td></tr>
                                            <tr ><td style="width:60px">  20 </td><td style="text-align:center;width:50px">X</td><td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txt20" Text="0" style="text-align:right;"></asp:TextBox>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel>
                                            </td><td style="text-align:right ;width:50px">=</td>
                                            <td>
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:Label runat="server" ID="lbl_pd_20" Text="0" Width="110px" style="text-align:right;"></asp:Label>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel> 
                                            </td>
                                            </tr>
                                            <tr ><td style="width:60px">  10 </td><td style="text-align:center;width:50px">X</td>
                                            <td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txt10" Text="0" style="text-align:right;"></asp:TextBox>
                                            </ContentTemplate>
                                            </asp:UpdatePanel> 
                                            </td>
                                            <td style="text-align:right ;width:50px">=</td>
                                            <td>
                                              <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >                                                                                           
                                                <asp:Label runat="server" ID="lbl_pd_10" Text="0" Width="110px" style="text-align:right;"></asp:Label>
                                                </ContentTemplate> 
                                                  </asp:UpdatePanel> 
                                            </td></tr>
                                            <tr ><td style="width:60px"> COINS</td><td style="text-align:center;width:50px">+</td><td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txtcoin" Text="0" style="text-align:right;"></asp:TextBox>
                                              </ContentTemplate> 
                                                  </asp:UpdatePanel> 
                                                   </td>


                                                <td style="text-align:right ;width:50px">=</td><td >
                                               <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                               <asp:Label runat="server" ID="lbl_pd_coin" Text="0" Width="110px" style="text-align:right;"></asp:Label>
                                                </ContentTemplate> 
                                               </asp:UpdatePanel> 

                                                </td></tr>


                                             <tr ><td style="width:60px"> Other</td><td style="text-align:center;width:50px">+</td><td >
                                            <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                            <asp:TextBox width="80px" CssClass="form-control" AutoPostBack="true"  runat="server" ID="txtother" Text="0" style="text-align:right;"></asp:TextBox>
                                              </ContentTemplate> 
                                                  </asp:UpdatePanel> 
                                                   </td>


                                                <td style="text-align:right ;width:50px">=</td><td >
                                               <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                               <asp:Label runat="server" ID="lbl_pd_others" Text="0" Width="110px" style="text-align:right;"></asp:Label>
                                                </ContentTemplate> 
                                               </asp:UpdatePanel> 

                                                </td></tr>







                                        <tr ><td style="width:60px"> Total</td><td style="text-align:center;width:50px"></td>
                                            <td >
                                            
                                               <asp:UpdatePanel runat="server" >
                                                    <ContentTemplate >
                                            <asp:Label runat="server" ID="Label16" Text="" style="text-align:right;"></asp:Label>

                                                        </ContentTemplate> 
                                                   </asp:UpdatePanel> 
                                                
                                                                                                                             </td><td style="text-align:center;width:50px">=</td>
                                            <td >
                                
                                                                                                                                  <asp:UpdatePanel runat="server" >
                                            <ContentTemplate >
                                                                                                                                 <asp:Label runat="server" ID="lbl_pd_total" Text="0" Width="110px" style="text-align:right;"></asp:Label></td></tr>
                                            </ContentTemplate> 
                                                                                                                                      </asp:UpdatePanel> 
                                                </td>
                                            </tr>

                                    </tbody>
                                </table>

                                       

                  <div class="form-group row border-sm-top ">
                      <div class="col-sm-2"></div>
                      <div class="col-sm-2">
                          <asp:Button ID="btn_supliment" runat="server" CssClass="btn btn btn-outline-primary " style="margin-left:300px" Width="250px" Text ="Generate Supplementary Report" Visible="false"  />
                                     <asp:Button CssClass="btn  btn-outline-primary    " style="margin-left:100px;margin-top:20px" runat="server" ID="btn_up_pd" Text ="Update" />
                      </div>
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
    <script src="js/daterangepicker.js" type="text/javascript" ></script>
    
	<script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
    

    <script type="text/javascript">


        $(document).ready(function () {

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


            $("#<%=txtdate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=txtdate.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

          
        }



    </script>



</asp:Content>
