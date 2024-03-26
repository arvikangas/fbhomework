using FuseboxHomework.Core.Dtos;
using FuseboxHomework.Core.EF.Model;
using FuseboxHomework.Core.Queries;
using FuseboxHomework.Core.Services;
using MediatR;
using NSubstitute;
using Shouldly;

namespace FuseboxHomework.UnitTests
{
    public class GetPricesHandlerTests
    {
        [Fact]
        public async Task handler_should_use_data_from_repository_if_exists()
        {
            var query = new GetPrices(new DateOnly(2024, 2, 1));

            var from = query.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
            var to = from.AddDays(1).AddTicks(-1).ToUniversalTime();

            var prices = new List<HourlyElectricityPrice>()
            {
                new HourlyElectricityPrice(from, 2)
            };

            _pricesRepository.GetPricesAsync(from, to).Returns(prices);

            var result = await _handler.Handle(query, default);

            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count().ShouldBe(1);

            await _eleringApi.DidNotReceiveWithAnyArgs().GetNpsPrices(default, default);
        }

        [Fact]
        public async Task handler_should_use_data_from_api_if_data_not_exists_in_database()
        {
            var query = new GetPrices(new DateOnly(2024, 2, 1));

            var from = query.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
            var to = from.AddDays(1).AddTicks(-1).ToUniversalTime();

            var apiResponse = new ApiResponseSingle<NpsPriceResponseDto>(true, new NpsPriceResponseDto(new List<NpsPriceDto>
            {
                new NpsPriceDto(((DateTimeOffset)from).ToUnixTimeSeconds(), 2)
            }));

            var response = new RestEase.Response<ApiResponseSingle<NpsPriceResponseDto>>(null, new HttpResponseMessage(System.Net.HttpStatusCode.OK), () => apiResponse);
            _eleringApi.GetNpsPrices(from, to).Returns(response);
            var result = await _handler.Handle(query, default);

            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count().ShouldBe(1);
        }

        [Fact]
        public async Task handler_should_save_data_to_database()
        {
            var query = new GetPrices(new DateOnly(2024, 2, 1));

            var from = query.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
            var to = from.AddDays(1).AddTicks(-1).ToUniversalTime();

            var apiResponse = new ApiResponseSingle<NpsPriceResponseDto>(true, new NpsPriceResponseDto(new List<NpsPriceDto>
            {
                new NpsPriceDto(((DateTimeOffset)from).ToUnixTimeSeconds(), 2)
            }));

            var response = new RestEase.Response<ApiResponseSingle<NpsPriceResponseDto>>(null, new HttpResponseMessage(System.Net.HttpStatusCode.OK), () => apiResponse);
            _eleringApi.GetNpsPrices(from, to).Returns(response);
            var result = await _handler.Handle(query, default);

            await _pricesRepository.Received().SavePricesAsync(Arg.Is<IEnumerable<HourlyElectricityPrice>>(x => x.Count() == 1 && x.First().Price == 2));
        }


        [Fact]
        public async Task handler_should_convert_data_from_api_correctly()
        {
            var query = new GetPrices(new DateOnly(2024, 2, 1));

            var from = query.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
            var to = from.AddDays(1).AddTicks(-1).ToUniversalTime();

            var apiResponse = new ApiResponseSingle<NpsPriceResponseDto>(true, new NpsPriceResponseDto(new List<NpsPriceDto>
            {
                new NpsPriceDto(1711440000, 2)
            }));

            var response = new RestEase.Response<ApiResponseSingle<NpsPriceResponseDto>>(null, new HttpResponseMessage(System.Net.HttpStatusCode.OK), () => apiResponse);
            _eleringApi.GetNpsPrices(from, to).Returns(response);
            var result = await _handler.Handle(query, default);

            var ticks = new DateTime(2024, 03, 26, 8, 0, 0).Ticks;
            await _pricesRepository.Received().SavePricesAsync(Arg.Is<IEnumerable<HourlyElectricityPrice>>(x =>
                x.Count() == 1 && x.First().DateTime.Ticks == ticks));
        }

        private readonly IPricesRepository _pricesRepository;
        private readonly IEleringApi _eleringApi;

        private readonly IRequestHandler<GetPrices, IEnumerable<HourlyElectricityPriceDto>> _handler;

        public GetPricesHandlerTests()
        {
            _pricesRepository = Substitute.For<IPricesRepository>();
            _eleringApi = Substitute.For<IEleringApi>();

            _handler = new GetPricesHandler(_pricesRepository, _eleringApi);
        }
    }
}