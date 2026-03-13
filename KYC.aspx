<%@ Page Title="KYC" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="KYC.aspx.vb" Inherits="Fiscus.KYC" EnableEventValidation="true" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <title>KYC</title>
        <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">

        <style>
            .custom-checkbox input[type="checkbox"] {
                appearance: none;
                -webkit-appearance: none;
                width: 18px;
                height: 18px;
                border: 2px solid #D1D5DB;
                border-radius: 50% !important;
                background: #fff;
                cursor: pointer;
                vertical-align: middle;
                position: relative;
                transition: all 0.2s ease;
                outline: none;
                margin: 0;
            }

            .custom-checkbox input[type="checkbox"]:checked {
                background-color: #10B981 !important;
                border-color: #10B981 !important;
            }

            .custom-checkbox input[type="checkbox"]:checked::after {
                content: '';
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                width: 8px;
                height: 8px;
                background: #fff;
                border-radius: 50%;
            }

            .custom-checkbox input[type="checkbox"]:focus {
                box-shadow: 0 0 0 3px rgba(16, 185, 129, 0.2);
                border-color: #10B981;
            }

            .custom-checkbox-label {
                cursor: pointer;
                user-select: none;
            }
        </style>

        <script type="text/javascript">
            function selectCustomOption(selectId, value, displayId, optionsId) {
                var realSelect = document.getElementById(selectId);
                if (realSelect) {
                    realSelect.value = value;
                }
                var b = document.getElementById(displayId);
                if (b) {
                    b.innerText = value;
                    b.style.color = '#374151';
                    b.style.fontWeight = '400';
                    b.style.borderColor = '#E5E7EB';
                    b.style.boxShadow = 'none';
                }
                var opts = document.getElementById(optionsId);
                if (opts) {
                    opts.style.display = 'none';
                }
            }
        </script>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




        <form id="kyc" runat="server" style="background-color: #F3F4FF; font-family: 'Inter', sans-serif; min-height: 100vh; padding: 20px;">
            <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
            <nav class="page-breadcrumb" style="margin-bottom: 24px;">
                <ol class="breadcrumb" style="background: transparent; padding: 0;">
                    <li class="breadcrumb-item"><a href="#" style="color: #6B7280; text-decoration: none;">Master</a></li>
                    <li class="breadcrumb-item active" style="color: #374151; font-weight: 500;">KYC</li>
                </ol>
            </nav>
            <asp:HiddenField ID="hfCapturedImage" runat="server" />
            <asp:HiddenField ID="hfCapturedSignature" runat="server" />
            <asp:HiddenField ID="hfCapturedPOI" runat="server" />
            <asp:HiddenField ID="hfCapturedPOA" runat="server" />


            <div class="row">
                <div class="col-md-12 grid-margin stretch-card">
                    <div class="card" style="background: white; border-radius: 16px; box-shadow: 0 10px 30px rgba(0,0,0,0.05); border: none;">
                        <div class="card-body" style="padding: 24px;">
                            <div id="smarttab">
                                <ul class="nav" style="border-bottom: 1px solid #E5E7EB; margin-bottom: 24px; gap: 8px;">
                                    <li class="nav-item">
                                        <a class="nav-link" href="#tab-1" style="font-weight: 500; font-size: 15px; color: #6B7280; padding: 12px 20px; border-radius: 8px; transition: all 0.2s;">
                                            Customer
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="#tab-2" style="font-weight: 500; font-size: 15px; color: #6B7280; padding: 12px 20px; border-radius: 8px; transition: all 0.2s;">
                                            Identity
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="#tab-3" style="font-weight: 500; font-size: 15px; color: #6B7280; padding: 12px 20px; border-radius: 8px; transition: all 0.2s;">
                                            Proof
                                        </a>
                                    </li>

                                </ul>

                                <div class="tab-content">
                                    <div id="tab-1" class="tab-pane" role="tabpanel">
                                        <div class="form-group row" style="margin-bottom: 6px; display: flex; align-items: center;">
                                            <label class="col-sm-2 col-form-label" for="txtcid" style="font-size: 15px; font-weight: 600; color: #4B5563;">Customer Id</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="upcid" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtcid" runat="server" CssClass="form-control"
                                                            MaxLength="10" TextMode="Search" AutoCompleteType="None"
                                                            AutoPostBack="true" style="border-radius: 6px; border: 1px solid #E5E7EB; font-weight: 700; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-sm-2 col-form-label" for="txtgrpid" style="font-size: 15px; font-weight: 600; color: #4B5563;">Group Id</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtgrpid" runat="server" MaxLength="10"
                                                            CssClass="form-control" AutoPostBack="true"
                                                            AutoCompleteType="None" style="border-radius: 6px; border: 1px solid #E5E7EB; font-weight: 700; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                        <div class="form-group row" style="margin-bottom: 12px; display: flex; align-items: center;">
                                            <label class="col-sm-2 col-form-label" for="txtcname" style="font-size: 15px; font-weight: 600; color: #4B5563;">Customer Name</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox runat="server" ID="txtcname"
                                                            CssClass="form-control" style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                            <label class="col-sm-2 col-form-label" for="txtadd" style="font-size: 14px; font-weight: 600; color: #4B5563;">Address</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox TextMode="MultiLine" Height="60" runat="server"
                                                            ID="txtadd" CssClass="form-control" style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s; resize: vertical;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>


                                        <div class="form-group row" style="margin-bottom: 6px; display: flex; align-items: center;">
                                            <label class="col-sm-2 col-form-label" for="txtnominee" style="font-size: 14px; font-weight: 600; color: #4B5563;">Name of the Nominee</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox runat="server" ID="txtnominee"
                                                            CssClass="form-control" MaxLength="50" style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-sm-2 col-form-label" for="txtrelation" style="font-size: 14px; font-weight: 600; color: #4B5563;">Relationship</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox runat="server" ID="txtrelation"
                                                            CssClass="form-control" MaxLength="50" style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                        </div>

                                        <div class="form-group row" style="margin-bottom: 6px; display: flex; align-items: center;">
                                            <label class="col-sm-2 col-form-label" for="txtpan" style="font-size: 14px; font-weight: 600; color: #4B5563;">PAN Number</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox runat="server" ID="txtpan" CssClass="form-control"
                                                            MaxLength="11" style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-sm-2 col-form-label" for="txtappno" style="font-size: 14px; font-weight: 600; color: #4B5563;">Application No</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox runat="server" ID="txtappno"
                                                            CssClass="form-control" MaxLength="50" style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>


                                                <div class="form-group row" style="margin-bottom: 6px; display: flex; align-items: center;">
                                                    <div class="col-sm-2">
                                                        <div class="form-check form-check-inline" style="margin: 0; display: flex; align-items: center;">
                                                            <asp:CheckBox runat="server" ID="isactiv"
                                                                    AutoPostBack="true" CssClass="custom-checkbox" />
                                                            <label class="form-check-label custom-checkbox-label" for='<%=isactiv.ClientID%>' style="font-size: 14px; font-weight: 500; color: #4B5563; margin-left: 6px;">
                                                                Active
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-check form-check-inline" style="margin: 0; display: flex; align-items: center;">
                                                            <asp:CheckBox runat="server" ID="cp"
                                                                    AutoPostBack="true" CssClass="custom-checkbox" />
                                                            <label class="form-check-label custom-checkbox-label" for='<%=cp.ClientID%>' style="font-size: 14px; font-weight: 500; color: #4B5563; margin-left: 6px;">
                                                                Channel Partner
                                                            </label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>

                                                <asp:Panel runat="server" id="otp" Visible="false">

                                                    <div class="form-group row" style="margin-bottom: 6px; display: flex; align-items: center;">


                                                        <label class="col-sm-2 col-form-label" for="txtotp" style="font-size: 14px; font-weight: 600; color: #4B5563;">OTP</label>

                                                        <div class="col-md-3">
                                                            <asp:TextBox runat="server" ID="txtotp"
                                                                CssClass="form-control" style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';"></asp:TextBox>

                                                        </div>
                                                        <asp:Label CssClass="col-sm-2 col-form-label-sm" runat="server"
                                                            ID="lblotp" Text="Invalid OTP" Visible="false"
                                                            style="color: #DC2626; font-size: 13px; font-weight: 500;"></asp:Label>


                                                    </div>
                                                </asp:Panel>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                    <div id="tab-2" class="tab-pane" role="tabpanel">

                                        <div class="row">
                                            <div class="col-md-6 grid-margin stretch-card">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div style="margin-bottom: 12px;">
                                                            <h6 class="card-title" style="color: #5B21B6; font-weight: 600; font-size: 16px; margin-bottom: 6px;">Photo</h6>
                                                            <div style="background: linear-gradient(90deg, #6D28D9, #2563EB); height: 3px; width: 30px; border-radius: 4px;"></div>
                                                        </div>
                                                        <div class="form-group row ">
                                                            <div class="col-sm-3"></div>
                                                            <asp:UpdatePanel ID="upImgCapture" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Image id="imgCapture" runat="server"
                                                                        Width="192px" Height="192px" style="border-radius: 12px; border: 1px solid #E5E7EB; object-fit: cover; background: #F9FAFB;" />
                                                                    <div class="form-group row ">
                                                                        <div class="col-sm-3"></div>
                                                                        <div class="col-sm-8" style="margin-top: 16px;">
                                                                            <div class="btn-group" role="group"
                                                                                aria-label="Basic example">
                                                                                <a ID="btn_img_upload"
                                                                                    class="btn btn-outline-primary"
                                                                                    style="padding: 8px 24px; border-radius: 8px 0 0 8px; font-weight: 500; font-size: 14px; border-color: #2563EB; color: #2563EB;" data-toggle="modal"
                                                                                    data-target="#uploadImg"
                                                                                    data-backdrop="static"
                                                                                    data-keyboard="false">Select</a>
                                                                                <a href="#code" ID="btn_img_Capture"
                                                                                    class="btn btn-outline-primary"
                                                                                    style="padding: 8px 24px; border-radius: 0 8px 8px 0; font-weight: 500; font-size: 14px; border-color: #2563EB; color: #2563EB;" data-toggle="modal"
                                                                                    data-target="#captureImg"
                                                                                    onclick="return showwebcam();"
                                                                                    data-backdrop="static"
                                                                                    data-keyboard="false">Capture</a>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>

                                                        </div>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="col-md-6 grid-margin stretch-card">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div style="margin-bottom: 12px;">
                                                            <h6 class="card-title" style="color: #5B21B6; font-weight: 600; font-size: 16px; margin-bottom: 6px;">Signature</h6>
                                                            <div style="background: linear-gradient(90deg, #6D28D9, #2563EB); height: 3px; width: 30px; border-radius: 4px;"></div>
                                                        </div>
                                                        <div class="form-group row ">
                                                            <div class="col-sm-3"></div>
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Image id="imgsign" runat="server" Width="232px"
                                                                        Height="192px" style="border-radius: 12px; border: 1px solid #E5E7EB; object-fit: contain; background: #F9FAFB;" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>


                                                        </div>

                                                        <div class="form-group row ">
                                                            <div class="col-sm-4"></div>
                                                            <div class="btn-group" role="group"
                                                                aria-label="Basic example" style="margin-top: 16px;">

                                                                 <button type="button" class="btn btn-outline-primary"
                                                                     data-toggle="modal" data-target="#uploadSig" style="padding: 6px 16px; border-radius: 6px 0 0 6px; font-weight: 500; font-size: 13px; border-color: #2563EB; color: #2563EB;">
                                                                     Select
                                                                 </button>
                                                                 <button type="button" class="btn btn-outline-primary"
                                                                     data-toggle="modal" data-target="#captureSig"
                                                                     onclick="showwebcamSig();" style="padding: 6px 16px; border-radius: 0 6px 6px 0; font-weight: 500; font-size: 13px; border-color: #2563EB; color: #2563EB;">
                                                                     Capture
                                                                 </button>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tab-3" class="tab-pane" role="tabpanel">
                                        <div class="row">
                                            <div class="col-md-6 grid-margin stretch-card">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div style="margin-bottom: 12px;">
                                                            <h6 class="card-title" style="color: #5B21B6; font-weight: 600; font-size: 16px; margin-bottom: 6px;">Proof of Identity</h6>
                                                            <div style="background: linear-gradient(90deg, #6D28D9, #2563EB); height: 3px; width: 30px; border-radius: 4px;"></div>
                                                        </div>
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                 <div class="form-group row" style="margin-bottom: 6px; display: flex; align-items: center;">
                                                                    <label class="col-sm-3 col-form-label"
                                                                        for="idprof" style="font-size: 14px; font-weight: 600; color: #4B5563;">Document Type</label>
                                                                     <div class="col-sm-9">
                                                                        <div style="position: relative; outline: none;" id="customIdProfDropdown" tabindex="0" onblur="setTimeout(function() { document.getElementById('idProfOptions').style.display = 'none'; document.getElementById('idProfSelected').style.borderColor='#E5E7EB'; document.getElementById('idProfSelected').style.boxShadow='none'; }, 200)">
                                                                            <div id="idProfSelected" 
                                                                                style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s; color: #6B7280; font-weight: 500; background: white url('data:image/svg+xml;utf8,<svg xmlns=\'http://www.w3.org/2000/svg\' width=\'14\' height=\'14\' viewBox=\'0 0 24 24\' fill=\'none\' stroke=\'%236B7280\' stroke-width=\'2\' stroke-linecap=\'round\' stroke-linejoin=\'round\'><polyline points=\'6 9 12 15 18 9\'></polyline></svg>') no-repeat right 10px center; cursor: pointer; user-select: none;" 
                                                                                onclick="var opts = document.getElementById('idProfOptions'); if(opts.style.display==='none'){opts.style.display='block'; this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';}else{opts.style.display='none'; this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';}">
                                                                                <-- Select -->
                                                                            </div>
                                                                            <div id="idProfOptions" 
                                                                                style="display: none; position: absolute; top: 100%; left: 0; right: 0; background: white; border: 1px solid #E5E7EB; border-radius: 8px; margin-top: 4px; box-shadow: 0 10px 25px rgba(0,0,0,0.1); z-index: 100; overflow-y: auto; max-height: 200px; padding: 4px;">
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'Aadhaar Card', 'idProfSelected', 'idProfOptions')">Aadhaar Card</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'Passport', 'idProfSelected', 'idProfOptions')">Passport</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'Voter ID', 'idProfSelected', 'idProfOptions')">Voter ID</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'PAN Card', 'idProfSelected', 'idProfOptions')">PAN Card</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'Ration Card', 'idProfSelected', 'idProfOptions')">Ration Card</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'Govt / Defence ID Card', 'idProfSelected', 'idProfOptions')">Govt / Defence ID Card</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'ID Card of Reputed Employer', 'idProfSelected', 'idProfOptions')">ID Card of Reputed Employer</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'Driving License', 'idProfSelected', 'idProfOptions')">Driving License</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'Photo ID Card Issued by Post Office', 'idProfSelected', 'idProfOptions')">Photo ID Card Issued by Post Office</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=idprof.ClientID%>', 'Photo ID Card Issued by Public Authority', 'idProfSelected', 'idProfOptions')">Photo ID Card Issued by Public Authority</div>
                                                                            </div>
                                                                        </div>
                                                                        <asp:DropDownList ID="idprof" runat="server" style="display: none;">
                                                                            <asp:ListItem Value=""><-- Select -->
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Aadhaar Card</asp:ListItem>
                                                                            <asp:ListItem>Passport</asp:ListItem>
                                                                            <asp:ListItem>Voter ID</asp:ListItem>
                                                                            <asp:ListItem>PAN Card</asp:ListItem>
                                                                            <asp:ListItem>Ration Card</asp:ListItem>
                                                                            <asp:ListItem>Govt / Defence ID Card
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>ID Card of Reputed Employer
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Driving License</asp:ListItem>
                                                                            <asp:ListItem>Photo ID Card Issued by Post
                                                                                Office</asp:ListItem>
                                                                            <asp:ListItem>Photo ID Card Issued by Public
                                                                                Authority</asp:ListItem>

                                                                        </asp:DropDownList>

                                                                        <script type="text/javascript">
                                                                            setTimeout(function() {
                                                                                var realSelect = document.getElementById('<%=idprof.ClientID%>');
                                                                                var displayBox = document.getElementById('idProfSelected');
                                                                                if (realSelect && displayBox) {
                                                                                    var val = realSelect.value;
                                                                                    if (val && val !== '<-- Select -->') {
                                                                                        displayBox.innerText = val;
                                                                                        displayBox.style.color = '#374151';
                                                                                        displayBox.style.fontWeight = '400';
                                                                                    }
                                                                                }
                                                                            }, 50);
                                                                        </script>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row" style="margin-bottom: 12px;">
                                                                    <label class="col-sm-3 col-form-label"
                                                                        for="txtiddocno" style="font-size: 15px; font-weight: 600; color: #4B5563;">Document No.</label>
                                                                    <div class="col-sm-9">
                                                                        <asp:TextBox ID="txtiddocno"
                                                                            CssClass="form-control" runat="server" style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 6px 12px; font-size: 15px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                                                        </asp:TextBox>

                                                                    </div>
                                                                </div>
                                                                 <div class="form-group row" style="display: flex; align-items: center;">
                                                                     <label class="col-sm-3 col-form-label" style="font-size: 14px; font-weight: 600; color: #4B5563;">Document Capture</label>
                                                                     <div class="col-sm-9">
                                                                         <asp:Image id="imgPOI" runat="server" Width="100%" Height="150px" style="border-radius: 8px; border: 1px dashed #D1D5DB; object-fit: contain; background: #F9FAFB;" />
                                                                         <div class="btn-group mt-2" role="group">
                                                                             <button type="button" class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#uploadPOI" style="font-weight: 500; font-size: 12px; border-color: #2563EB; color: #2563EB; border-radius: 6px 0 0 6px; padding: 4px 12px;">Select</button>
                                                                             <button type="button" class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#capturePOI" onclick="showwebcamPOI();" style="font-weight: 500; font-size: 12px; border-color: #2563EB; color: #2563EB; border-radius: 0 6px 6px 0; padding: 4px 12px;">Capture</button>
                                                                         </div>
                                                                     </div>
                                                                 </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 grid-margin stretch-card">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div style="margin-bottom: 24px;">
                                                            <h6 class="card-title" style="color: #5B21B6; font-weight: 600; font-size: 18px; margin-bottom: 8px;">Proof of Address</h6>
                                                            <div style="background: linear-gradient(90deg, #6D28D9, #2563EB); height: 3px; width: 40px; border-radius: 4px;"></div>
                                                        </div>
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>

                                                                 <div class="form-group row" style="margin-bottom: 6px; display: flex; align-items: center;">
                                                                    <label class="col-sm-3 col-form-label"
                                                                        for="addprof" style="font-size: 14px; font-weight: 600; color: #4B5563;">Document Type</label>
                                                                     <div class="col-sm-9">
                                                                        <div style="position: relative; outline: none;" id="customAddProfDropdown" tabindex="0" onblur="setTimeout(function() { document.getElementById('addProfOptions').style.display = 'none'; document.getElementById('addProfSelected').style.borderColor='#E5E7EB'; document.getElementById('addProfSelected').style.boxShadow='none'; }, 200)">
                                                                            <div id="addProfSelected" 
                                                                                style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s; color: #6B7280; font-weight: 500; background: white url('data:image/svg+xml;utf8,<svg xmlns=\'http://www.w3.org/2000/svg\' width=\'14\' height=\'14\' viewBox=\'0 0 24 24\' fill=\'none\' stroke=\'%236B7280\' stroke-width=\'2\' stroke-linecap=\'round\' stroke-linejoin=\'round\'><polyline points=\'6 9 12 15 18 9\'></polyline></svg>') no-repeat right 10px center; cursor: pointer; user-select: none;" 
                                                                                onclick="var opts = document.getElementById('addProfOptions'); if(opts.style.display==='none'){opts.style.display='block'; this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';}else{opts.style.display='none'; this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';}">
                                                                                <-- Select -->
                                                                            </div>
                                                                            <div id="addProfOptions" 
                                                                                style="display: none; position: absolute; top: 100%; left: 0; right: 0; background: white; border: 1px solid #E5E7EB; border-radius: 8px; margin-top: 4px; box-shadow: 0 10px 25px rgba(0,0,0,0.1); z-index: 100; overflow-y: auto; max-height: 200px; padding: 4px;">
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Aadhaar Card', 'addProfSelected', 'addProfOptions')">Aadhaar Card</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'PAN Card', 'addProfSelected', 'addProfOptions')">PAN Card</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Income Tax Assesment Order', 'addProfSelected', 'addProfOptions')">Income Tax Assesment Order</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Electricity Bill', 'addProfSelected', 'addProfOptions')">Electricity Bill</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Telephone Bill', 'addProfSelected', 'addProfOptions')">Telephone Bill</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Bank Account Statement', 'addProfSelected', 'addProfOptions')">Bank Account Statement</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Letter from Reputed Employer', 'addProfSelected', 'addProfOptions')">Letter from Reputed Employer</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Letter from Public Authority', 'addProfSelected', 'addProfOptions')">Letter from Public Authority</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Ration Card', 'addProfSelected', 'addProfOptions')">Ration Card</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Lease / sale Deed', 'addProfSelected', 'addProfOptions')">Lease / sale Deed</div>
                                                                                <div style="padding: 8px 12px; font-size: 15px; color: #374151; cursor: pointer; border-radius: 4px; transition: background 0.2s;" onmouseover="this.style.background='#F3F4F6'" onmouseout="this.style.background='transparent'" onclick="selectCustomOption('<%=addprof.ClientID%>', 'Address Proof of Close Relatives', 'addProfSelected', 'addProfOptions')">Address Proof of Close Relatives</div>
                                                                            </div>
                                                                        </div>
                                                                        <asp:DropDownList ID="addprof" runat="server" style="display: none;">
                                                                            <asp:ListItem Value=""><-- Select -->
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Aadhaar Card</asp:ListItem>
                                                                            <asp:ListItem>PAN Card</asp:ListItem>
                                                                            <asp:ListItem>Income Tax Assesment Order
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Electricity Bill
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Telephone Bill</asp:ListItem>
                                                                            <asp:ListItem>Bank Account Statement
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Letter from Reputed Employer
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Letter from Public Authority
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Ration Card</asp:ListItem>
                                                                            <asp:ListItem>Lease / sale Deed
                                                                            </asp:ListItem>
                                                                            <asp:ListItem>Address Proof of Close
                                                                                Relatives</asp:ListItem>

                                                                        </asp:DropDownList>

                                                                        <script type="text/javascript">
                                                                            setTimeout(function() {
                                                                                var realSelect = document.getElementById('<%=addprof.ClientID%>');
                                                                                var displayBox = document.getElementById('addProfSelected');
                                                                                if (realSelect && displayBox) {
                                                                                    var val = realSelect.value;
                                                                                    if (val && val !== '<-- Select -->') {
                                                                                        displayBox.innerText = val;
                                                                                        displayBox.style.color = '#374151';
                                                                                        displayBox.style.fontWeight = '400';
                                                                                    }
                                                                                }
                                                                            }, 50);
                                                                        </script>
                                                                    </div>
                                                                </div>
                                                                 <div class="form-group row" style="margin-bottom: 6px; display: flex; align-items: center;">
                                                                    <label class="col-sm-3 col-form-label"
                                                                        for="txtadd_doc" style="font-size: 14px; font-weight: 600; color: #4B5563;">Document No.</label>
                                                                    <div class="col-sm-9">
                                                                        <asp:TextBox ID="txtadd_doc"
                                                                            CssClass="form-control" runat="server" style="border-radius: 6px; border: 1px solid #E5E7EB; padding: 4px 10px; font-size: 14px; transition: all 0.2s;" onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';" onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <label class="col-sm-3 col-form-label">Document Capture</label>
                                                                    <div class="col-sm-9">
                                                                        <asp:Image id="imgPOA" runat="server" Width="100%" Height="150px" BorderStyle="Dashed" BorderWidth="1px" />
                                                                        <div class="btn-group mt-2" role="group">
                                                                            <button type="button" class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#uploadPOA">Select</button>
                                                                            <button type="button" class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#capturePOA" onclick="showwebcamPOA();">Capture</button>
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

                        </div>
                    </div>
                </div>


            <div class="form-group row" style="margin-top: 24px; padding-top: 24px; border-top: 1px solid #E5E7EB;">
                <div class="col-sm-12" style="display: flex; justify-content: flex-end; padding-right: 15px;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="btn-group" role="group" aria-label="Basic example" style="gap: 12px;">
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false"
                                    style="border-radius: 10px; padding: 10px 28px; font-weight: 500; font-family: 'Inter', sans-serif; background: white; border: 1px solid #D1D5DB; color: #4B5563; transition: all 0.2s; font-size: 15px; cursor: pointer;"
                                    onmouseover="this.style.background='#F3F4F6'; this.style.transform='translateY(-1px)';"
                                    onmouseout="this.style.background='white'; this.style.transform='translateY(0)';"
                                    CssClass="btn btn-secondary" />
                                <asp:Button ID="btnupdate" Text="Update" runat="server"
                                    style="background: linear-gradient(135deg, #7C3AED, #2563EB); color: white; border-radius: 10px; padding: 10px 28px; border: none; font-weight: 500; font-family: 'Inter', sans-serif; transition: all 0.2s; box-shadow: 0 4px 14px rgba(37,99,235,0.2); font-size: 15px; cursor: pointer;"
                                    onmouseover="this.style.transform='translateY(-2px)'; this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
                                    onmouseout="this.style.transform='translateY(0)'; this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';"
                                    CssClass="btn btn-primary" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                                Text="Upload" Width="100px" />
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
                            <asp:Button runat="server" ID="btn_img_can" href="#" class="btn btn-outline-primary  btn"
                                Text="Upload" Width="100px" />
                            <a ID="btn_img_cap" href="#" class="btn btn-outline-primary btn" Text="Capture"
                                Width="100px" onclick="take_snapshot();">Capture</a>


                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="uploadSig" tabindex="-1" role="dialog" aria-labelledby="sigModalLabel"
                aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="sigModalLabel">Select a Signature to Upload</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:Panel runat="server" ID="pnlSigUp" CssClass="modalPopup">
                                <div>
                                    <asp:FileUpload id="sig_file" style="margin-left:30px" width="380px"
                                        Cssclass="btn btn-outline-primary  btn" runat="server" /><br />
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button ID="btn_up_sig" runat="server" CssClass="btn btn-outline-primary btn"
                                Text="Upload" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="captureSig" tabindex="-1" role="dialog" aria-labelledby="sigCaptureLabel"
                aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="sigCaptureLabel">Capture Signature</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div id="webcamSig"></div>
                                    </td>
                                    <td><img id="capturedSig" /></td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btn_sig_can" href="#" class="btn btn-outline-primary  btn"
                                Text="Upload" Width="100px" />
                            <a ID="btn_sig_cap_btn" href="#" class="btn btn-outline-primary btn" Text="Capture"
                                Width="100px" onclick="take_signature_snapshot();">Capture</a>
                        </div>
                    </div>
                </div>
            </div>

             <!-- POI Modals -->
            <div class="modal fade" id="uploadPOI" tabindex="-1" role="dialog" aria-labelledby="poiModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="poiModalLabel">Select POI to Upload</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:FileUpload id="poi_file" style="margin-left:30px" width="380px" Cssclass="btn btn-outline-primary  btn" runat="server" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button ID="btn_up_poi" runat="server" CssClass="btn btn-outline-primary btn" Text="Upload" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="capturePOI" tabindex="-1" role="dialog" aria-labelledby="poiCaptureLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="poiCaptureLabel">Capture POI</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div id="webcamPOI"></div>
                            <img id="capturedPOI" style="width:200px; height:150px;" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btn_poi_can" class="btn btn-outline-primary  btn" Text="Upload" Width="100px" />
                            <a class="btn btn-outline-primary btn" onclick="take_poi_snapshot();">Capture</a>
                        </div>
                    </div>
                </div>
            </div>

            <!-- POA Modals -->
            <div class="modal fade" id="uploadPOA" tabindex="-1" role="dialog" aria-labelledby="poaModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="poaModalLabel">Select POA to Upload</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:FileUpload id="poa_file" style="margin-left:30px" width="380px" Cssclass="btn btn-outline-primary  btn" runat="server" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button ID="btn_up_poa" runat="server" CssClass="btn btn-outline-primary btn" Text="Upload" Width="100px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="capturePOA" tabindex="-1" role="dialog" aria-labelledby="poaCaptureLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="poaCaptureLabel">Capture POA</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div id="webcamPOA"></div>
                            <img id="capturedPOA" style="width:200px; height:150px;" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btn_poa_can" class="btn btn-outline-primary  btn" Text="Upload" Width="100px" />
                            <a class="btn btn-outline-primary btn" onclick="take_poa_snapshot();">Capture</a>
                        </div>
                    </div>
                </div>
            </div>
        </form>





        <link rel="stylesheet" type="text/css" href="css/smart_tab.min.css" />
        <script src="js/jquery.js" type="text/javascript"></script>
        <script src="js/jquery.smartTab.min.js" type="text/javascript"></script>
        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/webcamjs/1.0.25/webcam.js"></script>


        <script type="text/javascript">
            $(document).ready(function () {

                $('#smarttab').smartTab({
                    selected: 0, // Initial selected tab, 0 = first tab
                    // theme: 'dark', // theme for the tab, related css need to include for other than default theme
                    orientation: 'horizontal', // Nav menu orientation. horizontal/vertical
                    justified: false, // Nav menu justification. true/false
                    autoAdjustHeight: false, // Automatically adjust content height
                    backButtonSupport: false, // Enable the back button support
                    //enableURLhash: true, // Enable selection of the tab based on url hash
                    transition: {
                        animation: 'none', // Effect on navigation, none/fade/slide-horizontal/slide-vertical/slide-swing
                        speed: '400', // Transion animation speed
                        easing: '' // Transition animation easing. Not supported without a jQuery easing plugin
                    },
                    keyboardSettings: {
                        keyNavigation: false, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
                        keyLeft: [37], // Left key code
                        keyRight: [39] // Right key code
                    }
                });





                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);

                // Place here the first init of the autocomplete
                InitAutoCompl();
                InitAutoComplGrp();

                $("#smarttab").on("showTab", function (e, anchorObject, tabIndex) {


                    switch (tabIndex) {

                        case 0:
                            window.setTimeout(function () {
                                document.getElementById('<%=txtcid.ClientID %>').focus();
                            }, 0);
                            return true;

                            break;
                        case 1:
                            setTimeout(function () {
                                //  document.getElementById('<%=txtgrpid.ClientID %>').focus();
                            }, 500);

                            break;
                        case 2:
                            setTimeout(function () {
                                document.getElementById('<%=idprof.ClientID %>').focus();
                            }, 1000);

                            break;
                    }
                });



            });

            function InitializeRequest(sender, args) {
            }

            function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete
                InitAutoCompl();
                InitAutoComplGrp();
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

                        $("#<%=txtcid.ClientID %>").val(ui.item.memberno);
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
            } function InitAutoComplGrp() {

                $("#<%=txtgrpid.ClientID %>").autocomplete({
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

                        $("#<%=txtgrpid.ClientID %>").val(ui.item.memberno);
                        return false;

                    },
                    open: function () {
                        $("ul.ui-menu").width($(this).innerWidth() + 180);

                    },
                    select: function (event, ui) {

                        $("#<%=txtgrpid.ClientID %>").val(ui.item.memberno);


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


            function take_snapshot() {
                // take snapshot and get image data
                Webcam.snap(function (data_uri) {
                    // display results in page
                    $("#capturedImg")[0].src = data_uri;
                    // save to hidden field for server-side processing
                    $("#<%=hfCapturedImage.ClientID %>").val(data_uri);
                });
                Webcam.reset();
            }


            function upload_img() {

                $.ajax({
                    type: "POST",
                    url: "KYC.aspx/SaveCapturedImage",
                    data: "{data: '" + $("#capturedImg")[0].src + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) { }
                });



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

            function showwebcamSig() {

                Webcam.set({
                    width: 220,
                    height: 190,
                    image_format: 'jpeg',
                    jpeg_quality: 100
                });
                Webcam.attach('#webcamSig');

            };

            function take_signature_snapshot() {
                // take snapshot and get image data
                Webcam.snap(function (data_uri) {
                    // display results in page
                    $("#capturedSig")[0].src = data_uri;
                    // save to hidden field for server-side processing
                    $("#<%=hfCapturedSignature.ClientID %>").val(data_uri);
                });
                Webcam.reset();
            }

            function showwebcamPOI() {
                Webcam.set({
                    width: 220,
                    height: 190,
                    image_format: 'jpeg',
                    jpeg_quality: 100
                });
                Webcam.attach('#webcamPOI');
            }

            function take_poi_snapshot() {
                Webcam.snap(function (data_uri) {
                    $("#capturedPOI")[0].src = data_uri;
                    $("#<%=hfCapturedPOI.ClientID %>").val(data_uri);
                });
                Webcam.reset();
            }

            function showwebcamPOA() {
                Webcam.set({
                    width: 220,
                    height: 190,
                    image_format: 'jpeg',
                    jpeg_quality: 100
                });
                Webcam.attach('#webcamPOA');
            }

            function take_poa_snapshot() {
                Webcam.snap(function (data_uri) {
                    $("#capturedPOA")[0].src = data_uri;
                    $("#<%=hfCapturedPOA.ClientID %>").val(data_uri);
                });
                Webcam.reset();
            }

            //      // DOWNLOAD THE IMAGE.
            //  downloadImage = function (name, datauri) {





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