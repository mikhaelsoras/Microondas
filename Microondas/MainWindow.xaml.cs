using System;
using System.Windows;
using Classes.Microondas;

namespace MicroondasProject
{
    public partial class MainWindow : Window
    {
        private MainWindowDados dados;

        public MainWindow()
        {
            InitializeComponent();

            dados = new MainWindowDados();
            dados.MicroondasAtual.TempoRestanteChanged += TempoRestanteChanged;
            DataContext = dados;
        }

        private void TempoRestanteChanged(Microondas obj)
        {
            dados.Entrada = obj.Cozido;
            dados.Tempo = obj.TempoRestante.ToString(@"mm\:ss");
        }

        async void Iniciar()
        {
            try
            {
                var tempo = dados.GetTempo();
                var potencia = dados.GetPotencia();
                var entrada = dados.Entrada.Trim();

                dados.InputEnabled = false;
                await dados.MicroondasAtual.Iniciar(tempo, potencia, entrada);
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

        private void TesteClick(object sender, RoutedEventArgs e)
        {
            //ServiceLocator.Get<ILocalDataService>().Salvar();
            //var txt = ServiceLocator.Get<ILocalDataService>().Carregar();

            dados.MicroondasAtual.Funcoes.Add(new FuncaoMicroondas(5, new TimeSpan(0, 0, 25), "Teste", "instrucao teste", '@', "frango"));
        }

        private void ConsultarClick(object sender, RoutedEventArgs e)
        {
            var window = new ConsultaWindow(dados.MicroondasAtual);
            window.ShowDialog();
        }
    }
}
