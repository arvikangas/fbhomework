using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.Dtos
{
    public record HourlyElectricityPriceDto(DateTime DateTime, decimal Price, bool Cheapers);
}
