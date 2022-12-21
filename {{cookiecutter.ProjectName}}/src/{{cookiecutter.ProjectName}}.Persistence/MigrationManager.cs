using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
namespace {{cookiecutter.ProjectName}}.Persistence
{
    [ExcludeFromCodeCoverage]
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var {{cookiecutter.ProjectName}}Context = scope.ServiceProvider.GetRequiredService<{{cookiecutter.ProjectName}}DbContext>();
            var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<{{cookiecutter.ProjectName}}DbContext>>();
            try
            {
                logger.LogInformation("DB Migration Started");
                {{cookiecutter.ProjectName}}Context.Database.Migrate();
                logger.LogInformation("DB Migration Completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }

            return host;
        }
    }
}