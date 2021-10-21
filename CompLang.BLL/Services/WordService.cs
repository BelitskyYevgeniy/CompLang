using AutoMapper;
using CompLang.BLL.Interfaces.Providers;
using CompLang.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Services
{
    public class WordService: IWordService
    {
        private readonly IWordProvider _wordProvider;
        private readonly IWordUsageProvider _wordUsageProvider;
        private readonly IMapper _mapper;
        public WordService(IWordProvider wordProvider, IWordUsageProvider wordUsageProvider, IMapper mapper)
        {
            this._wordProvider = wordProvider;
            this._wordUsageProvider = wordUsageProvider;
            this._mapper = mapper;
        }
        public async Task<Word> EditAsync(string name, string newName, CancellationToken ct = default)
        {
            newName = newName.ToLower();
            var oldEntity = await _wordProvider.GetByNameAsync(name, ct).ConfigureAwait(false);
            if (oldEntity == null)
            {
                return null;
            }
            var newEntity = await _wordProvider.GetByNameAsync(newName, ct).ConfigureAwait(false);
            if (newEntity == null)
            {
                newEntity = oldEntity;
                newEntity.Name = newName;
            }
            else
            {
                var usages = oldEntity.WordUsages;
                await _wordProvider.DeleteAsync(oldEntity, ct).ConfigureAwait(false);
                
                foreach (var usage in usages)
                {
                    usage.Word = newEntity;
                    newEntity.WordUsages.Add(await _wordUsageProvider.UpdateAsync(usage, ct));
                }
                
            }
            newEntity = await _wordProvider.UpdateAsync(newEntity, ct).ConfigureAwait(false);
            return _mapper.Map<Word>(newEntity);

        }
    }
}
