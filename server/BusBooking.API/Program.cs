using BusBooking.API.Data;
using BusBooking.API.Repositories.Interfaces;
using BusBooking.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using BusBooking.API.Services.Interfaces;
using BusBooking.API.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
// Controllers + Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// PostgreSQL+ EF Core
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBusRouteRepository, BusRouteRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<ISeatService, SeatService>();
// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Ollama HTTP Client
builder.Services.AddHttpClient("Ollama", client =>
{
    client.BaseAddress = new Uri("http://localhost:11434");
    client.Timeout = TimeSpan.FromSeconds(120);
});

// Chat Service
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();
app.UseCors("AllowFrontend");

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // disabled for local dev
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();