<%@ Page Title="Customer Inquiry" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="CustomerInquiry.aspx.vb" Inherits="Fiscus.CustomerInquiry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    	<link rel="stylesheet" href="css/Customer.css" />

    <style>
        .tcid,input {
            border: 0;
            background-color:transparent ;
            color:#526484;
            padding: 0 .8rem;
            margin-top: 3px;
            }

        .tcid:focus,input:focus {
            border: 0;
            background-color:transparent ;
            color:#526484;
            padding: 0 .8rem;
            margin-top: 3px;

        }

        .loader{
           position:absolute;
    top:0;
    left:0;
    right:0;
    bottom:0;
    background-color:rgba(0, 0, 0, 0.65);
    z-index:9999;
    color:white;
  
        }

        .spinner{
    position: absolute;
    margin: auto;
    top:0;
    bottom: 0;
    left: 0;
    right: 0;
}


        .lnk{

            color:#526484;
        }

        .lnk:hover{

            color:#007bff !important;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    

    <form class="forms-sample" runat="server" >
		<asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
                       
				<nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Home</a></li>
						<li class="breadcrumb-item active" >Customer Inquiry</li>
					</ol>
				</nav>


        <asp:UpdatePanel ID="alertmsg" runat="server" Visible="false">
			<ContentTemplate>
		<div class="alert alert-danger  alert-dismissible fade show" role="alert">
	<strong>Hi 
		<asp:Label ID="sesuser" runat="server" ></asp:Label> !

	 Invalid Customer Id 
				<button type="button" class="close" data-dismiss="alert" aria-label="Close">

		<span aria-hidden="true">&times;</span>
	</button>
				
	</div>

			</ContentTemplate>
			</asp:UpdatePanel>

            <asp:UpdatePanel  ID="upprog" runat="server" Visible="false" >
                             <ContentTemplate>
                                 <div class="loader ">

  <div class="spinner-border text-primary spinner  "  role="status">
    <span class="sr-only">Loading...</span>
  </div>

               </div>
          
                             </ContentTemplate>
            
                 
        </asp:UpdatePanel>

		<div class="row">
			<div class="container-xl wide-lg ">
			<div class="nk-content-body ">
				<div class="nk-block-head ">
				 <div class="nk-block-between-md g-3">
                                
                                 
					 <div class="nk-block-head-content">
						 <div class="nk-block-head-sub">
							    <asp:UpdatePanel runat="server" ID="upcid" >
                                    <Triggers >
                                        <asp:PostBackTrigger ControlID="txtcid" />
                                        <asp:PostBackTrigger ControlID="btn_clear" />
                                    </Triggers>
                                      <ContentTemplate>
                                          
                                      <label class="col-form-label text-primary">Customer ID</label>
                             <asp:TextBox ID="txtcid" runat="server" CssClass="tcid" Width="250px" autofocus="true"  autocomplete="off"  AutoPostBack="true"   placeholder="Search here "></asp:TextBox>
                             <asp:LinkButton ID="btn_clear" runat="server" CssClass="btn btn-sm btn-dim btn-danger ml-2" Style="border-radius: 4px; padding: 2px 8px; margin-bottom: 4px;">
                                 <em class="icon ni ni-cross"></em> <span>Clear</span>
                             </asp:LinkButton>
                                              
                                      </ContentTemplate>
                                  </asp:UpdatePanel>
						 </div>

      
                         <asp:UpdatePanel runat="server" ID="upCust" Visible="false" >
                             <ContentTemplate>


						 <div class="align-center pb-2 gx-4 gy-3 "">
							 <div>
							 <h3 class="nk-block-title fw-normal">
                                 <asp:Label ID="lblfname" runat="server" CssClass="col-form-label text-primary" ></asp:Label>
							 </h3>
								 </div>
                             <asp:UpdatePanel ID="upcp" runat="server" Visible="false" >
                                 <ContentTemplate>

                             <h6><span class="badge badge-primary">Channel Partner</span></h6>
                                     
                                 </ContentTemplate>
                             </asp:UpdatePanel> 
						 </div>
						 <div class="nk-block-des"><p>
                                 <asp:Label ID="lbllname" runat="server" ></asp:Label>
						                           </p></div>
                                 
                             </ContentTemplate>
                         </asp:UpdatePanel>

					 </div>

                         <asp:UpdatePanel runat="server" ID="upcustprofile" Visible="false" >
                    <ContentTemplate>


                     <div class="nk-block-head-content d-none d-md-block">
                         <div class="nk-refwg-stats card-inner bg-lighter">
                         <div class="nk-refwg-group g-3 ">
                             <div class="nk-refwg-info g-3">
                                 <div class="nk-refwg-sub">
                                     <div class="title">
                                         <asp:Label ID="lblshare" runat="server" ></asp:Label>
                                     </div>
                                     <div class="sub-text">Shares Held</div>
                                 </div>
                                 <div class="nk-refwg-sub">
                                     <div class="title">
                                         <asp:Label ID="lblshrval" runat="server" ></asp:Label>
                                     </div>
                                     <div class="sub-text">Share Value</div>
                                 </div>
                             </div>
                            
                         </div>
                         
                     </div>
                   
                     </div>
                        </ContentTemplate>
                             </asp:UpdatePanel>
                     
				 </div>
                    </div>

                <asp:UpdatePanel runat="server" ID="upinvdet" Visible="false" >
                    <ContentTemplate>


                <div class="nk-block">
                    <div class="card card-bordered">
                        <div class="nk-refwg">
                            <div class="nk-refwg-invite card-inner">
                                <div class="form-group row border-sm-bottom  ">
                                <div class="col-sm-4">
                                    <asp:UpdatePanel runat="server" >
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="txtcid" />
                                        </Triggers>
                                        <ContentTemplate>

                                        
                                    <asp:Image ID="imgCapture" runat="server"  width="120" height="120"  />
                                            </ContentTemplate>
                                    </asp:UpdatePanel>
                                        </div>
                                    <div class="col-md-4">
                                <div class="sub-text ">Address</div>
                                        <div class="title ">
                                            <asp:Label ID="lbladd" Width="100%" runat="server" ></asp:Label>
                                        </div>
                                <div class="sub-text ">Pin Code</div>
                                        <div class="title">
                                             <asp:Label ID="lblpin" Width="100%" runat="server" ></asp:Label>

                                        </div>
                                        
                                        </div>
                                    </div>
                                <div class="form-group row">
                                    <div class="col-sm-4">
                                        <div class="sub-text ">Gender</div>
                                        <div class="title align-center">
                                             <asp:Label ID="lblgen" Width="100%" runat="server" ></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="sub-text ">Mobile No</div>
                                        <div class="title"> <asp:Label ID="lblmobile" Width="100%" runat="server" ></asp:Label></div>
                                    </div>
                                       <div class="col-sm-4">
                                        <div class="sub-text ">Phone No</div>
                                        <div class="title"> <asp:Label ID="lblphone" Width="100%" runat="server" ></asp:Label></div>
                                    </div>
                                </div>
                                
                            </div>
                            
                            <div class="nk-refwg-stats card-inner bg-lighter">
                  
                                <div class="form-group row border-sm-bottom">
                                    
                                    <div class="col-sm-6">
                                        <div class="sub-text ">Date of Birth</div>
                                        <div class="title"> <asp:Label ID="lbldob" Width="100%" runat="server" ></asp:Label></div>
                                    </div>
                                       <div class="col-sm-5">
                                        <div class="sub-text ">PAN Card</div>
                                        <div class="title"> <asp:Label ID="lblpan" Width="100%" runat="server" ></asp:Label></div>
                                    </div>
                                </div>

                                    <div class="form-group row">
                                    
                                    <div class="col-sm-6">
                                        <div class="sub-text ">Group Id</div>
                                        <div class="title"> <asp:Label ID="lblgrp" Width="100%" runat="server" ></asp:Label></div>
                                    </div>
                                       <div class="col-sm-5">
                                        <div class="sub-text ">Application No</div>
                                        <div class="title"> <asp:Label ID="lblappno" Width="100%" runat="server" ></asp:Label></div>
                                    </div>
                                </div>

                                <div class="form-group row border-sm-top ">
                                    
                                    <div class="col-sm-6">
                                        <div class="sub-text ">Nominee</div>
                                        <div class="title"> <asp:Label ID="lblnominee" Width="100%" runat="server" ></asp:Label></div>
                                    </div>
                                       <div class="col-sm-5">
                                        <div class="sub-text ">Relationship</div>
                                        <div class="title"> <asp:Label ID="lblrelation" Width="100%" runat="server" ></asp:Label></div>
                                    </div>
                                </div>
                              
                            </div>
                        </div>
                        </div>
                    </div>
                    
                    <div class="nk-block">
                        <div class="row gy-gs">
                            <div class="col-md-6 col-lg-4">
                                <div class="nk-wg-card is-dark  card ">
                                    <div class="card-inner">
                                        <div class="nk-iv-wg2">
                                            <div class="nk-iv-wg2-title">
                                                <h6 class="title">Available Balance <em class="icon ni ni-info" data-toggle="tooltip" data-placement="left" title="" data-original-title="Total Available balance after Detectuion of Loans"></em></h6>
                                            </div>
                                            <div class="nk-iv-wg2-text">
                                                <div class="nk-iv-wg2-amount">
                                                    <asp:Label ID="lblcTotalBal" runat="server" ></asp:Label>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-lg-4">
                                <div class="nk-wg-card is-s1 card card-bordered">
                                    <div class="card-inner">
                                        <div class="nk-iv-wg2">
                                            <div class="nk-iv-wg2-title">
                                                <h6 class="title">Total Invested <em class="icon ni ni-info"></em></h6>
                                            </div>
                                            <div class="nk-iv-wg2-text">
                                                <div class="nk-iv-wg2-amount">
                                                    <asp:Label ID="lblcDep" runat="server" ></asp:Label>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 col-lg-4">
                                <div class="nk-wg-card is-s3 card card-bordered">
                                    <div class="card-inner">
                                        <div class="nk-iv-wg2">
                                            <div class="nk-iv-wg2-title">
                                                <h6 class="title">Total Loans <em class="icon ni ni-info"></em></h6>
                                            </div>
                                            <div class="nk-iv-wg2-text">
                                                <div class="nk-iv-wg2-amount">
                                                    <asp:Label ID="lbltLoan" runat="server" ></asp:Label>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                <div class="nk-block">
                    <div class="row gy-gs">
                        <div class="col-md-6 col-lg-4">
                            <div class="nk-wg-card card card-bordered h-100">
                                <div class="card-inner h-100">
                                    <div class="nk-iv-wg2">
                                        <div class="nk-iv-wg2-title">
                                            <h6 class="title">Deposit Accounts</h6>
                                        </div>
                                        <div class="nk-iv-wg2-text">
                                            <div class="nk-iv-wg2-amount ui-v2">
                                                <asp:Label ID="lblcDTotal" runat="server" ></asp:Label>
                                            </div>
                                            <ul class="nk-iv-wg2-list">
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_cDS" runat="server"  Text="Daily Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcDS" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_cFD" runat="server"  Text="Fixed Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcFD" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_cKMK" runat="server"  Text="KMK Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcKMK" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_cRD" runat="server"  Text="Recurring Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcRD" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_cRID" runat="server"  Text="Reinvestment Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcRID" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_cSB" runat="server"  Text="Savings Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcSB" runat="server" ></asp:Label></span></li>
                                                <li class="total"><span class="item-label">Total</span><span class="item-value"><asp:Label ID="lblcTotal" runat="server" ></asp:Label></span></li>
                                            </ul>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6 col-lg-4">
                            <div class="nk-wg-card card card-bordered h-100">
                                <div class="card-inner h-100">
                                    <div class="nk-iv-wg2">
                                        <div class="nk-iv-wg2-title">
                                            <h6 class="title">Loan Accounts</h6>
                                        </div>
                                        <div class="nk-iv-wg2-text">
                                            <div class="nk-iv-wg2-amount ui-v2"><asp:Label runat="server" ID="lblcLBal"></asp:Label> </div>
                                            <ul class="nk-iv-wg2-list">
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_cDCL" runat="server"  Text="Daily Collection"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcDCL" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_CDL" runat="server"  Text="Deposit Loan"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcDL" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_cJL" runat="server"  Text="Jewel Loan"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcJL" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_CML" runat="server"  Text="Mortgage Loan"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblcML" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label">&nbsp</span> </li>
                                                <li><span class="item-label">&nbsp</span> </li>
                                               <li class="total"><span class="item-label">Total</span><span class="item-value"><asp:Label ID="lblcLtotal" runat="server" ></asp:Label></span></li>
                                            </ul>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-lg-4">
                            <div class="nk-wg-card card card-bordered h-100">
                                <div class="card-inner h-100">
                                    <div class="nk-iv-wg2">
                                        <div class="nk-iv-wg2-title">
                                            <h6 class="title">Balance in Group Account <em class="icon ni ni-info text-primary"></em></h6>
                                        </div>
                                        <div class="nk-iv-wg2-text">
                                            <div class="nk-iv-wg2-amount ui-v2"><asp:Label runat="server" ID="lblGBal"></asp:Label></div>
                                            <ul class="nk-iv-wg2-list">
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_gDS" runat="server"  Text="Daily Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblgDS" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_gFD" runat="server"  Text="Fixed Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblgFD" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_gKMK" runat="server"  Text="KMK Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblgKMK" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_gRD" runat="server"  Text="Recurring Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblgRD" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_gRID" runat="server"  Text="Reinvestment Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblgRID" runat="server" ></asp:Label></span></li>
                                                <li><span class="item-label"><asp:LinkButton CssClass="lnk"  ID="btn_gSB" runat="server"  Text="Savings Deposit"></asp:LinkButton></span><span class="item-value"><asp:Label ID="lblgSB" runat="server" ></asp:Label></span></li>
                                                <li class="total"><span class="item-label">Total</span><span class="item-value"><asp:Label ID="lblgTotal" runat="server" ></asp:Label></span></li>
                                            </ul>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        

                    </div>
                </div>

                        
                    </ContentTemplate>
                </asp:UpdatePanel>
				
			</div>

				</div>
		</div>
	</form>

    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
    
	    
    
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

                    $("#<%=txtcid.ClientID %>").val(ui.item.memberno);
                    return false;

                },
				select: function (event, ui) {

					$("#<%=txtcid.ClientID %>").val(ui.item.memberno);
                    

					return false;
                },
                
                open: function () {
                    $("ul.ui-menu").width($(this).innerWidth()+120);
     
                },
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

  .ui-autocomplete {
    max-height: 450px;
    overflow-y: auto;
    /* prevent horizontal scrollbar */
    overflow-x: hidden;
  }
  
  


	    .memno {
			z-index: 10;  
			position: absolute;  
			right: 10px;  
			top: 10px;
			font-size:9px;
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
