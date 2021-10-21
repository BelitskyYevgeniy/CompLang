using AutoMapper;
using CompLang.BLL.Models;
using CompLang.DAL.Entities;

namespace CompLang.BLL.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<TextEntity, Text>().ReverseMap();
            CreateMap<WordEntity, Word>().ReverseMap();

        }
    }
}