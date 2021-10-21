using CompLang.DAL.Context;
using CompLang.DAL.Entities;
using CompLang.DAL.Interfaces.Repository;
using CompLang.DAL.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.DAL.Repository
{
    public class TextRepository: GenericRepository<TextEntity>, ITextRepository
    {
        public TextRepository(DictionaryDbContext db) : base(db)
        {

        }
        public async Task<TextEntity> GetByTitleAsync(string title, bool toTrack = false, CancellationToken ct = default)
        {
            var words = await FindAsync(
                toTrack: toTrack,
                filters: new Expression<Func<TextEntity, bool>>[] { entity => title == entity.Title },
                include: q => q.Include(e => e.WordUsages).ThenInclude(e => e.Word),
                ct: ct).ConfigureAwait(false);
            return words.FirstOrDefault();
        }
    }
}
