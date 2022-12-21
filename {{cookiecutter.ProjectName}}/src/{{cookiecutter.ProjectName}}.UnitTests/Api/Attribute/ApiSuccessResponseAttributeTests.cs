using FluentAssertions;
using {{cookiecutter.ProjectName}}.Api.Attribute;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;
using Xunit;

namespace {{cookiecutter.ProjectName}}.UnitTests.Api.Attribute
{
    public class ApiSuccessResponseAttributeTests
    {
        [Fact]
        public void ShouldSetStatusdDescriptionAndResponseType()
        {
            var attr = new ApiSuccessResponseAttribute(201, "something created", typeof(BankCreatedResponse));
            
            attr.StatusCode.Should().Be(201);
            attr.Description.Should().Be("something created");
            attr.Type.Name.Should().Be(nameof(BankCreatedResponse));
        }
    }
}