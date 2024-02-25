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

            ShowResponsesCommand = new RelayCommand(arg =>
            {
                if (arg is Vacancy vacancy)
                {
                    ShowResponsesVM dialog = new ShowResponsesVM(vacancy);
                    ModalDialog = dialog;
                }
            }, arg =>
            {
                if (arg is Vacancy vacancy && vacancy.Responses != null)
                {
                    return vacancy.Responses.Any();
                }

                return false;
            });

            Dispatcher.CurrentDispatcher.Invoke(async () =>
            {
                List<Vacancy> vacancies = await db.Vacancies.AsNoTracking().ToListAsync();
                Vacancies = new ObservableCollection<Vacancy>(vacancies);
            });
        }

        public RelayCommand ShowResponsesCommand { get; }

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
