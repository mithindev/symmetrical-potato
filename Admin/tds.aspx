<%@ Page Title="TDS" Language="vb" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="tds.aspx.vb" Inherits="Fiscus.tds" %>
<%@ Import Namespace="Fiscus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet">
    <style>
        body, .tds-container, .page-title, .form-label, .btn, .table {
            font-family: 'Inter', sans-serif !important;
        }
        .tds-container {
            padding: 24px;
            background-color: #F3F4FF;
            min-height: 100vh;
        }
        .page-header {
            margin-bottom: 24px;
        }
        .page-title {
            font-weight: 700;
            color: #1F2937;
            font-size: 24px;
            letter-spacing: -0.02em;
        }
        .card-bordered {
            border: none;
            border-radius: 16px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.04);
            background: #ffffff;
            margin-bottom: 20px;
        }
        .btn {
            border-radius: 10px;
            font-weight: 500;
            padding: 10px 24px;
            transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
            font-size: 14px;
        }
        
        /* Updated Premium Loader */
        .loader-overlay {
            position: fixed;
            top: 0; left: 0; right: 0; bottom: 0;
            background: rgba(255, 255, 255, 0.7);
            z-index: 9999;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            backdrop-filter: blur(8px);
        }
        .loader-container {
            background: white;
            padding: 30px 40px;
            border-radius: 24px;
            box-shadow: 0 20px 50px rgba(0,0,0,0.1);
            display: flex;
            flex-direction: column;
            align-items: center;
        }
        .spinner {
            width: 48px;
            height: 48px;
            border: 4px solid #E5E7EB;
            border-top: 4px solid #7C3AED;
            border-radius: 50%;
            animation: spin 0.8s cubic-bezier(0.68, -0.55, 0.265, 1.55) infinite;
        }
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        .loader-text {
            margin-top: 20px;
            font-weight: 600;
            color: #4B5563;
            font-size: 15px;
        }

        /* Table Styling */
        .premium-table {
            border-radius: 12px !important;
            overflow: hidden;
            border: 1px solid #E5E7EB !important;
        }
        .premium-table th {
            background: #F9FAFB !important;
            color: #4B5563 !important;
            font-weight: 600 !important;
            text-transform: none !important;
            font-size: 13px !important;
            padding: 12px 16px !important;
            border-bottom: 1px solid #E5E7EB !important;
            white-space: nowrap;
        }
        .premium-table td {
            font-size: 13px !important;
            padding: 10px 16px !important;
            border-color: #F3F4F6 !important;
            color: #374151 !important;
            white-space: nowrap;
        }
        
        /* Specialized Column Headers */
        .header-tds {
            background-color: #FEF3C7 !important;
            color: #92400E !important;
        }
        .header-grand-total {
            background-color: #7C3AED !important;
            color: white !important;
        }
        .header-tds-total {
            background-color: #DC2626 !important;
            color: white !important;
        }

        /* Pagination Styling */
        .custom-pager table {
            margin: 20px auto !important;
        }
        .custom-pager td {
            padding: 0 4px !important;
        }
        .custom-pager a, .custom-pager span {
            display: flex !important;
            align-items: center !important;
            justify-content: center !important;
            width: 32px !important;
            height: 32px !important;
            border-radius: 8px !important;
            font-size: 13px !important;
            font-weight: 600 !important;
            text-decoration: none !important;
            transition: all 0.2s !important;
        }
        .custom-pager a {
            background: white !important;
            border: 1px solid #E5E7EB !important;
            color: #6B7280 !important;
        }
        .custom-pager a:hover {
            border-color: #7C3AED !important;
            color: #7C3AED !important;
            background: #F3F4FF !important;
        }
        .custom-pager span {
            background: #7C3AED !important;
            color: white !important;
            border: 1px solid #7C3AED !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="tds-container">
            <div class="page-header">
                <h3 class="page-title">TDS Management</h3>
                <p style="color: #6B7280; font-size: 14px; margin-top: 4px;">Generate, export and verify TDS reports for all branches.</p>
                <div style="background: linear-gradient(90deg, #7C3AED, #2563EB); height: 3px; width: 40px; border-radius: 4px; margin-top: 10px;"></div>
            </div>
            
            <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExport" />
                        <asp:PostBackTrigger ControlID="btnExportCSV" />
                    </Triggers>
                    <ContentTemplate>
                        
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div class="loader-overlay">
                                    <div class="loader-container">
                                        <div class="spinner"></div>
                                        <div class="loader-text">Fetching TDS Data...</div>
                                        <p style="color: #9CA3AF; font-size: 12px; margin-top: 8px;">This might take a few seconds</p>
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        
                        <div class="row">
                            <div class="col-md-12">
                                <div class="card card-bordered">
                                    <div class="card-body" style="padding: 24px;">
                                        <div class="row align-items-end">
                                            <div class="col-sm-3">
                                                <div class="form-group mb-0">
                                                    <label class="form-label" style="font-size: 13px; font-weight: 600; color: #4B5563; margin-bottom: 8px; display: block;">Financial Year</label>
                                                    <asp:DropDownList ID="ddlFY" runat="server" CssClass="form-control" style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 8px 12px; font-size: 14px; color: #374151; outline: none; transition: border-color 0.2s;" onfocus="this.style.borderColor='#7C3AED';" onblur="this.style.borderColor='#E5E7EB';">
                                                        <asp:ListItem Text="2025-2026" Value="2025"></asp:ListItem>
                                                        <asp:ListItem Text="2024-2025" Value="2024"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-group mb-0">
                                                    <label class="form-label" style="font-size: 13px; font-weight: 600; color: #4B5563; margin-bottom: 8px; display: block;">Data Source</label>
                                                    <asp:DropDownList ID="ddlDataSource" runat="server" CssClass="form-control" style="border-radius: 8px; border: 1px solid #E5E7EB; padding: 8px 12px; font-size: 14px; color: #374151; outline: none; transition: border-color 0.2s;" onfocus="this.style.borderColor='#7C3AED';" onblur="this.style.borderColor='#E5E7EB';">
                                                        <asp:ListItem Text="Local Branch" Value="Local"></asp:ListItem>
                                                        <asp:ListItem Text="Consolidated Hub" Value="Hub"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 text-right">
                                                <div style="display: flex; gap: 10px; justify-content: flex-end;">
                                                    <asp:Button ID="btnFetch" runat="server" Text="Fetch TDS Report" 
                                                        style="background: linear-gradient(135deg, #7C3AED, #2563EB); border: none; color: white; box-shadow: 0 4px 12px rgba(124, 58, 237, 0.25);" 
                                                        CssClass="btn" OnClick="btnFetch_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div style="margin-top: 24px; padding-top: 24px; border-top: 1px solid #F3F4F6; display: flex; align-items: center; justify-content: space-between;">
                                            <div style="display: flex; align-items: center; gap: 12px; flex: 1;">
                                                <label style="font-size: 13px; font-weight: 600; color: #4B5563; white-space: nowrap;">Export Filename:</label>
                                                <div style="position: relative; flex: 0 1 300px;">
                                                    <asp:TextBox ID="txtExportFileName" runat="server" placeholder="e.g. FY25_TDS_Report"
                                                        style="width: 100%; border-radius: 8px; border: 1px solid #E5E7EB; padding: 8px 45px 8px 12px; font-size: 14px; outline: none;"
                                                        onfocus="this.style.borderColor='#7C3AED';" onblur="this.style.borderColor='#E5E7EB';"></asp:TextBox>
                                                    <span style="position: absolute; right: 12px; top: 50%; transform: translateY(-50%); color: #9CA3AF; font-size: 12px; font-weight: 500;">.dat</span>
                                                </div>
                                            </div>
                                            <div style="display: flex; gap: 10px;">
                                                <asp:Button ID="btnExport" runat="server" Text="Export to .DAT" 
                                                    style="border: 1px solid #10B981; color: #10B981; background: transparent;" 
                                                    CssClass="btn" OnClick="btnExport_Click" onmouseover="this.style.background='#ECFDF5'" onmouseout="this.style.background='transparent'" />
                                                <asp:Button ID="btnExportCSV" runat="server" Text="Export CSV" 
                                                    style="border: 1px solid #3B82F6; color: #3B82F6; background: transparent;" 
                                                    CssClass="btn" OnClick="btnExportCSV_Click" onmouseover="this.style.background='#EFF6FF'" onmouseout="this.style.background='transparent'" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-4">
                            <div class="col-md-12">
                                <div class="card card-bordered" style="overflow: hidden;">
                                    <div class="card-body" style="padding: 0;">
                                        <div class="table-responsive" style="overflow-x: auto;">
                                            <asp:GridView ID="gvTDSReport" runat="server" CssClass="table premium-table" AutoGenerateColumns="False" 
                                                ShowFooter="True" Width="100%" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvTDSReport_PageIndexChanging" 
                                                PagerStyle-CssClass="custom-pager">
                                                <HeaderStyle CssClass="premium-header" />
                                                <Columns>
                                                    <asp:BoundField DataField="MemberNo" HeaderText="Member No" />
                                                    <asp:BoundField DataField="FirstName" HeaderText="Name" />
                                                    <asp:BoundField DataField="address" HeaderText="Complete Address" />
                                                    <asp:BoundField DataField="pincode" HeaderText="Pincode" />
                                                    <asp:BoundField DataField="mobile" HeaderText="Mobile" />
                                                    <asp:BoundField DataField="dob" HeaderText="Date of Birth" />
                                                    <asp:BoundField DataField="PAN" HeaderText="PAN No" />
                                                    
                                                    <%-- Month Columns (Abbreviated to keep width manageable) --%>
                                                    <asp:BoundField DataField="Apr_RD" HeaderText="Apr RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Apr_FD" HeaderText="Apr FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Apr_RID" HeaderText="Apr RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Apr_SB" HeaderText="Apr SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Apr_KMK" HeaderText="Apr KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Apr_Total" HeaderText="Apr Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Apr_TDS" HeaderText="Apr TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />
                                                    
                                                    <%-- ... other months follow same pattern ... --%>
                                                    <%-- May --%>
                                                    <asp:BoundField DataField="May_RD" HeaderText="May RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="May_FD" HeaderText="May FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="May_RID" HeaderText="May RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="May_SB" HeaderText="May SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="May_KMK" HeaderText="May KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="May_Total" HeaderText="May Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="May_TDS" HeaderText="May TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- June --%>
                                                    <asp:BoundField DataField="Jun_RD" HeaderText="Jun RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jun_FD" HeaderText="Jun FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jun_RID" HeaderText="Jun RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jun_SB" HeaderText="Jun SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jun_KMK" HeaderText="Jun KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jun_Total" HeaderText="Jun Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Jun_TDS" HeaderText="Jun TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- July --%>
                                                    <asp:BoundField DataField="Jul_RD" HeaderText="Jul RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jul_FD" HeaderText="Jul FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jul_RID" HeaderText="Jul RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jul_SB" HeaderText="Jul SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jul_KMK" HeaderText="Jul KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jul_Total" HeaderText="Jul Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Jul_TDS" HeaderText="Jul TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- August --%>
                                                    <asp:BoundField DataField="Aug_RD" HeaderText="Aug RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Aug_FD" HeaderText="Aug FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Aug_RID" HeaderText="Aug RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Aug_SB" HeaderText="Aug SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Aug_KMK" HeaderText="Aug KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Aug_Total" HeaderText="Aug Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Aug_TDS" HeaderText="Aug TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- September --%>
                                                    <asp:BoundField DataField="Sep_RD" HeaderText="Sep RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Sep_FD" HeaderText="Sep FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Sep_RID" HeaderText="Sep RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Sep_SB" HeaderText="Sep SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Sep_KMK" HeaderText="Sep KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Sep_Total" HeaderText="Sep Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Sep_TDS" HeaderText="Sep TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- October --%>
                                                    <asp:BoundField DataField="Oct_RD" HeaderText="Oct RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Oct_FD" HeaderText="Oct FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Oct_RID" HeaderText="Oct RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Oct_SB" HeaderText="Oct SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Oct_KMK" HeaderText="Oct KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Oct_Total" HeaderText="Oct Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Oct_TDS" HeaderText="Oct TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- November --%>
                                                    <asp:BoundField DataField="Nov_RD" HeaderText="Nov RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Nov_FD" HeaderText="Nov FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Nov_RID" HeaderText="Nov RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Nov_SB" HeaderText="Nov SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Nov_KMK" HeaderText="Nov KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Nov_Total" HeaderText="Nov Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Nov_TDS" HeaderText="Nov TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- December --%>
                                                    <asp:BoundField DataField="Dec_RD" HeaderText="Dec RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Dec_FD" HeaderText="Dec FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Dec_RID" HeaderText="Dec RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Dec_SB" HeaderText="Dec SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Dec_KMK" HeaderText="Dec KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Dec_Total" HeaderText="Dec Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Dec_TDS" HeaderText="Dec TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- January --%>
                                                    <asp:BoundField DataField="Jan_RD" HeaderText="Jan RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jan_FD" HeaderText="Jan FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jan_RID" HeaderText="Jan RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jan_SB" HeaderText="Jan SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jan_KMK" HeaderText="Jan KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Jan_Total" HeaderText="Jan Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Jan_TDS" HeaderText="Jan TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- February --%>
                                                    <asp:BoundField DataField="Feb_RD" HeaderText="Feb RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Feb_FD" HeaderText="Feb FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Feb_RID" HeaderText="Feb RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Feb_SB" HeaderText="Feb SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Feb_KMK" HeaderText="Feb KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Feb_Total" HeaderText="Feb Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Feb_TDS" HeaderText="Feb TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <%-- March --%>
                                                    <asp:BoundField DataField="Mar_RD" HeaderText="Mar RD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Mar_FD" HeaderText="Mar FD" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Mar_RID" HeaderText="Mar RID" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Mar_SB" HeaderText="Mar SB" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Mar_KMK" HeaderText="Mar KMK" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Mar_Total" HeaderText="Mar Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="bg-light" />
                                                    <asp:BoundField DataField="Mar_TDS" HeaderText="Mar TDS" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds" />

                                                    <asp:BoundField DataField="TotalInterest" HeaderText="Grand Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-grand-total" />
                                                    <asp:BoundField DataField="TotalMemberTDS" HeaderText="TDS Total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="header-tds-total" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div style="padding: 40px; text-align: center; color: #6B7280;">
                                                        <i class="fas fa-search" style="font-size: 24px; margin-bottom: 12px; display: block;"></i>
                                                        No data found for the selected period.
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            <div class="col-lg-12 stretch-card mt-4">
                                <div class="card card-bordered" style="border-left: 4px solid #0EA5E9;">
                                    <div class="card-header" style="background: white; border-bottom: 1px solid #F3F4F6; padding: 16px 24px;">
                                        <h5 style="margin: 0; font-size: 16px; font-weight: 700; color: #0369A1;">
                                            <i class="fas fa-server" style="margin-right: 8px;"></i>Batch Consolidation Hub
                                        </h5>
                                    </div>
                                    <div class="card-body" style="padding: 24px;">
                                        <p style="color: #6B7280; font-size: 14px; margin-bottom: 20px;">Place all branch <strong>.dat</strong> files into the <strong>root /tds/</strong> folder. Click below to consolidate data into the central hub.</p>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:Button ID="btnBatchImport" runat="server" Text="Run Batch Import" 
                                                    style="background: #0EA5E9; border: none; color: white; width: 100%; box-shadow: 0 4px 12px rgba(14, 165, 233, 0.2);" 
                                                    CssClass="btn" OnClick="btnBatchImport_Click" />
                                            </div>
                                        </div>
                                        
                                        <asp:UpdateProgress ID="upgImport" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <div style="margin-top: 20px; padding: 12px 20px; background: #FEF3C7; border-radius: 8px; color: #92400E; font-size: 13px; font-weight: 500; display: flex; align-items: center; gap: 10px;">
                                                    <div class="spinner-border spinner-border-sm" role="status"></div>
                                                    Processing branch files... Please wait.
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        
                                        <div style="margin-top: 24px; padding: 20px; background: #F9FAFB; border: 1px solid #E5E7EB; border-radius: 12px;">
                                            <h6 style="font-size: 13px; font-weight: 700; color: #374151; margin-bottom: 12px; text-transform: uppercase; letter-spacing: 0.05em;">Import Summary</h6>
                                            <asp:Label ID="lblImportStatus" runat="server" style="font-size: 14px; font-weight: 600;"></asp:Label>
                                            <div style="margin-top: 12px; font-size: 13px; color: #4B5563; line-height: 1.6;">
                                                <asp:Literal ID="litDashboardLog" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-lg-12 stretch-card mt-4">
                                <div class="card card-bordered">
                                    <div class="card-header" style="background: white; border-bottom: 1px solid #F3F4F6; padding: 16px 24px;">
                                        <h5 style="margin: 0; font-size: 16px; font-weight: 700; color: #1F2937;">Member Transaction Verification</h5>
                                    </div>
                                    <div class="card-body" style="padding: 24px;">
                                        <div class="row mb-4">
                                            <div class="col-md-5">
                                                <label class="form-label" style="font-size: 13px; font-weight: 600; color: #4B5563; margin-bottom: 8px; display: block;">Search Member No</label>
                                                <div style="display: flex; gap: 10px;">
                                                    <asp:TextBox ID="txtSearchMemberNo" runat="server" placeholder="Enter Member ID..."
                                                        style="flex: 1; border-radius: 8px; border: 1px solid #E5E7EB; padding: 8px 12px; font-size: 14px;"
                                                        onfocus="this.style.borderColor='#7C3AED';" onblur="this.style.borderColor='#E5E7EB';"></asp:TextBox>
                                                    <asp:Button ID="btnVerifyMember" runat="server" Text="Verify" 
                                                        style="background: #6B7280; border: none; color: white;" 
                                                        CssClass="btn" OnClick="btnVerifyMember_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive" style="border-radius: 12px; border: 1px solid #E5E7EB;">
                                            <asp:GridView ID="gvMemberDetails" runat="server" CssClass="table premium-table" AutoGenerateColumns="False" 
                                                ShowFooter="True" Width="100%" EmptyDataText="No transactions found for this member ID." 
                                                AllowPaging="True" PageSize="10" OnPageIndexChanging="gvMemberDetails_PageIndexChanging" 
                                                PagerStyle-CssClass="custom-pager">
                                                <HeaderStyle CssClass="premium-header" />
                                                <Columns>
                                                    <asp:BoundField DataField="MonthName" HeaderText="Month" />
                                                    <asp:BoundField DataField="Product" HeaderText="Product" />
                                                    <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                    <asp:BoundField DataField="AcNo" HeaderText="Account No" />
                                                    <asp:BoundField DataField="Narration" HeaderText="Narration" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </form>
        </div>
</asp:Content>
