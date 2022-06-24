using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class GetAllCompanyOrdersResponseDTO
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string OrderLocation { get; set; }
        public string OrderDate { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
    }
}
