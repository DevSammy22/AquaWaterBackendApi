using System;
using System.Collections.Generic;

namespace AquaWater.Dto.Response
{
    public class ProductResponseDTO
    {
        public ICollection<ImageResponseDTO> Photos { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public bool InStock { get => QuantityAvailable > 0; }
        public string Description { get; set; }
        public ICollection<ReviewResponseDTO> Reviews { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
