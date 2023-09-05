﻿using AutoMapper;
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
    public MapperGlobalProfile()
    {
        CreateMap<Product, PageProductDTO>()
            .ForMember(dest => dest.SameProducts, src => src.Ignore());
        CreateMap<Product, ListProductDTO>();
        CreateMap<Brand, BrandDTO>();
        CreateMap<Demand, DemandDTO>();
        CreateMap<Country, CountryDTO>();
        CreateMap<SubCategory, SubCategoryDTO>();
        CreateMap<AppFile, AppFileDTO>();
        CreateMap<Feedback, FeedbackDTO>();
        CreateMap<User, UserDTO>();
    }
}
