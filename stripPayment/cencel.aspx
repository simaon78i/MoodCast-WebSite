<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cencel.aspx.cs" Inherits="cancel" %>
<!DOCTYPE html>
<html>
<head>
    <title>Payment Cancelled</title>
</head>
<body>
    <h1>❌ Payment Cancelled</h1>
    <asp:Label ID="lblMessage" runat="server" Text="Your payment was cancelled."></asp:Label>
    <br /><br />
    <a href="stripe-checkout.aspx">Try Again</a>
</body>
</html>
