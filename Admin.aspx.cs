using Npgsql;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class admin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["isAdmin"] == null || !(bool)Session["isAdmin"])
        {
            Response.Redirect("HomePage.aspx");
        }
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
    protected void GridViewUsers_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewUsers.EditIndex = e.NewEditIndex;
        LoadUsers();  
    }

    protected void GridViewUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewUsers.EditIndex = -1;
        LoadUsers();
    }

    protected void GridViewUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id = (int)GridViewUsers.DataKeys[e.RowIndex].Value;

        string username = e.NewValues["username"].ToString();
        string email = e.NewValues["email"].ToString();
        string fullName = e.NewValues["fullname"].ToString();
        string password = e.NewValues["password"].ToString(); 

        string connString = System.Configuration.ConfigurationManager
                            .ConnectionStrings["MoodCastDb"].ConnectionString;

        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = @"UPDATE users
                       SET username=@u,
                           email=@e,
                           fullname=@f,
                           password=@p
                       WHERE id=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@f", fullName);
                cmd.Parameters.AddWithValue("@p", password);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        GridViewUsers.EditIndex = -1; 
        LoadUsers();
    }

    protected void GridViewUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = (int)GridViewUsers.DataKeys[e.RowIndex].Value;

        string connString = System.Configuration.ConfigurationManager
                            .ConnectionStrings["MoodCastDb"].ConnectionString;

        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = "DELETE FROM users WHERE id=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        LoadUsers();  
    }

}
