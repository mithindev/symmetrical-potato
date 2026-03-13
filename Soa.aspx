<%@ Page Title="Statement Of Accounts" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Soa.aspx.vb" Inherits="Fiscus.Soa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/jquery.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
	<script src="js/jquery-ui.min.js" type="text/javascript"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <form  runat="server" >
		<asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
                       
				<nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Home</a></li>
				
                        <li class="breadcrumb-item active" >Statement of Accounts</li>
					</ol>
				</nav>
		   		<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								
                                            

               <div class="form-group row ">
                   <label class="col-sm-2 col-form-label ">Account No</label>
                   <div class="col-sm-3">
                       <asp:UpdatePanel runat="server" >
                           <ContentTemplate>

                           
                    <div class="input-group">
                                                            
                                                                
                                                             <asp:TextBox  ID="txtacn" runat="server" CssClass="form-control"  AutoPostBack="true"  ></asp:TextBox>
                                                                    
                                                            <div class="input-group-append">
                                                                <asp:LinkButton runat="server" ID="soa"  >
                                                                   <span class="input-group-addon input-group-text text-primary " style="height:35px">


<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-arrow-right"><line x1="5" y1="12" x2="19" y2="12"></line><polyline points="12 5 19 12 12 19"></polyline></svg>
                                                                </span> </asp:LinkButton>
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
		   
		
	</form>

      
   

    <script >

        $(document).ready(function () {

            const urlParams = new URLSearchParams(window.location.search);
            window.globalThis = urlParams.get('typ');
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            getsoa();

            setTimeout(function () {
                document.getElementById('<%=txtacn.ClientID %>').focus();
              }, 200);

            InitAutoComplacn();
           // InitAutocomplJewl();

        });


        function InitializeRequest(sender, args) {
        }



        function EndRequest(sender, args) {

            InitAutoComplacn();

        }

        function InitAutoComplacn() {

            var myParam = window.globalThis;
            
            var url = '';

            if (myParam == 'l') {
                url = '<%=ResolveUrl("~/membersearch.asmx/GetSOALoan") %>'
            } else {

                url = '<%=ResolveUrl("~/membersearch.asmx/GetSOA") %>'
            }


            $("#<%=txtacn.ClientID %>").autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          url: url,  //'<%=ResolveUrl("~/membersearch.asmx/GetDeposit") %>',
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
                                    image: item.ImageURL,
                                    product: item.Product,
                                    amt: item.Amount,
								}
							}))
						}
					});
				},
                minLength: 1,
                focus: function (event, ui) {

                    $("#<%=txtacn.ClientID %>").val(ui.item.memberno);
                    return false;

                },
                open: function () {
                    $("ul.ui-menu").width($(this).innerWidth() + 210);

                },

				select: function (event, ui) {

                    $("#<%=txtacn.ClientID %>").val(ui.item.memberno);
                    var x = ui.item.memberno.substring(0, 1);
                    if (x == "6") {

                        var acn = ui.item.memberno;

                        if (acn.length )
                        window.location.href = "../soaloan.aspx?acno=" + ui.item.memberno


                    } else {
                        window.location.href = "../soadeposit.aspx?acno=" + ui.item.memberno
                    }
                    return false;



                     return false;
                 }
             })
                  .autocomplete("instance")._renderItem = function (ul, item) {
                      return $("<li>")
                          .append("<div>")
                          .append("<div class='img-div column'>")
                          .append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
                          .append("</div")
                          .append("<div class='info-div column'>")
                          .append("<div class='username'>" + item.firstname + "</div>")
                          .append("<div class='userinfo'>" + item.lastname + "</div>")
                          .append("<div class='userinfo'>" + item.address + "</div>")
                          .append("<div class='userinfo1'>" + item.amt + " </div>")
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


        .userinfo1
        {

            margin: 0px;
            padding-top: 10px;
			font-size:12px;
        }


        .ui-autocomplete {
    max-height: 510px;
    overflow-y: auto;
    /* prevent horizontal scrollbar */
    overflow-x: hidden;
  }

    </style>
   


</asp:Content>
