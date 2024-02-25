using HRIS_Software.Models.Database;

namespace HRIS_Software.Models.ModalDialogs
{
    internal sealed class ShowContactInfoVM : ModalDialogVM<Employee>
    {
        public ShowContactInfoVM(Employee employee) : base(employee)
        {
            Title = string.Format("{0} {1} {2}", employee.Surname, employee.FirstName, employee.Patronymic);
        }
    }
}
