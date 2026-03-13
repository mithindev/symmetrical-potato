<%@ Page Title="Denomination" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Denomination.aspx.vb" Inherits="Fiscus.Denomination" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/Dashboard.css" />
    <link rel="stylesheet" href="css/stylelist.css" />
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

            <form id="payment" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Admin</a></li>
						<li class="breadcrumb-item active" >Denomination</li>
					</ol>
				</nav>

                <asp:Panel runat="server" ID="pnllst" style="display:block">
            <div class="container-xl wide-xl">
                <div class="nk-content-inner">
                    <div class="nk-content-body">
                        
                        <div class="nk-block">
                         

                            <div class="card card-bordered is-dark  card-stretch">
                                <div class="card-inner-group">
                                    <div class="card-inner position-relative card-tools-toggle">
                                        <div class="card-title-group">
                                            <div class="card-tools">
                                                <div class="form-inline flex-nowrap gy-2 ">
                                                    
                                                        <span>Date&nbsp;&nbsp;&nbsp;</span>
                                                    
                                                    <div class="form-wrap w-150px">
                                                        <asp:UpdatePanel runat="server" >
                                                            <ContentTemplate>

                                                            
                                   <asp:TextBox  ID="txtdate" runat="server" Width="120px" AutoPostBack="true"   CssClass="datepicker form-control "  data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                                </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        </div>
                                        <div class="form-wrap w-150px">
                                                        <asp:UpdatePanel runat="server" >
                                                            <ContentTemplate>

                                                            
                                                            <div class="form-check form-check-inline">
											<label class="form-check-label">
												<asp:CheckBox  runat="server"  ID="inclpass" AutoPostBack="true"   />
												Passed Vouchers
											<i class="input-frame"></i></label>
										</div>
                                                                </ContentTemplate>
                                                        </asp:UpdatePanel>
                                            </div>

                                                        <%--<select class="form-select form-select-sm select2-hidden-accessible" data-search="off" data-placeholder="Bulk Action" data-select2-id="1" tabindex="-1" aria-hidden="true">
                                                            <option value="" data-select2-id="3">Bulk Action</option>
                                                            <option value="email">Send Email</option>
                                                            <option value="group">Change Group</option>
                                                            <option value="suspend">Suspend User</option>
                                                            <option value="delete">Delete User</option>
                                                        </select><span class="select2 select2-container select2-container--default" dir="ltr" data-select2-id="2" style="width: 112.667px;"><span class="selection"><span class="select2-selection select2-selection--single" role="combobox" aria-haspopup="true" aria-expanded="false" tabindex="0" aria-disabled="false" aria-labelledby="select2-f26j-container"><span class="select2-selection__rendered" id="select2-f26j-container" role="textbox" aria-readonly="true"><span class="select2-selection__placeholder">Bulk Action</span></span><span class="select2-selection__arrow" role="presentation"><b role="presentation"></b></span></span></span><span class="dropdown-wrapper" aria-hidden="true"></span></span>--%>
                                                    
                                                    <%--<div class="btn-wrap"><span class="d-none d-md-block">
                                                        <button class="btn btn-dim btn-outline-light disabled">Apply</button></span><span class="d-md-none">
                                                            <%--<button class="btn btn-dim btn-outline-light btn-icon disabled"><em class="icon ni ni-arrow-right"></em></button>
                                                        </span></div>--%>
                                                </div>
                                            </div>
                                            <%--<div class="card-tools mr-n1">
                                                <ul class="btn-toolbar gx-1">
                                                    <li><a href="#" class="btn btn-icon search-toggle toggle-search" data-target="search"><em class="icon ni ni-search"></em></a></li>
                                                    <li class="btn-toolbar-sep"></li>
                                                    <li>
                                                        <div class="toggle-wrap"><a href="#" class="btn btn-icon btn-trigger toggle" data-target="cardTools"><em class="icon ni ni-menu-right"></em></a>
                                                            <div class="toggle-content" data-content="cardTools">
                                                                <ul class="btn-toolbar gx-1">
                                                                    <li class="toggle-close"><a href="#" class="btn btn-icon btn-trigger toggle" data-target="cardTools"><em class="icon ni ni-arrow-left"></em></a></li>
                                                                    <li>
                                                                        <div class="dropdown"><a href="#" class="btn btn-trigger btn-icon dropdown-toggle" data-toggle="dropdown">
                                                                            <div class="dot dot-primary"></div>
                                                                            <em class="icon ni ni-filter-alt"></em></a>
                                                                            <div class="filter-wg dropdown-menu dropdown-menu-xl dropdown-menu-right">
                                                                                <div class="dropdown-head"><span class="sub-title dropdown-title">Filter Users</span><div class="dropdown"><a href="#" class="btn btn-sm btn-icon"><em class="icon ni ni-more-h"></em></a></div>
                                                                                </div>
                                                                                <div class="dropdown-body dropdown-body-rg">
                                                                                    <div class="row gx-6 gy-3">
                                                                                        <div class="col-6">
                                                                                            <div class="custom-control custom-control-sm custom-checkbox">
                                                                                                <input type="checkbox" class="custom-control-input" id="hasBalance"><label class="custom-control-label" for="hasBalance"> Have Balance</label></div>
                                                                                        </div>
                                                                                        <div class="col-6">
                                                                                            <div class="custom-control custom-control-sm custom-checkbox">
                                                                                                <input type="checkbox" class="custom-control-input" id="hasKYC"><label class="custom-control-label" for="hasKYC"> KYC Verified</label></div>
                                                                                        </div>
                                                                                        <div class="col-6">
                                                                                            <div class="form-group">
                                                                                                <label class="overline-title overline-title-alt">Role</label><select class="form-select form-select-sm select2-hidden-accessible" data-select2-id="4" tabindex="-1" aria-hidden="true"><option value="any" data-select2-id="6">Any Role</option>
                                                                                                    <option value="investor">Investor</option>
                                                                                                    <option value="seller">Seller</option>
                                                                                                    <option value="buyer">Buyer</option>
                                                                                                </select><span class="select2 select2-container select2-container--default" dir="ltr" data-select2-id="5" style="width: auto;"><span class="selection"><span class="select2-selection select2-selection--single" role="combobox" aria-haspopup="true" aria-expanded="false" tabindex="0" aria-disabled="false" aria-labelledby="select2-y6yo-container"><span class="select2-selection__rendered" id="select2-y6yo-container" role="textbox" aria-readonly="true" title="Any Role">Any Role</span><span class="select2-selection__arrow" role="presentation"><b role="presentation"></b></span></span></span><span class="dropdown-wrapper" aria-hidden="true"></span></span></div>
                                                                                        </div>
                                                                                        <div class="col-6">
                                                                                            <div class="form-group">
                                                                                                <label class="overline-title overline-title-alt">Status</label><select class="form-select form-select-sm select2-hidden-accessible" data-select2-id="7" tabindex="-1" aria-hidden="true"><option value="any" data-select2-id="9">Any Status</option>
                                                                                                    <option value="active">Active</option>
                                                                                                    <option value="pending">Pending</option>
                                                                                                    <option value="suspend">Suspend</option>
                                                                                                    <option value="deleted">Deleted</option>
                                                                                                </select><span class="select2 select2-container select2-container--default" dir="ltr" data-select2-id="8" style="width: auto;"><span class="selection"><span class="select2-selection select2-selection--single" role="combobox" aria-haspopup="true" aria-expanded="false" tabindex="0" aria-disabled="false" aria-labelledby="select2-g6ul-container"><span class="select2-selection__rendered" id="select2-g6ul-container" role="textbox" aria-readonly="true" title="Any Status">Any Status</span><span class="select2-selection__arrow" role="presentation"><b role="presentation"></b></span></span></span><span class="dropdown-wrapper" aria-hidden="true"></span></span></div>
                                                                                        </div>
                                                                                        <div class="col-12">
                                                                                            <div class="form-group">
                                                                                                <button type="button" class="btn btn-secondary">Filter</button></div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="dropdown-foot between"><a class="clickable" href="#">Reset Filter</a><a href="#">Save Filter</a></div>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                    <li>
                                                                        <div class="dropdown"><a href="#" class="btn btn-trigger btn-icon dropdown-toggle" data-toggle="dropdown"><em class="icon ni ni-setting"></em></a>
                                                                            <div class="dropdown-menu dropdown-menu-xs dropdown-menu-right">
                                                                                <ul class="link-check">
                                                                                    <li><span>Show</span></li>
                                                                                    <li class="active"><a href="#">10</a></li>
                                                                                    <li><a href="#">20</a></li>
                                                                                    <li><a href="#">50</a></li>
                                                                                </ul>
                                                                                <ul class="link-check">
                                                                                    <li><span>Order</span></li>
                                                                                    <li class="active"><a href="#">DESC</a></li>
                                                                                    <li><a href="#">ASC</a></li>
                                                                                </ul>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </li>
                                                </ul>
                                            </div>--%>
                                        </div>
                                      <%--  <div class="card-search search-wrap" data-search="search">
                                            <div class="card-body">
                                                <div class="search-content"><a href="#" class="search-back btn btn-icon toggle-search" data-target="search"><em class="icon ni ni-arrow-left"></em></a>
                                                    <input type="text" class="form-control border-transparent form-focus-none" placeholder="Search by user or email">
                                                    <button class="search-submit btn btn-icon"><em class="icon ni ni-search"></em></button>
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                    <asp:UpdatePanel runat="server">
                                             <ContentTemplate>

                                    <div class="card-inner p-0">
                                        <div class="nk-tb-list nk-tb-ulist is-compact" runat="server" id="rpScrollClass">
                                         
                                            <asp:Repeater ID="rpscroll" runat="server" >
                                                <HeaderTemplate>
                                                       <div class="nk-tb-item nk-tb-head">
                                                
                                                <div class="nk-tb-col"><span class="sub-text">T.ID</span></div>
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Name</span></div>
                                                
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Account</span></div>
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Type</span></div>
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Amount</span></div>
                                              
                                                
                                            </div>

                                                </HeaderTemplate>

                                            <ItemTemplate>
                                            <div class="nk-tb-item">
                                                <div class="nk-tb-col nk-tb-check" >
                                                  
                                       
                                                <span class="sub-text"><asp:Label   ID="lblid" for="uid" runat="server" Text='<%# Eval("id") %>' ></asp:Label></span>
                                                    </div>
                                                <div class="nk-tb-col tb-col-lg">
                                                    
                                                        
                                                        <div class="user-name"><span class="tb-lead">
                                                            <asp:Label ID="lbllname" runat="server" Text='<%# Eval("name") %>' ></asp:Label>
                                                                               </span>
                                                            
                                                        </div>
                                                    <span><asp:Label ID="lbllacno" runat="server" Text='<%# Eval("acno") %>' ></asp:Label></span>
                                                    
                                                </div>
                                                <div class="nk-tb-col tb-col-lg"><span>
                                                    <asp:Label ID="lbllach" runat="server" Text='<%# Eval("ached") %>' ></asp:Label>
                                                                                 </span></div>
                                                
                                                <div class="nk-tb-col tb-col-md"><span>
                                                    <asp:Label ID="lblltyp" runat="server" Text='<%# Eval("typ") %>' ></asp:Label>
                                                                                 </span></div>
                                                <div class="nk-tb-col tb-col-lg"><span>
                                                    <asp:Label ID="lblamt" runat="server" Text='<%# Eval("amount") %>' style="float:right;text-align:right" ></asp:Label>
                                                                                 </span></div>
                                              <%--  <div class="nk-tb-col tb-col-lg">
                                                    <span class="tb-lead">
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("amountc") %>' style="float:right;text-align:right"></asp:Label>
                                                    </span>
                                                </div>
                                                <div class="nk-tb-col tb-col-lg">
                                                    <div class="user-name tb-amount " style="text-align:right  ">
                                                    <span class="sub-text" ><asp:Label ID="Label4" Font-Size="Smaller"   runat="server" Text='<%# Eval("sesusr") %>'  ></asp:Label></span>
                                                        </div>

                                                    <span class="sub-text" style="text-align:right;float:right"><asp:Label ID="Label5" Font-Size="Smaller"  runat="server" Text='<%# Eval("entrat") %>' ></asp:Label></span>
                                                        
                                                </div>
                                              --%>  
                                             
                                                    
                                                <div class="nk-tb-col nk-tb-col-tools">
                                                    <ul class="nk-tb-actions gx-2">
                                                      
                                                       <%-- <li class="nk-tb-action-hidden"><a href="#" class="btn btn-sm btn-icon btn-trigger" data-toggle="tooltip" data-placement="top" title="" data-original-title="Wallet"><em class="icon ni ni-wallet-fill"></em></a></li>
                                                        <li class="nk-tb-action-hidden"><a href="#" class="btn btn-sm btn-icon btn-trigger" data-toggle="tooltip" data-placement="top" title="" data-original-title="Send Email"><em class="icon ni ni-mail-fill"></em></a></li>
                                                        <li class="nk-tb-action-hidden"><a href="#" class="btn btn-sm btn-icon btn-trigger" data-toggle="tooltip" data-placement="top" title="" data-original-title="Suspend"><em class="icon ni ni-user-cross-fill"></em></a></li>
                                                      --%>  <li >
                                                                 <asp:LinkButton class="btn btn-sm btn-icon btn-trigger dropdown-toggle" runat="server"  CommandName="ViewClick" CommandArgument='<%# Eval("id") %>' ><em class="icon ni ni-chevron-right"></em></asp:LinkButton>

                                                            <%--<div class="drodown"><a href="#" class="btn btn-sm btn-icon btn-trigger dropdown-toggle" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                                <div class="dropdown-menu dropdown-menu-right">
                                                                    <ul class="link-list-opt no-bdr">
                                                                        
                                                                        <li><asp:LinkButton runat="server"  CommandName="ViewClick" CommandArgument='<%# Eval("id") %>' ><em class="icon ni ni-eye"></em><span>View Details</span></asp:LinkButton> </li>
                                                                        <li><asp:LinkButton runat="server"  CommandName="ApproveClick" CommandArgument='<%# Eval("id") %>'><em class="icon ni ni-check-fill-c"></em><span>Approve</span></asp:LinkButton></li>
                                                                        <li><asp:LinkButton runat="server"  CommandName="RejectClick" CommandArgument='<%# Eval("id") %>'><em class="icon ni ni-cross-fill-c"></em><span>Reject</span></asp:LinkButton> </li>
                                                                        <%--<li><a href="#"><em class="icon ni ni-na"></em><span>Suspend User</span></a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>--%>
                                                        </li>
                                                    </ul>
                                                </div>
                                             
                                            </div>
                                         </ItemTemplate>
                                                </asp:Repeater>

                                                 

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
                    </asp:Panel>

                <asp:Panel runat="server" ID="pnldenom" style="display:none ">
                
                             <div class="card card-body is-dark " id="demomtab">
                                 

                                 
                                                 
                            


                		<div class="row" >


					<div class="col-md-6 grid-margin stretch-card">
            <div class="card is-dark">
              <div class="card-body ">
								<h6 class="card-title text-primary ">Voucher Info</h6>
								
									
                  <asp:UpdatePanel runat="server" >
                      <ContentTemplate>

                      
                       <div class="div-spread ">

                                    <asp:Panel ID="pnlvdetails" runat="server">

                                        <div class="form-group row ">

                                            <label class="col-sm-4 col-form-label text-danger-muted  ">Date</label>
                                                    <asp:Label ID="tdate" CssClass="col-sm-8 col-form-label " runat="server"></asp:Label>
                                        </div>
                                        <div class="form-group row ">

                                            <label class="col-sm-4 col-form-label text-danger-muted ">Account No</label>
                                                    <asp:Label ID="lblacn" CssClass="col-form-label col-sm-8  " runat="server"></asp:Label>
                                        </div>
                                        <div class="form-group row ">

                                            <label class="col-sm-4 col-form-label text-danger-muted ">Account</label>
                                                     <asp:Label ID="lblproduct" CssClass="col-sm-8 col-form-label "  runat="server"></asp:Label>
                                        </div>
                                        <div class="form-group row ">

                                            <label class="col-sm-4 col-form-label text-danger-muted ">Customer Name</label>
                                                      <asp:Label ID="lblname" CssClass="col-sm-8 col-form-label "  runat="server"></asp:Label>
                                        </div>
                                        <div class="form-group row ">

                                            <label class="col-sm-4 col-form-label text-danger-muted ">Amount</label>
                                                 <asp:Label ID="txtamt" CssClass="col-sm-4 col-form-label "  runat="server"></asp:Label>   
                                        </div>
                                        <div class="form-group row ">

                                            <asp:label class="col-sm-4 col-form-label text-danger-muted " ID="lblsc" runat="server" >Service Charge</asp:label>
                                                     <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>

                                                                <asp:Label ID="txtsrvchr" Style="margin-left: 5px" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                        </div>
                                        <div class="form-group row ">

                                            <label class="col-sm-4 col-form-label text-danger-muted ">Transaction</label>
                                             <asp:Label ID="lbltrans" CssClass="col-sm-4 col-form-label "  runat="server"></asp:Label>
                                                                <asp:Label ID="lblnature" CssClass="col-form-label" runat="server" ForeColor="Red" ></asp:Label>
                                                    
                                        </div>
                                         <div class="form-group row ">

                                             <label class="col-sm-4 col-form-label text-danger-muted ">Due Info</label>
                                             <asp:Label ID="lbldue" CssClass="col-form-label col-sm-8" runat="server"></asp:Label>
                                         </div>





                                            <asp:Panel ID="shrd" runat="server" Visible="False">
                                                <table style="margin-left: 0px; width: 25%;" class="customers ">
                                                    <tbody>
                                                        <tr style="text-align: center;">
                                                            <th style="text-align: center; width: 35%; font-variant: small-caps; font-size: small">Voucher No</th>
                                                            <th style="text-align: center; width: 65%; font-variant: small-caps; font-size: small">Amount</th>
                                                            <th></th>

                                                        </tr>
                                                    </tbody>
                                                </table>

                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>

                                                       <asp:GridView runat="server" ID="disp" AutoGenerateColumns="false" Style="margin-top: -2px; margin-left: 0px;"
                                                            AllowPaging="true" OnPageIndexChanging="OnPaging" EmptyDataText="No Records"
                                                            Width="20%" Visible="true" ShowHeaderWhenEmpty="true" ShowHeader="false" CssClass="customers alt ">
                                                            <Columns>


                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblitm" runat="server" Text='<%#Eval("vno")%>' Font-Size="Small" Style="text-align: left"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="20%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblnet" runat="server" Text='<%#Eval("amt")%>' Font-Size="Small" Style="text-align: right; float: right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="25%" />
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <table style="margin-left: 3px; width: 25%;" class="customers">
                                                    <tbody>
                                                        <tr style="text-align: center;">
                                                            <th style="text-align: center; width: 30%; font-variant: small-caps; font-size: small">Total</th>
                                                            <th style="text-align: center; width: 70%; font-variant: small-caps; font-size: medium">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:Label ID="lblsharedtotal" runat="server" Style="float: right"></asp:Label>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>

                                                            </th>

                                                        </tr>
                                                    </tbody>
                                                </table>

                                            </asp:Panel>
                                    </asp:Panel>
                                </div>

                          </ContentTemplate>
                  </asp:UpdatePanel>

                  </div>
                </div>
                        </div>
                            <div class="col-md-6 grid-margin stretch-card  ">
                                  <div class="card is-dark  ">
              <div class="card-body ">
								<h6 class="card-title text-primary ">Denomination</h6>
                     <asp:Panel ID="denom_in" runat="server" Visible="true" >
                         <div>
                             <asp:Panel runat="server" ID="cashDenomInput" Visible="true">
                                           <div class="form-group row ">
              <label class="col-sm-3 col-form-label  text-primary ">Cash Remitted By</label>
              <div class="col-sm-4">
                  <asp:TextBox CssClass="form-control " Width="200px" Style="float: left; margin-left: 10px" ID="remit" runat="server"></asp:TextBox>
                  <asp:Label CssClass="col-form-label col-sm-2  " Style="float: left; margin-top: 5px; margin-left: 10px" ID="amt_nt" runat="server" ForeColor="Red" Font-Size="Medium" Visible="False"></asp:Label>
              </div>
          </div>

              

          <div class="div-txt">
              <div class="div-left" style="margin-left:30px">
                     <asp:UpdatePanel ID="up1k" runat="server">
             <ContentTemplate>

                 <span style="float: left; margin-top: 5px">2000 X </span>
                 <asp:TextBox runat="server" ID="txt1k" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                 <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                 <asp:Label ID="lbl1k" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

             </ContentTemplate>
         </asp:UpdatePanel>
           
              </div>
          </div>
          <br />
 <div class="div-txt">
     <div class="div-left" style="margin-left: 30px">
                <asp:UpdatePanel ID="up500" runat="server">
             <ContentTemplate>

                 <span style="margin-top: 5px;float:left;">&nbsp;&nbsp;500 X </span>
                 <asp:TextBox runat="server" ID="txt500" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px;float:left" Width="50px"></asp:TextBox>
                 <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                 <asp:Label ID="lbl500" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

             </ContentTemplate>
         </asp:UpdatePanel>
         
     </div>
 </div>


