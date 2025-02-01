using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct
    {
        protected AppDbContext Context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            Context = context;
            _dbSet = context.Set<T>();
        }

        public async ValueTask AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public async ValueTask<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking();
        }

        public async ValueTask<bool> AnyAsync(TId id)
        {
            return await _dbSet.AnyAsync(x => x.Id.Equals(id));
        }
    }
}
