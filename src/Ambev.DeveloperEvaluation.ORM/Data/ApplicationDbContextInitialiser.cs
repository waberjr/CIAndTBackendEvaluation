using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Data;

/// <summary>
/// Used for initialise the database operations. Here we can migrate changes and seed data if its necessary
/// </summary>
public class ApplicationDbContextInitialiser(
    DefaultContext context,
    ILogger<ApplicationDbContextInitialiser> logger,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
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

    public async Task SeedAsync()
    {
        try
        {
            logger.LogInformation("Seeding database...");

            if (!await context.Database.CanConnectAsync())
            {
                logger.LogWarning("Database is not available. Skipping migration.");
                return;
            }

            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            // throw;
        }
    }

    private async Task TrySeedAsync()
    {
        await SeedDefaultUsers();
    }

    private async Task SeedDefaultUsers()
    {
        logger.LogInformation("Seeding admin user...");

        // todo: get from config
        var customerUser = new User
        {
            Email = "customer@test.com",
            Username = "Customer",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        var managerUser = new User
        {
            Email = "manager@test.com",
            Username = "Manager",
            Status = UserStatus.Active,
            Role = UserRole.Manager
        };

        var adminUser = new User
        {
            Email = "admin@test.com",
            Username = "Administrator",
            Status = UserStatus.Active,
            Role = UserRole.Admin
        };

        List<User> usersToAdd = [customerUser, managerUser, adminUser];

        foreach (var user in usersToAdd)
        {
            var existingUser = await userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                logger.LogInformation("User with email {Email} already exists. Skipping creation.", user.Email);
                continue;
            }

            user.Password = passwordHasher.HashPassword("Password@123");

            await userRepository.CreateAsync(user);
        }
    }
}