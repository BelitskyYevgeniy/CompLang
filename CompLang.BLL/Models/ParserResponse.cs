using Newtonsoft.Json;

namespace CompLang.BLL.Models.ParserResponse
{
    public class ParserResponse
    {
        [JsonProperty("word")]
        public string Word { get; set; }
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}
