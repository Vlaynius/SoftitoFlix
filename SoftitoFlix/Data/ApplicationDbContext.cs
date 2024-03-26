using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Models;

namespace SoftitoFlix.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category>? Categories { get; set; }
        public DbSet<Director>? Directors { get; set; }
        public DbSet<Media>? Medias { get; set; }
        public DbSet<Media_Category>? Media_Categories { get; set; }
        public DbSet<Media_Director>? Media_Directors { get; set; }
        public DbSet<Media_Restriction>? Media_Restrictions { get; set; }
        public DbSet<Media_Star>? Media_Stars { get; set; }
        public DbSet<Person>? People { get; set; }
        public DbSet<Restriction>? Restrictions { get; set; }
        public DbSet<Star>? Stars { get; set; }
        
     


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Media_Star>().HasKey(ru => new { ru.MediaId, ru.StarId });
            modelBuilder.Entity<Media_Director>().HasKey(ru => new { ru.MediaId, ru.DirectorId });
            modelBuilder.Entity<Media_Category>().HasKey(ru => new { ru.MediaId, ru.CategoryId });
            modelBuilder.Entity<Media_Restriction>().HasKey(ru => new { ru.MediaId, ru.RestrictionId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
