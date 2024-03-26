using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuseboxHomework.Core.EF.Model
{
    public class HourlyElectricityPrice
    {
        public HourlyElectricityPrice(DateTime dateTime, decimal price)
        {
            DateTime = dateTime;
            Price = price;
        }

        public DateTime DateTime { get; set; }
        public decimal Price { get; set; }
    }
}
