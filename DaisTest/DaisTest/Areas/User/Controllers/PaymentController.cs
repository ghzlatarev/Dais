using DaisTest.Areas.User.ViewModels;
using DaisTest.Controllers;
using DaisTest.Data.Models;
using DaisTest.Data.Models.Identity;
using DaisTest.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace DaisTest.Areas.User.Controllers
{
	[Area("User")]
	public class PaymentController : Controller
	{
		private readonly IMemoryCache _memoryCache;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAccountService accountService;
		private readonly IPaymentService paymentService;

		public PaymentController(IPaymentService paymentService, IAccountService accountService, IMemoryCache memoryCache, UserManager<ApplicationUser> userManager)
		{
			this.paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
			this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}

		[HttpGet("mypayments")]
		public async Task<IActionResult> Index()
		{
			if (!this._memoryCache.TryGetValue("MyPayments", out IPagedList<Payment> payments))
			{
				string userId = _userManager.GetUserId(User);
				payments = await this.paymentService.ListPaymentsByUserId(userId);

				MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(25),
					SlidingExpiration = TimeSpan.FromSeconds(5)
				};

				this._memoryCache.Set("MyPayments", payments, options);
			}

			var model = new PaymentIndexViewModel(payments);

			return View(model);
		}

		[HttpGet("mypayments/filter")]
		public async Task<IActionResult> Filter(string sortOrder, string searchTerm, int? pageNumber, int? pageSize)
		{
			sortOrder = sortOrder ?? string.Empty;
			searchTerm = searchTerm ?? string.Empty;

			string userId = _userManager.GetUserId(User);
			var payments = await this.paymentService.ListPaymentsByUserId(userId, sortOrder);
			
			var model = new PaymentIndexViewModel(payments, sortOrder, searchTerm);

			return PartialView("_PaymentTablePartial", model.Table);
		}

		[HttpGet("registerpayment")]
		public async Task<IActionResult> Register(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			string userId = _userManager.GetUserId(User);
			var accounts = await this.accountService.FilterAccountsAsync(userId);
			var model = new RegisterViewModel(accounts);
			return View(model);
		}

		[HttpPost("registerpayment")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (ModelState.IsValid)
			{
				string userId = _userManager.GetUserId(User);
				var payment = await this.paymentService.AddPaymentAsync(userId, model.AccountPayable, model.AccountReceivable, model.Ammount, model.Description);
				
			}

			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		[HttpGet("sendpayment/{id}")]
		public async Task<IActionResult> Send(string accountNumber, decimal paymentValue, Guid id)
		{
			if (accountNumber == null)
			{
				throw new ApplicationException($"Passed ID parameter is absent.");
			}

			var account = await accountService.FindAccountByNumber(accountNumber);

			if(account.AccountBalance >= paymentValue)
			{
				await this.accountService.SendPaymentAsync(account, paymentValue);
				await this.paymentService.ApprovePaymentStatusAsync(id);
			}
			else
			{
				await this.paymentService.DenyPaymentStatusAsync(id);
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpGet("cancelpayment/{id}")]
		public async Task<IActionResult> Cancel(Guid id)
		{
			await this.paymentService.DenyPaymentStatusAsync(id);

			return RedirectToAction("Index", "Home");
		}
	}
}
