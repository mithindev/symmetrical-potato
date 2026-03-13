<%@ Page Title="Bank Book" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="BankBook.aspx.vb" Inherits="Fiscus.BankBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <form id="Bannkbook" runat="server" >

        <asp:ScriptManager ID="SM1" runat="server" ></asp:ScriptManager>

        <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Books</a></li>
						<li class="breadcrumb-item active" >Bank Book</li>
					</ol>
				</nav>

          <div class="card card-body ">
              
                  
                                           

                                               <div class="form-group row  border-bottom">

                                                   
                                                       <label  class=" col-form-label text-primary  ">From&nbsp;&nbsp;</label>
                                                       <asp:TextBox ID="txtfrm1" CssClass="col-sm-2 form-control "   runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <label  class=" col-form-label text-primary  ">&nbsp;&nbsp;To</label>
                                                   
                                                       <div class="col-sm-3">
                                                           <div class="input-group">
                                                        <asp:TextBox ID="txtto1" CssClass=" form-control "  runat="server" Width="120px" data-validation-engine="validate[required,funcCall[DateFormat[]]" data-errormessage-value-missing="Date Missing"></asp:TextBox>
                                                       <span class="input-group-btn input-group-append">
                       <asp:LinkButton runat="server" id="btn_show" CssClass ="btn btn-outline-primary ">

                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="inherit"  class="feather feather-search"><circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                                                                </asp:LinkButton>
                                                          </span>
                                                           </div>
            
                                                   </div>
                      </div>

               
                                <asp:updatePanel ID="UpdatePanel2" runat="server" Visible="true" >
                                <ContentTemplate >

                                     <section id="content1">
                                 <table style="border:0px;width:100%;margin-left:0px;float:left "  class="table table-condensed ">
                                  <tbody>
                                         <tr>
                                            <td style="width:45%;text-align:center;color:white;font-variant:small-caps;background-color:#727cf5;">Bank Name</td>
                                         <td style="width:20%;text-align:center;color:white;font-variant:small-caps;background-color:#727cf5;">Opening</td>
                                             <td style="width:20%;text-align:center;color:white;font-variant:small-caps;background-color:#727cf5;">Debit</td>
                                             <td style="width:20%;text-align:center;color:white;font-variant:small-caps;background-color:#727cf5;">Credit</td>
                                             <td style="width:20%;text-align:center;color:white;font-variant:small-caps;background-color:#727cf5;">Closing</td>
                                                                                  </tr>
                                      </tbody>
                                  </table> 
               



                                      <asp:GridView  runat="server" ID="disp_ndh3" AutoGenerateColumns="false" style="margin-top:-0px;margin-left:0px;float:left;z-index:1" 
                            AllowPaging ="false"  PageSize ="100" Width="100%"   EnableViewState="false" EmptyDataText="No Records Found"
                               Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-condensed  "   >
                                <Columns>
                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_typ_supl" runat="server" Width="100%" text='<%#Eval("achead")%>' Style="text-align:left ;float:left;font-size:smaller  "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="45%" />

                                   </asp:TemplateField>

                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_ob" runat="server" text='<%#Eval("ob", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="20%" />
                                     
                                   
                                    </asp:TemplateField>


                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_dr" runat="server" text='<%#Eval("debit", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="20%" />
                                     
                                   
                                    </asp:TemplateField>

                                   <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cr" runat="server" text='<%#Eval("credit", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="20%" />
                                     
                                   
                                    </asp:TemplateField>

                                    
                                    <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:Label ID="lbl_cb" runat="server" text='<%#Eval("cb", "{0:N}")%>' Style="text-align:right;float:right ;font-size:smaller "></asp:Label>
                                    </ItemTemplate>
                                        <ItemStyle Width ="20%" />
                                     
                                   
                                    </asp:TemplateField>


                                </Columns>

                                </asp:GridView> 


                        


                                                             <asp:UpdatePanel runat ="server" Visible="false" >
                        <ContentTemplate>                            <table style="border:1px;width:100%;margin-left:0px;float :left " class ="table table-condensed  ">

                                                 <tbody>
                                                     <tr >
                                                         <td style="width:45%">Total</td>
                                                       
                                                         <td style ="width:20%">
                                                             
                                                         <asp:Label ID="Label1" runat="server" Text="0" Style="text-align:right;float:right "></asp:Label></td>
                                                         <td style ="width:45%"></td> 

                                                         <td style ="width:20%">
                                                             <asp:Label ID="Label2" runat="server" Style="text-align:right;float:right "></asp:Label>
                                                         </td>
                                                     </tr>
                                                 </tbody>
                                             </table>
                            </ContentTemplate>

                        </asp:UpdatePanel>


                                          

                    </section> 

                                    </ContentTemplate>
                                                                </asp:updatePanel>

                                   
              
                                                          

              </div>
         

     
             
         </form>

    <style>

        .table-condensed tr th {
border: 0px solid #6e7bd9;
color: white;
background-color: #6e7bd9;
}

.table-condensed, .table-condensed tr td {
border: 1px solid #6e7bd9;
}

tr:nth-child(even) {
background: #f8f7ff
}

tr:nth-child(odd) {
background: #fff;
}
    </style>

	

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


  

            $("#<%=txtfrm1.ClientID%>").focus(function () {

                
                    $("#<%=txtfrm1.ClientID%>").daterangepicker({


                        "autoUpdateInput": false,
                        "autoApply": true,
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                    }, function (start, end, label) {

                        $("#<%=txtfrm1.ClientID%>").val(start.format('DD-MM-YYYY'));
                        $("#<%=txtto1.ClientID%>").val(end.format('DD-MM-YYYY'));

                    });
                





            });
        }

        


    </script>

</asp:Content>
