using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Models;

namespace SoftitoFlix.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,long>
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Director> Directors { get; set; } = default!;
        public DbSet<Episode> Episodes { get; set; } = default!;
        public DbSet<Media> Medias { get; set; } = default!;
        public DbSet<Media_Category> Media_Categories { get; set; } = default!;
        public DbSet<Media_Director> Media_Directors { get; set; } = default!;
        public DbSet<Media_Restriction> Media_Restrictions { get; set; } = default!;
        public DbSet<Media_Star> Media_Stars { get; set; } = default!;
        public DbSet<Plan> Plans { get; set; } = default!;
        public DbSet<Restriction> Restrictions { get; set; } = default!;
        public DbSet<Star> Stars { get; set; } = default!;
        public DbSet<User_Watched> User_Watcheds { get; set; } = default!;
        public DbSet<User_Plan> User_Plans { get; set; } = default!;
        public DbSet<User_Favorite> User_Favorites { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Episode>().HasIndex(e => new { e.MediaId, e.SeasonNumber, e.EpisodeNumber }).IsUnique();
            modelBuilder.Entity<Media_Star>().HasKey(ms => new { ms.MediaId, ms.StarId });
            modelBuilder.Entity<Media_Director>().HasKey(md => new { md.MediaId, md.DirectorId });
            modelBuilder.Entity<Media_Category>().HasKey(mc => new { mc.MediaId, mc.CategoryId });
            modelBuilder.Entity<Media_Restriction>().HasKey(mr => new { mr.MediaId, mr.RestrictionId });
            modelBuilder.Entity<User_Watched>().HasKey(uw => new { uw.EpisodeId, uw.UserId });
            modelBuilder.Entity<User_Favorite>().HasKey(uf => new { uf.MediaId, uf.UserId });

        }
    }
}
