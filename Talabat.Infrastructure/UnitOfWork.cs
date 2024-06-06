using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure;
using Talabat.Infrastructure.Generic_Repository;

namespace Talabat.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;
		private Hashtable _repositories;

		public UnitOfWork(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			_repositories = new Hashtable();
		}
		public async Task<int> Compelete()
			=> await _dbContext.SaveChangesAsync();


		public async ValueTask DisposeAsync()
		  => await _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(key))
			{
                var repo = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(key, repo);
			}
            return _repositories[key] as IGenericRepository<TEntity>;
        }
    }
}
