using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Domain;

namespace ShopTARgv24.Data
{
    public class ShopTARgv24Context : DbContext
    {
        public ShopTARgv24Context(DbContextOptions<ShopTARgv24Context> options)
        : base(options) { }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<FileToDatabase> FileToDatabase { get; set; }
        public DbSet<Kindergarten> Kindergartens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealEstate>().ToTable("RealEstates");
            modelBuilder.Entity<Spaceship>().ToTable("Spaceships");
            modelBuilder.Entity<FileToApi>().ToTable("FileToApis");
            modelBuilder.Entity<FileToDatabase>().ToTable("FileToDatabase");
            modelBuilder.Entity<Kindergarten>().ToTable("Kindergartens");

            base.OnModelCreating(modelBuilder);
        }
    }
}