using System.IO;

namespace ServicesLocator.Interfaces
{
    public interface IFileService : IServiceLocator
    {
        string GetExePath(string nomeArquivo);
        void Salvar(string caminho, string conteudo);
        string Carregar(string caminho);
        StreamWriter GetStreamWriter(string caminho, bool append = false);
        bool FileExists(string caminho);
    }
}
