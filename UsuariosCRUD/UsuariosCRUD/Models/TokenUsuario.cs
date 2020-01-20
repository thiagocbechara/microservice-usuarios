using System;

namespace UsuariosCRUD.Models
{
    public class TokenUsuario
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
    }
}