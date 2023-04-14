using System.Net;
using System.Text.Json;

namespace Quiz_server.Helper
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var response = context.Response;

                response.ContentType = "application/json";

                switch (e)
                {
                    case AppException err:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case KeyNotFoundException err:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    //case InvalidOperationException err:
                    //    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    //    break;

                    //case DbException err:
                    //    response.StatusCode = (int)HttpStatusCode.; 
                    //    break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = e?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
