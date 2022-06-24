using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AquaWater.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid LocationId { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTime RefreshTokenCreatedAt { get; set; }
        public DateTime RefreshTokenExpiryAt { get; set; }
        public string ProfilePictureUrl { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
    }
}