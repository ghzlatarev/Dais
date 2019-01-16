using DaisTest.Data.Models;
using DaisTest.Data.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaisTest.Data.Context.Contracts
{
	public interface IDataContext : IDisposable
	{
		DbSet<ApplicationUser> Users { get; set; }

		DbSet<Account> Accounts { get; set; }

		DbSet<UsersAccounts> UsersAccounts { get; set; }

		DbSet<Payment> Payments { get; set; }


		int SaveChanges();

		Task<int> SaveChangesAsync(bool applyDeletionRules, bool applyAuditInfoRules);

		DbSet<TEntity> Set<TEntity>() where TEntity : class;

		EntityEntry Entry(object entity);
	}
}
