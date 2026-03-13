<%@ Page Title="Share Holders" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ShareHolders.aspx.vb" Inherits="Fiscus.ShareHolders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link runat="server" rel="shortcut icon" href="Images/favicon.ico" type="image/x-icon"/>
                <link runat="server" rel="icon" href="Images/favicon.ico" type="image/ico"/>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <form id="sharereg" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Share</a></li>
						<li class="breadcrumb-item active" >Share Holders</li>
					</ol>
				</nav>

          <div class="card card-body ">
              <h6 class="card-title text-primary  ">Share Holders List</h6>
              <asp:Updatepanel runat="server" >
                          <ContentTemplate>
                  
              <div class="form-group row">
                  
                  
                      <div class="col-md-6"></div>
                          
                      <label class="col-form-label">Shares between</label>
                              <div class="col-md-1">
                                  <asp:TextBox runat="server" ID="txtfrm" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                  
                              </div>
                      <label class="col-form-label">and</label>
                              <div class="col-md-1">
                                  <asp:TextBox runat="server" ID="txtto" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                              </div>
              
                            
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

</asp:Updatepanel>

              <asp:UpdatePanel runat="server" >
                   <Triggers >
                      <asp:PostBackTrigger ControlID ="btn_prnt" />
                  </Triggers>
                  <ContentTemplate>

                      <div id="prntarea">
                  
               <table style="border:0px;width:100%;margin-left:0px;float:left "  class="table table-bordered">
                                  <thead>

                                         <th style="width:10%;">S.No</th>
                                         <th style="width:15%;">Customer Id</th>
                                         <th style="width:25%;">Name</th>
                                         <th style="width:15%;">Shares Held</th>
                                         <th style="width:25%;">Share Value</th>

                                         
                                      </thead>
                                  </table>

                        <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1;page-break-before:auto" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="50" AlternatingRowStyle-CssClass="customers alt "
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "  >
                            <Columns>

                                <asp:TemplateField HeaderText="S No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="10%" />
            </asp:TemplateField>
                
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("cid")%>' Style="text-align:left ;float:left"></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>

                                 
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("Firstname")%>' Style="text-align:left ;float:left"></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="25%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrval" runat="server" text='<%#Eval("allocation")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("shrval")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="25%" />

                                        </asp:TemplateField>


                                </Columns>
                            </asp:GridView>


                                                                                <table style="border:1px;width:100%;margin-left:0px;float :left " class ="table table-condensed ">
                                                        <tbody >
                                                            <tr>
                                                                <td style="width:50%">Total</td>
                                                                <td style ="width:15%">
                                                                <asp:Label ID="lblshrnos" runat ="server" style="text-align:right;float:right " ></asp:Label>

                                                                </td>
                                                    <td style ="width:25%">
                                        <asp:Label ID="lblnetshrval" runat ="server" style="text-align:right;float:right  " ></asp:Label>


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
                header: "<h4>KARAVILAI NIDHI LIMITED</h4></br>Karavilai Branch</br><span>List of Share Holders</span></br>"



            });
        }

    </script>


      <%--     <style>

        .table-condensed tr th {
border: 1px solid #6e7bd9;
color: white ;
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

