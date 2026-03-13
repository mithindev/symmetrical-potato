<%@ Page Title="Payment - Loans" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="LoanPayment.aspx.vb" Inherits="Fiscus.LoanPayment"   %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
        <link href="css/daterangepicker.css" rel="stylesheet" />
        <link href="css/ValidationEngine.css" rel="stylesheet" />
        <link href="css/validationEngine.jquery.css" rel="stylesheet" />

    <style>

        @media print {  
  @page {
    size:A5; /* landscape */
    /* you can also specify margins here: */
    margin: 3mm;
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif
     /* for compatibility with both A4 and Letter */


  }
   .outert {
        border: solid #000 !important;
        border-width: 1px 1px 1px 1px !important;
    }

    .prntarea{

             height: 105mm;
             width: 148.0mm;
             margin: 0;
            page-break-after:always;

        }
}

        @media screen{
        .prntarea{
             height: 105mm;
            width: 148.0mm;
               margin: 0;
            
        }
        

        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="frmpayment" name="frmpayment" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Loan</a></li>
						<li class="breadcrumb-item active" >Payment</li>
					</ol>
				</nav>
	
			

       
                    
                        
        
        		<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
                        
						<div class="card">
							<div class="card-body">
			                                   
                                               <asp:UpdatePanel runat="server" >
                                                   <ContentTemplate>

                                                       <asp:Panel runat="server" ID="pnltrans" style="display:block" >
                                                       <div class="form-group row ">
                                                    <label class="col-sm-2 col-form-label ">Account No</label>
                                                           <div class="col-sm-3">
                                                                 <asp:UpdatePanel runat="server" >
                                                               <ContentTemplate>

                                                       <div class="input-group mb-3">
                                                         
                                                               
                                                             <asp:TextBox  ID="txtacn" runat="server" CssClass="form-control"  AutoPostBack="true"   ></asp:TextBox>
                                                                   
                                                            <div class="input-group-append">
                                                                <asp:LinkButton runat="server" ID="soa" Enabled="false"   >
                                                                   <span class="input-group-addon input-group-text text-primary " style="height:35px;cursor:pointer">

<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-file-text"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path><polyline points="14 2 14 8 20 8"></polyline><line x1="16" y1="13" x2="8" y2="13"></line><line x1="16" y1="17" x2="8" y2="17"></line><polyline points="10 9 9 9 8 9"></polyline></svg>
                                                                </span> </asp:LinkButton>
                                                                </div>
                                                           </div>
                                                                </ContentTemplate>
                                                           </asp:UpdatePanel>
                                                                                </div>
                                                              
                                                            
                                                       
                                                           
                                                      
                                                           <label class="col-sm-2 col-form-label ">Date</label>
                                                       <div class="col-sm-2">
                                                           <asp:TextBox runat="server" CssClass="form-control" ID="tdate" AutoPostBack="True" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing" ></asp:TextBox>
                                                    </div>
                                                       </div>

                                                       <div class="form-group row ">
                                                           <label class="col-sm-2 col-form-label ">Loan</label>
                                                           <asp:Label ID="lblproduct" CssClass="col-sm-3 col-form-label text-danger  " runat="server"></asp:Label>

                                                           <label class="col-sm-2 col-form-label  ">Customer Name</label>
                                                           <asp:Label ID="lblname" runat="server" CssClass="col-sm-3 col-form-label text-danger" ></asp:Label>
                                                                                                                  </div>
                                                       <div class="form-group row border-bottom ">
                                                       <label class="col-sm-3 col-form-label ">Loan Amount</label>
                                <asp:Label ID="lblamt"  CssClass="col-form-label col-sm-2 text-danger " runat="server"></asp:Label>
                                                           <label class="col-sm-2 col-form-label ">Account Balance</label>
                                                           <asp:Label ID="lblbal" CssClass="col-form-label col-sm-2 text-danger "  runat="server"></asp:Label>
                                                           </div>

                                                       <asp:Panel ID="pnlsch" runat="server" Visible="false"  > 
                                                           
                                                           <asp:UpdatePanel runat="server" >
                                                               <ContentTemplate>

                                                       <div class="form-group row">
                                                           
                                            <label class="col-sm-2 col-form-label ">Scheme</label>
                                         <div class="col-sm-3">
                                              <asp:UpdatePanel ID="sch" runat="server"  >
                                    <ContentTemplate>
                                             <asp:DropDownList ID="schlst" runat="server" AutoPostBack="true"  CssClass ="form-control " >
                                                

                                             </asp:DropDownList>
                                          </ContentTemplate>
                                </asp:UpdatePanel>
                                        </div>
                                                           
                                                            <div class="col-sm-2">
                                                                <asp:UpdatePanel runat="server">
                                                                    <Triggers>
                                                                        <asp:PostBackTrigger ControlID="schlst" />
                                                                    </Triggers>
                                                                    <ContentTemplate>
                                                                        <asp:Label ID="lblcard" runat="server" CssClass="text-primary" Font-Bold="true"  Text=""></asp:Label> 
                                                                                    
                                                          

                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                </div>
                                                           

                                                               <asp:Panel runat="server" Visible ="false" ID="pnlpink" >
                                                               <div class="col-sm-1">

<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#E91E63" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-credit-card"><rect x="1" y="4" width="22" height="16" rx="2" ry="2"></rect><line x1="1" y1="10" x2="23" y2="10"></line></svg>

                                                            </div>   
                                                           </asp:Panel>
                                                           <asp:Panel runat="server" Visible ="false" ID="pnlBlue" >
                                                               <div class="col-sm-1">

<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#5b42f3" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-credit-card"><rect x="1" y="4" width="22" height="16" rx="2" ry="2"></rect><line x1="1" y1="10" x2="23" y2="10"></line></svg>

                                                            </div>   
                                                           </asp:Panel>
                                                           <asp:Panel runat="server" Visible ="false" ID="pnlgreen" >
                                                               <div class="col-sm-1">

<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#46c35f" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-credit-card"><rect x="1" y="4" width="22" height="16" rx="2" ry="2"></rect><line x1="1" y1="10" x2="23" y2="10"></line></svg>

                                                            </div>   
                                                           </asp:Panel>

                                                            <asp:Panel runat="server" Visible ="false" ID="pnlwhite" >
                                                               <div class="col-sm-1">

<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#0e0f0f" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-credit-card"><rect x="1" y="4" width="22" height="16" rx="2" ry="2"></rect><line x1="1" y1="10" x2="23" y2="10"></line></svg>

                                                            </div>   
                                                           </asp:Panel>

                                                           <asp:Label CssClass="col-sm-2 text-danger " runat="server" ID="lblroi"></asp:Label>

                                                           </div>
                                                                   
                                                               </ContentTemplate>
                                                           </asp:UpdatePanel>
                                          </asp:Panel>  

                                                       <div class="form-group row ">

                                                       <label class="col-sm-2 col-form-label ">Payment</label>
                                                       <div class="col-sm-3">
                                                           <asp:UpdatePanel runat="server" >
                                                               
                                                               <ContentTemplate>

                                                           <asp:TextBox runat="server" CssClass="form-control"  ID="txtamt" AutoPostBack="true" data-validation-engine="validate[required,funcCall[chkdecim[]]" data-errormessage-value-missing="Amount Missing" data-errormessage-custom-error="Enter a Valid Amount" ></asp:TextBox>
                                                                   </ContentTemplate>
                                                           </asp:UpdatePanel>
                                                           </div>
                                               <label class="col-sm-2 col-form-label ">Service Charge</label>
                                                           <div class="col-sm-2">
                                                               <asp:UpdatePanel runat="server" >
                                                                   <ContentTemplate>

                                                                   
                                                                <asp:TextBox runat="server" CssClass="form-control"  ID="txtsc" AutoPostBack="true" data-validation-engine="validate[required,funcCall[chkdecim[]]" data-errormessage-value-missing="Service Charge Missing" ></asp:TextBox>
                                                                       </ContentTemplate>
                                                               </asp:UpdatePanel>
                                                           </div>
                                                       </div>

                                                       
                                                       <div class="form-group row ">
                                                               <label class="col-sm-2 col-form-label ">Mode of Payment</label>
                                                               <div class="col-sm-3">
                                                                   <asp:UpdatePanel ID="UpdatePanel11"  runat="server">
                                    <ContentTemplate>
                                    
                                        <asp:DropDownList ID="mop" runat="server" AutoPostBack="true"   CssClass="form-control ">
                                            <asp:ListItem>Cash</asp:ListItem>
                                            <asp:ListItem>Account</asp:ListItem>
                                            <asp:ListItem>Transfer</asp:ListItem>
                                        </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                                               </div>
                                                               </div>
                                                               
                                                       <asp:Panel Visible="false" runat="server" ID="pnlsbtrf">
                                        <div class="form-group row ">
                                          
                                             <asp:Label ID="lblsb" Text ="SB Account No" CssClass="col-sm-2 col-form-label " runat="server" Visible="true" ></asp:Label>
                                                     
                                                    
                 
                                                               <div class="col-sm-3">
                    <asp:TextBox  ID="txt_sb" runat="server" Visible="true"  CssClass="form-control"  AutoPostBack="true" data-validation-engine="validate[required]"  data-errormessage-value-missing="Valid Account No is required!"></asp:TextBox>
                                                                   </div>
                                                           </div>
                                                               
                             </asp:Panel>

                    
                                                           

                                                       <asp:Panel ID="pnltran" runat="server"   Visible="false"  >
                                                       <div class="form-group row ">
                                                           
                                                       
                                                                    <asp:Label ID="lbl" Text ="Transfer To" CssClass="col-sm-2 col-form-label " runat="server" Visible="false" ></asp:Label>
                                                                    <div class="col-sm-3">
                                                                        <asp:DropDownList ID="bnk" runat="server" Visible="false"  ></asp:DropDownList>
                                                                    </div>
                                                       
                                                       </div>
                                                           </asp:Panel>

                                                       <div class="form-group row ">
                                                           <label class="col-sm-2 col-form-label">Notes</label>
                                                           <div class="col-sm-3">
                                                           <asp:TextBox runat="server" CssClass="form-control" Width="150px"   ID="txtnotes"  ></asp:TextBox>
                                                               </div>
                                                       </div>
                                                                                     <asp:Panel runat="server" ID="pnlotp" Visible="false" >
                             
                                  <div class="card card-body is-dark  ">
                                      <asp:label ID="lbloptcaption" runat="server"  CssClass="col-sm-2 col-form-label " text="Enter OTP"></asp:label>
                                      <div class="col-sm-3">
                                  <asp:TextBox runat="server" ID="txtotp" CssClass="form-control "  Width="200px" data-validation-engine="validate[required" data-errormessage-value-missing="Enter a Valid OTP"> </asp:TextBox>
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
            Processing.....
        </div>
    </div>
</ProgressTemplate>
</asp:UpdateProgress>

                                                       <asp:UpdatePanel runat="server" ID="pnlbtn">
                                                           <ContentTemplate>

                                                           
                                                       <div class="form-group row border-bottom ">
                                                       </div>
                                                       <div class="form-group row">
                                                           <div class="col-sm-4">

                                                           </div>
                                                                
                                                           <div class="col-sm-4">
                                                               <div class="btn-group " role="group" >
                                                                 <asp:Button ID="btn_up_can" runat="server" CssClass="btn-outline-secondary  btn"  CausesValidation="false"   Text="Clear" />
                        <asp:Button ID="btn_up_rcpt" runat="server" CssClass="btn-outline-primary btn"  Text="Save"   />
                                                               </div>
                                                           </div>

                                                       </div>
                                                               </ContentTemplate>
                                                       </asp:UpdatePanel>

                                                     
                                                           </asp:Panel>

                                                         <asp:Panel runat="server" ID="pnlprnt" style="display:none"  >

                                                               <asp:UpdatePanel runat="server" ID="pnlbtnprnt" Visible="true" >
                                                           <ContentTemplate>
                                                               <div class="form-group row border-bottom  ">
                        <div class="col-md-3"></div>
                        <div class="col-md-6">
                            <div class="btn-group" role="group" >
                                                        <asp:Button runat="server" ID="btntog"  Class="btn btn-outline-primary" Text="Close" />
                                                        <button type="button"  Class="btn btn-outline-primary" onclick="PrintOC();">Print Office Copy</button> 
                                                        <button type="button"  Class="btn btn-outline-primary" onclick="PrintCC();" >Print Customer Copy</button> 
                                            </div>           
                        </div>
                    </div>
                                                           </ContentTemplate>
                                                       </asp:UpdatePanel>
                        <div id="vouchprint">
                   <div  class="prntarea "   >
                   
                   
                       <table style="border:1px solid #000;width:100%">
                           <tbody>
                               <tr>

                             <td>
                        <table style="border-collapse: collapse; " >
        <tbody>
            <tr style="border: none;">
                <td style="width: 10%;"><img src="Images/KBF-LOGO.png" alt="" width="72" height="60" /></td>
                <td style="width: 58%;">
                    <table border="0" style="border-collapse: collapse; width:100%; height: 86px;" >
                        <tbody>
                            <tr style="height: 10px;">
                                <td style="width: 100%;font-size:18px;height:10px;font-weight:600">Karavilai Nidhi Limited
                                    
                                </td>
                            </tr>   
                            
                            <tr style="height: 10px;">
                                <td style="width: 100%; height: 10px;text-align: left; font-size: 11px;"><span>Reg No : 18-37630/97</span><br/><span style="margin-top:5px;">8-12A,Vijayam, Main Road Karavilai,Villukuri P. O</span></td>
                            </tr>
                            <tr>
                                <td style="width: 100%;font-size:12px;">Branch : <asp:Label id="pbranch" runat="server"></asp:Label></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="width:32%">
                    <table style="float: right; margin-left: 20px; height: 63px" width:"100%" >
                        <tbody>
                            <tr >
                                <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:600"><asp:label runat="server"  id="lblcpt" ></asp:label> </td>
                            </tr>
                            <tr >
                                <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 "><asp:label runat="server"  id="lblcptr" ></asp:label> </td>
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
    
                                      

        <table  style="border-top:1px solid ;border-bottom:1px solid;">
            <tr >
                <td style="width:69%;padding-left:10px">
                     
            <table style="width:100%;border-style:none;">
                <tbody>
                    <tr style="height:30px;">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Account No</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td  style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pacno" runat="server" ></asp:Label></td>
                    </tr>
                    
                    <tr style="height: 30px;border-bottom:1px solid black">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Account</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pglh" runat="server" ></asp:Label></td>
                    </tr>
                    
                    <tr style="height: 30px;">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Member No</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pcid" runat="server" ></asp:Label></td>
                    </tr>
                    
                    <tr style="height: 30px;border-bottom:1px solid black">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Customer Name</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td  style="width: 263px; height: 21px;font-size: 15px""><asp:Label ID="pcname" runat="server" ></asp:Label></td>
                    </tr>
                    
                    <tr style="height: 30px;">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Amount</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td  style="width: 263px; height: 21px;font-size:15px"><b><asp:Label ID="pamt" runat="server" ></asp:Label></b>
                            
                        </td>
                    </tr>
                    
                    <tr style="height: 25px;">
                        <td colspan="3" style="font-size: 12px; width: 423px; height: 21px;">
                            <asp:Label runat="server" id="pnar"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        
                </td>

            <td style="width:31%">
            </td>
            </tr>
            <tr>
            <td colspan="2" style="padding-left:5px">
                  Amount in Words :&nbsp;<asp:Label ID="paiw" runat="server"  ></asp:Label>
                    
            </td>
            </tr>
        </table>
       
        
                       
    
        
        <table   style="height:15px;margin-top:10px;width:100%">
            <tbody>
                <tr>
                    <td style="width:35%; text-align: center;font-size: 14px">Incharge / Manager</td>
                    
                    <td style="width: 30%; text-align: center;font-size: 14px">Cashier</td>
                    <td style="width: 35%; text-align: center;font-size: 13px">(&nbsp;<asp:Label runat="server" ID="premit"></asp:Label>&nbsp;)</td>
                    
                    
                </tr>
            </tbody>
        </table>
    
                                 </td>      
                                </tr>
                           </tbody>
                       </table>
                               

                   </div>
                           
                            
                            <div  class="prntarea "   >
                   
                   
                       <table style="border:1px solid #000;width:100%">
                           <tbody>
                               <tr>

                             <td>
                        <table style="border-collapse: collapse; " >
        <tbody>
            <tr style="border: none;">
                <td style="width: 10%;"><img src="Images/KBF-LOGO.png" alt="" width="72" height="60" /></td>
                <td style="width: 58%;">
                    <table border="0" style="border-collapse: collapse; width:100%; height: 86px;" >
                        <tbody>
                            <tr style="height: 10px;">
                                <td style="width: 100%;font-size:18px;height:10px;font-weight:600">Karavilai Nidhi Limited
                                    
                                </td>
                            </tr>   
                            
                            <tr style="height: 10px;">
                                <td style="width: 100%; height: 10px;text-align: left; font-size: 11px;"><span>Reg No : 18-37630/97</span><br/><span style="margin-top:5px;">8-12A,Vijayam, Main Road Karavilai,Villukuri P. O</span></td>
                            </tr>
                            <tr>
                                <td style="width: 100%;font-size:12px;">Branch : <asp:Label id="pcbranch" runat="server"></asp:Label></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="width:32%">
                    <table style="float: right; margin-left: 20px; height: 63px" width:"100%" >
                        <tbody>
                            <tr >
                                <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:600"><asp:label runat="server"  id="lblccpt" ></asp:label> </td>
                            </tr>
                            <tr >
                                <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 "><asp:label runat="server"  id="lblccptr" ></asp:label> </td>
                            </tr>

                             <tr style="height: 21px;">
                                <td style="width: 82px; height: 21px;font-size: 15px">No&nbsp; &nbsp; :</td>
                                <td style="width: 90px; height: 21px;font-size: 15px"><asp:Label ID="pcvno" runat="server" ></asp:Label></td>
                            </tr>
                            <tr style="height: 21px;">
                                <td style="width: 82px; height: 21px;font-size: 15px">Date :</td>
                                <td style="width: 90px; height: 21px;font-size: 15px"><asp:Label ID="pcdate" runat="server" ></asp:Label></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    
                                      

        <table  style="border-top:1px solid ;border-bottom:1px solid;">
            <tr >
                <td style="width:69%;padding-left:10px">
                     
            <table style="width:100%;border-style:none;">
                <tbody>
                    <tr style="height:30px;">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Account No</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td  style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pcacno" runat="server" ></asp:Label></td>
                    </tr>
                    
                    <tr style="height: 30px;border-bottom:1px solid black">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Account</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pcglh" runat="server" ></asp:Label></td>
                    </tr>
                    
                    <tr style="height: 30px;">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Member No</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pccid" runat="server" ></asp:Label></td>
                    </tr>
                    
                    <tr style="height: 30px;border-bottom:1px solid black">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Customer Name</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td  style="width: 263px; height: 21px;font-size: 15px""><asp:Label ID="pccname" runat="server" ></asp:Label></td>
                    </tr>
                    
                    <tr style="height: 30px;">
                        <td style="font-size: 12px; width: 139px; height: 21px;">Amount</td>
                        <td style="width: 21px; height: 21px;"></td>
                        <td  style="width: 263px; height: 21px;font-size:15px"><b><asp:Label ID="pcamt" runat="server" ></asp:Label></b>
                            
                        </td>
                    </tr>
                    
                    <tr style="height: 25px;">
                        <td colspan="3" style="font-size: 12px; width: 423px; height: 21px;">
                            <asp:Label runat="server" id="pcnar"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        
                </td>

            <td style="width:31%">
            </td>
            </tr>
            <tr>
            <td colspan="2" style="padding-left:5px">
                  Amount in Words :&nbsp;<asp:Label ID="pcaiw" runat="server"  ></asp:Label>
                    
            </td>
            </tr>
        </table>
       
        
                       
    
        
        <table   style="height:15px;margin-top:10px;width:100%">
            <tbody>
                <tr>
                    <td style="width:35%; text-align: center;font-size: 14px">Incharge / Manager</td>
                    
                    <td style="width: 30%; text-align: center;font-size: 14px">Cashier</td>
                    <td style="width: 35%; text-align: center;font-size: 13px">(&nbsp;<asp:Label runat="server" ID="pcremit"></asp:Label>&nbsp;)</td>
                    
                    
                </tr>
            </tbody>
        </table>
    
                                 </td>      
                                </tr>
                           </tbody>
                       </table>
                               

                   </div>
                           
                        </div>      
                        </asp:Panel>
                      

                                               </ContentTemplate>
                                                   </asp:UpdatePanel>
                                            
                                            
                                        
                                </div>
                            </div>
                            

                        
                        </div>
                    </div>
            
             
          
        
                    

                
                       
                
                
               

                        
                 


        </form>



    

	  <script src="js/jquery.js" type="text/javascript"></script>

	 
    <script src="js/daterangepicker.js" type="text/javascript" ></script>
	
    <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="js/jquery.validationEngine.js" type="text/javascript" ></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
   	 <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    
   <script src="js/printThis.js" type="text/javascript"></script>
   
	<script type="text/javascript">


        $(document).ready(function () {

            $("#frmpayment").validationEngine('attach');
            $("#frmpayment").validationEngine({ promptPosition: "topleft", scroll: false, focusFirstField: true, showArrow: true });
            $("#frmpayment").validationEngine('attach', { promptPosition: "topleft", scroll: false, showArrow: true });




          

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            
            Dpick();
            InitAutoComplacn();
            InitAutoComplSB();



        });


        function InitializeRequest(sender, args) {
        }
        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            Dpick();

            InitAutoComplacn();
            InitAutoComplSB();
        }




        function InitAutoComplSB() {

            $("#<%=txt_sb.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/membersearch.asmx/GetSbAc") %>',
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
                open: function () {
                    $("ul.ui-menu").width($(this).innerWidth() + 210);

                },

				select: function (event, ui) {

                    $("#<%=txt_sb.ClientID %>").val(ui.item.memberno);



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
        function Dpick() {

            $("#<%=tdate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": true,
                "showDropdowns": false,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=tdate.ClientID%>").val(start.format('DD-MM-YYYY'));
            });

         }



      
        function InitAutoComplacn() {


            $("#<%=txtacn.ClientID %>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/membersearch.asmx/GetNewLoan") %>',
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
                                    product: item.Product
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
                         .append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
                         .append("<div class='userinfo'>" + item.lastname + "<div class='product text-muted'>" + item.product + "</div></div>")
                         .append("<div class='userinfo'>" + item.address + "</div>")
                         .append("</br>")
                         .append("</div")
                         .append("</div>")
                         .appendTo(ul);
                 };
        }



        function DateFormat(field, rules, i, options) {
            var regex = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
            if (!regex.test(field.val())) {
                return "Please enter date in dd/MM/yyyy format."
            }
        };

        function chkdecim(field, rules, i, options) {
            var regex = /^\d+\.?\d{0,2}$/;
            if (!regex.test(field.val())) {
                return "Please enter a Valid Number."
            }
        }

        function detach() {
            $("#frmpaymet").validationEngine('detach');

        }


        function PrintCC() {

            

            $('#vouchprint').printThis({
                importCSS: false,
                importStyle: true,         // import style tags
                printContainer: true,


            });


        }

        function PrintOC() {

            
                
                $("#<%=lblcptr.ClientID%>").text("OFFICE COPY");
                $("#<%=lblccptr.ClientID%>").text("OFFICE COPY");
            

            $('#vouchprint').printThis({
                importCSS: false,
                importStyle: true,
                printContainer: true,



            });
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

        .ui-autocomplete {
    max-height: 510px;
    overflow-y: auto;
    /* prevent horizontal scrollbar */
    overflow-x: hidden;
  }

    </style>
        
</asp:Content>

