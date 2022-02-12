using Infrastructure.Interfaces;
namespace FormsHandler
{
    public class AppConfig : Infrastructure.Models.AppConfig, IAppConfig
    {
        public string AmanCsvFolderPath { get; set; }
        public string AmanUploadCsvTime { get; set; }

    }
}
