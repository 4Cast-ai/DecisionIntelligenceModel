using Infrastructure.Interfaces;
namespace Expert
{
    public class AppConfig : Infrastructure.Models.AppConfig, IAppConfig
    {
        public string AmanCsvFolderPath { get; set; }
        public string AmanUploadCsvTime { get; set; }

    }
}
