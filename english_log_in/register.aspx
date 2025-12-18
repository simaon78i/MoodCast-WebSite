<%@ Page Language="C#" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="Register" ResponseEncoding="utf-8"%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>MoodCast – Sign In</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="../style/registerStyle.css" />
    <link rel="icon" type="image/x-icon" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/favicon_vuriu5.ico">
    <link rel="icon" type="image/png" sizes="192x192" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/android-chrome-192x192_aimsuf.png">
    <link rel="icon" type="image/png" sizes="512x512" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/android-chrome-512x512_ajdasc.png">
    <link rel="icon" type="image/png" sizes="32x32" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/favicon-32x32_xwynos.png">
    <link rel="icon" type="image/png" sizes="16x16" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/favicon-16x16_oyvtdb.png">
    <link rel="apple-touch-icon" sizes="180x180" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/apple-touch-icon_qivl3g.png">
    <link rel="manifest" href="../site.json">
<style>
center {
    position: fixed !important;
    bottom: 0px !important;
    right: 0px !important;
    z-index: 99999 !important;
    text-align: right !important;
    background-color: transparent !important;
    padding: 5px !important;
    left: auto !important;
    top: auto !important;
    margin: 0 !important;
}
</style>
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
