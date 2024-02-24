using HRIS_Software.ViewModels.WindowsVMs;
using HRIS_Software.Views.Windows;
using System.Windows;

namespace HRIS_Software
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            AuthenticationVM authCtx = new AuthenticationVM();
            Authentication auth = new Authentication
            {
                DataContext = authCtx
            };

            bool? result = auth.ShowDialog();
            if (result == true)
            {
                Main main = new Main
                {
                    DataContext = new MainVM(authCtx.Login)
                };

                ShutdownMode = ShutdownMode.OnLastWindowClose;

                main.Show();
            }
            else
            {
                Shutdown();
            }

            base.OnStartup(e);
        }
    }
}
