using AquaWater.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class EditUserResponseDTO
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public LocationDTO Location { get; set; }
        public string PhoneNumber { get; set; }
    }
}
