using System;

namespace Classes.Microondas
{
    public class TempoForaDoLimite : Exception
    {
        public TempoForaDoLimite() { }

        public TempoForaDoLimite(string message)
            : base(message) { }
    }

    public class PotenciaForaDoLimite : Exception
    {
        public PotenciaForaDoLimite() { }

        public PotenciaForaDoLimite(string message)
            : base(message) { }
    }

    public class TempoNaoInformado : Exception
    {
        public TempoNaoInformado() { }

        public TempoNaoInformado(string message)
            : base(message) { }
    }
}
