using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classes.Microondas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLocator.Services;
using ServicesLocator.Locator;
using ServicesLocator.Interfaces;

namespace Classes.Microondas.Tests
{
    // Metodo Cenario Comportamento experado
    [TestClass()]
    public class MicroondasTests
    {
        [TestMethod()]
        public void CadastrarFuncao_FuncaoCadastradaSemArquivo_FuncaoEncontrada()
        {
            var microondas = new Microondas();

            var potencia = 3;
            var tempo = new TimeSpan(0, 0, 12);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "alimento";

            microondas.CadastrarFuncao(potencia, tempo, nome, instrucao, caractere, alimento, false);

            var funcao = microondas.Funcoes.FirstOrDefault(f =>
                f.Nome == nome && f.Instrucao == instrucao &&
                f.Potencia == potencia && TimeSpan.Equals(f.Tempo, tempo) &&
                f.Caractere == caractere && f.Alimento == alimento);

            Assert.IsNotNull(funcao);
        }

        [TestMethod()]
        public async Task Iniciar_SemFuncao_EntradaAquecidaAsync()
        {
            var microondas = new Microondas();

            var tempo = new TimeSpan(0, 0, 2);
            var potencia = 2;
            var entrada = "teste";

            await microondas.Iniciar(tempo, potencia, entrada);

            Assert.AreEqual(microondas.EntradaAquecida, entrada + "....");
        }

        [TestMethod()]
        public async Task Iniciar_ComFuncao_EntradaAquecidaAsync()
        {
            var microondas = new Microondas();

            var tempo = new TimeSpan(0, 0, 2);
            var potencia = 3;
            var entrada = "teste";

            var funcao = new FuncaoMicroondas(potencia, tempo);

            await microondas.Iniciar(funcao, entrada);

            Assert.AreEqual(microondas.EntradaAquecida, entrada + "......");
        }

        [TestMethod()]
        public async Task InicioRapido_EntradaAquecidaAsync()
        {
            var microondas = new Microondas();
            var entrada = "teste";

            await microondas.InicioRapido(entrada);

            Assert.AreEqual(microondas.EntradaAquecida, entrada +
                "................................................" +
                "................................................" +
                "................................................" +
                "................");
        }

        [TestMethod()]
        public async Task CancelarInicioRapido_Cancelado_EntradaAquecidaParcialmenteAsync()
        {
            var microondas = new Microondas();
            var entrada = "teste";

            var inicioRapidoTask = microondas.InicioRapido(entrada);

            await Task.Delay(2100);

            microondas.Cancelar();
            await inicioRapidoTask;

            Assert.AreEqual(microondas.EntradaAquecida, entrada +
                "................");
        }

        [TestMethod()]
        public async Task ContinuarInicioRapido_Pausado_EntradaAquecidaParcialmenteAsync()
        {
            var microondas = new Microondas();
            var entrada = "teste";

            var inicioRapidoTask = microondas.InicioRapido(entrada);

            await Task.Delay(2100);

            microondas.Pausar();

            await inicioRapidoTask;

            await Task.Delay(2100);

            await microondas.Continuar();

            Assert.AreEqual(microondas.EntradaAquecida, entrada +
                "................................................" +
                "................................................" +
                "................................................" +
                "................");
        }

        [TestMethod()]
        public void SalvarCarregarFuncoesLocalmente_UmaFuncao_FuncaoCarregada()
        {
            try
            {
                new FileService().RegisterService();
            }
            catch (ServicoJaRegistradoException) { }

            var FS = ServiceLocator.Get<IFileService>();
            FS.Deletar(FS.GetExePath("funcoes.json"));

            var microondas = new Microondas();

            var potencia = 3;
            var tempo = new TimeSpan(0, 0, 12);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "alimento";

            microondas.CadastrarFuncao(potencia, tempo, nome, instrucao, caractere, alimento);

            microondas.SalvarFuncoesCadastradas();

            microondas.CarregarFuncoesCadastradas();

            var funcao = microondas.Funcoes.FirstOrDefault(f =>
                f.Nome == nome && f.Instrucao == instrucao &&
                f.Potencia == potencia && TimeSpan.Equals(f.Tempo, tempo) &&
                f.Caractere == caractere && f.Alimento == alimento);

            Assert.IsNotNull(funcao);
        }

        [TestMethod()]
        public async Task Iniciar_SemFuncaoPorArquivo_EntradaAquecida()
        {
            try {
                new FileService().RegisterService();
            }
            catch (ServicoJaRegistradoException) { }

            var FS = ServiceLocator.Get<IFileService>();

            var microondas = new Microondas();

            var tempo = new TimeSpan(0, 0, 2);
            var potencia = 2;
            var entrada = "teste";

            FS.Salvar(FS.GetExePath("arquivo.txt"), entrada);

            await microondas.Iniciar(tempo, potencia, FS.GetExePath("arquivo.txt"));

            var arqValor = FS.Carregar(FS.GetExePath("arquivo.txt"));

            Assert.AreEqual(arqValor, entrada + "....");
        }
    }
}