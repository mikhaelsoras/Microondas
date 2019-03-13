using Newtonsoft.Json;
using ServicesLocator.Interfaces;
using ServicesLocator.Locator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Classes.Microondas
{
    public class Microondas
    {
        #region Eventos
        public Action<Microondas> TempoRestanteChanged;
        public Action<string> Concluido;
        public Action<string> Erro;

        void OnTempoRestanteChanged()
        {
            TempoRestanteChanged?.Invoke(this);
        }

        void OnConcluido()
        {
            Concluido?.Invoke(Cozido);
        }

        //Retorna true caso o evento exista
        bool OnErro(string msg)
        {  
            Erro?.Invoke(msg);
            return Erro != null;
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

        public Microondas()
        {
            //Funções padrão
            Funcoes.Add(new FuncaoMicroondas(2, new TimeSpan(0, 1, 0), "Descongelar", "Instrução para função descongelar.", '?', null, true));
            Funcoes.Add(new FuncaoMicroondas(6, new TimeSpan(0, 2, 0), "Lasanha", "Instrução para função lasanha.", ';', "Lasanha", true));
            Funcoes.Add(new FuncaoMicroondas(7, new TimeSpan(0, 0, 50), "Pipoca", "Instrução para função pipoca.", '=', "Pipoca", true));
            Funcoes.Add(new FuncaoMicroondas(4, new TimeSpan(0, 2, 0), "Arroz", "Instrução para função arroz.", '-', "Arroz", true));
            Funcoes.Add(new FuncaoMicroondas(3, new TimeSpan(0, 2, 0), "Sopa", "Instrução para função sopa.", '+', "Sopa", true));

            CarregarFuncoesCadastradas();
        }

        public void CadastrarFuncao(int potencia, TimeSpan tempo, string nome, string instrucao, char caractere, string alimento)
        {
            if (nome.Trim() == "")
            {
                if (!OnErro("O nome da função não informado"))
                    throw new NomeFuncaoNaoInformadaException("O nome da função não informado");
            }

            var funcao = new FuncaoMicroondas(potencia, tempo, nome, instrucao, caractere, alimento);
            funcao.Validar(alimento); // garante que está dentro das restrições.
            Funcoes.Add(funcao);

            SalvarFuncoesCadastradas();
        }

        public void CarregarFuncoesCadastradas()
        {
            var FS = ServiceLocator.Get<IFileService>();
            try
            {
                var json = FS.Carregar(FS.GetExePath("funcoes.json"));
                var list = JsonConvert.DeserializeObject<List<FuncaoMicroondas>>(json);

                foreach (var item in list)
                    funcoes.Add(item);
            }
            catch (FileNotFoundException)
            {
                //Não fazer nada 
            }
            catch (Exception e)
            {
                if (!OnErro(e.Message))
                    throw;
            }
        }

        public void SalvarFuncoesCadastradas()
        {
            var FS = ServiceLocator.Get<IFileService>();
            try
            {
                var funcoesUsuario = Funcoes.Where(f => !f.Predefinida);
                var json = JsonConvert.SerializeObject(funcoesUsuario);

                FS.Salvar(FS.GetExePath("funcoes.json"), json);
            }
            catch (Exception e)
            {
                if (!OnErro(e.Message))
                    throw;
            }
        }

        public void Iniciar(TimeSpan tempo, int potencia, string entrada)
        {
            try
            {
                var funcao = new FuncaoMicroondas(potencia, tempo);
                Iniciar(funcao, entrada);
            }
            catch (Exception e)
            {
                if (!OnErro(e.Message))
                    throw;
            }
        }

        public void Iniciar(FuncaoMicroondas funcao, string entrada)
        {
            try
            {
                funcao.Validar(entrada.Trim());

                FuncaoAtual = funcao;
                Cozido = entrada;
                TempoRestante = funcao.Tempo;

                Ligar();
            }
            catch (Exception e)
            {
                if (!OnErro(e.Message))
                    throw;
            }
        }

        public void InicioRapido(string entrada)
        {
            try
            {
                var tempo = new TimeSpan(0, 0, 30);
                Iniciar(tempo, 8, entrada);
            }
            catch (Exception e)
            {
                if (!OnErro(e.Message))
                    throw;
            }
        }

        async void Ligar()
        {
            try
            {
                var tick = new TimeSpan(0, 0, 1);
                while (TempoRestante.TotalSeconds > 0)
                {
                    await Aquecer(tick);
                    TempoRestante = TempoRestante.Subtract(tick);
                }
                OnConcluido();
            }
            catch (Exception e)
            {
                if (!OnErro(e.Message))
                    throw;
            }
        }

        async Task Aquecer(TimeSpan tempo)
        {
            await Task.Delay(tempo);            
            for (int i = 0; i < FuncaoAtual.Potencia; i++)
                Cozido += FuncaoAtual.Caractere;
        }
    }
}
