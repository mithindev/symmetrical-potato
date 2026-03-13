<%@ Page Title="Journal Entry" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Journal.aspx.vb" Inherits="Fiscus.Journal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        @media print {  
  @page {
    size:148.50mm 210mm; /* landscape */
    /* you can also specify margins here: */
    margin: 0;
     /* for compatibility with both A4 and Letter */
  }
  
}
        .prntarea{
             height: 105mm;
            width: 148.5mm;
            

    margin: 0;

        }
    </style>

    <link rel="stylesheet" href="css/daterangepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="newreceipt" runat="server" >

        <asp:ScriptManager ID="SM3" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Transaction</a></li>
						<li class="breadcrumb-item active" >Journal</li>
					</ol>
				</nav>
	
			

    
    	

        <asp:UpdatePanel runat="server" >
            <ContentTemplate>



        		<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								<asp:Panel ID="pnlvch" style="display:block" runat="server" >
                                <h6 class="card-title text-primary ">Credit Account</h6>

                                

                                
                                <asp:UpdatePanel runat="server" >
                                    <ContentTemplate>

								<div class="form-group row ">
									<label class="col-sm-2 col-form-label ">Date</label>
									<div class="col-sm-3">
										<asp:TextBox runat="server" CssClass="form-control"   ID="tdate" AutoPostBack="true" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing" ></asp:TextBox>
									</div>
									</div>
								<div class="form-group row">
									
									
									<label class="col-sm-2 col-form-label ">GL Head</label>
									 
									<div class="col-sm-3">
										<asp:TextBox ID="txtled" onfocus="return acled()"  CssClass="form-control " runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
									</div>
									</div>
								<div class="form-group row">
									<label class="col-sm-2 col-form-label ">Credit</label>
									<div class="col-sm-3">
										<asp:TextBox runat="server" CssClass="form-control" AutoCompleteType="None"    ID="txtamt" data-validation-engine="validate[required,funcCall[chkdecim[]]" data-errormessage-value-missing="Amount Missing" ></asp:TextBox>
									</div>
									</div>
								<div class="form-group row">
									<label class="col-sm-2 col-form-label ">Narration</label>
									<div class="col-sm-3">
										<asp:TextBox runat="server" CssClass="form-control"  Height="60px" Width="300px" TextMode="MultiLine"  ID="txtnar"  ></asp:TextBox>
									</div>
									</div>
								<div class="form-group row ">
									<label class="col-sm-2 col-form-label ">Debit to</label>
									
								</div>

								<asp:Panel runat="server" ID="gldr"  >
									<div class="form-group row">
									
									
									<label class="col-sm-2 col-form-label text-danger  ">GL Head</label>
									 
									<div class="col-sm-3">
										<asp:TextBox ID="txtled_d"  CssClass="form-control " onfocus="return acledd()" runat="server" data-validation-engine="validate[required]" data-errormessage-value-missing="Deposit Type Missing"></asp:TextBox>
									</div>
									</div>
								</asp:Panel>

				
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

                                        <asp:UpdatePanel runat="server" id="pnlbtn">
                                            <ContentTemplate>

                                            
								<div class="form-group row border-bottom  "></div>
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
                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                    </asp:Panel>


                                   <asp:Panel runat="server" ID="pnlprnt" style="display:none"  >
                    
                
                <div class="card card-body" id="printtab">

                    <div class="form-group row border-bottom  ">
                        <div class="col-md-4"></div>
                        <div class="col-md-4">
                            <div class="btn-group" role="group" >
                            <asp:button id="btntog" runat="server"   Class="btn btn-outline-primary" UseSubmitBehavior="false"  Text="Close" />
                            <button type="button"  Class="btn btn-outline-primary" onclick="PrintOC();">Print</button> 
                                                       
                                            </div>           
                        </div>
                    </div>

              
                              
                   <div id="vouchprint" class="prntarea"  >
              
                         <asp:UpdatePanel runat="server" >
                    <ContentTemplate>


                        <table border="0" style="border-collapse: collapse; " >
        <tbody>
            <tr style="border: none;">
                <td style="width: 10%;"><img src="Images/KBF-LOGO.png" alt="" width="72" height="60" /></td>
                <td style="width: 53%;">
                    <table border="0" style="border-collapse: collapse; width: 0%; height: 86px;" >
                        <tbody>
                            <tr style="height: 10px;">
                                <td style="width: 100%;font-size:20px;height:10px;"><b>KARAVILAI NIDHI LIMITED</b></td>
                            </tr>   
                            <tr style="height: 10px;">
                                <td style="width: 100%; height: 10px;font-size:15px; text-align: left; font-size: 11px;"><span>Reg No : 18-37630/97</span><br/>8-12A,Vijayam, Main Road Karavilai,Villukuri P. O</td>
                            </tr>
                            <tr>
                                <td style="width: 100%;font-size:15px;">Branch : <b><asp:Label id="pbranch" runat="server"></asp:Label></b></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="width: 37%;">
                    <table style="float: right; margin-left: 20px; height: 63px" width:"100%" >
                        <tbody>
                            <tr >
                                <td colspan="2" style="width:172px; height: 21px; text-align:left;background-color:#eee;color:#000;font-weight:bold "><label id="lblcptr" >JOURNAL VOUCHER</label> </td>
                            </tr>

                             <tr style="height: 21px;">
                                <td style="width: 82px; height: 21px;font-size: 15px">No&nbsp; &nbsp; :</td>
                                <td style="width: 90px; height: 21px;font-size: 15px"><asp:Label ID="pvno" runat="server" ></asp:Label></td>
                            </tr>
                            <tr style="height: 21px;">
                                <td style="width: 82px; height: 21px;font-size: 15px">Date :</td>
                                <td style="width: 90px; height: 21px;font-size: 15px"><asp:Label ID="pdate" runat="server" ></asp:Label></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    
    

                        <table  style="border-bottom:1px solid;width:100%;font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif">
            <tr >
                <td style="width:100%;">
                     
            <table style="width:100%;padding-left:10px;padding-right:20px;border-collapse:collapse" >
                <tbody>
                    <tr style="height:25px;border:1px solid">
                        <td style="width:50%;text-align:center;border:1px solid">Particulars</td>
                        <td style="width:25%;text-align:center;border:1px solid">Debit</td>
                        <td style="width:25%;text-align:center;border:1px solid">Credit</td>
                    </tr>
                   
                    <tr style="height:35px;">
                        <td style="width:50%;text-align:left;border:1px solid;padding-left:5px"><asp:Label runat="server" ID="lblglhcr"></asp:Label></td>
                        <td style="width:25%;text-align:right;border:1px solid"></td>
                        <td style="width:25%;text-align:right;border:1px solid;padding-right:5px"><asp:Label runat="server" ID="lblcr"></asp:Label></td>
                    </tr>
                   <tr style="height:35px;">
                        <td style="width:50%;text-align:left;border:1px solid;padding-left:5px"><asp:Label runat="server" ID="lblglhdr"></asp:Label></td>
                        <td style="width:25%;text-align:right;border:1px solid;padding-right:5px"><asp:Label runat="server" ID="lbldr"></asp:Label></td>
                        <td style="width:25%;text-align:right;border:1px solid"></td>
                    </tr>
                    <tr style="height:35px;">
                        <td style="width:50%;text-align:center;border:1px solid;padding-left:5px">Total</td>
                        <td style="width:25%;text-align:right;border:1px solid;padding-right:5px"><asp:Label runat="server" Font-Bold="true"  ID="lbltdr"></asp:Label></td>
                        <td style="width:25%;text-align:right;border:1px solid;padding-right:5px"><asp:Label runat="server" Font-Bold="true"  ID="lbltcr"></asp:Label></td>
                    </tr>
                    <tr  style="border:none;height:15px;">
                        <td colspan="3" style="border:none;text-align:left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr  style="border:none;" >
                        <td colspan="3" style="border:none;text-align:left">
                            <asp:Label runat="server" id="pnar"></asp:Label>
                        </td>
                    </tr>
                    <tr style="border:none;height:35px">
            <td colspan="3" style="border:none;text-align:left;font-size:small" >
                  Amount in Words :&nbsp;<asp:Label ID="paiw" runat="server"  ></asp:Label>
      
            </td>
            </tr>
                </tbody>
            </table>
        
                

            
               
            </tr>
            
        </table>
       
        
                       
    
        
                        <table   style="height:15px;margin-top:20px;width:100%">
            <tbody>
                <tr>
                    <td style="width:50%; text-align: center;font-size: 15px">Incharge / Manager</td>
                    
                    <td style="width:50%; text-align: center;font-size: 15px">Cashier</td>
                   
                    
                    
                </tr>
            </tbody>
        </table>
    
                         </ContentTemplate>
                </asp:UpdatePanel>
                  
                   </div>            
                        
                        
                       
                
                
               </div>

    </asp:Panel>

								</div>
								</div>
						</div>
							</div>

                
            </ContentTemplate>
        </asp:UpdatePanel>

								</form>


   
		    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="js/daterangepicker.js" type="text/javascript" ></script>
	<script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
    
    

        <script src="js/printThis.js" type="text/javascript"></script>

    <script>



        function PrintOC() {

       

            $('#vouchprint').printThis({
                importCSS: false,
                importStyle: true,




            });
        }

        function PrintCC() {

        
            $('#vouchprint').printThis({
                importCSS: false,
                importStyle: true,         // import style tags



            });


        }


    </script>

	<script type="text/javascript">




        $(document).ready(function () {



            
            
     
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
            InitAutoCompl();
        
     
    });


        function InitializeRequest(sender, args) {
        }



        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete

            InitAutoCompl();
          //  InitAutocomplJewl();
            
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


        function acled() {

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


        function acledd() {

          

            $("#<%=txtled_d.ClientID %>").autocomplete({
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

                     $("#<%=txtled_d.ClientID %>").val(ui.item.jewel);


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
