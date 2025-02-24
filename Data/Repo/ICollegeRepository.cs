using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CollegeApp.Data.Repo
{
    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        //Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);
        Task<T> CreateAsync(T dbRecord);
        Task<T> UpdateAsync(T dbRecord);
        Task<bool> DeleteAsync(T dbRecord);

    }
}
