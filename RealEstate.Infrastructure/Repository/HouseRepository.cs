using RealEstate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using RealEstate.Infrastructure.Context;
using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using RealEstate.Core.Models.Enums;

namespace RealEstate.Infrastructure.Repository
{
    public class HouseRepository : IHouseRepository
    {
        private readonly RealEstateContext _context;
        public HouseRepository(RealEstateContext context)
        {
            _context = context;
        }
        public async Task AddAsync(House house)
        {
            await _context.Houses.AddAsync(house);
            _context.SaveChanges();
        }
        public async Task UpdateAsync(House house)
        {
            var h = await GetByIdAsync(house.Id);
            if (h != null)
            {
                h.UpdateDetails(house.Title,house.Price,house.EmployeeId);
            }
            _context.SaveChanges();
        }
        public async Task<List<House>> GetAllAsync()
        {
            return _context.Houses.ToList();
        }

        public async Task<House?> GetByIdAsync(int id)
        {
           return await _context.Houses.FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async void Remove(House house)
        {
           var h = await GetByIdAsync(house.Id);
            if (h != null)
            {
                h.MarkAsUnavailable();
            }
            _context.SaveChanges();
        }

        public async void SaveChanges()
        {
             _context.SaveChanges();
        }
        
        public IQueryable<House> GetAllWitAsQuery()
        {
            return _context.Houses
              .Include(h => h.Employee)
              .Include(a=> a.Address)
              .AsQueryable();
        }

        public async Task<List<string>> GetAvailableCitiesAsync()
        {
            return await _context.Houses.Where(x => x.Address.City != null)
                .Select(x => x.Address.City)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Enum_NumberOfRooms>> GetAvailableRoomCountsAsync()
        {
             return await _context.Houses.Where(x => x.NumberOfRooms != null
                  && x.NumberOfRooms >= 0)
                 .Select(x => x.NumberOfRooms)
                 .Distinct()
                 .ToListAsync();
    
        }

        public async Task<List<int>> GetAvailableBathroomCountAsync()
        {
            return await _context.Houses.Where(x => x.NumberOfBathrooms != null
                  && x.NumberOfBathrooms >= 0)
                 .Select(x => x.NumberOfBathrooms)
                 .Distinct()
                 .ToListAsync();
        }
    }
}
