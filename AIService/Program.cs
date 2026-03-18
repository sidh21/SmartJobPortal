using AIService.Interfaces;
using AIService.Middleware;
using AIService.Services;
using FluentValidation;
using MediatR;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure port for Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "5004";
Console.WriteLine($"🚀 Starting AIService on port {port}");
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Serilog Configuration
builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration));

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger Configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SmartJobPortal AI Service",
        Version = "v1",
        Description = "AI-powered resume screening, job description generation, and candidate ranking"
    });
});

// CORS Configuration - Allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// MediatR + Validation Pipeline
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>),
                    typeof(ValidationBehavior<,>));
});

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Register Services
builder.Services.AddHttpClient<IGeminiService, GeminiService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(60); // 60 second timeout for AI requests
});
builder.Services.AddScoped<IAIRepository, AIRepository>();

var app = builder.Build();

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AIService v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "SmartJobPortal AI Service API";
});

// Middleware Pipeline (ORDER IS CRITICAL!)
app.UseCors("AllowAll");  // ✅ CORS must be FIRST
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

// Health Check Endpoints
app.MapGet("/", () => Results.Ok(new
{
    service = "AIService",
    status = "running",
    version = "1.0.0",
    timestamp = DateTime.UtcNow,
    endpoints = new[]
    {
        "/health",
        "/swagger",
        "/api/AI/screen-resume",
        "/api/AI/generate-jd",
        "/api/AI/rank-candidates"
    }
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "AIService",
    timestamp = DateTime.UtcNow,
    uptime = Environment.TickCount64 / 1000.0 + " seconds"
}));

// Log startup information
app.Logger.LogInformation("✅ AIService started successfully on port {Port}", port);
app.Logger.LogInformation("📚 Swagger available at: /swagger");
app.Logger.LogInformation("🏥 Health check available at: /health");

app.Run();