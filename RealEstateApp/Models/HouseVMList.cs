using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.WebUI.Models
{
    public class HouseVMList
    {
        public List<HouseVM> Houses { get; set; } = new List<HouseVM>();
    }
}
