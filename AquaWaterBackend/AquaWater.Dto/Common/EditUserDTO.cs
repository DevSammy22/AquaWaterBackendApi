using AquaWater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Common
{
    public class EditUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public LocationDTO Location { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
