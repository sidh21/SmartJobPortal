using FluentValidation;
using JobService.Interfaces;
using JobService.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure port for Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "5002";
Console.WriteLine($"Starting JobService on port {port}");
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Serilog
builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ CORS - THIS IS CRITICAL
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// MediatR + Validation
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Repository
builder.Services.AddScoped<IJobRepository, JobRepository>();

// JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (!string.IsNullOrEmpty(jwtKey))
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
}

var app = builder.Build();

// Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI();

// ✅ CRITICAL ORDER - CORS MUST BE FIRST
app.UseCors("AllowAll");

// Remove HTTPS redirect if causing issues
// app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

if (!string.IsNullOrEmpty(builder.Configuration["Jwt:Key"]))
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();

// Health check
app.MapGet("/", () => "JobService is running!");
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "JobService",
    timestamp = DateTime.UtcNow
}));

app.Run();