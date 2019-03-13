using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Classes.Microondas;

namespace MicroondasProject
{
    public class MainWindowDados : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Props

        public Microondas MicroondasAtual { get; private set; }

        private string filtroFuncoes;
        public string FiltroFuncoes
        {
            get { return filtroFuncoes; }
            set
            {
                filtroFuncoes = value;
                OnPropertyChanged("FiltroFuncoes");
                CVFuncoes?.Refresh();
            }
        }

        public ICollectionView CVFuncoes { get; private set; }

        private string tempo;
        public string Tempo
        {
            get { return tempo; }
            set
            {
                tempo = value;
                OnPropertyChanged("Tempo");
            }
        }

        private string entrada;
        public string Entrada
        {
            get { return entrada; }
            set
            {
                entrada = value;
                OnPropertyChanged("Entrada");
            }
        }

        private string potencia;
        public string Potencia
        {
            get { return potencia; }
            set
            {
                potencia = value;
                OnPropertyChanged("Potencia");
            }
        }

        private bool inputEnabled;
        public bool InputEnabled
        {
            get { return inputEnabled; }
            set
            {
                inputEnabled = value;
                OnPropertyChanged("InputEnabled");
            }
        }
        #endregion
        
        private bool FiltrarFuncoes(object item)
        {
            var filtro = filtroFuncoes.Trim().ToLower();
            if (filtro.Length == 0)
                return true;

            var funcao = item as FuncaoMicroondas;
            var res = funcao.Nome.ToLower().Contains(filtro);
            return res;
        }

        public MainWindowDados()
        {
            Entrada = "";
            Tempo = "0:00";
            Potencia = "10";
            inputEnabled = true;
            filtroFuncoes = "";

            MicroondasAtual = new Microondas();

            CVFuncoes = CollectionViewSource.GetDefaultView(MicroondasAtual.Funcoes);
            CVFuncoes.Filter = FiltrarFuncoes;
        }

        public TimeSpan GetTempo()
        {
            if (Tempo.Trim() == "")
                throw new TempoNaoInformado("O tempo não foi informado.");

            TimeSpan valor;
            if (TimeSpan.TryParse("0:" + Tempo.Trim(), out valor))
                return valor;
            else
                throw new Exception("O tempo informado é inválido.");
        }

        public int GetPotencia()
        {
            int valor;
            if (int.TryParse(Potencia, out valor))
                return valor;
            else
                throw new Exception("A potência informada é inválida.");
        }
    }
}
