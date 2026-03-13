<%@ Page Title="Rate of Interest" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
	CodeBehind="RateofInterest.aspx.vb" Inherits="Fiscus.RateofInterest" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<link rel="stylesheet" href="css/daterangepicker.css" type="text/css" />
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
		<form class="frmfc" runat="server">
			<asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

			<nav class="page-breadcrumb">
				<ol class="breadcrumb">
					<li class="breadcrumb-item"><a href="#">Master</a></li>
					<li class="breadcrumb-item active">Rate of Interest</li>
				</ol>
			</nav>
			<asp:UpdatePanel runat="server">
				<ContentTemplate>


					<div class="row">
						<div class="col-md-12 grid-margin stretch-card">
							<div class="card">
								<div class="card-body">
									<div class="card-title">Rate of Interest</div>


									<div class="form-group row">
										<label class="col-form-label col-md-1">Product</label>
										<div class="col-md-2">
											<asp:UpdatePanel runat="server">
												<ContentTemplate>


													<asp:DropDownList ID="ddprod" runat="server" CssClass="form-control"
														AutoPostBack="true">
														<asp:ListItem Value="0">Select</asp:ListItem>
														<asp:ListItem Value="DS">DS</asp:ListItem>
														<asp:ListItem Value="FD">FD</asp:ListItem>
														<asp:ListItem Value="KMK">KMK</asp:ListItem>
														<asp:ListItem Value="RD">RD</asp:ListItem>
														<asp:ListItem Value="RID">RID</asp:ListItem>
														<asp:ListItem Value="SB">SB</asp:ListItem>
														<asp:ListItem Value="DCL">DCL</asp:ListItem>
														<asp:ListItem Value="DL">DL</asp:ListItem>
														<asp:ListItem Value="JL">JL</asp:ListItem>
														<asp:ListItem Value="ML">ML</asp:ListItem>
													</asp:DropDownList>
												</ContentTemplate>
											</asp:UpdatePanel>
										</div>
										<label class="col-form-label col-md-1">From</label>
										<div class="col-md-2">
											<asp:TextBox ID="txtfrm" runat="server" CssClass="form-control">
											</asp:TextBox>
										</div>
										<label class="col-form-label col-md-1">To</label>
										<div class="col-md-2">
											<asp:TextBox ID="txtto" runat="server" CssClass="form-control">
											</asp:TextBox>
										</div>
									</div>


									<div class="form-group row">
										<label class="col-form-label col-md-1">Between</label>
										<div class="col-md-2">
											<asp:TextBox ID="txtprdfrm" runat="server" CssClass="form-control">
											</asp:TextBox>
										</div>
										<label class="col-form-label col-md-1">And</label>
										<div class="col-md-2">
											<asp:TextBox ID="txtprdto" runat="server" CssClass="form-control">
											</asp:TextBox>
										</div>
									</div>

									<div class="form-group row">
										<label class="col-form-label col-md-1">Interest(C)</label>
										<div class="col-md-2">
											<asp:TextBox ID="txtcint" runat="server" CssClass="form-control">
											</asp:TextBox>
										</div>
										<label class="col-form-label col-md-1">Interest(D)</label>
										<div class="col-md-2">
											<asp:TextBox ID="txtdint" runat="server" CssClass="form-control">
											</asp:TextBox>
										</div>
										<label class="col-form-label col-md-1">Penel</label>
										<div class="col-md-2">
											<asp:TextBox ID="txtpenel" runat="server" CssClass="form-control">
											</asp:TextBox>
										</div>

									</div>




									<asp:UpdatePanel runat="server" ID="upsch" Visible="false">
										<ContentTemplate>
											<div class="form-group row">
												<label class="col-form-label col-md-1">Scheme</label>
												<div class="col-md-2">
													<asp:DropDownList ID="ddsch" runat="server" CssClass="form-control">
														<asp:ListItem Value="">Select</asp:ListItem>
														<asp:ListItem Value="PRIME ULTRA">Prime Ultra</asp:ListItem>
														<asp:ListItem Value="PRIME PLUS">Prime Plus</asp:ListItem>
														<asp:ListItem Value="PRIME">Prime</asp:ListItem>
														<asp:ListItem Value="PRIME SPECIAL">Prime Special</asp:ListItem>
													</asp:DropDownList>
												</div>
												<label class="col-form-label col-md-1">Share</label>
												<div class="col-md-2">
													<asp:DropDownList ID="ddshare" runat="server"
														CssClass="form-control">
														<asp:ListItem Value="0">Select</asp:ListItem>
														<asp:ListItem Value="99">Below 1000</asp:ListItem>
														<asp:ListItem Value="101">Above 1000</asp:ListItem>
													</asp:DropDownList>
												</div>
												<label class="col-form-label col-md-1">Deposit</label>
												<div class="col-md-2">
													<asp:DropDownList ID="ddagst" runat="server"
														CssClass="form-control">
														<asp:ListItem Value="">Select</asp:ListItem>
														<asp:ListItem Value="FD">FD</asp:ListItem>
														<asp:ListItem Value="RD">RD</asp:ListItem>
														<asp:ListItem Value="RID">RID</asp:ListItem>
													</asp:DropDownList>
												</div>
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>

									<div class="form-group row">
										<div class="col-md-4">

										</div>
										<div class="col-md-4">
											<asp:Button runat="server" ID="btnadd" Text="Add to List"
												CssClass="btn btn-outline-primary " />
											<asp:Button runat="server" ID="btnclr" Text="Clear"
												CssClass="btn btn-outline-secondary " />
										</div>
									</div>

								</div>
							</div>
						</div>
					</div>

				</ContentTemplate>
			</asp:UpdatePanel>
			<script src="../js/jquery.min.js" type="text/javascript"></script>
			<script src="../js/daterangepicker.js" type="text/javascript"></script>
			<script>
				$(document).ready(function () {








					$(document).ready(function () {


						var prm = Sys.WebForms.PageRequestManager.getInstance();
						prm.add_initializeRequest(InitializeRequest);
						prm.add_endRequest(EndRequest);
						InitDatePick();


					});

					function InitializeRequest(sender, args) {
					}
					function EndRequest(sender, args) {
						// after update occur on UpdatePanel re-init the Autocomplete

						InitDatePick();
					}
				});

				function InitDatePick() {

					$("#<%=txtfrm.ClientID%>").daterangepicker({

						"singleDatePicker": true,
						"autoUpdateInput": true,
						showDropdowns: false,
						"autoApply": true,
						locale: {
							format: 'DD-MM-YYYY'
						},
					}, function (start, end, label) {

						$("#<%=txtfrm.ClientID%>").val(start.format('DD-MM-YYYY'));
					});

					$("#<%=txtto.ClientID%>").daterangepicker({

						"singleDatePicker": true,
						"autoUpdateInput": true,
						showDropdowns: false,
						"autoApply": true,
						locale: {

							format: 'DD-MM-YYYY'
						},
					}, function (start, end, label) {

						$("#<%=txtto.ClientID%>").val(start.format('DD-MM-YYYY'));
					});



				}

			</script>

		</form>
	</asp:Content>