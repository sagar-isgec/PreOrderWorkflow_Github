<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="RaiseEnquery.aspx.cs" Inherits="PreOrderWorkflow.RaiseEnquery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div style="text-align:center;font-size:16px;font-weight:bold">Raise Enquiry</div>
    <div class="container">
        <div class="col-lg-12" style="background-color: #fff; min-height: 600px; font-size: 11px;margin-top:20px">
            <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" Width="95%" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#e9e9e9">
                <Columns>
                    <asp:TemplateField HeaderText="WFID">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnkViewWorkflow" Text=' <%#Eval("WFID") %>' CommandArgument='<%#Eval("WFID") %>' OnClick="lnkViewWorkflow_Click"></asp:LinkButton>
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

                 <%--   <asp:TemplateField HeaderText="Supplier">
                        <ItemTemplate>
                      <%#Eval("SupplierName") %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

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

                    <asp:TemplateField HeaderText="Raise Enquiry" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn-link" runat="server" ID="btnRaiseEnquiry" OnClick="btnRaiseEnquiry_Click" CommandArgument='<%#Eval("WFID") %>' Text='<i class="fa fa-mail-forward" style="font-size:16px"></i>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Return">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn-link" runat="server" ID="btnReturm" OnClick="btnReturm_Click" CommandArgument='<%#Eval("WFID") %>' Text='<i class="fa fa-backward" style="font-size:16px"></i>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>
        </div>
    </div>
</asp:Content>
