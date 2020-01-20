using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using UsuariosCRUD;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;

namespace UsuariosCRUDTeste
{
    public class UsuariosApi
    {
        private readonly HttpClient _client;

        public UsuariosApi()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }
    }
}
