using FuseboxHomework.Core.EF.Model;
using FuseboxHomework.Core.Queries;
using FuseboxHomework.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.EF.Repositories
{
    public class PricesRepository : IPricesRepository
    {
        private readonly FbDbContext _fbDbContext;

        public PricesRepository(FbDbContext fbDbContext)
        {
            _fbDbContext = fbDbContext;
        }

        public async Task<IEnumerable<HourlyElectricityPrice>> GetPricesAsync(DateTime from, DateTime to)
        {
            var prices = await _fbDbContext.HourlyElectricityPrices.Where(x => x.DateTime >= from && x.DateTime <= to).ToListAsync();
            return prices;
        }

        public async Task SavePricesAsync(IEnumerable<HourlyElectricityPrice> prices)
        {
            await _fbDbContext.HourlyElectricityPrices.AddRangeAsync(prices);
            await _fbDbContext.SaveChangesAsync();
        }
    }
}
