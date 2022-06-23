using CleverBit.Task1.Data.Shared.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CleverBit.Task1.Data.Shared.Concrete
{
    public class EFRepository<TEntity> : IEFRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly DbContext _context;
        public EFRepository(DbContext context)
        {
            _dbSet = context.Set<TEntity>();
            _context = context;
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = _dbSet.FirstOrDefault(predicate);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = _dbSet.FirstOrDefault(predicate);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = _dbSet.FirstOrDefault(predicate);
            return entity;
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            return entity;
        }

        public TEntity Get(int id)
        {
            var entity = _dbSet.FirstOrDefault(x => x.Id == id);
            return entity;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            var query = _dbSet.AsQueryable();
            return query;
        }

        public IQueryable<TEntity> GetAllIncluding<TProperty>(params Expression<Func<TEntity, TProperty>>[] propertySelectors)
        {
            var result = _dbSet.AsQueryable();

            for (int i = 0; i < propertySelectors.Length; i++)
                result = result.Include(propertySelectors[i]);

            return result;
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            var result = _dbSet.Where(predicate).ToList();
            return result;
        }

        public List<TEntity> GetAllList()
        {
            var result = _dbSet.ToList();
            return result.ToList();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await _dbSet.Where(predicate).ToListAsync();
            return result;
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            var result = await _dbSet.AsQueryable().ToListAsync();
            return result;
        }

        public TEntity Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public int Insert(ICollection<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            var affectedRowCount = _context.SaveChanges();

            return affectedRowCount;
        }

        public int InsertAndGetId(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public async Task<int> InsertAndGetIdAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<int> InsertAsync(ICollection<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            var affectedRowCount = await _context.SaveChangesAsync();

            return affectedRowCount;
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Single(predicate);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.SingleAsync(predicate);
        }

        public TEntity Update(TEntity entity)
        {
            _context.Entry<TEntity>(entity).State = EntityState.Modified;
            _context.SaveChanges();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry<TEntity>(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
