using System.Text;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Services;

namespace BooksStore.Domain.Services
{
    public class QueryProcessingService : IQueryProcessingService
    {
        public (string conditions, bool isSearchOrFilterUsed) GenerateSqlQueryConditions(QueryConditions queryConditions)
        {
            var isSearchOrFilterUsed = false;
            var result = new StringBuilder();
            var searchConditions = GenerateSearchConditions(queryConditions.SearchConditions);
            var filterConditions = GenerateFilterConditions(queryConditions.FilterConditions);
            var orderConditions = GenerateOrderConditions(queryConditions.OrderConditions);
            var rowsFetchConditions =
                GenerateRowsFetchConditions(queryConditions.CurrentPage, queryConditions.PageSize);

            if (!string.IsNullOrEmpty(searchConditions) || !string.IsNullOrEmpty(filterConditions))
            {
                result.Append(" WHERE");
            }

            if (!string.IsNullOrEmpty(searchConditions))
            {
                isSearchOrFilterUsed = true;
                result.Append(" ");
                result.AppendLine(searchConditions);
            }

            if (!string.IsNullOrEmpty(filterConditions))
            {
                isSearchOrFilterUsed = true;
                result.Append(" ");
                result.AppendLine(filterConditions);
            }

            if (!string.IsNullOrEmpty(orderConditions))
            {
                result.Append(" ");
                result.AppendLine(orderConditions);
            }

            if (!string.IsNullOrEmpty(rowsFetchConditions))
            {
                result.Append(" ");
                result.AppendLine(rowsFetchConditions);
            }

            return (result.ToString(), isSearchOrFilterUsed);
        }

        private string GenerateSearchConditions(Condition[] conditions)
        {
            if (conditions == null)
            {
                return string.Empty;
            }

            var result = new StringBuilder();

            for (int i = 0; i < conditions.Length; i++)
            {
                if (i > 0)
                {
                    result.Append(" OR ");
                }

                result.Append($"{conditions[i].PropertyName} LIKE '%{conditions[i].PropertyValue}%'");
            }

            return result.ToString();
        }

        private string GenerateFilterConditions(Condition[] conditions)
        {
            if (conditions == null)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            var rangeLength = conditions.Length;

            for (int i = 0; i < rangeLength; i++)
            {
                if (i > 0 && rangeLength == 2)
                {
                    result.Append(" AND ");
                }

                if (i == 0)
                {
                    result.Append($"{conditions[i].PropertyName} >= '{conditions[i].PropertyValue}'");
                }
                else if (i == 1)
                {
                    result.Append($"{conditions[i].PropertyName} <= '{conditions[i].PropertyValue}'");
                }
            }

            return result.ToString();
        }

        private string GenerateOrderConditions(Condition[] conditions)
        {
            if (conditions == null)
            {
                return string.Empty;
            }

            var result = new StringBuilder("ORDER BY ");

            for (int i = 0; i < conditions.Length; i++)
            {
                if (i > 0)
                {
                    result.Append(", ");
                }

                result.Append($"{conditions[i].PropertyName} {conditions[i].PropertyValue}");
            }

            return result.ToString();
        }

        private string GenerateRowsFetchConditions(int currentPage, int pageSize)
        {
            if (pageSize == 0)
            {
                return string.Empty;
            }

            var result = $"OFFSET {(currentPage - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            return result;
        }
    }
}