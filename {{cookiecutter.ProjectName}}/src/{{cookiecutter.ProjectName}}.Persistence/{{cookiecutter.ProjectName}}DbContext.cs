using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using {{cookiecutter.ProjectName}}.Persistence.DAO;

namespace {{cookiecutter.ProjectName}}.Persistence
{
    [ExcludeFromCodeCoverage]
    public class {{cookiecutter.ProjectName}}DbContext : DbContext
    {
        public {{cookiecutter.ProjectName}}DbContext(DbContextOptions<{{cookiecutter.ProjectName}}DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Bank> Banks { get; set; }
    }
}