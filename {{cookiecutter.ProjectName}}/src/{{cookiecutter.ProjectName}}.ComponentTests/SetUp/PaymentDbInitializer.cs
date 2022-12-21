using System.Collections.Generic;
using {{cookiecutter.ProjectName}}.Persistence;
using {{cookiecutter.ProjectName}}.Persistence.DAO;

namespace {{cookiecutter.ProjectName}}.ComponentTests.SetUp
{
    /// <summary>
    /// Add any initial data set-up required for test suite. It will runs only one during all tests runs
    /// </summary>
    public class {{cookiecutter.ProjectName}}DbInitializer
    {
        private readonly {{cookiecutter.ProjectName}}DbContext _context;

        public {{cookiecutter.ProjectName}}DbInitializer({{cookiecutter.ProjectName}}DbContext context) => 
            _context = context;

        public void Seed()
        {
            //SeedBanks(_context);
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