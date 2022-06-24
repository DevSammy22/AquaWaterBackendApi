using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Domain.Entities
{
    public class AdminUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public virtual User User { get; set; }
    }
}
