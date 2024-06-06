﻿using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.Product;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(P => P.Brand, O => O.MapFrom(S => S.Brand.Name))
                .ForMember(P => P.Category, O => O.MapFrom(S => S.Category.Name))
                .ForMember(P => P.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<ShippingAddressDTO, ShippingAddress>();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(orderItemDto => orderItemDto.ProductId, O => O.MapFrom(orderItem => orderItem.Product.ProductId))
                .ForMember(orderItemDto => orderItemDto.ProductName, O => O.MapFrom(orderItem => orderItem.Product.ProductName))
                .ForMember(orderItemDto => orderItemDto.PictureURL, O => O.MapFrom(orderItem => orderItem.Product.PictureURL))
                .ForMember(orderItemDto => orderItemDto.PictureURL, O =>
                {
                    O.MapFrom<OrderItemPictureUrlResolver>();
                });

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(ordrToReturnDto => ordrToReturnDto.DeliveryMethod, O => O.MapFrom(order => order.DeliveryMethod.ShortName))
                .ForMember(ordrToReturnDto => ordrToReturnDto.DeliveryMethodCoast, O => O.MapFrom(order => order.DeliveryMethod.Cost));


        }
    }
}
