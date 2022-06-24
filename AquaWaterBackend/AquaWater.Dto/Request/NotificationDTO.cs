using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class NotificationDTO
    {
        public string id { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
