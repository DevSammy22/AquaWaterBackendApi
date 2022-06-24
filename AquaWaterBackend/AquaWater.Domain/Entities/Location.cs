using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaWater.Domain.Entities
{
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OtherDetails { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public virtual Company Company { get; set; }
    }
}
