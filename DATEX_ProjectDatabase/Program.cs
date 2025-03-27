using Hangfire;
using Microsoft.EntityFrameworkCore;
using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Service;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Repository;
using DATEX_ProjectDatabase.SignalR;
using OfficeOpenXml;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database contexts
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Hangfire
builder.Services.AddHangfire(configuration =>
    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddHttpClient();

ExcelPackage.LicenseContext = LicenseContext.Commercial;

// Register your services and repositories
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
/*builder.Services.AddScoped<IProjectManagerRepository, ProjectManagerRepository>();
builder.Services.AddScoped<IProjectManagerService, ProjectManagerService>();*/
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ProjectJobService>();
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();
builder.Services.AddScoped<IVocAnalysisRepository, VocAnalysisRepository>();
builder.Services.AddScoped<VocAnalysisService>();

/*builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

*/

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
    .EnableTokenAcquisition();*/

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadAccess", policy =>
        policy.RequireClaim("scope", "api://eirjhfij4-4595-245-038947t39g/Read"));
});

// Add SignalR services
builder.Services.AddSignalR();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Map the SignalR hub
app.MapHub<MailStatusHub>("/mailStatusHub");

// Use Hangfire dashboard
app.UseHangfireDashboard();

// Set up a recurring job
RecurringJob.AddOrUpdate<ProjectJobService>(
    "check-voc-eligibility",
    service => service.CheckAndNotifyVocEligibilityAsync(),
    Cron.Daily);

// Run the application
app.Run();