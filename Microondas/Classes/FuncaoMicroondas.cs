using System;

namespace Classes.Microondas
{
    public class FuncaoMicroondas
    {
        public readonly string Nome;
        public readonly string Instrucao;
        public readonly string[] Alimentos; // se nenhum alimento existir, é considerado valido.
        public readonly char Caractere;

        public readonly int Potencia;
        public readonly TimeSpan Tempo;

        public FuncaoMicroondas()
        {
            Nome = "Padrão";
            Instrucao = "Uso Padrão.";
            Caractere = '.';

            Potencia = 0;
            Tempo = TimeSpan.Zero;
        }

        public FuncaoMicroondas(int potencia, TimeSpan tempo) : base()
        {
            Potencia = potencia;
            Tempo = tempo;
        }

        public FuncaoMicroondas(int potencia, TimeSpan tempo, string nome, string instrucao, 
            char caractere, string[] alimentos) : this(potencia, tempo)
        {
            Nome = nome;
            Instrucao = instrucao;
            Caractere = caractere;
            Alimentos = alimentos;
        }
    }
}
