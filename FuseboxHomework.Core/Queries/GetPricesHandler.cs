using FuseboxHomework.Core.Dtos;
using FuseboxHomework.Core.EF;
using FuseboxHomework.Core.EF.Model;
using FuseboxHomework.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FuseboxHomework.Core.Queries
{
    public class GetPricesHandler : IRequestHandler<GetPrices, IEnumerable<HourlyElectricityPriceDto>>
    {
        private readonly IPricesRepository _pricesRepository;
        private readonly IEleringApi _eleringApi;

        public GetPricesHandler(IPricesRepository pricesRepository, IEleringApi eleringApi)
        {
            _pricesRepository = pricesRepository;
            _eleringApi = eleringApi;
        }

        public async Task<IEnumerable<HourlyElectricityPriceDto>> Handle(GetPrices request, CancellationToken cancellationToken)
        {
            var from = request.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
            var to = from.AddDays(1).AddTicks(-1).ToUniversalTime();

            var prices = await _pricesRepository.GetPricesAsync(from, to);

            var intermediate = new List<(DateTime, decimal)>();
            if(!prices.Any())
            {
                var eleringDataApiResponse = await _eleringApi.GetNpsPrices(from, to);

                if (!eleringDataApiResponse.ResponseMessage.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                var eleringData = eleringDataApiResponse.GetContent();

                var newPrices = eleringData.Data.Ee.Select(x => new HourlyElectricityPrice(DateTimeOffset.FromUnixTimeSeconds(x.Timestamp).DateTime, x.Price));

                await _pricesRepository.SavePricesAsync(newPrices);

                prices = newPrices.ToList();
            }

            var orderedPrices = prices.OrderBy(x => x.Price).ThenBy(x => x.DateTime);
            var topFive = orderedPrices.Take(5).ToList();
            var topFiveDtos = topFive.Select(x => new HourlyElectricityPriceDto(x.DateTime.ToLocalTime(), x.Price, true));

            var last = orderedPrices.Skip(5);
            var lastDtos = last.Select(x => new HourlyElectricityPriceDto(x.DateTime.ToLocalTime(), x.Price, false));

            var combined = topFiveDtos.Concat(lastDtos);
            var combinedOrdered = combined.OrderBy(x => x.DateTime);

            return combinedOrdered;
        }
    }
}
