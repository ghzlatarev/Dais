using DaisTest.Data.Context;
using DaisTest.Data.Models;
using DaisTest.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace DaisTest.Services
{
	public class AccountService : IAccountService
	{
		private readonly DataContext dataContext;

		public AccountService(DataContext dataContext)
		{
			this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
		}

		public async Task<IPagedList<Account>> FilterAccountsAsync(string id, string filter = "", int pageNumber = 1, int pageSize = 10)
		{

			var query = this.dataContext.UsersAccounts
				.Where(ua => ua.UserId == id)
				.Select(a => a.Account);

			return await query.ToPagedListAsync(pageNumber, pageSize);
		}

		public async Task<Account> FindAccountByNumber(string accountNumber)
		{
			return await this.dataContext.Accounts
				.FirstAsync(a => a.AccountNumber == accountNumber);
		}

		public async Task<Account> SendPaymentAsync(Account accountPayable, decimal paymentValue)
		{
			accountPayable.AccountBalance -= paymentValue;

			this.dataContext.Accounts.Update(accountPayable);
			await this.dataContext.SaveChangesAsync();
			return accountPayable;
		}
	}
}
