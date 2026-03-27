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
        //public async Task<IActionResult> Index()
        //{
        //    var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var realEstateContext = _context.Houses
        //        .Include(h => h.Employee)
        //        .Include(x => x.Address)
        //        .ToList();
        //    //Yeni tipe map ettik
        //    HouseVMSort houseVMSort = new HouseVMSort
        //    {
        //        Houses = realEstateContext
        //    };
        //    return View(houseVMSort);
        //}

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
                .Include(a=> a.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (house == null)
            {
                return NotFound();
            }
            HouseVM houseVM = new HouseVM
            {
                Address = house.Address,
                //City = house.Address.City,
                //District = house.Address.District,
                //ApartmentNo = house.Address.ApartmentNo,
                //BuildingNo = house.Address.BuildingNo,
                //Street = house.Address.Street,
                Area = house.Area,
                ContactNumber = house.ContactNumber,
                Description = house.Description,
                Price = house.Price,
                Title = house.Title,
                Id  = house.Id,
                IsAvailable = house.IsAvailable,
                ListingDate = house.ListingDate,
                ImageUrl = house.ImageUrl,
                NumberOfBathrooms = house.NumberOfBathrooms,
                NumberOfRooms = house.NumberOfRooms,
                Employee = house.Employee
            };

            return View(houseVM);
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
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(HouseVMSort searchModel)
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 1. SORGUYU BAŞLAT (Henüz veritabanına gitmiyor, sadece SQL cümlesi hazırlıyoruz)
            var query = _context.Houses
                .Include(x=>x.Address)
                .Include(x=>x.Employee)         
                .AsQueryable();

            searchModel.AvailableCities = await _context.Houses.Where(x => x.Address != null && !string.IsNullOrEmpty(x.Address.City)
            )
                .Select(x => x.Address.City)
                .Distinct()
                .ToListAsync();

            searchModel.AvailableBathrooms = await _context.Houses.Where(x => x.NumberOfBathrooms != null && x.NumberOfBathrooms >= 0)
                .Select(x => x.NumberOfBathrooms)
                .Distinct()
                .ToListAsync();

            searchModel.AvailableRooms = await _context.Houses.Where(x => x.NumberOfRooms != null)
                .Select(x => x.NumberOfRooms)
                .Distinct()
                .ToListAsync();

            // 2. FİLTRELEME (WHERE) ADIMLARI
            // Eğer kullanıcı minPrice kutusuna bir şey yazmışsa:
             
            if(!string.IsNullOrEmpty(searchModel.SearchText))
            {
                // 1. DÖNGÜSÜZ SPLIT İŞLEMİ: Metni boşluklardan ayır ve birden fazla boşluk varsa onları temizle
                var words = searchModel.SearchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // 2. HER KELİME İÇİN DÖNGÜ (EF Core'un SQL'e çevirebildiği güvenli yöntem)
                foreach (var word in words)
                {
                    // Kullanıcı "İstanbul Havuzlu" yazdıysa:
                    // Sistem "İstanbul" kelimesini şehirde VEYA başlıkta VEYA açıklamada arar.
                    // Sonra "Havuzlu" kelimesini şehirde VEYA başlıkta VEYA açıklamada arar. (AND mantığı)
                    query = query.Where(x =>
                        (x.Address.City != null && x.Address.City.Contains(word)) ||
                        (x.Title != null && x.Title.Contains(word)) ||
                        (x.Description != null && x.Description.Contains(word))
                    );
                }
            }

            // Fiyat
            if (searchModel.minPrice.HasValue)
            {
                query = query.Where(x => x.Price >= searchModel.minPrice.Value);
            }

            if(searchModel.maxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= searchModel.maxPrice.Value);
            }
            // Metrekare
            if (searchModel.minArea.HasValue)
            {
                query = query.Where(x => x.Area >= searchModel.minArea.Value);
            }
            if (searchModel.maxArea.HasValue)
            {
                query = query.Where(x => x.Area <= searchModel.maxArea.Value);
            }
                            // Şehir
            if(!String.IsNullOrEmpty(searchModel.City))
            {
                query = query.Where(x => x.Address.City == searchModel.City);
            }
            // Oda Sayısı
            if(searchModel.numberOfRooms.HasValue)
            {
                query = query.Where(x => x.NumberOfRooms == searchModel.numberOfRooms.Value);
            }
            // Banyo Sayısı
            if(searchModel.NumberOfBathrooms.HasValue)
            {
                query = query.Where(x => x.NumberOfBathrooms == searchModel.NumberOfBathrooms.Value);
            }

            //3.SIRALAMA(ORDER BY) ADIMI
            //Senin yazdığın harika switch yapısını burada kullanıyoruz
            query = searchModel.sortOrder switch
            {
                "date_asc" => query.OrderBy(x => x.ListingDate),
                "price_desc" => query.OrderByDescending(x => x.Price),
                "price_asc" => query.OrderBy(x => x.Price),
                // GÖREV 3: Fiyata göre azalan (price_desc) durumunu sen yaz
                _ => query.OrderByDescending(x => x.ListingDate) // Varsayılan sıralama (En yeniler)
            };

            // 4. VERİYİ ÇEK VE MODELİ HAZIRLA
            // İşte şimdi ToListAsync() diyerek SQL'i çalıştırıyoruz!
            searchModel.Houses = await query.ToListAsync();

            // 5. SONUCU EKRANA GÖNDER
            return View(searchModel);
        }
        private bool HouseExists(int id)
        {
            return _context.Houses.Any(e => e.Id == id);
        }
    }
}
