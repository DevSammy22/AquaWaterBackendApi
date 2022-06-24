using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaWater.Domain.Entities
{
    public class CompanyManager
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
    }
}
