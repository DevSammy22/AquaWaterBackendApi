using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class CreateProductGalleryRequest
    {
        public string ImageUrl { get; set; }
        public bool IsDefault { get; set; }
    }
}
