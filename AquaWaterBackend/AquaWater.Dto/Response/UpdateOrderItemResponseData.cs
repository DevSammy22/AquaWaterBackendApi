using System;

namespace AquaWater.Dto.Response
{
    public class UpdateOrderItemResponseData
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentProductPrice { get; set; }
    }
}
