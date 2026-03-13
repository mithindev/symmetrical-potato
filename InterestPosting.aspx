<%@ Page Title="Interest Posting" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="InterestPosting.aspx.vb" Inherits="Fiscus.InterestPosting" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/daterangepicker.css" />

    <style type="text/css">
   
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form id="intpost" runat="server" >

            
        <asp:ScriptManager ID="SM1" runat="server" AsyncPostBackTimeout="360000" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Home</a></li>
						<li class="breadcrumb-item active" >Interest Posting</li>
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
                   Interest Specifics
                   </a>
           </li>
           <li id="jTab">
               <a class="nav-link" href="#tab-2">
                  Posting
               </a>
           </li>
            <li id="dTab"><a class="nav-link" href="#tab-3" >
                  Standing Instruction
               </a></li>
    </ul>
                                       
										<div class="tab-content">
                                           <div id="tab-1" class="tab-pane" role="tabpanel">

                                               
                                                <asp:UpdatePanel ID="upint" runat="server" ><ContentTemplate>

                                                    <div class="form-group row ">
                                                        <label class="col-sm-2 col-form-label text-primary ">Date</label>
                                                        <div class="col-sm-2">
                                                                <asp:TextBox ID="tdate" runat="server" CssClass=" form-control  " AutoCompleteType="None" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                        
                                                            
                                                        </div>
                                                    </div>

                                                    <div class="form-group row ">
                                                        <label class="col-sm-2 col-form-label text-primary ">Account</label>
                                                        <div class="col-sm-2">
                                                        <asp:UpdatePanel ID="up5" runat="server" >
                                            <ContentTemplate>

                                        <asp:DropDownList ID="prod" runat="server" CssClass="form-control "  AutoPostBack="true" data-validation-engine="validate[required]" data-errormessage-value-missing="Select an Option" ></asp:DropDownList>
                                                </ContentTemplate></asp:UpdatePanel>
                                                            </div>
                                                    </div>

                                                    <div class="form-group row ">
                                                        <label class="col-sm-2 col-form-label text-primary ">Type</label>
                                                        <div class="col-sm-2">
                                                            <asp:UpdatePanel ID="up6" runat="server" >
                                            <ContentTemplate>
                                        <asp:DropDownList ID="inttyp" runat="server" CssClass="form-control "  AutoPostBack="true"  data-validation-engine="validate[required]" data-errormessage-value-missing="Select an Option">
                                            <asp:ListItem Value =""><-- Select --></asp:ListItem>
                                            <asp:ListItem value ="Monthly">Monthly</asp:ListItem>
                                            <asp:ListItem Value="Quaterly">Quaterly</asp:ListItem>
                                            <asp:ListItem Value="Quaterly">Half Yearly</asp:ListItem>
                                            <asp:ListItem Value ="Yearly">Yearly</asp:ListItem>
                                        </asp:DropDownList>
                                            
                                            </ContentTemplate>
                                            </asp:UpdatePanel>

                                                        </div>
                                                    </div>

                                                       <asp:Panel runat="server" ID="prd_cap" Visible="false">
                                                           <div class="form-group row">
                                                         <label class="col-sm-2 col-form-label text-primary ">Period</label>
                                                           <div class="col-sm-2">
                                                               <asp:TextBox ID="txtfrm"  CssClass=" form-control " runat="server"  data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                
                                                
                                                      </div> 
                                                           
                                                           <label class="col-sm-1 col-form-label text-primary ">To</label>
                                                           <div class="col-sm-2">
                                                               <asp:TextBox ID="txtto" CssClass="form-control  "  runat="server"  data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                           </div>
                                                           </div>
                                     </asp:Panel>

                                                    <asp:Panel ID="fd_day" runat="server" Visible="false"  >
                                                    <div class="form-group row">
                                                                                             <label class="col-sm-2 col-form-label text-primary ">Day Of Deposit</label>          
                                                        <div class="col-sm-2">
                                           <asp:TextBox ID="txtday" runat="server" CssClass="form-control " AutoPostBack="true" data-validation-engine="validate[required,custom[integer]]" data-errormessage-value-missing="Day Missing" ></asp:TextBox>
                                                            </div>
                                                        <div class="form-check form-check-inline">
                                                            <asp:Panel ID="runMaturedDepPanel" runat="server" Visible="false">
                                                                <label class="form-check-label">
							<asp:CheckBox  runat="server"  ID="runMaturedDepCheckBox"   AutoPostBack="true"  />
							Run Interest for Matured Deposit
