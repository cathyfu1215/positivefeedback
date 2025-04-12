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
using System.Text.Json;

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
        
        // Data for charts
        public Dictionary<string, int> StrengthsCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ImprovementsCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> SuggestionsCounts { get; set; } = new Dictionary<string, int>();
        
        // JSON data for charts
        public string StrengthsChartData { get; set; }
        public string ImprovementsChartData { get; set; }

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
                
                // Prepare chart data
                PrepareChartData();
                
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

            // Calculate strengths frequencies
            StrengthsCounts = FeedbackList
                .SelectMany(f => f.Strengths)
                .GroupBy(s => s)
                .ToDictionary(g => g.Key, g => g.Count());

            // Get most common strength
            var topStrength = StrengthsCounts.OrderByDescending(kvp => kvp.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(topStrength.Key))
            {
                MostCommonStrength = topStrength.Key;
            }

            // Calculate improvements frequencies
            ImprovementsCounts = FeedbackList
                .SelectMany(f => f.Improvements)
                .GroupBy(s => s)
                .ToDictionary(g => g.Key, g => g.Count());

            // Calculate suggestions frequencies
            SuggestionsCounts = FeedbackList
                .SelectMany(f => f.HirableSuggestions)
                .GroupBy(s => s)
                .ToDictionary(g => g.Key, g => g.Count());
        }
        
        private void PrepareChartData()
        {
            if (FeedbackList.Count == 0)
            {
                return;
            }
            
            // Create JSON data for the strengths radar chart
            var strengthsData = new
            {
                labels = StrengthsCounts.Keys.ToArray(),
                datasets = new[]
                {
                    new
                    {
                        label = "Strengths",
                        data = StrengthsCounts.Values.ToArray(),
                        backgroundColor = "rgba(75, 192, 192, 0.2)",
                        borderColor = "rgba(75, 192, 192, 1)",
                        borderWidth = 1
                    }
                }
            };
            
            StrengthsChartData = JsonSerializer.Serialize(strengthsData);
            
            // Create JSON data for the improvements bar chart
            var improvementsData = new
            {
                labels = ImprovementsCounts.Keys.ToArray(),
                datasets = new[]
                {
                    new
                    {
                        label = "Improvements",
                        data = ImprovementsCounts.Values.ToArray(),
                        backgroundColor = "rgba(255, 159, 64, 0.2)",
                        borderColor = "rgba(255, 159, 64, 1)",
                        borderWidth = 1
                    }
                }
            };
            
            ImprovementsChartData = JsonSerializer.Serialize(improvementsData);
        }
    }
}