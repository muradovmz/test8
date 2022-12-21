using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;
using {{cookiecutter.ProjectName}}.Framework.Shared.Exception;
using {{cookiecutter.ProjectName}}.Framework.Shared.Extension;
using {{cookiecutter.ProjectName}}.Persistence.Provider.Contract;

namespace {{cookiecutter.ProjectName}}.Application.Features.Bank.Query
{
    public class BankByIdQueryHandler : IRequestHandler<BankByIdQuery, BankResponse>
    {
        private readonly IBankProvider _bankProvider;
        private readonly IMapper _mapper;

        public BankByIdQueryHandler(IBankProvider bankProvider, IMapper mapper)
        {
            _bankProvider = bankProvider;
            _mapper = mapper;
        }

        public async Task<BankResponse> Handle(BankByIdQuery query, CancellationToken cancellationToken)
        {
            var bank = await _bankProvider.Bank(query.BankId);
            bank.EnsureNotNull<NotFoundException>($"Bank with {query.BankId} not exist.");
            return _mapper.Map<BankResponse>(bank);
        }
    }
}