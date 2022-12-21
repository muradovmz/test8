using FluentValidation;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Command;
using {{cookiecutter.ProjectName}}.Persistence.Provider.Contract;

namespace {{cookiecutter.ProjectName}}.Application.Features.Bank.Validator
{
    public class CreateBankCommandValidator : AbstractValidator<CreateBankCommand>
    {
        public CreateBankCommandValidator(IBankProvider bankProvider)
        {
            RuleFor(x => x.IfscCode)
                .NotEmpty()
                .Must(s => bankProvider.Bank(s).Result == null).WithMessage("Bank details already exists");
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}