using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Interfaces
{
    public interface IHouseRepository
    {
        public Task<House?> GetByIdAsync(int id);
        public Task<List<House>> GetAllAsync();
        public Task AddAsync(House house);
        public void Remove(House house);

    }
}
