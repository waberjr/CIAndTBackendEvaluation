using System.Data.Common;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Infrastructure.Data;

public class SqliteDbFixture : IAsyncLifetime
{
    public DbConnection Connection { get; private set; } = default!;
    public DefaultContext NewContext()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseSqlite(Connection)
            .Options;

        return new DefaultContext(options);
    }

    public async Task InitializeAsync()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        await Connection.OpenAsync();

        using var ctx = NewContext();
        await ctx.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync() => Connection.DisposeAsync().AsTask();
}