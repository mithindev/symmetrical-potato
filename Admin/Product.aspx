<%@ Page Title="Products" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Product.aspx.vb" Inherits="Fiscus.Product" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <form id="roles" runat="server" >

        <asp:ScriptManager ID="SM3" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Master</a></li>
						<li class="breadcrumb-item active" >Products</li>
					</ol>
				</nav>
	
			

    

        		<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								<h6 class="card-title text-primary ">Products</h6>


								<div class="form-group row ">
									<label class="col-sm-2 col-form-label ">Product Name</label>
									<div class="col-sm-3">
										  <asp:TextBox runat="server" ID="prodname" Style="margin-left :10px;float:left" Width ="200px" CssClass="form-control "></asp:TextBox>
									</div>
								</div>
																<div class="form-group row ">
									<label class="col-sm-2 col-form-label ">Short Name</label>
									<div class="col-sm-3">
										<asp:TextBox runat="server" ID="shrtname" Style="margin-left :10px;float:left" Width ="200px" CssClass="form-control "></asp:TextBox>
									</div>
								</div>
																<div class="form-group row ">
									<label class="col-sm-2 col-form-label ">Account No Prefix</label>
									<div class="col-sm-3">
										<asp:TextBox runat="server" ID="acnprefix" Style="margin-left :10px;float:left" Width ="200px" CssClass="form-control "></asp:TextBox>
									</div>
								</div>
																<div class="form-group row ">
									<label class="col-sm-2 col-form-label ">Product Type</label>
									<div class="col-sm-3">
										<asp:DropDownList runat="server" ID="typ" Style="margin-left:10px;float:left"  CssClass="form-control " >
                            <asp:ListItem>Select</asp:ListItem>
                            <asp:ListItem>Deposit</asp:ListItem>
                            <asp:ListItem>Loan</asp:ListItem>
                        </asp:DropDownList>
									</div>
								</div>


															
												<div class="form-group row border-bottom "></div>
												<div class="form-group row ">
													<div class="col-sm-4"></div>
													<div class="col-sm-4">
														<div class="btn-group " role="group" >

															 <asp:Button ID="btn_clr" runat ="server" CssClass ="btn btn-outline-secondary  " Text ="Clear" />
															<asp:Button ID="btn_update" runat ="server" CssClass ="btn btn-outline-primary " Text ="Update" />
														</div>
													</div>

												</div>

								</div>
							</div>
						</div>
					</div>
		 </form>

		
</asp:Content>
