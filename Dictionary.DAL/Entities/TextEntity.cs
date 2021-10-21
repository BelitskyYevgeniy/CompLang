using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompLang.DAL.Entities
{
    public class TextEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public List<WordUsageEntity> WordUsages { get; set; } = new List<WordUsageEntity>();
    }
}
