using System.Linq.Expressions;

namespace DailyDynamo.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        #region GET Methods

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filters);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(Expression<Func<T, bool>> filters);
        Task<T> GetAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties);

        #endregion


        #region Add, Update and Delete Methods

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

        #endregion

        #region Extensions or Miscellaneous Methods
        Task<bool> IsExistsAsync(Expression<Func<T, bool>> filter);
        #endregion

    }





}
