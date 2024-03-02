namespace HRIS_Software.Models.Database
{
    public partial class Employee
    {
        public string FullName => string.Format("{0} {1} {2}", Surname, FirstName, Patronymic);
    }
}
