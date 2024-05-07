using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.Product;
using Talabat.Core.Entitites.Basket;
using Talabat.Core.Entitites.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        private readonly IConfiguration _configuration;

        public MappingProfiles(IConfiguration configuration)
        {
            _configuration = configuration;

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(P => P.Brand, O => O.MapFrom(S => S.Brand.Name))
                .ForMember(P => P.Category, O => O.MapFrom(S => S.Category.Name))
                //.ForMember(P => P.PictureUrl, O => O.MapFrom(S => $"{_configuration["ApiBaseUrl"]}/{S.PictureUrl}"));
                .ForMember(P => P.Category, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
			CreateMap<Address, AddressDto>();
			CreateMap<Address, AddressDto>().ReverseMap();

            //CreateMap<ShippingAddressDTO, Core.Entities.Order_Aggregate.ShippingAddress>();


        }
    }
}
