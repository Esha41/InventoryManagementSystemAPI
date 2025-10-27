using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Comman.ExtensionMethods;
using Ettad.CrossCutting.Comman.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.CrossCutting.Comman.Models
{
    public class FilterData
    {
        public string sortField { get; set; }
        public int sortDirection { get; set; }
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public string Logic { get; set; }
        public IEnumerable<FilterData> Filters { get; set; }
    }

    // =======================================================================
    // DEFINITION FOR PagedListRequest
    // =======================================================================
    /// <summary>
    /// Represents the overall request for a paginated list from the client.
    /// </summary>
    public class PagedListRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public FilterData Filter { get; set; }
    }


    // =======================================================================
    // THE PAGINATED LIST IMPLEMENTATION
    // =======================================================================
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsyncForTableBinding(IQueryable<T> source, PagedListRequest request)
        {
            // This line calls the ToFilterView method from FilterProvider.cs
            if (request.Filter != null)
            {
                source = source.ToFilterView(request.Filter);
            }

            var count = await source.CountAsync();

            if (request.PageSize <= 0)
            {
                request.PageSize = count > 0 ? count : 10; // Handle PageSize=0 case
            }

            var items = await source
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, request.Page, request.PageSize);
        }
    }

}
