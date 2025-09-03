using System.Net.Http.Headers;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Ambev.DeveloperEvaluation.Functional.Api.Factories;

public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(s =>
        {
            // troca o DbContext por SQLite in-memory
            var descriptor = s.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (descriptor != null) s.Remove(descriptor);

            s.AddDbContext<DefaultContext>(opt =>
            {
                var conn = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
                conn.Open(); // manter viva
                opt.UseSqlite(conn);
            });

            // seed opcional, se quiser
            var sp = s.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            ctx.Database.EnsureCreated();
        });
    }

    public HttpClient CreateClientWithRole(string role, string? userId = null)
    {
        var client = CreateClient();

        // gere token com a mesma secret da appsettings de teste
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string,string?>
            {
                ["Jwt:SecretKey"] = "your-long-secret-key"
            }).Build();

        var jwt = new JwtTokenGenerator(config).GenerateToken(new TestUser(userId ?? Guid.NewGuid().ToString(), "tester", role));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return client;
    }

    private record TestUser(string Id, string Username, string Role) : IUser;
}