using System.Collections.Generic;
using {{cookiecutter.ProjectName}}.Persistence;
using {{cookiecutter.ProjectName}}.Persistence.DAO;
using {{cookiecutter.ProjectName}}.Tests.Common.TestData;

namespace {{cookiecutter.ProjectName}}.UnitTests.Persistence
{
    public class {{cookiecutter.ProjectName}}DbInitializer
    {
        public static void Initialize({{cookiecutter.ProjectName}}DbContext context)
        {
            SeedBanks(context);
        }

        private static void SeedBanks({{cookiecutter.ProjectName}}DbContext context)
        {
            var data = new List<Bank>
            {
                new() { Name = "Bank1", Id = 1, IfscCode = "IfscCode-a" },
                new() { Name = "Bank2", Id = 2, IfscCode = "IfscCode-b" },
                new() { Name = "Bank3", Id = 3, IfscCode = "IfscCode-c" },
            };
            context.AddRange(data);
            context.SaveChanges();
        }
    }   
}