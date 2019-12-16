using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;

namespace Notepad1
{
    public class LocApp : Application
    {
        [STAThread]

        // replaces the default main
        public static void Main()
        {
            App app = new App();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            MainWindow wnd = new MainWindow();
            wnd.Closed += Wnd_Closed;
            app.Run(wnd);
        }

        private static void Wnd_Closed(object sender, EventArgs e)
        {
            MainWindow wnd = sender as MainWindow;
            if (!string.IsNullOrEmpty(wnd.LangSwitch))
            {
                string lang = wnd.LangSwitch;

                wnd.Closed -= Wnd_Closed;

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

                wnd = new MainWindow();
                wnd.Closed += Wnd_Closed;
                wnd.Show();
            }
            else
            {
                App.Current.Shutdown();
            }
        }
    }
}
