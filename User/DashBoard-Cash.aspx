<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/User/User.Master" CodeBehind="DashBoard-Cash.aspx.vb" Inherits="Fiscus.DashBoard_Cash" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
      <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            
<link rel="stylesheet" href="../css/Dashboard.css" />
          <link rel ="stylesheet" href="../css/stylelist.css" />
            <form id="dbc" runat="server" >
                <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
            
            <div class="nk-content nk-content-fluid">
                <div class="container-xl wide-xl">
                    <div class="nk-content-inner">  
                        <div class="nk-content-body">
                            <div class="nk-block-head nk-block-head-sm">
                                <div class="nk-block-between">
                                    <div class="nk-block-head-content">
                                        <h3 class="nk-block-title page-title"><% =Session("sesusr")%></h3>
                                        <div class="nk-block-des text-soft">
                                            <p>Welcome to Fiscus beta Dashboard </p>
                                        </div>
                                    </div>
                                    <div class="nk-block-head-content">
                                        <div class="toggle-wrap nk-block-tools-toggle"><a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-more-v"></em></a>
                                            <div class="toggle-expand-content" data-content="pageMenu">
                                                <ul class="nk-block-tools g-3">
                                                    <li><a href="#" class="btn btn-white btn-dim btn-outline-primary"><em class="icon ni ni-calendar-booking-fill"></em><span><asp:Label runat="server" ID="dt"></asp:Label></span></a></li>
                                                   <li><a href="#" class="btn btn-white btn-dim btn-outline-primary"><em class="icon ni ni-check-fill-c"></em><span><asp:Label runat="server" ID="lblvch" Text="0/0"></asp:Label></span></a> </li>
                                                    <li><a href="../dayBook.aspx" class="btn btn-white btn-dim btn-outline-primary"><em class="icon ni ni-reports"></em><span>Day Book</span></a></li>
                                                    <li class="nk-block-tools-opt">
                                                        <div class="dropdown"><a href="#" class="dropdown-toggle btn btn-icon btn-primary" data-toggle="dropdown"><em class="icon ni ni-plus"></em></a>
                                                            <div class="dropdown-menu dropdown-menu-right">
                                                                <ul class="link-list-opt no-bdr">
                                                                    <li><a href="../Customer.aspx"><em class="icon ni ni-user-add-fill"></em><span>New Member</span></a></li>
                                                                    <li><a href="../DepositOpening.aspx"><em class="icon ni ni-coin-alt-fill"></em><span>New Deposit</span></a></li>
                                                                    <li><a href="../LoanOpening.aspx"><em class="icon ni ni-note-add-fill-c"></em><span>New Loan</span></a></li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="nk-block">
                                <div class="row g-gs">
                                    <div class="col-md-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Cash Balance</h6>
                                                    </div>
                                                    <div class="card-tools"></div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblcbal"></asp:Label></span></div>
                                                
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Payment</h6>
                                                    </div>
                                                    <div class="card-tools"></div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label ID="lbldebit" runat="server"></asp:Label></span></div>
                                                
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Receipt</h6>
                                                    </div>
                                                    <div class="card-tools"></div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label ID="lblcredit" runat="server"></asp:Label> </span></div>
                                                
                                            </div>
                                        </div>
                                    </div>

                                   
                                 
                                </div>
                            </div>
                            <br />
                            <asp:Panel runat="server" id="pnldenom">
                            <div class="nk-block">
                         

                            <div class="card card-bordered   card-stretch">
                                <div class="card-inner-group">
                                    <div class="card-inner position-relative card-tools-toggle">
                                        <div class="card-title-group">
                                            <div class="card-tools">
                                                <div class="form-inline flex-nowrap gy-2 ">
                                                    
                                                        <span class="tb-lead">Denomination &nbsp;&nbsp;&nbsp;</span>
                                                    
                                    
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
                                        <div class="nk-tb-list nk-tb-ulist is-compact">
                                         
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
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("amount") %>' style="float:right;text-align:right" ></asp:Label>
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
                                </asp:Panel>

                            <div class="nk-block">
                                <div class="col-md-12  col-xxl-12">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner border-bottom">
                                                <div class="card-title-group">
                                                    <div class="card-title">
                                                        <h6 class="title">Staff on Duty</h6>
                                                    </div>
                                              
                                                </div>
                                            </div>
                                            <asp:Repeater runat="server"  ID="rpstaff">
                                                <ItemTemplate>
                                               
                                            <div class="nk-activity">
                                                <div class="nk-activity-item">
                                                    <div class="nk-activity-media user-avatar bg-light  ">
                                                        <div class="label"><asp:Label ID="Label1" runat="server"  text='<%#Eval("intl")%>'></asp:Label></div>
                                                        </div>
                                                    <div class="nk-activity-data ">
                                                        <div class="text-secondary  ">
                                                            <asp:LinkButton runat="server"  CommandName="pokeClick" CommandArgument='<%# Eval("user") %>' >
                                                            <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-log-out"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path><polyline points="16 17 21 12 16 7"></polyline><line x1="21" y1="12" x2="9" y2="12"></line></svg>
                                                                </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="nk-activity-data">
                                                        <div class="label"><asp:Label ID="lbldat" runat="server"  text='<%#Eval("user")%>' ></asp:Label>
                                                    </div>
                                                       </div>
                                                     <div class="nk-activity-data">
                                                        <div class="time"><asp:Label ID="lblsl" runat="server" text='<%#Eval("role")%>' ></asp:Label>
                                                        
                                                        </div>
                                                </div>
                                                    <div class="nk-activity-data">
                                                    <span class="time"> <asp:Label ID="Label2" runat="server" text='<%#Eval("log")%>' ></asp:Label>
                                                    </span></div>

                                                    
                                                    </div>
                                                                                               
                                            </div>
                                                    </ItemTemplate>
                                            </asp:Repeater>
                                                
                                        </div>
                                    </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

                <div class="nk-footer nk-footer-fluid bg-lighter">
                <div class="container-xl wide-lg">
                    <div class="nk-footer-wrap align-content-between">
                    <div class="nk-footer-copyright"> © 2025 KNLTD.
                    </div>
                        <div class="nk-footer-copyright">
                             Hand-crafted by

                       <span class="text-primary ">  Team Ingenious  </span>
                        </div>

                    </div>

                </div>

            </div>
                </form>
            <script>

                $(document).ready(function () {

                    
                
                });


              

            </script>
            
</asp:Content>
