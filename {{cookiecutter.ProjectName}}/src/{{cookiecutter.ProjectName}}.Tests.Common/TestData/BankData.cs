using Bogus;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Command;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;

namespace {{cookiecutter.ProjectName}}.Tests.Common.TestData
{
    public static class BankData
    {
        public static Faker<BankResponse> BankResponseFaker { get; } =
            new Faker<BankResponse>()
                .RuleFor(x => x.Id, 1)
                .RuleFor(x => x.Name, f => f.Name.FindName())
                .RuleFor(x => x.IfscCode, f => f.Random.String(6, 6));

        public static Faker<CreateBankCommand> BankCommandFaker { get; } =
            new Faker<CreateBankCommand>()
                .RuleFor(x => x.Name, f => f.Name.FindName())
                .RuleFor(x => x.IfscCode, f => f.Random.String(6, 6));


        public static Faker<{{cookiecutter.ProjectName}}.Persistence.DAO.Bank> BankDaoFaker { get; } =
            new Faker<{{cookiecutter.ProjectName}}.Persistence.DAO.Bank>()
                .RuleForType(typeof(long), f => f.Random.Long())
                .RuleForType(typeof(string), f => f.Random.String())
                .RuleForType(typeof(int), f => f.Random.Int());
    }
}