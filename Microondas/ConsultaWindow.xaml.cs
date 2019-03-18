using MicroondasProject.Models;
using MicroondasProject.ViewModels;
using System;
using System.Windows;

namespace MicroondasProject
{
    public partial class ConsultaWindow : Window
    {
        ConsultaWindowViewModel dados;

        public ConsultaWindow(Microondas microondas)
        {
            InitializeComponent();
            dados = new ConsultaWindowViewModel(microondas);
            DataContext = dados;
        }

        private void AdicionarClick(object sender, RoutedEventArgs e)
        {
            var potencia = dados.GetPotencia();
            var tempo = dados.GetTempo();
            var caractere = dados.GetCaractere();
            var nome = dados.Nome.Trim();
            var instrucao = dados.Instrucao.Trim();

            string alimento = "";
            if (dados.Alimento != null)
                alimento = dados.Alimento.Trim();

            try
            {
                dados.MicroondasAtivo.CadastrarFuncao(potencia, tempo, nome, instrucao, caractere, alimento);
                dados.Potencia = "";
                dados.Tempo = "";
                dados.Caractere = "";
                dados.Nome = "";
                dados.Instrucao = "";
                dados.Alimento = "";
            }
            catch (Exception ex)
            {
                ExibirErro(ex.Message);
            }
        }

        private void ExibirErro(string obj)
        {
            MessageBox.Show(obj, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
