﻿using AutoMapper;
using CompLang.BLL.Interfaces.Providers;
using CompLang.BLL.Models;
using CompLang.DAL.Entities;
using CompLang.DAL.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.BLL.Providers
{
    public class TextProvider: ITextProvider
    {
        private readonly ITextRepository _textRepository;
        private readonly IMapper _mapper;
        public TextProvider(ITextRepository textRepository, IMapper mapper)
        {
            this._textRepository = textRepository;
            this._mapper = mapper;
        }

        public Text MapEntityToModelAsync(TextEntity entity)
        {
            var text = _mapper.Map<Text>(entity);
            var sb = new StringBuilder();
            foreach (var wordUsage in entity.WordUsages)
            {
                sb.Append(wordUsage.Word.Name);
            }
            text.Content = sb.ToString();
            return text;
        }
        public Task<TextEntity> GetByTitleAsync(string title, bool toTrack = false, CancellationToken ct = default)
        {
            return _textRepository.GetByTitleAsync(title, toTrack, ct);
        }

        public Task<TextEntity> CreateAsync(TextEntity entity, CancellationToken ct = default)
        {
            return _textRepository.CreateAsync(entity, ct);
        }
        public Task<TextEntity> UpdateAsync(TextEntity entity, CancellationToken ct = default)
        {
            return _textRepository.UpdateAsync(entity, ct);
        }
        public async Task<TextEntity> GetByTitleAsync(string title, CancellationToken ct = default)
        {
            var text = await _textRepository.GetByTitleAsync(title, false, ct);
            return text;
        }
        public async Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken ct = default)
        {
            var texts = await _textRepository.GetAllAsync(ct: ct);
            var result = new List<string>();
            foreach(var text in texts)
            {
                result.Add(text.Title);
            }
            return result;
        }
    }
}
