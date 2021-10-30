using Newtonsoft.Json;
using System.Collections.Generic;

namespace CompLang.BLL.Models.ParserResponse
{
    public class ResultModelResponse
    {
        [JsonProperty("words")]
        public List<WordModelResponse> Words { get; set; }
    }
}
