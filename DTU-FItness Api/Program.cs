using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using DtuFitnessApi.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole();
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
            policyBuilder.WithOrigins("https://localhost:7033") 
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials(); 
        });
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7239"; 
        options.Audience = "api1"; 
        options.RequireHttpsMetadata = true; 
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true, 
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


app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


