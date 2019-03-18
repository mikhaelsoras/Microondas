using System;
using System.Windows.Input;

namespace MicroondasProject.ViewModels.Commands
{
    public class IniciarCommand : ICommand
    {
        private MainWindowViewModel mainWindowViewModel;

        public IniciarCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (mainWindowViewModel != null && mainWindowViewModel.IsLigado == false)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            mainWindowViewModel.Iniciar();
        }
    }
}
