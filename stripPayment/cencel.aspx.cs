using System;
public partial class cancel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["email"] != null)
        {
            lblMessage.Text = string.Format("Sorry, {0}. Your payment was cancelled.", Session["email"]);
            // בעתיד אפשר לרשום ביטול למסד או לוג  
        }
    }
}