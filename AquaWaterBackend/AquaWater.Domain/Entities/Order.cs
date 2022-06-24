using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Domain.Entities
{
   public class Order
   {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int OrderStatus { get; set; }
        public long OTP { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
   }
}
