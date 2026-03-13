<%@ Page Title="General Ledger" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Ledger.aspx.vb" Inherits="Fiscus.Ledger" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/daterangepicker.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <form id="despanalysis" runat="server">

        <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

        <nav class="page-breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Financial Reports</a></li>
                <li class="breadcrumb-item active">Ledger</li>
            </ol>
        </nav>

          <asp:UpdatePanel runat="server" >
              <Triggers>
                  <asp:PostBackTrigger ControlID="btnprnt" />
              </Triggers>
              <ContentTemplate>


        <div class="card card-body ">
            <h6 class="card-title  text-primary ">General Ledger</h6>

            <div class="form-group row border-bottom ">
            <label class="col-form-label text-primary ">From</label>
            <div class="col-sm-2">
                <asp:TextBox ID="txtfrm" CssClass="form-control " Height="37px" style="margin-left:-7px" runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>

            </div>
            <label class=" col-form-label text-primary ">To</label>
            <div class="col-sm-2">
                 <asp:TextBox ID="txtto" CssClass="form-control " Height="37px"  runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
            </div>
            <label class="col-sm-1 col-form-label text-primary ">Ledger</label>
               <div class="col-sm-4">
                                    <div class="input-group">

                                        <asp:TextBox ID="txtprod" Height="37px" CssClass="form-control "  runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
                                        <span class="input-group-btn input-group-append">
                                            <asp:LinkButton runat="server" ID="btn_inw" CssClass="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" id="btnprnt" CssClass ="btn btn-outline-primary " style="margin-left: 2px;" OnClientClick="document.forms[0].target = '_blank'; setTimeout(function(){document.forms[0].target='';}, 100);">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-printer"><polyline points="6 9 6 2 18 2 18 9"></polyline><path d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2"></path><rect x="6" y="14" width="12" height="8"></rect></svg>
                                            </asp:LinkButton>
                                        </span>

                                    </div>

                                </div>
            
            </div>

            <table style="margin-left:0px;width:100%" class="table table-bordered "> 
                                 <thead>
                                     
                                     <th style="text-align:center;width:18%;font-variant:small-caps;font-size:medium" >Date</th>
                                    <th style="text-align:center;width:32%;font-variant:small-caps;font-size:medium" >Particulars</th>   
                                    <th style="text-align:center;width:16%;font-variant:small-caps;font-size:medium" >Debit</th>
                                     <th style="text-align:center;width:16%;font-variant:small-caps;font-size:medium" >credit</th>
                                     <th style="text-align:center;width:23%;font-variant:small-caps;font-size:medium" >balance</th>

                                            
                        
                                    </thead>
                             </table>
                
                            <table style="margin-left:0px;width:100%" class="table table-bordered ">
                                <tbody>
                                    <tr>
                                        <td style="text-align:center;width:80%;font-variant:small-caps;font-size:medium">Opening Balance</td>
                                        <td style ="text-align:right ;width:20%;font-variant:small-caps;font-size:medium">
                                            <asp:UpdatePanel runat="server" >
                                                <ContentTemplate>
                                                    <asp:Label ID="lbl_ob" style="float:right;text-align:right "  runat ="server" ></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div>
                                 
                                 <asp:UpdatePanel ID="jspc" runat="server">
                                 <ContentTemplate >
                             <div >

                               <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-top:-2px;margin-left:0px;float:left;z-index:1;overflow-x:auto;" 
                            AllowPaging ="false"  
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered  "  >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lbldt" runat="server" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center   "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="18%" />
                                </asp:TemplateField> 

                                 <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblnar" runat="server" text='<%#Eval("narration")%>' Style="text-align:left ;float:left "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="32%" />
                                </asp:TemplateField>
                    
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lbldebit" runat="server" text='<%#Eval("debit", "{0:N}")%>' Style="text-align:right;float:right"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="16%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblcredit" runat="server" text='<%#Eval("credit", "{0:N}")%>' Style="text-align:right;float:right"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="16%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                  <asp:Label ID="lblbal" runat="server" text='<%#Eval("balance", "{0:N}")%>' Style="text-align:right  ;float:right  "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="23%" />
                                </asp:TemplateField>
                    

                
                      </Columns>
                                   </asp:GridView> 

                                 </div>
                                   </ContentTemplate>
                                     </asp:UpdatePanel>
                                </div>

                  <table style="margin-left:0px;width:100%" class="table table-bordered  "> 
                                 <tbody>
                                     <tr style="text-align:center;">
                                     <td style="text-align:center;width:80%" >
                                        <p>Closing Balance</p>

                                     </td>
                                      <td style="text-align:center;width:20%" >
                                          <asp:UpdatePanel runat="server" >
                                              <ContentTemplate>
                                         <asp:Label runat="server" ID="lblnet" style="float:right;text-align:right " Text=""></asp:Label>
                                                  </ContentTemplate>
                                              </asp:UpdatePanel>
                                         </td>
                                          

                                            </tr>
                                     </tbody>
                                     </table>



            </div>

                  
              </ContentTemplate>
          </asp:UpdatePanel>
          </form>

    


	    <script src="../js/jquery.js" type="text/javascript"></script>

<script src="js/daterangepicker.js" type="text/javascript" ></script>    
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
    

	<script type="text/javascript">




        $(document).ready(function () {



            
            
     
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
            InitAutoCompl();
        InitAutocomplJewl();
     
    });


        function InitializeRequest(sender, args) {
        }



        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete

            InitAutoCompl();
            InitAutocomplJewl();
            
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

            

        }


        function InitAutocomplJewl() {

            $("#<%=txtprod.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/membersearch.asmx/GetLedger") %>',
						data: "{ 'input': '" + request.term + "'}",
						dataType: "json",
						type: "POST",
						contentType: "application/json; charset=utf-8",
						success: function (data) {
							response($.map(data.d, function (item) {
								return {
									jewel: item.Jewel,
									
								}
							}))
						}
					});
				},
				minLength: 1,
				select: function (event, ui) {

                    $("#<%=txtprod.ClientID %>").val(ui.item.jewel);


                        return false;
                    }
                })
                .autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>")
                        .append("<div class='info-div column'>")
                        .append("<div class='username'>" + item.jewel+"</div> ")
                        .append("</div")
                        .append("</div>")
                        .appendTo(ul);
                };

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
   
  
</asp:Content>
