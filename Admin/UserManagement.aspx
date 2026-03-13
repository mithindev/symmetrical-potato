<%@ Page Title="User Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="UserManagement.aspx.vb" Inherits="Fiscus.UserManagement" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/daterangepicker.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
    <script src="../js/daterangepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            initDatePickers();
        });

        if (typeof (Sys) !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                initDatePickers();
            });
        }

        function initDatePickers() {
            $('.date-picker').daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                autoUpdateInput: false,
                locale: {
                    format: 'DD-MM-YYYY'
                }
            });

            $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('DD-MM-YYYY'));
            });

            $('.date-picker').on('input', function (e) {
                var val = this.value.replace(/\D/g, '');
                var newVal = "";
                if (val.length > 0) {
                    newVal += val.substring(0, 2);
                    if (val.length > 2) {
                        newVal += "-" + val.substring(2, 4);
                        if (val.length > 4) {
                            newVal += "-" + val.substring(4, 8);
                        }
                    }
                }
                this.value = newVal.substring(0, 10);
            });
        }
    </script>
    <style>
        .um-input {
            border-radius: 8px !important;
            border: 1px solid #E5E7EB !important;
            padding: 8px 12px !important;
            font-size: 14px !important;
            transition: all 0.2s !important;
            font-family: 'Inter', sans-serif !important;
            background-color: #fff !important;
            color: #374151 !important;
            width: 100% !important;
        }
        .um-input:focus {
            border-color: #7C3AED !important;
            box-shadow: 0 0 0 3px rgba(124,58,237,0.15) !important;
            outline: none !important;
            background-color: #fff !important;
        }
        .um-card {
            background: white;
            border-radius: 16px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.05);
            border: none;
            padding: 28px;
            margin-bottom: 20px;
        }
        .um-section-title {
            color: #5B21B6;
            font-weight: 600;
            font-size: 16px;
            margin-bottom: 6px;
            font-family: 'Inter', sans-serif;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        .um-accent-bar {
            background: linear-gradient(90deg, #6D28D9, #2563EB);
            height: 3px;
            width: 40px;
            border-radius: 4px;
            margin-bottom: 20px;
        }
        .um-label {
            font-size: 13px;
            font-weight: 600;
            color: #4B5563;
            font-family: 'Inter', sans-serif;
            margin-bottom: 6px;
            display: block;
        }
        .um-required {
            color: #EF4444;
            margin-left: 2px;
        }
        .um-credentials-box {
            background: #F3F4FF;
            border-radius: 12px;
            padding: 24px;
            border: 1px dashed #C4B5FD;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form runat="server"
        style="background-color: #F3F4FF; font-family: 'Inter', sans-serif; min-height: 100vh; padding: 20px;">
        <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

        <%-- Breadcrumb --%>
        <nav class="page-breadcrumb" style="margin-bottom: 24px;">
            <ol class="breadcrumb" style="background: transparent; padding: 0;">
                <li class="breadcrumb-item"><a href="#" style="color: #6B7280; text-decoration: none;">Admin</a></li>
                <li class="breadcrumb-item active" style="color: #374151; font-weight: 500;">User Management</li>
            </ol>
        </nav>

        <%-- Alert Panel --%>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel ID="alertmsg" runat="server" Visible="false">
                    <div class="alert alert-secondary alert-dismissible fade show" role="alert"
                        style="border-radius: 12px; background: linear-gradient(to right, #ffffff, #F8FAFC); border: 1px solid #E2E8F0; border-left: 4px solid #7C3AED; box-shadow: 0 4px 15px rgba(0,0,0,0.03); display: flex; align-items: center; padding: 12px 40px 12px 20px; position: relative; margin-bottom: 20px;">
                        <div style="display: flex; align-items: center; gap: 12px;">
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                                <circle cx="12" cy="12" r="10"></circle>
                                <line x1="12" y1="16" x2="12" y2="12"></line>
                                <line x1="12" y1="8" x2="12.01" y2="8"></line>
                            </svg>
                            <h5 style="margin: 0;">
                                <asp:Label ID="lblinfo" runat="server" CssClass="text-dark"
                                    style="font-size: 14px; font-weight: 500; font-family: 'Inter', sans-serif;"></asp:Label>
                            </h5>
                        </div>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"
                            style="position: absolute; right: 16px; top: 50%; transform: translateY(-50%); padding: 0; margin: 0; background: none; border: none; opacity: 0.5; transition: opacity 0.2s;"
                            onmouseover="this.style.opacity='1'" onmouseout="this.style.opacity='0.5'">
                            <span aria-hidden="true" style="font-size: 20px; color: #64748B;">&times;</span>
                        </button>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%-- Main Content --%>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <%-- Page Header --%>
                <div style="margin-bottom: 24px;">
                    <h4 style="font-size: 1.4rem; font-weight: 700; color: #0F172A; margin: 0 0 4px 0; font-family: 'Inter', sans-serif; display: flex; align-items: center; gap: 10px;">
                        <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                            <path d="M16 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path>
                            <circle cx="8.5" cy="7" r="4"></circle>
                            <line x1="20" y1="8" x2="20" y2="14"></line>
                            <line x1="23" y1="11" x2="17" y2="11"></line>
                        </svg>
                        Create New User Profile
                    </h4>
                    <p style="margin: 0; color: #6B7280; font-size: 14px; font-family: 'Inter', sans-serif; padding-left: 32px;">
                        Fill in the details accurately to register a new user in the system.
                    </p>
                </div>

                <%-- ROW 1: Personal Info + Contact Info --%>
                <div class="row" style="margin-bottom: 0;">

                    <%-- Personal Information Card --%>
                    <div class="col-md-6" style="margin-bottom: 20px;">
                        <div class="um-card" style="height: 100%;">
                            <h6 class="um-section-title">
                                <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path><circle cx="12" cy="7" r="4"></circle></svg>
                                Personal Information
                            </h6>
                            <div class="um-accent-bar"></div>

                            <div class="row">
                                <div class="col-md-6 form-group" style="margin-bottom: 16px;">
                                    <label for="txtfn" class="um-label">First Name <span class="um-required">*</span></label>
                                    <asp:TextBox CssClass="form-control um-input" runat="server" ID="txtfn"
                                        placeholder="First name"
                                        onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                        onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                    </asp:TextBox>
                                </div>
                                <div class="col-md-6 form-group" style="margin-bottom: 16px;">
                                    <label for="txtln" class="um-label">Last Name</label>
                                    <asp:TextBox CssClass="form-control um-input" runat="server" ID="txtln"
                                        placeholder="Last name"
                                        onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                        onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                    </asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 form-group" style="margin-bottom: 16px;">
                                    <label for="gender" class="um-label">Gender <span class="um-required">*</span></label>
                                    <asp:DropDownList runat="server" ID="gender" CssClass="form-control um-input"
                                        onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                        onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                        <asp:ListItem Value=""><-Select-></asp:ListItem>
                                        <asp:ListItem>Male</asp:ListItem>
                                        <asp:ListItem>FeMale</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 form-group" style="margin-bottom: 0;">
                                    <label for="txtdob" class="um-label">Date of Birth</label>
                                    <asp:TextBox ID="txtdob" runat="server" CssClass="form-control um-input date-picker"
                                        placeholder="DD-MM-YYYY"
                                        onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                        onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%-- Contact Information Card --%>
                    <div class="col-md-6" style="margin-bottom: 20px;">
                        <div class="um-card" style="height: 100%;">
                            <h6 class="um-section-title">
                                <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"></path></svg>
                                Contact Information
                            </h6>
                            <div class="um-accent-bar"></div>

                            <div class="form-group" style="margin-bottom: 16px;">
                                <label for="txtmobile" class="um-label">Mobile No <span class="um-required">*</span></label>
                                 <asp:TextBox CssClass="form-control um-input" runat="server" ID="txtmobile"
                                    placeholder="10-digit mobile number"
                                    onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                    onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                </asp:TextBox>
                            </div>

                            <div class="form-group" style="margin-bottom: 16px;">
                                <label for="txtemail" class="um-label">Email Address <span class="um-required">*</span></label>
                                 <asp:TextBox CssClass="form-control um-input" runat="server" ID="txtemail"
                                    placeholder="example@email.com"
                                    onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                    onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                </asp:TextBox>
                            </div>

                            <div class="form-group" style="margin-bottom: 0;">
                                <label for="txtadd" class="um-label">Residential Address</label>
                                <asp:TextBox TextMode="MultiLine" Height="72px" runat="server" ID="txtadd"
                                    CssClass="form-control um-input"
                                    placeholder="Full address..."
                                    style="resize: vertical;"
                                    onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                    onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>

                <%-- ROW 2: Employment + Credentials --%>
                <div class="row">

                    <%-- Employment Information Card --%>
                    <div class="col-md-6" style="margin-bottom: 20px;">
                        <div class="um-card" style="height: 100%;">
                            <h6 class="um-section-title">
                                <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="2" y="7" width="20" height="14" rx="2" ry="2"></rect><path d="M16 21V5a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16"></path></svg>
                                Employment Details
                            </h6>
                            <div class="um-accent-bar"></div>

                            <div class="form-group" style="margin-bottom: 16px;">
                                <label for="txtdesi" class="um-label">Designation <span class="um-required">*</span></label>
                                <asp:DropDownList CssClass="form-control um-input" runat="server" ID="txtdesi"
                                    onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                    onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                    <asp:ListItem Value=""><-Select-></asp:ListItem>
                                    <asp:ListItem>Cashier</asp:ListItem>
                                    <asp:ListItem>Clerk</asp:ListItem>
                                    <asp:ListItem>Executive</asp:ListItem>
                                    <asp:ListItem>Manager</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group" style="margin-bottom: 0;">
                                <label for="txtdoj" class="um-label">Joining Date</label>
                                <asp:TextBox CssClass="form-control um-input date-picker" runat="server" ID="txtdoj"
                                    placeholder="DD-MM-YYYY"
                                    onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                    onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <%-- System Credentials Card --%>
                    <div class="col-md-6" style="margin-bottom: 20px;">
                        <div class="um-card" style="height: 100%;">
                            <h6 class="um-section-title">
                                <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect><path d="M7 11V7a5 5 0 0 1 10 0v4"></path></svg>
                                System Credentials
                            </h6>
                            <div class="um-accent-bar"></div>

                            <div class="um-credentials-box">
                                <div class="form-group" style="margin-bottom: 16px;">
                                    <label for="txtuser" class="um-label">Username <span class="um-required">*</span></label>
                                    <asp:TextBox CssClass="form-control um-input" runat="server" ID="txtuser"
                                        placeholder="Choose a username"
                                        onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                        onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                    </asp:TextBox>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 form-group" style="margin-bottom: 0;">
                                        <label for="txtpass" class="um-label">Password <span class="um-required">*</span></label>
                                        <asp:TextBox CssClass="form-control um-input" TextMode="Password" runat="server" ID="txtpass"
                                            placeholder="Password"
                                            onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                            onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-6 form-group" style="margin-bottom: 0;">
                                        <label for="txtcpass" class="um-label">Confirm Password <span class="um-required">*</span></label>
                                        <asp:TextBox CssClass="form-control um-input" TextMode="Password" runat="server" ID="txtcpass"
                                            placeholder="Re-enter password"
                                            onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                            onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <%-- Action Buttons --%>
                <div style="border-top: 1px solid #E5E7EB; padding-top: 20px; margin-top: 4px; display: flex; justify-content: flex-end; gap: 12px;">
                    <asp:Button runat="server" ID="btnclr" CssClass="btn" Text="Clear Form"
                        style="border-radius: 10px; padding: 10px 28px; font-weight: 500; font-family: 'Inter', sans-serif; background: transparent; border: 1px solid #D1D5DB; color: #4B5563; transition: all 0.2s;"
                        onmouseover="this.style.background='#F3F4F6'; this.style.transform='translateY(-1px)';"
                        onmouseout="this.style.background='transparent'; this.style.transform='translateY(0)';" />
                    <asp:Button runat="server" ID="btnupdate" CssClass="btn" Text="Save User"
                        style="background: linear-gradient(135deg, #7C3AED, #2563EB); color: white; border-radius: 10px; padding: 10px 28px; border: none; font-weight: 500; font-family: 'Inter', sans-serif; transition: all 0.2s; box-shadow: 0 4px 14px rgba(37,99,235,0.2);"
                        onmouseover="this.style.transform='translateY(-2px)'; this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
                        onmouseout="this.style.transform='translateY(0)'; this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

</asp:Content>
