<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="itforms.aspx.vb" Inherits="fiscus.itforms" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="frmitforms" runat="server">
                  <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
          
    

          

        <div class="center-box">
        <div class="info-form-box-loan ">
<div class="top-bar">
                     <span class="promise-line">Form 15G (Consolidated)</span> 
                  </div>
            <section >
                        <div class="div-spread ">


            <cc1:tabcontainer ID="tabcontainer1" runat="server" CssClass="Tab"     Visible="true"  ActiveTabIndex="0" Width="100%"  >

                   <cc1:TabPanel id="formtab" runat="server" style="margin-left:0px" HeaderText="In">
                    <HeaderTemplate>Form Selection</HeaderTemplate>
                                           <ContentTemplate>


                                         <div  style="float:left;margin-top:20px">

                                             <table style="margin-left: 30px">
                                                 <tbody>
                                                     <tr>

                                                         <td style="border: none">
                                                         <div class="input-daterange input-group" id="datepicker">
                                                             <span class="input-group-addon" style="margin-left:5px;width:55px; height: 37px">From</span>
                                                             <asp:TextBox ID="txtfrm" style="margin-left:-7px" Height="37px"   CssClass="datepicker  form-control " runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                             <span class="input-group-addon" style="width: 40px; height: 37px">To</span>
                                                             <asp:TextBox ID="txtto" CssClass="datepicker form-control  "  Height="37px"  Style="margin-left: -7px" runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                                                  <span class ="input-group-btn " >
    <asp:ImageButton runat="server" style="margin-left:0px" Width ="50px" Height="37px"  ImageUrl="~/images/search_logo.png" id="btn_inw" CssClass="btn btn-outline-secondary  btn" CausesValidation="False"  />
                                                                                          </span>
                                                         </div>
                                                         </td>
                                                                                                              </tr>
                                                 </tbody>
                                             </table>
                                             <br />

                                                                                               <div class="GridviewDiv" >
                              <table style="border:1px;width:95%;margin-left:30px;float:left "  class="GridviewTable">
                                  <tbody>

                                         <tr style="color:white">
                                             <td style="width:3%;text-align:center;color:white;font-variant:small-caps "></td>
                                             <td style="width:5%;text-align:center;color:white;font-variant:small-caps ">S.No</td>
                                         <td style="width:15%;text-align:center;color:white;font-variant:small-caps ">Member No</td>
                                         <td style="width:25%;text-align:center;color:white;font-variant:small-caps">Name</td>
                                         <td style="width:17%;text-align:center;color:white;font-variant:small-caps">Interest</td>
                                             <td style="width:5%;text-align:center;color:white;font-variant:small-caps">26AS</td>
                                                                                  </tr>
                                      </tbody>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          
                                          <Triggers>
                                              <asp:PostBackTrigger ControlID="gvin" />
                                          </Triggers>
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvin" AutoGenerateColumns="false" style="margin-top:0px;margin-left:30px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingin" PageSize="125" EmptyDataText="No Data Found" OnRowDataBound="gvin_RowDataBound"  OnSelectedIndexChanged="OnSelectedIndexChangedgvin" 
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="customers "  >
                            <Columns>

