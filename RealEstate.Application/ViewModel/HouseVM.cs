using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RealEstate.Core.Models;
using RealEstate.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.ViewModel
{
    public class HouseVM
    {
        public HouseVM() { }

        public int Id { get; set; }
        public string ImageUrl { get; set; }

        [Display(Name = "Başlık")]
        public string Title { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; } = "";
        [Display(Name = "Oda sayısı")]
        public Enum_NumberOfRooms NumberOfRooms { get; set; }
        [Display(Name = "Banyo sayısı")]
        public int NumberOfBathrooms { get; set; } = 1;
        [Display(Name = "Metrekare")]
        public double Area { get; set; }
        [Display(Name = "Telefon No.")]
        public string ContactNumber { get; set; } = default!;
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        public DateTime ListingDate { get; set; }
        [Display(Name = "Uygunluk")]
        public bool IsAvailable { get; set; }
        //public Address Address { get; set; } = default!;
        public bool IsRental { get; set; }

       
        public int EmployeeId { get; set; }
        public string EmployeeAuth0Sub { get; set; }
        [ValidateNever]
        public string EmployeeEmail { get; set; }
        [ValidateNever]
        public string EmployeeFirstName{ get; set; }
        [ValidateNever]
        public string EmployeeLastName { get; set; }


        public string City { get; init; } = default!;
        public string District { get; init; } = default!;
        public string? Street { get; init; }
        public string? BuildingNo { get; init; }
        public string? ApartmentNo { get; init; }

    }
}
