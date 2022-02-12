using AutoMapper;
using Model.Data;
using Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<OrgModelPolygon, OrgModelPolygonData>()
               .ReverseMap();

            CreateMap<UserType, UserTypeData>()
               .ReverseMap();

            CreateMap<ActivityTemplate, ActivityTemplateDataInfo>()
                .ReverseMap();

            CreateMap<Threshold, ThresholdData>()
               .ReverseMap();

            //CreateMap<OrganizationObject, OrganizationObjectData>()
            //    //.ForMember(dest => dest.activities, opt => opt.MapFrom(src => src.Activity.ToList()))TODO
            //    .ForMember(dest => dest.descriptions, opt => opt.MapFrom(src => new List<DescriptionsData>()))
            //    .ForMember(dest => dest.models, opt => opt.MapFrom(src => new List<ModelData>()))
            //    .ForMember(dest => dest.children, opt => opt.MapFrom(src => new List<OrganizationObjectData>()))
            //    .ForMember(dest => dest.guid, opt => opt.MapFrom(src => src.OrgObjGuid))
            //    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(dest => dest.remark, opt => opt.MapFrom(src => src.Remark))
            //    .ForMember(dest => dest.org_type, opt => opt.MapFrom(src => src.OrgObjType))
            //    .ForMember(dest => dest.order, opt => opt.MapFrom(src => src.Order))
            //    .ReverseMap();

            CreateMap<Description, DescriptionsData>()
                .ForMember(dest => dest.creator, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.creator_user_guid, opt => opt.MapFrom(src => src.CreatorUserGuid))
                .ForMember(dest => dest.description_guid, opt => opt.MapFrom(src => src.DescriptionGuid))
                .ForMember(dest => dest.modify, opt => opt.MapFrom(src => src.Modify))
                .ForMember(dest => dest.modify_user_guid, opt => opt.MapFrom(src => src.ModifyUserGuid))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.remark, opt => opt.MapFrom(src => src.Remark))
                .ReverseMap();

            CreateMap<FormTemplate, FormTemplateDataInfo>()
                .ForMember(dest => dest.form_template_guid, opt => opt.MapFrom(src => src.FormTemplateGuid))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.modified_date, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.create_date, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.creator_user_guid, opt => opt.MapFrom(src => src.CreatorUserGuid))
                .ReverseMap();

            CreateMap<Form, FormDetails>()
                .ForMember(dest => dest.form_guid, opt => opt.MapFrom(src => src.FormGuid))
                .ForMember(dest => dest.form_template_guid, opt => opt.MapFrom(src => src.FormTemplateGuid))
                .ForMember(dest => dest.activity_guid, opt => opt.MapFrom(src => src.ActivityGuid))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.FormTemplateGu.Name))
                .ForMember(dest => dest.approve_user_guid, opt => opt.MapFrom(src => src.ApproveUserGuid))
                .ForMember(dest => dest.approve_user_name, opt => opt.MapFrom(src => src.ApproveUserGu.UserName))
                .ForMember(dest => dest.approve_date, opt => opt.MapFrom(src => src.ApproveDate))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                //.ForMember(dest => dest.org_obj_guid, opt => opt.MapFrom(src => src.OrgObjGuid))
                //.ForMember(dest => dest.org_obj_name, opt => opt.MapFrom(src => src.OrgObjGu.Name))
                .ForMember(dest => dest.entity_guid, opt => opt.MapFrom(src => src.EntityGuid))
                .ForMember(dest => dest.entity_type, opt => opt.MapFrom(src => src.EntityType))
                //.ForMember(dest => dest.unit_name, opt => opt.MapFrom(src => src.))
                .ReverseMap();

            CreateMap<ModelComponent, ModelData>()
                .ForMember(dest => dest.model_component_guid, opt => opt.MapFrom(src => src.ModelComponentGuid))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.professional_instruction, opt => opt.MapFrom(src => src.ProfessionalInstruction))
                .ForMember(dest => dest.model_description_text, opt => opt.MapFrom(src => src.ModelDescriptionText))
                .ForMember(dest => dest.source, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.model_component_order, opt => opt.MapFrom(src => src.ModelComponentOrder))
                .ForMember(dest => dest.weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.create_date, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.modified_date, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.modified_user_guid, opt => opt.MapFrom(src => src.ModifiedUserGuid))
                .ForMember(dest => dest.metric_required, opt => opt.MapFrom(src => src.MetricRequired))
                .ForMember(dest => dest.metric_measuring_unit, opt => opt.MapFrom(src => src.MetricMeasuringUnit))
                .ForMember(dest => dest.metric_rollup_method, opt => opt.MapFrom(src => src.MetricRollupMethod))
                .ForMember(dest => dest.metric_calender_rollup, opt => opt.MapFrom(src => src.MetricCalenderRollup))
                .ForMember(dest => dest.metric_formula, opt => opt.MapFrom(src => src.MetricFormula))
                .ForMember(dest => dest.metric_is_visible, opt => opt.MapFrom(src => src.MetricIsVisible))
                .ForMember(dest => dest.metric_not_display_if_irrelevant, opt => opt.MapFrom(src => src.MetricNotDisplayIfIrrelevant))
                .ForMember(dest => dest.metric_expired_period, opt => opt.MapFrom(src => src.MetricExpiredPeriod))
                .ForMember(dest => dest.metric_expired_period_secondary, opt => opt.MapFrom(src => src.MetricExpiredPeriodSecondary))
                .ForMember(dest => dest.metric_comment_obligation_level, opt => opt.MapFrom(src => src.MetricCommentObligationLevel))
                .ForMember(dest => dest.metric_gradual_decrease_precent, opt => opt.MapFrom(src => src.MetricGradualDecreasePrecent))
                .ForMember(dest => dest.metric_gradual_decrease_period, opt => opt.MapFrom(src => src.MetricGradualDecreasePeriod))
                .ForMember(dest => dest.metric_minimum_feeds, opt => opt.MapFrom(src => src.MetricMinimumFeeds))
                .ForMember(dest => dest.show_origion_value, opt => opt.MapFrom(src => src.ShowOrigionValue))
                .ForMember(dest => dest.calcAsSum, opt => opt.MapFrom(src => src.CalcAsSum))
                .ForMember(dest => dest.groupChildren, opt => opt.MapFrom(src => src.GroupChildren))
                .ForMember(dest => dest.TemplateType, opt => opt.MapFrom(src => src.TemplateType.HasValue ? src.TemplateType : 0))
                .ForMember(dest => dest.metric_source, opt => opt.MapFrom(src => src.Source == (int)Data.ModelComponentSource.metric ? src.MetricSource ?? 1 : default(int?)))
                .ReverseMap();

            CreateMap<ActivityFile, ActivityFileData>()
               .ForMember(dest => dest.ActivityFileGuid, opt => opt.MapFrom(src => src.ActivityFileGuid))
               .ForMember(dest => dest.ActivityGuid, opt => opt.MapFrom(src => src.ActivityGuid))
               .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
               .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
               .ReverseMap();

            CreateMap<ConvertionTable, ConvertionTableData>()
               .ForMember(dest => dest.model_component_guid, opt => opt.MapFrom(src => src.ModelComponentGuid))
               .ForMember(dest => dest.level_id, opt => opt.MapFrom(src => src.LevelId))
               .ForMember(dest => dest.start_range, opt => opt.MapFrom(src => src.StartRange))
               .ForMember(dest => dest.end_range, opt => opt.MapFrom(src => src.EndRange))
               .ForMember(dest => dest.conversion_table_modified_date, opt => opt.MapFrom(src => src.ConversionTableModifiedDate))
               .ForMember(dest => dest.conversion_table_status, opt => opt.MapFrom(src => src.ConversionTableStatus))
               .ForMember(dest => dest.conversion_table_create_date, opt => opt.MapFrom(src => src.ConversionTableCreateDate))
               .ForMember(dest => dest.start_range_score_displayed, opt => opt.MapFrom(src => src.StartRangeScoreDisplayed))
               .ForMember(dest => dest.end_range_score_displayed, opt => opt.MapFrom(src => src.EndRangeScoreDisplayed))
               .ForMember(dest => dest.conversion_table_score_order, opt => opt.MapFrom(src => src.ConversionTableScoreOrder))
               .ForMember(dest => dest.conversion_table_final_score, opt => opt.MapFrom(src => src.ConversionTableFinalScore))
              .ReverseMap();

            CreateMap<OrganizationUnion, OrganizationUnionData>()
                .ForMember(dest => dest.OrganizationUnionId, opt => opt.MapFrom(src => src.OrganizationUnionId))
               .ForMember(dest => dest.OrganizationUnionGuid, opt => opt.MapFrom(src => src.OrganizationUnionGuid))
               .ForMember(dest => dest.OrgObjGuid, opt => opt.MapFrom(src => src.OrgObjGuid))
               .ForMember(dest => dest.ParentOrgObjGuid, opt => opt.MapFrom(src => src.ParentOrgObjGuid))
               .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
              .ReverseMap();

            //CreateMap<OrgObjTree, OrganizationObjectData>()
            //  .ForMember(dest => dest.guid, opt => opt.MapFrom(src => src.guid))
            //  .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
            //  .ForMember(dest => dest.remark, opt => opt.MapFrom(src => src.remark))
            //  .ForMember(dest => dest.parent_guid, opt => opt.MapFrom(src => src.parent_guid))
            //  .ForMember(dest => dest.org_type, opt => opt.MapFrom(src => src.org_type))
            //  .ForMember(dest => dest.permission_units, opt => opt.MapFrom(src => src.permission_units))
            //  .ForMember(dest => dest.children, opt => opt.MapFrom(src => src.children))
            // .ReverseMap();


            CreateMap<FormGroupData, FormItemData>()
              .ForMember(dest => dest.model_component_guid, opt => opt.MapFrom(src => src.model_component_guid))
              .ForMember(dest => dest.form_element_guid, opt => opt.MapFrom(src => src.form_element_guid))
              .ForMember(dest => dest.comment, opt => opt.MapFrom(src => src.comment))
              .ForMember(dest => dest.metric_form_irrelevant, opt => opt.MapFrom(src => src.metric_form_irrelevant))
              .ForMember(dest => dest.score, opt => opt.MapFrom(src => src.score))
              .ForMember(dest => dest.metric_measuring_unit, opt => opt.MapFrom(src => src.metric_measuring_unit))
             .ReverseMap();
        }
    }
}