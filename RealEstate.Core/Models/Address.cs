using System.ComponentModel.DataAnnotations;

namespace RealEstate.Core.Models
{
    public record Address
    {
        [Display(Name = "Şehir")]
        public string City { get; init; } = default!;
        [Display(Name = "İlçe")]
        public string District { get; init; } = default!;
        [Display(Name = "Sokak")]
        public string? Street { get; init; }
        [Display(Name = "Bina Numaarası")]
        public string? BuildingNo { get; init; }
        [Display(Name = "Daire Numarası")]
        public string? ApartmentNo { get; init; }

        public override string ToString()
            => $"{District}, {City}";

    }
}