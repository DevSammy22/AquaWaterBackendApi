using AquaWater.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaWater.Data.Repository.Implementations
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private AppDbContext _appDbContext { get; set; }
        private DbSet<T> _dbSet;
        public GenericRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAysnc(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task<bool> InsertAsync(T entityToInsert)
        {
            await _dbSet.AddAsync(entityToInsert);
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task UpdateAsync(T entityToUpdate)
        {
            _dbSet.Update(entityToUpdate);
            await _appDbContext.SaveChangesAsync();
        }
        public virtual async Task DeleteAsync(Guid id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                await DeleteAsync(entityToDelete);
                return;
            }
            var typeName = typeof(T).Name;
            throw new ArgumentException($"{typeName} with Id {id} does not exist");
        }
        public async Task DeleteAsync(T entityToDelete)
        {
            _dbSet.Remove(entityToDelete);
            await _appDbContext.SaveChangesAsync();
        }

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_dbSet == null)
                    _dbSet = _appDbContext.Set<T>();
                return _dbSet;
            }
        }
        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Entities.AsNoTracking();
            }
        }
    }
}
