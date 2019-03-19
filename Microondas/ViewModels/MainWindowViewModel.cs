using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using MicroondasProject.Models;
using MicroondasProject.ViewModels.Commands;
using MicroondasProject.Views;

namespace MicroondasProject.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Props
        public ICollectionView CVFuncoes { get; private set; }

        private string filtroFuncoes;
        public string FiltroFuncoes
        {
            get { return filtroFuncoes; }
            set
            {
                filtroFuncoes = value;
                OnPropertyChanged();
                CVFuncoes?.Refresh();
            }
        }


        private string tempo;
        public string Tempo
        {
            get { return tempo; }
            set
            {
                tempo = value;
                OnPropertyChanged();
            }
        }

        private string entrada;
        public string Entrada
        {
            get { return entrada; }
            set
            {
                entrada = value;
                OnPropertyChanged();
            }
        }

        private string potencia;
        public string Potencia
        {
            get { return potencia; }
            set
            {
                potencia = value;
                OnPropertyChanged();
            }
        }

        private bool isLigado;
        public bool IsLigado
        {
            get { return isLigado; }
            set
            {
                isLigado = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDesligado));

                CancelarCommand.OnCanExecuteChanged();
                ContinuarCommand.OnCanExecuteChanged();
                PausarCommand.OnCanExecuteChanged();
                ConsultarCommand.OnCanExecuteChanged();

                IniciarFuncaoCommand.OnCanExecuteChanged();
                IniciarCommand.OnCanExecuteChanged();
                InicioRapidoCommand.OnCanExecuteChanged();
            }
        }

        public bool IsDesligado => !isLigado;

        public bool IsPausado => microondas.IsPausado;
        public bool IsNaoPausado => !IsPausado;
        #endregion

        #region Commands
        public IniciarCommand IniciarCommand { get; set; }
        public InicioRapidoCommand InicioRapidoCommand { get; set; }
        public IniciarFuncaoCommand IniciarFuncaoCommand { get; set; }
        public CancelarCommand CancelarCommand { get; set; }
        public ContinuarCommand ContinuarCommand { get; set; }
        public PausarCommand PausarCommand { get; set; }
        public ConsultarCommand ConsultarCommand { get; set; }
        #endregion

        private Microondas microondas;

        public MainWindowViewModel()
        {
            Entrada = "";
            Tempo = "0:00";
            Potencia = "10";
            isLigado = false;
            filtroFuncoes = "";

            this.IniciarCommand = new IniciarCommand(this);
            this.IniciarFuncaoCommand = new IniciarFuncaoCommand(this);
            this.InicioRapidoCommand = new InicioRapidoCommand(this);
            this.CancelarCommand = new CancelarCommand(this);
            this.ContinuarCommand = new ContinuarCommand(this);
            this.PausarCommand = new PausarCommand(this);
            this.ConsultarCommand = new ConsultarCommand(this);

            microondas = new Microondas();
            microondas.PausarChanged += PausarChanged;
            microondas.TempoRestanteChanged += TempoRestanteChanged;
            microondas.Concluido += ConcluidoAquecimento;
            microondas.Cancelado += CanceladoAquecimento;
            microondas.Erro += ExibirErro;

            CVFuncoes = CollectionViewSource.GetDefaultView(microondas.Funcoes);
            CVFuncoes.Filter = FiltrarFuncoes;
        }

        private bool FiltrarFuncoes(object item)
        {
            var filtro = filtroFuncoes.Trim().ToLower();
            if (filtro.Length == 0)
                return true;

            var funcao = item as FuncaoMicroondas;
            var res = funcao.Nome.ToLower().Contains(filtro);
            return res;
        }

        private void TempoRestanteChanged(Microondas obj)
        {
            Entrada = obj.EntradaAquecida;
            Tempo = obj.TempoRestante.ToString(@"mm\:ss");
            Potencia = obj.FuncaoAtual.Potencia.ToString();
        }

        private void PausarChanged(bool obj)
        {
            OnPropertyChanged(nameof(IsPausado));
            OnPropertyChanged(nameof(IsNaoPausado));

            ContinuarCommand.OnCanExecuteChanged();
            PausarCommand.OnCanExecuteChanged();
        }

        private void ConcluidoAquecimento(string obj)
        {
            MessageBox.Show("Concluido: " + obj);
            IsLigado = false;
        }

        private void ExibirErro(string obj)
        {
            if (IsLigado)
                IsLigado = false;
            MessageBox.Show(obj, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public TimeSpan GetTempo()
        {
            if (Tempo.Trim() == "")
                throw new TempoNaoInformadoException("O tempo não foi informado.");

            TimeSpan valor;
            if (TimeSpan.TryParse("0:" + Tempo.Trim(), out valor))
                return valor;
            else
                throw new Exception("O tempo informado é inválido.");
        }

        public int GetPotencia()
        {
            if (Potencia.Trim().Length == 0)
                throw new Exception("Potência não informada.");
            int valor;
            if (int.TryParse(Potencia, out valor))
                return valor;
            else
                throw new Exception("A potência informada é inválida.");
        }

        public void CanceladoAquecimento()
        {
            MessageBox.Show("Cancelado");
            IsLigado = false;
        }

        public void Iniciar()
        {
            try
            {
                var tempo = GetTempo();
                var potencia = GetPotencia();
                var funcao = new FuncaoMicroondas(potencia, tempo);

                Iniciar(funcao);
            }
            catch (Exception e)
            {
                ExibirErro(e.Message);
            }
        }

        public void Iniciar(FuncaoMicroondas funcaoMicroondas)
        {
            try
            {
                var entrada = Entrada.Trim();
                IsLigado = true;
                microondas.Iniciar(funcaoMicroondas, entrada);
            }
            catch (Exception e)
            {
                ExibirErro(e.Message);
            }
        }

        public void InicioRapido()
        {
            try
            {
                var entrada = Entrada.Trim();
                IsLigado = true;
                microondas.InicioRapido(entrada);
            }
            catch (Exception ex)
            {
                ExibirErro(ex.Message);
            }
        }

        public void Pausar()
        {
            microondas.Pausar();
        }

        public void Continuar()
        {
            microondas.Continuar();
        }

        public void Cancelar()
        {
            microondas.Cancelar();
        }

        public void Consultar()
        {
            var window = new ConsultaWindow(microondas);
            window.ShowDialog();
        }
    }
}
