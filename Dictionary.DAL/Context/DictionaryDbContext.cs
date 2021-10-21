using CompLang.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompLang.DAL.Context
{
    public class DictionaryDbContext: DbContext
    {
        public DbSet<WordEntity> Words { get; set; }
        public DbSet<TextEntity> Texts { get; set; }

        public DictionaryDbContext()
        {
            Database.EnsureCreated();
        }

        public DictionaryDbContext(DbContextOptions<DictionaryDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordEntity>().HasIndex(e => e.Name).IsUnique();
            modelBuilder.Entity<TextEntity>().HasIndex(e => e.Title).IsUnique();
        }
    }
}
