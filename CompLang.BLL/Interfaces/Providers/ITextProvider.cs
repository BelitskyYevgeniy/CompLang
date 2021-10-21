using CompLang.BLL.Models;
using CompLang.DAL.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Interfaces.Providers
{
    public interface ITextProvider
    {
        Task<TextEntity> GetByTitleAsync(string title, CancellationToken ct = default);
        Text MapEntityToModelAsync(TextEntity entity);

        Task<TextEntity> CreateAsync(TextEntity entity, CancellationToken ct = default);
        Task<TextEntity> UpdateAsync(TextEntity entity, CancellationToken ct = default);
        Task<TextEntity> GetByTitleAsync(string title, bool toTrack = false, CancellationToken ct = default);
    }
}
