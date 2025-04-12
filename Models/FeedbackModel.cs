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
            "Peer",
            "Professor / Teacher",
            "HR / Recruiter",
            "Team Lead / Manager",
            "Other"
        };

        public static readonly List<string> StrengthOptions = new List<string>
        {
            "Technical skills",
            "Communication",
            "Problem solving",
            "Teamwork",
            "Adaptability",
            "Leadership",
            "Time management",
            "Creativity"
        };

        public static readonly List<string> ImprovementOptions = new List<string>
        {
            "Confidence",
            "Technical depth",
            "Communication clarity",
            "Structured thinking",
            "Project experience",
            "Handling pressure",
            "Field-specific knowledge",
            "Practice with real scenarios"
        };

        public static readonly List<string> HirableSuggestionOptions = new List<string>
        {
            "More portfolio projects",
            "Practice with system design",
            "Practice with algorithms",
            "Stronger behavioral stories",
            "Refine resume",
            "Networking",
            "Mock interviews",
            "Research companies better"
        };
    }
}
