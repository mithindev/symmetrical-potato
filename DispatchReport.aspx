<%@ Page Title="Dispatch Register" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="DispatchReport.aspx.vb" Inherits="Fiscus.DispatchReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <form id="intpost" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Reports</a></li>
						<li class="breadcrumb-item active" >Dispatch Report</li>
					</ol>
				</nav>

				<div class="row">
					<div class="card card-body ">

						        <div class="form-group row  border-bottom">

                                                   
                                                       <label  class=" col-form-label text-primary  ">From&nbsp;&nbsp;</label>
                                                       <asp:TextBox ID="txtfrm" CssClass="col-sm-2 form-control "   runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                    <label  class=" col-form-label text-primary  ">To&nbsp;&nbsp;</label>
                                    <asp:TextBox ID="txtto" CssClass="col-sm-2 form-control "   runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <label  class=" col-form-label text-primary  ">&nbsp;&nbsp;Product</label>
                                                   
                                                       <div class="col-sm-2">
                                                           <div class="input-group">
                                                        <asp:TextBox ID="txtproduct" CssClass=" form-control "  runat="server"  data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                               
                                                           </div>
            
                                                   </div>
                      </div>

                                                                                             
                              <table style="border:1px;width:90%;margin-left:0px;float:left "  class="table table-bordered">
                                  <thead>

                                         
                                             <th style="width:5%;">S.No</th>
                                         <th style="width:12%;">Date</th>
                                         <th style="width:13%;">Account No</th>
                                             <th style="width:35%;">Customer Name</th>
                                         <th style="width:10%;">Amount</th>
                                         <th style="width:15%;">Particulars</th>
                                         
                                      </thead>
                                  </table>

                        <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="50" EmptyDataText="No Data Found"  
                             Width="90%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>

                                <asp:TemplateField HeaderText="S No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="5%" />
            </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Font-Size="Small" Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("acno")%>' Font-Size="Small" Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="13%" />

                                        </asp:TemplateField>

                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("FirstName")%>'  Font-Size="Small" Style="text-align:left  ;float:left   "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="35%" />

                                        </asp:TemplateField>


                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lbldr" runat="server" text='<%#Eval("drd")%>' Font-Size="Small" Style="text-align:right  ;float:right "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="10%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("due")%>' Font-Size="Small"   Style="text-align:left    ;float:left     "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>

                                 </Columns>
                            </asp:GridView>

                            <table style="border:1px;width:90%;margin-left:0px;float :left " class ="table table-bordered  ">
                                                        <tbody >
                                                            <tr>
                                                                <td style="width:65%">Total</td>
                                                                <td style ="width:10%">
                                                                    <asp:Label ID="lbltotal" Font-Size="Small"    runat ="server" style="text-align:right;float:right " ></asp:Label>
                                                                </td>
                                                    <td style ="width:15%">
                                                                </td>
                                                                
                                                            </tr>
                                                        </tbody>
                                                    </table>


                                                                                


					</div>
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

          
        }



    </script>

</asp:Content>
