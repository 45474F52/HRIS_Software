using System.Windows;
using HRIS_Software.Core;
using System.Data.SqlClient;

namespace HRIS_Software.ViewModels.WindowsVMs
{
    public sealed class AuthenticationVM : BaseVM
    {
        public AuthenticationVM()
        {
            Title = "Аутентификация";

            LoginCommand = new RelayCommand(_ => Authenticate());
        }

        public RelayCommand LoginCommand { get; private set; }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _dialogResult;
        public bool DialogResult
        {
            get => _dialogResult;
            private set
            {
                _dialogResult = value;
                OnPropertyChanged();
            }
        }

        private void Authenticate()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = Properties.Settings.Default.DataSource ?? string.Empty,
                InitialCatalog = Properties.Settings.Default.InitialCatalog ?? string.Empty,
                TrustServerCertificate = true,
                UserID = Login ?? string.Empty,
                Password = Password ?? string.Empty
            };

            if (TestConnection(builder.ConnectionString))
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private bool TestConnection(string connectionString)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;

                try
                {
                    connection.Open();
                    connection.Close();

                    result = true;
                }
                catch (SqlException)
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
