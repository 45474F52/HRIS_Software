using HRIS_Software.Core;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.ModalDialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Threading;

namespace HRIS_Software.ViewModels.PagesVMs
{
    internal sealed class VacanciesVM : BaseVM
    {
        public VacanciesVM(Entities db)
        {
            Title = "Вакансии";

            ShowResponsesCommand = new RelayCommand<Vacancy>(
                vacancy => ModalDialog = new ShowResponsesVM(vacancy),
                vacancy => vacancy.Responses != null && vacancy.Responses.Any());

            Dispatcher.CurrentDispatcher.Invoke(async () =>
            {
                List<Vacancy> vacancies = await db.Vacancies.AsNoTracking().ToListAsync();
                Vacancies = new ObservableCollection<Vacancy>(vacancies);
            });
        }

        public RelayCommand<Vacancy> ShowResponsesCommand { get; }

        private ObservableCollection<Vacancy> _vacancies;
        public ObservableCollection<Vacancy> Vacancies
        {
            get => _vacancies;
            set
            {
                _vacancies = value;
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
