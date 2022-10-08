using Frenchinnov.GloryCashier.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Test
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
               
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
              
            };
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            if(exception is GloryCashierException)
            {
                MessageBox.Show("GloryCashierException");
            }
          
        }
    }
}
