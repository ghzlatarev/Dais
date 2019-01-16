using DaisTest.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaisTest.Models
{
	public class AccountTableViewModel
	{
		public AccountTableViewModel(Account account)
		{
			this.AccountNumber = account.AccountNumber;
			this.AccountBalance = account.AccountBalance;
			this.Id = account.Id;
		}

		public string AccountNumber { get; set; }

		public decimal AccountBalance { get; set; }

		public Guid Id { get; set; }
	}
}
