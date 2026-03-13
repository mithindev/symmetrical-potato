<%@ Page Title="Deposit Payment" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master"
    CodeBehind="DepositPayment.aspx.vb" Inherits="Fiscus.DepositPayment" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link rel="stylesheet" href="css/daterangepicker.css" />
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
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <form id="newpayment" runat="server">

            <asp:ScriptManager ID="SM3" runat="server"></asp:ScriptManager>

            <nav class="page-breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="#">Deposit</a></li>
                    <li class="breadcrumb-item active">Payment</li>
                </ol>
            </nav>






            <div class="row">
                <div class="col-md-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">

                            <asp:UpdatePanel runat="server" ID="tabUP">
                                <ContentTemplate>

                                    <asp:Panel runat="server" ID="pnltrans" style="display:block">

                                        <div class="form-group row">
                                            <label class="col-sm-2 col-form-label">Account No</label>
                                            <div class="col-sm-4">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtacn" runat="server" CssClass="form-control"
                                                        style="font-weight:bold;" AutoPostBack="true"></asp:TextBox>
                                                    <div class="input-group-append">
                                                        <asp:LinkButton runat="server" ID="soa" Enabled="false">
                                                            <span class="input-group-text">📄</span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <label class="col-sm-2 col-form-label text-right">Date</label>
                                            <div class="col-sm-3">
                                                <asp:TextBox runat="server" ID="tdate" AutoPostBack="True"
                                                    CssClass="form-control" />
                                            </div>
                                        </div>

                                        <asp:Panel runat="server" ID="pnlact" Visible="false">

                                            <!-- CUSTOMER CARD (New Design) -->
                                            <div
                                                style="background:#fff; padding:16px; border-radius:12px; box-shadow:0 3px 12px rgba(0,0,0,0.05); margin-bottom:18px; display:flex; justify-content:space-between; align-items:center; border:1px solid #e2e8f0;">
                                                <div>
                                                    <div style="font-size:16px; font-weight:600; color:#1e293b;">
                                                        <asp:Label runat="server" ID="lblname" />
                                                    </div>
                                                    <div style="font-size:12px; color:#64748b; margin-top:4px;">
                                                        <asp:Label runat="server" ID="lbladd" />
                                                    </div>
                                                    <div style="font-size:12px; color:#64748b;">
                                                        <asp:Label runat="server" ID="lblmobile" />
                                                    </div>
                                                </div>
                                                <asp:Image id="imgCapture" runat="server" Visible="false"
                                                    style="width:60px; height:60px; border-radius:50%; object-fit:cover; border:2px solid #e2e8f0;" />
                                            </div>

                                            <div class="form-group row">
                                                <label class="col-sm-2 col-form-label">Deposit Type</label>
                                                <div class="col-sm-3">
                                                    <asp:Label ID="lblproduct" runat="server"
                                                        CssClass="form-control-plaintext font-weight-bold" />
                                                </div>
                                                <label class="col-sm-2 col-form-label text-right">Amount</label>
                                                <div class="col-sm-2">
                                                    <asp:Label ID="lblamt" runat="server"
                                                        CssClass="form-control-plaintext text-danger font-weight-bold" />
                                                </div>
                                                <label class="col-sm-1 col-form-label text-right">Balance</label>
                                                <div class="col-sm-2">
                                                    <asp:Label ID="lblbal" runat="server"
                                                        CssClass="form-control-plaintext text-success font-weight-bold" />
                                                </div>
                                            </div>
                                            <asp:Panel ID="rdinfo" Visible="false" runat="server">

                                                <div class="card card-body ">
                                                    <div class="form-group row  border-sm-bottom">
                                                        <label class="col-sm-2 col-form-label ">Total
                                                            Due</label>
                                                        <asp:Label ID="rd_due" runat="server"
                                                            CssClass="col-sm-1 col-form-label text-success border-sm-right  "
                                                            Font-Size="Large" Font-Bold="true"></asp:Label>
                                                        <label class="col-sm-2 col-form-label  ">Paid </label>
                                                        <asp:Label ID="rd_duepaid" runat="server"
                                                            CssClass="col-sm-1 col-form-label text-success border-sm-right  "
                                                            Font-Size="Large" Font-Bold="True"></asp:Label>
                                                        <label class="col-sm-2 col-form-label  ">Balance</label>
                                                        <asp:Label ID="rd_duebalance" runat="server" Font-Bold="True"
                                                            CssClass="col-sm-1 col-form-label text-danger  border-sm-right "
                                                            Font-Size="Large"></asp:Label>
                                                        <asp:Label ID="lblhed" runat="server"
                                                            CssClass="col-sm-1 col-form-label " Width="100%">
                                                        </asp:Label>
                                                        <asp:Label ID="rdlateby" runat="server" Font-Bold="True"
                                                            CssClass="col-sm-1 col-form-label "></asp:Label>
                                                    </div>
                                                    <div class="form-group row ">

                                                        <label class="col-sm-2 col-form-label ">No of
                                                            Due</label>
                                                        <div class="col-sm-1 border-sm-right ">
                                                            <asp:UpdatePanel runat="server" ID="rddue" Visible="true">
                                                                <ContentTemplate>
                                                                    <asp:TextBox runat="server" CssClass="form-control"
                                                                        Width="100%" style="text-align:right "
                                                                        ID="txtnod" AutoPostBack="true"
                                                                        data-validation-engine="validate[required,funcCall[chkdecim[]]"
                                                                        data-errormessage-value-missing="No of due Missing">
                                                                    </asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-sm-1 col-form-label ">Penalty</label>
                                                        <div class="col-sm-2 border-sm-right ">

                                                            <asp:UpdatePanel runat="server" ID="rdpenal" Visible="true">
                                                                <ContentTemplate>
                                                                    <asp:TextBox runat="server" CssClass="form-control"
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
                                                                        Width="100%" ForeColor="#09c">
                                                                    </asp:Label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>

                                                    </div>

                                                </div>
                                            </asp:Panel>


                                            <div class="form-group row ">
                                                <label class="col-sm-2 col-form-label ">Payment Amount</label>

                                                <div class="col-sm-3">
                                                    <asp:UpdatePanel ID="rcpt" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox runat="server" CssClass="form-control"
                                                                ID="txtamt" AutoPostBack="true"
                                                                data-validation-engine="validate[required,funcCall[chkdecim[]]"
                                                                data-errormessage-value-missing="Receipt Amount Missing">
                                                            </asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </div>
                                            </div>


                                            <div class="form-group row ">
                                                <label class="col-sm-2 col-form-label ">Notes</label>
                                                <div class="col-sm-3">
                                                    <asp:TextBox runat="server" CssClass="form-control"
                                                        style="text-align:left  " ID="txtnar"></asp:TextBox>
                                                </div>
                                            </div>






                                            <div class="form-group row">
                                                <label class="col-sm-2 col-form-label ">Mode of Payment</label>
                                                <div class="col-sm-3">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>

                                                            <asp:DropDownList ID="mop" runat="server"
                                                                AutoPostBack="true" CssClass="form-control ">
                                                                <asp:ListItem>Cash</asp:ListItem>
                                                                <asp:ListItem>Account</asp:ListItem>
                                                                <asp:ListItem>Transfer</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </div>

                                                <asp:Label ID="lblsb" Text="SB Account No" runat="server"
                                                    CssClass="col-sm-2 col-form-label " Visible="false">
                                                </asp:Label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="txt_sb" runat="server" Visible="false"
                                                        CssClass="form-control" Width="130px" AutoPostBack="true"
                                                        data-validation-engine="validate[required]"
                                                        data-errormessage-value-missing="Valid Account No is required!">
                                                    </asp:TextBox>
                                                </div>

                                                <asp:Label runat="server" ID="lbl_sb_bal"
                                                    CssClass="col-sm-2 text-success "></asp:Label>
                                            </div>

                                            <div class="form-group row">
                                                <asp:Label ID="lbl" Text="Transfer From"
                                                    CssClass="col-sm-2 col-form-label " runat="server" Visible="false">
                                                </asp:Label>
                                                <div class="col-sm-3">
                                                    <asp:UpdatePanel ID="lstbnk" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="bnk" runat="server" Width="200px"
                                                                CssClass="form-control " Visible="false">
                                                            </asp:DropDownList>
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



                                                    <div class="form-group row border-sm-bottom "></div>
                                                    <div class="form-group row ">
                                                        <div class="col-sm-4"></div>
                                                        <div class="col-sm-4">
                                                            <div class="btn-group " role="group">
                                                                <asp:Button ID="btn_up_can" runat="server"
                                                                    CssClass="btn-outline-secondary  btn" Text="Clear"
                                                                    OnClientClick="return detach();" />
                                                                <asp:Button ID="btn_up_rcpt" runat="server"
                                                                    CssClass="btn-outline-primary btn" Text="Save" />
                                                            </div>

                                                        </div>
                                                    </div>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </asp:Panel>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlprnt" style="display:none">

                                        <asp:UpdatePanel runat="server" ID="pnlbtnprnt" Visible="true">
                                            <ContentTemplate>
                                                <div class="form-group row border-bottom  ">
                                                    <div class="col-md-3"></div>
                                                    <div class="col-md-6">
                                                        <div class="btn-group" role="group">
                                                            <asp:Button runat="server" ID="btntog"
                                                                Class="btn btn-outline-primary" Text="Close" />
                                                            <button type="button" Class="btn btn-outline-primary"
                                                                onclick="PrintOC();">Print Office
                                                                Copy</button>
                                                            <button type="button" Class="btn btn-outline-primary"
                                                                onclick="PrintCC();">Print Customer
                                                                Copy</button>
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
                                                                                                Karavilai
                                                                                                Nidhi
                                                                                                Limited

                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr style="height: 10px;">
                                                                                            <td
                                                                                                style="width: 100%; height: 10px;text-align: left; font-size: 11px;">
                                                                                                <span>Reg No
                                                                                                    :
                                                                                                    18-37630/97</span><br /><span
                                                                                                    style="margin-top:5px;">8-12A,Vijayam,
                                                                                                    Main
                                                                                                    Road
                                                                                                    Karavilai,Villukuri
                                                                                                    P.
                                                                                                    O</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td
                                                                                                style="width: 100%;font-size:12px;">
                                                                                                Branch :
                                                                                                <asp:Label id="pbranch"
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
                                                                                                No&nbsp;
                                                                                                &nbsp; :
                                                                                            </td>
                                                                                            <td
                                                                                                style="width: 90px; height: 21px;font-size: 15px">
                                                                                                <asp:Label ID="pvno"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr style="height: 21px;">
                                                                                            <td
                                                                                                style="width: 82px; height: 21px;font-size: 15px">
                                                                                                Date :</td>
                                                                                            <td
                                                                                                style="width: 90px; height: 21px;font-size: 15px">
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
                                                                                            Customer Name
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 21px; height: 21px;">
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                                            <asp:Label ID="pcname"
                                                                                                runat="server">
                                                                                            </asp:Label>
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
                                                                                                id="pnar">
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
                                                                            Amount in Words :&nbsp;
                                                                            <asp:Label ID="paiw" runat="server">
                                                                            </asp:Label>

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
                                                                                    ID="premit"></asp:Label>
                                                                                &nbsp;)</td>


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
                                                                                                Karavilai
                                                                                                Nidhi
                                                                                                Limited

                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr style="height: 10px;">
                                                                                            <td
                                                                                                style="width: 100%; height: 10px;text-align: left; font-size: 11px;">
                                                                                                <span>Reg No
                                                                                                    :
                                                                                                    18-37630/97</span><br /><span
                                                                                                    style="margin-top:5px;">8-12A,Vijayam,
                                                                                                    Main
                                                                                                    Road
                                                                                                    Karavilai,Villukuri
                                                                                                    P.
                                                                                                    O</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td
                                                                                                style="width: 100%;font-size:12px;">
                                                                                                Branch :
                                                                                                <asp:Label id="pcbranch"
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
                                                                                                No&nbsp;
                                                                                                &nbsp; :
                                                                                            </td>
                                                                                            <td
                                                                                                style="width: 90px; height: 21px;font-size: 15px">
                                                                                                <asp:Label ID="pcvno"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr style="height: 21px;">
                                                                                            <td
                                                                                                style="width: 82px; height: 21px;font-size: 15px">
                                                                                                Date :</td>
                                                                                            <td
                                                                                                style="width: 90px; height: 21px;font-size: 15px">
                                                                                                <asp:Label ID="pcdate"
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
                                                                                            <asp:Label ID="pcglh"
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
                                                                                            <asp:Label ID="pccid"
                                                                                                runat="server">
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr
                                                                                        style="height: 30px;border-bottom:1px solid black">
                                                                                        <td
                                                                                            style="font-size: 12px; width: 139px; height: 21px;">
                                                                                            Customer Name
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 21px; height: 21px;">
                                                                                        </td>
                                                                                        <td
                                                                                            style="width: 263px; height: 21px;font-size: 15px">
                                                                                            <asp:Label ID="pccname"
                                                                                                runat="server">
                                                                                            </asp:Label>
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
                                                                                                <asp:Label ID="pcamt"
                                                                                                    runat="server">
                                                                                                </asp:Label>
                                                                                            </b>

                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr style="height: 25px;">
                                                                                        <td colspan="3"
                                                                                            style="font-size: 12px; width: 423px; height: 21px;">
                                                                                            <asp:Label runat="server"
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
                                                                        <td colspan="2" style="padding-left:5px">
                                                                            Amount in Words :&nbsp;
                                                                            <asp:Label ID="pcaiw" runat="server">
                                                                            </asp:Label>

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
                                                                                    ID="pcremit">
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



                                </ContentTemplate>

                            </asp:UpdatePanel>





                        </div>
                    </div>
                </div>
            </div>
        </form>




        <script src="../js/jquery.js" type="text/javascript"></script>

        <script src="js/daterangepicker.js" type="text/javascript"></script>
        <script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
        <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
        <script src="js/jquery-ui.min.js" type="text/javascript"></script>
        <link href="css/jquery-ui.min.css" rel="stylesheet" />

        <script src="js/printThis.js" type="text/javascript"></script>

        <script type="text/javascript">


            $(document).ready(function () {


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


            function detach() {
                if (window.jQuery && $.fn.validationEngine) {
                    $("#newpayment").validationEngine('detach');
                }
                return true;
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