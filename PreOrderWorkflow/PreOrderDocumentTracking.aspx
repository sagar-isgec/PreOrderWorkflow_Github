<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="PreOrderDocumentTracking.aspx.cs" Inherits="PreOrderWorkflow.PreOrderDocumentTracking" EnableEventValidation="false"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="scripts/datepicker.js"></script>
    <script src="scripts/datepickerui.js"></script>
    <script>
        $(function () {
            $(".txtDateFrom").datepicker({
                dateFormat: 'dd-mm-yy',
                // startDate: '-3d'
            });
        });

    </script>
    <script>
        $(function () {
            $(".txtDateTo").datepicker({
                dateFormat: 'dd-mm-yy',
                // startDate: '-3d'
            });

        });

    </script>

    <style type="text/css">
        .auto-style1 {
            width: 175px;
        }

        .auto-style3 {
            width: 189px;
        }

        .auto-style2 {
            width: 5px;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div style="text-align: center; font-size: 16px; font-weight: bold">
        Pre Order Document Tracking
       
        <div style="text-align: right; font-size: 12px">
            <asp:Label ForeColor="Black" Font-Bold="true" BackColor="Orange" runat="server" ID="lblParent">
         Parent Record
            </asp:Label>
            <asp:Label ForeColor="Black" Font-Bold="true" BackColor="Yellow" runat="server" ID="lblChild">
         Child Record
            </asp:Label>
        </div>
    </div>
    <div style="margin-top: 0px; margin-left: 100px; font-size: 12px;">
        <table>
            <tr>
                <td class="auto-style3"></td>
                <td class="auto-style1" style="font-weight: bold">&nbsp From</td>
                <td style="font-weight: bold">&nbsp To</td>
                <td class="auto-style2"></td>
                <td class="auto-style3"></td>
                <td class="auto-style1" style="font-weight: bold">&nbsp From</td>
                <td style="font-weight: bold">&nbsp To</td>
            </tr>
            <tr>
                <td class="auto-style3" style="font-weight: bold">Project</td>
                <td class="auto-style1">
                    <asp:DropDownList runat="server" ID="ddlProjectFrom" Height="20" Width="172" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlProjectTo" Height="20" Width="172" AutoPostBack="true"></asp:DropDownList></td>
                <td class="auto-style2"></td>
                <td class="auto-style3" style="font-weight: bold">PMDL Doc. No.</td>

                <td class="auto-style1">
                    <asp:DropDownList runat="server" ID="ddlPMDLDocFrom" Height="20" Width="172" OnSelectedIndexChanged="ddlPMDL_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPMDLDocTo" Height="20" Width="172"></asp:DropDownList></td>
                <tr>

                    <td class="auto-style3">
                        <br />
                    </td>

                </tr>
                <tr>
                    <td class="auto-style3" style="font-weight: bold">Buyer</td>
                    <td class="auto-style1">
                        <asp:DropDownList runat="server" ID="ddlBuyerFrom" Height="20" Width="172" OnSelectedIndexChanged="ddlBuyer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlBuyerTo" Height="20" Width="172"></asp:DropDownList></td>
                    <td class="auto-style2"></td>
                    <td class="auto-style3" style="font-weight: bold">Tech. Spec. Release Date</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDateFrom" CssClass="txtDateFrom" Height="20" Width="172" OnTextChanged="txtDateFrom_SelectedDateChange" AutoPostBack="true"></asp:TextBox></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDateTo" CssClass="txtDateTo" Height="20" Width="172"></asp:TextBox></td>
                    <td class="auto-style2">&nbsp&nbsp
                    </td>

                    <td class="auto-style3">
                        <br />
                    </td>

                </tr>
                <br />
                <td class="auto-style3"></td>
                <td class="auto-style1"></td>
                <td class="auto-style2"></td>
                <td class="auto-style1" style="align-items: center">
                    <asp:Button runat="server" ID="btnGetRecord" Text="Generate Report" OnClick="btnGetRecord_Click" Height="23px" Width="130px" Visible="true" /></td>
                <td class="auto-style1" style="align-items: center">
                    <asp:Button runat="server" ID="btnExport" Text="Export to excel" OnClick="btnExport_Click" Height="23px" Width="130px" Visible="true" /></td>
            </tr>
        </table>
    </div>
    <br />
    <div style="text-align: center; font-size: 12px">
        <asp:Label ForeColor="Red" Font-Bold="true" Font-Size="Medium" runat="server" ID="lblNoRecord">
       No Record Found for the selected combination !
        </asp:Label>
    </div>
    <div  style="width:98%;overflow-x:visible; overflow-y:visible; margin:auto;font-size:11px">
       
        <asp:GridView runat="server" ID="gvData"  overflow="auto" CssClass="table table-bordered table-hover" RowStyle-VerticalAlign="Bottom" Width="2200px" AutoGenerateColumns="false" Visible="true" AllowPaging="True" PageSize="15" OnPageIndexChanging="gvData_PageIndexChanging" Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" RowDataBound="gvData_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="WFID">
                    <ItemTemplate>
                        <%#Eval("WFID") %>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Parent WFID">
                    <ItemTemplate>
                        <%#Eval("Parent_WFID") %>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Project">
                    <ItemTemplate>
                        <%#Eval("Project") %>
                    </ItemTemplate>
                    <HeaderStyle Width="300px" />
                    <ItemStyle Width="300px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Element">

                    <ItemTemplate>
                        <%#Eval("Element") %>
                    </ItemTemplate>
                    <HeaderStyle Width="200px" />
                    <ItemStyle Width="200px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Specification No">
                    <ItemTemplate>
                        <%#Eval("SpecificationNo") %>
                    </ItemTemplate>
                    <HeaderStyle Width="300px" />
                    <ItemStyle Width="300px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="PMDL Doc No">
                    <ItemTemplate>
                        <%#Eval("PMDLDocNo") %>
                    </ItemTemplate>
                    <HeaderStyle Width="200px" />
                    <ItemStyle Width="200px" />
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Buyer">
                    <ItemTemplate>
                        <%#Eval("BuyerName") %>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Project Manager">
                    <ItemTemplate>
                        <%#Eval("ManagerName") %>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="WF Status">
                    <ItemTemplate>
                        <%#Eval("WF_Status") %>
                    </ItemTemplate>
                    <ItemStyle Width="300px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Supplier Code">
                    <ItemTemplate>
                        <%#Eval("SupplierCode") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Supplier Name">
                    <ItemTemplate>
                        <%#Eval("SupplierName") %>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Created Date">
                    <ItemTemplate>
                        <%#Eval("dt_Created") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Technical Specification Release Date">
                    <ItemTemplate>
                        <%#Eval("dt_tsr") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Enquiry In Progress Date">
                    <ItemTemplate>
                        <%#Eval("dt_eip") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enquiry Raised Date">
                    <ItemTemplate>
                        <%#Eval("dt_er") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />

                </asp:TemplateField>

                <asp:TemplateField HeaderText="Technical Offer Received Date">
                    <ItemTemplate>
                        <%#Eval("dt_tor") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Commercial Offer Finalized Date">
                    <ItemTemplate>
                        <%#Eval("dt_cof") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IDMS Receipt No.">
                    <ItemTemplate>
                        <%#Eval("ReceiptNo") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IDMS Receipt Status">
                    <ItemTemplate>
                        <%#Eval("ReceiptStatus") %>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IDMS Receipt Details" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton CssClass="btn-link" runat="server" ID="btnReceipt" OnClick="btnReceipt_Click" CommandArgument='<%#Eval("WFID") %>' Visible='<%#GetVisible(Eval("Parent_WFID").ToString())%>' Text='<i class="fa fa-mail-forward" style="font-size:16px"></i>' />
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                </asp:TemplateField>
            </Columns>
            
            <HeaderStyle BackColor="#E9E9E9"></HeaderStyle>
            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />

        </asp:GridView>
            
    </div>

</asp:Content>
