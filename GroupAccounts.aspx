<%@ Page Title="Group Accounts" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="GroupAccounts.aspx.vb" Inherits="Fiscus.GroupAccounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    	<link rel="stylesheet" href="css/Customer.css" />

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <form  runat="server" >
		<asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>
                       
				<nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Customer Inquiry</a></li>
						
                        <li class="breadcrumb-item active" >Group Accounts</li>
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
                                    <div class="nk-block-des">
                                    <asp:Label runat="server" ID="lblcid" ></asp:Label>
                                        </div>
                                    <h4 class="nk-block-title fw-normal"><asp:Label runat="server" ID="lblfname" ></asp:Label></h4>
                                    <div class="nk-block-des">
                                        <asp:Label runat="server" ID="lbllname"></asp:Label>
                                        
                                    </div>
                                    <div class="nk-block-des">
                                        <asp:Label runat="server" ID="lbladd"></asp:Label>
                                        
                                    </div>
                                    <div class="nk-block-des">
                                        <asp:Label runat="server" ID="lblmobile"></asp:Label>
                                        
                                    </div>

                                </div>

                                  <div class="nk-block-head-content">
                                 <ul class="nk-block-tools gx-3">
                                   <li>
                                      <div class="btn-group">
	<button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <asp:Label ID="lbldep" runat="server" Text="Fixed Deposit"></asp:Label>
	</button>
	<div class="dropdown-menu">
				                            <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnDS"><p>Daily Deposit</p></asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnFD"><p>Fixed Deposit</p></asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnKMK"><p>KMK Deposit</p></asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnRD"><p>Recurring Deposit</p></asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnRID"><p>Reinvestment Deposit</p></asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnSB"><p>Savings Deposit</p></asp:LinkButton>
                                    <div class="dropdown-divider"></div>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnDCL"><p>Daily Collection</p></asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnDL"><p>Deposit Loan</p></asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnJL"><p>Jewel Loan</p></asp:LinkButton>
                                    <asp:LinkButton runat="server" CssClass="dropdown-item" ID="btnML"><p>Mortgage Loan</p></asp:LinkButton>

	</div>
</div>
                                       </li>
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
                                            <div class="nk-iv-wg3-title">Total Deposit</div>
                                            <div class="nk-iv-wg3-group  flex-lg-nowrap gx-4">
                                                <div class="nk-iv-wg3-sub">
                                                    <div class="nk-iv-wg3-amount">
                                                        <div class="number"><asp:Label ID="lblDepBal" runat="server" Text="0.00"></asp:Label> <small class="currency currency-usd"></small></div>
                                                    </div>
                                                    <div class="nk-iv-wg3-subtitle">&nbsp;</div>
                                                </div>
                                                <div class="nk-iv-wg3-sub">
                                                    <div class="nk-iv-wg3-amount"><span></span>
                                                        <div class="number"><asp:Label ID="lbldl" CssClass="text-warning " runat="server" Text="0"></asp:Label></div>
                                                    </div>
                                                    <div class="nk-iv-wg3-subtitle">Total Accounts </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-7">
                                        <div class="nk-iv-wg3">
                                            <div class="nk-iv-wg3-title">Total Balance </div>
                                            <div class="nk-iv-wg3-group flex-md-nowrap g-4">
                                                <div class="nk-iv-wg3-sub-group gx-4">
                                                    <div class="nk-iv-wg3-sub">
                                                        <div class="nk-iv-wg3-amount">
                                                            <div class="number"><asp:Label ID="lblcDep" runat="server" Text="0.00"></asp:Label></div>
                                                        </div>
                                                        <div class="nk-iv-wg3-subtitle">&nbsp;&nbsp;</div>
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
                                   
                                    
                                </ul>
                            </div>
                      
                        </div>
                    </div>
                </div>


                      <div class="nk-block nk-block-lg">
                    <div class="nk-block-head-sm">
                      
                            
                        <div class="nk-block-head-content">
                            <h5 class="nk-block-title">List of Accounts </h5>
                        </div>
                             
                        <div class="card card-body ">

                            <asp:Repeater runat="server" ID="rpSgrp" OnItemDataBound="OnItemDataBound">

                                <HeaderTemplate >
                                    <table class="table table-bordered   ">
                                        <thead>
                                        <tr>
                                            <th>&nbsp;</th>
                                            <th style="text-align:center">Customer Id</th>
                                            <th style="text-align:center">Customer Name</th>
                                            <th style="text-align:center">Total Accounts</th>
                                            <th style="text-align:center">Deposit Amount</th>    
                                            <th style="text-align:center">Balance</th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <tr >
                                    <td>                <img alt="" style="cursor: pointer" height="12" width="12" src="images/plus.svg" class="btnmore" />
