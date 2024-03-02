using HRIS_Software.Core;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.ModalDialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace HRIS_Software.ViewModels.PagesVMs
{
    internal sealed class CoursesVM : BaseVM
    {
        private readonly List<Course> _coursesBase = new List<Course>();

        public CoursesVM(Entities db)
        {
            Title = "Курсы";

            CreateCourseCommand = new RelayCommand(() =>
            {
                Course course = new Course();
                ObservableCourse observableCourse = new ObservableCourse(course);

                CourseHandlerVM courseHandler = new CourseHandlerVM(observableCourse);
                courseHandler.OnSave += async () =>
                {
                    _coursesBase.Add(course);
                    Courses.Add(observableCourse);

                    //db.Courses.Add(course);
                    //await db.SaveChangesAsync();
                };

                ModalDialog = courseHandler;
            });

            EditCourseCommand = new RelayCommand<ObservableCourse>(course =>
            {
                int index = Courses.IndexOf(course);

                CourseHandlerVM courseHandler = new CourseHandlerVM(course);
                courseHandler.OnSave += async () =>
                {
                    Courses.RemoveAt(index);
                    Courses.Insert(index, course);

                    //await db.SaveChangesAsync();
                };

                ModalDialog = courseHandler;
            });

            MarkCourseAsRemovableCommand = new RelayCommand<ObservableCourse>(course => course.IsMarkedAsRemovable = true);

            DeleteCourseCommand = new RelayCommand<ObservableCourse>(async course =>
            {
                Courses.Remove(course);

                Course target = await db.Courses.FindAsync(course.Id);
                _coursesBase.Remove(target);

                //db.Courses.Remove(target);
                //await db.SaveChangesAsync();
            });

            Application.Current.Dispatcher.Invoke(async () =>
            {
                _coursesBase.AddRange(await db.Courses.ToListAsync());
                Courses = new ObservableCollection<ObservableCourse>(
                    _coursesBase.Select(course => new ObservableCourse(course)));
            });
        }

        public RelayCommand CreateCourseCommand { get; }
        public RelayCommand<ObservableCourse> EditCourseCommand { get; }
        public RelayCommand<ObservableCourse> MarkCourseAsRemovableCommand { get; }
        public RelayCommand<ObservableCourse> DeleteCourseCommand { get; }

        private ObservableCollection<ObservableCourse> _courses;
        public ObservableCollection<ObservableCourse> Courses
        {
            get => _courses;
            private set
            {
                _courses = value;
                OnPropertyChanged();
            }
        }

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
    }
}
