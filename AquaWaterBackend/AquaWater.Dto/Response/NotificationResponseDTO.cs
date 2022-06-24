using AquaWater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class NotificationResponseDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Details { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User User { get; set; }
    }
}
