using Npgsql;
using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class admin : System.Web.UI.Page
{
    private string connString = ConfigurationManager.ConnectionStrings["MoodCastDb"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["isAdmin"] == null || !(bool)Session["isAdmin"])
        {
            Response.Redirect("HomePage.aspx");
        }
        if (!IsPostBack)
        {
            LoadAllData();
        }
    }

    private void LoadAllData()
    {
        LoadUsers();
        LoadAdmin();
        LoadVerifications();
    }

    private void LoadUsers()
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string query = "SELECT id, username, email, fullname, is_verified, counter, created_at FROM users ORDER BY id ASC";
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

    private void LoadVerifications()
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string query = "SELECT * FROM user_verifications ORDER BY created_at DESC";
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var adapter = new NpgsqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                GridViewVerifications.DataSource = dt;
                GridViewVerifications.DataBind();
            }
        }
    }

    private void LoadAdmin()
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string query = "SELECT id, username, created_at FROM admin";
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

        // שליפת ערכים מהשורה הנערכת
        GridViewRow row = GridViewUsers.Rows[e.RowIndex];
        string username = ((TextBox)row.Cells[1].Controls[0]).Text;
        string email = ((TextBox)row.Cells[2].Controls[0]).Text;
        string fullName = ((TextBox)row.Cells[3].Controls[0]).Text;
        bool isVerified = ((CheckBox)row.Cells[4].Controls[0]).Checked;
        int counter = int.Parse(((TextBox)row.Cells[5].Controls[0]).Text);

        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string sql = @"UPDATE users 
                           SET username=@u, email=@e, fullname=@f, is_verified=@v, counter=@c 
                           WHERE id=@id";
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@f", fullName);
                cmd.Parameters.AddWithValue("@v", isVerified);
                cmd.Parameters.AddWithValue("@c", counter);
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