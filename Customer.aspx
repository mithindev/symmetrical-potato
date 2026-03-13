<%@ Page Title="Customer" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
	CodeBehind="Customer.aspx.vb" Inherits="Fiscus.Customer" EnableEventValidation="true" %>

	<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
		<title>Customer </title>

		<style>
			input[type=search]::-webkit-search-cancel-button {
				-webkit-appearance: searchfield-cancel-button;

			}
		</style>

		<link href="css/daterangepicker.css" rel="stylesheet" />
		<link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">

	</asp:Content>
	
	<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

		<form class="forms-sample" runat="server"
			style="background-color: #F3F4FF; font-family: 'Inter', sans-serif; min-height: 100vh; padding: 20px;">
			
			<asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
			<nav class="page-breadcrumb" style="margin-bottom: 24px;">
				<ol class="breadcrumb" style="background: transparent; padding: 0;">
					<li class="breadcrumb-item"><a href="#" style="color: #6B7280; text-decoration: none;">Master</a>
					</li>
					<li class="breadcrumb-item active" style="color: #374151; font-weight: 500;">Customer</li>
				</ol>
			</nav>

			<asp:UpdatePanel runat="server">
				<ContentTemplate>
					<asp:Panel ID="alertmsg" runat="server" Visible="true">
						<div class="alert alert-secondary alert-dismissible fade show" role="alert"
							style="border-radius: 12px; background: linear-gradient(to right, #ffffff, #F8FAFC); border: 1px solid #E2E8F0; border-left: 4px solid #7C3AED; box-shadow: 0 4px 15px rgba(0,0,0,0.03); display: flex; align-items: center; padding: 12px 40px 12px 20px; position: relative;">
							<div style="display: flex; align-items: center; gap: 12px;">
								<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
									<circle cx="12" cy="12" r="10"></circle>
									<line x1="12" y1="16" x2="12" y2="12"></line>
									<line x1="12" y1="8" x2="12.01" y2="8"></line>
								</svg>
								<h5 style="margin: 0; display: flex; align-items: center;">
									<asp:Label ID="lblinfo" runat="server" CssClass="text-dark"
										style="font-size: 14px; font-weight: 500; font-family: 'Inter', sans-serif;"></asp:Label>
								</h5>
							</div>

							<button type="button" class="close" data-dismiss="alert" aria-label="Close"
								style="position: absolute; right: 16px; top: 50%; transform: translateY(-50%); padding: 0; margin: 0; background: none; border: none; opacity: 0.5; transition: opacity 0.2s; display: flex; align-items: center; justify-content: center;" onmouseover="this.style.opacity='1'" onmouseout="this.style.opacity='0.5'">
								<span aria-hidden="true" style="font-size: 20px; color: #64748B; line-height: 1;">&times;</span>
							</button>

						</div>
					</asp:Panel>
				</ContentTemplate>
			</asp:UpdatePanel>

			<asp:UpdatePanel runat="server">
				<ContentTemplate>
					<div class="row" style="margin-bottom: 16px;">
						<div class="col-md-6 grid-margin stretch-card" style="margin-bottom: 16px;">
							<div class="card"
								style="background: white; border-radius: 16px; box-shadow: 0 10px 30px rgba(0,0,0,0.05); border: none; padding: 24px; height: 100%;">
								<div class="card-body" style="padding: 0;">
									<div style="margin-bottom: 16px;">
										<h6 class="card-title"
											style="color: #5B21B6; font-weight: 600; font-size: 18px; margin-bottom: 8px; font-family: 'Inter', sans-serif;">
											Personal Details</h6>
										<div
											style="background: linear-gradient(90deg, #6D28D9, #2563EB); height: 3px; width: 40px; border-radius: 4px;">
										</div>
									</div>


									<div class="form-group row" style="margin-bottom: 8px;">
										<label class="col-sm-4 col-form-label" for="txtcid"
											style="font-size: 15px; font-weight: 700; color: #111827;">Customer
											Id</label>
										<div class="col-sm-8">
											<asp:UpdatePanel ID="upcid" runat="server">

												<ContentTemplate>
													<asp:TextBox ID="txtcid" runat="server" CssClass="form-control"
														TextMode="Search" AutoPostBack="true"
														style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s;"
														onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
														onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
													</asp:TextBox>


												</ContentTemplate>
											</asp:UpdatePanel>
										</div>
									</div>

									<div class="form-group row " style="margin-bottom: 8px;">
										<label class="col-sm-4 col-form-label" for="txtFirstName"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">First
											Name</label>
										<div class="col-sm-8">

											<asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"
												placeholder="First Name"
												style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s;"
												onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
												onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
											</asp:TextBox>
											<asp:RequiredFieldValidator ID="FVtxtfirstname" runat="server"
												ControlToValidate="txtFirstName" Text=" Fill this !" Font-Size="Small"
												ForeColor="Red" SetFocusOnError="True"
												style="margin-top: 4px; display: inline-block;">
											</asp:RequiredFieldValidator>

										</div>

									</div>
									<div class="form-group row" style="margin-bottom: 8px;">
										<label class="col-sm-4 col-form-label " for="txtLastName"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">Last Name</label>
										<div class="col-sm-8">
											<asp:TextBox id="txtLastName" runat="server" CssClass="form-control"
												placeholder="Last Name"
												style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s;"
												onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
												onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
											</asp:TextBox>
										</div>

									</div>
									<div class="form-group row " style="margin-bottom: 8px;">
										<label class="col-sm-4 col-form-label " for="txtDOB"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">Date of
											Birth</label>
										<div class="col-sm-8">
											<asp:TextBox id="txtDOB" runat="server" CssClass="form-control"
												placeholder="(dd-mm-yyyy)"
												style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s;"
												onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
												onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
											</asp:TextBox>
										</div>
									</div>

									<div class="form-group row " style="margin-bottom: 0;">
										<label class="col-sm-4 col-form-label " for="SelectGender"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">Gender</label>
										<div class="col-sm-8">
											<asp:UpdatePanel ID="GENDER" runat="server">
												<ContentTemplate>
													<div style="position: relative; outline: none;" id="customGenderDropdown" tabindex="0" onblur="setTimeout(function() { document.getElementById('genderOptions').style.display = 'none'; document.getElementById('genderSelected').style.borderColor='#E5E7EB'; document.getElementById('genderSelected').style.boxShadow='none'; }, 200)">
														<div id="genderSelected" 
															style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s; color: #6B7280; font-weight: 500; background: white url('data:image/svg+xml;utf8,<svg xmlns=\'http://www.w3.org/2000/svg\' width=\'16\' height=\'16\' viewBox=\'0 0 24 24\' fill=\'none\' stroke=\'%236B7280\' stroke-width=\'2\' stroke-linecap=\'round\' stroke-linejoin=\'round\'><polyline points=\'6 9 12 15 18 9\'></polyline></svg>') no-repeat right 12px center; cursor: pointer; user-select: none;" 
															onclick="var opts = document.getElementById('genderOptions'); if(opts.style.display==='none'){opts.style.display='block'; this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';}else{opts.style.display='none'; this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';}">
															Select Gender
														</div>
														<div id="genderOptions" 
															style="display: none; position: absolute; top: 100%; left: 0; right: 0; background: white; border: 1px solid #E5E7EB; border-radius: 8px; margin-top: 4px; box-shadow: 0 10px 25px rgba(0,0,0,0.1); z-index: 100; overflow: hidden; padding: 4px;">
															<div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="document.getElementById('<%=SelectGender.ClientID%>').value='Male'; var b=document.getElementById('genderSelected'); b.innerText='Male'; b.style.color='#374151'; b.style.fontWeight='400';">Male</div>
															<div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="document.getElementById('<%=SelectGender.ClientID%>').value='FeMale'; var b=document.getElementById('genderSelected'); b.innerText='FeMale'; b.style.color='#374151'; b.style.fontWeight='400';">FeMale</div>
															<div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="document.getElementById('<%=SelectGender.ClientID%>').value='Others'; var b=document.getElementById('genderSelected'); b.innerText='Others'; b.style.color='#374151'; b.style.fontWeight='400';">Others</div>
														</div>
													</div>

													<asp:DropDownList ID="SelectGender" runat="server" style="display: none;">
														<asp:ListItem Value="">Select Gender</asp:ListItem>
														<asp:ListItem>Male</asp:ListItem>
														<asp:ListItem>FeMale</asp:ListItem>
														<asp:ListItem>Others</asp:ListItem>

													</asp:DropDownList>

													<script type="text/javascript">
														setTimeout(function() {
															var realSelect = document.getElementById('<%=SelectGender.ClientID%>');
															var displayBox = document.getElementById('genderSelected');
															if (realSelect && displayBox) {
																var val = realSelect.value;
																if (val && val !== 'Select Gender') {
																	displayBox.innerText = val;
																	displayBox.style.color = '#374151';
																	displayBox.style.fontWeight = '400';
																} else {
																	displayBox.innerText = 'Select Gender';
																	displayBox.style.color = '#6B7280';
																	displayBox.style.fontWeight = '500';
																}
															}
														}, 50);
													</script>
												</ContentTemplate>
											</asp:UpdatePanel>

										</div>
									</div>



								</div>
							</div>
						</div>
						<div class="col-md-6 grid-margin stretch-card" style="margin-bottom: 16px;">
							<div class="card"
								style="background: white; border-radius: 16px; box-shadow: 0 10px 30px rgba(0,0,0,0.05); border: none; padding: 24px; height: 100%;">
								<div class="card-body" style="padding: 0;">
									<div style="margin-bottom: 16px;">
										<h6 class="card-title"
											style="color: #5B21B6; font-weight: 600; font-size: 18px; margin-bottom: 8px; font-family: 'Inter', sans-serif;">
											Contact Details</h6>
										<div
											style="background: linear-gradient(90deg, #6D28D9, #2563EB); height: 3px; width: 40px; border-radius: 4px;">
										</div>
									</div>
									<div class="form-group row" style="margin-bottom: 8px;">
										<label for="txtAddress" class="col-sm-3 col-form-label"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">Address</label>
										<div class="col-sm-9">
											<asp:TextBox CssClass="form-control" runat="server" ID="txtAddress"
												height="100px" TextMode="MultiLine"
												style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s; resize: vertical;"
												onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
												onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
											</asp:TextBox>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
												ControlToValidate="txtAddress" Text=" Fill this !" Font-Size="Small"
												ForeColor="Red" SetFocusOnError="True"
												style="margin-top: 4px; display: inline-block;">
											</asp:RequiredFieldValidator>
										</div>
									</div>

									<div class="form-group row" style="margin-bottom: 8px;">
										<label for="txtPincode" class="col-sm-3 col-form-label"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">Pincode</label>
										<div class="col-sm-9">
											<asp:TextBox ID="txtPincode" CssClass="form-control" runat="server"
												placeholder="PinCode" MaxLength="6"
												style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s;"
												onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
												onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
											</asp:TextBox>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
												ControlToValidate="txtPincode" Text=" Fill this !" Font-Size="Small"
												ForeColor="Red" SetFocusOnError="True"
												style="margin-top: 4px; display: inline-block;">
											</asp:RequiredFieldValidator>
										</div>
									</div>


									<div class="form-group row" style="margin-bottom: 8px;">
										<label for="txtphone" class="col-sm-3 col-form-label"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">Phone No</label>
										<div class="col-sm-9">
											<asp:TextBox ID="txtphone" CssClass="form-control"
												placeholder="STD-PHONE NO" runat="server"
												style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s;"
												onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
												onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
											</asp:TextBox>
										</div>
									</div>

									<div class="form-group row" style="margin-bottom: 8px;">
										<label for="txtMobile" class="col-sm-3 col-form-label"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">Mobile</label>
										<div class="col-sm-9">
											<asp:TextBox ID="txtMobile" CssClass="form-control" placeholder="Mobile "
												runat="server" MaxLength="10"
												style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s;"
												onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
												onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
											</asp:TextBox>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
												ControlToValidate="txtMobile" Text=" Fill this !" Font-Size="Small"
												ForeColor="Red" SetFocusOnError="True"
												style="margin-top: 4px; display: inline-block;">
											</asp:RequiredFieldValidator>
										</div>
									</div>


									<div class="form-group row" style="margin-bottom: 0;">
										<label for="txtEmail" class="col-sm-3 col-form-label"
											style="font-size: 15px; font-weight: 600; color: #4B5563;">Email</label>
										<div class="col-sm-9">
											<asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="Email Id"
												runat="server"
												style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 8px 12px; font-size: 14px; transition: all 0.2s;"
												onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
												onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
											</asp:TextBox>
										</div>
									</div>

								</div>
							</div>
						</div>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>
			<div class="form-group" style="border-bottom: 1px solid #E5E7EB; margin: 16px 0;"></div>
			<div class="form-group row ">
				<div class="col-sm-12" style="display: flex; justify-content: flex-end; padding-right: 15px;">
					<asp:UpdatePanel runat="server">
						<ContentTemplate>


							<div class="btn-group" role="group" aria-label="Basic example" style="gap: 12px;">
								<asp:Button runat="server" CausesValidation="false" ID="btnCancel"
									class="btn btn-outline-secondary" Text="Cancel"
									style="border-radius: 10px; padding: 10px 28px; font-weight: 500; font-family: 'Inter', sans-serif; background: transparent; border: 1px solid #D1D5DB; color: #4B5563; transition: all 0.2s;"
									onmouseover="this.style.background='#F3F4F6'; this.style.transform='translateY(-1px)';"
									onmouseout="this.style.background='transparent'; this.style.transform='translateY(0)';" />
								<asp:button runat="server" Text="Save" class="btn btn-primary" ID="btnsave"
									style="background: linear-gradient(135deg, #7C3AED, #2563EB); color: white; border-radius: 10px; padding: 10px 28px; border: none; font-weight: 500; font-family: 'Inter', sans-serif; transition: all 0.2s; box-shadow: 0 4px 14px rgba(37,99,235,0.2);"
									onmouseover="this.style.transform='translateY(-2px)'; this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
									onmouseout="this.style.transform='translateY(0)'; this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';" />
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</div>
		</form>




		<script src="js/jquery.js" type="text/javascript"></script>
		<script src="js/jquery-ui.min.js" type="text/javascript"></script>
		<link href="css/jquery-ui.min.css" rel="stylesheet" />
		<script src="js/daterangepicker.js" type="text/javascript"></script>


		<script type="text/javascript">
			$(document).ready(function () {




				var prm = Sys.WebForms.PageRequestManager.getInstance();
				prm.add_initializeRequest(InitializeRequest);
				prm.add_endRequest(EndRequest);

				// Place here the first init of the autocomplete
				InitAutoCompl();
			});

			function InitializeRequest(sender, args) {
			}

			function EndRequest(sender, args) {
				// after update occur on UpdatePanel re-init the Autocomplete
				InitAutoCompl();
			}

			function InitAutoCompl() {

				$("#<%=txtDOB.ClientID%>").daterangepicker({

					"singleDatePicker": true,
					"autoUpdateInput": true,
					showDropdowns: false,
					"autoApply": true,
					locale: {
						format: 'DD-MM-YYYY'
					},
				}, function (start, end, label) {

					$("#<%=txtDOB.ClientID%>").val(start.format('DD-MM-YYYY'));
				});




				$("#<%=txtcid.ClientID %>").autocomplete({
					source: function (request, response) {
						$.ajax({
							url: '<%=ResolveUrl("~/membersearch.asmx/GetUsers") %>',
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
										image: item.ImageURL
									}
								}))
							}
						});
					},
					minLength: 1,
					select: function (event, ui) {

						$("#<%=txtcid.ClientID %>").val(ui.item.memberno);


						return false;
					}
				})
					.autocomplete("instance")._renderItem = function (ul, item) {
						return $("<li>")
							.append("<div style='container'>")
							.append("<div class='img-div column'>")
							.append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
							.append("</div")
							.append("<div class='info-div column'>")
							.append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
							.append("<div class='userinfo'>" + item.lastname + "</div>")
							.append("<div class='userinfo'>" + item.address + "</div>")
							.append("</br>")
							.append("</div")
							.append("</div>")
							.appendTo(ul);
					};
			}




		</script>


		<style>
			.memno {
				z-index: 10;
				position: absolute;
				right: 10px;
				top: 10px;
				font-size: 9px;
				color: #000;
			}

			.img-div {
				display: inline-block;
				vertical-align: top;
			}

			.info-div {
				display: inline-block;
			}

			.username {
				display: inline-block;
				font-weight: bold;
				margin-bottom: 0em;
			}

			.userimage {
				float: left;
				max-height: 48px;
				max-width: 48px;
				margin-right: 10px;
			}

			.userinfo {

				margin: 0px;
				padding: 0px;
				font-size: 10px;
			}
		</style>


	</asp:Content>