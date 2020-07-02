namespace BooksStore.App.Contracts.Query
{
    /// <summary>
    /// The entity identifier query.
    /// </summary>
    public abstract class EntityIdQuery : Query
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }
    }
    /// <summary>
    /// The book identifier query.
    /// </summary>
    public sealed class BookIdQuery : EntityIdQuery { }
    /// <summary>
    /// The category identifier query.
    /// </summary>
    public sealed class CategoryIdQuery : EntityIdQuery { }
    /// <summary>
    /// The publisher identifier query.
    /// </summary>
    public sealed class PublisherIdQuery : EntityIdQuery { }
    /// <summary>
    /// The order identifier query.
    /// </summary>
    public sealed class OrderIdQuery : EntityIdQuery { }
}