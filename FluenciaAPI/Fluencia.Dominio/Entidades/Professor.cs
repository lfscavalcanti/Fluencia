using System;

namespace Fluencia.Dominio.Entidades
{
    public class Professor
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public bool PrimeiroAcesso { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}