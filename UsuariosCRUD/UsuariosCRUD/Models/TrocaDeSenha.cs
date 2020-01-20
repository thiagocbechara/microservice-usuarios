using System;

namespace UsuariosCRUD.Models
{
    public class TrocaDeSenha
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public bool SolicitacaoAtiva { get; set; }

        public Usuario Usuario { get; set; }
    }
}