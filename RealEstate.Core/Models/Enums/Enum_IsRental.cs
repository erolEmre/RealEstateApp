using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Models.Enums
{
    public enum Enum_IsRental
    {
        [Display(Name = "Satılık")]// Hem Kiralık hem Satılık (Filtreleme için)
        Sale = 0,
        [Display(Name = "Kiralık")]// Satılık
        Rent = 1,      // Kiralık
        All = 2

}
}
