<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="Home_Page" Async="true" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>MoodCast</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="style/HomePageStyle.css" />
    <link rel="icon" type="image/x-icon" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/favicon_vuriu5.ico"/>
    <link rel="icon" type="image/png" sizes="192x192" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/android-chrome-192x192_aimsuf.png"/>
    <link rel="icon" type="image/png" sizes="512x512" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/android-chrome-512x512_ajdasc.png"/>
    <link rel="icon" type="image/png" sizes="32x32" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/favicon-32x32_xwynos.png"/>
    <link rel="icon" type="image/png" sizes="16x16" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/favicon-16x16_oyvtdb.png"/>
    <link rel="apple-touch-icon" sizes="180x180" href="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979534/apple-touch-icon_qivl3g.png"/>
    <link rel="manifest" href="site.json"/>
</head>
<body>
    <div id="loading-overlay">
    <div class="loader-content">
        <img src="https://res.cloudinary.com/ddja5g5wa/image/upload/v1765979535/android-chrome-512x512_ajdasc.png" class="pulse-logo" alt="MoodCast Logo" />
        
        <div class="spinner"></div>
        
        <h2 class="loading-title">MoodCast</h2>
        <p class="loading-subtitle">Finding the perfect movie for your mood...</p>
    </div>
</div>

<style>
    #loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: radial-gradient(circle, #1a1a1a 0%, #000000 100%);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 99999;
        color: #ffffff;
        font-family: 'Segoe UI', sans-serif;
        overflow: hidden;
    }

    .loader-content {
        position: relative;
        z-index: 10;
        text-align: center;
    }

    .pulse-logo {
        width: 150px;
        margin-bottom: 20px;
        animation: logo-pulse 2s infinite ease-in-out;
    }

    @keyframes logo-pulse {
        0%, 100% { transform: scale(1); opacity: 0.9; }
        50% { transform: scale(1.1); opacity: 1; }
    }

    .spinner {
        width: 40px;
        height: 40px;
        border: 3px solid rgba(255, 255, 255, 0.1);
        border-top: 3px solid #e74c3c;
        border-radius: 50%;
        margin: 0 auto 15px;
        animation: spin 1s linear infinite;
    }

    @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
    }

    .floating-poster {
        position: absolute;
        width: 110px;
        height: 160px;
        object-fit: cover;
        border-radius: 10px;
        box-shadow: 0 10px 25px rgba(0,0,0,0.7);
        opacity: 0;
        z-index: 1;
        pointer-events: none;
    }

    @keyframes flyIn {
        0% { transform: scale(0.4) translate(0, 0); opacity: 0; }
        15% { opacity: 0.6; }
        85% { opacity: 0.6; }
        100% { transform: scale(1.4) translate(var(--x), var(--y)); opacity: 0; }
    }

    .loading-title { font-size: 2.2rem; letter-spacing: 5px; margin: 10px 0; text-transform: uppercase; }
    .loading-subtitle { font-size: 1rem; color: #aaaaaa; font-style: italic; }
</style>

<script>
    /* Array of 4 poster URLs from Cloudinary */
    const posterUrls = [
        "https://res.cloudinary.com/ddja5g5wa/image/upload/v1766003766/action_b1qxmn.png",
        "https://res.cloudinary.com/ddja5g5wa/image/upload/v1766003765/comedy_vqb2qe.png",
        "https://res.cloudinary.com/ddja5g5wa/image/upload/v1766003765/romantic_hhbdvg.png",
        "https://res.cloudinary.com/ddja5g5wa/image/upload/v1766003765/drama_yj9tho.png"
    ];

    function createFloatingPosters() {
        const overlay = document.getElementById('loading-overlay');
        const posterCount = 12; // Increased to 12 for 4 posters

        for (let i = 0; i < posterCount; i++) {
            const img = document.createElement('img');
            img.src = posterUrls[i % posterUrls.length];
            img.className = 'floating-poster';

            const startTop = Math.random() * 100;
            const startLeft = Math.random() * 100;
            const moveX = (Math.random() - 0.5) * 800; 
            const moveY = (Math.random() - 0.5) * 800;

            img.style.top = startTop + '%';
            img.style.left = startLeft + '%';
            img.style.setProperty('--x', moveX + 'px');
            img.style.setProperty('--y', moveY + 'px');

            img.style.animation = `flyIn 6s infinite linear`;
            img.style.animationDelay = (Math.random() * 6) + 's';

            overlay.appendChild(img);
        }
    }

    window.addEventListener('load', function() {
        const loader = document.getElementById('loading-overlay');
        
        /* SessionStorage logic: Clear on app close, persist during navigation */
        if (sessionStorage.getItem('moodcast_init')) {
            if (loader) loader.remove();
        } else {
            createFloatingPosters();
            sessionStorage.setItem('moodcast_init', 'true');

            setTimeout(function() {
                if (loader) {
                    loader.style.transition = 'opacity 1.2s ease-out';
                    loader.style.opacity = '0';
                    setTimeout(() => loader.remove(), 1200);
                }
            }, 4500); 
        }
    });
</script>
    <form id="form1" runat="server">
        <nav>
            <a >MoodCast</a>
            <div style="color:white">
                <a id="login" runat="server" href="english_log_in/login.aspx">sign in</a>
                <a id="signup" runat="server" href="english_log_in/register.aspx">sign up</a>
                <a id="lblWelcome" runat="server" ></a>
                <asp:HyperLink ID="premiumLink" runat="server" NavigateUrl="~/stripPayment/stripCheckout.aspx" Text="Upgrade To Premium" Visible="false" />
                <asp:LinkButton ID="btnLogout" runat="server" OnClick="btnLogout_Click" Visible="false">Log Out</asp:LinkButton>
                <asp:HyperLink ID="adminLink" runat="server" NavigateUrl="~/admin.aspx" Text="Admin Panel" Visible="false" />


            </div>
        </nav>

        <main>
            <div class="mood-box">
                <h1>What is your mood today?</h1>
                <asp:TextBox ID="txtMood" runat="server" CssClass="input-box" placeholder="Describe your mood..." />
                <asp:Button ID="btnSubmit" runat="server" Text="Get Recommendations" CssClass="btn-primary" OnClick="btnSubmit_Click" />
                <asp:Literal ID="lblCounterMassage" runat="server"/>
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
