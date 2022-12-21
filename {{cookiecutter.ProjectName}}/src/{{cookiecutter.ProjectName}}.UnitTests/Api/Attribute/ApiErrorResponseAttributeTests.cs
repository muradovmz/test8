using FluentAssertions;
using {{cookiecutter.ProjectName}}.Api.Attribute;
using {{cookiecutter.ProjectName}}.Framework.Api.Error;
using Xunit;

namespace {{cookiecutter.ProjectName}}.UnitTests.Api.Attribute
{
    public class ApiErrorResponseAttributeTests
    {
        [Fact]
        public void ShouldSetStatusdDescriptionAndErrorResponseType()
        {
            var attr = new ApiErrorResponseAttribute(400, "something bad");
            
            attr.StatusCode.Should().Be(400);
            attr.Description.Should().Be("something bad");
            attr.Type.Name.Should().Be(nameof(ErrorResponse));
        }
    }
    
    
}