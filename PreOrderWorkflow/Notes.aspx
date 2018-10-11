<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" Inherits="PreOrderWorkflow.Notes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <div style="float: left; width: 50px">Notes</div>
     <div style="background-color: #cccaca; width: 800px; min-height: 100px; margin-top: 30px; font-weight: bold; overflow: hidden; font-size: 12px">
           
                <asp:Repeater runat="server" ID="rptNotes">
                    <ItemTemplate>

                        <table>
                            <tr id="row" runat="server">
                                <td style="width: 400px">
                                    <asp:CheckBox ID="Checkbox1" runat="server"/>
                                    <asp:Label ID="x" runat="server" Text='<%#Eval("Description").ToString().Length>80 ? Eval("Description").ToString().Substring(0,80):Eval("Title").ToString() %>' ></asp:Label>

                                </td>
                                <td style="width: 200px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<%# Convert.ToDateTime(Eval("Created_Date")).ToString("dd-MM-yyyy HH:mm ")%></td>
                                <td style="width: 200px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<%#Eval("EmployeeName")%></td>
                                <td> <asp:Label  ID="sNoteId" runat="server" Visible="false" Text='<%#Eval("NotesId").ToString() %>'></asp:Label></td>
                               <%-- <td>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <asp:HiddenField runat="server" ID="hdfNoteId"<%#Eval("NotesId") %> /></td>--%>
                            </tr>
                        </table>
                       
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        <br/>
        <asp:Button ID="btnAttach" CssClass="button" Text="Attach Offer" runat="server"  BackColor="#53a9ff" ForeColor="White" OnClick="btnAttach_Click" />
    </form>
</body>
</html>
