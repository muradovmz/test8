using MediatR;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;

namespace {{cookiecutter.ProjectName}}.Application.Features.Bank.Query
{
    public class BankByIdQuery : IRequest<BankResponse>
    {
        public int BankId { get; set; }
    }
}