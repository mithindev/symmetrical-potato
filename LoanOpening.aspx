<%@ Page Title="Loan Opening" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="LoanOpening.aspx.vb" Inherits="Fiscus.LoanOpening" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="css/daterangepicker.css" rel="stylesheet" />
        <link href="css/ValidationEngine.css" rel="stylesheet" />
        <link rel="stylesheet" type="text/css" href="css/smart_tab.min.css" />

        <style>
            .table-bordered>tbody>tr:first-child:hover {
                background-color: #fff !important;
            }

            /* Fintech Dashboard Styling for tab-1 only */
            .customer-card {
                background: #ffffff;
                border-radius: 10px;
                padding: 30px;
                border: 1px solid #e2e8f0;
                margin-top: 15px;
            }

            .section-title {
                color: #2d3748;
                font-weight: 700;
                font-size: 1.15rem;
                padding-bottom: 12px;
                border-bottom: 2px solid #f1f5f9;
                letter-spacing: 0.02em;
            }

            .customer-card .form-label {
                font-size: 0.85rem;
                font-weight: 600;
                color: #4a5568;
                margin-bottom: 6px;
                text-transform: uppercase;
                letter-spacing: 0.04em;
            }

            .customer-card .form-control {
                border-radius: 6px;
                border: 1px solid #cbd5e1;
                color: #1e293b;
                box-shadow: none;
                transition: border-color 0.2s, box-shadow 0.2s;
            }

            .customer-card .form-control:focus {
                border-color: #5a67d8;
                box-shadow: 0 0 0 3px rgba(90, 103, 216, 0.15);
            }

            .customer-card .form-control.bg-light {
                background-color: #f8fafc !important;
                border-color: #e2e8f0;
                color: #64748b;
                opacity: 1;
            }

            .customer-card .btn-primary {
                background-color: #5a67d8;
                border-color: #5a67d8;
                font-weight: 500;
                transition: transform 0.1s;
            }

            .customer-card .btn-primary:active {
                transform: translateY(1px);
            }

            .customer-card .btn-outline-secondary {
                color: #64748b;
                border-color: #cbd5e1;
            }

            .customer-card .btn-outline-secondary:hover {
                background-color: #f8fafc;
                color: #334155;
            }

            /* Custom Green Round Checkbox */
            .green-round-checkbox {
                display: inline-flex;
                align-items: center;
                margin-top: 8px;
            }
            .green-round-checkbox input[type="checkbox"] {
                appearance: none;
                -webkit-appearance: none;
                background-color: #fff;
                margin: 0;
                font: inherit;
                color: currentColor;
                width: 1.25em;
                height: 1.25em;
                border: 2px solid #38a169;
                border-radius: 50%;
                display: grid;
                place-content: center;
                cursor: pointer;
                transition: all 0.2s ease-in-out;
            }
            .green-round-checkbox input[type="checkbox"]::before {
                content: "";
                width: 0.65em;
                height: 0.65em;
                border-radius: 50%;
                transform: scale(0);
                transition: 120ms transform ease-in-out;
                box-shadow: inset 1em 1em white;
                background-color: #fff;
            }
            .green-round-checkbox input[type="checkbox"]:checked {
                background-color: #38a169;
            }
            .green-round-checkbox input[type="checkbox"]:checked::before {
                transform: scale(1);
            }
            .green-round-checkbox label {
                margin-left: 0.5rem;
                margin-bottom: 0;
                cursor: pointer;
                color: #2f855a;
                font-weight: 600;
                font-size: 0.9rem;
            }
        </style>

    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form id="frmnewloan" runat="server">

            <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

            <nav class="page-breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="#">Loan</a></li>
                    <li class="breadcrumb-item active">Account Opening</li>
                </ol>
            </nav>

            <div class="row">
                <div class="col-md-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <div id="smarttab">
                                <ul class="nav" id="tabitm">
                                    <li id="cTab">
                                        <a class="nav-link" href="#tab-1">
                                            Customer Specifics
                                        </a>
                                    </li>
                                    <li id="jTab">
                                        <a class="nav-link" href="#tab-2">
                                            Jewel Specifics
                                        </a>
                                    </li>
                                    <li id="dTab"><a class="nav-link" href="#tab-3">
                                            Deposit Specifics
                                        </a></li>
                                </ul>

                                <div class="tab-content">
                                    <div id="tab-1" class="tab-pane active" role="tabpanel">
                                        <div class="customer-card shadow-sm">
                                            <h5 class="section-title mb-4">Customer Details</h5>
                                            <asp:Label Visible="false" ID="tabin" Text="1" runat="server"></asp:Label>

                                            <div class="row">
                                                <div class="col-md-6 form-group">
                                                    <label class="form-label" for="txtcid">Date</label>
                                                    <asp:TextBox ID="txtadate" CssClass="form-control bg-light"
                                                        ReadOnly="true" runat="server"
                                                        data-validation-engine="validate[required,funcCall[DateFormat[]]"
                                                        data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                </div>

                                                <div class="col-md-6 form-group">
                                                    <label class="form-label">Customer Id</label>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtcid" runat="server"
                                                                CssClass="form-control" AutoCompleteType="None"
                                                                AutoPostBack="True"
                                                                data-validation-engine="validate[required]"
                                                                data-errormessage-value-missing="Valid Customer Id is required!">
                                                            </asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>

                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-md-6 form-group">
                                                            <label class="form-label">Customer Name</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtname"
                                                                    CssClass="form-control bg-light" ReadOnly="true"
                                                                    runat="server"
                                                                    data-validation-engine="validate[required]"
                                                                    data-errormessage-value-missing="Customer Name Missing">
                                                                </asp:TextBox>
                                                                <asp:TextBox ID="txtcof"
                                                                    CssClass="form-control bg-light ml-2"
                                                                    ReadOnly="true" runat="server"
                                                                    data-validation-engine="validate[required]">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6 form-group">
                                                            <label class="form-label">Address</label>
                                                            <div class="d-flex align-items-start">
                                                                <asp:TextBox ID="txtadd"
                                                                    CssClass="form-control bg-light flex-grow-1"
                                                                    ReadOnly="true" runat="server" Height="70px"
                                                                    TextMode="MultiLine">
                                                                </asp:TextBox>
                                                                <asp:Image id="imgCapture" runat="server"
                                                                    Visible="false"
                                                                    style="margin-left:15px; border-radius: 8px; border: 1px solid #e2e8f0; object-fit: cover;"
                                                                    Width="70px" Height="70px" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6 form-group">
                                                            <label class="form-label">Loan Type</label>
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="deptyp" runat="server"
                                                                        CssClass="form-control custom-select"
                                                                        data-validation-engine="validate[required]"
                                                                        data-errormessage-value-missing="Select a Product!"
                                                                        AutoPostBack="True">
                                                                        <asp:ListItem Text="Select"></asp:ListItem>
                                                                        <asp:ListItem Text="DCL" Value="DCL">
                                                                        </asp:ListItem>
                                                                        <asp:ListItem text="DL" Value="DL">
                                                                        </asp:ListItem>
                                                                        <asp:ListItem Text="JL" Value="JL">
                                                                        </asp:ListItem>
                                                                        <asp:ListItem Text="ML" Value="ML">
                                                                        </asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox ID="chkOtherBranchShare" runat="server" Text="ShareHolder in other branch" Visible="False" CssClass="green-round-checkbox" AutoPostBack="True" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>

                                                        <div class="col-md-6 form-group">
                                                            <asp:UpdatePanel runat="server" ID="pnlsch" Visible="False">
                                                                <ContentTemplate>
                                                                    <label class="form-label">Scheme</label>
                                                                    <asp:DropDownList ID="sch" runat="server"
                                                                        CssClass="form-control custom-select"
                                                                        data-validation-engine="validate[required]"
                                                                        data-errormessage-value-missing="Select a Scheme!"
                                                                        AutoPostBack="True">
                                                                        <asp:ListItem Text="<- SELECT - >">
                                                                        </asp:ListItem>
                                                                        <asp:ListItem Text="PRIME"></asp:ListItem>
                                                                        <asp:ListItem Text="PRIME PLUS"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>

                                                    <div class="row" runat="server" id="roiDiv" visible="false">
                                                        <div class="col-md-6 form-group">
                                                            <label class="form-label">Rate of Interest</label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtcint"
                                                                    CssClass="form-control bg-light" ReadOnly="true"
                                                                    runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtdint"
                                                                    CssClass="form-control bg-light ml-2"
                                                                    ReadOnly="true" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                            <div class="form-group row mt-4">
                                                <div class="col-sm-12 text-right">
                                                    <asp:UpdatePanel runat="server" RenderMode="Inline">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btn_clr_ms" runat="server" Text="Clear"
                                                                CssClass="btn btn-outline-secondary px-4 py-2 mr-2" />
                                                            <asp:Button ID="btn_nxt_ms" runat="server" Text="Next Step"
                                                                CssClass="btn btn-primary px-4 py-2"
                                                                CausesValidation="False" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="tab-2" class="tab-pane " role="tabpanel">


                                        <asp:UpdatePanel ID="jwlup" runat="server">
                                            <ContentTemplate>

                                                <h5 style="color:#6e7bd9; margin-bottom: 15px; font-weight: bold;">Jewel
                                                    Data Entry</h5>
                                                <table style="width:100%" class="table table-bordered  ">
                                                    <thead>

                                                        <th style="width:35%;">Particulars</th>
                                                        <th style="width:15%;">Quantity</th>
                                                        <th style="width:25%;">Gross Weight</th>
                                                        <th style="width:25%;">Net Weight</th>

                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td style="text-align:center;width:35%">

                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>

                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="jwlspecs"
                                                                                AutoPostBack="true" Width="150px"
                                                                                runat="server"
                                                                                onfocus="InitAutocomplJewl();"
                                                                                CssClass="form-control"></asp:TextBox>

                                                                            <div class="input-group-append ">
                                                                                <span class="input-group-addon ">
                                                                                    <a href="#" style="cursor:pointer;"
                                                                                        onclick="showTab();"
                                                                                        class="btn btn-outline-primary">
                                                                                        <svg xmlns="http://www.w3.org/2000/svg"
                                                                                            width="18" height="18"
                                                                                            viewBox="0 0 24 24"
                                                                                            fill="none"
                                                                                            stroke="currentColor"
                                                                                            stroke-width="2"
                                                                                            stroke-linecap="round"
                                                                                            stroke-linejoin="round"
                                                                                            class="feather feather-plus">
                                                                                            <line x1="12" y1="5" x2="12"
                                                                                                y2="19"></line>
                                                                                            <line x1="5" y1="12" x2="19"
                                                                                                y2="12"></line>
                                                                                        </svg>
                                                                                    </a>

                                                                                </span>
                                                                            </div>
                                                                        </div>

                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>

                                                            </td>
                                                            <td style="text-align:center;width:15% ">
                                                                <asp:TextBox ID="txtqty" runat="server"
                                                                    Style="text-align:right;font-size:medium"
                                                                    CssClass="form-control "
                                                                    data-validation-engine="validate[required,custom[integer]"
                                                                    data-errormessage-value-missing="Quantity Missing"
                                                                    data-errormessage-custom-error="Enter a Valid Quantity">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td style="text-align:center;width:25% ">
                                                                <asp:TextBox ID="txtgross" runat="server"
                                                                    Style="text-align:right;font-size:medium"
                                                                    CssClass="form-control"
                                                                    data-validation-engine="validate[required,funcCall[chkdecim[]]"
                                                                    data-errormessage-value-missing="Weight Missing"
                                                                    data-errormessage-custom-error="Enter a Valid Gross Weight">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td style="text-align:center;width:25%">
                                                                <asp:TextBox ID="txtnet" runat="server"
                                                                    AutoPostBack="true"
                                                                    Style="text-align:right;font-size:medium"
                                                                    CssClass="form-control "
                                                                    data-validation-engine="validate[required,funcCall[chkdecim[]]"
                                                                    data-errormessage-value-missing="Weight Missing"
                                                                    data-errormessage-custom-error="Enter a Valid Net Weight">
                                                                </asp:TextBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" style="text-align:right;">
                                                                <asp:Image id="imgJewelCapture" runat="server"
                                                                    Visible="false" Width="48px" Height="48px"
                                                                    style="vertical-align:middle; margin-right: 10px;" />
                                                                <a ID="btn_img_upload"
                                                                    class="btn btn-outline-secondary btn-sm"
                                                                    data-toggle="modal" data-target="#uploadImg"
                                                                    data-backdrop="static" data-keyboard="false">Select
                                                                    Image</a>
                                                                <a href="#code" ID="btn_img_Capture"
                                                                    class="btn btn-outline-secondary btn-sm"
                                                                    data-toggle="modal" data-target="#captureImg"
                                                                    onclick="return showwebcam();"
                                                                    data-backdrop="static" data-keyboard="false">Capture
                                                                    Image</a>
                                                                <asp:Button ID="btnAddJewel" runat="server"
                                                                    Text="Add to List"
                                                                    CssClass="btn btn-primary btn-sm ml-2"
                                                                    CausesValidation="true" />
                                                            </td>
                                                        </tr>
                                                        <tr class="sensitive ">
                                                            <td colspan="4">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>

                                                                        <div class="form-group row">
                                                                            <label class="col-sm-3 col-form-label">Name
                                                                                of the Jewel</label>
                                                                            <div class="col-sm-3">
                                                                                <asp:TextBox runat="server"
                                                                                    ID="txtjwlname"
                                                                                    CssClass="form-control ">
                                                                                </asp:TextBox>
                                                                            </div>
                                                                            <div class="col-sm-2">
                                                                                <asp:LinkButton
                                                                                    CssClass="btn btn-outline-primary "
                                                                                    runat="server" ID="jladd"
                                                                                    Text="Save"
                                                                                    OnClientClick="showTab();">
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                        </div>


                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>

                                                    </tbody>

                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="jspc" runat="server">
                                            <ContentTemplate>
                                                <h5
                                                    style="color:#6e7bd9; margin-top: 20px; margin-bottom: 15px; font-weight: bold;">
                                                    Staged Jewels</h5>
                                                <asp:GridView runat="server" ID="disp" AutoGenerateColumns="false"
                                                    AllowPaging="true" OnPageIndexChanging="OnPaging"
                                                    OnRowCommand="disp_RowCommand" Width="100%" Visible="true"
                                                    ShowHeaderWhenEmpty="false" ShowHeader="false"
                                                    CssClass="table  table-bordered ">
                                                    <Columns>



                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a href="javascript:void(0);"
                                                                    onclick="showFullImage(this.children[0].src); return false;"
                                                                    title="Click to enlarge">
                                                                    <img src='<%#Eval("base64image")%>'
                                                                        style="width: 48px; height: 48px; object-fit: cover; cursor: pointer; border-radius: 4px; border: 2px solid #6e7bd9; padding: 2px;"
                                                                        onerror="this.style.display='none'" />
                                                                </a>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitm" runat="server"
                                                                    text='<%#Eval("itm")%>' Style="text-align:left  ">
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="30%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblqty" runat="server"
                                                                    text='<%#Eval("qty")%>'
                                                                    Style="text-align:right;float:right"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblgross" runat="server"
                                                                    text='<%#Eval("gross")%>'
                                                                    Style="text-align:right;float:right"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="25%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblnet" runat="server"
                                                                    text='<%#Eval("net")%>'
                                                                    Style="text-align:right;float:right   "></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnRemoveJewel" runat="server"
                                                                    Text="Remove" CssClass="text-danger"
                                                                    CommandName="RemoveJewel"
                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                    CausesValidation="false"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>


                                            </ContentTemplate>

                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel runat="server">

                                            <ContentTemplate>
                                                <h5
                                                    style="color:#6e7bd9; margin-top: 20px; margin-bottom: 15px; font-weight: bold;">
                                                    Final Calculation</h5>

                                                <table style="width:100%" class="table  table-bordered ">
                                                    <tbody>
                                                        <tr style="text-align:center;">
                                                            <td style="text-align:center;width:35%">
                                                                <p>Total</p>

                                                            </td>
                                                            <td style="text-align:center;width:15% ">
                                                                <asp:Label runat="server" ID="lbltqty"
                                                                    style="float:right;text-align:right " Text="0">
                                                                </asp:Label>
                                                            </td>
                                                            <td style="text-align:right ;width:25% ">
                                                                <asp:Label runat="server" ID="lblgross"
                                                                    style="float:right;text-align:right " Text="0">
                                                                </asp:Label>
                                                            </td>
                                                            <td style="text-align:center;width:25%">
                                                                <asp:Label runat="server" ID="lblnet"
                                                                    style="float:right;text-align:right " Text="0">
                                                                </asp:Label>
                                                            </td>



                                                        </tr>
                                                    </tbody>
                                                </table>

                                                <div class="form-group row ">
                                                    <label class="col-sm-2 col-form-label text-primary"> Rate per Gram
                                                    </label>

                                                    <asp:Label ID="lblrate" class="col-sm-3 col-form-label "
                                                        runat="server" Text="0.00"></asp:Label>

                                                    <label class="col-sm-2 col-form-label text-primary"> Total Value
                                                    </label>

                                                    <asp:Label ID="lblval" CssClass="col-sm-3 col-form-label "
                                                        runat="server" Text="0.00"></asp:Label>

                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>


                                                <div class="form-group row ">

                                                    <asp:UpdatePanel runat="server">

                                                        <ContentTemplate>
                                                            <div class="col-sm-2">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="form-check form-check-inline">
                                                                            <label class="form-check-label">
                                                                                <asp:CheckBox runat="server" ID="issms"
                                                                                    AutoPostBack="true" />
                                                                                SMS Alert
                                                                                <i class="input-frame"></i>
                                                                            </label>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>

                                                                        <div class="form-check form-check-inline">
                                                                            <label class="form-check-label">
                                                                                <asp:CheckBox runat="server"
                                                                                    ID="isrebate" AutoPostBack="true" />
                                                                                Rebate Applicable
                                                                                <i class="input-frame"></i>
                                                                            </label>
                                                                        </div>

                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>


                                                </div>





                                                <asp:UpdatePanel runat="server" id="rebate" Visible="false">


                                                    <ContentTemplate>
                                                        <div class="form-group row ">
                                                            <label class="col-sm-2 col-form-label ">Branch</label>
                                                            <div class="col-sm-3">
                                                                <asp:DropDownList CssClass="form-check-inline  "
                                                                    runat="server" AutoPostBack="true" ID="ssbr">
                                                                    <asp:ListItem Value=""><-select-></asp:ListItem>
                                                                    <asp:ListItem Value="KARAVILAI">KARAVILAI
                                                                    </asp:ListItem>
                                                                    <asp:ListItem Value="VILLUKURI">VILLUKURI
                                                                    </asp:ListItem>
                                                                    <asp:ListItem Value="MEKKAMANDABAM">MEKKAMANDABAM
                                                                    </asp:ListItem>
                                                                    <asp:ListItem Value="ALENCODE">ALENCODE
                                                                    </asp:ListItem>
                                                                    <asp:ListItem Value="PADMANABHAPURAM">
                                                                        PADMANABHAPURAM</asp:ListItem>

                                                                </asp:DropDownList>

                                                            </div>

                                                        </div>
                                                        <div class="form-group row ">
                                                            <label class="col-sm-2 col-form-label ">Deposit</label>
                                                            <div class="col-sm-3">
                                                                <asp:DropDownList CssClass="form-check-inline  "
                                                                    runat="server" AutoPostBack="true" ID="ssdep">
                                                                    <asp:ListItem Value=""><-select-></asp:ListItem>
                                                                    <asp:ListItem Value="FD">FD</asp:ListItem>
                                                                    <asp:ListItem Value="RD">RD</asp:ListItem>
                                                                    <asp:ListItem Value="RID">RID</asp:ListItem>


                                                                </asp:DropDownList>

                                                            </div>
                                                        </div>
                                                        <div class="form-group row ">
                                                            <label class="col-sm-2 col-form-label ">Account No</label>
                                                            <div class="col-sm-3">
                                                                <asp:TextBox ID="txtrebateacn" runat="server"
                                                                    CssClass="form-control "></asp:TextBox>

                                                            </div>
                                                        </div>


                                                    </ContentTemplate>
                                                </asp:UpdatePanel>


                                            </ContentTemplate>
                                        </asp:UpdatePanel>


                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server"
                                            AssociatedUpdatePanelID="pnljwl">
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
                                                        Processing.....
                                                    </div>
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>


                                        <asp:UpdatePanel runat="server" ID="pnljwl">
                                            <ContentTemplate>


                                                <div class="form-group row border-bottom"></div>
                                                <div class="form-group row">
                                                    <div class="col-sm-4"></div>
                                                    <div class="col-sm-4">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <div class="btn-group " role="group">
                                                                    <asp:Button ID="btn_clr_jsp" runat="server"
                                                                        Text="Clear"
                                                                        CssClass="btn btn-outline-secondary  "
                                                                        OnClientClick="return detach()" />
                                                                    <asp:Button ID="btn_nxt_jsp" runat="server"
                                                                        Text="Update"
                                                                        CssClass="btn btn-outline-primary "
                                                                        Enabled="true" CausesValidation="false"
                                                                        OnClientClick="return detach()" />
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>


                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>

                                    <div id="tab-3" class="tab-pane" role="tabpanel">

                                        <h6 class="card-title text-primary ">Deposit Details</h6>

                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>



                                                <table style="width:80%" class="table table-bordered ">
                                                    <thead>

                                                        <th style="width:30%;">Account No</th>
                                                        <th style="width:18%;">Deposit Date</th>
                                                        <th style="width:24%;">Deposit Amount</th>
                                                        <th style="width:29%;">Account Balance</th>



                                                    </thead>
                                                </table>
                                                <table style="width:80%" class="table table-hover">
                                                    <tbody>
                                                        <tr>
                                                            <td style="text-align:center;width:30%">

                                                                <asp:UpdatePanel runat="server" ID="dl_specs">
                                                                    <ContentTemplate>

                                                                        <asp:TextBox ID="txtacn" runat="server"
                                                                            AutoPostBack="true"
                                                                            Style="text-align:left;font-size:medium;float:left "
                                                                            CssClass="form-control"
                                                                            data-validation-engine="validate[required]"
                                                                            data-errormessage-value-missing="Account No Missing"
                                                                            data-errormessage-custom-error="Enter a Valid Account No">
                                                                        </asp:TextBox>

                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td style="text-align:center;width:18% ">
                                                                <asp:UpdatePanel ID="dl_ddat" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtddt" runat="server"
                                                                            Enabled="false"
                                                                            Style="text-align:right;font-size:medium"
                                                                            CssClass="form-control "
                                                                            data-validation-engine="validate[required,funcCall[DateFormat[]]"
                                                                            data-errormessage-value-missing="Date Missing">
                                                                        </asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td style="text-align:center;width:24% ">
                                                                <asp:UpdatePanel ID="dl_depamt" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtdepamt" runat="server"
                                                                            Enabled="false"
                                                                            Style="text-align:right;font-size:medium"
                                                                            CssClass="form-control"
                                                                            data-validation-engine="validate[required,custom[integer]]"
                                                                            data-errormessage-value-missing="Deposit Amount Missing">
                                                                        </asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td style="text-align:center;width:29%">
                                                                <asp:UpdatePanel ID="dl_acbal" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="input-group ">

                                                                            <asp:TextBox ID="txtacbal" runat="server"
                                                                                ReadOnly="true" Width="120px"
                                                                                Style="text-align:right;font-size:medium"
                                                                                CssClass="form-control "
                                                                                data-validation-engine="validate[required,custom[integer]]"
                                                                                data-errormessage-value-missing="Account Balance Missing"
                                                                                data-errormessage-custom-error="Insufficient balance">
                                                                            </asp:TextBox>
                                                                            <div class="input-group-append ">
                                                                                <span class="input-group-addon ">
                                                                                    <asp:Button ID="btnadd"
                                                                                        runat="server"
                                                                                        CssClass="btn btn-outline-primary  "
                                                                                        Text="+" Font-Size="Large"
                                                                                        CausesValidation="False" />
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>


                                                        </tr>
                                                    </tbody>
                                                </table>

                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>

                                                        <asp:GridView runat="server" ID="dispdl"
                                                            AutoGenerateColumns="false" AllowPaging="true"
                                                            OnPageIndexChanging="OnPaging" Width="80%" Visible="true"
                                                            ShowHeaderWhenEmpty="false" ShowHeader="false"
                                                            CssClass="table  table-hover  ">
                                                            <Columns>


                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblitm" runat="server"
                                                                            text='<%#Eval("acn")%>'
                                                                            Style="text-align:left  "></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="30%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblqty" runat="server"
                                                                            text='<%#Eval("acdate")%>'
                                                                            Style="text-align:left ;float:right">
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="18%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgross" runat="server"
                                                                            text='<%#Eval("depamt")%>'
                                                                            Style="text-align:right;float:right">
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="24%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblnet" runat="server"
                                                                            text='<%#Eval("curbal")%>'
                                                                            Style="text-align:right;float:right   ">
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="29%" />
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>


                                                    </ContentTemplate>

                                                </asp:UpdatePanel>

                                                <table style="width:80%" class="table table-bordered ">
                                                    <tbody>
                                                        <tr style="text-align:center;">
                                                            <td style="text-align:center;width:30%">
                                                                <p>Total</p>

                                                            </td>
                                                            <td style="text-align:center;width:18% ">

                                                            </td>
                                                            <td style="text-align:right ;width:24% ">

                                                            </td>
                                                            <td style="text-align:center;width:29%">
                                                                <asp:Label runat="server" ID="lblamt"
                                                                    style="float:right;text-align:right " Text="0">
                                                                </asp:Label>
                                                            </td>


                                                        </tr>
                                                    </tbody>
                                                </table>

                                            </ContentTemplate>

                                        </asp:UpdatePanel>



                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>


                                                <div class="form-group row">
                                                    <label class="col-sm-1 col-form-label">C-Int</label>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" CssClass="form-control "
                                                            style="text-align:right" ID="lblc"></asp:Label>
                                                    </div>
                                                    <label class="col-sm-1 col-form-label">D-Int</label>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" CssClass="form-control"
                                                            style="text-align:right" ID="lbld"></asp:Label>
                                                    </div>

                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdateProgress ID="UpdateProgress2" runat="server"
                                            AssociatedUpdatePanelID="pnldl">
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
                                                        Processing.....
                                                    </div>
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>

                                        <asp:UpdatePanel runat="server" ID="pnldl">
                                            <ContentTemplate>


                                                <div class="form-group row border-bottom"></div>
                                                <div class="form-group row">
                                                    <div class="col-sm-4"></div>
                                                    <div class="col-sm-4">
                                                        <div class="btn-group" role="group">
                                                            <asp:Button ID="btn_clr_ds" runat="server" Text="Clear"
                                                                CssClass="btn btn-outline-secondary  "
                                                                OnClientClick="return detach()" />
                                                            <asp:Button ID="btn_nxt_ds" runat="server" Enabled="False"
                                                                Text="Update" CssClass="btn btn-outline-primary "
                                                                CausesValidation="False"
                                                                OnClientClick="return detach()" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>

                                </div>


                            </div>

                        </div>

                    </div>
                </div>
            </div>

            <div class="modal fade" id="uploadImg" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
                aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">Select an Image to Upload</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:Panel runat="server" ID="pnlimgup" CssClass="modalPopup">
                                <div>
                                    <asp:FileUpload id="img_file" style="margin-left:30px" width="380px"
                                        Cssclass="btn btn-outline-primary  btn" runat="server" /><br />
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button ID="btn_upimg" runat="server" CssClass="btn btn-outline-primary btn"
                                Text="Upload" Width="100px" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="captureImg" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
                aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel1">Capture Image</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div id="webcamImg"></div>
                                    </td>
                                    <td><img id="capturedImg" /></td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btn_img_can" href="#" class="btn btn-outline-primary btn"
                                Text="Upload" Width="100px" OnClientClick="upload_img();" CausesValidation="false" />
                            <a ID="btn_img_cap" href="#" class="btn btn-outline-primary btn" Text="Capture"
                                Width="100px" onclick="take_snapshot();">Capture</a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="imagePreviewModal" tabindex="-1" role="dialog" aria-hidden="true"
                style="z-index: 1060;">
                <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                    <div class="modal-content shadow-lg border-0" style="border-radius: 12px; overflow: hidden;">
                        <div class="modal-header bg-light border-bottom-0">
                            <h5 class="modal-title font-weight-bold" style="color:#6e7bd9;">Jewel Image Preview</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body text-center p-4">
                            <img id="fullImagePreview" src="" alt="Jewel Preview"
                                style="width: 100%; height: auto; max-height: 70vh; object-fit: contain; border: 1px solid #ddd; border-radius: 8px;" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <a id="btnHiddenModalTrigger" data-toggle="modal" data-target="#imagePreviewModal"
                style="display:none;"></a>

        </form>

        <style>
            /* .table-condensed tr th {
            border: 0px solid #6e7bd9;
            color: white;
            background-color: #6e7bd9;
            }
            */
            .table-condensed,
            .table-condensed tr td {
                border: 1px solid #6e7bd9;
            }

            tr:nth-child(even) {
                background: #f8f7ff
            }

            tr:nth-child(odd) {
                background: #fff;
            }
        </style>

        <script src="js/jquery.js" type="text/javascript"></script>

        <script src="js/daterangepicker.js" type="text/javascript"></script>
        <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
        <script src="js/jquery.validationEngine.js" type="text/javascript"></script>


        <script src="js/jquery.smartTab.min.js" type="text/javascript"></script>

        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/webcamjs/1.0.25/webcam.js"></script>
        <script type="text/javascript">
            function showFullImage(src) {
                if (src && src.length > 0 && src !== "data:image/jpeg;base64,") {
                    document.getElementById('fullImagePreview').src = src;
                    document.getElementById('btnHiddenModalTrigger').click();
                } else {
                    alert("No valid image data found for this jewel.");
                }
            }
        </script>
        <script type="text/javascript">
            function showwebcam() {
                Webcam.set({
                    width: 220,
                    height: 190,
                    image_format: 'jpeg',
                    jpeg_quality: 100
                });
                Webcam.attach('#webcamImg');
            };
            function take_snapshot() {
                Webcam.snap(function (data_uri) {
                    $("#capturedImg")[0].src = data_uri;
                });
                Webcam.reset();
            }
            function upload_img() {
                $.ajax({
                    type: "POST",
                    url: "LoanOpening.aspx/SaveCapturedImage",
                    data: "{data: '" + $("#capturedImg")[0].src + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) { }
                });
            }
        </script>

        <script type="text/javascript">




            $(document).ready(function () {



                $("#frmnewloan").validationEngine('attach');
                $("#frmnewloan").validationEngine({ promptPosition: "topleft", scroll: false, focusFirstField: true, showArrow: true });
                $("#frmnewloan").validationEngine('attach', { promptPosition: "topleft", scroll: false, showArrow: true });


                $('#smarttab').smartTab({
                    selected: 0, // Initial selected tab, 0 = first tab
                    // theme: 'dark', // theme for the tab, related css need to include for other than default theme
                    orientation: 'horizontal', // Nav menu orientation. horizontal/vertical
                    justified: false, // Nav menu justification. true/false
                    autoAdjustHeight: false, // Automatically adjust content height
                    backButtonSupport: true, // Enable the back button support
                    enableURLhash: true, // Enable selection of the tab based on url hash
                    transition: {
                        animation: 'slide-swing', // Effect on navigation, none/fade/slide-horizontal/slide-vertical/slide-swing
                        speed: '400', // Transion animation speed
                        easing: '' // Transition animation easing. Not supported without a jQuery easing plugin
                    },
                    keyboardSettings: {
                        keyNavigation: false, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
                        keyLeft: [37], // Left key code
                        keyRight: [39] // Right key code
                    }
                });


                $("#smarttab").on("showTab", function (e, anchorObject, tabIndex) {


                    switch (tabIndex) {
                        case 0:
                            setTimeout(function () {
                                document.getElementById('<%=txtcid.ClientID %>').focus();
                            }, 200);

                            break;
                        case 1:
                            setTimeout(function () {
                                document.getElementById('<%=jwlspecs.ClientID %>').focus();
                            }, 100);

                            break;
                        case 2:
                            setTimeout(function () {
                                document.getElementById('<%=txtacn.ClientID %>').focus();
                            }, 100);

                            break;
                    }
                });

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);


                // Place here the first init of the autocomplete
                InitAutocomplJewl();
                InitAutoCompl();
                InitAutoComplacn();
                ShowCTab();
            });


            function InitializeRequest(sender, args) {
            }



            function ShowJlTab() {

                document.getElementById("tabitm").children[1].style.display = "block";
                document.getElementById("tabitm").children[2].style.display = "none";

            }



            function ShowCTab() {

                if ('<% =Session("Tabin")%>' == 0) {
                    document.getElementById("tabitm").children[1].style.display = "none";
                    document.getElementById("tabitm").children[2].style.display = "none";
                }

                if ('<% =Session("Tabin")%>' == 1) {
                    document.getElementById("tabitm").children[1].style.display = "block";
                    document.getElementById("tabitm").children[2].style.display = "none";
                }
                if ('<% =Session("Tabin")%>' == 2) {
                    document.getElementById("tabitm").children[1].style.display = "none";
                    document.getElementById("tabitm").children[2].style.display = "block";
                }

            }



            function ShowCxTab() {



            }

            function ShowDlTab() {
                document.getElementById("tabitm").children[1].style.display = "none";
                document.getElementById("tabitm").children[2].style.display = "block";

            }


            function ShowDLS(tabin) {


                $('#smarttab').smartTab("goToTab", tabin);
                return true;

            }


            function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete


                InitAutoCompl();
                InitAutocomplJewl();
                InitAutoComplacn();
            }


            function InitAutocomplJewl() {

                $("#<%=jwlspecs.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/membersearch.asmx/GetJewel") %>',
                            data: "{ 'input': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        jewel: item.Jewel,

                                    }
                                }))
                            }
                        });
                    },
                    minLength: 1,

                    select: function (event, ui) {

                        $("#<%=jwlspecs.ClientID %>").val(ui.item.jewel);
                        return false;
                    }
                })
                    .autocomplete("instance")._renderItem = function (ul, item) {
                        return $("<li>")
                            .append("<div class='info-div column'>")
                            .append("<div class='username'>" + item.jewel + "</div> ")
                            .append("</div")
                            .append("</div>")
                            .appendTo(ul);
                    };

            }


            function InitAutoCompl() {

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


            function InitAutoComplacn() {

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
                                        product: item.Product
                                    }
                                }))
                            }
                        });
                    },
                    minLength: 1,

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
                            .append("<div class='username'>" + item.firstname + "<div class='memno text-muted'>" + item.memberno + "</div></div>")
                            .append("<div class='userinfo'>" + item.lastname + "<div class='product text-muted'>" + item.product + "</div></div>")
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
                $("#frmnewloan").validationEngine('detach');

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

            ui-autocomplete {
                max-height: 510px;
                overflow-y: auto;
                /* prevent horizontal scrollbar */
                overflow-x: hidden;
            }

            .sensitive {
                display: none;
            }
        </style>

        <script type="text/javascript">

            function showTab() {
                $(".sensitive").toggle();
            }


        </script>


    </asp:Content>