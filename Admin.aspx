<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <meta charset="utf-8" /> 
    <title>MoodCast Admin - Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="style/adminStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <nav>
            <a href="HomePage.aspx">← Back to Home</a>
        </nav>

        <main>
            <div class="admin-box">
                <h2>All Registered Users</h2>
                <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="False" 
                    CssClass="gridview-style" DataKeyNames="id"
                    OnRowEditing="GridViewUsers_RowEditing" 
                    OnRowCancelingEdit="GridViewUsers_RowCancelingEdit" 
                    OnRowUpdating="GridViewUsers_RowUpdating" 
                    OnRowDeleting="GridViewUsers_RowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="action edit"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="return confirm('Delete user?');" CssClass="action delete"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="action update"><i class="fa-solid fa-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="action cancel"><i class="fa-solid fa-xmark"></i></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="username" HeaderText="Username" />
                        <asp:BoundField DataField="email" HeaderText="Email" />
                        <asp:BoundField DataField="fullname" HeaderText="Full Name" />
                        
                        <%-- העמודות החדשות --%>
                        <asp:CheckBoxField DataField="is_verified" HeaderText="Verified" />
                        <asp:BoundField DataField="counter" HeaderText="Tries Left" />
                        
                        <asp:BoundField DataField="created_at" HeaderText="Created" ReadOnly="True" />
                    </Columns>
                </asp:GridView>

                <br />
                <h2>Verification Codes (Temp)</h2>
                <asp:GridView ID="GridViewVerifications" runat="server" AutoGenerateColumns="True" CssClass="gridview-style" EmptyDataText="No active codes." />

                <br />
                <h2>Admin Accounts</h2>
                <asp:GridView ID="GridViewAdmin" runat="server" AutoGenerateColumns="True" CssClass="gridview-style" />
            </div>
        </main>
    </form>
</body>
</html>