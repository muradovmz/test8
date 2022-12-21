using System;
using FluentAssertions;
using {{cookiecutter.ProjectName}}.Framework.Shared.Exception;
using {{cookiecutter.ProjectName}}.Framework.Shared.Util;
using Xunit;

namespace {{cookiecutter.ProjectName}}.Framework.UnitTests.Utils
{
    public class EnsureUtilityTests
    {
        [Fact]
        public void EnsureThat_WhenConditionIsFalse_ThrowsException()
        { 
            Action action = () => EnsureUtility.EnsureThat<BadRequestException>(false, "Bad Request");
            action.Should().Throw<BadRequestException>().WithMessage("Bad Request");
        }
    }  
}

