using AutoMapper;
using Model.Data;
using Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Dal
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Model.Entities.ConvertionTable, ConvertionTableData>()
            //    .ReverseMap();


            CreateMap<Model.Entities.OutSourceScore, OutSourceScoreData>()
              .ReverseMap();

            

            CreateMap<ActivityTemplate, ActivityTemplateDataInfo>()
                .ReverseMap();

            //CreateMap<OrganizationObject, OrganizationObjectData>()
            //    //.ForMember(dest => dest.activities, opt => opt.MapFrom(src => src.Activity.ToList()))TODO
            //    .ForMember(dest => dest.descriptions, opt => opt.MapFrom(src => new List<DescriptionsData>()))
            //    .ForMember(dest => dest.models, opt => opt.MapFrom(src => new List<ModelData>()))
            //    .ForMember(dest => dest.children, opt => opt.MapFrom(src => new List<OrganizationObjectData>()))
            //    //parent_guid = parent_guid,
            //    .ForMember(dest => dest.guid, opt => opt.MapFrom(src => src.OrgObjGuid))
            //    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(dest => dest.remark, opt => opt.MapFrom(src => src.Remark))
            //    .ForMember(dest => dest.org_type, opt => opt.MapFrom(src => src.OrgObjType))
            //    .ForMember(dest => dest.order, opt => opt.MapFrom(src => src.Order))
            //    .ReverseMap();
        }
    }
}