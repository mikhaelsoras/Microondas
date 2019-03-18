using MicroondasProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicroondasProject.ViewModels.Commands
{
    public class IniciarFuncaoCommand : ICommand
    {
        private MainWindowViewModel mainWindowViewModel;

        public IniciarFuncaoCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (mainWindowViewModel != null && mainWindowViewModel.IsLigado == false && parameter is FuncaoMicroondas)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            mainWindowViewModel.Iniciar(parameter as FuncaoMicroondas);
        }
    }
}
