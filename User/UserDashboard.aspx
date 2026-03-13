<%@ Page Title="Dash Board" Language="vb" AutoEventWireup="false" MasterPageFile="~/User/User.Master" CodeBehind="UserDashboard.aspx.vb" Inherits="Fiscus.UserDashBoard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
      <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            
<link rel="stylesheet" href="../css/Dashboard.css" />
          
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
                                                    <li><a href="#" class="btn btn-white   btn-dim btn-outline-primary"><em class="icon ni ni-calendar-booking-fill"></em><span><asp:Label runat="server" ID="dt"></asp:Label></span></a></li>
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
                                        <div class="card card-bordered is-dark card-full">
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
                                        <div class="card card-bordered is-dark card-full">
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
                                        <div class="card card-bordered is-dark card-full">
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

                                       <div class="col-md-4">
                                          <div class="card card-bordered is-dark card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Gold Rate</h6>
                                                    </div>
                                                    <div class="card-tools"><em class="card-hint icon ni ni-help-fill" data-toggle="tooltip" data-placement="left" title="" data-original-title="Total Balance in Account"></em></div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblgoldrate"></asp:Label> </span><span class="change down text-danger"><em class="icon ni ni-arrow-long-down"></em>1.93%</span></div>
                                      
                                                </div>
                                              </div>
                                        
                                    </div>

                                    <div class="col-md-4">
                                          <div class="card card-bordered is-dark card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">SMS Balance</h6>
                                                    </div>
                                                    <div class="card-tools"><em class="card-hint icon ni ni-help-fill" data-toggle="tooltip" data-placement="left" title="" data-original-title="Total Balance in Account"></em></div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblsms"></asp:Label> </span></div>
                                      
                                                </div>
                                              </div>
                                        
                                    </div>

                                    <div class="col-md-4">
                                         <asp:Panel runat="server" ID="pnlpc" Visible="true" >
                                          <div class="card card-bordered is-dark  card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Petty Cash</h6>
                                                    </div>
                                                    <div class="card-tools"><em class="card-hint icon ni ni-help-fill" data-toggle="tooltip" data-placement="left" title="" data-original-title="Total Balance in Account"></em></div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblpettycash"></asp:Label> </span></div>
                                      
                                                </div>
                                              </div>
                                    </asp:Panel>    
                                    </div>
                                    
                                   
                                    
                                
                                      <div class="col-xl-12 col-xxl-8">
                                        <div class="card card-bordered is-dark card-full">
                                            <div class="card-inner border-bottom">
                                                <div class="card-title-group">
                                                    <div class="card-title">
                                                        <h6 class="title">Recent Accounts</h6>
                                                    </div>
                                                    <div class="card-tools"><a href="../ProTrialBalance.aspx" class="link">View All</a></div>
                                                </div>
                                            </div>
                                            <div class="nk-tb-list">
                                                <div class="nk-tb-item nk-tb-head">
                                                    <div class="nk-tb-col"><span>Product</span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span>Customer Name</span></div>
                                                    <div class="nk-tb-col tb-col-lg"><span>Date</span></div>
                                                    <div class="nk-tb-col"><span>Amount</span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span>&nbsp;</span></div>
                                                    <div class="nk-tb-col"><span>&nbsp;</span></div>
                                                </div>
                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light"><span>DS</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lbldsacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lbldsfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lbldsdt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lbldsamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light"><span>FD</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lblfdacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lblfdfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lblfddt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lblfdamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light "><span>KMK</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lblKMKacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lblKMKfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lblKMKdt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lblKMKamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light"><span>RD</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lblrdacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lblrdfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lblrddt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lblrdamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light"><span>RID</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lblridacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lblridfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lblriddt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lblridamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light"><span>SB</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lblsbacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lblsbfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lblsbdt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lblsbamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light"><span>DCL</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lbldclacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lbldclfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lbldcldt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lbldclamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light"><span>DL</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lbldlacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lbldlfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lbldldt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lbldlamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light "><span>JL</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lbljlacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lbljlfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lbljldt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lbljlamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <div class="align-center">
                                                            <div class="user-avatar user-avatar-sm bg-light"><span>ML</span></div>
                                                            <span class="tb-sub ml-2 user-name "><asp:Label runat="server" ID="lblmlacn"></asp:Label></span></div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-sm">
                                                        <div class="user-card">
                                                            
                                                            <div class="user-name"><span class="tb-lead"><asp:Label runat="server" ID="lblmlfn"></asp:Label></span></div>
                                                        </div>
                                                    </div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="tb-sub"><asp:Label runat="server" ID="lblmldt"></asp:Label></span></div>
                                                    <div class="nk-tb-col"><span class="tb-sub tb-amount"><asp:Label runat="server" ID="lblmlamt"></asp:Label></span></div>
                                                    <div class="nk-tb-col tb-col-sm"><span class="tb-sub text-success"></span></div>
                                                    <div class="nk-tb-col nk-tb-col-action">
                                                        <div class="dropdown"><a href="#" class="text-soft dropdown-toggle btn btn-sm btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-chevron-right"></em></a>
                                                            
                                                        </div>
                                                    </div>
                                                </div>

                                                                                            </div>
                                        </div>
                                    </div>
                                     <div class="col-md-6  col-xxl-8">
                                        <div class="card card-bordered is-dark card-full">
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
                                                    <div class="nk-activity-data">
                                                        <div class="label"><asp:Label ID="lbldat" runat="server"  text='<%#Eval("user")%>' ></asp:Label>
</div>
                                                        <span class="time"> <asp:Label ID="lbldr" runat="server" text='<%#Eval("log")%>' ></asp:Label>
</span></div>
                                                     <div class="nk-activity-data">
                                                        <div class="time"><asp:Label ID="lblsl" runat="server" text='<%#Eval("role")%>' ></asp:Label>
                                                        
                                                        </div>
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

