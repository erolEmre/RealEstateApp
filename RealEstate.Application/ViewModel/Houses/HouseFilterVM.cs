using RealEstate.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.ViewModel.Houses
{
    public class HouseFilterVM
    {
        public string? SearchText { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public double? MinArea { get; init; }
        public int? MaxArea { get; init; }
        public string? City { get; init; }
        public Enum_NumberOfRooms? NumberOfRooms { get; init; }
        public int? NumberOfBathrooms { get; init; }
        public string? SortOrder { get; init; }
        public bool isFiltered { get; init; }

        public List<string> AvailableCities { get; set; } = new List<string>();
        public List<int> AvailableBathrooms { get; set; } = new List<int>();
        public List<Enum_NumberOfRooms> AvailableRooms { get; set; } = new List<Enum_NumberOfRooms>();
    }
}
