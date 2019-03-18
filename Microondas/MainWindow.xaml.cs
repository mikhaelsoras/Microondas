using System;
using System.Windows;
using System.Windows.Controls;
using Classes.Microondas;

namespace MicroondasProject
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel dados;

        public MainWindow()
        {
            InitializeComponent();

            dados = new MainWindowViewModel();
            dados.MicroondasAtual.TempoRestanteChanged += TempoRestanteChanged;
            dados.MicroondasAtual.Concluido += ConcluidoAquecimento;
            dados.MicroondasAtual.Cancelado += CanceladoAquecimento;
            dados.MicroondasAtual.Erro += ExibirErro;
            DataContext = dados;
        }

        private void CanceladoAquecimento()
        {
            MessageBox.Show("Cancelado");
            dados.IsLigado = false;
        }

        private void ExibirErro(string obj)
        {
            if (dados.IsLigado)
                dados.IsLigado = false;
            MessageBox.Show(obj, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ConcluidoAquecimento(string obj)
        {
            MessageBox.Show("Concluido: " + obj);
            dados.IsLigado = false;
        }

        private void TempoRestanteChanged(Microondas obj)
        {
            dados.Entrada = obj.EntradaAquecida;
            dados.Tempo = obj.TempoRestante.ToString(@"mm\:ss");
            dados.Potencia = obj.FuncaoAtual.Potencia.ToString();
        }

        private void Iniciar()
        {
            try
            {
                var tempo = dados.GetTempo();
                var potencia = dados.GetPotencia();
                var entrada = dados.Entrada.Trim();

                dados.IsLigado = true;
                dados.MicroondasAtual.Iniciar(tempo, potencia, entrada);
            }
            catch (Exception e)
            {
                ExibirErro(e.Message);
            }
        }

        private void Iniciar(FuncaoMicroondas funcaoMicroondas)
        {
            try
            {
                var entrada = dados.Entrada.Trim();
                dados.IsLigado = true;
                dados.MicroondasAtual.Iniciar(funcaoMicroondas, entrada);
            }
            catch (Exception e)
            {
                ExibirErro(e.Message);
            }
        }

        private void IniciarClick(object sender, RoutedEventArgs e)
        {
            Iniciar();
        }

        private void InicioRapidoClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var entrada = dados.Entrada.Trim();

                dados.IsLigado = true;
                dados.MicroondasAtual.InicioRapido(entrada);
            }
            catch (Exception ex)
            {
                ExibirErro(ex.Message);
            }
        }

        private void ConsultarClick(object sender, RoutedEventArgs e)
        {
            var window = new ConsultaWindow(dados.MicroondasAtual);
            window.Owner = this;
            window.ShowDialog();
        }

        private void FuncaoClick(object sender, RoutedEventArgs e)
        {
            var fm = (sender as Button).DataContext as FuncaoMicroondas;
            Iniciar(fm);
        }

        private void PausarClick(object sender, RoutedEventArgs e)
        {
            if (!dados.IsPausado)
                dados.MicroondasAtual.Pausar();
            else
                dados.MicroondasAtual.Continuar();
        }

        private void CancelarClick(object sender, RoutedEventArgs e)
        {
            dados.MicroondasAtual.Cancelar();
        }
    }
}
