using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Services;

namespace BooksStore.Domain.Services
{
    public class BookSqlQueryProcessingService : SqlQueryProcessingService, IBookSqlQueryProcessingService
    {
        protected override string GenerateSearchConditions(Condition[] conditions)
        {
            var processeedConditions = PreprocessConditions(conditions);

            return base.GenerateSearchConditions(processeedConditions);
        }

        protected override string GenerateFilterConditions(Condition[] conditions)
        {
            var processeedConditions = PreprocessConditions(conditions);

            return base.GenerateFilterConditions(processeedConditions);
        }

        protected override string GenerateOrderConditions(Condition[] conditions)
        {
            var processeedConditions = PreprocessConditions(conditions);

            return base.GenerateOrderConditions(processeedConditions);
        }

        private Condition[] PreprocessConditions(Condition[] conditions)
        {
            if (conditions != null)
            {
                var processedConditions = new List<Condition>();
                foreach (var condition in conditions)
                {
                    var propertyName = condition.PropertyName == nameof(BookResponse.CategoryName) 
                                       || condition.PropertyName == nameof(BookResponse.PublisherName)
                        ? "Name"
                        : condition.PropertyName;
                    string alias;

                    if (condition.PropertyName == nameof(BookResponse.CategoryName))
                    {
                        alias = "C.";
                    }
                    else if (condition.PropertyName == nameof(BookResponse.PublisherName))
                    {
                        alias = "P.";
                    }
                    else
                    {
                        alias = "B.";
                    }
                    var newCondition = new Condition(propertyName, condition.PropertyValue) {Alias = alias};
                    processedConditions.Add(newCondition);
                }

                return processedConditions.ToArray();
            }

            return null;
        }
    }
}