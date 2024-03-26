using FuseboxHomework.Core.EF.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.EF.Configurations
{
    public class HourlyElectricityPriceEntityConfiguration : IEntityTypeConfiguration<HourlyElectricityPrice>
    {
        public void Configure(EntityTypeBuilder<HourlyElectricityPrice> builder)
        {
            builder.HasKey(x => x.DateTime);
        }
    }
}
