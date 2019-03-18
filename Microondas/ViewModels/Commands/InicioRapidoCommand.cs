using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicroondasProject.ViewModels.Commands
{
    public class InicioRapidoCommand : ICommand
    {
        private MainWindowViewModel mainWindowViewModel;

        public event EventHandler CanExecuteChanged;

        public InicioRapidoCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (mainWindowViewModel != null && mainWindowViewModel.IsLigado == false)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            mainWindowViewModel.InicioRapido();
        }
    }
}
