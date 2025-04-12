using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FeedbackApp.Models;
using FeedbackApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace FeedbackApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SupabaseService _supabaseService;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public FeedbackModel Feedback { get; set; }
        
        public string SubmissionError { get; set; }

        public IndexModel(ILogger<IndexModel> logger, SupabaseService supabaseService, IConfiguration configuration)
        {
            _logger = logger;
            _supabaseService = supabaseService;
            _configuration = configuration;
            Feedback = new FeedbackModel();
        }

        public void OnGet()
        {
            // Check if Supabase credentials are configured
            if (string.IsNullOrEmpty(_configuration["Supabase:Url"]) || 
                string.IsNullOrEmpty(_configuration["Supabase:Key"]))
            {
                SubmissionError = "This app requires Supabase credentials to function. Ask the administrator to provide them.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if Supabase credentials are configured
            if (string.IsNullOrEmpty(_configuration["Supabase:Url"]) || 
                string.IsNullOrEmpty(_configuration["Supabase:Key"]))
            {
                SubmissionError = "Supabase credentials are not configured. Unable to submit feedback.";
                return Page();
            }
            
            // Validate the form data
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ensure lists are initialized and have at least one item
            if (Feedback.Strengths == null || Feedback.Strengths.Count == 0 ||
                Feedback.Improvements == null || Feedback.Improvements.Count == 0 ||
                Feedback.HirableSuggestions == null || Feedback.HirableSuggestions.Count == 0)
            {
                if (Feedback.Strengths == null || Feedback.Strengths.Count == 0)
                {
                    ModelState.AddModelError("Feedback.Strengths", "Please select at least one strength");
                }

                if (Feedback.Improvements == null || Feedback.Improvements.Count == 0)
                {
                    ModelState.AddModelError("Feedback.Improvements", "Please select at least one area for improvement");
                }

                if (Feedback.HirableSuggestions == null || Feedback.HirableSuggestions.Count == 0)
                {
                    ModelState.AddModelError("Feedback.HirableSuggestions", "Please select at least one suggestion for improvement");
                }

                return Page();
            }

            // Set the timestamp for when the feedback was created
            Feedback.CreatedAt = DateTime.UtcNow;

            try
            {
                // Submit the feedback to Supabase
                var success = await _supabaseService.SubmitFeedbackAsync(Feedback);

                if (success)
                {
                    return RedirectToPage("/ThankYou");
                }
                else
                {
                    SubmissionError = "Unable to save feedback to Supabase. Please check Supabase configuration.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting feedback to Supabase");
                SubmissionError = "An unexpected error occurred. Please try again later.";
                return Page();
            }
        }
    }
}
