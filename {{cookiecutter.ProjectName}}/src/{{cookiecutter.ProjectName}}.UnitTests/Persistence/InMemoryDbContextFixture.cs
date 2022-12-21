using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using {{cookiecutter.ProjectName}}.Persistence;

namespace {{cookiecutter.ProjectName}}.UnitTests.Persistence
{
    public class InMemoryDbContextFixture : IDisposable
    {
        public IMapper Mapper { get; }
        public {{cookiecutter.ProjectName}}DbContext Context { get; }

        public InMemoryDbContextFixture()
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddMaps("{{cookiecutter.ProjectName}}.Persistence"); });

            Mapper = mappingConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<{{cookiecutter.ProjectName}}DbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            Context = new {{cookiecutter.ProjectName}}DbContext(options);
            Context.Database.EnsureCreated();
            {{cookiecutter.ProjectName}}DbInitializer.Initialize(Context);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                Context.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}