<%@ Page Title="Share Allocation" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="ShareAllocation.aspx.vb" Inherits="Fiscus.ShareAllocation" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="css/daterangepicker.css" rel="stylesheet" />
        <link href="css/ValidationEngine.css" rel="stylesheet" />
        <link href="css/validationEngine.jquery.css" rel="stylesheet" />
        <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
        <style>
            @media print {
                html, body { height: 90%; }
                @page {
                    size: A4;
                    margin: 7mm;
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif
                }
                .outert { border: solid #000 !important; border-width: 1px 1px 1px 1px !important; }
                .prntarea { height: 207mm; width: 210.0mm; margin: 7mm; }
                table.KKK { width: 567px; background-color: #ffffff; border-collapse: collapse; border-width: 1px; border-color: #c0c0c0; border-style: solid; color: #000000; margin-left: 0px; }
                table.KKK td { height: 35px; text-align: center; border-width: 1px; border-color: #c0c0c0; border-style: solid; padding-right: 5px; }
            }
            @media screen {
                .prntarea { height: 205mm; width: 185mm; margin: 5mm; }
                table.KKK { width: 567px; background-color: #ffffff; border-collapse: collapse; border-width: 1px; border-color: #c0c0c0; border-style: solid; color: #000000; margin-left: 0px; }
                table.KKK td { height: 25px; text-align: center; border-width: 1px; border-color: #c0c0c0; border-style: solid; padding-right: 5px; }
            }
            .shr-input {
                border-radius: 8px !important;
                border: 1px solid #E5E7EB !important;
                padding: 6px 12px !important;
                font-size: 14px !important;
                transition: all 0.2s !important;
                font-family: 'Inter', sans-serif !important;
            }
            .shr-input:focus {
                border-color: #7C3AED !important;
                box-shadow: 0 0 0 3px rgba(124,58,237,0.15) !important;
                outline: none !important;
            }
            .shr-select {
                border-radius: 8px !important;
                border: 1px solid #E5E7EB !important;
                padding: 6px 10px !important;
                font-size: 14px !important;
                transition: all 0.2s !important;
                font-family: 'Inter', sans-serif !important;
                background-color: white !important;
            }
            .shr-select:focus {
                border-color: #7C3AED !important;
                box-shadow: 0 0 0 3px rgba(124,58,237,0.15) !important;
                outline: none !important;
            }
            .shr-label {
                font-size: 14px;
                font-weight: 600;
                color: #4B5563;
                font-family: 'Inter', sans-serif;
            }
            .shr-card {
                background: white;
                border-radius: 16px;
                box-shadow: 0 10px 30px rgba(0,0,0,0.05);
                border: none;
                padding: 24px;
            }
            .shr-card-title {
                color: #5B21B6;
                font-weight: 600;
                font-size: 16px;
                margin-bottom: 6px;
                font-family: 'Inter', sans-serif;
            }
            .shr-accent-bar {
                background: linear-gradient(90deg, #6D28D9, #2563EB);
                height: 3px;
                width: 40px;
                border-radius: 4px;
                margin-bottom: 18px;
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form id="frmshare" name="frmshare" runat="server"
            style="background-color: #F3F4FF; font-family: 'Inter', sans-serif; min-height: 100vh; padding: 20px;">

            <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

            <%-- Breadcrumb --%>
            <nav class="page-breadcrumb" style="margin-bottom: 24px;">
                <ol class="breadcrumb" style="background: transparent; padding: 0;">
                    <li class="breadcrumb-item"><a href="#" style="color: #6B7280; text-decoration: none;">Share</a></li>
                    <li class="breadcrumb-item active" style="color: #374151; font-weight: 500;">Allocation</li>
                </ol>
            </nav>

            <%-- Alert Panel --%>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Panel ID="alertmsg" runat="server" Visible="true">
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

            <%-- Main Form Panel --%>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <asp:Panel ID="pnlshr" runat="server" style="display:block">

                        <div class="row" style="margin-bottom: 16px;">

                            <%-- LEFT CARD: Certificate & Customer Info --%>
                            <div class="col-md-6 grid-margin stretch-card" style="margin-bottom: 16px;">
                                <div class="shr-card" style="width:100%;">
                                    <h6 class="shr-card-title">Share Details</h6>
                                    <div class="shr-accent-bar"></div>

                                    <div class="form-group row" style="margin-bottom: 12px;">
                                        <label class="col-sm-4 col-form-label shr-label">Certificate No</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox AutoPostBack="true" id="txtcno" runat="server"
                                                CssClass="form-control shr-input"
                                                onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" style="margin-bottom: 12px;">
                                        <label class="col-sm-4 col-form-label shr-label">Customer Id</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtcid" runat="server" CssClass="form-control shr-input"
                                                AutoPostBack="true" AutoCompleteType="None"
                                                data-validation-engine="validate[required]"
                                                data-errormessage-value-missing="Customer Id Missing"
                                                data-errormessage-custom-error="Enter a Valid Customer Id"
                                                onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" style="margin-bottom: 12px;">
                                        <label class="col-sm-4 col-form-label shr-label">Customer Name</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtname" CssClass="form-control shr-input" Enabled="False"
                                                runat="server"
                                                data-validation-engine="validate[required]"
                                                data-errormessage-value-missing="Customer Name Missing"
                                                data-errormessage-custom-error="Enter a Valid Account No"
                                                style="background-color: #F9FAFB; color: #6B7280;">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" style="margin-bottom: 0;">
                                        <label class="col-sm-4 col-form-label shr-label">Address</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtadd" CssClass="form-control shr-input" Enabled="False"
                                                runat="server" Width="100%" Height="70px" TextMode="MultiLine"
                                                style="background-color: #F9FAFB; color: #6B7280; resize: none;">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <%-- RIGHT CARD: Allocation Details --%>
                            <div class="col-md-6 grid-margin stretch-card" style="margin-bottom: 16px;">
                                <div class="shr-card" style="width:100%;">
                                    <h6 class="shr-card-title">Allocation Details</h6>
                                    <div class="shr-accent-bar"></div>

                                    <div class="form-group row" style="margin-bottom: 12px;">
                                        <label class="col-sm-4 col-form-label shr-label">Date</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" CssClass="form-control shr-input"
                                                data-validation-engine="validate[required,funcCall[DateFormat[]]]"
                                                ID="txtadate" Width="100%"
                                                data-errormessage-value-missing="Valid Date is required!"
                                                onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" style="margin-bottom: 12px;">
                                        <label class="col-sm-4 col-form-label shr-label">Value / Share</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" Enabled="false" CssClass="form-control shr-input"
                                                data-validation-engine="validate[required]" ID="txtshrval"
                                                Text="10.00" Width="100%"
                                                data-errormessage-value-missing="Valid Number is required!"
                                                style="background-color: #F9FAFB; color: #6B7280;">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" style="margin-bottom: 12px;">
                                        <label class="col-sm-4 col-form-label shr-label">No of Shares</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" CssClass="form-control shr-input"
                                                data-validation-engine="validate[required]" ID="txtnos"
                                                AutoPostBack="true" Width="100%"
                                                data-errormessage-value-missing="Valid Number is required!"
                                                onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row" style="margin-bottom: 12px;">
                                        <label class="col-sm-4 col-form-label shr-label">Share Value</label>
                                        <div class="col-sm-8">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox runat="server" Enabled="false"
                                                        CssClass="form-control shr-input"
                                                        data-validation-engine="validate[required]" ID="txtval"
                                                        Width="100%"
                                                        data-errormessage-value-missing="Valid Number is required!"
                                                        style="background-color: #F9FAFB; color: #6B7280;">
                                                    </asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <div class="form-group row" style="margin-bottom: 12px;">
                                        <label class="col-sm-4 col-form-label shr-label">Allocate As</label>
                                        <div class="col-sm-8">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" CssClass="form-control shr-select"
                                                        AutoPostBack="true"
                                                        data-validation-engine="validate[required]" ID="shrtyp"
                                                        Width="100%"
                                                        data-errormessage-value-missing="Please Select an Option"
                                                        onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                        onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                                        <asp:ListItem Value="" Text="<-Select->"></asp:ListItem>
                                                        <asp:ListItem Value="New" Text="New"></asp:ListItem>
                                                        <asp:ListItem Value="Transfer" Text="Transfer"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <asp:UpdatePanel runat="server" ID="trf" Visible="false">
                                        <ContentTemplate>
                                            <div class="form-group row" style="margin-bottom: 12px;">
                                                <label class="col-sm-4 col-form-label shr-label">Transfer From</label>
                                                <div class="col-sm-8">
                                                    <asp:UpdatePanel runat="server" id="up_trf">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtshrcert"
                                                                style="display:inline;float:left" Visible="false"
                                                                Width="100%" runat="server" CssClass="form-control shr-input"
                                                                AutoPostBack="true" AutoCompleteType="None"
                                                                data-validation-engine="validate[required]"
                                                                data-errormessage-value-missing="Share Certificate No Missing"
                                                                data-errormessage-custom-error="Enter a Valid Customer Id"
                                                                onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                                onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                                            </asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="form-group row" style="margin-bottom: 0;">
                                        <label class="col-sm-4 col-form-label shr-label">Mode of Payment</label>
                                        <div class="col-sm-8">
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="mop" runat="server" AutoPostBack="true"
                                                        CssClass="form-control shr-select" Width="100%"
                                                        onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                        onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                                        <asp:ListItem>Cash</asp:ListItem>
                                                        <asp:ListItem>Account</asp:ListItem>
                                                        <asp:ListItem>Transfer</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div><%-- end .row --%>

                        <%-- SB Account Panel --%>
                        <asp:Panel Visible="false" runat="server" ID="pnlsbtrf">
                            <div class="shr-card" style="margin-bottom: 16px;">
                                <div class="form-group row" style="margin-bottom: 0;">
                                    <asp:Label ID="lblsb" Text="SB Account No"
                                        CssClass="col-sm-2 col-form-label shr-label" runat="server" Visible="true">
                                    </asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txt_sb" runat="server" Visible="true"
                                            CssClass="form-control shr-input" AutoPostBack="true"
                                            data-validation-engine="validate[required]"
                                            data-errormessage-value-missing="Valid Account No is required!"
                                            onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                            onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label runat="server" ID="lbl_sb_bal"
                                            style="font-weight: 600; font-size: 15px; line-height: 38px;"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <%-- Transfer Panel --%>
                        <asp:Panel ID="pnltran" runat="server" Visible="false">
                            <div class="shr-card" style="margin-bottom: 16px;">
                                <div class="form-group row" style="margin-bottom: 0;">
                                    <asp:Label ID="lbl" Text="Transfer To"
                                        CssClass="col-sm-2 col-form-label shr-label" runat="server" Visible="false">
                                    </asp:Label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="bnk" runat="server" Visible="false"
                                            CssClass="form-control shr-select">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <%-- Action Buttons --%>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div style="border-bottom: 1px solid #E5E7EB; margin: 4px 0 20px 0;"></div>
                                <div class="form-group row">
                                    <div class="col-sm-12" style="display: flex; justify-content: flex-end; gap: 12px; padding-right: 15px;">
                                        <asp:Button ID="btn_clr_ms" runat="server" Text="Clear"
                                            CssClass="btn"
                                            OnClientClick="detach();" CausesValidation="false"
                                            style="border-radius: 10px; padding: 10px 28px; font-weight: 500; font-family: 'Inter', sans-serif; background: transparent; border: 1px solid #D1D5DB; color: #4B5563; transition: all 0.2s;"
                                            onmouseover="this.style.background='#F3F4F6'; this.style.transform='translateY(-1px)';"
                                            onmouseout="this.style.background='transparent'; this.style.transform='translateY(0)';" />
                                        <asp:Button ID="btn_nxt_ms" runat="server" Text="Save"
                                            CssClass="btn"
                                            style="background: linear-gradient(135deg, #7C3AED, #2563EB); color: white; border-radius: 10px; padding: 10px 28px; border: none; font-weight: 500; font-family: 'Inter', sans-serif; transition: all 0.2s; box-shadow: 0 4px 14px rgba(37,99,235,0.2);"
                                            onmouseover="this.style.transform='translateY(-2px)'; this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
                                            onmouseout="this.style.transform='translateY(0)'; this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';" />
                                        <asp:Button ID="btn_update" runat="server" Text="Update"
                                            CssClass="btn" Visible="false"
                                            style="background: linear-gradient(135deg, #059669, #0284C7); color: white; border-radius: 10px; padding: 10px 28px; border: none; font-weight: 500; font-family: 'Inter', sans-serif; transition: all 0.2s; box-shadow: 0 4px 14px rgba(5,150,105,0.2);"
                                            onmouseover="this.style.transform='translateY(-2px)'; this.style.boxShadow='0 6px 20px rgba(5,150,105,0.3)';"
                                            onmouseout="this.style.transform='translateY(0)'; this.style.boxShadow='0 4px 14px rgba(5,150,105,0.2)';" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </asp:Panel>

                    <%-- Print Panel --%>
                    <asp:Panel runat="server" ID="pnlprnt" style="display:block">
                        <div class="shr-card" style="margin-bottom: 16px;">
                            <div class="form-group row" style="margin-bottom: 0;">
                                <div class="col-md-3"></div>
                                <div class="col-md-6">
                                    <div class="btn-group" role="group" style="gap: 12px;">
                                        <asp:Button runat="server" ID="btntog"
                                            style="border-radius: 10px; padding: 10px 28px; font-weight: 500; font-family: 'Inter', sans-serif; background: transparent; border: 1px solid #D1D5DB; color: #4B5563; transition: all 0.2s;"
                                            Class="btn" Text="Close"
                                            onmouseover="this.style.background='#F3F4F6'; this.style.transform='translateY(-1px)';"
                                            onmouseout="this.style.background='transparent'; this.style.transform='translateY(0)';" />
                                        <button type="button"
                                            style="border-radius: 10px; padding: 10px 28px; font-weight: 500; font-family: 'Inter', sans-serif; background: linear-gradient(135deg, #7C3AED, #2563EB); color: white; border: none; transition: all 0.2s; box-shadow: 0 4px 14px rgba(37,99,235,0.2);"
                                            onmouseover="this.style.transform='translateY(-2px)'; this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
                                            onmouseout="this.style.transform='translateY(0)'; this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';"
                                            onclick="PrintOC();">Print</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                                        <asp:updatePanel runat="server">
                                            <%-- PRINT AREA --%>
                                            
                                            <ContentTemplate>
                                                <div id="vouchprint">
                                                    <style>
                                                        @media print {
                                                            @page {
                                                                size: A4;
                                                                margin: 0;
                                                            }
                                                            body, html {
                                                                margin: 0 !important;
                                                                padding: 0 !important;
                                                            }
                                                            .cert-container {
                                                                position: relative;
                                                                width: 210mm;
                                                                height: 297mm;
                                                            }
                                                            .prnt-field {
                                                                position: absolute;
                                                                font-family: Arial, sans-serif;
                                                                font-size: 16pt;
                                                                font-weight: bold;
                                                                color: #000;
                                                            }
                                                        }
                                                        @media screen {
                                                            .cert-container {
                                                                position: relative;
                                                                width: 210mm;
                                                                height: 297mm;
                                                                border: 1px solid #ccc;
                                                                background: #f9f9f9;
                                                                margin: 10px auto;
                                                            }
                                                            .prnt-field {
                                                                position: absolute;
                                                                font-family: Arial, sans-serif;
                                                                font-size: 16pt;
                                                                font-weight: bold;
                                                                color: #000;
                                                            }
                                                        }
                                                    </style>

                                                    <div class="cert-container">
                                                        <div class="prnt-field" style="left: 128.92pt; top: 446.40pt;">
                                                            <span id="lblfolio"></span>
                                                        </div>
                                                        <div class="prnt-field" style="left: 284.83pt; top: 446.40pt;">
                                                            <asp:Label ID="lblcerno" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="prnt-field" style="left: 497.20pt; top: 266.10pt;">
                                                            <asp:Label ID="lblprntcid1" runat="server" ClientIDMode="Static" ></asp:Label>
                                                        </div>
                                                        <div class="prnt-field" style="left: 284.83pt; top: 491.76pt;">
                                                            <asp:Label ID="lblname" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="prnt-field" style="left: 426.56pt; top: 440.73pt;">
                                                            <asp:Label ID="lbldisno" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="prnt-field" style="left: 327.35pt; top: 528.77pt;">
                                                            <asp:Label ID="lblshr" Text="" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="prnt-field" style="left: 398.21pt; top: 570.19pt;">
                                                            <asp:Label ID="lbldt" Text="" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                             </ContentTemplate>
                                         </asp:updatePanel>

                    </asp:Panel><%-- /pnlprnt --%>

                </ContentTemplate>
            </asp:UpdatePanel><%-- /main UpdatePanel --%>

        </form>

        <script src="js/jquery.js" type="text/javascript"></script>
        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />

        <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
        <script src="js/jquery.validationEngine.js" type="text/javascript"></script>
        <script src="../js/moment.min.js" type="text/javascript"></script>
        <script src="js/daterangepicker.js" type="text/javascript"></script>

        <script src="js/printThis.js" type="text/javascript"></script>



        <style>
            .ui-autocomplete {
                max-height: 510px;
                overflow-y: auto;
                /* prevent horizontal scrollbar */
                overflow-x: hidden;
            }




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


        <script type="text/javascript">
            $(document).ready(function () {


                //$("#frmshare").validationEngine('');
                //' $("#frmshare").validationEngine({ promptPosition: "topleft", scroll: false, focusFirstField: true, showArrow: true });



                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);





                // Place here the first init of the autocomplete
                InitAutoCompl();
                InitAutoComplSB();
                Dpick();
                InitAutoComplGrp();

                $("#frmshare").validationEngine('attach', { promptPosition: "topleft", scroll: false, showArrow: true });
            });

            function InitializeRequest(sender, args) {
            }

            function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete
                InitAutoCompl();
                InitAutoComplGrp();
                Dpick();
                InitAutoComplSB();
            }



            function InitAutoComplSB() {

                $("#<%=txt_sb.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/membersearch.asmx/GetSbAc") %>',
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
                    open: function () {
                        $("ul.ui-menu").width($(this).innerWidth() + 210);

                    },

                    select: function (event, ui) {

                        $("#<%=txt_sb.ClientID %>").val(ui.item.memberno);



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


            function Dpick() {

                $("#<%=txtadate.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    showDropdowns: false,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtadate.ClientID%>").val(start.format('DD-MM-YYYY'));
                });

            }


            function InitAutoCompl() {





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
                    focus: function (event, ui) {

                        $("#<%=txtcid.ClientID %>").val(ui.item.firstname);
                        return false;

                    },
                    open: function () {
                        $("ul.ui-menu").width($(this).innerWidth() + 180);

                    },

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

            function InitAutoComplGrp() {

                $("#<%=txtshrcert.ClientID %>").autocomplete({
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

                        $("#<%=txtshrcert.ClientID %>").val(ui.item.memberno);


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


            function DateFormat(field, rules, i, options) {
                var regex = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
                if (!regex.test(field.val())) {
                    return "Please enter date in dd/MM/yyyy format."
                }
            };

            function chkdecim(field, rules, i, options) {
                var regex = /^\d+\.?\d{0,2}$/;
                if (!regex.test(field.val())) {
                    return "Please enter a Valid Number."
                }
            }

            function detach() {
                $("#frmshare").validationEngine('detach');

            }


            function PrintOC() {
                var folio = prompt("Enter Folio Number:", "");
                if (folio !== null) {
                    $('#lblfolio').text(folio);
                    var cidRaw = $('#<%=txtcid.ClientID%>').val();
                    if (cidRaw && cidRaw.trim() !== "") {
                        var cidClean = cidRaw.replace(/^.*KBF\s*/i, '');
                        $('#lblprntcid1').text(cidClean);
                    }

                    $('#vouchprint').printThis({
                        importCSS: true,
                        importStyle: true,
                        printContainer: true
                    });
                }
            }


        </script>

    </asp:Content>