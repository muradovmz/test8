using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using {{cookiecutter.ProjectName}}.Framework.Api.Error;
using {{cookiecutter.ProjectName}}.Framework.Shared.Exception;
using {{cookiecutter.ProjectName}}.Framework.Shared.Extension;
using static System.String;
using ArgumentException = {{cookiecutter.ProjectName}}.Framework.Shared.Exception.ArgumentException;

namespace {{cookiecutter.ProjectName}}.Framework.Api.Filter
{
    /// <summary>
    /// Default exception filter for teh api. It will catch all exception, log it and return error response 
    /// </summary>
    public class {{cookiecutter.ProjectName}}ExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public {{cookiecutter.ProjectName}}ExceptionFilterAttribute(ILogger<{{cookiecutter.ProjectName}}ExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        protected virtual void HandleException(ExceptionContext exceptionContext)
        {
            var exception = exceptionContext.Exception;

            var (httpCode, errorCode, message) = exception switch
            {
                ArgumentException _ => (HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), Empty),
                BadRequestException _ => (HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), Empty),
                NotFoundException _ => (HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString(), Empty),
                ConflictException _ => (HttpStatusCode.Conflict, HttpStatusCode.Conflict.ToString(), Empty),
                ForbiddenException _ => (HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString(), Empty),
                UnAuthorisedException _ => (HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString(), Empty),
                HttpException ex => (ex.HttpStatusCode, ex.HttpStatusCode.ToString(), Empty),
                _ => (HttpStatusCode.InternalServerError, HttpStatusCode.InternalServerError.ToString(),
                    "Something went wrong")
            };

            WriteError(exceptionContext, CreateErrorResponse(httpCode, errorCode, message, exception));
        }
        
        private static ErrorResponse CreateErrorResponse(HttpStatusCode httpCode, string errorCode, string message,
            Exception exception)
        {
            var error = new ErrorResponse
            {
                Error = new ErrorDescription
                {
                    Status = httpCode,
                    Code = errorCode,
                    Description = string.IsNullOrWhiteSpace(message) ? exception.Message : message
                }
            };
            return error;
        }

        protected virtual void WriteError(ExceptionContext exceptionContext, ErrorResponse error)
        {
            exceptionContext.Result = new ObjectResult(error)
            {
                StatusCode = (int?)error.Error.Status
            };

            _logger.LogError(exceptionContext.Exception, $"Sending ErrorResponse: {error}");
        }
    }
}