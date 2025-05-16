using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Unit_Trust_Backend.Investor.Interfaces;
using Unit_Trust_Backend.Staff.Interfaces;
using Unit_Trust_Backend.Staff.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Unit_Trust_Backend.Investor.Services;
using Stripe;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// CORS policy setup to allow only localhost:3000 (React app)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000") // React app's URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add DbContext using MySQL
builder.Services.AddDbContext<MyDatabase>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    )
);

// Configure JWT Authentication
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

// Register services for Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInvestorService, InvestorService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IApplicantListService, ApplicantListService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IReportService, ReportService>();


// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Default to wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"C:\Users\T590\source\repos\Unit_Trust_Management_System\Unit_Trust_Backend\img"),
    RequestPath = "/img" // This is the URL path to access images
});

app.UseCors(); // Enable CORS with the default policy

app.UseHttpsRedirection();
app.UseAuthentication(); // Ensure Authentication is used before Authorization
app.UseAuthorization();

app.MapControllers(); // Map API controllers to routes

app.Run(); // Run the application
