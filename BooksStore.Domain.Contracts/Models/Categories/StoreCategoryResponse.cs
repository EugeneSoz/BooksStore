using System.Collections.Generic;

namespace OnlineBooksStore.Domain.Contracts.Models.Categories
{
    public class StoreCategoryResponse : EntityBase
    {
        public string Name { get; set; }

        //если свойство не равно null, тогда категория является подкатегорией
        public long? ParentId { get; set; }
        //id элемента html разметки
        public string ControlId { get; set; }
        //родительская категория
        public bool IsParent { get; set; }
        //имеются ли дочернии категории
        public bool HasChildren { get; set; }
        //скрыты ли дочернии категории
        public bool IsCollapsed { get; set; } = true;
        public List<StoreCategoryResponse> Children { get; set; }
    }
}
