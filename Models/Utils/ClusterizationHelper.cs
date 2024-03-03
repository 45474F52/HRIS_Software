using System.Linq;
using System.Collections.Generic;
using HRIS_Software.Models.Database;

namespace HRIS_Software.Models.Utils
{
    internal sealed class ClusterizationHelper
    {
        public static IEnumerable<Salary>[] ClusterSalaries(IEnumerable<Salary> salaries)
        {
            List<Salary>[] clusters = new List<Salary>[3];

            List<Salary> sortedSalaries = salaries.OrderBy(salary => salary.Wage).ToList();

            var low = sortedSalaries.Take(sortedSalaries.Count / 3).ToList();
            var medium = sortedSalaries.Skip(sortedSalaries.Count / 3).Take(sortedSalaries.Count / 3).ToList();
            var height = sortedSalaries.Skip(2 * (sortedSalaries.Count / 3)).ToList();

            clusters[0] = low;
            clusters[1] = medium;
            clusters[2] = height;

            return clusters;
        }
    }
}
