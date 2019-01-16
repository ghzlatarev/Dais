using DaisTest.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DaisTest.Areas.User.ViewModels
{
	public class RegisterViewModel
	{
		public RegisterViewModel() { }

		public RegisterViewModel(IEnumerable<Account> userAccounts)
		{
			this.userAccounts = userAccounts;
		}

		[Required]
		[MinLength(22)]
		[MaxLength(22)]
		[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
		[Display(Name = "Account receivable")]
		public string AccountReceivable { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required]
		[RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Amount can't have more than 2 decimal places")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive")]
		[Display(Name = "Ammount")]
		public decimal Ammount { get; set; }

		[Required]
		[Display(Name = "Account payable")]
		public string AccountPayable { get; set; }

		public IEnumerable<Account> userAccounts { get; set; }
	}
}
