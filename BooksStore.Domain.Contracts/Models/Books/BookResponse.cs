using System;
using OnlineBooksStore.Domain.Contracts.Models;

namespace BooksStore.Domain.Contracts.Models.Books
{
    public class BookResponse : EntityBase
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public int Year { get; set; }
        public string Language { get; set; }
        public int PageCount { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public string BookCover { get; set; }
        public long? CategoryId { get; set; }
        public long? PublisherId { get; set; }
        public string CategoryName { get; set; }
        public string PublisherName { get; set; }
        public DateTime Created { get; set; }
    }
}
