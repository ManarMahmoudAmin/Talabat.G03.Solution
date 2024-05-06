using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Core
{
	public interface IUnitOfWork : IAsyncDisposable
	{
		public IGenaricRepository<T> Repository<T>() where T : BaseEntity;
		public Task<int> Compelete();
	}
}
