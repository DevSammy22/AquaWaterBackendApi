using System;
using System.Collections.Generic;

namespace AquaWater.Dto.Response
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public ICollection<ImageResponseDTO> Photos { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public bool InStock { get => QuantityAvailable > 0; }
        public double Rating { get; set; } 
        public int NoOfRating { get; set; } 
    }
}
