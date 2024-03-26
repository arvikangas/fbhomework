using FuseboxHomework.Core.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.Services
{
    public interface IPricesRepository
    {
        Task<IEnumerable<HourlyElectricityPrice>> GetPricesAsync(DateTime from, DateTime to);
        Task SavePricesAsync(IEnumerable<HourlyElectricityPrice> prices);
    }
}
