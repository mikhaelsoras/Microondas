using MicroondasProject.Models;
using MicroondasProject.ViewModels.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace MicroondasProject.ViewModels
{
    public class ConsultaWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Props
        public ICollectionView CVFuncoes { get; private set; }

        private string filtroAlimento;
        public string FiltroAlimento
        {
            get { return filtroAlimento; }
            set
            {
                filtroAlimento = value;
                OnPropertyChanged();
                CVFuncoes?.Refresh();
            }
        }

        #region Cadastro
        private string nome;
        public string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                OnPropertyChanged();
            }
        }

        private string instrucao;
        public string Instrucao
        {
            get { return instrucao; }
            set
            {
                instrucao = value;
                OnPropertyChanged();
            }
        }

        private string alimento;
        public string Alimento
        {
            get { return alimento; }
            set
            {
                alimento = value;
                OnPropertyChanged();
            }
        }

        private string caractere;
        public string Caractere
        {
            get { return caractere; }
            set
            {
                caractere = value;
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
        #endregion

        #endregion

        public AdicionarFuncaoCommand AdicionarFuncaoCommand { get; set; }

        private Microondas microondas;

        public ConsultaWindowViewModel(Microondas microondas)
        {
            this.microondas = microondas;
            filtroAlimento = "";
            CVFuncoes = CollectionViewSource.GetDefaultView(this.microondas.Funcoes);
            CVFuncoes.Filter = FiltrarFuncoes;

            AdicionarFuncaoCommand = new AdicionarFuncaoCommand(this);
        }

        private bool FiltrarFuncoes(object item)
        {
            var filtro = filtroAlimento.Trim().ToLower();
            if (filtro.Length == 0)
                return true;

            var funcao = item as FuncaoMicroondas;
            if (funcao.Alimento == null || funcao.Alimento == "")
                return false;

            var res = funcao.Alimento.ToLower().Contains(filtro);
            return res;
        }

        public char GetCaractere()
        {
            if (Caractere.Trim().Length == 0)
                throw new Exception("Caractere não informado.");
            else if (Caractere.Length != 1)
                throw new Exception("Caractere inválido.");

            return Caractere.Trim()[0];
        }

        public TimeSpan GetTempo()
        {
            if (Potencia.Trim().Length == 0)
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

        public void Adicionar()
        {
            var potencia = GetPotencia();
            var tempo = GetTempo();
            var caractere = GetCaractere();
            var nome = Nome.Trim();
            var instrucao = Instrucao.Trim();

            string alimento = "";
            if (Alimento != null)
                alimento = Alimento.Trim();

            try
            {
                microondas.CadastrarFuncao(potencia, tempo, nome, instrucao, caractere, alimento);
                Potencia = "";
                Tempo = "";
                Caractere = "";
                Nome = "";
                Instrucao = "";
                Alimento = "";
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
