using DaisTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace DaisTest.Services.Contracts
{
	public interface IPaymentService
	{
		Task<Payment> AddPaymentAsync(string userId, string accountPayable, string accountReceivable, decimal ammount, string description, string status = "Pending");

		Task<IPagedList<Payment>> ListPaymentsByUserId(string userId, string sortOrder = "", int pageNumber = 1, int pageSize = 10);

		Task ApprovePaymentStatusAsync(Guid id);

		Task DenyPaymentStatusAsync(Guid id);
	}
}
