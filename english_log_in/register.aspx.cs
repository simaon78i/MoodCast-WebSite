using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    private string connString = ConfigurationManager.ConnectionStrings["MoodCastDb"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        

    }
    public void btnRegister_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;
        string strName = name.Text;
        string userEmail = email.Text;

        if (password != confirmPassword)
        {
            lblErrorMessage.Text = "Password doesn't match please try again :)";
        }
        else
        {
            Session["username"] = username;
            Session["password"] = password;
            InsertUser(username, password, strName, userEmail);
            Response.Redirect("../HomePage.aspx");
        }
    }
    protected void InsertUser(string username, string password, string name, string email)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string quary = "INSERT INTO users (username, password, email, fullName) VALUES (@username, @password, @email, @fullName)";
            using (NpgsqlCommand cmd = new NpgsqlCommand(quary, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@fullName", name);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    lblErrorMessage.Text = "Error: " + ex.Message;
                    return;
                }
            }
        }

    }
    protected void bthomepage_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
    }
    protected void btnGoogleRegister_Click(object sender, EventArgs e)
    {
        RedirectToGoogleForRegistration();
    }

    private void RedirectToGoogleForRegistration()
    {
        string clientId = ConfigurationManager.AppSettings["googleClientId"];

        string redirectUri = "https://localhost:44308/oauth2callback.aspx";

        string scope = "email profile";
        string responseType = "code";
        string authUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
        "client_id=" + clientId +
        "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
        "&response_type=code" +
        "&scope=" + HttpUtility.UrlEncode(scope) +
        "&access_type=online" +
        "&prompt=select_account" +
        "&state=register";  



        
        Response.Redirect(authUrl);
    }



}