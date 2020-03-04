using DrawManager.Domain.Entities;
using DrawManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DrawManager.Database.SqlServer.Repositories
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        protected DbContext _context;
        protected readonly DbSet<TEntity> _dbset;

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return _dbset.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.FirstOrDefaultAsync(predicate);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbset.AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.Where(predicate).AsEnumerable();
        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return _dbset.ToListAsync();
        }

        public Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.Where(predicate).ToListAsync();
        }
    }
}
