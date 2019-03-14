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
    [TestClass()]
    public class FuncaoMicroondasTests
    {
        [TestMethod()]
        public void ValidarEntrada_SemArquivo_Sucesso()
        {
            var potencia = 3;
            var tempo = new TimeSpan(0, 0, 12);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "porco";

            var funcao = new FuncaoMicroondas(potencia, tempo, nome, instrucao, caractere, alimento);

            var exceptionGerado = false;
            try
            {
                funcao.ValidarEntrada(alimento, false);
            }
            catch (Exception)
            {
                exceptionGerado = true;
            }

            Assert.IsFalse(exceptionGerado);
        }

        [TestMethod()]
        public void ValidarEntrada_SemArquivo_Falha()
        {
            var potencia = 3;
            var tempo = new TimeSpan(0, 0, 12);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "porco";

            var funcao = new FuncaoMicroondas(potencia, tempo, nome, instrucao, caractere, alimento);

            var exceptionGerado = false;
            try
            {
                funcao.ValidarEntrada("123", false);
            }
            catch (Exception)
            {
                exceptionGerado = true;
            }

            Assert.IsTrue(exceptionGerado);
        }

        [TestMethod()]
        public void ValidarEntrada_ComArquivo_Sucesso()
        {
            try
            {
                new FileService().RegisterService();
            }
            catch (ServicoJaRegistradoException) { }
            var FS = ServiceLocator.Get<IFileService>();

            var potencia = 3;
            var tempo = new TimeSpan(0, 0, 12);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "porco";

            FS.Salvar(FS.GetExePath("arquivo.txt"), alimento);

            var funcao = new FuncaoMicroondas(potencia, tempo, nome, instrucao, caractere, alimento);

            var arquivoAlimento = FS.Carregar(FS.GetExePath("arquivo.txt"));

            var exceptionGerado = false;
            try
            {
                funcao.ValidarEntrada(arquivoAlimento);
            }
            catch (Exception)
            {
                exceptionGerado = true;
            }

            Assert.IsFalse(exceptionGerado);
        }

        [TestMethod()]
        public void ValidarPotencia_IgualZero_GerarException()
        {
            var potencia = 0;
            var tempo = new TimeSpan(0, 0, 12);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "porco";

            var funcao = new FuncaoMicroondas(potencia, tempo, nome, instrucao, caractere, alimento);

            var exceptionGerado = false;
            try
            {
                funcao.Validar();
            }
            catch (Exception)
            {
                exceptionGerado = true;
            }

            Assert.IsTrue(exceptionGerado);
        }

        [TestMethod()]
        public void ValidarPotencia_MaiorQueDez_GerarException()
        {
            var potencia = 11;
            var tempo = new TimeSpan(0, 0, 12);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "porco";

            var funcao = new FuncaoMicroondas(potencia, tempo, nome, instrucao, caractere, alimento);

            var exceptionGerado = false;
            try
            {
                funcao.Validar();
            }
            catch (Exception)
            {
                exceptionGerado = true;
            }

            Assert.IsTrue(exceptionGerado);
        }

        [TestMethod()]
        public void ValidarTempo_MenorQueUm_GerarException()
        {
            var potencia = 3;
            var tempo = new TimeSpan(0, 0, 0);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "porco";

            var funcao = new FuncaoMicroondas(potencia, tempo, nome, instrucao, caractere, alimento);

            var exceptionGerado = false;
            try
            {
                funcao.Validar();
            }
            catch (Exception)
            {
                exceptionGerado = true;
            }

            Assert.IsTrue(exceptionGerado);
        }

        [TestMethod()]
        public void ValidarTempo_MaiorQueDoisMinutos_GerarException()
        {
            var potencia = 3;
            var tempo = new TimeSpan(0, 2, 1);
            var nome = "teste";
            var instrucao = "instrucao test";
            var caractere = '@';
            var alimento = "porco";

            var funcao = new FuncaoMicroondas(potencia, tempo, nome, instrucao, caractere, alimento);

            var exceptionGerado = false;
            try
            {
                funcao.Validar();
            }
            catch (Exception)
            {
                exceptionGerado = true;
            }

            Assert.IsTrue(exceptionGerado);
        }
    }
}