using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using Npgsql;

public partial class Home_Page : System.Web.UI.Page
{
    private string connString = ConfigurationManager.ConnectionStrings["MoodCastDb"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            EnsureDatabaseSetup();
            UpdateUserUI();
        }

        if (Session["isAdmin"] != null && (bool)Session["isAdmin"])
        {
            adminLink.Visible = true;
        }
    }

    private void EnsureDatabaseSetup()
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();

            string sqlUsers = @"
                CREATE TABLE IF NOT EXISTS users (
                    id SERIAL PRIMARY KEY,
                    username VARCHAR(50) NOT NULL ,
                    password TEXT NOT NULL,
                    email VARCHAR(100) NOT NULL UNIQUE,
                    fullName VARCHAR(100),
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );";

            string sqlAdmin = @"
                CREATE TABLE IF NOT EXISTS admin (
                    id SERIAL PRIMARY KEY,
                    username VARCHAR(50) NOT NULL UNIQUE,
                    password TEXT NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );";

            using (var cmd = new NpgsqlCommand(sqlUsers, conn)) { cmd.ExecuteNonQuery(); }
            using (var cmd = new NpgsqlCommand(sqlAdmin, conn)) { cmd.ExecuteNonQuery(); }

            string adminUsername = ConfigurationManager.AppSettings["adminUserName"];
            string adminPassword = ConfigurationManager.AppSettings["adminPassword"];

            string insertAdmin = "INSERT INTO admin (username, password) VALUES (@username, @password);";
            using (var cmd = new NpgsqlCommand(insertAdmin, conn))
            {
                cmd.Parameters.AddWithValue("@username", adminUsername);
                cmd.Parameters.AddWithValue("@password", adminPassword);
                try { cmd.ExecuteNonQuery(); } catch (PostgresException ex) { if (ex.SqlState != "23505") throw; }
            }

            string insertUser = "INSERT INTO users (username, password, email, fullName) VALUES (@username, @password, @email, @fullName);";
            using (var cmd = new NpgsqlCommand(insertUser, conn))
            {
                cmd.Parameters.AddWithValue("@username", adminUsername);
                cmd.Parameters.AddWithValue("@password", adminPassword);
                cmd.Parameters.AddWithValue("@email", "simaon78ifrac@gmail.com");
                cmd.Parameters.AddWithValue("@fullName", "Shimon Ifrach");
                try { cmd.ExecuteNonQuery(); } catch (PostgresException ex) { if (ex.SqlState != "23505") throw; }
            }
        }
    }

    private void UpdateUserUI()
    {
        if (Session["isAdmin"] != null && (bool)Session["isAdmin"])
        {
            login.Visible = false;
            signup.Visible = false;
            btnLogout.Visible = true;
            lblWelcome.InnerText = "Welcome page admin " + Session["UserName"] + " :)";
        }
        else if (Session["UserName"] != null)
        {
            login.Visible = false;
            signup.Visible = false;
            btnLogout.Visible = true;
            lblWelcome.InnerText = "Welcome " + Session["UserName"] + " :) ";
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("HomePage.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    protected async void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Session["username"] == null || Session["password"] == null)
        {
            Response.Redirect("english_log_in/register.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        string mood = txtMood.Text;
        try
        {
            string moviesText = await GetMovieRecommendations(mood);
            List<string> movieTitles = ExtractMovieTitles(moviesText);

            StringBuilder htmlOutput = new StringBuilder();
            htmlOutput.Append("<h2>Movie recommendations based on your mood:</h2><div class='recommendations-row'>");

            int count = 0;
            foreach (string title in movieTitles)
            {
                if (count >= 4) break;

                string cleanTitle = title.Split(new[] { '(', '-', ':' })[0].Trim();
                MovieInfo info = await GetMovieInfo(cleanTitle);

                if (info == null || string.IsNullOrEmpty(info.PosterUrl) || info.Rating == 0.0)
                {
                    System.Diagnostics.Debug.WriteLine("Skipping: " + title);
                    continue;
                }

                htmlOutput.AppendFormat(
                    "<div class='movie-card'>" +
                    "<img src='{0}' class='card-img-top' />" +
                    "<div class='card-body'>" +
                    "<h5 class='card-title'>{1}</h5>" +
                    "<p class='card-text'>⭐ rating: {2} / 10</p>" +
                    "<iframe src='{3}?mute=1' frameborder='0' allowfullscreen></iframe>" +
                    "</div></div>",
                    info.PosterUrl,
                    info.Title,
                    info.Rating.ToString("0.0"),
                    info.TrailerUrl
                );


                count++;
            }

            htmlOutput.Append("</div>");
            litRecommendations.Text = htmlOutput.ToString();
        }
        catch (Exception ex)
        {
            litRecommendations.Text = "<p style='color:red;'>\u05d0\u05d9\u05e8\u05e2\u05d4 \u05e9\u05d2\u05d9\u05d0\u05d4: " + Server.HtmlEncode(ex.Message) + "</p>";
        }
    }

    private async Task<string> GetMovieRecommendations(string mood)
    {
        string apiKey = ConfigurationManager.AppSettings["OpenAIKey"];
        string prompt = "Give me a list of exactly 4 movies that appear on TMDB in any language and match the following mood: \"" + mood + "\". For each movie, provide only the movie title without a summary.";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            string bodyJson = "{" +
                "\"model\": \"gpt-3.5-turbo\"," +
                "\"messages\": [{ \"role\": \"user\", \"content\": \"" + prompt.Replace("\"", "\\\"") + "\" }]" +
                "}";

            var content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            string resStr = await res.Content.ReadAsStringAsync();

            JObject parsed = JObject.Parse(resStr);
            if (parsed["choices"] != null && parsed["choices"].HasValues && parsed["choices"][0] != null && parsed["choices"][0]["message"] != null && parsed["choices"][0]["message"]["content"] != null)
            {
                return parsed["choices"][0]["message"]["content"].ToString();
            }

            return "";
        }
    }

    private List<string> ExtractMovieTitles(string raw)
    {
        var titles = new List<string>();
        var lines = raw.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            string clean = line.Trim().TrimStart('-', '•', '1', '2', '3', '4', '.', ' ');
            if (!string.IsNullOrEmpty(clean)) titles.Add(clean);
            if (titles.Count == 4) break;
        }

        if (titles.Count < 4)
        {
            var parts = raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                string clean = part.Trim();
                if (!string.IsNullOrEmpty(clean) && !titles.Contains(clean)) titles.Add(clean);
                if (titles.Count == 4) break;
            }
        }

        return titles;
    }

    private async Task<MovieInfo> GetMovieInfo(string title)
    {
        string tmdbKey = ConfigurationManager.AppSettings["TMDBKey"];

        using (HttpClient client = new HttpClient())
        {
            string searchUrl = "https://api.themoviedb.org/3/search/movie?api_key=" + tmdbKey + "&query=" + Uri.EscapeDataString(title) + "&language=en";
            string responseStr = await client.GetStringAsync(searchUrl);
            JObject json = JObject.Parse(responseStr);

            JToken first = null;
            if (json["results"] != null && json["results"].HasValues)
            {
                foreach (JToken result in json["results"])
                {
                    first = result;
                    break;
                }
            }

            if (first == null) return null;

            string posterPath = first["poster_path"] != null ? first["poster_path"].ToString() : "";
            string realTitle = first["title"] != null ? first["title"].ToString() : title;

            double rating = 0.0;
            if (first["vote_average"] != null)
            {
                double.TryParse(first["vote_average"].ToString(), out rating);
            }

            string posterUrl = !string.IsNullOrEmpty(posterPath) ? "https://image.tmdb.org/t/p/w500" + posterPath : "";

            string id = first["id"] != null ? first["id"].ToString() : "";
            string videoUrl = "https://api.themoviedb.org/3/movie/" + id + "/videos?api_key=" + tmdbKey;
            string videoResponse = await client.GetStringAsync(videoUrl);
            JObject videoJson = JObject.Parse(videoResponse);

            string trailerKey = null;
            if (videoJson["results"] != null)
            {
                foreach (var v in videoJson["results"])
                {
                    if (v["site"] != null && v["site"].ToString() == "YouTube" &&
                        v["type"] != null && v["type"].ToString() == "Trailer" &&
                        v["key"] != null)
                    {
                        trailerKey = v["key"].ToString();
                        break;
                    }
                }
            }

            string trailerUrl = !string.IsNullOrEmpty(trailerKey) ? "https://www.youtube.com/embed/" + trailerKey : "";

            return new MovieInfo
            {
                Title = realTitle,
                PosterUrl = posterUrl,
                Rating = rating,
                TrailerUrl = trailerUrl
            };
        }
    }

    public class MovieInfo
    {
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public double Rating { get; set; }
        public string TrailerUrl { get; set; }
    }
}