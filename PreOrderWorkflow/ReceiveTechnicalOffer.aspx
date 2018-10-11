<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="ReceiveTechnicalOffer.aspx.cs" Inherits="PreOrderWorkflow.ReceiveTechnicalOffer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>
        .form-panel {
            background: #ffffff;
            margin: 10px;
            padding: 10px;
            box-shadow: 0px 3px 2px #aab2bd;
            text-align: left;
        }

        .mt {
            margin-top: 5px;
        }

        .row {
            margin-right: -1px;
            margin-left: -1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align:center;font-size:16px;font-weight:bold">Receive Technical Offer</div>
      <div class="container">
    <div class="col-lg-12" style="background-color:#fff;min-height:600px;font-size:11px;margin-top:20px">
        <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" Width="95%" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#e9e9e9">
            <Columns>
                <asp:TemplateField HeaderText="WFID">
                    <ItemTemplate>
                        <%#Eval("WFID") %>
                    </ItemTemplate>
                </asp:TemplateField>

                    <asp:TemplateField HeaderText="WF ParentId " Visible="false">
                    <ItemTemplate>
                        <%#Eval("Parent_WFID") %>
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
                        <asp:Button  CssClass="btn-link" runat="server"  ID="btnReceiveTechOffer" OnClick="btnReceiveTechOffer_Click" CommandArgument='<%#Eval("WFID")+"&"+ Eval("Parent_WFID")+"&"+Eval("WF_Status") %>' Text="Edit" />  <%--Visible='<%#Eval("WF_Status").ToString()=="Enquiry Raised" %>'--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
        </div>
</asp:Content>
