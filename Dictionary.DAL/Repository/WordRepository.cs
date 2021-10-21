using CompLang.DAL.Context;
using CompLang.DAL.Entities;
using CompLang.DAL.Interfaces.Repository;
using CompLang.DAL.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.DAL.Repository
{
    public class WordRepository: GenericRepository<WordEntity>, IWordRepository
    {
        public WordRepository(DictionaryDbContext db) : base(db)
        {
            
        }
        public async Task<int> GetCountByNamesAsync(IEnumerable<string> likeNames = null,
            CancellationToken ct = default)
        {
            List<Expression<Func<WordEntity, bool>>> filters = new List<Expression<Func<WordEntity, bool>>>()
            { 
                e => "abcdefghijklmnopqrstuvwxyz".Contains(e.Name.Substring(0, 1))
            };
            if (likeNames != null)
            {
                filters.Add(e => likeNames.Any(w => e.Name.Contains(w)));
            }
            return await GetQuery(0, int.MaxValue, false, filters, null, null, ct).CountAsync(ct).ConfigureAwait(false);
        }
        public override async Task<int> GetCountAsync(CancellationToken ct = default)
        {
            IEnumerable<Expression<Func<WordEntity, bool>>> filters = new List<Expression<Func<WordEntity, bool>>>()
            {
                e => "abcdefghijklmnopqrstuvwxyz".Contains(e.Name.Substring(0, 1))
            };
            return await GetQuery(0, int.MaxValue, false, filters, null, null, ct).CountAsync(ct).ConfigureAwait(false);

        }
        public async Task<WordEntity> GetByNameAsync(string name, bool toTrack = false, CancellationToken ct = default)
        {
            var comparableName = name.ToLower();
            var words = await FindAsync(
                toTrack: toTrack,
                filters: new Expression<Func<WordEntity, bool>>[] { entity => comparableName == entity.Name },
                include: q => q.Include(e => e.WordUsages),
                ct: ct).ConfigureAwait(false);
            return words.FirstOrDefault();
        }

        public override Task<WordEntity> CreateAsync(WordEntity entity, CancellationToken ct = default)
        {
            entity.Name = entity.Name.ToLower();
            return base.CreateAsync(entity, ct);
        }
        

    }
}
