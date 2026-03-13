<%@ Page Title="Payment" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="GlPayment.aspx.vb" Inherits="Fiscus.GlPayment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link rel="stylesheet" href="css/daterangepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="newreceipt" runat="server" >

        <asp:ScriptManager ID="SM3" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Transaction</a></li>
						<li class="breadcrumb-item active" >Payment</li>
					</ol>
				</nav>
	
			

    
   


					<asp:UpdatePanel runat="server" >
						<ContentTemplate>

						
        		<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								<div class="form-group row ">
									<label class="col-sm-2 col-form-label ">Date</label>
									<div class="col-sm-3">
										<asp:TextBox runat="server" CssClass="form-control"   ID="tdate" AutoPostBack="true" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing" ></asp:TextBox>
									</div>
									</div>
								<div class="form-group row">
									
									
									<label class="col-sm-2 col-form-label ">GL Head</label>
									 
									<div class="col-sm-3">
										<asp:TextBox ID="txtled"  CssClass="form-control " runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
									</div>
									</div>
								<div class="form-group row">
									<label class="col-sm-2 col-form-label ">Payment</label>
									<div class="col-sm-3">
										<asp:TextBox runat="server" CssClass="form-control"   ID="txtamt" data-validation-engine="validate[required,funcCall[chkdecim[]]" data-errormessage-value-missing="Amount Missing" ></asp:TextBox>
									</div>
									</div>
								<div class="form-group row">
									<label class="col-sm-2 col-form-label ">Narration</label>
									<div class="col-sm-3">
										<asp:TextBox runat="server" CssClass="form-control"  Height="60px" Width="300px" TextMode="MultiLine"  ID="txtnar" AutoPostBack="true" ></asp:TextBox>
									</div>
									</div>

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
<br/>
            Processing.....
        </div>
    </div>
</ProgressTemplate>
</asp:UpdateProgress>
								<asp:UpdatePanel runat="server" ID="pnlbtn">
									<ContentTemplate>

									
								<div class="form-group row border-bottom "></div>
								<div class="form-group row ">
									<div class="col-sm-4"></div>
									<div class="col-sm-4">
										<div class="btn-group " role="group" >
											  <asp:Button id="btn_clr" Text="Clear" CausesValidation="false" CssClass="btn btn-outline-secondary " runat="server" />
                                                <asp:Button ID="btn_update" Text="Update" CausesValidation="false" CssClass="btn btn-outline-primary " runat="server" />
										</div>
									</div>
								</div>
										</ContentTemplate>
								</asp:UpdatePanel>

								</div>
								</div>
						</div>
							</div>

							</ContentTemplate>
					</asp:UpdatePanel>
								</form>
		    <script src="../js/jquery.js" type="text/javascript"></script>
			<script src="js/daterangepicker.js" type="text/javascript"></script>
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

            $("#<%=tdate.ClientID%>").focus(function () {


                $("#<%=tdate.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": false,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=tdate.ClientID%>").val(start.format('DD-MM-YYYY'));
                      

                });
            });


            

        }


        function InitAutocomplJewl() {

            $("#<%=txtled.ClientID %>").autocomplete({
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

                    $("#<%=txtled.ClientID %>").val(ui.item.jewel);


                    return false;
                }
            })
                .autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>")
                        .append("<div class='info-div column'>")
                        .append("<div class='username'>" + item.jewel + "</div> ")
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
