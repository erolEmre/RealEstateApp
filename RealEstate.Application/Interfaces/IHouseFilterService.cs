using RealEstate.Application.ViewModel.Houses;
using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Interfaces
{
    public interface IHouseFilterService
    {
        public IQueryable<House> Apply(IQueryable<House> query, HouseVMSort houseFilter);

    }
}
