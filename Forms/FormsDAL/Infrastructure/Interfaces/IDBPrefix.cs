namespace Infrastructure.Interfaces
{
    public interface IDBPrefix
    {
        public string DynamicDBPrefix { get; set; }
        public string ManagerDBPrefix { get; set; }
    }

}
