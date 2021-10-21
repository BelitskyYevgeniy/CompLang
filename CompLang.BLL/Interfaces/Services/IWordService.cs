using CompLang.BLL.Models;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Services
{
    public interface IWordService
    {
        Task<Word> EditAsync(string name, string newName, CancellationToken ct = default);

    }
}