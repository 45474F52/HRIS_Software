using System;
using HRIS_Software.Core;
using HRIS_Software.Models.Database;

namespace HRIS_Software.Models.ModalDialogs
{
    internal sealed class CourseHandlerVM : ModalDialogVM<ObservableCourse>
    {
        public event Action OnSave = delegate { };

        public CourseHandlerVM(ObservableCourse course) : base(course)
        {
            Title = string.IsNullOrEmpty(course.CourseName) ? "Создание курса" : "Редактирование курса";

            SaveCommand = new RelayCommand(() =>
            {
                CancelCommand.Execute(null);

                if (course.IsDirty)
                {
                    OnSave();
                }
            });
        }

        public RelayCommand SaveCommand { get; }
    }
}
