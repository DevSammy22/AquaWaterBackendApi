using AquaWater.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class UpdateOrderRequestDTO
    {
        public string OrderID { get; set; }
        public ICollection<UpdateOrderItemRequest> OrderItems { get; set; }
    }
}
