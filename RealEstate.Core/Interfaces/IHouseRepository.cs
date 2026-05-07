using RealEstate.Core.Models;
using RealEstate.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Interfaces
{
    public interface IHouseRepository
    {
        public Task<House?> GetByIdAsync(int id);
        public Task<List<House>> GetAllAsync();
        public Task AddAsync(House house);
        public Task UpdateAsync(House house);
        public void Remove(House house);
        public void SaveChanges();
        //public Task<List<House>> GetAllWithEmployeesAsync();
        public IQueryable<House> GetAllWitAsQuery();
        Task<List<string>> GetAvailableCitiesAsync();
        Task<List<Enum_NumberOfRooms>> GetAvailableRoomCountsAsync();
        public Task<List<int>> GetAvailableBathroomCountAsync();
    }
}
