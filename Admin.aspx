<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet"
      href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css"
      integrity="sha512-mz4..."
      crossorigin="anonymous" referrerpolicy="no-referrer" />

      <meta charset="utf-8" /> 
    <title>MoodCast Admin - Users</title>
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
                <asp:GridView ID="GridViewUsers"
              runat="server"
              AutoGenerateColumns="True"
              DataKeyNames="id"
              CssClass="gridview-style"
              OnRowEditing="GridViewUsers_RowEditing"
              OnRowCancelingEdit="GridViewUsers_RowCancelingEdit"
              OnRowUpdating="GridViewUsers_RowUpdating"
              OnRowDeleting="GridViewUsers_RowDeleting">

    <Columns>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server"
                                CommandName="Edit"
                                ToolTip="עריכה"
                                CssClass="action edit">
                    <i class="fa-solid fa-pen"></i>
                </asp:LinkButton>
                <asp:LinkButton ID="btnDelete" runat="server"
                                CommandName="Delete"
                                ToolTip="מחיקה"
                                OnClientClick="return confirm('למחוק משתמש זה?');"
                                CssClass="action delete">
                    <i class="fa-solid fa-trash"></i>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>


                <br />
                <h2>Admin Panel - User Management</h2>
                <asp:GridView ID="GridViewAdmin" runat="server" AutoGenerateColumns="true"
                EmptyDataText="No users found."
                CssClass="gridview-style" />
            </div>
        </main>

        <footer>
            creator : Shimon Ifrach &copy; 2025 MoodCast. All rights reserved.
            <a href="mailto:simaon78ifrac@gmail.com?subject=i%20have%20a%20question%20about%20MoodCast"
               style="color: #60a5fa;"> contact me</a>
        </footer>
    </form>
</body>
</html>
