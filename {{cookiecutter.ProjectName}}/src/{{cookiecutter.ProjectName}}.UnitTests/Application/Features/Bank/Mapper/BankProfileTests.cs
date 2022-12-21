using AutoMapper;
using FluentAssertions;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Mapper;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;
using Xunit;

namespace {{cookiecutter.ProjectName}}.UnitTests.Application.Features.Bank.Mapper
{
    public class BankProfileTests
    {
        private readonly IMapper _mapper;

        public BankProfileTests()
        {
            _mapper = InitializeAutoMapper();
        }

        [Fact]
        public void ShouldMapBankDaoToDto()
        {
            var dao = new {{cookiecutter.ProjectName}}.Persistence.DAO.Bank { Id = 1, Name = "icici", IfscCode = "icici123" };
            var response = _mapper.Map<BankResponse>(dao);
            response.Should().BeEquivalentTo(dao);
        }

        private static IMapper InitializeAutoMapper()
        {
            return new MapperConfiguration(mc => { mc.AddProfile(new BankMapper()); }).CreateMapper();
        }
    }
}