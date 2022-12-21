using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using {{cookiecutter.ProjectName}}.Application.Tools;
using {{cookiecutter.ProjectName}}.Domain.Repository;
using {{cookiecutter.ProjectName}}.Persistence.Provider;
using {{cookiecutter.ProjectName}}.Persistence.Provider.Contract;
using {{cookiecutter.ProjectName}}.Persistence.Repository;


namespace {{cookiecutter.ProjectName}}.Api.Extension
{
    [ExcludeFromCodeCoverage]
    internal static class ApiDependencyExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDomainDependency(configuration)
                           .AddPersistenceDependency(configuration)
                           .AddApplication(configuration);
        }

        private static IServiceCollection AddDomainDependency(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        private static IServiceCollection AddPersistenceDependency(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<IBankRepository, BankRepository>()
                .AddScoped<IBankProvider, BankProvider>();
        }
    }
}