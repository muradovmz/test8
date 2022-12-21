using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using {{cookiecutter.ProjectName}}.Api.Extension;
using {{cookiecutter.ProjectName}}.Persistence;
using Serilog;

namespace {{cookiecutter.ProjectName}}.Api
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static int Main(string[] args)
        {
            // This logger added to get logs during start up only to log any error during start up
            // Later logger configured by AddApiLogger() method will be used in the app 
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
            Log.Information("{{cookiecutter.ProjectName}} API Starting...");

            try
            {
                CreateHostBuilder(args).Build()
                    .MigrateDatabase()
                    .Run();
                Log.Information("{{cookiecutter.ProjectName}} API Stopped...");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occured during {{cookiecutter.ProjectName}} API bootstrapping....");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddApiConfigServer(config.Build());
                })
                .AddApiLogger()
                .ConfigureWebHostDefaults(builder => { builder.UseStartup<Startup>(); });
        
    }
}