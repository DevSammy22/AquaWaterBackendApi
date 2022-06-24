using System;
using System.Collections.Generic;

namespace AquaWater.Dto.Request
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public ICollection<OrderItemRequestDTO> OrderItem { get; set; }
    }
}
