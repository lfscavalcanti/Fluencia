namespace Fluencia.Api.Models
{
    public class CadastrarProfessorModel
    {
        public string Nome { get; set; } = string.Empty;
    }

    public class LoginProfessorModel
    {
        public string Codigo { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class AlterarSenhaModel
    {
        public int Id { get; set; }
        public string NovaSenha { get; set; } = string.Empty;
    }
}