using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Data.Repository
{
    public interface IGenericRepo<T> where T : class
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }

        Task DeleteAsync(Guid id);
        Task DeleteAsync(T entityToDelete);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAysnc(Guid id);
        Task<bool> InsertAsync(T entityToInsert);
        Task UpdateAsync(T entityToUpdate);
    }
}
