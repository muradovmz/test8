using System;
using {{cookiecutter.ProjectName}}.Framework.Api.Error;
using Swashbuckle.AspNetCore.Annotations;

namespace {{cookiecutter.ProjectName}}.Api.Attribute
{
    /// <summary>
    /// Abstraction over default swagger attrs and also set default error type as one place  
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiErrorResponseAttribute : SwaggerResponseAttribute
    {
        public ApiErrorResponseAttribute(int statusCode, string description = null)
            : base(statusCode, description, typeof(ErrorResponse))
        {
        }
    }
}