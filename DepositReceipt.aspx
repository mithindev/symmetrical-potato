<%@ Page Title="Deposit Receipt" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="DepositReceipt.aspx.vb" Inherits="Fiscus.DepositReceipt" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <link href="css/ValidationEngine.css" rel="stylesheet" />
        <link href="css/validationEngine.jquery.css" rel="stylesheet" />
        <link href="css/daterangepicker.css" rel="stylesheet" />
        <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">

        <style>
            @media print {
                @page {
                    size: A5;
                    /* landscape */
                    /* you can also specify margins here: */
                    margin: 3mm;
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif
                        /* for compatibility with both A4 and Letter */


                }

                .outert {
                    border: solid #000 !important;
                    border-width: 1px 1px 1px 1px !important;
                }

                .prntarea {

                    height: 105mm;
                    width: 148.0mm;
                    margin: 0;
                    page-break-after: always;

                }
            }

            @media screen {
                .prntarea {
                    height: 105mm;
                    width: 148.0mm;
                    margin: 0;

                }


            }
        </style>

        <style>
            /* Reprint receipt overrides — each receipt is standalone, not half-page */
            @media screen {
                #reprintvouchprint .prntarea {
                    height: auto !important;
                    width: 148.0mm !important;
                    margin: 0 auto 50px auto !important;
                    border: 1px solid #ccc !important;
                    padding: 10px !important;
                    box-shadow: 0 4px 8px rgba(0,0,0,0.1);
                    background: #fff;
                    display: block !important;
                    overflow: visible !important;
                }
            }
            @media print {
                #reprintvouchprint .prntarea {
                    height: auto !important;
                    min-height: 0 !important;
                    width: 148.0mm !important;
                    margin: 0 !important;
                    padding: 0 !important;
                    box-shadow: none !important;
                    border: none !important;
                    display: block !important;
                }
            }
        </style>
        <style>
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
            .shr-btn-primary {
                background: linear-gradient(135deg, #7C3AED, #2563EB) !important;
                color: white !important;
                border-radius: 10px !important;
                padding: 10px 28px !important;
                border: none !important;
                font-weight: 500 !important;
                font-family: 'Inter', sans-serif !important;
                transition: all 0.2s !important;
                box-shadow: 0 4px 14px rgba(37,99,235,0.2) !important;
            }
            .shr-btn-primary:hover {
                transform: translateY(-2px) !important;
                box-shadow: 0 6px 20px rgba(37,99,235,0.3) !important;
            }
            .shr-btn-secondary {
                border-radius: 10px !important;
                padding: 10px 28px !important;
                font-weight: 500 !important;
                font-family: 'Inter', sans-serif !important;
                background: transparent !important;
                border: 1px solid #D1D5DB !important;
                color: #4B5563 !important;
                transition: all 0.2s !important;
            }
            .shr-btn-secondary:hover {
                background: #F3F4F6 !important;
                transform: translateY(-1px) !important;
            }
            @media screen {
                .page-wrapper .page-content {
                    margin-top: 70px !important;
                }
            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form id="newreceipt" runat="server" style="background-color: #F3F4FF; font-family: 'Inter', sans-serif; min-height: 100vh; padding: 20px;">

            <asp:ScriptManager ID="SM3" runat="server"></asp:ScriptManager>

            <div id="sctop"></div>
            <nav class="page-breadcrumb" style="margin-bottom: 24px;">
                <ol class="breadcrumb" style="background: transparent; padding: 0;">
                    <li class="breadcrumb-item"><a href="#" style="color: #6B7280; text-decoration: none;">Deposit</a></li>
                    <li class="breadcrumb-item active" style="color: #374151; font-weight: 500;">Receipt</li>
                    <li class="breadcrumb-item ms-auto">
                        <button type="button" class="btn shr-btn-secondary btn-sm" data-toggle="modal" data-target="#reprintModal" style="padding: 6px 16px !important; font-size: 13px !important;">Reprint Receipt</button>
                    </li>
                </ol>
            </nav>

            <div class="row">
                <div class="col-md-12 grid-margin stretch-card">
                    <div class="shr-card" style="width:100%;">

                            <asp:UpdatePanel runat="server" ID="tabUP">
                                <ContentTemplate>

                                    <asp:Panel runat="server" ID="pnltrans" style="display:block">
                                        <div class="form-group row" style="margin-bottom: 12px;">
                                            <label class="col-sm-2 col-form-label shr-label">Account NO</label>
                                            <div class="col-sm-3">
                                                <div class="input-group">

                                                    <asp:TextBox ID="txtacn" runat="server" CssClass="form-control shr-input"
                                                        AutoPostBack="true"></asp:TextBox>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton runat="server" ID="soa" Enabled="false">
                                                            <span class="input-group-addon input-group-text"
                                                                style="height:35px">

                                                                <svg xmlns="http://www.w3.org/2000/svg" width="24"
                                                                    height="24" viewBox="0 0 24 24" fill="none"
                                                                    stroke="currentColor" stroke-width="2"
                                                                    stroke-linecap="round" stroke-linejoin="round"
                                                                    class="feather feather-file-text">
                                                                    <path
                                                                        d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z">
                                                                    </path>
                                                                    <polyline points="14 2 14 8 20 8"></polyline>
                                                                    <line x1="16" y1="13" x2="8" y2="13"></line>
                                                                    <line x1="16" y1="17" x2="8" y2="17"></line>
                                                                    <polyline points="10 9 9 9 8 9"></polyline>
                                                                </svg>
                                                            </span>
                                                        </asp:LinkButton>
                                                    </div>

                                                </div>
                                            </div>

                                            <label class="col-sm-2 col-form-label shr-label">Date</label>
                                            <div class="col-sm-3">
                                                <asp:TextBox runat="server" CssClass="form-control shr-input" ID="tdate"
                                                    AutoPostBack="True"
                                                    data-validation-engine="validate[required,funcCall[DateFormat[]]"
                                                    data-errormessage-value-missing="Valid Date is required!">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="margin-bottom: 12px;">
                                            <label class="col-sm-2 col-form-label shr-label">Deposit</label>
                                            <asp:Label ID="lblproduct" CssClass="col-sm-3 col-form-label shr-label" style="color: #5B21B6 !important;"
                                                runat="server"></asp:Label>
                                        </div>

                                        <asp:Panel runat="server" ID="pnlact" Visible="false">
                                            <!-- CUSTOMER CARD -->
                                            <div
                                                style="background:#fff; padding:16px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05); margin-bottom:18px; display:flex; justify-content:space-between; align-items:center;">
                                                <div>
                                                    <div style="font-size:16px; font-weight:600; color:#1e293b;">
                                                        <asp:Label runat="server" ID="lblname" />
                                                    </div>
                                                    <div style="font-size:12px; color:#64748b; margin-top:4px;">
                                                        <asp:Label runat="server" ID="lbladd" />
                                                    </div>
                                                    <div style="font-size:12px; color:#64748b; margin-top:2px;">
                                                        <asp:Label runat="server" ID="lblmobile" />
                                                    </div>
                                                </div>
                                                <asp:Image id="imgCapture" runat="server" Visible="true"
                                                    style="width:60px; height:60px; border-radius:50%; object-fit:cover;" />
                                            </div>
                                        </asp:Panel>
                                        <div class="form-group row" style="margin-bottom: 12px;">
                                            <label class="col-sm-2 col-form-label shr-label">Deposit Amount</label>
                                            <asp:Label ID="lblamt" CssClass="col-sm-3 col-form-label shr-label" style="color: #5B21B6 !important;"
                                                runat="server"></asp:Label>
                                            <label class="col-sm-2 col-form-label shr-label">Account Balance</label>
                                            <asp:Label ID="lblbal" CssClass="col-sm-3 col-form-label shr-label" style="color: #059669 !important;"
                                                Font-Bold="true" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel ID="rdinfo" Visible="false" runat="server">

                                            <div style="background-color: #F8FAFC; border: 1px solid #E2E8F0; border-radius: 12px; padding: 16px; margin-bottom: 16px;">
                                                <div class="form-group row border-sm-bottom" style="margin-bottom: 8px;">
                                                    <label class="col-sm-2 col-form-label shr-label">Total Due</label>
                                                    <asp:Label ID="rd_due" runat="server"
                                                        CssClass="col-sm-1 col-form-label border-sm-right" style="color: #059669 !important; font-weight: 600;"
                                                        Font-Size="Large"></asp:Label>
                                                    <label class="col-sm-2 col-form-label shr-label">Paid </label>
                                                    <asp:Label ID="rd_duepaid" runat="server"
                                                        CssClass="col-sm-1 col-form-label border-sm-right" style="color: #059669 !important; font-weight: 600;"
                                                        Font-Size="Large"></asp:Label>
                                                    <label class="col-sm-2 col-form-label shr-label">Balance</label>
                                                    <asp:Label ID="rd_duebalance" runat="server"
                                                        CssClass="col-sm-1 col-form-label border-sm-right" style="color: #DC2626 !important; font-weight: 600;"
                                                        Font-Size="Large"></asp:Label>
                                                    <asp:Label ID="lblhed" runat="server"
                                                        CssClass="col-sm-1 col-form-label shr-label" Width="100%"></asp:Label>
                                                    <asp:Label ID="rdlateby" runat="server" Font-Bold="True"
                                                        CssClass="col-sm-1 col-form-label shr-label"></asp:Label>
                                                </div>
                                                <div class="form-group row" style="margin-bottom: 0;">

                                                    <label class="col-sm-2 col-form-label shr-label">No of Due</label>
                                                    <div class="col-sm-1 border-sm-right ">
                                                        <asp:UpdatePanel runat="server" ID="rddue" Visible="true">
                                                            <ContentTemplate>
                                                                <asp:TextBox runat="server" CssClass="form-control shr-input"
                                                                    Width="100%" style="text-align:right " ID="txtnod"
                                                                    AutoPostBack="true"
                                                                    data-validation-engine="validate[required,funcCall[chkdecim[]]"
                                                                    data-errormessage-value-missing="No of due Missing">
                                                                </asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-sm-1 col-form-label shr-label">Penalty</label>
                                                    <div class="col-sm-2 border-sm-right ">

                                                        <asp:UpdatePanel runat="server" ID="rdpenal" Visible="true">
                                                            <ContentTemplate>
                                                                <asp:TextBox runat="server" CssClass="form-control shr-input"
                                                                    Width="100%" style="text-align:right "
                                                                    ID="txtpenalty" AutoPostBack="true"
                                                                    data-validation-engine="validate[required,funcCall[chkdecim[]]"
                                                                    data-errormessage-value-missing="Amount Missing">
                                                                </asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblcalc" runat="server"
                                                                    Font-Size="Smaller"
                                                                    style="text-align:left ;vertical-align:middle"
                                                                    Width="100%" ForeColor="#09c"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                </div>

                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="dsinfo" Visible="false" runat="server">
                                            <div style="background-color: #F0FDF4; border: 1px solid #DCFCE7; border-radius: 12px; padding: 16px; margin-bottom: 16px;">
                                                <div class="form-group row border-sm-bottom" style="margin-bottom: 8px;">
                                                    <label class="col-sm-2 col-form-label shr-label">Total Due</label>
                                                    <asp:Label ID="ds_due" runat="server"
                                                        CssClass="col-sm-1 col-form-label border-sm-right" style="color: #059669 !important; font-weight: 600;"
                                                        Font-Size="Large"></asp:Label>
                                                    <label class="col-sm-2 col-form-label shr-label">Paid</label>
                                                    <asp:Label ID="ds_duepaid" runat="server"
                                                        CssClass="col-sm-1 col-form-label border-sm-right" style="color: #059669 !important; font-weight: 600;"
                                                        Font-Size="Large"></asp:Label>
                                                    <label class="col-sm-2 col-form-label shr-label">Balance</label>
                                                    <asp:Label ID="ds_duebalance" runat="server"
                                                        CssClass="col-sm-1 col-form-label border-sm-right" style="color: #DC2626 !important; font-weight: 600;"
                                                        Font-Size="Large"></asp:Label>
                                                </div>
                                                <div class="form-group row" style="margin-bottom: 0;">
                                                    <label class="col-sm-2 col-form-label shr-label">No of Due</label>
                                                    <div class="col-sm-1 border-sm-right">
                                                        <asp:UpdatePanel runat="server" ID="dsdue" Visible="true">
                                                            <ContentTemplate>
                                                                <asp:TextBox runat="server" CssClass="form-control shr-input"
                                                                    Width="100%" style="text-align:right "
                                                                    ID="txtds_nod" AutoPostBack="true"
                                                                    data-validation-engine="validate[required,funcCall[chkdecim[]]"
                                                                    data-errormessage-value-missing="No of due Missing">
                                                                </asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <div class="form-group row" style="margin-bottom: 12px;">
                                            <label class="col-sm-2 col-form-label shr-label">Receipt Amount</label>

                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="rcpt" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox runat="server" CssClass="form-control shr-input" ID="txtamt"
                                                            AutoPostBack="true"
                                                            data-validation-engine="validate[required,funcCall[chkdecim[]]"
                                                            data-errormessage-value-missing="Receipt Amount Missing">
                                                        </asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>
                                        <div class="form-group row" style="margin-bottom: 12px;">
                                            <label class="col-sm-2 col-form-label shr-label">Notes</label>
                                            <div class="col-sm-3">
                                                <asp:TextBox runat="server" CssClass="form-control shr-input"
                                                    style="text-align:left  " ID="txtnar"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row" style="margin-bottom: 12px;">
                                            <label class="col-sm-2 col-form-label shr-label">Mode of Payment</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                    <ContentTemplate>

                                                        <asp:DropDownList ID="mop" runat="server" AutoPostBack="true"
                                                            CssClass="form-control shr-select">
                                                            <asp:ListItem>Cash</asp:ListItem>
                                                            <asp:ListItem>Account</asp:ListItem>
                                                            <asp:ListItem>Transfer</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>

                                            <asp:Label ID="lblsb" Text="SB Account No" runat="server"
                                                CssClass="col-sm-2 col-form-label shr-label" Visible="false"></asp:Label>

                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txt_sb" runat="server" Visible="false"
                                                    CssClass="form-control shr-input" Width="130px" AutoPostBack="true"
                                                    data-validation-engine="validate[required]"
                                                    data-errormessage-value-missing="Valid Account No is required!">
                                                </asp:TextBox>
                                            </div>

                                            <asp:Label runat="server" ID="lbl_sb_bal" CssClass="col-sm-2 text-success ">
                                            </asp:Label>
                                        </div>
                                        <div class="form-group row" style="margin-bottom: 12px;">
                                            <asp:Label ID="lbl" Text="Transfer From" CssClass="col-sm-2 col-form-label shr-label"
                                                runat="server" Visible="false"></asp:Label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="lstbnk" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="bnk" runat="server" Width="200px"
                                                            CssClass="form-control shr-select" Visible="false"></asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server"
                                            AssociatedUpdatePanelID="pnlbtn">
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
                                        <asp:UpdatePanel runat="server" ID="pnlbtn">
                                            <ContentTemplate>


                                                <div class="form-group row border-sm-bottom" style="margin: 20px 0; border-bottom: 1px solid #E5E7EB;"></div>
                                                <div class="form-group row">
                                                    <div class="col-sm-12" style="display: flex; justify-content: flex-end; gap: 12px; padding-right: 15px;">
                                                        <asp:Button ID="btn_up_can" runat="server"
                                                            CssClass="btn shr-btn-secondary" Text="Clear"
                                                            OnClientClick="return detach();" />
                                                        <asp:Button ID="btn_up_rcpt" runat="server"
                                                            CssClass="btn shr-btn-primary" Text="Save" />
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>

                                    <asp:Panel runat="server" ID="pnlprnt" style="display:none">

                                        <asp:UpdatePanel runat="server" ID="pnlbtnprnt" Visible="true">
                                            <ContentTemplate>
                                                <div class="form-group row border-bottom" style="margin-bottom: 24px; padding-bottom: 12px;">
                                                    <div class="col-sm-12" style="display: flex; justify-content: center; gap: 12px;">
                                                        <asp:Button runat="server" ID="btntog"
                                                            CssClass="btn shr-btn-secondary" Text="Close" />
                                                        <button type="button" class="btn shr-btn-primary"
                                                            onclick="PrintOC();">Print Office Copy</button>
                                                        <button type="button" class="btn shr-btn-primary"
                                                            onclick="PrintCC();">Print Customer Copy</button>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div id="vouchprint">

                                            <div class="prntarea ">


                                                <table style="border:1px solid #000;width:100%">
                                                    <tbody>
                                                        <tr>

                                                            <td>
                                                                <table style="border-collapse: collapse; ">
                                                                    <tbody>
                                                                        <tr style="border: none;">
                                                                            <td style="width: 10%;"><img
                                                                                    src="Images/KBF-LOGO.png" alt=""
                                                                                    width="72" height="60" /></td>
                                                                            <td style="width: 58%;">
                                                                                <table border="0"
                                                                                    style="border-collapse: collapse; width:100%; height: 86px;">
                                                                                    <tbody>
                                                                                        <tr style="height: 10px;">
                                                                                            <td
                                                                                                style="width: 100%;font-size:18px;height:10px;font-weight:600">
                                                                                                Karavilai Nidhi Limited

                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr style="height: 10px;">
                                                                                            <td
                                                                                                style="width: 100%; height: 10px;text-align: left; font-size: 11px;">
                                                                                                <span>Reg No :
                                                                                                    18-37630/97</span><br /><span
                                                                                                    style="margin-top:5px;">8-12A,Vijayam,
                                                                                                    Main Road
                                                                                                    Karavilai,Villukuri
                                                                                                    P. O</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td
                                                                                                style="width: 100%;font-size:12px;">
                                                                                                Branch : <asp:Label
                                                                                                    id="pbranch"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td style="width:32%">
                                                                                <table
                                                                                    style="float: right; margin-left: 20px; height: 63px"
                                                                                    width:"100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td colspan="2"
                                                                                                style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:600">
                                                                                                <asp:label
                                                                                                    runat="server"
                                                                                                    id="lblcpt">
                                                                                                </asp:label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2"
                                                                                                style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 ">
                                                                                                <asp:label
                                                                                                    runat="server"
                                                                                                    id="lblcptr">
                                                                                                </asp:label>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr style="height: 21px;">
                                                                                            <td
                                                                                                style="width: 82px; height: 21px;font-size: 15px">
                                                                                                No&nbsp; &nbsp; :</td>
                                                                                            <td
                                                                                                style="width: 90px; height: 21px;font-size: 15px; font-weight: bold;">
                                                                                                <asp:Label ID="pvno"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr style="height: 21px;">
                                                                                            <td
                                                                                                style="width: 82px; height: 21px;font-size: 12.5px">
                                                                                                Date :</td>
                                                                                            <td
                                                                                                style="width: 90px; height: 21px;font-size: 12.5px">
                                                                                                <asp:Label ID="pdate"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>



                                                                <table
                                                                    style="border-top:1px solid ;border-bottom:1px solid;">
                                                                    <tr>
                                                                        <td style="width:69%;padding-left:10px">

                                                                            <table
                                                                                style="width:100%;border-style:none;">
                                                                                <tbody>
                                                                                    <tr style="height:30px;">
                                                                                        <td
                                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                                            Account No</td>
                                                                                        <td
                                                                                            style="width: 21px; height: 21px;">
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                                            <asp:Label ID="pacno"
                                                                                                runat="server">
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr
                                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                                        <td
                                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                                            Account</td>
                                                                                        <td
                                                                                            style="width: 21px; height: 21px;">
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                                            <asp:Label ID="pglh"
                                                                                                runat="server">
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr style="height: 30px;">
                                                                                        <td
                                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                                            Member No</td>
                                                                                        <td
                                                                                            style="width: 21px; height: 21px;">
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                                            <asp:Label ID="pcid"
                                                                                                runat="server">
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr
                                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                                        <td
                                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                                            Customer Name</td>
                                                                                        <td
                                                                                            style="width: 21px; height: 21px;">
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                                            <asp:Label ID="pcname" runat="server" Font-Bold="true"></asp:Label>
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr style="height: 30px;">
                                                                                        <td
                                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                                            Amount</td>
                                                                                        <td
                                                                                            style="width: 21px; height: 21px;">
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 263px; height: 21px;font-size:15px">
                                                                                            <b>
                                                                                                <asp:Label ID="pamt"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </b>

                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr style="height: 25px;">
                                                                                        <td colspan="3"
                                                                                            style="font-size: 12px; width: 423px; height: 21px;">
                                                                                            <asp:Label runat="server"
                                                                                                id="pnar"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>

                                                                        </td>

                                                                        <td style="width:31%">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" style="padding-left:5px; font-size: 11px;">
                                                                            Note :&nbsp;<asp:Label ID="pnote"
                                                                                runat="server" Font-Bold="true"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" style="padding-left:5px">
                                                                            Amount in Words :&nbsp;<asp:Label ID="paiw"
                                                                                runat="server"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                </table>





                                                                <table style="height:15px;margin-top:10px;width:100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td
                                                                                style="width:35%; text-align: center;font-size: 14px">
                                                                                Incharge / Manager</td>

                                                                            <td
                                                                                style="width: 30%; text-align: center;font-size: 14px">
                                                                                Cashier</td>
                                                                            <td
                                                                                style="width: 35%; text-align: center;font-size: 13px">
                                                                                (&nbsp;<asp:Label runat="server"
                                                                                    ID="premit"></asp:Label>&nbsp;)</td>


                                                                        </tr>
                                                                    </tbody>
                                                                </table>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>


                                            </div>


                                            <div class="prntarea ">


                                                <table style="border:1px solid #000;width:100%">
                                                    <tbody>
                                                        <tr>

                                                            <td>
                                                                <%-- Header --%>
                                                                    <table style="border-collapse: collapse; ">
                                                                        <tbody>
                                                                            <tr style="border: none;">
                                                                                <td style="width: 10%;"><img
                                                                                        src="Images/KBF-LOGO.png" alt=""
                                                                                        width="72" height="60" /></td>
                                                                                <td style="width: 58%;">
                                                                                    <table border="0"
                                                                                        style="border-collapse: collapse; width:100%; height: 86px;">
                                                                                        <tbody>
                                                                                            <tr style="height: 10px;">
                                                                                                <td
                                                                                                    style="width: 100%;font-size:18px;height:10px;font-weight:600">
                                                                                                    Karavilai Nidhi
                                                                                                    Limited

                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr style="height: 10px;">
                                                                                                <td
                                                                                                    style="width: 100%; height: 10px;text-align: left; font-size: 11px;">
                                                                                                    <span>Reg No :
                                                                                                        18-37630/97</span><br /><span
                                                                                                        style="margin-top:5px;">8-12A,Vijayam,
                                                                                                        Main Road
                                                                                                        Karavilai,Villukuri
                                                                                                        P. O</span>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td
                                                                                                    style="width: 100%;font-size:12px;">
                                                                                                    Branch : <asp:Label
                                                                                                        id="pcbranch"
                                                                                                        runat="server">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="width:32%">
                                                                                    <table
                                                                                        style="float: right; margin-left: 20px; height: 63px"
                                                                                        width:"100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td colspan="2"
                                                                                                    style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:600">
                                                                                                    <asp:label
                                                                                                        runat="server"
                                                                                                        id="lblccpt">
                                                                                                    </asp:label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2"
                                                                                                    style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 ">
                                                                                                    <asp:label
                                                                                                        runat="server"
                                                                                                        id="lblccptr">
                                                                                                    </asp:label>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr style="height: 21px;">
                                                                                                <td
                                                                                                    style="width: 82px; height: 21px;font-size: 15px">
                                                                                                    No&nbsp; &nbsp; :
                                                                                                </td>
                                                                                                <td
                                                                                                    style="width: 90px; height: 21px;font-size: 15px; font-weight: bold;">
                                                                                                    <asp:Label
                                                                                                        ID="pcvno"
                                                                                                        runat="server">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr style="height: 21px;">
                                                                                                <td
                                                                                                    style="width: 82px; height: 21px;font-size: 12.5px">
                                                                                                    Date :</td>
                                                                                                <td
                                                                                                    style="width: 90px; height: 21px;font-size: 12.5px">
                                                                                                    <asp:Label
                                                                                                        ID="pcdate"
                                                                                                        runat="server">
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>


                                                                    <table
                                                                        style="border-top:1px solid ;border-bottom:1px solid;">
                                                                        <tr>
                                                                            <td style="width:69%;padding-left:10px">

                                                                                <table
                                                                                    style="width:100%;border-style:none;">
                                                                                    <tbody>
                                                                                        <tr style="height:30px;">
                                                                                            <td
                                                                                                style="font-size: 12px; width: 139px; height: 21px;">
                                                                                                Account No</td>
                                                                                            <td
                                                                                                style="width: 21px; height: 21px;">
                                                                                            </td>
                                                                                            <td
                                                                                                style="width: 263px; height: 21px;font-size: 15px">
                                                                                                <asp:Label ID="pcacno"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr
                                                                                            style="height: 30px;border-bottom:1px solid black">
                                                                                            <td
                                                                                                style="font-size: 12px; width: 139px; height: 21px;">
                                                                                                Account</td>
                                                                                            <td
                                                                                                style="width: 21px; height: 21px;">
                                                                                            </td>
                                                                                            <td
                                                                                                style="width: 263px; height: 21px;font-size: 15px">
                                                                                                <asp:Label ID="pcglh" runat="server" Font-Bold="true"></asp:Label>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr style="height: 30px;">
                                                                                            <td
                                                                                                style="font-size: 12px; width: 139px; height: 21px;">
                                                                                                Member No</td>
                                                                                            <td
                                                                                                style="width: 21px; height: 21px;">
                                                                                            </td>
                                                                                            <td
                                                                                                style="width: 263px; height: 21px;font-size: 15px">
                                                                                                <asp:Label ID="pccid"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr
                                                                                            style="height: 30px;border-bottom:1px solid black">
                                                                                            <td
                                                                                                style="font-size: 12px; width: 139px; height: 21px;">
                                                                                                Customer Name</td>
                                                                                            <td
                                                                                                style="width: 21px; height: 21px;">
                                                                                            </td>
                                                                                            <td
                                                                                                style="width: 263px; height: 21px;font-size: 15px">
                                                                                                <asp:Label ID="pccname" runat="server" Font-Bold="true"></asp:Label>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr style="height: 30px;">
                                                                                            <td
                                                                                                style="font-size: 12px; width: 139px; height: 21px;">
                                                                                                Amount</td>
                                                                                            <td
                                                                                                style="width: 21px; height: 21px;">
                                                                                            </td>
                                                                                            <td
                                                                                                style="width: 263px; height: 21px;font-size:15px">
                                                                                                <b>
                                                                                                    <asp:Label
                                                                                                        ID="pcamt"
                                                                                                        runat="server">
                                                                                                    </asp:Label>
                                                                                                </b>

                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr style="height: 25px;">
                                                                                            <td colspan="3"
                                                                                                style="font-size: 12px; width: 423px; height: 21px;">
                                                                                                <asp:Label
                                                                                                    runat="server"
                                                                                                    id="pcnar">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>

                                                                            </td>

                                                                            <td style="width:31%">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="padding-left:5px; font-size: 11px;">
                                                                                Note :&nbsp;<asp:Label
                                                                                    ID="pcnote" runat="server" Font-Bold="true">
                                                                                </asp:Label>

                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="padding-left:5px">
                                                                                Amount in Words :&nbsp;<asp:Label
                                                                                    ID="pcaiw" runat="server">
                                                                                </asp:Label>

                                                                            </td>
                                                                        </tr>
                                                                    </table>


                                                                    <%-- Footer --%>
                                                                        <table
                                                                            style="height:15px;margin-top:10px;width:100%">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td
                                                                                        style="width:35%; text-align: center;font-size: 14px">
                                                                                        Incharge / Manager</td>

                                                                                    <td
                                                                                        style="width: 30%; text-align: center;font-size: 14px">
                                                                                        Cashier</td>
                                                                                    <td
                                                                                        style="width: 35%; text-align: center;font-size: 13px">
                                                                                        (&nbsp;<asp:Label runat="server"
                                                                                            ID="pcremit"></asp:Label>
                                                                                        &nbsp;)
                                                                                    </td>


                                                                                </tr>
                                                                            </tbody>
                                                                        </table>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>


                                            </div>

                                        </div>
                                    </asp:Panel>

                                    <!-- REPRINT MODAL -->
                                    <div class="modal fade" id="reprintModal" tabindex="-1" role="dialog" aria-labelledby="reprintModalLabel" aria-hidden="true">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title" id="reprintModalLabel">Reprint Receipt</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body" style="padding: 24px 24px 0 24px;">
                                                    <div class="form-group row" style="margin-bottom: 16px;">
                                                        <label class="col-sm-4 col-form-label shr-label">Date</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox runat="server" CssClass="form-control shr-input" ID="txtReprintDate"
                                                                data-validation-engine="validate[required,funcCall[DateFormat[]]"
                                                                data-errormessage-value-missing="Valid Date is required!">
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row mt-2" style="margin-bottom: 16px;">
                                                        <label class="col-sm-4 col-form-label shr-label">Transaction ID</label>
                                                        <div class="col-sm-8">
                                                            <asp:TextBox runat="server" CssClass="form-control shr-input" ID="txtReprintTransID"
                                                                data-validation-engine="validate[required]"
                                                                data-errormessage-value-missing="Transaction ID is required!">
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer" style="padding: 16px 24px 24px 24px; border-top: none;">
                                                    <button type="button" class="btn shr-btn-secondary" data-dismiss="modal">Close</button>
                                                    <asp:Button ID="btn_reprint_submit" runat="server" CssClass="btn shr-btn-primary validate-skip" CausesValidation="false" Text="Search &amp; Reprint" OnClientClick="if($('[id$=txtReprintDate]').val() == '' || $('[id$=txtReprintTransID]').val() == '') { alert('Please enter both Date and Transaction ID.'); return false; }" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- END REPRINT MODAL -->

                                    <asp:Panel runat="server" ID="pnlReprintResults" Visible="false">
                                        <asp:UpdatePanel runat="server" ID="pnlBtnReprintResults" Visible="true">
                                            <ContentTemplate>
                                                <div class="form-group row border-bottom" style="margin-bottom: 24px; padding-bottom: 12px;">
                                                    <div class="col-sm-12" style="display: flex; justify-content: center; gap: 12px;">
                                                        <asp:Button runat="server" ID="btnReprintClose" CssClass="btn shr-btn-secondary" Text="Close" />
                                                        <button type="button" class="btn shr-btn-primary" onclick="PrintOCReprint();">Print Office Copy</button>
                                                        <button type="button" class="btn shr-btn-primary" onclick="PrintCCReprint();">Print Customer Copy</button>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                                
                                        <div id="reprintvouchprint">
                                            <asp:Repeater ID="rptReprint" runat="server">
                                                    <SeparatorTemplate>
                                                        <div style="page-break-before: always; break-before: page;"></div>
                                                    </SeparatorTemplate>
                                                    <ItemTemplate>
                                                            
                                                    <!-- RECEIPT -->
                                                    <div class="prntarea" style="margin-bottom: 50px;">
                                                        <table style="border:1px solid #000;width:100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <table style="border-collapse: collapse;">
                                                                            <tbody>
                                                                                <tr style="border: none;">
                                                                                    <td style="width: 10%;"><img src="Images/KBF-LOGO.png" alt="" width="72" height="60" /></td>
                                                                                    <td style="width: 58%;">
                                                                                        <table border="0" style="border-collapse: collapse; width:100%; height: 86px;">
                                                                                            <tbody>
                                                                                                <tr style="height: 10px;">
                                                                                                    <td style="width: 100%;font-size:18px;height:10px;font-weight:600">Karavilai Nidhi Limited</td>
                                                                                                </tr>
                                                                                                <tr style="height: 10px;">
                                                                                                    <td style="width: 100%; height: 10px;text-align: left; font-size: 11px;">
                                                                                                        <span>Reg No : 18-37630/97</span><br /><span style="margin-top:5px;">8-12A,Vijayam, Main Road Karavilai,Villukuri P. O</span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 100%;font-size:12px;">Branch : <asp:Label id="rpbranch" runat="server" Text='<%# Eval("Branch") %>'></asp:Label></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width:32%">
                                                                                        <table style="float: right; margin-left: 20px; height: 63px" width:"100%">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:600"><%# Eval("ReceiptType") %></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400" class="reprint-copy-type">OFFICE COPY</td>
                                                                                                </tr>
                                                                                                <tr style="height: 21px;">
                                                                                                    <td style="width: 82px; height: 21px;font-size: 15px">No&nbsp; &nbsp; :</td>
                                                                                                    <td style="width: 90px; height: 21px;font-size: 15px; font-weight: bold;"><asp:Label ID="rpvno" runat="server" Text='<%# Eval("TransID") %>'></asp:Label></td>
                                                                                                </tr>
                                                                                                <tr style="height: 21px;">
                                                                                                    <td style="width: 82px; height: 21px;font-size: 12.5px">Date :</td>
                                                                                                    <td style="width: 90px; height: 21px;font-size: 12.5px"><asp:Label ID="rpdate" runat="server" Text='<%# Eval("Date") %>'></asp:Label></td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                        <table style="border-top:1px solid ;border-bottom:1px solid;">
                                                                            <tr>
                                                                                <td style="width:69%;padding-left:10px">
                                                                                    <table style="width:100%;border-style:none;">
                                                                                        <tbody>
                                                                                            <tr style="height:30px;">
                                                                                                <td style="font-size: 12px; width: 139px; height: 21px;">Account No</td>
                                                                                                <td style="width: 21px; height: 21px;"></td>
                                                                                                <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="rpacno" runat="server" Text='<%# Eval("AccountNo") %>'></asp:Label></td>
                                                                                            </tr>
                                                                                            <tr style="height: 30px;border-bottom:1px solid black">
                                                                                                <td style="font-size: 12px; width: 139px; height: 21px;">Account</td>
                                                                                                <td style="width: 21px; height: 21px;"></td>
                                                                                                <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="rpglh" runat="server" Text='<%# Eval("AccountHead") %>'></asp:Label></td>
                                                                                            </tr>
                                                                                            <tr style="height: 30px;">
                                                                                                <td style="font-size: 12px; width: 139px; height: 21px;">Member No</td>
                                                                                                <td style="width: 21px; height: 21px;"></td>
                                                                                                <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="rpcid" runat="server" Text='<%# Eval("MemberNo") %>'></asp:Label></td>
                                                                                            </tr>
                                                                                            <tr style="height: 30px;border-bottom:1px solid black">
                                                                                                <td style="font-size: 12px; width: 139px; height: 21px;">Customer Name</td>
                                                                                                <td style="width: 21px; height: 21px;"></td>
                                                                                                <td style="width: 263px; height: 21px;font-size: 15px"><asp:Label ID="rpcname" runat="server" Font-Bold="true" Text='<%# Eval("MemberName") %>'></asp:Label></td>
                                                                                            </tr>
                                                                                            <tr style="height: 30px;">
                                                                                                <td style="font-size: 12px; width: 139px; height: 21px;">Amount</td>
                                                                                                <td style="width: 21px; height: 21px;"></td>
                                                                                                <td style="width: 263px; height: 21px;font-size:15px"><b><asp:Label ID="rpamt" runat="server" Text='<%# Eval("AmountFormatted") %>'></asp:Label></b></td>
                                                                                            </tr>
                                                                                            <tr style="height: 25px;">
                                                                                                <td colspan="3" style="font-size: 12px; width: 423px; height: 21px;"><asp:Label runat="server" id="rpnar" Text='<%# Eval("Narration") %>'></asp:Label></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="width:31%"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" style="padding-left:5px">Note :&nbsp;<asp:Label ID="rpnote" runat="server" Font-Bold="true" Text='<%# Eval("UserNote") %>'></asp:Label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" style="padding-left:5px">Amount in Words :&nbsp;<asp:Label ID="rpaiw" runat="server" Text='<%# Eval("AmountInWords") %>'></asp:Label></td>
                                                                            </tr>
                                                                        </table>
                                                                        <table style="height:15px;margin-top:10px;width:100%">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td style="width:35%; text-align: center;font-size: 14px">Incharge / Manager</td>
                                                                                    <td style="width: 30%; text-align: center;font-size: 14px">Cashier</td>
                                                                                    <td style="width: 35%; text-align: center;font-size: 13px">(&nbsp;<asp:Label runat="server" ID="rpremit" Text='<%# Eval("MemberName") %>'></asp:Label>&nbsp;)</td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </asp:Panel>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btn_reprint_submit" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
            </div>
        </form>




        <script src="../js/jquery.js" type="text/javascript"></script>
        <script src="js/jquery.validationEngine.js" type="text/javascript"></script>
        <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
        <script src="js/daterangepicker.js" type="text/javascript"></script>
        <script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
        <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />
        <script src="js/printThis.js" type="text/javascript"></script>

        <script type="text/javascript">


            $(document).ready(function () {

                $("#newreceipt").validationEngine('attach');

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);
                InitAutoComplacn();
                InitAutoComplSB();

            });

            function InitializeRequest(sender, args) {
            }
            function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete

                InitAutoComplacn();
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
                    focus: function (event, ui) {

                        $("#<%=txt_sb.ClientID %>").val(ui.item.memberno);
                        return false;

                    },
                    open: function () {
                        $("ul.ui-menu").width($(this).innerWidth() + 180);

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

            function detach() {
                $("#newreceipt").validationEngine('detach');

            }

            function DateFormat(field, rules, i, options) {
                var regex = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
                if (!regex.test(field.val())) {
                    return "Please enter date in dd/MM/yyyy format."
                }
            }

            function chkdecim(field, rules, i, options) {
                var regex = /^\d+\.?\d{0,2}$/;
                if (!regex.test(field.val())) {
                    return "Please enter a Valid Number."
                }

            }

            function InitAutoComplacn() {


                $("#<%=tdate.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    showDropdowns: false,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=tdate.ClientID%>").val(start.format('DD-MM-YYYY'));
                });
                
                $("#<%=txtReprintDate.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    showDropdowns: false,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtReprintDate.ClientID%>").val(start.format('DD-MM-YYYY'));
                });


                $("#<%=txtacn.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/membersearch.asmx/GetDeposit") %>',
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
                                        image: item.ImageURL,
                                        product: item.Product,
                                        amt: item.Amount,
                                    }
                                }))
                            }
                        });
                    },
                    minLength: 1,
                    focus: function (event, ui) {

                        $("#<%=txtacn.ClientID %>").val(ui.item.memberno);
                        return false;

                    },
                    open: function () {
                        $("ul.ui-menu").width($(this).innerWidth() + 210);

                    },

                    select: function (event, ui) {

                        $("#<%=txtacn.ClientID %>").val(ui.item.memberno);

                        $("#<%=txtacn.ClientID %>").removeData('autocomplete');

                        return false;
                    }
                })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                            .append("<div>")
                            .append("<div class='img-div column'>")
                            .append("<img class='userimage' src='" + item.image + "' class='img-rounded'  />")
                            .append("</div")
                            .append("<div class='info-div column'>")
                            .append("<div class='username'>" + item.firstname + "</div>")
                            .append("<div class='userinfo'>" + item.lastname + "</div>")
                            .append("<div class='userinfo'>" + item.address + "</div>")
                            .append("<div class='userinfo1'>" + item.amt + " </div>")
                            .append("</br>")
                            .append("</div")
                            .append("</div>")
                            .appendTo(ul);
                    };
            }

            function PrintCC() {



                $('#vouchprint').printThis({
                    importCSS: false,
                    importStyle: true,         // import style tags
                    printContainer: true,


                });


            }

            function PrintOC() {



                $("#<%=lblcptr.ClientID%>").text("OFFICE COPY");
                $("#<%=lblccptr.ClientID%>").text("OFFICE COPY");


                $('#vouchprint').printThis({
                    importCSS: false,
                    importStyle: true,
                    printContainer: true,



                });
            }

            function PrintOCReprint() {
                $(".reprint-copy-type").text("OFFICE COPY");
                $('#reprintvouchprint').printThis({
                    importCSS: false,
                    importStyle: true,
                    printContainer: true,
                });
            }

            function PrintCCReprint() {
                $(".reprint-copy-type").text("CUSTOMER COPY");
                $('#reprintvouchprint').printThis({
                    importCSS: false,
                    importStyle: true,
                    printContainer: true,
                });
            }


        </script>

        <style>
            .product {
                z-index: 10;
                position: absolute;
                right: 10px;
                top: 21px;
                font-size: 12px;

            }

            .memno {
                z-index: 10;
                position: absolute;
                right: 10px;
                top: 10px;
                font-size: 12px;
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

            .userinfo1 {

                margin: 0px;
                padding-top: 10px;
                font-size: 12px;
            }


            .ui-autocomplete {
                max-height: 510px;
                overflow-y: auto;
                /* prevent horizontal scrollbar */
                overflow-x: hidden;
            }
        </style>



    </asp:Content>
