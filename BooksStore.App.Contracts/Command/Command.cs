namespace BooksStore.App.Contracts.Command
{
    /// <summary>
    /// Represents a base command class
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }
    }
}