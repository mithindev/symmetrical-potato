<%@ Page Title="Deposit Closure" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="DepositClosure.aspx.vb" Inherits="Fiscus.DepositClosure" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/daterangepicker.css" rel="stylesheet" />

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

      <form id="depositclosure" runat="server" >

        <asp:ScriptManager ID="SM4" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Deposit</a></li>
						<li class="breadcrumb-item active" >Closure</li>
					</ol>
				</nav>
	
			

    
            <asp:UpdatePanel  runat="server" >
			<ContentTemplate>
                <asp:Panel ID="alertmsg" runat="server" Visible="false" >
		<div class="alert alert-danger  alert-dismissible fade show" role="alert">
			<h5>
	 <asp:Label ID="lblinfo" runat="server" CssClass="text-dark" ></asp:Label></h5>
           
				<button type="button" class="close" data-dismiss="alert" aria-label="Close">

		<span aria-hidden="true">&times;</span>
	</button>
				
	</div>
                    </asp:Panel>

                  
		
        
				</ContentTemplate>
			</asp:UpdatePanel>
          
     
          <asp:UpdatePanel runat="server" >
              <ContentTemplate>

              
        		<div class="row">
				<div class="col-md-12 card card-body ">
                    <asp:Panel runat="server" ID="pnltrans" style="display:block" >
						<div class="form-group row">
							<div class="col-md-8">
		
                                       
										
                                           
                                              <asp:UpdatePanel runat="server" ID="tabUP">
                                                  <ContentTemplate>


               <div class="form-group row ">
                   <label class="col-sm-3 col-form-label ">Account No</label>
                   <div class="col-sm-4">
                       <asp:UpdatePanel runat="server" >
                           <ContentTemplate>

                           
                    <div class="input-group">
                                                           
                                                             <asp:TextBox  ID="txtacn" runat="server" CssClass="form-control"  AutoPostBack="true"  ></asp:TextBox>
                                                            <div class="input-group-append">
                                                                <asp:LinkButton runat="server" ID="soa" Enabled="false"   >
                                                                   <span class="input-group-addon input-group-text text-primary" style="height:35px">

