using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Classes.Microondas
{
    public class Microondas
    {
        #region Eventos
        public Action<Microondas> TempoRestanteChanged;
        public Action<string> Concluido;

        void OnTempoRestanteChanged()
        {
            TempoRestanteChanged?.Invoke(this);
        }

        void OnConcluido()
        {
            Concluido?.Invoke(Cozido);
        }
        #endregion

        #region Props
        private ObservableCollection<FuncaoMicroondas> funcoes;
        public ObservableCollection<FuncaoMicroondas> Funcoes
        {
            get
            {
                if (funcoes == null)
                    funcoes = new ObservableCollection<FuncaoMicroondas>();
                return funcoes;
            }
            set
            {
                funcoes = value;
            }
        }

        public FuncaoMicroondas FuncaoAtual = new FuncaoMicroondas();

        private TimeSpan tempoRestante;
        public TimeSpan TempoRestante
        {
            get
            {
                return tempoRestante;
            }
            private set
            {
                tempoRestante = value;
                OnTempoRestanteChanged();
            }
        }

        public string Cozido { get; private set; }
        #endregion


        public async Task Iniciar(TimeSpan tempo, int potencia, string entrada)
        {
            var funcao = new FuncaoMicroondas(potencia, tempo);
            await Iniciar(funcao, entrada);
        }

        public async Task Iniciar(FuncaoMicroondas funcao, string entrada)
        {
            funcao.Validar(entrada.Trim());

            FuncaoAtual = funcao;
            Cozido = entrada;
            TempoRestante = funcao.Tempo;

            await Ligar();
        }

        public async Task InicioRapido(string entrada)
        {
            var tempo = new TimeSpan(0, 0, 30);
            await Iniciar(tempo, 8, entrada);
        }

        async Task Ligar()
        {
            var tick = new TimeSpan(0, 0, 1);

            while (TempoRestante.TotalSeconds > 0)
            {
                await Aquecer(tick);
                TempoRestante = TempoRestante.Subtract(tick);
            }

            OnConcluido();
        }

        async Task Aquecer(TimeSpan tempo)
        {
            await Task.Delay(tempo);            
            for (int i = 0; i < FuncaoAtual.Potencia; i++)
                Cozido += FuncaoAtual.Caractere;
        }
    }
}
