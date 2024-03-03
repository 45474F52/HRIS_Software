using HRIS_Software.Core;
using HRIS_Software.Models;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.ModalDialogs;
using HRIS_Software.Models.Utils;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
            }, canExecute: () => _salaries.Any());

            SalariesStatisticsChart = new ObservableChart(new CartesianChart());
            SalariesClustersChart = new ObservableChart(new CartesianChart());
            SalariesRegressionChart = new ObservableChart(new CartesianChart());

            LoadSalaries().ContinueWith(task => LoadCharts());
        }

        private readonly Random random = new Random();
        private IEnumerable<Salary> GenerateSalaries()
        {
            for (int i = 0; i < 100; i++)
            {
                decimal wage = random.Next(30_000, 100_000);
                DateTime date = new DateTime(2024, random.Next(1, 13), 1);
                yield return new Salary
                {
                    Date = date, Wage = wage,
                    Employee = new Employee
                    {
                        Surname = GetRandomString(5, 10),
                        FirstName = GetRandomString(10, 20),
                        Patronymic = GetRandomString(10, 15),
                        Department = new Department { DepartmentName = GetRandomString(10, 20) },
                        Position = _db.Positions.Find(random.Next(1, 32)),
                        ContactData = new ContactData
                        {
                            Address = GetRandomString(15, 30),
                            Email = GetRandomString(25, 35),
                            Number = "8 (800) 555-35-35"
                        }
                    }
                };
            }
        }

        private string GetRandomString(int min, int max)
        {
            int count = random.Next(min, max);
            StringBuilder sb = new StringBuilder(count);
            for (int i = 0; i < count; i++)
                sb.Append((char)random.Next(32, 64));
            return sb.ToString();
        }

        private Task LoadSalaries()
        {
            return Application.Current.Dispatcher.Invoke(async () =>
            {
                _salaries.AddRange(await _db.Salaries.AsNoTracking().ToListAsync());
                _salaries.AddRange(GenerateSalaries()); // TEST
                DropFilters();
            });
        }

        private Task LoadCharts()
        {
            return Application.Current.Dispatcher.Invoke(async () =>
            {
                SalariesStatisticsChart.Series = AnalyticsHelper.GetSalariesPerDateChartData(_salaries, out var xAxes);
                SalariesStatisticsChart.XAxes = xAxes;

                List<String> positions = await _db.Positions.OrderBy(p => p.Id).AsNoTracking().Select(p => p.PositionName).ToListAsync();

                SalariesClustersChart.Series = AnalyticsHelper.GetSalaryClusterizationChartData(_salaries);
                SalariesClustersChart.XAxes = new Axis[]
                {
                    new Axis
                    {
                        IsVisible = false,
                        ForceStepToMin = true,
                        MinStep = 1,
                        Labels = positions
                    }
                };

                SalariesRegressionChart.Series = AnalyticsHelper.ApplyLinearRegressionToSalaries(_salaries, out var xAxes2);
                SalariesRegressionChart.XAxes = xAxes2;
            });
        }

        public RelayCommand UpdateCommand { get; }
        public RelayCommand FilterCommand { get; }

        public decimal AverageSalary => FilteredSalaries?.Average(s => s.Wage) ?? default;

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

        public ObservableChart SalariesStatisticsChart { get; }
        public ObservableChart SalariesClustersChart { get; }
        public ObservableChart SalariesRegressionChart { get; }

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
