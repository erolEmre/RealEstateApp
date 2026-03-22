using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstate.Core.Models;
using RealEstate.Infrastructure.Context;
using RealEstateApp.WebUI.Models;

namespace RealEstateApp.WebUI.Controllers
{
    [Authorize(Roles = "Agent")]
    public class HousesController : Controller
    {
        private readonly RealEstateContext _context;

        public HousesController(RealEstateContext context)
        {
            _context = context;
        }

        // GET: Houses
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var realEstateContext = _context.Houses
                .Include(h => h.Employee)
                .Include(x => x.Address)
                .ToList();
            //Yeni tipe map ettik
            HouseVMSort houseVMSort = new HouseVMSort
            {
                Houses = realEstateContext
            };
            return View(houseVMSort);
        }

        // GET: Houses/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.Houses
                .Include(h => h.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (house == null)
            {
                return NotFound();
            }

            return View(house);
        }

        // GET: Houses/Create
        [Authorize(Roles = "Agent")]
        public IActionResult Create()
        {
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            return View();
        }

        // POST: Houses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HouseVM houseVM)
        {
            if (!ModelState.IsValid)
                return View(houseVM);

            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Auth0Sub == employeeId);
            if (employee == null) 
            {
                throw new Exception("");
            }

            var address = new Address
            {
                City = houseVM.City,
                District = houseVM.District,
                Street = houseVM.Street,
                BuildingNo = houseVM.BuildingNo,
                ApartmentNo = houseVM.ApartmentNo
            };

            House house = new House(
                houseVM.Price,
                houseVM.Title,
                houseVM.Area,
                employee.Id,
                address
            );
            house.UpdateRooms(houseVM.NumberOfRooms, houseVM.NumberOfBathrooms, employee.Id);
            house.SetDescription(houseVM.Description);
            house.SetContactNumber(houseVM.ContactNumber);
            house.SetImageUrl(houseVM.ImageUrl);



            if (ModelState.IsValid)
            {
                _context.Add(house);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", house.EmployeeId);
            return View(house);
        }

        // GET: Houses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.Houses.FindAsync(id);
            if (house == null)
            {
                return NotFound();
            }
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", house.EmployeeId);
            return View(house);
        }

        // POST: Houses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HouseVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // 1. Veritabanından asıl nesneyi çek (Sadece Id ile)
            var house = await _context.Houses.FindAsync(vm.Id);

            if (house == null) return NotFound();

            try
            {
                // 2. Domain nesnesindeki metodları kullanarak verileri aktar
                // Örn: EmployeeId'yi sistemden (logged in user) alıyorsan onu gönder
                house.UpdateDetails(vm.Title, vm.Price, house.EmployeeId);

                house.SetDescription(vm.Description);
                house.SetContactNumber(vm.ContactNumber);
                house.ChangeArea(vm.Area);
                house.UpdateRooms(vm.NumberOfRooms, vm.NumberOfBathrooms, house.EmployeeId);
                if (vm.IsAvailable) house.MarkAsAvailable();
                else house.MarkAsUnavailable();

                // 3. Değişiklikleri kaydet
                _context.Update(house);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Houses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.Houses
                .Include(h => h.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (house == null)
            {
                return NotFound();
            }

            return View(house);
        }

        // POST: Houses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var house = await _context.Houses.FindAsync(id);
            if (house != null)
            {
                _context.Houses.Remove(house);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<List<House>> sortOrder(string sortOrder)
        {
            var listing = _context.Houses.ToList();

            listing = sortOrder switch
            {
                "date_asc" => listing.OrderBy(x => x.ListingDate).ToList(),
                "date_desc" => listing.OrderByDescending(x => x.ListingDate).ToList(),
                //"price" => listing.Where(x => x.Price >)
            };
            return listing;
        }
        private bool HouseExists(int id)
        {
            return _context.Houses.Any(e => e.Id == id);
        }
    }
}
