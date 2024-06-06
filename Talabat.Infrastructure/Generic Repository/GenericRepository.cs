using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure.Generic_Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            

            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }


        public async Task<T?> GetAsync(int id)
        {
           ///if (typeof(T) == typeof(Product))
           ///    return await _dbContext.Set<Product>().Where(P => P.Id == id).Include(P => P.Brand)
           ///        .Include(P => P.Category).FirstOrDefaultAsync() as T;          
           /////return await dbContext.Set<T>().FindAsync(id);
            return await _dbContext.FindAsync<T>(id);


        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
            => SpecificationsEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec);

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

		public void Add(T entity)
			=> _dbContext.Add(entity);

		public void Update(T entity)
			=> _dbContext.Update(entity);

		public void Delete(T entity)
			=> _dbContext.Remove(entity);
	}
}
