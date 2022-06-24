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
    internal class ProductGalleryEntityConfiguration:IEntityTypeConfiguration<ProductGallery>
    {
        public void Configure(EntityTypeBuilder<ProductGallery> builder)
        {
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired();
            builder.Property(x => x.IsDefault).IsRequired();
        }
    }
}
