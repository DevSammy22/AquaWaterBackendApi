using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class CustomerRequestDTO
    {
        [Required]
        public decimal EarnedCash { get; set; }
        [Required]
        public long ConsumptionLevel { get; set; }
        [Required]
        public UserRegistrationRequestDTO User { get; set; }
    }
}
