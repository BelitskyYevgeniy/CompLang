using AutoMapper;
using CompLang.BLL.Interfaces.Providers;
using CompLang.BLL.Interfaces.Services;
using CompLang.BLL.Models;
using CompLang.DAL.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Services
{
    public class TextService: ITextService
    {
        private readonly ITextProvider _textProvider;
        private readonly IWordProvider _wordProvider;
        private readonly IWordUsageProvider _wordUsageProvider;
        private readonly IMapper _mapper;
        private readonly IParseTextService _parseTextService;
       

       

        public TextService(ITextProvider textProvider, IWordProvider wordProvider, IWordUsageProvider wordUsageProvider, IParseTextService parseTextService, IMapper mapper)
        {
            this._mapper = mapper;
            this._parseTextService = parseTextService;
            this._wordProvider = wordProvider;
            this._wordUsageProvider = wordUsageProvider;
            this._textProvider = textProvider;
        }

        public async Task<Text> GetByTitleAsync(string title, CancellationToken ct = default)
        {
            var entity = await _textProvider.GetByTitleAsync(title, false, ct);
            if(entity == null)
            {
                return null;
            }
            var text = _mapper.Map<Text>(entity);
            StringBuilder sb = new StringBuilder();
            foreach(var wordUsage in entity.WordUsages)
            {
                sb.Append(wordUsage.Word.Name);
            }
            return text;
        }

        public async Task<Text> AddAsync(Text text, CancellationToken ct = default)
        {
            var wordsAndSymbols = _parseTextService.ParseTextAsync(text.Content, ct);
            var textEntity = _mapper.Map<TextEntity>(text);
            textEntity = await _textProvider.CreateAsync(textEntity, ct);
            foreach (var wordsOrSymbol in wordsAndSymbols.Result)
            {
                var existingWord = await _wordProvider.GetByNameAsync(wordsOrSymbol, ct);
                if(existingWord == null)
                {
                    existingWord = await _wordProvider.CreateAsync(wordsOrSymbol, ct);
                }
                var wordUsage = await _wordUsageProvider.CreateAsync(existingWord, textEntity, ct);
                textEntity.WordUsages.Add(wordUsage);
            }
            await _textProvider.UpdateAsync(textEntity, ct);
            return text;
        }
    }
}
