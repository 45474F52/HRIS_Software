using System.Linq;
using System.Collections.Generic;

namespace HRIS_Software.Models
{
    internal sealed class DataForFilters
    {
        public readonly IEnumerable<DynamicItem> filters;

        public DataForFilters(IEnumerable<DynamicItem> filters) => this.filters = filters;

        public T GetValue<T>(int index) => (T)filters.ElementAt(index).Value;
    }
}
