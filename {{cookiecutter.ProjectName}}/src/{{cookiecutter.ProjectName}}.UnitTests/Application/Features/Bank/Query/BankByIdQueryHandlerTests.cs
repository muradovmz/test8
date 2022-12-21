using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Query;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;
using {{cookiecutter.ProjectName}}.Framework.Shared.Exception;
using {{cookiecutter.ProjectName}}.Persistence.Provider.Contract;
using {{cookiecutter.ProjectName}}.Tests.Common.TestData;
using Xunit;
namespace {{cookiecutter.ProjectName}}.UnitTests.Application.Features.Bank.Query;

public class BankByIdQueryHandlerTests
{
    private readonly Mock<IBankProvider> _provider;
    private readonly Mock<IMapper> _mapper;
    private readonly BankByIdQueryHandler _handler;
    
    public BankByIdQueryHandlerTests()
    {
        _provider = new Mock<IBankProvider>();
        _mapper = new Mock<IMapper>();
        _handler = new BankByIdQueryHandler(_provider.Object, _mapper.Object);
    }
    
    [Fact]
    public async Task ShouldReturnResponseIfBankExists()
    {
        var dao = BankData.BankDaoFaker.Generate();
        var response = BankData.BankResponseFaker.Generate();
        _provider.Setup(x => x.Bank(1)).ReturnsAsync(dao);
        _mapper.Setup(x => x.Map<BankResponse>(dao)).Returns<object>((_) => response);
         
        var result = await _handler.Handle(new BankByIdQuery() { BankId = 1}, default);

        result.Should().BeEquivalentTo(response);
    }
    
    [Fact]
    public async Task ShouldThrowExceptionIfBankNotExist()
    {
        _provider.Setup(x => x.Bank(1)).ReturnsAsync(({{cookiecutter.ProjectName}}.Persistence.DAO.Bank)default);

        await _handler.Invoking(x => x.Handle(new BankByIdQuery() { BankId = 1 }, default))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Bank with 1 not exist.");
    }
}