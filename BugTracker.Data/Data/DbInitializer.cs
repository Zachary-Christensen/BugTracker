using BugTracker.Data.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Data.Data
{
    public class DbInitializer
    {
        public static async Task PopulateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            List<string> roleNames = new List<string> { "Admin", "ProjectManager", "Developer", "Submitter",
            "Demo_Admin", "Demo_ProjectManager", "Demo_Developer", "Demo_Submitter" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            await CreateDemoUsers(serviceProvider, configuration, roleNames.FindAll(
                delegate (string role)
                {
                    return role.StartsWith("Demo");
                }));
            //await CreateSuperUser(serviceProvider, configuration);
        }

        private static async Task CreateDemoUsers(IServiceProvider serviceProvider, IConfiguration configuration, List<string> demoRoleNames)
        {
            var password = "DemoUser1!";

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var bugTrackerCtx = serviceProvider.GetRequiredService<BTContext>();

            foreach (var roleName in demoRoleNames)
            {
                var demoUser = new User
                {
                    UserName = roleName,
                    Email = roleName + "@email.com"
                };

                User user = await userManager.FindByEmailAsync(demoUser.Email);

                if (user == null)
                {


                    IdentityResult createSuperUser = await userManager.CreateAsync(demoUser, password);
                    if (createSuperUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(demoUser, roleName);
                        await userManager.AddToRoleAsync(demoUser, "Demo_Submitter");
                        Console.WriteLine(roleName + " created");
                    }
                    else
                    {
                        Console.WriteLine(roleName + " not created");
                    }
                }
            }
        }
    }
}
