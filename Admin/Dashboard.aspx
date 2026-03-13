<%@ Page Title="Dash Board" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Dashboard.aspx.vb" Inherits="Fiscus.WebForm1" EnableSessionState="ReadOnly" %>


        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            
<link rel="stylesheet" href="../css/Dashboard.css" />
            <form id="dash" runat="server">
                    <asp:ScriptManager ID="SM3" runat="server" ></asp:ScriptManager>


            
            <div class="nk-content nk-content-fluid">
                <div class="container-xl wide-xl">
                    <div class="nk-content-inner">  
                        <div class="nk-content-body">
                            <div class="nk-block-head nk-block-head-sm">
                                <div class="nk-block-between">
                                    <div class="nk-block-head-content">
                                        
                                        <h3 class="nk-block-title page-title" style="font-variant-caps:petite-caps"><% =Session("sesusr")%></h3>
                                        <div class="nk-block-des text-soft">
                                            <p>Welcome to Fiscus beta Dashboard </p>
                                        </div>
                                            
                                    </div>
                                    <div class="nk-block-head-content">
                                        <div class="toggle-wrap nk-block-tools-toggle"><a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-more-v"></em></a>
                                            <div class="toggle-expand-content" data-content="pageMenu">
                                                <ul class="nk-block-tools g-3">
                                                    <li><a href="#" class="btn btn-white btn-dim btn-outline-primary"><em class="icon ni ni-calendar-booking-fill"></em><span><asp:Label runat="server" ID="dt"></asp:Label></span></a></li>
                                                    <li><a href="Scroll.aspx" class="btn btn-white btn-dim btn-outline-primary"><em class="icon ni ni-check-fill-c"></em><span><asp:Label runat="server" ID="lblvch" Text="0/0"></asp:Label> <asp:Label runat="server" ID="lblScrollTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></span></a> </li>
                                                    <li><a href="../dayBook.aspx" class="btn btn-white btn-dim btn-outline-primary"><em class="icon ni ni-reports"></em><span>Day Book</span></a></li>
                                                    <li><a href="#" class="btn btn-white btn-dim btn-outline-primary" data-toggle="modal" data-target="#knlmodal" data-backdrop="static" data-keyboard="false"><em class="icon ni ni-coin-alt-fill"></em><span>Calculator</span></a></li>
                                                    
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
                                                        <h6 class="subtitle">Cash Balance <asp:Label runat="server" ID="lblOpeningTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
                                                    </div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblcbal"></asp:Label></span><span class="change up text-danger"><em class="icon ni ni-arrow-long-up"></em>1.93%</span></div>
                                                <div class="invest-data">
                                                    <div class="invest-data-amount g-2">
                                                        <div class="invest-data-history">
                                                            <div class="title">Receipt</div>
                                                            <div class="amount"><asp:Label ID="lblcredit" runat="server"></asp:Label></div>
                                                        </div>
                                                        <div class="invest-data-history">
                                                            <div class="title">Payment</div>
                                                            <div class="amount"><asp:Label ID="lbldebit" runat="server"></asp:Label></div>
                                                        </div>
                                                    </div>
                                                    <div class="invest-data-ck">
                                                        <div class="chartjs-size-monitor">
                                                            <div class="chartjs-size-monitor-expand">
                                                                <div class=""></div>
                                                            </div>
                                                            <div class="chartjs-size-monitor-shrink">
                                                                <div class=""></div>
                                                            </div>
                                                        </div>
                                                        <canvas class="iv-data-chart" id="totalDeposit" style="display: block; height: 78px; width: 154px;" width="206" height="122"></canvas>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Today Deposit <asp:Label runat="server" ID="lblDepTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
                                                    </div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lbldeptoday"></asp:Label> </span><span class="change down text-danger"><em class="icon ni ni-arrow-long-down"></em>1.93%</span></div>
                                                <div class="invest-data">
                                                    <div class="invest-data-amount g-2">
                                                        <div class="invest-data-history">
                                                            <div class="title">This Month</div>
                                                            <div class="amount"><asp:Label runat="server" ID="lbldepmonth"></asp:Label></div>
                                                        </div>
                                                        <div class="invest-data-history">
                                                            <div class="title">This Week</div>
                                                            <div class="amount"><asp:Label runat="server" ID="lbldepweek"></asp:Label></div>
                                                        </div>
                                                    </div>
                                                    <div class="invest-data-ck">
                                                        <div class="chartjs-size-monitor">
                                                            <div class="chartjs-size-monitor-expand">
                                                                <div class=""></div>
                                                            </div>
                                                            <div class="chartjs-size-monitor-shrink">
                                                                <div class=""></div>
                                                            </div>
                                                        </div>
                                                        <canvas class="iv-data-chart chartjs-render-monitor" id="totalWithdraw" width="206" height="122" style="display: block; height: 78px; width: 154px;"></canvas>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Today Loans <asp:Label runat="server" ID="lblLoanTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
                                                    </div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblloantoday"></asp:Label> </span><span class="change down text-danger"><em class="icon ni ni-arrow-long-down"></em>1.93%</span></div>
                                                <div class="invest-data">
                                                    <div class="invest-data-amount g-2">
                                                        <div class="invest-data-history">
                                                            <div class="title">This Month</div>
                                                            <div class="amount"><asp:Label runat="server" ID="lblloanmonth"></asp:Label></div>
                                                        </div>
                                                        <div class="invest-data-history">
                                                            <div class="title">This Week</div>
                                                            <div class="amount"><asp:Label runat="server" ID="lblloanweek"></asp:Label></div>
                                                        </div>
                                                    </div>
                                                    <div class="invest-data-ck">
                                                        <div class="chartjs-size-monitor">
                                                            <div class="chartjs-size-monitor-expand">
                                                                <div class=""></div>
                                                            </div>
                                                            <div class="chartjs-size-monitor-shrink">
                                                                <div class=""></div>
                                                            </div>
                                                        </div>
                                                        <canvas class="iv-data-chart chartjs-render-monitor" id="totalBalance" width="206" height="122" style="display: block; height: 78px; width: 154px;"></canvas>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                          <div class="card card-bordered  card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Gold Rate <asp:Label runat="server" ID="lblGoldTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
                                                    </div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblgoldrate"></asp:Label> </span><span class="change down text-danger"><em class="icon ni ni-arrow-long-down"></em>1.93%</span></div>
                                      
                                                </div>
                                              </div>
                                        
                                    </div>
                                    <div class="col-md-4">
                                          <div class="card card-bordered   card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">Petty Cash <asp:Label runat="server" ID="lblPettyTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
                                                    </div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblpettycash"></asp:Label> </span></div>
                                      
                                                </div>
                                              </div>
                                        
                                    </div>

                                    <%-- SMS Balance --%>
                                    <div class="col-md-4">
                                          <div class="card card-bordered  card-full">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-0">
                                                    <div class="card-title">
                                                        <h6 class="subtitle">SMS Balance</h6>
                                                    </div>
                                                </div>
                                                <div class="card-amount"><span class="amount"><asp:Label runat="server" ID="lblsms"></asp:Label> </span>
                                                     <span class="change up text-soft" style="font-size: 10px; margin-left: 10px;">
                                                        <asp:Label runat="server" ID="lblSmsTime"></asp:Label>
                                                    </span>
                                                </div>
                                      
                                                </div>
                                              </div>
                                        
                                    </div>
                                    



                                    <%-- Deposits --%>
                                    <div class="col-md-6 col-xxl-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner d-flex flex-column h-100">
                                                <div class="card-title-group mb-3">
                                                    <div class="card-title">
                                                        <h6 class="title">Deposits <asp:Label runat="server" ID="lblStatsTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
                                                        <p>all time</p>
                                                    </div>
                                                 
                                                </div>
                                                <div class="progress-list gy-3">
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Daily Deposit</div>
                                                            <div class="progress-amount"><%=ViewState("DS") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar" data-progress="58" style="width:<%=ViewState("DS") %>%;"></div>
                                                        </div>
                                                    </div>
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Fixed Deposit</div>
                                                            <div class="progress-amount"><%=ViewState("FD") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar bg-pink" data-progress="29" style="width:<%=ViewState("FD") %>%;"></div>
                                                        </div>
                                                    </div>
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">KMK Deposit</div>
                                                            <div class="progress-amount"><%=ViewState("KMK") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar bg-orange" data-progress="18.49" style="width: <%=ViewState("KMK") %>%;"></div>
                                                        </div>
                                                    </div>
                                                  
                                                    
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Recurring Deposit</div>
                                                            <div class="progress-amount"><%=ViewState("RD") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar bg-azure" data-progress="33" style="width:<%=ViewState("RD") %>%"></div>
                                                        </div>
                                                    </div>
                                                      <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Reinvestment Deposit</div>
                                                            <div class="progress-amount"><%=ViewState("RID") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar bg-teal" data-progress="16" style="width:<%=ViewState("RID") %>%"></div>
                                                        </div>
                                                    </div>
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Savings Deposit</div>
                                                            <div class="progress-amount"><%=ViewState("SB") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar bg-dark   " data-progress="16" style="width:<%=ViewState("SB") %>%"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="invest-top-ck mt-auto">
                                                    <div class="chartjs-size-monitor">
                                                        <div class="chartjs-size-monitor-expand">
                                                            <div class=""></div>
                                                        </div>
                                                        <div class="chartjs-size-monitor-shrink">
                                                            <div class=""></div>
                                                        </div>
                                                    </div>
                                                    <canvas class="iv-plan-purchase chartjs-render-monitor" id="planPurchase1" width="795" height="75" style="display: block; height: 50px; width: 530px;"></canvas>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- Loans --%>
                                  
                                      <div class="col-md-6 col-xxl-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner d-flex flex-column h-100">
                                                <div class="card-title-group mb-3">
                                                    <div class="card-title">
                                                        <h6 class="title">Loans</h6>
                                                        <p>all time </p>
                                                    </div>
                                                    
                                                </div>
                                                <div class="progress-list gy-3">
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Daily Collection Loan</div>
                                                            <div class="progress-amount"><%=ViewState("DCL") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar" data-progress="58" style="width:<%=ViewState("DCL") %>%;"></div>
                                                        </div>
                                                    </div>
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Deposit Loan</div>
                                                            <div class="progress-amount"><%=ViewState("DL") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar bg-orange" data-progress="18.49" style="width: <%=ViewState("DL") %>%;"></div>
                                                        </div>
                                                    </div>
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Jewel Loan</div>
                                                            <div class="progress-amount"><%=ViewState("JL") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar bg-teal" data-progress="16" style="width:<%=ViewState("JL") %>%;"></div>
                                                        </div>
                                                    </div>
                                                    <div class="progress-wrap">
                                                        <div class="progress-text">
                                                            <div class="progress-label">Mortgage Plan</div>
                                                            <div class="progress-amount"><%=ViewState("ML") %>%</div>
                                                        </div>
                                                        <div class="progress progress-md">
                                                            <div class="progress-bar bg-pink" data-progress="29" style="width: <%=ViewState("ML") %>%"></div>
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                                <div class="invest-top-ck mt-auto">
                                                    <div class="chartjs-size-monitor">
                                                        <div class="chartjs-size-monitor-expand">
                                                            <div class=""></div>
                                                        </div>
                                                        <div class="chartjs-size-monitor-shrink">
                                                            <div class=""></div>
                                                        </div>
                                                    </div>
                                                    <canvas class="iv-plan-purchase chartjs-render-monitor" id="planPurchase" width="795" height="75" style="display: block; height: 50px; width: 530px;"></canvas>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-xxl-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner d-flex flex-column h-100">
                                                <div class="card-title-group mb-3">
                                                    <div class="card-title">
                                                        <h6 class="title">Maturing this Week <asp:Label runat="server" ID="lblMaturityTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
                                                        <p>FD, RD, RID</p>
                                                    </div>
                                                </div>
                                                <div style="max-height: 280px; overflow-y: auto;">
                                                    <div class="nk-tb-list nk-tb-flush">
                                                        <div class="nk-tb-item nk-tb-head">
                                                            <div class="nk-tb-col"><span>A/c No</span></div>
                                                            <div class="nk-tb-col"><span>Name</span></div>
                                                            <div class="nk-tb-col"><span>Maturity</span></div>
                                                        </div>
                                                        <asp:Repeater ID="rpMaturity" runat="server">
                                                            <ItemTemplate>
                                                                <div class="nk-tb-item" style="padding: 0.5rem 0.25rem;">
                                                                    <div class="nk-tb-col">
                                                                        <span class="tb-sub"><span class="badge badge-dim badge-outline-primary" style="font-size: 10px; padding: 2px 4px;"><%# Eval("product") %></span> <br /><small><%# Eval("acno") %></small></span>
                                                                    </div>
                                                                    <div class="nk-tb-col">
                                                                        <span class="tb-lead" style="font-size: 12px;"><%# Eval("FirstName") %></span>
                                                                    </div>
                                                                    <div class="nk-tb-col">
                                                                        <span class="tb-sub" style="font-size: 11px;"><%# Eval("mdate", "{0:dd MMM}") %></span><br />
                                                                        <span class="tb-amount" style="font-size: 12px;"><%# Eval("mamt", "{0:C}") %></span>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </div>
                                                <div class="mt-auto pt-2">
                                                    <a href="../DepositAnalysis.aspx" class="link link-sm">View Analysis</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- Jewel Overdue --%>
                                    <div class="col-md-6 col-xxl-4">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner d-flex flex-column h-100">
                                                <div class="card-title-group mb-3">
                                                    <div class="card-title">
                                                        <h6 class="title">JL Overdue (10+ Months)</h6>
                                                        <p>crosses 10 months</p>
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel ID="upnlOverdue" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Timer ID="tmrOverdue" runat="server" Interval="500" OnTick="tmrOverdue_Tick" />
                                                        <asp:Label runat="server" ID="lblOverdueError" ForeColor="Red" Font-Size="8pt" Visible="false"></asp:Label>
                                                        <div style="max-height: 280px; overflow-y: auto;">
                                                            <div class="nk-tb-list nk-tb-flush">
                                                                <div class="nk-tb-item nk-tb-head">
                                                                    <div class="nk-tb-col"><span>A/c No <asp:Label runat="server" ID="lblOverdueTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></span></div>
                                                                    <div class="nk-tb-col"><span>Repaid</span></div>
                                                                    <div class="nk-tb-col"><span>Balance</span></div>
                                                                </div>
                                                                <asp:Panel runat="server" ID="pnlOverdueLoading" Visible="true">
                                                                    <div class="p-4 text-center">
                                                                        <div class="spinner-border text-primary" role="status">
                                                                            <span class="sr-only">Loading...</span>
                                                                        </div>
                                                                        <p class="mt-2 text-soft">Calculating 10-Month Overdue...</p>
                                                                        <p class="text-soft small">(This runs in the background)</p>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:Repeater ID="rpJLOverdue" runat="server">
                                                                    <ItemTemplate>
                                                                        <div class="nk-tb-item" style="padding: 0.5rem 0.25rem;">
                                                                            <div class="nk-tb-col">
                                                                                <span class="tb-lead" style="font-size: 12px;"><%# Eval("FirstName") %></span> <br />
                                                                                <span class="tb-sub"><small><%# Eval("acno") %></small></span>
                                                                            </div>
                                                                            <div class="nk-tb-col">
                                                                                <span class="tb-amount" style="font-size: 11px;"><%# Eval("repaid", "{0:C}") %></span>
                                                                            </div>
                                                                            <div class="nk-tb-col">
                                                                                <span class="tb-amount text-danger" style="font-size: 12px;"><%# Eval("balance", "{0:C}") %></span>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                <asp:PlaceHolder runat="server" ID="phNoData" Visible="false">
                                                                    <div class="p-4 text-center text-soft">
                                                                        <em class="icon ni ni-info-fill fs-36px text-light"></em>
                                                                        <p>No overdue accounts found.</p>
                                                                    </div>
                                                                </asp:PlaceHolder>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="mt-auto pt-2">
                                                    <a href="../LoanList.aspx?prod=JL" class="link link-sm">View All JL</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                  
                                <%-- Recent Accounts --%>
                                    <div class="col-xl-12 col-xxl-12">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner border-bottom">
                                                <div class="card-title-group">
                                                    <div class="card-title">
                                                       <h6>Recent Accounts <asp:Label runat="server" ID="lblRecentTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
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

                                    <%-- Staff on Duty --%>
                                     <div class="col-md-12  col-xxl-12">
                                        <div class="card card-bordered  card-full">
                                            <div class="card-inner border-bottom">
                                                <div class="card-title-group">
                                                    <div class="card-title">
                                                        <h6 class="title">Staff on Duty <asp:Label runat="server" ID="lblStaffTime" Font-Size="8pt" ForeColor="Gray"></asp:Label></h6>
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
            </div>



            <%-- SIMPLIFIED DEPOSIT CALCULATOR MODAL (KYC STYLE) --%>
            <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800;900&display=swap" rel="stylesheet">
            <div class="modal fade" id="knlmodal" tabindex="-1" role="dialog">
                <div class="modal-dialog modal-dialog-centered" style="max-width: 480px;">
                    <div class="modal-content" style="border: none; border-radius: 16px; background: #ffffff; box-shadow: 0 10px 40px rgba(0,0,0,0.1); overflow: hidden; font-family: 'Inter', sans-serif;">
                        
                        <!-- Header -->
                        <div style="padding: 24px 32px; border-bottom: 1px solid #E5E7EB; background: white;">
                            <div style="display: flex; justify-content: space-between; align-items: center;">
                                <h5 style="margin: 0; font-weight: 700; color: #374151; font-size: 18px;">Deposit Calculator</h5>
                                <button type="button" class="close" data-dismiss="modal" style="padding: 10px; cursor: pointer; opacity: 0.5;">
                                    <span aria-hidden="true" style="font-size: 24px; color: #6B7280;">&times;</span>
                                </button>
                            </div>
                        </div>

                        <!-- Body -->
                        <div class="modal-body" style="padding: 32px; background: #FBFBFF;">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div style="display: flex; flex-direction: column; gap: 20px;">
                                        
                                        <!-- Amount Input -->
                                        <div>
                                            <label style="display: block; font-size: 14px; font-weight: 600; color: #4B5563; margin-bottom: 8px;">Deposit Amount</label>
                                            <div style="position: relative;">
                                                <span style="position: absolute; left: 14px; top: 50%; transform: translateY(-50%); color: #6B7280; font-weight: 600; font-size: 15px;">&#8377;</span>
                                                <asp:TextBox ID="txamt" runat="server" CssClass="form-control kyc-input" placeholder="0.00" style="padding-left: 32px !important;"></asp:TextBox>
                                            </div>
                                        </div>

                                        <!-- Tenure Input -->
                                        <div>
                                            <label style="display: block; font-size: 14px; font-weight: 600; color: #4B5563; margin-bottom: 8px;">Tenure (Months)</label>
                                            <asp:TextBox ID="txtprd" runat="server" CssClass="form-control kyc-input" placeholder="Enter months"></asp:TextBox>
                                        </div>

                                        <!-- Selection Rows -->
                                        <div style="display: flex; gap: 30px;">
                                            <div>
                                                <label style="display: block; font-size: 14px; font-weight: 600; color: #4B5563; margin-bottom: 8px;">Deposit Type</label>
                                                <asp:RadioButtonList ID="depositType" runat="server" RepeatDirection="Horizontal" CssClass="custom-radio">
                                                    <asp:ListItem Value="FD">FD</asp:ListItem>
                                                    <asp:ListItem Value="RD">RD</asp:ListItem>
                                                    <asp:ListItem Value="RID">RID</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div>
                                                <label style="display: block; font-size: 14px; font-weight: 600; color: #4B5563; margin-bottom: 8px;">Senior Citizen</label>
                                                <asp:RadioButtonList ID="SeniorCitizen" runat="server" RepeatDirection="Horizontal" CssClass="custom-radio">
                                                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                    <asp:ListItem Value="No">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>

                                        <!-- Buttons -->
                                        <div style="display: flex; gap: 12px; margin-top: 10px;">
                                            <asp:Button runat="server" ID="btncalc" Text="Calculate" CssClass="btn-calc-primary" />
                                            <asp:Button runat="server" ID="btnclr" Text="Clear" CssClass="btn-calc-secondary" />
                                        </div>

                                        <!-- Results Section -->
                                        <div style="margin-top: 10px; border-top: 1px solid #E5E7EB; padding-top: 20px;">
                                            <asp:Label ID="lblCalcError" runat="server" CssClass="text-danger" style="display: block; text-align: center; margin-bottom: 15px; font-weight: 500; font-size: 13px;"></asp:Label>
                                            
                                            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 15px;">
                                                <div style="background: white; border: 1px solid #E5E7EB; border-radius: 10px; padding: 15px;">
                                                    <span style="font-size: 11px; color: #6B7280; text-transform: uppercase; font-weight: 700; letter-spacing: 0.5px; display: block; margin-bottom: 4px;">Maturity Value</span>
                                                    <div style="font-size: 18px; font-weight: 700; color: #111827;">
                                                        <asp:Label ID="depositCal" runat="server" Text="0.00"></asp:Label>
                                                    </div>
                                                </div>
                                                <div style="background: white; border: 1px solid #E5E7EB; border-radius: 10px; padding: 15px;">
                                                    <span style="font-size: 11px; color: #6B7280; text-transform: uppercase; font-weight: 700; letter-spacing: 0.5px; display: block; margin-bottom: 4px;">Interest</span>
                                                    <div style="font-size: 18px; font-weight: 700; color: #10B981;">
                                                        <asp:Label ID="InterestLabel" runat="server" Text="0.00"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div style="margin-top: 15px; text-align: center; color: #4B5563; font-weight: 900; font-size: 16px;">
                                                <asp:Label ID="lblMonthlyPayout" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>

                                    </div>

                                    <style>
                                        /* KYC Style Inputs */
                                        .kyc-input {
                                            height: 48px !important;
                                            border-radius: 8px !important;
                                            border: 1px solid #E5E7EB !important;
                                            background: #ffffff !important;
                                            font-weight: 500 !important;
                                            color: #374151 !important;
                                            font-size: 14px !important;
                                            padding: 0 14px !important;
                                            transition: all 0.2s ease !important;
                                            box-shadow: none !important;
                                        }
                                        .kyc-input:focus {
                                            border-color: #7C3AED !important;
                                            box-shadow: 0 0 0 3px rgba(124,58,237,0.15) !important;
                                        }

                                        /* Custom Circular Radio (Green) - KYC Style */
                                        .custom-radio table { border-collapse: separate; border-spacing: 24px 0; margin-left: -24px; }
                                        .custom-radio input[type="radio"] {
                                            appearance: none;
                                            -webkit-appearance: none;
                                            width: 18px;
                                            height: 18px;
                                            border: 2px solid #D1D5DB;
                                            border-radius: 50% !important;
                                            background: #fff;
                                            cursor: pointer;
                                            vertical-align: middle;
                                            position: relative;
                                            transition: all 0.2s ease;
                                            outline: none;
                                            margin-bottom: 2px;
                                        }
                                        .custom-radio input[type="radio"]:checked {
                                            background-color: #10B981 !important;
                                            border-color: #10B981 !important;
                                        }
                                        .custom-radio input[type="radio"]:checked::after {
                                            content: '';
                                            position: absolute;
                                            top: 50%;
                                            left: 50%;
                                            transform: translate(-50%, -50%);
                                            width: 8px;
                                            height: 8px;
                                            background: #fff;
                                            border-radius: 50%;
                                        }
                                        .custom-radio label {
                                            cursor: pointer;
                                            padding-left: 6px;
                                            font-size: 14px;
                                            font-weight: 500;
                                            color: #4B5563;
                                            margin: 0 !important;
                                        }

                                        /* Buttons */
                                        .btn-calc-primary {
                                            flex: 1; height: 46px; border-radius: 10px; border: none; 
                                            background: linear-gradient(135deg, #7C3AED, #2563EB); 
                                            color: white; font-weight: 600; font-size: 14px;
                                            cursor: pointer; transition: all 0.2s;
                                            box-shadow: 0 4px 6px rgba(37, 99, 235, 0.15);
                                        }
                                        .btn-calc-primary:hover { transform: translateY(-1px); box-shadow: 0 6px 12px rgba(37, 99, 235, 0.2); }
                                        
                                        .btn-calc-secondary {
                                            height: 46px; border-radius: 10px; border: 1px solid #D1D5DB; 
                                            background: #ffffff; color: #4B5563; font-weight: 600; font-size: 14px;
                                            padding: 0 24px; cursor: pointer; transition: all 0.2s;
                                        }
                                        .btn-calc-secondary:hover { background: #F3F4F6; }
                                    </style>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>


                
                

            <div class="nk-footer nk-footer-fluid bg-lighter">
                <div class="container-xl wide-lg">
                    <div class="nk-footer-wrap align-content-between">
                    <div class="nk-footer-copyright"> © 2026 KNLTD.
                    </div>
                        <div class="nk-footer-copyright">
                             Hand-crafted by

                       <span class="text-primary ">  Team Ingenious  </span>
                        </div>

                    </div>

                </div>

            </div>

                </form>
            <script src="../js/chart.js" defer></script>

            <script>
                
                $(document).ready(function () {

                    var ctx = document.getElementById('totalDeposit').getContext('2d');
                    var chart = new Chart(ctx, {
                        // The type of chart we want to create
                        type: 'bar',

                        // The data for our dataset
                        data: {
                            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                            datasets: [{
                               
                                backgroundColor: ['rgb(157,204,255)','rgb(157,204,255)','rgb(157,204,255)','rgb(157,204,255)','rgb(157,204,255)','rgb(157,204,255)','rgb(20,133,255)'],
                               // borderColor: 'rgb(255, 99, 132)',
                                data: [20, 10, 5, 2, 20, 30, 45]
                            }]
                        },

                        // Configuration options go here
                        options: {

                            legend: {
                                display: false
                            },
                             scales: {
                                xAxes: [{
                                    ticks: {
                                        display: false
                                    },
                                    gridLines: {
                                        color: '#fff'
                                    }

                                 }],
                                 yAxes: [{
                                     display: false,
                                     gridLines: {
                                         color: '#fff'
                                     }
                                 }]
                            },


                        }
                    });

                    var ctx1 = document.getElementById('totalWithdraw').getContext('2d');
                    var chart = new Chart(ctx1, {
                        // The type of chart we want to create
                        type: 'line',

                        // The data for our dataset
                        data: {
                            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                            datasets: [{

                                backgroundColor: 'rgb(157,204,255)',//, 'rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(20,133,255)'],
                                // borderColor: 'rgb(255, 99, 132)',
                                data: [20, 10, 5, 2, 20, 30, 45]
                            }]
                        },

                        // Configuration options go here
                        options: {

                            legend: {
                                display: false
                            },
                            scales: {
                                xAxes: [{
                                    ticks: {
                                        display: false
                                    },
                                    gridLines: {
                                        color: '#fff'
                                    }

                                }],
                                yAxes: [{
                                    display: false,
                                    gridLines: {
                                        color: '#fff'
                                    }
                                }]
                            },


                        }
                    });
                    var ctx2 = document.getElementById('totalBalance').getContext('2d');
                    var chart = new Chart(ctx2, {
                        // The type of chart we want to create
                        type: 'bar',

                        // The data for our dataset
                        data: {
                            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                            
                            datasets: [{

                                backgroundColor: ['rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(157,204,255)', 'rgb(20,133,255)'],
                                // borderColor: 'rgb(255, 99, 132)',
                                data: [20, 10, 5, 2, 20, 30, 45],
                                


                            }]
                        },

                        // Configuration options go here
                        options: {

                            legend: {
                                display: false
                            },
                            scales: {
                                xAxes: [{
                                    ticks: {
                                        display: false
                                    },
                                    gridLines: {
                                        color: '#fff'
                                    }

                                }],
                                yAxes: [{
                                    display: false,
                                    gridLines: {
                                        color: '#fff'
                                    }
                                }]
                            },


                        }
                    });
                });


              

            </script>
</asp:Content>

