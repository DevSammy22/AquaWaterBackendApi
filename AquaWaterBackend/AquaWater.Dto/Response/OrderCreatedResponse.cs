using AquaWater.Dto.Request;
using System;
using System.Collections.Generic;

namespace AquaWater.Dto.Response
{
    public class OrderResponse
    {
        public string CustomerId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItemResponseDTO> OrderItem { get; set; }
    }
}
