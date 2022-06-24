using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class LocationRequestDTO
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OtherDetails { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
