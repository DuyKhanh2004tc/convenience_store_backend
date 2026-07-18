using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.Contracts.External;
using ConvenienceStore.Application.Services;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Infrastructure.Data;
using ConvenienceStore.Infrastructure.ExternalServices.Jwt;
using ConvenienceStore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Hangfire;
using ConvenienceStore.Infrastructure.BackgroundJobs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure EF Core DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// Configure Repositories & UnitOfWork
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBranchOrderService, BranchOrderService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRevenueReportJob, RevenueReportJob>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ConvenienceStore.Application.Contracts.External.ICacheService, ConvenienceStore.Infrastructure.ExternalServices.Caching.RedisCacheService>();
builder.Services.AddScoped<ConvenienceStore.Application.Services.Interfaces.ICategoryService, ConvenienceStore.Application.Services.CategoryService>();
builder.Services.AddScoped<ConvenienceStore.Application.Services.Interfaces.IProductService, ConvenienceStore.Application.Services.ProductService>();
builder.Services.AddScoped<ConvenienceStore.Application.Services.Interfaces.IBranchService, ConvenienceStore.Application.Services.BranchService>();
builder.Services.AddScoped<ConvenienceStore.Application.Services.Interfaces.ISupplierService, ConvenienceStore.Application.Services.SupplierService>();

// HR, Shift Scheduling & Attendance Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

// Configure Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services.AddHangfireServer();

// Configure Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisConnection"];
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ConvenienceStore.Application.Contracts.External.ICacheService>();
            var jti = context.Principal?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti)?.Value;
            
            if (!string.IsNullOrEmpty(jti))
            {
                var isBlacklisted = await cacheService.GetAsync($"blacklist:{jti}");
                if (!string.IsNullOrEmpty(isBlacklisted))
                {
                    context.Fail("This token has been blacklisted.");
                }
            }
        }
    };
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ConvenienceStore.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await DbInitializer.InitializeAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.UseHttpsRedirection();
app.UseCors("AllowFlutterApp");

// Commented out HTTPS redirect to allow HTTP connections from Flutter during development
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire");

// Schedule Revenue Report Job
RecurringJob.AddOrUpdate<IRevenueReportJob>("daily-revenue-job", job => job.CalculateDailyRevenueAsync(), "59 23 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

app.MapControllers();

app.Run();
