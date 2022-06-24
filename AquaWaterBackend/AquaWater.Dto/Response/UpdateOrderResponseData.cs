using System;
using System.Collections.Generic;

namespace AquaWater.Dto.Response
{
    public class UpdateOrderResponseData
    {
        public Guid Id { get; set; }

        public ICollection<UpdateOrderItemResponseData> OrderItem { get; set; }
    }
}
