namespace Fluencia.Servicos.Interfaces
{
    public interface IExtracaoService
    {
        Task<int> ExtrairQuestoes(Stream stream, string nomeArquivo);
    }
}