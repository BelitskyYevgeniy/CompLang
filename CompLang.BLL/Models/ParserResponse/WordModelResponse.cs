using Newtonsoft.Json;

namespace CompLang.BLL.Models.ParserResponse
{
    public class WordModelResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}
