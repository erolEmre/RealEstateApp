using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RealEstate.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Context
{
    public class RealEstateContextFactory : IDesignTimeDbContextFactory<RealEstateContext>
    {
        public RealEstateContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RealEstateContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=Master;User Id=sa;Password=YOUR_DB_PASSWORD_VARIABLE;TrustServerCertificate=True;"
            );

            return new RealEstateContext(optionsBuilder.Options);
        }
    }
}
