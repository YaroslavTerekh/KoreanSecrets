using AutoMapper;
using KoreanSecrets.Domain.Common.Settings;
using KoreanSecrets.Domain.DataTransferObjects;
using KoreanSecrets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.BL.Helpers;

public class MapperGlobalProfile : Profile
{
    public MapperGlobalProfile(HostSettings hostSettings)
    {
        CreateMap<Product, PageProductDTO>()
            .ForMember(dest => dest.SameProducts, src => src.Ignore())
            .ForMember(dest => dest.Volumes, src => src.MapFrom(t => t.Volumes.OrderByDescending(t => t.Price)))
            .ForMember(dest => dest.Icon, src => src.MapFrom(t => t.AdditionalIcon))
            .ForMember(dest => dest.Quantity, src => src.MapFrom(t => t.Quantity));
        CreateMap<Product, ListProductDTO>()
            .ForMember(dest => dest.Volumes, src => src.MapFrom(t => t.Volumes.OrderByDescending(t => t.Price)))
            .ForMember(dest => dest.Icon, src => src.MapFrom(t => t.AdditionalIcon))
            .ForMember(dest => dest.Quantity, src => src.MapFrom(t => t.Quantity));
        CreateMap<Bucket, BucketDTO>();
        CreateMap<Brand, BrandDTO>();
        CreateMap<Demand, DemandDTO>();
        CreateMap<Country, CountryDTO>();
        CreateMap<SubCategory, SubCategoryDTO>();
        CreateMap<AppFile, AppFileDTO>();
        CreateMap<Feedback, FeedbackDTO>();
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.OrdersCount, src => src.MapFrom(t => t.Purchases.Count))
            .ForMember(dest => dest.IsPhoneNumberConfirmed, src => src.Ignore()); // ToDo: add phone number confirmation
        CreateMap<Volume, VolumeDTO>();
        CreateMap<Report, ReportDTO>();
        CreateMap<Banner, BannerDTO>();
        CreateMap<Category, CategoryDTO>();
            //.ForMember(dest => dest.Demands, src => src.MapFrom(t => t.CategoryDemands.Select(t => t.Demand).ToList()))
            //.ForMember(dest => dest.SubCategories, src => src.MapFrom(t => t.CategorySubCategories.Select(t => t.SubCategory).ToList()))
            //.ForMember(dest => dest.Countries, src => src.MapFrom(t => t.CategoryCountries.Select(t => t.Country).ToList()))
            //.ForMember(dest => dest.Brands, src => src.MapFrom(t => t.CategoryBrands.Select(t => t.Brand).ToList()));
        CreateMap<PurchasedProduct, PurchaseProductDTO>();
        CreateMap<AddressInfo, AddressInfoDTO>();
        CreateMap<AppFile, AppFileDTO>();
        //.ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => String.Concat(hostSettings.ApplicationUrl, src.FilePath.Replace(@"\", "/"))));
        CreateMap<ProductUser, ListProductDTO>()
            .ForMember(dest => dest.MainPhoto, src => src.MapFrom(t => t.Likes.MainPhoto))
            .ForMember(dest => dest.MainPhotoId, src => src.MapFrom(t => t.Likes.MainPhotoId))
            .ForMember(dest => dest.Quantity, src => src.MapFrom(t => t.Likes.Quantity))
            .ForMember(dest => dest.CreatedDate, src => src.MapFrom(t => t.Likes.CreatedDate))
            .ForMember(dest => dest.Brand, src => src.MapFrom(t => t.Likes.Brand))
            .ForMember(dest => dest.BrandId, src => src.MapFrom(t => t.Likes.BrandId))
            .ForMember(dest => dest.DiscountPrice, src => src.MapFrom(t => t.Likes.DiscountPrice))
            .ForMember(dest => dest.Icon, src => src.MapFrom(t => t.Likes.AdditionalIcon))
            .ForMember(dest => dest.Id, src => src.MapFrom(t => t.Likes.Id))
            .ForMember(dest => dest.IsInStock, src => src.MapFrom(t => t.Likes.IsInStock))
            .ForMember(dest => dest.IsLikedByUser, src => src.Ignore())
            .ReverseMap();
    }
}
