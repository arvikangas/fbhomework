using FuseboxHomework.Core.Queries;
using FuseboxHomework.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FuseboxHomework.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Prices(DateOnly date)
        {
            var query = new GetPrices(date);
            var res = await _mediator.Send(query);
            var vm = new PricesViewModel(date, res);
            return View(vm);
        }
    }
}
