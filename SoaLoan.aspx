<%@ Page Title="Loan - Statements" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="SoaLoan.aspx.vb" Inherits="Fiscus.SoaLoan" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link rel="stylesheet" href="css/Customer.css" />
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form runat="server">
            <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

            <nav class="page-breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="#">Loans</a></li>

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

                                            <%-- Jewel images (from jlspec) shown after member photo --%>
                                            <asp:PlaceHolder ID="phJewelImages" runat="server" />
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
                                            <li><a href="#" runat="server" id="shwtab" onclick="showTab();"
                                                    class="btn btn-icon btn-light"><em
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
                                                        <a class="dropdown-item d-flex align-items-center"
                                                            href="../soaprint.aspx?ptype=c" target="_blank"><svg
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
                                                            </svg> <span class="">Print Statement
                                                                (C-Interest)</span></a>
                                                        <a class="dropdown-item d-flex align-items-center"
                                                            href="../soaprint.aspx?ptype=d" target="_blank"><svg
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
                                                            </svg> <span class="">Print Statement
                                                                (D-Interest)</span></a>
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
                                                        <div class="nk-iv-wg3-subtitle">Loan Amount</div>
                                                    </div>
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>

                                                            <asp:Panel runat="server" ID="pnldint" Visible="true">
                                                                <div class="nk-iv-wg3-sub"><span
                                                                        class="nk-iv-wg3-plus text-soft"></span>
                                                                    <div class="nk-iv-wg3-amount">
                                                                        <div class="number">
                                                                            <asp:Label runat="server" ID="lblmamt">
                                                                            </asp:Label> <span class="number-up">
                                                                                <asp:Label runat="server" ID="lblroi">
                                                                                </asp:Label> %
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--<div class="nk-iv-wg3-subtitle">Maturity Amount
                                                                </div>--%>
                                                </div>
                                                </asp:Panel>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6 col-lg-4 offset-lg-2">


                                        <div class="nk-iv-wg3 pl-md-3">
                                            <div class="nk-iv-wg3-group flex-lg-nowrap gx-4">
                                                <div class="nk-iv-wg3-sub">
                                                    <div class="nk-iv-wg3-amount">
                                                        <div class="number">
                                                            <asp:Label runat="server" CssClass="text-danger-muted "
                                                                ID="lblbal"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="nk-iv-wg3-subtitle">Balance</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
                                        <div class="sub-text">Scheme</div>
                                        <div class="lead-text">
                                            <asp:Label runat="server" ID="lblsch"></asp:Label>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="sub-text">Rate of Interest</div>
                                        <div class="lead-text">
                                            <asp:Label runat="server" ID="lblcroi"></asp:Label>
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
                                        <div class="sub-text">Last Receipt</div>
                                        <div class="lead-text">
                                            <asp:Label runat="server" ID="lbllastrcpt"></asp:Label>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="sub-text">Period</div>
                                        <div class="lead-text">
                                            <asp:Label runat="server" ID="lblprd"></asp:Label>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="sub-text">Rate Per Gram</div>
                                        <div class="lead-text"><span class="currency currency-usd"></span>
                                            <asp:Label runat="server" ID="lblrpg"></asp:Label>
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
                                        <div class="sub-text">SMS Alert</div>
                                        <div class="lead-text">
                                            <asp:Label runat="server" ID="lblalert"></asp:Label>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="sub-text">Field Collection</div>
                                        <div class="lead-text">
                                            <asp:Label runat="server" ID="lblfc"></asp:Label>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="sub-text">Collection By</div>
                                        <div class="lead-text">
                                            <asp:Label runat="server" ID="lblagent"></asp:Label>
                                        </div>
                                    </li>
                                </ul>

                            </div>
                        </div>
                    </div>
                    <div>
                        <asp:UpdatePanel ID="jltab" runat="server" Visible="false">
                            <ContentTemplate>


                                <div class="nk-block nk-block-lg">
                                    <div class="nk-block-head " style="padding-top:20px">
                                        <h4 class="nk-block-title">Jewel Specifics</h4>
                                    </div>

                                    <div class="card card-bordered    ">
                                        <div class="card-inner">

                                            <%-- jltab --%>
                                                <table style="margin-left:0px;width:100%"
                                                    class="table table-bordered   ">
                                                    <thead>
                                                        <tr style="text-align:center;">
                                                            <th style="text-align:center;width:35%;">Particulars</th>
                                                            <th style="text-align:center;width:15%;">Quantity</th>
                                                            <th style="text-align:center;width:25%;">Gross Weight</th>
                                                            <th style="text-align:center;width:25%;">Net Weight</th>

                                                        </tr>
                                                    </thead>
                                                </table>

                                                <asp:GridView runat="server" ID="dispjl" AutoGenerateColumns="false"
                                                    style="margin-top:-2px;margin-left:0px;" AllowPaging="true"
                                                    OnPageIndexChanging="OnPaging" Width="100%" Visible="true"
                                                    ShowHeaderWhenEmpty="false" ShowHeader="false"
                                                    CssClass="table table-bordered   ">
                                                    <Columns>


                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitm" runat="server"
                                                                    text='<%#Eval("itm")%>' Style="text-align:left  ">
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="35%" />
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
                                                            <ItemStyle Width="25%" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>


                                                <table style="margin-left:0px;width:100%"
                                                    class="table table-bordered   ">
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

                                                <%-- eojl tab --%>


                                        </div>
                                    </div>
                                </div>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div>

                        <asp:UpdatePanel runat="server" ID="dltab" Visible="false">
                            <ContentTemplate>


                                <div class="nk-block nk-block-lg ">
                                    <div class="nk-block-head " style="padding-top:20px">
                                        <h4 class="nk-block-title">Deposit Specifics</h4>
                                    </div>
                                    <div class="card card-bordered ">
                                        <div class="card-inner ">
                                            <%-- dltab --%>
                                                <table style="margin-left:30px;width:80%"
                                                    class="table table-bordered   ">
                                                    <thead>
                                                        <tr style="text-align:center;">
                                                            <th style="text-align:center;width:35%;">Account No</th>
                                                            <th style="text-align:center;width:17%;">Deposit Date</th>
                                                            <th style="text-align:center;width:24%;">Deposit Amount</th>
                                                            <th style="text-align:center;width:24%;">Account Balance
                                                            </th>
                                                            <th></th>

                                                        </tr>
                                                    </thead>
                                                </table>

                                                <asp:GridView runat="server" ID="dispdl" AutoGenerateColumns="false"
                                                    style="margin-top:-2px;margin-left:30px;float:left;z-index:1"
                                                    AllowPaging="true" OnPageIndexChanging="OnPaging" Width="80%"
                                                    Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"
                                                    CssClass="table table-bordered   ">
                                                    <Columns>


                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitm" runat="server"
                                                                    text='<%#Eval("acn")%>' Style="text-align:left  ">
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="35%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblqty" runat="server"
                                                                    text='<%#Eval("acdate")%>'
                                                                    Style="text-align:left ;float:right"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="17%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblgross" runat="server"
                                                                    text='<%#Eval("depamt")%>'
                                                                    Style="text-align:right;float:right"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="24%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblnet" runat="server"
                                                                    text='<%#Eval("curbal")%>'
                                                                    Style="text-align:right;float:right   "></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="24%" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                                <table style="margin-left:30px;width:80%" class="customers ">
                                                    <tbody>
                                                        <tr style="text-align:center;">
                                                            <td style="text-align:center;width:35%">
                                                                <p>Total</p>

                                                            </td>
                                                            <td style="text-align:center;width:17% ">

                                                            </td>
                                                            <td style="text-align:right ;width:24% ">

                                                            </td>
                                                            <td style="text-align:center;width:24%">
                                                                <asp:Label runat="server" ID="Label1"
                                                                    style="float:right;text-align:right " Text="0">
                                                                </asp:Label>
                                                            </td>


                                                        </tr>
                                                    </tbody>
                                                </table>

                                                <%-- dltab end --%>
                                        </div>

                                    </div>
                                </div>


                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>

                    <div class="nk-block nk-block-lg ">
                        <div class="nk-block-head " style="padding-top:20px">
                            <h4 class="nk-block-title">Transactions</h4>
                        </div>
                        <div class="card card-bordered ">
                            <asp:UpdatePanel ID="gv" runat="server">

                                <ContentTemplate>




                                    <asp:Repeater runat="server" id="rptab">
                                        <HeaderTemplate>
                                            <table class="table table-bordered" style="width:100%">
                                                <thead>
                                                    <tr>
                                                        <th></th>
                                                        <th></th>

                                                        <th colspan="3" style="text-align:center;">Transaction</th>
                                                        <th colspan="3" style="text-align:center;" class="sensitive ">
                                                            D-int</th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width:9%;text-align:center">Date</th>
                                                        <th style="width:24%;text-align:center">Narration</th>
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
                                                    <asp:Label ID="lblnar" runat="server" text='<%#Eval("Narration")%>'
                                                        Style="text-align:left;float:left "></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lbldue" runat="server" CssClass="sub-text "
                                                        text='<%#Eval("due")%>' Style="padding-top:10px;"></asp:Label>
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
                                        ShowHeader="false" CssClass="table table-bordered  ">
                                        <Columns>


                                            <asp:TemplateField>
                                                <ItemTemplate>

                                                    <asp:Label ID="lbldat" runat="server"
                                                        text='<%#Eval("date", "{0:MM-dd-yyyy}")%>'
                                                        Style="text-align:center;float:right "></asp:Label>

                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />

                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnar" runat="server" text='<%#Eval("Narration")%>'
                                                        Style="text-align:left;float:left "></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lbldue" runat="server" text='<%#Eval("due")%>'
                                                        Style="text-align:left;float:left "></asp:Label>
                                                    <asp:Label ID="lbltid" runat="server" Text='<%#Eval("tid")%>'
                                                        Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="23%" />

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


            <div class="modal" tabindex="-1" role="dialog" id="depmodal">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Label ID="lblEacn" CssClass="text-primary " Text="" runat="server"></asp:Label>
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
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-3">
                                                <div class="form-check form-check-inline">
                                                    <label class="form-check-label">
                                                        <asp:CheckBox runat="server" ID="ispb" />
                                                        SMS Alert
                                                        <i class="input-frame"></i>
                                                    </label>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="form-group row ">
                                            <div class="col-sm-3"></div>
                                            <div class="col-sm-4">
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
        </form>



        <script src="js/jquery.js" type="text/javascript"></script>
        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />



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
            function showTab() {
                $(".sensitive").toggle();
            }
        </script>

    </asp:Content>