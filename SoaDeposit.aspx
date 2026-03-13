<%@ Page Title="Deposit - Statement" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="SoaDeposit.aspx.vb" Inherits="Fiscus.SoaDeposit" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link rel="stylesheet" href="css/Customer.css" />
        <link rel="stylesheet" href="css/daterangepicker.css" />
        <style type="text/css">
            @media print {
                @page {
                    size: A4 portrait;
                    /* landscape */
                    /* you can also specify margins here: */
                    margin: 5mm;
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    /* for compatibility with both A4 and Letter */
                }

                .prntarea {

                    height: 297mm;
                    width: 210mm;
                    margin: 7mm
                }

            }

            .label-cap {
                font-size: 12px;
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            }

            .prntarea {
                height: 297mm;
                width: 210mm;
                margin: 7mm;



            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form runat="server">
            <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

            <nav class="page-breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="#">Deposit</a></li>

                    <li class="breadcrumb-item active">Statement of Accounts</li>
                </ol>
            </nav>

            <asp:UpdatePanel ID="alertmsg" runat="server" Visible="false">
                <ContentTemplate>
                    <div class="alert alert-danger   alert-dismissible fade show" role="alert">
                        <strong>Hi
                            <asp:Label ID="sesuser" runat="server"></asp:Label>
                            <asp:Label ID="lblinfo" runat="server"></asp:Label>

                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">

                                <span aria-hidden="true">&times;</span>
                            </button>

                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="alertinfo" runat="server" Visible="false">
                <ContentTemplate>
                    <div class="alert alert-success   alert-dismissible fade show" role="alert">
                        <strong>Hi
                            <asp:Label ID="lbluser" runat="server"></asp:Label>
                            <asp:Label ID="lblmsg" runat="server"></asp:Label>

                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">

                                <span aria-hidden="true">&times;</span>
                            </button>

                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="container-xl wide-lg ">
                <div class="nk-content-inner">
                    <div class="nk-content-body ">
                        <div class="nk-block-head ">
                            <div class="nk-block-head-content">
                                <div class="nk-block-head-sub">

                                    <asp:LinkButton runat="server" CssClass="text-soft back-to " ID="btnback">
                                        <em class="icon ni ni-arrow-left" style="font-size:1rem"></em><span>Back</span>
                                    </asp:LinkButton>
                                </div>

                                <div class="nk-block-between-md g-4">
                                    <div class="nk-block-head" style="display:flex; flex-direction:row">
                                        <div class="nk-block-head-content">
                                            <div class="nk-block-des">
                                                <asp:Label runat="server" ID="lblcid"></asp:Label>

                                            </div>
                                            <h4 class="nk-block-title fw-normal">
                                                <asp:Label runat="server" ID="lblfname"></asp:Label>
                                            </h4>
                                            <div class="nk-block-des">
                                                <asp:Label runat="server" ID="lbllname"></asp:Label>
                                            </div>
                                            <div class="nk-block-des">
                                                <asp:Label runat="server" ID="lbladd"></asp:Label>
                                            </div>
                                            <div class="nk-block-des">
                                                <asp:Label runat="server" ID="lblmobile"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="nk-block-des">
                                            <asp:Image id="imgCapture" runat="server" Visible="false"
                                                style="margin-left:30px" Width="96px" Height="96px" />
                                        </div>
                                    </div>
                                    <div class="nk-block-head-content">
                                        <ul class="nk-block-tools gx-3">

                                            <li>
                                                <div>
                                                    <p class="text-primary ">
                                                        <asp:Label runat="server" ID="lblacn"></asp:Label>


                                                        <span id="lblba" runat="server"
                                                            class="badge badge-outline badge-success">
                                                            <asp:Label runat="server" ID="lblstus"></asp:Label>
                                                        </span>

                                                    </p>
                                                </div>

                                            </li>
                                            <li class="order-md-last">
                                                <asp:LinkButton runat="server" ID="btn_delete" class="btn btn-danger">
                                                    <em class="icon ni ni-cross"></em><span>Cancel this plan</span>
                                                </asp:LinkButton>
                                            </li>
                                            <li><a href="#" onclick="showTab();" class="btn btn-icon btn-light"><em
                                                        class="icon ni ni-reload"></em></a></li>
                                            <li>
                                                <div class="dropdown mb-2">
                                                    <button class="btn p-0" type="button" id="dropdownMenuButton"
                                                        data-toggle="dropdown" aria-haspopup="true"
                                                        aria-expanded="false">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                            viewBox="0 0 24 24" fill="none" stroke="currentColor"
                                                            stroke-width="2" stroke-linecap="round"
                                                            stroke-linejoin="round"
                                                            class="feather feather-more-vertical">
                                                            <circle cx="12" cy="12" r="1"></circle>
                                                            <circle cx="12" cy="5" r="1"></circle>
                                                            <circle cx="12" cy="19" r="1"></circle>
                                                        </svg>
                                                    </button>
                                                    <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">

                                                        <a class="dropdown-item d-flex align-items-center" href="#"
                                                            data-toggle="modal" data-target="#depmodal"
                                                            data-backdrop="static" data-keyboard="false"><svg
                                                                xmlns="http://www.w3.org/2000/svg" width="24"
                                                                height="24" viewBox="0 0 24 24" fill="none"
                                                                stroke="currentColor" stroke-width="2"
                                                                stroke-linecap="round" stroke-linejoin="round"
                                                                class="feather feather-edit-2 icon-sm mr-2">
                                                                <path
                                                                    d="M17 3a2.828 2.828 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5L17 3z">
                                                                </path>
                                                            </svg> <span class="">Edit</span> </a>
                                                        <a class="dropdown-item d-flex align-items-center" href="#"
                                                            data-toggle="modal" data-target="#delmodal"
                                                            data-backdrop="static" data-keyboard="false"><svg
                                                                xmlns="http://www.w3.org/2000/svg" width="24"
                                                                height="24" viewBox="0 0 24 24" fill="none"
                                                                stroke="currentColor" stroke-width="2"
                                                                stroke-linecap="round" stroke-linejoin="round"
                                                                class="feather feather-trash icon-sm mr-2">
                                                                <polyline points="3 6 5 6 21 6"></polyline>
                                                                <path
                                                                    d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2">
                                                                </path>
                                                            </svg> <span class="">Delete</span></a>
                                                        <a class="dropdown-item d-flex align-items-center" href="#"
                                                            data-toggle="modal" data-target="#fltmodal"
                                                            data-backdrop="static" data-keyboard="false"><svg
                                                                xmlns="http://www.w3.org/2000/svg" width="24"
                                                                height="24" viewBox="0 0 24 24" fill="none"
                                                                stroke="currentColor" stroke-width="2"
                                                                stroke-linecap="round" stroke-linejoin="round"
                                                                class="feather feather-filter  icon-sm mr-2">
                                                                <polygon
                                                                    points="22 3 2 3 10 12.46 10 19 14 21 14 12.46 22 3">
                                                                </polygon>
                                                            </svg> <span class="">Change Period</span></a>

                                                        <a class="dropdown-item d-flex align-items-center" href="#"
                                                            data-toggle="modal" data-target="#knlmodal"
                                                            data-keyboard="true"><svg xmlns="http://www.w3.org/2000/svg"
                                                                width="24" height="24" viewBox="0 0 24 24" fill="none"
                                                                stroke="currentColor" stroke-width="2"
                                                                stroke-linecap="round" stroke-linejoin="round"
                                                                class="feather feather-filter  icon-sm mr-2">
                                                                <polygon
                                                                    points="22 3 2 3 10 12.46 10 19 14 21 14 12.46 22 3">
                                                                </polygon>
                                                            </svg> <span class="">Show</span></a>
                                                        <a class="dropdown-item d-flex align-items-center" href="#"
                                                            data-toggle="modal" data-target="#Modalviewprint"
                                                            data-backdrop="static" data-keyboard="false"><svg
                                                                xmlns="http://www.w3.org/2000/svg" width="24"
                                                                height="24" viewBox="0 0 24 24" fill="none"
                                                                stroke="currentColor" stroke-width="2"
                                                                stroke-linecap="round" stroke-linejoin="round"
                                                                class="feather feather-printer icon-sm mr-2">
                                                                <polyline points="6 9 6 2 18 2 18 9"></polyline>
                                                                <path
                                                                    d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2">
                                                                </path>
                                                                <rect x="6" y="14" width="12" height="8"></rect>
                                                            </svg> <span class="">Print Bond</span></a>


                                                        <a class="dropdown-item d-flex align-items-center"
                                                            href="../soaprint.aspx" target="_blank"><svg
                                                                xmlns="http://www.w3.org/2000/svg" width="24"
                                                                height="24" viewBox="0 0 24 24" fill="none"
                                                                stroke="currentColor" stroke-width="2"
                                                                stroke-linecap="round" stroke-linejoin="round"
                                                                class="feather feather-printer icon-sm mr-2">
                                                                <polyline points="6 9 6 2 18 2 18 9"></polyline>
                                                                <path
                                                                    d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2">
                                                                </path>
                                                                <rect x="6" y="14" width="12" height="8"></rect>
                                                            </svg> <span class="">Print Statement</span></a>
                                                        <a class="dropdown-item d-flex align-items-center" href="#"><svg
                                                                xmlns="http://www.w3.org/2000/svg" width="24"
                                                                height="24" viewBox="0 0 24 24" fill="none"
                                                                stroke="currentColor" stroke-width="2"
                                                                stroke-linecap="round" stroke-linejoin="round"
                                                                class="feather feather-download icon-sm mr-2">
                                                                <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4">
                                                                </path>
                                                                <polyline points="7 10 12 15 17 10"></polyline>
                                                                <line x1="12" y1="15" x2="12" y2="3"></line>
                                                            </svg> <span class="">Download</span></a>

                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="nk-block nk-block-lg ">
                            <div class="card card-bordered">
                                <div class="card-inner">
                                    <div class="row gy-gs">
                                        <div class="col-md-6">
                                            <div class="nk-iv-wg3">
                                                <div class="nk-iv-wg3-group flex-lg-nowrap gx-4">
                                                    <div class="nk-iv-wg3-sub">
                                                        <div class="nk-iv-wg3-amount">
                                                            <div class="number">
                                                                <asp:Label runat="server" ID="lblamt"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="nk-iv-wg3-subtitle">Invested Amount</div>
                                                    </div>
                                                    <div class="nk-iv-wg3-sub"><span
                                                            class="nk-iv-wg3-plus text-soft"></span>
                                                        <div class="nk-iv-wg3-amount">
                                                            <div class="number">
                                                                <asp:Label runat="server" ID="lblmamt"></asp:Label>
                                                                <span class="number-up">
                                                                    <asp:Label runat="server" ID="lblroi"></asp:Label> %
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <div class="nk-iv-wg3-subtitle">Maturity Amount</div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6 col-lg-4 offset-lg-2">


                                            <div class="nk-iv-wg3 pl-md-3">
                                                <div class="nk-iv-wg3-group flex-lg-nowrap gx-4">
                                                    <div class="nk-iv-wg3-sub">
                                                        <div class="nk-iv-wg3-amount">
                                                            <div class="number">
                                                                <asp:Label runat="server" ID="lblbal"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="nk-iv-wg3-subtitle">Balance</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- start-->



                                        <!-- end-->

                                    </div>
                                </div>
                                <div id="schemeDetails" class="nk-iv-scheme-details">
                                    <ul class="nk-iv-wg3-list">
                                        <li>
                                            <div class="sub-text">Product</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblprod"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Account No</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblacno"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Renewal From</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblrenewal"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Rate of Interest</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblcroi"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Nominee</div>
                                            <div class="lead-text  ">
                                                <asp:Label runat="server" ID="lblnomi"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Maturity Amount(C)</div>
                                            <div class="lead-text  ">
                                                <asp:Label runat="server" ForeColor="Red" ID="lblmatc"></asp:Label>
                                            </div>
                                        </li>


                                    </ul>
                                    <ul class="nk-iv-wg3-list">
                                        <li>
                                            <div class="sub-text">Date</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lbldat"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">W.E.F</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblwef"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Term</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblprd"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Maturity Date</div>
                                            <div class="lead-text"><span class="currency currency-usd"></span>
                                                <asp:Label runat="server" ID="lblmdat"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Relationship</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblrel"></asp:Label>
                                            </div>
                                        </li>
                                    </ul>
                                    <ul class="nk-iv-wg3-list">
                                        <li>
                                            <div class="sub-text">Deposit Loan</div>
                                            <div class="lead-text  ">
                                                <asp:Label runat="server" CssClass="text-danger" ID="lbldl"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Standing Instruction</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblsi"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Pass Book</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblpb"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Issued On</div>
                                            <div class="lead-text">
                                                <asp:Label runat="server" ID="lblpbon"></asp:Label>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="sub-text">Address</div>
                                            <div class="lead-text text-right">
                                                <asp:Label runat="server" ID="lblnadd"></asp:Label>
                                            </div>
                                        </li>

                                    </ul>

                                </div>
                            </div>
                        </div>

                        <div class="nk-block nk-block-lg ">
                            <div class="nk-block-head ">
                                <h5 class="nk-block-title">Transactions</h5>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lblstprd" CssClass="col-form-label text-primat">
                                        </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="card card-bordered ">
                                <asp:UpdatePanel ID="gv" runat="server">

                                    <ContentTemplate>






                                        <asp:Repeater runat="server" id="rptab">
                                            <HeaderTemplate>
                                                <table class="table table-borderless" style="width:100%">
                                                    <tbody>
                                                        <tr style="text-align:center">

                                                            <td style="text-align:center;width:28%"></td>
                                                            <td style="text-align:center;width:36%""><p class="
                                                                text-primary text-center ">Transaction</p></td>
                                                            <td  style=" text-align:center;width:36%""
                                                                class="sensitive ">
                                                                <p class="text-primary text-center ">D-int</p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table class="table table-bordered" style="width:100%">
                                                    <thead>

                                                        <tr>
                                                            <th style="width:8%;text-align:center">Date</th>
                                                            <th style="width:20%;text-align:center">Narration</th>
                                                            <th style="width:12%;text-align:center">Debit</th>
                                                            <th style="width:12%;text-align:center">Credit</th>
                                                            <th style="width:12%;text-align:center">Balance</th>
                                                            <th style="width:12%" class="sensitive">Debit</th>
                                                            <th style="width:12%" class="sensitive">Credit</th>
                                                            <th style="width:12%" class="sensitive">Balance</th>
                                                        </tr>
                                                    </thead>

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbldat" runat="server"
                                                            text='<%#Eval("date", "{0:MM-dd-yyyy}")%>'
                                                            Style="text-align:center;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblnar" runat="server"
                                                            text='<%#Eval("Narration")%>'
                                                            Style="text-align:left;float:left "></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lbldue" CssClass="sub-text " runat="server"
                                                            text='<%#Eval("due")%>' style="padding-top:10px">
                                                        </asp:Label>
                                                        <asp:Label ID="lbltid" runat="server" Text='<%#Eval("tid")%>'
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldr" runat="server"
                                                            text='<%#Eval("drd", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblcr" runat="server"
                                                            text='<%#Eval("crd", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblbal" runat="server"
                                                            text='<%#Eval("dbal", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>
                                                    </td>
                                                    <td class="sensitive" style="background-color:beige">
                                                        <asp:Label ID="lblcdr" runat="server"
                                                            text='<%#Eval("drc", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>
                                                    </td>
                                                    <td class="sensitive" style="background-color:beige">
                                                        <asp:Label ID="lblccr" runat="server"
                                                            text='<%#Eval("crc", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>
                                                    </td>
                                                    <td class="sensitive" style="background-color:beige">
                                                        <asp:Label ID="lblcbal" runat="server"
                                                            text='<%#Eval("cbal", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>


                                        <asp:GridView runat="server" ID="disp" AutoGenerateColumns="false" Width="100%"
                                            AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="60"
                                            OnRowDataBound="gv_dep_RowDataBound"
                                            OnSelectedIndexChanged="OnSelectedIndexChanged_gv_dep"
                                            AutoGenerateSelectButton="false" Visible="false" ShowHeaderWhenEmpty="false"
                                            ShowHeader="false" CssClass="table table-bordered">
                                            <Columns>


                                                <asp:TemplateField>
                                                    <ItemTemplate>

                                                        <asp:Label ID="lbldat" runat="server"
                                                            text='<%#Eval("date", "{0:MM-dd-yyyy}")%>'
                                                            Style="text-align:center;"></asp:Label>

                                                    </ItemTemplate>
                                                    <ItemStyle Width="4%" />

                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblnar" runat="server"
                                                            text='<%#Eval("Narration")%>'
                                                            Style="text-align:left;float:left "></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lbldue" runat="server" text='<%#Eval("due")%>'
                                                            Style="text-align:left;float:left "></asp:Label>
                                                        <asp:Label ID="lbltid" runat="server" Text='<%#Eval("tid")%>'
                                                            Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="24%" />

                                                </asp:TemplateField>


                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldr" runat="server"
                                                            text='<%#Eval("drd", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>

                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcr" runat="server"
                                                            text='<%#Eval("crd", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>

                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbal" runat="server"
                                                            text='<%#Eval("dbal", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>

                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcdr" runat="server"
                                                            text='<%#Eval("drc", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>

                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="sensitive" Width="12%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblccr" runat="server"
                                                            text='<%#Eval("crc", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>

                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="sensitive" Width="12%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcbal" runat="server"
                                                            text='<%#Eval("cbal", "{0:#,##,###.00}")%>'
                                                            Style="text-align:right ;float:right "></asp:Label>

                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="sensitive" Width="12%" />
                                                </asp:TemplateField>


                                            </Columns>

                                        </asp:GridView>


                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="form-group row "></div>
                                <div class="form-group row "></div>
                                <div class="form-group row "></div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal " tabindex="-1" role="dialog" id="knlmodal">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-body">

                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:Repeater ID="rpknl" runat="server">
                                        <HeaderTemplate>


                                            <table class="table table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th>Account No</th>
                                                        <th>Member No</th>
                                                        <th>Customer Name</th>
                                                        <th>Amount</th>
                                                    </tr>
                                                </thead>

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblacn" text='<%#Eval("acno")%>'>
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblmem" text='<%#Eval("cid")%>'>
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblname"
                                                        text='<%#Eval("firstname")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblamt"
                                                        text='<%#Eval("amount", "{0:#,##,###.00}")%>'></asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>

                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" tabindex="-1" role="dialog" id="delmodal">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-body ">
                            <p>Are you sure to Delete this Account ?</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button runat="server" id="btn_del" Text="Delete" class="btn btn-danger " />
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal " tabindex="-1" role="dialog" id="fltmodal">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                Enter the Period
                            </h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:UPdatePanel runat="server">
                                <ContentTemplate>

                                    <div class="form-group row">
                                        <label class="col-form-label col-sm-2">From</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtfrm" runat="server" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-form-label col-sm-2">To</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtto" runat="server" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UPdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button runat="server" id="btnfltr" Text="Apply" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" tabindex="-1" role="dialog" id="depmodal">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Label ID="lblEacn" Text="" runat="server"></asp:Label>
                            </h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <asp:UPdatePanel runat="server">
                                <ContentTemplate>

                                    <div class="container-fluid">
                                        <div class="form-group row">
                                            <label class="col-form-label   col-sm-3">Customer Id</label>
                                            <div class="col-md-6">
                                                <asp:TextBox runat="server" ID="txtcid" AutoPostBack="true"
                                                    CssClass="form-control "></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row ">

                                            <label class="col-form-label col-sm-3 ">Customer Name</label>
                                            <div class="col-sm-5">
                                                <asp:Label Text="" runat="server" ID="lblEname"></asp:Label>
                                                <br />
                                                <asp:Label Text="" runat="server" ID="lblElname"></asp:Label>
                                            </div>

                                        </div>
                                        <div class="form-group row ">
                                            <label class="col-form-label col-sm-3">Address</label>
                                            <div class="col-sm-4">
                                                <asp:Label Text="" runat="server" ID="lblEadd"></asp:Label>
                                            </div>

                                        </div>
                                        <div class="form-group row ">
                                            <label class="col-form-label col-sm-3 ">Date</label>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" ID="txtdate" CssClass="form-control ">
                                                </asp:TextBox>
                                            </div>
                                            <label class="col-form-label col-sm-1">W.E.F</label>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" ID="txtwef" CssClass="form-control "
                                                    AutoPostBack="true"></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="form-group row ">
                                            <label class="col-form-label col-sm-3 ">Maturity </label>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" ID="txtmdate" Enabled="false"
                                                    CssClass="form-control "></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row ">
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-3">
                                                <div class="form-check form-check-inline">
                                                    <label class="form-check-label">
                                                        <asp:CheckBox runat="server" ID="ispb" />
                                                        Pass Book
                                                        <i class="input-frame"></i>
                                                    </label>
                                                </div>
                                            </div>

                                            <label class="col-form-label col-sm-2 ">Issued On</label>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" ID="txtpbon" CssClass="form-control ">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-5">
                                                <div class="form-check form-check-inline">
                                                    <label class="form-check-label">
                                                        <asp:CheckBox runat="server" ID="fc" />
                                                        Field Collection
                                                        <i class="input-frame"></i>
                                                    </label>
                                                </div>
                                            </div>

                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddagent" runat="server">
                                                    <asp:ListItem><--Select--></asp:ListItem>
                                                    <asp:ListItem>Sankar</asp:ListItem>
                                                    <asp:ListItem>Viswanath</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UPdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button runat="server" id="btn_save" Text="Save changes" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>



            <div class="modal" tabindex="-1" role="dialog" id="Modalviewprint" data-backdrop="static"
                data-keyboard="false">
                <div class="modal-dialog modal-xl" role="document">
                    <div class="modal-content">
                        <div class="modal-body">

                            <asp:UpdatePanel runat="server" Visible="true">
                                <ContentTemplate>

                                    <div class="prntarea" id="vouchprint" style="margin-left:65px">



                                        <table style="border:0px solid #000; margin-top:-1.8cm; margin-left:1cm;">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <table style="width:340px;">
                                                            <tr>
                                                                <td style="padding-left:0.0005cm; padding-top:2.15cm; position:relative; left:-1cm;">
                                                                    W.E.F:
                                                                </td>
                                                                <td style="padding-top:2.15cm; position:relative;">
                                                                    <asp:Label ID="lblWefLeft" runat="server"
                                                                        style="position:relative; left:-3.9cm;">
                                                                    </asp:Label>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr style="height:19px">
                                                                <td style="padding-left:2cm;"><asp:Label ID="lblDateLeft" runat="server"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left:2cm;"><asp:Label ID="lblAccountNoLeft" runat="server"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left:2cm; padding-top:0.8cm;"><asp:Label ID="lblMaturityValueLeft" runat="server"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left:2cm; padding-top:0.3cm;"><asp:Label ID="lblMaturityDateLeft" runat="server"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left:2cm; padding-top:0.3cm;"><asp:Label ID="lblPeriodLeft" runat="server"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left:2cm; padding-top:0.1cm;"><asp:Label ID="lblInterestRateLeft" runat="server"></asp:Label></td>
                                                            </tr>

                                                            
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table style="width:780px;height:65px">
                                                            <tbody>
                                                                <tr style="height:115px">
                                                                    <td style="width:100%;padding-left:10px">
                                                                        &nbsp&nbsp
                                                                    </td>
                                                                </tr>


                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr
                                                    style="height:20px;border-bottom:1px solid #000;border-top:1px solid #000">

                                                    <td style="width:100%;text-align:center;margin-top:10px;">



                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td style="position:relative; left:-1cm;">
                                                        <table style="height:150px;width:90%; margin-top: -3cm;">
                                                            <tbody>
                                                                <tr>
                                                                    <td style="width:20%"><span
                                                                            class="label-cap">Branch</span></td>
                                                                    <td style="padding-left:10px">
                                                                        <asp:Label ID="lblhome" runat="server"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>

                                                                    <td style="text-align:right;width:20%"><span
                                                                            class="label-cap">Date :&nbsp;&nbsp;</span>
                                                                        <asp:Label runat="server" ID="prntdate"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align:start"><span
                                                                            class="label-cap">Received From</span></td>
                                                                    <td style="padding-left:10px">
                                                                        <asp:Label runat="server" ID="prntcustomer"
                                                                            Font-Bold="true"></asp:Label>


                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align:start"></td>
                                                                    <td style="padding-left:1px; position:relative; left:0.2cm;">
                                                                        <asp:Label runat="server" ID="prntfirstname"
                                                                            Font-Size="Small"></asp:Label>
                                                                        <br />
                                                                        <asp:Label runat="server" ID="prntadd"
                                                                            Font-Size="X-Small"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td><span class="label-cap">DEPOSIT </span></td>
                                                                    <td style="padding-left:5px; position:relative; left:0.1cm;">
                                                                        <asp:Label ID="prntprod" runat="server"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>

                                                                <tr style="height:20px">
                                                                    <td>
                                                                        <p class="label-cap">the Sum of</p>
                                                                    </td>
                                                                    <td style="padding-left:15px;" colspan="2">
                                                                        <asp:Label runat="server" ID="prntamt"
                                                                            Font-Bold="true" Font-Size="Medium">
                                                                        </asp:Label>
                                                                        <asp:Label runat="server" ID="prntaiw"
                                                                            Font-Size="Small"></asp:Label>
                                                                    </td>
                                                                    <td style="padding-left:10px"></td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="border:none">
                                                    <td style="vertical-align:top">
                                                        <table style="width:340px;">
                                                            <!-- <tr style="height:1cm"><td></td></tr> -->
                                                            <tr id="trMaturityValueLeftBottom" runat="server" visible="false" style="height:39px">
                                                                <td style="padding-left:0.1cm; position:relative; top:-1.5cm;"><span style="font-weight:bold">Mat Amt:</span><asp:Label ID="lblMaturityValueLeftBottom" runat="server" style="display:inline-block; max-width: 20ch; word-wrap: break-word; white-space: normal;"></asp:Label></td>
                                                            </tr>
                                                            <tr style="height:39px">
                                                                <td style="padding-left:0.1cm;"><asp:Label ID="lblNameLeft" runat="server" style="display:inline-block; max-width: 20ch; word-wrap: break-word; white-space: normal;"></asp:Label></td>
                                                            </tr>
                                                            <tr style="height:39px">
                                                                <td style="padding-left:0.1cm; position:relative; top:0.5cm;"><asp:Label ID="lblAddressLeft" runat="server" style="display:inline-block; max-width: 20ch; word-wrap: break-word; white-space: normal;"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="border:none; position:relative; left:-1cm;">
                                                        <table
                                                            style="width:90%;border:1px solid #000;border-collapse:collapse;height:74px;table-layout:fixed;margin-left:20px;margin-right:20px;">
                                                            <tbody>
                                                                <tr style="border:1px solid #000">
                                                                    <td
                                                                        style="width:30%;text-align:center;border:1px solid #000">
                                                                        <p class="label-cap"> Account No</p>
                                                                    </td>
                                                                    <td style="text-align:center;border:1px solid #000">
                                                                        <p class="label-cap">Effective Date</p>
                                                                    </td>
                                                                    <td style="text-align:center;border:1px solid #000">
                                                                        <p class="label-cap">Period</p>
                                                                    </td>
                                                                    <td style="text-align:center;border:1px solid #000">
                                                                        <p class="label-cap">Interest Rate</p>
                                                                    </td>
                                                                    <td style="text-align:center;border:1px solid #000">
                                                                        <p class="label-cap">Maturity Date</p>
                                                                    </td>

                                                                </tr>
                                                                <tr style="height:50px">
                                                                    <td
                                                                        style="width:30%;text-align:center;border:1px solid #000">
                                                                        <asp:Label runat="server" ID="prntaccno"
                                                                            Font-Bold="true"></asp:Label>
                                                                        <br />

                                                                    </td>
                                                                    <td style="text-align:center;border:1px solid #000">

                                                                        <asp:Label runat="server" ID="prntwef"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align:center;border:1px solid #000">
                                                                        <asp:Label runat="server" ID="prntprd"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align:center;border:1px solid #000">
                                                                        <asp:Label runat="server" ID="prntroi"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align:center;border:1px solid #000">
                                                                        <asp:Label runat="server" ID="prntmdt"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>

                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height:50px;">
                                                    <td></td>
                                                    <td style="position:relative; left:-1cm;">
                                                        <asp:Label ID="prntint" runat="server" Font-Size="Small">
                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td colspan="2" style="position:relative; left:-1cm;">
                                                        <table style="height:50px;table-layout:fixed">
                                                            <tbody>

                                                <tr style="height:10px">
                                                    <td>
                                                        <p class="label-cap"> Maturity Value</p>
                                                    </td>
                                                    <td style="padding-left:15px">
                                                        <asp:Label runat="server" ID="prntmamt" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                        <asp:Label runat="server" ID="prntmaiw" Font-Bold="false"
                                                            Font-Size="Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height:5px">
                                                    <td></td>
                                                    <td colspan="1" style="padding-left:10px">

                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:right;padding-bottom:10px;padding-right:10px"></td>
                                        </tr>
                                        </tbody>
                                        </table>



                                    </div>




                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="form-group row ">

                            <div class="col-sm-3"></div>
                            <div class="col-sm-3">
                                <button id="btn" type="button" class=" btn btn-primary "
                                    onclick="printdep()">PRINT</button>
                                <button id="btncan" type="button" class=" btn btn-secondary "
                                    data-dismiss="modal">Cancel</button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </form>



        <script src="js/jquery.js" type="text/javascript"></script>
        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />
        <script src="js/daterangepicker.js" type="text/javascript"></script>
        <script src="js/printThis.js" type="text/javascript"></script>


        <script type="text/javascript">
            $(document).ready(function () {


                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);

                // Place here the first init of the autocomplete
                InitAutoCompl();
                dpick();

            });

            function InitializeRequest(sender, args) {



            }

            function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete
                InitAutoCompl();
            }


            function dpick() {

                $("#<%=txtfrm.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    showDropdowns: false,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtfrm.ClientID%>").val(start.format('DD-MM-YYYY'));
                });

                $("#<%=txtto.ClientID%>").daterangepicker({

                    "singleDatePicker": true,
                    "autoUpdateInput": true,
                    showDropdowns: false,
                    "autoApply": true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                }, function (start, end, label) {

                    $("#<%=txtto.ClientID%>").val(start.format('DD-MM-YYYY'));
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

                        $("#<%=txtcid.ClientID %>").val(ui.item.memberno);
                        return false;

                    },
                    select: function (event, ui) {

                        $("#<%=txtcid.ClientID %>").val(ui.item.memberno);
                        return false;
                    },

                    open: function () {
                        $("ul.ui-menu").width($(this).innerWidth() + 130);

                    },
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

        </script>

        <style>
            .ui-autocomplete {
                max-height: 450px;
                overflow-y: auto;
                z-index: 1500;

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



        <style>
            .sensitive {
                display: none;
            }
        </style>

        <script type="text/javascript">

            function showTab() {
                $(".sensitive").toggle();
            }

            function printdep() {
                const style = document.createElement('style');
                style.textContent = `
        @media print {
            #vouchprint {
                transform: rotate(-90deg) scale(1.3);
                transform-origin: center;
                position: absolute;
                top: 25%;
                left: 25%;
            }
        }
    `;
                document.head.appendChild(style);

                $('#vouchprint').printThis({
                    importCSS: false,
                    importStyle: true,
                    printContainer: true,
                    afterPrint: function () {
                        $('#Modalviewprint').modal('hide');
                        document.head.removeChild(style);
                    }





                });
            }

            function showdet() {
                $('#Modalviewprint').toggle();

            }



        </script>
    </asp:Content>