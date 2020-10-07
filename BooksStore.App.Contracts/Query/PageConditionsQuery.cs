﻿namespace BooksStore.App.Contracts.Query
{
    public class PageConditionsQuery : Query
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}