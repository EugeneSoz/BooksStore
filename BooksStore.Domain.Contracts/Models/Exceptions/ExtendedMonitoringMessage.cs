namespace BooksStore.Domain.Contracts.Models.Exceptions
{
    public class ExtendedMonitoringMessage : MonitoringMessage
    {
        public string Identity { get; set; }
    }
}