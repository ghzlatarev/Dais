using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DaisTest.Configurations;
using DaisTest.Data.Models.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DaisTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);

			SeedData(host).GetAwaiter().GetResult();
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();

		private static async Task SeedData(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
					var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					await IdentityDataInitializer.SeedDataAsync(_userManager, _roleManager);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}
		}
	}
}
