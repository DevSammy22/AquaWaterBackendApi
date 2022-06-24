using AquaWater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AquaWater.Data.EntityConfiguration
{
    internal class NotificationEntityConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(x => x.IsRead).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}
