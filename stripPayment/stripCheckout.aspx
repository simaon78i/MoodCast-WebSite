<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stripCheckout.aspx.cs" Inherits="stripe_checkout" %>

<!DOCTYPE html>
<html>
<head>
    <title>Stripe Checkout Demo</title>
    <link rel="stylesheet" href="../style/stripStyle.css" />
    <script src="https://js.stripe.com/v3/"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="pay-box">
            <asp:Button runat="server" ID="btnPay" Text="Continue to Payment" OnClick="btnPay_Click" cssClass="btn-stripe"/>
        </div>
    </form>
</body>
</html>
