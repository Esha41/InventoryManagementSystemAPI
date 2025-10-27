using Ettad.CrossCutting.Comman.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Ettad.CrossCutting.Comman.Providers
{
    public static class FilterProvider
    {
        public static IQueryable<T> ToFilterView<T>(this IQueryable<T> query, FilterData filter)
        {
            // Apply filtering logic if filters are present
            if ((filter.Filters != null && filter.Filters.Any()) || !string.IsNullOrEmpty(filter.Field))
            {
                query = Filter(query, filter);
            }

            // Apply sorting logic if a sort field is provided
            if (!string.IsNullOrEmpty(filter.sortField))
            {
                string sortDirection = filter.sortDirection == 1 ? "asc" : "desc";
                query = query.OrderBy($"{filter.sortField} {sortDirection}");
            }
            return query;
        }

        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, FilterData filter)
        {
            if (filter != null)
            {
                var filters = GetAllFilters(filter);
                if (filters.Any())
                {
                    var values = filters.Select(f => f.Value).ToArray();
                    string where = Transform(filter, filters);
                    queryable = queryable.Where(where, values);
                }
            }
            return queryable;
        }

        private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"doesnotcontain", "Contains"},
        };

        private static IList<FilterData> GetAllFilters(FilterData filter)
        {
            var filters = new List<FilterData>();
            GetFilters(filter, filters);
            return filters;
        }

        private static void GetFilters(FilterData filter, IList<FilterData> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var item in filter.Filters)
                {
                    GetFilters(item, filters);
                }
            }
            else if (!string.IsNullOrEmpty(filter.Field))
            {
                filters.Add(filter);
            }
        }

        private static string Transform(FilterData filter, IList<FilterData> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                return "(" + string.Join(" " + filter.Logic + " ",
                    filter.Filters.Select(f => Transform(f, filters)).ToArray()) + ")";
            }

            int index = filters.IndexOf(filter);
            var comparison = Operators[filter.Operator];

            if (filter.Operator == "doesnotcontain")
            {
                return $"(!{filter.Field}.ToString().{comparison}(@{index}))";
            }

            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                return $"{filter.Field}.{comparison}(@{index})";
            }

            return $"{filter.Field} {comparison} @{index}";
        }
    }
}
