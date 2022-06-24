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
    internal class AdminUserEntityConfiguration : IEntityTypeConfiguration<AdminUser>
    {
        public void Configure(EntityTypeBuilder<AdminUser> builder)
        {
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.BusinessEmail).IsRequired();
            builder.Property(x => x.BusinessPhoneNumber).IsRequired();
           
        }
    }
}