<br />

          <div class="div-txt">

     <div class="div-left" style="margin-left: 30px">

         <asp:UpdatePanel ID="UpdatePanel10" runat="server">
             <ContentTemplate>
                 <span style="float: left; margin-top: 5px">&nbsp;&nbsp;200 X </span>
                 <asp:TextBox runat="server" ID="txt200" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                 <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                 <asp:Label ID="lbl200" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

             </ContentTemplate>
         </asp:UpdatePanel>
     </div>
 </div>
 <br />






 <div class="div-txt">

     <div class="div-left" style="margin-left: 30px">
         <asp:UpdatePanel ID="up100" runat="server">
             <ContentTemplate>
                 <span style="float: left; margin-top: 5px">&nbsp;&nbsp;&nbsp;100 X </span>
                 <asp:TextBox runat="server" ID="txt100" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                 <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                 <asp:Label ID="lbl100" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

             </ContentTemplate>
         </asp:UpdatePanel>
     </div>
 </div>
 <br />
 <div class="div-txt">

     <div class="div-left" style="margin-left: 30px">
         <asp:UpdatePanel ID="up50" runat="server">
             <ContentTemplate>

                 <span style="float: left; margin-top: 5px">&nbsp;&nbsp;&nbsp;&nbsp;50 X </span>
                 <asp:TextBox runat="server" ID="txt50" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                 <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                 <asp:Label ID="lbl50" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

             </ContentTemplate>
         </asp:UpdatePanel>

     </div>
 </div>
 <br />
 <div class="div-txt">

     <div class="div-left" style="margin-left: 30px">
         <asp:UpdatePanel ID="up20" runat="server">
             <ContentTemplate>

                 <span style="float: left; margin-top: 5px">&nbsp;&nbsp;&nbsp;&nbsp;20 X </span>
                 <asp:TextBox runat="server" ID="txt20" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                 <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                 <asp:Label ID="lbl20" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

             </ContentTemplate>
         </asp:UpdatePanel>
     </div>
 </div>
 <br />
 <div class="div-txt">

     <div class="div-left" style="margin-left: 30px">
         <asp:UpdatePanel ID="up10" runat="server">
             <ContentTemplate>
                 <span style="float: left; margin-top: 5px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;10 X </span>
                 <asp:TextBox runat="server" ID="txt10" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                 <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                 <asp:Label ID="lbl10" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

             </ContentTemplate>
         </asp:UpdatePanel>
     </div>
 </div>
 <br />
 <div class="div-txt">

     <div class="div-left" style="margin-left: 30px">
         <asp:UpdatePanel ID="upcoin" runat="server">
             <ContentTemplate>
                 <span style="float: left; margin-top: 5px">Coins</span>
                 <asp:TextBox runat="server" ID="txtcoin" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                 <span style="margin-top: 5px; margin-left: 0px; float: left">=</span>
                 <asp:Label ID="lblcoin" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 11px; text-align: right" Text="0.00"></asp:Label>

             </ContentTemplate>
         </asp:UpdatePanel>

     </div>
 </div>
 <br />
 <div class="border-sm-top">
 </div>
 <div class="div-txt">
     <div class="div-left" style="margin-left: 30px">
         <asp:UpdatePanel ID="uptotal" runat="server">
             <ContentTemplate>
                 <span style="margin-left: 15px"><strong>Total</strong></span>
                 <asp:Label ID="lbltotal" runat="server" Text="0.00" Width="100px" Style="margin-top: 5px; margin-left: 79px; text-align: right" ForeColor="#0599ce"></asp:Label>
             </ContentTemplate>
         </asp:UpdatePanel>

     </div>
 </div>
                             </asp:Panel>
                         </div>
                         
                <div class="form-group row border-sm-bottom "></div>
                
                <div class="form-group row">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-8">
                        <div class="btn-group  " role="group" >
                        <asp:Button runat="server" ID="btn_can" CssClass="btn btn-outline-secondary  " Text="Back" CausesValidation="False" />
                        <asp:Button runat="server" ID="btn_denom_clr" CssClass="btn btn-outline-secondary  " Text="Clear" CausesValidation="False" />
                        <asp:Button runat="server" ID="btn_denom_update" CssClass="btn btn-outline-primary  " Enabled="true" Text="Update" CausesValidation="False" />
                            <asp:Button runat="server" ID="btn_prnt" Visible="false"  CssClass="btn btn-outline-primary  " Text="Print" CausesValidation="False" />
                        <asp:Button runat="server" ID="btn_join" CssClass="btn btn-outline-primary  " Text="Shared" CausesValidation="False" />
                            </div>

                    </div>
                    
                </div>

            </asp:Panel>

            <asp:Panel ID="denom_out" runat="server"  Visible="false">
                
                <div class="form-group row ">
                    <div class="col-md-8">
                        <span class="caption">Enter the Denomination for Balance  </span>
                    </div>
                </div>

                <div class="div-left" style="margin-top: 10px; ">
                    <span style="float: left; margin-top: 5px">To be Given :</span>&nbsp;&nbsp;<asp:Label Width="200px" Style="float: left; margin-left: 10px; margin-top: 5px" ID="lbldenombal" runat="server"></asp:Label>
                    <asp:Label  ID="bal_nt" runat="server" ForeColor="Red" Font-Size="Medium" Visible="False"></asp:Label>
                </div>
                <br />

                <div class="div-txt">
                    <div class="div-left" style="margin-left: 30px">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <span style="float: left; margin-top: 5px">2000 X </span>
                                <asp:TextBox runat="server" ID="txtb1k" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                                <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                                <asp:Label ID="lblb1k" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="div-txt">

                    <div class="div-left" style="margin-left: 30px;margin-top:15px">

                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <span style="float: left; margin-top: 5px">&nbsp;&nbsp;500 X </span>
                                <asp:TextBox runat="server" ID="txtb500" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                                <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                                <asp:Label ID="lblb500" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <br />





                 <div class="div-txt">

                    <div class="div-left" style="margin-left: 30px">

                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <span style="float: left; margin-top: 5px">&nbsp;&nbsp;200 X </span>
                                <asp:TextBox runat="server" ID="txtb200" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                                <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                                <asp:Label ID="lblb200" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <br />







                <div class="div-txt">
                    <div class="div-left" style="margin-left: 30px">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <span style="float: left; margin-top: 5px">&nbsp;&nbsp;100 X </span>
                                <asp:TextBox runat="server" ID="txtb100" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                                <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                                <asp:Label ID="lblb100" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <br />
                <div class="div-txt">

                    <div class="div-left" style="margin-left: 30px">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>

                                <span style="float: left; margin-top: 5px">&nbsp;&nbsp;&nbsp;&nbsp;50 X </span>
                                <asp:TextBox runat="server" ID="txtb50" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                                <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                                <asp:Label ID="lblb50" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
                <br />
                <div class="div-txt">
                    <div class="div-left" style="margin-left: 30px">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>

                                <span style="float: left; margin-top: 5px">&nbsp;&nbsp;&nbsp;&nbsp;20 X </span>
                                <asp:TextBox runat="server" ID="txtb20" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                                <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                                <asp:Label ID="lblb20" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <br />
                <div class="div-txt">
                    <div class="div-left" style="margin-left: 30px">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <span style="float: left; margin-top: 5px">&nbsp;&nbsp;&nbsp;&nbsp;10 X </span>
                                <asp:TextBox runat="server" ID="txtb10" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                                <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                                <asp:Label ID="lblb10" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <br />
                <div class="div-txt">

                    <div class="div-left" style="margin-left: 30px">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <span style="float: left; margin-top: 5px">&nbsp;&nbsp;Coins</span>
                                <asp:TextBox runat="server" ID="txtbcoin" CssClass="form-control" AutoPostBack="true" Style="margin-left: 10px; float: left" Width="50px"></asp:TextBox>
                                <span style="margin-top: 5px; margin-left: 5px; float: left">=</span>
                                <asp:Label ID="lblbcoin" Width="100px" runat="server" Style="margin-top: 5px; margin-left: 10px; text-align: right" Text="0.00"></asp:Label>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>

                <div class="sub-titles"></div>
                <br />

                <div class="div-txt">
                    <div class="div-left" style="margin-left: 30px">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <span style="margin-left: 15px"><strong>Total</strong></span>
                                <asp:Label ID="lblbtotal" runat="server" Text="0.00" Width="100px" Style="margin-top: 5px; margin-left: 80px; text-align: right" ForeColor="#0599ce"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>

                <div class="sub-titles"></div>
                <br />
                <div class="div-left">
                    <div style="margin-left: 50px">
                        <asp:Button runat="server" ID="btn_bal_clr" CssClass="btn btn-outline-secondary  " Text="Clear" CausesValidation="False" />
                        <asp:Button runat="server" ID="btn_bal_update" CssClass="btn btn-outline-primary  " Text="Update" CausesValidation="False" />

                    </div>
                    <br />
                </div>
            </asp:Panel>




			</div>
                                      </div>

                                </div>
                            </div>
                </div>
                    </asp:Panel>

                <asp:Panel runat="server" ID="pnlprnt"  style="display:none" >
                    
                          <asp:HiddenField runat="server" ID="damt" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="dword" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="camt" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="cword" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="trans" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="tglh" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="srvchr" ClientIDMode="Static" />
                           <asp:HiddenField runat="server" ID="srcwrds" ClientIDMode="Static" />


                <div class="card card-body  " id="printtab">

                    <div class="form-group row border-bottom  ">
                        <div class="col-md-3"></div>
                        <div class="col-md-6">
                            <div class="btn-group" role="group" >
                                                        <asp:button id="btntog" runat="server"   Class="btn btn-outline-primary   " Text="Close" />
                                                        <button type="button" id="printOCBtn" runat="server" Class="btn btn-outline-primary" onclick="PrintOC();">Print Office Copy</button> 
                                                        <button type="button" id="printCCBtn" runat="server" Class="btn btn-outline-primary" onclick="PrintCC();" >Print Customer Copy</button> 
                                                        <button type="button" id="printJrnlBtn" runat="server" Class="btn btn-outline-primary" onclick="PrintJC();" >Print Journal</button> 
                                            </div>           
                        </div>
                    </div>

                <div>
                                <asp:UpdatePanel runat="server" id="denVouchPrint">
                <ContentTemplate>

                          
                <div id="vouchprint">
                    <div class="prntarea">
               
               
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
                            <td style="width: 82px; height: 21px;font-size: 15px; font-weight: bold;">No&nbsp; &nbsp; :</td>
                            <td style="width: 90px; height: 21px;font-size: 15px"><asp:Label ID="pvno" runat="server" ></asp:Label></td>
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
                    <td style="width: 263px; height: 21px;font-size: 15px"><b><asp:Label ID="pglh" runat="server" ></asp:Label></b></td>
                </tr>
                
                <tr style="height: 30px;">
                    <td style="font-size: 12px; width: 139px; height: 21px;">Member No</td>
                    <td style="width: 21px; height: 21px;"></td>
                    <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="pcid" runat="server" ></asp:Label></td>
                </tr>
                
                <tr style="height: 30px;border-bottom:1px solid black">
                    <td style="font-size: 12px; width: 139px; height: 21px;">Customer Name</td>
                    <td style="width: 21px; height: 21px;"></td>
                    <td  style="width: 263px; height: 21px;font-size: 15px""><b><asp:Label ID="pcname" runat="server" ></asp:Label></b></td>
                </tr>
                
                <tr style="height: 30px;">
                    <td style="font-size: 12px; width: 139px; height: 21px;">Amount</td>
                    <td style="width: 21px; height: 21px;"></td>
                    <td  style="width: 263px; height: 21px;font-size:15px"><b><asp:Label ID="pamt" runat="server" ></asp:Label></b>
                        
                    </td>
                </tr>

                <tr style="height: 25px;">
                    <td style="font-size: 12px; width: 170px; height: 21px;">Note</td>
                    <td colspan="3" style="font-size: 12px; width: 423px; height: 21px;">
                        <asp:Label runat="server" id="plbldue"></asp:Label>
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
        <table style="width:100%;margin-top :10px;  border: 1px solid; padding-right: 0px; float: right;" id="dtab">
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
        </table>
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
                    <!-- ============================================== -->
                    <!--          SECOND RECEIPT STARTS HERE            -->
                    <!-- ============================================== -->
                    <!-- Second Receipt Removed -->
                </div>
                    
                    
                </ContentTemplate>
            </asp:UpdatePanel>
                    <asp:UpdatePanel runat="server" id="journalVouchPrint">
    <ContentTemplate>
        <div id="jvouchprint" class="prntarea">
            <table border="0" style="border-collapse: collapse; ">
                <tbody>
                    <tr style="border: none;">
                        <td style="width: 10%;"><img src="Images/KBF-LOGO.png" alt="" width="72" height="60" /></td>
                        <td style="width: 53%;">
                            <table border="0" style="border-collapse: collapse; width: 0%; height: 86px;">
                                <tbody>
                                    <tr style="height: 10px;">
                                        <td style="width: 100%;font-size:20px;height:10px;"><b>KARAVILAI NIDHI LIMITED</b></td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td style="width: 100%; height: 10px;font-size:15px; text-align: left; font-size: 11px;"><span>Reg No : 18-37630/97</span><br/>8-12A,Vijayam, Main Road Karavilai,Villukuri P. O</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;font-size:15px;">Branch : <b><asp:Label id="jpbranch" runat="server"></asp:Label></b></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td style="width: 37%;">
                            <table style="float: right; margin-left: 20px; height: 63px" width: "100%">
                                <tbody>
                                    <tr>
                                        <td colspan="2" style="width:172px; height: 21px; text-align:left;background-color:#eee;color:#000;font-weight:bold "><label id="jlblcptr">JOURNAL VOUCHER</label> </td>
                                    </tr>

                                    <tr style="height: 21px;">
                                        <td style="width: 82px; height: 21px;font-size: 15px">No&nbsp; &nbsp; :</td>
                                        <td style="width: 90px; height: 21px;font-size: 15px; font-weight: bold;">
                                            <asp:Label ID="jpvno" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height: 21px;">
                                        <td style="width: 82px; height: 21px;font-size: 12px">Date :</td>
                                        <td style="width: 90px; height: 21px;font-size: 12px">
                                            <asp:Label ID="jpdate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>



            <table style="border-bottom:1px solid;width:100%;font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif">
                <tr>
                    <td style="width:100%;">

                        <table style="width:100%;padding-left:10px;padding-right:20px;border-collapse:collapse">
                            <tbody>
                                <tr style="height:25px;border:1px solid">
                                    <td style="width:50%;text-align:center;border:1px solid">Particulars</td>
                                    <td style="width:25%;text-align:center;border:1px solid">Debit</td>
                                    <td style="width:25%;text-align:center;border:1px solid">Credit</td>
                                </tr>

                                <tr style="height:35px;">
                                    <td style="width:50%;text-align:left;border:1px solid;padding-left:5px">
                                        <asp:Label runat="server" ID="lblglhcr" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width:25%;text-align:right;border:1px solid"></td>
                                    <td style="width:25%;text-align:right;border:1px solid;padding-right:5px">
                                        <asp:Label runat="server" ID="lblcr"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height:35px;">
                                    <td style="width:50%;text-align:left;border:1px solid;padding-left:5px">
                                        <asp:Label runat="server" ID="lblglhdr" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width:25%;text-align:right;border:1px solid;padding-right:5px">
                                        <asp:Label runat="server" ID="lbldr"></asp:Label>
                                    </td>
                                    <td style="width:25%;text-align:right;border:1px solid"></td>
                                </tr>
                                <tr style="height:35px;">
                                    <td style="width:50%;text-align:center;border:1px solid;padding-left:5px">Total</td>
                                    <td style="width:25%;text-align:right;border:1px solid;padding-right:5px">
                                        <asp:Label runat="server" Font-Bold="true" ID="lbltdr"></asp:Label>
                                    </td>
                                    <td style="width:25%;text-align:right;border:1px solid;padding-right:5px">
                                        <asp:Label runat="server" Font-Bold="true" ID="lbltcr"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="border:none;height:15px;">
                                    <td colspan="3" style="border:none;text-align:left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="border:none;">
                                    <td colspan="3" style="border:none;text-align:left">
                                        <asp:Label runat="server" id="jpnar"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="border:none;height:35px">
                                    <td colspan="3" style="border:none;text-align:left;font-size:small">
                                        Amount in Words :&nbsp;
                                        <asp:Label ID="jpaiw" runat="server"></asp:Label>

                                    </td>
                                </tr>
                            </tbody>
                        </table>





                </tr>

            </table>





            <table style="height:15px;margin-top:20px;width:100%">
                <tbody>
                    <tr>
                        <td style="width:50%; text-align: center;font-size: 15px">Incharge / Manager</td>

                        <td style="width:50%; text-align: center;font-size: 15px">Cashier</td>



                    </tr>
                </tbody>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
                </div>
                      
                
                
               </div>

    </asp:Panel>


        </form>

    <script src="js/printThis.js" type="text/javascript"></script>
    <script src="js/daterangepicker.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {





            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            InitAutoComplacn();
            

        });

        function InitializeRequest(sender, args) {
        }
        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete

            InitAutoComplacn();
            
        }

        function InitAutoComplacn() {

            $("#<%=txtdate.ClientID%>").daterangepicker({

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



       
        function PrintOC() {

            

            vtyp = document.getElementById('<%= trans.ClientID%>').value;

            camt = document.getElementById('<%= camt.ClientID%>').value; 
            cword = document.getElementById('<%= cword.ClientID%>').value; 
            tglh = document.getElementById('<%= tglh.ClientID%>').value;
            

            if (vtyp === "PAYMENT") {
             

                $("#<%=lblcpt.ClientID%>").text("PAYMENT");
                $("#<%=lblcptr.ClientID%>").text("OFFICE COPY");
            }

            if (vtyp === "RECEIPT") {

                $("#<%=lblcpt.ClientID%>").text("RECEIPT");
                $("#<%=lblcptr.ClientID%>").text("OFFICE COPY");
            }

            
            $("#<%= pamt.ClientID %>").text(camt);
            $("#<%= paiw.ClientID %>").text(cword); 
            $("#<%=pglh.ClientID %>").text(tglh);
            

            document.getElementById("dtab").style.display = "none"


            $('#vouchprint').printThis({
                importCSS: false,
                importStyle: true,  
                printContainer: true,



            });
        }

        function PrintCC() {

            vtyp = document.getElementById('<%= trans.ClientID%>').value; 

            damt = document.getElementById('<%= damt.ClientID%>').value; 
            dword = document.getElementById('<%= dword.ClientID%>').value;
            srvchg = document.getElementById('<%= srvchr.ClientID%>').value;
            swrd = document.getElementById('<%= srcwrds.ClientID%>').value;
            tglh = document.getElementById('<%= tglh.ClientID%>').value;
            glh = document.getElementById('<%= pglh.ClientID%>').innerText;

            //alert(srvchg);

            if (srvchg == "") {

                if (vtyp === "PAYMENT") {

                    $("#<%=lblcpt.ClientID%>").text("PAYMENT");
                    $("#<%=lblcptr.ClientID%>").text("CUSTOMER COPY");
                }

                if (vtyp === "RECEIPT") {

                    $("#<%=lblcpt.ClientID%>").text("RECEIPT");
                    $("#<%=lblcptr.ClientID%>").text("CUSTOMER COPY");

                }



                $("#<%=pamt.ClientID %>").text(damt);
                $("#<%=paiw.ClientID %>").text(dword);
                $("#<%=pglh.ClientID %>").text(tglh);


                document.getElementById("dtab").style.display = "block"

            } else {

                if (vtyp === "RECEIPT") {

                    $("#<%=lblcpt.ClientID%>").text("PAYMENT");
                      $("#<%=lblcptr.ClientID%>").text("CUSTOMER COPY");
                }

                if (vtyp === "PAYMENT") {

                    $("#<%=lblcpt.ClientID%>").text("RECEIPT");
                    $("#<%=lblcptr.ClientID%>").text("CUSTOMER COPY");

                }


                $("#<%=pamt.ClientID %>").text(srvchg);
                $("#<%=paiw.ClientID %>").text(swrd);
                        
                //  $("#<%=pglh.ClientID %>").text("Service Charges");

                document.getElementById("dtab").style.display = "none"

            }

            $('#vouchprint').printThis({
                importCSS: false,
                importStyle: true ,         // import style tags
                printContainer: true,
                afterPrint: function () {
                    //alert("print Completed")

                    

                    if (srvchg != "" ) {

                     //   $("#<%=pamt.ClientID %>").text(srvchg);
                     //   $("#<%=paiw.ClientID %>").text(swrd);
                        
                      //  $("#<%=pglh.ClientID %>").text("Service Charges");

                     //   document.getElementById("dtab").style.display = "none"


                    }
                }

                });


        }

        function PrintJC() {



            $('#jvouchprint').printThis({
                importCSS: false,
                importStyle: true,




            });
        }


    </script>

            

</asp:Content>
