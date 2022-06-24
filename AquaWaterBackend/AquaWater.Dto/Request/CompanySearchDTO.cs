using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class CompanySearchDTO
    {
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }
}
