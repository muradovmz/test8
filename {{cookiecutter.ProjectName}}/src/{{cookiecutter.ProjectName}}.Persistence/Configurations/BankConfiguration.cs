
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using {{cookiecutter.ProjectName}}.Persistence.DAO;
using System.Reflection.Emit;

namespace {{cookiecutter.ProjectName}}.Persistence.Configurations
{
    public class BankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.HasIndex(b => b.IfscCode).IsUnique();
        }
    }
}
