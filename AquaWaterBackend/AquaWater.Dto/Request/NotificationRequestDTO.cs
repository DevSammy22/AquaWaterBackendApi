using System;

namespace AquaWater.Dto.Request
{
    public  class NotificationRequestDTO
    {
        public string Details { get; set; }
        public DateTime NotificationTime { get; set; }  
        public  Guid UserId { get; set; }
    }
}
