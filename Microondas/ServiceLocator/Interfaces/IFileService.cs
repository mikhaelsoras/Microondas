namespace ServiceLocator.Interfaces
{
    public interface IFileService : IServiceLocator
    {
        string GetExePath(string nomeArquivo);
        void Salvar(string caminho, string conteudo);
        string Carregar(string caminho);
    }
}
