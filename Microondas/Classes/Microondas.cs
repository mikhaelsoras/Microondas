using System;
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

        #region Properties

        public FuncaoMicroondas Funcao = new FuncaoMicroondas();

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
            Funcao = new FuncaoMicroondas(potencia, tempo);

            ValidarTempo(tempo);
            tempoRestante = tempo;
            Cozido = entrada;

            await Ligar();
        }

        public async Task InicioRapido(string entrada)
        {
            var tempo = new TimeSpan(0,0,30);
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
            for (int i = 0; i < Funcao.Potencia; i++)
                Cozido += '.';
        }

        private static void ValidarPotencia(int value)
        {
            if (value > 10)
                throw new PotenciaForaDoLimite("Potência com valor maior que o máximo permitido.");
            else if (value < 1)
                throw new PotenciaForaDoLimite("Potência com valor menor que o minimo permitido.");
        }

        private static void ValidarTempo(TimeSpan value)
        {
            if (value.TotalSeconds > 120)
                throw new TempoForaDoLimite("Tempo com valor maior que o máximo permitido.");
            else if (value.TotalSeconds < 1)
                throw new TempoForaDoLimite("Tempo com valor menor que o minimo permitido.");
        }
    }
}
