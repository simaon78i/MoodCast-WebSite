# 🎬 MoodCast — AI-Powered Movie Recommendations Based on Your Mood

**MoodCast** is a smart movie-recommendation website that suggests films based on your current mood.  
Powered by AI, integrated with TMDB, Google OAuth, **Stripe Checkout**, it delivers a fully personalized cinematic (and payment) experience.

---

## 🌟 Features

- 🎭 **Mood-based movie recommendations** — just tell us how you feel
- 🤖 **AI Integration (OpenAI GPT)** — interprets your mood text and suggests matching movies
- 🎬 **TMDB Integration** — fetches posters, ratings, and trailers in real time
- 👤 **User system** — sign up with a custom form or sign in with Google
- 💳 **Stripe Checkout payments (test mode)** — seamless credit-card payments for premium features
- 🛠️ **Admin Panel** — view all registered users
- 💡 **Built with:**  
  - ASP.NET Web Forms (Framework)  
  - C#  
  - HTML5 & CSS3  
  - PostgreSQL (via Npgsql)

---

## 🚀 Getting Started

### 🔧 Prerequisites

- Visual Studio with ASP.NET Framework support  
- PostgreSQL installed and running  
- API keys / credentials for:  
  - [OpenAI](https://platform.openai.com/)  
  - [TMDB](https://www.themoviedb.org/)  
  - [Google OAuth](https://console.cloud.google.com/)  
  - **Stripe (test keys)** — create a free account at [stripe.com](https://stripe.com) and grab `sk_test_*` / `pk_test_*`

### ⚙️ Configuration

All sensitive keys are stored in `web.config` (never commit real keys):

```xml
<appSettings>
  <add key="OpenAIKey" value="..." />
  <add key="TMDBKey" value="..." />
  <add key="GoogleClientId" value="..." />
  <add key="GoogleClientSecret" value="..." />
  <add key="StripeSecretKey" value="sk_test_..." /> <!-- Stripe -->
  <add key="adminUserName" value="admin" />
  <add key="adminPassword" value="••••••" />
</appSettings>

<connectionStrings>
  <add name="MoodCastDb" connectionString="Host=...;Username=...;Password=...;Database=..." />
</connectionStrings>
