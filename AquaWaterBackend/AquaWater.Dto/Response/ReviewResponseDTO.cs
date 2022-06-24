using System;

namespace AquaWater.Dto.Response
{
    public class ReviewResponseDTO
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public string Comment { get; set; }
        
    }
}
