using System;

namespace AquaWater.Dto.Response
{
    public class CreateOrderItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
