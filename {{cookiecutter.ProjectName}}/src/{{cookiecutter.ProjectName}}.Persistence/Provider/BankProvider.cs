using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using {{cookiecutter.ProjectName}}.Persistence.DAO;
using {{cookiecutter.ProjectName}}.Persistence.Provider.Contract;

namespace {{cookiecutter.ProjectName}}.Persistence.Provider
{
    /// <summary>
    /// Provider returns DAO and will be mainly used by queries handlers (read operations)
    /// These classes will wrap abstraction over DB context
    /// </summary>
    public class BankProvider : IBankProvider
    {
        private readonly {{cookiecutter.ProjectName}}DbContext _dbContext;

        public BankProvider({{cookiecutter.ProjectName}}DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Bank> Bank(int bankId) =>
            await _dbContext.Banks.FirstOrDefaultAsync(x => x.Id == bankId);

        public async Task<Bank> Bank(string code) =>
            await _dbContext.Banks.FirstOrDefaultAsync(x => x.IfscCode == code);

        public async Task<List<Bank>> Banks() =>
            await _dbContext.Banks.ToListAsync();
    }
}