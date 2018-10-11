<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="PreOrderProjDetails.aspx.cs" Inherits="PreOrderWorkflow.PreOrderProjDetails" EnableEventValidation="false"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="scripts/datepicker.js"></script>
    <script src="scripts/datepickerui.js"></script>
    <script>
        $(function () {
            $(".txtDateFrom").datepicker({
                dateFormat: 'yy-mm-dd',
                // startDate: '-3d'
            });
        });
       
    </script>
        <script>
        $(function () {
            $(".txtDateTo").datepicker({
                dateFormat: 'yy-mm-dd',
                // startDate: '-3d'
            });
           
        });
       
    </script>
     <style type="text/css">
         .auto-style1 {
             width: 189px;
         }
         .auto-style2 {
             width: 88px;
         }
         .auto-style3 {
             width: 189px;
         }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div style="text-align:center;font-size:16px;font-weight:bold">Pre Order Project Details
     </div>
    <br />
    <div style="margin-top: 20px; margin-left:200px; font-size: 14px">
        <table>
            <tr>
                <td class="auto-style3"></td>
                <td class="auto-style1" style="font-weight:bold">&nbsp From</td>
                <td style="font-weight:bold">&nbsp To</td>
            </tr>
            <tr>
                <td class="auto-style3" style="font-weight:bold">Project</td>
                <td class="auto-style1">
                    <asp:DropDownList runat="server" ID="ddlProjectFrom" Height="20" Width="172"></asp:DropDownList></td>
                <td ><asp:DropDownList runat="server"   ID="ddlProjectTo" Height="20" Width="172"></asp:DropDownList></td>
                  <tr>
                 
                <td class="auto-style3">
                    <br />
                      </td>
              
            </tr>
            <tr>
                <td class="auto-style3" style="font-weight:bold">Technical Release Date</td>
                <td>
                    <asp:TextBox runat="server" ID="txtDateFrom" CssClass="txtDateFrom" Height="20" Width="172"></asp:TextBox></td>
                <td><asp:TextBox runat="server" ID="txtDateTo"  CssClass="txtDateTo" Height="20" Width="172"></asp:TextBox></td>
                 <td class="auto-style2">&nbsp&nbsp
                    </td>
                <br />
                  <td class="auto-style1"><asp:Button runat="server" ID="btnGetRecord" Text="Get PreOrder Data" OnClick="btnGetRecord_Click" Height="23px" Width="145px"  Visible="true"/></td>
                <td class="auto-style1"><asp:Button runat="server" ID="btnExport" Text="Export to excel" OnClick="btnExport_Click" Height="23px" Width="145px"  Visible="true"/></td>
            </tr>
        </table>
    </div>
    <div class="container">
        <div class="col-lg-12" style="background-color: #fff; min-height: 600px; font-size: 11px;margin-top:20px">
            <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" Width="95%" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#e9e9e9" Visible="true" AllowPaging="True" PageSize="15" OnPageIndexChanging="gvData_PageIndexChanging" Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" >
                <Columns>
                    <asp:TemplateField HeaderText="WFID">
                        <ItemTemplate>
                             <%#Eval("WFID") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Project">
                        <ItemTemplate>
                            <%#Eval("Project") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Element">
                        <ItemTemplate>
                            <%#Eval("Element") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Specification No">
                        <ItemTemplate>
                            <%#Eval("SpecificationNo") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="PMDL Doc No">
                        <ItemTemplate>
                            <%#Eval("PMDLDocNo") %>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Buyer">
                        <ItemTemplate>
                            <%#Eval("BuyerName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Project Manager">
                        <ItemTemplate>
                            <%#Eval("ManagerName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="WF Status">
                        <ItemTemplate>
                            <%#Eval("WF_Status") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="User">
                        <ItemTemplate>
                            <%#Eval("EmployeeName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Technical Release Date">
                        <ItemTemplate>
                            <%#Convert.ToDateTime(Eval("DateTime")).ToString("dd-MM-yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Latest Status Date">
                        <ItemTemplate>
                            <%#Convert.ToDateTime(Eval("LatestStatusDateTime")).ToString("dd-MM-yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

<HeaderStyle BackColor="#E9E9E9"></HeaderStyle>
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />

            </asp:GridView>
        </div>
    </div>
    
</asp:Content>
