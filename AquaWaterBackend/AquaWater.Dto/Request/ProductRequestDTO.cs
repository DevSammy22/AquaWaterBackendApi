using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class ProductRequestDTO
    {
        [Required]
        public Guid ProductId { get; set; }
    }
}
