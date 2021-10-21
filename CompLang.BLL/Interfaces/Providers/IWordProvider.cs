using CompLang.BLL.Models;
using CompLang.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Interfaces.Providers
{
    public interface IWordProvider
    {
        IEnumerable<Word> MapToWord(params WordEntity[] entities);
        Task<int> GetCountAsync(CancellationToken ct = default);
        Task<int> GetCountByNamesAsync(IEnumerable<string> likeNames = null,
            CancellationToken ct = default);
        Task<bool> DeleteAsync(string name, CancellationToken ct = default);
        Task<bool> DeleteAsync(WordEntity entity, CancellationToken ct = default);
        Task<WordEntity> UpdateAsync(WordEntity entity, CancellationToken ct = default);
        Task<IEnumerable<Word>> GetAsync(int start = 0, int count = int.MaxValue,
           bool withoutSymbols = true,
           SortType sortType = SortType.Name,
           SortDirection sortDirection = SortDirection.Ascending,
           IEnumerable<string> likeNames = null,
           CancellationToken ct = default);
        Task<WordEntity> GetByNameAsync(string name, CancellationToken ct = default);
        Task<WordEntity> CreateAsync(string name, CancellationToken ct = default);
        bool ValidateWord(string word);
    }
}
