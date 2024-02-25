using Autofac;
using HRIS_Software.Models.Database;
using HRIS_Software.ViewModels.PagesVMs;
using HRIS_Software.ViewModels.WindowsVMs;
using HRIS_Software.Views.Windows;
using System.Windows;

namespace HRIS_Software
{
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            AuthenticationVM authCtx = new AuthenticationVM();
            Authentication auth = new Authentication
            {
                DataContext = authCtx
            };

            if (auth.ShowDialog() == true)
            {
                ContainerBuilder builder = new ContainerBuilder();

                builder.Register(x => new Entities(authCtx.Builder)).AsSelf().InstancePerLifetimeScope();

                _container = builder.Build();

                Main main = new Main
                {
                    DataContext = new MainVM(authCtx.Login, _container.Resolve<Entities>())
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

        protected override void OnExit(ExitEventArgs e)
        {
            _container?.Dispose();
            base.OnExit(e);
        }
    }
}
