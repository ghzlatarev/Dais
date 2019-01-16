using DaisTest.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DaisTest.Data.Models
{
	public class Payment
	{
		//	id

		//FromAccountId/number

		//ToAccountID/number
		//Ammount

		//Description
		//PaymentStatusId
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string AccountNumberFrom { get; set; }

		public string AccountNumberTo { get; set; }

		[Range(0, double.MaxValue)]
		[Column(TypeName = "decimal(18,2)")]
		public decimal PaymentValue { get; set; }

		public string Description { get; set; }

		public string Status { get; set; }

		public string UserId { get; set; }

		public ApplicationUser User { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime CreatedOn { get; set; }

	}
}
