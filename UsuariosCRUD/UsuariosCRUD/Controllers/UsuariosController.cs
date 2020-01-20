using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UsuariosCRUD.Data;
using UsuariosCRUD.Models;

namespace UsuariosCRUD.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosContext _context;
        private readonly IConfiguration _configuration;

        public UsuariosController(UsuariosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/v1/Usuarios[?pageSize=3&pageIndex=10]
        [HttpGet, ProducesResponseType(typeof(PaginatedItems<Usuario>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsuarios([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var totalItems = await _context.Usuarios.LongCountAsync();
            var items = await _context.Usuarios.
                OrderBy(u => u.NomeUsuario)
                .ThenBy(u => u.UltimoNome)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
            var model = new PaginatedItems<Usuario>(pageIndex, pageSize, totalItems, items);
            return Ok(model);
        }

        // GET: api/v1/Usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/v1/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario([FromRoute] long id, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/Usuarios
        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/v1/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        public IActionResult PostSolicitarTrocaSenha([FromRoute] long usuarioId)
        {
            if (!UsuarioExists(usuarioId))
            {
                return NotFound();
            }

            _context.TrocaDeSenhas.Where(x => x.UsuarioId == usuarioId).ForEachAsync(x => x.SolicitacaoAtiva = false);

            var trocaSenha = new TrocaDeSenha
            {
                DataSolicitacao = DateTime.Now,
                SolicitacaoAtiva = true,
                UsuarioId = usuarioId
            };

            _context.TrocaDeSenhas.Add(trocaSenha);

            return Ok();
        }

        private bool UsuarioExists(long id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        [HttpPost("{usuarioId}/token")]
        public async Task<IActionResult> CriarToken(long usuarioId)
        {
            if (!UsuarioExists(usuarioId))
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            var token = BuildToken(usuario);
            _context.TokenUsuarios.Add(token);
            return Ok(token.Token);
        }

        private TokenUsuario BuildToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var dataExpiracao = DateTime.UtcNow.AddHours(1);
            var token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: dataExpiracao,
               signingCredentials: credenciais);
            return new TokenUsuario()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiraEm = dataExpiracao
            };
        }
    }
}