using RealEstate.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Models
{
    public class House
    {
        public House()
        {
            
        }
        public House(decimal price, string title,double area, int employeeId,Address address)
        {
            if (employeeId == 0)
                throw new Exception("EmployeeId required");

            Title = title;
            Address = address;
            ListingDate = DateTime.UtcNow;
            EmployeeId = employeeId;
            ChangeArea(area);
            ChangePrice(price);
            IsAvailable = true;
        }
        private void EnsureOwner(int currentEmployeeId)
        {
            if (EmployeeId != currentEmployeeId)
                throw new UnauthorizedAccessException("Unauthorized operation.");
        }

        public void UpdateDetails(string title,decimal price,int currentEmployeeId)
        {
            EnsureOwner(currentEmployeeId);
            Title = title;
            ChangePrice(price);
        }
        public void UpdateRooms(Enum_NumberOfRooms numberOfRooms, int numberOfBathrooms, int currentEmployeeId)
        {
            EnsureOwner(currentEmployeeId);
            NumberOfRooms = numberOfRooms;
            NumberOfBathrooms = numberOfBathrooms;
        }
        public int Id { get; private set; }
        [Display(Name = "Başlık")]
        public string Title { get; private set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; private set; } = "";
        [Display(Name = "Oda sayısı")]
        public Enum_NumberOfRooms NumberOfRooms { get; private set; }
        [Display(Name = "Banyo sayısı")]
        public int NumberOfBathrooms { get; private set; } = 1;
        [Display(Name = "Metrekare")]
        public double Area { get; private set; }

        public void ChangeArea(double area)
        {
            if (area <= 0)
                throw new Exception("Area must be positive.");

            Area = area;
        }
        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fiyat")]
        public decimal Price { get; private set; }
        public void ChangePrice(decimal price)
        {
            if (price <= 0)
                throw new Exception("Price must be positive.");
            Price = price;
        }
        [Display(Name = "İlan Tarihi")]
        public DateTime ListingDate { get; private set; }
        [Display(Name = "Uygunluk")]
        public bool IsAvailable { get; private set; }

        public void MarkAsUnavailable()
        {
            if (!IsAvailable)
                throw new Exception("House already unavailable.");

            IsAvailable = false;
        }
        public void SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
        }
              
        public void MarkAsAvailable()
        {
            IsAvailable = true;
        }
        public void SetAddress(Address address)
        {
            Address = address;
        }
        public void SetContactNumber(string contactNumber)
        {
            ContactNumber = contactNumber;
        }
        public void SetDescription(string description)
        {
           Description = description;
        }
        public void Publish()
        {
            if (Address == null)
                throw new Exception("Adres boşken ilan yayınlanamaz");
            MarkAsAvailable();
        }
        [Display(Name = "Telefon Numarası")]
        public string? ContactNumber { get; private set; } = default!;
        [Display(Name = "Adres")]
        public Address? Address { get; private set; } = default!;

        public int EmployeeId { get; private set; }
        public string ImageUrl { get; private set; } = default!;

        public Employee Employee { get; private set; }
    }
}
