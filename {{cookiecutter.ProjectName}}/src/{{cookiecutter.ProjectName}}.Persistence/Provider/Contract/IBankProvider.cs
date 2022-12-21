using System.Collections.Generic;
using System.Threading.Tasks;
using {{cookiecutter.ProjectName}}.Persistence.DAO;

namespace {{cookiecutter.ProjectName}}.Persistence.Provider.Contract
{
    public interface IBankProvider
    {
        Task<Bank> Bank(int bankId);
        Task<Bank> Bank(string code);
        Task<List<Bank>> Banks();
    }
}