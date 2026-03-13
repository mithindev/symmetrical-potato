<%@ Page Title="Share Ledger" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ShareLedger.aspx.vb" Inherits="Fiscus.ShareLedger" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="shareled" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Share</a></li>
						<li class="breadcrumb-item active" >Share Ledger</li>
					</ol>
				</nav>

          <div class="card card-body ">
              <h6 class="card-title text-primary  ">Share Ledger</h6>
              
              <div class="row ">
                <div class="col-md-9 ">
                    <div class="form-group row">
                        
                        <label class="col-sm-3 col-form-label">Customer Id</label>
                        <div class="col-sm-3">
                        <asp:TextBox ID="txtcid"  Width="120px" runat="server" CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                                            </div>




                    </div>
                    <div class="form-group row ">
                        <label class="col-sm-3 col-form-label ">Customer Name</label>
                        
                            <asp:Label runat="server" ID="lblfname" CssClass="col-md-3 col-form-label text-primary "></asp:Label>
                        
                         <asp:Label runat="server" ID="lblconame" CssClass="col-md-4 col-form-label text-primary "></asp:Label></div>
                    
                     <div class="form-group row ">
                        <label class="col-sm-3 col-form-label ">Address</label>
                        <div class="col-sm-5">
                            <asp:Label runat="server" ID="lbladd" CssClass="col-form-label text-primary "></asp:Label>
                        </div>
                    </div>

                    <div class="form-group row ">
                        <label class="col-sm-3 col-form-label ">Mobile No</label>
                        <div class="col-sm-3">
                            <asp:Label runat="server" ID="lblpin" CssClass="col-form-label text-primary "></asp:Label>
                        </div>
                    </div>


                    </div>
              <div class="col-md-3">
                    <div class="form-group row">
                        <label class="col-sm-3"></label>
                        
                    </div>
                  <asp:Image ID="memimg" runat="server"  Width="192px" Height="192px" AlternateText=""/>
                </div>
              </div>
                  </div>

        <asp:UpdatePanel runat="server" >
            <ContentTemplate>

                 
                              <table style="border:1px;width:100%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>
                                         <th style="width:15%; ">Date</th>
                                         <th style="width:20%;">Certificate No</th>
                                         <th style="width:30%;">Distintive No</th>
                                         <th style="width:15%;">No of Shares</th>
                                         <th style="width:20%;">Value</th>
                                      </thead>
                                  </table>

                        <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="20"
                             Width="100%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "  >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" Width="100%" text='<%#Eval("date", "{0:dd-MM-yyyy}")%>' Style="text-align:center;float:right "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />

                                </asp:TemplateField>
                
                                 <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsl" runat="server" text='<%#Eval("sl")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                 
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrfrm" runat="server" text='<%#Eval("shrfrm")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField >
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrto" runat="server" text='<%#Eval("shrto")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%"  />

                                        </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshr" runat="server" text='<%#Eval("allocation")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="15%" />

                                        </asp:TemplateField>
                                <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblshrval" runat="server" text='<%#Eval("shrval")%>' Style="text-align:right ;float:right  "></asp:Label>

                                            </ItemTemplate>
                                                                           <ItemStyle Width="20%" />

                                        </asp:TemplateField>

                                </Columns>
                            </asp:GridView>


                                                                                <table style="border:1px;width:100%;margin-left:0px;float :left " class ="table table-condensed  ">
                                                        <tbody >
                                                            <tr>
                                                                <td style="width:65%">Total</td>
                                                                <td style ="width:15%">
                                                                    <asp:Label ID="lblshrnos" runat ="server" style="text-align:right;float:right " ></asp:Label>
                                                                </td>
                                                    <td style ="width:20%">
                                                                    <asp:Label ID="lblnetshrval" runat ="server" style="text-align:right;float:right  " ></asp:Label>
                                                                </td>
                                                                
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                 
                </ContentTemplate>
        </asp:UpdatePanel>
           

              
        </form>


    
   <%--      <style>

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

       <script src="../js/jquery.js" type="text/javascript"></script>
    
	<script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
    

	<script type="text/javascript">




        $(document).ready(function () {

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

         // ShowCTab();
        // Place here the first init of the autocomplete
            InitAutoCompl();
       //     InitAutoComplG();
           // InitAutoComplSB();
        
    });




        function InitializeRequest(sender, args) {
        }



       

        function EndRequest(sender, args) {
            
            InitAutoCompl();
           

        }
       



        function InitAutoCompl() {
            $("#<%=txtcid.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/membersearch.asmx/GetUsers") %>',
						data: "{ 'input': '" + request.term + "'}",
						dataType: "json",
						type: "POST",
						contentType: "application/json; charset=utf-8",
						success: function (data) {
							response($.map(data.d, function (item) {
								return {
									memberno: item.Memberno,
									firstname: item.Firstname,
									lastname: item.Lastname,
									address: item.Address,
									image: item.ImageURL
								}
							}))
						}
					});
				},
                minLength: 1,
                focus: function (event, ui) {

                    $("#<%=txtcid.ClientID %>").val(ui.item.firstname);
                    return false;

                },
                open: function () {
                    $("ul.ui-menu").width($(this).innerWidth() + 180);

                },

				select: function (event, ui) {

                    $("#<%=txtcid.ClientID %>").val(ui.item.memberno);


                    return false;
                }
            })
                .autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>")
                        .append("<div style='container'>")
                        .append("<div class='img-div column'>")
                        .append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
                        .append("</div")
                        .append("<div class='info-div column'>")
                        .append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
                        .append("<div class='userinfo'>" + item.lastname + "</div>")
                        .append("<div class='userinfo'>" + item.address + "</div>")
                        .append("</br>")
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
