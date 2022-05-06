using AutoMapper;
using Insurance.Business.DTOs;
using Insurance.Business.Models;

namespace Insurance.Business.Profiles
{
    public class ProductServiceProfile : Profile
    {
        public ProductServiceProfile()
        {
            CreateMap<ProductType, ProductTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SurchargeRate, opt => opt.MapFrom(src => src.SurchargeRate));
        }
    }
}