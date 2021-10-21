using CompLang.DAL.Entities;
using CompLang.DAL.Interfaces.Repository.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.DAL.Interfaces.Repository
{
    public interface ITextRepository: IGenericRepositoryAsync<TextEntity>
    {
        Task<TextEntity> GetByTitleAsync(string title, bool toTrack = false, CancellationToken ct = default);
    }
}
