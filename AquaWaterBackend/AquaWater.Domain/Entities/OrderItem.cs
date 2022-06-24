using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaWater.Domain.Entities
{
    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentProductPrice { get; set; }
        public virtual Order Orders { get; set; }
        public virtual Product Product { get; set; }
    }
}