using Microsoft.EntityFrameworkCore;
using RealEstate.Core.Interfaces.Employees;
using RealEstate.Core.Models;
using RealEstate.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RealEstate.Infrastructure.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        readonly RealEstateContext _realEstateContext;
        readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeRepository(RealEstateContext realEstateContext, IHttpContextAccessor httpContextAccessor)
        {
            _realEstateContext = realEstateContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Employee> GetEmployeByIdAsync(int Id)
        {
           return await _realEstateContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Employee> GetEmployeeByAuthIdAsyc(string Id)
        {
           return await _realEstateContext.Employees.FirstOrDefaultAsync(x => x.Auth0Sub == Id);
        }

        public async Task<List<Employee>> GetEmployeesAsycn()
        {
            return await _realEstateContext.Employees.ToListAsync();
        }

        public async Task<Employee> ReadEmployee()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context.User.Identity.IsAuthenticated)
            {
                var Id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Id != null)
                {
                    var Employee = await GetEmployeeByAuthIdAsyc(Id);
                    return Employee;
                }
            }
            else throw new Exception("Kullanıcı bulunamadı");
            return null;
        }
    }
}
