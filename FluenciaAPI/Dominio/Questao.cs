namespace FluenciaAPI.Dominio
{
    public class Questao
    {
        public int Id { get; set; }
        public string Nivel { get; set; }
        public string Topico { get; set; }
        public string Enunciado { get; set; }
        public string OpcaoA { get; set; }
        public string OpcaoB { get; set; }
        public string OpcaoC { get; set; }
        public string OpcaoD { get; set; }
        public string OpcaoE { get; set; } = "Não sei responder";
        public char RespostaCorreta { get; set; }
    }
}