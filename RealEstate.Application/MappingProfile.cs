using AutoMapper;
using RealEstate.Application.ViewModel;
using RealEstate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Mappings
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {

            CreateMap<House, HouseVM>()
            // Eğer House içinde "Address.City" varsa ve VM içinde sadece "City" varsa:
            // .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Address.District))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.BuildingNo, opt => opt.MapFrom(src => src.Address.BuildingNo))
            .ForMember(dest => dest.ApartmentNo, opt => opt.MapFrom(src => src.Address.ApartmentNo))
            //.ForMember(dest=> dest.Employee.Id, opt=>opt.MapFrom(src=> src.EmployeeId))
            .ReverseMap(); // Bu sayede VM -> House dönüşümü de aktif olur


            //CreateMap<Employee, Employee>().ReverseMap();
        }
    }
}
