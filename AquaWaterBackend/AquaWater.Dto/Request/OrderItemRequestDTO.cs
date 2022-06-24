using System;

namespace AquaWater.Dto.Request
{
    public class OrderItemRequestDTO
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentProductPrice { get; set; }
    }
}
