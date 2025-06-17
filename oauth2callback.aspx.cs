using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Npgsql;

public partial class oauth2callback : System.Web.UI.Page
{
    private string connString = ConfigurationManager.ConnectionStrings["MoodCastDb"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        string code = Request.QueryString["code"];
        string mode = Request.QueryString["mode"]; // register or login

        if (!string.IsNullOrEmpty(code))
        {
            string token = ExchangeCodeForToken(code);
            dynamic user = GetUserInfo(token);

            //check if user is null
            string email = user.email;
            string name = user.name;

            if (mode == "register")
            {
                if (!UserExists(email))
                {
                    Response.Write("<script>alert('Welcome');window.location='Home.aspx';</script>");

                    RegisterNewUser(email, name); // create new user in DB
                }
            }

            Session["email"] = email;
            Session["username"] = "Google User";

            Response.Redirect("~/HomePage.aspx");
            Response.Write("<script>alert('Welcome, " + name + "," + email + "!');window.location='Home.aspx';</script>");

        }
    }


    private string ExchangeCodeForToken(string code)
    {
        using (var client = new WebClient())
        {
            var values = new NameValueCollection();
            values["code"] = code;
            values["client_id"] = ConfigurationManager.AppSettings["googleClientId"];
            values["client_secret"] = ConfigurationManager.AppSettings["googleClientSecret"];
            values["redirect_uri"] = "https://localhost:44308/oauth2callback.aspx";
            values["grant_type"] = "authorization_code";

            byte[] response = client.UploadValues("https://oauth2.googleapis.com/token", "POST", values);
            string json = Encoding.UTF8.GetString(response);
            dynamic data = JsonConvert.DeserializeObject(json);
            return data.access_token;
        }
    }

    private dynamic GetUserInfo(string accessToken)
    {
        using (var client = new WebClient())
        {
            client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
            string json = client.DownloadString("https://www.googleapis.com/oauth2/v2/userinfo");
            return JsonConvert.DeserializeObject(json);
        }
    }

    private bool UserExists(string email)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            string query = "SELECT COUNT(*) FROM users WHERE email = @email";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }


    private void RegisterNewUser(string email, string name)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string quary = "INSERT INTO users (username,password, email, fullName) VALUES (@username,@password,@email, @fullName)";
            using (NpgsqlCommand cmd = new NpgsqlCommand(quary, conn))
            {
                cmd.Parameters.AddWithValue("@username", name);
                cmd.Parameters.AddWithValue("@password", "google"); // Set a default password or handle it as needed
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@fullName", name);
                try
                {
                    cmd.ExecuteNonQuery();
                    Session["username"] = name;
                    Session["password"] = "google"; // Set a default password or handle it as needed
                }
                catch (NpgsqlException ex)
                {
                    return;
                }
            }
        }
    }

}
