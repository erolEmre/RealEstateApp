using RealEstate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using RealEstate.Infrastructure.Context;
using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
