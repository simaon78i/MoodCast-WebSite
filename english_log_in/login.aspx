<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MoodCast – Sign In</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        /* Reset & Base Styles */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: url('../assets/bga.png') no-repeat center center fixed;
            background-size: cover;            
            color: #fff;
            height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
        }

        /* Container with glass effect */
        form {
            background: rgba(255, 255, 255, 0.7);
            border: 1px solid rgba(255, 255, 255, 0.3);
            backdrop-filter: blur(16px);
            -webkit-backdrop-filter: blur(16px);
            border-radius: 20px;
            padding: 40px;
            width: 90%;
            max-width: 400px;
            box-shadow: 0 0 40px rgba(0, 0, 0, 0.3);
            animation: fadeIn 1s ease-in-out;
            text-align: center;
            color:black;
        }

        h1 {
            font-size: 2rem;
            font-weight: 800;
            text-align: center;
            margin-bottom: 30px;
            color: black;
        }

        .input-box {
            width: 100%;
            padding: 14px 18px;
            margin-bottom: 20px;
            background: rgba(255, 255, 255, 0.1);
            border: 1px solid #3b82f6;
            border-radius: 12px;
            color: black;
            font-size: 1rem;
            transition: all 0.3s ease-in-out;
        }

        .input-box:focus {
            outline: none;
            border-color: #60a5fa;
            background: rgba(255, 255, 255, 0.15);
        }

        .btn-primary {
            width: 100%;
            padding: 14px;
            background: linear-gradient(to right, #3b82f6, #2563eb);
            border: none;
            border-radius: 12px;
            color: #fff;
            font-weight: 700;
            font-size: 1rem;
            cursor: pointer;
            transition: transform 0.2s, background 0.3s;
        }

        .btn-primary:hover {
            transform: translateY(-2px);
            background: linear-gradient(to right, #2563eb, #1d4ed8);
        }

        .error-message {
            text-align: center;
            color: red;
            font-weight: 500;
            margin-top: 12px;
        }

        .register-link {
            margin-top: 25px;
            text-align: center;
            font-size: 0.95rem;
            color: black;
        }

        .register-link a {
            color: #60a5fa;
            text-decoration: none;
            font-weight: 500;
        }

        .register-link a:hover {
            text-decoration: underline;
        }

        @keyframes fadeIn {
            0% {
                opacity: 0;
                transform: scale(0.95);
            }
            100% {
                opacity: 1;
                transform: scale(1);
            }
        }

        @media (max-width: 500px) {
            form {
                padding: 30px 20px;
                border-radius: 16px;
            }
        }
    </style>
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
