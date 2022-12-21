using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using {{cookiecutter.ProjectName}}.Api;
using {{cookiecutter.ProjectName}}.Framework.Api.Filter;
using {{cookiecutter.ProjectName}}.Persistence;
using {{cookiecutter.ProjectName}}.Application;

namespace {{cookiecutter.ProjectName}}.ComponentTests.SetUp
{
    /// <summary>
    /// We can override default behaviour of actual api start up class in case we need to set up something custom 
    /// </summary>
    public class TestServerStartup : Startup, IDisposable
    {
        public TestServerStartup(IConfiguration configuration, IWebHostEnvironment env)
            : base(configuration, env)
        {
        }

        protected override void AddApiAuthentication(IServiceCollection services)
        {
        }

        protected override void AddDbContexts(IServiceCollection services)
        {
            services.AddDbContext<{{cookiecutter.ProjectName}}DbContext>(
                options => options.UseInMemoryDatabase("{{cookiecutter.ProjectName}}Db"));
        }

        protected override void AddMvcServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                {
                    options.Filters.Add(typeof({{cookiecutter.ProjectName}}ExceptionFilterAttribute));
                    options.Filters.Add<ModelValidationFilter>();
                })
                .AddApplicationPart(typeof(Startup).Assembly);
        }

        public void Dispose()
        {
        }
    }
}