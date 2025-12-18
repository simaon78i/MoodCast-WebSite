<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Verify.aspx.cs" Inherits="Verify" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>Verify Account | MoodCast</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="../style/verifyStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <h1>Verify Email</h1>
        <p class="subtitle">Enter the 6-digit code sent to your Gmail.</p>

        <asp:TextBox ID="txtCode" runat="server" CssClass="input-box" 
            placeholder="000000" MaxLength="6" autocomplete="off"></asp:TextBox>
        
        <asp:Button ID="btnVerify" runat="server" Text="Confirm Code" 
            CssClass="btn-primary" OnClick="btnVerify_Click" />

        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Text=""></asp:Label>

        <div class="resend-link">
            Didn't get a code? 
            <asp:LinkButton ID="btnResend" runat="server" OnClick="btnResend_Click">Resend Email</asp:LinkButton>
        </div>
    </form>
</body>
</html>