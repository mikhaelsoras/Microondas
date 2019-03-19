using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicroondasProject.ViewModels.Commands
{
    public class AdicionarFuncaoCommand : ICommand
    {
        private ConsultaWindowViewModel consultarWindowViewModel;

        public AdicionarFuncaoCommand(ConsultaWindowViewModel consultarWindowViewModel)
        {
            this.consultarWindowViewModel = consultarWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            consultarWindowViewModel.Adicionar();
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
