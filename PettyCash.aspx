<%@ Page Title="Petty Cash" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="PettyCash.aspx.vb" Inherits="Fiscus.PettyCash" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/daterangepicker.css" />
    
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
}
        .prntarea{
             height: 105mm;
            width: 148.0mm;
            background-color:#fff;

    margin: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    	<form id="pettycash" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Transaction</a></li>
						<li class="breadcrumb-item active" >Petty Cash</li>
					</ol>
				</nav>
	
			

    
   

        		<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								    <div id="smarttab">
                                        <ul class="nav" id="tabitm"  >
           <li id="cTab">
               <a class="nav-link" href="#tab-1">
                    Transaction
                   </a>
           </li>
           <li id="jTab">
               <a class="nav-link" href="#tab-2">
                  Transfer
               </a>
           </li>
            <li id="dTab"><a class="nav-link" href="#tab-3" >
                  New Entry
               </a></li>
    </ul>
                                       
										<div class="tab-content">
                                           <div id="tab-1" class="tab-pane" role="tabpanel">

                                               <div class="card card-body ">
                                                   <div class="form-group row ">
                                                   <label class="col-sm-2 col-form-label ">Date</label>
                                                   <div class="col-sm-3">
                    <div class="input-group">

                          <asp:TextBox  ID="txtdate" runat="server" AutoCompleteType="None"   Width ="120px" CssClass="form-control "  data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
            <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_inw" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                      
                    </div>
                </div>
                                                       </div>
                                                   <div class="form-group row border-top "></div>
                                                      
                                                             <table style="width:100%;margin-left:0px;float:left "  class="table table-bordered ">
                                  <thead>
                                         <tr style="color:white"><th style="width:10%;">Trans Id</th>
                                         <th style="width:10%;">Type </th>
                                         <th style="width:20%;">Account No</th>
                                         <th style="width:30%;">Customer Name</th>
                                         <th style="width:15%;">Debit</th>
                                         <th style="width:15%;">Credit</th>
                                             
                                         </tr>
                                      </thead>
                                  </table> 
                                                               
                                                                <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" 
                            AllowPaging ="true" OnPageIndexChanging ="OnPaging" PageSize ="30" Width="100%" style="margin-top:0px;margin-left:0px;float:left;z-index:1" 
                               Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered "   >

                                            <Columns>

                                                <asp:TemplateField >
                                                <ItemTemplate>
                                                <asp:Label ID="lbl_cr_id" runat="server" text='<%#Eval("tid")%>' Style="text-align:right;float:right;font-size:small"></asp:Label>
                                                </ItemTemplate>
                                                    <ItemStyle Width ="10%" />
                                                                                            </asp:TemplateField>
                                                
                                                <asp:TemplateField>
                                                <ItemTemplate>
                                                <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("product")%>' Style="text-align:left ;float:left;font-size:small "></asp:Label>
                                                </ItemTemplate>
                                                    <ItemStyle Width ="10%" />
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField>
                                                <ItemTemplate>
                                                <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("acno")%>' Style="text-align:left ;float:left;font-size:small "></asp:Label>
                                                </ItemTemplate>
                                                    <ItemStyle Width ="20%" />
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField>
                                                <ItemTemplate>
                                                <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("FirstName")%>' Style="text-align:left ;float:left;font-size:small "></asp:Label>
                                                </ItemTemplate>
                                                    <ItemStyle Width ="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                <ItemTemplate>
                                                <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("dr", "{0:N}")%>' Style="text-align:right;float:right ;font-size:small "></asp:Label>
                                                </ItemTemplate>
                                                    <ItemStyle Width ="15%" />
                                                </asp:TemplateField>
                                                
                                             <asp:TemplateField>
                                                <ItemTemplate>
                                                <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("cr", "{0:N}")%>' Style="text-align:right;float:right ;font-size:small "></asp:Label>
                                                </ItemTemplate>
                                                    <ItemStyle Width ="15%" />
                                                </asp:TemplateField>

                                            </Columns>


                                            </asp:GridView> 

                                                                                               <table style="border:0px;width:100%;margin-left:0px;float:left " class="table table-bordered"  >
                                  <tbody>
                                         <tr >
                                          <td style="width:70%;text-align:center;font-variant:small-caps">Total</td>
                                         <td style="width:15%;text-align:center;font-variant:small-caps;">

                                             <asp:UpdatePanel runat="server" >
                                                 <ContentTemplate >
                                             <asp:Label ID="lbl_sum_dr" runat="server" style="float:right;text-align:right"></asp:Label>
                                                     </ContentTemplate>
                                                 </asp:UpdatePanel>
                                         </td>
                                         <td style="width:15%;text-align:center;font-variant:small-caps;">
                                             <asp:UpdatePanel runat="server" >
                                                 <ContentTemplate >
                                             <asp:Label ID="lbl_sum_cr" runat="server" style="float:right;text-align:right"></asp:Label>
                                                     </ContentTemplate>
                                                 </asp:UpdatePanel>
                                         </td>
                                             
                                         </tr>
                                          </tbody>


                                  </table> 
                                                               <table style="border:0px;width:100%;margin-left:0px;float:left " class="table table-bordered" >
                                  <tbody>
                                         <tr >
                                         <td style="width:70%;text-align:center;font-variant:small-caps">Balance</td>
                                         <td style="width:15%;text-align:center;font-variant:small-caps;">

                                             <asp:UpdatePanel runat="server" >
                                                 <ContentTemplate >
                                             <asp:Label   ID="lbl_bal_dr" runat="server" style="float:right;text-align:right"> </asp:Label>
                                                     </ContentTemplate>
                                                 </asp:UpdatePanel>
                                         </td>
                                         <td style="width:15%;text-align:center;font-variant:small-caps;">
                                             <asp:UpdatePanel runat="server" >
                                                 <ContentTemplate >
                                             <asp:Label  ID="lbl_bal_cr" runat="server" style="float:right;text-align:right"></asp:Label>
                                                     </ContentTemplate>
                                                 </asp:UpdatePanel>
                                         </td>
                                             
                                         </tr>
                                          </tbody>


                                  </table> 



                                                       
                                                   
                                               </div>
                                               
                                               </div>
                                            <div id="tab-2" class="tab-pane " role="tabpanel" >

                                                <asp:UpdatePanel runat="server" >
                                                    <ContentTemplate>

                                                <asp:Panel runat="server" ID="pnltrf" style="display:block;" >
                                                    <div class="card card-body ">
                                                <asp:UpdatePanel runat="server" >
                                                    <ContentTemplate>
                                                        <div class="form-group row border-bottom ">
                                                            <label class="col-sm-2 col-form-label ">Balance to be Transfered </label>
                                                            <div class="col-sm-2">
                                                            <asp:Label runat="server" ID="lbltrf" CssClass="text-primary " Font-Size="Large" Font-Bold="true" style="float:right "></asp:Label>
                                                                </div>
                                                        </div>

                                                <div class="form-group row">
                                                    <label class="col-sm-2 col-form-label ">Transfer To</label>
                                                    <div class="col-sm-3">
                                                        <asp:UpdatePanel runat="server" >
                                                            <ContentTemplate>
                                                                
                                                         <asp:TextBox  ID="sbacno" CssClass="form-control custom-select-sm "   Width="200px" runat="server" AutoPostBack="true"  ></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <label class="col-sm-2 col-form-label ">Customer Name</label>
                                                          <div class="col-sm-3">
                                                    <asp:UpdatePanel runat="server" >
                                                        <ContentTemplate>

                                                    
                                                    <asp:Label ID="lblname" runat="server" CssClass="text-primary col-form-label"></asp:Label>
                                                              
                                                            </ContentTemplate>
                                                            
                                                    </asp:UpdatePanel>
                                                              </div>
                                                    <label class="col-sm-2 col-form-label ">Balance</label>
                                                    <asp:Label ID="lblacbal" runat="server" CssClass="col-sm-3 text-primary "></asp:Label>

                                                </div>

                                                <div class ="form-group row ">
                                                    <label class="col-sm-2 col-form-label ">Amount to Transfer</label>
                                                    <div class="col-sm-3">
                                                         <asp:TextBox ID="txtamt" runat="server" Width="120px" CssClass="form-control "></asp:TextBox>
                                                    </div>
                                                </div>
                                                    <div class="form-group row border-bottom "></div>
                                                    <div class="form-group row ">
                                                        <div class="col-sm-4"></div>
                                                        <div class="col-sm-4">
                                                            <div class="btn-group " role="group" >
                                                                 <asp:Button ID ="Button1" CssClass ="btn btn-outline-secondary  " Text ="Clear" runat="server" Visible="true" />
                                                                 <asp:Button ID ="btn_trans" CssClass ="btn btn-outline-primary " Text ="Transfer" runat="server" Visible="true" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                        
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                        </div>
                                                    

                                            </asp:Panel>
                                                  
                                                <asp:Panel runat="server" ID="pnlpr" style="display:none;">

                                                    <div class="card card-body" id="printtab">

                                        <div class="form-group row border-bottom  ">
                                                            <div class="col-md-3"></div>
                                                                <div class="col-md-6">
                            <div class="btn-group" role="group" >

                                                        <asp:button id="btntog" runat="server"   Class="btn btn-outline-primary   " Text="Close" />
                                                        <button type="button"  Class="btn btn-outline-primary" onclick="PrintOC();">Print Office Copy</button> 
                                                      
                                            </div>           
                        </div>
                    </div>

                                 
                   
                                                           <div id="vouchprint" class="prntarea "   >
                   
                   
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
                                <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 "><asp:label runat="server"  id="lblcptr" Text="OFFICE COPY" ></asp:label> </td>
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
            <%--<table style="width:100%;margin-top :10px;  border: 1px solid; padding-right: 0px; float: right;" id="dtab">
                <tbody>
                    <tr style=" border: none;">
                        <td style="width: 49px;  font-size: 12px; text-align: right; ">2000 x</td>
                        <td style="width: 21px;  font-size: 12px;text-align: right;"><asp:Label ID="p2kcount" runat="server" ></asp:Label></td>
                        <td style="width: 22px;  font-size: 12px;">=</td>
                        <td style="width: 89px;  font-size: 12px; text-align: right;padding-right:10px"><asp:Label ID="p2kval" runat="server" ></asp:Label></td>
                    </tr>
                    <tr ">
                        <td style="width: 48px;  font-size: 12px; text-align: right; padding-right: 5px;">500 x</td>
                        <td style="width: 21px;  font-size: 12px;text-align: right;"><asp:Label ID="p500count" runat="server" ></asp:Label></td>
                        <td style="width: 22px;  font-size: 12px;">=</td>
                        <td style="width: 89px;  font-size: 12px; text-align: right;padding-right:10px"><asp:Label ID="p500val" runat="server" ></asp:Label></td>
                    </tr>
                    <tr >
                        <td style="width: 48px;  font-size: 12px; text-align: right; padding-right: 5px;">200 x</td>
                        <td style="width: 21px;  font-size: 12px;text-align: right;"><asp:Label ID="p200count" runat="server" ></asp:Label></td>
                        <td style="width: 22px;  font-size: 12px;">=</td>
                        <td style="width: 89px;  font-size: 12px; text-align: right;padding-right:10px"><asp:Label ID="p200val" runat="server" ></asp:Label></td>
                    </tr>
                    <tr ">
                        <td style="width: 48px;  font-size: 12px; text-align: right; padding-right: 5px;">100 x</td>
                        <td style="width: 21px;  font-size: 12px;text-align: right;"><asp:Label ID="p100count" runat="server" ></asp:Label></td>
                        <td style="width: 22px;  font-size: 12px;">=</td>
                        <td style="width: 89px;  font-size: 12px; text-align: right;padding-right:10px"><asp:Label ID="p100val" runat="server" ></asp:Label></td>
                    </tr>
                    <tr ">
                        <td style="width: 48px;  font-size: 12px; text-align: right; padding-right: 5px;">50 x</td>
                        <td style="width: 21px;  font-size: 12px;text-align: right;"><asp:Label ID="p50count" runat="server" ></asp:Label></td>
                        <td style="width: 22px;  font-size: 12px;">=</td>
                        <td style="width: 89px;  font-size: 12px; text-align: right;padding-right:10px"><asp:Label ID="p50val" runat="server" ></asp:Label></td>
                    </tr>
                    <tr ">
                        <td style="width: 48px;  font-size: 12px; text-align: right; padding-right: 5px;">20 x</td>
                        <td style="width: 21px;  font-size: 12px;text-align: right;"><asp:Label ID="p20count" runat="server" ></asp:Label></td>
                        <td style="width: 22px;  font-size: 12px;">=</td>
                        <td style="width: 89px;  font-size: 12px; text-align: right;padding-right:10px"><asp:Label ID="p20val" runat="server" ></asp:Label></td>
                    </tr>
                    <tr >
                        <td style="width: 48px;  font-size: 12px; text-align: right; padding-right: 5px;">10 x</td>
                        <td style="width: 21px;  font-size: 12px;text-align: right;"><asp:Label ID="p10count" runat="server" ></asp:Label></td>
                        <td style="width: 22px;  font-size: 12px;">=</td>
                        <td style="width: 89px;  font-size: 12px; text-align: right;padding-right:10px"><asp:Label ID="p10val" runat="server" ></asp:Label></td>
                    </tr>
                    <tr >
                        <td colspan="2" style="width: 60px; height: 31px; font-size: 12px; text-align: right; padding-right: 10px;">Coins</td>
                        
                        <td style="width: 22px;  font-size: 12px;text-align: right;">=</td>
                        <td style="width: 89px;  font-size: 12px; text-align: right;padding-right:10px"><asp:Label ID="pcoin" runat="server" ></asp:Label></td>
                    </tr>
                    <tr style="border:1px solid " >
                        <td colspan="3" style="text-align: left; padding-right: 0px;  height: 29px;font-size: 13px;">Total</td>
                        <td  style="text-align: right; padding-right: 10px;height:29px;font-size: 15px;font-weight:bold"><asp:Label ID="ptotal" runat="server" ></asp:Label></td>
                    </tr>
                </tbody>
            </table>--%>
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
                      
                        
                        
                                                    </div>
                                                </asp:Panel> 
                                                  
                                                
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>



                                            </div>
                                            <div id="tab-3" class="tab-pane " role="tabpanel" >
                                                
                                                    <div class="form-group row ">
                                                    <label class="col-sm-2 col-form-label text-primary ">Loan</label>
                                                    <div class="col-sm-3">
                                                         <asp:TextBox runat="server" CssClass ="form-control " ID="txtac"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row ">
                                                    <label class="col-sm-2 col-form-label text-primary ">Account No</label>
                                                    <div class="col-sm-3">
                                                         <asp:TextBox runat="server" CssClass ="form-control " ID="txtacn"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row ">
                                                    <label class="col-sm-2 col-form-label text-primary ">Customer Name</label>
                                                    <div class="col-sm-3">
                                                         <asp:TextBox runat="server" CssClass ="form-control " ID="txtname"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row ">
                                                    <label class="col-sm-2 col-form-label text-primary ">Debit</label>
                                                    <div class="col-sm-3">
                                                         <asp:TextBox runat="server" CssClass ="form-control " ID="txtdr"></asp:TextBox>
                                                    </div>
                                                    <label class="col-sm-2 col-form-label text-primary ">Credit</label>
                                                    <div class="col-sm-3">
                                                         <asp:TextBox runat="server" CssClass ="form-control " ID="txtcr"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row border-sm-bottom  "></div>
                                                <div class="form-group  row ">
                                                    <div class="col-sm-4"></div>
                                                    <div class="col-sm-4">
                                                        <div class="btn-group " role="group">
                                                            <asp:Button CssClass ="btn btn-sm btn-outline-secondary  "  runat="server" ID="btn_clr" Text ="Clear" />
                                                            <asp:Button CssClass ="btn btn-sm btn-outline-primary "  runat="server" ID="btn_add_diff" Text ="Update" />
                                                        </div>
                                                    </div>
                                                </div>

                                                </div>
                                            </div>
                                        </div>
                                </div>
                            </div>
                        </div>
                    </div>
            </form>

    
	
	    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="js/daterangepicker.js" type="text/javascript"></script>
	<script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui.min.css" rel="stylesheet" />
    <script src="js/printThis.js" type="text/javascript"></script>

	<script type="text/javascript">




        $(document).ready(function () {



            
            
        $('#smarttab').smartTab({
            selected: 0, // Initial selected tab, 0 = first tab
          //  theme: 'dark', // theme for the tab, related css need to include for other than default theme
            orientation: 'horizontal', // Nav menu orientation. horizontal/vertical
            justified: false, // Nav menu justification. true/false
            autoAdjustHeight: false, // Automatically adjust content height
            backButtonSupport: true, // Enable the back button support
            enableURLhash: true, // Enable selection of the tab based on url hash
            transition: {
                animation: 'slide-swing', // Effect on navigation, none/fade/slide-horizontal/slide-vertical/slide-swing
                speed: '400', // Transion animation speed
                easing: '' // Transition animation easing. Not supported without a jQuery easing plugin
            },
            keyboardSettings: {
                keyNavigation: true, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
                keyLeft: [37], // Left key code
                keyRight: [39] // Right key code
            }
        });


           

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        // Place here the first init of the autocomplete
            InitAutoCompl();
            InitAutoComplacn();
        
    });


        function InitializeRequest(sender, args) {
        }





        

 

    

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            
            InitAutoComplacn();
            InitAutoCompl();
        }


       

        function InitAutoCompl() {


            $("#<%=txtdate.ClientID%>").daterangepicker({

                "singleDatePicker": true,
                "autoUpdateInput": false,
                "autoApply": true,
                locale: {
                    format: 'DD-MM-YYYY'
                },
            }, function (start, end, label) {

                $("#<%=txtdate.ClientID%>").val(start.format('DD-MM-YYYY'));
            });


        }

        function PrintOC() {

                            //document.getElementById('lblcptr').innerHTML = "OFFICE COPY";
           

               $('#vouchprint').printThis({
                   importCSS: false,
                   importStyle: true,




               });
           }
      

        function InitAutoComplacn() {

       

            $("#<%=sbacno.ClientID %>").autocomplete({
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
                                    image: item.ImageURL,
                                    product: item.Product
								}
							}))
						}
					});
				},
                minLength: 1,
                focus: function (event, ui) {

                    $("#<%=sbacno.ClientID %>").val(ui.item.memberno);
                    return false;

                },
                open: function () {
                    $("ul.ui-menu").width($(this).innerWidth() + 180);

                },

				select: function (event, ui) {

                    $("#<%=sbacno.ClientID %>").val(ui.item.memberno);
                    $("#<%=lblname.ClientID %>").val(ui.item.firstname);


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
