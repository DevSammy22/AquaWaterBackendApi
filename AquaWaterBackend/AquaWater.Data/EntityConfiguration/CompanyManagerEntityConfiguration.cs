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
    internal class CompanyManagerEntityConfiguration : IEntityTypeConfiguration<CompanyManager>
    {
        public void Configure(EntityTypeBuilder<CompanyManager> builder)
        {
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            
        }
    }
}
