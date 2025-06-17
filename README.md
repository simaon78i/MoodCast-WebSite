# 🎬 MoodCast — AI-Powered Movie Recommendations Based on Your Mood

**MoodCast** is a smart movie recommendation website that suggests films based on your current mood.  
Powered by AI, integrated with TMDB and Google OAuth, it provides a personalized cinematic experience.

---

## 🌟 Features

- 🎭 **Mood-based movie recommendations** — Just tell us how you feel!
- 🤖 **AI Integration (OpenAI GPT)** — Interprets your mood text and suggests matching movies.
- 🎬 **TMDB Integration** — Fetches movie posters, ratings, and trailers in real time.
- 👤 **User system** — Sign up with a custom form or sign in with Google.
- 🛠️ **Admin Panel** — View all registered users.
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
- API keys for:
  - [OpenAI](https://platform.openai.com/)
  - [TMDB](https://www.themoviedb.org/)
  - [Google OAuth](https://console.developers.google.com/)

### ⚙️ Configuration

The project uses a `web.config` file to store all sensitive configuration keys:

- `OpenAIKey`
- `TMDBKey`
- `GoogleClientId`
- `GoogleClientSecret`
- `adminUserName`, `adminPassword`
- `MoodCastDb` connection string (PostgreSQL)

> ⚠️ These keys are **not included** in this repository.  
> Please create your own `web.config` file based on your environment and **never commit sensitive data**.

---

## 🖼️ Screenshot

![MoodCast Screenshot](assets/example.png)

---

## 🎥 Demo Video

> The following video shows a brief walkthrough of the MoodCast experience:

▶️ [Watch the demo on YouTube](https://youtu.be/M9_5bSHv584)
(or view locally at `assets/presentationG.mp4`)

---

## 🛡️ Security Notice

- This is a public repository. Do **not** commit private API keys or `web.config` files.
- Use `.gitignore` to protect sensitive configuration and compiled files.

---

## 🧠 Credits

- Movie metadata via [TMDB](https://www.themoviedb.org/)
- Mood interpretation via [OpenAI](https://platform.openai.com/)
- OAuth integration via [Google Cloud Console](https://console.cloud.google.com/)

---

## 📜 License

This project is open-source and available under the **MIT License**.
