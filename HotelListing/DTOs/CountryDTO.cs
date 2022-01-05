using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.DTOs
{
    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
        public string Name { get; set; }
        [StringLength(maximumLength: 5, ErrorMessage = "Country short name is too long")]
        public string ShortName { get; set; }
    }
    public class CountryDTO: CreateCountryDTO
    {
        [Key]
        public int Id { get; set; }
        public IList<HotelDTO> Hotels { get; set; }
    }

}
