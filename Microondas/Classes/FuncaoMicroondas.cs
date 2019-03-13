using System;

namespace Classes.Microondas
{
    public class FuncaoMicroondas
    {
        public string Nome { get; private set; }
        public string Instrucao { get; private set; }
        public string Alimento { get; private set; }
        public char Caractere { get; private set; }

        public int Potencia { get; private set; }
        public TimeSpan Tempo { get; private set; }
        public bool Predefinida { get; private set; }

        public FuncaoMicroondas()
        {
            Nome = "Padrão";
            Instrucao = "Uso Padrão.";
            Caractere = '.';

            Potencia = 0;
            Tempo = TimeSpan.Zero;
            Predefinida = false;
        }

        public FuncaoMicroondas(int potencia, TimeSpan tempo) : this()
        {
            Potencia = potencia;
            Tempo = tempo;
        }

        public FuncaoMicroondas(int potencia, TimeSpan tempo, string nome, string instrucao, 
            char caractere, string alimento, bool predefinida = false) : this(potencia, tempo)
        {
            Nome = nome;
            Instrucao = instrucao;
            Caractere = caractere;
            Alimento = alimento;
            Predefinida = predefinida;
        }

        public void Validar(string entrada)
        {
            ValidarPotencia(Potencia);
            ValidarTempo(Tempo);
            ValidarEntrada(entrada);
        }

        void ValidarEntrada(string entrada)
        {
            //se não possuir alimento definido ele considera que todos são permitidos.
            if (Alimento == null || Alimento == "")
                return;

            var lcAlimento = Alimento.ToLower().Trim();
            var lcEntrada = entrada.ToLower().Trim();

            if (lcEntrada.Contains(lcAlimento))                
                return;

            throw new AlimentoIncompativel("A função não é compativel com o alimento " + entrada);
        }

        void ValidarPotencia(int value)
        {
            if (value > 10)
                throw new PotenciaForaDoLimite("Potência com valor maior que o máximo permitido.");
            else if (value < 1)
                throw new PotenciaForaDoLimite("Potência com valor menor que o minimo permitido.");
        }

        void ValidarTempo(TimeSpan value)
        {
            if (value.TotalSeconds > 120)
                throw new TempoForaDoLimite("Tempo com valor maior que o máximo permitido.");
            else if (value.TotalSeconds < 1)
                throw new TempoForaDoLimite("Tempo com valor menor que o minimo permitido.");
        }
    }
}
