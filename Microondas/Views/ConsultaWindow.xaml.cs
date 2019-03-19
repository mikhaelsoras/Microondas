using MicroondasProject.Models;
using MicroondasProject.ViewModels;
using System;
using System.Windows;

namespace MicroondasProject.Views
{
    public partial class ConsultaWindow : Window
    {
        public ConsultaWindow(Microondas microondas)
        {
            InitializeComponent();
            (DataContext as ConsultaWindowViewModel).SetMicroondas(microondas);
        }
    }
}
