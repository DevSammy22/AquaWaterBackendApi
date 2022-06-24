using AquaWater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Data.EntityConfiguration
{
    internal class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(x => x.Country).IsRequired();
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.City).IsRequired();
            builder.Property(x => x.Street).IsRequired();
            builder.Property(x => x.HouseNumber).IsRequired();
           
        }
    }
}
