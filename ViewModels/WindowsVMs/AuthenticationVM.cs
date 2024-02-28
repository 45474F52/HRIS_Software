using System.Security;
using HRIS_Software.Core;
using System.Data.SqlClient;
using HRIS_Software.Models.Extensions;
using HRIS_Software.Models.ModalDialogs;
using System.Data.Entity.Core.EntityClient;

namespace HRIS_Software.ViewModels.WindowsVMs
{
    internal sealed class AuthenticationVM : BaseVM
    {
        public AuthenticationVM()
        {
            Password = new SecureString();

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

        private SecureString _password;
        public SecureString Password
        {
            get => _password;
            set
            {
                if (_password != null)
                {
                    _password.Dispose();
                }

                _password = value.Copy();
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
                Password = Password.ToUnsecuredString()
            };

            ClearSecurePassword();

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

        private void ClearSecurePassword()
        {
            _password.Dispose();
            _password = new SecureString();
        }
    }
}
