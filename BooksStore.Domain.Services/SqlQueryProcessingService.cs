using System.Collections.Generic;
using System.Text;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Services;

namespace BooksStore.Domain.Services
{
    public class SqlQueryProcessingService : ISqlQueryProcessingService
    {
        public SqlQueryConditions GenerateSqlQueryConditions(QueryConditions queryConditions)
        {
            var whereConditions = new StringBuilder();
            var searchConditions = GenerateSearchConditions(queryConditions.SearchConditions);
            var filterConditions = GenerateFilterConditions(queryConditions.FilterConditions);
            var orderConditions = GenerateOrderConditions(queryConditions.OrderConditions);
            var rowsFetchConditions =
                GenerateRowsFetchConditions(queryConditions.CurrentPage, queryConditions.PageSize);

            var result = new SqlQueryConditions();
            if (!string.IsNullOrEmpty(searchConditions) || !string.IsNullOrEmpty(filterConditions))
            {
                whereConditions.Append(" WHERE");
            }

            if (!string.IsNullOrEmpty(searchConditions))
            {
                whereConditions.Append(" ");
                whereConditions.AppendLine(searchConditions);
            }

            if (!string.IsNullOrEmpty(filterConditions))
            {
                var symbols = !string.IsNullOrEmpty(searchConditions) ? " AND " : " ";
                whereConditions.Append(symbols);
                whereConditions.AppendLine(filterConditions);
            }

            result.WhereConditions = whereConditions.ToString();
            result.OrderConditions = orderConditions;
            result.FetchConditions = rowsFetchConditions;

            return result;
        }

        protected virtual string GenerateSearchConditions(Condition[] conditions)
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

                result.Append($"{conditions[i].Alias}{conditions[i].PropertyName} LIKE '%{conditions[i].PropertyValue}%'");
            }

            return result.ToString();
        }

        protected virtual string GenerateFilterConditions(Condition[] conditions)
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
                    result.Append($"{conditions[i].Alias}{conditions[i].PropertyName} >= '{conditions[i].PropertyValue}'");
                }
                else if (i == 1)
                {
                    result.Append($"{conditions[i].Alias}{conditions[i].PropertyName} <= '{conditions[i].PropertyValue}'");
                }
            }

            return result.ToString();
        }

        protected virtual string GenerateOrderConditions(Condition[] conditions)
        {
            if (conditions == null)
            {
                return string.Empty;
            }

            var result = new StringBuilder("\n ORDER BY ");

            for (int i = 0; i < conditions.Length; i++)
            {
                if (i > 0)
                {
                    result.Append(", ");
                }

                result.Append($"{conditions[i].Alias}{conditions[i].PropertyName} {conditions[i].PropertyValue}");
            }

            return result.ToString();
        }

        private string GenerateRowsFetchConditions(int currentPage, int pageSize)
        {
            if (pageSize == 0)
            {
                return string.Empty;
            }

            var result = $"\n OFFSET {(currentPage - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            return result;
        }
    }
}