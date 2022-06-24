using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaWater.Domain.Entities
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }
        public bool IsFeature { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ProductGallery> ProductGallery { get; set; }
        public virtual ICollection<CustomerFavourite> CustomerFavourites { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
