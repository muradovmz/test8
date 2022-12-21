using System.Diagnostics.CodeAnalysis;

namespace {{cookiecutter.ProjectName}}.Framework.Shared.Exception
{
    [ExcludeFromCodeCoverage]
    public class BadRequestException : System.Exception
    {
        public BadRequestException(string message, System.Exception ex = null) : base(message, ex)
        {
        }
    }
}