<%@ Page Title="Roles" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Roles.aspx.vb" Inherits="Fiscus.Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <form id="roles" runat="server" >

        <asp:ScriptManager ID="SM3" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Admin</a></li>
						<li class="breadcrumb-item active" >Roles</li>
					</ol>
				</nav>
	
			

    

        		<div class="row">
					<div class="col-md-12 grid-margin stretch-card">
						<div class="card">
							<div class="card-body">
								<h6 class="card-title text-primary ">Role Allocation</h6>
								<div class="form-group row  border-bottom ">
									<label class="col-sm-1 col-form-label" >User</label>
									<div class="col-sm-3">
										<asp:DropDownList runat="server" ID="usr" CssClass="form-control " Style="margin-left: -7px" Width="150px" ></asp:DropDownList>
                                                
									</div>
									<label class="col-sm-1 col-form-label" >Role</label>
									<div class="col-sm-3">
										<asp:DropDownList runat="server" ID="usrrole" CssClass="form-control " Style="margin-left: -7px" Width="150px" ></asp:DropDownList>
                                                
									</div>
									<div class="col-sm-1">
										<asp:Button ID="btn_inw" runat="server" CssClass="btn btn-outline-primary btn-sm  " Text="ADD TO LIST"  />
									</div>

									
								</div>

                                <asp:UpdatePanel runat="server" >
                                    <ContentTemplate>

								<table style="border: 0px; width: 60%; margin-left: 0px; float: left; margin-top: 20px" class="table table-bordered">
                                                    <tbody>

                                                        <thead>
                                                            <th style="width:10%;">S.No</th>
                                                            <th style="width:30%;">User Name</th>
                                                            <th style="width:20%;">Roles</th>
                                                        </thead>
                                                    </tbody>
                                                </table>
                                    
								            <asp:GridView runat="server" ID="gvin" AutoGenerateColumns="false" Style="margin-top: 0px; margin-left: 0px; z-index: 1"
                                                    AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="15" EmptyDataText="No Data Found"
                                                    Width="60%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false" CssClass="table table-bordered  ">
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="S No">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>

                                                                <asp:Label ID="lbldat" runat="server" Width="100%" Text='<%#Eval("username")%>' Style="text-align: left; float: left"></asp:Label>

                                                            </ItemTemplate>
                                                            <ItemStyle Width="30%" />

                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsl" runat="server" Text='<%#Eval("role")%>' Style="text-align: left; float: left"></asp:Label>

                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />

                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                
                                <div class="form-group border-bottom "></div>
                                <div class="form-group row ">
                                    <div class="col-sm-4">

                                    </div>
                                    <div class="col-sm-4">
                                        <div class="btn-group " role="group">
                                            <asp:Button ID="btn_clear" runat="server" Text="Clear" CssClass="btn btn-outline-secondary  " OnClientClick="return detach();" />
                                        <asp:Button ID="btn_up" runat="server" Text="Update" CssClass="btn btn-outline-primary " OnClientClick="return detach();" />
                                        </div> 
                                    </div>
                                </div>

								</div>
							</div>
						</div>
					</div>
		   </form>

	     



</asp:Content>
