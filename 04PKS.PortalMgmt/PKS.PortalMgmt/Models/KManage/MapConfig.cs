using AutoMapper;
using PKS.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models.KManage
{
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<PKS_KTEMPLATE_PARAMETER, TemplateParameterDTO>();
            CreateMap<TemplateParameterDTO, PKS_KTEMPLATE_PARAMETER>()
                .ForMember(t => t.CREATEDBY, opt => opt.Ignore())
                .ForMember(t => t.CREATEDDATE, opt => opt.Ignore())
                .ForMember(t => t.LASTUPDATEDBY, opt => opt.Ignore())
                .ForMember(t => t.LASTUPDATEDDATE, opt => opt.Ignore())
                .ForMember(t => t.PKS_KTEMPLATE_CATEGORY_PARAMETER, opt => opt.Ignore());

            CreateMap<PKS_KFRAGMENT_TYPE_PARAMETER, WidgetTypeParamDTO>()
                .ForMember(dest => dest.WidgetTypeId, opt => opt.MapFrom(src => src.KFRAGMENTTYPEID));
            
            CreateMap<PKS_KFRAGMENT_TYPE, WidgetTypeDTO>()
            .ForMember(dest => dest.WidgetTypeParams, opt => opt.MapFrom(src => src.PKS_KFRAGMENT_TYPE_PARAMETER));


            CreateMap<WidgetTypeParamDTO, PKS_KFRAGMENT_TYPE_PARAMETER>()
                .ForMember(dest => dest.PKS_KFRAGMENT_TYPE, opt => opt.Ignore())
                .ForMember(dest => dest.KFRAGMENTTYPEID, opt => opt.MapFrom(src => src.WidgetTypeId));
            CreateMap<WidgetTypeDTO, PKS_KFRAGMENT_TYPE>()
                //.ForMember(dest => dest.PKS_KFRAGMENT_TYPE_PARAMETER, opt => opt.Ignore())
                .ForMember(dest => dest.PKS_KFRAGMENT, opt => opt.Ignore())
                .ForMember(dest => dest.PKS_KFRAGMENT_TYPE_PARAMETER, opt => opt.MapFrom(src => src.WidgetTypeParams));

        }
    }
}