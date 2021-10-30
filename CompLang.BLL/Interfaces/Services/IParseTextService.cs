using CompLang.BLL.Models.ParserResponse;
using System.Threading.Tasks;

namespace CompLang.BLL.Interfaces.Services
{
    public interface IParseTextService
    {
        Task<ResultModelResponse> ParseTextAsync(string text);
    }
}
