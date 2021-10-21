using AutoMapper;
using CompLang.BLL.Interfaces.Providers;
using CompLang.BLL.Models;
using CompLang.DAL.Entities;
using CompLang.DAL.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Providers
{
    public class WordProvider :  IWordProvider
    {
        private IWordRepository _wordRepository;
        private IMapper _mapper;
        public WordProvider(IWordRepository wordRepository, IMapper mapper)
        {
            this._wordRepository = wordRepository;
            this._mapper = mapper;
        }

        public IEnumerable<Word> MapToWord(params WordEntity[] entities)
        {
            int count = entities.Length;
            var words = new Word[count];
            for (int i = 0; i < count; ++i)
            {
                words[i] = _mapper.Map<Word>(entities[i]);
                words[i].Count = entities[i].WordUsages.Count;
            }
            return words;
        }
        public bool ValidateWord(string word)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(word, "^[a-zA-Z]+$");
        }
        public async Task<WordEntity> CreateAsync(string name, CancellationToken ct = default)
        {
            var existingWord = await _wordRepository.GetByNameAsync(name, false, ct).ConfigureAwait(false);
            if (existingWord != null)
            {
                return null;
            }
            return await _wordRepository.CreateAsync(new WordEntity { Name = name }, ct).ConfigureAwait(false);
        }
        public Task<int> GetCountAsync(CancellationToken ct = default)
        {
            return _wordRepository.GetCountAsync(ct);
        }
        public Task<int> GetCountByNamesAsync(IEnumerable<string> likeNames = null,
            CancellationToken ct = default)
        {
            
            return _wordRepository.GetCountByNamesAsync(likeNames, ct);
        }
        public async Task<IEnumerable<Word>> GetAsync(int start = 0, int count = int.MaxValue,
           bool withoutSymbols = true,
           SortType sortType = SortType.Name,
           SortDirection sortDirection = SortDirection.Ascending,
           IEnumerable<string> likeNames = null,
           CancellationToken ct = default)
        {
            Func<IQueryable<WordEntity>, IOrderedQueryable<WordEntity>> orderBy = null;
            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    {
                        switch (sortType)
                        {
                            case SortType.Count:
                                {
                                    orderBy = q => q.OrderBy(e => e.WordUsages.Count);
                                }; break;
                            case SortType.Name:
                                {
                                    orderBy = q => q.OrderBy(e => e.Name);
                                }; break;
                        }
                    };break;
                case SortDirection.Descending:
                    {
                        switch (sortType)
                        {
                            case SortType.Count:
                                {
                                    orderBy = q => q.OrderByDescending(e => e.WordUsages.Count);
                                }; break;
                            case SortType.Name:
                                {
                                    orderBy = q => q.OrderByDescending(e => e.Name);
                                }; break;
                        }
                    };break;
            }
            List<Expression<Func<WordEntity, bool>>> filters =
                new List<Expression<Func<WordEntity, bool>>>();
            if (!(likeNames == null))
            {
                foreach (var likeName in likeNames)
                {
                    filters.Add(e => e.Name.Contains(likeName));
                }
            }
            
            if (withoutSymbols)
            {
                var letters = "abcdtfghijklmnopqrstuvwxyz".ToLower();

                filters.Add( e => letters.Contains(e.Name.Substring(0,1)) );
            }
            var entities = await _wordRepository.FindAsync(start, 
                count, 
                false, 
                filters, 
                orderBy,
                q => q.Include(e => e.WordUsages), 
                ct).ConfigureAwait(false);
            return MapToWord(entities.ToArray());
        }

        public Task<WordEntity> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return _wordRepository.GetByNameAsync(name.ToLower(), true, ct);
        }

        public async Task<bool> DeleteAsync(string name, CancellationToken ct = default)
        {
            var entity = await _wordRepository.GetByNameAsync(name, true, ct).ConfigureAwait(false);
            if(entity == null)
            {
                return false;
            }
            return await DeleteAsync(entity, ct).ConfigureAwait(false);
        }
        public async Task<bool> DeleteAsync(WordEntity entity, CancellationToken ct = default)
        {
            return await _wordRepository.DeleteAsync(entity, ct).ConfigureAwait(false);
        }

        public Task<WordEntity> UpdateAsync(WordEntity entity, CancellationToken ct = default)
        {
            return _wordRepository.UpdateAsync(entity, ct);
        }

    }
}
