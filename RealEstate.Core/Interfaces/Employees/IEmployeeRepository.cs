using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Interfaces.Employees
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeByIdAsync(int Id);
        Task<List<Employee>> GetEmployeesAsycn();
        Task<Employee> GetEmployeeByAuthIdAsyc(string Id);
        Task<Employee> ReadEmployee();
    }
}
