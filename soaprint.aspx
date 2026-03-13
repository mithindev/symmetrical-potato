<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="soaprint.aspx.vb" Inherits="Fiscus.soaprint" MasterPageFile="~/Admin/Admin.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Statement of Accounts</title>
    <style type="text/css">
           @media print {  
  @page {
    size: A4; /* landscape */
    /* you can also specify margins here: */
    margin:10mm;
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
     /* for compatibility with both A4 and Letter */
  }

  table{
      page-break-after:auto;

  }
  tr{page-break-inside:avoid;
         page-break-after:auto;
    }
  td{page-break-inside:avoid;
     page-break-after:auto;
  }
  thead{display:table-header-group }
         
}
    

    </style>

    </asp:content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="frmsoaprnt" runat="server">
        <asp:ScriptManager ID="SM3" runat="server" ></asp:ScriptManager>
        <div id="vouchprint">
         <table style="border-collapse: collapse;width:100%" >
        <tbody>
            <tr style="border: none;">
                <td style="width:75px;align-content:center"><img src="Images/KBF-LOGO.png" alt="" width="72" height="60" /></td>
                <td>
                    <table border="0" style="border-collapse: collapse; width:100%; height: 86px;" >
                        <tbody>
                            <tr >
                                <td style="width: 100%;font-size:20px;height:10px;font-weight:600">Karavilai Nidhi Limited
                                    
                                </td>
                            </tr>   
                            
                            <tr>
                                <td style="width: 100%; height: 10px;text-align: left; font-size: 14px;vertical-align:top"><span>Reg No : 18-37630/97</span><br/><span style="margin-top:5px;">8-12A,Vijayam, Main Road Karavilai,Villukuri P. O</span></td>
                            </tr>
                        
                        </tbody>
                    </table>
                </td>
                <td style="vertical-align:top;padding-left:10px;float:right">
                    <asp:Label runat="server" ID="lblrpt"></asp:Label>
                </td>
                
            </tr>
        </tbody>
    </table>

        <h2 style="padding:10px;width:100%;text-align:center;font-variant-caps:all-small-caps">Statement of Accounts</h2>
        
            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblstprd" CssClass="col-form-label text-primat"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
        <h3 style="padding-left:10px">Customer Details</h3>
            <asp:UpdatePanel runat="server" >
                <ContentTemplate>

                
        <table style="border:1px solid #000;width:100%;table-layout:fixed">
            <tbody>
                <tr>
                    <td style="width:50%;border-right:1px solid #000;">
                        <table style="width:100%;">
                            <tbody>
                                <tr>
                                    <td>Customer Id</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblcid"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Account No</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblacn"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Name of the Customer</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblname"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Contact No</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblmobile"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Email ID</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblmail"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Nominee</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblnominee"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Branch</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblbr"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td style="width:50%;vertical-align:top"><table style="width:100%;height:100%">
                        <tbody >
                            <tr>
                                <td style="width:100%;vertical-align:top"> 
                                    Address of the Customer:<br /><br />
                                    <asp:Label ID="lbllname" runat="server"></asp:Label><br />
                                   <asp:Label ID="lbladd" runat="server"></asp:Label>

                                </td>
                            </tr>
                        </tbody>
                        </table></td>
                </tr>
            </tbody>
        </table>
        <br />
        <table style="width:100%;border:1px solid #000;border-collapse:collapse;table-layout:fixed;">
            <tbody>
                <tr style="height:30px">
                    <td style="border:1px solid #000;text-align:center;width:12%;">Date</td>
                    <td style="border:1px solid #000;text-align:center;width:32%;">Particulars</td>
                    <td style="border:1px solid #000;text-align:center;width:11%;">Transaction Type</td>
                    <td style="border:1px solid #000;text-align:center;width:15%;">Debit</td>
                    <td style="border:1px solid #000;text-align:center;width:15%;">Credit</td>
                    <td style="border:1px solid #000;text-align:center;width:15%;">Balance</td>
                </tr>
            </tbody>
        </table>

                  <asp:GridView  runat="server" ID="disp" AutoGenerateColumns="false" Width="100%" 
                            AllowPaging ="false"  AutoGenerateSelectButton="false" 
                              Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered"   >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lbldat" runat="server" text='<%#Eval("date", "{0:MM-dd-yyyy}")%>' Style="text-align:center;" Width="100%"></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />

                                </asp:TemplateField>
                                     
                                   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblnar" runat="server" text='<%#Eval("Narration")%>' Style="text-align:left;float:left;padding-left:10px; "></asp:Label>
                                                <br />
                                            <asp:Label ID="lbldue" runat="server" text='<%#Eval("due")%>' Style="text-align:left;float:left;padding-left:10px;font-size:12px;" Width="100%"></asp:Label>
                                                
                                            </ItemTemplate>
                                                <ItemStyle Width="32%" />

                                        </asp:TemplateField>
                                     <asp:TemplateField  >
                                    <ItemTemplate>
                                <asp:Label ID="lbltyp" runat="server" text='<%#Eval("type")%>' Style="text-align:center" Width="100%"></asp:Label>

                                    </ItemTemplate>
                                       <ItemStyle CssClass="sensitive" Width="11%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate >
                                <asp:Label ID="lbldr" runat="server" text='<%#Eval("drd", "{0:#,##,###.00}")%>' Style="text-align:right ;float:right;padding-right:10px; "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField>
                                    <ItemTemplate>
                                <asp:Label ID="lblcr" runat="server" text='<%#Eval("crd", "{0:#,##,###.00}")%>' Style="text-align:right ;float:right;padding-right:10px; "></asp:Label>
                                        
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                <asp:Label ID="lblbal" runat="server" text='<%#Eval("dbal", "{0:#,##,###.00}")%>' Style="text-align:right ;float:right;padding-right:10px; "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>

                              
                                
                             
                                 
                            </Columns>

                        </asp:GridView>
                    <br />
                    <br />
                    <asp:Label runat="server" ID="lblbal" style="padding-left:20px"></asp:Label>
                    <br />
                    <br />
                    <p style="width:100%;text-align:center">** This is a Computer Generated Statement **</p>
                    </ContentTemplate>
            </asp:UpdatePanel>

            </div>
    </form>
     <script src="js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/printThis.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {

                $('#vouchprint').printThis({
                    importCSS: false,
                    importStyle: true,
                    printContainer: true,



                });

            });

        </script>

    </asp:Content>