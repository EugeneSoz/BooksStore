namespace OnlineBooksStore.Domain.Contracts.Models.Database
{
    /// <summary>
    /// Describe a type of returned message from server
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The error message
        /// </summary>
        Error,
        /// <summary>
        /// The information message
        /// </summary>
        Info
    }
}