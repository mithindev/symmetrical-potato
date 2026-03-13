<%@ Page Title="Scroll Passing" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Scroll.aspx.vb" Inherits="Fiscus.Scroll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel ="stylesheet" href="../css/dashboard.css" />
    <link rel="stylesheet" href="../css/stylelist.css" />

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form id="payment" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Admin</a></li>
						<li class="breadcrumb-item active" >Scroll</li>
					</ol>
				</nav>

           <asp:UpdatePanel runat="server" >
               <ContentTemplate>

               
            <div class="container-xl wide-xl">
                <div class="nk-content-inner">
                    <div class="nk-content-body">
                        <asp:UpdatePanel runat="server" ID="pnlscrldata" Visible="true" >
                            <ContentTemplate>

                            
                        <div class="nk-block">
                         

                            <div class="card card-bordered is-dark  card-stretch">
                                <div class="card-inner-group">
                                    <div class="card-inner position-relative card-tools-toggle">
                                        <div class="card-title-group">
                                            <div class="card-tools">
                                                <div class="form-inline flex-nowrap gx-3">
                                                    <div class="form-wrap w-150px">
                       
                                                        <div class="form-check form-check-inline">
											<label class="form-check-label">
												<asp:CheckBox  runat="server"  ID="inclpass" AutoPostBack="true"   />
												Passed Vouchers
											<i class="input-frame"></i></label>
										</div>
                                                        <%--<select class="form-select form-select-sm select2-hidden-accessible" data-search="off" data-placeholder="Bulk Action" data-select2-id="1" tabindex="-1" aria-hidden="true">
                                                            <option value="" data-select2-id="3">Bulk Action</option>
                                                            <option value="email">Send Email</option>
                                                            <option value="group">Change Group</option>
                                                            <option value="suspend">Suspend User</option>
                                                            <option value="delete">Delete User</option>
                                                        </select><span class="select2 select2-container select2-container--default" dir="ltr" data-select2-id="2" style="width: 112.667px;"><span class="selection"><span class="select2-selection select2-selection--single" role="combobox" aria-haspopup="true" aria-expanded="false" tabindex="0" aria-disabled="false" aria-labelledby="select2-f26j-container"><span class="select2-selection__rendered" id="select2-f26j-container" role="textbox" aria-readonly="true"><span class="select2-selection__placeholder">Bulk Action</span></span><span class="select2-selection__arrow" role="presentation"><b role="presentation"></b></span></span></span><span class="dropdown-wrapper" aria-hidden="true"></span></span>--%>
                                                    </div>
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
                                        <div class="nk-tb-list nk-tb-ulist is-compact">
                                         
                                            <asp:Repeater ID="rpscroll" runat="server" >
                                                <HeaderTemplate>
                                                       <div class="nk-tb-item nk-tb-head">
                                                
                                                <div class="nk-tb-col"><span class="sub-text">T.ID</span></div>
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Name</span></div>
                                             
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Account</span></div>
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Type</span></div>
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Amount</span></div>
                                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">C-Amount</span></div>
                                                <div class="nk-tb-col tb-col-md"><span class="sub-text">Entry By</span></div>
                                                
                                            </div>

                                                </HeaderTemplate>

                                            <ItemTemplate>
                                            <div class="nk-tb-item">
                                                <div class="nk-tb-col nk-tb-check" >
                                                    <asp:CheckBox runat="server" ID="smschk"   Checked="true"  />
                                       
                                                <span class="sub-text"><asp:Label   ID="lblid" for="uid" runat="server" Text='<%# Eval("id") %>' ></asp:Label></span>
                                                    </div>
                                                <div class="nk-tb-col tb-col-lg">
                                                    
                                                        
                                                        <div class="user-name"><span class="tb-lead">
                                                            <asp:Label ID="lbllname" runat="server" Text='<%# Eval("name") %>' ></asp:Label>
                                                                               </span>
                                                            
                                                        </div>
                                                    <span><asp:Label ID="lbllacno"  runat="server" Text='<%# Eval("acno") %>' ></asp:Label></span>
                                                    
                                                </div>
                                                <div class="nk-tb-col tb-col-lg"><span>
                                                    <asp:Label ID="lbllach" runat="server" Text='<%# Eval("ached") %>' ></asp:Label>
                                                                                 </span></div>
                                                
                                                <div class="nk-tb-col tb-col-md"><span>
                                                    <asp:Label ID="lblltyp" runat="server" Text='<%# Eval("typ") %>' ></asp:Label>
                                                                                 </span></div>
                                                <div class="nk-tb-col tb-col-lg"><span>
                                                    <asp:Label ID="lbldamt" runat="server" Text='<%# Eval("amount") %>' style="float:right;text-align:right" ></asp:Label>
                                                                                 </span></div>
                                                <div class="nk-tb-col tb-col-lg">
                                                    <span class="tb-lead">
                                                        <asp:Label ID="lblcamt" runat="server" Text='<%# Eval("amountc") %>' style="float:right;text-align:right"></asp:Label>
                                                    </span>
                                                </div>
                                                <div class="nk-tb-col tb-col-lg">
                                                    <div class="user-name tb-amount " style="text-align:right  ">
                                                    <span class="sub-text" ><asp:Label ID="Label4" Font-Size="Smaller"   runat="server" Text='<%# Eval("sesusr") %>'  ></asp:Label></span>
                                                        </div>

                                                    <span class="sub-text" style="text-align:right;float:right"><asp:Label ID="Label5" Font-Size="Smaller"  runat="server" Text='<%# Eval("entrat") %>' ></asp:Label></span>
                                                        
                                                </div>
                                                
                                             
                                                    
                                                <div class="nk-tb-col nk-tb-col-tools">
                                                    <ul class="nk-tb-actions gx-2">
                                                      
                                                       <%-- <li class="nk-tb-action-hidden"><a href="#" class="btn btn-sm btn-icon btn-trigger" data-toggle="tooltip" data-placement="top" title="" data-original-title="Wallet"><em class="icon ni ni-wallet-fill"></em></a></li>
                                                        <li class="nk-tb-action-hidden"><a href="#" class="btn btn-sm btn-icon btn-trigger" data-toggle="tooltip" data-placement="top" title="" data-original-title="Send Email"><em class="icon ni ni-mail-fill"></em></a></li>
                                                        <li class="nk-tb-action-hidden"><a href="#" class="btn btn-sm btn-icon btn-trigger" data-toggle="tooltip" data-placement="top" title="" data-original-title="Suspend"><em class="icon ni ni-user-cross-fill"></em></a></li>
                                                      --%>  <li>
                                                            <div class="drodown"><a href="#" class="btn btn-sm btn-icon btn-trigger dropdown-toggle" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                                <div class="dropdown-menu dropdown-menu-right">
                                                                    <ul class="link-list-opt no-bdr">
                                                                        
                                                                        <li><asp:LinkButton runat="server"  CommandName="ViewClick" CommandArgument='<%# Eval("id") %>' ><em class="icon ni ni-eye"></em><span>View Details</span></asp:LinkButton> </li>
                                                                        <li><asp:LinkButton runat="server"  CommandName="ApproveClick" CommandArgument='<%# Eval("id") %>'><em class="icon ni ni-check-fill-c"></em><span>Approve</span></asp:LinkButton></li>
                                                                        <li><asp:LinkButton runat="server"  CommandName="RejectClick" CommandArgument='<%# Eval("id") %>'><em class="icon ni ni-cross-fill-c"></em><span>Reject</span></asp:LinkButton> </li>
                                                                        <%--<li><a href="#"><em class="icon ni ni-na"></em><span>Suspend User</span></a></li>--%>
                                                                    </ul>
                                                                </div>
                                                            </div>
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
                                </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="upscrlrejct" Visible="false" >
                            <ContentTemplate>

                            
                        <div class="nk-block">

                            <div class="row">
				<div class="card card-body is-dark ">


                    <h6 class="card-title text-primary">Voucher Info</h6>

                    


                    <asp:UpdatePanel runat="server" >
                        <ContentTemplate>
                            <asp:Panel ID="vinfo" runat="server" Visible="true" >


                    <div class="form-group row border-sm-bottom">
                        <asp:label runat="server" CssClass="col-sm-1 col-form-label text-danger " text="Date"></asp:label>
                        <asp:Label runat="server" CssClass="col-sm-2 col-form-label "   ID="lbldt"></asp:Label>
                        <asp:label runat="server"  CssClass="col-sm-2  text-danger col-form-label  " text="Entered By"></asp:label>
                        <asp:Label runat="server"  ID="lblentryby" CssClass="col-sm-3 col-form-label  "></asp:Label>
                        
                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-danger " text="Type"></asp:label>
                        <asp:Label runat="server"   ID="lbltyp" CssClass="col-sm-2 col-form-label "></asp:Label>
                    </div>


                    <div class="form-group row border-sm-bottom ">
                        
                        <asp:label runat="server" CssClass="col-sm-1 col-form-label text-danger " text="GL Head"></asp:label>
                        <asp:Label runat="server"   ID="lblprod" CssClass="col-sm-2 col-form-label " ></asp:Label>
                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-danger " text="Customer Name"></asp:label>
                        <asp:Label runat="server" CssClass="col-sm-3 col-form-label " ID="lblname" ></asp:Label>
                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-danger " text="Account No"></asp:label>
                        <asp:Label runat="server"  ID="lblacn" CssClass="col-sm-2 col-form-label "></asp:Label>
                    </div>

                    <div class="form-group row ">
                        <asp:label runat="server" CssClass="col-sm-1 col-form-label text-danger " text="Amount"></asp:label>
                         <asp:Label runat="server"  ID="lblamt" CssClass="col-sm-2 col-form-label text-primary " Font-Bold="true" ></asp:Label>

                      
                      
                        <asp:label runat="server" CssClass="col-sm-2 col-form-label text-danger " text="C_Amount"></asp:label>
                         <asp:Label runat="server"  ID="lblcamt" CssClass="col-sm-3 col-form-label text-warning  " Font-Bold="true" ></asp:Label>
                                          		<div class="col-sm-2">
                                            <div class="form-check form-check-inline">
											<label class="form-check-label">
												<asp:CheckBox  runat="server"  ID="chksms"   />
												SMS Alert
											<i class="input-frame"></i></label>
										</div>
                                                   
					   </div>
                      
                        
                               <asp:Label runat="server"  CssClass=" col-sm-2 col-form-label  "  ID="lblmobile"   ></asp:Label>
                        
                    </div>

                                </asp:Panel>
                                <asp:Panel runat="server" ID="pnlotp" Visible="false" >
                                   
                                        <div class="card card-body is-dark  ">
                                             <div class="form-group row ">
                                            <label class="col-sm-2 col-form-label ">Reason to Reject</label>
                                            <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="txtreason" CssClass="form-control " style="margin-top:0px;margin-left:0px" Width="200px" data-validation-engine="validate[required" data-errormessage-value-missing="Enter a Valid Reason"> </asp:TextBox>
                                                </div>
                                            <asp:label ID="lbloptcaption" runat="server"  CssClass="col-sm-2 col-form-label " text="Enter OTP" Visible="false"  ></asp:label>
                                            <div class="col-sm-3">
                                        <asp:TextBox runat="server" ID="txtotp" CssClass="form-control "  Width="200px" Visible="false"  data-validation-engine="validate[required" data-errormessage-value-missing="Enter a Valid Otp"> </asp:TextBox>
                                                </div>
                                            <div class="col-sm-1">

      <asp:Button runat="server" ID ="btnpay" CssClass ="btn btn-outline-danger"  Text ="Reject" />
                                                </div>
                                            </div>
                                        </div>
                                   
                                </asp:Panel>



                                <asp:Panel runat="server" Visible ="true" ID="btnpnl">
                    <div class="form-group row border-sm-bottom"></div>
                    <div class="form-group row ">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-6">

                        <div class="btn-group " role="group" >
                                         <asp:Button runat="server" ID="btn_rject" CssClass="btn btn-outline-danger   " Text ="Reject" CausesValidation="false" />
                      <asp:Button runat="server" ID="btn_authorize" CssClass="btn btn-outline-success  " Text ="Authorize" CausesValidation="false"  UseSubmitBehavior="false" OnClientClick="this.disabled=true" />
                      <asp:Button runat="server" ID="btn_clear" CssClass="btn btn-outline-secondary    " Text ="Clear" CausesValidation="false" />
         
                        </div>
                    </div>
                        </div>
                                    </asp:Panel>
                                
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    




					 <div class="div-spread ">
              <asp:Panel ID="vin" runat ="server"  Visible="false" >
                  
                          
                 
                      <table>
                          <tbody>
                              <tr>
                        <td><asp:Label Style="margin-left:0px;margin-top:8px;width:auto;font-size:small " runat="server" Text="Reason" ID="lblcap"></asp:Label>  </td>
                      <td>
</td>
                              </tr>
                              </tbody>
                              </table>
                  <div class ="div-left ">
                      <br />
                      <div class ="div-left">
                                    </div>
                  </div>
                  </asp:Panel>
                       

				</div>
			</div>

                </div>
                        </div>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

                   </ContentTemplate>
           </asp:UpdatePanel>





            <!--- actual content  -->
            

        <%--    <div class="modal" tabindex="-1" role="dialog" id="Modalviewdet" data-backdrop="static" data-keyboard="false" >
                <div class="modal-dialog modal-xl" role="document" >
                    <div class="modal-content">
                        <div class="modal-body">

                            <asp:UpdatePanel runat="server" Visible="true">
                <ContentTemplate>

                

			

                    </ContentTemplate>
            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
                </div>--%>



                     <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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
                                    <br />
                                    Processing.....
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
			</form>
	
    <script type="text/javascript">

        function showdet()

        {
            $('#Modalviewdet').toggle();

        }


        function mailerror(msg) {

            alert(msg);
        }

        
    </script>
    
</asp:Content>
