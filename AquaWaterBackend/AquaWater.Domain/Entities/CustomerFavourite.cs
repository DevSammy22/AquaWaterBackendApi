using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Domain.Entities
{
    public class CustomerFavourite
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }  
        public Guid ProductId { get; set; } 
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }

    }
}
