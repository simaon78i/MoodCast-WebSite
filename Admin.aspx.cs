using System;
using System.Data;
using Npgsql;

public partial class admin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUsers();
            LoadAdmin();
        }
    }

    private void LoadUsers()
    {
        string connString = System.Configuration.ConfigurationManager
            .ConnectionStrings["MoodCastDb"].ConnectionString;

        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string query = "SELECT * FROM users";

            using (var cmd = new NpgsqlCommand(query, conn))
            using (var adapter = new NpgsqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                GridViewUsers.DataSource = dt;
                GridViewUsers.DataBind();
            }
        }
    }
    private void LoadAdmin()
    {
        string connString = System.Configuration.ConfigurationManager
            .ConnectionStrings["MoodCastDb"].ConnectionString;

        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string query = "SELECT * FROM admin";

            using (var cmd = new NpgsqlCommand(query, conn))
            using (var adapter = new NpgsqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                GridViewAdmin.DataSource = dt;
                GridViewAdmin.DataBind();
            }
        }
    }
}
