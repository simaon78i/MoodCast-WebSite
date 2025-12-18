using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        // Enforce TLS 1.2 for Google/TMDB connections
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        if (!IsPostBack)
        {
            EnsureDatabaseSetup();
            UpdateUserUI();
        }

        if (Session["isAdmin"] != null && (bool)Session["isAdmin"])
        {
            adminLink.Visible = true;
        }
        if (Session["counter"] != null && (int)Session["counter"]!=-1)
        {
            premiumLink.Visible = true;
            lblCounterMassage.Text = "<br/>You have left " + Session["counter"] + " tries in your free acount if you want more upgrade to premium<br/>";
        }
        if( Session["counter"] != null && (int)Session["counter"] == -1)
        {
            lblCounterMassage.Text = "<br/>You have unlimited tries with your premium account enjoy :)<br/>";
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
                    is_verified BOOLEAN DEFAULT FALSE,
                    counter INT DEFAULT 5,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );";

            string sqlAdmin = @"
                CREATE TABLE IF NOT EXISTS admin (
                    id SERIAL PRIMARY KEY,
                    username VARCHAR(50) NOT NULL UNIQUE,
                    password TEXT NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );";
            string sqlVerification = @"
                CREATE TABLE IF NOT EXISTS user_verifications (
                    id SERIAL PRIMARY KEY,
                    user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                    code VARCHAR(10) NOT NULL,
                    expires_at TIMESTAMP NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );";

            using (var cmd = new NpgsqlCommand(sqlUsers, conn)) { cmd.ExecuteNonQuery(); }
            using (var cmd = new NpgsqlCommand(sqlAdmin, conn)) { cmd.ExecuteNonQuery(); }
            using (var cmd = new NpgsqlCommand(sqlVerification, conn)) { cmd.ExecuteNonQuery(); }
           
            string adminUsername = ConfigurationManager.AppSettings["adminUserName"];
            string adminPassword = ConfigurationManager.AppSettings["adminPassword"];

            string insertAdmin = "INSERT INTO admin (username, password) VALUES (@username, @password);";
            using (var cmd = new NpgsqlCommand(insertAdmin, conn))
            {
                cmd.Parameters.AddWithValue("@username", adminUsername);
                cmd.Parameters.AddWithValue("@password", adminPassword);
                try { cmd.ExecuteNonQuery(); } catch (PostgresException ex) { if (ex.SqlState != "23505") throw; }
            }

            string insertUser = "INSERT INTO users (username, password, email, fullName, is_verified,counter) VALUES (@username, @password, @email, @fullName,@verify,@counter);";
            using (var cmd = new NpgsqlCommand(insertUser, conn))
            {
                cmd.Parameters.AddWithValue("@username", adminUsername);
                cmd.Parameters.AddWithValue("@password", HashPassword(adminPassword));
                cmd.Parameters.AddWithValue("@email", "simaon78ifrac@gmail.com");
                cmd.Parameters.AddWithValue("@fullName", "Shimon Ifrach");
                cmd.Parameters.AddWithValue("@verify", true);
                cmd.Parameters.AddWithValue("@counter", -1);
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

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
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
        else if (Session["counter"] != null && (int)Session["counter"] == 0)
        {
            lblCounterMassage.Text = "<p style='color:red;'>You have exhausted your free recommendations. Please upgrade to premium for unlimited access.</p>";
            return;
        }
        string mood = txtMood.Text;
        litRecommendations.Text = "<p>Searching for the best matches with trailers...</p>"; 

        try
        {
            // 1. Get a larger pool of candidates
            string moviesText = await GetMovieRecommendations(mood);

            if (moviesText.StartsWith("Error") || moviesText.Contains("\"error\"") || moviesText.Contains("UNAVAILABLE") || moviesText.Contains("RESOURCE_EXHAUSTED"))
            {
                 litRecommendations.Text = "<p style='color:red;'><b>Connection Error:</b> " + moviesText + "</p>";
                 return;
            }

            List<string> allCandidates = ExtractMovieTitles(moviesText);

            if (allCandidates.Count == 0)
            {
                 litRecommendations.Text = "<p style='color:orange;'>Could not parse movies. Raw AI response:</p><pre>" + moviesText + "</pre>";
                 return;
            }

            StringBuilder htmlOutput = new StringBuilder();
            htmlOutput.Append("<h2>Recommendations based on your mood:</h2><div class='recommendations-row'>");

            int validMoviesFound = 0;

            foreach (string title in allCandidates)
            {
                if (validMoviesFound >= 4) break;

                string cleanTitle = title.Trim();
                MovieInfo info = await GetMediaInfo(cleanTitle);

                // Skip if missing poster
                if (info == null || string.IsNullOrEmpty(info.PosterUrl))
                {
                    continue;
                }

                // Skip if missing trailer
                if (string.IsNullOrEmpty(info.TrailerUrl))
                {
                    continue; 
                }

                // FIX: Updated iframe attributes for Fullscreen and Security
                string trailerHtml = string.Format(
                    "<iframe src='{0}' " +
                    "frameborder='0' " +
                    "allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share; fullscreen' " +
                    "allowfullscreen='true' " +
                    "referrerpolicy='strict-origin-when-cross-origin'></iframe>", 
                    info.TrailerUrl);
                
                htmlOutput.AppendFormat(
                    "<div class='movie-card'>" +
                    "<img src='{0}' class='card-img-top' alt='{1}' />" + // Added alt text
                    "<div class='card-body'>" +
                    "<h5 class='card-title'>{1}</h5>" +
                    "<p class='card-text'>⭐ rating: {2} / 10</p>" +
                    "{3}" +
                    "</div></div>",
                    info.PosterUrl,
                    info.Title,
                    info.Rating.ToString("0.0"),
                    trailerHtml
                );

                validMoviesFound++;
            }

            htmlOutput.Append("</div>");
            
            if (validMoviesFound == 0)
            {
                 litRecommendations.Text = "<p>AI suggested titles, but none had valid posters or trailers available. Try a different mood.</p>";
            }
            else
            {
                 litRecommendations.Text = htmlOutput.ToString();
            }
        }
        catch (Exception ex)
        {
            litRecommendations.Text = "<p style='color:red;'>System Error: " + Server.HtmlEncode(ex.Message) + "</p>";
        }
        updateUserCounter();
    }
    private void updateUserCounter()
    {
        using (var conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            string username = Session["username"].ToString();
            string getCounterQuery = "SELECT counter FROM users WHERE username = @username;";
            int currentCounter = 0;
            using (var cmd = new NpgsqlCommand(getCounterQuery, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    currentCounter = Convert.ToInt32(result);
                }
            }
            if (currentCounter > 0)
            {
                currentCounter--;
                string updateCounterQuery = "UPDATE users SET counter = @counter WHERE username = @username;";
                using (var cmd = new NpgsqlCommand(updateCounterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@counter", currentCounter);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.ExecuteNonQuery();
                }
                Session["counter"] = currentCounter;
            }
        }
        
    }

    private async Task<string> GetMovieRecommendations(string mood)
    {
        string apiKey = ConfigurationManager.AppSettings["GoogleKey"];
        if (apiKey != null) apiKey = apiKey.Trim();

        // Stable Model
        string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-001:generateContent?key=" + apiKey;

        // Prompt
        string prompt = "Recommend exactly 12 movies, TV series, or anime that **strictly and perfectly match** the following specific mood/genre: \"" + mood + "\". " +
                        "From the titles that fit this specific mood, prioritize the ones with the **highest ratings** (IMDB/TMDB) and critical acclaim. " +
                        "**Crucial:** Do not recommend high-rated movies that do not fit the requested mood. Relevance is the #1 priority. " +
                        "If the user asks for 'anime', the list **must** contain only Japanese animation. " +
                        "List only the exact English title of each on a separate line without numbering, years, or extra characters.";

        using (HttpClient client = new HttpClient())
        {
            JObject jsonBody = new JObject();
            JObject part = new JObject();
            part["text"] = prompt;
            JArray parts = new JArray();
            parts.Add(part);
            JObject contentObj = new JObject();
            contentObj["parts"] = parts;
            JArray contents = new JArray();
            contents.Add(contentObj);
            jsonBody["contents"] = contents;

            string jsonString = jsonBody.ToString();
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await client.PostAsync(apiUrl, content);
            string resStr = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                return "Error from Google API: " + res.StatusCode + " - " + resStr;
            }

            JObject parsed = JObject.Parse(resStr);

            if (parsed["candidates"] != null && parsed["candidates"].HasValues)
            {
                JToken firstCandidate = parsed["candidates"][0];
                if (firstCandidate["content"] != null && 
                    firstCandidate["content"]["parts"] != null && 
                    firstCandidate["content"]["parts"].HasValues)
                {
                    return firstCandidate["content"]["parts"][0]["text"].ToString();
                }
            }

            return "No recommendations found.";
        }
    }

    private List<string> ExtractMovieTitles(string raw)
    {
        var titles = new List<string>();
        var lines = raw.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            string clean = line.Trim().TrimStart(new char[] { '-', '•', '*', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.', ' ', '"', '\'' });
            
            if (clean.Contains("("))
            {
                clean = clean.Split('(')[0].Trim();
            }

            if (!string.IsNullOrEmpty(clean) && clean.Length > 1)
            {
                titles.Add(clean);
            }
        }
        
        if (titles.Count < 4)
        {
            var parts = raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                string clean = part.Trim().TrimStart(new char[] { '-', '•', '*', '.', ' ' });
                if (!string.IsNullOrEmpty(clean) && !titles.Contains(clean)) titles.Add(clean);
            }
        }

        return titles;
    }

    private async Task<MovieInfo> GetMediaInfo(string title)
    {
        string tmdbKey = ConfigurationManager.AppSettings["TMDBKey"];

        using (HttpClient client = new HttpClient())
        {
            string searchUrl = "https://api.themoviedb.org/3/search/multi?api_key=" + tmdbKey + "&query=" + Uri.EscapeDataString(title) + "&language=en";
            string responseStr = await client.GetStringAsync(searchUrl);
            JObject json = JObject.Parse(responseStr);

            JToken first = null;
            if (json["results"] != null && json["results"].HasValues)
            {
                foreach (JToken result in json["results"])
                {
                    // Syntax Check: Manual null check instead of ?.
                    string mediaType = result["media_type"] != null ? result["media_type"].ToString() : null;
                    
                    if (mediaType == "movie" || mediaType == "tv")
                    {
                        first = result;
                        break;
                    }
                }
            }

            if (first == null) return null;

            // FIX: Clean Poster Path (Remove leading slash if exists to prevent double slash)
            string rawPoster = first["poster_path"] != null ? first["poster_path"].ToString() : "";
            string posterUrl = "";
            if (!string.IsNullOrEmpty(rawPoster))
            {
                // Ensure no double slash by trimming and adding manually
                posterUrl = "https://image.tmdb.org/t/p/w500/" + rawPoster.TrimStart('/');
            }
            
            string realTitle = first["title"] != null ? first["title"].ToString() : (first["name"] != null ? first["name"].ToString() : title);

            double rating = 0.0;
            if (first["vote_average"] != null)
            {
                double.TryParse(first["vote_average"].ToString(), out rating);
            }

            string id = first["id"] != null ? first["id"].ToString() : "";
            string mediaTypeFound = first["media_type"] != null ? first["media_type"].ToString() : "movie";
            
            string videoUrl;
            if (mediaTypeFound == "tv")
            {
                 videoUrl = "https://api.themoviedb.org/3/tv/" + id + "/videos?api_key=" + tmdbKey;
            }
            else
            {
                 videoUrl = "https://api.themoviedb.org/3/movie/" + id + "/videos?api_key=" + tmdbKey;
            }

            string trailerKey = null;
            try 
            {
                string videoResponse = await client.GetStringAsync(videoUrl);
                JObject videoJson = JObject.Parse(videoResponse);

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
            }
            catch 
            {
            }

            // FIX: YouTube Params for Fullscreen (fs=1), Minimal Branding (modestbranding=1), No Related (rel=0)
            string trailerUrl = !string.IsNullOrEmpty(trailerKey) ? "https://www.youtube.com/embed/" + trailerKey + "?mute=1&fs=1&modestbranding=1&rel=0" : "";

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
