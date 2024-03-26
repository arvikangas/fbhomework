using FuseboxHomework.Core.Dtos;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.Services
{
    public interface IEleringApi
    {
        [Get("api/nps/price")]
        public Task<Response<ApiResponseSingle<NpsPriceResponseDto>>> GetNpsPrices(
            [Query(Format = "yyyy-MM-ddTHH:mm:ss.fffZ")] DateTime start, [Query(Format = "yyyy-MM-ddTHH:mm:ss.fffZ")] DateTime end);
    }
}
