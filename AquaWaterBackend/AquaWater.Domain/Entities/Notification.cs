using System;

namespace AquaWater.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }    
        public string Details { get; set; }
        public bool IsRead { get; set; }  
        public DateTime CreatedAt { get; set; }  
        public virtual User User { get; set; }
    }
}
