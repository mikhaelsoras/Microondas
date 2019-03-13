using ServiceLocator.Interfaces;
using System;
using System.IO;

namespace ServiceLocator.Services
{
    public class FileService : IFileService
    {
        public string GetExePath(string nomeArquivo)
        {
            var caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);
            return caminho;
        }

        public string Carregar(string caminho)
        {
            using (StreamReader arquivo = new StreamReader(caminho))
            {
                return arquivo.ReadToEnd();
            }
        }

        public void RegisterService()
        {
            MicroondasProject.ServiceLocator.Set<IFileService>(this);
        }

        public void Salvar(string caminho, string conteudo)
        {
            using (StreamWriter arquivo = new StreamWriter(caminho))
            {
                arquivo.Write(conteudo);
            }
        }
    }
}
