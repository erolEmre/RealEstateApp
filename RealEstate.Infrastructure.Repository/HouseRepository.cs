using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.Core.Interfaces;
using RealEstate.Core.Models;

namespace RealEstate.Infrastructure.Repository
{
    public class HouseRepository : IHouseRepository
    {
        public Task AddAsync(House house)
        {
            throw new NotImplementedException();
        }

        public Task<List<House>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<House?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(House house)
        {
            throw new NotImplementedException();
        }
    }
}
