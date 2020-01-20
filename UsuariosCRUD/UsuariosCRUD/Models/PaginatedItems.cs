using System.Collections.Generic;

namespace UsuariosCRUD.Models
{
    public class PaginatedItems<T>
    {
        public PaginatedItems(int page, int pageSize, long totalItems, IEnumerable<T> items)
        {
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = items;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}