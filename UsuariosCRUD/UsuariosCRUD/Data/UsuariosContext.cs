using Microsoft.EntityFrameworkCore;
using UsuariosCRUD.Models;

namespace UsuariosCRUD.Data
{
    public class UsuariosContext : DbContext
    {
        public UsuariosContext(DbContextOptions<UsuariosContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TrocaDeSenha> TrocaDeSenhas { get; set; }
        public DbSet<TokenUsuario> TokenUsuarios { get; set; }
    }
}