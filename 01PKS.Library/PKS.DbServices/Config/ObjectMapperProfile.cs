using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PKS.DbModels;
using PKS.DbServices.Models;
using PKS.Utils;
using PKS.Services;

namespace PKS.DbServices
{
    /// <summary>自定义对象映射配置</summary>
    public sealed class ObjectMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public ObjectMapperProfile()
        {
            CreateMap<PKS_KG_PublicCatalog, KG_CatalogNode>()
                //.ForMember(desc => desc.Title, opt => opt.MapFrom(src => src.Code + ":" + src.Name))
                //.ForMember(desc => desc.ParentId, opt => opt.ResolveUsing(src => src.ParentId.GetValueOrDefault()))
                .ForMember(desc => desc.PatternURL, opt => opt.MapFrom(src => src.ImageURL))
                .ForMember(desc => desc.ImageURL, opt => opt.ResolveUsing(src => src.ImageURL.ResolveBracketPattern()))
                .ForMember(desc => desc.Level, opt => opt.MapFrom(src => src.LevelNumber))
                .ForMember(desc => desc.Order, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(desc => desc.CreatedDate, opt => opt.ResolveUsing(src => src.CreatedDate.ToStandardString()))
                .ForMember(desc => desc.LastUpdatedDate, opt => opt.ResolveUsing(src => src.LastUpdatedDate.ToStandardString()))
                .ReverseMap()
                //.ForMember(desc => desc.ParentId, opt => opt.ResolveUsing(src => ToNullableInt(src.ParentId)))
                .ForMember(desc => desc.ImageURL, opt => opt.MapFrom(src => src.PatternURL))
                .ForMember(desc => desc.CreatedDate, opt => opt.Ignore())
                .ForMember(desc => desc.LastUpdatedDate, opt => opt.Ignore())
                .ForMember(desc => desc.Topics, opt => opt.Ignore())
                .ForMember(desc => desc.Children, opt => opt.Ignore())
                .ForMember(desc => desc.Parent, opt => opt.Ignore())
                ;
            CreateMap<PKS_KG_PrivateCatalog, KG_CatalogNode>()
                //.ForMember(desc => desc.Title, opt => opt.MapFrom(src => src.Code + ":" + src.Name))
                //.ForMember(desc => desc.ParentId, opt => opt.ResolveUsing(src => src.ParentId.GetValueOrDefault()))
                .ForMember(desc => desc.PatternURL, opt => opt.MapFrom(src => src.ImageURL))
                .ForMember(desc => desc.ImageURL, opt => opt.ResolveUsing(src => src.ImageURL.ResolveBracketPattern()))
                .ForMember(desc => desc.Level, opt => opt.MapFrom(src => src.LevelNumber))
                .ForMember(desc => desc.Order, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(desc => desc.CreatedDate, opt => opt.ResolveUsing(src => src.CreatedDate.ToStandardString()))
                .ForMember(desc => desc.LastUpdatedDate, opt => opt.ResolveUsing(src => src.LastUpdatedDate.ToStandardString()))
                .ReverseMap()
                //.ForMember(desc => desc.ParentId, opt => opt.ResolveUsing(src => ToNullableInt(src.ParentId)))
                .ForMember(desc => desc.ImageURL, opt => opt.MapFrom(src => src.PatternURL))
                .ForMember(desc => desc.CreatedDate, opt => opt.Ignore())
                .ForMember(desc => desc.LastUpdatedDate, opt => opt.Ignore())
                .ForMember(desc => desc.Topics, opt => opt.Ignore())
                .ForMember(desc => desc.Children, opt => opt.Ignore())
                .ForMember(desc => desc.Parent, opt => opt.Ignore())
                ;
            CreateMap<PKS_KG_PrivateCatalog, KG_CatalogModel>()
                //.ForMember(desc => desc.Title, opt => opt.MapFrom(src => src.Code + ":" + src.Name))
                //.ForMember(desc => desc.ParentId, opt => opt.ResolveUsing(src => src.ParentId.GetValueOrDefault()))
                .ForMember(desc => desc.PatternURL, opt => opt.MapFrom(src => src.ImageURL))
                .ForMember(desc => desc.ImageURL, opt => opt.ResolveUsing(src => src.ImageURL.ResolveBracketPattern()))
                .ForMember(desc => desc.Level, opt => opt.MapFrom(src => src.LevelNumber))
                .ForMember(desc => desc.Order, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(desc => desc.CreatedDate, opt => opt.ResolveUsing(src => src.CreatedDate.ToStandardString()))
                .ForMember(desc => desc.LastUpdatedDate, opt => opt.ResolveUsing(src => src.LastUpdatedDate.ToStandardString()))
                .ForMember(desc => desc.Children, opt => opt.Ignore())
                .ForMember(desc => desc.Parent, opt => opt.Ignore())
                .ReverseMap()
                //.ForMember(desc => desc.ParentId, opt => opt.ResolveUsing(src => ToNullableInt(src.ParentId)))
                .ForMember(desc => desc.ImageURL, opt => opt.MapFrom(src => src.PatternURL))
                .ForMember(desc => desc.CreatedDate, opt => opt.Ignore())
                .ForMember(desc => desc.LastUpdatedDate, opt => opt.Ignore())
                .ForMember(desc => desc.Topics, opt => opt.Ignore())
                .ForMember(desc => desc.Children, opt => opt.Ignore())
                .ForMember(desc => desc.Parent, opt => opt.Ignore())
                ;
            CreateMap<PKS_KG_Topic, KG_TopicModel>()
                .ForMember(desc => desc.CreatedDate, opt => opt.ResolveUsing(src => src.CreatedDate.Value.ToStandardDateString()))
                .ForMember(desc => desc.PrivateCatalogs, opt => opt.ResolveUsing(src => src.GetPrivateCatalogs()))
                .ForMember(desc => desc.PublicCatalogs, opt => opt.ResolveUsing(src => src.GetPublicCatalogs()))
                ;
            CreateMap<KG_NewTopic, PKS_KG_Topic>()
                .ForMember(desc => desc.Contents, opt => opt.Ignore())
                .ForMember(desc => desc.CreatedBy, opt => opt.Ignore())
                .ForMember(desc => desc.CreatedDate, opt => opt.Ignore())
                .ForMember(desc => desc.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(desc => desc.LastUpdatedDate, opt => opt.Ignore())
                .ForMember(desc => desc.PrivateCatalog, opt => opt.Ignore())
                .ForMember(desc => desc.PublicCatalog, opt => opt.Ignore())
                ;
            CreateMap<PKS_KG_Topic, KG_NewTopicModel>()
                .ForMember(desc => desc.CreatedDate, opt => opt.ResolveUsing(src => src.CreatedDate.Value.ToStandardDateString()))
                ;
        }
    }
}
