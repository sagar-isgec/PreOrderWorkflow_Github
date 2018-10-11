<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="ChangeProjectStatus.aspx.cs" Inherits="PreOrderWorkflow.ChangeProjectStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div style="text-align:center;font-size:16px;font-weight:bold">Change Project Status
     </div>
    <br />
    <div>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlProject" runat="server" CssClass="auto-style1" Height="28px" Width="146px" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlElement" runat="server" CssClass="auto-style1" Height="28px" Width="146px" OnSelectedIndexChanged="ddlElement_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
         <asp:DropDownList ID="ddlSpecification" runat="server" CssClass="auto-style1" Height="28px" Width="146px" OnSelectedIndexChanged="ddlSpecification_SelectedIndexChanged"  AutoPostBack="true">
        </asp:DropDownList> &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlPMDL" runat="server" CssClass="auto-style1" Height="28px" Width="146px" OnSelectedIndexChanged="ddlPMDL_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>

   
       

    </div>
    <div class="container">
        <div class="col-lg-12" style="background-color: #fff; min-height: 600px; font-size: 11px;margin-top:20px">
            <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" Width="95%" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#e9e9e9" Visible="true">
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
                    <asp:TemplateField HeaderText="Project Manager">
                        <ItemTemplate>
                            <%#Eval("ManagerName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                <%--    <asp:TemplateField HeaderText="Supplier">
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

                   <%-- <asp:TemplateField HeaderText="Raise Enquiry" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn-link" runat="server" ID="btnRaiseEnquiry" OnClick="btnRaiseEnquiry_Click" CommandArgument='<%#Eval("WFID") %>' Text='<i class="fa fa-mail-forward" style="font-size:16px"></i>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                      <asp:TemplateField HeaderText="Reactivate" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="" runat="server" ID="btnReactivate" OnClick="btnReactivate_Click" CommandArgument='<%#Eval("WFID") %>' Text='<i class=""fa fa-unlock-alt" aria-hidden="true" style="font-size:16px"></i>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Deactivate" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="" runat="server" ID="btnDeactivate" OnClick="btnDeactivate_Click" CommandArgument='<%#Eval("WFID") %>' Text='<i class="fa fa-trash" aria-hidden="true" style="font-size:16px"></i>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Resend link to Supplier" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="" runat="server" ID="btnResend" OnClick="btnResend_Click" CommandArgument='<%#Eval("WFID") %>' Text='<i class="fa fa-backward" aria-hidden="true" style="font-size:16px"></i>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Return" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton Font-Size="11" runat="server" ID="btnReturn" OnClick="btnReturn_Click" CommandArgument='<%#Eval("WFID") %>'  Text='<i class="fa fa-backward" aria-hidden="true" style="font-size:16px"></i>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                       <%--<asp:TemplateField HeaderText="Commercial offer Finalized" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton Font-Size="11" runat="server" ID="btnClosed" OnClick="btnClosed_Click" CommandArgument='<%#Eval("WFID") %>'  ForeColor="Red" Text='<i class="fa fa-unlock-alt" aria-hidden="true" style="font-size:16px"></i>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>

            </asp:GridView>
        </div>
    </div>
    
</asp:Content>
