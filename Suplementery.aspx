<%@ Page Title="Supplementary" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Suplementery.aspx.vb" Inherits="Fiscus.Suplementery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
<style>
    tr.sensitive:hover {
        background-color:#fff !important;
        color:#000 !important ;
        
    }
    tr.sensitive:hover td{
        font-weight:300 !important;
    }
</style>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


      <form id="suplement" runat="server">

        <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

        <nav class="page-breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Reports</a></li>
                <li class="breadcrumb-item active">Supplementary Report</li>
            </ol>
        </nav>

        <div class="card card-body ">
            
                    
                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>


                        <div class="card-body ">

                            

                            <asp:Repeater ID="rpsuplement" runat="server" OnItemDataBound="OnItemDataBound">
                                <HeaderTemplate>
                                    <table class="table table-bordered  ">
                                        <thead>
                                            
                                            <th style="width:5%"></th>
                                            <th style="width:55%;">Account Head</th>
                                              <th style="width:20%;">Debit</th>
                                         <th style="width:20%;">credit</th>
                                         
                                        
                                        </thead>
                                        <tbody>
                                        
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>    <img alt="" style="cursor: pointer;fill:#000" height="12" width="12" src="images/plus.svg"  /></td>
                                        <td>
                                              <asp:Label ID="lbl_typ_supl" runat="server" text='<%#Eval("suplimentry")%>' Style="text-align:left ;float:left;font-size:small "></asp:Label>
                                        </td>
                                        <td>
                                             <asp:Label ID="lbl_dr_supl" runat="server" text='<%#Eval("DR", "{0:N}")%>' Style="text-align:right;float:right ;font-size:small "></asp:Label>
                                        </td>
                                        <td>
                                             <asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("CR", "{0:N}")%>' Style="text-align:right;float:right ;font-size:small "></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="display:none" class="sensitive">
                                        
                                  <td colspan="4" >
                                        <asp:Repeater ID="rpbrkup" runat="server" >
                                            <HeaderTemplate>
                                                 <table  class="table table-bordered">
                                                <thead>
                                         
                                         <th style="width:10%;">Trans ID</th>
                                            <th style="width:20%;">Account No</th>
                                             <th style="width:30%;">Name</th>
                                      <th style="width:20%;">Debit</th>
                                         <th style="width:20%;">credit</th>
                                                                        </thead>          
                                      
                                  

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width:10%;"><asp:Label ID="lbl_cr_id" runat="server" text='<%#Eval("id")%>' Style="text-align:left ;float:left ;font-size:small"></asp:Label></td>
                                                    <td style="width:20%;"><asp:Label ID="lbl_typ_supl" runat="server" text='<%#Eval("acno")%>' Style="text-align:left ;float:left;font-size:small "></asp:Label></td>
                                                    <td style="width:30%;"><asp:Label ID="Label1" runat="server" text='<%#Eval("FirstName")%>' Style="text-align:left ;float:left;font-size:small "></asp:Label></td>
                                                    <td style="width:20%;"><asp:Label ID="lbl_dr_supl" runat="server" text='<%#Eval("DRc", "{0:N}")%>' Style="text-align:right;float:right ;font-size:small "></asp:Label></td>
                                                   <td style="width:20%;"><asp:Label ID="lbl_cr_supl" runat="server" text='<%#Eval("CRc", "{0:N}")%>' Style="text-align:right;float:right ;font-size:small "></asp:Label></td>
                                                    
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                                </table> 
                                            </FooterTemplate>
                                        </asp:Repeater>
                                      </td>
                                    </tr>
                                </ItemTemplate>
                                
                                <FooterTemplate>

                                    
                                    </tbody>
                                    </table>
                                        
                                </FooterTemplate>
                            </asp:Repeater>
                            <table class="table table-bordered ">
                                <tbody>
                                    <tr> 
                                        
                                          <td style="width:60%;text-align:center;font-variant:small-caps">Total</td>


                                      <td style="width:20%;font-variant:small-caps">

                                          <asp:Label ID="lbltdr" style="text-align:right; float:right;" runat="server"></asp:Label>

                                      </td>
                                         <td style="width:20%;font-variant:small-caps;">
                                             <asp:Label ID="lblrcr" style="text-align:right;float:right;" runat="server"></asp:Label> </td>
                                                                                  </tr>
                                </tbody>
                            </table>



                       

                             

                            </div>

                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        

                   
                
            

            </div>

          </form>

      
   
   
    
<script type="text/javascript">
    $("body").on("click", "[src*=plus]", function () {
        $(this).closest("tr").next().show();
        $(this).attr("src", "images/minus.svg");
    });
    $("body").on("click", "[src*=minus]", function () {
        $(this).attr("src", "images/plus.svg");
        $(this).closest("tr").next().hide();
    });
</script>

</asp:Content>
