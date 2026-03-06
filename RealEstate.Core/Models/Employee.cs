
namespace RealEstate.Core.Models
{
    public class Employee
    {
        public Employee()
        {
            
        }
        public Employee(string auth0Sub, string email)
        {
            Auth0Sub = auth0Sub;
            Email = email;
        }
        public int Id { get; private set; }
        public string Auth0Sub { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        private readonly List<House> _createdHouses = new();
        public IReadOnlyCollection<House> CreatedHouses => _createdHouses;

        public House CreateHouse(decimal price,double area, string title)
        {
            var house = new House(price, title,area, Id);
            _createdHouses.Add(house);
            return house;
        }

    }
}
