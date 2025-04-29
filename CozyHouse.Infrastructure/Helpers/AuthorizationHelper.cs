using CozyHouse.Core.Domain.Enums;
using CozyHouse.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CozyHouse.Infrastructure.Helpers
{
    public static class AuthorizationHelper
    {
        public static string DefaultManagerId { get; } = "E7373406-2A07-487C-B052-1D9917DF39F8";

        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<IdentityRole>>();
            try
            {
                logger?.LogInformation("Starting role seeding process");
                RoleManager<ApplicationRole> roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                string[] roles = { Roles.User.ToString(), Roles.Manager.ToString() };

                foreach (string role in roles)
                {
                    logger?.LogInformation($"Checking if role {role} exists");
                    bool roleExists = false;

                    try
                    {
                        roleExists = await roleManager.RoleExistsAsync(role);
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, $"Error checking if role {role} exists");
                        continue; 
                    }

                    if (!roleExists)
                    {
                        logger?.LogInformation($"Creating role {role}");
                        try
                        {
                            var result = await roleManager.CreateAsync(new ApplicationRole() { Name = role });
                            if (result.Succeeded)
                            {
                                logger?.LogInformation($"Role {role} created successfully");
                            }
                            else
                            {
                                logger?.LogWarning($"Failed to create role {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger?.LogError(ex, $"Error creating role {role}");
                        }
                    }
                    else
                    {
                        logger?.LogInformation($"Role {role} already exists");
                    }
                }

                logger?.LogInformation("Role seeding completed");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error occurred during role seeding");
            }
        }

        public static async Task SeedDefaultManagerAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<ApplicationUser>>();
            try
            {
                logger?.LogInformation("Starting default manager seeding process");
                UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                ApplicationUser? defaultManagerUser = null;
                try
                {
                    defaultManagerUser = await userManager.FindByIdAsync(DefaultManagerId);
                    logger?.LogInformation($"Searching for default manager by ID: {(defaultManagerUser != null ? "found" : "not found")}");
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "Error finding default manager by ID");
                }

                if (defaultManagerUser == null)
                {
                    try
                    {
                        defaultManagerUser = await userManager.FindByEmailAsync("cozyHouse@gmail.com");
                        logger?.LogInformation($"Searching for default manager by email: {(defaultManagerUser != null ? "found" : "not found")}");
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "Error finding default manager by email");
                    }
                }

                if (defaultManagerUser == null)
                {
                    logger?.LogInformation("Creating default manager account");
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

                    try
                    {
                        var result = await userManager.CreateAsync(defaultManagerUser, "CozyHouse");
                        if (result.Succeeded)
                        {
                            logger?.LogInformation("Default manager created successfully");

                            try
                            {
                                var roleResult = await userManager.AddToRoleAsync(defaultManagerUser, Roles.Manager.ToString());
                                if (roleResult.Succeeded)
                                {
                                    logger?.LogInformation("Manager role assigned to default manager");
                                }
                                else
                                {
                                    logger?.LogWarning($"Failed to assign Manager role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                                }
                            }
                            catch (Exception ex)
                            {
                                logger?.LogError(ex, "Error assigning Manager role to default manager");
                            }
                        }
                        else
                        {
                            logger?.LogWarning($"Failed to create default manager: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "Error creating default manager");
                    }
                }
                else
                {
                    logger?.LogInformation("Default manager already exists");

                    try
                    {
                        bool isInRole = await userManager.IsInRoleAsync(defaultManagerUser, Roles.Manager.ToString());
                        if (!isInRole)
                        {
                            logger?.LogInformation("Adding Manager role to existing default manager");
                            var roleResult = await userManager.AddToRoleAsync(defaultManagerUser, Roles.Manager.ToString());
                            if (roleResult.Succeeded)
                            {
                                logger?.LogInformation("Manager role assigned to existing default manager");
                            }
                            else
                            {
                                logger?.LogWarning($"Failed to assign Manager role to existing manager: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "Error checking or assigning role to existing default manager");
                    }
                }

                logger?.LogInformation("Default manager seeding completed");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error occurred during default manager seeding");
            }
        }
    }
}