<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-file-text"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path><polyline points="14 2 14 8 20 8"></polyline><line x1="16" y1="13" x2="8" y2="13"></line><line x1="16" y1="17" x2="8" y2="17"></line><polyline points="10 9 9 9 8 9"></polyline></svg>
                                                                </span> </asp:LinkButton>
                                                                                </div>
                                                              
                                                            </div>
                               </ContentTemplate>
                       </asp:UpdatePanel>
                   </div>
               
                                
                                </div>
                                                      <div class="form-group row ">
                                                          <label class="col-sm-3 col-form-label ">Date</label>
                                                          <div class="col-sm-3">
                                                              <asp:TextBox runat="server" CssClass="form-control"  ID="tdate" AutoPostBack="True" data-validation-engine="validate[required,funcCall[DateFormat[]]"  data-errormessage-value-missing="Valid Date is required!" ></asp:TextBox>
                                                          </div>
                                                          <label class="col-sm-3 col-form-label ">Maturity Date</label>
                                                          <asp:Label ID="MaturityLabel" CssClass="col-sm-3 col-form-label text-danger" runat="server"></asp:Label>
                                                      </div>
                           
                                                      <div class="form-group row ">
                                                          <label class="col-sm-3 col-form-label ">Deposit</label>
                                                          <asp:Label ID="lblproduct" CssClass="col-sm-3 col-form-label text-primary " runat="server"></asp:Label>
                                                          <label class="col-sm-3 col-form-label ">Customer Name</label>
                                                          
                                <td style="border:none"><asp:Label ID="lblname" CssClass="col-sm-3 col-form-label text-primary  " runat="server"></asp:Label></td>
                                                      </div>


                                                      <div class="form-group row ">
                                                          <label class="col-sm-3 col-form-label ">Deposit Amount</label>
                                                          <asp:Label ID="lblamt" CssClass="col-sm-3 col-form-label text-primary"  runat="server"></asp:Label>
                                                          <label class="col-sm-3 col-form-label ">Account Balance</label>
                                                          <asp:Label ID="lblbal" CssClass="col-sm-3 col-form-label text-success  " Font-Bold="true"  runat="server"></asp:Label>
                                                      </div>



                                                      
                                                          


                                                      <div class="form-group row ">
                                                          <label class="col-sm-3 col-form-label ">Payment Amount</label>

                                                          <div class="col-sm-3">
                                                               <asp:UpdatePanel ID="rcpt" runat="server">
                                    <ContentTemplate>
                                    <asp:TextBox runat="server" CssClass="form-control"  ID="txtamt" AutoPostBack="true" data-validation-engine="validate[required,funcCall[chkdecim[]]" data-errormessage-value-missing="Receipt Amount Missing" ></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                   
                                                          </div>
                                                      </div>


                                                      <div class="form-group row ">
                                                          <label class="col-sm-3 col-form-label ">Notes</label>
                                                          <div class="col-sm-3">
                                                              <asp:TextBox runat="server" CssClass="form-control" style="text-align:left  "  ID="txtnotes"  ></asp:TextBox>
                                                          </div>
                                                      </div>

                           
                    
                    
                                <asp:UpdatePanel runat="server" >
                                    <ContentTemplate>

                                    

                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label ">Mode of Payment</label>
                                <div class="col-sm-3">
                                    <asp:UpdatePanel ID="UpdatePanel10"  runat="server">
                                    <ContentTemplate>
                                    
                                        <asp:DropDownList ID="mop" runat="server" AutoPostBack="true"  CssClass="form-control ">
                                            <asp:ListItem>Cash</asp:ListItem>
                                            <asp:ListItem>Account</asp:ListItem>
                                            <asp:ListItem>Transfer</asp:ListItem>
                                        </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    
                                </div>
                                

                                         
                                   
                            </div>

                                        </ContentTemplate>
                                </asp:UpdatePanel>

                                                      <asp:UpdatePanel runat="server" >
                            <ContentTemplate>
                        
                    <div class="form-group row">
                                <asp:Label ID="lblsb" Text ="SB Account No" runat="server" CssClass="col-sm-3 col-form-label " Visible="false" ></asp:Label>

                                <div class="col-sm-3">
                                  
                                    <asp:TextBox  ID="txt_sb" runat="server" Visible="false"  CssClass="form-control" Width="130px" AutoPostBack="true" data-validation-engine="validate[required]"  data-errormessage-value-missing="Valid Account No is required!"></asp:TextBox>
                                </div>

                                <asp:Label runat="server" ID="lbl_sb_bal" CssClass="col-sm-2 text-success "  ></asp:Label>
                               
                            
                    </div>
                                </ContentTemplate>
                        </asp:UpdatePanel>

                                                    <asp:UpdatePanel runat="server" >
                                                        <ContentTemplate>

                                                        
                                                      <div class="form-group row" >
                                                           <asp:Label ID="lbl" Text ="Transfer From" CssClass="col-sm-3 col-form-label " runat="server" Visible="false" ></asp:Label>
                                                          <div class="col-sm-3">
                                                               <asp:UpdatePanel ID="lstbnk"  runat="server" >
                                            <ContentTemplate >
                                                <asp:DropDownList ID="bnk" runat="server" Width="200px" CssClass="form-control " Visible="false"  ></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                                          </div>
                                                      </div>
                                                            </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                      
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
 <asp:HiddenField runat="server" ID="damt" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="dword" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="camt" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="cword" ClientIDMode="Static" />

                                                      <asp:UpdatePanel runat="server" ID="pnlbtn">
                                                          <ContentTemplate>

                                                          
                                                      <div class="form-group row border-sm-bottom "></div>
                                                      <div class="form-group row ">
                                                          <div class="col-sm-4"></div>
                                                          <div class="col-sm-4">
                                                              <div class="btn-group " role="group" >
                                                              <asp:Button ID="btn_up_can" runat="server" CssClass="btn-outline-secondary  btn" Text="Clear"  CausesValidation="false" />
                        <asp:Button ID="btn_up_rcpt" runat="server" CssClass="btn-outline-primary btn"  Text="Save"   />
                                                                  </div>

                                                          </div>
                                                      </div>
                                                              </ContentTemplate>
                                                      </asp:UpdatePanel>

                        </ContentTemplate>

                    </asp:UpdatePanel>
 
                                           
                                        
                                </div>
                            <div class="col-md-4">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>

                                    
                                <asp:Panel runat="server" ID="pnlint" Visible="false" >

                                    
                                    <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>

                                        
                                        <div  id="colgrp">
                                 <div class="btn-group align-content-between " role="group" style="width:100%"  >
  <button class="btn btn-outline-primary" type="button"  data-toggle="collapse"  data-target="#dint"  aria-expanded="true" aria-controls="dint">
          D-Interest
  </button>
                                                                  
  <button class="btn btn-outline-primary" type="button" data-toggle="collapse"  data-target="#cint" aria-expanded="false" aria-controls="cint">
    C-Interest
  </button>
 
  
                                                               </div>


                                                       
          <div class="collapse " id="cint" data-parent="#colgrp" >

              <asp:UpdatePanel runat="server" >
                  <ContentTemplate>

                  

                  <table class="table table-hover" style="width:95%">
                      <tbody>

                      <tr>      
                                <th style ="text-align:center;width:30%">  </th>
                                <th style="text-align:center;width:40%">Maturity</th>
                               <th style="text-align:center;width:40%">PreClousre</th>
                            </tr>
                      <tr>
           <td style="padding:5px;"> <label>Period</label></td>
           <td style="padding:5px;"> <asp:Label runat="server" style="float:right "  Text="0.00" ID="lblperiod"></asp:Label></td>
           <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0.00" ID="lblpenalperiod"></asp:Label>   </td>
                      </tr>
                      <tr>
                           <td style="padding:5px;"><label class="text-primary">ROI - C</label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0%" ID="lblroi"></asp:Label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0%" ID="lblpenalroi"></asp:Label>    </td>
                      </tr>
                      <tr>
                         <td style="padding:5px;"><label>Total Interest</label></td>
                         <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0.00" ID="lblactualint"></asp:Label></td>
                         <td style="padding:5px;"><asp:Label runat="server" style="float:right "  text="0.00" ID="lblpenalint"></asp:Label></td>
                     </tr>
                      <tr>
                          <td style="padding:5px;"><label>Interest Paid</label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0.00" ID="lblintpaid"></asp:Label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0.00" ID="lblpenalintpaid"></asp:Label> </td>
                      </tr>
                      <tr>
                          <td style="padding:5px;"><label>To be Paid</label></td>
                          <td style="padding:5px;"> 
                              <asp:UpdatePanel runat="server" >
                                  <ContentTemplate>
                                      <asp:TextBox  runat="server" Width="100%"  AutoPostBack="true"  style="float:right"   Text="0.00" CssClass="form-control text-right  "  ID="lbl2bpaid"></asp:TextBox>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              </td>
                          <td style="padding:5px;float:right"><asp:Label runat="server" style="float:right" Width="100%" text="0.00" ID="lblpenal2bpaid"></asp:Label></td>
                      </tr>
                      <tr>
                          <td style="padding:5px;"><label>Penel Interest</label></td>
                          <td style="padding:5px;float:right;vertical-align:bottom;text-align:match-parent"><asp:Label runat="server" style="float:right " Text="0.00" ID="lblmpenal"></asp:Label></td>
                          <td style="padding:5px;">
                              <asp:UpdatePanel runat="server" >
                                  <ContentTemplate>
                                      <asp:TextBox runat="server" Width="100%" AutoPostBack="true"   text="0.00" ID="lblpenal" CssClass="form-control text-right"  ></asp:TextBox>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              </td>
                      </tr>

                      </tbody>
                  </table>

                      <%-- workinGS --%>

                      <table class="table table-hover" style="width:95%">
                      <tbody>

                      <tr>      
                                <th style ="text-align:center;width:30%">  </th>
                                <th style="text-align:center;width:40%">CLOSING</th>
                               <th style="text-align:center;width:40%">PRE CLOUSRE</th>
                            </tr>
                      <tr>
           <td style="padding:5px;"> <label>Period</label></td>
           <td style="padding:5px;"> <asp:Label runat="server" style="float:right "  Text="0.00" ID="Label1"></asp:Label></td>
           <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0.00" ID="Label2"></asp:Label>   </td>
                      </tr>
                      <tr>
                           <td style="padding:5px;"><label class="text-primary">ROI - C</label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0%" ID="Label3"></asp:Label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0%" ID="Label4"></asp:Label>    </td>
                      </tr>
                      <tr>
                         <td style="padding:5px;"><label>Total Interest</label></td>
                         <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0.00" ID="Label5"></asp:Label></td>
                         <td style="padding:5px;"><asp:Label runat="server" style="float:right "  text="0.00" ID="Label6"></asp:Label></td>
                     </tr>
                      <tr>
                          <td style="padding:5px;"><label>Interest Paid</label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0.00" ID="Label7"></asp:Label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0.00" ID="Label8"></asp:Label> </td>
                      </tr>
                      <tr>
                          <td style="padding:5px;"><label>To be Paid</label></td>
                          <td style="padding:5px;"> 
                              <asp:UpdatePanel runat="server" >
                                  <ContentTemplate>
                                      <asp:TextBox  runat="server" Width="100%"  AutoPostBack="true"  style="float:right"   Text="0.00" CssClass="form-control text-right  "  ID="TextBox1"></asp:TextBox>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              </td>
                          <td style="padding:5px;float:right"><asp:Label runat="server" style="float:right" Width="100%" text="0.00" ID="Label9"></asp:Label></td>
                      </tr>
                      <tr>
                          <td style="padding:5px;"><label>Penel Interest</label></td>
                          <td style="padding:5px;float:right;vertical-align:bottom;text-align:match-parent"><asp:Label runat="server" style="float:right " Text="0.00" ID="Label10"></asp:Label></td>
                          <td style="padding:5px;">
                              <asp:UpdatePanel runat="server" >
                                  <ContentTemplate>
                                      <asp:TextBox runat="server" Width="100%" AutoPostBack="true"   text="0.00" ID="TextBox2" CssClass="form-control text-right"  ></asp:TextBox>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              </td>
                      </tr>

                      </tbody>
                  </table>
              
  
     </ContentTemplate>
              </asp:UpdatePanel>
          
          
          
            </div>

                                                           
        <div class="collapse show" id="dint"  data-parent="#colgrp">
  
            <asp:UpdatePanel runat="server" >
                <ContentTemplate>
            
      
               <table class="table table-secondary" style="width:95%">
                      <tbody>
                        <tr>      
                                <th style ="text-align:center;width:30%">  </th>
                                <th style="text-align:center;width:40%">Maturity</th>
                               <th style="text-align:center;width:40%">PreClousre</th>
                            </tr>
                      <tr>
           <td style="padding:5px;"> <label>Period</label></td>
           <td style="padding:5px;"> <asp:Label runat="server" style="float:right " Text="0.00" ID="lblperiod_d"></asp:Label></td>
           <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0.00" ID="lblpenalperiod_d"></asp:Label>  </td>
                      </tr>
                      <tr>
                           <td style="padding:5px;"><label class="text-primary">ROI - D</label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0%" ID="lblroi_d"></asp:Label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0%" ID="lblpenalroi_d"></asp:Label>    </td>
                      </tr>
                     <tr>
                         <td style="padding:5px;"><label>Total Interest</label></td>
                         <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0.00" ID="lblactualint_d"></asp:Label></td>
                         <td style="padding:5px;"><asp:Label runat="server" style="float:right "  text="0.00" ID="lblpenalint_d"></asp:Label></td>
                     </tr>
                      <tr>
                          <td style="padding:5px;"><label>Interest Paid</label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " Text="0.00" ID="lblintpaid_d"></asp:Label></td>
                          <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0.00" ID="lblpenalintpaid_d"></asp:Label> </td>
                      </tr>
                      <tr>
                          <td style="padding:5px;"><label>To be Paid</label></td>
                          <td style="padding:5px;"> 
                              <asp:UpdatePanel runat="server" >
                                  <ContentTemplate>
                                      <asp:TextBox  runat="server" Width="100%"  AutoPostBack="true"  style="float:right"   Text="0.00" CssClass="form-control text-right  "  ID="lbl2bpaid_d"></asp:TextBox>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              </td>
                          <td style="padding:5px;float:right"><asp:Label runat="server" style="float:right" Width="100%" text="0.00" ID="lblpenal2bpaid_d"></asp:Label></td>
                      </tr>
                      <tr>
                          <td style="padding:5px;"><label>Penel Interest</label></td>
                          <td style="padding:5px;float:right"><asp:Label runat="server" style="float:right " Text="0.00" ID="lblmpenal_d"></asp:Label></td>
                          <td style="padding:5px;">
                              <asp:UpdatePanel runat="server" >
                                  <ContentTemplate>
                                      <asp:TextBox runat="server" Width="100%" AutoPostBack="true"   text="0.00" ID="lblpenal_d" CssClass="form-control text-right"  ></asp:TextBox>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              </td>
                      </tr>
                          </tbody>
                  </table>

               <%-- 
                    <table class="table table-secondary" style="width:95%">
                   <tbody>
                       <tr>
                           <th style="text-align:center;width:30%"></th>
                               <th style="text-align:center;width:40%">
                                    Closing
                               </th>
                                <th style="text-align:center;width:40%">
                                    PreClousre
                                </th>
                       </tr>
                       <tr>
                           <td style="padding:5px;"> <label>Period</label></td>
                           <td style="padding:5px;"> <asp:Label runat="server" style="float:right " Text="0.00" ID="lblperiod_d_w"></asp:Label></td>
                           <td style="padding:5px;"><asp:Label runat="server" style="float:right " text="0.00" ID="lblpenalperiod_d_w"></asp:Label>  </td>
                      </tr>
                   </tbody>
               </table>
              --%>

                    </ContentTemplate>
              </asp:UpdatePanel>
            

                                                           
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                    
                                    </asp:Panel>
                                        </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            
                            </div>

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
                                <td style="width: 82px; height: 21px;font-size: 12px">No&nbsp; &nbsp; :</td>
                                <td style="width: 90px; height: 21px;font-size: 12px"><asp:Label ID="pvno" runat="server" ></asp:Label></td>
                            </tr>
                            <tr style="height: 21px;">
                                <td style="width: 82px; height: 21px;font-size: 12px">Date :</td>
                                <td style="width: 90px; height: 21px;font-size: 12px"><asp:Label ID="pdate" runat="server" ></asp:Label></td>
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
                                <td style="width: 82px; height: 21px;font-size: 12px">No&nbsp; &nbsp; :</td>
                                <td style="width: 90px; height: 21px;font-size: 12px"><asp:Label ID="pcvno" runat="server" ></asp:Label></td>
                            </tr>
                            <tr style="height: 21px;">
                                <td style="width: 82px; height: 21px;font-size: 12px">Date :</td>
                                <td style="width: 90px; height: 21px;font-size: 12px"><asp:Label ID="pcdate" runat="server" ></asp:Label></td>
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
                         
                            <asp:Panel runat="server" ID="diffAmtPrint">
                                                        <div  class="prntarea "   id=>
               
               
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
                            <td style="width: 100%;font-size:12px;">Branch : <asp:Label id="pcdbranch" runat="server"></asp:Label></td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td style="width:32%">
                <table style="float: right; margin-left: 20px; height: 63px" width:"100%" >
                    <tbody>
                        <tr >
                            <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:600"><asp:label runat="server"  id="lblccdpt" ></asp:label> </td>
                        </tr>
                        <tr >
                            <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 "><asp:label runat="server"  id="lblccdptr" ></asp:label> </td>
                        </tr>

                         <tr style="height: 21px;">
                            <td style="width: 82px; height: 21px;font-size: 15px">No&nbsp; &nbsp; :</td>
                            <td style="width: 90px; height: 21px;font-size: 15px"><asp:Label ID="pcdvno" runat="server" ></asp:Label></td>
                        </tr>
                        <tr style="height: 21px;">
                            <td style="width: 82px; height: 21px;font-size: 15px">Date :</td>
                            <td style="width: 90px; height: 21px;font-size: 15px"><asp:Label ID="pcddate" runat="server" ></asp:Label></td>
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
                    <td  style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pcdacno" runat="server" ></asp:Label></td>
                </tr>
                
                <tr style="height: 30px;border-bottom:1px solid black">
                    <td style="font-size: 12px; width: 139px; height: 21px;">Account</td>
                    <td style="width: 21px; height: 21px;"></td>
                    <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pcdglh" runat="server" ></asp:Label></td>
                </tr>
                
                <tr style="height: 30px;">
                    <td style="font-size: 12px; width: 139px; height: 21px;">Member No</td>
                    <td style="width: 21px; height: 21px;"></td>
                    <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pcdcid" runat="server" ></asp:Label></td>
                </tr>
                
                <tr style="height: 30px;border-bottom:1px solid black">
                    <td style="font-size: 12px; width: 139px; height: 21px;">Customer Name</td>
                    <td style="width: 21px; height: 21px;"></td>
                    <td  style="width: 263px; height: 21px;font-size: 15px""><asp:Label ID="pcdcname" runat="server" ></asp:Label></td>
                </tr>
                
                <tr style="height: 30px;">
                    <td style="font-size: 12px; width: 139px; height: 21px;">Amount</td>
                    <td style="width: 21px; height: 21px;"></td>
                    <td  style="width: 263px; height: 21px;font-size:15px"><b><asp:Label ID="pcdamt" runat="server" ></asp:Label></b>
                        
                    </td>
                </tr>
                
                <tr style="height: 25px;">
                    <td colspan="3" style="font-size: 12px; width: 423px; height: 21px;">
                        <asp:Label runat="server" id="pcdnar"></asp:Label>
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
              Amount in Words :&nbsp;<asp:Label ID="pcdaiw" runat="server"  ></asp:Label>
                
        </td>
        </tr>
    </table>
   
    
                   

    
    <table   style="height:15px;margin-top:10px;width:100%">
        <tbody>
            <tr>
                <td style="width:35%; text-align: center;font-size: 14px">Incharge / Manager</td>
                
                <td style="width: 30%; text-align: center;font-size: 14px">Cashier</td>
                <td style="width: 35%; text-align: center;font-size: 13px">(&nbsp;<asp:Label runat="server" ID="pcdremit"></asp:Label>&nbsp;)</td>
                
                
            </tr>
        </tbody>
    </table>

                             </td>      
                            </tr>
                       </tbody>
                   </table>
                           

               </div>
                            </asp:Panel>
                        </div>      
                        </asp:Panel>
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
	<script type="text/javascript">


        $(document).ready(function () {

        
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            InitAutoComplacn();
            InitAutoComplSB();
            dpick();
            
            setTimeout(function () {
                document.getElementById('<%=txtacn.ClientID %>').focus();
              }, 200);

        });

        function InitializeRequest(sender, args) {
        }
        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            dpick();
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
                focus: function (event, ui) {
                    $("#<%=txt_sb.ClientID %>").val(ui.item.memberno);
                    return false;

                },
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


        function dpick() {

            $("#<%=tdate.ClientID%>").daterangepicker({

                 "singleDatePicker": true,
                 "autoUpdateInput": true,
                 showDropdowns: false,
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
                         url: '<%=ResolveUrl("~/membersearch.asmx/GetDeposit") %>',
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


        function PrintCC() {


            damt = document.getElementById('<%= damt.ClientID%>').value;
                    dword = document.getElementById('<%= dword.ClientID%>').value;

                    $("#<%=lblcptr.ClientID%>").text("CUSTOMER COPY");
                    $("#<%=lblccptr.ClientID%>").text("CUSTOMER COPY");
                    $("#<%=lblccdptr.ClientID%>").text("CUSTOMER COPY");
                    $("#<%=pamt.ClientID %>").text(damt);
                    $("#<%=paiw.ClientID %>").text(dword);
                    $("#<%=pcamt.ClientID %>").text(damt);
                    $("#<%=pcaiw.ClientID %>").text(dword); 



            $('#vouchprint').printThis({
                importCSS: false,
                importStyle: true,         // import style tags
                printContainer: true,


            });


        }

        function PrintOC() {


            camt= document.getElementById('<%= camt.ClientID%>').value; 
            cword = document.getElementById('<%= cword.ClientID%>').value; 

            $("#<%=lblcptr.ClientID%>").text("OFFICE COPY");
            $("#<%=lblccptr.ClientID%>").text("OFFICE COPY");
            $("#<%=lblccdptr.ClientID%>").text("OFFICE COPY");
            $("#<%=pamt.ClientID %>").text(camt);
            $("#<%=paiw.ClientID %>").text(cword); 
            $("#<%=pcamt.ClientID %>").text(camt);
            $("#<%=pcaiw.ClientID %>").text(cword);


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
