using CompLang.DAL.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Interfaces.Providers
{
    public interface IWordUsageProvider
    {
        Task<WordUsageEntity> CreateAsync(WordUsageEntity entity, CancellationToken ct = default);
        Task<WordUsageEntity> UpdateAsync(WordUsageEntity entity, CancellationToken ct = default);
    }
}