using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedbackApp.Models;
using FeedbackApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FeedbackApp.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ILogger<DashboardModel> _logger;
        private readonly SupabaseService _supabaseService;
        private readonly IConfiguration _configuration;

        public List<FeedbackModel> FeedbackList { get; set; } = new List<FeedbackModel>();
        public bool IsLoading { get; set; } = true;
        public bool IsSupabaseConfigured { get; set; } = false;
        public string MostCommonRole { get; set; } = "N/A";
        public string MostCommonStrength { get; set; } = "N/A";

        public DashboardModel(ILogger<DashboardModel> logger, SupabaseService supabaseService, IConfiguration configuration)
        {
            _logger = logger;
            _supabaseService = supabaseService;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            // Check if Supabase is configured
            IsSupabaseConfigured = !string.IsNullOrEmpty(_configuration["Supabase:Url"]) && 
                                  !string.IsNullOrEmpty(_configuration["Supabase:Key"]);

            if (!IsSupabaseConfigured)
            {
                IsLoading = false;
                return;
            }

            try
            {
                // Load feedback data
                FeedbackList = await _supabaseService.GetAllFeedbackAsync();
                
                // Calculate analytics
                CalculateAnalytics();
                
                IsLoading = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                IsLoading = false;
            }
        }

        private void CalculateAnalytics()
        {
            if (FeedbackList.Count == 0)
            {
                return;
            }

            // Calculate most common role
            var roleGroups = FeedbackList
                .GroupBy(f => f.FeedbackProviderRole)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count);

            var topRole = roleGroups.FirstOrDefault();
            if (topRole != null)
            {
                MostCommonRole = topRole.Role;
            }

            // Calculate most common strength
            var allStrengths = FeedbackList
                .SelectMany(f => f.Strengths)
                .GroupBy(s => s)
                .Select(g => new { Strength = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count);

            var topStrength = allStrengths.FirstOrDefault();
            if (topStrength != null)
            {
                MostCommonStrength = topStrength.Strength;
            }
        }
    }
}