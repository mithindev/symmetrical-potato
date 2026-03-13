<%@ Page Title="Cancelled Vouchers" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="CancelledVouchers.aspx.vb" Inherits="Fiscus.CancelledVouchers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <form id="canvo" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Reports</a></li>
						<li class="breadcrumb-item active" >Canceled Vouchers</li>
					</ol>
				</nav>

          <div class="card card-body ">
			  <div class="form-group row  border-bottom">

                                                   
                                                       <label  class=" col-form-label text-primary  ">From&nbsp;&nbsp;</label>
                                                       <asp:TextBox ID="txtnewfrm" CssClass="col-sm-2 form-control "   runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                   
                                                       <label  class=" col-form-label text-primary  ">&nbsp;&nbsp;To</label>
                                                   
                                                       <div class="col-sm-3">
                                                           <div class="input-group">
                                                        <asp:TextBox ID="txtnewto" CssClass=" form-control "  runat="server"  data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_new" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                               
                                                           </div>
            
                                                   </div>
                      </div>

                      <table style="width:100%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>

                                         <tr style="color:white">
                                        <th style="width:8%;text-align:center;color:white;font-variant:small-caps ">S.NO</th>
                                        <th style="width:15%;text-align:center;color:white;font-variant:small-caps ">Date</th>
                                         <th style="width:10%;text-align:center;color:white;font-variant:small-caps">T.ID</th>
                                         <th style="width:15%;text-align:center;color:white;font-variant:small-caps">Type</th>
                                         <th style="width:29%;text-align:center;color:white;font-variant:small-caps">A/C Head</th>
                                             <th style="width:13%;text-align:center;color:white;font-variant:small-caps">Amount</th>
                                             <th style="width:20%;text-align:center;color:white;font-variant:small-caps">Cancelled By</th>
                                         </tr>
                                      </thead>
                                  </table>

                                      <asp:UpdatePanel runat="server" >
                                          <ContentTemplate>
                                                                                                            
                        <asp:GridView  runat="server" ID="gvnew" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPagingnew" PageSize="50" EmptyDataText="No Data Found" EnableViewState="false" 
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
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
                                    <ItemStyle Width="15%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("tid")%>' Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("vtype")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("achead")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="29%" />

                                        </asp:TemplateField>



                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("amount", "{0:N}")%>'   Style="text-align:right   ;float:right    "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="13%" />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("rejectby")%>' Style="text-align:left;float:left  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                   </asp:TemplateField> 



                             
                                 </Columns>
                            </asp:GridView>

                                              
                                        

                                    </ContentTemplate>

                                                                </asp:UpdatePanel>
			  </div>
		  </form>

      <script src="../js/jquery.js" type="text/javascript"></script>
    
	<script src="../js/daterangepicker.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/daterangepicker.css" />
    

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

          
        }



    </script>

  
</asp:Content>
