using MediatR;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;
using {{cookiecutter.ProjectName}}.Domain.Repository;

namespace {{cookiecutter.ProjectName}}.Application.Features.Bank.Command
{
    /// <summary>
    /// Handlers can be treated as broken thick application service into smaller specific functional classes
    /// </summary>
    public class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, BankCreatedResponse>
    {
        private readonly IBankRepository _bankRepository;

        public CreateBankCommandHandler(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<BankCreatedResponse> Handle(CreateBankCommand command, CancellationToken cancellationToken)
        {
            var bank = new Domain.Core.Bank.Builder()
                .WithIfscCode(command.IfscCode)
                .WithName(command.Name)
                .Build();
            var bankId = await _bankRepository.Create(bank);
            return new BankCreatedResponse { BankId = bankId };
        }
    }
}