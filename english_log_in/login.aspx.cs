using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    private string connString = ConfigurationManager.ConnectionStrings["MoodCastDb"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;
        bool isAdmin = checkIfAdmin(username, password);
        bool isUser = checkIfUser(username, password);



    }
    protected void bthomepage_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
    }
    protected void btnGoogleLogin_Click(object sender, EventArgs e)
    {
        string clientId = ConfigurationManager.AppSettings["googleClientId"];
        string redirectUri = "https://localhost:44308/oauth2callback.aspx";

        string url =
            "https://accounts.google.com/o/oauth2/v2/auth" +
            "?client_id=" + clientId +
            "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
            "&response_type=code" +
            "&scope=openid%20email%20profile" +
            "&access_type=online";

        Response.Redirect(url);
    }

    protected bool checkIfAdmin(string username, string password)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            string query = "SELECT COUNT(*) FROM admin WHERE username = @username AND password = @password";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count == 0)
                {
                    lblErrorMessage.Text = "Invalid username or password. Please try again.";
                    return false;
                }
                else
                {
                    Session["username"] = username;
                    Session["password"] = password;
                    Session["isAdmin"] = true; // Set session variable to indicate admin login
                    Response.Redirect("../HomePage.aspx");
                }
            }
        }
        return true;

    }
    protected bool checkIfUser(string username, string password)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                {
                    lblErrorMessage.Text = "Invalid username or password. Please try again.";
                    return false;
                }
                else
                {
                    Session["username"] = username;
                    Session["password"] = password;
                    Response.Redirect("../HomePage.aspx");
                }
            }
        }
        return true;
    }

}