using DaisTest.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DaisTest.Data.Models
{
	public class UsersAccounts
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public Guid AccountId { get; set; }
		public Account Account { get; set; }

	}
}
