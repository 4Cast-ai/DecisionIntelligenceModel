using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Enums
{
    public enum ApiServiceNames
    {
        [Display(Name = "Gateway", Description = "Gateway endpoint ", ResourceType = typeof(string))]
        Gateway,
        [Display(Name = "CalcApi", Description = "CalcApi endpoint ", ResourceType = typeof(string))]
        CalcApi,
        [Display(Name = "ExpertApi", Description = "ExpertApi endpoint ", ResourceType = typeof(string))]
        ExpertApi,
        [Display(Name = "EventsApi", Description = "EventsApi endpoint ", ResourceType = typeof(string))]
        EventsApi,
        [Display(Name = "ReportApi", Description = "ReportApi endpoint ", ResourceType = typeof(string))]
        ReportApi,
        [Display(Name = "InterfaceApi", Description = "InterfaceApi endpoint ", ResourceType = typeof(string))]
        InterfaceApi,
        [Display(Name = "DalApi", Description = "DBGateApi endpoint ", ResourceType = typeof(string))]
        DalApi
    }
}
