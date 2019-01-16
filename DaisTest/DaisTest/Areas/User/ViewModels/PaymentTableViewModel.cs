using DaisTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaisTest.Areas.User.ViewModels
{
	public class PaymentTableViewModel
	{
		public PaymentTableViewModel() { }

		public PaymentTableViewModel(Payment payment)
		{
			this.AccountPayable = payment.AccountNumberFrom;
			this.AccountReceivable = payment.AccountNumberTo;
			this.Ammount = payment.PaymentValue;
			this.Description = payment.Description;
			this.Status = payment.Status;
			this.Id = payment.Id;
			this.CreatedOn = payment.CreatedOn;
		}
		
		public string AccountPayable { get; set; }

		public string AccountReceivable { get; set; }

		public decimal Ammount { get; set; }

		public string Description { get; set; }

		public string Status { get; set; }

		public Guid Id { get; set; }

		public DateTime CreatedOn { get; set; }
	}
}
