using Microsoft.EntityFrameworkCore;
using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Context
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options): base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; } 
        public DbSet<House> Houses { get; set; }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.CreatedHouses)
                .WithOne(h => h.Employee)
                .HasForeignKey(h => h.EmployeeId);


            modelBuilder.Entity<House>(builder =>
            {
                builder.OwnsOne(h => h.Address);
                builder.Property(h => h.Price).HasColumnType("decimal(18,2)");
            });

        }
}
    
}
