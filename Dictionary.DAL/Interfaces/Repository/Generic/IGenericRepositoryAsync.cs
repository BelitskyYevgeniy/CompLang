using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;


namespace CompLang.DAL.Interfaces.Repository.Generic
{
    public interface IGenericRepositoryAsync<TEntity>
        where TEntity : class
    {
        Task<int> GetCountAsync(CancellationToken ct = default);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(int start = 0, int count = int.MaxValue, CancellationToken ct = default);
        Task<TEntity> GetByIdAsync(object id, bool toTrack = false, CancellationToken ct = default);
        Task<IReadOnlyCollection<TEntity>> FindAsync(int skip = 0, int count = int.MaxValue,
            bool toTrack = false,
            IEnumerable<Expression<Func<TEntity, bool>>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            CancellationToken ct = default);
        IQueryable<TEntity> GetQuery(int skip = 0, int count = int.MaxValue,
            bool toTrack = false,
            IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            CancellationToken ct = default);
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(object id, CancellationToken ct = default);
        Task<bool> DeleteAsync(TEntity entity, CancellationToken ct = default);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default);
        
    }
}
