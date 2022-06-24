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
    internal class ProductEntityConfiguration: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.QuantityAvailable).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.IsFeature).IsRequired();
        }
    }
}