<asp:TemplateField>
   <HeaderTemplate>
    <asp:CheckBox runat="server" ID="cbHeader"  />
   </HeaderTemplate>
   <ItemTemplate>
         <asp:RadioButton ID="cur_sel" AutoPostBack ="true"  style="align-content:stretch "  runat="server" OnCheckedChanged ="cur_sel_CheckedChanged" OnClick="SelectSingleRadiobutton(this.id)"/>
                                            <asp:HiddenField id ="hf"  runat="server" Value ='<%#Eval("Memberno")%>' />
                                          
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
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("Memberno")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("Firstname")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="25%" />

                                        </asp:TemplateField>
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("interest", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="17%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                    
                                          <ItemTemplate>
       
    <asp:CheckBox runat="server" ID="itmchk" OnCheckedChanged="tds_chked"  CssClass ="custom-checkbox " AutoPostBack="true"   style="margin-top:-5px;margin-left:1px;align-content:center  " />
       
   </ItemTemplate>
                           <ItemStyle Width="5%" HorizontalAlign="Center"  />

                                    
                                </asp:TemplateField>
                        
                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                                                                   
                                    <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>
                                             <table style="border:1px;width:95%;margin-left:30px;float:left " class="customers alt">
                                  <tbody>
                                      <tr>
                                          <td style="width:48%">Total

                                          </td>
                                           <td style="width:17%">
                                              <asp:Label ID="lbldsum" runat="server" style="float:right " ></asp:Label>
                                          </td>
                                          <td style="width:5%">
                                              <asp:Label ID="lblmsum" runat="server" style="float:left  "></asp:Label>
                                          </td>
                                      </tr>
                                    </tbody>

                                                 </table> 

                                            <asp:UpdatePanel runat="server" >
                                                <ContentTemplate>
                                                       <asp:Button runat="server" ID="btn_update_26as" CssClass="btn btn-outline-primary "  style="margin-left:30px;margin-top:20px" Visible="false"   Text ="Update" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                                                                                                                     
                                                                                </div>

                                             
             </div>       

             </ContentTemplate> 
                       </cc1:TabPanel>

                   <cc1:TabPanel ID="tab_income" runat ="server" style="margin-left:0px">
    <HeaderTemplate >Income Details</HeaderTemplate>
    <ContentTemplate>

        <asp:UpdatePanel runat="server" ID="incp">

            <ContentTemplate>

                <div class ="div-spread ">

                    <div class ="input-group ">
                          
                                                       
                    
                        
                        <span class="input-group-addon" style="margin-left:72px;width:138px; height: 37px">Form To Submit</span>
                    
                            <span class="input-group-addon" style="margin-left:-8px;width:75px; height: 37px">15G
                    <asp:RadioButton runat="server" AutoPostBack="true"   ID="formg"/> 
                        </span>
                        <span class="input-group-addon" style="margin-left:-8px;width:75px; height: 37px">15H
                            <asp:RadioButton runat="server" AutoPostBack="true"   ID="formh"  />
                        </span>
                    </div>
                </div>


        <table class="table_box ">
            <tbody >
                                   
                <tr style="border:none">
                    <td style="font-size:smaller;width:250px">Member No</td>
                    <td style="width:200px"><asp:TextBox ID="txtuid" CssClass="form-control " Width="150px" runat="server" ></asp:TextBox></td> 
                    <td style="font-size:smaller;width:250px">Name of the Assessee</td>
                    <td style="width:200px"><asp:TextBox ID="txtname" CssClass="form-control " Width="150px" runat="server" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size:smaller ;width:150px">PAN of the Assessee</td>
                    <td style="width:200px"><asp:TextBox ID="txtpan" CssClass="form-control " Width="150px" runat="server" ></asp:TextBox></td>
                    <td style="font-size:small;width:150px">Status</td>
                    <td style="width:200px"><asp:TextBox ID="txtstatus" CssClass="form-control " Width="150px" runat="server" ></asp:TextBox></td> 
                </tr>
                <tr>
                    <td style="font-size:smaller;">Residential Status</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtres" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller;">Previous year(P.Y) (for which declaration is being made)</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtpy" CssClass="form-control " Width="150px"></asp:TextBox> </td>
                </tr>
                <tr>
                    <td style="font-size:smaller;">Flat/Door/Block No.</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtdoor" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size :smaller ">Road/Street/Lane</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtroad" CssClass="form-control " Width="150px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size:smaller " >Name of  premises</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtpremises" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller ">Area/Locality</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtarea" CssClass="form-control " Width="150px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size:smaller ">Town/City/ District</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtcity" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller ">State</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtstate" CssClass="form-control " Width="150px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size:smaller">PIN</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtpin" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller ">Email</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtemial" CssClass="form-control " Width="150px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size:smaller ">STD Code</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtstd" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller ">Telephone No</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtphone" CssClass="form-control " Width="150px"></asp:TextBox></td>
                </tr>

                <tr>
                    <td style="font-size:smaller ">Mobile No.</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtmobile" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller ">Whether assessed  to tax under the  Income-tax Act,1961</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txttax" CssClass="form-control " Width="150px"></asp:TextBox></td>
                                    </tr>
                <tr>
                    <td style="font-size:smaller " >If yes, latest assessment year for which assessed</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txttaxyr" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller ">Estimated income  for which  this declaration is made</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtincome" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    
                </tr>
                <tr>
                    <td style="font-size:smaller ">Estimated total income of the P.Y.  in which Estimated income for which  this declaration is made to be included.</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txttotalincome" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller ">Total No. of Form No. 15G filed</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txt15gno" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    
                </tr>
                <tr>
                    <td style="font-size:smaller ">Aggregate amount of  income for which Form No.15G filed</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txt15gamt" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td  style="font-size:smaller ">Date on which  Declaration is received  (DD/MM/YYYY)</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txt_dcl_date" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    
                </tr>
                <tr>
                    <td style="font-size:smaller ">Amount of income paid</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtincomepaid" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    <td style="font-size:smaller ">Date on which the  income has been  paid/credited (DD/MM/YYYY)</td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtincomepaiddate" CssClass="form-control " Width="150px"></asp:TextBox></td>
                    
                </tr>
                <tr>
                    <td style="font-size:smaller "><asp:Label runat="server" ID="lbldob" Visible="false"  Font-Size="Smaller" Text="D.O.B"></asp:Label></td>
                    <td style="width:200px"><asp:TextBox runat="server" ID="txtdob" CssClass="form-control " Visible="false"   Width="150px"></asp:TextBox></td>
                </tr>
            </tbody>
            
        </table>

                </ContentTemplate>
            </asp:UpdatePanel>


                                                                                         <div class="GridviewDiv" >
                              <table style="border:1px;width:60%;margin-left:70px;float:left "  class="GridviewTable">
                                  <tbody>

                                         <tr style="color:white">
                                             <td style="width:10%;text-align:center;color:white;font-variant:small-caps ">S.No</td>
                                         <td style="width:15%;text-align:center;color:white;font-variant:small-caps ">Deposit</td>
                                         <td style="width:20%;text-align:center;color:white;font-variant:small-caps">Account No</td>
                                         <td style="width:20%;text-align:center;color:white;font-variant:small-caps">Interest</td>
                                                                                  </tr>
                                      </tbody>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
              
                      <asp:GridView  runat="server" ID="gv_income" AutoGenerateColumns="false" style="margin-top:0px;margin-left:70px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingin" PageSize="50" EmptyDataText="No Data Found" OnRowDataBound="gvin_RowDataBound"  OnSelectedIndexChanged="OnSelectedIndexChangedgvin" 
                             Width="60%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="customers "  >
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
                           <ItemStyle Width="8%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                                 
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblproduct" runat="server" text='<%#Eval("product")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblacno" runat="server" text='<%#Eval("acno")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblamt" runat="server" text='<%#Eval("amount", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>

          
                                 </Columns>
                            </asp:GridView>
                                                       <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>
                                             <table style="border:1px;width:60%;margin-left:70px;float:left " class="customers alt">
                                  <tbody>
                                      <tr>
                                          <td style="width:70%">Total</td>
                                          <td style="width:30%">
                                              <asp:Label ID="lbl_incom_total" runat="server" style="float:right "></asp:Label>
                                          </td>
                                      </tr>
                                    </tbody>

                                                 </table> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>   
    </ContentTemplate>                                          
                                              </asp:UpdatePanel> 
                                                                                             </div> 

        <div class ="sub-titles ">

        </div>
        <div class="div-spread " style="margin-left:70px" >
            <asp:Button ID="Button1" runat="server" CssClass="btn btn-outline-secondary  " Text="Clear" CausesValidation="false"  />
            <asp:Button ID="btn_update" runat="server" CssClass="btn btn-primary " Text="UPDATE" CausesValidation="true" />
        </div>
    </ContentTemplate>
</cc1:TabPanel>
                   <cc1:TabPanel ID="tabforms" runat="server" style="margin-left:0px" HeaderText="sf">
                    <HeaderTemplate>Saved Forms</HeaderTemplate>
                    <ContentTemplate >

                                                                                                       <div class="GridviewDiv" >
                              <table style="border:1px;width:95%;margin-left:30px;float:left "  class="GridviewTable">
                                  <tbody>

                                         <tr style="color:white">
                                             <td style="width:8%;text-align:center;color:white;font-variant:small-caps ">S.No</td>
                                         <td style="width:23%;text-align:center;color:white;font-variant:small-caps">UID</td>
                                         <td style="width:22%;text-align:center;color:white;font-variant:small-caps">Name</td>
                                         <td style="width:10%;text-align:center;color:white;font-variant:small-caps">Pan</td>
                                          <td style="width:10%;text-align:center;color:white;font-variant:small-caps">Income Paid</td>
                                             

                                                                                  </tr>
                                      </tbody>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gv_saved" AutoGenerateColumns="false" style="margin-top:0px;margin-left:30px;float:left;z-index:1" 
                            AllowPaging ="false"   PageSize="500" EmptyDataText="No Data Found" 
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="customers "  >
                            <Columns>

                                <asp:TemplateField>
   <ItemTemplate>

                                         <%# Container.DataItemIndex + 1 %>

   </ItemTemplate>
                           <ItemStyle Width="8%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                           
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("UID")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="23%" />

                                        </asp:TemplateField>
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("NAME")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="22%" />

                                        </asp:TemplateField>

                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("pan")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>

                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("estimateincome", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>

                            

                        
                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                                                                   
                                    <asp:UpdatePanel runat="server"  Visible="false" >
                                        <ContentTemplate>
                                             <table style="border:1px;width:95%;margin-left:30px;float:left " class="customers alt">
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
                                                                                </div>
                    </ContentTemplate>
                </cc1:TabPanel>

                  <cc1:TabPanel ID="f26as" runat="server" style="margin-left:0px" HeaderText="F26AS" >

                      <HeaderTemplate>
                          Form 26AS
                      </HeaderTemplate>
                      <ContentTemplate>

                                <div  style="float:left;margin-top:20px">

                                             <table style="margin-left: 10px">
                                                 <tbody>
                                                     <tr>

                                                         <td style="border: none">
                                                            <div class="input-group ">
                                                             <span class="input-group-addon" style="margin-left:5px;width:65px; height: 37px">Quater</span>
                                                             <asp:TextBox ID="txtqtr" style="margin-left:-7px" Height="37px"   CssClass="form-control " runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                             <span class="input-group-addon" style="width: 50px; height: 37px">Year</span>
                                                             <asp:TextBox ID="txtyr" CssClass="form-control  "  Height="37px"  Style="margin-left: -7px" runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                                                  <span class ="input-group-btn " >
    <asp:ImageButton runat="server" style="margin-left:0px" Width ="50px" Height="37px"  ImageUrl="~/images/search_logo.png" id="btn_as" CssClass="btn btn-outline-secondary  btn" CausesValidation="False"  />
    <asp:Button runat="server" style="height:37px" Text ="Export" ID="btnexp" />
                                                                                          </span>
                                                        </div>                                                      
                                                         </td>
                                                                                                              </tr>
                                                 </tbody>
                                             </table>
                                             <br />

                                                                                               <div class="GridviewDiv" >
                              <table style="border:1px;width:95%;margin-left:30px;float:left "  class="GridviewTable">
                                  <tbody>

                                         <tr style="color:white">
                                             
                                             <td style="width:5%;text-align:center;color:white;font-variant:small-caps ">S.No</td>
                                         <td style="width:12%;text-align:center;color:white;font-variant:small-caps ">Member No</td>
                                         <td style="width:25%;text-align:center;color:white;font-variant:small-caps">Name</td>
                                         <td style="width:13%;text-align:center;color:white;font-variant:small-caps">PAN</td>
                                             <td style="width:15%;text-align:center;color:white;font-variant:small-caps">Interest</td>
                                             <td style="width:12%;text-align:center;color:white;font-variant:small-caps">TDS</td>
                                             <td style="width:5%;text-align:center;color:white;font-variant:small-caps">PAID</td>
                                            
                                                                                  </tr>
                                      </tbody>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <Triggers>
                                              <asp:PostBackTrigger ControlID="disp_as" />
                                          </Triggers>
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="disp_as" AutoGenerateColumns="false" style="margin-top:0px;margin-left:30px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingin" PageSize="50" EmptyDataText="No Data Found" OnRowDataBound="gvin_RowDataBound"  OnSelectedIndexChanged="OnSelectedIndexChangedgvin" 
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="customers "  HeaderStyle-BackColor ="#09c" >
                            <Columns>


                                <asp:TemplateField>
   <ItemTemplate>

                                         <%# Container.DataItemIndex + 1 %>

   </ItemTemplate>
                                  
                           <ItemStyle Width="5%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                           
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblmno" runat="server" text='<%#Eval("Memberno")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="12%" />
                                    
                                        </asp:TemplateField>
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" text='<%#Eval("name")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="25%" />
                                     
                                        </asp:TemplateField>

                                          <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblpan" runat="server" text='<%#Eval("pan")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="13%" />
                           
                                        </asp:TemplateField>


                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblint" runat="server" text='<%#Eval("interest", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />
                       
                                        </asp:TemplateField>

                    <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lbltds" runat="server" text='<%#Eval("tds", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="12%" />
                          
                                        </asp:TemplateField>
                                 
                                <asp:TemplateField >
                                     <ItemTemplate>
       
    <asp:CheckBox runat="server" ID="paidchk" OnCheckedChanged="paid_chked"  CssClass ="custom-checkbox " AutoPostBack="true"   style="margin-top:-5px;margin-left:1px;align-content:center  " />
       
   </ItemTemplate>
                           <ItemStyle Width="5%" HorizontalAlign="Center"  />

                                    
                                </asp:TemplateField>
                               

             

                        
                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                                                                   
                                    <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>
                                             <table style="border:1px;width:95%;margin-left:30px;float:left " class="customers alt">
                                  <tbody>
                                      <tr>
                                          <td style="width:54%">Total</td>
                                          <td style="width:15%">
                                              <asp:Label ID="lblinttotal" runat="server" style="float:right " ></asp:Label>
                                          </td>
                                          <td style="width:12%">
                                              <asp:Label ID="lbltdstotal" runat="server" style="float:right   "></asp:Label>
                                          </td>
                                         <td style="width:5%">
                                             
                                          </td>
                                       
                                           
                                      </tr>
                                    </tbody>

                                                 </table> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                                                                                                                     
                                                                                </div>


             </div>       

                         
                          <div class="div-spread ">
                               <br />
                          <br />

                              <asp:UpdatePanel runat="server" ID="pnltdspaid" Visible="false" >
                                  <Triggers>
                                      <asp:PostBackTrigger ControlID="disp_as" />
                                  </Triggers>
                                  <ContentTemplate>
                                      
                                      <div class="form-row col-md-12">
                                          <div class="col-md-3">
                                          <label for="txttdspaid">Amount</label>
                                          <asp:TextBox runat="server" ID="txttdspaid" CssClass="form-control " ReadOnly="true" ></asp:TextBox>
                                      </div>

                                           <div class="col-md-3">
                                          <label for="ddled">Ledger</label>
                                          <asp:DropDownList CssClass="custom-select " Width="250px" runat="server" ID="ddled">
                                              <asp:ListItem>Select</asp:ListItem>
                                          </asp:DropDownList>
                                      </div>
                                        
                                           <div class="col-md-3">
                                          <label for="ddbank">Bank</label>
                                          <asp:DropDownList CssClass="custom-select " Width="250px" runat="server" ID="ddbank">
                                              <asp:ListItem>Select</asp:ListItem>
                                          </asp:DropDownList>
                                      </div>
                                           <div class="col-md-2">
                                          <label for="tdate">Date</label>
                                          <asp:TextBox runat="server" ID="tdate" CssClass="form-control "  ></asp:TextBox>
                                      </div>


                                          <div class="col-md-1">
                                              <asp:Button ID="btn_upadate_tds" runat="server" CssClass="btn btn-outline-primary " style="margin-top:35px" Text="Update" />
                                          </div>
                                        
                                          </div>
                                        
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                               <br />
                                      <br />
                                     
                              <asp:UpdatePanel ID="pnlbrnch" runat="server" Visible="false" style="margin-top:60px"  >
                                  <ContentTemplate>
                                       <div class="form-row col-md-12">
                                             <div class="col-md-2">
                                             <label for="txtvky">VKY</label>
                                          <asp:TextBox runat="server" ID="txtvky" CssClass="form-control " AutoPostBack="true"   ></asp:TextBox>
                                                          </div>
                                            <div class="col-md-2">
                                             <label for="txtmkm">MKM</label>
                                          <asp:TextBox runat="server" ID="txtmkm" CssClass="form-control " AutoPostBack="true" ></asp:TextBox>
                                                          </div>
                                            <div class="col-md-2">
                                             <label for="txtal">AL</label>
                                          <asp:TextBox runat="server" ID="txtal" CssClass="form-control " AutoPostBack="true" ></asp:TextBox>
                                                          </div>
                                            <div class="col-md-2">
                                             <label for="txtpdm">PDM</label>
                                          <asp:TextBox runat="server" ID="txtpdm" CssClass="form-control " AutoPostBack="true" ></asp:TextBox>
                                                          </div>

                                            <div class="col-md-2">
                                             <label for="txttotal">Total</label>
                                          <asp:TextBox runat="server" ID="txttotal" CssClass="form-control " ReadOnly="true" ></asp:TextBox>
                                                          </div>

                                   </div> 
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                          </div>

                      </ContentTemplate>

                  </cc1:TabPanel>

                        <cc1:TabPanel ID="TabPanel2" runat="server" style="margin-left:0px" HeaderText="TDS RECEIVABLE" >

                      <HeaderTemplate>
                         TDS RECEIVABLE
                      </HeaderTemplate>
                      <ContentTemplate>
                            <div  style="float:left;margin-top:20px">

                                           
                                             <br />

                                                                                               <div class="GridviewDiv" >
                              <table style="border:1px;width:95%;margin-left:30px;float:left "  class="GridviewTable">
                                  <tbody>

                                         <tr style="color:white">
                                             
                                      <td style="width:5%;text-align:center;color:white;font-variant:small-caps ">S.No</td>
                                      <td style="width:12%;text-align:center;color:white;font-variant:small-caps">Date</td>
                                      <td style="width:12%;text-align:center;color:white;font-variant:small-caps ">Member No</td>
                                       <td style="width:25%;text-align:center;color:white;font-variant:small-caps">Name</td>
                                       <td style="width:13%;text-align:center;color:white;font-variant:small-caps">Debit</td>
                                       <td style="width:13%;text-align:center;color:white;font-variant:small-caps">CREDIT</td>
                                       <td style="width:13%;text-align:center;color:white;font-variant:small-caps">BALANCE</td>      
                                            
                                                                                  </tr>
                                      </tbody>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                         
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="disp_tds" AutoGenerateColumns="false" style="margin-top:0px;margin-left:30px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingin" PageSize="50" EmptyDataText="No Data Found" OnRowDataBound="gvin_RowDataBound"  OnSelectedIndexChanged="OnSelectedIndexChangedgvin" 
                             Width="95%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="customers "  HeaderStyle-BackColor ="#09c" >
                            <Columns>


                                <asp:TemplateField>
   <ItemTemplate>

                                         <%# Container.DataItemIndex + 1 %>

   </ItemTemplate>
                                  
                           <ItemStyle Width="5%" HorizontalAlign="Center"  />

  </asp:TemplateField>



                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblmno" runat="server" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="12%" />
                                    
                                        </asp:TemplateField>
                      
                           
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblmno" runat="server" text='<%#Eval("Memberno")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="12%" />
                                    
                                        </asp:TemplateField>
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" text='<%#Eval("name")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="25%" />
                                     
                                        </asp:TemplateField>

                                          <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblpan" runat="server" text='<%#Eval("debit", "{0:N}")%>' Style="text-align:left;float:right   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="13%" />
                           
                                        </asp:TemplateField>


                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblint" runat="server" text='<%#Eval("credit", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="13%" />
                       
                                        </asp:TemplateField>

                    <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lbltds" runat="server" text='<%#Eval("balance", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="13%" />
                          
                                        </asp:TemplateField>
                                 
                      
                        
                                 </Columns>
                            </asp:GridView>

                                          </ContentTemplate>
                                          </asp:UpdatePanel>                            
                                                                                                   
                                    <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>
                                             <table style="border:1px;width:95%;margin-left:30px;float:left " class="customers alt">
                                  <tbody>
                                      <tr>
                                          <td style="width:53%">Total</td>
                                          <td style="width:13%">
                                            <asp:Label ID="lbldr" runat="server" style="float:right   "></asp:Label>
                                          </td>
                                          <td style="width:13%">
                                              
                                                <asp:Label ID="lblcr" runat="server" style="float:right " ></asp:Label>
                                          </td>
                                         <td style="width:13%">
                                             <asp:Label ID="lblbal" runat="server" style="float:right   "></asp:Label>
                                          </td>
                                       
                                           
                                      </tr>
                                    </tbody>

                                                 </table> 
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                                                                                                                     
                                                                                </div>


             </div>       

                         
                      

                          </ContentTemplate> 
                            </cc1:TabPanel> 
                


                </cc1:tabcontainer> 
                         
                         
                            </div> 
                            </section>
            </div> 
            </div> 
    </form>
</asp:Content>