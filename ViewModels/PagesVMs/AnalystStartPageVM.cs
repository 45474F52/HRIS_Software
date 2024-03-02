using HRIS_Software.Core;
using HRIS_Software.Models.Utils;
using HRIS_Software.Models.Database;

namespace HRIS_Software.ViewModels.PagesVMs
{
    internal sealed class AnalystStartPageVM : BaseVM
    {
        public AnalystStartPageVM(CurrentViewService currentViewService, Entities db)
        {
            Title = "Кадровая аналитика";

            CoursesCommand = new RelayCommand(() => currentViewService.SetView(new CoursesVM(db)));
            CourseAnalysisCommand = new RelayCommand(() => { });
            SalaryAnalyticsCommand = new RelayCommand(() => currentViewService.SetView(new SalaryAnalyticsVM(db)));
            WorkingHoursAnalyticsCommand = new RelayCommand(() => { });
        }

        public RelayCommand CoursesCommand { get; }
        public RelayCommand CourseAnalysisCommand { get; }
        public RelayCommand SalaryAnalyticsCommand { get; }
        public RelayCommand WorkingHoursAnalyticsCommand { get; }
    }
}
