using System;

namespace Classes.Microondas
{
    public class TempoForaDoLimiteException : Exception
    {
        public TempoForaDoLimiteException(string message)
            : base(message) { }
    }

    public class NomeFuncaoNaoInformadaException : Exception
    {
        public NomeFuncaoNaoInformadaException(string message)
            : base(message) { }
    }

    public class AlimentoIncompativelException : Exception
    {
        public AlimentoIncompativelException(string message)
            : base(message) { }
    }

    public class PotenciaForaDoLimiteException : Exception
    {
        public PotenciaForaDoLimiteException(string message)
            : base(message) { }
    }

    public class TempoNaoInformadoException : Exception
    {
        public TempoNaoInformadoException(string message)
            : base(message) { }
    }
}
