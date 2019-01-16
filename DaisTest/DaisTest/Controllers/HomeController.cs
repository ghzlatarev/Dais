using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DaisTest.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Identity;
using DaisTest.Services.Contracts;
using DaisTest.Data.Models.Identity;
using X.PagedList;
using DaisTest.Data.Models;

namespace DaisTest.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMemoryCache _memoryCache;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAccountService accountService;

		public HomeController(IAccountService accountService, IMemoryCache memoryCache, UserManager<ApplicationUser> userManager)
		{
			this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}

		public async Task<IActionResult> Index()
		{
			string id = _userManager.GetUserId(User);
			if (!_memoryCache.TryGetValue("ListOfAccounts", out IPagedList<Account> accounts))
			{
				accounts = await this.accountService.FilterAccountsAsync(id);

				MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(25),
					SlidingExpiration = TimeSpan.FromSeconds(5)
				};

				_memoryCache.Set("ListOfAccounts", accounts, options);
			}

			var model = new AccountIndexViewModel(accounts);

			return View(model);
		}
	}
}
