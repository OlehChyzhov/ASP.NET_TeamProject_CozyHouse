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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnectionString"));
});

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

// Всі action методи тепер не доступні, якщо не залогінитись
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// Якщо не залогінений, то перекинь його на сторінку логування
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await AuthorizationHelper.SeedRolesAsync(services);
    await AuthorizationHelper.SeedDefaultManagerAsync(services);
}

app.UseAuthentication(); // Перевірка, чи користувач залогінений
app.UseAuthorization(); // Перевірка, чи користувач може доступитись до сторінок

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
