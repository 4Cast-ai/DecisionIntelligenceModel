using Infrastructure.Interfaces;

namespace Infrastructure.Models
{
    public class DBPrefix : IDBPrefix
    {
        public string DynamicDBPrefix { get; set; }
        public string ManagerDBPrefix { get; set; }
    }
}
