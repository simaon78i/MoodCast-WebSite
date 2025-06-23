<%@ Page Language="C#" AutoEventWireup="true" CodeFile="success.aspx.cs" Inherits="success" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Successful</title>
    <link rel="stylesheet" href="../style/successStyle.css"/>
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
