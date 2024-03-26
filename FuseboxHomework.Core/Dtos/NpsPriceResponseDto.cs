using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.Dtos
{
    public record NpsPriceResponseDto(IEnumerable<NpsPriceDto> Ee);
}
