using HRIS_Software.ViewModels.WindowsVMs;
using HRIS_Software.Views.Windows;
using System.Data.Entity.Core.EntityClient;

namespace HRIS_Software.Models.Services
{
    internal sealed class LoginService
    {
        public static string StartLogin(out EntityConnectionStringBuilder connectionStringBuilder)
        {
            connectionStringBuilder = default;

            AuthenticationVM authCtx = new AuthenticationVM();
            Authentication auth = new Authentication
            {
                DataContext = authCtx
            };

            if (auth.ShowDialog() == true)
            {
                connectionStringBuilder = authCtx.Builder;
                return authCtx.Login;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
