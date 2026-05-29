using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Application.ViewModel.Houses;
using RealEstate.Core.Interfaces.Houses.HouseRepository;
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
        private readonly IHouseRepository _houseRepository;
        private readonly IMapper _mapper;
        readonly IHouseFilterService _houseFilterService;

        public HousesController(IMapper mapper, IHouseRepository houseRepository, IHouseFilterService houseFilterService)
        {
            _houseRepository = houseRepository;
            _mapper = mapper;
            _houseFilterService = houseFilterService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(HouseVMSort searchModel)
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = _houseRepository.GetAllWitAsQuery();

            searchModel.Houses = _houseFilterService.Apply(query, searchModel).ToList();
            searchModel.AvailableCities = await _houseRepository.GetAvailableCitiesAsync();
            searchModel.AvailableRooms = await _houseRepository.GetAvailableRoomCountsAsync();
            searchModel.AvailableBathrooms = await _houseRepository.GetAvailableBathroomCountAsync();
            
            return View(searchModel);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _houseRepository.GetAllWitAsQuery()
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

     
        [Authorize(Roles = "Agent")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
          
            var auth0Sub = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var Houses = await _houseRepository
                .GetAllWitAsQuery()
                .FirstOrDefaultAsync(x => x.Employee.Auth0Sub == auth0Sub);


            var model = new HouseVM
            {
                EmployeeId = Houses.Employee?.Id ?? 0,
                EmployeeAuth0Sub = Houses.Employee.Auth0Sub,
                EmployeeEmail = Houses.Employee.Email,
                EmployeeFirstName = Houses.Employee.FirstName,
                EmployeeLastName = Houses.Employee.LastName,
                 
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HouseVM houseVM)
        {
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
                await _houseRepository.AddAsync(house);
                return RedirectToAction(nameof(Index));
            }
          
            return View(house);
        }

        // GET: Houses/Edit/5
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Edit(int? id)
        {
            var house = await _houseRepository.GetAllWitAsQuery()
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
                                
                var existingHouse = await _houseRepository.GetByIdAsync(vm.Id);

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

               _houseRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _houseRepository
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
            var house = await _houseRepository.GetByIdAsync(id);
            if (house != null)
            {
                _houseRepository.Remove(house);
            }

            _houseRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
       
    }
}
