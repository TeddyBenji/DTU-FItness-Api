using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using DtuFitnessApi.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); // Clear existing providers
builder.Logging.AddConsole(); // Add console logging
builder.Logging.SetMinimumLevel(LogLevel.Trace);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});


builder.Services.AddScoped<ClubService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("https://localhost:7033") // Client application URL
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials(); // Allowing credentials is optional based on your security requirements
        });
});

// Add DbContext configuration here if not already added
// For example, if using Entity Framework Core:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7239"; // URL of your IdentityServer
        options.Audience = "api1"; // The API resource identifier you configured in IdentityServer
        options.RequireHttpsMetadata = false; // Set to true in production
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true, // Ensure the token is intended for this API
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateLifetime = false,
            
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
    app.UseDeveloperExceptionPage();
}

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseCors(builder =>
    builder.WithOrigins("https://localhost:7033")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());

app.UseAuthentication();

// Use authorization middleware
app.UseAuthorization();

//app.UseHttpsRedirection();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


