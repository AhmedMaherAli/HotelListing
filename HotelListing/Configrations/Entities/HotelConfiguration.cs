using HotelListing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configrations.Entities
{

    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                    new Hotel
                    {
                        Id = 1,
                        Name = "Four Seasons",
                        Address = "112 st Mourad, Giza city",
                        Rating = 4.2,
                        CountryId = 1
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
                        CountryId = 3
                    }
                );
        }
    }
}
