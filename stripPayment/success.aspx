<%@ Page Language="C#" AutoEventWireup="true" CodeFile="success.aspx.cs" Inherits="success" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Successful</title>
    <link rel="stylesheet" href="../style/successStyle.css"/>
    <link rel="icon" type="image/x-icon" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/favicon_vuriu5.ico">
    <link rel="icon" type="image/png" sizes="192x192" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/android-chrome-192x192_aimsuf.png">
    <link rel="icon" type="image/png" sizes="512x512" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/android-chrome-512x512_ajdasc.png">
    <link rel="icon" type="image/png" sizes="32x32" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/favicon-32x32_xwynos.png">
    <link rel="icon" type="image/png" sizes="16x16" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/favicon-16x16_oyvtdb.png">
    <link rel="apple-touch-icon" sizes="180x180" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/apple-touch-icon_qivl3g.png">
    <link rel="manifest" href="../site.json">
</head>
<body>
    <form id="form1" runat="server">
        <div class="pay-box">
            <h1 style="margin-bottom:20px;">🎉 Payment Successful</h1>

            <asp:Label ID="lblMessage"
                       runat="server"
                       Text="Thank you for your payment!"
                       Style="font-size:1.1rem;font-weight:bold;" />

            <br /><br />

            <a href="../HomePage.aspx" class="btn-home">Return to Homepage</a>
        </div>
    </form>
</body>
</html>
