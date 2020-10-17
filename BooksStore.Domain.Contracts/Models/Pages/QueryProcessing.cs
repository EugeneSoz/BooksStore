using System.Collections.Generic;
using System.Text;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class QueryProcessing<T>
    {
        private readonly PageOptions1 _options1;

        public QueryProcessing(PageOptions1 options1)
        {
            _options1 = options1;
        }

        public string GetQueryConditions(string alias = null)
        {
            var conditions = GenerateQueryConditions();
            var orderProperties = GenerateQueryOrderProperties(alias);
            var whereCondition = conditions.Count == 0 ? string.Empty : "WHERE " + string.Join(" AND ", conditions);
            var orderCondition =
                orderProperties.Length == 0 ? string.Empty : "ORDER BY " + orderProperties;

            return $"{whereCondition} {orderCondition}" +
                   $" OFFSET {(_options1.CurrentPage - 1) * _options1.PageSize} ROWS FETCH NEXT {_options1.PageSize} ROWS ONLY";
        }

        private List<string> GenerateQueryConditions()
        {
            var conditions = new List<string>();
            if (_options1.SearchPropertyNames?.Length == 1 && !string.IsNullOrEmpty(_options1.SearchTerm))
            {
                conditions.Add(Search(_options1.SearchPropertyNames, _options1.SearchTerm));
            }

            if (!string.IsNullOrEmpty(_options1.FilterPropertyName) && _options1.FilterPropertyValue != 0)
            {
                conditions.Add(Filter(_options1.FilterPropertyName, _options1.FilterPropertyValue));
            }

            return conditions;
        }

        private string GenerateQueryOrderProperties(string alias)
        {
            if (!string.IsNullOrEmpty(_options1.SortPropertyName))
            {
                var prefix = !string.IsNullOrEmpty(alias) ? alias + "." : string.Empty;
                var order = _options1.DescendingOrder ? "DESC" : "ASC";
                return $"{prefix}{_options1.SortPropertyName} {order}";
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
