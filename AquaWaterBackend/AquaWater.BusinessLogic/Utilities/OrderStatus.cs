using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Utilities
{
    public enum OrderStatus
    {
        Paid = 1,
        OnHold = 2,
        Declined = 3,
        Failed = 4,
        ToBeDelivered = 5,
        Delivered = 6
    }
}
