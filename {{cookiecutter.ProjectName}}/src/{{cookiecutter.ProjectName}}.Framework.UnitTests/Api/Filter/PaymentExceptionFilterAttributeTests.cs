using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using {{cookiecutter.ProjectName}}.Framework.Api.Error;
using {{cookiecutter.ProjectName}}.Framework.Api.Filter;
using {{cookiecutter.ProjectName}}.Framework.Shared.Exception;
using Xunit;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace {{cookiecutter.ProjectName}}.Framework.UnitTests.Api.Filter
{
    public class {{cookiecutter.ProjectName}}ExceptionFilterAttributeTests
    {
        private readonly {{cookiecutter.ProjectName}}ExceptionFilterAttribute _filter;

        public {{cookiecutter.ProjectName}}ExceptionFilterAttributeTests()
        {
            var logger = new Mock<ILogger<{{cookiecutter.ProjectName}}ExceptionFilterAttribute>>().Object;
            _filter = new {{cookiecutter.ProjectName}}ExceptionFilterAttribute(logger);
        }
        
        [Theory]
        [MemberData(nameof(Exceptions))]
        public void GivenAnyHandledExceptionRaisedShouldSetErrorDetailsInContext(
            Exception ex, string expectedCode, HttpStatusCode expectedStatus)
        {
            var exceptionContext = CreateExceptionContext(ex);
            
            _filter.OnException(exceptionContext);

            var errorResponse = ErrorResponseReader(exceptionContext);
            errorResponse.Description.Should().BeEquivalentTo(ex.Message);
            errorResponse.Code.Should().BeEquivalentTo(expectedCode);
            errorResponse.Status.Should().Be(expectedStatus);
        }


        private static ExceptionContext CreateExceptionContext(Exception exception) =>
            new(
                    new ActionContext
                    {
                        HttpContext = new DefaultHttpContext(),
                        RouteData = new RouteData(),
                        ActionDescriptor = new ActionDescriptor()
                    }, new List<IFilterMetadata>())
                { Exception = exception };
        
        public static IEnumerable<object[]> Exceptions =>
            new List<object[]>
            {
                new object[]
                {
                    new NotFoundException("Not exist"),
                    nameof(HttpStatusCode.NotFound),
                    HttpStatusCode.NotFound
                },
                new object[]
                {
                    new {{cookiecutter.ProjectName}}.Framework.Shared.Exception.ArgumentException("Bad Request"),
                    nameof(HttpStatusCode.BadRequest),
                    HttpStatusCode.BadRequest
                },
                new object[]
                {
                    new Exception("Something went wrong"),
                    nameof(HttpStatusCode.InternalServerError),
                    HttpStatusCode.InternalServerError
                },
                new object[]
                {
                    new HttpException(HttpStatusCode.BadRequest, "some http error"),
                    nameof(HttpStatusCode.BadRequest),
                    HttpStatusCode.BadRequest
                }
            };


        private static ErrorDescription ErrorResponseReader(ExceptionContext exceptionContext) =>
            ((exceptionContext.Result as ObjectResult)?.Value as ErrorResponse)?.Error;
    }
}