<%@ Page Title="Ledger Creation" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="GeneralLedger.aspx.vb" Inherits="Fiscus.GeneralLedger" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/ValidationEngine.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form id="frmgl" runat="server">

        <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

        <nav class="page-breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Master</a></li>
                <li class="breadcrumb-item active">Ledger</li>
            </ol>
        </nav>
            
        <div class="card card-body ">
            <div id="smarttab">
                <ul class="nav" id="tabitm">
                    <li>
                        <a class="nav-link" href="#tab-1">Ledger Creation
                        </a>
                    </li>
                        <li>
                        <a class="nav-link" href="#tab-2">List of Ledgers
                        </a>
                    </li>
                
                    </ul>
                <div class="tab-content">
                    <div id="tab-1" class="tab-pane" role="tabpanel">

                        <asp:updatePanel>
                            <ContentTemplate>
                                
                                    <div class="card card-body ">
                                        <div class="form-group row ">
                                            <label class="col-sm-2 col-form-label text-primary ">Ledger Name</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel runat="server" >
                                        <ContentTemplate>
                                    <asp:TextBox runat="server" CssClass="form-control"   ID="txtled" AutoPostBack="True" data-validation-engine="validate[required]" data-errormessage-value-missing="Name Missing"  ></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                            </div>
                                        </div >
                                        <div class="form-group row ">
                                            
                                            <label class="col-sm-2 col-form-label text-primary ">Under</label>
                                            <div class="col-sm-3">
                                                 <asp:DropDownList ID="undr" runat="server" CssClass="form-control " data-validation-engine="validate[required]" data-errormessage-value-missing="Select an Option" >
                                    <asp:ListItem><-Select-></asp:ListItem>
                                    <asp:ListItem Value="Assets" Text ="Assets"></asp:ListItem>
                                    <asp:ListItem Value="Expenses" text="Expenses"></asp:ListItem>
                                    <asp:ListItem Value="Income" Text="Income"></asp:ListItem>
                                    <asp:ListItem Value="Liabilities" Text="Liabilities"></asp:ListItem>
                                                          </asp:DropDownList>
                                            </div>
                                            </div>

                                        <div class="form-group row ">
                                            <label class="col-sm-2 col-form-label text-primary ">OPening Balance</label>
                                            <div class="col-sm-3">
                                                <asp:TextBox runat="server" CssClass="form-control"   ID="txtob" AutoPostBack="True" data-validation-engine="validate[required,funcCall[chkdecim]]" ></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="form-group row border-bottom"></div>

                                        <div class="form-group row ">
                                        <div class="col-sm-4"></div>

                                        <div class="col-sm-4">
                                            <div class="btn-group " role="group" >
                                                 
                        <asp:Button  runat="server"   ID="btn_clr" CssClass="btn btn-outline-secondary  " Text ="Clear" OnClientClick  ="return detach();" />
                        <asp:Button  runat="server"   ID="btn_update" CssClass="btn btn-outline-primary " Text ="Update"  />
                                                </div>
                                        </div>
                                            </div>
                                        </div>
                                        
                                    
                                    
                               
                            </ContentTemplate>
                        </asp:updatePanel>
                        

                        </div>
                    <div id="tab-2" class="tab-pane" role="tabpanel">

                        <asp:UpdatePanel runat="server" >
                            <ContentTemplate>

                        <div class="card card-body ">
                              <table style="width:80%;margin-left:0px;float:left "  class="table table-bordered  ">
                                  <thead>
                                         <th style="width:43%;">A/C Head</th>
                                         <th style="width:25%;">Under</th>
                                         <th style="width:15%;">Opening Balance</th>
                                         
                                      </thead>
                                  </table>
                <asp:UpdatePanel runat="server" >
                    <ContentTemplate>
                        <asp:GridView  runat="server" ID="disp"  AutoGenerateColumns="false" style="margin-top:0px;margin-left:0px;float:left" 
                            AllowPaging ="true" OnPageIndexChanging="OnPaging" PageSize="35" OnRowDataBound="disp_RowDataBound" OnSelectedIndexChanged   ="OnSelectedIndexChanged" 
                             Width="80%" Visible="true" ShowHeaderWhenEmpty="false" ShowHeader="false"  CssClass="table table-bordered   "   >
                            <Columns>


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                  <asp:Label ID="lblached" runat="server" text='<%#Eval("ledger")%>' Style="text-align:left "></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="37%" />

                                </asp:TemplateField>
                                     
                                   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblundr" runat="server" text='<%#Eval("under")%>' Style="text-align:left  ;float:left "></asp:Label>
                                                </ItemTemplate>
                                                                           <ItemStyle Width="25%" />

                                                </asp:TemplateField>

   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblob" runat="server" text='<%#Eval("ob", "{0:N}")%>' Style="text-align:right ;float:right  "></asp:Label>
                                                </ItemTemplate>
                                           <ItemStyle Width="15%" />

                                                </asp:TemplateField>

                                </Columns> 
                                                </asp:GridView>
                        </ContentTemplate>
                                                            </asp:UpdatePanel>
            
                        </div>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                        
                        </div>
                    
                    </div>

                    </div>
            </div>

                
            </form>

   

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/jquery.smartTab.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/smart_tab.min.css" />
     <script src="../js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="../js/jquery.validationEngine.js" type="text/javascript" ></script>



    <script type="text/javascript">


        $(document).ready(function () {

            $("#frmgl").validationEngine('attach');
            $("#frmgl").validationEngine({ promptPosition: "topleft", scroll: false, focusFirstField: true, showArrow: true });
            $("#frmgl").validationEngine('attach', { promptPosition: "topleft", scroll: false, showArrow: true });


            $('#smarttab').smartTab({
                selected: 0, // Initial selected tab, 0 = first tab
                theme: 'default', // theme for the tab, related css need to include for other than default theme
                orientation: 'horizontal', // Nav menu orientation. horizontal/vertical
                justified: false, // Nav menu justification. true/false
                autoAdjustHeight: false, // Automatically adjust content height
                backButtonSupport: true, // Enable the back button support
                enableURLhash: true, // Enable selection of the tab based on url hash
                transition: {
                    animation: 'slide-swing', // Effect on navigation, none/fade/slide-horizontal/slide-vertical/slide-swing
                    speed: '400', // Transion animation speed
                    easing: '' // Transition animation easing. Not supported without a jQuery easing plugin
                },
                keyboardSettings: {
                    keyNavigation: true, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
                    keyLeft: [37], // Left key code
                    keyRight: [39] // Right key code
                }
            });

           
        });

     

        function chkdecim(field, rules, i, options) {
            var regex = /^\d+\.?\d{0,2}$/;
            if (!regex.test(field.val())) {
                return "Please enter a Valid Number."
            }
        }

        function detach() {
            $("#frmgoldrate").validationEngine('detach');

        }

      



    </script>


</asp:Content>
