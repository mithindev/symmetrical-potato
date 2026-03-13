<%@ Page Title="Audit Trail" Language="VB" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AuditTrail.aspx.vb" Inherits="Fiscus.AuditTrail" %>

    <%@ Import Namespace="System.Data" %>
    <%@ Import Namespace="Fiscus" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
    </asp:Content>

            <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

                <form id="dash" runat="server">

                    <style>
                        :root {
                            --primary-gradient: linear-gradient(135deg, #7C3AED, #2563EB);
                            --card-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.05), 0 8px 10px -6px rgba(0, 0, 0, 0.05);
                            --hover-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
                        }

                        body {
                            font-family: 'Inter', sans-serif;
                            background-color: #F3F4FF !important;
                            color: #1F2937;
                        }

                        .audit-container {
                            padding: 24px;
                            max-width: 1400px;
                            margin: 0 auto;
                        }

                        /* ===== Header Section ===== */
                        .page-header {
                            background: white;
                            padding: 24px 32px;
                            border-radius: 16px;
                            box-shadow: var(--card-shadow);
                            margin-bottom: 24px;
                            display: flex;
                            justify-content: space-between;
                            align-items: center;
                            position: relative;
                            overflow: hidden;
                        }

                        .page-header::before {
                            content: '';
                            position: absolute;
                            left: 0;
                            top: 0;
                            height: 100%;
                            width: 6px;
                            background: var(--primary-gradient);
                        }

                        .page-title {
                            margin: 0;
                            font-size: 24px;
                            font-weight: 800;
                            color: #111827;
                            letter-spacing: -0.02em;
                        }

                        /* ===== Buttons ===== */
                        .btn-premium {
                            display: inline-flex;
                            align-items: center;
                            gap: 8px;
                            padding: 10px 20px;
                            border-radius: 10px;
                            font-weight: 600;
                            font-size: 14px;
                            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                            cursor: pointer;
                            border: none;
                            text-decoration: none;
                        }

                        .btn-primary-gradient {
                            background: var(--primary-gradient);
                            color: white;
                            box-shadow: 0 4px 12px rgba(124, 58, 237, 0.25);
                        }

                        .btn-primary-gradient:hover {
                            transform: translateY(-2px);
                            box-shadow: 0 8px 20px rgba(124, 58, 237, 0.35);
                            filter: brightness(1.1);
                        }

                        .btn-outline {
                            background: white;
                            color: #4B5563;
                            border: 1px solid #E5E7EB;
                        }

                        .btn-outline:hover {
                            background: #F9FAFB;
                            border-color: #D1D5DB;
                            color: #111827;
                        }

                        /* ===== Summary Cards ===== */
                        .summary-grid {
                            display: grid;
                            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
                            gap: 20px;
                            margin-bottom: 32px;
                        }

                        .summary-card {
                            background: white;
                            padding: 24px;
                            border-radius: 16px;
                            box-shadow: var(--card-shadow);
                            display: flex;
                            align-items: center;
                            gap: 20px;
                            transition: all 0.3s ease;
                            border: 1px solid rgba(255, 255, 255, 0.8);
                        }

                        .summary-card:hover {
                            transform: translateY(-5px);
                            box-shadow: var(--hover-shadow);
                            border-color: #7C3AED;
                        }

                        .icon-box {
                            width: 56px;
                            height: 56px;
                            border-radius: 12px;
                            display: flex;
                            align-items: center;
                            justify-content: center;
                            font-size: 24px;
                        }

                        .icon-purple { background: rgba(124, 58, 237, 0.1); color: #7C3AED; }
                        .icon-blue { background: rgba(37, 99, 235, 0.1); color: #2563EB; }
                        .icon-emerald { background: rgba(16, 185, 129, 0.1); color: #10B981; }

                        .card-label {
                            font-size: 13px;
                            font-weight: 600;
                            color: #6B7280;
                            text-transform: uppercase;
                            letter-spacing: 0.05em;
                            margin-bottom: 4px;
                        }

                        .card-value {
                            font-size: 22px;
                            font-weight: 800;
                            color: #111827;
                        }

                        /* ===== Premium Table ===== */
                        .table-card {
                            background: white;
                            border-radius: 16px;
                            box-shadow: var(--card-shadow);
                            overflow: hidden;
                            margin-bottom: 24px;
                        }

                        .premium-table {
                            margin: 0 !important;
                            border: none !important;
                        }

                        .premium-table th {
                            background: #F9FAFB !important;
                            color: #4B5563 !important;
                            font-weight: 700 !important;
                            text-transform: uppercase !important;
                            font-size: 11px !important;
                            letter-spacing: 0.05em !important;
                            padding: 16px 20px !important;
                            border-bottom: 2px solid #F3F4F6 !important;
                            white-space: nowrap;
                        }

                        .premium-table td {
                            padding: 14px 20px !important;
                            font-size: 13px !important;
                            border-bottom: 1px solid #F3F4F6 !important;
                            color: #374151 !important;
                            vertical-align: middle;
                        }

                        .premium-table tr:last-child td {
                            border-bottom: none !important;
                        }

                        .premium-table tr:hover td {
                            background-color: #FDFEFE !important;
                        }

                        .badge-action {
                            padding: 4px 10px;
                            border-radius: 6px;
                            font-size: 11px;
                            font-weight: 700;
                            text-transform: uppercase;
                        }

                        /* ===== Modals ===== */
                        .modal-overlay {
                            position: fixed;
                            top: 0; left: 0; width: 100%; height: 100%;
                            background: rgba(17, 24, 39, 0.4);
                            backdrop-filter: blur(8px);
                            display: none;
                            z-index: 2000;
                            align-items: center;
                            justify-content: center;
                        }

                        .modal-content {
                            background: white;
                            width: 90%;
                            max-width: 600px;
                            border-radius: 20px;
                            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
                            overflow: hidden;
                            animation: modalSlideUp 0.4s cubic-bezier(0.16, 1, 0.3, 1);
                        }

                        @keyframes modalSlideUp {
                            from { transform: translateY(30px); opacity: 0; }
                            to { transform: translateY(0); opacity: 1; }
                        }

                        .modal-header {
                            background: var(--primary-gradient);
                            padding: 24px 32px;
                            display: flex;
                            justify-content: space-between;
                            align-items: center;
                            color: white;
                        }

                        .modal-body {
                            padding: 32px;
                        }

                        .modal-footer {
                            padding: 20px 32px;
                            background: #F9FAFB;
                            display: flex;
                            justify-content: flex-end;
                            gap: 12px;
                        }

                        /* Progress Loader Overlay */
                        .loader-overlay {
                            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
                            background: rgba(255, 255, 255, 0.8);
                            backdrop-filter: blur(5px);
                            display: flex; flex-direction: column; align-items: center; justify-content: center;
                            z-index: 3000;
                        }
                    </style>

                    <div class="audit-container">
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                        <!-- LOADER -->
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <div class="loader-overlay">
                                    <div class="spinner-border text-primary" style="width: 3rem; height: 3rem; color: #7C3AED !important;" role="status"></div>
                                    <h5 style="margin-top: 16px; font-weight: 700; color: #111827;">Fetching Audit Logs...</h5>
                                    <p style="color: #6B7280;">Secured system activity is being synchronized.</p>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                        <!-- HEADER -->
                        <div class="page-header">
                            <div>
                                <h3 class="page-title">Audit Trail</h3>
                                <p style="color: #6B7280; font-size: 14px; margin-top: 4px;">Comprehensive history of system changes and user activities.</p>
                            </div>
                            <div style="display: flex; gap: 12px;">
                                <asp:Button ID="btnAboutAudit" runat="server" Text="About Audit"
                                    CssClass="btn-premium btn-outline" OnClientClick="showAboutModal(); return false;" />
                                <asp:Button ID="btnDownloadAudit" runat="server" Text="Download Logs"
                                    CssClass="btn-premium btn-primary-gradient"
                                    OnClientClick="showDownloadModal(); return false;" />
                            </div>
                        </div>

                        <!-- SUMMARY DASHBOARD -->
                        <div class="summary-grid">
                            <div class="summary-card">
                                <div class="icon-box icon-purple">
                                    <i class="fas fa-database"></i>
                                </div>
                                <div>
                                    <div class="card-label">Total Records</div>
                                    <div class="card-value"><asp:Label ID="lblTotalRecords" runat="server" Text="0" /></div>
                                </div>
                            </div>
                            <div class="summary-card">
                                <div class="icon-box icon-blue">
                                    <i class="fas fa-clock-rotate-left"></i>
                                </div>
                                <div style="max-width: 200px;">
                                    <div class="card-label">Last Activity</div>
                                    <div class="card-value" style="font-size: 16px;"><asp:Label ID="lblLastActivity" runat="server" Text="-" /></div>
                                </div>
                            </div>
                            <div class="summary-card">
                                <div class="icon-box icon-emerald">
                                    <i class="fas fa-microchip"></i>
                                </div>
                                <div>
                                    <div class="card-label">Most Active Hub</div>
                                    <div class="card-value"><asp:Label ID="lblTopTable" runat="server" Text="-" /></div>
                                </div>
                            </div>
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <!-- GRID LOGS -->
                                <div class="table-card">
                                    <div style="padding: 20px 24px; border-bottom: 1px solid #F3F4F6; background: #FFF;">
                                        <h4 style="margin: 0; font-size: 16px; font-weight: 700; color: #111827;">System Activity Logs</h4>
                                    </div>
                                    <div style="overflow-x: auto;">
                                        <asp:GridView ID="gvAuditTrail" runat="server" AutoGenerateColumns="false" 
                                            CssClass="table premium-table" Width="100%" GridLines="None"
                                            EmptyDataText="No audit records found.">
                                            <Columns>
                                                <asp:BoundField DataField="AuditId" HeaderText="ID" />
                                                <asp:BoundField DataField="TableName" HeaderText="Module/Table" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="RecordId" HeaderText="Record ID" />
                                                <asp:BoundField DataField="ColumnName" HeaderText="Field" ItemStyle-ForeColor="#6B7280" />
                                                <asp:BoundField DataField="CreatedBy" HeaderText="Origin" />
                                                <asp:BoundField DataField="OldValue" HeaderText="Previous State" />
                                                <asp:BoundField DataField="NewValue" HeaderText="New State" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <span class='badge-action <%# If(Eval("ActionType").ToString() = "UPDATE", "text-blue-600 bg-blue-100", "text-red-600 bg-red-100") %>' 
                                                              style='padding: 4px 8px; border-radius: 4px; font-weight: 700; font-size: 10px;'>
                                                            <%# Eval("ActionType") %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ChangedBy" HeaderText="Operator" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="ChangedOn" HeaderText="Timestamp"
                                                    DataFormatString="{0:dd MMM yyyy HH:mm}" ItemStyle-ForeColor="#6B7280" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="padding: 16px 24px; background: #F9FAFB; border-top: 1px solid #F3F4F6; display: flex; justify-content: space-between; align-items: center;">
                                        <span style="font-size: 12px; color: #6B7280; font-style: italic;">
                                            <i class="fas fa-shield-alt" style="margin-right: 4px;"></i>Immutable security log
                                        </span>
                                        <span style="font-size: 12px; font-weight: 600; color: #4B5563;">Showing latest 10 audit records</span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <!-- ABOUT MODAL -->
                        <div id="aboutModal" class="modal-overlay">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 style="margin: 0; font-weight: 800;">About Audit Trail</h4>
                                    <button type="button" onclick="closeAboutModal()" style="background: transparent; border: none; color: white; font-size: 20px; cursor: pointer;"><i class="fas fa-times"></i></button>
                                </div>
                                <div class="modal-body">
                                    <p style="font-size: 15px; line-height: 1.6; color: #4B5563; margin-bottom: 24px;">
                                        The Audit Trail system provides multi-layered transparency and absolute accountability for every critical operation performed within the Fiscus platform.
                                    </p>
                                    <div style="display: grid; gap: 16px;">
                                        <div style="display: flex; gap: 12px; align-items: start;">
                                            <div style="color: #7C3AED; margin-top: 4px;"><i class="fas fa-check-circle"></i></div>
                                            <div><strong style="color: #111827;">Comprehensive Tracking</strong><br/><span style="font-size: 13px; color: #6B7280;">Monitors all UPDATE and DELETE operations automatically.</span></div>
                                        </div>
                                        <div style="display: flex; gap: 12px; align-items: start;">
                                            <div style="color: #7C3AED; margin-top: 4px;"><i class="fas fa-history"></i></div>
                                            <div><strong style="color: #111827;">Differential Logging</strong><br/><span style="font-size: 13px; color: #6B7280;">Stores both the previous (original) and new values for comparison.</span></div>
                                        </div>
                                        <div style="display: flex; gap: 12px; align-items: start;">
                                            <div style="color: #7C3AED; margin-top: 4px;"><i class="fas fa-lock"></i></div>
                                            <div><strong style="color: #111827;">Immutable Storage</strong><br/><span style="font-size: 13px; color: #6B7280;">Data is cryptographically protected and cannot be altered by any user.</span></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <span style="margin-right: auto; font-size: 11px; color: #9CA3AF; font-weight: 600; text-transform: uppercase;">Compliance: Companies Act, 2013</span>
                                    <button type="button" class="btn-premium btn-outline" onclick="closeAboutModal()">Close</button>
                                </div>
                            </div>
                        </div>

                        <!-- DOWNLOAD MODAL -->
                        <div id="downloadModal" class="modal-overlay">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 style="margin: 0; font-weight: 800;">Download Audit Logs</h4>
                                    <button type="button" onclick="closeDownloadModal()" style="background: transparent; border: none; color: white; font-size: 20px; cursor: pointer;"><i class="fas fa-times"></i></button>
                                </div>
                                <div class="modal-body">
                                    <div style="margin-bottom: 24px;">
                                        <div style="background: #F3F4F6; padding: 16px; border-radius: 12px; display: flex; gap: 12px; margin-bottom: 16px; border: 1px solid #E5E7EB;">
                                            <asp:RadioButton ID="rbAllData" runat="server" GroupName="DownloadType" Checked="true" />
                                            <label for="rbAllData" style="margin: 0;">
                                                <strong style="color: #111827; display: block;">Full Export</strong>
                                                <span style="font-size: 12px; color: #6B7280;">Download the entire vault history in PDF format.</span>
                                            </label>
                                        </div>

                                        <div style="background: #F3F4F6; padding: 16px; border-radius: 12px; display: flex; gap: 12px; border: 1px solid #E5E7EB;">
                                            <asp:RadioButton ID="rbDateRange" runat="server" GroupName="DownloadType" />
                                            <label for="rbDateRange" style="margin: 0;">
                                                <strong style="color: #111827; display: block;">Selective Window</strong>
                                                <span style="font-size: 12px; color: #6B7280;">Filter records between specific architectural periods.</span>
                                            </label>
                                        </div>
                                    </div>

                                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px; margin-bottom: 8px;">
                                        <div>
                                            <label style="display: block; font-size: 12px; font-weight: 700; color: #4B5563; margin-bottom: 6px;">Start Date</label>
                                            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date" style="width: 100%; padding: 10px; border-radius: 8px; border: 1px solid #D1D5DB;" />
                                        </div>
                                        <div>
                                            <label style="display: block; font-size: 12px; font-weight: 700; color: #4B5563; margin-bottom: 6px;">End Date</label>
                                            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date" style="width: 100%; padding: 10px; border-radius: 8px; border: 1px solid #D1D5DB;" />
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn-premium btn-outline" onclick="closeDownloadModal()">Cancel</button>
                                    <asp:Button ID="btnDownload" runat="server" Text="Download PDF"
                                        CssClass="btn-premium btn-primary-gradient" OnClick="btnDownload_Click" />
                                </div>
                            </div>
                        </div>
                    </div>


                </form>

                <!-- SCRIPTS -->
                <script type="text/javascript">
                    function showAboutModal() {
                        document.getElementById('aboutModal').style.display = 'flex';
                    }

                    function closeAboutModal() {
                        document.getElementById('aboutModal').style.display = 'none';
                    }

                    function showDownloadModal() {
                        document.getElementById('downloadModal').style.display = 'flex';
                    }

                    function closeDownloadModal() {
                        document.getElementById('downloadModal').style.display = 'none';
                    }
                </script>
            </asp:Content>