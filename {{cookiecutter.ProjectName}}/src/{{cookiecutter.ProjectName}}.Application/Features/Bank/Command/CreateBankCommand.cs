using MediatR;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;

namespace {{cookiecutter.ProjectName}}.Application.Features.Bank.Command
{
    public class CreateBankCommand : IRequest<BankCreatedResponse>
    {
        public string IfscCode { get; set; }
        public string Name { get; set; }
    }
}