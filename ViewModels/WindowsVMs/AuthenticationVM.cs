using System.Windows;
using HRIS_Software.Core;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using HRIS_Software.Models.ModalDialogs;

namespace HRIS_Software.ViewModels.WindowsVMs
{
    internal sealed class AuthenticationVM : BaseVM
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

        public EntityConnectionStringBuilder Builder { get; private set; }

        private void Authenticate()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = Properties.Settings.Default.DataSource ?? string.Empty,
                InitialCatalog = Properties.Settings.Default.InitialCatalog ?? string.Empty,
                MultipleActiveResultSets = true,
                TrustServerCertificate = true,
                UserID = Login ?? string.Empty,
                Password = Password ?? string.Empty
            };

            if (TestConnection(builder.ConnectionString))
            {
                Builder = new EntityConnectionStringBuilder()
                {
                    Provider = "System.Data.SqlClient",
                    Metadata = "res://*/Models.Database.HRIS_Model.csdl|res://*/Models.Database.HRIS_Model.ssdl|res://*/Models.Database.HRIS_Model.msl",
                    ProviderConnectionString = builder.ConnectionString
                };

                DialogResult = true;
            }
            else
            {
                ModalDialog = new ShowMessageVM("Неверный логин или пароль", "Ошибка аутентификации", MessageType.Error);
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
