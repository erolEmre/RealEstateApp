using AutoMapper;
using RealEstate.Application.ViewModel.Houses;
using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<House, HouseVM>()
            // Adres parçalarını VM'in köküne çıkar (Flattening)
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Address.District))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.BuildingNo, opt => opt.MapFrom(src => src.Address.BuildingNo))
            .ForMember(dest => dest.ApartmentNo, opt => opt.MapFrom(src => src.Address.ApartmentNo))

                // Çalışan (Employee) bilgilerini VM'e ekle
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id))
                .ForMember(dest => dest.EmployeeAuth0Sub, opt => opt.MapFrom(src => src.Employee.Auth0Sub));

            // ReverseMap() yok, çünkü sadece GET (Read-Only) için kullanıyoruz.    

            // Not: Title, Price, Description gibi isimleri aynı olan alanları 
            // AutoMapper zaten otomatik olarak eşler, onları tek tek yazmana gerek yok.

            

        }
    }
}
