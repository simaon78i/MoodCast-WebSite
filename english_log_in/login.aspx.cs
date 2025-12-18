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
        if (checkIfAdmin(username, password))
        {
            Session["username"] = username;
            Session["password"] = password;
            Session["isAdmin"] = true; // Set session variable to indicate admin login
            Response.Redirect("../HomePage.aspx");
        }
        else if (checkIfUser(username, password))
        {
            Session["username"] = username;
            Session["password"] = HashPassword(password); // Store hashed password in session
            Response.Redirect("../HomePage.aspx");
        }
        else
        {
            lblErrorMessage.Text = "Invalid username or password. Please try again.";
        }
    }

    protected void bthomepage_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
    }

    protected void btnGoogleLogin_Click(object sender, EventArgs e)
    {
        string clientId = ConfigurationManager.AppSettings["googleClientId"];
        string redirectUri = "https://www.MoodCastApp.somee.com/oauth2callback.aspx";

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
            conn.Open();
            
            string query = "SELECT COUNT(*) FROM admin WHERE username = @username AND password = @password";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if(count <= 0) return false;
                string quary = "SELECT counter FROM users WHERE username = @username";
                using (NpgsqlCommand cmdCheck = new NpgsqlCommand(quary, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@username", username);
                    var result = cmdCheck.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int counter = Convert.ToInt32(result);
                        Session["counter"] = counter;
                    }
                }
                return true;
            }
        }
    }

    protected bool checkIfUser(string username, string password)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string query = "SELECT password FROM users WHERE username = @username";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);

                var result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return false;
                }
                string quaryV = "SELECT is_verified FROM users WHERE username = @username";
                using (NpgsqlCommand cmdCheck = new NpgsqlCommand(quaryV, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@username", username);
                    var resultV = cmdCheck.ExecuteScalar();
                    if (resultV == null || resultV == DBNull.Value || !(bool)resultV)
                    {
                        Response.Redirect("verify.aspx");
                        return false;
                    }
                }
                string storedHashedPassword = result.ToString();

                if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                {
                    string quary = "SELECT counter FROM users WHERE username = @username";
                    using (NpgsqlCommand cmdCheck = new NpgsqlCommand(quary, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@username", username);
                        var resultC = cmdCheck.ExecuteScalar();
                        if (resultC != null && resultC != DBNull.Value)
                        {
                            int counter = Convert.ToInt32(resultC);
                            Session["counter"] = counter;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}