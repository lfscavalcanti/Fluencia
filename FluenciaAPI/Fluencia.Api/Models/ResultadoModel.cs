namespace Fluencia.Api.Models
{
    public class SalvarResultadoModel
    {
        public int AlunoId { get; set; }
        public string NivelIdentificado { get; set; } = string.Empty;
        public string TopicosDominados { get; set; } = string.Empty;
        public string TopicosPendentes { get; set; } = string.Empty;
        public string PlanoEstudos { get; set; } = string.Empty;
    }
}