using HRIS_Software.Core;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.Utils;

namespace HRIS_Software.ViewModels.PagesVMs
{
    internal sealed class EmployessStartVM : BaseVM
    {
        public EmployessStartVM(CurrentViewService currentViewService, Entities db)
        {
            Title = "Панель управления сотрудниками";

            GoToEmployees = new RelayCommand(_ => currentViewService.SetView(new EmployeesVM(db)));
            GoToVacancies = new RelayCommand(_ => currentViewService.SetView(new VacanciesVM(db)));
        }

        public RelayCommand GoToEmployees { get; }
        public RelayCommand GoToVacancies { get; }
    }
}
