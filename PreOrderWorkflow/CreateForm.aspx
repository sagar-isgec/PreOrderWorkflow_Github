<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="CreateForm.aspx.cs" Inherits="PreOrderWorkflow.CreateForm" %>

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
            margin-top: 10px;
        }

        .row {
            margin-right: -1px;
            margin-left: -1px;
        }
    </style>

  <%--  <script src="scripts/jquery-editable-select.min.js"></script>
    <link href="scripts/jquery-editable-select.min.css" rel="stylesheet" />

    <script>
        $(function () {
            $('#ContentPlaceHolder1_ddlProject')
                .editableSelect()
                .on('select.editable-select', function (e, li) {
                    $('#last-selected').html(
                        li.val() + '. ' + li.text()
                    );
                });
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div style="text-align:center;font-size:16px;font-weight:bold"> Create Technical Specification</div>
  <asp:UpdatePanel runat="server"><ContentTemplate>
            <asp:HiddenField runat="server" ID="hdfWFID" />
          <asp:HiddenField runat="server" ID="hdfHistoryID" />
            <asp:HiddenField runat="server" ID="hdfStatus" />
            <div class="col-lg-12">
                <div class="form-panel">
                    <div class="row mt">
                        <div class="form-group">
                            <label class="col-sm-2">Project:</label>
                            <div class="col-sm-3">
                                <asp:DropDownList runat="server" ID="ddlProject" CssClass="form-control" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-7">
                                <asp:Label runat="server" ID="lblProjectName"></asp:Label></div>
                        </div>
                    </div>
                    <div class="row mt">
                        <div class="form-group">
                            <label for="pwd" class="col-sm-2">Element :</label>
                            <div class="col-sm-3">
                                <asp:DropDownList runat="server" ID="ddlElement" CssClass="form-control" OnSelectedIndexChanged="ddlElement_SelectedIndexChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-7">
                                <asp:Label runat="server" ID="lblProjectElementName"></asp:Label></div>
                        </div>
                    </div>
                    <div class="row mt">
                        <div class="form-group">
                            <label for="pwd" class="col-sm-2">Specification No/Details:</label>
                            <div class="col-sm-3">
                                <%--<asp:DropDownList Visible="false" CssClass="form-control" runat="server" ID="ddlSpecification" OnSelectedIndexChanged="ddlSpecification_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                <asp:TextBox runat="server" ID="txtSpecification" CssClass="form-control" placeholder="Type Specification No. or Details"/>
                                <ajaxToolkit:AutoCompleteExtender ServiceMethod="GetSpecificationMethod" MinimumPrefixLength="11"
                                    CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtSpecification"
                                    ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListElementID="div">
                                </ajaxToolkit:AutoCompleteExtender>
                                <div id="div" style="overflow-y: scroll; overflow-x: hidden; max-height: 400px;"></div>
                            </div>
                            <div class="col-sm-7">
                                <asp:Label runat="server" ID="lblSpecDesc"></asp:Label></div>
                        </div>
                    </div>
                    <div class="row mt">
                        <div class="form-group">
                            <label for="pwd" class="col-sm-2">PMDL Document No:</label>
                          <%--  <div class="col-sm-6">
                               <asp:TextBox runat="server" ID="txtPMDLdoc" CssClass="form-control" placeholder="Type PMDL Document No. or Description"/>
                                <ajaxToolkit:AutoCompleteExtender ServiceMethod="GetPMDLdocMethod" MinimumPrefixLength="3"
                                    CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtPMDLdoc"
                                    ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListElementID="divPMDLdoc">
                                </ajaxToolkit:AutoCompleteExtender>
                                <div id="divPMDLdoc" style="overflow-y: scroll; overflow-x: hidden; max-height: 400px;"></div>
                            </div>
                           <%-- <div class="col-sm-4">
                                <asp:Label runat="server" ID="lblPMDLdoc"></asp:Label></div>--%>
                            <div class="col-sm-6">
                                <asp:ListBox runat="server" ID="ddlPMDL" SelectionMode="Multiple" Height="100" Width="100%" style="height:120" 
                                     AppendDataBoundItems="true"
                                    />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="lblPMDL"></asp:Label>
                                <asp:CheckBox runat="server" ID="chkbx1" Text="Show All Documents" OnCheckedChanged="ckhbx1_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                <br />
                                <asp:CheckBox runat="server" ID="chkbx2" Text="Show Only Released Documents From PMDL" OnCheckedChanged="ckhbx2_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                <br />
                                <asp:CheckBox runat="server" ID="chkbx3" Text="Show Only Unissued Documents" OnCheckedChanged="ckhbx3_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                        </div>
                    </div>
                        </div>



                    <div class="row mt">
                        <div class="form-group">
                            <label for="pwd" class="col-sm-2">Buyer:</label>
                            <div class="col-sm-3">
                                 <asp:DropDownList Visible="true" CssClass="form-control" runat="server" ID="ddlBuyer" OnSelectedIndexChanged="ddlBuyer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                               <%-- <asp:TextBox runat="server" ID="txtBuyer" CssClass="form-control" placeholder="Type name or emp id and select from the dropdown" />
                                <ajaxToolkit:AutoCompleteExtender ServiceMethod="GetUSer" MinimumPrefixLength="3"
                                    CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtBuyer"
                                    ID="AutoCompleteExtender3" runat="server" FirstRowSelected="false" CompletionListElementID="divBuyer">
                                </ajaxToolkit:AutoCompleteExtender>
                                <div id="divBuyer" style="overflow-y: scroll; overflow-x: hidden; max-height: 400px;"></div>--%>
                            </div>
                             <div class="col-sm-7">
                                <asp:Label runat="server" ID="lblBuyerName"></asp:Label></div>
                        </div>
                        </div>
                  
                    <div class="row mt">
                        <div class="form-group">
                            <label for="pwd" class="col-sm-2"> Project Manager:</label>
                            <div class="col-sm-3">
                                 <asp:DropDownList Visible="true" CssClass="form-control" runat="server" ID="ddlManager" OnSelectedIndexChanged="ddlManager_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                               <%-- <asp:TextBox runat="server" ID="txtBuyer" CssClass="form-control" placeholder="Type name or emp id and select from the dropdown" />
                                <ajaxToolkit:AutoCompleteExtender ServiceMethod="GetUSer" MinimumPrefixLength="3"
                                    CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtBuyer"
                                    ID="AutoCompleteExtender3" runat="server" FirstRowSelected="false" CompletionListElementID="divBuyer">
                                </ajaxToolkit:AutoCompleteExtender>
                                <div id="divBuyer" style="overflow-y: scroll; overflow-x: hidden; max-height: 400px;"></div>--%>
                            </div>
                             <div class="col-sm-7">
                                <asp:Label runat="server" ID="lblManagername"></asp:Label></div>
                        </div>
                    </div>
                      <div class="row mt">
                    <div class="form-group">
                        <label class="col-sm-2">Message:</label>
                        <div class="col-sm-6">
                            <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" Height="100"  CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

                       <div class="row mt">
                      <div class="form-group">
                        <label class="col-sm-2"></label>
                        <div class="col-sm-1">
                        <%--    <asp:FileUpload runat="server" ID="fileUpload" AllowMultiple="true" />   changed btnNotes visibility for testing....set its visibility to false after testing ---change sagar---      --%>
                                <asp:Button runat="server" ID="btnAttachment" CssClass="btn btn-info btn-sm" Text="Attachment" OnClick="btnAttachment_Click"/>
                        </div>
                         <div class="col-sm-2">
                        <asp:Button runat="server" ID="btnNotes" CssClass="btn btn-info btn-sm" Text="Notes" OnClick="btnNotes_Click" Visible="false" />
                    </div>
                        
                    </div>
                </div>
                    <div class="row mt">
                        <div class="form-group" style="margin-left:230px;margin-top:20px">
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Release" OnClick="btnRelease_Click" />
                        </div>
                    </div>

                    <div class="row mt" runat="server" id="divAlert" visible="false">
                        <div style="text-align:center;color:forestgreen;font-size:14px">Record Saved</div>
                    </div>

                </div>
            </div>

        
      </ContentTemplate>
      <Triggers>
          <asp:PostBackTrigger ControlID="btnAttachment" />
          <asp:PostBackTrigger ControlID="btnNotes" />

      </Triggers>
  </asp:UpdatePanel>

</asp:Content>
