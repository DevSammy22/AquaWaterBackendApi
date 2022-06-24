using AquaWater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AquaWater.Data.EntityConfiguration
{
    public class CustomerFavouriteEntityConfiguration : IEntityTypeConfiguration<CustomerFavourite>
    {
        public void Configure(EntityTypeBuilder<CustomerFavourite> builder)
        {
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.CustomerId).IsRequired();
        }
    }
}