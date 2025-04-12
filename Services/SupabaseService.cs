using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FeedbackApp.Models;
using Microsoft.Extensions.Configuration;

namespace FeedbackApp.Services
{
    public class SupabaseService
    {
        private readonly HttpClient _httpClient;
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly string _tableName = "feedback";

        public SupabaseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _supabaseUrl = configuration["Supabase:Url"];
            _supabaseKey = configuration["Supabase:Key"];
            
            // Configure HTTP client with Supabase headers if credentials are available
            _httpClient.DefaultRequestHeaders.Clear();
            
            if (!string.IsNullOrEmpty(_supabaseUrl) && !string.IsNullOrEmpty(_supabaseKey))
            {
                _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
            }
            
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> SubmitFeedbackAsync(FeedbackModel feedback)
        {
            try
            {
                // Check if Supabase credentials are available
                if (string.IsNullOrEmpty(_supabaseUrl) || string.IsNullOrEmpty(_supabaseKey))
                {
                    Console.WriteLine("Supabase credentials are not configured. Cannot submit feedback.");
                    return false;
                }
                
                var url = $"{_supabaseUrl}/rest/v1/{_tableName}";
                var content = JsonSerializer.Serialize(feedback);
                var requestContent = new StringContent(content, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(url, requestContent);
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                
                var responseContent = await response.Content.ReadAsStringAsync();
                var statusCode = (int)response.StatusCode;
                Console.WriteLine($"Error submitting feedback: StatusCode={statusCode}, Response={responseContent}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when submitting feedback: {ex.Message}");
                return false;
            }
        }
    }
}
