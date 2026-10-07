namespace FluenciaAPI.Dominio
{
    public class Resposta
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int QuestaoId { get; set; }
        public char RespostaData { get; set; }
        public bool NãoSei { get; set; }
        public bool Acertou { get; set; }

        public Questao Questao { get; set; }
    }
}