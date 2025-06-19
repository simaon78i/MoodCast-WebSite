<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="Home_Page" Async="true" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>MoodCast</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="style/HomePageStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <nav>
            <a >MoodCast</a>
            <div style="color:white">
                <a id="login" runat="server" href="english_log_in/login.aspx">sign in</a>
                <a id="signup" runat="server" href="english_log_in/register.aspx">sign up</a>
                <a id="lblWelcome" runat="server" ></a>
                <asp:LinkButton ID="btnLogout" runat="server" OnClick="btnLogout_Click" Visible="false">Log Out</asp:LinkButton>
                <asp:HyperLink ID="adminLink" runat="server" NavigateUrl="~/admin.aspx" Text="Admin Panel" Visible="false" />


            </div>
        </nav>

        <main>
            <div class="mood-box">
                <h1>What is your mood today?</h1>
                <asp:TextBox ID="txtMood" runat="server" CssClass="input-box" placeholder="Describe your mood..." />
                <asp:Button ID="btnSubmit" runat="server" Text="Get Recommendations" CssClass="btn-primary" OnClick="btnSubmit_Click" />
                
                <asp:Literal ID="litRecommendations" runat="server" />
            </div>
        </main>
    </form>
    <footer style="text-align:center; padding: 10px; font-size: 14px; color:white;">
       creator : Shimon Ifrach &copy; 2025 MoodCast. All rights reserved.
    <a href="https://mail.google.com/mail/?view=cm&fs=1&to=simaon78ifrac@gmail.com&su=I%20have%20a%20question%20about%20the%20MoodCast%20website"
        target="_blank" rel="noopener noreferrer">
        Contact me
    </a>
    </footer>
</body>
</html>
