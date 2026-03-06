using RealEstate.Core.Models;
using RealEstate.Core.Models.Enums;

namespace RealEstateApp.WebUI.Models
{
    public class HouseVM
    {
        public HouseVM() { }

        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = "";
        public Enum_NumberOfRooms NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; } = 1;
        public double Area { get; set; }
        public string ContactNumber { get; set; } = default!;
        public decimal Price { get; set; }
        public DateTime ListingDate { get; set; }
        public bool IsAvailable { get; set; }
        //public Address Address { get; set; } = default!;
        
    }
}
