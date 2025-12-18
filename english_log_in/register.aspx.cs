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
        if (!IsPostBack)
        {
            if (Session["massage"] != null)
            {
                lblErrorMessage.Text = Session["massage"].ToString();
                Session["massage"] = null; // Clear the session message after displaying it
            }
        }
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
            return;
        }
        if (!CheckPassword(password))
        {
            return;
        }
        else if (!userEmail.EndsWith("@gmail.com") && !userEmail.EndsWith("@mail.huji.ac.il"))
            {
                lblErrorMessage.Text = "Please use a valid email like example@gmail.com";
                return;
            }
            else
            {
                Session["temp_username"] = username;
                Session["password"] = password;
                Session["UnverifiedEmail"] = userEmail;

                if (InsertUser(username, password, strName, userEmail))
                {
                    Response.Redirect("verify.aspx");
                }
            }
    }

    protected bool InsertUser(string username, string password, string name, string email)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            try
            {
                conn.Open();
                string quary = "INSERT INTO users (username, password, email, fullName,is_verified) VALUES (@username, @password, @email, @fullName,@is_verified) RETURNING id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(quary, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", HashPassword(password));
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@fullName", name);
                    cmd.Parameters.AddWithValue("@is_verified", false);

                    object result = cmd.ExecuteScalar();
                    if (result != null) {
                        int userId = Convert.ToInt32(result);
                        Session["UnverifiedUserId"] = userId;

                    }
                    return true;
                }
            }
            catch (NpgsqlException ex)
            {
                lblErrorMessage.Text = "Error: " + ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error: " + ex.Message;
                return false;
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

        string redirectUri = "https://www.MoodCastApp.somee.com/oauth2callback.aspx";

        string scope = "email profile";
        string responseType = "code";
        string authUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
        "client_id=" + clientId +
        "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
        "&response_type=" + responseType +
        "&scope=" + HttpUtility.UrlEncode(scope) +
        "&access_type=online" +
        "&prompt=select_account" +
        "&state=register";

        Response.Redirect(authUrl);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    private bool CheckPassword(string password)
    {
        if (password.Length < 8)
        {
            lblErrorMessage.Text = "Password must be at least 8 characters long.";
            return false;
        }
        else if (password.All(char.IsLetter))
        {
            lblErrorMessage.Text = "Password must contain at least one number or special character.";
            return false;
        }
        return true;
        
    }
}