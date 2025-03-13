using System.Net;

namespace BooksManagementSystem.Application.ResponseBase
{
    public class Response<T>
    {
        #region Props
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public object? Meta { get; set; }
        #endregion

        #region ctors 
        public Response()
        {

        }
        public Response(T data, string message)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public Response(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }
        #endregion
    }
}
