namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class SqlQueryConditions
    {
        public string WhereConditions { get; set; }
        public string OrderConditions { get; set; }
        public string FetchConditions { get; set; }

        public override string ToString()
        {
            return $"{WhereConditions}{OrderConditions}{FetchConditions}";
        }
    }
}