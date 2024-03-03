using HRIS_Software.Core;
using LiveChartsCore;
using System.Collections.Generic;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.WPF;

namespace HRIS_Software.Models
{
    internal sealed class ObservableChart : ObservableObject
    {
        private readonly CartesianChart _chart;

        public ObservableChart(CartesianChart chart)
        {
            _chart = chart;
        }

        public IEnumerable<ISeries> Series
        {
            get => _chart.Series;
            set
            {
                _chart.Series = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<ICartesianAxis> XAxes
        {
            get => _chart.XAxes;
            set
            {
                _chart.XAxes = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<ICartesianAxis> YAxes
        {
            get => _chart.YAxes;
            set
            {
                _chart.YAxes = value;
                OnPropertyChanged();
            }
        }
    }
}
