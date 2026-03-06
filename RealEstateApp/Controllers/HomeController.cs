using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Infrastructure.Context;
using RealEstateApp.WebUI.Models;

namespace RealEstateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RealEstateContext _context;
        public HomeController(ILogger<HomeController> logger,RealEstateContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            EmployeeVM vm = new EmployeeVM()
            {
                Employees = _context.Employees.ToList()
            };
            return View(vm);
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
