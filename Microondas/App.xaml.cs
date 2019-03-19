using MicroondasProject.Views;
using ServicesLocator.Services;
using System.Windows;

namespace MicroondasProject
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Inicia do ServiceLocator
            new FileService().RegisterService();

            //Inicia o form principal
            MainWindow wnd = new MainWindow();
            wnd.Show();
        }
    }
}
