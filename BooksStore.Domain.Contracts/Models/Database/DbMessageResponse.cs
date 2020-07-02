namespace OnlineBooksStore.Domain.Contracts.Models.Database
{
    public class DbMessageResponse
    {
        public MessageType MessageType { get; set; }
        public string Message { get; set; }
    }
}