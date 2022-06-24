using AquaWater.Domain.Commons;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Common;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using System;
using System.Linq;

namespace AquaWater.Dto.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CompanyManager, CompanyManagerRequestDTO>().ReverseMap();
            CreateMap<CustomerRequestDTO, Customer>()
                .ForMember(x => x.User, y => y.Ignore());
            CreateMap<Location, LocationRequestDTO>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<User, EditUserResponseDTO>()
            .ForMember(x => x.Location, y => y.MapFrom(x => x.Location));
            CreateMap<Location, LocationDTO>();
            CreateMap<User, UserRegistrationRequestDTO>().ReverseMap();
            CreateMap<Customer, CustomerRequestDTO>().ReverseMap()
            .ForMember(x => x.User, y => y.MapFrom(x => x.User));


            CreateMap<Company, CompanyManagerRequestDTO>().ReverseMap();
            CreateMap<Notification, NotificationRequestDTO>().ReverseMap();
            CreateMap<OrderRequest, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem));
            CreateMap<CreateOrderItem, OrderItem>();
            CreateMap<OrderItemRequestDTO, OrderItem>()
                .ForMember(x => x.Orders, y => y.Ignore())
                .ForMember(x => x.Product, y => y.Ignore());
            CreateMap<Order, OrderRequest>();
            CreateMap<OrderRequest, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem))
                .ForMember(x => x.Transaction, y => y.Ignore())
                .ForMember(x => x.Customer, y => y.Ignore());
            CreateMap<OrderItem, OrderItemResponseDTO>();

            CreateMap<OrderResponse, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem))

                .ReverseMap();

            CreateMap<Company, CompanyResponseDTO>()
                .ForMember(x => x.Location, y => y.MapFrom(z => z.Location))
                .ForMember(x => x.Product, y => y.MapFrom(z => z.Product.FirstOrDefault()));

            CreateMap<Product, ProductDTO>()
                .ForMember(x => x.NoOfRating, y => y.MapFrom(z => z.Ratings != null ? z.Ratings.Count : 0))
                .ForMember(x => x.Photos, y => y.MapFrom(z => z.ProductGallery))
                .ForMember(x => x.Rating, y => y.MapFrom(z => z.Ratings != null && z.Ratings.Count > 0 ?
                                    (z.Ratings.Sum(x => x.Rate) / z.Ratings.Count) : 0)).ReverseMap();

            CreateMap<Location, LocationResponseDTO>();
            CreateMap<Review, ReviewResponseDTO>().ReverseMap();
            CreateMap<ProductGallery, ImageResponseDTO>().ReverseMap();
            CreateMap<Product, ProductResponseDTO>()
            .ForMember(x => x.Photos, y => y.MapFrom(z => z.ProductGallery))
            .ForMember(x => x.Reviews, y => y.MapFrom(z => z.Reviews))
            .ReverseMap();

            CreateMap<Order, OrderResponse>();
            CreateMap<OrderDTO, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem))
                .ForMember(x => x.Id, y => y.Ignore());
            CreateMap<Order, OrderDTO>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem));
            CreateMap<OrderItemRequestDTO, OrderItem>();
            CreateMap<OrderItem, OrderItemRequestDTO>();


            CreateMap<UpdateOrderResponseData, Order>();
            CreateMap<Order, UpdateOrderResponseData>();
            CreateMap<UpdateOrderResponseData, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem));
            CreateMap<Order, UpdateOrderResponseData>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem));

            CreateMap<UpdateOrderItemResponseData, OrderItem>();
            CreateMap<OrderItem, UpdateOrderItemResponseData>();

            CreateMap<UpdateOrderRequestDTO, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItems))
                .ForMember(x => x.Transaction, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore());
            CreateMap<Order, UpdateOrderRequestDTO>();

            CreateMap<UpdateOrderItemRequest, OrderItem>();
            CreateMap<OrderItem, UpdateOrderItemRequest>();

            CreateMap<Order, UpdateOrderResponseData>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem));

            CreateMap<Company, CompanyBasicResponseDTO>();


            CreateMap<ProductUpdateRequest, Product>()
                .ForMember(x => x.ProductGallery, y => y.MapFrom(x => x.ProductGallery))
                .ForMember(x => x.CustomerFavourites, y => y.Ignore())
                .ForMember(x => x.Ratings, y => y.Ignore())
                .ForMember(x => x.Reviews, y => y.Ignore());
            CreateMap<ProductGalleryUpdateRequest, ProductGallery>();

            CreateMap<Product, ProductUpdateRequest>()
                .ForMember(x => x.ProductGallery, y => y.MapFrom(x => x.ProductGallery));
            CreateMap<ProductGallery, ProductGalleryUpdateRequest>();

            CreateMap<UpdateProductResponse, ProductUpdateRequest>()
                .ForMember(x => x.ProductGallery, y => y.MapFrom(x => x.ProductGallery));

            CreateMap<ProductUpdateRequest, UpdateProductResponse>()
                .ForMember(x => x.ProductGallery, y => y.MapFrom(x => x.ProductGallery));

            CreateMap<ProductGalleryUpdateRequest, ProductGalleryUpdateResponse>();
            CreateMap<ProductGalleryUpdateResponse, ProductGalleryUpdateRequest>();


            CreateMap<ProductCreateRequestDTO, Product>()
                .ForMember(x => x.ProductGallery, y => y.MapFrom(x => x.ProductGallery))
                .ForMember(x => x.Ratings, y => y.Ignore())
                .ForMember(x => x.Reviews, y => y.Ignore())
                .ForMember(x => x.CustomerFavourites, y => y.Ignore());
            CreateMap<Product, ProductCreatedResponseDTO>()
                .ForMember(x => x.ProductGallery, y => y.MapFrom(x => x.ProductGallery));

            CreateMap<CreateProductGalleryRequestDTO, ProductGallery>();
            CreateMap<ProductGallery, CreateProductGalleryRequestDTO>();

            CreateMap<ProductCreatedResponseDTO, Product>()
                .ForMember(x => x.ProductGallery, y => y.MapFrom(x => x.ProductGallery))
                .ForMember(x => x.CustomerFavourites, y => y.Ignore())
                .ForMember(x => x.Ratings, y => y.Ignore())
                .ForMember(x => x.Reviews, y => y.Ignore());
            CreateMap<Product, ProductCreatedResponseDTO>()
                .ForMember(x => x.ProductGallery, y => y.MapFrom(x => x.ProductGallery));

            CreateMap<ProductGalleryCreatedResponseDTO, ProductGallery>().ReverseMap();
            CreateMap<ProductGallery, ProductGalleryCreatedResponseDTO>();


        }
    }
}