</td>
                                    <td><asp:label  runat="server" ID="lblgcid" Text='<%# Eval("cid") %>'></asp:label></td>
                                    <td><asp:label runat="server" ID="lblname"  Text='<%# Eval("FirstName") %>'></asp:label></td>
                                    <td><asp:label runat="server" ID="lblnoa"   style="text-align:center"  Text='<%# Eval("noa", "{0:00}") %>'></asp:label></td>
                                    <td><asp:label runat="server" ID="lblsamt" style="float:right"  CssClass="text-right " Text='<%# Eval("samt", "{0:C}") %>'></asp:label></td>
                                    <td><asp:label runat="server" ID="lblsbal" style="float:right"  Text='<%# Eval("bal", "{0:C}") %>'></asp:label> </td>
                                    </tr>
                              
                              <tr style="display:none ">
                                    <td>&nbsp</td>
                                  <td colspan="5" >
                            <div class ="nk-iv-wg3-list">
                            <asp:Repeater runat="server" ID="rpbrkup">
                                <HeaderTemplate>
                                      <table class="table table-bordered table-hover  " >
                                    <tr >
                                        <th style="text-align:center">Date</th>
                                        <th style="text-align:center">Account No</th>
                                        <th style="text-align:center">Amount</th>
                                        <th style="text-align:center">Term</th>
                                        <th style="text-align:center">Maturity Date</th>
                                        <th style="text-align:center">Maturity Amount</th>
                                        <th style="text-align:center">Balance</th>
                                    </tr>
                              
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                    <td>
                                        <asp:Label ID="lbldt" runat="server" Text='<%# Eval("date", "{0:dd MMM yyyy}") %>'></asp:Label>
                                    </td>
                                        <td>
                                          <span> <a  href="javascript:showsoa('<%# Eval("acno") %>');" > <asp:Label ID="lblacno" runat="server" Text='<%# Eval("acno") %>'></asp:Label></a></span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblamt" runat="server" style="float:right" CssClass="text-right " Text='<%# Eval("amount", "{0:C}") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblprd" runat="server" CssClass="text-center  " Text='<%# Eval("term") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblmdt" runat="server" Text='<%# Eval("mdate", "{0:dd MMM yyyy}") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblmamt" runat="server" style="float:right" CssClass="text-left" Text='<%# Eval("mamt", "{0:C}") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbal" runat="server" style="float:right" CssClass="text-left " Text='<%# Eval("bal", "{0:C}") %>'></asp:Label>
                                        </td>
                                    
                                </tr>
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            
                            
                                    
                                
                            
                                </asp:Repeater>

                                </div>
                                 </td>       </tr>
                            </ItemTemplate>

                                <FooterTemplate>
                                    </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>

                          </div>
                        
                    </div>
             </div>
                    

                    </div>
                </div>
              </div>

		  </form>

    <script src="js/jquery.js"  type="text/javascript" ></script>
<script type="text/javascript">
    $("body").on("click", "[src*=plus]", function () {
        $(this).closest("tr").next().show();
        $(this).attr("src", "images/minus.svg");
    });
    $("body").on("click", "[src*=minus]", function () {
        $(this).attr("src", "images/plus.svg");
        $(this).closest("tr").next().hide();
    });

    function showsoa(acno) {
        
        var dep = acno
        

        var depcode = dep.substring(0, 1)
        
        switch (depcode) {
            case "7":
                window.open("../soadeposit.aspx?acno=" + acno, '_blank');
                break;
            case "6":
                window.open("../soaloan.aspx?acno=" + acno, '_blank');
                break;
            
        };

        

    }

</script>
</asp:Content>
