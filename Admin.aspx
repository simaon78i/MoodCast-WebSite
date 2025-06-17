<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MoodCast Admin - Users</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        html, body {
            height: 100%;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: url('assets/bgp.png') no-repeat center center fixed;
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
            justify-content: flex-start;
            align-items: center;
            color: white;
        }

        nav a {
            color: white;
            font-weight: bold;
            font-size: 1.5rem;
            text-decoration: none;
        }

        nav a:hover {
            color: #3b82f6;
        }

        main {
            max-width: 1200px;
            margin: 0 auto;
            padding: 30px;
        }

        h2 {
            font-size: 2rem;
            margin-bottom: 20px;
            color: #1e293b;
            text-align: center;
        }

        .admin-box {
            background: rgba(255, 255, 255, 0.7);
            border-radius: 20px;
            padding: 30px;
            box-shadow: 0 0 30px rgba(0,0,0,0.1);
        }

        .gridview-style {
            width: 100%;
            border-collapse: collapse;
        }

        .gridview-style th, .gridview-style td {
            padding: 12px;
            border: 1px solid #ccc;
            text-align: left;
        }

        .gridview-style th {
            background-color: #2563eb;
            color: white;
        }

        .gridview-style tr:nth-child(even) {
            background-color: #f9fafb;
        }

        footer {
            position: fixed;
            bottom: 0;
            left: 0;
            width: 100%;
            background-color: #1e293b;
            color: white;
            padding: 10px 0;
            text-align: center;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav>
            <a href="HomePage.aspx">← Back to Home</a>
        </nav>

        <main>
            <div class="admin-box">
                <h2>All Registered Users</h2>
                <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="true"
                    EmptyDataText="No users found."
                    CssClass="gridview-style" />
                <br />
                <h2>Admin Panel - User Management</h2>
                <asp:GridView ID="GridViewAdmin" runat="server" AutoGenerateColumns="true"
                EmptyDataText="No users found."
                CssClass="gridview-style" />
            </div>
        </main>

        <footer>
            creator : Shimon Ifrach &copy; 2025 MoodCast. All rights reserved.
            <a href="mailto:simaon78ifrac@gmail.com?subject=i%20have%20a%20question%20about%20MoodCast"
               style="color: #60a5fa;"> contact me</a>
        </footer>
    </form>
</body>
</html>
