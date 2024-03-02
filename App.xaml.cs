using Autofac;
using HRIS_Software.Models.Database;
using HRIS_Software.Models.Services;
using HRIS_Software.ViewModels.WindowsVMs;
using HRIS_Software.Views.Windows;
using System;
using System.Data.Entity.Core.EntityClient;
using System.Windows;

namespace HRIS_Software
{
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            string login = LoginService.StartLogin(out EntityConnectionStringBuilder connectionStringBuilder);

            if (!string.IsNullOrEmpty(login))
            {
                BuildContainer(connectionStringBuilder);

                MainVM vm = new MainVM(login, _container.Resolve<Entities>());
                vm.OnLoginChanged += OnLoginChanged;

                Main main = new Main
                {
                    DataContext = vm
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

        private void BuildContainer(EntityConnectionStringBuilder connectionStringBuilder)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Register(x => new Entities(connectionStringBuilder)).AsSelf().InstancePerLifetimeScope();
            _container = builder.Build();
        }

        private void OnLoginChanged(EntityConnectionStringBuilder connectionStringBuilder, Action<Entities> newDB)
        {
            BuildContainer(connectionStringBuilder);
            newDB(_container.Resolve<Entities>());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container?.Dispose();
            base.OnExit(e);
        }
    }
}
