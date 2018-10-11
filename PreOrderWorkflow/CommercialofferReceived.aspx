<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="CommercialofferReceived.aspx.cs" Inherits="PreOrderWorkflow.CommercialofferReceived" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center; font-size: 16px; font-weight: bold">Commercial Offer Receive</div>
    <div class="container">
        <div class="col-lg-12" style="background-color: #fff; min-height: 600px; font-size: 11px; margin-top: 20px">
            <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" Width="95%" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#e9e9e9">
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

                    <asp:TemplateField HeaderText="Supplier">
                        <ItemTemplate>
                            <%#Eval("SupplierName") %>
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

                    <asp:TemplateField HeaderText="DateTime">
                        <ItemTemplate>
                            <%#Convert.ToDateTime(Eval("DateTime")).ToString("dd-MM-yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button CssClass="btn-link" runat="server" ID="btnReceivecommercial" OnClick="btnReceivecommercial_Click" CommandArgument='<%#Eval("WFID") %>' Text="Receive commercial offer" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>
    </div>
</asp:Content>
