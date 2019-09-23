using System.Collections.Generic;

namespace Demo.GraphQLService.Dtos
{
    public class PagedListDto<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int IndexFrom { get; set; }
        public List<T> Items { get; set; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
    }
}