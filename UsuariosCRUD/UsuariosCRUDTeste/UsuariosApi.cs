using Xunit;
using UsuariosCRUD;
using System.Net.Http;
using UsuariosCRUDTeste.Startups_Configs;
using System.Threading.Tasks;
using System.Net;

namespace UsuariosCRUDTeste
{
    public class UsuariosApi : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public UsuariosApi(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetUsuarios()
        {
            var response = await _client.GetAsync("api/v1/Usuarios");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
