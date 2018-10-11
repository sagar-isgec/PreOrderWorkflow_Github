<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="RaiseEnquiryForm.aspx.cs" Inherits="PreOrderWorkflow.RaiseEnquiryForm" %>

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
            position: relative;
            min-height: 1px;
            float: left;
            width: 16.66666667%;
            left: -16px;
            top: 2px;
            padding-left: 15px;
            padding-right: 15px;
        }
        .auto-style2 {
            position: relative;
            min-height: 1px;
            float: left;
            width: 33.33333333%;
            left: -171px;
            top: 24px;
            padding-left: 15px;
            padding-right: 15px;
        }
    </style>
   <%-- <script type="text/javascript">
   
        function KeySelected(source, eventArgs) {

          var SupplierCode_Name;
          SupplierCode_Name = document.getElementById('<%=this.txtSupplier.ClientID%>').value
          var SupplierCode_Name_Len = SupplierCode_Name.lastIndexOf('-');
          SupplierCode_Name = SupplierCode_Name.substring(0, SupplierCode_Name_Len - 1);
          document.getElementById('<%=this.txtSupplier.ClientID%>').value = SupplierCode_Name;
          document.getElementById('<%=this.txtSupplier.ClientID%>').focus();     

      }
  
    </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="mb" runat="server" id="hHeader" style="text-align:center;font-size:16px;font-weight:bold"><i class="fa fa-angle-right"></i></div>
    <asp:HiddenField  runat="server" ID="hdfBuyerId"/> <asp:HiddenField  runat="server" ID="hdfRandomNo"/><asp:HiddenField  runat="server" ID="hdfParentWFID"/>

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
                        <asp:DropDownList runat="server" ID="ddlSupplier" CssClass="form-control" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged" AutoPostBack="true" />
                      <%--  <asp:TextBox runat="server" CssClass="form-control" ID="txtSupplier" placeholder="Type Supplier Name or Supplier Code"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender ServiceMethod="GetSupplier" MinimumPrefixLength="3"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtSupplier"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListElementID="divBuyer">
                        </ajaxToolkit:AutoCompleteExtender>
                        <div id="divBuyer" style="overflow-y: scroll; overflow-x: hidden; max-height: 400px;"></div>--%>
                    </div>
                </div>
            </div>
             <div class="row mt">
                        <div class="form-group">
                            <label for="pwd" class="col-sm-3">Supplier Name</label>
                            <div class="col-sm-8">
                                 <asp:TextBox runat="server" CssClass="form-control" ID="txtSupplier" ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Supplier Email</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSupplierEmail"></asp:TextBox>
                
                    </div>
                </div>
            </div>

            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3">Email Subject</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtEmailSub"></asp:TextBox>
                
                    </div>
                </div>
            </div>
            <div class="row mt" runat="server" id="trNotes">
                <div class="form-group">
                    <label class="col-sm-3">Message</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" CssClass="form-control" TextMode="MultiLine" ID="txtNotes" Height="90"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row mt">
                <div class="form-group">
                    <label class="col-sm-3"></label>
                    <div class="col-sm-8">
                           <asp:Button runat="server" ID="btnAttachment" CssClass="btn btn-info btn-sm" Text="Attachment" OnClick="btnAttachment_Click"/>
                          <asp:Button runat="server" ID="btnNotes" CssClass="btn btn-info btn-sm" Text="Notes" OnClick="btnNotes_Click"/>
                       <%-- <asp:FileUpload runat="server" ID="fileAttachment" AllowMultiple="true" />--%>
                    </div>
                </div>
            </div>

            <div class="row mt">
                <div class="form-group">
                    <label class="auto-style2"></label>
                    <div class="auto-style1">
                        <asp:Button runat="server" ID="btnSendEnquiry" Text="Send Enquiry" CssClass="btn btn-sm btn-primary" Height="33px" OnClick="btnSendEnquiry_Click" />
                         
                    </div>
                    <div="col-sm-2">
                        <asp:Button runat="server" ID="btnIdmsReceipt" Text="IDMS Receipt" CssClass="btn btn-sm btn-primary" Height="33px" Visible="false" OnClick="btnIdmsReceipt_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
