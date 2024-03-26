using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.IdentityModel.Tokens;
using DtuFitnessApi.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: makes the output JSON easier to read
    });

builder.Services.AddScoped<ClubService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<NotificationService>();

// Add DbContext configuration here if not already added
// For example, if using Entity Framework Core:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5250"; // URL of your IdentityServer
        options.Audience = "api1"; // The API resource identifier you configured in IdentityServer
        options.RequireHttpsMetadata = false; // Set to true in production
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true, // Ensure the token is intended for this API
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            
        };
    });


builder.Services.AddAuthorization(options =>
    {
        
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseRouting();

app.UseAuthentication();

// Use authorization middleware
app.UseAuthorization();

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();


