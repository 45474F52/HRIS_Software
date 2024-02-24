namespace HRIS_Software.ViewModels.WindowsVMs
{
    public sealed class MainVM : BaseVM
    {
        public MainVM(string login)
        {
            Title = "Human Resources Information System";

            Login = login;
            OnPropertyChanged(nameof(Login));
        }

        public string Login { get; }
    }
}
