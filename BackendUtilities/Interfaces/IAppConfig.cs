using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface IAppConfig
    {
        string Domain { get; set; }
        Dictionary<string, string> Endpoints { get; set; }
        string Version { get; set; }
        string DefaultLocale { get; set; }
        string AmanGuid { get; set; }
        string DefaultCultureName { get; set; }
        string GroupName { get; set; }
        string ResponseTimeout { get; set; }
    }
}