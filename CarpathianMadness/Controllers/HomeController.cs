using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarpathianMadness.Models;
using CarpathianMadness.Services;

namespace CarpathianMadness.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;        
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            this._homeService = new HomeService();
            
        }

        public IActionResult Index()
        {

           

            _homeService.SearchHome(8);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
