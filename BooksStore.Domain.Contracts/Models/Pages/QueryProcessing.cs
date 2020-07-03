using System.Collections.Generic;
using System.Text;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class QueryProcessing<T>
    {
        private readonly PageOptions _options;

        public QueryProcessing(PageOptions options)
        {
            _options = options;
        }

        public string GetQueryConditions(string alias = null)
        {
            var conditions = GenerateQueryConditions();
            var orderProperties = GenerateQueryOrderProperties(alias);
            var whereCondition = conditions.Count == 0 ? string.Empty : "WHERE " + string.Join(" AND ", conditions);
            var orderCondition =
                orderProperties.Length == 0 ? string.Empty : "ORDER BY " + orderProperties;

            return $"{whereCondition} {orderCondition}" +
                   $" OFFSET {(_options.CurrentPage - 1) * _options.PageSize} ROWS FETCH NEXT {_options.PageSize} ROWS ONLY";
        }

        private List<string> GenerateQueryConditions()
        {
            var conditions = new List<string>();
            if (_options.SearchPropertyNames?.Length == 1 && !string.IsNullOrEmpty(_options.SearchTerm))
            {
                conditions.Add(Search(_options.SearchPropertyNames, _options.SearchTerm));
            }

            if (!string.IsNullOrEmpty(_options.FilterPropertyName) && _options.FilterPropertyValue != 0)
            {
                conditions.Add(Filter(_options.FilterPropertyName, _options.FilterPropertyValue));
            }

            return conditions;
        }

        private string GenerateQueryOrderProperties(string alias)
        {
            if (!string.IsNullOrEmpty(_options.SortPropertyName))
            {
                var prefix = !string.IsNullOrEmpty(alias) ? alias + "." : string.Empty;
                var order = _options.DescendingOrder ? "DESC" : "ASC";
                return $"{prefix}{_options.SortPropertyName} {order}";
            }

            return string.Empty;
        }

        private string Search(string[] propertyNames, string searchTerm)
        {
            var condition = new StringBuilder();
            foreach (var propertyName in propertyNames)
            {
                if (condition.Length > 0)
                {
                    condition.Append("OR");
                }

                if (string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }

                condition.Append($"{propertyName} LIKE '%{searchTerm}%'");
            }

            return condition.ToString();
        }

        private string Filter(string propertyName, long value)
        {
            return $"{propertyName} = {value}";
        }
    }
}
