using AquaWater.Dto.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Common
{
    public class CompanyManagerRequestDTO
    {
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public UserRegistrationRequestDTO User { get; set; }
    }
}
