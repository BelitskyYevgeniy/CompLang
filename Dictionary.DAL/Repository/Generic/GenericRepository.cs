﻿using CompLang.DAL.Context;
using CompLang.DAL.Interfaces.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.DAL.Repository.Generic
{
    public class GenericRepository<TEntity> : IGenericRepositoryAsync<TEntity>
        where TEntity : class
    {
        protected readonly DictionaryDbContext _dbContext;
        protected virtual bool ValidatePagination(int start, int count)
        {
            return start >= 0 && count > 0;
        }

        public GenericRepository(DictionaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual Task<int> GetCountAsync(CancellationToken ct = default)
        {
            return _dbContext.Set<TEntity>().CountAsync(ct);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id, bool toTrack = false, CancellationToken ct = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(new object[] { id }, ct).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
        {
            var entityEntry = await _dbContext.Set<TEntity>().AddAsync(entity, ct).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
            return entityEntry.Entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            var entityEntry = _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
            return entityEntry.Entity;
        }
        public virtual async Task<IReadOnlyCollection<TEntity>> FindAsync(int skip = 0, int count = int.MaxValue,
            bool toTrack = false,
            IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            CancellationToken ct = default)
        {

            return await GetQuery(skip, count, toTrack, filters, orderBy, include, ct).ToListAsync(ct).ConfigureAwait(false);
        }

        public virtual IQueryable<TEntity> GetQuery(int skip = 0, int count = int.MaxValue,
            bool toTrack = false,
            IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            CancellationToken ct = default)
        {

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (!toTrack)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            query = query == null || orderBy == null ? query : orderBy(query);
            skip = skip < 0 || skip > int.MaxValue ? 0 : skip;
            count = count <= 0 || count > int.MaxValue ? int.MaxValue : count;
            return query.Skip(skip).Take(count);
            
        }

        public virtual async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct: ct).ConfigureAwait(false);
            if (entity != null)
            {
                return await DeleteAsync(entity).ConfigureAwait(false);
            }

            return false;
        }
        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default) //unsafe
        {

            _dbContext.Set<TEntity>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        public virtual async Task<bool> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            var entityEntry = _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
            return entityEntry != null;
        }

        public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(int start = 0, int count = int.MaxValue, CancellationToken ct = default)
        {
            return await _dbContext.Set<TEntity>().ToListAsync(ct).ConfigureAwait(false);
        }

        public virtual async Task<bool> DeleteAsync(object id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct: ct).ConfigureAwait(false);
            if(entity == null)
            {
                return false;
            }
            return await DeleteAsync(entity, ct: ct).ConfigureAwait(false);
        }
    }
}