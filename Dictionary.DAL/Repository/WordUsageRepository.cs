using CompLang.DAL.Context;
using CompLang.DAL.Entities;
using CompLang.DAL.Interfaces.Repository;
using CompLang.DAL.Repository.Generic;

namespace CompLang.DAL.Repository
{
    public class WordUsageRepository: GenericRepository<WordUsageEntity>, IWordUsageRepository
    {
        public WordUsageRepository(DictionaryDbContext db): base(db)
        {

        }
    }
}
