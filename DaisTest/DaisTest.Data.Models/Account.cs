using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DaisTest.Data.Models
{
	public class Account
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		
		public string AccountNumber { get; set; }

		[Range(0, double.MaxValue)]
		[Column(TypeName = "decimal(18,2)")]
		public decimal AccountBalance { get; set; }

		public IEnumerable<UsersAccounts> UsersAccounts { get; set; }

	}
}
