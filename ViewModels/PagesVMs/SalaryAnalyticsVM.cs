using HRIS_Software.Core;
using HRIS_Software.Models;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.ModalDialogs;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HRIS_Software.ViewModels.PagesVMs
{
    internal sealed class SalaryAnalyticsVM : BaseVM
    {
        private readonly Entities _db;
        private readonly List<Salary> _salaries = new List<Salary>();

        public SalaryAnalyticsVM(Entities db)
        {
            _db = db;

            Title = "Аналитика зарплат";

            UpdateCommand = new RelayCommand(async () => await LoadSalaries());

            FilterCommand = new RelayCommand(() =>
            {
                IEnumerable<DynamicItem> filterItems = new DynamicItem[]
                {
                    new DynamicItem("Номер", "Номер телефона сотрудника", typeof(string), ""),
                    new DynamicItem("Почта", "Электронная почта сотрудника", typeof(string), ""),
                    new DynamicItem("ФИО", "Полное имя сотрудника", typeof(string), ""),
                    new DynamicItem("Дата", "Дата получения зарплаты", typeof(string), ""),
                    new DynamicItem("Искать даты ДО",
                        "Получить только те зарплаты, где дата получения — ДО указанной", typeof(bool), false),
                    new DynamicItem("Искать даты ВКЛЮЧИТЕЛЬНО",
                        "Получить только те зарплаты, где дата получения включает указанную", typeof(bool), false),
                };

                DataForFilters filtersData = new DataForFilters(filterItems);

                FilterDialogVM dialog = new FilterDialogVM("Фильтрация списка зарплат", filtersData);
                dialog.OnApplyFilters += applied =>
                {
                    if (applied)
                    {
                        ApplyFilters(
                            number: filtersData.GetValue<string>(0),
                            email: filtersData.GetValue<string>(1),
                            name: filtersData.GetValue<string>(2),
                            date: filtersData.GetValue<string>(3),
                            datesBefore: filtersData.GetValue<bool>(4),
                            datesInclusive: filtersData.GetValue<bool>(5));
                    }
                    else
                    {
                        DropFilters();
                    }
                };

                ModalDialog = dialog;
            }, () => _salaries.Any());

            LoadSalaries();

            var salaries = GenerateSalaries()
                .GroupBy(s => new { s.Date.Month })
                .Select(g => g.OrderByDescending(s => s.Wage).First())
                .OrderBy(x => x.Date)
                .Select(x => new DateTimePoint(x.Date, (double)x.Wage))
                .ToList();

            XAxes = new Axis[]
            {
                new DateTimeAxis(TimeSpan.FromDays(30), date => date.ToString("dd MMMM"))
            };

            Series = new ISeries[]
            {
                new ColumnSeries<DateTimePoint>
                {
                    Values = salaries,
                    EasingFunction = EasingFunctions.BounceInOut,
                }
            };
        }
        private readonly Random random = new Random();
        private IEnumerable<Salary> GenerateSalaries()
        {
            for (int i = 0; i < 50; i++)
            {
                decimal wage = random.Next(30_000, 100_000);
                DateTime date = new DateTime(2024, random.Next(1, 13), 1);
                yield return new Salary { Date = date, Wage = wage };
            }
        }

        private Task LoadSalaries()
        {
            return Application.Current.Dispatcher.Invoke(async () =>
            {
                _salaries.AddRange(await _db.Salaries.AsNoTracking().ToListAsync());
                _salaries.Add(new Salary()
                {
                    Date = DateTime.Now,
                    Wage = 50_000M,
                    Bonus = 2000M,
                    Deduction = 50M,
                    Employee = new Employee()
                    {
                        Surname = "Abo",
                        FirstName = "Bus",
                        Patronymic = "Sergeevitch",
                        Department = new Department() { DepartmentName = "Microsoft" },
                        Position = new Position() { PositionName = "Director" },
                        ContactData = new ContactData()
                        {
                            Address = "Unknown",
                            Email = "danchin276@mail.ru",
                            Number = "89773162628"
                        }
                    }
                });
                DropFilters();
            });
        }

        public RelayCommand UpdateCommand { get; }
        public RelayCommand FilterCommand { get; }

        public decimal AverageSalary => FilteredSalaries.Average(s => s.Wage);

        private BaseVM _modalDialog;
        public BaseVM ModalDialog
        {
            get => _modalDialog;
            private set
            {
                _modalDialog = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Salary> _filteredSalaries;
        public ObservableCollection<Salary> FilteredSalaries
        {
            get => _filteredSalaries;
            private set
            {
                _filteredSalaries = value;
                OnPropertyChanged();
            }
        }

        private ISeries[] _series;
        public ISeries[] Series
        {
            get => _series;
            private set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        private Axis[] _xAxes;
        public Axis[] XAxes
        {
            get => _xAxes;
            private set
            {
                _xAxes = value;
                OnPropertyChanged();
            }
        }

        private void ApplyFilters(string number, string email, string name, string date, bool datesBefore, bool datesInclusive)
        {
            HashSet<Salary> salaries = new HashSet<Salary>();

            if (!string.IsNullOrWhiteSpace(number))
            {
                salaries.UnionWith(
                    _salaries.Where(s => s.Employee.ContactData.Number.Contains(number)));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                salaries.UnionWith(
                    _salaries.Where(s => s.Employee.ContactData.Email.ToLower().Contains(email.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                salaries.UnionWith(
                    _salaries.Where(s => s.Employee.FullName.ToLower().Contains(name.ToLower())));
            }

            if (DateTime.TryParse(date, out DateTime value))
            {
                if (datesBefore)
                {
                    salaries.UnionWith(_salaries.Where(s => datesInclusive ? s.Date >= value : s.Date > value));
                }
                else
                {
                    salaries.UnionWith(_salaries.Where(s => datesInclusive ? s.Date <= value : s.Date < value));
                }
            }

            FilteredSalaries.Clear();
            
            foreach (Salary salary in salaries)
            {
                FilteredSalaries.Add(salary);
            }

            OnPropertyChanged(nameof(FilteredSalaries));
        }

        private void DropFilters() => FilteredSalaries = new ObservableCollection<Salary>(_salaries);
    }
}
