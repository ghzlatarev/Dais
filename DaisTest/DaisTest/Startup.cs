using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaisTest.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DaisTest.Data.Context;
using DaisTest.Data.Models.Identity;
using DaisTest.Services.Contracts;
using DaisTest.Services;

namespace DaisTest
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment environment)
		{
			Configuration = configuration;
			HostingEnvironment = environment;
		}

		public IConfiguration Configuration { get; }

		public IHostingEnvironment HostingEnvironment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			RegisterData(services);

			RegisterAuthentication(services);
			
			RegisterServicesData(services);

			RegisterInfrastructure(services);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				//app.UseNotFoundExceptionHandler();
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "internalservererror",
					template: "500",
					defaults: new { controller = "Error", action = "InternalServerError" });

				routes.MapRoute(
					name: "notfound",
					template: "404",
					defaults: new { controller = "Error", action = "PageNotFound" });

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}");
			});
		}

		private void RegisterData(IServiceCollection services)
		{
			if (HostingEnvironment.IsDevelopment())
			{
				services.AddDbContext<DataContext>(options =>
					 options.UseSqlServer(Configuration.GetConnectionString("DevelopmentConnection")));
			}
			else
			{
				services.AddDbContext<DataContext>(options =>
					 options.UseSqlServer(Environment.GetEnvironmentVariable("AZURE_ESS_DB_Connection")));
			}
		}

		private void RegisterAuthentication(IServiceCollection services)
		{
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<DataContext>()
				.AddDefaultTokenProviders();

			if (this.HostingEnvironment.IsDevelopment())
			{
				services.Configure<IdentityOptions>(options =>
				{
					// Password settings
					options.Password.RequireDigit = false;
					options.Password.RequiredLength = 3;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
					options.Password.RequireLowercase = false;
					options.Password.RequiredUniqueChars = 0;

					// Lockout settings
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
					options.Lockout.MaxFailedAccessAttempts = 999;
				});
			}

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Admin", policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireRole("Administrator");
				});

				options.AddPolicy("Default", policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireRole("User", "Administrator");
				});
			});
		}

		private void RegisterServicesData(IServiceCollection services)
		{
			//// Identity
			services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
			services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();

			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IPaymentService, PaymentSerrvice>();
		}

		private void RegisterInfrastructure(IServiceCollection services)
		{
			services.AddRouting(options => options.LowercaseUrls = true);

			services.AddResponseCaching();
			services.AddMemoryCache();

			services.AddMvc(options =>
			{
				options.CacheProfiles.Add("Default",
					new CacheProfile()
					{
						Duration = 60
					});

				options.CacheProfiles.Add("Short",
					new CacheProfile()
					{
						Duration = 30
					});
			});
		}
	}
}
