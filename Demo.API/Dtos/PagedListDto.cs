using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Dtos
{
    public class PagedListDto<T> : IPagedList<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int IndexFrom { get; set; }
        public IList<T> Items { get; set; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
    }
}