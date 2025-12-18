using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json.Linq;

public partial class success : System.Web.UI.Page
{
    string connectionString= ConfigurationManager.ConnectionStrings["MoodCastDb"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        // 1. Get the Checkout Session ID that Stripe appended to the URL
        string checkoutSessionId = Request.QueryString["session_id"];
        if (string.IsNullOrEmpty(checkoutSessionId))
        {
            lblMessage.Text = "No Stripe session ID was provided.";
            return;
        }

        // 2. Retrieve the session details from Stripe
        JObject stripeSession;
        using (var client = new WebClient())
        {
            client.Headers.Add(
                HttpRequestHeader.Authorization,
                "Bearer " + ConfigurationManager.AppSettings["stripeSecretKey"]);

            string json = client.DownloadString(
                "https://api.stripe.com/v1/checkout/sessions/" + checkoutSessionId);

            stripeSession = JObject.Parse(json);
        }

        // 3. Check payment status (expects "paid")
        string paymentStatus = stripeSession["payment_status"].ToString();
        if (paymentStatus != "paid")
        {
            lblMessage.Text = "Payment not completed (status: " + paymentStatus + ").";
            return;
        }

        
        if (Session["email"] != null)
        {
            lblMessage.Text = string.Format("Thank you, "+Session["username"]+", for your payment!");
            
        }
        else
        {
            lblMessage.Text = "Thank you! Your payment was successful.";
        }

        using (var conn = new Npgsql.NpgsqlConnection(connectionString))
        {
            conn.Open();
            string updateCounterQuery = "UPDATE users SET counter =@counter WHERE username = @username";
            using (var cmd = new Npgsql.NpgsqlCommand(updateCounterQuery, conn))
            {
                cmd.Parameters.AddWithValue("@username", Session["username"].ToString());
                cmd.Parameters.AddWithValue("@counter", -1);
                cmd.ExecuteNonQuery();
            }
            Session["counter"] = -1;

        }
        // Example: insert payment details into your database here
        // decimal amountPaid = (decimal)stripeSession["amount_total"] / 100m;
        // string email = Session["email"]?.ToString();
        // ... (DB insert)
    }
}
