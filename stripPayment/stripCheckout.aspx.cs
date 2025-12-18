using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Web.UI;
using Newtonsoft.Json.Linq;

public partial class stripe_checkout : Page
{
    protected void btnPay_Click(object sender, EventArgs e)
    {
        string amountText = "10";
        decimal amount;

        if (!decimal.TryParse(amountText, out amount))
            return;

        int amountInCents = (int)(amount * 100);

        using (var client = new WebClient())
        {
            client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + ConfigurationManager.AppSettings["stripeSecretKey"]);

            var values = new NameValueCollection();
            values["success_url"] = "https://www.MoodCastApp.somee.com/stripPayment/success.aspx?session_id={CHECKOUT_SESSION_ID}";
            values["cancel_url"] = "https://www.MoodCastApp.somee.com/stripPayment/cancel.aspx";
            values["mode"] = "payment";
            values["payment_method_types[0]"] = "card";
            values["line_items[0][price_data][currency]"] = "usd";
            values["line_items[0][price_data][product_data][name]"] = "Custom Product";
            values["line_items[0][price_data][unit_amount]"] = amountInCents.ToString();
            values["line_items[0][quantity]"] = "1";

            byte[] response = client.UploadValues("https://api.stripe.com/v1/checkout/sessions", "POST", values);
            string responseString = System.Text.Encoding.UTF8.GetString(response);

            JObject session = JObject.Parse(responseString);
            string redirectUrl = session["url"].ToString();

            Response.Redirect(redirectUrl);
        }
    }
}