<i class="input-frame"></i></label>
                                                            </asp:Panel>
        </div>
                                                    </div>
                                                                            </asp:Panel>


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
            Processing.....
        </div>
    </div>
</ProgressTemplate>
</asp:UpdateProgress>


                                                    <asp:UpdatePanel runat="server" ID="pnlbtn">
                                                        <ContentTemplate>

                                                    <div class="form-group row border-sm-top">
                                                        <div class="col-sm-4"></div>
                                                        <div class="cols-sm-4">

                                                            <div class="btn-group " role="group">
                                        
                         <asp:Button ID="btnclr" runat ="server" CssClass =" btn btn-outline-secondary  " Text ="clear" />


                         <asp:Button ID="btnSubmit" runat ="server"  CausesValidation="False"  CssClass =" btn btn-outline-primary"  Text ="Update" />
                                                                </div>
                                                        </div>
                                                    </div>

                                                            
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>

                                               
                                               </div>


                                            



                                            <div id="tab-2" class="tab-pane " role="tabpanel">

                                                                
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                                                
                                                <div class="form-group row ">
                                                    <label class="col-sm-2 col-form-label ">Date</label>
                                                    <div class="col-sm-2">
                                                                <asp:TextBox ID="txtidate" runat="server" CssClass="form-control " data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                    </div>
                                                    <label class="col-sm-2 col-form-label ">Account</label>
                                                    <div class="col-sm-3">
                                                        <div class="input-group ">
                                                        <asp:TextBox ID="txtproduct" runat="server" CssClass="form-control "  data-validation-engine="validate[required]" data-errormessage-value-missing="Product Missing"></asp:TextBox>
                                                            <span class="input-group-btn input-group-append">
                                                                <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                                
                                                                
                                                            </span>
                                                            </div>
                                                    </div>
                                                    
                                                </div>


                                             
        

                   <table style="margin-left:0px;width:100%" class="table table-bordered "> 
                                 <thead>
                                     <tr>
                                    <th style="width:8%;">S.No</th>
                                     <th style="text-align:center;width:10%;" >Date</th>
                                    <th style="text-align:center;width:10%;" >Account No</th> 
                                         <th style="text-align:center;width:22%;" >Name</th>   
                                    <th style="text-align:center;width:12%;" >Amount</th>
                                     <th style="text-align:center;width:8%;" >Period</th>
                                     <th style="text-align:center;width:8%;" >Roi</th>
                                         <th style="text-align:center;width:11%;" >Interest</th> 
                                  <th style="text-align:center;width:11%;"  >By cash</th> 

                                            </tr>
                        
                                    </thead>
                             </table>
    
                          

                       

                             
                                 <asp:UpdatePanel ID="jspc" runat="server">
                                 <ContentTemplate >
                             
                               <asp:GridView  runat="server" ID="disp"  AutoGenerateColumns="false" style="margin-top:-2px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true"  OnPageIndexChanging="disp_PageIndexChanging"  PageSize="100"  OnRowDataBound ="gv_dep_RowDataBound"    OnSelectedIndexChanged ="OnSelectedIndexChanged_gv_grp" 
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "   >
                            <Columns>

                                   <asp:TemplateField HeaderText="S No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="8%" />
            </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblitm" runat="server" text='<%#Eval("acdate", "{0:dd-MM-yyyy}")%>' Style="text-align:center   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                     
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblacn" runat="server" text='<%#Eval("acn")%>' Style="text-align:left ;float:left "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                    
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblname" runat="server" text='<%#Eval("name")%>' Style="text-align:left ;float:left "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="22%" />
                                </asp:TemplateField>
                   

                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblgross" runat="server" text='<%#Eval("amt", "{0:N}")%>' Style="text-align:right;float:right"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lbliprd" runat="server" text='<%#Eval("prd")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblicint" runat="server" text='<%#Eval("cint", "{0:N}")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblicamt" runat="server" text='<%#Eval("camt", "{0:N}")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="11%" />

                                </asp:TemplateField >
                                <asp:TemplateField  >
                                    <ItemTemplate >
                                        <asp:Label ID="LBLbycas" runat="server" Text ='<%#Eval("damt", "{0:N}")%>' style="text-align:right;float:right  "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="11%" />
                                </asp:TemplateField>

                   </Columns> 
                                </asp:GridView>
                                     
                                      </ContentTemplate>
                                 </asp:UpdatePanel>

                                 <table style="margin-left:0px;width:100%" class="table table-bordered text-primary  "> 
                                 <tbody>
                                     <tr style="text-align:center;">
                                     <td style="text-align:center;width:70%" >
                                        <p>Total</p>

                                     </td>
                                      <td style="text-align:center;width:15%" >
                                          <asp:UpdatePanel runat="server" >
                                              <ContentTemplate>
                                         <asp:Label runat="server" ID="lblnet" style="float:right;text-align:right " Text="0"></asp:Label>
                                                  </ContentTemplate>
                                              </asp:UpdatePanel>
                                         </td>
                                         <td style="text-align:center;width:15%" >
                                             <asp:UpdatePanel runat="server" >
                                              <ContentTemplate>
                                         <asp:Label runat="server" ID="lblbyc" Width="100%" style="float:right;text-align:right " Text="0"></asp:Label>
                                                  </ContentTemplate>
                                              </asp:UpdatePanel>
                                         
                                         </td>
                                          

                                            </tr>
                                     </tbody>
                                     </table> 

                       <asp:UpdatePanel runat="server" >
                                     <ContentTemplate >
                                 <asp:Panel ID="int_revised" runat="server" Visible="false" >

                                     <div class="card card-body ">
                                         <div class="form-group row">
                                             <label class="col-form-label ">Account No</label>
                                             <div class="col-md-2">
                                                 <asp:TextBox ID="int_rev_acn" runat="server" CssClass="form-control "  ></asp:TextBox>
                                             </div>
                                             <label class="col-form-label ">Interest</label>
                                             <div class="col-md-2">
                                                 <asp:TextBox ID="int_rev_amt" runat="server" CssClass="form-control "  ></asp:TextBox>
                                             </div>
                                             <asp:Label ID="byc_caption" runat="server" CssClass="col-form-label" Text="By Cash" ></asp:Label>
                                             <div class="col-md-2">
                                                 <asp:TextBox ID="byc_rev" runat="server" CssClass="form-control "  ></asp:TextBox>
                                                 </div>
                                             <div class="col-md-2">
                                                  <asp:Button CssClass="btn btn-outline-primary " ID="btn_revised" runat="server"    CausesValidation ="False"  Text ="Update INTEREST" />
                                             </div>
                                         </div>
                                     </div>

                                     
                                 </asp:Panel>
                                     </ContentTemplate>
                                 </asp:UpdatePanel>

                                 <div class="form-group row border-sm-bottom"></div>
                                 <div class="form-group row ">
                                     <div class="col-sm-4"></div>
                                     <div class="col-sm-4">
                                         <div class="btn-group " role="group" >

                                  <asp:Button CssClass="btn btn-outline-secondary  " id="btn_post_clr" runat="server"    OnClientClick ="return detach()" Text ="Cancel" />   

                                           <asp:Button CssClass="btn btn-outline-primary " ID="btn_post" runat="server"   CausesValidation ="False"  Text ="Update" />
                   
                                         </div>

                                         </div>
                                     </div>
                                 
                 </ContentTemplate>
        </asp:UpdatePanel>



                      
                          

                                     
                                  


                                            </div>

                                            
                                            



                                            <div id="tab-3" class="tab-pane " role="tabpanel" >


                                                <asp:UpdatePanel runat="server" >
                                                    <ContentTemplate>
                                                <div class="form-group row ">
                                                    <label class="col-sm-1 col-form-label text-primary ">Date</label>
                                                    <div class="col-sm-2">
                                                             <asp:TextBox ID="txt_si_date" runat="server" CssClass="datepicker form-control " Style="margin-left: -7px"  Height="38px"  Width="130px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                    </div>
                                                    <label class="col-sm-1 col-form-label text-primary ">Account</label>
                                                    
                                                    <div class="col-sm-2">
                                                         <asp:TextBox ID="txt_si_prod" runat="server" CssClass="form-control " Style="margin-left: -7px"  Width="130px" Height="38px"  data-validation-engine="validate[required]" data-errormessage-value-missing="Product Missing"></asp:TextBox>
                                                    </div>
                                                    <label class="col-sm-1 col-form-label text-primary ">Day</label>
                                                    <div class="col-sm-3">
                                                        <div class="input-group">
                                                        <asp:TextBox ID="txt_si_day" runat="server" CssClass="form-control " Style="margin-left: -7px"  Width="130px"  Height="38px"  data-validation-engine="validate[required]" data-errormessage-value-missing="Product Missing"></asp:TextBox>

                                                          <span class="input-group-btn input-group-append">
                                                                <asp:LinkButton runat="server" id="btn_si" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                            </div>
                                                    </div>
                                                    

                                                </div>


                                  
                                    
                                         <table style="margin-left:0px;width:80%" class="table table-bordered "> 
                                 <thead>
                                     <tr style="text-align:center;">
                                     <th style="text-align:center;width:18%;" >Transfer From</th>
                                     <th style="text-align:center;width:15%;" >Transfer to</th>
                                     <th style="text-align:center;width:20%;" >Amount</th>
                                    <th style="text-align:center;width:17%;" >
                                        <asp:UpdatePanel runat="server" >
                                            <ContentTemplate>
                                                <asp:Label ID="lbl_bycas" runat="server" Text ="By Cash" Width="100%" ></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </th>                                             </tr>
                        
                                    </thead>
                             </table>
                                    
                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                 <ContentTemplate >
                             
                               <asp:GridView  runat="server" ID="disp_si" AutoGenerateColumns="false" style="margin-top:-2px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true"  OnPageIndexChanging="OnPagingsi" PageSize="2000"
                             Width="80%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>


                                                     
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblacn" runat="server" text='<%#Eval("acn")%>' Style="text-align:left ;float:left "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="18%" />
                                </asp:TemplateField>
                    
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblsiacno" runat="server" text='<%#Eval("siacno")%>' Style="text-align:left ;float:left "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                   
                   
                                
                                
                                
                                             <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lbl_cint" runat="server" text='<%#Eval("camt", "{0:N}")%>' Style="text-align:right;float:right"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>

                            
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lbl_dint" runat="server" text='<%#Eval("damt", "{0:N}")%>' Style="text-align:right;float:right   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="17%" />
                                </asp:TemplateField>
                                  <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                  <asp:Label ID="lblacdate" runat="server" text='<%#Eval("acdate", "{0:dd-MM-yyyy}")%>' Visible="false"  Style="text-align:center   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="1px" />
                                </asp:TemplateField>
                    


                   </Columns> 
                                </asp:GridView>
                                 
                                                                  

                                     </ContentTemplate>

                                 </asp:UpdatePanel>

                                 <table style="margin-left:00px;width:80%" class="table table-bordered  text-primary  "> 
                                 <tbody>
                                     <tr style="text-align:center;">
                                     <td style="text-align:center;width:33%" >
                                        <p>Total</p>

                                     </td>
                                         <td style="text-align:center;width:20%" >
                                          <asp:UpdatePanel runat="server" >
                                              <ContentTemplate>
                                         <asp:Label runat="server" ID="lbl_si_cint_total" style="float:right;text-align:right " Text="0"></asp:Label>
                                                  </ContentTemplate>
                                              </asp:UpdatePanel>
                                         </td>
                                      
                                      <td style="text-align:center;width:17%" >
                                          <asp:UpdatePanel runat="server" >
                                              <ContentTemplate>
                                         <asp:Label runat="server" ID="lbl_si_bycas_total" style="float:right;text-align:right " Text="0"></asp:Label>
                                                  </ContentTemplate>
                                              </asp:UpdatePanel>
                                         </td>
                                          

                                            </tr>
                                     </tbody>

                                     </table> 

                                                <div class="form-group row border-sm-bottom "></div>
                                                <div class="form-group row ">
                                                    <div class="col-sm-4"></div>
                                                    <div class="col-sm-4">
                                                        <div class="btn-group " role="group">
                                                              <asp:Button CssClass =" btn btn-outline-secondary  " ID="btn_si_clr" runat="server" CausesValidation ="false"   Text ="Clear"/>
                                                        <asp:Button CssClass="btn btn-outline-primary  " ID="btn_si_up" runat="server"  CausesValidation ="false"  Text="Update" />
                                          
                                                        </div>
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
                

               

            </form>

    
      
	    <script src="../js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/daterangepicker.js"></script>
	<script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
    

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


            $("#<%=tdate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=tdate.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

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

            $("#<%=txt_si_date.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=txt_si_date.ClientID%>").val(start.format('DD-MM-YYYY'));
            });



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
   
     <style>

        
        
        .sensitive {
            display: block ;
        }

    </style>

    <script type="text/javascript" >

        function showTab() {
           // $(".sensitive").toggle();
        }


    </script>

 

</asp:Content>
