using DaisTest.Data.Models;
using DaisTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace DaisTest.Areas.User.ViewModels
{
	public class PaymentIndexViewModel
	{
		public PaymentIndexViewModel(IPagedList<Payment> payments, string sortOrder = "", string searchTerm = "")
		{
			this.Table = new TableViewModel<PaymentTableViewModel>()
			{
				Items = payments.Select(u => new PaymentTableViewModel(u)),
				Pagination = new PaginationViewModel()
				{
					PageCount = payments.PageCount,
					PageNumber = payments.PageNumber,
					PageSize = payments.PageSize,
					HasNextPage = payments.HasNextPage,
					HasPreviousPage = payments.HasPreviousPage,
					SearchTerm = searchTerm,
					SortOrder = sortOrder,
					AreaRoute = "User",
					ControllerRoute = "Payment",
					ActionRoute = "Filter"
				}
			};
		}

		public TableViewModel<PaymentTableViewModel> Table { get; set; }
	}
}
