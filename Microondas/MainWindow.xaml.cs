using System;
using System.Windows;
using Classes.Microondas;

namespace MicroondasProject
{

    public partial class MainWindow : Window
    {
        private DadosFormPrincipal dados;
        private Microondas microondas;

        public MainWindow()
        {
            InitializeComponent();

            microondas = new Microondas();
            microondas.TempoRestanteChanged += TempoRestanteChanged;
            dados = new DadosFormPrincipal();
            DataContext = dados;
        }

        private void TempoRestanteChanged(Microondas obj)
        {
            dados.Entrada = obj.Cozido;
        }

        async void Iniciar()
        {
            try
            {
                var tempo = dados.GetTempo();
                var potencia = dados.GetPotencia();
                var entrada = dados.Entrada.Trim();

                dados.InputEnabled = false;
                await microondas.Iniciar(tempo, potencia, entrada);
                dados.InputEnabled = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                dados.InputEnabled = true;
            }
        }

        private void IniciarClick(object sender, RoutedEventArgs e)
        {
            Iniciar();
        }

        private void InicioRapidoClick(object sender, RoutedEventArgs e)
        {
            dados.Potencia = "8";
            dados.Tempo = "0:20";

            Iniciar();
        }
    }
}
