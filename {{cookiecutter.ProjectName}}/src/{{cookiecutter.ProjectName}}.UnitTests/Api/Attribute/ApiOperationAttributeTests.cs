using FluentAssertions;
using {{cookiecutter.ProjectName}}.Api.Attribute;
using Xunit;

namespace {{cookiecutter.ProjectName}}.UnitTests.Api.Attribute
{
    public class ApiOperationAttributeTests
    {
        [Fact]
        public void ShouldSetSummaryAndDescription()
        {
            var attr = new ApiOperationAttribute("summary", "desc");
            
            attr.Summary.Should().Be("summary");
            attr.Description.Should().Be("desc");
        }
    }
}