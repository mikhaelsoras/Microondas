using Newtonsoft.Json;
using ServicesLocator.Interfaces;
using ServicesLocator.Locator;
using System;

namespace Classes.Microondas
{
    public class FuncaoMicroondas
    {
        public string Nome { get; set; }
        public string Instrucao { get; set; }
        public string Alimento { get; set; }
        public char Caractere { get; set; }

        public int Potencia { get; set; }
        public TimeSpan Tempo { get; set; }

        [JsonIgnore]
        public bool Predefinida { get; protected set; }

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

        public virtual void Validar()
        {
            ValidarPotencia(Potencia);
            ValidarTempo(Tempo);
        }

        public virtual void ValidarEntrada(string entrada, bool buscarArquivo = true)
        {
            //se não possuir alimento definido ele considera que todos são permitidos.
            if (Alimento == null || Alimento == "")
                return;

            var FS = ServiceLocator.Get<IFileService>();

            string lcEntrada;

            if (buscarArquivo && FS.FileExists(entrada))
                lcEntrada = FS.Carregar(entrada).ToLower().Trim();
            else
                lcEntrada = entrada.ToLower().Trim();

            var lcAlimento = Alimento.ToLower().Trim();

            if (lcEntrada.Contains(lcAlimento))                
                return;

            throw new AlimentoIncompativelException("A função não é compativel com o alimento " + lcEntrada);
        }

        protected virtual void ValidarPotencia(int value)
        {
            if (value > 10)
                throw new PotenciaForaDoLimiteException("Potência com valor maior que o máximo permitido.");
            else if (value < 1)
                throw new PotenciaForaDoLimiteException("Potência com valor menor que o minimo permitido.");
        }

        protected virtual void ValidarTempo(TimeSpan value)
        {
            if (value.TotalSeconds > 120)
                throw new TempoForaDoLimiteException("Tempo com valor maior que o máximo permitido.");
            else if (value.TotalSeconds < 1)
                throw new TempoForaDoLimiteException("Tempo com valor menor que o minimo permitido.");
        }
    }
}
