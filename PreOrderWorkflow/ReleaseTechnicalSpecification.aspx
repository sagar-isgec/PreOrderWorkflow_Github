<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="ReleaseTechnicalSpecification.aspx.cs" Inherits="PreOrderWorkflow.ReleaseTechnicalSpecification" %>

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
            margin-top: 15px;
        }

        .row {
            margin-right: -1px;
            margin-left: -1px;
        }
    </style>
       
    <script>

        function myFunction() {
            var txt;
            var r = confirm("Are you sure to Delete!");
            if (r == true) {
                return true;
            } else {
                return false;
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div style="text-align: center; font-size: 16px; font-weight: bold">Release Technical Specification</div>
    <div style="margin-left:110px"><asp:Button runat="server" ID="btnNewForm" Text="New Specification" OnClick="btnNewForm_Click" CssClass="btn btn-sm btn-primary" /></div>
    <div class="container">
        <div class="col-lg-12" style="background-color: #fff; min-height: 600px; font-size: 11px; margin-top: 20px" runat="server" id="divData" visible="false">
            <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" Width="100%" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#e9e9e9">
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

                    <asp:TemplateField HeaderText="BuyerName">
                        <ItemTemplate>
                            <%#Eval("BuyerName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:TemplateField HeaderText="Supplier">
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

                  <%--  <asp:TemplateField HeaderText="Release">
                        <ItemTemplate>
                            <asp:Button CssClass="btn-link" runat="server" ID="btnReleaseShow" Text="Release" OnClick="btnReleaseShow_Click" CommandArgument='<%#Eval("WFID") +"&" +Eval("Buyer") +"&"+ Eval("SpecificationNo")  +"&"+ Eval("BuyerName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="" runat="server" Visible='<%#Eval("WF_Status").ToString()=="Technical Specification Released Returned" || Eval("WF_Status").ToString() == "Created"  %>' ID="btnEdit" OnClick="btnEdit_Click" CommandArgument='<%#Eval("WFID")+"&"+ Eval("WF_Status")%>' Text='<i class="fa fa-pencil-square-o" aria-hidden="true" style="font-size:16px"></i>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn-link" runat="server" Visible='<%#Eval("WF_Status").ToString()=="Technical Specification Released Returned" || Eval("WF_Status").ToString() == "Created" %>' ID="btnDelete" OnClick="btnDelete_Click" OnClientClick="return myFunction()" CommandArgument='<%#Eval("WFID") %>' Text='<i class="fa fa-trash" aria-hidden="true" style="font-size:16px"></i>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>

        <div runat="server" id="DivNoRecords" style="text-align: center; font-size: 16px; color: #ff6a00; margin-top: 50px" visible="false" class="col-lg-6">
            No Record Found
        </div>

    </div>

 <%--   <div>
        <asp:Label runat="server" ID="lblItemCode"></asp:Label>
        <ajaxToolkit:ModalPopupExtender ID="mpeRelease" Drag="true" runat="server" TargetControlID="lblItemCode" PopupControlID="pnlShowIsgecitemCode" CancelControlID="btncloseCancel"></ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlShowIsgecitemCode" runat="server" CssClass="mfp-dialog-login" Width="50%" Height="350" BorderStyle="Solid" BorderWidth="1" BorderColor="Black" Style="display: none; background: #fff; overflow: scroll; margin-left: 50px; margin-top: 0px">

            <div class="modal-header">
                <asp:Button ID="btncloseCancel" runat="server" CssClass="btn btn-xs btn-danger" Text="x" Style="width: 14px; float: right; margin-top: 2px; font-size: 10px; font-weight: bold" />
                <h4 class="modal-title">Release Technical Specification</h4>

                <div>
                  <b> WFID:</b> <asp:Label runat="server" ID="lblWFId" /> ,
                   <b> Specification: </b> <asp:Label runat="server" ID="lblSpecification" /> ,
                 <b>  Buyer: </b>  <asp:Label runat="server" ID="lblBuyerName" /> 
                </div>
            </div>

            <div class="modal-body">
                <asp:HiddenField runat="server" ID="hdfWFID" />
                <asp:HiddenField runat="server" ID="hdfBuyer" />
                <asp:HiddenField runat="server" ID="hdfSpecification" />
                <div class="row mt">
                    <div class="form-group">
                        <label class="col-sm-3">Notes:</label>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" Height="100" Width="300"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row mt">
                    <div class="form-group">
                        <label class="col-sm-3">Attachment:</label>
                        <div class="col-sm-8">
                            <asp:FileUpload runat="server" ID="fileAttachment" AllowMultiple="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button runat="server" ID="btnRelease" OnClick="btnRelease_Click" Text="Release" CssClass="btn btn-primary" />
            </div>

        </asp:Panel>
    </div>--%>
</asp:Content>
