using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Data;

/// <summary>
/// Used for initialise the database operations. Here we can migrate changes and seed data if its necessary
/// </summary>
public class ApplicationDbContextInitialiser(DefaultContext context, ILogger<ApplicationDbContextInitialiser> logger)
{
    /// <summary>
    /// Migrate database
    /// </summary>
    public async Task MigrateAsync()
    {
        try
        {
            logger.LogInformation("Migrating database...");
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
        }
    }
}