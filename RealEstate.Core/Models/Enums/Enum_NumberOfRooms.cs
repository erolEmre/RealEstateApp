using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Models.Enums
{
  public enum Enum_NumberOfRooms
  {
        [Display(Name = "1+0")]
        BirArtıSıfır = 0,
        [Display(Name = "1+1")]
        BirArtıBir = 1,
        [Display(Name = "2+1")]
        İkiArtıBir = 2, 
        [Display(Name = "3+1")]
        ÜçArtıBir = 3,
        [Display(Name = "4+1")]
        DörtArtıBir = 4,
        [Display(Name = "5+1")]
        BeşArtıBir = 5,        
    }
}
