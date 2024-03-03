using HRIS_Software.Models.Database;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using MathNet.Numerics.LinearRegression;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRIS_Software.Models.Utils
{
    internal sealed class AnalyticsHelper
    {
        public static ISeries[] GetSalariesPerDateChartData(IEnumerable<Salary> salaries, out Axis[] XAxes)
        {
            XAxes = new Axis[] { new DateTimeAxis(TimeSpan.FromDays(30), date => date.ToString("MMMM")) };
            
            List<DateTimePoint> values = salaries
                .GroupBy(salary => new { salary.Date.Month })
                .Select(group => group.OrderByDescending(salary => salary.Wage).First())
                .OrderBy(salary => salary.Date)
                .Select(salary => new DateTimePoint(salary.Date, (double)salary.Wage))
                .ToList();

            return new ISeries[]
            {
                new ColumnSeries<DateTimePoint>
                {
                    Values = values,
                    EasingFunction = EasingFunctions.BounceInOut,
                }
            };
        }

        public static ISeries[] GetSalaryClusterizationChartData(IEnumerable<Salary> salaries)
        {
            IEnumerable<Salary>[] clusters = ClusterizationHelper.ClusterSalaries(salaries);

            double geometrySize = 12.5d;
            LvcPoint dataPadding = new LvcPoint(.3d, .3d);
            Coordinate mapping(Salary salary, int _) => new Coordinate(salary.Employee.Position.Id, (double)salary.Wage);

            return new ISeries[]
            {
                new ScatterSeries<Salary, RoundedRectangleGeometry>
                {
                    Name = "Низкие",
                    GeometrySize = geometrySize,
                    DataPadding = dataPadding,
                    Fill = new SolidColorPaint(SKColors.Blue),
                    Stroke = null,
                    Values = clusters[0],
                    Mapping = mapping
                },
                new ScatterSeries<Salary, RoundedRectangleGeometry>
                {
                    Name = "Средние",
                    GeometrySize = geometrySize,
                    DataPadding = dataPadding,
                    Fill = new SolidColorPaint(SKColors.Green),
                    Stroke = null,
                    Values = clusters[1],
                    Mapping = mapping
                },
                new ScatterSeries<Salary, RoundedRectangleGeometry>
                {
                    Name = "Высокие",
                    GeometrySize = geometrySize,
                    DataPadding = dataPadding,
                    Fill = new SolidColorPaint(SKColors.Red),
                    Stroke = null,
                    Values = clusters[2],
                    Mapping = mapping
                }
            };
        }

        [Obsolete("Работает некорректно")]
        public static ISeries[] ApplyLinearRegressionToSalaries(IEnumerable<Salary> salaries, out Axis[] XAxes)
        {
            HashSet<double> salariesHashSet = new HashSet<double>();
            foreach (Salary salary in salaries)
                salariesHashSet.Add(salary.Employee.Position.Id);

            double[] independends = salariesHashSet.ToArray();

            var averageSalaries = salaries
                .GroupBy(s => s.Employee.Position.Id)
                .Select(g => new
                {
                    Position = g.Key,
                    Value = g.Average(s => s.Wage)
                })
                .OrderBy(a => a.Position);

            double[] dependends = averageSalaries.Select(a => (double)a.Value).ToArray();

            (double intercept, double slope) = SimpleRegression.Fit(independends.OrderBy(x => x).ToArray(), dependends);

            double[] predictedY = new double[independends.Length];
            for (int i = 0; i < independends.Length; i++)
            {
                double independend = independends[i];

                independend *= slope;
                independend += intercept;

                predictedY[i] = independend;
            }

            //double correlation = Correlation.Pearson(independends, dependends);

            XAxes = new Axis[]
            {
                new Axis
                {
                    IsVisible = false,
                    Name = "Должность",
                    Labels = salaries
                                .OrderBy(s => s.Employee.Position.Id)
                                .Select(s => s.Employee.Position.PositionName)
                                .ToList()
                }
            };

            return new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = "Линейная регрессия",
                    Values = new[] { intercept, slope * independends.Max() + intercept }
                },
                new ScatterSeries<double>
                {
                    Name = "Прогнозируемая зарплата",
                    Values = predictedY
                }
            };
        }
    }
}
