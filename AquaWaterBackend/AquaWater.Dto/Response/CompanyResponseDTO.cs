using System.Collections.Generic;

namespace AquaWater.Dto.Response
{
    public class CompanyResponseDTO
    {
        public string CompanyName { get; set; }
        
        public LocationResponseDTO Location { get; set; }

        public ProductResponseDTO Product { get; set; }

    }
}