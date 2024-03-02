using System.Collections.Generic;

namespace HRIS_Software.Models.Database
{
    internal sealed class ObservableCourse : ObservableEntity<Course>
    {
        public ObservableCourse(Course course) : base(course)
        {
            _oldName = course.CourseName;
            _oldDuration = course.DurationInHours;
            _oldCost = course.Cost;
        }

        private string _oldName;
        public string CourseName
        {
            get => _entity.CourseName;
            set
            {
                if (_oldName == value)
                {
                    _entity.CourseName = _oldName;
                    IsDirty = false;
                }
                else
                {
                    _oldName = _entity.CourseName ?? value;
                    _entity.CourseName = value;
                    IsDirty = true;
                }

                OnPropertyChanged();
            }
        }

        private int _oldDuration;
        public int DurationInHours
        {
            get => _entity.DurationInHours;
            set
            {
                if (_oldDuration == value)
                {
                    _entity.DurationInHours = _oldDuration;
                    IsDirty = false;
                }
                else
                {
                    _oldDuration = _entity.DurationInHours;
                    _entity.DurationInHours = value;
                    IsDirty = true;
                }

                OnPropertyChanged();
            }
        }

        private decimal _oldCost;
        public decimal Cost
        {
            get => _entity.Cost;
            set
            {
                if (_oldCost == value)
                {
                    _entity.Cost = _oldCost;
                    IsDirty = false;
                }
                else
                {
                    _oldCost = _entity.Cost;
                    _entity.Cost = value;
                    IsDirty = true;
                }

                OnPropertyChanged();
            }
        }

        public int Id => _entity.Id;
        public ICollection<EmployeesCours> EmployeesCourses => _entity.EmployeesCourses;
    }
}
