using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaWater.Domain.Entities
{
    public class Company
   {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public Guid LocationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<CompanyManager> CompanyManager { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual Location Location { get; set; }
   }
}
