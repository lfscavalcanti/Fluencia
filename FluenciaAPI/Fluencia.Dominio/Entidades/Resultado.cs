namespace FluenciaAPI.Dominio
{
    public class Resultado
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public string NivelIdentificado { get; set; }
        public string TopicosDominados { get; set; }
        public string TopicosPendentes { get; set; }
        public string PlanoEstudos { get; set; }
        public DateTime DataAvaliacao { get; set; }
    }
}