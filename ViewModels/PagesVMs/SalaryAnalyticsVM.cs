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

            UpdateCommand = new RelayCommand(async () =>
            {
                _salaries.Clear();
                await LoadData().ContinueWith(task => LoadCharts());
            });

            DropFiltersCommand = new RelayCommand(ReloadFilteredCollection);

            SalariesStatisticsChart = new ObservableChart(new CartesianChart());
            SalariesClustersChart = new ObservableChart(new CartesianChart());
            SalariesRegressionChart = new ObservableChart(new CartesianChart());

            LoadData().ContinueWith(task => LoadCharts());
        }

        private readonly Random random = new Random();
        private IEnumerable<Salary> GenerateSalaries()
        {
            for (int i = 0; i < 100; i++)
            {
                decimal wage = random.Next(35_000, 120_000);
                DateTime date = new DateTime(2024, random.Next(1, 13), random.Next(1, 31));
                yield return new Salary
                {
                    Date = date, Wage = wage,
                    Employee = new Employee
                    {
                        Surname = GetRandomString(5, 10),
                        FirstName = GetRandomString(10, 20),
                        Patronymic = GetRandomString(10, 15),
                        Department = _db.Departments.Find(random.Next(1, 10)),
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
                sb.Append((char)random.Next(97, 122));
            return sb.ToString();
        }

        private Task LoadData()
        {
            return Application.Current.Dispatcher.Invoke(async () =>
            {
                _salaries.AddRange(await _db.Salaries.AsNoTracking().ToListAsync());
                _salaries.AddRange(GenerateSalaries()); // TEST

                List<Department> departments = await _db.Departments.OrderBy(d => d.Id).AsNoTracking().ToListAsync();
                departments.Insert(0, new Department { Id = 0, DepartmentName = string.Empty });
                Departments = departments;
                OnPropertyChanged(nameof(Departments));
                List<Position> positions = await _db.Positions.OrderBy(p => p.Id).AsNoTracking().ToListAsync();
                positions.Insert(0, new Position { Id = 0, PositionName = string.Empty });
                Positions = positions;
                OnPropertyChanged(nameof(Positions));

                ReloadFilteredCollection();
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
        public RelayCommand DropFiltersCommand { get; }

        public IEnumerable<Department> Departments { get; private set; }

        private int _selectedDepartment;
        public int SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                if (value != _selectedDepartment)
                {
                    _selectedDepartment = value;
                    OnPropertyChanged();
                    Filter();
                }
            }
        }

        public IEnumerable<Position> Positions { get; private set; }

        private int _selectedPosition;
        public int SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                if (value != _selectedPosition)
                {
                    _selectedPosition = value;
                    OnPropertyChanged();
                    Filter();
                }
            }
        }

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
                OnPropertyChanged(nameof(AverageSalary));
            }
        }

        public ObservableChart SalariesStatisticsChart { get; }
        public ObservableChart SalariesClustersChart { get; }
        public ObservableChart SalariesRegressionChart { get; }

        private void Filter()
        {
            if (SelectedDepartment > 0 || SelectedPosition > 0)
            {
                HashSet<Salary> salaries = new HashSet<Salary>();

                if (SelectedDepartment > 0)
                    salaries.UnionWith(_salaries.Where(s => s.Employee.Position.Id == SelectedPosition));
                if (SelectedPosition > 0) // в смысле ноль блять? был не ноль а стал ноль     сука
                    salaries.UnionWith(_salaries.Where(s => s.Employee.Department.Id == SelectedDepartment));

                FilteredSalaries.Clear();

                foreach (Salary salary in salaries)
                {
                    FilteredSalaries.Add(salary);
                }

                OnPropertyChanged(nameof(FilteredSalaries));
            }
            else ReloadFilteredCollection();
        }

        private void ApplyFilters(string number, string email, string name, DateTime date, bool datesBefore, bool datesInclusive)
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

            if (datesBefore)
            {
                salaries.UnionWith(_salaries.Where(s => datesInclusive ? s.Date >= date : s.Date > date));
            }
            else
            {
                salaries.UnionWith(_salaries.Where(s => datesInclusive ? s.Date <= date : s.Date < date));
            }

            FilteredSalaries.Clear();
            
            foreach (Salary salary in salaries)
            {
                FilteredSalaries.Add(salary);
            }

            OnPropertyChanged(nameof(FilteredSalaries));
        }

        private void ReloadFilteredCollection() => FilteredSalaries = new ObservableCollection<Salary>(_salaries);
    }
}
