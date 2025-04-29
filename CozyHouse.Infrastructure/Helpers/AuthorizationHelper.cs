using CozyHouse.Core.Domain.Enums;
using CozyHouse.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CozyHouse.Infrastructure.Helpers
{
    public static class AuthorizationHelper
    {
        public static string DefaultManagerId { get; } = "E7373406-2A07-487C-B052-1D9917DF39F8";
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            RoleManager<ApplicationRole> roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            string[] roles = { Roles.User.ToString(), Roles.Manager.ToString() };
            foreach (string role in roles)
            {
                if (await roleManager.RoleExistsAsync(role) == false)
                {
                    await roleManager.CreateAsync(new ApplicationRole() { Name = role });
                }
            }
        }

        public static async Task SeedDefaultManagerAsync(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? defaultManagerUser = await userManager.FindByIdAsync(DefaultManagerId);

            if (defaultManagerUser == null)
            {
                defaultManagerUser = new ApplicationUser()
                {
                    Id = Guid.Parse(DefaultManagerId),
                    UserName = "cozyHouse@gmail.com",
                    Email = "cozyHouse@gmail.com",
                    PersonName = "Default Manager",
                    PhoneNumber = "+380-63-72-19499",
                    Location = "Not your business",
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                await userManager.CreateAsync(defaultManagerUser, "CozyHouse");
                await userManager.AddToRoleAsync(defaultManagerUser, Roles.Manager.ToString());
            }
        }
    }
}
