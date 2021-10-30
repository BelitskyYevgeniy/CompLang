using AutoMapper;
using CompLang.BLL.Interfaces.Services;
using CompLang.BLL.Models;
using CompLang.BLL.Models.ParserResponse;
using CompLang.DAL.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

        public async Task<ResultModelResponse> ParseTextAsync(string text)
        {

            WebRequest request = WebRequest.Create("http://localhost:5000/texts");
            request.Method = "POST";
            string query = $"text={text}";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false));
            JObject data;
            if (response.StatusDescription == "OK")
            {
                var result = new List<KeyValuePair<string, string>>();
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    return JsonConvert.DeserializeObject<ResultModelResponse>(responseFromServer);                    
                }
            }
            throw new System.Exception(response.StatusCode.ToString());
        }
    }
}
