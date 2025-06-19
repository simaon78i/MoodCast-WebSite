<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Login" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>MoodCast – Sign In</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="../style/loginStyle.css" /> 
</head>
<body>
    <form id="form1" runat="server">
        <h1>Welcome back to MoodCast</h1>
        <asp:TextBox ID="txtUsername" runat="server" CssClass="input-box" Placeholder="Username" />
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-box" Placeholder="Password" />
        <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn-primary" OnClick="btnLogin_Click" />
        <br />
        <br />
        <asp:Button ID="homepage" runat="server" Text="back to home page" CssClass="btn-primary" OnClick="bthomepage_Click" />
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" />
        <br />
        <br />
        OR
        <br />
        <br />
        <asp:Button ID="btnGoogleLogin" runat="server" Text="sign in with Google" CssClass="btn-primary" OnClick="btnGoogleLogin_Click" />
        <div class="register-link">
           Don't have an account yet? <a href="register.aspx"> sign up here</a>
        </div>
    </form>
</body>
</html>
