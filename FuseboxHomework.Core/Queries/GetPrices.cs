using FuseboxHomework.Core.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.Queries
{
    public record GetPrices(DateOnly Date) : IRequest<IEnumerable<HourlyElectricityPriceDto>>;
}
