using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace Library.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly PostgreSqlContainer _container;

        public CustomWebApplicationFactory(PostgreSqlContainer container)
        {
            _container = container;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var connectionString = _container.GetConnectionString();
                var settings = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = connectionString
                };
                configBuilder.AddInMemoryCollection(settings);
            });
        }
    }
}
