using HRIS_Software.Core;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.Utils;
using HRIS_Software.ViewModels.PagesVMs;
using System.Collections.Generic;

namespace HRIS_Software.ViewModels.WindowsVMs
{
    public sealed class MainVM : BaseVM
    {
        private readonly Stack<BaseVM> _views = new Stack<BaseVM>();

        public MainVM(string login, Entities db)
        {
            Title = "Human Resources Information System";

            Login = login;
            OnPropertyChanged(nameof(Login));

            CurrentViewService currentViewService = new CurrentViewService(value => SetView(value));

            SetView(new EmployessStartVM(currentViewService, db));

            BackCommand = new RelayCommand(_ => GoBack(), _ => CanGoBack);
        }

        public RelayCommand BackCommand { get; }

        public string Login { get; }
        public bool CanGoBack => _views.Count > 1;

        private BaseVM _currentView;
        public BaseVM CurrentView
        {
            get => _currentView;
            private set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    Title = string.Format("{0}:    {1}", Login, _currentView.Title);
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanGoBack));
                }
            }
        }

        private void SetView(BaseVM view)
        {
            if (CurrentView != view)
            {
                _views.Push(view);
                CurrentView = view;
            }
        }

        private void GoBack()
        {
            if (_views.Count > 0)
            {
                _ = _views.Pop();
                CurrentView = _views.Peek();
            }
        }
    }
}
