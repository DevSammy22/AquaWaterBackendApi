using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class ProductSearchDTO
    {
        [Required]
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public bool InStock { get; set; }
        public decimal Price { get; set; }
    }
}
