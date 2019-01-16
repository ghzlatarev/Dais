using DaisTest.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DaisTest.Configurations
{
    public static class IdentityDataInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("User"))
            {
                IdentityRole newRole = new IdentityRole() { Name = "User" };
                await roleManager.CreateAsync(newRole);
            }

            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                IdentityRole newRole = new IdentityRole() { Name = "Administrator" };
                await roleManager.CreateAsync(newRole);
            }
        }
    }
}
