<%@ Page Title="Deposit Opening" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="DepositOpening.aspx.vb" Inherits="Fiscus.DepositOpening" EnableEventValidation="true" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link rel="stylesheet" type="text/css" href="css/smart_tab.min.css" />
        <link href="css/daterangepicker.css" rel="stylesheet" />
        <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
        <style>
            /* ── Design system ─────────────────────────────────── */
            .do-page { background-color:#F3F4FF; font-family:'Inter',sans-serif; min-height:100vh; padding:20px; }

            .do-card {
                background:white; border-radius:16px;
                box-shadow:0 10px 30px rgba(0,0,0,0.05);
                border:none; padding:26px; margin-bottom:20px;
            }
            .do-card-title {
                color:#5B21B6; font-weight:600; font-size:16px;
                margin-bottom:6px; font-family:'Inter',sans-serif;
                display:flex; align-items:center; gap:9px;
            }
            .do-accent-bar {
                background:linear-gradient(90deg,#6D28D9,#2563EB);
                height:3px; width:40px; border-radius:4px; margin-bottom:20px;
            }
            .do-label {
                font-size:13px; font-weight:600; color:#4B5563;
                font-family:'Inter',sans-serif; margin-bottom:6px; display:block;
            }
            .do-input {
                border-radius:8px !important; border:1px solid #E5E7EB !important;
                padding:7px 12px !important; font-size:14px !important;
                transition:all 0.2s !important; font-family:'Inter',sans-serif !important;
                width:100% !important; color:#374151 !important; background:#fff !important;
            }
            .do-input:focus {
                border-color:#7C3AED !important;
                box-shadow:0 0 0 3px rgba(124,58,237,0.15) !important;
                outline:none !important;
            }
            .do-input[readonly], .do-input[disabled] {
                background:#F9FAFB !important; color:#6B7280 !important;
            }
            .do-select {
                border-radius:8px !important; border:1px solid #E5E7EB !important;
                padding:7px 10px !important; font-size:14px !important;
                transition:all 0.2s !important; font-family:'Inter',sans-serif !important;
                width:100% !important; color:#374151 !important; background:#fff !important;
            }
            .do-select:focus {
                border-color:#7C3AED !important;
                box-shadow:0 0 0 3px rgba(124,58,237,0.15) !important;
                outline:none !important;
            }
            .do-sub-box {
                background:#F3F4FF; border-radius:12px;
                border:1px dashed #C4B5FD; padding:20px; margin-top:4px;
            }
            .do-sub-title {
                font-size:13px; font-weight:700; color:#5B21B6;
                font-family:'Inter',sans-serif; margin-bottom:14px;
                display:flex; align-items:center; gap:7px;
            }

            /* ── SmartTab override ─────────────────────────────── */
            #smarttab .nav { border-bottom:2px solid #E5E7EB; gap:4px; padding-bottom:0; flex-wrap:wrap; }
            #smarttab .nav li a.nav-link {
                border-radius:8px 8px 0 0 !important;
                padding:9px 20px !important;
                font-size:13px !important; font-weight:600 !important;
                color:#6B7280 !important; font-family:'Inter',sans-serif !important;
                border:1px solid transparent !important;
                transition:all 0.2s !important; background:transparent !important;
            }
            #smarttab .nav li a.nav-link.active,
            #smarttab .nav li a.nav-link[aria-selected="true"] {
                color:#7C3AED !important;
                border-color:#E5E7EB #E5E7EB white !important;
                background:white !important;
                border-bottom-color:white !important;
            }
            #smarttab .nav li a.nav-link:hover { color:#7C3AED !important; background:#F5F3FF !important; }
            #smarttab .tab-content { padding:24px 0 0 0 !important; }

            /* ── Plan circular checkboxes ───────────────────────── */
            .plan-checks { display:flex; gap:10px; flex-wrap:wrap; margin-top:4px; }

            /* Hide the native input but keep it in the DOM for ASP.NET */
            .plan-checks input[type="checkbox"] { display:none !important; }

            /* The pill label */
            .plan-checks label {
                display:inline-flex;
                align-items:center;
                gap:8px;
                padding:7px 16px 7px 10px;
                border-radius:999px;
                border:2px solid #D1FAE5;
                background:#F0FDF4;
                color:#374151;
                font-size:13px;
                font-weight:600;
                font-family:'Inter',sans-serif;
                cursor:pointer;
                transition:all 0.2s;
                user-select:none;
            }
            .plan-checks label:hover {
                border-color:#34D399;
                background:#ECFDF5;
            }

            /* The circular indicator — unchecked state */
            .plan-checks label::before {
                content:'';
                display:inline-block;
                width:18px;
                height:18px;
                border-radius:50%;
                border:2px solid #A7F3D0;
                background:white;
                flex-shrink:0;
                transition:all 0.2s;
            }

            /* Checked state: green fill + white tick via background-image */
            .plan-checks input[type="checkbox"]:checked + label {
                border-color:#059669;
                background:#D1FAE5;
                color:#065F46;
            }
            .plan-checks input[type="checkbox"]:checked + label::before {
                background:#059669;
                border-color:#059669;
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 12 12'%3E%3Cpath d='M2 6l3 3 5-5' stroke='white' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' fill='none'/%3E%3C/svg%3E");
                background-repeat:no-repeat;
                background-position:center;
                background-size:10px 10px;
            }

            /* ── Autocomplete styles (unchanged IDs) ───────────── */
            .product { z-index:10; position:absolute; right:10px; top:21px; font-size:12px; }
            .memno { z-index:10; position:absolute; right:10px; top:10px; font-size:12px; color:#000; }
            .img-div { display:inline-block; vertical-align:top; }
            .info-div { display:inline-block; }
            .username { display:inline-block; font-weight:bold; margin-bottom:0em; }
            .userimage { float:left; max-height:48px; max-width:48px; margin-right:10px; }
            .userinfo { margin:0; padding:0; font-size:10px; }
            .ui-autocomplete { max-height:510px; overflow-y:auto; overflow-x:hidden; }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form id="newDeposit" runat="server" class="do-page">

            <asp:ScriptManager ID="SM3" runat="server"></asp:ScriptManager>

            <%-- Breadcrumb --%>
            <nav class="page-breadcrumb" style="margin-bottom:24px;">
                <ol class="breadcrumb" style="background:transparent;padding:0;">
                    <li class="breadcrumb-item"><a href="#" style="color:#6B7280;text-decoration:none;">Deposit</a></li>
                    <li class="breadcrumb-item active" style="color:#374151;font-weight:500;">Account Opening</li>
                </ol>
            </nav>

            <%-- Page header --%>
            <div style="margin-bottom:22px;">
                <h4 style="font-size:1.3rem;font-weight:700;color:#0F172A;margin:0 0 4px 0;font-family:'Inter',sans-serif;display:flex;align-items:center;gap:10px;">
                    <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <rect x="2" y="5" width="20" height="14" rx="2"></rect><line x1="2" y1="10" x2="22" y2="10"></line>
                    </svg>
                    Deposit Account Opening
                </h4>
                <p style="margin:0;color:#6B7280;font-size:14px;font-family:'Inter',sans-serif;padding-left:32px;">
                    Fill all three tabs to open a new deposit account.
                </p>
            </div>

            <%-- ══ SmartTab wrapper ══ --%>
            <div class="do-card" style="padding:24px;">
                <div id="smarttab">

                    <%-- Tab navigation --%>
                    <ul class="nav" id="tabitm">
                        <li><a class="nav-link" href="#tab-1">Customer Specifics</a></li>
                        <li><a class="nav-link" href="#tab-2">Nomination</a></li>
                        <li><a class="nav-link" href="#tab-3">Standing Instruction</a></li>
                    </ul>

                    <div class="tab-content">

                        <%-- ══════════════════════════════════ TAB 1 ══ --%>
                        <div id="tab-1" class="tab-pane" role="tabpanel">
                            <asp:UpdatePanel runat="server" ID="tabUP">
                                <ContentTemplate>

                                    <div class="row" style="margin-top:8px;">

                                        <%-- Left column: Customer info --%>
                                        <div class="col-md-6" style="margin-bottom:16px;">
                                            <h6 class="do-card-title">
                                                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path><circle cx="12" cy="7" r="4"></circle></svg>
                                                Customer Details
                                            </h6>
                                            <div class="do-accent-bar"></div>

                                            <div class="form-group" style="margin-bottom:14px;">
                                                <label class="do-label">Customer Id</label>
                                                <asp:UpdatePanel runat="server" id="upcid">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtcid" runat="server"
                                                            CssClass="form-control do-input" AutoCompleteType="None"
                                                            AutoPostBack="True"
                                                            data-validation-engine="validate[required]"
                                                            data-errormessage-value-missing="Valid Customer Id is required!"
                                                            onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                            onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                        </asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <div class="form-group" style="margin-bottom:14px;">
                                                <label class="do-label">Customer Name</label>
                                                <div class="row" style="margin:0;gap:8px;flex-wrap:nowrap;">
                                                    <div style="flex:1;padding:0;">
                                                        <asp:TextBox ID="txtname" CssClass="form-control do-input" ReadOnly="true" runat="server"
                                                            data-validation-engine="validate[required]"
                                                            data-errormessage-value-missing="Customer Name Missing">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div style="flex:1;padding:0;">
                                                        <asp:TextBox ID="txtcof" CssClass="form-control do-input" ReadOnly="true" runat="server"
                                                            data-validation-engine="validate[required]">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group" style="margin-bottom:0;">
                                                <label class="do-label">Address</label>
                                                <div style="display:flex;align-items:flex-start;gap:14px;">
                                                    <asp:TextBox ID="txtadd" CssClass="form-control do-input" ReadOnly="true"
                                                        runat="server" Height="80px" TextMode="MultiLine" style="flex:1;resize:none;">
                                                    </asp:TextBox>
                                                    <asp:Image id="imgCapture" runat="server" Visible="false"
                                                        Width="80px" Height="80px" style="border-radius:8px;border:1px solid #E5E7EB;flex-shrink:0;" />
                                                </div>
                                            </div>
                                        </div>

                                        <%-- Right column: Deposit details --%>
                                        <div class="col-md-6" style="margin-bottom:16px;">
                                            <h6 class="do-card-title">
                                                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="2" y="5" width="20" height="14" rx="2"></rect><line x1="2" y1="10" x2="22" y2="10"></line></svg>
                                                Deposit Details
                                            </h6>
                                            <div class="do-accent-bar"></div>

                                            <%-- Deposit type --%>
                                            <div class="form-group" style="margin-bottom:14px;">
                                                <label class="do-label">Deposit Type</label>
                                                <asp:UpdatePanel ID="dept" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="deptyp" AutoPostBack="True" runat="server"
                                                            CssClass="form-control do-select"
                                                            data-validation-engine="validate[required]"
                                                            data-errormessage-value-missing="Select a Product!"
                                                            onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                            onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <%-- Date + Renew toggle --%>
                                            <div class="form-group" style="margin-bottom:14px;">
                                                <label class="do-label">Date (W.E.F)</label>
                                                <div style="display:flex;align-items:center;gap:10px;">
                                                    <asp:TextBox ID="ddt" runat="server" CssClass="form-control do-input" style="flex:1;"
                                                        data-validation-engine="validate[required,funcCall[DateFormat[]"
                                                        data-errormessage-value-missing="Date Missing"
                                                        onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                        onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                    </asp:TextBox>
                                                    <asp:UpdatePanel runat="server" style="display:none;">
                                                        <ContentTemplate>
                                                            <div style="display:flex;align-items:center;gap:6px;white-space:nowrap;">
                                                                <asp:CheckBox runat="server" ID="isrenew" AutoPostBack="true" />
                                                                <span style="font-size:13px;font-weight:500;color:#4B5563;font-family:'Inter',sans-serif;">Renew</span>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:UpdatePanel runat="server" id="up_trf_renew" style="display:inline;">
                                                        <ContentTemplate>
                                                            <asp:TextBox runat="server" ID="txtrenewacn"
                                                                CssClass="form-control do-input" AutoPostBack="true"
                                                                Visible="false" style="width:130px;">
                                                            </asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>

                                            <%-- Plan checkboxes — custom green circular style --%>
                                            <div class="form-group" style="margin-bottom:14px;">
                                                <label class="do-label">Plan</label>
                                                <div class="plan-checks">
                                                    <asp:CheckBox ID="chkRenewPlan" runat="server" Text="Renew" AutoPostBack="true" />
                                                    <asp:CheckBox ID="chkSeniorPlan" runat="server" Text="Senior" />
                                                    <asp:CheckBox ID="chkTransferPlan" runat="server" Text="Transfer" />
                                                    <asp:CheckBox ID="chkRegularPlan" runat="server" Text="Regular" />
                                                </div>
                                            </div>

                                            <%-- Period --%>
                                            <div class="form-group" style="margin-bottom:14px;">
                                                <label class="do-label">Period (In Months)</label>
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="prd" runat="server"
                                                            CssClass="form-control do-input" AutoPostBack="true"
                                                            data-validation-engine="validate[required,custom[integer]"
                                                            data-errormessage-value-missing="Period Missing"
                                                            data-errormessage-custom-error="Enter a Valid Period"
                                                            onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                            onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                        </asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <%-- Interest rate boxes --%>
                                            <div class="form-group" style="margin-bottom:0;">
                                                <label class="do-label">Interest</label>
                                                <div style="display:flex;gap:10px;">
                                                    <asp:TextBox ID="cintr" CssClass="form-control do-input prd" ReadOnly="true" runat="server"
                                                        data-validation-engine="validate[required,funcCall[chkdecim[]"
                                                        data-errormessage-value-missing="INTEREST Missing">
                                                    </asp:TextBox>
                                                    <asp:TextBox ID="dint" CssClass="form-control do-input prd" ReadOnly="true" runat="server"
                                                        data-validation-engine="validate[required,funcCall[chkdecim[]"
                                                        data-errormessage-value-missing="INTEREST Missing">
                                                    </asp:TextBox>
                                                </div>
                                                <asp:Panel runat="server" ID="pnlsch" style="display:none"></asp:Panel>
                                            </div>

                                        </div>
                                    </div>

                                    <%-- Tab 1 buttons --%>
                                    <div style="border-top:1px solid #E5E7EB;padding-top:18px;margin-top:8px;display:flex;justify-content:flex-end;gap:12px;">
                                        <asp:Button ID="btnclr" CssClass="btn" runat="server" Text="Clear" CausesValidation="False"
                                            style="border-radius:10px;padding:9px 26px;font-weight:500;font-family:'Inter',sans-serif;background:transparent;border:1px solid #D1D5DB;color:#4B5563;transition:all 0.2s;"
                                            onmouseover="this.style.background='#F3F4F6';this.style.transform='translateY(-1px)';"
                                            onmouseout="this.style.background='transparent';this.style.transform='translateY(0)';" />
                                        <asp:Button ID="btnsave" CssClass="btn" runat="server" Text="Save"
                                            style="background:linear-gradient(135deg,#7C3AED,#2563EB);color:white;border-radius:10px;padding:9px 26px;border:none;font-weight:500;font-family:'Inter',sans-serif;transition:all 0.2s;box-shadow:0 4px 14px rgba(37,99,235,0.2);"
                                            onmouseover="this.style.transform='translateY(-2px)';this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
                                            onmouseout="this.style.transform='translateY(0)';this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';" />
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div><%-- /tab-1 --%>

                        <%-- ══════════════════════════════════ TAB 2 ══ --%>
                        <div id="tab-2" class="tab-pane" role="tabpanel">
                            <asp:UpdatePanel ID="tab2up" runat="server">
                                <ContentTemplate>

                                    <div style="margin-top:8px;">
                                        <h6 class="do-card-title">
                                            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>
                                            Nomination Details
                                        </h6>
                                        <div class="do-accent-bar"></div>
                                    </div>

                                    <%-- Group Id --%>
                                    <div class="form-group row" style="margin-bottom:14px;">
                                        <div class="col-md-4">
                                            <label class="do-label">Group Id</label>
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtgrpid" runat="server"
                                                        CssClass="form-control do-input" AutoPostBack="true"
                                                        data-validation-engine="validate[required]"
                                                        data-errormessage-value-missing="Group id required!"
                                                        onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                        onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                    </asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-4" style="padding-top:30px;">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <label id="lblgrp" runat="server"
                                                        style="font-size:13px;font-variant-caps:small-caps;color:#5B21B6;font-weight:600;font-family:'Inter',sans-serif;"></label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <%-- Nominee + Address --%>
                                    <div class="row" style="margin-bottom:14px;">
                                        <div class="col-md-6">
                                            <label class="do-label">Nominee Name</label>
                                            <asp:TextBox ID="txtnominee" runat="server" CssClass="form-control do-input"
                                                onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                            </asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <label class="do-label">Nominee Address</label>
                                            <asp:TextBox ID="txtnadd" CssClass="form-control do-input" runat="server"
                                                Height="80px" TextMode="MultiLine" style="resize:none;"
                                                onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <%-- Relationship + Age --%>
                                    <div class="row" style="margin-bottom:20px;">
                                        <div class="col-md-6">
                                            <label class="do-label">Relationship</label>
                                            <asp:TextBox ID="txtrelation" CssClass="form-control do-input" runat="server"
                                                onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                            </asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <label class="do-label">Age (In Years)</label>
                                            <asp:TextBox ID="txtage" CssClass="form-control do-input" runat="server"
                                                onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <%-- If Nominee is Minor --%>
                                    <div class="do-sub-box">
                                        <div class="do-sub-title">
                                            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"></circle><line x1="12" y1="8" x2="12" y2="12"></line><line x1="12" y1="16" x2="12.01" y2="16"></line></svg>
                                            If Nominee is Minor
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label class="do-label">Date of Birth</label>
                                                <asp:TextBox ID="txtdob" CssClass="form-control do-input" runat="server"
                                                    onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                    onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                </asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <label class="do-label">Guardian Name</label>
                                                <asp:TextBox ID="txtgurd" CssClass="form-control do-input" runat="server"
                                                    onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                    onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                </asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <label class="do-label">Guardian Address</label>
                                                <asp:TextBox ID="txtgrdadd" CssClass="form-control do-input" runat="server"
                                                    Height="80px" TextMode="MultiLine" style="resize:none;"
                                                    onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                    onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- UpdateProgress kept exactly as-is --%>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnldsbtn">
                                        <ProgressTemplate>
                                            <div id="blocker">
                                                <div style="margin:0 auto">
                                                    <div class="sk-circle">
                                                        <div class="sk-circle1 sk-child"></div><div class="sk-circle2 sk-child"></div>
                                                        <div class="sk-circle3 sk-child"></div><div class="sk-circle4 sk-child"></div>
                                                        <div class="sk-circle5 sk-child"></div><div class="sk-circle6 sk-child"></div>
                                                        <div class="sk-circle7 sk-child"></div><div class="sk-circle8 sk-child"></div>
                                                        <div class="sk-circle9 sk-child"></div><div class="sk-circle10 sk-child"></div>
                                                        <div class="sk-circle11 sk-child"></div><div class="sk-circle12 sk-child"></div>
                                                    </div>
                                                    <br />Processing.....
                                                </div>
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                    <asp:UpdatePanel runat="server" ID="pnldsbtn">
                                        <ContentTemplate>
                                            <div style="border-top:1px solid #E5E7EB;padding-top:18px;margin-top:20px;display:flex;justify-content:flex-end;gap:12px;">
                                                <asp:Button ID="btn_da1_clr" CssClass="btn" runat="server" Text="Clear" CausesValidation="False"
                                                    style="border-radius:10px;padding:9px 26px;font-weight:500;font-family:'Inter',sans-serif;background:transparent;border:1px solid #D1D5DB;color:#4B5563;transition:all 0.2s;"
                                                    onmouseover="this.style.background='#F3F4F6';this.style.transform='translateY(-1px)';"
                                                    onmouseout="this.style.background='transparent';this.style.transform='translateY(0)';" />
                                                <asp:Button ID="btn_da1_nxt" CssClass="btn" runat="server" Text="Save" CausesValidation="false"
                                                    style="background:linear-gradient(135deg,#7C3AED,#2563EB);color:white;border-radius:10px;padding:9px 26px;border:none;font-weight:500;font-family:'Inter',sans-serif;transition:all 0.2s;box-shadow:0 4px 14px rgba(37,99,235,0.2);"
                                                    onmouseover="this.style.transform='translateY(-2px)';this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
                                                    onmouseout="this.style.transform='translateY(0)';this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div><%-- /tab-2 --%>

                        <%-- ══════════════════════════════════ TAB 3 ══ --%>
                        <div id="tab-3" class="tab-pane" role="tabpanel">
                            <asp:UpdatePanel runat="server" ID="tab3up">
                                <ContentTemplate>

                                    <div style="margin-top:8px;">
                                        <h6 class="do-card-title">
                                            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path><polyline points="22 4 12 14.01 9 11.01"></polyline></svg>
                                            Standing Instruction
                                        </h6>
                                        <div class="do-accent-bar"></div>
                                    </div>

                                    <div class="row" style="margin-bottom:14px;">
                                        <div class="col-md-5">
                                            <label class="do-label">Account No</label>
                                            <asp:UpdatePanel runat="server" ID="uptxtsi">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtsiacn" runat="server"
                                                        CssClass="form-control do-input" AutoPostBack="True"
                                                        onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                        onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                    </asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-5" style="padding-top:30px;">
                                            <asp:UpdatePanel runat="server" ID="upcust">
                                                <ContentTemplate>
                                                    <asp:Label ID="cust" runat="server"
                                                        style="font-size:14px;font-weight:600;color:#5B21B6;font-family:'Inter',sans-serif;">
                                                    </asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <%-- OTP panel - hidden by default, shown by backend --%>
                                    <asp:Panel runat="server" ID="pnlotp" Visible="false">
                                        <div class="row" style="margin-bottom:14px;">
                                            <div class="col-md-4">
                                                <label class="do-label">OTP</label>
                                                <asp:UpdatePanel runat="server" ID="upotp">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtotp" runat="server"
                                                            CssClass="form-control do-input" AutoPostBack="True"
                                                            onfocus="this.style.borderColor='#7C3AED';this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                            onblur="this.style.borderColor='#E5E7EB';this.style.boxShadow='none';">
                                                        </asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%-- UpdateProgress kept exactly as-is --%>
                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlbtn">
                                        <ProgressTemplate>
                                            <div id="blocker">
                                                <div style="margin:0 auto">
                                                    <div class="sk-circle">
                                                        <div class="sk-circle1 sk-child"></div><div class="sk-circle2 sk-child"></div>
                                                        <div class="sk-circle3 sk-child"></div><div class="sk-circle4 sk-child"></div>
                                                        <div class="sk-circle5 sk-child"></div><div class="sk-circle6 sk-child"></div>
                                                        <div class="sk-circle7 sk-child"></div><div class="sk-circle8 sk-child"></div>
                                                        <div class="sk-circle9 sk-child"></div><div class="sk-circle10 sk-child"></div>
                                                        <div class="sk-circle11 sk-child"></div><div class="sk-circle12 sk-child"></div>
                                                    </div>
                                                    <br />Processing.....
                                                </div>
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                    <asp:UpdatePanel runat="server" ID="pnlbtn">
                                        <ContentTemplate>
                                            <div style="border-top:1px solid #E5E7EB;padding-top:18px;margin-top:8px;display:flex;justify-content:flex-end;gap:12px;">
                                                <asp:Button ID="btn_si_upclr" CssClass="btn" runat="server" Text="Clear" CausesValidation="False"
                                                    style="border-radius:10px;padding:9px 26px;font-weight:500;font-family:'Inter',sans-serif;background:transparent;border:1px solid #D1D5DB;color:#4B5563;transition:all 0.2s;"
                                                    onmouseover="this.style.background='#F3F4F6';this.style.transform='translateY(-1px)';"
                                                    onmouseout="this.style.background='transparent';this.style.transform='translateY(0)';" />
                                                <asp:Button ID="btn_si_update" CssClass="btn" runat="server" Text="Save"
                                                    style="background:linear-gradient(135deg,#7C3AED,#2563EB);color:white;border-radius:10px;padding:9px 26px;border:none;font-weight:500;font-family:'Inter',sans-serif;transition:all 0.2s;box-shadow:0 4px 14px rgba(37,99,235,0.2);"
                                                    onmouseover="this.style.transform='translateY(-2px)';this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
                                                    onmouseout="this.style.transform='translateY(0)';this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div><%-- /tab-3 --%>

                    </div><%-- /tab-content --%>
                </div><%-- /smarttab --%>
            </div><%-- /do-card wrapper --%>

        </form>

        <%-- ══ Scripts — completely unchanged from original ══ --%>
        <script src="js/jquery.js" type="text/javascript"></script>
        <script src="js/daterangepicker.js" type="text/javascript"></script>
        <script src="js/jquery.smartTab.min.js" type="text/javascript"></script>
        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />

        <script type="text/javascript">

            $(document).ready(function () {

                $('#smarttab').smartTab({
                    selected: 0,
                    orientation: 'horizontal',
                    justified: false,
                    autoAdjustHeight: false,
                    backButtonSupport: false,
                    enableURLhash: true,
                    transition: { animation: 'none', speed: '400', easing: '' },
                    keyboardSettings: { keyNavigation: false, keyLeft: [37], keyRight: [39] }
                });

                $("#smarttab").on("showTab", function (e, anchorObject, tabIndex) {
                    switch (tabIndex) {
                        case 0:
                            setTimeout(function () { document.getElementById('<%=txtcid.ClientID %>').focus(); }, 200);
                            return true;
                            break;
                        case 1:
                            setTimeout(function () { document.getElementById('<%=txtgrpid.ClientID %>').focus(); }, 500);
                            break;
                        case 2:
                            setTimeout(function () { document.getElementById('<%=txtsiacn.ClientID %>').focus(); }, 500);
                            break;
                    }
                });

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);

                ShowCTab();
                InitAutoCompl();
                InitAutoComplG();
                InitAutoComplSB();

                setTimeout(function () { document.getElementById('<%=txtcid.ClientID %>').focus(); }, 200);
                $("html, body").animate({ scrollTop: 0 }, "fast");
            });

            function InitializeRequest(sender, args) { }

            function EndRequest(sender, args) {
                InitAutoCompl();
                InitAutoComplG();
                InitAutoComplSB();
            }

            function InitAutoComplG() {
                $("#<%=txtgrpid.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/membersearch.asmx/GetUsers") %>',
                            data: "{ 'input': '" + request.term + "'}",
                            dataType: "json", type: "POST", contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return { memberno: item.Memberno, firstname: item.Firstname, lastname: item.Lastname, address: item.Address, image: item.ImageURL }
                                }))
                            }
                        });
                    },
                    minLength: 1,
                    select: function (event, ui) { $("#<%=txtgrpid.ClientID %>").val(ui.item.memberno); return false; }
                }).autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>").append("<div style='container'>").append("<div class='img-div column'>")
                        .append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
                        .append("</div").append("<div class='info-div column'>")
                        .append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
                        .append("<div class='userinfo'>" + item.lastname + "</div>")
                        .append("<div class='userinfo'>" + item.address + "</div>")
                        .append("</br>").append("</div").append("</div>").appendTo(ul);
                };
            }

            function InitAutoCompl() {
                $("#<%=txtcid.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/membersearch.asmx/GetUsers") %>',
                            data: "{ 'input': '" + request.term + "'}",
                            dataType: "json", type: "POST", contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return { memberno: item.Memberno, firstname: item.Firstname, lastname: item.Lastname, address: item.Address, image: item.ImageURL }
                                }))
                            }
                        });
                    },
                    minLength: 1,
                    open: function () { $("ul.ui-menu").width($(this).innerWidth() + 180); },
                    select: function (event, ui) { $("#<%=txtcid.ClientID %>").val(ui.item.memberno); return false; }
                }).autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>").append("<div style='container'>").append("<div class='img-div column'>")
                        .append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
                        .append("</div").append("<div class='info-div column'>")
                        .append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
                        .append("<div class='userinfo'>" + item.lastname + "</div>")
                        .append("<div class='userinfo'>" + item.address + "</div>")
                        .append("</br>").append("</div").append("</div>").appendTo(ul);
                };
            }

            function InitAutoComplSB() {
                $("#<%=txtsiacn.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/membersearch.asmx/GetSbAc") %>',
                            data: "{ 'input': '" + request.term + "'}",
                            dataType: "json", type: "POST", contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return { memberno: item.Memberno, firstname: item.Firstname, lastname: item.Lastname, address: item.Address, image: item.ImageURL }
                                }))
                            }
                        });
                    },
                    minLength: 1,
                    focus: function (event, ui) { $("#<%=txtsiacn.ClientID %>").val(ui.item.memberno); return false; },
                    open: function () { $("ul.ui-menu").width($(this).innerWidth() + 210); },
                    select: function (event, ui) {
                        $("#<%=txtsiacn.ClientID %>").val(ui.item.memberno);
                        $("#<%=cust.ClientID %>").text(ui.item.firstname);
                        return false;
                    }
                }).autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>").append("<div style='container'>").append("<div class='img-div column'>")
                        .append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
                        .append("</div").append("<div class='info-div column'>")
                        .append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
                        .append("<div class='userinfo'>" + item.lastname + "</div>")
                        .append("<div class='userinfo'>" + item.address + "</div>")
                        .append("</br>").append("</div").append("</div>").appendTo(ul);
                };
            }

            function ShowNTab() {
                document.getElementById("tabitm").children[1].style.display = "block";
                document.getElementById("tabitm").children[2].style.display = "none";
            };

            function ShowCTab() {
                document.getElementById("tabitm").children[1].style.display = "none";
                document.getElementById("tabitm").children[2].style.display = "none";
                ShowDLS(0);
            };

            function ShowSTab() {
                document.getElementById("tabitm").children[2].style.display = "block";
            };

            function ShowDLS(tabin) {
                $('#smarttab').smartTab("goToTab", tabin);
                return false;
            };

        </script>

    </asp:Content>