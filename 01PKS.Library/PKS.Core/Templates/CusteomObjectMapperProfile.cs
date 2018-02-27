using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace PKS.Core.Template
{
    /// <summary>自定义对象映射配置</summary>
    public sealed class CusteomObjectMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public CusteomObjectMapperProfile()
        {
            //CreateMap<CRE_RoleBO, BORoleModel>()
            //    .ReverseMap();
            //CreateMap<CRE_BOLayer, BOTRoleLayerModel>()
            //    .ReverseMap()
            //    .ForMember(desc => desc.Id, opt => opt.Ignore())
            //    .ForMember(desc => desc.RoleId, opt => opt.Ignore())
            //    ;
            //CreateMap<CRE_BOT, BOTModel>()
            //    .IgnoreAllPropertiesWithAnInaccessibleSetter()
            //    .ReverseMap();
            //CreateMap<CRE_DefaultBOComparedPT, BOTCompareModel>()
            //    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            //    .ReverseMap();
            //CreateMap<GT_ETLConverters, ETLConverterModel>()
            //    .ReverseMap()
            //    .IgnoreAuditedMembers();
            //CreateMap<CRE_BOFields, BOFieldModel>()
            //    .ForMember(dest => dest.BOTIds, opt => opt.ResolveUsing(src => ModelUtil.ToIdList(src.BOTs)))
            //    .ReverseMap()
            //    .ForMember(src => src.BOTs, opt => opt.ResolveUsing(src => ModelUtil.ToIdsString(src.BOTIds)))
            //    ;
            //CreateMap<CRE_A2Services, A2ServiceModel>()
            //    .ReverseMap()
            //    .IgnoreAuditedMembers()
            //    ;
        }
    }
}
