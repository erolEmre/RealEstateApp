namespace RealEstate.Core.Models
{
    public record Address
    {
        public string City { get; init; } = default!;
        public string District { get; init; } = default!;
        public string? Street { get; init; }
        public string? BuildingNo { get; init; }
        public string? ApartmentNo { get; init; }

        public override string ToString()
            => $"{District}, {City}";

    }
}