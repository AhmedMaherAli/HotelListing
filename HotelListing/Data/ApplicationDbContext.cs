using HotelListing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {}

        DbSet<Country> Countries { get; set; }
        DbSet<Hotel> Hotels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(

                new Country
                {
                    Id=1,
                    Name="Egypt",
                    ShortName="Eg"
                },
                new Country
                {
                    Id=2,
                    Name = "United Kingdom",
                    ShortName = "UK"
                },
                new Country
                {
                    Id=3,
                    Name = "Norway",
                    ShortName = "Nor"
                }

           );

            modelBuilder.Entity<Hotel>().HasData(

                new Hotel
                {
                    Id = 1,
                    Name = "Four Seasons",
                    Address="112 st Mourad, Giza city",
                    Rating=4.2,
                    CountryId=1
                },
                new Hotel
                {
                    Id = 2,
                    Name = "The global entertainment",
                    Address = "London the second floor",
                    Rating = 4.8,
                    CountryId = 2
                },
                new Hotel
                {
                    Id = 3,
                    Name = "The highest Mountain",
                    Address = "5The main street of norway",
                    Rating = 4.7,
                    CountryId =3
                }

           );

        }

    }
}
