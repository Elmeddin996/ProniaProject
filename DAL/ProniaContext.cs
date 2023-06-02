using Microsoft.EntityFrameworkCore;
using ProniaProject.Models;

namespace ProniaProject.DAL
{
    public class ProniaContext:DbContext
    {
        public ProniaContext(DbContextOptions<ProniaContext> options) : base(options)
        {

        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }
        public DbSet<PlantTag> PlantTags { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlantTag>().HasKey(x => new { x.TagId, x.PlantId });


            base.OnModelCreating(modelBuilder);
        }
    }
}
