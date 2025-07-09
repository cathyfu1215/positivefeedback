# Interview Feedback Web App

![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)

A lightweight, anonymous feedback collection platform for job seekers to gather insights after interviews, mock interviews, or professional conversations. Built with C# (ASP.NET Razor Pages) and Supabase (PostgreSQL), this application is optimized for quick, low-effort responses to maximize participation from feedback providers.

> 📝 **Note:** This project was created by Cathy as a learning exercise for C#, developed with the assistance of Cursor IDE and Claude-3.7-Sonnet AI.

## Table of Contents

- [Purpose](#-purpose)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture & Design](#-architecture--design)
- [Database Schema](#-database-schema)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [Usage](#-usage)
- [Deployment](#-deployment)
- [Contributing](#-contributing)
- [License](#-license)

## 🚀 Purpose

This application serves as a dedicated tool for job seekers to collect anonymous, constructive feedback from various sources. By removing identifying information and keeping the form short, it encourages honest responses from:

- Interviewers and HR professionals
- Mock interview partners
- Career advisors and professors
- Networking contacts
- Peers and colleagues

The anonymity and simplicity of the form (under 2 minutes to complete) help candidates gather valuable insights they might not otherwise receive through traditional channels.

## 🎯 Features

- **Anonymous Submission**: No login required, completely private feedback
- **Role Selection**: Identify feedback provider type (Peer, HR, Professor, etc.)
- **Quick-Select Categories**:
  - Strengths (e.g., communication, technical knowledge)
  - Areas for improvement
  - Suggestions to increase hiring potential
- **Low-Friction Interface**: Primarily checkbox-based with minimal typing required
- **Optional Comments**: Space for additional context or personalized advice
- **Timestamp-Based Identification**: Each submission receives a unique ID based on submission time
- **Responsive Design**: Works seamlessly on mobile, tablet, and desktop devices
- **Embeddable**: Can be integrated into personal websites or Google Sites via iframe
- **Data Ownership**: All feedback belongs to you, stored in your Supabase instance

## 💻 Tech Stack

### Frontend
- **Framework**: ASP.NET Core Razor Pages
- **Language**: C# (.NET 7.0+)
- **Styling**: CSS with responsive design principles
- **JavaScript**: Minimal JS for enhanced UI interactions

### Backend
- **API Integration**: Supabase REST API via HttpClient
- **Authentication**: Supabase API key authentication
- **Data Processing**: Server-side C# handling form submissions

### Database
- **Provider**: Supabase (PostgreSQL)
- **Data Structure**: Single-table design with arrays for categorical feedback
- **Security**: Row-level security policies via Supabase

### Development & Deployment
- **Version Control**: Git
- **Hosting Options**:
  - Replit (for development and testing)
  - Render (for production)
  - Microsoft Azure (for production)
- **Requirements**: HTTPS endpoint required for secure data transmission

## 🏗 Architecture & Design

The application follows a simple yet effective architecture:

1. **Frontend Layer**: Razor Pages provide the user interface and form handling
2. **Service Layer**: Handles communication with Supabase through REST API calls
3. **Data Layer**: Supabase PostgreSQL database stores all feedback entries

The design prioritizes:
- **Simplicity**: Minimal clicks to complete feedback
- **Anonymity**: No tracking or personal identifiers
- **Accessibility**: Works across devices and browsers
- **Performance**: Fast loading and submission times

## 🗃️ Database Schema

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

## 🚦 Getting Started

### Prerequisites

- .NET 7.0 SDK or higher
- Supabase account
- Git (optional, for version control)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/cathyfu1215/positivefeedback.git
   cd positivefeedback
   ```

2. Restore dependencies and run the app:
   ```bash
   dotnet restore
   dotnet run
   ```

### Configuration

1. Create a Supabase project at [supabase.com](https://supabase.com)
2. Set up the database schema using the SQL provided in the Database Schema section
3. Configure your application settings:
   ```json
   {
     "Supabase": {
       "Url": "your-supabase-project-url",
       "Key": "your-supabase-api-key"
     }
   }
   ```

## 📝 Usage

### For Feedback Recipients (Job Seekers)

1. **Set Up**: Deploy your instance of the application
2. **Share**: Send the feedback form link to anyone who might provide valuable input
3. **Analyze**: Review collected feedback to identify patterns and areas for growth

### For Feedback Providers

1. **Access**: Open the shared feedback form link
2. **Select Role**: Choose your relationship to the job seeker
3. **Provide Feedback**: Check applicable boxes for strengths and areas for improvement
4. **Add Context**: Optionally provide more detailed comments
5. **Submit**: Send your anonymous feedback

## 🚀 Deployment

### Replit (Development/Testing)

1. Create a new Replit project
2. Import your code or connect to your repository
3. Configure environment variables for Supabase credentials
4. Run the application

### Render or Azure (Production)

1. Connect your repository to your chosen platform
2. Configure build settings for a .NET application
3. Set environment variables for Supabase credentials
4. Deploy with HTTPS enabled

### Embedding

To embed the feedback form in your personal website or Google Site:

```html
<iframe 
  src="https://your-deployed-app-url" 
  width="100%" 
  height="800px" 
  style="border:none;">
</iframe>
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## Screenshots

![Screenshot 2025-07-08 at 10 08 23 PM](https://github.com/user-attachments/assets/08d2d4ca-f071-4555-954a-cbdea1743926)
![Screenshot 2025-07-08 at 10 08 34 PM](https://github.com/user-attachments/assets/51ce17b5-41b0-4590-a513-a091ff94d49e)

