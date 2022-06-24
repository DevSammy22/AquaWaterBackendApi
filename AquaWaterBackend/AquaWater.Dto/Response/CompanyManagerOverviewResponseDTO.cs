using AquaWater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class CompanyManagerOverviewResponseDTO
    {
        public int TotalSupplies { get; set; }
        public int TotalOrdersReceived { get; set; }
        public int Customers { get; set; }
        public int FavouriteCustomers { get; set; }
    }
}
