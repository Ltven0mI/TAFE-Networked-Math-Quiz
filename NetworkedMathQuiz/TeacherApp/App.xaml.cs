using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TeacherApp.ViewModels;
using TeacherApp.ViewModels.Interfaces;
using TeacherApp.Views;
using Unity;

namespace TeacherApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize ENet
            ENet.Library.Initialize();

            // Create Container
            var container = new UnityContainer();

            // Register Services

            // Register ViewModels
            container.RegisterType<IViewMainWindowVM, MainWindowVM>();

            // Create MainView
            var view = container.Resolve<MainWindowView>();

            // Show the MainView
            view.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Deinitialize ENet
            ENet.Library.Deinitialize();
        }
    }
}
