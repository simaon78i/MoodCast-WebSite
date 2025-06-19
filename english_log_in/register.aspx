<%@ Page Language="C#" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="Register" ResponseEncoding="utf-8"%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>MoodCast – Sign In</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="../style/registerStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <h1>Welcome to MoodCast</h1>
        <asp:TextBox ID="name" runat="server" CssClass="input-box" Placeholder="Name" />
        <asp:TextBox ID="email" runat="server" CssClass="input-box" Placeholder="Email" />
        <asp:TextBox ID="txtUsername" runat="server" CssClass="input-box" Placeholder="Username" />
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-box" Placeholder="Password" />
        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="input-box" Placeholder="Confirm Password" />
        <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" CssClass="btn-primary" OnClick="btnRegister_Click" />
        <br />
        <br />
        <asp:Button ID="homepage" runat="server" Text="back to home page" CssClass="btn-primary" OnClick="bthomepage_Click" />
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" />
        <br />
        <br />
        OR
        <br />
        <br />
        <asp:Button ID="btnGoogleRegister" runat="server" Text="Sign Up with Google" OnClick="btnGoogleRegister_Click" CssClass="btn-primary" />
        <div class="register-link" style="color:black;">
           allready have an account? <a href="login.aspx"> sign in here</a>
        </div>
    </form>
</body>
</html>
