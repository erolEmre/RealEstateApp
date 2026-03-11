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
            //EmployeeVM vm = new EmployeeVM()
            //{
            //    Employees = _context.Employees.ToList()
            //};

            //HouseVMList vm2 = new HouseVMList()
            //{
            //    Houses = _context.Houses.Select(x => new HouseVM()
            //    {
            //        Id = x.Id,
            //        Title = x.Title,
            //        Price = x.Price,
            //        NumberOfRooms = x.NumberOfRooms

            //    }).ToList()
            //};

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
