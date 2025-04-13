using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FeedbackApp.Models
{
    public class FeedbackModel
    {
        [JsonPropertyName("feedback_provider_role")]
        [Required(ErrorMessage = "Please select your role")]
        public string FeedbackProviderRole { get; set; }

        [JsonPropertyName("strengths")]
        [Required(ErrorMessage = "Please select at least one strength")]
        public List<string> Strengths { get; set; } = new List<string>();

        [JsonPropertyName("improvements")]
        [Required(ErrorMessage = "Please select at least one area for improvement")]
        public List<string> Improvements { get; set; } = new List<string>();

        [JsonPropertyName("hirable_suggestions")]
        [Required(ErrorMessage = "Please select at least one hiring suggestion")]
        public List<string> HirableSuggestions { get; set; } = new List<string>();

        [JsonPropertyName("comments")]
        public string Comments { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Options for the different lists
        public static readonly List<string> RoleOptions = new List<string>
        {
            "Technical Interviewer",
            "Hiring Manager",
            "HR/Recruiter",
            "Professor/Instructor",
            "Project Mentor / Supervisor",
            "Peer/Classmate",
            "Industry Professional",
            "Other"
        };

        public static readonly List<string> StrengthOptions = new List<string>
        {
            "Tech Communication",
            "Code Quality",
            "Algorithms",
            "System Design",
            "Debugging",
            "Data Structures",
            "API Integration",
            "Testing Approach",
            "Problem-Solving",
            "Technical Depth",
            "Collaboration",
            "Learning Agility",
            "Project Planning",
            "Tech Versatility"
        };

        public static readonly List<string> ImprovementOptions = new List<string>
        {
            "Technical Communication",
            "Problem-Solving Approach",
            "Code Quality & Best Practices",
            "System Design Knowledge",
            "Time & Project Management",
            "Algorithmic Thinking",
            "Collaborative Coding Skills",
            "Technical Depth vs. Breadth",
            "API & Documentation Usage",
            "Testing & Debugging Practices"
        };

        public static readonly List<string> HirableSuggestionOptions = new List<string>
        {
            "Specialized Technical Training",
            "Open Source Contributions",
            "Side Project Development",
            "Technical Writing/Documentation",
            "Software Architecture Experience",
            "Full-Stack Development Skills",
            "Cloud/DevOps Exposure",
            "ML/AI Knowledge Building",
            "Technical Mentorship"
        };
    }
}
