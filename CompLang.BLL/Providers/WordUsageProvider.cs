using CompLang.BLL.Interfaces.Providers;
using CompLang.DAL.Entities;
using CompLang.DAL.Interfaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Providers
{
    public class WordUsageProvider: IWordUsageProvider
    {
        private readonly IWordUsageRepository _wordUsageRepository;
        public WordUsageProvider(IWordUsageRepository wordUsageRepository)
        {
            this._wordUsageRepository = wordUsageRepository;
        }

        public Task<WordUsageEntity> CreateAsync(WordEntity word, TextEntity text, CancellationToken ct = default)
        {
            return _wordUsageRepository.CreateAsync(new WordUsageEntity { Word = word, Text = text }, ct);
        }

        public Task<WordUsageEntity> UpdateAsync(WordUsageEntity entity, CancellationToken ct = default)
        {
            return _wordUsageRepository.UpdateAsync(entity, ct);
        }
    }
}