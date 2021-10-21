using CompLang.DAL.Entities;
using CompLang.DAL.Interfaces.Repository.Generic;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.DAL.Interfaces.Repository
{
    public interface IWordRepository: IGenericRepositoryAsync<WordEntity>
    {
        Task<int> GetCountByNamesAsync(IEnumerable<string> likeNames = null,
             CancellationToken ct = default);
        Task<WordEntity> GetByNameAsync(string name, bool toTrack = false, CancellationToken ct = default);
    }
}
