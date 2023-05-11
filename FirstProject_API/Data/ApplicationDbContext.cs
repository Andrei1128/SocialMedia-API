using FirstProject_API.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstProject_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 440,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Beach Villa",
                    Details = "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.",
                    ImageUrl = "",
                    Occupancy = 10,
                    Rate = 100,
                    Sqft = 240,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 3,
                    Name = "New Villa",
                    Details = "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.",
                    ImageUrl = "",
                    Occupancy = 12,
                    Rate = 200,
                    Sqft = 340,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 4,
                    Name = "Old Villa",
                    Details = "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.",
                    ImageUrl = "",
                    Occupancy = 2,
                    Rate = 4,
                    Sqft = 140,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                }
            );
        }
    }
}
