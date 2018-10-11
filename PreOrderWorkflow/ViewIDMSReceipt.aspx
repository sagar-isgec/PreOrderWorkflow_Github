<%@ Page Title="" Language="C#" MasterPageFile="~/mstInner.Master" AutoEventWireup="true" CodeBehind="ViewIDMSReceipt.aspx.cs" Inherits="PreOrderWorkflow.ViewIDMSReceipt" EnableEventValidation="false"  %>
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
             width: 175px;
         }
         .auto-style3 {
             width: 189px;
         }
         .auto-style4 {
             width: 338px;
         }
         
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div style="text-align:center;font-size:16px;font-weight:bold">IDMS Receipt Detail</div>
       <br />
    <br />
     <div style="text-align:center; font-size:12px">
    <asp:Label ForeColor="Red" Font-Bold="true" Font-Size="Medium" runat="server" ID="lblNoRecord">
        
      </asp:Label>
         </div>
    <div  style="width:98%;overflow-x:auto;margin:auto;">
            <asp:GridView runat="server" ID="gvData"  AutoGenerateColumns="false" Width="100%" CssClass="table table-bordered table-hover" HeaderStyle-BackColor="#e9e9e9" Visible="true" AllowPaging="True" PageSize="15" OnPageIndexChanging="gvData_PageIndexChanging" Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" RowDataBound="gvData_RowDataBound" >
                <Columns>
                    <asp:TemplateField HeaderText="Receipt No.">
                        <ItemTemplate>
                             <%#Eval("t_rcno") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Revision No.">
                        <ItemTemplate>
                             <%#Eval("t_revn") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Receipt creation Date">
                        <ItemTemplate>
                            <%#Eval("receiptCreationDate") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Project ID">
                        <ItemTemplate>
                            <%#Eval("t_cprj") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Vendor Name">
                        <ItemTemplate>
                            <%#Eval("VendorName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Requisition No.">
                        <ItemTemplate>
                            <%#Eval("t_rqno") %>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Requisition Line">
                        <ItemTemplate>
                            <%#Eval("t_rqln") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <%-- <asp:TemplateField HeaderText="Requisition Date">
                        <ItemTemplate>
                            <%#Eval("t_rdat") %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="Linked Lot Item">
                        <ItemTemplate>
                            <%#Eval("t_item") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Lot item description">
                        <ItemTemplate>
                            <%#Eval("t_dsca") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                  
                     <asp:TemplateField HeaderText="Receipt Status(Current)">
                        <ItemTemplate>
                            <%#Eval("Doc_Status") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Receipt Status updated on">
                        <ItemTemplate>
                            <%#Eval("approvedate") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Receipt Status Maintained by(ID)">
                        <ItemTemplate>
                            <%#Eval("MaintainedById") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Receipt Status Maintained by(Name)">
                        <ItemTemplate>
                            <%#Eval("Name") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mechanical Response Received">
                        <ItemTemplate>
                            <%#Eval("mrr") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mechanical Technical Clearance">
                        <ItemTemplate>
                            <%#Eval("mtc") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Evaluator submission Date (Mech.)">
                        <ItemTemplate>
                            <%#Convert.ToString(Eval("mech_evaldate")).Equals("01-01-1900 00:00:00")?"":Eval("mech_evaldate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Evaluator Name (Mech.)">
                        <ItemTemplate>
                            <%#Eval("mech_evalname") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Structure Response Received">
                        <ItemTemplate>
                            <%#Eval("strr") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Structure Technical Clearance">
                        <ItemTemplate>
                            <%#Eval("sttc") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Evaluator submission Date (structure.)">
                        <ItemTemplate>
                          <%#Convert.ToString(Eval("struct_evaldate")).Equals("01-01-1900 00:00:00")?"":Eval("struct_evaldate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Evaluator Name (structure.)">
                        <ItemTemplate>
                            <%#Eval("struct_evalname") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Piping Response Received">
                        <ItemTemplate>
                            <%#Eval("Pipe_rr") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Piping Technical Clearance">
                        <ItemTemplate>
                            <%#Eval("Pipe_tc") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Evaluator submission Date (Piping.)">
                        <ItemTemplate>
                             <%#Convert.ToString(Eval("Pipe_evaldate")).Equals("01-01-1900 00:00:00")?"":Eval("Pipe_evaldate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Evaluator Name (Piping.)">
                        <ItemTemplate>
                            <%#Eval("Pipe_evalname") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Process Response Received">
                        <ItemTemplate>
                            <%#Eval("Process_rr") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Process Technical Clearance">
                        <ItemTemplate>
                            <%#Eval("Process_tc") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Evaluator submission Date (Process.)">
                        <ItemTemplate>
                             <%#Convert.ToString(Eval("Process_evaldate")).Equals("01-01-1900 00:00:00")?"":Eval("Process_evaldate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Evaluator Name (Process.)">
                        <ItemTemplate>
                            <%#Eval("Process_evalname") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="CI Response Received">
                        <ItemTemplate>
                            <%#Eval("CI_rr") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CI Technical Clearance">
                        <ItemTemplate>
                            <%#Eval("CI_tc") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Evaluator submission Date (C&I)">
                        <ItemTemplate>
                             <%#Convert.ToString(Eval("CI_evaldate")).Equals("01-01-1900 00:00:00")?"":Eval("CI_evaldate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Evaluator Name (C&I)">
                        <ItemTemplate>
                            <%#Eval("CI_evalname") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Electrical Response Received">
                        <ItemTemplate>
                            <%#Eval("Electrical_rr") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Electrical Technical Clearance">
                        <ItemTemplate>
                            <%#Eval("Electrical_tc") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Evaluator submission Date (Elec.)">
                        <ItemTemplate>
                            <%#Convert.ToString(Eval("Electrical_evaldate")).Equals("01-01-1900 00:00:00")?"":Eval("Electrical_evaldate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Evaluator Name (Elec.)">
                        <ItemTemplate>
                            <%#Eval("Electrical_evalname") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Quality Response Received">
                        <ItemTemplate>
                            <%#Eval("Quality_rr") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quality Technical Clearance">
                        <ItemTemplate>
                            <%#Eval("Quality_tc") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Evaluator submission Date (Quality.)">
                        <ItemTemplate>
                             <%#Convert.ToString(Eval("Quality_evaldate")).Equals("01-01-1900 00:00:00")?"":Eval("Quality_evaldate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Evaluator Name (Quality)">
                        <ItemTemplate>
                            <%#Eval("Quality_evalname") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>

<HeaderStyle BackColor="#E9E9E9"></HeaderStyle>
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />

            </asp:GridView>
        </div>
     
</asp:Content>
