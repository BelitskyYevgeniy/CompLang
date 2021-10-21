using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompLang.DAL.Entities
{
    public class WordEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public List<WordUsageEntity> WordUsages { get; set; } = new List<WordUsageEntity>();
    }
}
