using RealEstate.Core.Models;
using RealEstate.Core.Models.Enums;

namespace RealEstate.Application.ViewModel.Houses
{
    public class HouseVMSort
    {
        public List<House> Houses { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
        public string? City { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? minArea { get; set; }
        public int? maxArea { get; set; }
        public string? SearchText { get; set; }
        public bool isFiltered 
        { 
            get 
            {
                if (minArea.HasValue || maxArea.HasValue 
                    || minPrice.HasValue || maxPrice.HasValue || !string.IsNullOrEmpty(City) || numberOfRooms.HasValue
                    ||NumberOfBathrooms.HasValue)
                    return true;
                else return false;
            } 
        }
        public string? sortOrder { get; set; }
        public Enum_NumberOfRooms? numberOfRooms { get; set; }
        public List<string> AvailableCities { get; set; } = new List<string>();
        public List<int> AvailableBathrooms { get; set; } = new List<int>();
        public List<Enum_NumberOfRooms> AvailableRooms { get; set; } = new List<Enum_NumberOfRooms>();
        public int Page { set; get; } = 0;
        public int Count { get; set; } = 6;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public Enum_IsRental IsRental { get; set; } = Enum_IsRental.All;
    }

}
