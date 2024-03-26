using FuseboxHomework.Core.Dtos;

namespace FuseboxHomework.Web.Models
{
    public record PricesViewModel(DateOnly date, IEnumerable<HourlyElectricityPriceDto> Prices);
}
