using System;
using System.Windows.Forms;
using DevExpress.Mvvm;
using EnpassJSONViewer.Services;

namespace EnpassJSONViewer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServiceContainer.Default.RegisterService(new JSONEnpassDatabaseLoader());
            ServiceContainer.Default.RegisterService(new WinformsClipboardService());

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}