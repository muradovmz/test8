using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace {{cookiecutter.ProjectName}}.Application.Tools
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMediatR(Assembly.GetExecutingAssembly())
                           .AddHttpContextAccessor();
        }
    }
}

