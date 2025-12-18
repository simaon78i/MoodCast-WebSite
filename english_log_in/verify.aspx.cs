using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Npgsql;

public partial class Verify : System.Web.UI.Page
{
    private string connString = ConfigurationManager.ConnectionStrings["MoodCastDb"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UnverifiedUserId"] == null)
        {
            Response.Redirect("register.aspx");
            return;
        }

        if (!IsPostBack)
        {
            SendAndSaveCode();
        }
    }

    protected void btnVerify_Click(object sender, EventArgs e)
    {
        string inputCode = txtCode.Text.Trim();
        int userId = Convert.ToInt32(Session["UnverifiedUserId"]);

        if (ValidateCode(userId, inputCode))
        {
            MarkUserAsVerified(userId);

            Session["UserName"] = Session["temp_username"];
            Session["counter"] = 5;

            Session.Remove("UnverifiedUserId");
            Session.Remove("UnverifiedEmail");
            Session.Remove("temp_username");

            Response.Redirect("../HomePage.aspx");
        }
        else
        {
            lblErrorMessage.Text = "Invalid or expired code.";
        }
    }

    private void SendAndSaveCode()
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string cleanSql = "DELETE FROM user_verifications WHERE expires_at < @now";
            using (var cleanCmd = new NpgsqlCommand(cleanSql, conn))
            {
                cleanCmd.Parameters.AddWithValue("@now", DateTime.Now);
                cleanCmd.ExecuteNonQuery();
            }
        }
        Random rand = new Random();
        string code = rand.Next(100000, 999999).ToString();
        int userId = Convert.ToInt32(Session["UnverifiedUserId"]);
        string userEmail = Session["UnverifiedEmail"].ToString();

        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string deleteSql = "DELETE FROM user_verifications WHERE user_id = @uid";
            using (var delCmd = new NpgsqlCommand(deleteSql, conn))
            {
                delCmd.Parameters.AddWithValue("@uid", userId);
                delCmd.ExecuteNonQuery();
            }

            string insertSql = "INSERT INTO user_verifications (user_id, code, expires_at) VALUES (@uid, @code, @exp)";
            using (var cmd = new NpgsqlCommand(insertSql, conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@exp", DateTime.Now.AddMinutes(15));
                cmd.ExecuteNonQuery();
            }
        }

        SendEmail(userEmail, code);
    }
    private void SendEmail(string toEmail, string code)
    {
        // Clear previous errors
        lblErrorMessage.Text = "";

        try
        {
            // 1. Ensure modern security protocol
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // 2. Fetch credentials
            string fromEmail = ConfigurationManager.AppSettings["SmtpEmail"];
            string appPassword = ConfigurationManager.AppSettings["SmtpPassword"];

            // DEBUG: Check if credentials exist in web.config
            if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(appPassword))
            {
                lblErrorMessage.Text = "Debug Error: SMTP credentials missing in web.config.";
                return;
            }

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, appPassword),
                EnableSsl = true,
                // 3. Optional: Add a timeout to see if the server is unreachable
                Timeout = 10000
            };

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromEmail, "MoodCast Team");
            mailMessage.Subject = "Your MoodCast Verification Code";
            mailMessage.Body = string.Format("<h2>Welcome!</h2><p>Your code is: <b>{0}</b></p>", code);
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(toEmail);

            // 4. Send
            smtpClient.Send(mailMessage);
        }
        catch (SmtpException smtpEx)
        {
            // Catch specific SMTP errors (Authentication, Port issues)
            lblErrorMessage.Text = string.Format("SMTP Error: {0} (Status: {1})", smtpEx.Message, smtpEx.StatusCode);
            if (smtpEx.InnerException != null)
            {
                lblErrorMessage.Text += " | Inner: " + smtpEx.InnerException.Message;
            }
        }
        catch (Exception ex)
        {
            // Catch all other errors
            lblErrorMessage.Text = "General Error: " + ex.Message;
            if (ex.InnerException != null)
            {
                lblErrorMessage.Text += " | Details: " + ex.InnerException.Message;
            }
        }
    }

    private bool ValidateCode(int userId, string code)
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = "SELECT COUNT(*) FROM user_verifications WHERE user_id = @uid AND code = @code AND expires_at > @now";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@now", DateTime.Now);
                return (long)cmd.ExecuteScalar() > 0;
            }
        }
    }

    private void MarkUserAsVerified(int userId)
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = "UPDATE users SET is_verified = TRUE WHERE id = @uid";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    protected void btnResend_Click(object sender, EventArgs e)
    {
        SendAndSaveCode();
        lblErrorMessage.Text = "A new code has been sent.";
        lblErrorMessage.ForeColor = System.Drawing.Color.Blue;
    }
}