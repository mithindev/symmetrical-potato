<%@ Page Title="Member Merge" Language="VB" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="MemberMerge.aspx.vb" Inherits="Fiscus.MemberMerge" %>

    <%@ Import Namespace="System.Data" %>
    <%@ Import Namespace="System.IO" %>
    <%@ Import Namespace="Fiscus" %>

        <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">

            <style>
                .mm-card {
                    background: white;
                    border-radius: 16px;
                    box-shadow: 0 10px 30px rgba(0,0,0,0.05);
                    border: none;
                    padding: 28px;
                    height: 100%;
                }
                .mm-card-title {
                    color: #5B21B6;
                    font-weight: 600;
                    font-size: 16px;
                    margin-bottom: 6px;
                    font-family: 'Inter', sans-serif;
                    display: flex;
                    align-items: center;
                    gap: 9px;
                }
                .mm-accent-bar {
                    background: linear-gradient(90deg, #6D28D9, #2563EB);
                    height: 3px;
                    width: 40px;
                    border-radius: 4px;
                    margin-bottom: 22px;
                }
                .mm-label {
                    font-size: 13px;
                    font-weight: 600;
                    color: #4B5563;
                    font-family: 'Inter', sans-serif;
                    margin-bottom: 7px;
                    display: block;
                }
                .mm-input {
                    border-radius: 8px !important;
                    border: 1px solid #E5E7EB !important;
                    padding: 9px 12px !important;
                    font-size: 14px !important;
                    transition: all 0.2s !important;
                    font-family: 'Inter', sans-serif !important;
                    width: 100% !important;
                    color: #374151 !important;
                    background-color: #fff !important;
                }
                .mm-input:focus {
                    border-color: #7C3AED !important;
                    box-shadow: 0 0 0 3px rgba(124,58,237,0.15) !important;
                    outline: none !important;
                }
                .mm-file-upload {
                    border-radius: 8px;
                    border: 2px dashed #C4B5FD;
                    padding: 18px 14px;
                    background: #F5F3FF;
                    width: 100%;
                    font-family: 'Inter', sans-serif;
                    font-size: 13px;
                    color: #6B7280;
                    transition: all 0.2s;
                    display: block;
                    box-sizing: border-box;
                }
                .mm-file-upload:hover {
                    border-color: #7C3AED;
                    background: #EDE9FE;
                }
                .mm-status-panel {
                    background: white;
                    border-radius: 14px;
                    border-left: 4px solid #7C3AED;
                    box-shadow: 0 8px 24px rgba(0,0,0,0.04);
                    padding: 18px 24px;
                    display: flex;
                    align-items: center;
                    gap: 14px;
                    margin-top: 20px;
                }
            </style>

            <form id="mergeForm" runat="server"
                style="background-color: #F3F4FF; font-family: 'Inter', sans-serif; min-height: 100vh; padding: 20px;">

                <%-- Breadcrumb --%>
                <nav class="page-breadcrumb" style="margin-bottom: 24px;">
                    <ol class="breadcrumb" style="background: transparent; padding: 0;">
                        <li class="breadcrumb-item"><a href="#" style="color: #6B7280; text-decoration: none;">Admin</a></li>
                        <li class="breadcrumb-item active" style="color: #374151; font-weight: 500;">Member Merge</li>
                    </ol>
                </nav>

                <%-- Page Header --%>
                <div style="margin-bottom: 24px;">
                    <h4 style="font-size: 1.35rem; font-weight: 700; color: #0F172A; margin: 0 0 4px 0; font-family: 'Inter', sans-serif; display: flex; align-items: center; gap: 10px;">
                        <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                            <polyline points="17 1 21 5 17 9"></polyline>
                            <path d="M3 11V9a4 4 0 0 1 4-4h14"></path>
                            <polyline points="7 23 3 19 7 15"></polyline>
                            <path d="M21 13v2a4 4 0 0 1-4 4H3"></path>
                        </svg>
                        Member Synchronization Center
                    </h4>
                    <p style="margin: 0; color: #6B7280; font-size: 14px; font-family: 'Inter', sans-serif; padding-left: 32px;">
                        Securely export and merge member data across business branches.
                    </p>
                </div>

                <%-- Two Cards: Export + Import --%>
                <div class="row">

                    <%-- Export / Download Card --%>
                    <div class="col-md-6" style="margin-bottom: 20px;">
                        <div class="mm-card">
                            <h6 class="mm-card-title">
                                <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                                    <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
                                    <polyline points="7 10 12 15 17 10"></polyline>
                                    <line x1="12" y1="15" x2="12" y2="3"></line>
                                </svg>
                                Export Data
                            </h6>
                            <div class="mm-accent-bar"></div>

                            <p style="font-size: 13px; color: #6B7280; margin: 0 0 20px 0; font-family: 'Inter', sans-serif;">
                                Select a table and download a backup file for synchronization with another branch.
                            </p>

                            <div class="form-group" style="margin-bottom: 20px;">
                                <label class="mm-label">Select Table to Export</label>
                                <asp:DropDownList ID="ddlTables" runat="server"
                                    CssClass="form-control mm-input"
                                    onfocus="this.style.borderColor='#7C3AED'; this.style.boxShadow='0 0 0 3px rgba(124,58,237,0.15)';"
                                    onblur="this.style.borderColor='#E5E7EB'; this.style.boxShadow='none';" />
                            </div>

                            <asp:Button ID="btnDownload" runat="server" Text="Download Backup File"
                                OnClick="btnDownload_Click"
                                CssClass="btn"
                                style="width: 100%; background: linear-gradient(135deg, #7C3AED, #2563EB); color: white; border-radius: 10px; padding: 11px 20px; border: none; font-weight: 600; font-family: 'Inter', sans-serif; font-size: 14px; transition: all 0.2s; box-shadow: 0 4px 14px rgba(37,99,235,0.2); cursor: pointer;"
                                onmouseover="this.style.transform='translateY(-2px)'; this.style.boxShadow='0 6px 20px rgba(37,99,235,0.3)';"
                                onmouseout="this.style.transform='translateY(0)'; this.style.boxShadow='0 4px 14px rgba(37,99,235,0.2)';" />
                        </div>
                    </div>

                    <%-- Import & Merge Card --%>
                    <div class="col-md-6" style="margin-bottom: 20px;">
                        <div class="mm-card">
                            <h6 class="mm-card-title">
                                <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="#059669" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                                    <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
                                    <polyline points="17 8 12 3 7 8"></polyline>
                                    <line x1="12" y1="3" x2="12" y2="15"></line>
                                </svg>
                                Import &amp; Merge
                            </h6>
                            <div class="mm-accent-bar" style="background: linear-gradient(90deg, #059669, #0284C7);"></div>

                            <p style="font-size: 13px; color: #6B7280; margin: 0 0 20px 0; font-family: 'Inter', sans-serif;">
                                Upload a backup file from another branch to merge and synchronize records.
                            </p>

                            <div class="form-group" style="margin-bottom: 20px;">
                                <label class="mm-label">Upload Backup File</label>
                                <asp:FileUpload ID="fuTableFile" runat="server"
                                    CssClass="mm-file-upload" />
                                <p style="font-size: 12px; color: #9CA3AF; margin: 8px 0 0 0; font-family: 'Inter', sans-serif;">
                                    Accepted format: <strong>.csv</strong> or <strong>.bak</strong> exported from this system.
                                </p>
                            </div>

                            <asp:Button ID="btnUpload" runat="server" Text="Upload &amp; Merge Data"
                                OnClick="btnUpload_Click"
                                CssClass="btn"
                                style="width: 100%; background: linear-gradient(135deg, #059669, #0284C7); color: white; border-radius: 10px; padding: 11px 20px; border: none; font-weight: 600; font-family: 'Inter', sans-serif; font-size: 14px; transition: all 0.2s; box-shadow: 0 4px 14px rgba(5,150,105,0.2); cursor: pointer;"
                                onmouseover="this.style.transform='translateY(-2px)'; this.style.boxShadow='0 6px 20px rgba(5,150,105,0.3)';"
                                onmouseout="this.style.transform='translateY(0)'; this.style.boxShadow='0 4px 14px rgba(5,150,105,0.2)';" />
                        </div>
                    </div>

                </div>

                <%-- Status Panel --%>
                <div class="mm-status-panel">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#7C3AED" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <circle cx="12" cy="12" r="10"></circle>
                        <line x1="12" y1="16" x2="12" y2="12"></line>
                        <line x1="12" y1="8" x2="12.01" y2="8"></line>
                    </svg>
                    <div>
                        <div style="font-size: 11px; font-weight: 600; color: #6B7280; text-transform: uppercase; letter-spacing: 0.6px; margin-bottom: 3px; font-family: 'Inter', sans-serif;">
                            Operation Status
                        </div>
                        <asp:Label ID="lblStatus" runat="server"
                            style="font-size: 15px; font-weight: 600; color: #111827; font-family: 'Inter', sans-serif;" />
                    </div>
                </div>

            </form>

        </asp:Content>