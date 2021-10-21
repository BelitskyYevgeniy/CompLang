using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Interfaces.Services
{
    public interface IParseTextService
    {
        Task<string[]> ParseTextAsync(string text, CancellationToken ct = default);
    }
}
