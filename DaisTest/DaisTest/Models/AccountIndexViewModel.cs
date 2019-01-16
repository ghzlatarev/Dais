using DaisTest.Data.Models;
using DaisTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace DaisTest.Models
{
	public class AccountIndexViewModel
	{
		public AccountIndexViewModel(IPagedList<Account> accounts, string searchTerm = "")
		{
			this.Table = new TableViewModel<AccountTableViewModel>()
			{
				Items = accounts.Select(u => new AccountTableViewModel(u)),
				Pagination = new PaginationViewModel()
				{
					PageCount = accounts.PageCount,
					PageNumber = accounts.PageNumber,
					PageSize = accounts.PageSize,
					HasNextPage = accounts.HasNextPage,
					HasPreviousPage = accounts.HasPreviousPage,
					SearchTerm = searchTerm,
					ControllerRoute = "Home",
					ActionRoute = "Index"
				}
			};
		}

		public TableViewModel<AccountTableViewModel> Table { get; set; }
	}
}
