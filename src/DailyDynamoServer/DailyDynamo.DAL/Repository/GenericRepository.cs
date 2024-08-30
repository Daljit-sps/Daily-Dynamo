using DailyDynamo.DAL.Context;
using DailyDynamo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DailyDynamo.DAL.Repository
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        private readonly DailyDynamoDatabaseContext _context;

        public GenericRepository(DailyDynamoDatabaseContext context)
        {
            _context = context;

        }
        #region GET Methods

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filters)
        {
            return await _context.Set<T>().Where(filters).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            var data = _context.Set<T>();
            foreach (var property in includeProperties)
                await data.Include(property).LoadAsync();

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties)
        {
            var filteredData = _context.Set<T>().Where(filters);
            if (includeProperties != null)
                foreach (var property in includeProperties)
                    await filteredData.Include(property).LoadAsync();


            return await filteredData.ToListAsync();

        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filters)
        {
            return await _context.Set<T>().Where(filters).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
                await _context.Set<T>().Include(property).LoadAsync();

            return await _context.Set<T>().FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includeProperties)
        {

            var filteredData = _context.Set<T>().Where(filters);
            if (includeProperties != null)
                foreach (var property in includeProperties)
                    await filteredData.Include(property).LoadAsync();


            return await filteredData.FirstOrDefaultAsync();
        }

        #endregion

        #region Add, Update and Delete Methods
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                await _context.AddAsync(entity);
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
            return entities;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            return entity;
        }


        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }

        #endregion

        #region Extensions or Miscellaneous Methods

        public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AnyAsync(filter);
        }

        #endregion
    }
}
