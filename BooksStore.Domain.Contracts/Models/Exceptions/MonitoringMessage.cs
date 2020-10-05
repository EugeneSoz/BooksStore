namespace BooksStore.Domain.Contracts.Models.Exceptions
{
    public class MonitoringMessage
    {
        public string Route { get; set; }

        public string RequestUrl { get; set; }

        public long? ElapsedMilliseconds { get; set; }

        public object ActionParameters { get; set; }

        public ResultType ResultType { get; set; }

        public string RequestorIp { get; set; }

        public string Error { get; set; }
    }
}