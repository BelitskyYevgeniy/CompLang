using CompLang.BLL.Interfaces.Services;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Services
{
    public class ParseTextService: IParseTextService
    {
        public ParseTextService()
        {

        }

        public Task<string[]> ParseTextAsync(string text, CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                var list = new List<string>();
                foreach (var match in Regex.Matches(text, @"[a-zA-Z]+|\s|\W"))
                {
                    list.Add(match.ToString());
                }
                return list.ToArray();
            }, ct);
        }
    }
}
