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
        public Action Cancelado;
        public Action<string> Erro;
        public Action<bool> PausarChanged;

        private void OnTempoRestanteChanged()
        {
            TempoRestanteChanged?.Invoke(this);
        }

        private void OnConcluido(string txt)
        {
            Concluido?.Invoke(txt);
        }
        private void OnPauseChanged(bool isPaused)
        {
            IsPausado = isPaused;
            pedidoPausa = false;
            PausarChanged?.Invoke(IsPausado);
        }
        private void OnCancelado()
        {
            pedidoPausa = false;
            pedidoCancelar = false;
            TempoRestante = TimeSpan.Zero;
            FuncaoAtual = null;
            contadorSegundos = TimeSpan.Zero;

            Cancelado?.Invoke();
        }
        //Retorna true caso o evento exista
        private bool OnErro(string msg)
        {  
            Erro?.Invoke(msg);
            return Erro != null;
        }
        #endregion

        private bool pedidoPausa, pedidoCancelar;
        // vai receber quanto tempo foi acumulado antes de aquecer a entrada
        private TimeSpan contadorSegundos;

        #region Props

        //verifica se deve pausar após cada segundo

        public bool IsPausado { get; private set; }

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

        public FuncaoMicroondas FuncaoAtual;

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

        public string EntradaAquecida { get; private set; }
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

                EntradaAquecida = entrada;
                TempoRestante = FuncaoAtual.Tempo;
                contadorSegundos = TimeSpan.Zero;

                Ligar(entrada);
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

        async void Ligar(string entrada)
        {
            try
            {
                var FS = ServiceLocator.Get<IFileService>();
                var sec = new TimeSpan(0, 0, 1);
                var tick = new TimeSpan(sec.Ticks / 10); //tempo de espera entre cada atualização

                OnPauseChanged(false);

                if (FS.FileExists(entrada))
                    await Aquecer(entrada, FS, sec, tick);
                else
                    await Aquecer(sec, tick);

                if (!pedidoPausa && !pedidoCancelar)
                    OnConcluido(EntradaAquecida);
                else
                {
                    if (pedidoCancelar)
                        OnCancelado();
                    else if (pedidoPausa)
                        OnPauseChanged(true);
                }
            }
            catch (Exception e)
            {
                OnPauseChanged(false);
                if (!OnErro(e.Message))
                    throw;
            }
        }

        private async Task Aquecer(TimeSpan sec, TimeSpan tick)
        {
            while (TempoRestante.TotalSeconds > 0 && !pedidoPausa && !pedidoCancelar)
            {
                await Task.Delay(tick);
                contadorSegundos = contadorSegundos.Add(tick);

                if (contadorSegundos.TotalMilliseconds >= sec.TotalMilliseconds)
                {
                    Aquecer();
                    contadorSegundos = contadorSegundos.Subtract(sec);
                }

                TempoRestante = TempoRestante.Subtract(tick);
            }
        }

        private async Task Aquecer(string caminho, IFileService FS, TimeSpan sec, TimeSpan tick)
        {
            using (StreamWriter sw = FS.GetStreamWriter(caminho, true))
            {
                while (TempoRestante.TotalSeconds > 0 && !pedidoPausa && !pedidoCancelar)
                {
                    await Task.Delay(tick);
                    contadorSegundos = contadorSegundos.Add(tick);

                    if (contadorSegundos.TotalMilliseconds >= sec.TotalMilliseconds)
                    {
                        Aquecer(sw);
                        contadorSegundos = contadorSegundos.Subtract(sec);
                    }

                    TempoRestante = TempoRestante.Subtract(tick);
                }
            }
            EntradaAquecida = FS.Carregar(caminho);
        }

        public void Continuar()
        {
            if (IsPausado && tempoRestante.TotalSeconds > 0)
                Ligar(EntradaAquecida);
        }

        public void Pausar()
        {
            if (!IsPausado && tempoRestante.TotalSeconds > 0)
                pedidoPausa = true;
        }

        public void Cancelar()
        {
            if (tempoRestante.TotalSeconds > 0)
            {
                pedidoCancelar = true;
            }
        }

        private void Aquecer(StreamWriter sw = null)
        {
            var caracteres = "";
            for (int i = 0; i < FuncaoAtual.Potencia; i++)
                caracteres += FuncaoAtual.Caractere;

            if (sw == null)
                EntradaAquecida += caracteres;
            else
                sw.Write(caracteres);
        }
    }
}
