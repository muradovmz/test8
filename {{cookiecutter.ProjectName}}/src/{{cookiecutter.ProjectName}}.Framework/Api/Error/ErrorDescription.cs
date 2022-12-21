using System.Net;

namespace {{cookiecutter.ProjectName}}.Framework.Api.Error
{
    public class ErrorDescription
    {
        public HttpStatusCode Status { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}