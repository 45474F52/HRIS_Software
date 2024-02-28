using HRIS_Software.Core;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.ModalDialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Threading;

namespace HRIS_Software.ViewModels.PagesVMs
{
    internal sealed class EmployeesVM : BaseVM
    {
        public EmployeesVM(Entities db)
        {
            Title = "Сотрудники";

            ShowContactData = new RelayCommand<Employee>(employee =>
            {
                ShowContactInfoVM dialog = new ShowContactInfoVM(employee);
                ModalDialog = dialog;
            });

            Dispatcher.CurrentDispatcher.Invoke(async () =>
            {
                List<Employee> employees = await db.Employees.AsNoTracking().ToListAsync();
                Employees = new ObservableCollection<Employee>(employees);
            });
        }

        public RelayCommand<Employee> ShowContactData { get; }

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        private object _modalDialog;
        public object ModalDialog
        {
            get => _modalDialog;
            private set
            {
                _modalDialog = value;
                OnPropertyChanged();
            }
        }
    }
}
