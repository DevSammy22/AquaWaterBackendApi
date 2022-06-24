using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Domain.Entities
{
  public class Customer
  {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public decimal EarnedCash { get; set; }
        public long ConsumptionLevel { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Review> Review { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<CustomerFavourite> CustomerFavourites { get; set; }
    }
}
