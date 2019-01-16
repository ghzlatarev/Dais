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
	public class PaymentSerrvice : IPaymentService
	{
		private readonly DataContext dataContext;

		public PaymentSerrvice(DataContext dataContext)
		{
			this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
		}

		public async Task<Payment> AddPaymentAsync(string userId, string accountPayable, string accountReceivable, decimal ammount, string description, string status = "Pending")
		{
			var payment = new Payment
			{
				AccountNumberFrom = accountPayable,
				AccountNumberTo = accountReceivable,
				PaymentValue = ammount,
				Description = description,
				Status = status,
				UserId = userId,
				CreatedOn = DateTime.Now
			};

			await this.dataContext.Payments.AddAsync(payment);
			await this.dataContext.SaveChangesAsync();
			return payment;
		}

		public async Task ApprovePaymentStatusAsync(Guid id)
		{
			var payment = await this.dataContext.Payments
				.FirstAsync(x => x.Id == id);

			payment.Status = "Approved";
			this.dataContext.Update(payment);
			await this.dataContext.SaveChangesAsync();
		}

		public async Task DenyPaymentStatusAsync(Guid id)
		{
			var payment = await this.dataContext.Payments
				.FirstAsync(x => x.Id == id);

			payment.Status = "Denied";
			this.dataContext.Update(payment);
			await this.dataContext.SaveChangesAsync();
		}

		public async Task<IPagedList<Payment>> ListPaymentsByUserId( string userId, string sortOrder = "", int pageNumber = 1, int pageSize = 10)
		{
			var query = this.dataContext.Payments
				.Where(p => p.UserId.Equals(userId));

			switch (sortOrder)
			{
				case "date_asc":
					query = query.OrderBy(c => c.CreatedOn);
					break;
				case "date_desc":
					query = query.OrderByDescending(c => c.CreatedOn);
					break;
			}

			return await query.ToPagedListAsync(pageNumber, pageSize);
		}
	}
}
