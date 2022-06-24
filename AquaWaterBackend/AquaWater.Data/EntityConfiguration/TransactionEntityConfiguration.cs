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
    internal class TransactionEntityConfiguration: IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.OrderId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.TransactionReference).IsRequired();
            builder.Property(x => x.OrderDate).IsRequired();
        }
    
    }
}
