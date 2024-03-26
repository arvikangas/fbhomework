using FuseboxHomework.Core.EF.Configurations;
using FuseboxHomework.Core.EF.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.EF
{
    public class FbDbContext : DbContext
    {
        public DbSet<HourlyElectricityPrice> HourlyElectricityPrices { get; set; }

        public FbDbContext() { } // This one

        public FbDbContext(DbContextOptions<FbDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HourlyElectricityPriceEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
