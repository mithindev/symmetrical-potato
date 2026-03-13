<%@ Page Title="List of Deposits" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="DepositList.aspx.vb" Inherits="Fiscus.DepositList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    	<link rel="stylesheet" href="css/Customer.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


     <form class="forms-sample" runat="server" >
		<asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
                       
				<nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Home</a></li>
						<li class="breadcrumb-item " >Customer Inquiry</li>
                        <li class="breadcrumb-item active" >List of Accounts</li>
					</ol>
				</nav>


    
	
			<div class="container-xl wide-lg ">
                <div class="nk-content-inner">
			<div class="nk-content-body ">
				<div class="nk-block-head ">
				 
                                
                                 
					 <div class="nk-block-head-content">
						 <div class="nk-block-head-sub">

                             <asp:LinkButton runat="server" CssClass="text-soft back-to " ID="btnback">
                             <em class="icon ni ni-arrow-left" style="font-size:1rem" ></em><span>Back</span>
							    </asp:LinkButton>
						 </div>
                         							
                         <div class="nk-block-between-md g-4">
                             <div class="nk-block-head-content">
                                 <span><asp:Label ID="lblcid" runat="server" Text="KBF0000110"></asp:Label></span>
                                 <h4 class="nk-block-title fw-normal text-primary ">
                                     <asp:Label ID="lblfname" runat="server" Text="KBF0000110"></asp:Label>
                                 </h4> 
                                 <div class="nk-block-des"><p>&nbsp;<asp:Label ID="lbllname" runat="server" Text="KBF0000110"></asp:Label></p></div>
                             </div>
                            
                             <div class="nk-block-head-content">
                                 <ul class="nk-block-tools gx-3">
                                   <li><a href="#" class="btn btn-primary"><span><asp:Label ID="lbldep" runat="server" Text="Fixed Deposit"></asp:Label></span> </a></li>
                                      
                                 </ul>
                             </div>
                         </div>
      
                     </div>

					 </div>

                  
                        <div class="nk-block">
                    <div class="card card-bordered">
                        <div class="card-inner-group">

                                  
                            <div class="card-inner">
                                <div class="row gy-gs">
                                    <div class="col-lg-5">
                                        <div class="nk-iv-wg3">
                                            <div class="nk-iv-wg3-title">Total Balance</div>
                                            <div class="nk-iv-wg3-group  flex-lg-nowrap gx-4">
                                                <div class="nk-iv-wg3-sub">
                                                    <div class="nk-iv-wg3-amount">
                                                        <div class="number"><asp:Label ID="lblDepBal" runat="server" Text="0.00"></asp:Label> <small class="currency currency-usd"></small></div>
                                                    </div>
                                                    <div class="nk-iv-wg3-subtitle">Available Balance</div>
                                                </div>
                                                <div class="nk-iv-wg3-sub"><span class="nk-iv-wg3-plus text-soft"><em class="icon ni ni-plus"></em></span>
                                                    <div class="nk-iv-wg3-amount">
                                                        <div class="number-sm"><asp:Label ID="lbldl" CssClass="text-warning " runat="server" Text="0.00"></asp:Label></div>
                                                    </div>
                                                    <div class="nk-iv-wg3-subtitle">Locked Balance <em class="icon ni ni-info-fill" data-toggle="tooltip" data-placement="right" title="" data-original-title="You can't use"></em></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-7">
                                        <div class="nk-iv-wg3">
                                            <div class="nk-iv-wg3-title">This Month <em class="icon ni ni-info-fill" data-toggle="tooltip" data-placement="right" title="" data-original-title="Current Month Profit"></em></div>
                                            <div class="nk-iv-wg3-group flex-md-nowrap g-4">
                                                <div class="nk-iv-wg3-sub-group gx-4">
                                                    <div class="nk-iv-wg3-sub">
                                                        <div class="nk-iv-wg3-amount">
                                                            <div class="number"><asp:Label ID="lblcDep" runat="server" Text="0.00"></asp:Label></div>
                                                        </div>
                                                        <div class="nk-iv-wg3-subtitle">New Deposit</div>
                                                    </div>
                                                <%--    <div class="nk-iv-wg3-sub"><span class="nk-iv-wg3-plus text-soft"><em class="icon ni ni-plus"></em></span>
                                                        <div class="nk-iv-wg3-amount">
                                                            <div class="number-sm">1,50.05</div>
                                                        </div>
                                                        <div class="nk-iv-wg3-subtitle">Today Profit</div>
                                                    </div>--%>
                                                </div>
                                                <div class="nk-iv-wg3-sub flex-grow-1 ml-md-3">
                                                    <div class="nk-iv-wg3-ck">
                                                        <div class="chartjs-size-monitor">
                                                            <div class="chartjs-size-monitor-expand">
                                                                <div class=""></div>
                                                            </div>
                                                            <div class="chartjs-size-monitor-shrink">
                                                                <div class=""></div>
                                                            </div>
                                                        </div>
                                                        <canvas class="chart-profit chartjs-render-monitor" id="profitCM" style="display: block; height: 45px; width: 327px;" width="490" height="67"></canvas>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="card-inner border-top ">
                                <ul class="nk-iv-wg3-nav">
                                    <li><asp:LinkButton runat="server"  ID="btnDS"> <em class="icon ni ni-report-profit "></em><p>DS&nbsp;</p></asp:LinkButton></li>
                                    <li><asp:LinkButton runat="server"  ID="btnFD"> <em class="icon ni ni-report-profit "></em><p>FD&nbsp;</p></asp:LinkButton></li>
                                    <li><asp:LinkButton runat="server"  ID="btnKMK"> <em class="icon ni ni-report-profit "></em><p>KMK</p></asp:LinkButton></li>
                                    <li><asp:LinkButton runat="server"  ID="btnRD"> <em class="icon ni ni-report-profit "></em><p>RD&nbsp;</p></asp:LinkButton></li>
                                    <li><asp:LinkButton runat="server"  ID="btnRID"> <em class="icon ni ni-report-profit "></em><p>RID</p></asp:LinkButton></li>
                                    <li><asp:LinkButton runat="server"  ID="btnSB"> <em class="icon ni ni-report-profit "></em><p>SB&nbsp;</p></asp:LinkButton></li>
                                    
                                </ul>
                            </div>
                      
                        </div>
                    </div>
                </div>


                <div class="nk-block nk-block-lg">
                    <div class="nk-block-head-sm">
                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>

                            
                        <div class="nk-block-head-content">
                            <h5 class="nk-block-title">Active Plan <span class="count text-base">(<asp:Label ID=lblapC runat="server" ></asp:Label>)</span></h5>
                        </div>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="nk-iv-scheme-list">
                        <asp:Repeater runat="server" ID="rpAPlan">
                            <ItemTemplate>
                                 <div class="nk-iv-scheme-item">
                            <div class="nk-iv-scheme-icon is-running"><em class="icon ni ni-update"></em><p><asp:Label ID="Label1" runat="server" Text='<%# Eval("prd") %>' ></asp:Label></p></div>
                            <div class="nk-iv-scheme-info">
                                <div class="nk-iv-scheme-name"><asp:Label ID="lblacn" runat="server" Text='<%# Eval("acno") %>' ></asp:Label></div>
                                <div class="nk-iv-scheme-desc">Amount - <span class="amount"><asp:Label ID="lblamt" runat="server" Text='<%# Eval("amount", "{0:C}") %>' ></asp:Label></span></div>
                            </div>
                            <div class="nk-iv-scheme-term">
                                <div class="nk-iv-scheme-start nk-iv-scheme-order"><span class="nk-iv-scheme-label text-soft">Start Date</span><span class="nk-iv-scheme-value date"><asp:Label ID="lbldate" runat="server" Text='<%# Eval("date", "{0:dd MMM yyyy}") %>' ></asp:Label></span></div>
                                <div class="nk-iv-scheme-end nk-iv-scheme-order"><span class="nk-iv-scheme-label text-soft">End Date</span><span class="nk-iv-scheme-value date"><asp:Label ID="lblmdate" runat="server" Text='<%# Eval("mdate", "{0:dd MMM yyyy}") %>' ></asp:Label></span></div>
                            </div>
                            <div class="nk-iv-scheme-amount">
                                <div class="nk-iv-scheme-amount-a nk-iv-scheme-order"><span class="nk-iv-scheme-label text-soft">Maturity</span><span class="nk-iv-scheme-value amount"><asp:Label ID="lblmamt" runat="server" Text='<%# Eval("mamt", "{0:C}") %>' ></asp:Label></span></div>
                                <div class="nk-iv-scheme-amount-b nk-iv-scheme-order"><span class="nk-iv-scheme-label text-soft">Balance</span><span class="nk-iv-scheme-value amount"><asp:Label ID="lblbal" runat="server" Text='<%# Eval("bal", "{0:C}") %>' ></asp:Label></span></div>
                            </div>
                            <div class="nk-iv-scheme-more">
                                
                                <a class="btn btn-icon btn-lg btn-round btn-trans"      href="SoaDeposit.aspx?acno=<%# Eval("acno") %>"><em class="icon ni ni-forward-ios"></em></a>
                                
                            </div>
                            <div class="nk-iv-scheme-progress">
                                <div class="progress-bar" data-progress="25" style="width:<%# Eval("term") %>%"></div>
                            </div>
                        </div>
                            </ItemTemplate> 
                        </asp:Repeater>

                       
                        
                    </div>
                </div>

                <div class="nk-block nk-block-lg">
                    <div class="nk-block-head-sm">
                        <div class="nk-block-between-md ">
                            <asp:UpdatePanel runat="server" >
                                <ContentTemplate>

                            <div class="nk-block-head-content">

                                <h5 class="nk-block-title">Recently End <span class="count text-base">(<asp:Label ID=lblEpc runat="server" ></asp:Label>)</span></h5>
                            </div>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                           
                        </div>
                    </div>
                    <div class="nk-iv-scheme-list">

                        <asp:Repeater runat="server" ID="rpEPlan" >
                            <ItemTemplate>

                            
                        <div class="nk-iv-scheme-item">
                            <div class="nk-iv-scheme-icon is-done"><em class="icon ni ni-offer"></em></div>
                          <div class="nk-iv-scheme-info">
                                <div class="nk-iv-scheme-name"><asp:Label ID="lblacn" runat="server" Text='<%# Eval("acno") %>' ></asp:Label></div>
                                <div class="nk-iv-scheme-desc">Amount - <span class="amount"><asp:Label ID="lblamt" runat="server" Text='<%# Eval("amount", "{0:C}") %>' ></asp:Label></span></div>
                            </div>
                            <div class="nk-iv-scheme-term">
                                <div class="nk-iv-scheme-start nk-iv-scheme-order"><span class="nk-iv-scheme-label text-soft">Start Date</span><span class="nk-iv-scheme-value date"><asp:Label ID="lbldate" runat="server" Text='<%# Eval("date", "{0:dd MMM yyyy}") %>' ></asp:Label></span></div>
                                <div class="nk-iv-scheme-end nk-iv-scheme-order"><span class="nk-iv-scheme-label text-soft">End Date</span><span class="nk-iv-scheme-value date"><asp:Label ID="lblmdate" runat="server" Text='<%# Eval("mdate", "{0:dd MMM yyyy}") %> '></asp:Label></span></div>
                            </div>
                            <div class="nk-iv-scheme-amount">
                                <div class="nk-iv-scheme-amount-a nk-iv-scheme-order"><span class="nk-iv-scheme-label text-soft">Maturity</span><span class="nk-iv-scheme-value amount"><asp:Label ID="lblmamt" runat="server" Text='<%# Eval("mamt", "{0:C}") %>' ></asp:Label></span></div>
                                <div class="nk-iv-scheme-amount-b nk-iv-scheme-order"><span class="nk-iv-scheme-label text-soft">Balance</span><span class="nk-iv-scheme-value amount"><asp:Label ID="lblbal" runat="server" Text='<%# Eval("bal", "{0:C}") %>' ></asp:Label></span></div>
                            </div>
                            <div class="nk-iv-scheme-more"><a class="btn btn-icon btn-lg btn-round btn-trans"      href="SoaDeposit.aspx?acno=<%# Eval("acno") %>"> <em class="icon ni ni-forward-ios"></em></a></div>
                          
                        </div>
                        
                                </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

                     
				 </div>
                    
                </div>
                </div>
                
            
         </form>


</asp:Content>
