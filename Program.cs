using DotNetEnv;



using FeedbackApp.Services;
// This loads the environment variables from the .env file
Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<SupabaseService>();
builder.Services.AddScoped<SupabaseService>();

// Add Supabase configuration from environment variables if available
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
var supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY");

if (!string.IsNullOrEmpty(supabaseUrl) && !string.IsNullOrEmpty(supabaseKey))
{
    builder.Configuration["Supabase:Url"] = supabaseUrl;
    builder.Configuration["Supabase:Key"] = supabaseKey;
}

var app = builder.Build();

// Add headers for embedding
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self' *.google.com");
    context.Response.Headers.Add("X-Frame-Options", "ALLOW-FROM https://sites.google.com");
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
