<%@ Page Title="Share Register" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ShareRegister.aspx.vb" Inherits="Fiscus.ShareRegister" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="sharereg" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Share</a></li>
						<li class="breadcrumb-item active" >Share Register</li>
					</ol>
				</nav>

          <div class="card card-body ">
              <h6 class="card-title text-primary  ">Share Register</h6>
              
                <asp:UpdatePanel runat="server" >

                    <ContentTemplate>
                        <div class="form-group row">
                            <label class="col-form-label ">Search</label>
                            <div class="col-md-3">
                                <asp:UpdatePanel runat="server" >
                                    <ContentTemplate>
                                        <asp:TextBox runat="server" ID="txtname" CssClass="form-control " AutoPostBack="true"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </div>

                            <div class="col-md-6"></div>
                            
                               <div class="col-sm-2">
                                <asp:LinkButton runat="server" id="btn_prnt" CssClass ="btn btn-outline-primary ">
                                    
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-printer"><polyline points="6 9 6 2 18 2 18 9"></polyline><path d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2"></path><rect x="6" y="14" width="12" height="8"></rect></svg>
              </asp:LinkButton>
                 
                                <asp:LinkButton runat="server" id="btn_xl" CssClass ="btn btn-outline-primary ">
                                    
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-save"><path d="M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2z"></path><polyline points="17 21 17 13 7 13 7 21"></polyline><polyline points="7 3 7 8 15 8"></polyline></svg>
              </asp:LinkButton>
                  </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

              <asp:UpdatePanel runat="server" >

                  <Triggers >
                      <asp:PostBackTrigger ControlID ="btn_xl" />
                  </Triggers>
                  <ContentTemplate>
                      <div id="prntarea">
                <table   class="table table-bordered ">
                                  <thead>

                                         
                                             <th style="width:3%; ">S.No</th>
                                         <th style="width:11%; ">Date</th>
                                        
                                         <th style="width:8%;">C.No</th>
                                         <th style="width:23%;">Name</th>
                                         <th style="width:25%;">Address</th>
                                             <th style="width:15%;">Share Value</th>
                                         <th style="width:11%;">Shares Held</th>
                                      </thead>
                                  </table>

                        <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false"  
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="50"  
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false" style="table-layout:fixed " CssClass="table table-bordered  "  >
                            <Columns>

                                <asp:TemplateField HeaderText="S No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="6%" />
            </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="11%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("sl")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="8%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("Firstname")%>' Style="text-align:left ;float:left"></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="23%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("Lastname")%>' Font-Size="Small"  Style="text-align:left  ;float:left   "></asp:Label>
                                                <br />
                                                <asp:Label ID="Label1" runat="server" text='<%#Eval("address")%>'  Font-Size="9px"  Width="100px"  Style="text-align:left ;float:left;"></asp:Label>
                                            </ItemTemplate>
                                                                           <ItemStyle Width="25%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("shrval")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrval" runat="server" text='<%#Eval("allocation")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="11%" />

                                        </asp:TemplateField>

                                </Columns>
                            </asp:GridView>


                                                                                <table style="border:0px;width:100%;margin-left:0px;float :left " class ="table-condensed  ">
                                                        <tbody >
                                                            <tr style="height:50px;">
                                                                <td style="width:78.5%;text-align:center ">Total</td>
                                                                <td style ="width:13.5%">
                                                         <asp:Label ID="lblnetshrval" runat ="server" style="text-align:right;float:right ;padding-right:5px; " ></asp:Label>

                                                                </td>
                                                    <td style ="width:8%">
                                                              <asp:Label ID="lblshrnos" runat ="server" style="text-align:right;float:right;padding-right:5px; " ></asp:Label>

                                                                </td>
                                                                
                                                            </tr>
                                                        </tbody>
                                                    </table>

                      </div>
                  </ContentTemplate>
              </asp:UpdatePanel>

                                                                                </div>

             
        </form>
     <script src="js/printThis.js" type="text/javascript"></script>
    <script >

        function printshare() {
            $('#prntarea').printThis({
                importCSS: false,
                importStyle: true,
                header: "<h4>KARAVILAI NIDHI LIMITED</h4></br>Karavilai Branch</br><span>Share Register</span></br>"



            });
        }

    </script>

      
         <%--<style>

        .table-condensed tr th {
border: 0px solid #6e7bd9;
color: black;
background-color: #6e7bd9;
}

.table-condensed, .table-condensed tr td {
border: 1px solid #6e7bd9;
}

tr:nth-child(even) {
background: #f8f7ff
}

tr:nth-child(odd) {
background: #fff;
}
    </style>--%>

</asp:Content>
