using System.Net;

namespace ProductProject.Application.Validation
{
    public class CustomValidationException(HttpStatusCode statusCode, List<string> errors) : Exception()
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
        public List<string> Errors { get; } = errors;
    }

}
