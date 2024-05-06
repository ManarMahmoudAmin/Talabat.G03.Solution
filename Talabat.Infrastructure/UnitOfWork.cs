using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure._Data;

namespace Talabat.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbContext;
		private Hashtable _repositories;

		public UnitOfWork(StoreContext dbContext)
		{
			_dbContext = dbContext;
			_repositories = new Hashtable();
		}
		public async Task<int> Compelete()
			=> await _dbContext.SaveChangesAsync();


		public async ValueTask DisposeAsync()
		  => await _dbContext.DisposeAsync();

		public IGenaricRepository<T> Repository<T>() where T : BaseEntity
		{
			var key = typeof(T).Name;
			if (!_repositories.ContainsKey(key))
			{
				var repo = new GenericRepository<T>(_dbContext) as GenericRepository<BaseEntity>;
				_repositories.Add(key, repo);
			}
			return (IGenaricRepository<T>)_repositories[key];
		}
	}
}
