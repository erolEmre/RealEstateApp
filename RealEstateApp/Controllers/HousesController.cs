using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.ViewModel;
using RealEstate.Core.Interfaces;
using RealEstate.Core.Models;
using RealEstate.Core.Models.Enums;
using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Repository;
using RealEstateApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealEstateApp.WebUI.Controllers
{
    [Authorize(Roles = "Agent")]
    public class HousesController : Controller
    {
        private readonly IHouseRepository _houseRepositroy;
        private readonly IMapper _mapper;

        public HousesController(IMapper mapper,IHouseRepository houseRepository)
        {
            _houseRepositroy = houseRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(HouseVMSort searchModel)
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = _houseRepositroy.GetAllWitAsQuery();

            searchModel.AvailableCities = await _houseRepositroy.GetAvailableCitiesAsync();

            searchModel.AvailableRooms = await _houseRepositroy.GetAvailableRoomCountsAsync();
            
            searchModel.AvailableBathrooms = await _houseRepositroy.GetAvailableBathroomCountAsync();
            
            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
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

            if (searchModel.minPrice.HasValue)
            {
                query = query.Where(x => x.Price >= searchModel.minPrice.Value);
            }

            if (searchModel.maxPrice.HasValue)
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
            if (!String.IsNullOrEmpty(searchModel.City))
            {
                query = query.Where(x => x.Address.City == searchModel.City);
            }
            // Oda Sayısı
            if (searchModel.numberOfRooms.HasValue)
            {
                query = query.Where(x => x.NumberOfRooms == searchModel.numberOfRooms.Value);
            }
            // Banyo Sayısı
            if (searchModel.NumberOfBathrooms.HasValue)
            {
                query = query.Where(x => x.NumberOfBathrooms == searchModel.NumberOfBathrooms.Value);
            }

            query = searchModel.sortOrder switch
            {
                "date_asc" => query.OrderBy(x => x.ListingDate),
                "price_desc" => query.OrderByDescending(x => x.Price),
                "price_asc" => query.OrderBy(x => x.Price),
                _ => query.OrderByDescending(x => x.ListingDate) // Varsayılan sıralama (En yeniler)
            };

            searchModel.Houses = await query.ToListAsync();

            return View(searchModel);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _houseRepositroy.GetAllWitAsQuery()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (house == null)
            {
                return NotFound();
            }
            
            var houseVM = _mapper.Map<HouseVM>(house);
            return View(houseVM);
        }

        [AllowAnonymous]
        public IActionResult AgentLogin()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "Auth0");
        }

        // GET: Houses/Create
        [Authorize(Roles = "Agent")]

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // 1. Oturum açmış kullanıcının Auth0 ID'sini al
            var auth0Sub = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // 2. Veritabanından bu çalışanı bul
            var Houses = await _houseRepositroy
                .GetAllWitAsQuery()
                .FirstOrDefaultAsync(x => x.Employee.Auth0Sub == auth0Sub);


            // 3. Modeli daha en baştan bu bilgilerle doldur
            var model = new HouseVM
            {
                EmployeeId = Houses.Employee?.Id ?? 0,
                EmployeeAuth0Sub = Houses.Employee.Auth0Sub,
                EmployeeEmail = Houses.Employee.Email,
                EmployeeFirstName = Houses.Employee.FirstName,
                EmployeeLastName = Houses.Employee.LastName,
                                                              
                // Eğer çalışan yoksa 0 döner
                // İstersen ekranda göstermek için ismini de ekleyebilirsin
            };

            // 4. İçi dolu modeli View'a gönder (Böylece Layout hatası almazsın)
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HouseVM houseVM)
        {
            if (houseVM.EmployeeId == 0)
            {
                throw new Exception("");
            }

            if (!ModelState.IsValid)
                return View(houseVM);


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
                houseVM.EmployeeId,
                address
            );
            house.UpdateRooms(houseVM.NumberOfRooms, houseVM.NumberOfBathrooms, houseVM.EmployeeId);
            house.SetDescription(houseVM.Description);
            house.SetContactNumber(houseVM.ContactNumber);
            house.SetImageUrl(houseVM.ImageUrl);
            house.ListingStatus(houseVM.IsRental);


            if (ModelState.IsValid)
            {
                await _houseRepositroy.AddAsync(house);
                return RedirectToAction(nameof(Index));
            }
          
            return View(house);
        }

        // GET: Houses/Edit/5
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Edit(int? id)
        {
            var house = await _houseRepositroy.GetAllWitAsQuery()
                        .FirstOrDefaultAsync(x => x.Id == id);
           

            if (house == null) return NotFound();

            // TEK SATIR: House -> HouseVM dönüşümü
            var houseVM = _mapper.Map<HouseVM>(house);

            return View(houseVM);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HouseVM vm)
        {
          
            if (ModelState.IsValid)
            {
                
                //var existingHouse = await _context.Houses.FindAsync(vm.Id);
                var existingHouse = await _houseRepositroy.GetByIdAsync(vm.Id);

                existingHouse.UpdateDetails(vm.Title, vm.Price, vm.EmployeeId);
                existingHouse.SetDescription(vm.Description);
                existingHouse.SetAddress(new Address
                {
                    ApartmentNo = vm.ApartmentNo,
                    BuildingNo = vm.BuildingNo,
                    Street = vm.Street,
                    City = vm.City,
                    District = vm.District
                });
                existingHouse.SetContactNumber(vm.ContactNumber);
                existingHouse.UpdateRooms(vm.NumberOfRooms, vm.NumberOfBathrooms, vm.EmployeeId);
                existingHouse.ChangeArea(vm.Area);
                existingHouse.SetImageUrl(vm.ImageUrl);
                existingHouse.ListingStatus(vm.IsRental);

               _houseRepositroy.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Houses/Delete/5
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _houseRepositroy
                .GetAllWitAsQuery()
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
            var house = await _houseRepositroy.GetByIdAsync(id);
            if (house != null)
            {
                _houseRepositroy.Remove(house);
            }

            _houseRepositroy.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
       
    }
}
