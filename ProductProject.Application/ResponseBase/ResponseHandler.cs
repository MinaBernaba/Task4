using System.Net;

namespace BooksManagementSystem.Application.ResponseBase
{
    public class ResponseHandler
    {
        public static Response<T> Success<T>(T entity, string? message = null, object? meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = message == null ? "Done successfully" : message,
                Meta = meta
            };
        }
        public static Response<T> Created<T>(object? meta = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created successfully",
                Meta = meta
            };
        }
        public static Response<T> Updated<T>(object? meta = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Updated successfully",
                Meta = meta
            };
        }
        public static Response<T> Deleted<T>(object? meta = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Deleted Successfully",
                Meta = meta
            };
        }
        public static Response<T> UnAuthorized<T>(string? message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message == null ? "Unauthorized" : message
            };
        }
        public static Response<T> BadRequest<T>(string? message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message == null ? "Bad Request" : message
            };
        }
        public static Response<T> UnprocessableEntity<T>(string? message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message == null ? "Unprocessable entity" : message
            };
        }
        public static Response<T> NotFound<T>(string error)
        {
            var response = new Response<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = "Failed to process request"
            };
            response.Errors.Add(error);
            return response;
        }
    }
}