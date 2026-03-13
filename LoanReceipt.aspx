<%@ Page Title="Receipt - Loans" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="LoanReceipt.aspx.vb" Inherits="Fiscus.LoanReceipt" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="css/daterangepicker.css" rel="stylesheet" />
        <link href="../css/ValidationEngine.css" rel="stylesheet" />
        <link href="../css/validationEngine.jquery.css" rel="stylesheet" />
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

                .page-wrapper .page-content {
                    margin-top: 70px !important;
                }

            }

            /* Round Green Checkbox Styles */
            .custom-chk-container {
                display: block;
                position: relative;
                padding-left: 30px;
                cursor: pointer;
                font-size: 14px;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
                line-height: 20px;
            }

            .custom-chk-container input {
                position: absolute;
                opacity: 0;
                cursor: pointer;
                height: 0;
                width: 0;
            }

            .chk-checkmark {
                position: absolute;
                top: 0;
                left: 0;
                height: 20px;
                width: 20px;
                background-color: #E5E7EB;
                border-radius: 50%;
                transition: all 0.2s;
            }

            .custom-chk-container:hover input ~ .chk-checkmark {
                background-color: #D1D5DB;
            }

            .custom-chk-container input:checked ~ .chk-checkmark {
                background-color: #10B981;
            }

            .chk-checkmark:after {
                content: "";
                position: absolute;
                display: none;
            }

            .custom-chk-container input:checked ~ .chk-checkmark:after {
                display: block;
            }

            .custom-chk-container .chk-checkmark:after {
                left: 7px;
                top: 3px;
                width: 5px;
                height: 10px;
                border: solid white;
                border-width: 0 2px 2px 0;
                -webkit-transform: rotate(45deg);
                -ms-transform: rotate(45deg);
                transform: rotate(45deg);
            }
        </style>

    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div style="font-family: 'Inter', sans-serif;">

        <form id="frmrcpt" name="frmrcpt" runat="server">

            <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

            <nav class="page-breadcrumb" style="margin-bottom: 24px;">
                <ol class="breadcrumb" style="background: transparent; padding: 0;">
                    <li class="breadcrumb-item"><a href="#" style="color: #6B7280; text-decoration: none;">Loan</a></li>
                    <li class="breadcrumb-item active" style="color: #374151; font-weight: 500;">Receipt</li>
                    <li class="breadcrumb-item ms-auto">
                        <button type="button" class="btn shr-btn-secondary btn-sm" data-toggle="modal" data-target="#reprintModal" style="padding: 6px 16px !important; font-size: 13px !important;">Reprint Receipt</button>
                    </li>
                </ol>
            </nav>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <div class="row">

                        <div class="col-md-12  card card-body">
                            <asp:Panel runat="server" ID="pnltrans" style="display:block">
                                <div class="form-group row">

                                    <%-- <div class="col-md-8 ">

                                        <div class="form-group row ">
                                            <label class="col-md-2 col-form-label ">Account No</label>
                                            <div class="col-md-5">
                                                <div class="input-group">

                                                    <asp:TextBox ID="txtacn" runat="server" CssClass="form-control"
                                                        AutoPostBack="true"></asp:TextBox>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton runat="server" ID="soa" Enabled="false">
                                                            <span
                                                                class="input-group-addon input-group-text text-primary"
                                                                style="height:35px;cursor:pointer">

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


                                        </div>

                                        <asp:Panel runat="server" ID="pnlact" Visible="false">

                                            <div class="nk-block-between-md g-4 mb-3 border-bottom pb-3">
                                                <div class="nk-block-head" style="display:flex; flex-direction:row">
                                                    <div class="nk-block-head-content">
                                                        <h4 class="nk-block-title fw-normal text-primary">
                                                            <asp:Label runat="server" ID="lblname" />
                                                        </h4>
                                                        <div class="nk-block-des text-muted">
                                                            <asp:Label runat="server" ID="lbladd" />
                                                        </div>
                                                        <div class="nk-block-des text-muted">
                                                            <asp:Label runat="server" ID="lblmobile" />
                                                        </div>
                                                    </div>
                                                    <div class="nk-block-des">
                                                        <asp:Image id="imgCapture" runat="server" Visible="false"
                                                            style="margin-left:30px" Width="80px" Height="80px"
                                                            CssClass="img-thumbnail" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label class="col-md-2 col-form-label ">Date</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tdate"
                                                        AutoPostBack="True"
                                                        data-validation-engine="validate[required,funcCall[DateFormat[]]"
                                                        data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group row ">
                                                <label class="col-md-2 col-form-label ">Loan</label>
                                                    <asp:Label ID="lblproduct" CssClass="text-danger mr-2"
                                                        runat="server" style="margin-right:0.5rem" />
                                                    <asp:Label ID="lblsch" CssClass="badge badge-outline badge-primary"
                                                        Font-Size="12px" Font-Bold="true" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group row border-bottom pb-2">
                                                <label class="col-md-2 col-form-label ">Loan Amount</label>
                                                <asp:Label ID="lblamt"
                                                    CssClass="col-form-label col-md-3 text-danger font-weight-bold"
                                                    runat="server" />
                                                <label class="col-md-3 col-form-label ">Account Balance</label>
                                                <asp:Label ID="lblbal"
                                                    CssClass="col-form-label col-md-3 text-danger font-weight-bold"
                                                    runat="server" />
                                            </div>
                                            <div class="form-group row pt-2">
                                                <label class="col-md-3 col-form-label ">Receipt</label>
                                                <div class="col-md-5">


                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtamt"
                                                        AutoPostBack="true"
                                                        data-validation-engine="validate[required,custom[integer]"
                                                        data-errormessage-value-missing="Amount Missing"
                                                        data-errormessage-custom-error="Enter a Valid Amount">
                                                    </asp:TextBox>


                                                </div>
                                            </div>
                                            <asp:Panel runat="server" ID="jlrecovery" Visible="False">

                                                <table class="tab-box " style="float:left;margin-left:50px ">
                                                    <tbody>
                                                        <tr style="height:50px">
                                                            <td
                                                                style="border:none;font-variant-caps:small-caps;width:200px;color:red ">
                                                                Auction Loss</td>
                                                            <td style="border:none">
                                                                <asp:Label ID="lbl_acn_loss" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:50px">
                                                            <td
                                                                style="border:none;font-variant-caps:small-caps;color:red">
                                                                Recovery</td>
                                                            <td style="border:none">
                                                                <asp:TextBox CssClass="form-control " Width="130px"
                                                                    ID="txt_jla" runat="server" AutoPostBack="True">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:50px">
                                                            <td
                                                                style="border:none;font-variant-caps:small-caps;color:red">
                                                                Narration</td>
                                                            <td style="border:none">
                                                                <asp:textbox CssClass="form-control " Width="250px"
                                                                    ID="txtnar" runat="server"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:50px">
                                                            <td
                                                                style="border:none;font-variant-caps:small-caps;width:200px;color:red ">
                                                                Actual Amount</td>
                                                            <td style="border:none">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:Label ID="lbl_actual" runat="server">
                                                                        </asp:Label>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>

                                                        </tr>

                                                    </tbody>
                                                </table>


                                            </asp:Panel>
                                            <div class="form-group row ">
                                                <label class="col-md-3 col-form-label ">Payment Mode</label>
                                                <div class="col-md-5">
                                                    <asp:DropDownList ID="mop" runat="server" AutoPostBack="true"
                                                        CssClass="form-control ">
                                                        <asp:ListItem>Cash</asp:ListItem>
                                                        <asp:ListItem>Account</asp:ListItem>
                                                        <asp:ListItem>Transfer</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlsbtrf" runat="server" Visible="false">
                                                <asp:UpdatePanel ID="sbtrf" runat="server">
                                                    <ContentTemplate>

                                                        <div class="form-group row ">

                                                            <asp:Label ID="lblsb" CssClass="col-md-3 col-form-label "
                                                                Text="Account No" runat="server" Visible="false">
                                                            </asp:Label>



                                                            <div class="col-md-5">
                                                                <asp:TextBox ID="txt_sb" runat="server" Visible="false"
                                                                    CssClass="form-control" AutoPostBack="true"
                                                                    data-validation-engine="validate[required]"
                                                                    data-errormessage-value-missing="Valid Account No is required!">
                                                                </asp:TextBox>



                                                            </div>
                                                            <div class="col-sm-2">

                                                                <asp:Label runat="server" ID="lbl_sb_bal"></asp:Label>
                                                            </div>


                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </asp:Panel>
                                            <asp:Panel ID="pnltran" runat="server" Visible="false">
                                                <div class="form-group row ">
                                                    <asp:Label ID="lbl" CssClass="col-md-3 col-form-label "
                                                        Text="Transfer To" runat="server" Visible="false"></asp:Label>

                                                    <div class="col-md-5">
                                                        <asp:UpdatePanel ID="lstbnk" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="bnk" CssClass="form-control "
                                                                    Width="200px" runat="server" Visible="false">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class="form-group row  ">
                                                <label class="col-md-3 col-form-label ">Notes</label>
                                                <div class="col-md-5">
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtnotes">
                                                    </asp:TextBox>
                                                </div>
                                            </div>

                                        </asp:Panel>
                                </div> --%>

                                <div class="col-md-8"
                                    style="padding:18px 22px; background:#f6f8fc; border-radius:10px;">

                                    <!-- ACCOUNT SEARCH -->
                                    <div class="card"
                                        style="background: white; border-radius: 16px; box-shadow: 0 10px 30px rgba(0,0,0,0.05); border: none; padding: 24px; margin-bottom: 24px;">
                                        <div class="form-group row" style="margin-bottom: 0; align-items: center;">
                                            <label class="col-sm-4 col-form-label" for="txtacn"
                                                style="font-size: 15px; font-weight: 700; color: #111827; margin-bottom: 0;">
                                                Account Number
                                            </label>
                                            <div class="col-sm-8">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtacn" runat="server" CssClass="form-control"
                                                        AutoPostBack="true"
                                                        style="border-radius: 8px 0 0 8px; border: 1px solid #E5E7EB; border-right: none; padding: 10px 16px; font-size: 15px; font-weight: 600; transition: all 0.2s; height: 46px;"
                                                        onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                                        onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                                    </asp:TextBox>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton runat="server" ID="soa" Enabled="false">
                                                            <span class="input-group-text"
                                                                style="height: 46px; background: #F8FAFC; border: 1px solid #E5E7EB; border-left: none; border-radius: 0 8px 8px 0; color: #1e293b; font-size: 20px; font-weight: 900; padding: 0 15px; cursor: pointer; transition: all 0.2s;">
                                                                &#128196;
                                                            </span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <asp:Panel runat="server" ID="pnlact" Visible="false">

                                        <!-- CUSTOMER CARD -->
                                        <div
                                            style="background:#fff; padding:16px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05); margin-bottom:18px; display:flex; justify-content:space-between; align-items:center;">

                                            <div>
                                                <div style="font-size:16px; font-weight:600; color:#1e293b;">
                                                    <asp:Label runat="server" ID="lblname" />
                                                </div>

                                                <div style="font-size:12px; color:#64748b;">
                                                    <asp:Label runat="server" ID="lbladd" />
                                                </div>

                                                <div style="font-size:12px; color:#64748b;">
                                                    <asp:Label runat="server" ID="lblmobile" />
                                                </div>
                                            </div>

                                            <%-- Jewel images (from jlspec) shown before member photo --%>
                                            <asp:PlaceHolder ID="phJewelImages" runat="server" />

                                            <asp:Image id="imgCapture" runat="server" Visible="false"
                                                style="width:60px; height:60px; border-radius:50%; object-fit:cover;" />
                                        </div>


                                        <!-- DATE + LOAN TYPE -->
                                        <div
                                            style="background:#fff; padding:16px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05); margin-bottom:18px;">

                                            <div style="display:flex; gap:20px; align-items:end;">

                                                <div style="flex:1;">
                                                    <label style="font-size:11px; color:#94a3b8;">DATE</label>
                                                    <asp:TextBox runat="server" CssClass="form-control"
                                                        style="height:38px; margin-top:4px; border-radius:6px; font-size:13px;"
                                                        ID="tdate" AutoPostBack="True">
                                                    </asp:TextBox>
                                                </div>

                                                <div style="flex:1;">
                                                    <label style="font-size:11px; color:#94a3b8;">LOAN</label>
                                                    <div style="margin-top:6px; font-size:13px;">
                                                        <asp:Label ID="lblproduct" runat="server"
                                                            style="color:#dc2626; font-weight:600;" />
                                                        <span
                                                            style="margin-left:8px; font-size:11px; background:#eef2ff; padding:3px 8px; border-radius:15px;">
                                                            <asp:Label ID="lblsch" runat="server" />
                                                        </span>
                                                        <asp:Label ID="lblShareHolderNew" runat="server" Visible="False"
                                                            style="margin-left:8px; font-size:11px; background:#dcfce7; color:#166534; padding:3px 8px; border-radius:15px; font-weight:bold;">
                                                            ShareHolder Other Branch
                                                        </asp:Label>
                                                    </div>
                                                </div>

                                            </div>

                                            <!-- AMOUNT ROW -->
                                            <div style="display:flex; gap:15px; margin-top:16px;">

                                                <div
                                                    style="flex:1; background:#f8fafc; padding:12px; border-radius:8px;">
                                                    <div style="font-size:11px; color:#94a3b8;">Loan Amount</div>
                                                    <div style="font-size:15px; font-weight:700; color:#dc2626;">
                                                        <asp:Label ID="lblamt" runat="server" />
                                                    </div>
                                                </div>

                                                <div
                                                    style="flex:1; background:#f8fafc; padding:12px; border-radius:8px;">
                                                    <div style="font-size:11px; color:#94a3b8;">Balance</div>
                                                    <div style="font-size:15px; font-weight:700; color:#0f766e;">
                                                        <asp:Label ID="lblbal" runat="server" />
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <asp:Panel runat="server" ID="jlrecovery" Visible="False">
                                            <div
                                                style="background:#fff; padding:16px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05); margin-bottom:18px;">
                                                <div
                                                    style="font-size:12px; font-weight:600; color:#dc2626; margin-bottom:12px;">
                                                    AUCTION RECOVERY</div>

                                                <div style="display:flex; flex-direction:column; gap:12px;">
                                                    <div
                                                        style="display:flex; justify-content:space-between; align-items:center;">
                                                        <span
                                                            style="font-size:13px; color:#64748b; font-variant-caps:small-caps;">Auction
                                                            Loss</span>
                                                        <asp:Label ID="lbl_acn_loss" runat="server"
                                                            style="font-weight:600; color:#0f766e;"></asp:Label>
                                                    </div>

                                                    <div
                                                        style="display:flex; justify-content:space-between; align-items:center;">
                                                        <span
                                                            style="font-size:13px; color:#64748b; font-variant-caps:small-caps;">Recovery</span>
                                                        <asp:TextBox CssClass="form-control"
                                                            style="width:130px; height:36px; border-radius:6px; font-size:13px;"
                                                            ID="txt_jla" runat="server" AutoPostBack="True">
                                                        </asp:TextBox>
                                                    </div>

                                                    <div
                                                        style="display:flex; justify-content:space-between; align-items:center;">
                                                        <span
                                                            style="font-size:13px; color:#64748b; font-variant-caps:small-caps;">Narration</span>
                                                        <asp:TextBox CssClass="form-control"
                                                            style="width:200px; height:36px; border-radius:6px; font-size:13px;"
                                                            ID="txtnar" runat="server"></asp:TextBox>
                                                    </div>

                                                    <div
                                                        style="display:flex; justify-content:space-between; align-items:center; border-top:1px solid #f1f5f9; padding-top:12px;">
                                                        <span
                                                            style="font-size:13px; color:#64748b; font-variant-caps:small-caps;">Actual
                                                            Amount</span>
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lbl_actual" runat="server"
                                                                    style="font-weight:700; color:#0f766e;"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <!-- RECEIPT + PAYMENT IN ONE ROW -->
                                        <div
                                            style="background:#fff; padding:16px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05); margin-bottom:18px;">

                                            <div style="display:flex; gap:20px;">

                                                <div style="flex:1;">
                                                    <label style="font-size:11px; color:#94a3b8;">RECEIPT
                                                        AMOUNT</label>
                                                    <asp:TextBox runat="server" CssClass="form-control"
                                                        style="height:40px; margin-top:4px; font-size:14px; border-radius:6px;"
                                                        ID="txtamt" AutoPostBack="true">
                                                    </asp:TextBox>
                                                </div>

                                                <div style="flex:1;">
                                                    <label style="font-size:11px; color:#94a3b8;">PAYMENT
                                                        MODE</label>
                                                    <asp:DropDownList ID="mop" runat="server" CssClass="form-control"
                                                        AutoPostBack="true"
                                                        style="height:40px; margin-top:4px; border-radius:6px; font-size:13px;">
                                                        <asp:ListItem>Cash</asp:ListItem>
                                                        <asp:ListItem>Account</asp:ListItem>
                                                        <asp:ListItem>Transfer</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                            </div>
                                        </div>

                                        <asp:Panel ID="pnlsbtrf" runat="server" Visible="false">
                                            <div
                                                style="background:#fff; padding:16px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05); margin-bottom:18px;">
                                                <asp:UpdatePanel ID="sbtrf" runat="server">
                                                    <ContentTemplate>
                                                        <div style="display:flex; gap:20px; align-items:center;">
                                                            <div style="flex:1;">
                                                                <asp:Label ID="lblsb"
                                                                    style="font-size:11px; color:#94a3b8; font-weight:600;"
                                                                    Text="SB ACCOUNT NO" runat="server" Visible="false">
                                                                </asp:Label>
                                                                <asp:TextBox ID="txt_sb" runat="server" Visible="false"
                                                                    CssClass="form-control" AutoPostBack="true"
                                                                    style="height:40px; margin-top:4px; border-radius:6px; font-size:13px;"
                                                                    data-validation-engine="validate[required]"
                                                                    data-errormessage-value-missing="Valid Account No is required!">
                                                                </asp:TextBox>
                                                            </div>
                                                            <div style="flex:1; margin-top:16px;">
                                                                <asp:Label runat="server" ID="lbl_sb_bal"
                                                                    style="font-size:15px; font-weight:700; color:#0f766e;">
                                                                </asp:Label>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnltran" runat="server" Visible="false">
                                            <div
                                                style="background:#fff; padding:16px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05); margin-bottom:18px;">
                                                <div style="display:flex; gap:20px; align-items:center;">
                                                    <div style="flex:1;">
                                                        <asp:Label ID="lbl"
                                                            style="font-size:11px; color:#94a3b8; font-weight:600;"
                                                            Text="TRANSFER TO" runat="server" Visible="false">
                                                        </asp:Label>
                                                        <asp:UpdatePanel ID="lstbnk" runat="server"
                                                            style="margin-top:4px;">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="bnk" CssClass="form-control"
                                                                    style="height:40px; border-radius:6px; font-size:13px;"
                                                                    runat="server" Visible="false">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div style="flex:1;"></div>
                                                </div>
                                            </div>
                                        </asp:Panel>


                                        <!-- NOTES -->
                                        <div
                                            style="background:#fff; padding:14px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05);">

                                            <label style="font-size:11px; color:#94a3b8;">NOTES</label>

                                            <asp:TextBox runat="server" CssClass="form-control"
                                                style="height:36px; margin-top:4px; border-radius:6px; font-size:13px;"
                                                ID="txtnotes">
                                            </asp:TextBox>

                                        </div>

                                    </asp:Panel>

                                </div>

                                <div class="col-md-4">
                                    <asp:Panel runat="server" ID="pnlint" Visible="false">
                                        <div class="card card-body" id="colgrp">

                                            <div class="form-group row">
                                                <div class="btn-group align-content-between" role="group">

                                                    <button class="btn btn-outline-primary" type="button"
                                                        style="font-size:12px; padding:4px 10px;" data-toggle="collapse"
                                                        data-target="#dint" aria-expanded="true" aria-controls="dint">
                                                        D-Interest
                                                    </button>

                                                    <button class="btn btn-outline-primary" type="button"
                                                        style="font-size:12px; padding:4px 10px;" data-toggle="collapse"
                                                        data-target="#cint" aria-expanded="false" aria-controls="cint">
                                                        C-Interest
                                                    </button>

                                                    <button class="btn btn-outline-primary" type="button"
                                                        style="font-size:12px; padding:4px 10px;" data-toggle="collapse"
                                                        data-target="#othrs" aria-expanded="false"
                                                        aria-controls="othrs">
                                                        Others
                                                    </button>

                                                    <button class="btn btn-outline-primary" type="button"
                                                        style="font-size:12px; padding:4px 10px;" data-toggle="collapse"
                                                        data-target="#postalpurpose" aria-expanded="false"
                                                        aria-controls="postalpurpose">
                                                        Postal Purpose
                                                    </button>

                                                </div>
                                            </div>




                                            <div class="collapse show" id="dint" data-parent="#colgrp">

                                                <div class="form-group row ">
                                                    <label class="col-md-5 text-primary ">Total Credit</label>
                                                    <asp:Label runat="server" style="float:right;text-align:right"
                                                        CssClass="col-md-7 " Text="890.00" ID="lbldcr">
                                                    </asp:Label>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-5  text-primary ">Total Debit</label>
                                                    <asp:Label runat="server" style="float:right;text-align:right"
                                                        CssClass="col-md-7 text-align-right" Text="90.00" ID="lblddr">
                                                    </asp:Label>
                                                </div>
                                                <div class="form-group row border-bottom ">
                                                    <label class="col-md-5  text-primary ">Balance</label>

                                                    <asp:Label runat="server" style="float:right;text-align:right "
                                                        Width="120px" CssClass="col-md-7 " Text="800.00" ID="lbldbal">
                                                    </asp:Label>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-5  text-danger ">Period</label>
                                                    <asp:Label runat="server" CssClass="col-md-7" Width="120px"
                                                        style="float:right;text-align:right  " Text="0.00"
                                                        ID="lblperiod_d"></asp:Label>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-5  text-danger ">Rate of
                                                        Interest</label>
                                                    <asp:Label runat="server" CssClass="col-md-3  text-danger "
                                                        style="float:right;font-size:small;text-align:right " Text="0%"
                                                        ID="lblrebated"></asp:Label>
                                                    <asp:Label runat="server" CssClass="col-md-4" Width="120px"
                                                        style="float:right;text-align:right  " Text="0%" ID="lblroi_d">
                                                    </asp:Label>


                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-5 col-form-label   text-danger ">Interest
                                                        Amount</label>
                                                    <div class="col-md-7">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox runat="server" CssClass="form-control "
                                                                    AutoPostBack="true" ID="lblactualint_d"
                                                                    style="float:right;text-align:right;" Text="0.00"
                                                                    Width="110px"></asp:TextBox>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>




                                            </div>

                                            <div class="collapse" id="cint" data-parent="#colgrp">


                                                <div class="form-group row ">
                                                    <label class="col-md-5  text-danger ">Total Credit</label>
                                                    <asp:Label runat="server" style="float:right;text-align:right  "
                                                        CssClass="col-md-7" Text="0.00" ID="lblccr"></asp:Label>
                                                </div>
                                                <div class="form-group row ">
                                                    <label class="col-md-5  text-danger ">Total Debit</label>
                                                    <asp:Label runat="server" style="float:right;text-align:right "
                                                        Text="0.00" CssClass="col-sm-7" ID="lblcdr"></asp:Label>
                                                </div>
                                                <div class="form-group row ">
                                                    <label class="col-md-5  text-danger ">Balance</label>
                                                    <asp:Label runat="server" Width="120px"
                                                        style="float:right;text-align:right  " CssClass="col-md-7"
                                                        Text="0.00" ID="lblcbal">
                                                    </asp:Label>
                                                </div>

                                                <div class="form-group row  border-top ">
                                                    <label class="col-md-5  text-primary ">Period</label>
                                                    <asp:Label runat="server" CssClass="col-md-7"
                                                        style="float:right;text-align:right  " Text="0.00"
                                                        ID="lblperiod"></asp:Label>
                                                </div>
                                                <div class="form-group row ">
                                                    <label class="col-md-5  text-primary ">Rate of
                                                        Interest</label>
                                                    <asp:Label runat="server"
                                                        style="float:right;font-size:small;text-align:right "
                                                        CssClass="col-md-3" Text="0%" ID="lblrebateC">
                                                    </asp:Label>
                                                    <asp:Label runat="server" style="float:right;text-align:right  "
                                                        CssClass="col-md-3" Text="0%" ID="lblroi"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="form-group row ">

                                                    <label class="col-md-5 col-form-label  text-primary ">Interest
                                                        Amount</label>
                                                    <div class="col-md-7">
                                                        <asp:TextBox CssClass="form-control " runat="server"
                                                            style="float:right " Text="0.00" ID="lblactualint">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>


                                            <div class="collapse" id="othrs" data-parent="#colgrp">

                                                <div class="form-group row ">
                                                    <label class="col-md-5 col-form-label text-danger ">Others</label>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="txtothers" AutoPostBack="true"
                                                            CssClass=" form-control " runat="server">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row ">
                                                    <label
                                                        class="col-md-5 col-form-label text-danger ">Insurance</label>
                                                    <div class="col-md-7">
                                                        <asp:TextBox ID="txtins" AutoPostBack="true"
                                                            CssClass="form-control " runat="server">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="collapse" id="postalpurpose" data-parent="#colgrp">
                                                <div class="form-group row ">
                                                    <label class="col-md-5 col-form-label text-primary">Target
                                                        Date</label>
                                                    <div class="col-md-7">
                                                        <asp:TextBox runat="server" CssClass="form-control"
                                                            ID="txtPostalDate" TextMode="Date"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row ">
                                                    <div class="col-md-12 text-right">
                                                        <asp:Button ID="btnCalcPostal" runat="server" Text="Calculate"
                                                            CssClass="btn btn-sm btn-primary" style="margin-top:8px"
                                                            OnClick="btnCalcPostal_Click" />
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel ID="updPostal" runat="server">
                                                    <ContentTemplate>
                                                        <div class="form-group row ">
                                                            <label class="col-md-5 text-primary ">Total
                                                                Credit</label>
                                                            <asp:Label runat="server"
                                                                style="float:right;text-align:right" CssClass="col-md-7"
                                                                Text="0.00" ID="lblPostalCredit"></asp:Label>
                                                        </div>
                                                        <div class="form-group row">
                                                            <label class="col-md-5 text-danger ">Total Debit</label>
                                                            <asp:Label runat="server"
                                                                style="float:right;text-align:right"
                                                                CssClass="col-md-7 text-align-right" Text="0.00"
                                                                ID="lblPostalDebit"></asp:Label>
                                                        </div>
                                                        <div class="form-group row border-bottom">
                                                            <label class="col-md-5 text-primary ">Balance</label>
                                                            <asp:Label runat="server"
                                                                style="float:right;text-align:right" CssClass="col-md-7"
                                                                Text="0.00" ID="lblPostalBalance"></asp:Label>
                                                        </div>
                                                        <div class="form-group row">
                                                            <label class="col-md-5 text-danger ">Period</label>
                                                            <asp:Label runat="server" CssClass="col-md-7"
                                                                style="float:right;text-align:right" Text="0 Days"
                                                                ID="lblPostalPeriod"></asp:Label>
                                                        </div>
                                                        <div class="form-group row">
                                                            <label class="col-md-5 text-danger ">ROI</label>
                                                            <asp:Label runat="server" CssClass="col-md-7"
                                                                style="float:right;text-align:right" Text="0%"
                                                                ID="lblPostalROI"></asp:Label>
                                                        </div>
                                                        <div class="form-group row">
                                                            <label class="col-md-5 text-danger ">Interest
                                                                Amount</label>
                                                            <asp:Label runat="server" CssClass="col-md-7"
                                                                style="float:right;text-align:right; font-weight:bold"
                                                                Text="0.00" ID="lblPostalInterest"></asp:Label>
                                                        </div>
                                                        <div class="form-group row"
                                                            style="background:#fffcf6; padding-top:8px; border-top:1px solid #f1f5f9;">
                                                            <label class="col-md-5 text-danger"
                                                                style="font-weight:bold">Final Amount</label>
                                                            <asp:Label runat="server" CssClass="col-md-7 text-danger"
                                                                style="float:right;text-align:right; font-weight:bold; font-size:16px;"
                                                                Text="0.00" ID="lblPostalFinalAmount"></asp:Label>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnCalcPostal"
                                                            EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>

                                        </div>


                                </div>
                            </asp:Panel>
                        </div>

                    </div>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlbtns">
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
                    <asp:HiddenField runat="server" ID="damt" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="dword" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="camt" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="cword" ClientIDMode="Static" />

                    <asp:UpdatePanel runat="server" ID="pnlbtns">
                        <ContentTemplate>


                            <asp:Panel runat="server" ID="pnlbtn" Visible="false">
                                <div class="form-group row border-sm-top ">
                                    <div class="col-sm-4"></div>
                                    <div class="col-sm-4">
                                        <asp:Button ID="btn_up_can" style="margin-left:40px" runat="server"
                                            CssClass="btn-outline-secondary  btn" Text="Clear" />
                                        <asp:Button ID="btn_up_rcpt" runat="server" CssClass="btn-outline-primary btn"
                                            Text="Save" />
                                    </div>
                                </div>
                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlprnt" style="display:none">

                        <asp:UpdatePanel runat="server" ID="pnlbtnprnt" Visible="true">
                            <ContentTemplate>
                                <div class="form-group row border-bottom  ">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-6">
                                        <div class="btn-group" role="group">
                                            <asp:Button runat="server" ID="btntog" Class="btn btn-outline-primary"
                                                Text="Close" />
                                            <button type="button" Class="btn btn-outline-primary"
                                                onclick="PrintOC();">Print Office Copy</button>
                                            <button type="button" Class="btn btn-outline-primary"
                                                onclick="PrintCC();">Print Customer Copy</button>
                                        </div>
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
                                                            <td style="width: 10%;"><img src="Images/KBF-LOGO.png"
                                                                    alt="" width="72" height="60" /></td>
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
                                                                                    Karavilai,Villukuri P.
                                                                                    O</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100%;font-size:12px;">
                                                                                Branch : <asp:Label id="pbranch"
                                                                                    runat="server"></asp:Label>
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
                                                                                <asp:label runat="server" id="lblcpt">
                                                                                </asp:label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2"
                                                                                style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 ">
                                                                                <asp:label runat="server" id="lblcptr">
                                                                                </asp:label>
                                                                            </td>
                                                                        </tr>

                                                                        <tr style="height: 21px;">
                                                                            <td
                                                                                style="width: 82px; height: 21px;font-size: 15px">
                                                                                No&nbsp; &nbsp; :</td>
                                                                            <td
                                                                                style="width: 90px; height: 21px;font-size: 15px; font-weight: bold;">
                                                                                <asp:Label ID="pvno" runat="server">
                                                                                </asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 21px;">
                                                                            <td
                                                                                style="width: 82px; height: 21px;font-size: 12px">
                                                                                Date :</td>
                                                                            <td
                                                                                style="width: 90px; height: 21px;font-size: 12px">
                                                                                <asp:Label ID="pdate" runat="server">
                                                                                </asp:Label>
                                                                            </td>
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
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Account No</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <asp:Label ID="pacno" runat="server">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                    <tr
                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Account</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <b>
                                                                                <asp:Label ID="pglh" runat="server">
                                                                                </asp:Label>
                                                                            </b>
                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 30px;">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Member No</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <asp:Label ID="pcid" runat="server">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                    <tr
                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Customer Name</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <b>
                                                                                <asp:Label ID="pcname" runat="server">
                                                                                </asp:Label>
                                                                            </b>
                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 30px;">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Amount</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size:15px">
                                                                            <b>
                                                                                <asp:Label ID="pamt" runat="server">
                                                                                </asp:Label>
                                                                            </b>

                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 25px;">
                                                                        <td colspan="3"
                                                                            style="font-size: 12px; width: 423px; height: 21px;">
                                                                            <asp:Label runat="server" id="pnar">
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
                                                        <td colspan="2" style="padding-left:5px">
                                                            Amount in Words :&nbsp;<asp:Label ID="paiw" runat="server">
                                                            </asp:Label>

                                                        </td>
                                                    </tr>
                                                </table>





                                                <table style="height:15px;margin-top:10px;width:100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width:35%; text-align: center;font-size: 14px">
                                                                Incharge / Manager</td>

                                                            <td style="width: 30%; text-align: center;font-size: 14px">
                                                                Cashier</td>
                                                            <td style="width: 35%; text-align: center;font-size: 13px">
                                                                (&nbsp;<asp:Label runat="server" ID="premit">
                                                                </asp:Label>&nbsp;)</td>


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
                                                <table style="border-collapse: collapse; ">
                                                    <tbody>
                                                        <tr style="border: none;">
                                                            <td style="width: 10%;"><img src="Images/KBF-LOGO.png"
                                                                    alt="" width="72" height="60" /></td>
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
                                                                                    Karavilai,Villukuri P.
                                                                                    O</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100%;font-size:12px;">
                                                                                Branch : <asp:Label id="pcbranch"
                                                                                    runat="server"></asp:Label>
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
                                                                                <asp:label runat="server" id="lblccpt">
                                                                                </asp:label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2"
                                                                                style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 ">
                                                                                <asp:label runat="server" id="lblccptr">
                                                                                </asp:label>
                                                                            </td>
                                                                        </tr>

                                                                        <tr style="height: 21px;">
                                                                            <td
                                                                                style="width: 82px; height: 21px;font-size: 15px">
                                                                                No&nbsp; &nbsp; :</td>
                                                                            <td
                                                                                style="width: 90px; height: 21px;font-size: 15px; font-weight: bold;">
                                                                                <asp:Label ID="pcvno" runat="server">
                                                                                </asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 21px;">
                                                                            <td
                                                                                style="width: 82px; height: 21px;font-size: 12px">
                                                                                Date :</td>
                                                                            <td
                                                                                style="width: 90px; height: 21px;font-size: 12px">
                                                                                <asp:Label ID="pcdate" runat="server">
                                                                                </asp:Label>
                                                                            </td>
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
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Account No</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <asp:Label ID="pcacno" runat="server">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                    <tr
                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Account</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <b>
                                                                                <asp:Label ID="pcglh" runat="server">
                                                                                </asp:Label>
                                                                            </b>
                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 30px;">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Member No</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <asp:Label ID="pccid" runat="server">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                    <tr
                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Customer Name</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <b>
                                                                                <asp:Label ID="pccname" runat="server">
                                                                                </asp:Label>
                                                                            </b>
                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 30px;">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Amount</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size:15px">
                                                                            <b>
                                                                                <asp:Label ID="pcamt" runat="server">
                                                                                </asp:Label>
                                                                            </b>

                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 25px;">
                                                                        <td colspan="3"
                                                                            style="font-size: 12px; width: 423px; height: 21px;">
                                                                            <asp:Label runat="server" id="pcnar">
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
                                                        <td colspan="2" style="padding-left:5px">
                                                            Amount in Words :&nbsp;<asp:Label ID="pcaiw" runat="server">
                                                            </asp:Label>

                                                        </td>
                                                    </tr>
                                                </table>





                                                <table style="height:15px;margin-top:10px;width:100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width:35%; text-align: center;font-size: 14px">
                                                                Incharge / Manager</td>

                                                            <td style="width: 30%; text-align: center;font-size: 14px">
                                                                Cashier</td>
                                                            <td style="width: 35%; text-align: center;font-size: 13px">
                                                                (&nbsp;<asp:Label runat="server" ID="pcremit">
                                                                </asp:Label>&nbsp;)</td>


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
                                                <table style="border-collapse: collapse; ">
                                                    <tbody>
                                                        <tr style="border: none;">
                                                            <td style="width: 10%;"><img src="Images/KBF-LOGO.png"
                                                                    alt="" width="72" height="60" /></td>
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
                                                                                    Karavilai,Villukuri P.
                                                                                    O</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100%;font-size:12px;">
                                                                                Branch : <asp:Label id="ovr_branch"
                                                                                    runat="server"></asp:Label>
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
                                                                                <asp:label runat="server" id="ovr_type">
                                                                                </asp:label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2"
                                                                                style="width:172px; height: 21px; text-align: center;background-color:#eee;color:#000;font-weight:400 ">
                                                                                <asp:label runat="server" id="ovr_copy">
                                                                                </asp:label>
                                                                            </td>
                                                                        </tr>

                                                                        <tr style="height: 21px;">
                                                                            <td
                                                                                style="width: 82px; height: 21px;font-size: 15px">
                                                                                No&nbsp; &nbsp; :</td>
                                                                            <td
                                                                                style="width: 90px; height: 21px;font-size: 15px; font-weight: bold;">
                                                                                <asp:Label ID="ovr_no" runat="server">
                                                                                </asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 21px;">
                                                                            <td
                                                                                style="width: 82px; height: 21px;font-size: 12px">
                                                                                Date :</td>
                                                                            <td
                                                                                style="width: 90px; height: 21px;font-size: 12px">
                                                                                <asp:Label ID="ovr_dt" runat="server">
                                                                                </asp:Label>
                                                                            </td>
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
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Account No</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <asp:Label ID="ovr_acn" runat="server">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                    <tr
                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Account</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <b>
                                                                                <asp:Label ID="ovr_acc" runat="server"
                                                                                    Text="SAVINGS ACCOUNT">
                                                                                </asp:Label>
                                                                            </b>
                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 30px;">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Member No</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <asp:Label ID="ovr_cid" runat="server">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                    <tr
                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Customer Name</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                            <b>
                                                                                <asp:Label ID="ovr_name" runat="server">
                                                                                </asp:Label>
                                                                            </b>
                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 30px;">
                                                                        <td
                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                            Amount</td>
                                                                        <td style="width: 21px; height: 21px;">
                                                                        </td>
                                                                        <td
                                                                            style="width: 263px; height: 21px;font-size:15px">
                                                                            <b>
                                                                                <asp:Label ID="ovr_amt" runat="server">
                                                                                </asp:Label>
                                                                            </b>

                                                                        </td>
                                                                    </tr>

                                                                    <tr style="height: 25px;">
                                                                        <td colspan="3"
                                                                            style="font-size: 12px; width: 423px; height: 21px;">
                                                                            <asp:Label runat="server" id="Label11">
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
                                                        <td colspan="2" style="padding-left:5px">
                                                            Amount in Words :&nbsp;<asp:Label ID="ovr_aiw"
                                                                runat="server"></asp:Label>

                                                        </td>
                                                    </tr>
                                                </table>


                                                <table style="height:15px;margin-top:10px;width:100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width:35%; text-align: center;font-size: 14px">
                                                                Incharge / Manager</td>

                                                            <td style="width: 30%; text-align: center;font-size: 14px">
                                                                Cashier</td>
                                                            <td style="width: 35%; text-align: center;font-size: 13px">
                                                                (&nbsp;<asp:Label runat="server" ID="ovr_sign">
                                                                </asp:Label>&nbsp;)</td>


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
                    </div>
                    </div>


                    <asp:Panel runat="server" ID="pnlReprintResults" Visible="false">
                        <div class="row mt-4">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-header bg-light d-flex justify-content-between align-items-center">
                                        <h5 class="mb-0">Reprinted Receipt</h5>
                                        <div class="btn-group">
                                            <asp:LinkButton ID="btnReprintClose" runat="server" CssClass="btn btn-sm btn-outline-danger" OnClick="btnReprintClose_Click">Close</asp:LinkButton>
                                            <button type="button" class="btn btn-sm btn-outline-primary" onclick="PrintOCReprint();">Print Office Copy</button>
                                            <button type="button" class="btn btn-sm btn-outline-primary" onclick="PrintCCReprint();">Print Customer Copy</button>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <div id="reprintvouchprint">
                                            <asp:Repeater ID="rptReprint" runat="server">
                                                <SeparatorTemplate>
                                                    <div style="page-break-before: always; break-before: page;"></div>
                                                </SeparatorTemplate>
                                                <ItemTemplate>
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
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <div class="modal fade" id="reprintModal" role="dialog" aria-labelledby="reprintModalLabel" aria-hidden="true">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="reprintModalLabel">Search & Reprint Receipt</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <label>Transaction Date</label>
                                        <asp:TextBox runat="server" CssClass="form-control shr-input" ID="txtReprintDate" placeholder="Select Date"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>Transaction ID</label>
                                        <asp:TextBox runat="server" CssClass="form-control shr-input" ID="txtReprintTransID" placeholder="Enter Transaction ID"></asp:TextBox>
                                    </div>
                                    <div class="form-group mb-0 mt-3 d-flex align-items-center">
                                        <label class="custom-chk-container">
                                            <asp:CheckBox ID="chkServiceChargeReceipt" runat="server" />
                                            <span class="chk-checkmark"></span>
                                        </label>
                                        <span style="font-size: 14px; font-weight: 500; color: #4B5563; margin-left: 10px; cursor: pointer;" onclick="$('#<%=chkServiceChargeReceipt.ClientID%>').click();">Service charge receipt</span>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                    <asp:Button ID="btn_reprint_submit" runat="server" CssClass="btn btn-primary validate-skip" CausesValidation="false" Text="Search & Reprint" OnClientClick="if($('[id$=txtReprintDate]').val() == '' || $('[id$=txtReprintTransID]').val() == '') { alert('Please enter both Date and Transaction ID.'); return false; }" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>







        </form>



        <script src="../js/jquery.js" type="text/javascript"></script>

        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />

        <script src="js/daterangepicker.js" type="text/javascript"></script>
        <script src="../js/jquery.validationEngine-en.js" type="text/javascript"></script>
        <script src="../js/jquery.validationEngine.js" type="text/javascript"></script>

        <script src="js/printThis.js" type="text/javascript"></script>

        <script type="text/javascript">


            $(document).ready(function () {





                $("#frmrcpt").validationEngine('attach');
                $("#frmrcpt").validationEngine({ promptPosition: "topleft", scroll: false, focusFirstField: true, showArrow: true });
                $("#frmrcpt").validationEngine('attach', { promptPosition: "topleft", scroll: false, showArrow: true });

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

                $("#<%=txtReprintDate.ClientID%>").daterangepicker({
                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    showDropdowns: true,
                    "autoApply": true,
                    parentEl: "#reprintModal",
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {
                    $("#<%=txtReprintDate.ClientID%>").val(start.format('DD-MM-YYYY'));
                });
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
                    showDropdowns: true,
                    "autoApply": true,
                    parentEl: "#reprintModal",
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {
                    $("#<%=txtReprintDate.ClientID%>").val(start.format('DD-MM-YYYY'));
                });



                $("#<%=txtacn.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/membersearch.asmx/GetLoan") %>',
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
                $("#frmrcpt").validationEngine('detach');

            }


            function PrintCC() {


                damt = document.getElementById('<%= damt.ClientID%>').value;
                dword = document.getElementById('<%= dword.ClientID%>').value;

                $("#<%=lblcptr.ClientID%>").text("CUSTOMER COPY");
                $("#<%=lblccptr.ClientID%>").text("CUSTOMER COPY");
                $("#<%=pamt.ClientID %>").text(damt);
                $("#<%=paiw.ClientID %>").text(dword);
                $("#<%=pcamt.ClientID %>").text(damt);
                $("#<%=pcaiw.ClientID %>").text(dword);



                $('#vouchprint').printThis({
                    importCSS: false,
                    importStyle: true,         // import style tags
                    printContainer: true,


                });


            }

            function PrintOC() {


                camt = document.getElementById('<%= camt.ClientID%>').value;
                cword = document.getElementById('<%= cword.ClientID%>').value;

                $("#<%=lblcptr.ClientID%>").text("OFFICE COPY");
                $("#<%=lblccptr.ClientID%>").text("OFFICE COPY");
                $("#<%=pamt.ClientID %>").text(camt);
                $("#<%=paiw.ClientID %>").text(cword);
                $("#<%=pcamt.ClientID %>").text(camt);
                $("#<%=pcaiw.ClientID %>").text(cword);


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

        <%-- Jewel Image Lightbox Modal --%>
        <div id="jewelModal" onclick="closeJewelModal()"
            style="display:none; position:fixed; z-index:9999; top:0; left:0; width:100%; height:100%;
                   background:rgba(0,0,0,0.82); align-items:center; justify-content:center; cursor:zoom-out;">
            <div onclick="event.stopPropagation()"
                style="position:relative; background:#fff; border-radius:16px; padding:12px;
                       box-shadow:0 24px 60px rgba(0,0,0,0.5);">
                <button onclick="closeJewelModal()"
                    style="position:absolute; top:-14px; right:-14px; width:32px; height:32px;
                           border-radius:50%; border:none; background:#1e293b; color:#fff;
                           font-size:18px; line-height:1; cursor:pointer; box-shadow:0 2px 8px rgba(0,0,0,0.4);">
                    &times;
                </button>
                <img id="jewelModalImg" src=""
                    style="width:50vw; height:50vh; border-radius:10px; object-fit:contain; display:block;" />
            </div>
        </div>

        <script type="text/javascript">
            function openJewelModal(src) {
                document.getElementById('jewelModalImg').src = src;
                var m = document.getElementById('jewelModal');
                m.style.display = 'flex';
            }
            function closeJewelModal() {
                document.getElementById('jewelModal').style.display = 'none';
                document.getElementById('jewelModalImg').src = '';
            }
            document.addEventListener('keydown', function (e) {
                if (e.key === 'Escape') closeJewelModal();
            });
        </script>

        </div>
    </asp:Content>