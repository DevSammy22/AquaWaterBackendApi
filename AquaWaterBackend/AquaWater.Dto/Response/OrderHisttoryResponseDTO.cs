using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class OrderHisttoryResponseDTO
    {
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string OrderStatus { get; set; }
        public string ImageUrl { get; set; }
    }
}
