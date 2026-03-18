using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ADD THIS: Configure port for Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
Console.WriteLine($"Starting Gateway on port {port}");
builder.WebHost.UseUrls($"http://*:{port}");

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOcelot();

// Configure CORS for production
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // In development, allow localhost
            policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            // In production, only allow your live frontend
            policy.WithOrigins("https://smartjobportal-frontend.vercel.app")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

var app = builder.Build();

// ADD THIS: Health check endpoint
app.MapGet("/", () => "Gateway is running!");
app.MapGet("/health", () => "Healthy");

// Important: CORS must be before Ocelot
app.UseCors("AllowFrontend");

// Add authentication if needed
// app.UseAuthentication();
// app.UseAuthorization();

await app.UseOcelot();
app.Run();