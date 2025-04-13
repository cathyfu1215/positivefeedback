using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FeedbackApp.Models;
using FeedbackApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FeedbackApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SupabaseService _supabaseService;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public FeedbackModel FeedbackInput { get; set; }
        
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        
        [TempData]
        public string TempSuccessMessage { get; set; }

        public IndexModel(ILogger<IndexModel> logger, SupabaseService supabaseService, IConfiguration configuration)
        {
            _logger = logger;
            _supabaseService = supabaseService;
            _configuration = configuration;
            FeedbackInput = new FeedbackModel();
        }

        public void OnGet()
        {
            // Check if there's a success message in TempData
            if (TempSuccessMessage != null)
            {
                SuccessMessage = TempSuccessMessage;
            }
            
            // Check if Supabase credentials are configured
            if (string.IsNullOrEmpty(_configuration["Supabase:Url"]) || 
                string.IsNullOrEmpty(_configuration["Supabase:Key"]))
            {
                ErrorMessage = "This app requires Supabase credentials to function. Ask the administrator to provide them.";
            }
            
            // Initialize a new feedback model for the form
            FeedbackInput = new FeedbackModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if Supabase credentials are configured
            if (string.IsNullOrEmpty(_configuration["Supabase:Url"]) || 
                string.IsNullOrEmpty(_configuration["Supabase:Key"]))
            {
                ErrorMessage = "Supabase credentials are not configured. Unable to submit feedback.";
                return Page();
            }
            
            // Validate the form data
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ensure lists are initialized and have at least one item
            if (FeedbackInput.Strengths == null || FeedbackInput.Strengths.Count == 0 ||
                FeedbackInput.Improvements == null || FeedbackInput.Improvements.Count == 0 ||
                FeedbackInput.HirableSuggestions == null || FeedbackInput.HirableSuggestions.Count == 0)
            {
                if (FeedbackInput.Strengths == null || FeedbackInput.Strengths.Count == 0)
                {
                    ModelState.AddModelError("FeedbackInput.Strengths", "Please select at least one strength");
                }

                if (FeedbackInput.Improvements == null || FeedbackInput.Improvements.Count == 0)
                {
                    ModelState.AddModelError("FeedbackInput.Improvements", "Please select at least one area for improvement");
                }

                if (FeedbackInput.HirableSuggestions == null || FeedbackInput.HirableSuggestions.Count == 0)
                {
                    ModelState.AddModelError("FeedbackInput.HirableSuggestions", "Please select at least one suggestion for improvement");
                }

                return Page();
            }

            // Set the timestamp for when the feedback was created
            FeedbackInput.CreatedAt = DateTime.UtcNow;

            try
            {
                // Submit the feedback to Supabase
                var success = await _supabaseService.SubmitFeedbackAsync(FeedbackInput);

                if (success)
                {
                    // Set success message in TempData
                    TempSuccessMessage = "Thank you for your feedback! It has been submitted successfully.";
                    
                    // Clear form data
                    ModelState.Clear();
                    
                    // Redirect to the same page to avoid form resubmission on refresh
                    return RedirectToPage();
                }
                else
                {
                    ErrorMessage = "The Supabase table 'feedback' may not exist yet. Please make sure to create the table using the SQL provided in the application documentation.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting feedback to Supabase");
                ErrorMessage = "An unexpected error occurred. Please try again later.";
                return Page();
            }
        }
    }
}
