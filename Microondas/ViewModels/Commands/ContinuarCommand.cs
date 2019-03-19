using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicroondasProject.ViewModels.Commands
{
    public class ContinuarCommand : ICommand
    {
        private MainWindowViewModel mainWindowViewModel;

        public ContinuarCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (mainWindowViewModel != null && mainWindowViewModel.IsLigado && mainWindowViewModel.IsPausado)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            mainWindowViewModel.Continuar();
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
