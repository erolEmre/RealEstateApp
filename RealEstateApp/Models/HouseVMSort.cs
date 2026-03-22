using RealEstate.Core.Models;

namespace RealEstateApp.WebUI.Models
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

    }

}
