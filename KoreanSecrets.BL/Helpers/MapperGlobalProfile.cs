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
            .ForMember(dest => dest.Icon, src => src.MapFrom(t => t.AdditionalIcon));
        CreateMap<Product, ListProductDTO>()
            .ForMember(dest => dest.Icon, src => src.MapFrom(t => t.AdditionalIcon));
        CreateMap<Bucket, BucketDTO>();
        CreateMap<Brand, BrandDTO>();
        CreateMap<Demand, DemandDTO>();
        CreateMap<Country, CountryDTO>();
        CreateMap<SubCategory, SubCategoryDTO>();
        CreateMap<AppFile, AppFileDTO>();
        CreateMap<Feedback, FeedbackDTO>();
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.OrdersCount, src => src.Ignore())
            .ForMember(dest => dest.IsPhoneNumberConfirmed, src => src.Ignore()); // ToDo: add phone number confirmation
        CreateMap<Volume, VolumeDTO>();
        CreateMap<Banner, BannerDTO>();
        CreateMap<Category, CategoryDTO>();
            //.ForMember(dest => dest.Demands, src => src.MapFrom(t => t.CategoryDemands.Select(t => t.Demand).ToList()))
            //.ForMember(dest => dest.SubCategories, src => src.MapFrom(t => t.CategorySubCategories.Select(t => t.SubCategory).ToList()))
            //.ForMember(dest => dest.Countries, src => src.MapFrom(t => t.CategoryCountries.Select(t => t.Country).ToList()))
            //.ForMember(dest => dest.Brands, src => src.MapFrom(t => t.CategoryBrands.Select(t => t.Brand).ToList()));
        CreateMap<PurchasedProduct, PurchaseProductDTO>();
        CreateMap<AppFile, AppFileDTO>()
            .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => String.Concat(hostSettings.ApplicationUrl, src.FilePath.Replace(@"\", "/"))));
    }
}
