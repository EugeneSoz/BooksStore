using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class QueryProcessing<T>
    {
        private readonly QueryOptions _options;

        public QueryProcessing(QueryOptions options)
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
                conditions.Add(Filter(_options.SortPropertyName, _options.FilterPropertyValue));
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

        public IQueryable<T> ProcessQuery(IQueryable<T> query)
        {
            if (_options.SearchPropertyNames != null && 
                _options.SearchPropertyNames.Length == 1 &&
                !string.IsNullOrEmpty(_options.SearchPropertyNames[0])
                && !string.IsNullOrEmpty(_options.SearchTerm))
            {
                query = Search(query, _options.SearchPropertyNames[0], _options.SearchTerm);
            }

            if (_options.SearchPropertyNames != null &&
                _options.SearchPropertyNames.Length == 2 &&
                !Array.Exists(_options.SearchPropertyNames, v => string.IsNullOrWhiteSpace(v)) &&
                !string.IsNullOrEmpty(_options.SearchTerm))
            {
                query = SearchByTwoProperties(query, _options.SearchPropertyNames, _options.SearchTerm);
            }

            if (!string.IsNullOrEmpty(_options.FilterPropertyName)
                && _options.FilterPropertyValue != 0)
            {
                query = Filter(query, _options.FilterPropertyName, _options.FilterPropertyValue);
            }

            if (!string.IsNullOrEmpty(_options.SortPropertyName))
            {
                query = Order(query, _options.SortPropertyName, _options.DescendingOrder);
            }

            return query;
        }

        private IQueryable<T> Search(IQueryable<T> query, string propertyName, string searchTerm)
        {
            ParameterExpression pe = Expression.Parameter(typeof(T), "x");
            Expression source = pe;
            foreach (string member in propertyName.Split('.'))
            {
                source = Expression.Property(source, member);
            }
            ConstantExpression constant = Expression.Constant(searchTerm, typeof(string));

            MethodCallExpression body = Expression.Call(source, "Contains", Type.EmptyTypes, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, pe);

            return query.Where(lambda);
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

        private IQueryable<T> SearchByTwoProperties(IQueryable<T> query, string[] propertyName, string searchTerm)
        {
            ParameterExpression pe = Expression.Parameter(typeof(T), "x");
            ConstantExpression constant = Expression.Constant(searchTerm, typeof(string));

            MemberExpression firstProp = Expression.Property(pe, propertyName[0]);
            MemberExpression secondProp = Expression.Property(pe, propertyName[1]);
            MethodCallExpression firstComparison = Expression.Call(firstProp, "Contains", 
                Type.EmptyTypes, constant);
            MethodCallExpression secondComparison = Expression.Call(secondProp, "Contains", 
                Type.EmptyTypes, constant);

            Expression predicateBody = Expression.OrElse(firstComparison, secondComparison);

            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(predicateBody, pe);

            return query.Where(lambda);
        }

        public string Filter(string propertyName, long value)
        {
            return $"{propertyName} = {value}";
        }

        private IQueryable<T> Filter(IQueryable<T> query, string propertyName, long value)
        {
            ParameterExpression pe = Expression.Parameter(typeof(T), "x");
            Expression source = pe;
            foreach (string member in propertyName.Split('.'))
            {
                source = Expression.Property(source, member);
            }
            ConstantExpression constant = Expression.Constant(value, typeof(long));
            BinaryExpression equality = Expression.Equal(source, constant);

            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(equality, pe);

            return query.Where(lambda);
        }

        private IQueryable<T> Order(IQueryable<T> query, string propertyName, bool desc)
        {
            ParameterExpression pe = Expression.Parameter(typeof(T), "x");
            Expression source = pe;
            foreach (string member in propertyName.Split('.'))
            {
                source = Expression.Property(source, member);
            }

            LambdaExpression lambda = Expression.Lambda(typeof(Func<,>).MakeGenericType(typeof(T), source.Type),
                source, pe);

            return typeof(Queryable).GetMethods().Single(
                method => method.Name == (desc ? "OrderByDescending" : "OrderBy")
                && method.IsGenericMethodDefinition
                && method.GetGenericArguments().Length == 2
                && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), source.Type)
                .Invoke(null, new object[] { query, lambda }) as IQueryable<T>;
        }
    }
}
