using BooksManagementSystem.Application.ResponseBase;
using Microsoft.EntityFrameworkCore;
using ProductProject.Application.Validation;
using System.Net;
using System.Text.Json;

namespace ProductProject.Api.MiddleWare
{
    public class ErrorHandlerMiddleware(RequestDelegate _next)
    {

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var contextResponse = context.Response;
                contextResponse.ContentType = "application/json";

                var response = new Response<string>() { Succeeded = false, Message = "Failed to process request." };

                switch (error)
                {
                    case CustomValidationException validationEx:
                        response.StatusCode = validationEx.StatusCode;
                        contextResponse.StatusCode = (int)validationEx.StatusCode;
                        response.Errors = validationEx.Errors;
                        break;

                    case KeyNotFoundException:
                        response.StatusCode = HttpStatusCode.NotFound;
                        contextResponse.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case DbUpdateException:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        contextResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case Exception e:
                        response.Message += e.InnerException?.Message ?? string.Empty;
                        response.StatusCode = HttpStatusCode.BadRequest;
                        contextResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        contextResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(response);
                await contextResponse.WriteAsync(result);
            }
        }
    }

}
