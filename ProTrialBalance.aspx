<%@ Page Title="Trial Balance" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="ProTrialBalance.aspx.vb" Inherits="Fiscus.ProTrialBalance" EnableEventValidation="false" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link rel="stylesheet" href="css/daterangepicker.css" />
        <style>
            @media print {

                .reportheader {
                    display: block;
                }
            }


            .reportheader {

                display: none;

            }
        </style>
        <script src="../js/jquery.js" type="text/javascript"></script>

        <script src="js/daterangepicker.js" type="text/javascript"></script>
        <script src="js/printThis.js" type="text/javascript"></script>

        <script type="text/javascript">
            function doprnt(br, rpthead) {
                var rh = "<h4>Karavilai Nidhi Limited</h4><span>Branch:" + br + "</span><span>" + rpthead + "</span>"

                $('#vouchprint').printThis({
                    importCSS: true,
                    importStyle: true,         // import style tags
                    printContainer: true,
                    header: rh
                });

            }
        </script>

    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form id="protb" runat="server">

            <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

            <nav class="page-breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active">Deposits & Loans</li>
                </ol>
            </nav>

            <div class="card card-body ">

                <div class="d-flex justify-content-between align-items-baseline">
                    <h6 class="card-title mb-0">Trial Balance</h6>
                    <div class="dropdown mb-2">
                        <button class="btn p-0" type="button" id="dropdownMenuButton" data-toggle="dropdown"
                            aria-haspopup="true" aria-expanded="false">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"
                                stroke-linejoin="round"
                                class="feather feather-more-horizontal icon-lg text-muted pb-3px">
                                <circle cx="12" cy="12" r="1"></circle>
                                <circle cx="19" cy="12" r="1"></circle>
                                <circle cx="5" cy="12" r="1"></circle>
                            </svg>
                        </button>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton" style="">
                            <asp:LinkButton ID="btnprnt" runat="server" class="dropdown-item d-flex align-items-center"
                                OnClientClick="document.forms[0].target = '_blank'; setTimeout(function(){document.forms[0].target='';}, 100);">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                    fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"
                                    stroke-linejoin="round" class="feather feather-printer icon-sm mr-2">
                                    <polyline points="6 9 6 2 18 2 18 9"></polyline>
                                    <path
                                        d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2">
                                    </path>
                                    <rect x="6" y="14" width="12" height="8"></rect>
                                </svg> <span class="">Print</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnxl" runat="server" class="dropdown-item d-flex align-items-center">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                    fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"
                                    stroke-linejoin="round" class="feather feather-download icon-sm mr-2">
                                    <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
                                    <polyline points="7 10 12 15 17 10"></polyline>
                                    <line x1="12" y1="15" x2="12" y2="3"></line>
                                </svg> <span class="">Download</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" id="pnlbtn">
                    <ContentTemplate>



                        <div class="form-group row  border-sm-bottom">

                            <label class="col-form-label  col-sm-1 text-primary">Account</label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="prod" runat="server" Width="150px" Style="margin-left:0px"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <label class="col-form-label col-sm-1 text-primary">Date</label>
                            <div class="col-sm-3">
                                <div class="input-group">

                                    <asp:TextBox ID="txtdate" runat="server" AutoCompleteType="None" Width="120px"
                                        CssClass="form-control "
                                        data-validation-engine="validate[required,funcCall[DateFormat[]]"
                                        data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                    <span class="input-group-btn input-group-append">
                                        <asp:LinkButton runat="server" id="btn_inw" CssClass="btn btn-outline-primary ">

                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18"
                                                viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
                                                stroke-linecap="square" stroke-linejoin="inherit"
                                                class="feather feather-search">
                                                <circle cx="11" cy="11" r="8"></circle>
                                                <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
                                            </svg>
                                        </asp:LinkButton>
                                    </span>

                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-check form-check-inline">
                                    <%--<label class="form-check-label">
                                        <asp:CheckBox runat="server" ID="isadd" AutoPostBack="true" />
                                        Show Address
                                        <i class="input-frame"></i></label>--%>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <%-- <asp:ImageButton CssClass="btn btn-outline-primary "
                                    ImageUrl="~/Admin/Images/address.png" runat="server" ID="btnadd" visible="false"
                                    style="margin-left:0px" Width="50px" Height="37px" />
                                <asp:Button CssClass="btn btn-outline-primary " Text="Export" runat="server"
                                    ID="btnexport" style="margin-left:90px" Visible="false" />
                                <asp:Button CssClass="btn btn-outline-primary " Text="Statement" runat="server"
                                    ID="btnsoa" style="margin-left:90px" Visible="false" />
                                --%>
                            </div>


                        </div>
                        <div id="vouchprint">

                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>


                                    <div class="reportheader">

                                        <asp:Label ID="lblbr" runat="server" Text="Branch :"></asp:Label>
                                        <asp:Label runat="server" ID="lblreport" Text="Fixed Deposit"></asp:Label>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <table style="margin-left:0px;width:90%;margin-top:10px" class="table table-bordered  ">
                                <thead>
                                    <tr>
                                        <th style="text-align:center;width:10%;">S.No</th>
                                        <th style="text-align:center;width:20%;">Account No</th>
                                        <th style="text-align:center;width:55%;">Customer Name</th>
                                        <th style="text-align:center;width:25%;">Balance</th>


                                    </tr>


                                </thead>
                            </table>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>


                                    <asp:GridView runat="server" ID="disp" AutoGenerateColumns="false"
                                        style="margin-top:-0px;margin-left:0px;float:left;z-index:1" AllowPaging="true"
                                        OnPageIndexChanging="OnPaging" PageSize="50" Width="90%" Visible="false"
                                        ShowHeaderWhenEmpty="false" ShowHeader="false" CssClass="table table-bordered">
                                        <Columns>

                                            <asp:TemplateField HeaderText="S No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>


                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblacn" runat="server" text='<%#Eval("acno")%>'
                                                        Style="text-align:right;float:right;font-size:small">
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblparty" runat="server" text='<%#Eval("party")%>'
                                                        Style="text-align:left ;float:left;font-size:small ">
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="55%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblamt" runat="server"
                                                        text='<%#Eval("amt", "{0:N}")%>'
                                                        Style="text-align:right;float:right ;font-size:small ">
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>

                                </ContentTemplate>
                            </asp:UpdatePanel>


                            <asp:GridView runat="server" ID="dispadd" AutoGenerateColumns="false"
                                style="margin-top:-0px;margin-left:0px;float:left;z-index:1" Width="90%" Visible="true"
                                ShowHeaderWhenEmpty="false" ShowHeader="false" CssClass="table table-bordered  ">
                                <Columns>

                                    <asp:TemplateField HeaderText="S No">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>


                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblacn" runat="server" text='<%#Eval("acno")%>'
                                                Style="text-align:right;float:right;font-size:small"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblparty" runat="server" text='<%#Eval("party")%>'
                                                Style="text-align:left ;float:left;font-size:small "></asp:Label>
                                            <br />
                                            <asp:Label ID="lblAddress" runat="server" text='<%#Eval("address")%>'
                                                Style="text-align:left ;float:left;font-size:x-small"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="55%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblamt" runat="server" text='<%#Eval("amt", "{0:N}")%>'
                                                Style="text-align:right;float:right ;font-size:small "></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="25%" />
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>



                            <table style="margin-left:0px;width:90%" class="table table-bordered  ">
                                <tbody>

                                    <tr>
                                        <td
                                            style="text-align:center;width:30%;font-variant:small-caps;font-size:medium">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblnoc" runat="server"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td
                                            style="text-align:center;width:55%;font-variant:small-caps;font-size:medium">
                                            Total</td>
                                        <td
                                            style="text-align:center;width:25%;font-variant:small-caps;font-size:medium">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lbltotal" runat="server"
                                                        Style="text-align:right;float:right "></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlbtn">
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
                                <br />
                                <span style="margin:0 auto"> Processing..... </span>
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="div-left " style="margin-left:28px;margin-top:5px">
                    <asp:Button ID="btn_ref" runat="server" CssClass="btn btn-secondary " Visible="false"
                        Text="Refresh" />
                </div>




                <!--card body -->
            </div>


        </form>





        <script type="text/javascript">


            $(document).ready(function () {


                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);

                // Place here the first init of the autocomplete
                InitAutoCompl();

            });


            function InitializeRequest(sender, args) {
            }

            function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete
                InitAutoCompl();
            }

            function InitAutoCompl() {


                $("#<%=txtdate.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtdate.ClientID%>").val(start.format('DD-MM-YYYY'));
                });


            }




        </script>



    </asp:Content>