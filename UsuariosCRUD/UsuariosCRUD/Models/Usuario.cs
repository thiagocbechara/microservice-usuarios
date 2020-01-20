namespace UsuariosCRUD.Models
{
    public class Usuario
    {
        public long Id { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string Email { get; set; }
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
    }
}