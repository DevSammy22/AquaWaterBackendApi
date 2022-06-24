using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class OrderRequest
    {
        public ICollection<CreateOrderItem> OrderItem { get; set; }
    }
}
