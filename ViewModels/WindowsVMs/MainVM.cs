using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using HRIS_Software.Core;
using HRIS_Software.Models.Utils;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.Services;
using HRIS_Software.ViewModels.PagesVMs;

namespace HRIS_Software.ViewModels.WindowsVMs
{
    internal sealed class MainVM : BaseVM
    {
        public delegate void LoginChanged(EntityConnectionStringBuilder connectionStringBuilder, Action<Entities> newDB);
        public event LoginChanged OnLoginChanged;

        private Entities _db;
        private readonly CurrentViewService _currentViewService;
        private readonly Stack<BaseVM> _navigationHistory = new Stack<BaseVM>();

        public MainVM(string login, Entities db)
        {
            _db = db;

            Title = "Human Resources Information System";

            Login = login;

            _currentViewService = new CurrentViewService(value => SetView(value));

            SetViewByLogin(login);

            BackCommand = new RelayCommand(() => GoBack(), () => CanGoBack);
            LogoutCommand = new RelayCommand(() =>
            {
                string newLogin = LoginService.StartLogin(out var connectionStringBuilder);

                if (!string.IsNullOrEmpty(newLogin))
                {
                    Login = newLogin;
                    OnLoginChanged?.Invoke(connectionStringBuilder, newDB => _db = newDB);
                    SetViewByLogin(newLogin);
                }
            });
        }

        public RelayCommand BackCommand { get; }
        public RelayCommand LogoutCommand { get; }

        public bool CanGoBack => _navigationHistory.Count > 1;

        private string _login;
        public string Login
        {
            get => _login;
            private set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

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

        private void SetViewByLogin(string login)
        {
            _navigationHistory.Clear();

            switch (login)
            {
                case "HRManager":
                    SetView(new EmployessStartVM(_currentViewService, _db));
                    break;
                case "Accountant":
                    SetView(new EmployessStartVM(_currentViewService, _db));
                    break;
                case "HRSpecialist":
                    SetView(new EmployessStartVM(_currentViewService, _db));
                    break;
                case "HRAnalyst":
                    SetView(new AnalystStartPageVM(_currentViewService, _db));
                    break;
                case "Administrator":
                    SetView(new EmployessStartVM(_currentViewService, _db));
                    break;
                default:
                    break;
            }
        }

        private void SetView(BaseVM view)
        {
            if (CurrentView != view)
            {
                _navigationHistory.Push(view);
                CurrentView = view;
            }
        }

        private void GoBack()
        {
            if (_navigationHistory.Count > 0)
            {
                _ = _navigationHistory.Pop();
                CurrentView = _navigationHistory.Peek();
            }
        }
    }
}
