using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public abstract class EntitiesViewModel<TEntity>
    {
        public List<TEntity> Entities { get; set; }
        public Pagination Pagination { get; set; }
        public AdminToolbarViewModel ToolbarViewModel { get; set; }
        public AdminFilter AdminFilter { get; set; }
        public List<SortingProperty> TableHeaders { get; set; }
        public SortingProperty SortingProperty { get; set; }
    }
}