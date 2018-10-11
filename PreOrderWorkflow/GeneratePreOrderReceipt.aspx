<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="GeneratePreOrderReceipt.aspx.cs" Inherits="PreOrderWorkflow.GeneratePreOrderReceipt" %>

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
            margin-top: 12px;
        }

        .row {
            margin-right: -1px;
            margin-left: -1px;
        }
        .auto-style1 {
            margin-bottom: 15px;
            width: 668px;
            margin-left: 205px;
        }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >

      <div class="mb" runat="server" id="hHeader" style="text-align:center;font-size:16px;font-weight:bold">Generate IDMS Receipt</div>
    <asp:HiddenField  runat="server" ID="hdfBuyerId"/> <asp:HiddenField  runat="server" ID="hdfRandomNo"/>

    <div class="col-lg-10">
        <div class="form-panel">
          
            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Project</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtProject" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Element</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtElement" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Specification No/Details</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSpecification" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">PMDL Doc No</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPMDLDoc" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>


            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Buyer</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtBuyer" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
             <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Project Manager</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtManager" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
               <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Supplier Code</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSupplierCode" Enabled="false" ></asp:TextBox>
                       <%-- <ajaxToolkit:AutoCompleteExtender ServiceMethod="GetSupplier" MinimumPrefixLength="3"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtSupplier"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListElementID="divBuyer">
                        </ajaxToolkit:AutoCompleteExtender>
                        <div id="divBuyer" style="overflow-y: scroll; overflow-x: hidden; max-height: 400px;"></div>--%>
                    </div>
                </div>
            </div>

            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Supplier Name</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSupplier" Enabled="false" ></asp:TextBox>
                       <%-- <ajaxToolkit:AutoCompleteExtender ServiceMethod="GetSupplier" MinimumPrefixLength="3"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtSupplier"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListElementID="divBuyer">
                        </ajaxToolkit:AutoCompleteExtender>
                        <div id="divBuyer" style="overflow-y: scroll; overflow-x: hidden; max-height: 400px;"></div>--%>
                    </div>
                </div>
            </div>

                <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Supplier Email</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSupplierEmail" Enabled="false"></asp:TextBox>
                
                    </div>
                </div>
            </div>

            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Item</label>
                    <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlItem" CssClass="form-control"  AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChange" />
                            </div>
                            <%--<div class="col-sm-6">
                                <asp:Label runat="server" ID="lblItem"></asp:Label></div>--%>
                </div>
            </div>
<div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3"> Indent No-Line No</label>
                          <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlIndent" CssClass="form-control"  AutoPostBack="true" OnSelectedIndexChanged="ddlIndent_SelectedIndexChange"/>
                            </div>
                            <%--<div class="col-sm-6">
                                <asp:Label runat="server" ID="lblLine"></asp:Label></div>--%>
                    </div>
                </div>
            <div class="row mt" style="margin-top:20px">
                <div class="form-group">
                    <div class="col-sm-12">
                           
                           <asp:Button runat="server" ID="btnAttachment" CssClass="btn btn-info btn-sm" Text="Attach Offer" OnClick="btnAttachment_Click"  Height="33px" style="margin-left:300px" Width="157px"/>
                       <%-- <asp:FileUpload runat="server" ID="fileAttachment" AllowMultiple="true" />--%>
                        <asp:Button runat="server" ID="btnIdmsReceipt" Text="Generate IDMS Receipt" CssClass="btn btn-sm btn-primary" Height="33px" Visible="true" OnClick="btnIdmsReceipt_Click" style="margin-left:50px" Width="157px" />
                    </div>
            </div>
                    </div>
                 <div class="row mt">
                <div class="auto-style1" >
                    <div="col-sm-12" style="align-content:center" >
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       <asp:Label ID="lblNotify" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red" ></asp:Label>
                    </div>
                </div>
            </div>
            </div>
    </div>

</asp:Content>
