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
using RealEstate.Core.Interfaces.Houses.HouseRepository;

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
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(House house)
        {
            var h = await GetByIdAsync(house.Id);
            if (h != null)
            {
                h.UpdateDetails(house.Title,house.Price,house.EmployeeId);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<List<House>> GetAllAsync()
        {
            return await _context.Houses.ToListAsync();
        }

        public async Task<House?> GetByIdAsync(int id)
        {
           return await _context.Houses.FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task Remove(House house)
        {
           var h = await GetByIdAsync(house.Id);
            if (h != null)
            {
                _context.Houses.Remove(h);
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }
        
        public IQueryable<House> GetAllWitAsQuery()
        {
            return _context.Houses
              .Include(h => h.Employee)
              .Include(a=> a.Address)
              .AsQueryable();
        }

        
    }
}
