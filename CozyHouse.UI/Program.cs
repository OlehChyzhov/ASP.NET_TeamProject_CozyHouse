using CozyHouse.Core.Domain.IdentityEntities;
using CozyHouse.Core.Helpers;
using CozyHouse.Core.RepositoryInterfaces;
using CozyHouse.Core.ServiceContracts;
using CozyHouse.Core.Services;
using CozyHouse.Infrastructure.Database;
using CozyHouse.Infrastructure.Helpers;
using CozyHouse.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// ������ ��������� ��� ����������
builder.Logging.AddConsole();

// �������� ���������� ��� ������������ ���������� ����� �������������
var connString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    builder.Configuration.GetConnectionString("SqliteConnectionString");

Console.WriteLine($"Connection string provider: {connString?.Split(';').FirstOrDefault() ?? "Not found"}");

// Add services to the container.
builder.Services.AddControllersWithViews();

// ������������ ���� ����� ������� �� ����������
if (builder.Environment.IsProduction())
{
    // �� Azure ������������� SQL Server
    var azureConnString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(azureConnString))
    {
        Console.WriteLine("Configuring SQL Server for Azure production environment");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(azureConnString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });
    }
    else
    {
        // ���� ���� Azure connection string, ������������� SQLite �� �������� ������
        Console.WriteLine("WARNING: Azure connection string not found, using SQLite as fallback");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnectionString"));
        });
    }
}
else
{
    // � ���������� �������� ������������� SQLite
    Console.WriteLine("Configuring SQLite for development environment");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnectionString"));
    });
}

// ��������� ������
builder.Services.AddScoped<IShelterPetPublicationRepository, ShelterPetPublicationRepository>();
builder.Services.AddScoped<IShelterPetPublicationService, ShelterPetPublicationService>();
builder.Services.AddScoped<IUserPetPublicationRepository, UserPetPublicationRepository>();
builder.Services.AddScoped<IUserPetPublicationService, UserPetPublicationService>();
builder.Services.AddScoped<IShelterAdoptionRequestRepository, ShelterAdoptionRequestRepository>();
builder.Services.AddScoped<IShelterAdoptionRequestService, ShelterAdoptionRequestService>();
builder.Services.AddScoped<IUserAdoptionRequestRepository, UserAdoptionRequestRepository>();
builder.Services.AddScoped<IUserAdoptionRequestService, UserAdoptionRequestService>();
builder.Services.AddScoped<IPetImageRepository, PetImageRepository>();
builder.Services.AddScoped<IAuthorizationManageService, AuthorizationManageService>();
builder.Services.AddScoped<IPublicationImageHelper, PublicationImageHelper>();
builder.Services.AddScoped<IUserStatsService, UserStatsService>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

// �� action ������ ����� �� �������, ���� �� �����������
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// ���� �� ����������, �� �������� ���� �� ������� ���������
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authorization/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// ������ ������� health check endpoint, ���� �� ������ ��������������
app.MapGet("/health", () => "Application is running!").AllowAnonymous();

// ����� �������� �������� ���������� ���� �����, ��� � �������� �������
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    // � Azure ���������� ����������� ���� ����� �� �������, ��� �������� ����������� �����������
    if (app.Environment.IsProduction())
    {
        logger.LogInformation("Running in Production mode. Using minimal database initialization to ensure application can start.");

        try
        {
            // ҳ���� ���������� ��������� ���������� �� ���� ����� ��� ��������� �������
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            logger.LogInformation("Testing database connection...");
            var canConnect = await dbContext.Database.CanConnectAsync();
            logger.LogInformation($"Database connection test: {(canConnect ? "SUCCESS" : "FAILED")}");

            if (canConnect)
            {
                // ���� ���������� ������, ������� �������� ������ ���������� ��� �������
                logger.LogInformation("Database connected. Checking if roles exist...");
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                try
                {
                    var userRoleExists = await roleManager.RoleExistsAsync(CozyHouse.Core.Domain.Enums.Roles.User.ToString());
                    logger.LogInformation($"User role exists: {userRoleExists}");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Could not check if User role exists, but application will continue");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database connection test, but application will continue");
        }
    }
    else
    {
        // � ���������� �������� �������� ����� �����������
        try
        {
            // ������������ �������
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            logger.LogInformation("Checking database connection...");
            if (await dbContext.Database.CanConnectAsync())
            {
                logger.LogInformation("Database connection successful");

                logger.LogInformation("Applying migrations...");
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully!");

                // ����������� ����� �� ������������
                logger.LogInformation("Seeding roles...");
                await AuthorizationHelper.SeedRolesAsync(services);
                logger.LogInformation("Roles seeded successfully!");

                logger.LogInformation("Seeding default manager...");
                await AuthorizationHelper.SeedDefaultManagerAsync(services);
                logger.LogInformation("Default manager seeded successfully!");
            }
            else
            {
                logger.LogError("Cannot connect to the database. Application will start without database initialization.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database initialization. Application will continue without database initialization.");
        }
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // ��������, �� ���������� ����������
app.UseAuthorization(); // ��������, �� ���������� ���� ����������� �� �������

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();