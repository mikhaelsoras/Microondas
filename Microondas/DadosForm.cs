using System;
using System.ComponentModel;
using Classes.Microondas;

namespace MicroondasProject
{
    public class DadosFormPrincipal : INotifyPropertyChanged
    {
        #region Props
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public DadosFormPrincipal()
        {
            Entrada = "";
            Tempo = "0:00";
            Potencia = "10";
            inputEnabled = true;
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
