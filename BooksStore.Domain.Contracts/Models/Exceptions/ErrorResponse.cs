using System.Collections.Generic;

namespace BooksStore.Domain.Contracts.Models.Exceptions
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; }
    }
}