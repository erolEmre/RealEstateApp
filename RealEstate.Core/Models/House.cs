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
        public House(decimal price, string title,double area, int employeeId)
        {
            if (employeeId == 0)
                throw new Exception("EmployeeId required");

            Title = title;
            //Address = address;
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
        public string Title { get; private set; } 
        public string? Description { get; private set; } = "";
        public Enum_NumberOfRooms NumberOfRooms { get; private set; }
        public int NumberOfBathrooms { get; private set; } = 1;
        public double Area { get; private set; }

        public void ChangeArea(double area)
        {
            if (area <= 0)
                throw new Exception("Area must be positive.");

            Area = area;
        }
        [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
        public decimal Price { get; private set; }
        public void ChangePrice(decimal price)
        {
            if (price <= 0)
                throw new Exception("Price must be positive.");
            Price = price;
        }
        public DateTime ListingDate { get; private set; }
        public bool IsAvailable { get; private set; }

        public void MarkAsUnavailable()
        {
            if (!IsAvailable)
                throw new Exception("House already unavailable.");

            IsAvailable = false;
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
        public string? ContactNumber { get; private set; } = default!;

        public Address? Address { get; private set; } = default!;

        public int EmployeeId { get; private set; }
        //private readonly List<string?> _images = new();
        //public IReadOnlyCollection<string?> Images => _images;

        public Employee Employee { get; private set; }
    }
}
