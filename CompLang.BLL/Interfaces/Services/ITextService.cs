using CompLang.BLL.Models;
using CompLang.DAL.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Interfaces.Services
{
    public interface ITextService
    {
        Task<Text> AddAsync(Text text, CancellationToken ct = default);
        Task<Text> GetByTitleAsync(string title, CancellationToken ct = default);
    }
}
