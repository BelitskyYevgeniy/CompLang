using System.ComponentModel.DataAnnotations;

namespace CompLang.DAL.Entities
{
    public class WordUsageEntity
    {
        [Key]
        public int Id { get; set; }

        public WordEntity Word { get; set; }

        [Required]
        public TextEntity Text { get; set; }
    }
}
