using RealEstate.Core.Models;
using RealEstate.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Interfaces.Houses.HouseRepository
{
    public interface IHouseRepository
    {
        public Task<RealEstate.Core.Models.House?> GetByIdAsync(int id);
        public Task<List<RealEstate.Core.Models.House>> GetAllAsync();
        public Task AddAsync(RealEstate.Core.Models.House house);
        public Task UpdateAsync(RealEstate.Core.Models.House house);
        public void Remove(RealEstate.Core.Models.House house);
        public void SaveChanges();
        
        public IQueryable<RealEstate.Core.Models.House> GetAllWitAsQuery();
        Task<List<string>> GetAvailableCitiesAsync();
        Task<List<Enum_NumberOfRooms>> GetAvailableRoomCountsAsync();
        public Task<List<int>> GetAvailableBathroomCountAsync();
    }
}
