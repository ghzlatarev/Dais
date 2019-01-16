using DaisTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace DaisTest.Services.Contracts
{
	public interface IAccountService
	{
		Task<IPagedList<Account>> FilterAccountsAsync(string id, string filter = "", int pageNumber = 1, int pageSize = 10);

		Task<Account> FindAccountByNumber(string accountNumber);

		Task<Account> SendPaymentAsync(Account accountPayable, decimal paymentValue);
	}
}
