<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="Home_Page" Async="true"  %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MoodCast</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        /* איפוס */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        html, body {
            height: 100%;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: url('assets/bgam.png') no-repeat center center fixed;
            background-size: cover;
            color: #000;
            overflow-x: hidden;
        }

        body {
            padding-top: 80px;
            background-color: #f0f2f5;
        }

        nav {
            background: #1e293b;
            padding: 1rem 2rem;
            position: fixed;
            top: 0;
            width: 100%;
            z-index: 10;
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            color: white;
        }

        nav > a {
            color: white;
            font-weight: bold;
            font-size: 1.8rem;
            text-decoration: none;
        }

        nav div {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            margin-top: 0.5rem;
        }

        nav a {
            color: #cbd5e1;
            text-decoration: none;
            font-size: 1.5rem;
        }

        nav a:hover {
            color: #3b82f6;
        }

        main {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            color: black;
        }

        .mood-box {
            background: white;
            border-radius: 20px;
            padding: 30px;
            box-shadow: 0 0 30px rgba(0,0,0,0.1);
            margin-bottom: 40px;
            text-align: center;
        }

        .mood-box h1 {
            font-size: 2rem;
            margin-bottom: 20px;
        }

        .input-box {
            width: 100%;
            max-width: 600px;
            padding: 15px 20px;
            font-size: 1.2rem;
            border: 2px solid #2563eb;
            border-radius: 12px;
            transition: all 0.3s ease;
            color: #333;
        }

        .input-box:focus {
            outline: none;
            border-color: #3b82f6;
            box-shadow: 0 0 8px rgba(59, 130, 246, 0.7);
            background: #f9fafb;
        }

        .btn-primary {
            margin-top: 20px;
            width: 100%;
            max-width: 600px;
            padding: 16px;
            font-size: 1.3rem;
            font-weight: 700;
            background: linear-gradient(90deg, #3b82f6, #2563eb);
            border: none;
            border-radius: 12px;
            color: white;
            cursor: pointer;
            transition: background 0.3s ease, transform 0.2s ease;
        }

        .btn-primary:hover {
            background: linear-gradient(90deg, #2563eb, #1d4ed8);
            transform: translateY(-3px);
        }

        /* עיצוב רשימת ההמלצות */
        .recommendations-row {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 25px;
            margin-top: 30px;
        }

        .movie-card {
            background: white;
            border-radius: 15px;
            overflow: hidden;
            box-shadow: 0 8px 20px rgba(0,0,0,0.1);
            display: flex;
            flex-direction: column;
            transition: transform 0.3s ease;
        }

        .movie-card:hover {
            transform: scale(1.05);
            box-shadow: 0 12px 30px rgba(0,0,0,0.15);
        }

        .movie-card img {
            width: 100%;
            height: 400px;
            object-fit: cover;
            border-bottom: 1px solid #ddd;
        }

        .card-body {
            padding: 20px;
            flex-grow: 1;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }

        .card-title {
            font-size: 1.4rem;
            font-weight: 700;
            margin-bottom: 10px;
            color: #1e293b;
        }

        .card-text {
            font-size: 1rem;
            color: #475569;
            margin-bottom: 15px;
            flex-grow: 1;
        }

        iframe {
            width: 100%;
            height: 230px;
            border: none;
            border-radius: 0 0 15px 15px;
        }

        /* רספונסיביות */
        @media (max-width: 1024px) {
            .recommendations-row {
                grid-template-columns: repeat(2, 1fr);
            }

            .movie-card img {
                height: 300px;
            }
        }

        @media (max-width: 600px) {
            .recommendations-row {
                grid-template-columns: 1fr;
            }

            .movie-card img {
                height: 250px;
            }
        }
        footer {
            position: fixed;
            bottom: 0;
            left: 0;
            width: 100%;
            background-color: #1e293b;
            color: white;
            padding: 10px 0;
        }
    </style>
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
        <a href="mailto:simaon78ifrac@gmail.com?subject=i%20have%20a%20question%20about%20the%20webSite%20MoodCast">contact me</a>
</footer>
</body>
</html>
