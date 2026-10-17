namespace Fluencia.Servicos
{
    public interface IAiService
    {
        Task<string> GetAiResponseAsync(string prompt);
    }
}