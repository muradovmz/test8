using System.Linq;
using FluentAssertions;
using Moq;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Validator;
using DAO = {{cookiecutter.ProjectName}}.Persistence.DAO;
using {{cookiecutter.ProjectName}}.Persistence.Provider.Contract;
using {{cookiecutter.ProjectName}}.Tests.Common.TestData;
using Xunit;

namespace {{cookiecutter.ProjectName}}.UnitTests.Application.Features.Bank.Validator
{
    public class CreateBankCommandValidatorTests
    {
        private readonly CreateBankCommandValidator _validator;
        private readonly Mock<IBankProvider> _bankProvider;

        public CreateBankCommandValidatorTests()
        {
            _bankProvider = new Mock<IBankProvider>();
            _bankProvider.Setup(x => x.Bank(It.IsAny<string>())).ReturnsAsync((DAO.Bank)null);
            _validator = new CreateBankCommandValidator(_bankProvider.Object);
        }

        [Fact]
        public void ShouldFailValidationIfCommandHasInvalidData()
        {
            var command = BankData.BankCommandFaker.Generate();
            command.Name = "";
            _validator.Validate(command).IsValid.Should().BeFalse();

            command = BankData.BankCommandFaker.Generate();
            command.IfscCode = "";
            _validator.Validate(command).IsValid.Should().BeFalse();
        }

        [Fact]
        public void ShouldFailValidationIfIfscCodeExist()
        {
            _bankProvider.Setup(x => x.Bank(It.IsAny<string>())).ReturnsAsync(new DAO.Bank());
            var command = BankData.BankCommandFaker.Generate();
            _validator.Validate(command).IsValid.Should().BeFalse();
            _validator.Validate(command).Errors.First().ErrorMessage.Should().Be("Bank details already exists");
        }

        [Fact]
        public void ShouldPassValidationIfCommandIsValid()
        {
            var command = BankData.BankCommandFaker.Generate();
            _validator.Validate(command).IsValid.Should().BeTrue();
        }
    }
}