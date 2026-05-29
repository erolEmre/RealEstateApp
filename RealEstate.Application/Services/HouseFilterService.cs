using RealEstate.Application.Interfaces;
using RealEstate.Application.ViewModel.Houses;
using RealEstate.Core.Interfaces;
using RealEstate.Core.Interfaces.Houses.HouseRepository;
using RealEstate.Core.Models;
using RealEstate.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Services
{
    public class HouseFilterService : IHouseFilterService
    {
        readonly IHouseRepository _houseRepository;

        public HouseFilterService(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        public IQueryable<House> Apply(IQueryable<House> query, HouseVMSort houseFilter)
        {
            
            query = _houseRepository.GetAllWitAsQuery();
            
            if (!string.IsNullOrEmpty(houseFilter.SearchText))
            {
                var words = houseFilter.SearchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                    query = query.Where(x =>
                        x.Address.City.Contains(word) ||
                        x.Title.Contains(word) ||
                        x.Description.Contains(word));
            }

            if (houseFilter.minPrice.HasValue) query = query.Where(x => x.Price >= houseFilter.minPrice);
            if (houseFilter.maxPrice.HasValue) query = query.Where(x => x.Price <= houseFilter.maxPrice);
            if (houseFilter.minArea.HasValue) query = query.Where(x => x.Area >= houseFilter.minArea);
            if (houseFilter.maxArea.HasValue) query = query.Where(x => x.Area <= houseFilter.maxArea);
            if (!string.IsNullOrEmpty(houseFilter.City)) query = query.Where(x => x.Address.City == houseFilter.City);
            if (houseFilter.IsRental != Enum_IsRental.All) query = query.Where(x => x.IsRental == houseFilter.IsRental);
            if (houseFilter.numberOfRooms.HasValue) query = query.Where(x => x.NumberOfRooms == houseFilter.numberOfRooms);
            if (houseFilter.NumberOfBathrooms.HasValue) query = query.Where(x => x.NumberOfBathrooms == houseFilter.NumberOfBathrooms);
            query = houseFilter.sortOrder switch
            {
                "date_asc" => query.OrderBy(x => x.ListingDate),
                "price_desc" => query.OrderByDescending(x => x.Price),
                "price_asc" => query.OrderBy(x => x.Price),
                _ => query.OrderByDescending(x => x.ListingDate)
            };
           
                houseFilter.TotalCount = query.Count();
                houseFilter.TotalPages = (int)Math.Ceiling(houseFilter.TotalCount / (double)houseFilter.Count);

            return query.Skip(houseFilter.Page * houseFilter.Count).Take(houseFilter.Count);
        }

    }
}
