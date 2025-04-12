# 📝 Interview Feedback Web App (C# + Supabase)

A lightweight, anonymous feedback form for job seekers to share after interviews, mock interviews, or chats. It uses C# (ASP.NET Razor Pages) and Supabase (PostgreSQL), and it’s optimized for quick, low-effort responses from any feedback provider.

---

## 🚀 Purpose

Share this form with peers, professors, HR, or team leads to get quick, constructive feedback. The form is completely anonymous, takes under 2 minutes, and helps you grow based on real impressions.

---

## 🎯 Features

- ✅ Anonymous feedback form
- ✅ No company, no names, no pressure
- ✅ Role selection (e.g., "Peer", "HR", "Professor", etc.)
- ✅ Click-only checkboxes + optional comment box
- ✅ All data stored in Supabase (PostgreSQL)
- ✅ Unique ID = timestamp of submission
- ✅ Shareable + embeddable in Google Sites

---

## 🗃️ Supabase Schema

```sql
create table feedback (
  created_at timestamptz primary key default now(),
  feedback_provider_role text,
  strengths text[],             -- e.g., ['communication', 'clarity']
  improvements text[],          -- e.g., ['confidence', 'technical depth']
  hirable_suggestions text[],   -- e.g., ['more projects', 'stronger stories']
  comments text
);
```

## Tech Stack
* Frontend: C# with ASP.NET Razor Pages

* Backend: Supabase (REST API using HttpClient)

* Database: Supabase PostgreSQL

* Deployment: Replit, Render, or Azure (HTTPS required)

* Embed: iframe on Google Site
